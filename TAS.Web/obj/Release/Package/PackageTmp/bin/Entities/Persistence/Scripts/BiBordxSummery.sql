insert into BiBordxSummery(
SNo ,
UnderWriterYear , 
ReinsurerName ,
CedentName ,
InsuredDetailsCountry , 
BordxMonth ,
BordxYear ,
DealName ,
DealType ,
Status ,
CoverType ,
WarrantyType ,
AmtUsedAtPolicySale ,
DealerName ,
Location ,
Broker ,
FirstName ,
MiddleName ,
LastName ,
CoBuyer ,
Address ,
POBox ,
City ,
Zip ,
MobileNumber ,
BusinessName ,
BusinessTel ,
BusinessAddress ,
BusinessCity ,
ContactPerson ,
ContactPersonTel ,
VINNo ,
RegNo ,
Category ,
Manufacture ,
Model ,
Variant ,
CylinderCount ,
FourByFour ,
EngineCapacity ,
Gvw ,
GVWCategory ,
ModelYear ,
PolicyNo ,
BookletNo ,
SystemGeneratedNum ,
PolicySoldDate ,
VehiclePurcheseDate ,
VehicleRegistrationDate ,
ManfWarrantyStartDate ,
ManfWarrantyTerminationDate ,
ManfCoverInMonths ,
MileageLimitationInKMs ,
DateOfInsuranceRiskStart ,
DateOfInsuranceRiskTermination ,
ExtensionPeriodInMonths ,
ExtensionPeriodInKms ,
CutOffKm ,
GrossPremium , 
GrossPremiumExTax ,
SalesCommission ,
GrossPremiumLessSalesCommission , 
MarketingFee ,
InsurerFee ,
AdminFee ,
InternalGoodWill ,
DealerCommission ,
DocumentFee ,
ClientBrokerage ,
InsurerNRPRetention ,
ManufactureCommission ,
GrossPremiumLessCommission ,
NRPIncludingBrokerage ,
Brokerage ,
NetAbsoluteRiskPremium ,
SumInsured ,
NRPIncludingBrokerageUS ,
BrokerageUS ,
NetAbsoluteRiskPremiumUS ,
ConversionRateUS ,
StartDateRSA ,
TerminationDateRSA ,
PeriodInMonthsRSA ,
CardNumberRSA ,
GrossPremiumRSA ,
NRPRSA ,
InsurancePolicyNoOther ,
SalesmanOther ,
CommentOther ,
BaseCountryId ,
BaseCountry ,
BaseCurrencyName ,
BaseCurrencyId ,
BaseCurrencyPeriodId ,
CurrencyConversionRate ,
TransactionTypeCode ,
TransactionTypeId , 
ContractId ,
GrossPremiumBeforeTax ,
NRP ,
PolicyId , 
autoId,
BordxId 
)
SELECT Row_number() 
  OVER( 
    ORDER BY D.autoId) as SNo, * FROM (
SELECT DISTINCT 
--------------------------------- 

rec.uwyear                                                            AS 
UnderWriterYear, 

re.reinsurername                                                      AS 
ReinsurerName, 

i.insurershortname                                                    AS 
CedentName, 

con.countryname                                                       AS 
InsuredDetailsCountry, 

LEFT(Datename(month, Dateadd(month, b.month, -1)), 3)                 AS 
BordxMonth, 

b.year                                                                AS 
BordxYear, 

c.dealname                                                            AS 
DealName, 

dt.NAME                                                               AS 
DealType, 

( CASE 
    WHEN c.isactive = 0 THEN 'Inactive' 
    ELSE 'Active' 
  END )                                                               AS 
Status,
   
its.status                                                            AS 
CoverType,
--itemstatus
 
wt.warrantytypedescription                                            AS 
WarrantyType, 

p.hrsusedatpolicysale                                                 AS 
AmtUsedAtPolicySale, 

d.dealername                                                          AS 
DealerName, 

dlc.cityname                                                          AS 
Location, 

bk.Name  															  AS
[Broker],

-------------------------------------------------------------
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.firstname  
	END
																	AS 
FirstName, 

' '                                                                 AS 
MiddleName,
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.lastname 
	END
																	AS 
LastName, 
''																	AS 
CoBuyer ,

CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.address1 + ',' + cust.address2 + ',' + cust.address3 
	END
                                                                    AS 
																	
Address , 

' '																	AS 
POBox, 
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.cityname
	END
                                                                    AS 
City, 

CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.zipcode
	END
                                                                    AS 
Zip, 

 CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.mobileno + ' '
	END
														            AS 
MobileNumber, 
-----------------------------------------------------

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessName
	ELSE
		' '
	END													            AS 
BusinessName,


	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessTelNo
	ELSE
		' '
	END													            AS 
BusinessTel,


	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessAddress1 + ',' + cust.BusinessAddress2 
		+ ',' + cust.BusinessAddress3+ ',' + cust.BusinessAddress4
	ELSE
		' '
	END													            AS 
BusinessAddress,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		city.cityname
	ELSE
		' '
	END													            AS 
BusinessCity,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.firstname + ' ' +cust.lastname 
	ELSE
		' '
	END													            AS 
ContactPerson,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.mobileno + ' '
	ELSE
		' '
	END													           AS 
ContactPersonTel,


----------------------------- 
vd.vinno                                                              AS 
VINNo,
 
vd.plateno                                                            AS 
RegNo,
 
cc.commoditycategorydescription                                       AS 
Category,
 
m.makename                                                            AS 
Manufacture, 

mo.modelname                                                          AS 
Model, 

va.VariantName                                                        as 
Variant,

cyc.[count]                                                           AS 
CylinderCount, 

CASE WHEN (vpa.Id IS NULL) THEN 
    'No'                                                      
ELSE
	'Yes'
END AS 
FourByFour,
 
CONVERT(VARCHAR(50), enc.enginecapacitynumber) 
+ ' ' + enc.mesuretype                                                AS 
EngineCapacity,

Convert(varchar(25),vd.GrossWeight) + ' T'                            AS
Gvw, 

''															AS
GVWCategory,

vd.modelyear                                                          AS 
ModelYear, 

p.policyno                                                            AS 
PolicyNo, 

p.BookletNumber                                                               AS 
BookletNo, 

Upper(con.countrycode + '-' 
      + LEFT(tpab.branchcode, 3) + '-' 
      + LEFT(prt.code, 3) + '-' 
      + RIGHT('00000000'+Cast(p.uniqueref AS VARCHAR), 7))            AS 
SystemGeneratedNum, 

p.PolicySoldDate                                                      As  
PolicySoldDate,

CASE 
  WHEN ct.commoditycode = 'A' THEN vd.itempurchaseddate  
  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate 
  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate
  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate 
END                                                                   AS 
VehiclePurcheseDate,

CASE 
  WHEN ct.commoditycode = 'A' THEN vd.RegistrationDate 
  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate --no data capturing
  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate --no data capturing
  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate  --no datacapturing
END                                                                   AS 
VehicleRegistrationDate, 

CASE WHEN p.MWIsAvailable=1 THEN 
	p.MWStartDate     
ELSE
	CASE WHEN (mw.warrantymonths IS NULL OR mw.warrantymonths=0) THEN
		CAST(-53690 AS DATETIME) 
	ELSE
		p.MWStartDate   
	END                                          
END                                                                   AS 
ManfWarrantyStartDate,

CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(DAY, -1, 
    DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)) 
