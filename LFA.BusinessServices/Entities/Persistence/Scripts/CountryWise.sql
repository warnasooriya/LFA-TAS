SELECT DISTINCT	 CB.[Bordxmonth]
		,CB.[BordxYear]
		,CB.IsPaid
		,R.ReinsurerName
		,RC.UWYear
		,C.[PolicyId]
		,C.[PolicyCountryId]
		,C.[ClaimSubmittedDealerId]
		,C.[StatusId]
		,C.[ClaimNumber]
		,C.[ClaimMileageKm]
		,C.[PaidAmount]
		,C.PaidAmount * C.ConversionRate AS 'LocalPaidAmount'
		,C.IsGoodwill
		,C.IsBatching
		,C.ClaimDate
		,C.ApprovedDate
		,C.ConversionRate
		,C.[FailureDate]
		,CI.[ClaimId]
		,CI.[ItemName]
		,CI.[TotalPrice] AS 'PartPrice'
		,CI.[TotalPrice] * CI.ConversionRate AS 'LocalPartPrice'
		,ISNULL((SELECT  SUM( CLITB.[TotalPrice]) AS 'Labour'
				FROM  [dbo].[ClaimItem] CLITB
				INNER JOIN [dbo].[ClaimItemType] CLITTPB
					ON CLITTPB.Id = CLITB.[ClaimItemTypeId]
					AND CLITTPB.[ItemCode] = 'L'
					AND CLITB.[ClaimId] =  CI.[ClaimId]),0) AS 'Labour'
		,ISNULL((SELECT  SUM( CLITB.[TotalPrice] * CLITB.ConversionRate) AS 'Labour'
				FROM  [dbo].[ClaimItem] CLITB
				INNER JOIN [dbo].[ClaimItemType] CLITTPB
					ON CLITTPB.Id = CLITB.[ClaimItemTypeId]
					AND CLITTPB.[ItemCode] = 'L'
					AND CLITB.[ClaimId] =  CI.[ClaimId]),0) AS 'LocalLabour'
		,CI.[FaultId]
		,CI.[CauseOfFailureId]
		,CI.ConversionRate AS 'ItemConversionRate',
		0 as endosed
INTO #TempData
FROM ClaimBordx AS CB
LEFT JOIN ClaimBordxDetail AS CBD ON CBD.ClaimBordxId = CB.Id
LEFT JOIN Claim AS C ON C.Id = CBD.ClaimId
LEFT JOIN ClaimItem AS CI ON CI.ClaimId = C.Id
LEFT JOIN Policy AS P ON P.Id = C.PolicyId
LEFT JOIN Contract AS CON ON CON.Id = P.ContractId
LEFT JOIN ReinsurerContract AS RC ON RC.Id = CON.ReinsurerContractId
LEFT JOIN Reinsurer AS R ON R.Id = RC.ReinsurerId
LEFT JOIN ClaimItemType AS CTY  ON CTY.Id = CI.ClaimItemTypeId 
WHERE CB.Id = '{bordexId}'
AND CTY.[ItemCode] = 'P'

INSERT INTO #TempData
SELECT DISTINCT	 CB.[Bordxmonth]
		,CB.[BordxYear]
		,CB.IsPaid
		,R.ReinsurerName
		,RC.UWYear
		,C.[PolicyId]
		,C.[PolicyCountryId]
		,C.[ClaimSubmittedDealerId]
		,C.[StatusId]
		,C.[ClaimNumber]
		,C.[ClaimMileageKm]
		,C.[PaidAmount]
		,C.PaidAmount * C.ConversionRate AS 'LocalPaidAmount'
		,C.IsGoodwill
		,C.IsBatching
		,C.ClaimDate
		,C.ApprovedDate
		,C.ConversionRate
		,C.[FailureDate]
		,CI.[ClaimId]
		,CI.[ItemName]
		,CI.[TotalPrice] AS 'PartPrice'
		,CI.[TotalPrice] * CI.ConversionRate AS 'LocalPartPrice'
		,ISNULL((SELECT  SUM( CLITB.[TotalPrice]) AS 'Labour'
				FROM  [dbo].[ClaimItem] CLITB
				INNER JOIN [dbo].[ClaimItemType] CLITTPB
					ON CLITTPB.Id = CLITB.[ClaimItemTypeId]
					AND CLITTPB.[ItemCode] = 'L'
					AND CLITB.[ClaimId] =  CI.[ClaimId]),0) AS 'Labour'
		,ISNULL((SELECT  SUM( CLITB.[TotalPrice] * CLITB.ConversionRate) AS 'Labour'
				FROM  [dbo].[ClaimItem] CLITB
				INNER JOIN [dbo].[ClaimItemType] CLITTPB
					ON CLITTPB.Id = CLITB.[ClaimItemTypeId]
					AND CLITTPB.[ItemCode] = 'L'
					AND CLITB.[ClaimId] =  CI.[ClaimId]),0) AS 'LocalLabour'
		,CI.[FaultId]
		,CI.[CauseOfFailureId]
		,CI.ConversionRate AS 'ItemConversionRate',
		1 as endosed

