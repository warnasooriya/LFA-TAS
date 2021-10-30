select

 p.policyNo as policyNo,
 w.WarrantyTypeDescription as warrentyType,
 cu.firstname+' '+cu.lastname  as  customerName,
 cu.mobileno as mobileNumber,
 i.insurershortname as insurer,
 r.reinsurername as reInsurer,
 m.makename as make,
 md.modelname as model,
 vd.modelyear as modelYear,
 cc.[count]  as cyllinderCount,
 cast(ec.enginecapacitynumber as varchar(50))+' '+ec.mesuretype as engineCapacity,
cast(cast(vd.dealerprice * p.LocalCurrencyConversionRate as decimal(18,2)) as varchar(50)) +' '+ currencyvehicle.Code as salePrice,
its.Status as status,
rc.uwyear as uwYear,
vd.vinno as vin,
 vd.ItemPurchasedDate  as manfWarrentyStartDate,
 CASE 
      WHEN Isnull(mw.warrantymonths, 0) = 0 THEN  Cast('1753-1-1' AS DATETIME) 
      ELSE 
    Dateadd(day, -1, 
         
		   Dateadd(month, Isnull(mw.warrantymonths, 0), vd.ItemPurchasedDate))
    END  
as manfWarrentyEndDate,

  Dateadd(day,1,P.PolicyStartDate )
as extensionStartDate,
 Dateadd(day,1,P.PolicyEndDate)

as extensionEndDate,
  
	CASE WHEN il.Km=0 THEN
		'Unlimited'
	ELSE
		CONVERT(varchar(50),il.Km )
	END
		

as extensionMilage,
CONVERT(varchar(50),mw.WarrantyMonths ) as manfWarrentyMonths,
cast(mw.warrantykm as varchar(50)) as manfWarrentyMilage,

 CONVERT(varchar(10),il.Months)                      AS 
extensionPeriod,


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
		
	END  as cutoff


 from policy p
left join dealer d on d.id=p.dealerid
left join customer cu on cu.id=p.customerid
left join contract c on c.id=p.contractid
left join insurer i on i.id = c.insurerid
left join reinsurercontract rc on rc.id=c.ReinsurerContractID
left join reinsurer r on r.id = rc.reinsurerid
left join vehiclepolicy vp on vp.policyid=p.id
left join bandwpolicy bwp on bwp.policyid=p.id
left join yellowgoodpolicy ygp on ygp.policyid=p.id
left join otheritempolicy oip on oip.policyid=p.id
left join vehicledetails vd on vd.id = vp.vehicleid
left join make m on m.id=vd.makeid
left join model md on md.id = vd.modelid
left join cylindercount cc on cc.id = vd.cylindercountid
left join enginecapacity ec on ec.id = vd.enginecapacityid
left join itemstatus its on its.id = vd.itemstatusid
left join extensiontype ext on ext.id=p.extensiontypeid
left join ManufacturerWarrantyDetails mwd on mwd.ModelId = m.id and mwd.countryId=rc.CountryId
left join manufacturerwarranty mw on mw.makeid=m.id 
LEFT JOIN ContractExtensionPremium cep ON cep.id = p.ContractExtensionPremiumId 
left join warrantytype w on w.id=cep.WarrentyTypeId
left join ContractInsuaranceLimitation cil on c.Id = cil.ContractId
LEFT JOIN InsuaranceLimitation il ON il.id = cil.InsuaranceLimitationId
left join currency currencyvehicle on currencyvehicle.id = vd.dealercurrencyid
where p.id= '{PolicyId}'