ELSE
	CASE WHEN (mw.warrantymonths IS NULL OR mw.warrantymonths=0) THEN
		CAST(-53690 AS DATETIME) 
	ELSE
		DATEADD(DAY, -1, 
		DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)) 
	END                                                   
END																	  AS 
ManfWarrantyTerminationDate,

ISNULL(CONVERT(VARCHAR(10), mw.warrantymonths), '-')                  AS 
ManfCoverInMonths, 

CASE WHEN mw.IsUnlimited=1
THEN
	'Unlimited'
ELSE
ISNULL(CONVERT(VARCHAR(10), mw.warrantykm), '-')
END                                                                    AS 
MileageLimitationInKMs,
 
CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)           
ELSE
	p.PolicySoldDate                                         
END																	  AS 
DateOfInsuranceRiskStart,

CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0), 
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)))
	ELSE
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0),p.PolicySoldDate))	                                        
END	                                                                  AS 
DateOfInsuranceRiskTermination,

CONVERT(varchar(10),il.Months)                                       AS 
ExtensionPeriodInMonths, 
     
CASE WHEN il.Km=0 THEN
			'Unlimited'
		ELSE
			CONVERT(varchar(50), il.Km)
		END                                                         
 AS 
ExtensionPeriodInKms, 

CASE WHEN (p.MWIsAvailable=1 AND DATEADD(MONTH,Isnull(mw.warrantymonths, 0),p.MWStartDate)<= p.PolicySoldDate
	AND Isnull(mw.WarrantyKm, 0)<= p.HrsUsedAtPolicySale AND its.status = 'New')
	THEN--mw available and applicable
		CASE WHEN il.TopOfMW=1
		THEN
			CASE WHEN il.Km=0 THEN
				'Unlimited'
			ELSE
				CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
			END
		
		ELSE  
			CASE WHEN il.Km=0 THEN
				'Unlimited'
			ELSE
				CONVERT(varchar(50),il.Km )
			END
		
		END    
	ELSE -- no mw applicable
		CASE WHEN  its.status = 'New'
		THEN
			CASE WHEN il.TopOfMW=1
			THEN
				CASE WHEN il.Km=0 THEN
					'Unlimited'
				ELSE
					CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
				END
			ELSE  
				CASE WHEN il.Km=0 THEN
					'Unlimited'
				ELSE
					CONVERT(varchar(50),il.Km )
				END
			END    
		ELSE -- no mw and used vehicle , so cutoff starts from usage
			CONVERT(varchar(50),Isnull(p.HrsUsedAtPolicySale, 0) + il.Km)
		END
	END
 AS 
CutOffKm, 

p.premium                                                             AS 
GrossPremium, 

p.GrossPremiumBeforeTax                                               AS 
GrossPremiumExTax, 

--sales commission 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Sales%' )), 0.00)                AS 
SalesCommission, 

--end sales commission 
--gross premium less sales commission 
p.premium - Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium -(p.TotalTax/LocalCurrencyConversionRate)
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Sales%' )), 0.00)     AS 
GrossPremiumLessSalesCommission, 
--end gross premium less sales commission 

--marketing fee 
Isnull((SELECT CASE 
                WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Marketing%' )), 0.00)            AS 
MarketingFee, 
--end marketing fee 

--Insurer fee 
Isnull((SELECT CASE 
                WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Insurer Fee%' )), 0.00)          AS 
InsurerFee, 
---end insarance fee 

-- admin fee 
Isnull((SELECT CASE 
                WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Admin%' )), 0.00)                AS 
AdminFee, 
--end admin fee 

--start Internal GoodWill
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Internal GoodWill%' )), 0.00)         AS 
InternalGoodWill, 
--end Internal GoodWill 

--start Dealer Commission
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Dealer Commission%' )), 0.00)         AS 
DealerCommission, 
--end Dealer Commission 

--Start DocumentFee 
Isnull((SELECT CASE 
                WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Document%' )), 0.00)             AS 
DocumentFee, 
--end DocumentFee    

--ClientBrokerage 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Client Brokerage%' )), 0.00)         AS 
ClientBrokerage, 
--end ClientBrokerage 

--start Insurer NRP Retention
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Insurer NRP Retention%' )), 0.00)         AS 
InsurerNRPRetention, 
--end Insurer NRP Retention

--start Manufacture Commission
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Manufacture Commission%' )), 0.00)         AS 
ManufactureCommission, 
--end Manufacture Commission                  

--start gross premium less sales commission                         
p.premium - Isnull((SELECT CASE 
                           WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
                   ELSE ccm.commission 
                   END 
                    FROM   nrpcommissioncontractmapping ccm 
                           INNER JOIN nrpcommissiontypes cot 
                                   ON cot.id = ccm.nrpcommissionid 
                    WHERE  ccm.contractid = c.id 
                           AND cot.NAME LIKE ( '%Sales%' )), 0.00) 
				- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( '%Insurer Fee%' )), 0.00) 
					- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( 'Manufacture Commission%' )), 0.00) 
					- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( 'Insurer NRP Retention%' )), 0.00) 

						- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( 'Dealer Commission%' )), 0.00) 
						- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE (  'Internal GoodWill%' )), 0.00) 
					- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( 'Client Brokerage%' )), 0.00) 

						- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( '%Admin%'  )), 0.00) 
					
						- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE (  '%Document%' )), 0.00) 
						
						- Isnull( 
					(SELECT CASE 
								WHEN ccm.ispercentage = 1 THEN ccm.commission * 
										(CASE WHEN ccm.isonnrp = 1 THEN
										p.NRP
										ELSE
										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
										END)
										/ 100 
					ELSE ccm.commission 
					END 
						FROM   nrpcommissioncontractmapping ccm 
							INNER JOIN nrpcommissiontypes cot 
									ON cot.id = ccm.nrpcommissionid 
						WHERE  ccm.contractid = c.id 
					AND cot.NAME LIKE ( '%Marketing%')), 0.00) 

		
					
		AS 
GrossPremiumLessCommission, 
--end GrossPremiumLessCommission 

--strart NRPIncluding Brokerahe 
p.NRP
+ Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                                                    AS 
NRPIncludingBrokerage, 
--end NRPIncludingBrokerage 

