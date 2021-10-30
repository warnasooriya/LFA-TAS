
select distinct
---------------------------------

row_number()    Over(Order by p.PolicyEndDate) SNo,
rec.UWYear                                 as UnderWriterYear,
re.ReinsurerName                           as ReinsurerName,
i.InsurerShortName                         as CedentName,
con.CountryName                            as InsuredDetailsCountry,
c.dealname                                 as DealName,
dt.Name                                    as DealType,
(case when c.isActive  = 0 then 'Inactive' else 'Active' end) as Status,
its.Status                                 as CoverType,--itemstatus
wt.WarrantyTypeDescription                 as WarrantyType,
p.HrsUsedAtPolicySale                      as AmtUsedAtPolicySale,       
d.DealerName                               as DealerName,       
dlc.CityName                               as Location,

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
                                                                  AS LastName, 
''                                                     AS CoBuyer 
,
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.address1 + ',' + cust.address2 + ',' + cust.address3 
	END
                                                                    AS Address 
, 
' '                                                               AS POBox, 
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.cityname
	END
                                                                    AS City, 
CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		city.zipcode
	END
                                                                    AS Zip, 
 CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		''
	ELSE
		cust.mobileno + ' '
	END
														            AS MobileNumber, 
-----------------------------------------------------

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessName
	ELSE
		' '
	END													            AS BusinessName,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessTelNo
	ELSE
		' '
	END													            AS BusinessTel,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.BusinessAddress1 + ',' + cust.BusinessAddress2 
		+ ',' + cust.BusinessAddress3+ ',' + cust.BusinessAddress4
	ELSE
		' '
	END													            AS BusinessAddress,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		city.cityname
	ELSE
		' '
	END													            AS BusinessCity,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.firstname + ' ' +cust.lastname 
	ELSE
		' '
	END													            AS ContactPerson,

	CASE WHEN custType.CustomerTypeName='Corporate' 
	THEN
		cust.mobileno + ' '
	ELSE
		' '
	END													           AS ContactPersonTel,



 vd.VINNo                                  as VINNo,
 vd.PlateNo                                as RegNo,
 cc.CommodityCategoryDescription           as Category,
 m.MakeName                                as Manufacture,
 mo.ModelName                              as Model,
 cyc.[Count]                               as CylinderCount,
--case when vari.IsForuByFour  = 1 then 'Yes' else 'No' end as FourByFour,
 CONVERT(varchar(50), enc.EngineCapacityNumber)+' '+enc.MesureType as EngineCapacity,
 vd.ModelYear                              as ModelYear,
 p.policyno                                as PolicyNo,
 'N/A'                                     as BookletNo,
 UPPER(con.CountryCode+'-'+LEFT(tpab.BranchCode,3)+'-'+LEFT(prt.Code,3)+'-'+RIGHT('00000000'+CAST(p.UniqueRef AS VARCHAR),7))
		as SystemGeneratedNum,
case 
	when ct.CommodityCode='A' then
		vd.ItemPurchasedDate                       
	when ct.CommodityCode='E' then
		bwd.ItemPurchasedDate     
	when ct.CommodityCode='O' then
		oid.ItemPurchasedDate
	when ct.CommodityCode='Y' then
		ygd.ItemPurchasedDate	
end	as VehicleRegistrationDate,--check


case 
	when ct.CommodityCode='A' then
		case when isnull(mw.WarrantyMonths,0)=0 then
			cast('1753-1-1' as datetime)
		 else
		  DATEADD(day,-1,DATEADD(month,isnull(mw.WarrantyMonths,0),vd.ItemPurchasedDate))
		 end                      
	when ct.CommodityCode='E' then
		case when isnull(mw.WarrantyMonths,0)=0 then
			cast('1753-1-1' as datetime)
		 else
		DATEADD(day,-1,DATEADD(month,isnull(mw.WarrantyMonths,0),bwd.ItemPurchasedDate))   
		 end      
	when ct.CommodityCode='O' then
		case when isnull(mw.WarrantyMonths,0)=0 then
			cast('1753-1-1' as datetime)
		 else
		DATEADD(day,-1,DATEADD(month,isnull(mw.WarrantyMonths,0),oid.ItemPurchasedDate))
		 end  
	when ct.CommodityCode='Y' then
		case when isnull(mw.WarrantyMonths,0)=0 then
			cast('1753-1-1' as datetime)
		 else
		DATEADD(day,-1,DATEADD(month,isnull(mw.WarrantyMonths,0),ygd.ItemPurchasedDate))
		end  
end  as ManfWarrantyTerminationDate,

isnull( CONVERT(varchar(10),mw.WarrantyMonths),'-')    as ManfCoverInMonths,
isnull(CONVERT(varchar(10),mw.WarrantyKm),'-')       as MileageLimitationInKMs,

