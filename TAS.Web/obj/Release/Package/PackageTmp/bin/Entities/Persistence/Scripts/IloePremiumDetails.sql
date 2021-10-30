

select CONVERT(FLOAT,c.AnnualInterestRate) as AnnualInterestRate,il.Months as LoanPeriod,CONVERT(FLOAT,cep.NRP) as NRPRate, CONVERT(FLOAT,cep.Gross) as GrossRate,
CONVERT(FLOAT,c.ClaimLimitation) as MinimumPayment,CONVERT(FLOAT,c.LiabilityLimitation) as MaximumPayment,  CONVERT(INT,c.LiabilityLimitation/c.ClaimLimitation) as IncubationPeriod
from Contract c
inner join ContractInsuaranceLimitation ci on ci.ContractId = c.Id
inner join InsuaranceLimitation il on il.Id = ci.InsuaranceLimitationId
inner join ContractExtensions ce on ce.ContractInsuanceLimitationId = ci.id
inner join ContractExtensionPremium cep on cep.ContractExtensionId = ce.id

where c.id = '{contractId}' and ci.id = '{contractInsuranceLimitationId}' and cep.id = '{conractExtensionPremiumId}'