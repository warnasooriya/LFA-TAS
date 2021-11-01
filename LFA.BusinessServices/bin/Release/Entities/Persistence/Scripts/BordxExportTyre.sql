
SELECT Row_number()
  OVER(
    ORDER BY D.autoId) as SNo, * FROM (
SELECT DISTINCT
---------------------------------
b.EntryDateTime													AS		BDXExtractDate,
rec.uwyear                                                      AS		UnderWriterYear,
re.reinsurername                                                AS		ReinsurerName,
p.policyNo														AS		SystemGeneratedNumber,		-- New Feild
ceid.InvoiceCode												AS		InvoiceCode,
ceid.InvoiceNumber												AS      InvoiceNumber, -- New Feild
i.insurershortname                                              AS		CedentName,
'-'																AS		Bank,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		cust.firstname
	END
																AS		FirstName,
'-'                                                             AS		MiddleName,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		cust.lastname
	END
                                                                AS		LastName,
'-'																AS		CoBuyer,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(cust.address1 + ',' + cust.address2 + ',' + cust.address3,'-')
	END
                                                                AS		Address,
'-'                                                             AS		POBox,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(city.zipcode,'-')
	END
                                                                AS		Zip,
 CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
	CONVERT(VARCHAR, cust.mobileno + ' ')
	END
														        AS		MobileNumber,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(city.cityname,'-')
	END
                                                                AS		City,
con.CountryName													AS		Country,
p.EntryDateTime													AS		SystemTransactionDate,
p.UniqueRef														AS		SystemPolicyTransactionID,
-----------------------------
CONVERT(VARCHAR,b.year) + RIGHT('00'+convert(varchar(2),b.Month),2) AS		BordxNumber,
--CONVERT(VARCHAR,b.year) + CONVERT(VARCHAR,b.Month)  			AS		BordxNumber,-- New
CONVERT(VARCHAR,DATENAME(month, DATEADD(month, b.Month-1, CAST('2008-01-01' AS datetime)))) AS		BordxMonth,-- New
--CONVERT(VARCHAR,b.Month)										AS		BordxMonth,-- New
CONVERT(VARCHAR,b.year)											AS		BordxYear,--New
ISNULL(
(
select   CONCAT(FirstName ,' ', LastName)    from InternalUser iu
inner join policy pp on pp.SalesPersonId = iu.Id
where pp.Id=p.id
) , ' - ')															AS		Salesman,
' 0.00'																AS		SalesmanCommision,
ct.CommodityTypeDescription										AS		Commodity,
dt.Name															AS		DealType, -- New
c.dealname                                                      AS      DealName,
its.status                                                      AS		NewUsed,
d.dealername                                                    AS		DealerName,
dlc.cityname                                                    AS		DealerLocation,
CASE WHEN c.IsActive = '1'
THEN
	'Active'
ELSE
	'Inactive'
END																AS		Status,
'AD'															AS		CoverType,
wt.WarrantyTypeDescription										AS		WarrantyType,
ceid.AdditionalDetailsMileage									AS		KMSAtPolicySale,
i.InsurerFullName												AS		Insured,
ISNULL(
(
select VINNo from VehiclePolicy vvp inner join   VehicleDetails vvd
on vvp.VehicleId=vvd.Id where vvp.PolicyId=p.Id
) , ' - ') AS  VehicleIdentification,
'-'																AS		EngineNumber,
ic.PlateNumber													AS		PlateNumber,
cc.CommodityCategoryDescription									AS		Category,
'-'																AS		Manufacture,
'-'																AS		Model,
'-'																AS		Variant,
ISNULL(cyc.[count],'-')                                                     AS		CylinderCount,
CASE WHEN (vpa.Id IS NULL) THEN
    'No'
ELSE
	'Yes'
END																AS		FourByFour,
'-'																AS		Hybrid,
'-'																AS		ElectricVehicle,
ISNULL(CONVERT(VARCHAR(50), enc.enginecapacitynumber)
+ enc.mesuretype,'-')                                      AS		EngineCapacity,
ISNULL(Convert(varchar(25),vd.GrossWeight) + ' T' , '-')                      AS		Gvw,
ISNULL(vd.modelyear,'-')                                                   AS		ModelYear,
p.PolicySoldDate                                                AS		PolicySoldDate,
--CASE
--  WHEN ct.commoditycode = 'A' THEN vd.itempurchaseddate
--  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate
--  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate
--  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate
--END
'-'																AS		VehiclePurcheseDate,
--CASE
--  WHEN ct.commoditycode = 'A' THEN vd.RegistrationDate
--  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate --no data capturing
--  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate --no data capturing
--  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate  --no datacapturing
--END
'-'																AS		VehicleRegistrationDate,
CASE WHEN p.MWIsAvailable=1 THEN
	p.MWStartDate
ELSE
	CASE WHEN (mw.warrantymonths IS NULL OR mw.warrantymonths=0) THEN
		CAST(-53690 AS DATETIME)
	ELSE
		p.MWStartDate
	END
END                                                             AS		ManfWarrantyStartDate,
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
END															    AS		ManfWarrantyTerminationDate,
'-'																 AS		CancellationDate,
ISNULL(CONVERT(VARCHAR(10), mw.warrantymonths), '-')             AS		ManufLimitationInHours,
CASE WHEN mw.IsUnlimited=1
THEN
	'Unlimited'
ELSE
ISNULL(CONVERT(VARCHAR(10), mw.warrantykm), ' 00')
END                                                             AS		MileageLimitationInKMs,
' 00'																AS		ManfCoverHours,
dbo.checkAvailableByPolicyIdAndPosition('FL',p.id)				AS		FL,
dbo.checkAvailableByPolicyIdAndPosition('FR',p.id)				AS		FR,
dbo.checkAvailableByPolicyIdAndPosition('BL',p.id)				AS		RL,
dbo.checkAvailableByPolicyIdAndPosition('BR',p.id)				AS		RR,
dbo.checkAvailableByPolicyIdAndPosition('S',p.id)				AS		SP,
m.makename                                                      AS		TyreBrand,
ats.OriginalTireDepth												AS TreadDepth,
dbo.getTyreDetailsByPolicyIdAndPosition('ArticleNumber',p.id,'F') AS	F_ArticleNumber,
dbo.getNumberofTyresFront(p.Id)									 AS		NumberofTyresFront,
dbo.getTyreDetailsByPolicyIdAndPosition('Width',p.id,'F')		AS		F_Width,
dbo.getTyreDetailsByPolicyIdAndPosition('CrossSection',p.id,'F')AS		F_CrossSection,
dbo.getTyreDetailsByPolicyIdAndPosition('Diameter',p.id,'F')	AS		F_Diameter,
dbo.getTyreDetailsByPolicyIdAndPosition('LoadSpeed',p.id,'F')	AS		F_LoadSpeed,
dbo.getTyreDetailsByPolicyIdAndPosition('DotNumber',p.id,'F')	AS		F_DotNumber,

dbo.getTyreDetailsByPolicyIdAndPosition('ArticleNumber',p.id,'R') AS	R_ArticleNumber,
dbo.getNumberofTyresRear(p.Id)									AS		NumberofTyresRear,
dbo.getTyreDetailsByPolicyIdAndPosition('Width',p.id,'R')		AS		R_Width,
dbo.getTyreDetailsByPolicyIdAndPosition('CrossSection',p.id,'R')AS		R_CrossSection,
dbo.getTyreDetailsByPolicyIdAndPosition('Diameter',p.id,'R')	AS		R_Diameter,
dbo.getTyreDetailsByPolicyIdAndPosition('LoadSpeed',p.id,'R')	AS		R_LoadSpeed,
dbo.getTyreDetailsByPolicyIdAndPosition('DotNumber',p.id,'R')	AS		R_DotNumber,
CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)
ELSE
	p.PolicySoldDate
