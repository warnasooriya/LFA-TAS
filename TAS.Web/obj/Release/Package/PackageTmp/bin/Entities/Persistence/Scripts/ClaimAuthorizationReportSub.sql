SELECT P.PartName,P.PartNumber,CIT.ItemCode,CI.ParentId,
CI.AuthorizedAmount,CI.ConversionRate,CI.DiscountAmount,
CI.GoodWillAmount,CI.IsApproved,CI.Remark,CI.TotalGrossPrice,CI.TotalPrice,CI.AuthorizedAmount

 FROM 

[dbo].[ClaimItem] CI
LEFT JOIN [dbo].[Part] P ON CI.PartId=P.Id
INNER JOIN ClaimItemType CIT ON CIT.Id=CI.ClaimItemTypeId


where CI.ClaimId='{claimId}'
order by CI.SeqId