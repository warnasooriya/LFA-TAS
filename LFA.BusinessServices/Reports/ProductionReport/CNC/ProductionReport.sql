DECLARE @var1 VARCHAR(256) ,@var2 VARCHAR(256),@var3 VARCHAR(256),@var4 VARCHAR(256),@var5 VARCHAR(256);
set @var1='{DealerId}';
set @var2='{InsurerId}';
set @var3='{ReinsurerId}';
set @var4='{FromMonthId}';
set @var5='{ToMonthId}';

SELECT a.PolicyNo, b.CedentName as cedent, b.DealerName as dealer, a.status , b.CoverType as new , b.BaseCountry as country, LEFT(a.month, 3) as month,  CONVERT(varchar, isnull(b.PolicySoldDate,GETDATE()),101) as invoiceDate , RIGHT('00000' + CONVERT(VARCHAR(11),isnull(b.SNo,0)),6) as invoiceNo,isnull(b.SumInsured, 0) * mulpleVal as sumInsured, 1 as noOfUnits, a.grossPremium * mulpleVal  as grossPremium , b.GrossPremiumBeforeTax * mulpleVal as GrossPremiumExTax 
		, b.MarketingFee * a.mulpleVal as MarketingFee , b.SalesCommission * a.mulpleVal AS salesComm , b.ManufactureCommission * mulpleVal  AS manfComm, b.InsurerFee * mulpleVal  AS cedentFee, b.DocumentFee * mulpleVal  AS docFee, b.ClientBrokerage * mulpleVal  AS ClientBrokerage, b.InsurerNRPRetention * mulpleVal  AS cedentNrp, b.InternalGoodWill * mulpleVal  AS InternalGoodWill, b.DealerCommission * mulpleVal  AS DealerCommission
		, b.GrossPremiumLessCommission * a.mulpleVal as grossPremiumLessComm 
		,Isnull((SELECT TOP 1 CASE WHEN ccm.ispercentage = 1 THEN ccm.commission * (CASE WHEN ccm.isonnrp = 1 THEN ph.Premium ELSE ph.Premium END) / 100 ELSE ccm.commission END FROM nrpcommissioncontractmapping ccm INNER JOIN nrpcommissiontypes cot ON cot.id = ccm.nrpcommissionid WHERE ccm.contractid = a.contractid AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )),0) * mulpleVal  AS brokerage 
		,a.nrpUs * mulpleVal  as netPremium, b.AdminFee * mulpleVal  AS gapAdminstrtion ,a.nrpUs * mulpleVal  as nrpUs
		,ISNULL([BGT],0) * mulpleVal  AS BGT, ISNULL([CIGFL] , 0) * mulpleVal  CIGFL, ISNULL([CT],0) * mulpleVal  CT,ISNULL([ESC],0) * mulpleVal  ESC, ISNULL([IIT],0) * mulpleVal  IIT, ISNULL([NBTv],0) * mulpleVal  NBTv, ISNULL([ST],0) * mulpleVal  ST, ISNULL([STL],0) * mulpleVal  STL, ISNULL([VAT],0) * mulpleVal  VAT, ISNULL([WT] ,0)  * mulpleVal WT,ISNULL([BGTisApplied],0) AS BGTisApplied, ISNULL([CIGFLisApplied] , 0) CIGFLisApplied, ISNULL([CTisApplied],0) CTisApplied,ISNULL([ESCisApplied],0) ESCisApplied, ISNULL([IITisApplied],0) IITisApplied, ISNULL([NBTvisApplied],0) NBTvisApplied, ISNULL([STisApplied],0) STisApplied, ISNULL([STLisApplied],0) STLisApplied, ISNULL([VATisApplied],0) VATisApplied, ISNULL([WTisApplied] ,0) WTisApplied    