END																AS		DateOfInsuranceRiskStart,
CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0),
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)))
	ELSE
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0),p.PolicySoldDate))
END	                                                             AS		DateOfInsuranceRiskTermination,
 CONVERT(varchar(10),il.Months)                                  AS		ExtensionPeriodInMonths,
   il.Months															  AS ExtensionDurationInMonths,
 '-'															 AS		ExtentionDurationInHours,
 '-'															 AS		HrsCutOff,
 CASE WHEN il.Km=0 THEN
			'Unlimited'
		ELSE
			CONVERT(varchar(50), il.Km)
		END
																AS		MileageExtensionInKMS,
CASE WHEN (p.MWIsAvailable=1 AND DATEADD(MONTH,Isnull(mw.warrantymonths, 0),
			p.MWStartDate)<= p.PolicySoldDate
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
				CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
					--CONVERT(varchar(50),il.Km )
				END
			END
		ELSE -- no mw and used vehicle , so cutoff starts from usage
			CONVERT(varchar(50),Isnull(p.HrsUsedAtPolicySale, 0) + il.Km)
		END
	END
																AS		CutOffKm,
dbo.getSumInsured(p.id,ct.commoditycode)						AS		SumInsured,
CASE WHEN ats.TirePrice  <= c.LiabilityLimitation
	 THEN (ats.TirePrice * LocalCurrencyConversionRate)
	 WHEN ats.TirePrice IS NULL THEN
	 c.LiabilityLimitation *  LocalCurrencyConversionRate
	 ELSE  c.LiabilityLimitation *  LocalCurrencyConversionRate
	 END AS TotalLiability,
--(ats.TirePrice * LocalCurrencyConversionRate)					AS		TotalLiability,
'1 per Tyre'													AS		MaximumNoofClaims,
p.GrossPremiumBeforeTax                                         AS		GrossPremiumExcTax,
(p.TotalTax / LocalCurrencyConversionRate)					    AS		VAT,
'0.00'																AS		SalesTax,
p.premium														AS		GrossPremiumIncTax,


--marketing fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Marketing'),0.00)  AS	MarketingFee,
--end marketing fee
--Insurer fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Insurer Fee'),0.00)  AS	InsurerFee,
---end insarance fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Admin'),0.00)  AS	LicensingFee,

--start Internal GoodWill
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Internal GoodWill'),0.00)  AS	InternalGoodWill,
--end Internal GoodWill
'0.00'																AS			ManufactureCommission,
'0.00'																AS			ProducerCommision,
'0.00'																AS			SalesCommision,
'0.00'																AS			DocumentFee,
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
               AND cot.NAME LIKE ( 'Dealer Commission%' )), 0.00)AS			DealerCommission,
