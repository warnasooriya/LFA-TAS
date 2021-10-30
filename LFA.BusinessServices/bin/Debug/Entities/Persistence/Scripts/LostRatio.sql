SELECT
co.CountryName AS 'Country',
d.DealerName AS 'DealerContract',
rcn.UWYear AS 'ContractYear',
wt.WarrantyTypeDescription AS 'CoverType',
ISNULL(V.Status,ISNULL(Y.Status,ISNULL(BW.Status,ISNULL(OT.Status,'-')))) AS 'WarrantyType',
COUNT(p.Id) AS 'PolicyCount',
SUM(p.GrossPremiumBeforeTax) AS 'GrossPremium',
SUM(p.GrossPremiumBeforeTax * dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}'))   AS 'ErnedGrossPremium',
-- SUM(p.GrossPremiumBeforeTax*ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,GETDATE())<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,GETDATE()) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'ErnedGrossPremium',
SUM(p.NRP) AS 'NetPremium',
-- SUM(p.NRP*ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,GETDATE())<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,GETDATE()) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'ErnedNetPremium',
SUM(p.NRP * dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}'))  AS 'ErnedNetPremium',
-- AVG(ROUND((CONVERT(decimal(10,2),CASE WHEN DATEDIFF (day,p.PolicyStartDate,GETDATE())<0 THEN 0 ELSE DATEDIFF (day,p.PolicyStartDate,GETDATE()) END)/CONVERT(decimal(10,2),DATEDIFF(day,p.PolicyStartDate,p.PolicyEndDate))),0,0)) AS 'RiskCompleted',
AVG(dbo.GetRiskCompletedByPolicyId(p.PolicyStartDate,p.PolicyEndDate,'{EarnedDate}')) AS 'RiskCompleted',
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
join Bordx b on p.BordxId=b.Id
LEFT OUTER JOIN (
SELECT cl.PolicyId ,Sum(case when cl.PaidAmount > 0 then  cl.PaidAmount else  cl.AuthorizedAmount  end) ReinApprovedAmount, Count(cl.Id) ReinApprovedQty
FROM [dbo].[Claim] cl
WHERE FORMAT(cl.EntryDate,'yyyy-MM-dd') <= '{ClaimDate}' AND (cl.IsBatching=1 OR cl.IsApproved =1)
GROUP BY cl.PolicyId
)APClm ON APClm.policyId=p.Id
LEFT OUTER JOIN (
SELECT cl.PolicyId ,Sum(cl.PaidAmount) ReinApprovedAmount, Count(cl.Id) ReinApprovedQty
FROM [dbo].[Claim] cl
WHERE FORMAT(cl.EntryDate,'yyyy-MM-dd') <= '{ClaimDate}' AND cl.IsBatching=1 AND cl.IsApproved = 1
GROUP BY cl.PolicyId
)APClmN ON APClmN.policyId=p.Id
LEFT OUTER JOIN (
  SELECT vp.policyid,its.Status
  FROM vehiclepolicy vp
     INNER JOIN vehicledetails vd ON vd.id = vp.vehicleid
  INNER JOIN  itemstatus its ON its.ID= vd.itemstatusid
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
where p.DealerId='{DealerId}' AND co.Id='{CountryId}'  AND rcn.UWYear ='{UWYear}'
	{BordxFilter}
Group by co.CountryName,d.DealerName,rcn.UWYear,
wt.WarrantyTypeDescription,
ISNULL(V.Status,ISNULL(Y.Status,ISNULL(BW.Status,ISNULL(OT.Status,'-'))))