--borkerage 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                AS 
Brokerage, 
--end brokeage

p.NRP																				 AS 
NetAbsoluteRiskPremium, 

CASE 
  WHEN ct.commoditycode = 'A' THEN vd.dealerprice 
  WHEN ct.commoditycode = 'E' THEN bwd.dealerprice 
  WHEN ct.commoditycode = 'O' THEN oid.dealerprice 
  WHEN ct.commoditycode = 'Y' THEN ygd.dealerprice 
END                                                                   AS 
SumInsured, 

--strart NRPIncluding Brokerahe 
p.NRP
+ Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                                                    AS 
NRPIncludingBrokerageUS, 

Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					p.NRP
				 ELSE
					p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
				 END)
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                AS 
BrokerageUS, 

p.NRP                                                              AS 
NetAbsoluteRiskPremiumUS, 

p.LocalCurrencyConversionRate                                         AS 
ConversionRateUS,
 
'N/A'                                                                 AS 
StartDateRSA, 

'N/A'                                                                 AS 
TerminationDateRSA, 

'N/A'                                                                 AS 
PeriodInMonthsRSA, 

'N/A'                                                                 AS 
CardNumberRSA, 

'N/A'                                                                 AS 
GrossPremiumRSA, 

'N/A'                                                                 AS 
NRPRSA, 

'N/A'                                                                 AS 
InsurancePolicyNoOther, 

SalesUser.firstname + ' ' + SalesUser.lastname                        AS 
SalesmanOther, 

p.comment                                                             AS 
CommentOther, 

reccon.id                                                             AS 
BaseCountryId, 

reccon.countryname                                                    AS 
BaseCountry, 

reccurr.currencyname                                                  AS 
BaseCurrencyName, 

reccon.currencyid                                                     AS 
BaseCurrencyId, 

p.currencyperiodid                                                    AS 
BaseCurrencyPeriodId, 

p.localcurrencyconversionrate                                         AS 
CurrencyConversionRate,

ptt.code                                                              AS 
TransactionTypeCode, 

ph.transactiontypeid                                                  AS 
TransactionTypeId,  

p.contractid                                                          AS 
ContractId, 

p.grosspremiumbeforetax                                               AS 
GrossPremiumBeforeTax, 

p.nrp                                                                 AS 
NRP, 

p.id                                                                  AS 
PolicyId,

p.uniqueref                                                           as 
autoId,

p.BordxId															As
BordxId

FROM   policy p 
     
       LEFT JOIN bordxdetails bd 
              ON bd.policyid = p.id 
       LEFT JOIN bordx b 
              ON b.id = bd.bordxid 
       LEFT JOIN commoditytype ct 
              ON ct.commoditytypeid = p.commoditytypeid 
       LEFT JOIN product pr 
              ON pr.id = p.productid 
       LEFT JOIN producttype prt 
              ON prt.id = pr.producttypeid 
       LEFT JOIN dealer d 
              ON d.id = p.dealerid 
       LEFT JOIN dealerlocation dl 
              ON dl.id = p.dealerlocationid 
       LEFT JOIN city dlc 
              ON dlc.id = dl.cityid 
       LEFT JOIN contract c 
              ON c.id = p.contractid 
	   
       LEFT JOIN dealtype dt 
              ON dt.id = c.dealtype 
       LEFT JOIN insurer i 
              ON i.id = c.insurerid 
       LEFT JOIN reinsurercontract rec 
              ON rec.id = c.ReinsurerContractId 
       LEFT JOIN reinsurer re 
              ON re.id = rec.reinsurerid 
		LEFT JOIN Broker bk 
              ON bk.id = rec.brokerid 
       LEFT JOIN extensiontype e 
              ON e.id = p.extensiontypeid 
       LEFT JOIN currency curr 
              ON curr.id = p.premiumcurrencytypeid 
       LEFT JOIN currency currDealer 
              ON currDealer.id = p.dealerpaymentcurrencytypeid 
       LEFT JOIN currency currCustomer 
              ON currCustomer.id = p.customerpaymentcurrencytypeid 
       LEFT JOIN customer cust 
              ON cust.id = p.customerid 
	   LEFT JOIN CustomerType custType
			ON cust.CustomerTypeId = custType.Id
       LEFT JOIN country con 
              ON con.id = c.countryid 
       LEFT JOIN country reccon 
              ON reccon.id = rec.countryid 
       LEFT JOIN currency reccurr 
              ON reccurr.id = reccon.currencyid 
       LEFT JOIN city city 
              ON city.id = cust.cityid 
       LEFT JOIN vehiclepolicy vp 
              ON vp.policyid = p.id 
       LEFT JOIN vehicledetails vd 
              ON vd.id = vp.vehicleid 
       LEFT JOIN bandwpolicy bwp 
              ON bwp.policyid = p.id 
       LEFT JOIN brownandwhitedetails bwd 
              ON bwd.id = bwp.bandwid 
       LEFT JOIN otheritempolicy oip 
              ON oip.policyid = p.id 
       LEFT JOIN otheritemdetails oid 
              ON oid.id = oip.otheritemid 
       LEFT JOIN yellowgoodpolicy ygp 
              ON ygp.policyid = p.id 
       LEFT JOIN yellowgooddetails ygd 
              ON ygd.id = ygp.yellowgoodid 
       LEFT JOIN itemstatus its 
              ON its.id = ( CASE 
                              WHEN ct.commoditycode = 'A' THEN vd.itemstatusid 
                              WHEN ct.commoditycode = 'E' THEN bwd.itemstatusid 
                              WHEN ct.commoditycode = 'O' THEN oid.itemstatusid 
                              WHEN ct.commoditycode = 'Y' THEN ygd.itemstatusid 
                            END ) 
       LEFT JOIN commoditycategory cc 
              ON cc.commoditycategoryid = ( CASE 
                                              WHEN ct.commoditycode = 'A' THEN 
                                              vd.categoryid 
                                              WHEN ct.commoditycode = 'E' THEN 
                                              bwd.categoryid 
                                              WHEN ct.commoditycode = 'O' THEN 
                                              oid.categoryid 
                                              WHEN ct.commoditycode = 'Y' THEN 
                                              ygd.categoryid 
                                            END ) 
       LEFT JOIN make m 
              ON m.id = ( CASE 
                            WHEN ct.commoditycode = 'A' THEN vd.makeid 
                            WHEN ct.commoditycode = 'E' THEN bwd.makeid 
                            WHEN ct.commoditycode = 'O' THEN oid.makeid 
                            WHEN ct.commoditycode = 'Y' THEN ygd.makeid 
                          END ) 
       LEFT JOIN model mo 
              ON mo.id = ( CASE 
                             WHEN ct.commoditycode = 'A' THEN vd.modelid 
                             WHEN ct.commoditycode = 'E' THEN bwd.modelid 
                             WHEN ct.commoditycode = 'O' THEN oid.modelid 
                             WHEN ct.commoditycode = 'Y' THEN ygd.modelid 
                           END ) 
		LEFT JOIN variant va 
			ON va.id = ( CASE 
                        WHEN ct.commoditycode = 'A' THEN vd.Variant 
                        --WHEN ct.commoditycode = 'E' THEN bwd.modelid 
                        --WHEN ct.commoditycode = 'O' THEN oid.modelid 
                        --WHEN ct.commoditycode = 'Y' THEN ygd.modelid 
						END ) 
       LEFT JOIN cylindercount cyc 
              ON cyc.id = vd.cylindercountid 
       LEFT JOIN enginecapacity enc 
              ON enc.id = vd.enginecapacityid 
		LEFT JOIN ManufacturerWarrantyDetails mwd 
              ON mwd.modelid = mo.id 
                 AND mwd.countryid = rec.countryid 
       LEFT JOIN manufacturerwarranty mw 
              ON mw.makeid = m.id 
                  AND mw.id = mwd.ManufacturerWarrantyId 

       LEFT JOIN contractextensions ce 
              ON ce.id = p.ContractInsuaranceLimitationId 
		LEFT JOIN ContractExtensionPremium cep 
              ON cep.id = p.ContractExtensionPremiumId 
		LEFT JOIN ContractInsuaranceLimitation cil 
              ON cil.id = p.ContractExtensionsId
		LEFT JOIN InsuaranceLimitation il 
              ON il.id = cil.InsuaranceLimitationId	   
		LEFT JOIN warrantytype wt 
              ON wt.id = cep.WarrentyTypeId 
       LEFT JOIN contractextensionvariant cev 
              ON cev.contractextensionid = ce.id  and cev.VariantId = vd.Variant
       LEFT JOIN variant vari 
              ON vari.id = cev.variantid 
	   LEFT JOIN VariantPremiumAddon vpa 
              ON  vpa.VariantId = vari.id  and vpa.PremiumAddonTypeId in  (
			  SELECT Id from PremiumAddonType WHERE CommodityTypeId = ct.commoditytypeid AND
			  AddonTypeCode = 'F'
			  )
       LEFT JOIN premiumbasedon pboNett 
              ON pboNett.id = cep.PremiumBasedOnNett 
       LEFT JOIN premiumbasedon pboGross 
              ON pboGross.id = cep.PremiumBasedOnGross 
       LEFT JOIN nrpcommissioncontractmapping nrpccm 
              ON nrpccm.contractid = c.id 
                 AND nrpccm.nrpcommissionid IN ((SELECT id 
                                                 FROM   nrpcommissiontypes 
                                                 WHERE 
                     NAME IN ( 'Admin Fee', 
                               'Sales Commission' 
                             ))) 
       LEFT JOIN internaluser SalesUser 
              ON SalesUser.id = p.salespersonid 
       LEFT JOIN policyhistory ph 
              ON ph.policyid = p.id 
       LEFT JOIN policytransactiontype ptt 
              ON ptt.id = ph.transactiontypeid 
       LEFT JOIN tpabranch tpab 
              ON tpab.id = p.tpabranchid