--end Dealer Commission
--start gross premium less sales commission
p.premium
--- Isnull((SELECT CASE
--								WHEN ccm.ispercentage = 1 THEN ccm.commission *
--										(CASE WHEN ccm.isonnrp = 1 THEN
--										p.NRP
--										ELSE
--										p.Premium-(p.TotalTax/LocalCurrencyConversionRate)
--										END)
--										/ 100
--					ELSE ccm.commission
--					END
--						FROM   nrpcommissioncontractmapping ccm
--							INNER JOIN nrpcommissiontypes cot
--									ON cot.id = ccm.nrpcommissionid
--						WHERE  ccm.contractid = c.id
--					AND cot.NAME LIKE ( 'Dealer Commission%' )), 0.00)
																	AS		GrossPremiumLessCommission,
--end GrossPremiumLessCommission
p.NRP - ((p.NRP*5)/100)												AS		NRPRIRetention,
(p.NRP*5)/100														AS		NRPInsurerRetention,
p.NRP																AS		NetAbsoluteRiskPremium,
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
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)
																	AS		Brokerage,
p.NRP 							AS		NRP,
 p.LocalCurrencyConversionRate										AS		ConversionRate,
p.NRP																AS		USD_NRP,
ph.transactiontypeid                                                AS		TransactionTypeId,
ptt.code                                                            AS
TransactionTypeCode,
p.contractid                                                          AS
ContractId,
p.grosspremiumbeforetax                                               AS
GrossPremiumBeforeTax,
--p.nrp                                                                 AS NRP,
p.id                                                                  AS
PolicyId,
p.EntryDateTime														  AS
SystemPolicyTransactionDate,
p.uniqueref                                                           as autoId,
reccon.id                                                             AS BaseCountryId,
reccon.countryname                                                    AS BaseCountry,
p.localcurrencyconversionrate                                         AS CurrencyConversionRate

