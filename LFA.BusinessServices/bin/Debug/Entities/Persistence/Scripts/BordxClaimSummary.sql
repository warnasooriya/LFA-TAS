SELECT DISTINCT 
'' AS 'TransactionNumber',
CB.BordxYear AS 'ClaimBordxYear',
DATENAME(month, DATEADD(month, CB.[Bordxmonth]-1, CAST('2008-01-01' AS datetime))) AS 'ClaimBordxMonth',
CB.BordxNumber AS 'ClaimBordxReferneceNumber',
SUM(CL.TotalClaimAmount) AS 'ClaimAmount',
SUM(CL.[PaidAmount]) AS 'PaidAmount',
SUM(CL.TotalClaimAmount - CL.[PaidAmount]) AS 'Outstanding',
'' AS 'Remarks'
FROM ClaimBordx AS CB
LEFT OUTER JOIN [dbo].[Reinsurer] AS RI	ON RI.Id = CB.Reinsurer
LEFT OUTER JOIN [dbo].[ReinsurerContract] AS RIC ON RIC.[ReinsurerId] = RI.Id
LEFT OUTER JOIN [dbo].[Country] AS CNT ON CNT.Id = RIC.[CountryId]
LEFT OUTER JOIN [dbo].[Insurer] AS INSR	ON INSR.[Id] = CB.Insurer
LEFT OUTER JOIN [dbo].[ClaimBordxDetail] AS CBD	ON CBD.[ClaimBordxId] = CB.Id
LEFT OUTER JOIN [dbo].[Claim] AS CL	ON CL.Id = ClaimId
--WHERE CB.Id = '{bordexId}'
WHERE CB.BordxYear = '{bordexYear}'
--WHERE CB.BordxYear = '2019'
GROUP BY CB.BordxYear,
		CB.[Bordxmonth],
		CB.BordxNumber
		
