select Insurer.Id as Insurer,
Insurer.InsurerShortName,
Reinsurer.Id as Reinsurer,
Reinsurer.ReinsurerName,
ReinsurerContract.UWYear as UNRYear,
Dealer.Id as Dealer,
Dealer.DealerName,
WarrantyType.WarrantyTypeDescription as WarantyType,
AVG(dbo.GetRiskCompletedByPolicyId(Policy.PolicyStartDate,Policy.PolicyEndDate,'{EarnedDate}') * 100) AS 'EarnPercenSum',
ItemStatus.Status AS 'PolicyStatus'
from VehicleDetails
join VehiclePolicy on VehicleDetails.Id=VehiclePolicy.VehicleId
join Policy on VehiclePolicy.PolicyId=Policy.Id
join Contract on Policy.ContractId=Contract.Id
join Insurer on Contract.InsurerId=Insurer.Id
join ReinsurerContract on Contract.ReinsurerContractId=ReinsurerContract.Id
join Dealer on Policy.DealerId=Dealer.Id
INNER JOIN Country   ON Country.Id=Dealer.CountryId
join ContractExtensionPremium on Policy.ContractExtensionPremiumId=ContractExtensionPremium.Id
join WarrantyType on ContractExtensionPremium.WarrentyTypeId=WarrantyType.Id
join Reinsurer on ReinsurerContract.ReinsurerId=Reinsurer.Id
inner join ItemStatus ON VehicleDetails.ItemStatusId= ItemStatus.Id
--join claim on Policy.id=claim.policyid
join Bordx on Policy.BordxId=Bordx.Id

 where
 Policy.DealerId='{DealerId}' AND Country.Id='{CountryId}'  AND ReinsurerContract.UWYear ='{UWYear}' and
 VehicleDetails.MakeId=case
	when '{MakeId}'='00000000-0000-0000-0000-000000000000'
	then VehicleDetails.MakeId
	else '{MakeId}'
	end
	and VehicleDetails.ModelId=case
	when '{ModelId}'='00000000-0000-0000-0000-000000000000'
	then ModelId
	else '{ModelId}'
	end
	and VehicleDetails.cylinderCountId=case
	when '{CylinderCountId}'='00000000-0000-0000-0000-000000000000'
	then cylinderCountId
	else '{CylinderCountId}'
	end
	and VehicleDetails.EngineCapacityId=case
	when '{EngineCapacityId}'='00000000-0000-0000-0000-000000000000'
	then EngineCapacityId
	else '{EngineCapacityId}'
	end
	and Bordx.StartDate=case
	when '{BordxStartDate}'='1/15/9999 12:00:00 AM'
	then Bordx.StartDate
	else '{BordxStartDate}'
	end
	and Bordx.EndDate=case
	when '{BordxEndDate}'='1/15/9999 12:00:00 AM'
	then Bordx.EndDate
	else '{BordxEndDate}'
	end

	group by
	Insurer.Id, Insurer.InsurerShortName, Insurer.InsurerShortName,  Reinsurer.Id ,Reinsurer.ReinsurerName,
	ReinsurerContract.UWYear,Dealer.Id, Dealer.DealerName, ItemStatus.Status, WarrantyType.WarrantyTypeDescription