--inctd.SerialNumber												as SerialNumber
--citd.UnUsedTireDepth
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
                        WHEN ct.commoditycode = 'O' THEN oid.VariantId
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
		LEFT JOIN TireSizeVariantMap tvm
			  ON vari.Id = tvm.VariantId
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
		LEFT JOIN InvoiceCodeDetails icd
				ON icd.PolicyId = p.id
		LEFT JOIN InvoiceCode ic
				ON ic.id = icd.InvoiceCodeId
	    LEFT JOIN InvoiceCodeTireDetails inctd
				ON inctd.InvoiceCodeDetailId = icd.id
		LEFT JOIN AvailableTireSizesPattern atsp
				ON atsp.Id = inctd.AvailableTireSizesPatternId
		LEFT JOIN AvailableTireSizes ats
				ON ats.Id = atsp.AvailableTireSizesId
		LEFT JOIN ClaimItemTireDetails citd
				ON citd.InvoiceCodeTireId = inctd.id
	   LEFT JOIN CustomerEnterdInvoiceDetails ceid
			ON ceid.InvoiceCodeId =ic.Id
	   LEFT JOIN AdditionalPolicyMakeData apmd
			ON apmd.Id = ceid.AdditionalDetailsMakeId
	   LEFT JOIN AdditionalPolicyModelData apmodeld
			ON apmodeld.Id = ceid.AdditionalDetailsModelId

WHERE  b.id =  '{bordexId}'  {dealerFilter}
--WHERE  b.id =  '{F0ED3739-C6EC-476F-9C90-CB3AE4240E32}'
GROUP  BY
			p.id,
			p.policyNo,
			c.id,
			p.EntryDateTime	,
			b.EntryDateTime,
			rec.uwyear,
			re.reinsurername,
			i.insurershortname,
			b.month,
			b.year,
			CONVERT(VARCHAR,b.year) + REPLICATE('0',2-LEN(b.month)) + CONVERT(VARCHAR,b.month)      ,
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
			mw.IsUnlimited,
			ic.Code,
			ceid.InvoiceNumber,
			ic.PlateNumber,
			apmd.MakeName,
			apmodeld.ModelName,
			ceid.AdditionalDetailsModelYear,
			ceid.AdditionalDetailsMileage,
			ic.TireQuantity,
			inctd.ArticleNumber		,
			atsp.Pattern,
			c.AnnualInterestRate,
			il.Months ,
			c.LiabilityLimitation,
			p.MonthlyEMI,
			b.Number,
			i.InsurerFullName,
			vd.GrossWeight,
			c.ClaimLimitation,
			vari.VariantName,
			ats.TirePrice,
			ceid.InvoiceCode,
			ats.OriginalTireDepth

			--inctd.SerialNumber
UNION
SELECT DISTINCT
---------------------------------
b.EntryDateTime														  AS		BDXExtractDate,
rec.uwyear                                                            AS		UnderWriterYear,
re.reinsurername                                                      AS		ReinsurerName,
p.policyNo															  AS		SystemGeneratedNumber,		-- New Feild
ceid.InvoiceCode													  AS		InvoiceCode,
ceid.InvoiceNumber													  AS      InvoiceNumber, -- New Feild
i.insurershortname                                                    AS		CedentName,
'-'																	  AS		Bank,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		cust.firstname
	END
																	  AS		FirstName,
'-'                                                                   AS		MiddleName,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		cust.lastname
	END
																	  AS LastName,
'-'																	  AS CoBuyer
,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(cust.address1 + ',' + cust.address2 + ',' + cust.address3,'-')
	END
                                                                    AS Address
,
'-'                                                               AS POBox,
CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(city.zipcode,'-')
	END
                                                                    AS Zip,
 CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		cust.mobileno + ' '
	END
														            AS MobileNumber,
	CASE WHEN custType.CustomerTypeName='Corporate'
	THEN
		'-'
	ELSE
		ISNULL(city.cityname,'-')
	END
                                                                    AS City,
	con.CountryName													AS Country,
-----------------------------------------------------
p.EntryDateTime AS SystemTransactionDate,
p.UniqueRef As
SystemPolicyTransactionID,
-----------------------------
CONVERT(VARCHAR,b.year) + RIGHT('00'+convert(varchar(2),b.Month),2) AS		BordxNumber,
--CONVERT(VARCHAR,b.year) + CONVERT(VARCHAR,b.Month)  													AS BordxNumber,-- New
CONVERT(VARCHAR,DATENAME(month, DATEADD(month, b.Month-1, CAST('2008-01-01' AS datetime)))) AS		BordxMonth,-- New
--CONVERT(VARCHAR,b.Month)											AS BordxMonth,-- New
CONVERT(VARCHAR,b.year)											AS		BordxYear,--New
ISNULL(
(
select   CONCAT(FirstName ,' ', LastName)    from InternalUser iu
inner join policy pp on pp.SalesPersonId = iu.Id
where pp.Id=p.id
) , ' - ')															AS		Salesman,
'0.00'																AS		SalesmanCommision,
ct.CommodityTypeDescription										AS		Commodity,
dt.Name															AS DealType, -- New
c.dealname                                                      AS      DealName,
its.status                                                      AS		NewUsed,
d.dealername                                                    AS		DealerName,
dlc.cityname                                                    AS		DealerLocation,
CASE WHEN c.IsActive = '1'
THEN
	'Active'
