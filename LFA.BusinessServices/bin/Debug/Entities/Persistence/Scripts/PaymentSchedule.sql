SELECT DISTINCT CONTY.[CountryName] AS 'CoverHolder' -- policy  country
		--,P.PolicyNo
		--,C.ClaimNumber
		, DATENAME(month, DATEADD(month, CB.[Bordxmonth]-1, CAST('2008-01-01' AS datetime))) AS 'MonthOfAccount'
		, RC.[UWYear] as 'YearOfAccount'
		,YEAR(CON.[StartDate]) AS 'ContractYear'
		--, CON.[StartDate] as 'ContractYear'
		, DE.[DealerName] as 'Payee'
		, CONVERT(VARCHAR(11),C.[ClaimDate],106) as 'DateOfClaim' 
		, CONVERT(VARCHAR(11),C.[FailureDate],106) as 'DateOfLoss'
		, DATEADD(month , ISNULL(MW.WarrantyMonths,0) ,VD.[RegistrationDate])  as 'ManfWarrantyExpiryDate'
		,CASE WHEN P.MWIsAvailable=1 THEN
				DATEADD(DAY, -1, 
				DATEADD(MONTH, ISNULL(MW.warrantymonths, 0), P.MWStartDate)) 
			  ELSE
		 CASE WHEN (MW.warrantymonths IS NULL OR MW.warrantymonths=0) THEN
				CAST(-53690 AS DATETIME) 
			 ELSE
				DATEADD(DAY, -1, 
		 DATEADD(MONTH, ISNULL(MW.warrantymonths, 0), P.MWStartDate)) 
		     END                                                   
		END			AS 'ManfWarrantyExpiryDate'
		--,'' as 'ManfWarrantyExpiryDate'
		, ISNULL(C.[ClaimMileageKm],0) as 'KmatDateofLoss'
		, 0 as 'ManufacturerWarrantyCutoffKms'
		, 0 as 'ExtWarrantyExpiryKm'
		, C.ClaimNumber as 'ClaimRef'
		, VD.[VINNo] as 'Certchas'
		, I.[InsurerFullName] as 'Insured'
		, C.[PaidAmount] as 'PaiClaimAmountInLocal'
		, C.[PaidAmount] * C.ConversionRate as 'PaiClaimAmount'
		, '' as 'DatePremiumWasPaid'
		, CB.[BordxNumber]
		, CASE LEN(CB.[Bordxmonth]) WHEN 1 THEN  '0'  + CAST( CB.[Bordxmonth] as VARCHAR(1)) ELSE CAST( CB.[Bordxmonth] as VARCHAR(2)) END AS 'BordxMonth' 
FROM ClaimBordx AS CB 
LEFT JOIN ClaimBordxDetail AS CBD ON CBD.ClaimBordxId = CB.Id
LEFT JOIN Claim AS C ON C.Id = CBD.ClaimId
LEFT JOIN Policy AS P ON P.Id = C.PolicyId
LEFT JOIN Contract AS CON ON CON.Id = P.ContractId
LEFT JOIN ReinsurerContract AS RC ON RC.Id = CON.ReinsurerContractId
LEFT JOIN Reinsurer AS R ON R.Id = RC.ReinsurerId
LEFT JOIN Insurer AS I ON I.Id = RC.InsurerId
LEFT JOIN Country AS CONTY ON CONTY.Id = C.PolicyCountryId
LEFT JOIN Dealer AS DE ON  DE.Id = P.DealerId
LEFT JOIN VehiclePolicy AS VP ON VP.PolicyId = P.Id
LEFT JOIN VehicleDetails AS VD ON VD.Id = VP.VehicleId
LEFT JOIN ManufacturerWarrantyDetails AS MWD ON MWD.ModelId = VD.ModelId
LEFT JOIN ManufacturerWarranty AS MW ON MW.Id = MWD.ManufacturerWarrantyId
WHERE CB.Id = '{bordexId}'