WHERE  b.id = '{bordexId}' 
GROUP  BY 
			p.BordxId,														
			p.id, 
			c.id, 
			rec.uwyear, 
			re.reinsurername, 
			i.insurershortname, 
			b.month, 
			b.year, 
			c.dealname, 
			dt.NAME, 
			c.isactive, 
			wt.warrantytypedescription, 
			p.hrsusedatpolicysale, 
			d.dealername, 
			dl.location, 
			cust.address1, 
			cust.address2, 
			cust.address3, 
			city.cityname, 
			city.zipcode, 
			cust.mobileno, 
			its.status, 
			vd.vinno, 
			vd.plateno, 
			cc.commoditycategorydescription, 
			m.makename, 
			mo.modelname, 
			va.VariantName,
			cyc.[count], 
			enc.mesuretype, 
			enc.enginecapacitynumber, 
			vd.modelyear, 
			vd.itempurchaseddate, 
			mw.warrantymonths, 
			mw.warrantykm, 
			mo.noofdaystoriskstart, 
			p.policyenddate, 
			ce.attributespecification, 
			p.premium, 
			p.NRP, 
			rec.contractno, 
			cc.commoditycategorycode, 
			p.policyno, 
			cust.firstname, 
			cust.lastname, 
			rec.contractno, 
			c.startdate, 
			c.enddate, 
			ct.commoditytypedescription, 
			curr.code, 
			p.policyno, 
			city.cityname, 
			con.countryname, 
			p.policystartdate, 
			p.policyenddate, 
			cust.businessname, 
			p.comment, 
			c.liabilitylimitation, 
			ct.commoditycode, 
			bwd.itempurchaseddate, 
			oid.itempurchaseddate, 
			ygd.itempurchaseddate, 
			reccon.id, 
			reccon.countryname, 
			reccon.currencyid, 
			reccurr.currencyname, 
			p.currencyperiodid, 
			p.localcurrencyconversionrate, 
			ph.transactiontypeid, 
			ptt.code, 
			vd.dealerprice, 
			bwd.dealerprice, 
			oid.dealerprice, 
			ygd.dealerprice, 
			pboNett.code, 
			p.Premium, 
			pboGross.code, 
			SalesUser.firstname, 
			SalesUser.lastname, 
			dlc.cityname, 
			tpab.branchcode, 
			con.countrycode, 
			p.uniqueref, 
			prt.code, 
			p.contractid, 
			p.grosspremiumbeforetax, 
			p.nrp, 
			il.Months,
			il.Km,
			il.TopOfMW,
			vd.GrossWeight,
			p.BookletNumber,
			bk.Name,
			p.MWStartDate,
			p.TotalTax,
			vpa.Id,
			p.Co_Customer,
			custType.CustomerTypeName,
			cust.BusinessName,
			cust.BusinessTelNo,
			cust.BusinessAddress1,
			cust.BusinessAddress2,
			cust.BusinessAddress3,
			cust.BusinessAddress4,
			p.GrossPremiumBeforeTax,
			p.PolicySoldDate,
			vd.RegistrationDate,
			p.MWIsAvailable,
			p.LocalCurrencyConversionRate,
			p.MWIsAvailable,
			mw.IsUnlimited
UNION 
SELECT DISTINCT 
--------------------------------- 

rec.uwyear                                                            AS 
UnderWriterYear, 

re.reinsurername                                                      AS 
ReinsurerName, 

i.insurershortname                                                    AS 
CedentName, 

con.countryname                                                       AS 
InsuredDetailsCountry, 

LEFT(Datename(month, Dateadd(month, b.month, -1)), 3)                 AS 
BordxMonth, 

b.year                                                                AS 
BordxYear, 

c.dealname                                                            AS 
DealName, 