FROM ClaimBordx AS CB
LEFT JOIN ClaimBordxDetail AS CBD ON CBD.ClaimBordxId = CB.Id
LEFT JOIN Claim AS C ON C.Id = CBD.ClaimId
LEFT JOIN ClaimItem AS CI ON CI.ClaimId = C.Id
LEFT JOIN Policy AS P ON P.Id = C.PolicyId
LEFT JOIN Contract AS CON ON CON.Id = P.ContractId
LEFT JOIN ReinsurerContract AS RC ON RC.Id = CON.ReinsurerContractId
LEFT JOIN Reinsurer AS R ON R.Id = RC.ReinsurerId
LEFT JOIN ClaimItemType AS CTY  ON CTY.Id = CI.ClaimItemTypeId 
WHERE CB.Id = '{bordexId}' AND C.IsEndorsed=1
AND CTY.[ItemCode] = 'P'

SELECT  DISTINCT '' as 'SerialNo'
		, DL.[DealerName] as 'Dealer'
		, LEFT(DATENAME(month, DATEADD(month, A.[Bordxmonth] - 1, CAST('2008-01-01' AS datetime))) , 3 ) + '-'+ (CAST(A.[BordxYear] AS VARCHAR(11))) as 'BordxMonthYear'
		, A.UWYear as  'UWYear'
		, WT.WarrantyTypeDescription as 'TypePolicy'
		, Pl.PolicyNo as 'PolicyNo'
		, MKE.[MakeName] as 'Make'
		, MDL.[ModelName] as 'Model'
		, ECP.[EngineCapacityNumber] as 'EngineSize'
		, VH.VINNo as 'ChassisNo'
		, A.[ClaimNumber] as 'ClaimNo'
		, SERV.[DealerName] as 'ServiceCenter'
		, SERV.[Location] as 'Location'
		, A.ClaimMileageKm as 'Mileage'
		, FLTCT.FaultCategoryCode as 'Letter'
		, FLTAR.[FaultAreaCode] as 'Number'
		, FLT.[FaultCode] as 'FaultCode'
		, A.[ItemName] as 'FailedComponent'
		, FCFL.[CauseOfFailure] as 'CauseOfFailure'
		, CLSCD.[Description] as 'ClaimStatus'
		, 'Part' as 'Sorting'
		, A.Labour as 'Labour'
		, A.LocalLabour as 'LocalLabour'
		, A.PartPrice as 'Part'
		, A.LocalPartPrice as 'LocalPart'
		, A.Labour + A.PartPrice as 'AuthAmt'
		, A.LocalLabour + A.LocalPartPrice as 'AuthAmt'
		, ABS(A.[PaidAmount] - (A.Labour + A.PartPrice)) as 'Variance'
		, ABS(A.LocalPaidAmount - (A.LocalLabour + A.LocalPartPrice)) as 'LocalVariance'
		, CAST(0 AS  DECIMAL(18,2)) as 'GapGoodWill'
		, CAST(0 AS  DECIMAL(18,2)) as 'LocalGapGoodWill'
		, CASE A.IsPaid WHEN 1 THEN (A.Labour + A.PartPrice) ELSE 0 END as 'Authorized'
		, CASE A.IsPaid WHEN 1 THEN (A.LocalLabour + A.LocalPartPrice) ELSE 0 END as 'LocalAuthorized'
		, CAST(0 AS  DECIMAL(18,2)) as 'Inprogress'
		, CAST(0 AS  DECIMAL(18,2)) as 'LocalInprogress'
		, CASE A.IsBatching WHEN 1 THEN (A.Labour + A.PartPrice) ELSE 0 END as 'Paid'
		, CASE A.IsBatching WHEN 1 THEN (A.LocalLabour + A.LocalPartPrice) ELSE 0 END as 'LocalPaid'
		, CAST(0 AS  DECIMAL(18,2)) as 'Over180'
		, CAST(0 AS  DECIMAL(18,2)) as 'LocalOver180'
		, CLINV.[InvoiceDate] as 'InvoiceDate'
		, '' as 'DateClaimPaid'
		, (A.Labour + A.PartPrice) as 'TotalClaim'
		, (A.LocalLabour + A.LocalPartPrice) as 'LocalTotalClaim'
		, (A.Labour + A.PartPrice) as 'TotalClaimToReinsurance'
		, (A.LocalLabour + A.LocalPartPrice) as 'LocalTotalClaimToReinsurance'
		, A.ReinsurerName as 'UnderWriter'
		, A.[FailureDate] as 'FailureDate'
		, A.ClaimDate as 'ClaimDate'
		, 0 as 'ManufacturerWarrantyCutoffKms'
		, 0 as 'ExtWarrantyExpiryKm'
		, PL.PolicyStartDate as 'PolicyStartDate'
		, PL.PolicyEndDate as 'PolicyExpiryDate'

		, CNT.CountryName as 'CountryName'
		, CNT.Id as 'CountrId'
		, A.ConversionRate
		, A.ItemConversionRate,
		A.endosed