ELSE
	'Inactive'
END																	AS Status,
'AD'																AS		CoverType,
wt.WarrantyTypeDescription											AS WarrantyType,
ceid.AdditionalDetailsMileage									AS		KMSAtPolicySale,
i.InsurerFullName													AS Insured,
ISNULL(
(
select VINNo from VehiclePolicy vvp inner join   VehicleDetails vvd
on vvp.VehicleId=vvd.Id where vvp.PolicyId=p.Id
) , ' - ') AS  VehicleIdentification,
'-'																AS		EngineNumber,
ic.PlateNumber														AS PlateNumber,
cc.CommodityCategoryDescription										AS Category,
'-'                                                          AS Manufacture,
'-'                                                         AS Model,
'-'                                                       as Variant,
ISNULL(cyc.[count],'-')                                                           AS
CylinderCount,
CASE WHEN (vpa.Id IS NULL) THEN
    'No'
ELSE
	'Yes'
END AS FourByFour,
ISNULL(CONVERT(VARCHAR(50), enc.enginecapacitynumber)
+ enc.mesuretype,'-')                                              AS
EngineCapacity,
'-'																AS Hybrid,
'-'																AS ElectricVehicle,
ISNULL(Convert(varchar(25),vd.GrossWeight) + ' T','-')                        AS
Gvw,
ISNULL(vd.modelyear,'-')                                                   AS ModelYear,
p.PolicySoldDate                                                      As  PolicySoldDate,
--CASE
--  WHEN ct.commoditycode = 'A' THEN vd.itempurchaseddate
--  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate
--  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate
--  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate
--END
'-'	 AS VehiclePurcheseDate,
--CASE
--  WHEN ct.commoditycode = 'A' THEN vd.RegistrationDate
--  WHEN ct.commoditycode = 'E' THEN bwd.itempurchaseddate --no data capturing
--  WHEN ct.commoditycode = 'O' THEN oid.itempurchaseddate --no data capturing
--  WHEN ct.commoditycode = 'Y' THEN ygd.itempurchaseddate  --no datacapturing
--END
'-'	AS VehicleRegistrationDate,
CASE WHEN p.MWIsAvailable=1 THEN
	p.MWStartDate
ELSE
	CASE WHEN (mw.warrantymonths IS NULL OR mw.warrantymonths=0) THEN
		CAST(-53690 AS DATETIME)
	ELSE
		p.MWStartDate
	END
END                                                                   AS ManfWarrantyStartDate,
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
END																	  AS ManfWarrantyTerminationDate,
'-'																 AS		CancellationDate,
ISNULL(CONVERT(VARCHAR(10), mw.warrantymonths), '-')             AS		ManufLimitationInHours,
CASE WHEN mw.IsUnlimited=1
THEN
	'Unlimited'
ELSE
ISNULL(CONVERT(VARCHAR(10), mw.warrantykm), ' 00')
END                                                                    AS MileageLimitationInKMs,
' 00'																		AS ManfCoverHours,

