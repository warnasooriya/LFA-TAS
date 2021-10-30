app.controller('PolicyInquiryCtrl', function ($scope, $rootScope, $http, ngDialog, SweetAlert,
    $localStorage, $cookieStore, $filter, FileUploader, $translate) {
    $scope.ModalName = "Policy Inquiry";
    $scope.Automobile = false;
    $scope.Electronics = false;
    //$scope.StatusIsApproved = false;
    //$scope.StatusIsPolicyCanceled = false;
    //$scope.StatusIsPolicyRenewed = false;
    $scope.isCustomerTypeCorporate = false;
    $scope.isProductILOE = false;
    $scope.countries = [];
    $scope.Allcountries = [];
    $scope.dealers = [];
    $scope.dealersByCountry = [];
    $scope.CommodityTypes = [];

    $scope.attachmentisshow = false;


    //initialize uploaders
    $scope.customerDocUploader = new FileUploader({
        url: '/TAS.Web/api/Upload/UploadAttachment',
        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Customer' },
    });
    $scope.customerDocUploader.onProgressAll = function () {
        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
    }
    $scope.customerDocUploader.onSuccessItem = function (item, response, status, headers) {
        if (response != 'Failed') {
            $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
            $scope.currentUploadingItem++
            $scope.policySaveStatusMsg = $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.customerDocUploader.queue.length;
        } else {
            customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.errorUploadingAttachments'));
            $scope.customerDocUploader.cancelAll();
        }
    }
    $scope.customerDocUploader.onCompleteAll = function () {
        // swal.close();
        $scope.customerDocUploader.queue = [];
        $scope.uploadAttachments();
    }
    $scope.customerDocUploader.filters.push({
        name: 'customFilter',
        fn: function (item, options) {
            if ($scope.customerAttachmentType != "" && typeof $scope.customerAttachmentType != 'undefined') {
                if (item.size > 3000000) {
                    customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.maxSize'));
                    return false;
                } else {
                    return true;
                }
            } else {
                customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.attachmentType'));
                return false;
            }


        }
    });




    $scope.itemDocUploader = new FileUploader({
        url: '/TAS.Web/api/Upload/UploadAttachment',
        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Item' },
    });
    $scope.itemDocUploader.onProgressAll = function () {
        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
    }
    $scope.itemDocUploader.onSuccessItem = function (item, response, status, headers) {
        if (response != 'Failed') {
            $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
            $scope.currentUploadingItem++
            $scope.policySaveStatusMsg = $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.itemDocUploader.queue.length;
        } else {
            customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.errorUploadingAttachments'));
            $scope.itemDocUploader.cancelAll();
        }
    }
    $scope.itemDocUploader.onCompleteAll = function () {
        // swal.close();
        $scope.itemDocUploader.queue = [];
        $scope.uploadAttachments();
    }
    $scope.itemDocUploader.filters.push({
        name: 'customFilter',
        fn: function (item, options) {
            if ($scope.itemAttachmentType != "" && typeof $scope.itemAttachmentType != 'undefined') {

                if (item.size > 3000000) {
                    customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.maxSize'));
                    return false;
                } else {
                    return true;
                }
            } else {
                customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.attachmentType'));
                return false;
            }
        }
    });



    $scope.policyDocUploader = new FileUploader({
        url: '/TAS.Web/api/Upload/UploadAttachment',
        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Policy' },
    });
    $scope.policyDocUploader.onProgressAll = function () {
        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
    }
    $scope.policyDocUploader.onSuccessItem = function (item, response, status, headers) {
        if (response != 'Failed') {
            $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
            $scope.currentUploadingItem++
            $scope.policySaveStatusMsg = $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.policyDocUploader.queue.length;
        } else {
            customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.errorUploadingAttachments'));
            $scope.policyDocUploader.cancelAll();
        }
    }
    $scope.policyDocUploader.onCompleteAll = function () {
        // swal.close();
        $scope.policyDocUploader.queue = [];
        $scope.uploadAttachments();
    }
    $scope.policyDocUploader.filters.push({
        name: 'customFilter',
        fn: function (item, options) {
            if ($scope.policyAttachmentType != "" && typeof $scope.policyAttachmentType != 'undefined') {

                if (item.size > 3000000) {
                    customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.maxSize'));
                    return false;
                } else {
                    return true;
                }
            } else {
                customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.attachmentType'));
                return false;
            }
        }
    });




    $scope.paymentDocUploader = new FileUploader({
        url: '/TAS.Web/api/Upload/UploadAttachment',
        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Payment' },
    });
    $scope.paymentDocUploader.onProgressAll = function () {
        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
    }
    $scope.paymentDocUploader.onSuccessItem = function (item, response, status, headers) {
        if (response != 'Failed') {
            $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
            $scope.currentUploadingItem++
            $scope.policySaveStatusMsg = $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.paymentDocUploader.queue.length;
        } else {
            customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.errorUploadingAttachments'));
            $scope.paymentDocUploader.cancelAll();
        }
    }
    $scope.paymentDocUploader.onCompleteAll = function () {
        //  swal.close();
        $scope.paymentDocUploader.queue = [];
        $scope.uploadAttachments();
    }
    $scope.paymentDocUploader.filters.push({
        name: 'customFilter',
        fn: function (item, options) {
            if ($scope.paymentAttachmentType != "" && typeof $scope.paymentAttachmentType != 'undefined') {
                if (item.size > 3000000) {
                    customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.maxSize'));
                    return false;
                } else {
                    return true;
                }
            } else {
                customErrorMessage($filter('translate')('pages.policyInquiry.errorMessage.attachmentType'));
                return false;
            }
        }

    });

    $scope.Policy = {
        AddnSerialNo: "",
        Address1: "",
        Address2: "",
        Address3: "",
        Address4: "",
        Aspiration: "",
        BAndW: "",
        BodyType: "",
        BusinessAddress1: "",
        BusinessAddress2: "",
        BusinessAddress3: "",
        BusinessAddress4: "",
        BusinessName: "",
        BusinessTelNo: "",
        Category: "",
        City: "",
        Comment: "",
        CommodityType: "",
        Contract: "",
        Country: "",
        CoverType: "",
        Customer: "",
        CustomerPayment: "",
        CustomerPaymentCurrencyType: "",
        CustomerType: "",
        CylinderCount: "",
        DateOfBirth: "",
        Dealer: "",
        DealerLocation: "",
        DealerPayment: "",
        DealerPaymentCurrencyType: "",
        DealerPrice: "",
        DLIssueDate: "",
        DriveType: "",
        Discount: "",
        Email: "",
        EngineCapacity: "",
        ExtensionType: "",
        FirstName: "",
        FuelType: "",
        Gender: "",
        HrsUsedAtPolicySale: "",
        IDNo: "",
        IdType: "",
        InvoiceNo: "",
        Active: "",
        Approved: "",
        PartialPayment: "",
        PreWarrantyCheck: "",
        SpecialDeal: "",
        ItemPrice: "",
        ItemPurchasedDate: "",
        ItemStatus: "",
        LastName: "",
        Make: "",
        MobileNo: "",
        ModelCode: "",
        Model: "",
        ModelYear: "",
        Nationality: "",
        OtherTelNo: "",
        Password: "",
        PaymentMode: "",
        PlateNo: "",
        Policy: "",
        PolicyNo: "",
        PolicyStartDate: "",
        PolicySoldDate: "",
        Premium: "",
        PremiumCurrencyType: "",
        Product: "",
        ProfilePicture: "",
        RefNo: "",
        SalesPerson: "",
        SerialNo: "",
        Transmission: "",
        UsageType: "",
        UserName: "",
        VariantName: "",
        Vehicle: "",
        VehiclePrice: "",
        VINNo: "",
        IsEndorsementApproved: "",
        PolicyEndDate: "",
        DealerPaymentCurrencyTypeId: "",
        CustomerPaymentCurrencyTypeId: "",
        MWStartDate: "",
        MWEndDate: "",
        GrossWeight: "",
        IsApproved: "",
        IsPolicyCanceled: ""
    }

    $scope.Endorsement = {
        AddnSerialNo: "",
        Address1: "",
        Address2: "",
        Address3: "",
        Address4: "",
        Aspiration: "",
        BAndW: "",
        BodyType: "",
        BusinessAddress1: "",
        BusinessAddress2: "",
        BusinessAddress3: "",
        BusinessAddress4: "",
        BusinessName: "",
        BusinessTelNo: "",
        Category: "",
        City: "",
        Comment: "",
        CommodityType: "",
        Contract: "",
        Country: "",
        CoverType: "",
        Customer: "",
        CustomerPayment: "",
        CustomerPaymentCurrencyType: "",
        CustomerType: "",
        CylinderCount: "",
        DateOfBirth: "",
        Dealer: "",
        DealerLocation: "",
        DealerPayment: "",
        DealerPaymentCurrencyType: "",
        DealerPrice: "",
        DLIssueDate: "",
        DriveType: "",
        Discount: "",
        Email: "",
        EngineCapacity: "",
        ExtensionType: "",
        FirstName: "",
        FuelType: "",
        Gender: "",
        HrsUsedAtPolicySale: "",
        IDNo: "",
        IdType: "",
        InvoiceNo: "",
        Active: "",
        Approved: "",
        PartialPayment: "",
        PreWarrantyCheck: "",
        SpecialDeal: "",
        ItemPrice: "",
        ItemPurchasedDate: "",
        ItemStatus: "",
        LastName: "",
        Make: "",
        MobileNo: "",
        ModelCode: "",
        Model: "",
        ModelYear: "",
        Nationality: "",
        OtherTelNo: "",
        Password: "",
        PaymentMode: "",
        PlateNo: "",

        Policy: "",
        PolicyNo: "",
        PolicyStartDate: "",
        PolicySoldDate: "",
        Premium: "",
        PremiumCurrencyType: "",
        Product: "",
        ProfilePicture: "",
        RefNo: "",
        SalesPerson: "",
        SerialNo: "",
        Transmission: "",
        UsageType: "",
        UserName: "",
        VariantName: "",
        Vehicle: "",
        VehiclePrice: "",
        VINNo: "",
        EndorsementApproved: "",
        PolicyEndDate: "",
        DealerPaymentCurrencyTypeId: "",
        CustomerPaymentCurrencyTypeId: "",
        IsApproved: "",
        IsPolicyEndrosed: "",
        MWStartDate: "",
        MWEndDate: "",
        GrossWeight: ""
    }


    $scope.Cancelation = {
        AddnSerialNo: "",
        Address1: "",
        Address2: "",
        Address3: "",
        Address4: "",
        Aspiration: "",
        BAndW: "",
        BodyType: "",
        BusinessAddress1: "",
        BusinessAddress2: "",
        BusinessAddress3: "",
        BusinessAddress4: "",
        BusinessName: "",
        BusinessTelNo: "",
        Category: "",
        City: "",
        Comment: "",
        CommodityType: "",
        Contract: "",
        Country: "",
        CoverType: "",
        Customer: "",
        CustomerPayment: "",
        CancelationComment: "",
        CustomerPaymentCurrencyType: "",
        CustomerType: "",
        CylinderCount: "",
        DateOfBirth: "",
        Dealer: "",
        DealerLocation: "",
        DealerPayment: "",
        DealerPaymentCurrencyType: "",
        DealerPrice: "",
        DLIssueDate: "",
        DriveType: "",
        Discount: "",
        Email: "",
        EngineCapacity: "",
        ExtensionType: "",
        FirstName: "",
        FuelType: "",
        Gender: "",
        HrsUsedAtPolicySale: "",
        IDNo: "",
        IdType: "",
        InvoiceNo: "",
        Active: "",
        Approved: "",
        PartialPayment: "",
        PreWarrantyCheck: "",
        SpecialDeal: "",
        ItemPrice: "",
        ItemPurchasedDate: "",
        ItemStatus: "",
        LastName: "",
        Make: "",
        MobileNo: "",
        ModelCode: "",
        Model: "",
        ModifiedDate: "",
        ModifiedUser: "",
        ModelYear: "",
        Nationality: "",
        OtherTelNo: "",
        Password: "",
        PaymentMode: "",
        PlateNo: "",

        Policy: "",
        PolicyNo: "",
        PolicyStartDate: "",
        PolicySoldDate: "",
        PolicyEndDate: "",
        Premium: "",
        PremiumCurrencyType: "",
        Product: "",
        ProfilePicture: "",
        RefNo: "",
        SalesPerson: "",
        SerialNo: "",
        Transmission: "",
        UsageType: "",
        UserName: "",
        VariantName: "",
        Vehicle: "",
        VehiclePrice: "",
        VINNo: "",
        EndorsementApproved: "",
        DealerPaymentCurrencyTypeId: "",
        CustomerPaymentCurrencyTypeId: "",
        IsPolicyCanceled: "",
        MWStartDate: "",
        MWEndDate: "",
        GrossWeight: ""
    }



    $scope.Renewal = {
        AddnSerialNo: "",
        Address1: "",
        Address2: "",
        Address3: "",
        Address4: "",
        Aspiration: "",
        BAndW: "",
        BodyType: "",
        BusinessAddress1: "",
        BusinessAddress2: "",
        BusinessAddress3: "",
        BusinessAddress4: "",
        BusinessName: "",
        BusinessTelNo: "",
        Category: "",
        City: "",
        Comment: "",
        CommodityType: "",
        Contract: "",
        Country: "",
        CoverType: "",
        Customer: "",
        CustomerPayment: "",
        CustomerPaymentCurrencyType: "",
        CustomerType: "",
        CylinderCount: "",
        DateOfBirth: "",
        Dealer: "",
        DealerLocation: "",
        DealerPayment: "",
        DealerPaymentCurrencyType: "",
        DealerPrice: "",
        DLIssueDate: "",
        DriveType: "",
        Discount: "",
        Email: "",
        EngineCapacity: "",
        ExtensionType: "",
        FirstName: "",
        FuelType: "",
        Gender: "",
        HrsUsedAtPolicySale: "",
        IDNo: "",
        IdType: "",
        InvoiceNo: "",
        Active: "",
        Approved: "",
        PartialPayment: "",
        PreWarrantyCheck: "",
        SpecialDeal: "",
        ItemPrice: "",
        ItemPurchasedDate: "",
        ItemStatus: "",
        LastName: "",
        Make: "",
        MobileNo: "",
        ModelCode: "",
        Model: "",
        ModelYear: "",
        Nationality: "",
        OtherTelNo: "",
        Password: "",
        PaymentMode: "",
        PlateNo: "",

        Policy: "",
        PolicyNo: "",
        PolicyStartDate: "",
        PolicySoldDate: "",
        PolicyEndDate: "",
        Premium: "",
        PremiumCurrencyType: "",
        Product: "",
        ProfilePicture: "",
        RefNo: "",
        SalesPerson: "",
        SerialNo: "",
        Transmission: "",
        UsageType: "",
        UserName: "",
        VariantName: "",
        Vehicle: "",
        VehiclePrice: "",
        VINNo: "",
        EndorsementApproved: "",
        DealerPaymentCurrencyTypeId: "",
        CustomerPaymentCurrencyTypeId: "",
        IsPolicyRenewed: "",
        MWStartDate: "",
        MWEndDate: "",
        GrossWeight: ""
    }

    $scope.ClearCancelation = function () {
        $scope.Cancelation.additionalSerial = '',
            $scope.Cancelation.Address1 = '',
            $scope.Cancelation.Address2 = '',
            $scope.Cancelation.Address3 = '',
            $scope.Cancelation.Address4 = '',
            $scope.Cancelation.Aspiration = '',
            $scope.Cancelation.BAndW = '',
            $scope.Cancelation.BodyType = '',
            $scope.Cancelation.BusinessAddress1 = '',
            $scope.Cancelation.BusinessAddress2 = '',
            $scope.Cancelation.BusinessAddress3 = '',
            $scope.Cancelation.BusinessAddress4 = '',
            $scope.Cancelation.BusinessName = '',
            $scope.Cancelation.BusinessTelNo = '',
            $scope.Cancelation.Category = '',
            $scope.Cancelation.City = '',
            $scope.Cancelation.Comment = '',
            $scope.Cancelation.CommodityType = '',
            $scope.Cancelation.Contract = '',
            $scope.Cancelation.Country = '',
            $scope.Cancelation.CoverType = '',
            $scope.Cancelation.Customer = '',
            $scope.Cancelation.CustomerPayment = '',
            $scope.Cancelation.CancelationComment = '',
            $scope.Cancelation.CustomerPaymentCurrencyType = '',
            $scope.Cancelation.CustomerType = '',
            $scope.Cancelation.CylinderCount = '',
            $scope.Cancelation.DateOfBirth = '',
            $scope.Cancelation.Dealer = '',
            $scope.Cancelation.DealerLocation = '',
            $scope.Cancelation.DealerPayment = '',
            $scope.Cancelation.DealerPaymentCurrencyType = '',
            $scope.Cancelation.DealerPrice = '',
            $scope.Cancelation.DLIssueDate = '',
            $scope.Cancelation.DriveType = '',
            $scope.Cancelation.Discount = '',
            $scope.Cancelation.Email = '',
            $scope.Cancelation.EngineCapacity = '',
            $scope.Cancelation.ExtensionType = '',
            $scope.Cancelation.FirstName = '',
            $scope.Cancelation.FuelType = '',
            $scope.Cancelation.Gender = '',
            $scope.Cancelation.HrsUsedAtPolicySale = '',
            $scope.Cancelation.IDNo = '',
            $scope.Cancelation.IdType = '',
            $scope.Cancelation.InvoiceNo = '',
            $scope.Cancelation.Active = '',
            $scope.Cancelation.Approved = '',
            $scope.Cancelation.PartialPayment = '',
            $scope.Cancelation.PreWarrantyCheck = '',
            $scope.Cancelation.SpecialDeal = '',
            $scope.Cancelation.ItemPrice = '',
            $scope.Cancelation.ItemPurchasedDate = '',
            $scope.Cancelation.ItemStatus = '',
            $scope.Cancelation.LastName = '',
            $scope.Cancelation.Make = '',
            $scope.Cancelation.MobileNo = '',
            $scope.Cancelation.ModelCode = '',
            $scope.Cancelation.Model = '',
            $scope.Cancelation.ModifiedDate = '',
            $scope.Cancelation.ModifiedUser = '',
            $scope.Cancelation.ModelYear = '',
            $scope.Cancelation.Nationality = '',
            $scope.Cancelation.OtherTelNo = '',
            $scope.Cancelation.Password = '',
            $scope.Cancelation.PaymentMode = '',
            $scope.Cancelation.PlateNo = '',

            $scope.Cancelation.Policy = '',
            $scope.Cancelation.PolicyNo = '',
            $scope.Cancelation.PolicySoldDate = '',
            $scope.Cancelation.PolicyEndDate = '',
            $scope.Cancelation.Premium = '',
            $scope.Cancelation.PremiumCurrencyType = '',
            $scope.Cancelation.Product = '',
            $scope.Cancelation.ProfilePicture = '',
            $scope.Cancelation.RefNo = '',
            $scope.Cancelation.SalesPerson = '',
            $scope.Cancelation.SerialNo = '',
            $scope.Cancelation.Transmission = '',
            $scope.Cancelation.UsageType = '',
            $scope.Cancelation.UserName = '',
            $scope.Cancelation.VariantName = '',
            $scope.Cancelation.Vehicle = '',
            $scope.Cancelation.VehiclePrice = '',
            $scope.Cancelation.VINNo = '',
            $scope.Cancelation.EndorsementApproved = '',
            $scope.Cancelation.DealerPaymentCurrencyTypeId = '',
            $scope.Cancelation.CustomerPaymentCurrencyTypeId = '',
            $scope.Cancelation.MWStartDate = '',
            $scope.Cancelation.MWEndDate = '',
            $scope.Cancelation.GrossWeight = '',
            $scope.Cancelation.IsPolicyCanceled = ''
    }
    $scope.ClearRenewal = function () {
        $scope.Renewal.additionalSerial = "",
            $scope.Renewal.Address1 = '',
            $scope.Renewal.Address2 = '',
            $scope.Renewal.Address3 = '',
            $scope.Renewal.Address4 = '',
            $scope.Renewal.Aspiration = '',
            $scope.Renewal.BAndW = '',
            $scope.Renewal.BodyType = '',
            $scope.Renewal.BusinessAddress1 = '',
            $scope.Renewal.BusinessAddress2 = '',
            $scope.Renewal.BusinessAddress3 = '',
            $scope.Renewal.BusinessAddress4 = '',
            $scope.Renewal.BusinessName = '',
            $scope.Renewal.BusinessTelNo = '',
            $scope.Renewal.Category = '',
            $scope.Renewal.City = '',
            $scope.Renewal.Comment = '',
            $scope.Renewal.CommodityType = '',
            $scope.Renewal.Contract = '',
            $scope.Renewal.Country = '',
            $scope.Renewal.CoverType = '',
            $scope.Renewal.Customer = '',
            $scope.Renewal.CustomerPayment = '',
            $scope.Renewal.CustomerPaymentCurrencyType = '',
            $scope.Renewal.CustomerType = '',
            $scope.Renewal.CylinderCount = '',
            $scope.Renewal.DateOfBirth = '',
            $scope.Renewal.Dealer = '',
            $scope.Renewal.DealerLocation = '',
            $scope.Renewal.DealerPayment = '',
            $scope.Renewal.DealerPaymentCurrencyType = '',
            $scope.Renewal.DealerPrice = '',
            $scope.Renewal.DLIssueDate = '',
            $scope.Renewal.DriveType = '',
            $scope.Renewal.Discount = '',
            $scope.Renewal.Email = '',
            $scope.Renewal.EngineCapacity = '',
            $scope.Renewal.ExtensionType = '',
            $scope.Renewal.FirstName = '',
            $scope.Renewal.FuelType = '',
            $scope.Renewal.Gender = '',
            $scope.Renewal.HrsUsedAtPolicySale = '',
            $scope.Renewal.IDNo = '',
            $scope.Renewal.IdType = '',
            $scope.Renewal.InvoiceNo = '',
            $scope.Renewal.Active = '',
            $scope.Renewal.Approved = '',
            $scope.Renewal.PartialPayment = '',
            $scope.Renewal.PreWarrantyCheck = '',
            $scope.Renewal.SpecialDeal = '',
            $scope.Renewal.ItemPrice = '',
            $scope.Renewal.ItemPurchasedDate = '',
            $scope.Renewal.ItemStatus = '',
            $scope.Renewal.LastName = '',
            $scope.Renewal.Make = '',
            $scope.Renewal.MobileNo = '',
            $scope.Renewal.ModelCode = '',
            $scope.Renewal.Model = '',
            $scope.Renewal.ModelYear = '',
            $scope.Renewal.Nationality = '',
            $scope.Renewal.OtherTelNo = '',
            $scope.Renewal.Password = '',
            $scope.Renewal.PaymentMode = '',
            $scope.Renewal.PlateNo = '',

            $scope.Renewal.Policy = '',
            $scope.Renewal.PolicyNo = '',
            $scope.Renewal.PolicySoldDate = '',
            $scope.Renewal.PolicyEndDate = '',
            $scope.Renewal.Premium = '',
            $scope.Renewal.PremiumCurrencyType = '',
            $scope.Renewal.Product = '',
            $scope.Renewal.ProfilePicture = '',
            $scope.Renewal.RefNo = '',
            $scope.Renewal.SalesPerson = '',
            $scope.Renewal.SerialNo = '',
            $scope.Renewal.Transmission = '',
            $scope.Renewal.UsageType = '',
            $scope.Renewal.UserName = '',
            $scope.Renewal.VariantName = '',
            $scope.Renewal.Vehicle = '',
            $scope.Renewal.VehiclePrice = '',
            $scope.Renewal.VINNo = '',
            $scope.Renewal.EndorsementApproved = '',
            $scope.Renewal.DealerPaymentCurrencyTypeId = '',
            $scope.Renewal.CustomerPaymentCurrencyTypeId = '',
            $scope.Renewal.MWStartDate = '',
            $scope.Renewal.MWEndDate = '',
            $scope.Renewal.GrossWeight = '',
            $scope.Renewal.IsApproved = ''

    }
    $scope.ClearPolicy = function () {
        $scope.Policy.additionalSerial = '',
            $scope.Policy.Address1 = '',
            $scope.Policy.Address2 = '',
            $scope.Policy.Address3 = '',
            $scope.Policy.Address4 = '',
            $scope.Policy.Aspiration = '',
            $scope.Policy.BAndW = '',
            $scope.Policy.BodyType = '',
            $scope.Policy.BusinessAddress1 = '',
            $scope.Policy.BusinessAddress2 = '',
            $scope.Policy.BusinessAddress3 = '',
            $scope.Policy.BusinessAddress4 = '',
            $scope.Policy.BusinessName = '',
            $scope.Policy.BusinessTelNo = '',
            $scope.Policy.Category = '',
            $scope.Policy.City = '',
            $scope.Policy.Comment = '',
            $scope.Policy.CommodityType = '',
            $scope.Policy.Contract = '',
            $scope.Policy.Country = '',
            $scope.Policy.CoverType = '',
            $scope.Policy.Customer = '',
            $scope.Policy.CustomerPayment = '',
            $scope.Policy.CustomerPaymentCurrencyType = '',
            $scope.Policy.CustomerType = '',
            $scope.Policy.CylinderCount = '',
            $scope.Policy.DateOfBirth = '',
            $scope.Policy.Dealer = '',
            $scope.Policy.DealerLocation = '',
            $scope.Policy.DealerPayment = '',
            $scope.Policy.DealerPaymentCurrencyType = '',
            $scope.Policy.DealerPrice = '',
            $scope.Policy.DLIssueDate = '',
            $scope.Policy.DriveType = '',
            $scope.Policy.Discount = '',
            $scope.Policy.Email = '',
            $scope.Policy.EngineCapacity = '',
            $scope.Policy.ExtensionType = '',
            $scope.Policy.FirstName = '',
            $scope.Policy.FuelType = '',
            $scope.Policy.Gender = '',
            $scope.Policy.HrsUsedAtPolicySale = '',
            $scope.Policy.IDNo = '',
            $scope.Policy.IdType = '',
            $scope.Policy.InvoiceNo = '',
            $scope.Policy.Active = '',
            $scope.Policy.Approved = '',
            $scope.Policy.PartialPayment = '',
            $scope.Policy.PreWarrantyCheck = '',
            $scope.Policy.SpecialDeal = '',
            $scope.Policy.ItemPrice = '',
            $scope.Policy.ItemPurchasedDate = '',
            $scope.Policy.ItemStatus = '',
            $scope.Policy.LastName = '',
            $scope.Policy.Make = '',
            $scope.Policy.MobileNo = '',
            $scope.Policy.ModelCode = '',
            $scope.Policy.Model = '',
            $scope.Policy.ModelYear = '',
            $scope.Policy.Nationality = '',
            $scope.Policy.OtherTelNo = '',
            $scope.Policy.Password = '',
            $scope.Policy.PaymentMode = '',
            $scope.Policy.PlateNo = '',
            $scope.Policy.Policy = '',
            $scope.Policy.PolicyNo = '',
            $scope.Policy.PolicySoldDate = '',
            $scope.Policy.Premium = '',
            $scope.Policy.PremiumCurrencyType = '',
            $scope.Policy.Product = '',
            $scope.Policy.ProfilePicture = '',
            $scope.Policy.RefNo = '',
            $scope.Policy.SalesPerson = '',
            $scope.Policy.SerialNo = '',
            $scope.Policy.Transmission = '',
            $scope.Policy.UsageType = '',
            $scope.Policy.UserName = '',
            $scope.Policy.VariantName = '',
            $scope.Policy.Vehicle = '',
            $scope.Policy.VehiclePrice = '',
            $scope.Policy.VINNo = '',
            $scope.Policy.IsEndorsementApproved = '',
            $scope.Policy.PolicyEndDate = '',
            $scope.Policy.DealerPaymentCurrencyTypeId = '',
            $scope.Policy.CustomerPaymentCurrencyTypeId = '',
            $scope.Policy.MWStartDate = '',
            $scope.Policy.MWEndDate = '',
            $scope.Policy.GrossWeight = '',
            $scope.Policy.IsApproved = ''
    }
    $scope.ClearEndorsement = function () {
        $scope.Endorsement.AddnSerialNo = '',
            $scope.Endorsement.Address1 = '',
            $scope.Endorsement.Address2 = '',
            $scope.Endorsement.Address3 = '',
            $scope.Endorsement.Address4 = '',
            $scope.Endorsement.Aspiration = '',
            $scope.Endorsement.BAndW = '',
            $scope.Endorsement.BodyType = '',
            $scope.Endorsement.BusinessAddress1 = '',
            $scope.Endorsement.BusinessAddress2 = '',
            $scope.Endorsement.BusinessAddress3 = '',
            $scope.Endorsement.BusinessAddress4 = '',
            $scope.Endorsement.BusinessName = '',
            $scope.Endorsement.BusinessTelNo = '',
            $scope.Endorsement.Category = '',
            $scope.Endorsement.City = '',
            $scope.Endorsement.Comment = '',
            $scope.Endorsement.CommodityType = '',
            $scope.Endorsement.Contract = '',
            $scope.Endorsement.Country = '',
            $scope.Endorsement.CoverType = '',
            $scope.Endorsement.Customer = '',
            $scope.Endorsement.CustomerPayment = '',
            $scope.Endorsement.CustomerPaymentCurrencyType = '',
            $scope.Endorsement.CustomerType = '',
            $scope.Endorsement.CylinderCount = '',
            $scope.Endorsement.DateOfBirth = '',
            $scope.Endorsement.Dealer = '',
            $scope.Endorsement.DealerLocation = '',
            $scope.Endorsement.DealerPayment = '',
            $scope.Endorsement.DealerPaymentCurrencyType = '',
            $scope.Endorsement.DealerPrice = '',
            $scope.Endorsement.DLIssueDate = '',
            $scope.Endorsement.DriveType = '',
            $scope.Endorsement.Discount = '',
            $scope.Endorsement.Email = '',
            $scope.Endorsement.EngineCapacity = '',
            $scope.Endorsement.ExtensionType = '',
            $scope.Endorsement.FirstName = '',
            $scope.Endorsement.FuelType = '',
            $scope.Endorsement.Gender = '',
            $scope.Endorsement.HrsUsedAtPolicySale = '',
            $scope.Endorsement.IDNo = '',
            $scope.Endorsement.IdType = '',
            $scope.Endorsement.InvoiceNo = '',
            $scope.Endorsement.Active = '',
            $scope.Endorsement.Approved = '',
            $scope.Endorsement.PartialPayment = '',
            $scope.Endorsement.PreWarrantyCheck = '',
            $scope.Endorsement.SpecialDeal = '',
            $scope.Endorsement.ItemPrice = '',
            $scope.Endorsement.ItemPurchasedDate = '',
            $scope.Endorsement.ItemStatus = '',
            $scope.Endorsement.LastName = '',
            $scope.Endorsement.Make = '',
            $scope.Endorsement.MobileNo = '',
            $scope.Endorsement.ModelCode = '',
            $scope.Endorsement.Model = '',
            $scope.Endorsement.ModelYear = '',
            $scope.Endorsement.Nationality = '',
            $scope.Endorsement.OtherTelNo = '',
            $scope.Endorsement.Password = '',
            $scope.Endorsement.PaymentMode = '',
            $scope.Endorsement.PlateNo = '',

            $scope.Endorsement.Policy = '',
            $scope.Endorsement.PolicyNo = '',
            $scope.Endorsement.PolicySoldDate = '',
            $scope.Endorsement.Premium = '',
            $scope.Endorsement.PremiumCurrencyType = '',
            $scope.Endorsement.Product = '',
            $scope.Endorsement.ProfilePicture = '',
            $scope.Endorsement.RefNo = '',
            $scope.Endorsement.SalesPerson = '',
            $scope.Endorsement.SerialNo = '',
            $scope.Endorsement.Transmission = '',
            $scope.Endorsement.UsageType = '',
            $scope.Endorsement.UserName = '',
            $scope.Endorsement.VariantName = '',
            $scope.Endorsement.Vehicle = '',
            $scope.Endorsement.VehiclePrice = '',
            $scope.Endorsement.VINNo = '',
            $scope.Endorsement.EndorsementApproved = '',
            $scope.Endorsement.PolicyEndDate = '',
            $scope.Endorsement.DealerPaymentCurrencyTypeId = '',
            $scope.Endorsement.CustomerPaymentCurrencyTypeId = '',
            $scope.Endorsement.MWStartDate = '',
            $scope.Endorsement.MWEndDate = '',
            $scope.Endorsement.GrossWeight = '',
            $scope.Endorsement.IsApproved = ''
    }
    $scope.errorTab1 = "";



    $scope.policySearchGridloading = false;
    $scope.policySearchGridloadAttempted = false;
    $scope.customerSearchGridloading = false;
    $scope.customerGridloadAttempted = false;
    $scope.ProductContracts = [];
    $scope.ProductContractsE = [];
    $scope.ProductContractsR = [];
    $scope.ProductContractsC = [];
    $scope.PolicyDetailsEnable = false
    $scope.GrossTotal = 0.0;
    //-----------------Hard Code Combo: Load from DB when available-------------------------------------//
    $scope.PaymentModes = [{ Id: "b8fa922a-b294-4673-9ff8-10e35488253f", PaymentMode: "Check" }, { Id: "ff07bf9d-660f-4750-80d2-539b1d74f9f5", PaymentMode: "Pay Pal" }, { Id: "15b8a9d8-939d-4d1d-8c51-2dabe9057268", PaymentMode: "Credit Card" }];
    //--------------------------------------------------------------------------------------------------//
    LoadDetails();
    function LoadDetails() {
        //$http({
        //    method: 'POST',
        //    url: '/TAS.Web/api/DealerManagement/GetAllCommodities',
        //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //}).success(function (data, status, headers, config) {
        //    $scope.CommodityTypes = data;
        //}).error(function (data, status, headers, config) {
        //});
        $http({
            method: 'POST',
            url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.commodityTypes = data;
            $scope.policySearchInquiryGridSearchCriterias.commodityTypeId = data[2].CommodityTypeId;
            $scope.selectedCommodityTypeChanged();
        }).error(function (data, status, headers, config) {
        });

        //$http({
        //    method: 'POST',
        //    url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
        //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //}).success(function (data, status, headers, config) {
        //    $scope.commodityTypes = data;
        //}).error(function (data, status, headers, config) {
        //});
        $http({
            method: 'POST',
            url: '/TAS.Web/api/ContractManagement/GetAllCountries',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.countries = data;
            $scope.Allcountries = data;
        }).error(function (data, status, headers, config) {
        });

        $http({
            method: 'POST',
            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.dealers = data;
        }).error(function (data, status, headers, config) {
        });
    }

    //$scope.initiateClaimSearchGrid = function () {
    //    $scope.claimTable = new ngTableParams({
    //        page: 1,
    //        count: 10,
    //    }, {
    //            getData: function ($defer, params) {

    //                var page = params.page();
    //                var size = params.count();
    //                var data = {
    //                    page: page,
    //                    pageSize: size,
    //                    loggedInUserId: $localStorage.LoggedInUserId,
    //                }

    //                $scope.policyGridloading = true;
    //                $scope.policyGridloadAttempted = false;
    //                $http({
    //                    method: 'POST',
    //                    url: '/TAS.Web/api/claim/GetAllClaimsToProcessByUserId',
    //                    data: { 'data': data },
    //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                }).success(function (data, status, headers, config) {
    //                    var responseArr = JSON.parse(data);
    //                    if (responseArr != null) {
    //                        if (responseArr.data.length === 0)
    //                            customErrorMessage("No claim requests found.");
    //                        params.total(responseArr.totalRecords);
    //                        $defer.resolve(responseArr.data);
    //                    } else {
    //                        customErrorMessage("No claim requests found.");
    //                    }
    //                }).error(function (data, status, headers, config) {
    //                }).finally(function () {
    //                    $scope.policyGridloading = false;
    //                    $scope.policyGridloadAttempted = true;
    //                });
    //            }
    //        });

    $scope.selectedCommodityTypeChanged = function () {
        $scope.products = [];
        $scope.commodityCategories = [];
        if (isGuid($scope.policySearchInquiryGridSearchCriterias.commodityTypeId)) {
            $scope.isProductDetailsReadonly = false;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                data: { "Id": $scope.policySearchInquiryGridSearchCriterias.commodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
                if (!$scope.formAction) {

                    angular.forEach($scope.commodityCategories, function (value) {

                        if ($scope.policySearchInquiryGridSearchCriterias.categoryId == value.CommodityCategoryId) {
                            //$scope.serialNumberLength = value.Length;
                            $scope.serialNumberLength_temp = value.Length;
                            $scope.serialNumberCheck();
                            return false;
                        }
                    });

                }
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.product.commodityTypeId }
            }).success(function (data, status, headers, config) {
                $scope.products = data;
            }).error(function (data, status, headers, config) {
            });



            angular.forEach($scope.commodityTypes, function (value) {
                if ($scope.product.commodityTypeId == value.CommodityTypeId) {
                    $scope.currentCommodityTypeCode = value.CommonCode;
                    $scope.currentCommodityTypeName = value.CommodityTypeDescription;
                    $scope.currentCommodityTypeUniqueCode = value.CommodityCode;
                    // $scope, selectedCommodityType
                    return false;
                }
            });

        } else {
            $scope.currentCommodityTypeCode = '';
            $scope.currentCommodityTypeName = '';
            $scope.currentCommodityTypeUniqueCode = '';
        }
        //resetting product details
        //  if ($scope.formAction)
        $scope.productFormReset();
    }

    function isGuid(stringToTest) {
        var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
    }
    function emptyGuid() {
        return "00000000-0000-0000-0000-000000000000";
    }

    $scope.selectedCountryChanged = function () {
        $scope.cities = [];
        if (isGuid($scope.customer.countryId)) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                data: { "countryId": $scope.customer.countryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.cities = data;
                $scope.selectedCityChanged();
            }).error(function (data, status, headers, config) {

            });

            angular.forEach($scope.countries, function (value) {
                if ($scope.customer.countryId == value.Id) {
                    $scope.selectedCountry = value.CountryName;
                    return false;
                }
            });
        } else {
            $scope.selectedCountry = '';
        }
    }



    function setCommodityType() {
        $http({
            method: 'POST',
            url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.CommodityTypes = data;
            angular.forEach($scope.CommodityTypes, function (value) {
                if (value.CommodityTypeDescription == 'Automobile') {
                    $scope.CommodityTypeId = value.Id;
                } else if (value.CommodityTypeDescription == 'Automotive') {
                    $scope.CommodityTypeId = value.Id;
                }
            });
        }).error(function (data, status, headers, config) {
        });
    }

    $scope.SetPolicyCommodityTypeValue = function () {
        //clearAll();
        if ($scope.policySearch.CommodityType == undefined) {
            $scope.policySearch.CommodityType = "";
            $scope.policySearch.vinSerialName = "VIN No/Serial No";
        }
        var result = filterByCommodityTypeId($scope.CommodityTypes, $scope.policySearch.CommodityType)
        if (result != null) {
            if (result.CommodityTypeDescription == "Automobile") {
                $scope.policySearch.vinSerialName = "VIN No";
            } else if (result.CommodityTypeDescription == "Automotive") {
                $scope.policySearch.vinSerialName = "VIN No";
            } else if (result.CommodityTypeDescription == "Brown & White") {
                $scope.policySearch.vinSerialName = "Serial No";
            }
        }
    }

    $scope.SetDealerLocationsValues = function () {
        $scope.errorTab1 = "";
        if ($scope.Policy.DealerId != null) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByDealerId',
                data: { "Id": $scope.Policy.DealerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerLocations = data;
            }).error(function (data, status, headers, config) {
            });
        }
        angular.forEach($scope.Dealers, function (value) {
            if (value.Id == $scope.Policy.DealerId) {
                $scope.Policy.DealerPaymentCurrencyTypeId = value.CurrencyId;
                $scope.Policy.CustomerPaymentCurrencyTypeId = value.CurrencyId;
                angular.forEach($scope.Currencies, function (valueC) {
                    if (value.CurrencyId == valueC.Id)
                        $scope.DealerCurrencyName = valueC.Code;
                });
            }
        });

        if (isGuid($scope.Vehicle.DealerId)) {
            angular.forEach($scope.dealersByCountry, function (value) {
                if ($scope.Vehicle.DealerId == value.Id) {
                    $scope.Vehicle.currencyPeriodId = value.currencyPeriodId;
                    $scope.Vehicle.DealerCurrencyId = value.CurrencyId;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContractsByInsurerId',
                        data: { "Id": $scope.currentContract.insurerId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.reinsurerContracts = data;
                    }).error(function (data, status, headers, config) {
                    });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                        data: { "Id": value.CurrencyId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.VehicleCurrency = data.Code;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                            data: { "Id": value.CurrencyId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == false) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.policyInquiry.inforMessages.tasInformation'),
                                    text: "Selected dealer's currency(" + $scope.VehicleCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                            }
                        }).error(function (data, status, headers, config) {
                        });
                        return false;

                    });
                }
            });
        }
    }

    //--------------------Policy Search----------------------------------//

    var paginationOptionsPolicySearchInquiryGrid = {
        pageNumber: 1,
        pageSize: 25,
        sort: null
    };
    var getPolicySearchPage = function () {
        $scope.policySearchGridloading = true;
        $scope.policySearchGridloadAttempted = false;
        var policySearchGridParam =
        {
            'paginationOptionsPolicySearchInquiryGrid': paginationOptionsPolicySearchInquiryGrid,
            'policySearchInquiryGridSearchCriterias': $scope.policySearchInquiryGridSearchCriterias,

        }
        $http({
            method: 'POST',
            url: '/TAS.Web/api/PolicyReg/GetPoliciesForSearchGridInquiry',
            data: policySearchGridParam,
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            var response_arr = JSON.parse(data);
            $scope.gridOptionsPolicy.data = response_arr.data;
            $scope.gridOptionsPolicy.totalItems = response_arr.totalRecords;
        }).error(function (data, status, headers, config) {
        }).finally(function () {
            $scope.policySearchGridloading = false;
            $scope.policySearchGridloadAttempted = true;

        });
    };
    $scope.PolicySearchPopupReset = function () {

        $scope.policySearchInquiryGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            policyNo: "",
            serialNo: "",
            mobileNo: "",
            Country: emptyGuid(),
            DealerId: emptyGuid(),
            CustomerName: "",
            policyStartDate: "",
            policyEndDate: "",
            policySoldDateTo: "",
            policySoldDateFrom: "",


        }
    }
    $scope.PolicySearchPopup = function () {
        //$scope.attachmentisshow = false;
        var paginationOptionsPolicySearchGrid = {
            pageNumber: 1,
            // pageSize: 25,
            sort: null
        };
        $scope.policySearchInquiryGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            policyNo: "",
            serialNo: "",
            mobileNo: "",
            Country: emptyGuid(),
            DealerId: emptyGuid(),
            CustomerName: "",
            policyStartDate: "",
            policyEndDate: "",
            policySoldDateTo: "",
            policySoldDateFrom: "",


        };
        getPolicySearchPage();
        $scope.ClearCancelation();
        $scope.ClearRenewal();
        $scope.ClearPolicy();
        $scope.ClearEndorsement();
        SearchPolicyPopup = ngDialog.open({
            template: 'popUpSearchPolicy',
            className: 'ngdialog-theme-plain',
            closeByEscape: true,
            showClose: true,
            closeByDocument: true,
            scope: $scope
        });
        return true;
    };


    $scope.policySearchInquiryGridSearchCriterias = {
        commodityTypeId: "",
        policyNo: "",
        serialNo: "",
        mobileNo: "",
        Country: emptyGuid(),
        DealerId: emptyGuid(),
        CustomerName: "",
        policyStartDate: "",
        policyEndDate: "",
        policySoldDateTo: "",
        policySoldDateFrom: "",
    };
    $scope.policySearchGridloading = false;
    $scope.policySearchGridloadAttempted = false;
    //var PaginationOptionsPolicySearchInquiryGrid = {
    //    pageNumber: 1,
    //    pageSize: 25,
    //    sort: null
    //};
    $scope.gridOptionsPolicy = {
        paginationPageSizes: [25, 50, 75],
        paginationPageSize: 25,
        useExternalPagination: true,
        useExternalSorting: true,
        enableColumnMenus: false,
        columnDefs: [
            { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
            { name: $filter('translate')('pages.policyInquiry.popUpSearchPolicy.commodityType'), field: 'CommodityType', enableSorting: false, width: "*", cellClass: 'columCss' },
            { name: $filter('translate')('pages.policyInquiry.popUpSearchPolicy.policyNo'), field: 'PolicyNo', enableSorting: false, width: '30%', cellClass: 'columCss', },
            //{ name: 'Dealer Name', field: 'DealerId', enableSorting: false, cellClass: 'columCss' },
            { name: $filter('translate')('pages.policyInquiry.popUpSearchPolicy.customerName'), field: 'CustomerName', enableSorting: false, width: "*", cellClass: 'columCss' },
            { name: $filter('translate')('pages.policyInquiry.popUpSearchPolicy.customerMobileNo'), field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
            { name: $filter('translate')('pages.policyInquiry.popUpSearchPolicy.policyoldToDate'), field: 'PolicySoldDate', enableSorting: false, width: "*", cellClass: 'columCss' },

            {
                name: ' ',
                cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
                width: 60,
                enableSorting: false
            }
        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            //$scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
            //    if (sortColumns.length == 0) {
            //        PaginationOptionsPolicySearchInquiryGrid.sort = null;
            //    } else {
            //        PaginationOptionsPolicySearchInquiryGrid.sort = sortColumns[0].sort.direction;
            //    }
            //    getPolicySearchPage();
            //});
            gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                paginationOptionsPolicySearchInquiryGrid.pageNumber = newPage;
                paginationOptionsPolicySearchInquiryGrid.pageSize = pageSize;
                getPolicySearchPage();
            });
        }
    };
    $scope.refresSearchGridData = function () {

        if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.CustomerName != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.Country != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.DealerId != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.policyNo != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.mobileNo != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.policySoldDateTo != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.policySoldDateFrom != "") {
            getPolicySearchPage();
        } else if ($scope.policySearchInquiryGridSearchCriterias.commodityTypeId != "" && $scope.policySearchInquiryGridSearchCriterias.serialNo != "") {
            getPolicySearchPage();
        } else {
            SweetAlert.swal({
                title: $filter('translate')('pages.policyInquiry.inforMessages.policyInquirySearch'),
                text: $filter('translate')('pages.policyInquiry.inforMessages.pleaseEnterCommodity'),
                type: "warning",
                confirmButtonText: $filter('translate')('pages.policyInquiry.ok'),
                confirmButtonColor: "rgb(221, 107, 85)"
            });
        }

    }



    $scope.loadPolicy = function (policyId) {
        // alert(policyId);
        if (isGuid(policyId)) {
            SearchPolicyPopup.close();
            $scope.ClearCancelation();
            $scope.ClearEndorsement();
            $scope.ClearPolicy();
            $scope.ClearRenewal();
            $scope.attachmentisshow = false;
            $scope.Automobile = false;
            $scope.Electronics = false;
            $scope.itemDocUploader.queue = [];

            swal({ title: $filter('translate')('pages.policyInquiry.inforMessages.tasInformation'), text: $filter('translate')('pages.policyInquiry.inforMessages.requestingInformation'), showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetAllPolicyInquiryDetails',
                data: { "Id": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.attachmentisshow = true;


                if (data.policyDetails.CommodityTypeCode == "A") {
                    $scope.Automobile = true;
                } else if (data.policyDetails.CommodityTypeCode == "B") {
                    $scope.Automobile = true;
                }
                else {
                    $scope.Automobile = false;
                }
                //

                $scope.currentPolicyBundleId = policyId;



                $scope.Policy.AddnSerialNo = data.policyDetails.AddnSerialNo;
                $scope.Policy.Address1 = data.policyDetails.Address1;
                $scope.Policy.Address2 = data.policyDetails.Address2;
                $scope.Policy.Address3 = data.policyDetails.Address3;
                $scope.Policy.Address4 = data.policyDetails.Address4;
                $scope.Policy.Aspiration = data.policyDetails.Aspiration;
                $scope.Policy.BodyType = data.policyDetails.BodyType;
                $scope.Policy.BusinessAddress1 = data.policyDetails.BusinessAddress1;
                $scope.Policy.BusinessAddress2 = data.policyDetails.BusinessAddress2;
                $scope.Policy.BusinessAddress3 = data.policyDetails.BusinessAddress3;
                $scope.Policy.BusinessAddress4 = data.policyDetails.BusinessAddress4;
                $scope.Policy.BusinessName = data.policyDetails.BusinessName;
                $scope.Policy.BusinessTelNo = data.policyDetails.BusinessTelNo;
                $scope.Policy.Category = data.policyDetails.Category;
                $scope.Policy.City = data.policyDetails.City;
                $scope.Policy.Comment = data.policyDetails.Comment;
                $scope.Policy.CommodityType = data.policyDetails.CommodityType;
                $scope.Policy.Country = data.policyDetails.Country;
                $scope.Policy.CustomerPayment = data.policyDetails.CustomerPayment;
                $scope.Policy.CustomerPaymentCurrencyType = data.policyDetails.CustomerPaymentCurrencyType;
                $scope.Policy.CustomerType = data.policyDetails.CustomerType;

                if ($scope.Policy.CustomerType == "Corporate") {
                    $scope.isCustomerTypeCorporate = true;
                }
                $scope.Policy.CylinderCount = data.policyDetails.CylinderCount;
                $scope.Policy.DateOfBirth = data.policyDetails.DateOfBirth;
                $scope.Policy.Dealer = data.policyDetails.Dealer;
                $scope.Policy.DealerLocation = data.policyDetails.DealerLocation;
                $scope.Policy.DealerPayment = data.policyDetails.DealerPayment;
                $scope.Policy.DealerPaymentCurrencyType = data.policyDetails.DealerPaymentCurrencyType;
                $scope.Policy.DealerPrice = data.policyDetails.DealerPrice;
                $scope.Policy.Discount = data.policyDetails.Discount;
                $scope.Policy.DLIssueDate = data.policyDetails.DLIssueDate;
                $scope.Policy.DriveType = data.policyDetails.DriveTypeId;
                $scope.Policy.Email = data.policyDetails.Email;
                $scope.Policy.EngineCapacity = data.policyDetails.EngineCapacity;
                $scope.Policy.FirstName = data.policyDetails.Customer;
                $scope.Policy.FuelType = data.policyDetails.FuelType;
                $scope.Policy.Gender = data.policyDetails.Gender;
                $scope.Policy.HrsUsedAtPolicySale = data.policyDetails.KmAtPolicySale;
                $scope.Policy.IDNo = data.policyDetails.IDNo;
                $scope.Policy.IdType = data.policyDetails.IDType;
                $scope.Policy.InvoiceNo = data.policyDetails.InvoiceNo;
                $scope.Policy.PartialPayment = data.policyDetails.PartialPayment;
                $scope.Policy.SpecialDeal = data.policyDetails.IsSpecialDeal;
                $scope.Policy.ItemPrice = data.policyDetails.ItemPrice;
                $scope.Policy.ItemPurchasedDate = data.policyDetails.ItemPurchasedDate;
                $scope.Policy.ItemStatus = data.policyDetails.ItemStatus;
                $scope.Policy.LastName = data.policyDetails.LastName;
                $scope.Policy.Make = data.policyDetails.MakeId;
                $scope.Policy.MobileNo = data.policyDetails.MobileNo;
                $scope.Policy.Model = data.policyDetails.ModelId;
                $scope.Policy.ModelYear = data.policyDetails.ModelYear;
                $scope.Policy.Nationality = data.policyDetails.NationalityId;
                $scope.Policy.OtherTelNo = data.policyDetails.OtherTelNo;
                $scope.Policy.PaymentMode = data.policyDetails.PaymentModeId;
                $scope.Policy.PolicyNo = data.policyDetails.PolicyNo;
                $scope.Policy.PolicyStartDate = data.policyDetails.PolicyStartDate;
                $scope.Policy.PolicyEndDate = data.policyDetails.PolicyEndDate;
                $scope.Policy.PlateNo = data.policyDetails.PlateNo;
                $scope.Policy.PolicySoldDate = data.policyDetails.PolicySoldDate;
                $scope.Policy.Premium = data.policyDetails.Premium;
                $scope.Policy.PremiumCurrencyType = data.policyDetails.PremiumCurrencyName;
                $scope.Policy.Product = data.policyDetails.ProductId;
                $scope.Policy.RefNo = data.policyDetails.RefNo;
                $scope.Policy.SalesPerson = data.policyDetails.SalesPersonId;
                $scope.Policy.SerialNo = data.policyDetails.SerialNo;
                $scope.Policy.Transmission = data.policyDetails.TransmissionId;
                $scope.Policy.UsageType = data.policyDetails.UsageTypeId;
                $scope.Policy.VariantName = data.policyDetails.Variant;
                $scope.Policy.VehiclePrice = data.policyDetails.ItemPrice;
                $scope.Policy.VINNo = data.policyDetails.VINNo;
                $scope.Policy.DealerPaymentCurrencyTypeId = data.policyDetails.DealerPaymentCurrencyTypeId;
                $scope.Policy.CustomerPaymentCurrencyTypeId = data.policyDetails.CustomerPaymentCurrencyTypeId;
                $scope.Policy.SerialNo = data.policyDetails.SerialNo;
                $scope.Policy.InvoiceNo = data.policyDetails.InvoiceNo;
                $scope.Policy.ModelCode = data.policyDetails.ModelCode;
                $scope.Policy.MWStartDate = data.policyDetails.MWStartDate;
                $scope.Policy.MWEndDate = data.policyDetails.MWEndDate;
                $scope.Policy.GrossWeight = data.policyDetails.GrossWeight;
                if ($scope.Policy.Product == 'ILOE' || $scope.Policy.Product=='Involuntary Loss of Employment') {

                    $scope.isProductILOE = true;
                }
                $scope.Policy.Status = data.policyDetails.Status;


                //if (data.policyDetails.IsApproved != undefined && data.policyDetails.IsApproved != ""
                //    && data.policyDetails.IsApproved != null && data.policyDetails != ""
                //    && data.policyCancelationDetails == null && data.policyEnrolmentDetails == null
                //    && data.policyrenewal == null && data.policyDetails.IsApprovedflag == true) {

                //    $scope.Policy.IsApproved = data.policyDetails.IsApproved;
                //    //$scope.StatusIsApproved = true;
                //} else if (data.policyCancelationDetails == null && data.policyrenewal == null && data.policyDetails.IsApproved==null) {
                //    $scope.Policy.IsApproved = "Policy Not Approved";
                //    //$scope.StatusIsApproved = true;
                //}


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetAttachmentsByPolicyId',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {



                    $scope.attachments_temp = data.Attachments;

                    for (var i = 0; i < data.Attachments.length; i++) {
                        var file = {
                            name: data.Attachments[i].FileName,
                            size: data.Attachments[i].AttachmentSizeKB
                        }
                        var attachment = {
                            documentType: data.Attachments[i].DocumentType,
                            id: data.Attachments[i].Id,
                            file: file,
                            ref: data.Attachments[i].FileServerRef,
                            attachmentSection: data.Attachments[i].AttachmentSection
                        }

                        //if (data.Attachments[i].AttachmentSection === "Customer") {
                        //    $scope.customerDocUploader.queue.push(attachment)
                        //} else if (data.Attachments[i].AttachmentSection === "Item") {
                        $scope.itemDocUploader.queue.push(attachment)
                        //} else if (data.Attachments[i].AttachmentSection === "Policy") {
                        //    $scope.policyDocUploader.queue.push(attachment)
                        //} else if (data.Attachments[i].AttachmentSection === "Payment") {
                        //    $scope.paymentDocUploader.queue.push(attachment)
                        //} else if (data.Attachments[i].AttachmentSection === "Other") {
                        //    $scope.customerDocUploader.queue.push(attachment)
                        //}
                    }

                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetClaimHistorysByPolicyId',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimHistory = data;

                });

                if (data.policyEnrolmentDetails != null) {
                    $scope.Endorsement.AddnSerialNo = data.policyEnrolmentDetails.AddnSerialNo;
                    $scope.Endorsement.Address1 = data.policyEnrolmentDetails.Address1;
                    $scope.Endorsement.Address2 = data.policyEnrolmentDetails.Address2;
                    $scope.Endorsement.Address3 = data.policyEnrolmentDetails.Address3;
                    $scope.Endorsement.Address4 = data.policyEnrolmentDetails.Address4;
                    $scope.Endorsement.Aspiration = data.policyEnrolmentDetails.Aspiration;
                    $scope.Endorsement.BodyType = data.policyEnrolmentDetails.BodyType;
                    $scope.Endorsement.BusinessAddress1 = data.policyEnrolmentDetails.BusinessAddress1;
                    $scope.Endorsement.BusinessAddress2 = data.policyEnrolmentDetails.BusinessAddress2;
                    $scope.Endorsement.BusinessAddress3 = data.policyEnrolmentDetails.BusinessAddress3;
                    $scope.Endorsement.BusinessAddress4 = data.policyEnrolmentDetails.BusinessAddress4;
                    $scope.Endorsement.BusinessName = data.policyEnrolmentDetails.BusinessName;
                    $scope.Endorsement.BusinessTelNo = data.policyEnrolmentDetails.BusinessTelNo;
                    $scope.Endorsement.Category = data.policyEnrolmentDetails.Category;
                    $scope.Endorsement.City = data.policyEnrolmentDetails.City;
                    $scope.Endorsement.Comment = data.policyEnrolmentDetails.Comment;
                    $scope.Endorsement.CommodityType = data.policyEnrolmentDetails.CommodityType;
                    $scope.Endorsement.Country = data.policyEnrolmentDetails.Country;
                    $scope.Endorsement.CustomerPayment = data.policyEnrolmentDetails.CustomerPayment;
                    $scope.Endorsement.CustomerPaymentCurrencyType = data.policyEnrolmentDetails.CustomerPaymentCurrencyType;
                    $scope.Endorsement.CustomerType = data.policyEnrolmentDetails.CustomerType;
                    $scope.Endorsement.CylinderCount = data.policyEnrolmentDetails.CylinderCountId;
                    $scope.Endorsement.DateOfBirth = data.policyEnrolmentDetails.DateOfBirth;
                    $scope.Endorsement.Dealer = data.policyEnrolmentDetails.Dealer;
                    $scope.Endorsement.DealerLocation = data.policyEnrolmentDetails.DealerLocation;
                    $scope.Endorsement.DealerPayment = data.policyEnrolmentDetails.DealerPayment;
                    $scope.Endorsement.DealerPaymentCurrencyType = data.policyEnrolmentDetails.DealerPaymentCurrencyType;
                    $scope.Endorsement.DealerPrice = data.policyEnrolmentDetails.DealerPrice;
                    $scope.Endorsement.DLIssueDate = data.policyEnrolmentDetails.DLIssueDate;
                    $scope.Endorsement.DriveType = data.policyEnrolmentDetails.DriveType;
                    $scope.Endorsement.Discount = data.policyEnrolmentDetails.Discount;
                    $scope.Endorsement.Email = data.policyEnrolmentDetails.Email;
                    $scope.Endorsement.EngineCapacity = data.policyEnrolmentDetails.EngineCapacity;
                    $scope.Endorsement.ExtensionType = data.policyEnrolmentDetails.ExtensionType;
                    $scope.Endorsement.FirstName = data.policyEnrolmentDetails.Customer;
                    $scope.Endorsement.FuelType = data.policyEnrolmentDetails.FuelType;
                    $scope.Endorsement.Gender = data.policyEnrolmentDetails.Gender;
                    $scope.Endorsement.HrsUsedAtPolicySale = data.policyEnrolmentDetails.HrsUsedAtPolicySale;
                    $scope.Endorsement.IDNo = data.policyEnrolmentDetails.IDNo;
                    $scope.Endorsement.IdType = data.policyEnrolmentDetails.IDTypeId;
                    $scope.Endorsement.InvoiceNo = data.policyEnrolmentDetails.InvoiceNo;
                    $scope.Endorsement.PartialPayment = data.policyEnrolmentDetails.PartialPayment;
                    $scope.Endorsement.SpecialDeal = data.policyEnrolmentDetails.IsSpecialDeal;
                    $scope.Endorsement.ItemPrice = data.policyEnrolmentDetails.ItemPrice;
                    $scope.Endorsement.ItemPurchasedDate = data.policyEnrolmentDetails.ItemPurchasedDate;
                    $scope.Endorsement.ItemStatus = data.policyEnrolmentDetails.ItemStatusId;
                    $scope.Endorsement.LastName = data.policyEnrolmentDetails.LastName;
                    $scope.Endorsement.Make = data.policyEnrolmentDetails.Make;
                    $scope.Endorsement.MobileNo = data.policyEnrolmentDetails.MobileNo;
                    $scope.Endorsement.Model = data.policyEnrolmentDetails.Model;
                    $scope.Endorsement.ModelYear = data.policyEnrolmentDetails.ModelYear;
                    $scope.Endorsement.Nationality = data.policyEnrolmentDetails.Nationality;
                    $scope.Endorsement.OtherTelNo = data.policyEnrolmentDetails.OtherTelNo;
                    $scope.Endorsement.PaymentMode = data.policyEnrolmentDetails.PaymentModeId;
                    $scope.Endorsement.PolicyNo = data.policyEnrolmentDetails.PolicyNo;
                    $scope.Endorsement.PolicyStartDate = data.policyEnrolmentDetails.PolicyStartDate;
                    $scope.Endorsement.PolicyEndDate = data.policyEnrolmentDetails.PolicyEndDate;
                    $scope.Endorsement.PlateNo = data.policyEnrolmentDetails.PlateNo;
                    $scope.Endorsement.PolicySoldDate = data.policyEnrolmentDetails.PolicySoldDate;
                    $scope.Endorsement.Premium = data.policyEnrolmentDetails.Premium;
                    $scope.Endorsement.PremiumCurrencyType = data.policyEnrolmentDetails.PremiumCurrencyName;
                    $scope.Endorsement.Product = data.policyEnrolmentDetails.Product;
                    $scope.Endorsement.RefNo = data.policyEnrolmentDetails.RefNo;
                    $scope.Endorsement.SalesPerson = data.policyEnrolmentDetails.SalesPerson;
                    $scope.Endorsement.SerialNo = data.policyEnrolmentDetails.SerialNo;
                    $scope.Endorsement.Transmission = data.policyEnrolmentDetails.TransmissionId;
                    $scope.Endorsement.UsageType = data.policyEnrolmentDetails.UsageType;
                    $scope.Endorsement.VariantName = data.policyEnrolmentDetails.Variant;
                    $scope.Endorsement.VehiclePrice = data.policyEnrolmentDetails.ItemPrice;
                    $scope.Endorsement.VINNo = data.policyEnrolmentDetails.VINNo;
                    $scope.Endorsement.DealerPaymentCurrencyTypeId = data.policyEnrolmentDetails.DealerPaymentCurrencyTypeId;
                    $scope.Endorsement.CustomerPaymentCurrencyTypeId = data.policyEnrolmentDetails.CustomerPaymentCurrencyTypeId;
                    $scope.Endorsement.ModelCode = data.policyEnrolmentDetails.ModelCode;
                    $scope.Endorsement.MWStartDate = data.policyEnrolmentDetails.MWStartDate;
                    $scope.Endorsement.MWEndDate = data.policyEnrolmentDetails.MWEndDate;
                    $scope.Endorsement.GrossWeight = data.policyEnrolmentDetails.GrossWeight;
                }


                if (data.policyCancelationDetails == null && data.policyEnrolmentDetails.IsPolicyEndrosed != undefined   && data.policyrenewal == null  && data.policyDetails != null) {

                    $scope.Endorsement.IsApproved = data.policyEnrolmentDetails.IsPolicyEndrosed;
                    $scope.StatusIsApproved = true;
                }

                if (data.policyCancelationDetails != null) {
                    $scope.Cancelation.AddnSerialNo = data.policyCancelationDetails.AddnSerialNo;
                    $scope.Cancelation.Address1 = data.policyCancelationDetails.Address1;
                    $scope.Cancelation.Address2 = data.policyCancelationDetails.Address2;
                    $scope.Cancelation.Address3 = data.policyCancelationDetails.Address3;
                    $scope.Cancelation.Address4 = data.policyCancelationDetails.Address4;
                    $scope.Cancelation.Aspiration = data.policyCancelationDetails.Aspiration;
                    $scope.Cancelation.BodyType = data.policyCancelationDetails.BodyType;
                    $scope.Cancelation.BusinessAddress1 = data.policyCancelationDetails.BusinessAddress1;
                    $scope.Cancelation.BusinessAddress2 = data.policyCancelationDetails.BusinessAddress2;
                    $scope.Cancelation.BusinessAddress3 = data.policyCancelationDetails.BusinessAddress3;
                    $scope.Cancelation.BusinessAddress4 = data.policyCancelationDetails.BusinessAddress4;
                    $scope.Cancelation.BusinessName = data.policyCancelationDetails.BusinessName;
                    $scope.Cancelation.BusinessTelNo = data.policyCancelationDetails.BusinessTelNo;
                    $scope.Cancelation.Category = data.policyCancelationDetails.Category;
                    $scope.Cancelation.City = data.policyCancelationDetails.City;
                    $scope.Cancelation.Comment = data.policyCancelationDetails.Comment;
                    $scope.Cancelation.CommodityType = data.policyCancelationDetails.CommodityType;
                    $scope.Cancelation.Country = data.policyCancelationDetails.Country;
                    $scope.Cancelation.CustomerPayment = data.policyCancelationDetails.CustomerPayment;
                    $scope.Cancelation.CustomerPaymentCurrencyType = data.policyCancelationDetails.CustomerPaymentCurrencyType;
                    $scope.Cancelation.CustomerType = data.policyCancelationDetails.CustomerType;
                    $scope.Cancelation.CylinderCount = data.policyCancelationDetails.CylinderCountId;
                    $scope.Cancelation.DateOfBirth = data.policyCancelationDetails.DateOfBirth;
                    $scope.Cancelation.Dealer = data.policyCancelationDetails.Dealer;
                    $scope.Cancelation.DealerLocation = data.policyCancelationDetails.DealerLocation;
                    $scope.Cancelation.DealerPayment = data.policyCancelationDetails.DealerPayment;
                    $scope.Cancelation.DealerPaymentCurrencyType = data.policyCancelationDetails.DealerPaymentCurrencyType;
                    $scope.Cancelation.DealerPrice = data.policyCancelationDetails.DealerPrice;
                    $scope.Cancelation.DLIssueDate = data.policyCancelationDetails.DLIssueDate;
                    $scope.Cancelation.DriveType = data.policyCancelationDetails.DriveType;
                    $scope.Cancelation.Discount = data.policyCancelationDetails.Discount;
                    $scope.Cancelation.Email = data.policyCancelationDetails.Email;
                    $scope.Cancelation.EngineCapacity = data.policyCancelationDetails.EngineCapacity;
                    $scope.Cancelation.ExtensionType = data.policyCancelationDetails.ExtensionType;
                    $scope.Cancelation.FirstName = data.policyCancelationDetails.Customer;
                    $scope.Cancelation.FuelType = data.policyCancelationDetails.FuelType;
                    $scope.Cancelation.Gender = data.policyCancelationDetails.Gender;
                    $scope.Cancelation.HrsUsedAtPolicySale = data.policyCancelationDetails.HrsUsedAtPolicySale;
                    $scope.Cancelation.IDNo = data.policyCancelationDetails.IDNo;
                    $scope.Cancelation.IdType = data.policyCancelationDetails.IDTypeId;
                    $scope.Cancelation.InvoiceNo = data.policyCancelationDetails.InvoiceNo;
                    $scope.Cancelation.PartialPayment = data.policyCancelationDetails.PartialPayment;
                    $scope.Cancelation.SpecialDeal = data.policyCancelationDetails.IsSpecialDeal;
                    $scope.Cancelation.ItemPrice = data.policyCancelationDetails.ItemPrice;
                    $scope.Cancelation.ItemPurchasedDate = data.policyCancelationDetails.ItemPurchasedDate;
                    $scope.Cancelation.ItemStatus = data.policyCancelationDetails.ItemStatusId;
                    $scope.Cancelation.LastName = data.policyCancelationDetails.LastName;
                    $scope.Cancelation.Make = data.policyCancelationDetails.Make;
                    $scope.Cancelation.MobileNo = data.policyCancelationDetails.MobileNo;
                    $scope.Cancelation.Model = data.policyCancelationDetails.Model;
                    $scope.Cancelation.ModelYear = data.policyCancelationDetails.ModelYear;
                    $scope.Cancelation.Nationality = data.policyCancelationDetails.Nationality;
                    $scope.Cancelation.OtherTelNo = data.policyCancelationDetails.OtherTelNo;
                    $scope.Cancelation.PaymentMode = data.policyCancelationDetails.PaymentModeId;
                    $scope.Cancelation.PolicyNo = data.policyCancelationDetails.PolicyNo;
                    $scope.Cancelation.PolicyStartDate = data.policyCancelationDetails.PolicyStartDate;
                    $scope.Cancelation.PolicyEndDate = data.policyCancelationDetails.PolicyEndDate;
                    $scope.Cancelation.PlateNo = data.policyCancelationDetails.PlateNo;
                    $scope.Cancelation.PolicySoldDate = data.policyCancelationDetails.PolicySoldDate;
                    $scope.Cancelation.Premium = data.policyCancelationDetails.Premium;
                    $scope.Cancelation.PremiumCurrencyType = data.policyCancelationDetails.PremiumCurrencyName;
                    $scope.Cancelation.Product = data.policyCancelationDetails.ProductId;
                    $scope.Cancelation.RefNo = data.policyCancelationDetails.RefNo;
                    $scope.Cancelation.SalesPerson = data.policyCancelationDetails.SalesPersonId;
                    $scope.Cancelation.SerialNo = data.policyCancelationDetails.SerialNo;
                    $scope.Cancelation.Transmission = data.policyCancelationDetails.TransmissionId;
                    $scope.Cancelation.UsageType = data.policyCancelationDetails.UsageTypeId;
                    $scope.Cancelation.VariantName = data.policyCancelationDetails.Variant;
                    $scope.Cancelation.VehiclePrice = data.policyCancelationDetails.ItemPrice;
                    $scope.Cancelation.VINNo = data.policyCancelationDetails.VINNo;
                    $scope.Cancelation.CancelationComment = data.policyCancelationDetails.CancelationComment;
                    $scope.Cancelation.ModifiedDate = data.policyCancelationDetails.ModifiedDate;
                    $scope.Cancelation.ModifiedUser = data.policyCancelationDetails.ModifiedUser;
                    $scope.Cancelation.DealerPaymentCurrencyTypeId = data.policyCancelationDetails.DealerPaymentCurrencyTypeId;
                    $scope.Cancelation.CustomerPaymentCurrencyTypeId = data.policyCancelationDetails.CustomerPaymentCurrencyTypeId;
                    $scope.Cancelation.ModelCode = data.policyCancelationDetails.ModelCode;
                    $scope.Cancelation.MWStartDate = data.policyCancelationDetails.MWStartDate;
                    $scope.Cancelation.MWEndDate = data.policyCancelationDetails.MWEndDate;
                    $scope.Cancelation.GrossWeight = data.policyCancelationDetails.GrossWeight;
                    $scope.Cancelation.IsPolicyCancele = data.policyCancelationDetails.IsPolicyCancele;

                    //if (data.policyEnrolmentDetails.IsApproved == null && data.policyCancelationDetails.IsPolicyCancele == true &&
                    //    data.policyCancelationDetails.IsPolicyCanceled != undefined && data.policyrenewal == null) {

                    //    $scope.Cancelation.IsPolicyCanceled = data.policyCancelationDetails.IsPolicyCanceled;
                    //    $scope.StatusIsPolicyCanceled = true;
                    //}

                }


                if (data.policyrenewal != null) {
                    $scope.Renewal.AddnSerialNo = data.policyrenewal.AddnSerialNo;
                    $scope.Renewal.Address1 = data.policyrenewal.Address1;
                    $scope.Renewal.Address2 = data.policyrenewal.Address2;
                    $scope.Renewal.Address3 = data.policyrenewal.Address3;
                    $scope.Renewal.Address4 = data.policyrenewal.Address4;
                    $scope.Renewal.Aspiration = data.policyrenewal.Aspiration;
                    $scope.Renewal.BodyType = data.policyrenewal.BodyType;
                    $scope.Renewal.BusinessAddress1 = data.policyrenewal.BusinessAddress1;
                    $scope.Renewal.BusinessAddress2 = data.policyrenewal.BusinessAddress2;
                    $scope.Renewal.BusinessAddress3 = data.policyrenewal.BusinessAddress3;
                    $scope.Renewal.BusinessAddress4 = data.policyrenewal.BusinessAddress4;
                    $scope.Renewal.BusinessName = data.policyrenewal.BusinessName;
                    $scope.Renewal.BusinessTelNo = data.policyrenewal.BusinessTelNo;
                    $scope.Renewal.Category = data.policyrenewal.Category;
                    $scope.Renewal.City = data.policyrenewal.City;
                    $scope.Renewal.Comment = data.policyrenewal.Comment;
                    $scope.Renewal.CommodityType = data.policyrenewal.CommodityType;
                    $scope.Renewal.Country = data.policyrenewal.Country;
                    $scope.Renewal.CustomerPayment = data.policyrenewal.CustomerPayment;
                    $scope.Renewal.CustomerPaymentCurrencyType = data.policyrenewal.CustomerPaymentCurrencyType;
                    $scope.Renewal.CustomerType = data.policyrenewal.CustomerType;
                    $scope.Renewal.CylinderCount = data.policyrenewal.CylinderCountId;
                    $scope.Renewal.DateOfBirth = data.policyrenewal.DateOfBirth;
                    $scope.Renewal.Dealer = data.policyrenewal.Dealer;
                    $scope.Renewal.DealerLocation = data.policyrenewal.DealerLocation;
                    $scope.Renewal.DealerPayment = data.policyrenewal.DealerPayment;
                    $scope.Renewal.DealerPaymentCurrencyType = data.policyrenewal.DealerPaymentCurrencyType;
                    $scope.Renewal.DealerPrice = data.policyrenewal.DealerPrice;
                    $scope.Renewal.DLIssueDate = data.policyrenewal.DLIssueDate;
                    $scope.Renewal.DriveType = data.policyrenewal.DriveType;
                    $scope.Renewal.Discount = data.policyrenewal.Discount;
                    $scope.Renewal.Email = data.policyrenewal.Email;
                    $scope.Renewal.EngineCapacity = data.policyrenewal.EngineCapacity;
                    $scope.Renewal.ExtensionType = data.policyrenewal.ExtensionType;
                    $scope.Renewal.FirstName = data.policyrenewal.Customer;
                    $scope.Renewal.FuelType = data.policyrenewal.FuelType;
                    $scope.Renewal.Gender = data.policyrenewal.Gender;
                    $scope.Renewal.HrsUsedAtPolicySale = data.policyrenewal.HrsUsedAtPolicySale;
                    $scope.Renewal.IDNo = data.policyrenewal.IDNo;
                    $scope.Renewal.IdType = data.policyrenewal.IDTypeId;
                    $scope.Renewal.InvoiceNo = data.policyrenewal.InvoiceNo;
                    $scope.Renewal.PartialPayment = data.policyrenewal.PartialPayment;
                    $scope.Renewal.SpecialDeal = data.policyrenewal.IsSpecialDeal;
                    $scope.Renewal.ItemPrice = data.policyrenewal.ItemPrice;
                    $scope.Renewal.ItemPurchasedDate = data.policyrenewal.ItemPurchasedDate;
                    $scope.Renewal.ItemStatus = data.policyrenewal.ItemStatusId;
                    $scope.Renewal.LastName = data.policyrenewal.LastName;
                    $scope.Renewal.Make = data.policyrenewal.Make;
                    $scope.Renewal.MobileNo = data.policyrenewal.MobileNo;
                    $scope.Renewal.Model = data.policyrenewal.Model;
                    $scope.Renewal.ModelYear = data.policyrenewal.ModelYear;
                    $scope.Renewal.Nationality = data.policyrenewal.Nationality;
                    $scope.Renewal.OtherTelNo = data.policyrenewal.OtherTelNo;
                    $scope.Renewal.PaymentMode = data.policyrenewal.PaymentModeId;
                    $scope.Renewal.PolicyNo = data.policyrenewal.PolicyNo;
                    $scope.Renewal.PolicyStartDate = data.policyrenewal.PolicyStartDate;
                    $scope.Renewal.PolicyEndDate = data.policyrenewal.PolicyEndDate;
                    $scope.Renewal.PlateNo = data.policyrenewal.PlateNo;
                    $scope.Renewal.PolicySoldDate = data.policyrenewal.PolicySoldDate;
                    $scope.Renewal.Premium = data.policyrenewal.Premium;
                    $scope.Renewal.PremiumCurrencyType = data.policyrenewal.PremiumCurrencyName;
                    $scope.Renewal.Product = data.policyrenewal.ProductId;
                    $scope.Renewal.RefNo = data.policyrenewal.RefNo;
                    $scope.Renewal.SalesPerson = data.policyrenewal.SalesPersonId;
                    $scope.Renewal.SerialNo = data.policyrenewal.SerialNo;
                    $scope.Renewal.Transmission = data.policyrenewal.TransmissionId;
                    $scope.Renewal.UsageType = data.policyrenewal.UsageTypeId;
                    $scope.Renewal.VariantName = data.policyrenewal.Variant;
                    $scope.Renewal.VehiclePrice = data.policyrenewal.ItemPrice;
                    $scope.Renewal.VINNo = data.policyrenewal.VINNo;
                    $scope.Renewal.DealerPaymentCurrencyTypeId = data.policyrenewal.DealerPaymentCurrencyTypeId;
                    $scope.Renewal.CustomerPaymentCurrencyTypeId = data.policyrenewal.CustomerPaymentCurrencyTypeId;
                    $scope.Renewal.ModelCode = data.policyrenewal.ModelCode;
                    $scope.Renewal.MWStartDate = data.policyrenewal.MWStartDate;
                    $scope.Renewal.MWEndDate = data.policyrenewal.MWEndDate;
                    $scope.Renewal.GrossWeight = data.policyrenewal.GrossWeight;


                    //if (data.policyEnrolmentDetails.IsApproved != undefined && data.policyCancelationDetails == null && data.policyrenewal != null) {

                    //    $scope.Renewal.IsPolicyRenewed = data.policyrenewal.IsPolicyRenewed;
                    //    $scope.StatusIsPolicyRenewed = true;
                    //}
                }



            }).finally(function () { swal.close(); });
        }
    }

    $scope.downloadAttachmentUploaded = function (ref) {
        if (ref != '') {
            swal({ title: $filter('translate')('pages.policyInquiry.inforMessages.processing'), text: $filter('translate')('pages.policyInquiry.inforMessages.preparingDownload'), showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Upload/DownloadAttachment',
                data: { 'fileRef': ref },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt },
                responseType: 'arraybuffer',
            }).success(function (data, status, headers, config) {
                headers = headers();
                var filename = headers['x-filename'];
                var contentType = headers['content-type'];
                var linkElement = document.createElement('a');
                try {
                    var blob = new Blob([data], { type: contentType });
                    var url = window.URL.createObjectURL(blob);

                    linkElement.setAttribute('href', url);
                    linkElement.setAttribute("download", filename);

                    var clickEvent = new MouseEvent("click", {
                        "view": window,
                        "bubbles": true,
                        "cancelable": false
                    });
                    linkElement.dispatchEvent(clickEvent);
                } catch (ex) {
                    console.log(ex);
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }
    }

    $scope.downloadAttachment = function () {

        swal({ title: $filter('translate')('pages.policyInquiry.inforMessages.processing'), text: $filter('translate')('pages.policyInquiry.inforMessages.preparingDownload'), showConfirmButton: false });
        $http({
            method: 'POST',
            url: '/TAS.Web/api/PolicyReg/GetPolicyAttachmentById',
            data: { "policyid": $scope.currentPolicyBundleId },
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            , responseType: 'arraybuffer',
        }).success(function (data, status, headers, config) {
            try {
                var octetStreamMime = 'application/octet-stream';
                var success = false;

                // Get the headers
                headers = headers();

                // Get the filename from the x-filename header or default to "download.bin"
                var filename = headers['x-filename'] || 'Policy.pdf';

                // Determine the content type from the header or default to "application/octet-stream"
                var contentType = headers['content-type'] || octetStreamMime;

                try {
                    // Try using msSaveBlob if supported
                    console.log("Trying saveBlob method ...");
                    var blob = new Blob([data], { type: contentType });
                    if (navigator.msSaveBlob)
                        navigator.msSaveBlob(blob, filename);
                    else {
                        // Try using other saveBlob implementations, if available
                        var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                        if (saveBlob === undefined) throw "Not supported";
                        saveBlob(blob, filename);
                    }
                    console.log("saveBlob succeeded");
                    success = true;
                } catch (ex) {
                    console.log("saveBlob method failed with the following exception:");
                    console.log(ex);
                }

                if (!success) {
                    // Get the blob url creator
                    var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                    if (urlCreator) {
                        // Try to use a download link
                        var link = document.createElement('a');
                        if ('download' in link) {
                            // Try to simulate a click
                            try {
                                // Prepare a blob URL
                                console.log("Trying download link method with simulated click ...");
                                var blob = new Blob([data], { type: contentType });
                                var url = urlCreator.createObjectURL(blob);
                                link.setAttribute('href', url);

                                // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                link.setAttribute("download", filename);

                                // Simulate clicking the download link
                                var event = document.createEvent('MouseEvents');
                                event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                                link.dispatchEvent(event);
                                console.log("Download link method with simulated click succeeded");
                                success = true;

                            } catch (ex) {
                                console.log("Download link method with simulated click failed with the following exception:");
                                console.log(ex);
                            }
                        }

                        if (!success) {
                            // Fallback to window.location method
                            try {
                                // Prepare a blob URL
                                // Use application/octet-stream when using window.location to force download
                                console.log("Trying download link method with window.location ...");
                                var blob = new Blob([data], { type: octetStreamMime });
                                var url = urlCreator.createObjectURL(blob);
                                window.location = url;
                                console.log("Download link method with window.location succeeded");
                                success = true;
                            } catch (ex) {
                                console.log("Download link method with window.location failed with the following exception:");
                                console.log(ex);
                            }
                        }

                    }
                }

                if (!success) {
                    console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                    window.open(httpPath, '_blank', '');
                }
            }
            catch (ex) {

            }

            //headers = headers();
            //var filename = "Policy.pdf";//headers['x-filename'];
            //var contentType = headers['content-type'];
            //var linkElement = document.createElement('a');
            //try {

            //    var file = new Blob([data], { type: 'application/pdf' });
            //    var fileURL = URL.createObjectURL(file);

            //    saveAs(file, "download.pdf");
            //}
            //catch (ex) {

            //}

            //try{
            //    window.open(data, '_blank', '');
            //}
            //catch (ex) {

            //}
            //try {

            //    var blob = new Blob([data], { type: contentType });
            //    try {

            //        saveAs(blob, "download1.pdf");
            //    }
            //    catch (ex) {

            //    }

            //    try {
            //        var pdfFile = new Blob([data], {
            //            type: 'application/pdf'
            //        });
            //        var pdfUrl = URL.createObjectURL(pdfFile);
            //        var printwWindow = window.open(pdfUrl, '_blank', '');
            //        printwWindow.print();
            //    }
            //    catch (ex) {

            //    }

            //    var url = window.URL.createObjectURL(blob);

            //    linkElement.setAttribute('href', url);
            //    linkElement.setAttribute("download", filename);

            //    var clickEvent = new MouseEvent("click", {
            //        "view": window,
            //        "bubbles": true,
            //        "cancelable": false
            //    });
            //    linkElement.dispatchEvent(clickEvent);
            //} catch (ex) {
            //    console.log(ex);
            //}
        }).error(function (data, status, headers, config) {
        }).finally(function () {
            swal.close();
        });

    }

    //$scope.PolicySearchPopup = function () {
    //    var paginationOptionsPolicySearchInquiryGrid = {
    //        pageNumber: 1,
    //        // pageSize: 25,
    //        sort: null
    //    };
    //    $scope.policySearchInquiryGridSearchCriterias = {
    //        commodityTypeId: emptyGuid(),
    //        policyNo: "",
    //        serialNo: "",
    //        mobileNo: "",
    //        policyStartDate: "",
    //        policyEndDate: "",
    //    };
    //    getPolicySearchPage();
    //    SearchPolicyPopup = ngDialog.open({
    //        template: 'popUpSearchPolicy',
    //        className: 'ngdialog-theme-plain',
    //        closeByEscape: true,
    //        showClose: true,
    //        closeByDocument: true,
    //        scope: $scope
    //    });
    //    return true;
    //};




});



