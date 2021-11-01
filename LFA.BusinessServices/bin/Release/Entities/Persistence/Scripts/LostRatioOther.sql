SELECT
CONVERT(datetime,'{EarnedDate}') AS 'EarnedDate',
CONVERT(datetime,'{ClaimDate}') AS 'ClaimDate',
ISNULL(V.MakeName,'not specific') AS MakeName,
ISNULL(V.ModelName,'not specific')AS ModelName,
ISNULL(V.Count,'not specific') as CylinderCount,
ISNULL(V.EngineCapacityNumber,0) as EngineCapacityNumber,
ISNULL(b.StartDate,0) As 'BordxStartDate' ,
ISNULL(b.EndDate,0) As 'BordxEndDate' ,
il.InsuaranceLimitationName AS 'EXTmonth',
wt.WarrantyTypeDescription AS 'CoverType',
ISNULL(V.Status,ISNULL(Y.Status,ISNULL(BW.Status,ISNULL(OT.Status,'-')))) AS 'WarrantyType',
COUNT(p.Id) AS 'PolicyCount',
SUM(p.GrossPremiumBeforeTax) AS 'GrossPremium',
--SUM(p.GrossPremiumBeforeTax*ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,
--case
--when '{EarnedDate}'='1/15/9999 12:00:00 AM'
--then GETDATE()
--else '{EarnedDate}'
--end
--)<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,
--case
--when '{EarnedDate}'='1/15/9999 12:00:00 AM'
--then GETDATE()
--else '{EarnedDate}'
--end ) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'ErnedGrossPremium',
SUM(p.GrossPremiumBeforeTax * dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}'))   AS 'ErnedGrossPremium',