dbo.checkAvailableByPolicyIdAndPosition('FL',p.id) as FL,
dbo.checkAvailableByPolicyIdAndPosition('FR',p.id) as FR,
dbo.checkAvailableByPolicyIdAndPosition('BL',p.id) as RL,
dbo.checkAvailableByPolicyIdAndPosition('BR',p.id) as RR,
dbo.checkAvailableByPolicyIdAndPosition('S',p.id) as SP,
m.makename                                                          AS TyreBrand,
ats.OriginalTireDepth												AS TreadDepth,
dbo.getTyreDetailsByPolicyIdAndPosition('ArticleNumber',p.id,'F') as F_ArticleNumber,
dbo.getNumberofTyresFront(p.Id)									AS NumberofTyresFront,
dbo.getTyreDetailsByPolicyIdAndPosition('Width',p.id,'F') as F_Width,
dbo.getTyreDetailsByPolicyIdAndPosition('CrossSection',p.id,'F') as F_CrossSection,
dbo.getTyreDetailsByPolicyIdAndPosition('Diameter',p.id,'F') as F_Diameter,
dbo.getTyreDetailsByPolicyIdAndPosition('LoadSpeed',p.id,'F') as F_LoadSpeed,
dbo.getTyreDetailsByPolicyIdAndPosition('DotNumber',p.id,'F') as F_DotNumber,

dbo.getTyreDetailsByPolicyIdAndPosition('ArticleNumber',p.id,'R') as R_ArticleNumber,
dbo.getNumberofTyresRear(p.Id)						AS		NumberofTyresRear,
dbo.getTyreDetailsByPolicyIdAndPosition('Width',p.id,'R') as R_Width,
dbo.getTyreDetailsByPolicyIdAndPosition('CrossSection',p.id,'R') as R_CrossSection,
dbo.getTyreDetailsByPolicyIdAndPosition('Diameter',p.id,'R') as R_Diameter,
dbo.getTyreDetailsByPolicyIdAndPosition('LoadSpeed',p.id,'R') as R_LoadSpeed,
dbo.getTyreDetailsByPolicyIdAndPosition('DotNumber',p.id,'R') as R_DotNumber,
CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)
ELSE
	p.PolicySoldDate
END																	  AS DateOfInsuranceRiskStart,
CASE WHEN p.MWIsAvailable=1 THEN
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0),
	DATEADD(MONTH, ISNULL(mw.warrantymonths, 0), p.MWStartDate)))
	ELSE
	DATEADD(DAY, -1,DATEADD(MONTH,ISNULL(il.Months, 0),p.PolicySoldDate))
END	                                                                  AS DateOfInsuranceRiskTermination,
 CONVERT(varchar(10),il.Months)                                       AS ExtensionPeriodInMonths,
   il.Months															  AS ExtensionDurationInMonths,
 '-'																		AS ExtentionDurationInHours,
 '-'																	AS HrsCutOff,
 CASE WHEN il.Km=0 THEN
			'Unlimited'
		ELSE
			CONVERT(varchar(50), il.Km)
		END
 AS
MileageExtensionInKMS,
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
				CONVERT(varchar(50),Isnull(mw.WarrantyKm, 0) + il.Km)
					--CONVERT(varchar(50),il.Km )
				END
			END
		ELSE -- no mw and used vehicle , so cutoff starts from usage
			CONVERT(varchar(50),Isnull(p.HrsUsedAtPolicySale, 0) + il.Km)
		END
	END
 AS
CutOffKm,
dbo.getSumInsured(p.id,ct.commoditycode)						AS		SumInsured,
CASE WHEN ats.TirePrice  <= c.LiabilityLimitation
	 THEN (ats.TirePrice * LocalCurrencyConversionRate)
	 WHEN ats.TirePrice IS NULL THEN
	 c.LiabilityLimitation *  LocalCurrencyConversionRate
	 ELSE  c.LiabilityLimitation *  LocalCurrencyConversionRate
	 END AS TotalLiability,
--(ats.TirePrice * LocalCurrencyConversionRate)													AS		TotalLiability,
'1 per Tyre'																AS		MaximumNoofClaims,
p.GrossPremiumBeforeTax                                         AS		GrossPremiumExcTax,
(p.TotalTax / LocalCurrencyConversionRate)					    AS		VAT,
'0.00'																AS		SalesTax,
p.premium														AS		GrossPremiumIncTax,
--marketing fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Marketing'),0.00)  AS	MarketingFee,
--end marketing fee
--Insurer fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Insurer Fee'),0.00)  AS	InsurerFee,
---end insarance fee
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Admin'),0.00)  AS	LicensingFee,

--start Internal GoodWill
Isnull(dbo.calculateCommissionAmounts(c.Id,p.Id,'Internal GoodWill'),0.00)  AS	InternalGoodWill,
--end Internal GoodWill
'0.00'																AS			ManufactureCommission,
'0.00'																AS			ProducerCommision,
'0.00'																AS			SalesCommision,
'0.00'																AS			DocumentFee,
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
--start gross premium less sales commission
p.premium
		AS GrossPremiumLessCommission,
