declare @count int             

select @count = Count(*)
  FROM [CycleandCarriage].[dbo].[Bordx]

SELECT DISTINCT  

RIGHT('00000' + CONVERT(VARCHAR(11),B.SequenceNo),6) as SequenceNo, 
'' as PolicyReference,
D.DealerName as DealerName,
'' as DealerAddress1,
'' as DealerAddress2,
'' as DealerAddress3,
'' as InvoiceNumber,
I.InsurerFullName as Insured,
Dateadd(dd,0 , DATEDIFF(dd, 0,B.EntryDateTime)) AS BDXExtractDate,
DATENAME(month, DATEADD(month, B.[Month]-1, CAST('2008-01-01' AS datetime))) +' ' +CONVERT(VARCHAR(11),B.[Year]) AS ProductionMonth,
CONVERT(VARCHAR(11), B.StartDate, 106) +' to '+ CONVERT(VARCHAR(11), B.[EndDate], 106) as Period,
DATENAME(month, DATEADD(month, B.[Month]-1, CAST('2008-01-01' AS datetime))) +' ' +CONVERT(VARCHAR(11),B.[Year]) +' (Bordereau Number '+ 
RIGHT('00000' + CONVERT(VARCHAR(11),B.SequenceNo),6) + ')' as BordxName,
C.Code as CurrencyCode,
SUM(P.GrossPremiumBeforeTax) * P.LocalCurrencyConversionRate  as NetAmount       

FROM Bordx B    
INNER JOIN [dbo].[Policy] P ON P.BordxId = B.Id  
INNER JOIN [dbo].[Dealer] D ON D.Id = P.DealerId  
INNER JOIN [dbo].[Insurer] I ON I.Id = D.InsurerId   
INNER JOIN [dbo].[Currency] C ON C.Id = D.CurrencyId    
WHERE B.Id='{BordxId}' AND D.id='{DealerId}'   
GROUP BY D.DealerName,I.InsurerFullName,B.StartDate,B.[EndDate],B.[Month],B.[Year],B.[Number],C.Code,P.LocalCurrencyConversionRate,B.SequenceNo,B.EntryDateTime