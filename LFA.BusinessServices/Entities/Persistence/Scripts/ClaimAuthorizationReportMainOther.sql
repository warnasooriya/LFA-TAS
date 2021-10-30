
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
C.ClaimMileageKm,
C.CustomerComplaint,
C.DealerComment,
C.EngineerComment,
UCS.FirstName +' '+UCS.LastName AS ClaimSubmittedUser,
UCA.FirstName +' '+UCA.LastName AS ClaimApprovedUser,
C.ApprovedDate,
C.ClaimDate,
C.ConversionRate,
C.TotalClaimAmount,
C.AuthorizedAmount AS TotalPayable

--P.DealerLocationId

FROM [dbo].[Claim] C
INNER JOIN [dbo].[Policy] P on P.Id = C.PolicyId
INNER JOIN [dbo].[Dealer] PD on PD.Id = P.DealerId
INNER JOIN [dbo].[Country] DC on DC.Id = PD.CountryId
INNER JOIN [dbo].[DealerLocation] PDL on PDL.Id = P.DealerLocationId
INNER JOIN [dbo].[City] DCI on DCI.Id = PDL.CityId
INNER JOIN [dbo].[Customer] CU on CU.Id = P.CustomerId
INNER JOIN [dbo].[Make] M on M.Id=C.MakeId
INNER JOIN [dbo].[Model] MO on MO.Id = C.ModelId
INNER JOIN [dbo].[OtherItemPolicy] VP ON VP.PolicyId=P.Id
INNER JOIN [dbo].[OtherItemDetails] V ON V.Id = VP.OtherItemId
INNER JOIN [dbo].[ItemStatus] IT ON IT.Id = V.ItemStatusId
INNER JOIN [dbo].[InternalUser] UCS ON UCS.Id = C.ClaimSubmittedBy
INNER JOIN [dbo].[SystemUser] UCA ON UCA.LoginMapId = C.ApprovedBy
INNER JOIN [dbo].[Currency] CUR ON CUR.Id = C.ClaimCurrencyId
INNER JOIN [dbo].[InvoiceCodeDetails] ICD ON ICD.PolicyId = P.Id
INNER JOIN [dbo].[InvoiceCode] IC ON IC.Id = ICD.InvoiceCodeId
INNER JOIN [dbo].[CustomerEnterdInvoiceDetails] CEID ON  CEID.InvoiceCodeId = IC.Id

WHERE C.Id = '{claimId}'