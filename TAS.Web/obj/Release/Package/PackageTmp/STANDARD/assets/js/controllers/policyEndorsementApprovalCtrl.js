app.controller('PolicyEndorsementApprovalCtrl', function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {
    $scope.ModalName = "Policy Endorsement Approval";
    $scope.AutomobileP = true;
    $scope.AutomobileE = true;
    $scope.isProductILOE = false;
    $scope.selectedCustomerTypeName = '';
    var SearchPolicyPopup;
    $scope.xxx = true;

    LoadDetails();
    function LoadDetails() {
        $http({
            method: 'POST',
            url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.commodityTypes = data;
        }).error(function (data, status, headers, config) {
        });
    }

    $scope.ProductContract = {
        ProductId: "00000000-0000-0000-0000-000000000000",
        ParentProductId: "00000000-0000-0000-0000-000000000000",
        ContractId: "00000000-0000-0000-0000-000000000000",
        ExtensionTypeId: "00000000-0000-0000-0000-000000000000",
        CoverTypeId: "00000000-0000-0000-0000-000000000000",
        Contracts: [],
        ExtensionTypes: [],
        CoverTypes: [],
        Premium: 0,
        PremiumCurrencyName: '',
        PremiumCurrencyTypeId: "00000000-0000-0000-0000-000000000000",
        Name: ''
    };
    $scope.ProductContracts = [];
    $scope.ProductContractsE = [];
    $scope.GrossTotal = 0.0;
    $scope.currentPolicyBundleId = "00000000-0000-0000-0000-000000000000";
    
        
    $scope.PolicyH = {
        CommodityTypeH: true,
        ProductH: true,
        DealerH: true,
        DealerLocationH: true,
        ContractH: true,
        ExtensionTypeH: true,
        PremiumH: true,
        PremiumCurrencyTypeH: true,
        DealerPaymentCurrencyTypeH: true,
        CustomerPaymentCurrencyTypeH: true,
        CoverTypeH: true,
        HrsUsedAtPolicySaleH: true,
        PreWarrantyCheckH: true,
        PolicySoldDateH: true,
        SalesPersonH: true,
        PolicyNoH: true,
        SpecialDealH: true,
        PartialPaymentH: true,
        DealerPaymentH: true,
        CustomerPaymentH: true,
        PaymentModeH: true,
        RefNoH: true,
        CommentH: true,
        ItemH: true,
        TypeH: true,
        CustomerH: true,
        FirstNameH: true,
        LastNameH: true,
        NationalityH: true,
        DateOfBirthH: true,
        CountryH: true,
        GenderH: true,
        MobileNoH: true,
        OtherTelNoH: true,
        CustomerTypeH: true,
        UsageTypeH: true,
        AddressH: true,
        IDNoH: true,
        IdTypeH: true,
        CityH: true,
        DLIssueDateH: true,
        EmailH: true,
        BusinessNameH: true,
        BusinessAddressH: true,
        BusinessTelNoH: true,
        VehicleH: true,
        VINNoH: true,
        MakeH: true,
        ModelH: true,
        CategoryH: true,
        ItemStatusH: true,
        CylinderCountH: true,
        BodyTypeH: true,
        PlateNoH: true,
        ModelYearH: true,
        FuelTypeH: true,
        AspirationH: true,
        VariantH: true,
        TransmissionH: true,
        ItemPurchasedDateH: true,
        EngineCapacityH: true,
        DriveTypeH: true,
        VehiclePriceH: true,
        DealerPriceH: true,
        BAndWH: true,
        ItemPriceH: true,
        AddnSerialNoH: true,
        InvoiceNoH: true,
        ModelCodeH: true
    }
    $scope.EndorsementH = {
        CommodityTypeH: true,
        ProductH: true,
        DealerH: true,
        DealerLocationH: true,
        ContractH: true,
        ExtensionTypeH: true,
        PremiumH: true,
        PremiumCurrencyTypeH: true,
        DealerPaymentCurrencyTypeH: true,
        CustomerPaymentCurrencyTypeH: true,
        CoverTypeH: true,
        HrsUsedAtPolicySaleH: true,
        PreWarrantyCheckH: true,
        PolicySoldDateH: true,
        SalesPersonH: true,
        PolicyNoH: true,
        SpecialDealH: true,
        PartialPaymentH: true,
        DealerPaymentH: true,
        CustomerPaymentH: true,
        PaymentModeH: true,
        RefNoH: true,
        CommentH: true,
        ItemH: true,
        TypeH: true,
        CustomerH: true,
        FirstNameH: true,
        LastNameH: true,
        NationalityH: true,
        DateOfBirthH: true,
        CountryH: true,
        GenderH: true,
        MobileNoH: true,
        OtherTelNoH: true,
        CustomerTypeH: true,
        UsageTypeH: true,
        AddressH: true,
        IDNoH: true,
        IdTypeH: true,
        CityH: true,
        DLIssueDateH: true,
        EmailH: true,
        BusinessNameH: true,
        BusinessAddressH: true,
        BusinessTelNoH: true,
        VehicleH: true,
        VINNoH: true,
        MakeH: true,
        ModelH: true,
        CategoryH: true,
        ItemStatusH: true,
        CylinderCountH: true,
        BodyTypeH: true,
        PlateNoH: true,
        ModelYearH: true,
        FuelTypeH: true,
        AspirationH: true,
        VariantH: true,
        TransmissionH: true,
        ItemPurchasedDateH: true,
        EngineCapacityH: true,
        DriveTypeH: true,
        VehiclePriceH: true,
        DealerPriceH: true,
        BAndWH: true,
        ItemPriceH: true,
        AddnSerialNoH: true,
        InvoiceNoH: true,
        ModelCodeH: true
    }

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
        IsEndorsementApproved: ""
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
        EndorsementApproved: ""
    }


    //supportive functions
    var isGuid = function (stringToTest) {
        var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
    }
    var emptyGuid = function () {
        return "00000000-0000-0000-0000-000000000000";
    }

    var paginationOptionsPolicySearchGrid = {
        pageNumber: 1,
        pageSize: 25,
        sort: null
    };
    var getPolicySearchPage = function () {
        $scope.policySearchGridloading = true;
        $scope.policySearchGridloadAttempted = false;
        var policySearchGridParam =
            {
                'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                'type': 'forendorsementapproval',
                'userId': $localStorage.LoggedInUserId
            }
        $http({
            method: 'POST',
            url: '/TAS.Web/api/PolicyReg/GetPoliciesForSearchGrid',
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
        $scope.policySearchGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            policyNo: "",
            serialNo: "",
            mobileNo: "",
            policyStartDate: "",
            policyEndDate: "",
        };
    }

    $scope.PolicySearchPopup = function () {
        var paginationOptionsPolicySearchGrid = {
            pageNumber: 1,
            // pageSize: 25,
            sort: null
        };
        $scope.policySearchGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            policyNo: "",
            serialNo: "",
            mobileNo: "",
            policyStartDate: "",
            policyEndDate: "",
        };
        getPolicySearchPage();
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

    //--------------------Policy Search----------------------------------//
    $scope.policySearchGridSearchCriterias = {
        commodityTypeId: emptyGuid(),
        policyNo: "",
        serialNo: "",
        mobileNo: "",
        policyStartDate: "",
        policyEndDate: "",
    };
    $scope.policySearchGridloading = false;
    $scope.policySearchGridloadAttempted = false;
    var paginationOptionsPolicySearchGrid = {
        pageNumber: 1,
        pageSize: 25,
        sort: null
    };
    $scope.gridOptionsPolicy = {
        paginationPageSizes: [25, 50, 75],
        paginationPageSize: 25,
        useExternalPagination: true,
        useExternalSorting: true,
        enableColumnMenus: false,
        columnDefs: [
          { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
          { name: 'Commodity Type', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
          { name: 'Policy No', field: 'PolicyNo', width: '30%', enableSorting: false, cellClass: 'columCss', },
          { name: 'Vin or Serial', field: 'SerialNo', width: '20%', enableSorting: false, cellClass: 'columCss' },
          { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
          { name: 'Policy Sold Date', field: 'PolicySoldDate', enableSorting: false, cellClass: 'columCss' },

          {
              name: ' ',
              cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
              width: 60,
              enableSorting: false
          }
        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                if (sortColumns.length == 0) {
                    paginationOptionsPolicySearchGrid.sort = null;
                } else {
                    paginationOptionsPolicySearchGrid.sort = sortColumns[0].sort.direction;
                }
                getPolicySearchPage();
            });
            gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                paginationOptionsPolicySearchGrid.pageNumber = newPage;
                paginationOptionsPolicySearchGrid.pageSize = pageSize;
                getPolicySearchPage();
            });
        }
    };
    $scope.refresSearchGridData = function () {
        getPolicySearchPage();
    }
  

    $scope.loadPolicy = function (policyId) {
        if (isGuid(policyId)) {
            if (typeof SearchPolicyPopup != 'undefined')
                SearchPolicyPopup.close();
        swal({ title: "TAS Information", text: "Reading Endorsement Information..", showConfirmButton: false });
        
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetAllPolicyEndorsementDetailsForApproval',
                data: { "Id": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.currentPolicyBundleId = policyId;
                if ($scope.Policy.CommodityType != 'Automobile')
                {
                    $scope.AutomobileP = false;
                    $scope.AutomobileE = true;
                } else if ($scope.Policy.CommodityType != 'Automotive') {
                    $scope.AutomobileP = false;
                    $scope.AutomobileE = true;
                } else {

                    $scope.AutomobileP = true;
                    $scope.AutomobileE = false;
                }
                if (data.policyDetails.policyDetails.customer.dateOfBirth == '0001-01-01T00:00:00') {
                    $scope.Policy.DateOfBirth = "";
                }
                else {
                    $scope.Policy.DateOfBirth = data.policyDetails.policyDetails.customer.dateOfBirth;
                }
                if (data.policyDetails.policyDetails.customer.idIssueDate == '0001-01-01T00:00:00') {
                    $scope.Policy.DLIssueDate = "";
                }
                else {
                    $scope.Policy.DLIssueDate = data.policyDetails.policyDetails.customer.idIssueDate;
                } 
                $scope.Policy.AddnSerialNo = data.policyDetails.policyDetails.product.additionalSerial;
                $scope.Policy.Address1 = data.policyDetails.policyDetails.customer.address1;
                $scope.Policy.Address2 = data.policyDetails.policyDetails.customer.address2;
                $scope.Policy.Address3 = data.policyDetails.policyDetails.customer.address3;
                $scope.Policy.Address4 = data.policyDetails.policyDetails.customer.address4;
                $scope.Policy.Aspiration = data.policyDetails.policyDetails.product.aspirationType;
                $scope.Policy.BodyType = data.policyDetails.policyDetails.product.bodyType;
                $scope.Policy.BusinessAddress1 = data.policyDetails.policyDetails.customer.businessAddress1;
                $scope.Policy.BusinessAddress2 = data.policyDetails.policyDetails.customer.businessAddress2;
                $scope.Policy.BusinessAddress3 = data.policyDetails.policyDetails.customer.businessAddress3;
                $scope.Policy.BusinessAddress4 = data.policyDetails.policyDetails.customer.businessAddress4;
                $scope.Policy.BusinessName = data.policyDetails.policyDetails.customer.businessName;
                $scope.Policy.BusinessTelNo = data.policyDetails.policyDetails.customer.businessTelNo;
                $scope.Policy.Category = data.policyDetails.policyDetails.product.category;
                $scope.Policy.City = data.policyDetails.policyDetails.customer.city;
                $scope.Policy.Comment = data.policyDetails.policyDetails.payment.comment;
                $scope.Policy.CommodityType = data.policyDetails.policyDetails.product.commodityType;
                $scope.Policy.Country = data.policyDetails.policyDetails.customer.country;
                $scope.Policy.CustomerPayment = data.policyDetails.policyDetails.payment.customerPayment;
                $scope.Policy.CustomerPaymentCurrencyType = data.policyDetails.policyDetails.product.customerPaymentCurrencyType;
                $scope.Policy.CustomerType = data.policyDetails.policyDetails.customer.customerType;

                if ($scope.Policy.CustomerType == 'Corporate') {

                    $scope.selectedCustomerTypeName = $scope.Policy.CustomerType;
                } else {
                    $scope.selectedCustomerTypeName = '';
                }

                $scope.Policy.CylinderCount = data.policyDetails.policyDetails.product.cylinderCount;
                $scope.Policy.Dealer = data.policyDetails.policyDetails.product.dealer;
                $scope.Policy.DealerLocation = data.policyDetails.policyDetails.product.dealerLocation;
                $scope.Policy.DealerPayment = data.policyDetails.policyDetails.payment.dealerPayment;
                $scope.Policy.DealerPaymentCurrencyType = data.policyDetails.policyDetails.product.dealerPaymentCurrencyType;
                $scope.Policy.DealerPrice = data.policyDetails.policyDetails.product.dealerPrice;
                $scope.Policy.DriveType = "";
                $scope.Policy.Email = data.policyDetails.policyDetails.customer.email;
                $scope.Policy.EngineCapacity = data.policyDetails.policyDetails.product.engineCapacity;
                $scope.Policy.FirstName = data.policyDetails.policyDetails.customer.firstName;
                $scope.Policy.FuelType = data.policyDetails.policyDetails.product.fuelType;
                $scope.Policy.Gender = data.policyDetails.policyDetails.customer.gender;
                $scope.Policy.HrsUsedAtPolicySale = data.policyDetails.policyDetails.policy.hrsUsedAtPolicySale;
                $scope.Policy.IDNo = data.policyDetails.policyDetails.customer.idNo;
                $scope.Policy.IdType = data.policyDetails.policyDetails.customer.idType;
                $scope.Policy.InvoiceNo = data.policyDetails.policyDetails.product.invoiceNo;
                $scope.Policy.PartialPayment = data.policyDetails.policyDetails.payment.isPartialPayment;
                $scope.Policy.SpecialDeal = data.policyDetails.policyDetails.payment.isSpecialDeal;
                if ($scope.Policy.SpecialDeal == true) {
                    $scope.Policy.SpecialDeal = "Yes";
                } else {
                    $scope.Policy.SpecialDeal = "No";
                }
                $scope.Policy.ItemPrice = data.policyDetails.policyDetails.product.itemPrice;
                $scope.Policy.ItemPurchasedDate = data.policyDetails.policyDetails.product.itemPurchasedDate;
                $scope.Policy.ItemStatus = data.policyDetails.policyDetails.product.itemStatus;
                $scope.Policy.LastName = data.policyDetails.policyDetails.customer.lastName;
                $scope.Policy.Make = data.policyDetails.policyDetails.product.make;
                $scope.Policy.MobileNo = data.policyDetails.policyDetails.customer.mobileNo;
                $scope.Policy.Model = data.policyDetails.policyDetails.product.model;
                $scope.Policy.ModelYear = data.policyDetails.policyDetails.product.modelYear;
                $scope.Policy.Nationality = data.policyDetails.policyDetails.customer.nationality;
                $scope.Policy.OtherTelNo = data.policyDetails.policyDetails.customer.otherTelNo;
                $scope.Policy.PaymentMode = data.policyDetails.policyDetails.payment.paymentMode;
                $scope.Policy.PlateNo = data.policyDetails.policyDetails.product.additionalSerial;
                $scope.Policy.PolicySoldDate = data.policyDetails.policyDetails.policy.policySoldDate;
                $scope.Policy.Premium = data.policyDetails.policyDetails.policy.premium;
                $scope.Policy.PremiumCurrencyType = data.policyDetails.policyDetails.policy.productContracts[0].PremiumCurrencyName;
                $scope.Policy.Product = data.policyDetails.policyDetails.product.product;
                $scope.Policy.RefNo = data.policyDetails.policyDetails.payment.refNo;
                $scope.Policy.SalesPerson = data.policyDetails.policyDetails.policy.salesPerson;
                $scope.Policy.SerialNo = data.policyDetails.policyDetails.product.serialNumber;
                $scope.Policy.Transmission = data.policyDetails.policyDetails.product.transmissionType;
                $scope.Policy.UsageType = data.policyDetails.policyDetails.customer.usageType;
                $scope.Policy.VariantName = data.policyDetails.policyDetails.product.variant;
                $scope.Policy.VehiclePrice = data.policyDetails.policyDetails.product.itemPrice;
                $scope.Policy.VINNo = data.policyDetails.policyDetails.product.serialNumber;
                $scope.ProductContracts = data.policyDetails.policyDetails.policy.productContracts;


                $scope.ProductContractsE = data.endorsementDetails.policyDetails.policy.productContracts;


                $scope.Endorsement.AddnSerialNo = data.endorsementDetails.policyDetails.product.additionalSerial;
                $scope.Endorsement.Address1 = data.endorsementDetails.policyDetails.customer.address1;
                $scope.Endorsement.Address2 = data.endorsementDetails.policyDetails.customer.address2;
                $scope.Endorsement.Address3 = data.endorsementDetails.policyDetails.customer.address3;
                $scope.Endorsement.Address4 = data.endorsementDetails.policyDetails.customer.address4;
                $scope.Endorsement.Aspiration = data.endorsementDetails.policyDetails.product.aspirationType;
                $scope.Endorsement.BodyType = data.endorsementDetails.policyDetails.product.bodyType;
                $scope.Endorsement.BusinessAddress1 = data.endorsementDetails.policyDetails.customer.businessAddress1;
                $scope.Endorsement.BusinessAddress2 = data.endorsementDetails.policyDetails.customer.businessAddress2;
                $scope.Endorsement.BusinessAddress3 = data.endorsementDetails.policyDetails.customer.businessAddress3;
                $scope.Endorsement.BusinessAddress4 = data.endorsementDetails.policyDetails.customer.businessAddress4;
                $scope.Endorsement.BusinessName = data.endorsementDetails.policyDetails.customer.businessName;
                $scope.Endorsement.BusinessTelNo = data.endorsementDetails.policyDetails.customer.businessTelNo;
                $scope.Endorsement.Category = data.endorsementDetails.policyDetails.product.category;
                $scope.Endorsement.City = data.endorsementDetails.policyDetails.customer.city;
                $scope.Endorsement.Comment = data.endorsementDetails.policyDetails.payment.comment;
                $scope.Endorsement.CommodityType = data.endorsementDetails.policyDetails.product.commodityType;
                $scope.Endorsement.Country = data.endorsementDetails.policyDetails.customer.country;
                $scope.Endorsement.CustomerPayment = data.endorsementDetails.policyDetails.payment.customerPayment;
                $scope.Endorsement.CustomerPaymentCurrencyType = data.endorsementDetails.policyDetails.product.customerPaymentCurrencyType;
                $scope.Endorsement.CustomerType = data.endorsementDetails.policyDetails.customer.customerType;
                $scope.Endorsement.CylinderCount = data.endorsementDetails.policyDetails.product.cylinderCount;

                if (data.endorsementDetails.policyDetails.customer.dateOfBirth == '1753-01-01T00:00:00') {
                    $scope.Endorsement.DateOfBirth = "";
                }
                else {
                    $scope.Endorsement.DateOfBirth = data.endorsementDetails.policyDetails.customer.dateOfBirth;
                }
                if (data.endorsementDetails.policyDetails.customer.idIssueDate == '1753-01-01T00:00:00') {
                    $scope.Endorsement.DLIssueDate = "";
                }
                else {
                    $scope.Endorsement.DLIssueDate = data.endorsementDetails.policyDetails.customer.idIssueDate;
                }
                
                
                $scope.Endorsement.Dealer = data.endorsementDetails.policyDetails.product.dealer;
                $scope.Endorsement.DealerLocation = data.endorsementDetails.policyDetails.product.dealerLocation;
                $scope.Endorsement.DealerPayment = data.endorsementDetails.policyDetails.payment.dealerPayment;
                $scope.Endorsement.DealerPaymentCurrencyType = data.endorsementDetails.policyDetails.product.dealerPaymentCurrencyType;
                $scope.Endorsement.DealerPrice = data.endorsementDetails.policyDetails.product.dealerPrice;
                $scope.Endorsement.DriveType = "";
                $scope.Endorsement.Email = data.endorsementDetails.policyDetails.customer.email;
                $scope.Endorsement.EngineCapacity = data.endorsementDetails.policyDetails.product.engineCapacity;
                $scope.Endorsement.FirstName = data.endorsementDetails.policyDetails.customer.firstName;
                $scope.Endorsement.FuelType = data.endorsementDetails.policyDetails.product.fuelType;
                $scope.Endorsement.Gender = data.endorsementDetails.policyDetails.customer.gender;
                $scope.Endorsement.HrsUsedAtPolicySale = data.endorsementDetails.policyDetails.policy.hrsUsedAtPolicySale;
                $scope.Endorsement.IDNo = data.endorsementDetails.policyDetails.customer.idNo;
                $scope.Endorsement.IdType = data.endorsementDetails.policyDetails.customer.idType;
                $scope.Endorsement.InvoiceNo = data.endorsementDetails.policyDetails.product.invoiceNo;
                $scope.Endorsement.PartialPayment = data.endorsementDetails.policyDetails.payment.isPartialPayment;
                $scope.Endorsement.SpecialDeal = data.endorsementDetails.policyDetails.payment.isSpecialDeal;
                if ($scope.Endorsement.SpecialDeal == true) {
                    $scope.Endorsement.SpecialDeal = "Yes"
                } else {
                    $scope.Endorsement.SpecialDeal = "No"
                }
                $scope.Endorsement.ItemPrice = data.endorsementDetails.policyDetails.product.itemPrice;
                $scope.Endorsement.ItemPurchasedDate = data.endorsementDetails.policyDetails.product.itemPurchasedDate;
                $scope.Endorsement.ItemStatus = data.endorsementDetails.policyDetails.product.itemStatus;
                $scope.Endorsement.LastName = data.endorsementDetails.policyDetails.customer.lastName;
                $scope.Endorsement.Make = data.endorsementDetails.policyDetails.product.make;
                $scope.Endorsement.MobileNo = data.endorsementDetails.policyDetails.customer.mobileNo;
                $scope.Endorsement.Model = data.endorsementDetails.policyDetails.product.model;
                $scope.Endorsement.ModelYear = data.endorsementDetails.policyDetails.product.modelYear;
                $scope.Endorsement.Nationality = data.endorsementDetails.policyDetails.customer.nationality;
                $scope.Endorsement.OtherTelNo = data.endorsementDetails.policyDetails.customer.otherTelNo;
                $scope.Endorsement.PaymentMode = data.endorsementDetails.policyDetails.payment.paymentMode;
                $scope.Endorsement.PlateNo = data.endorsementDetails.policyDetails.product.additionalSerial;
                $scope.Endorsement.PolicySoldDate = data.endorsementDetails.policyDetails.policy.policySoldDate;
                $scope.Endorsement.Premium = data.endorsementDetails.policyDetails.policy.premium;
                $scope.Endorsement.PremiumCurrencyType = data.endorsementDetails.policyDetails.policy.productContracts[0].PremiumCurrencyName;
                $scope.Endorsement.Product = data.endorsementDetails.policyDetails.product.product;
                $scope.Endorsement.RefNo = data.endorsementDetails.policyDetails.payment.refNo;
                $scope.Endorsement.SalesPerson = data.endorsementDetails.policyDetails.policy.salesPerson;
                $scope.Endorsement.SerialNo = data.endorsementDetails.policyDetails.product.serialNumber;
                $scope.Endorsement.Transmission = data.endorsementDetails.policyDetails.product.transmissionType;
                $scope.Endorsement.UsageType = data.endorsementDetails.policyDetails.customer.usageType;
                $scope.Endorsement.VariantName = data.endorsementDetails.policyDetails.product.variant;
                $scope.Endorsement.VehiclePrice = data.endorsementDetails.policyDetails.product.itemPrice;
               
                if ($scope.Policy.Product == 'ILOE') {
                    $scope.isProductILOE = true;

                }
                
                $scope.Endorsement.ExtensionType = data.endorsementDetails.policyDetails.policy.productContracts[0].ExtensionType[0].ExtensionName;
                $scope.Endorsement.VINNo = data.endorsementDetails.policyDetails.product.serialNumber;
            }).finally(function () {
                swal.close();
            });

        }
    }

    $scope.approveEndorsementRequest = function () {
        if (isGuid($scope.currentPolicyBundleId))
        {
            swal({
                title: "Are you sure?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Approve it!",
                cancelButtonText: "No, Cancel!",
                closeOnConfirm: true,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    swal({ title: 'Processing...!', text: 'Approving endorsement request.', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/ApproveEndorsement',
                        data: { 'Id': $scope.currentPolicyBundleId ,userId: $localStorage.LoggedInUserId},
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    }).success(function (data, status, headers, config) {
                       
                        if (data == "Success")
                        {
                            $scope.resetAll();
                           
                            swal({ title: "TAS Information", text: "Endorsement approved successfully", showConfirmButton: true });
                            
                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                    }).error(function (data, status, headers, config) {
                        swal.close();
                    }).finally(function () {
                       
                    });
                }
            });
        } else {
            customErrorMessage("Please select a Endorsement Request.");
        }
    }
    $scope.rejectEndorsementRequest = function () {
        if (isGuid($scope.currentPolicyBundleId)) {
            swal({
                title: "Are you sure?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Reject it!",
                cancelButtonText: "No, Cancel!",
                closeOnConfirm: true,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    swal({ title: 'Processing...!', text: 'Rejecting endorsement request.', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/RejectEndorsement',
                        data: { 'Id': $scope.currentPolicyBundleId, userId: $localStorage.LoggedInUserId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    }).success(function (data, status, headers, config) {
                        
                        if (data == "Success") {
                            $scope.resetAll();

                            swal({ title: "TAS Information", text: "Endorsement rejected successfully", showConfirmButton: true });

                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                    }).error(function (data, status, headers, config) {
                        swal.close();
                    }).finally(function () {

                    });
                }
            });
        } else {
            customErrorMessage("Please select a Endorsement Request.");
        }
    }
    var customErrorMessage = function (msg) {
        toaster.pop('error', 'Error', msg);
    };

    $scope.resetAll = function () {
        $scope.currentPolicyBundleId = emptyGuid();

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
            IsEndorsementApproved: ""
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
            EndorsementApproved: ""
        }
        $scope.ProductContracts = "";
    }

   
});