FROM #TempData A
INNER JOIN [dbo].[Country] CNT
	ON CNT.Id = A.[PolicyCountryId]
INNER JOIN [dbo].[Dealer] SERV
	ON SERV.Id = A.[ClaimSubmittedDealerId]
INNER JOIN [dbo].[ClaimStatusCode] CLSCD
	ON CLSCD.Id = A.[StatusId]
INNER JOIN [dbo].[Policy] PL
	ON PL.Id = A.[PolicyId]
INNER JOIN [dbo].[Dealer] DL
	ON DL.Id = PL.[DealerId]
INNER JOIN [dbo].[VehiclePolicy] VPL
	ON VPL.[PolicyId] = PL.Id
INNER JOIN [dbo].[VehicleDetails] VH
	ON VH.Id = VPL.[VehicleId]
INNER JOIN [dbo].[EngineCapacity] ECP
	ON ECP.Id = VH.[EngineCapacityId]
INNER JOIN [dbo].[Make] MKE
	ON MKE.Id = VH.[MakeId]
INNER JOIN [dbo].[Model] MDL
	ON MDL.Id = VH.[ModelId]
LEFT JOIN [dbo].[ManufacturerWarrantyDetails] MWD
	ON  MWD.[ModelId] = VH.[ModelId]
		AND MWD.[CountryId]= A.[PolicyCountryId]
LEFT JOIN [dbo].[ManufacturerWarranty] MW
	ON MW.Id = MWD.[ManufacturerWarrantyId]
	AND MW.MakeId = VH.MakeId
LEFT JOIN [dbo].[ContractExtensions] CE
	ON CE.id = PL.ContractInsuaranceLimitationId
LEFT JOIN [dbo].[ContractExtensionPremium] CEP
	ON CEP.Id = PL.ContractExtensionPremiumId
LEFT JOIN [dbo].[ContractInsuaranceLimitation] CIL
	ON CIL.id = PL.ContractExtensionsId
LEFT JOIN [dbo].[InsuaranceLimitation] il
	ON il.id = cil.InsuaranceLimitationId
LEFT JOIN [dbo].[Warrantytype] WT
	ON WT.id = CEP.WarrentyTypeId
LEFT JOIN [dbo].[Fault] FLT -- INNER
	ON FLT.Id = A.[FaultId]
LEFT JOIN [dbo].[FaultCategory] FLTCT
	ON FLTCT.Id = FLT.[FaultCategoryId]
LEFT JOIN [dbo].[FaultArea] FLTAR
	ON FLTAR.Id = FLT.[FaultAreaId]
LEFT JOIN [dbo].[FaultCauseOfFailure] FCFL
	ON FCFL.Id = A.[CauseOfFailureId]
LEFT JOIN [dbo].[ClaimInvoiceEntryClaim] CLIC
	ON CLIC.[ClaimId] = A.ClaimId
LEFT JOIN [dbo].[ClaimInvoiceEntry] CLINV
	ON CLINV.Id = [ClaimInvoiceEntryId]


IF object_id('tempdb..#TempData') is not null
BEGIN
    DROP TABLE #TempData
END