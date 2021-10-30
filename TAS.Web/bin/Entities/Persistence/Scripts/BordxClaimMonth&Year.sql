--select CBD.Id, C.ClaimNumber,C.PolicyNumber,RC.[UWYear]
SELECT DISTINCT
	 (CAST(RC.[UWYear] AS VARCHAR(11)) + '-' +  CNT.[CountryName]) AS 'UWYear'	
	, COUNT(RC.[UWYear])AS 'PaidClaimCount'
	, SUM(C.[PaidAmount]) AS 'PaiClaimAmount'
	, (SUM(C.[PaidAmount]) * CBVD.[Rate]) AS 'PaiClaimAmountInLocal'
from ClaimBordx  CB
LEFT JOIN [ClaimBordxDetail] AS CBD ON CB.Id = CBD.ClaimBordxId
LEFT JOIN Claim AS C ON C.Id = CBD.ClaimId
LEFT JOIN [dbo].[ClaimBordxValueDetail] AS CBVD ON CBVD.[ClaimBordxId] = CB.Id
LEFT JOIN Policy AS P ON P.Id = C.PolicyId
LEFT JOIN Contract AS CON ON CON.Id = P.ContractId
LEFT JOIN ReinsurerContract AS RC ON CON.ReinsurerContractId = RC.Id
LEFT JOIN [dbo].[Country] CNT ON CNT.Id = RC.[CountryId]
Where CB.Id  = '{bordexId}'  AND C.IsInvoiceSubmit = 1
GROUP BY RC.[UWYear],
			CNT.[CountryName],
			CBVD.[Rate]