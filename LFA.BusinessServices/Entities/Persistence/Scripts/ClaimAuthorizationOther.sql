
SELECT DISTINCT
PD.DealerName,
C.ClaimNumber,
DC.CountryName +'-'+DCI.CityName AS CountryCity,
CU.FirstName +' '+CU.LastName AS CustomerName,
P.BookletNumber,
P.PolicyNo,
IT.Status,
CEID.InvoiceNumber as PlateNo,
IC.Code as VINNo,
M.MakeName,
MO.ModelName,
C.FailureDate,
CONCAT(C.ClaimMileageKm,' KM') AS ClaimMileageKm,
C.CustomerComplaint,
C.DealerComment,
C.EngineerComment,
UCS.FirstName +' '+UCS.LastName AS ClaimSubmittedUser,
UCA.FirstName +' '+UCA.LastName AS ClaimApprovedUser,
C.ApprovedDate,
C.ClaimDate,
C.ConversionRate,
C.TotalClaimAmount,
C.AuthorizedAmount AS TotalPayable,
REPLACE(REPLACE(PA.PartName,'BL -','RL -'),'BR -','RR -') AS PartName,
CONCAT('C',PA.PartNumber,'0000') as PartNumber,
CI.TotalPrice,
(CI.AuthorizedAmount * C.ConversionRate) AS AuthorizedAmount,
--CONCAT((CI.AuthorizedAmount * C.ConversionRate),' AED') AS AuthorizedAmount,
CI.IsApproved,
CURR.Code

FROM [dbo].[Claim] C
INNER JOIN [dbo].[ClaimItem] CI on CI.ClaimId =  C.Id
INNER JOIN [dbo].[Policy] P on P.Id = C.PolicyId
INNER JOIN [dbo].[Dealer] PD on PD.Id = P.DealerId
INNER JOIN [dbo].[Country] DC on DC.Id = PD.CountryId
INNER JOIN [dbo].[Currency] CURR on CURR.Id = DC.CurrencyId
INNER JOIN [dbo].[DealerLocation] PDL on PDL.Id = P.DealerLocationId
INNER JOIN [dbo].[City] DCI on DCI.Id = PDL.CityId
INNER JOIN [dbo].[Customer] CU on CU.Id = P.CustomerId
INNER JOIN [dbo].[Make] M on M.Id=C.MakeId
INNER JOIN [dbo].[Model] MO on MO.Id = C.ModelId
INNER JOIN [dbo].[OtherItemPolicy] VP ON VP.PolicyId=P.Id
INNER JOIN [dbo].[OtherItemDetails] V ON V.Id = VP.OtherItemId
INNER JOIN [dbo].[ItemStatus] IT ON IT.Id = V.ItemStatusId
INNER JOIN [dbo].[InternalUser] UCS ON UCS.Id = C.ClaimSubmittedBy
INNER JOIN [dbo].[InternalUser] UCA ON UCA.Id = C.ApprovedBy
INNER JOIN [dbo].[Currency] CUR ON CUR.Id = C.ClaimCurrencyId
INNER JOIN [dbo].[InvoiceCodeDetails] ICD ON ICD.PolicyId = P.Id
INNER JOIN [dbo].[InvoiceCode] IC ON IC.Id = ICD.InvoiceCodeId
INNER JOIN [dbo].[CustomerEnterdInvoiceDetails] CEID ON  CEID.InvoiceCodeId = IC.Id
LEFT JOIN [dbo].[Part] PA ON CI.PartId= PA.Id
INNER JOIN ClaimItemType CIT ON CIT.Id = CI.ClaimItemTypeId

--WHERE C.Id = '{claimId}'