--end GrossPremiumLessCommission
p.NRP - ((p.NRP*5)/100)												AS		NRPRIRetention,
(p.NRP*5)/100														AS		NRPInsurerRetention,
p.NRP																AS		NetAbsoluteRiskPremium,
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
               AND cot.NAME LIKE ( 'Reinsurer Brokerage%' )), 0.00)            AS Brokerage,
p.NRP  AS NRP,
 p.LocalCurrencyConversionRate AS ConversionRate,
 p.NRP AS USD_NRP,
ph.transactiontypeid                                                  AS
TransactionTypeId,
'EndorsementOld'                                                      AS
TransactionTypeCode,
ph.contractid                                                         AS
ContractId,
p.grosspremiumbeforetax                                               AS
GrossPremiumBeforeTax,
--p.nrp                                                                 AS NRP,
p.id                                                                  AS
PolicyId,
p.EntryDateTime														  AS
SystemPolicyTransactionDate,
p.uniqueref                                                           as autoId,
reccon.id                                                             AS BaseCountryId,
reccon.countryname                                                    AS BaseCountry,
p.localcurrencyconversionrate                                         AS CurrencyConversionRate

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
		LEFT JOIN otheritemdetails oid
              ON oid.id = oip.otheritemid
       LEFT JOIN otheritemdetailshistory oidh
              ON oidh.otheritemdetailsid = oip.otheritemid
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
					WHEN ct.commoditycode = 'O' THEN oid.VariantId
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
		LEFT JOIN TireSizeVariantMap tvm
			  ON vari.Id = tvm.VariantId
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
	   LEFT JOIN InvoiceCodeDetails icd
				ON icd.PolicyId = p.id
	  LEFT JOIN InvoiceCode ic
				ON ic.id = icd.InvoiceCodeId
	  LEFT JOIN InvoiceCodeTireDetails inctd
				ON inctd.InvoiceCodeDetailId = icd.id
	  LEFT JOIN AvailableTireSizesPattern atsp
				ON atsp.Id = inctd.AvailableTireSizesPatternId
	  LEFT JOIN AvailableTireSizes ats
				ON ats.Id = atsp.AvailableTireSizesId
	  LEFT JOIN ClaimItemTireDetails citd
				ON citd.InvoiceCodeTireId = inctd.id
	  LEFT JOIN CustomerEnterdInvoiceDetails ceid
			ON ceid.InvoiceCodeId =ic.Id
	   LEFT JOIN AdditionalPolicyMakeData apmd
			ON apmd.Id = ceid.AdditionalDetailsMakeId
	   LEFT JOIN AdditionalPolicyModelData apmodeld
			ON apmodeld.Id = ceid.AdditionalDetailsModelId

WHERE  b.id =  '{bordexId}'
--WHERE  b.id =  '{F0ED3739-C6EC-476F-9C90-CB3AE4240E32}'
       AND ptt.code = 'Endorsement'  {dealerFilter}
GROUP  BY
		p.id,
		p.policyNo,
		c.id,
		p.EntryDateTime	,
		b.EntryDateTime,
		rec.uwyear,
		re.reinsurername,
		i.insurershortname,
		b.month,
		b.year,
		CONVERT(VARCHAR,b.year) + REPLICATE('0',2-LEN(b.month)) + CONVERT(VARCHAR,b.month) ,
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
		mw.IsUnlimited,
		ic.Code,
		ceid.InvoiceNumber,
		ic.PlateNumber,
		apmd.MakeName,
		apmodeld.ModelName,
		ceid.AdditionalDetailsModelYear,
		ceid.AdditionalDetailsMileage,
		ic.TireQuantity,
		inctd.ArticleNumber	,
		atsp.Pattern,
		c.AnnualInterestRate,
		il.Months ,
		c.LiabilityLimitation,
		p.MonthlyEMI,
		b.Number,
		i.InsurerFullName,
		vd.GrossWeight,
		c.ClaimLimitation,
		p.TotalTax,
		vari.VariantName,
		ats.TirePrice,
		ceid.InvoiceCode,
		ats.OriginalTireDepth

		--inctd.SerialNumber
		) D
		Order By D.autoId

