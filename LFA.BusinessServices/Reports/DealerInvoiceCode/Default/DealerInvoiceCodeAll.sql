DECLARE @var1 datetime,@var2 datetime;
set @var1='{StartDate}';
set @var2='{EndDate}';


select DISTINCT [Code]
      ,@var1 as startDate
	  ,@var2 as endDate
      ,[CodeInt]
      ,[PurcheasedDate]
      ,[PlateNumber]
      ,IC.[TireQuantity]
      ,[GeneratedDate]
	  ,DE.DealerName
	  ,ICTD.ArticleNumber
	  ,ATSP.Pattern
  FROM [dbo].[InvoiceCode] AS IC
  LEFT OUTER JOIN [dbo].[Dealer] AS DE ON DE.Id = IC.DealerId
  LEFT OUTER JOIN [dbo].[InvoiceCodeDetails] AS ICD ON ICD.InvoiceCodeId=IC.Id
  LEFT OUTER JOIN [dbo].[InvoiceCodeTireDetails] AS ICTD ON ICTD.InvoiceCodeDetailId=ICD.Id
  LEFT OUTER JOIN [dbo].[AvailableTireSizesPattern] AS ATSP ON ICTD.[AvailableTireSizesPatternId]=ATSP.[Id]
  where [GeneratedDate] >= '{StartDate}' AND [GeneratedDate] <= '{EndDate}'
  Order By  CodeInt DESC