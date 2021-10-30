SELECT DISTINCT P.Id,
P.PolicyNo,
P.LocalCurrencyConversionRate,
P.PolicyStartDate,
P.PolicyEndDate,
P.PolicySoldDate,
P.HrsUsedAtPolicySale,
P.RefNo,
P.Comment,
P.Discount,
P.CustomerPayment,
P.DealerPayment,
c.FirstName + ' ' + C.LastName AS CUSTOMERNAME,
C.Email,
C.Address1,
C.Address2,
C.Address3,
C.Address4,
C.BusinessAddress1,
C.BusinessAddress2,
C.BusinessAddress3,
C.BusinessAddress4,
C.BusinessName,
C.DateOfBirth,
CT.CommodityTypeDescription,
D.DealerName,
PR.ProductName,
CON.DealName,
CON.ClaimLimitation,
CON.LiabilityLimitation,
UT.UsageTypeName,
RC.ContractNo,
CURR.Code,
OID.ItemPurchasedDate,
OID.DealerPrice,
OID.ItemPrice,
M.MakeName,
MO.ModelName,
CC.CommodityCategoryDescription,
IC.Code AS INVOICECODE,
IC.PlateNumber,
CEID.AdditionalDetailsModelYear,
CEID.AdditionalDetailsMileage,
C.MobileNo,
N.NationalityName,
CTY.CustomerTypeDescription,
CUN.CountryName,
ICTD.ArticleNumber + ' ' + ATSP.Pattern AS TyerSize,
IC.TireQuantity,
CEID.UsageTypeCode,
DATEADD(month, 12, OID.ItemPurchasedDate) AS ExpireDates         


FROM Policy AS P  
INNER JOIN Customer AS C ON P.CustomerId = C.Id  
INNER JOIN Nationality AS N ON N.Id = C.NationalityId  
INNER JOIN CustomerType AS CTY ON C.CustomerTypeId = CTY.Id   
INNER JOIN Country AS CUN ON C.CountryId = CUN.Id  
INNER JOIN CommodityType AS CT ON P.CommodityTypeId = CT.CommodityTypeId  
INNER JOIN Dealer AS D ON D.Id = P.DealerId  
INNER JOIN Product AS PR ON P.ProductId = PR.Id  
INNER JOIN  Contract AS CON ON P.ContractId = CON.Id  
INNER JOIN UsageType AS UT ON C.UsageTypeId = UT.Id  
INNER JOIN ReinsurerContract AS RC ON CON.ReinsurerContractId = RC.Id  
INNER JOIN Currency AS CURR ON P.PremiumCurrencyTypeId = CURR.Id  

INNER JOIN OtherItemPolicy AS OIP ON P.Id = OIP.PolicyId  
INNER JOIN otherItemDetails AS OID ON OIP.OtherItemId = OID.Id  
INNER JOIN Make AS M ON OID.MakeId = M.Id  
INNER JOIN Model AS MO ON OID.ModelId = MO.Id  
INNER JOIN CommodityCategory AS CC ON OID.CategoryId = CC.CommodityCategoryId  
INNER JOIN InvoiceCodeDetails AS ICD ON ICD.PolicyId = P.Id  
INNER JOIN InvoiceCode AS IC ON IC.Id = ICD.InvoiceCodeId  
INNER JOIN CustomerEnterdInvoiceDetails AS CEID ON IC.Id = CEID.InvoiceCodeId  
INNER JOIN InvoiceCodeTireDetails AS ICTD ON ICD.Id = ICTD.InvoiceCodeDetailId  
INNER JOIN AvailableTireSizesPattern AS ATSP ON ICTD.AvailableTireSizesPatternId = ATSP.Id  
WHERE P.PolicyBundleId = '{policyId}'  