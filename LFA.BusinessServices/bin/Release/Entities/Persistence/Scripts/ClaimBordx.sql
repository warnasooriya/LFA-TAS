--DECLARE @Id UNIQUEIDENTIFIER = '6ddfece9-eaa0-49fd-8893-f4a96192e4d5'

SELECT RI.[ReinsurerName]  AS 'Reinsurer'
	, CNT.[CountryName] AS 'Country'
	, INSR.[InsurerFullName]  AS 'CedentName'
	, CB.Fromdate as 'FromDate'
	, CB.Todate as 'ToDate'
	, RI.[ReinsurerName] + '-' + INSR.[InsurerFullName] + '-' +DATENAME(month, DATEADD(month, CB.[Bordxmonth]-1, CAST('2008-01-01' AS datetime))) +'-'+ (CAST(CB.[BordxYear] AS VARCHAR(11))) +'-'+  CB.[BordxNumber] as 'FileName'
FROM [dbo].[ClaimBordx] CB
INNER JOIN [dbo].[Reinsurer] RI
	ON RI.Id = CB.Reinsurer
	 AND CB.Id = '{bordexId}'--@Id
INNER JOIN [dbo].[ReinsurerContract] RIC
	ON RIC.[ReinsurerId] = RI.Id
INNER JOIN [dbo].[Country] CNT
	ON CNT.Id = RIC.[CountryId]
INNER JOIN [dbo].[Insurer] INSR
	ON INSR.[Id] = CB.Insurer