SUM(p.NRP) AS 'NetPremium',
--SUM(p.NRP*ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,
--case
--when '{EarnedDate}'='1/15/9999 12:00:00 AM'
--then GETDATE()
--else '{EarnedDate}'
--end
--)<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,
--case
--when '{EarnedDate}'='1/15/9999 12:00:00 AM'
--then GETDATE()
--else '{EarnedDate}'
--end
--) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'ErnedNetPremium',
SUM(p.NRP * dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}'))  AS 'ErnedNetPremium',
-- AVG(ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,GETDATE())<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,GETDATE()) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'RiskCompleted',
AVG(dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}') ) AS 'RiskCompleted',
SUM(ISNULL(APClmN.ReinApprovedQty,0)) AS 'ReInPaidClaimsCount',
SUM(ISNULL(APClmN.ReinApprovedAmount,0)) AS 'ReInPaidClaimsValue',
SUM(ISNULL(APClm.ReinApprovedQty,0)) AS 'ReInPaidReservedClaimsCount',
SUM(ISNULL(APClm.ReinApprovedAmount,0)) AS 'ReInPaidReservedClaimsValue',
0 AS 'SAPaidClaimsCount',
0 AS 'SAPaidClaimsValue',
0 AS 'SAPaidReservedClaimsCount',
0 AS 'SAPaidReservedClaimsValue'
FROM [dbo].[Policy] p
INNER JOIN [dbo].[Contract] cn ON cn.Id=p.ContractId
INNER JOIN [dbo].[ContractExtensionPremium] ce ON ce.Id=p.ContractExtensionPremiumID
INNER JOIN [dbo].[WarrantyType] wt ON wt.Id=ce.WarrentyTypeId
INNER JOIN [dbo].[ReinsurerContract] rcn ON rcn.Id=cn.ReinsurerContractID
INNER JOIN [dbo].[Dealer] d ON d.Id=p.DealerId
INNER JOIN [dbo].[Insurer] i ON i.Id=cn.InsurerId
INNER JOIN [dbo].[Reinsurer] ri ON ri.Id=rcn.ReinsurerID
INNER JOIN [dbo].[Country] co ON co.Id=d.CountryId
INNER JOIN Bordx b on p.BordxId=b.Id
--INNER JOIN [dbo].[BordxDetails] bd ON bd.PolicyId=p.Id
--INNER JOIN [dbo].[Bordx] b ON b.Id =bd.BordxId
INNER JOIN [dbo].[ContractInsuaranceLimitation] cil ON cil.ContractId =cn.Id
INNER JOIN [dbo].[InsuaranceLimitation] il on cil.InsuaranceLimitationId=il.Id
LEFT OUTER JOIN (
SELECT cl.PolicyId ,Sum(case when cl.PaidAmount > 0 then  cl.PaidAmount else  cl.AuthorizedAmount  end) ReinApprovedAmount, Count(cl.Id) ReinApprovedQty
FROM [dbo].[Claim] cl
WHERE FORMAT(cl.EntryDate,'yyyy-MM-dd') <= case
when '{ClaimDate}' = '1/15/9999 12:00:00 AM'
then GETDATE()
else '{ClaimDate}'
end
AND (cl.IsApproved =1 OR cl.IsBatching=1)
GROUP BY cl.PolicyId
)APClm ON APClm.policyId=p.Id
LEFT OUTER JOIN (
SELECT cl.PolicyId ,Sum(cl.PaidAmount) ReinApprovedAmount, Count(cl.Id) ReinApprovedQty
FROM [dbo].[Claim] cl
WHERE FORMAT(cl.EntryDate,'yyyy-MM-dd') <= case
when '{ClaimDate}'<'1/15/9999 12:00:00 AM'
then GETDATE()
else '{ClaimDate}'
end
AND cl.IsApproved =1 AND cl.IsBatching=1  AND
format(cl.ClaimDate,'yyyy-MM-dd') <= case
	when '{ClaimDate}'='1/15/9999 12:00:00 AM'
	then FORMAT(ClaimDate,'yyyy-MM-dd')
	else '{ClaimDate}'
	end
GROUP BY cl.PolicyId
)APClmN ON APClmN.policyId=p.Id
JOIN (
  SELECT vp.policyid,its.Status ,mk.MakeName,md.ModelName,co.Count,ec.EngineCapacityNumber
  FROM vehiclepolicy vp
  INNER JOIN vehicledetails vd ON vd.id = vp.vehicleid
  INNER JOIN  itemstatus its ON its.ID= vd.itemstatusid
  INNER JOIN  Make mk on mk.Id=vd.MakeId
  INNER JOIN  Model md on md.Id=vd.ModelId
  INNER JOIN  CylinderCount co on co.Id=vd.CylinderCountId
  INNER JOIN  EngineCapacity ec on ec.Id=vd.EngineCapacityId
  where vd.MakeId=case
	when '{MakeId}'='00000000-0000-0000-0000-000000000000'
	then vd.MakeId
	else '{MakeId}'
	end
	and vd.ModelId=case
	when '{ModelId}'='00000000-0000-0000-0000-000000000000'
	then ModelId
	else '{ModelId}'
	end
	and vd.cylinderCountId=case
	when '{cylinderCountId}'='00000000-0000-0000-0000-000000000000'
	then cylinderCountId
	else '{cylinderCountId}'
	end
	and vd.EngineCapacityId=case
	when '{EngineCapacityId}'='00000000-0000-0000-0000-000000000000'
	then EngineCapacityId
	else '{EngineCapacityId}'
	end
) V ON V.PolicyID=p.ID
LEFT OUTER JOIN (
  SELECT yp.policyid,its.Status
  FROM yellowgoodpolicy yp
     INNER JOIN yellowgooddetails yd ON yd.id = yp. YellowGoodId
  INNER JOIN  itemstatus its ON its.ID= yd.itemstatusid
) Y ON Y.PolicyID=p.ID
LEFT OUTER JOIN (
  SELECT bwp.policyid,its.Status
  FROM bandwpolicy  bwp
     INNER JOIN brownandwhitedetails bwd ON bwd.id = bwp.bandwid
  INNER JOIN  itemstatus its ON its.ID= bwd.itemstatusid
) BW ON BW.PolicyID=p.ID

LEFT OUTER JOIN (
  SELECT oip.policyid,its.Status
     FROM otheritempolicy oip
     INNER JOIN otheritemdetails oid ON oid.id = oip.otheritemid
  INNER JOIN  itemstatus its ON its.ID= oid.itemstatusid
) OT ON OT.PolicyID=p.ID

where b.StartDate=case
	when '{BordxStartDate}'='1/15/9999 12:00:00 AM'
	then b.StartDate
	else '{BordxStartDate}'
	end
	and b.EndDate=case
	when '{BordxEndDate}'='1/15/9999 12:00:00 AM'
	then b.EndDate
	else '{BordxEndDate}'
	end
	and cil.InsuaranceLimitationId =case
	when '{InsuaranceLimitationId}'='00000000-0000-0000-0000-000000000000'
	then cil.InsuaranceLimitationId
	else '{InsuaranceLimitationId}'
	end
and p.DealerId='{DealerId}' AND  co.Id='{CountryId}'   AND rcn.UWYear ='{UWYear}'
Group by co.CountryName,d.DealerName,rcn.UWYear,b.StartDate  , b.EndDate,il.InsuaranceLimitationName,V.MakeName,V.ModelName,V.Count,V.EngineCapacityNumber,wt.WarrantyTypeDescription,
ISNULL(V.Status,ISNULL(Y.Status,ISNULL(BW.Status,ISNULL(OT.Status,'-'))))