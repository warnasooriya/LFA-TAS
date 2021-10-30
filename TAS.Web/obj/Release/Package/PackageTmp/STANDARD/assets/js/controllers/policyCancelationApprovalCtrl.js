
app.controller('PolicyCancelationApprovalCtrl', function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {
    $scope.ModalName = "Policy Cancelation Approval";
    $scope.AutomobileP = true;
    $scope.AutomobileE = true;
    $scope.isProductILOE = false;
    $scope.currentProductTypeCode = '';
    $scope.selectedCustomerTypeName = '';

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
    $scope.GrossTotal = 0.0;
    $scope.errorTab1 = "";
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
                'type': 'forcancellationapproval',
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
        }
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
          { name: 'Policy No', field: 'PolicyNo',width:'30%', enableSorting: false, cellClass: 'columCss', },
          { name: 'Vin or Serial', field: 'SerialNo',width:'20%', enableSorting: false, cellClass: 'columCss' },
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
            swal({ title: "TAS Information", text: "Reading policy cancellation request information...", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetAllPolicyCancellationCommentForApproval',
                data: { "policyBundleId": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.cancelationComment = data;
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetAllPolicyEndorsementDetailsForApproval',
                data: { "Id": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.currentPolicyBundleId = policyId;
                if ($scope.Policy.CommodityType != 'Automobile') {
                    $scope.AutomobileP = false;
                    $scope.AutomobileE = false;
                } else if ($scope.Policy.CommodityType != 'Automotive') {
                    $scope.AutomobileP = false;
                    $scope.AutomobileE = false;
                } else {

                    $scope.AutomobileP = true;
                    $scope.AutomobileE = true;
                }
                $scope.ProductContracts = data.policyDetails.policyDetails.policy.productContracts;
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

                if ($scope.Policy.CustomerType == "Corporate") {
                    $scope.selectedCustomerTypeName = data.policyDetails.policyDetails.customer.customerType;
                } else {
                    $scope.selectedCustomerTypeName = "";
                }

                $scope.Policy.CylinderCount = data.policyDetails.policyDetails.product.cylinderCount;
                $scope.Policy.DateOfBirth = data.policyDetails.policyDetails.customer.dateOfBirth;
                $scope.Policy.Dealer = data.policyDetails.policyDetails.product.dealer;
                $scope.Policy.DealerLocation = data.policyDetails.policyDetails.product.dealerLocation;
                $scope.Policy.DealerPayment = data.policyDetails.policyDetails.payment.dealerPayment;
                $scope.Policy.DealerPaymentCurrencyType = data.policyDetails.policyDetails.product.dealerPaymentCurrencyType;
                $scope.Policy.DealerPrice = data.policyDetails.policyDetails.product.dealerPrice;
                $scope.Policy.DLIssueDate = data.policyDetails.policyDetails.customer.idIssueDate;
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
                $scope.currentProductTypeCode = data.policyDetails.policyDetails.product.productTypeCode;

                if ($scope.currentProductTypeCode == 'ILOE') {
                    $scope.isProductILOE = true;

                }

            }).finally(function () {
                swal.close();
            });

        }
    }

    $scope.policyCancellationApprove = function () {
        if (isGuid($scope.currentPolicyBundleId)) {
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
                    swal({ title: "TAS Information", text: "Approving Policy cancellation Request..", showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/PolicyCancellationApproval',
                        data: { "policyBundleId": $scope.currentPolicyBundleId, "userId": $localStorage.LoggedInUserId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == 'Success') {
                            $scope.policySaveStatusTitle = "Success!";
                            $scope.policySaveStatusMsg = "Policy Cancellation Approved successfully.";
                            $scope.policySaveStatusConfirmButtons = true;
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                            $scope.resetAll();

                        } else {
                            $scope.policySaveStatusTitle = "Error!";
                            $scope.policySaveStatusMsg = data;
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        }
                    }).error(function () {
                        swal.close();
                    });
                }
            })
        }
        else {
            customErrorMessage("Please select a policy cancellation request.");
        }
    }

    $scope.policyCancellationReject = function () {
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
                    swal({ title: "TAS Information", text: "Rejecting Policy cancellation Request..", showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/PolicyCancellationReject',
                        data: { "policyBundleId": $scope.currentPolicyBundleId, "userId": $localStorage.LoggedInUserId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == 'Success') {
                            $scope.policySaveStatusTitle = "Success!";
                            $scope.policySaveStatusMsg = "Policy Cancellation Rejected successfully.";
                            $scope.policySaveStatusConfirmButtons = true;
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                            $scope.resetAll();

                        } else {
                            $scope.policySaveStatusTitle = "Error!";
                            $scope.policySaveStatusMsg = data;
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        }
                    }).error(function () {
                        swal.close();
                    });
                }
            })
        }
        else {
            customErrorMessage("Please select a policy cancellation request.");
        }
    }


    var customErrorMessage = function (msg) {
        toaster.pop('error', 'Error', msg);
    };

    $scope.resetAll = function () {

        $scope.currentPolicyBundleId = emptyGuid();
        $scope.cancelationComment = '';
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
        $scope.GrossTotal = 0.0;
        $scope.errorTab1 = "";
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
    }

});