FROM
( 
 SELECT a.[PolicyNo] as PolicyNo, case a.IsPolicyCanceled when 1 then 'Cancelled' else 'Active' end as status, DateName( month , DateAdd( month , a.Month , -1 )) as month, 1 as noOfUnits, a.Premium as grossPremium, a.NRP as nrpUs, a.Id, a.contractid, a.dealerid, a.ContractExtensionPremiumId, a.BordxId , a.BordxCountryId,a.TotalTax,a.LocalCurrencyConversionRate , a.GrossPremiumBeforeTax ,case a.IsPolicyCanceled when 1 then -1 else 1 end mulpleVal
 FROM [dbo].[Policy] a  

 UNION ALL  

 SELECT a.[PolicyNo] as PolicyNo, ptt.description as status, DateName( month , DateAdd( month , a.Month , -1 )) as month, 1 as noOfUnits, a.Premium as grossPremium, a.NRP as nrpUs, a.Id, a.contractid, a.dealerid, a.ContractExtensionPremiumId, a.BordxId , a.BordxCountryId , a.TotalTax,a.LocalCurrencyConversionRate , a.GrossPremiumBeforeTax , case when ptt.code = 'Cancellation' then -1 else case when ptt.code = 'Transfer' then 0 else 1 end end mulpleVal 
 FROM [dbo].[Policy] a 
 INNER JOIN [dbo].[PolicyTransaction] pt ON pt.[PolicyId] = a.id  
 INNER JOIN [dbo].[PolicyTransactionType] ptt ON pt.[TransactionTypeId] = ptt.id  
)a  

INNER JOIN [dbo].[BiBordxSummery] b  
 ON b.BordxId = a.BordxId AND b.PolicyId = a.id   
 AND a.dealerid = CASE WHEN UPPER(@var1)='ALL' THEN a.dealerid ELSE @var1 END   
INNER JOIN [dbo].[contract] c   
 ON c.id = a.contractid  
 AND c.insurerid = CASE WHEN UPPER(@var2)='ALL' THEN c.insurerid ELSE @var2 END  
INNER JOIN [dbo].[ReinsurerContract] rinsc ON c.[ReinsurerContractId] = rinsc.id   
 AND  rinsc.[ReinsurerId] = CASE WHEN UPPER(@var3)='ALL' THEN rinsc.[ReinsurerId] ELSE @var3 END    
INNER JOIN [dbo].[Bordx] i 
 ON i.Id =  a.BordxId   
 AND (datefromparts(i.Year, i.Month, 1) BETWEEN ( SELECT TOP 1 datefromparts([Year], [Month], 1) FROM [dbo].[Bordx] WHERE ID = @var4 ) AND ( SELECT TOP 1 datefromparts([Year], [Month], 1) FROM [dbo].[Bordx] WHERE ID = @var5 ))  
LEFT JOIN ( SELECT CountryId, ISNULL([BGT],0) AS BGT, ISNULL([CIGFL] , 0) CIGFL, ISNULL([CT],0) CT,ISNULL([ESC],0) ESC, ISNULL([IIT],0) IIT, ISNULL([NBTv],0) NBTv, ISNULL([ST],0) ST, ISNULL([STL],0) STL, ISNULL([VAT],0) VAT, ISNULL([WT] ,0) WT FROM ( SELECT tt.TaxCode ,ct.TaxValue ,ct.CountryId FROM [dbo].[TaxTypes] tt INNER JOIN [dbo].[CountryTaxes] ct on ct.TaxTypeId = tt.Id ) AS j PIVOT ( SUM(TaxValue) FOR TaxCode IN ([BGT], [CIGFL], [CT], [ESC], [IIT], [NBTv], [ST], [STL], [VAT], [WT]) ) AS p)tct on tct.CountryId = a.BordxCountryId   
LEFT JOIN ( SELECT ContractId , ISNULL([BGT],0) AS BGTisApplied, ISNULL([CIGFL] , 0) CIGFLisApplied, ISNULL([CT],0) CTisApplied,ISNULL([ESC],0) ESCisApplied, ISNULL([IIT],0) IITisApplied, ISNULL([NBTv],0) NBTvisApplied, ISNULL([ST],0) STisApplied, ISNULL([STL],0) STLisApplied, ISNULL([VAT],0) VATisApplied, ISNULL([WT] ,0) WTisApplied FROM ( SELECT tt.TaxCode ,cnt.ContractId , CASE WHEN cnt.ContractId IS NULL THEN 0 ELSE 1 END AS taxApplicable FROM [dbo].[TaxTypes] tt INNER JOIN [dbo].[CountryTaxes] ct on ct.TaxTypeId = tt.Id INNER JOIN [dbo].[ContractTaxes] cnt on cnt.CountryTaxId = ct.Id ) AS j PIVOT ( MAX(taxApplicable) FOR TaxCode IN ([BGT], [CIGFL], [CT], [ESC], [IIT], [NBTv], [ST], [STL], [VAT], [WT]) ) AS p )tcta on tcta.ContractId = a.contractid    
LEFT JOIN policyhistory ph ON ph.policyid = a.id   
ORDER BY invoiceNo , policyNo ASC; 