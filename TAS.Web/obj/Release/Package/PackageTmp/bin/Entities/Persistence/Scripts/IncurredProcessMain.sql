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
join Bordx on Policy.BordxId=Bordx.Id
 where Contract.CountryId=case
	when '{CountryId}'='00000000-0000-0000-0000-000000000000'
	then Contract.CountryId
	else '{CountryId}'
	end
	and Dealer.Id=case
	when '{dealerId}'='00000000-0000-0000-0000-000000000000'
	then Dealer.Id
	else '{dealerId}'
	end
	and ReinsurerContract.UWYear=case
	when '{UWYear}'=''
	then ReinsurerContract.UWYear
	else '{UWYear}'
	end
	