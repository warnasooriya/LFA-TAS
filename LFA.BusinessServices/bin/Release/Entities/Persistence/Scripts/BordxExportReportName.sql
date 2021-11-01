declare @BordxReportTemplateName varchar(100)

select Distinct @BordxReportTemplateName =  brt.[Name]
  FROM BordxReportColumns brc
  INNER JOIN [BordxReportTemplateDetails] brtd ON brc.Id = brtd.BordxReportColumnsId
  INNER JOIN [BordxReportTemplate] brt ON brtd.BordxReportTemplateId = brt.Id
	WHERE brtd.BordxReportTemplateId = '{BordxReportTemplateId}'

SELECT DISTINCT TOP 1  Upper(re.ReinsurerName) +'_'+ CONVERT(VARCHAR,b.year) +'_'+ CONVERT(VARCHAR,b.year) +
REPLICATE('0',2-LEN(b.month)) + CONVERT(VARCHAR,b.month) +'_'+  '{ProductName}' +'_'+ CONVERT(varchar, b.EntryDateTime, 112)
+'_Policy' + @BordxReportTemplateName + ' '   AS 'ReportName'
FROM   policy p
       INNER JOIN bordxdetails bd
              ON bd.policyid = p.id
			  AND bd.bordxid = '{bordexId}'
       INNER JOIN bordx b
              ON b.id = bd.bordxid
       INNER JOIN contract c
              ON c.id = p.contractid
       INNER JOIN reinsurercontract rec
              ON rec.id = c.ReinsurerContractId
       INNER JOIN reinsurer re
              ON re.id = rec.reinsurerid