case 
	when ct.CommodityCode='A' then
		DATEADD(day,0,DATEADD(month,isnull(mw.WarrantyMonths,0),vd.ItemPurchasedDate))                      
	when ct.CommodityCode='E' then
		DATEADD(day,0,DATEADD(month,isnull(mw.WarrantyMonths,0),bwd.ItemPurchasedDate))       
	when ct.CommodityCode='O' then
		DATEADD(day,0,DATEADD(month,isnull(mw.WarrantyMonths,0),oid.ItemPurchasedDate)) 
	when ct.CommodityCode='Y' then
		DATEADD(day,0,DATEADD(month,isnull(mw.WarrantyMonths,0),ygd.ItemPurchasedDate)) 
end                                                   as DateOfInsuranceRiskStart,
   
CASE 
  WHEN ct.commoditycode = 'A' THEN Dateadd(month, 
		il.Months,
		Dateadd(day, -1,Dateadd(month,Isnull(mw.warrantymonths, 0), vd.itempurchaseddate)))
WHEN ct.commoditycode = 'E' THEN Dateadd(month, 
		il.Months,
		Dateadd(day, -1,Dateadd(month,Isnull(mw.warrantymonths, 0), vd.itempurchaseddate))) 
WHEN ct.commoditycode = 'O' THEN Dateadd(month, 
		il.Months,
		Dateadd(day, -1,Dateadd(month,Isnull(mw.warrantymonths, 0), vd.itempurchaseddate))) 
WHEN ct.commoditycode = 'Y' THEN Dateadd(month, 
		il.Months,
		Dateadd(day, -1,Dateadd(month,Isnull(mw.warrantymonths, 0), vd.itempurchaseddate))) 
END                       
 as DateOfInsuranceRiskTermination,

CONVERT(varchar(10),il.Months)
as ExtensionPeriodInMonths,

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
as ExtensionPeriodInKms,
p.Premium                                  as GrossPremium,
p.LocalCurrencyConversionRate               as CurrencyConversionRate


from policy p
left join WarrantyType wt on wt.id=p.coverTypeId
left join bordxDetails bd on bd.policyid=p.id
left join commodityType ct on ct.CommodityTypeId=p.commodityTypeId
left join product pr on pr.id = p.productid
left join ProductType prt on prt.id = pr.ProductTypeId
left join dealer d on d.id=p.dealerId
left join DealerLocation dl on dl.id = p.dealerlocationid
left join city dlc on dlc.id = dl.CityId
left join contract c on c.id = p.contractId
left join dealtype dt on dt.id = c.dealtype
left join insurer i on i.id = c.insurerId
left join ReinsurerContract rec on rec.id = c.ReinsurerContractId
left join reinsurer re on re.id = rec.ReinsurerId
left join ExtensionType e on e.id=p.ExtensionTypeId
left join currency curr on curr.id = p.PremiumCurrencyTypeId
left join currency currDealer on currDealer.id = p.DealerPaymentCurrencyTypeId
left join currency currCustomer on currCustomer.id = p.CustomerPaymentCurrencyTypeId
left join customer cust on cust.id = p.customerId
left join customertype custType on custType.id = cust.customertypeId
left join country con on con.Id= c.countryId
left join country reccon on reccon.Id= rec.CountryId
left join currency reccurr on reccurr.id = reccon.CurrencyId

left join city city on city.id = cust.cityid
left join vehiclePolicy vp on vp.PolicyId = p.id
left join VehicleDetails vd on vd.id = vp.VehicleId
left join BAndWPolicy bwp on bwp.PolicyId = p.id
left join BrownAndWhiteDetails bwd on bwd.id = bwp.BAndWId
left join OtherItemPolicy oip on oip.PolicyId = p.id
left join OtherItemDetails oid on oid.id = oip.OtherItemId
left join YellowGoodPolicy ygp on ygp.PolicyId = p.id
left join YellowGoodDetails ygd on ygd.id = ygp.YellowGoodId
left join ItemStatus its on  its.id = 
(
	case 
		when ct.CommodityCode = 'A' then
			vd.ItemStatusId
		when ct.CommodityCode='E' then
			bwd.ItemStatusId
		when ct.CommodityCode='O' then
			oid.ItemStatusId
		when ct.CommodityCode='Y' then
			ygd.ItemStatusId
	end
)

left join commodityCategory cc on cc.CommodityCategoryId= 
(
	case 
		when ct.CommodityCode = 'A' then
			vd.CategoryId
		when ct.CommodityCode='E' then
			bwd.CategoryId
		when ct.CommodityCode='O' then
			oid.CategoryId
		when ct.CommodityCode='Y' then
			ygd.CategoryId
	end
)