dt.NAME                                                               AS 
DealType, 

( CASE 
    WHEN c.isactive = 0 THEN 'Inactive' 
    ELSE 'Active' 
  END )                                                               AS 
Status, 

its.status                                                            AS 
CoverType,--itemstatus 

wt.warrantytypedescription                                            AS 
WarrantyType, 

ph.hrsusedatpolicysale                                                AS 
AmtUsedAtPolicySale, 

d.dealername                                                          AS 
DealerName, 

dlc.cityname                                                          AS 
Location, 

bk.Name 															  AS
[Broker],

-------------------------------------------------------------
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.firstname  
	END
AS 
FirstName, 

' '                                                                 AS 
MiddleName,

CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.lastname 
	END
                                                                  AS 
LastName, 

''                                                     AS 
CoBuyer ,

CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.address1 + ',' + cust.address2 + ',' + cust.address3 
	END
                                                                    AS 
Address ,
 
' '                                                               AS 
POBox,
 
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.cityname
	END
                                                                    AS 
City,
 
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.zipcode
	END
                                                                    AS 
Zip,
 
 CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.mobileno + ' '
	END
														            AS 
MobileNumber, 
-----------------------------------------------------

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessName
	ELSE
		' '
	END													            AS 
BusinessName,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessTelNo
	ELSE
		' '
	END													            AS 
BusinessTel,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessAddress1 + ',' + cust.BusinessAddress2 
		+ ',' + cust.BusinessAddress3+ ',' + cust.BusinessAddress4
	ELSE
		' '
	END													            AS 
BusinessAddress,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		city.cityname
	ELSE
		' '
	END													            AS 
BusinessCity,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.firstname + ' ' +cust.lastname 
	ELSE
		' '
	END													            AS 
ContactPerson,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.mobileno + ' '
	ELSE
		' '
	END													           AS 
ContactPersonTel,


----------------------------- 
----------------------------- 
vd.vinno                                                              AS 
VINNo,
 
vd.plateno                                                            AS 
RegNo,
 
cc.commoditycategorydescription                                       AS 
Category, 

m.makename                                                            AS 
Manufacture, 

mo.modelname                                                          AS 
Model,
 
va.VariantName                                                         as 
Variant,

cyc.[count]                                                           AS 
CylinderCount, 

CASE WHEN (vpa.Id IS NULL) THEN 
    'No'                                                      
ELSE
	'Yes'
END																	AS 
FourByFour,
  
CONVERT(VARCHAR(50), enc.enginecapacitynumber) 
+ ' ' + enc.mesuretype                                                AS 
EngineCapacity,

''                                                        AS 
Gvw ,

''													AS
GVWCategory,

vd.modelyear                                                          AS 
ModelYear, 

ph.policyno                                                           AS 
PolicyNo, 

p.BookletNumber                                                                   AS 
BookletNo, 

Upper(con.countrycode + '-' 
      + LEFT(tpab.branchcode, 3) + '-' 
      + LEFT(prt.code, 3) + '-' 
      + RIGHT('00000000'+Cast(p.uniqueref AS VARCHAR), 7))            AS 
SystemGeneratedNum, 

p.PolicySoldDate                                                      As  
PolicySoldDate, 

--policy history data not capturing
CASE 
  WHEN ct.commoditycode = 'A' THEN vd.itempurchaseddate  
  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate 
  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate
  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate 
END                                                                   AS 
VehiclePurcheseDate,

CASE 
  WHEN ct.commoditycode = 'A' THEN vd.RegistrationDate 
  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate --no data capturing
  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate --no data capturing
  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate  --no datacapturing
END                                                                   AS 
VehicleRegistrationDate, 

p.MWStartDate															as 
ManfWarrantyStartDate,

Dateadd(day, -1, 
           Dateadd(month, Isnull(mw.warrantymonths, 0), p.MWStartDate))  as
ManfWarrantyTerminationDate,

Isnull(CONVERT(VARCHAR(10), mw.warrantymonths), '-')                  AS 
ManfCoverInMonths, 

CASE WHEN mw.IsUnlimited=1
THEN
	'Unlimited'
ELSE
ISNULL(CONVERT(VARCHAR(10), mw.warrantykm), '-')
END                                                                  as 
MileageLimitationInKMs, 

Dateadd(month, Isnull(mw.warrantymonths, 0), p.MWStartDate)          as 
DateOfInsuranceRiskStart,

Dateadd(day, -1,Dateadd(month,Isnull(il.Months, 0), Dateadd(month, Isnull(mw.warrantymonths, 0), p.MWStartDate)))as 
DateOfInsuranceRiskTermination,

--Substring(ce.attributespecification, 0, 
--Charindex('-', ce.attributespecification) - 2)                        AS 
   CONVERT(varchar(10),il.Months)                                                            as 
ExtensionPeriodInMonths,
    
CASE WHEN il.Km=0 THEN
			'Unlimited'
		ELSE
			CONVERT(varchar(50), il.Km)
		END                                                    
 AS 
ExtensionPeriodInKms, 

CASE WHEN (p.MWIsAvailable=1 AND DATEADD(MONTH , Isnull(mw.warrantymonths, 0),p.MWStartDate)<= p.PolicySoldDate
	AND Isnull(mw.WarrantyKm, 0)<= p.HrsUsedAtPolicySale AND its.status = 'New')
	THEN--mw available and applicable
		CASE WHEN il.TopOfMW=1
		THEN
			CASE WHEN il.Km=0 THEN
				'Unlimited'
			ELSE
				CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
			END
		
		ELSE  
			CASE WHEN il.Km=0 THEN
				'Unlimited'
			ELSE
				CONVERT(varchar(50),il.Km )
			END
		
		END    
	ELSE -- no mw applicable
		CASE WHEN  its.status = 'New'
		THEN
			CASE WHEN il.TopOfMW=1
			THEN
				CASE WHEN il.Km=0 THEN
					'Unlimited'
				ELSE
					CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
				END
			ELSE  
				CASE WHEN il.Km=0 THEN
					'Unlimited'
				ELSE
					CONVERT(varchar(50),il.Km )
				END
			END    
		ELSE -- no mw and used vehicle , so cutoff starts from usage
			CONVERT(varchar(50),Isnull(p.HrsUsedAtPolicySale, 0) + il.Km)
		END
	END
 AS 
CutOffKm, 


ph.premium                                                            AS 
GrossPremium, 

p.GrossPremiumBeforeTax                                               AS 
GrossPremiumExTax, 

--sales commission 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Sales%' )), 0.00)                AS 
SalesCommission, 
--end sales commission 

--gross premium less sales commission 
ph.premium - Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Sales%' )), 0.00)    AS 
GrossPremiumLessSalesCommission, 
--end gross premium less sales commission 

