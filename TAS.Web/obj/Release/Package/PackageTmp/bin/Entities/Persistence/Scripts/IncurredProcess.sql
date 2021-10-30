select Insurer.Id as Insurer,
Insurer.InsurerShortName,
Reinsurer.Id as Reinsurer,
Reinsurer.ReinsurerName,
ReinsurerContract.UWYear as UNRYear,
Dealer.Id as Dealer,
Dealer.DealerName,
Policy.Id as PolicyId,
WarrantyType.WarrantyTypeDescription as WarantyType,
Policy.PolicyStartDate,
Policy.PolicyEndDate,
Policy.Premium,
Policy.MWStartDate
from VehicleDetails
join VehiclePolicy on VehicleDetails.Id=VehiclePolicy.VehicleId
join Policy on VehiclePolicy.PolicyId=Policy.Id
join Contract on Policy.ContractId=Contract.Id
join Insurer on Contract.InsurerId=Insurer.Id
join ReinsurerContract on Contract.ReinsurerContractId=ReinsurerContract.Id
join Dealer on Policy.DealerId=Dealer.Id
join ContractExtensionPremium on Policy.ContractExtensionPremiumId=ContractExtensionPremium.Id
join WarrantyType on ContractExtensionPremium.WarrentyTypeId=WarrantyType.Id
join Reinsurer on ReinsurerContract.ReinsurerId=Reinsurer.Id
--join claim on Policy.id=claim.policyid
join Bordx on Policy.BordxId=Bordx.Id
 where VehicleDetails.MakeId=case
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