left join make m on m.id = 
(
	case 
		when ct.CommodityCode = 'A' then
			vd.MakeId
		when ct.CommodityCode='E' then
			bwd.MakeId
		when ct.CommodityCode='O' then
			oid.MakeId
		when ct.CommodityCode='Y' then
			ygd.MakeId
	end
)
left join model mo on mo.id = 
(
	case 
		when ct.CommodityCode = 'A' then
			vd.ModelId
		when ct.CommodityCode='E' then
			bwd.ModelId
		when ct.CommodityCode='O' then
			oid.ModelId
		when ct.CommodityCode='Y' then
			ygd.ModelId
	end
)
left join CylinderCount cyc on cyc.Id = vd.CylinderCountId
left join EngineCapacity enc on enc.Id = vd.EngineCapacityId
left join ManufacturerWarrantyDetails mwd on mwd.ModelId = mo.id and mwd.countryId=rec.CountryId
left join ManufacturerWarranty mw on mw.makeid = m.id and mw.Id=mwd.ManufacturerWarrantyId
left join ContractInsuaranceLimitation cil on c.Id = cil.ContractId
LEFT JOIN InsuaranceLimitation il ON il.id = cil.InsuaranceLimitationId
left join ContractExtensions ce on ce.ContractInsuanceLimitationId = c.id and ce.ProductId = pr.id
left join ContractExtensionVariant cev on cev.ContractExtensionId =ce.id 
left join Variant vari on vari.Id = cev.VariantId
left join ContractExtensionPremium cep on cep.ContractExtensionId = ce.Id
left join PremiumBasedOn pboNett on pboNett.id = cep.PremiumBasedOnNett
left join PremiumBasedOn pboGross on pboGross.id = cep.PremiumBasedOnGross
left join NRPCommissionContractMapping nrpccm on nrpccm.ContractId = c.id and nrpccm.NRPCommissionId in ( 
				(select Id from NRPCommissionTypes where Name in ('Admin Fee','Sales Commission')))
left join InternalUser SalesUser on SalesUser.Id = p.SalesPersonId
left join policyhistory ph on ph.PolicyId = p.Id
left join PolicyTransactionType ptt on ptt.Id = ph.TransactionTypeId
left join TpaBranch tpab on tpab.Id = p.TPABranchId


where  p.dealerid ='{DealerId}'
AND P.Year = {Year}
And P.Month = {Month}
--and year(p.policySoldDate)={Year}
-- and  month(p.policySoldDate)={Month}
and p.DealerPolicy=1  and p.IsApproved=1 
and p.IsPolicyRenewed = 0 and (p.BordxId <> '00000000-0000-0000-0000-000000000000' 
and p.BordxId is not null)

group by p.id,c.id,rec.UWYear,re.ReinsurerName,i.InsurerShortName,
c.dealname,dt.Name,c.isActive,wt.WarrantyTypeDescription,p.HrsUsedAtPolicySale,
d.DealerName,dl.Location,cust.Address1,cust.Address2,cust.Address3,city.CityName,city.ZipCode,
cust.MobileNo,its.Status, vd.VINNo, vd.PlateNo, cc.CommodityCategoryDescription, m.MakeName,
 mo.ModelName,cyc.[Count],enc.MesureType,enc.EngineCapacityNumber, vd.ModelYear ,
 vd.ItemPurchasedDate,mw.WarrantyMonths,mw.WarrantyKm,mo.NoOfDaysToRiskStart,p.PolicyEndDate,
 ce.AttributeSpecification,p.Premium ,
 --c.PremiumTotal,
rec.ContractNo,cc.CommodityCategoryCode,p.policyno,
cust.FirstName,cust.LastName,rec.ContractNo,c.StartDate,c.EndDate,ct.CommodityTypeDescription,
curr.Code,p.PolicyNo,city.CityName,con.CountryName,p.PolicyStartDate,p.PolicyEndDate,
cust.BusinessName,p.Comment,c.LiabilityLimitation,
ct.CommodityCode,bwd.ItemPurchasedDate,oid.ItemPurchasedDate,ygd.ItemPurchasedDate,
reccon.Id,reccon.CountryName,reccon.CurrencyId,reccurr.CurrencyName,p.CurrencyPeriodId,p.LocalCurrencyConversionRate,
ph.TransactionTypeId,ptt.Code,vd.DealerPrice,bwd.DealerPrice ,oid.DealerPrice,ygd.DealerPrice,pboNett.Code
,pboGross.Code,SalesUser.FirstName,SalesUser.LastName,dlc.CityName,tpab.BranchCode,con.CountryCode,p.UniqueRef,prt.Code,p.contractId,
p.GrossPremiumBeforeTax,p.NRP,il.Months,il.TopOfMW,il.Km,custType.CustomerTypeName,cust.BusinessTelNo,
cust.BusinessAddress1,cust.BusinessAddress2,cust.BusinessAddress3,cust.BusinessAddress4