--marketing fee 
Isnull((SELECT CASE 
               WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Marketing%' )), 0.00)             as 
MarketingFee,
--end marketing fee 

--Insurer fee 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Insurer Fee%' )), 0.00)          AS 
InsurerFee, 
---end insarance fee 

-- admin fee 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Admin%' )), 0.00)                AS 
AdminFee, 
--end admin fee 

--start Internal GoodWill
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Internal GoodWill%' )), 0.00)         AS 
InternalGoodWill, 
--end Internal GoodWill 

--start Dealer Commission
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Dealer Commission%' )), 0.00)         AS 
DealerCommission, 
--end Dealer Commission

--Start DocumentFee 
Isnull((SELECT CASE 
              WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( '%Document%' )), 0.00)             AS 
DocumentFee, 
--end DocumentFee    

--ClientBrokerage 
Isnull((SELECT CASE 
            WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Client Brokerage%' )), 0.00)         AS 
ClientBrokerage, 
--end ClientBrokerage 
 
--start Insurer NRP Retention
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Insurer NRP Retention%' )), 0.00)         AS 
InsurerNRPRetention, 
--end Insurer NRP Retention

--start Manufacture Commission
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Manufacture Commission%' )), 0.00)         AS 
ManufactureCommission, 
--end Manufacture Commission                                      

--start gross premium less sales commission                         
ph.premium - Isnull((SELECT CASE 
                             WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
                    ELSE ccm.commission 
                    END 
                     FROM   nrpcommissioncontractmapping ccm 
                            INNER JOIN nrpcommissiontypes cot 
                                    ON cot.id = ccm.nrpcommissionid 
                     WHERE  ccm.contractid = c.id 
                            AND cot.NAME LIKE ( '%Sales%' )), 0.00) - Isnull( 
(SELECT CASE 
          WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
ELSE ccm.commission 
END 
 FROM   nrpcommissioncontractmapping ccm 
        INNER JOIN nrpcommissiontypes cot 
                ON cot.id = ccm.nrpcommissionid 
 WHERE  ccm.contractid = c.id 
        AND cot.NAME LIKE ( '%Insurer Fee%' )), 0.00)                 AS 
GrossPremiumLessCommission, 

CASE WHEN pboNett.code='RP' THEN CASE WHEN ct.commoditycode='A' THEN 
vd.dealerprice*p.NRP/100 WHEN ct.commoditycode='E' THEN 
bwd.dealerprice*p.NRP/100 WHEN ct.commoditycode='O' THEN 
oid.dealerprice*p.NRP/100 WHEN 
ct.commoditycode='Y' THEN ygd.dealerprice*p.NRP/100 END ELSE 
p.NRP END 
+ Isnull((SELECT CASE WHEN ccm.ispercentage=1 THEN ccm.commission* (CASE WHEN 
pboGross.code='RP' THEN CASE WHEN ct.commoditycode='A' THEN vd.dealerprice* ( 
CASE WHEN ccm.isonnrp=1 THEN p.NRP ELSE p.Premium END) /100 WHEN 
ct.commoditycode='E' THEN bwd.dealerprice* (CASE WHEN ccm.isonnrp=1 THEN 
p.NRP ELSE p.Premium END) /100 WHEN ct.commoditycode='O' THEN 
oid.dealerprice* ( 
CASE WHEN ccm.isonnrp=1 THEN p.NRP ELSE p.Premium END) /100 WHEN 
ct.commoditycode='Y' THEN ygd.dealerprice* (CASE WHEN ccm.isonnrp=1 THEN 
p.NRP ELSE p.Premium END) /100 END ELSE (CASE WHEN ccm.isonnrp=1 
THEN 
p.NRP ELSE p.Premium END) END)/100 ELSE ccm.commission END FROM 
nrpcommissioncontractmapping ccm INNER JOIN nrpcommissiontypes cot ON 
cot.id=ccm.nrpcommissionid WHERE ccm.contractid = c.id AND cot.NAME LIKE ( 
'Reinsurer Brokerage%') ), 0.00)                                                   AS 
NRPIncludingBrokerage, 
--end NRPIncludingBrokerage 

--borkerage 
Isnull((SELECT CASE 
                 WHEN ccm.ispercentage = 1 THEN ccm.commission *
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium--should add nrp to ph
				 ELSE
					ph.Premium
				 END) / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                AS 
Brokerage, 
--end brokeage 

CASE 
  WHEN pboNett.code = 'RP' THEN 
    CASE 
      WHEN ct.commoditycode = 'A' THEN vd.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'E' THEN bwd.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'O' THEN oid.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'Y' THEN ygd.dealerprice * p.NRP / 100 
    END 
  ELSE p.NRP 
END                                                                   AS 
NetAbsoluteRiskPremium, 

CASE 
  WHEN ct.commoditycode = 'A' THEN vd.dealerprice 
  WHEN ct.commoditycode = 'E' THEN bwd.dealerprice 
  WHEN ct.commoditycode = 'O' THEN oid.dealerprice 
  WHEN ct.commoditycode = 'Y' THEN ygd.dealerprice 
END                                                                   AS 
SumInsured, 

CASE WHEN pboNett.code='RP' THEN CASE WHEN ct.commoditycode='A' THEN 
vd.dealerprice*p.NRP/100 WHEN ct.commoditycode='E' THEN 
bwd.dealerprice*p.NRP/100 WHEN ct.commoditycode='O' THEN 
oid.dealerprice*p.NRP/100 WHEN 
ct.commoditycode='Y' THEN ygd.dealerprice*p.NRP/100 END ELSE 
p.NRP END 
+ Isnull((SELECT CASE WHEN ccm.ispercentage=1 THEN ccm.commission* (CASE WHEN 
pboGross.code='RP' THEN CASE WHEN ct.commoditycode='A' THEN vd.dealerprice* ( 
CASE WHEN ccm.isonnrp=1 THEN p.NRP ELSE p.Premium END) /100 WHEN 
ct.commoditycode='E' THEN bwd.dealerprice* (CASE WHEN ccm.isonnrp=1 THEN 
p.NRP ELSE p.Premium END) /100 WHEN ct.commoditycode='O' THEN 
oid.dealerprice* ( 
CASE WHEN ccm.isonnrp=1 THEN p.NRP ELSE p.Premium END) /100 WHEN 
ct.commoditycode='Y' THEN ygd.dealerprice* (CASE WHEN ccm.isonnrp=1 THEN 
p.NRP ELSE p.Premium END) /100 END ELSE (CASE WHEN ccm.isonnrp=1 
THEN 
p.NRP ELSE p.Premium END) END)/100 ELSE ccm.commission END FROM 
nrpcommissioncontractmapping ccm INNER JOIN nrpcommissiontypes cot ON 
cot.id=ccm.nrpcommissionid WHERE ccm.contractid = c.id AND cot.NAME LIKE ( 
'Reinsurer Brokerage%') ), 0.00)                                                   AS 
NRPIncludingBrokerageUS, 

Isnull((SELECT CASE 
                  WHEN ccm.ispercentage = 1 THEN ccm.commission * 
				 (CASE WHEN ccm.isonnrp = 1 THEN
					ph.Premium --should add nrp to ph
				 ELSE
					ph.Premium
				 END) 
				 / 100 
       ELSE ccm.commission 
       END 
        FROM   nrpcommissioncontractmapping ccm 
               INNER JOIN nrpcommissiontypes cot 
                       ON cot.id = ccm.nrpcommissionid 
        WHERE  ccm.contractid = c.id 
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)                AS 
BrokerageUS, 

CASE 
  WHEN pboNett.code = 'RP' THEN 
    CASE 
      WHEN ct.commoditycode = 'A' THEN vd.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'E' THEN bwd.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'O' THEN oid.dealerprice * p.NRP / 100 
      WHEN ct.commoditycode = 'Y' THEN ygd.dealerprice * p.NRP / 100 
    END 
  ELSE p.NRP 
END                                                                   AS 
NetAbsoluteRiskPremiumUS, 

p.LocalCurrencyConversionRate                                         AS 
ConversionRateUS,

'N/A'                                                                 AS 
StartDateRSA, 

'N/A'                                                                 AS 
TerminationDateRSA, 

'N/A'                                                                 AS 
PeriodInMonthsRSA, 

'N/A'                                                                 AS 
CardNumberRSA, 

'N/A'                                                                 AS 
GrossPremiumRSA, 

'N/A'                                                                 AS 
NRPRSA, 

'N/A'                                                                 AS 
InsurancePolicyNoOther, 

SalesUser.firstname + ' ' + SalesUser.lastname                        AS 
SalesmanOther, 

ph.comment                                                            AS 
CommentOther, 

reccon.id                                                             AS 
BaseCountryId, 

reccon.countryname                                                    AS 
BaseCountry, 

reccurr.currencyname                                                  AS 
BaseCurrencyName, 

reccon.currencyid                                                     AS 
BaseCurrencyId, 

ph.currencyperiodid                                                   AS 
BaseCurrencyPeriodId, 

p.localcurrencyconversionrate                                         AS 
CurrencyConversionRate, 

'EndorsementOld'                                                      AS 
TransactionTypeCode, 

ph.transactiontypeid                                                  AS 
TransactionTypeId, 

ph.contractid                                                         AS 
ContractId, 

p.grosspremiumbeforetax                                               AS 
GrossPremiumBeforeTax, 

p.nrp                                                                 AS 
NRP, 

p.id                                                                  AS 
PolicyId,

p.uniqueref                                                           as 
autoId,

p.BordxId															As
BordxId
FROM   policy p 
       LEFT JOIN policyhistory ph 
              ON ph.policyid = p.id 
       LEFT JOIN warrantytype wt 
              ON wt.id = ph.covertypeid 
       LEFT JOIN bordxdetails bd 
              ON bd.policyid = p.id 
       LEFT JOIN bordx b 
              ON b.id = bd.bordxid 
       LEFT JOIN commoditytype ct 
              ON ct.commoditytypeid = ph.commoditytypeid 
       LEFT JOIN product pr 
              ON pr.id = ph.productid 
       LEFT JOIN producttype prt 
              ON prt.id = pr.producttypeid 
       LEFT JOIN dealer d 
              ON d.id = ph.dealerid 
       LEFT JOIN dealerlocation dl 
              ON dl.id = ph.dealerlocationid 
       LEFT JOIN city dlc 
              ON dlc.id = dl.cityid 
       LEFT JOIN contract c 
              ON c.id = ph.contractid 
	   
       LEFT JOIN dealtype dt 
              ON dt.id = c.dealtype 
       LEFT JOIN insurer i 
              ON i.id = c.insurerid 
       LEFT JOIN reinsurercontract rec 
              ON rec.id = c.ReinsurerContractId 
	    LEFT JOIN Broker bk 
              ON bk.id = rec.brokerid 
       LEFT JOIN reinsurer re 
              ON re.id = rec.reinsurerid 
       LEFT JOIN extensiontype e 
              ON e.id = ph.extensiontypeid 
       LEFT JOIN currency curr 
              ON curr.id = ph.premiumcurrencytypeid 
       LEFT JOIN currency currDealer 
              ON currDealer.id = ph.dealerpaymentcurrencytypeid 
       LEFT JOIN currency currCustomer 
              ON currCustomer.id = ph.customerpaymentcurrencytypeid 
       LEFT JOIN customer cust 
              ON cust.id = ph.customerid 
	   LEFT JOIN CustomerType custType
			ON cust.CustomerTypeId = custType.Id
       LEFT JOIN country con 
              ON con.id = c.countryid 
       LEFT JOIN country reccon 
              ON reccon.id = rec.countryid 
       LEFT JOIN currency reccurr 
              ON reccurr.id = reccon.currencyid 
       LEFT JOIN city city 
              ON city.id = cust.cityid 
       LEFT JOIN vehiclepolicy vp 
              ON vp.policyid = p.id 
       LEFT JOIN vehicledetailshistory vd 
              ON vd.vehicledetailsid = vp.vehicleid 
       LEFT JOIN bandwpolicy bwp 
              ON bwp.policyid = p.id 
       LEFT JOIN brownandwhitedetailshistory bwd 
              ON bwd.brownandwhitedetailsid = bwp.bandwid 
       LEFT JOIN otheritempolicy oip 
              ON oip.policyid = p.id 
       LEFT JOIN otheritemdetailshistory oid 
              ON oid.otheritemdetailsid = oip.otheritemid 
       LEFT JOIN yellowgoodpolicy ygp 
              ON ygp.policyid = p.id 
       LEFT JOIN yellowgooddetailshistory ygd 
              ON ygd.yellowgooddetailsid = ygp.yellowgoodid 
       LEFT JOIN itemstatus its 
              ON its.id = ( CASE 
                              WHEN ct.commoditycode = 'A' THEN vd.itemstatusid 
                              WHEN ct.commoditycode = 'E' THEN bwd.itemstatusid 
                              WHEN ct.commoditycode = 'O' THEN oid.itemstatusid 
                              WHEN ct.commoditycode = 'Y' THEN ygd.itemstatusid 
                            END ) 
       LEFT JOIN commoditycategory cc 
              ON cc.commoditycategoryid = ( CASE 
                                              WHEN ct.commoditycode = 'A' THEN 
                                              vd.categoryid 
                                              WHEN ct.commoditycode = 'E' THEN 
                                              bwd.categoryid 
                                              WHEN ct.commoditycode = 'O' THEN 
                                              oid.categoryid 
                                              WHEN ct.commoditycode = 'Y' THEN 
                                              ygd.categoryid 
                                            END ) 
       LEFT JOIN make m 
              ON m.id = ( CASE 
                            WHEN ct.commoditycode = 'A' THEN vd.makeid 
                            WHEN ct.commoditycode = 'E' THEN bwd.makeid 
                            WHEN ct.commoditycode = 'O' THEN oid.makeid 
                            WHEN ct.commoditycode = 'Y' THEN ygd.makeid 
                          END ) 
       LEFT JOIN model mo 
              ON mo.id = ( CASE 
                             WHEN ct.commoditycode = 'A' THEN vd.modelid 
                             WHEN ct.commoditycode = 'E' THEN bwd.modelid 
                             WHEN ct.commoditycode = 'O' THEN oid.modelid 
                             WHEN ct.commoditycode = 'Y' THEN ygd.modelid 
                           END ) 
		LEFT JOIN variant va  
		ON va.id = ( CASE 
					WHEN ct.commoditycode = 'A' THEN vd.Variant 
					--WHEN ct.commoditycode = 'E' THEN bwd.modelid 
					--WHEN ct.commoditycode = 'O' THEN oid.modelid 
					--WHEN ct.commoditycode = 'Y' THEN ygd.modelid 
					END ) 
       LEFT JOIN cylindercount cyc 
              ON cyc.id = vd.cylindercountid 
       LEFT JOIN enginecapacity enc 
              ON enc.id = vd.enginecapacityid 
     	LEFT JOIN ManufacturerWarrantyDetails mwd 
              ON mwd.modelid = mo.id 
                 AND mwd.countryid = rec.countryid 
       LEFT JOIN manufacturerwarranty mw 
              ON mw.makeid = m.id 
                  AND mw.id = mwd.ManufacturerWarrantyId 
     LEFT JOIN contractextensions ce 
              ON ce.id = p.ContractInsuaranceLimitationId 
		LEFT JOIN ContractExtensionPremium cep 
              ON cep.id = p.ContractExtensionPremiumId 
		LEFT JOIN ContractInsuaranceLimitation cil 
              ON cil.id = p.ContractExtensionsId
		LEFT JOIN InsuaranceLimitation il 
              ON il.id = cil.InsuaranceLimitationId	   
	
       LEFT JOIN contractextensionvariant cev  
              ON cev.contractextensionid = ce.id  and cev.VariantId = vd.Variant
       LEFT JOIN variant vari 
              ON vari.id = cev.variantid 
		 LEFT JOIN VariantPremiumAddon vpa 
              ON  vpa.VariantId = vari.id  and vpa.PremiumAddonTypeId in  (
			  SELECT Id from PremiumAddonType WHERE CommodityTypeId = ct.commoditytypeid AND
			  AddonTypeCode = 'F'
			  )
       LEFT JOIN premiumbasedon pboNett 
              ON pboNett.id = cep.PremiumBasedOnNett 
       LEFT JOIN premiumbasedon pboGross 
              ON pboGross.id = cep.PremiumBasedOnGross 
       LEFT JOIN nrpcommissioncontractmapping nrpccm 
              ON nrpccm.contractid = c.id 
                 AND nrpccm.nrpcommissionid IN ((SELECT id 
                                                 FROM   nrpcommissiontypes 
                                                 WHERE 
                     NAME IN ( 'Admin Fee', 
                               'Sales Commission' 
                             ))) 
       LEFT JOIN internaluser SalesUser 
              ON SalesUser.id = ph.salespersonid 
           LEFT JOIN policytransactiontype ptt 
              ON ptt.id = ph.transactiontypeid 
       LEFT JOIN tpabranch tpab 
              ON tpab.id = p.tpabranchid 

WHERE  b.id =  '{bordexId}'   
       AND ptt.code = 'Endorsement' 
GROUP  BY 
		p.BordxId,
		p.id, 
		c.id, 
		rec.uwyear, 
		re.reinsurername, 
		i.insurershortname, 
		b.month, 
		b.year, 
		c.dealname, 
		dt.NAME, 
		c.isactive, 
		wt.warrantytypedescription, 
		ph.hrsusedatpolicysale, 
		d.dealername, 
		dl.location, 
		cust.address1, 
		cust.address2, 
		cust.address3, 
		city.cityname, 
		city.zipcode, 
		cust.mobileno, 
		its.status, 
		vd.vinno, 
		vd.plateno, 
		cc.commoditycategorydescription, 
		m.makename, 
		mo.modelname, 
		va.VariantName,
		cyc.[count], 
		enc.mesuretype, 
		enc.enginecapacitynumber, 
		vd.modelyear, 
		vd.itempurchaseddate, 
		mw.warrantymonths, 
		mw.warrantykm, 
		mo.noofdaystoriskstart, 
		ph.policyenddate, 
		ce.attributespecification, 
		ph.premium, 
		rec.contractno, 
		cc.commoditycategorycode, 
		ph.policyno, 
		cust.firstname, 
		cust.lastname, 
		rec.contractno, 
		c.startdate, 
		c.enddate, 
		ct.commoditytypedescription, 
		curr.code, 
		ph.policyno, 
		city.cityname, 
		con.countryname, 
		ph.policystartdate, 
		ph.policyenddate, 
		cust.businessname, 
		ph.comment, 
		c.liabilitylimitation, 
		ct.commoditycode, 
		bwd.itempurchaseddate, 
		oid.itempurchaseddate, 
		ygd.itempurchaseddate, 
		reccon.id, 
		reccon.countryname, 
		reccon.currencyid, 
		reccurr.currencyname, 
		ph.currencyperiodid, 
		p.localcurrencyconversionrate, 
		ph.transactiontypeid, 
		ptt.code, 
		ph.id, 
		vd.dealerprice, 
		bwd.dealerprice, 
		oid.dealerprice, 
		ygd.dealerprice, 
		pboNett.code, 
		p.Premium, 
		pboGross.code, 
		SalesUser.firstname, 
		SalesUser.lastname, 
		dlc.cityname, 
		tpab.branchcode, 
		con.countrycode, 
		p.uniqueref, 
		prt.code, 
		ph.contractid, 
		p.grosspremiumbeforetax, 
		p.nrp,
		p.BookletNumber  ,
		bk.Name,
		p.MWStartDate,
		il.Months,
		il.TopOfMW,
		il.Km,
		vpa.Id,
		p.Co_Customer,
		custType.CustomerTypeName,
		cust.BusinessName,
		cust.BusinessTelNo,
		cust.BusinessAddress1,
		cust.BusinessAddress2,
		cust.BusinessAddress3,
		cust.BusinessAddress4,
		p.GrossPremiumBeforeTax,
		p.PolicySoldDate,
		vd.RegistrationDate,
		p.LocalCurrencyConversionRate,
		p.MWIsAvailable,
		p.HrsUsedAtPolicySale,
		mw.IsUnlimited
		) D
		Order By D.autoId
	