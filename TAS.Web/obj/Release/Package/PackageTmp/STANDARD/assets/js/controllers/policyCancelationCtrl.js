app.controller('PolicyCancelationCtrl', function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {
    $scope.ModalName = "Policy Cancellation";
    $scope.AutomobileP = true;
    $scope.AutomobileE = true;
    $scope.selectedCustomerTypeName = '';
    $scope.isProductILOE = false;
    $scope.currentProductCode = '';
    $scope.currentProductTypeCode = '';
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
    //-----------------Hard Code Combo: Load from DB when available-------------------------------------//
    // $scope.PaymentModes = [{ Id: "b8fa922a-b294-4673-9ff8-10e35488253f", PaymentMode: "Check" }, { Id: "ff07bf9d-660f-4750-80d2-539b1d74f9f5", PaymentMode: "Pay Pal" }, { Id: "15b8a9d8-939d-4d1d-8c51-2dabe9057268", PaymentMode: "Credit Card" }];
    //--------------------------------------------------------------------------------------------------//
    //LoadDetails();
    //function LoadDetails() {
    //    $http({
    //        method: 'POST',
    //        url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
    //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //}).success(function (data, status, headers, config) {
    //        $scope.CommodityTypes = data;
    //        $rootScope.policySearch = {
    //            CommodityType: '',
    //            PolicyNo: '',
    //            SerialNo: '',
    //            MobileNo: '',
    //            vinSerialName: 'VIN No/Serial No',
    //            StartDate: '',
    //            EndDate: ''
    //        };
    //        $rootScope.policySearch.CommodityType = $scope.CommodityTypes[0].CommodityTypeDescription;
    //    }).error(function (data, status, headers, config) {
    //    });
    //    $http({
    //        method: 'POST',
    //        url: '/TAS.Web/api/PolicyReg/GetPoliciesForCancelation',
    //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //}).success(function (data, status, headers, config) {
    //        $scope.Policies = data;
    //    }).error(function (data, status, headers, config) {
    //    });
    //    $http({
    //        method: 'POST',
    //        url: '/TAS.Web/api/Customer/GetAllCustomers',
    //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //}).success(function (data, status, headers, config) {
    //        $scope.Customers = data;
    //    }).error(function (data, status, headers, config) {
    //    });
    //    $http({
    //        method: 'POST',
    //        url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
    //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //}).success(function (data, status, headers, config) {
    //        $scope.Vehicles = data;
    //    }).error(function (data, status, headers, config) {
    //    });
    //    $http({
    //        method: 'POST',
    //        url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllBrownAndWhiteDetails',
    //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //}).success(function (data, status, headers, config) {
    //        $scope.BAndWs = data;
    //    }).error(function (data, status, headers, config) {
    //    });
    //}



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
                'type': 'forcancellation',
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
          { name: 'Policy No', field: 'PolicyNo',  width:'30%', enableSorting: false, cellClass: 'columCss', },
          { name: 'Vin or Serial', field: 'SerialNo',  width:'20%', enableSorting: false, cellClass: 'columCss' },
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



            swal({ title: "TAS Information", text: "Reading Policy Information..", showConfirmButton: false });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetAllPolicyEndorsementDetailsForApproval',
                data: { "Id": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.currentPolicyBundleId = policyId;
                if (data.policyDetails.policyDetails.product.commodityType == "Automobile") {
                    $scope.AutomobileP = true;
                    $scope.AutomobileE = false;
                } else if (data.policyDetails.policyDetails.product.commodityType == "Automotive") {
                    $scope.AutomobileP = true;
                    $scope.AutomobileE = false;
                } else {

                    $scope.AutomobileP = false;
                    $scope.AutomobileE = true;
                }
                $scope.currentProductTypeCode = data.policyDetails.policyDetails.product.productTypeCode;
                $scope.currentProductCode = data.policyDetails.policyDetails.product.commodityType;
                if ($scope.currentProductCode === 'TYRE') {

                            $scope.isProductILOE = false;

                } else if ($scope.currentProductTypeCode === 'ILOE') {
                            $scope.isProductILOE = true;
                } else {

                            $scope.isProductILOE = false;
                        }
                $scope.ProductContracts = data.policyDetails.policyDetails.policy.productContracts;
                angular.forEach($scope.ProductContracts, function (value) {
                    $scope.ProductContract.PremiumCurrencyName = value.PremiumCurrencyName;
                });
               
                //alert(data.policyDetails.policyDetails.product.additionalSerial);
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
                    $scope.selectedCustomerTypeName = data.policyDetails.policyDetails.customer.customerType;
                } else {
                    $scope.selectedCustomerTypeName = '';
                }
                
                $scope.Policy.CylinderCount = data.policyDetails.policyDetails.product.cylinderCount;
                $scope.Policy.DateOfBirth = data.policyDetails.policyDetails.customer.dateOfBirth;
                $scope.Policy.Dealer = data.policyDetails.policyDetails.product.dealer;
                $scope.Policy.DealerLocation = data.policyDetails.policyDetails.product.dealerLocation;
                $scope.Policy.DealerPayment = data.policyDetails.policyDetails.payment.dealerPayment;
                $scope.Policy.DealerPaymentCurrencyType = data.policyDetails.policyDetails.product.dealerPaymentCurrencyType;
                $scope.Policy.DealerPrice = data.policyDetails.policyDetails.product.dealerPrice;
                $scope.Policy.DLIssueDate = data.policyDetails.policyDetails.customer.idIssueDate;
                
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
                $scope.Policy.SpecialDeal = (data.policyDetails.policyDetails.payment.isSpecialDeal == "true") ? "Yes" : "No";
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

                $scope.Policy.DriveType = data.policyDetails.policyDetails.product.driveType;
                $scope.Policy.Product = data.policyDetails.policyDetails.product.product;
                $scope.Policy.RefNo = data.policyDetails.policyDetails.payment.refNo;
                $scope.Policy.SalesPerson = data.policyDetails.policyDetails.policy.salesPerson;
                $scope.Policy.SerialNo = data.policyDetails.policyDetails.product.serialNumber;
                $scope.Policy.Transmission = data.policyDetails.policyDetails.product.transmissionType;
                $scope.Policy.UsageType = data.policyDetails.policyDetails.customer.usageType;
                $scope.Policy.VariantName = data.policyDetails.policyDetails.product.variant;
                $scope.Policy.VehiclePrice = data.policyDetails.policyDetails.product.itemPrice;
                $scope.Policy.VINNo = data.policyDetails.policyDetails.product.serialNumber;
                $scope.Policy.PremiumCurrencyType = data.policyDetails.policyDetails.policy.productContracts[0].PremiumCurrencyName;
                
            }).finally(function () {
                swal.close();
            });


        }
    }

    $scope.policyCancellation = function () {
        if (isGuid($scope.currentPolicyBundleId)) {
            // alert($scope.cancelationComment);
            if (typeof $scope.cancelationComment === 'undefined' || $scope.cancelationComment == '') {
                customErrorMessage("Please enter comment to cancel this policy");
            } else {
                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Request it!",
                    cancelButtonText: "No, Cancel!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {

                        swal({ title: "TAS Information", text: "Requesting Policy cancellation..", showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/PolicyReg/PolicyCancellationRequest',
                            data: { "policyBundleId": $scope.currentPolicyBundleId, "comment": $scope.cancelationComment, "userId": $localStorage.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'success') {
                                $scope.policySaveStatusTitle = "Success!";
                                $scope.policySaveStatusMsg = "Policy Cancellation Requested successfully.";
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
                });
            }
        }
        else {
            customErrorMessage("Please select a policy");
        }
    }
    var customErrorMessage = function (msg) {
        toaster.pop('error', 'Error', msg);
    };

    $scope.selectedProdctChanged = function () {
        if (isGuid($scope.ProductContract.productId)) {
            //$scope.isProductDetailsReadonly = false;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllChildProducts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.product.productId }
            }).success(function (data, status, headers, config) {
                $scope.productContracts = [];
                $scope.currentProductCode = data[0].Productcode; //for ui change for tyre product
                $scope.currentProductTypeCode = data[0].ProductTypeCode;
                if ($scope.currentProductCode === 'TYRE') {
                    
                    $scope.isProductILOE = false;
                    
                } else if ($scope.currentProductTypeCode === 'ILOE') {
                    $scope.isProductILOE = true;
                } else {
                
                    $scope.isProductILOE = false;
                }
                


            }).error(function (data, status, headers, config) {
            });

          

        } else {
           
        }
    }


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

    //function SetPolicyValues() {
    //    document.getElementById("loading").style.display = "none";
    //    $scope.errorTab1 = "";
    //    $scope.myPolicySelectedRows = $scope.gridApiPolicy.selection.getSelectedRows();
    //    angular.forEach($scope.myPolicySelectedRows, function (value, key) {
    //        angular.forEach($scope.Policies, function (val) {
    //            if (value.Id == val.Id) {
    //                $http({
    //                    method: 'POST',
    //                    url: '/TAS.Web/api/PolicyReg/GetPolicyById',
    //                    data: { "Id": val.Id },
    //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //            }).success(function (data, status, headers, config) {
    //                    $scope.Policy = data;
    //                    $scope.Policy.PolicyId = $scope.Policy.Id;                       
    //                    $scope.Policy.Id = "00000000-0000-0000-0000-000000000000";
    //                    $scope.ProductContracts = data.ContractProducts;
    //                    angular.forEach($scope.ProductContracts, function (value) {
    //                        if (value.RSA)
    //                            value.CoverTypes = value.RSAProviders;
    //                        $scope.GrossTotal = $scope.GrossTotal + value.Premium;
    //                    });
    //                    $scope.GrossTotal = parseFloat($scope.GrossTotal).toFixed(2);
    //                    if ($scope.Policy.IsSpecialDeal)
    //                        $scope.Policy.SpecialDeal = "Yes";
    //                    else
    //                        $scope.Policy.SpecialDeal = "No";
    //                    angular.forEach($scope.Customers, function (valueC) {
    //                        if ($scope.Policy.CustomerId == valueC.Id) {
    //                            $scope.Policy.CustomerId = valueC.Id;
    //                            $scope.Policy.NationalityId = valueC.NationalityId;
    //                            $scope.Policy.CountryId = valueC.CountryId;
    //                            $scope.Policy.MobileNo = valueC.MobileNo;
    //                            $scope.Policy.OtherTelNo = valueC.OtherTelNo;
    //                            $scope.Policy.FirstName = valueC.FirstName;
    //                            $scope.Policy.LastName = valueC.LastName;
    //                            $scope.Policy.DateOfBirth = valueC.DateOfBirth;
    //                            $scope.Policy.Email = valueC.Email;
    //                            $scope.Policy.CustomerTypeId = valueC.CustomerTypeId;
    //                            $scope.Policy.UsageTypeId = valueC.UsageTypeId;
    //                            $scope.Policy.CityId = valueC.CityId;
    //                            $scope.Policy.Email = valueC.Email;
    //                            $scope.Policy.Address1 = valueC.Address1;
    //                            $scope.Policy.Address2 = valueC.Address2;
    //                            $scope.Policy.Address3 = valueC.Address3;
    //                            $scope.Policy.Address4 = valueC.Address4;
    //                            $scope.Policy.IDNo = valueC.IDNo;
    //                            $scope.Policy.IDTypeId = valueC.IDTypeId;
    //                            $scope.Policy.DLIssueDate = valueC.DLIssueDate;
    //                            $scope.Policy.Gender = valueC.Gender;
    //                            $scope.Policy.BusinessName = valueC.BusinessName;
    //                            $scope.Policy.BusinessAddress1 = valueC.BusinessAddress1;
    //                            $scope.Policy.BusinessAddress2 = valueC.BusinessAddress2;
    //                            $scope.Policy.BusinessAddress3 = valueC.BusinessAddress3;
    //                            $scope.Policy.BusinessAddress4 = valueC.BusinessAddress4;
    //                            $scope.Policy.BusinessTelNo = valueC.BusinessTelNo;
    //                        }
    //                    });
    //                    if ($scope.Policy.Type == "Vehicle") {
    //                        angular.forEach($scope.Vehicles, function (valueV) {
    //                            if ($scope.Policy.ItemId == valueV.Id) {
    //                                $scope.Policy.VehicleId = valueV.Id;
    //                                $scope.Policy.VINNo = valueV.VINNo;
    //                                $scope.Policy.MakeId = valueV.MakeId;
    //                                $scope.Policy.ModelId = valueV.ModelId;
    //                                $scope.Policy.CategoryId = valueV.CategoryId;
    //                                $scope.Policy.ItemStatusId = valueV.ItemStatusId;
    //                                $scope.Policy.CylinderCountId = valueV.CylinderCountId;
    //                                $scope.Policy.BodyTypeId = valueV.BodyTypeId;
    //                                $scope.Policy.PlateNo = valueV.PlateNo;
    //                                $scope.Policy.ModelYear = valueV.ModelYear;
    //                                $scope.Policy.FuelTypeId = valueV.FuelTypeId;
    //                                $scope.Policy.AspirationId = valueV.AspirationId;
    //                                $scope.Policy.Variant = valueV.Variant;
    //                                $scope.Policy.TransmissionId = valueV.TransmissionId;
    //                                $scope.Policy.ItemPurchasedDate = valueV.ItemPurchasedDate;
    //                                $scope.Policy.EngineCapacityId = valueV.EngineCapacityId;
    //                                $scope.Policy.DriveTypeId = valueV.DriveTypeId;
    //                                $scope.Policy.VehiclePrice = valueV.VehiclePrice;
    //                                $scope.Policy.DealerPrice = valueV.DealerPrice;

    //                            }
    //                        });
    //                    }
    //                    else {
    //                        angular.forEach($scope.BAndWs, function (valueB) {
    //                            if ($scope.Policy.ItemId == valueB.Id) {
    //                                $scope.Policy.BAndWId = valueB.Id;
    //                                $scope.Policy.ItemPurchasedDate = valueB.ItemPurchasedDate;
    //                                $scope.Policy.MakeId = valueB.MakeId;
    //                                $scope.Policy.ModelId = valueB.ModelId;
    //                                $scope.Policy.SerialNo = valueB.SerialNo;
    //                                $scope.Policy.ItemPrice = valueB.ItemPrice;
    //                                $scope.Policy.CategoryId = valueB.CategoryId;
    //                                $scope.Policy.ModelYear = valueB.ModelYear;
    //                                $scope.Policy.AddnSerialNo = valueB.AddnSerialNo;
    //                                $scope.Policy.ItemStatusId = valueB.ItemStatusId;
    //                                $scope.Policy.InvoiceNo = valueB.InvoiceNo;
    //                                $scope.Policy.ModelCode = valueB.ModelCode;
    //                                $scope.Policy.DealerPrice = valueB.DealerPrice;
    //                            }
    //                        });
    //                    }
    //                    angular.forEach($scope.CommodityTypes, function (cval) {
    //                        if (cval.CommodityTypeId == $scope.Policy.CommodityTypeId) {
    //                            $scope.Policy.CommodityType = cval.CommodityTypeDescription;
    //                            if ($scope.Policy.CommodityType == "Automobile")
    //                                $scope.AutomobileP = true;
    //                            else
    //                                $scope.AutomobileP = false;
    //                        }
    //                    });
    //                    if ($scope.Policy.ProductId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ProductId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Product/GetProductById',
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
    //                            data: { "productId": $scope.Policy.ProductId }
    //                        }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Product = data.Productcode + ' - ' + data.Productname;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.DealerId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.DealerId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/DealerManagement/GetDealerById',
    //                            data: { "Id": $scope.Policy.DealerId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Dealer = data.DealerName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.DealerLocationId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.DealerLocationId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationById',
    //                            data: { "Id": $scope.Policy.DealerLocationId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.DealerLocation = data.Location;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    //if ($scope.Policy.ContractId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ContractId != undefined) {
    //                    //    $http({
    //                    //        method: 'POST',
    //                    //        url: '/TAS.Web/api/ContractManagement/GetContractId',
    //                    //        data: { "Id": $scope.Policy.ContractId }
    //                    //    }).success(function (data, status, headers, config) {
    //                    //        $scope.Policy.Contract = data.Contract.DealName;
    //                    //    }).error(function (data, status, headers, config) {
    //                    //    });
    //                    //}
    //                    //if ($scope.Policy.ExtensionTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ExtensionTypeId != undefined) {
    //                    //    $http({
    //                    //        method: 'POST',
    //                    //        url: '/TAS.Web/api/ContractManagement/GetExtensionTypeById',
    //                    //        data: { "Id": $scope.Policy.ExtensionTypeId }
    //                    //    }).success(function (data, status, headers, config) {
    //                    //        $scope.Policy.ExtensionType = data.ExtensionName;
    //                    //    }).error(function (data, status, headers, config) {
    //                    //    });
    //                    //}
    //                    //if ($scope.Policy.PremiumCurrencyTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.PremiumCurrencyTypeId != undefined) {
    //                    //    $http({
    //                    //        method: 'POST',
    //                    //        url: '/TAS.Web/api/DealerManagement/GetCurrencyById',
    //                    //        data: { "Id": $scope.Policy.PremiumCurrencyTypeId }
    //                    //    }).success(function (data, status, headers, config) {
    //                    //        $scope.Policy.Currency = data.Code;
    //                    //    }).error(function (data, status, headers, config) {
    //                    //    });
    //                    //}
    //                    //if ($scope.Policy.CoverTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CoverTypeId != undefined) {
    //                    //    $http({
    //                    //        method: 'POST',
    //                    //        url: '/TAS.Web/api/ContractManagement/GetWarrantyTypeById',
    //                    //        data: { "Id": $scope.Policy.CoverTypeId }
    //                    //    }).success(function (data, status, headers, config) {
    //                    //        $scope.Policy.CoverType = data.WarrantyTypeDescription;
    //                    //    }).error(function (data, status, headers, config) {
    //                    //    });
    //                    //}
    //                    angular.forEach($scope.ProductContracts, function (value) {
    //                        angular.forEach(value.Contracts, function (valuein) {
    //                            if (value.ContractId == valuein.Id) {
    //                                value.Contract = valuein.DealName;
    //                            }
    //                        });
    //                        angular.forEach(value.ExtensionTypes, function (valuein) {
    //                            if (value.ExtensionTypeId == valuein.Id) {
    //                                value.ExtensionType = valuein.ExtensionName;
    //                            }
    //                        });
    //                        if (value.RSA) {
    //                            angular.forEach(value.RSAProviders, function (valuein) {
    //                                if (value.CoverTypeId == valuein.Id) {
    //                                    value.RSAProvider = valuein.ProviderName;
    //                                }
    //                            });
    //                        }
    //                        else {
    //                            angular.forEach(value.CoverTypes, function (valuein) {
    //                                if (value.CoverTypeId == valuein.Id) {
    //                                    value.CoverType = valuein.WarrantyTypeDescription;
    //                                }
    //                            });
    //                        }
    //                        angular.forEach(value.Contracts, function (valuein) {
    //                            if (value.ContractId == valuein.ContractId) {
    //                                value.Contract = valuein.DealName;
    //                            }
    //                        });

    //                        if (value.PremiumCurrencyTypeId != "00000000-0000-0000-0000-000000000000" && value.PremiumCurrencyTypeId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/DealerManagement/GetCurrencyById',
    //                                data: { "Id": $scope.Policy.PremiumCurrencyTypeId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                value.Currency = data.Code;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                    });
    //                    if ($scope.Policy.SalesPersonId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.SalesPersonId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/User/GetUsersById',
    //                            data: { "Id": $scope.Policy.SalesPersonId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.SalesPerson = data.FirstName + ' ' + data.LastName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    angular.forEach($scope.PaymentModes, function (pval) {
    //                        if (pval.Id == $scope.Policy.PaymentModeId)
    //                            $scope.Policy.PaymentMode = pval.PaymentMode;
    //                    });
    //                    if ($scope.Policy.NationalityId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.NationalityId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Customer/GetNationalityById',
    //                            data: { "Id": $scope.Policy.NationalityId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Nationality = data.NationalityName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.CountryId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CountryId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/ContractManagement/GetCountryById',
    //                            data: { "Id": $scope.Policy.CountryId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Country = data.CountryName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.CustomerTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CustomerTypeId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Customer/GetCustomerTypeById',
    //                            data: { "Id": $scope.Policy.CustomerTypeId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.CustomerType = data.CustomerTypeName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.UsageTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.UsageTypeId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Customer/GetUsageTypeById',
    //                            data: { "Id": $scope.Policy.UsageTypeId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.UsageType = data.UsageTypeName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.IDTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.IDTypeId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Customer/GetIdTypeById',
    //                            data: { "Id": $scope.Policy.IDTypeId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.IdType = data.IdTypeName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.CityId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CityId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/Customer/GetCityById',
    //                            data: { "Id": $scope.Policy.CityId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.City = data.CityName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    $scope.Policy.MobileNo = $scope.Policy.MobileNo;
    //                    $scope.Policy.OtherTelNo = $scope.Policy.OtherTelNo;
    //                    $scope.Policy.FirstName = $scope.Policy.FirstName;
    //                    $scope.Policy.LastName = $scope.Policy.LastName;
    //                    $scope.Policy.DateOfBirth = $scope.Policy.DateOfBirth;
    //                    $scope.Policy.Email = $scope.Policy.Email;
    //                    $scope.Policy.Email = $scope.Policy.Email;
    //                    $scope.Policy.Address1 = $scope.Policy.Address1;
    //                    $scope.Policy.Address2 = $scope.Policy.Address2;
    //                    $scope.Policy.Address3 = $scope.Policy.Address3;
    //                    $scope.Policy.Address4 = $scope.Policy.Address4;
    //                    $scope.Policy.IDNo = $scope.Policy.IDNo;
    //                    $scope.Policy.DLIssueDate = $scope.Policy.DLIssueDate;
    //                    $scope.Policy.Gender = $scope.Policy.Gender;
    //                    $scope.Policy.BusinessName = $scope.Policy.BusinessName;
    //                    $scope.Policy.BusinessAddress1 = $scope.Policy.BusinessAddress1;
    //                    $scope.Policy.BusinessAddress2 = $scope.Policy.BusinessAddress2;
    //                    $scope.Policy.BusinessAddress3 = $scope.Policy.BusinessAddress3;
    //                    $scope.Policy.BusinessAddress4 = $scope.Policy.BusinessAddress4;
    //                    $scope.Policy.BusinessTelNo = $scope.Policy.BusinessTelNo;
    //                    if (val.VehicleId != "00000000-0000-0000-0000-000000000000") {
    //                        $scope.Policy.VINNo = $scope.Policy.VINNo;
    //                        $scope.Policy.PlateNo = $scope.Policy.PlateNo;
    //                        $scope.Policy.ModelYear = $scope.Policy.ModelYear;
    //                        $scope.Policy.ItemPurchasedDate = $scope.Policy.ItemPurchasedDate;
    //                        $scope.Policy.VehiclePrice = $scope.Policy.VehiclePrice;
    //                        if ($scope.Policy.CylinderCountId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CylinderCountId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetCylinderCountById',
    //                                data: { "cylinderCountId": $scope.Policy.CylinderCountId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.CylinderCount = data.Count;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.DriveTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.DriveTypeId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetDriveTypeById',
    //                                data: { "driveTypeId": $scope.Policy.DriveTypeId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.DriveType = data.Type;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.EngineCapacityId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.EngineCapacityId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetEngineCapacityById',
    //                                data: { "engineCapacityId": $scope.Policy.EngineCapacityId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.EngineCapacity = data.EngineCapacityNumber;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.FuelTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.FuelTypeId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetFuelTypeById',
    //                                data: { "fuelTypeId": $scope.Policy.FuelTypeId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.FuelType = data.FuelTypeDescription;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.BodyTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.BodyTypeId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetVehicleBodyTypeById',
    //                                data: { "vehicleBodyTypeId": $scope.Policy.BodyTypeId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.BodyType = data.VehicleBodyTypeDescription;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.TransmissionId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.TransmissionId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetTransmissionTypeById',
    //                                data: { "TransmissionTypeId": $scope.Policy.TransmissionId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.Transmission = data.TransmissionTypeCode;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.AspirationId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.AspirationId != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/AutomobileAttributes/GetVehicleAspirationTypeById',
    //                                data: { "VehicleAspirationTypeId": $scope.Policy.AspirationId },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.Aspiration = data.AspirationTypeCode;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                        if ($scope.Policy.Variant != "00000000-0000-0000-0000-000000000000" && $scope.Policy.Variant != undefined) {
    //                            $http({
    //                                method: 'POST',
    //                                url: '/TAS.Web/api/VariantManagement/GetVariantById',
    //                                data: { "Id": $scope.Policy.Variant },
    //                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                        }).success(function (data, status, headers, config) {
    //                                $scope.Policy.VariantName = data.VariantName;
    //                            }).error(function (data, status, headers, config) {
    //                            });
    //                        }
    //                    }
    //                    else {
    //                        $scope.Policy.ItemPurchasedDate = $scope.Policy.ItemPurchasedDate;
    //                        $scope.Policy.SerialNo = $scope.Policy.SerialNo;
    //                        $scope.Policy.ModelYear = $scope.Policy.ModelYear;
    //                        $scope.Policy.AddnSerialNo = $scope.Policy.AddnSerialNo;
    //                        $scope.Policy.InvoiceNo = $scope.Policy.InvoiceNo;
    //                        $scope.Policy.ModelCode = $scope.Policy.ModelCode;
    //                        $scope.Policy.DealerPrice = $scope.Policy.DealerPrice;
    //                    }
    //                    if ($scope.Policy.ItemStatusId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ItemStatusId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/CommodityItemAttributes/GetItemStatusById',
    //                            data: { "Id": $scope.Policy.ItemStatusId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.ItemStatus = data.Status;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.MakeId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.MakeId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/MakeAndModelManagement/GetMakeById',
    //                            data: { "Id": $scope.Policy.MakeId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Make = data.MakeName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ModelId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelById',
    //                            data: { "Id": $scope.Policy.ModelId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Model = data.ModelName;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                    if ($scope.Policy.CategoryId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.CategoryId != undefined) {
    //                        $http({
    //                            method: 'POST',
    //                            url: '/TAS.Web/api/CommodityItemAttributes/GetCommodityCategoryById',
    //                            data: { "CommodityCategoryId": $scope.Policy.CategoryId, "CommodityTypeId": $scope.Policy.CommodityTypeId },
    //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //                    }).success(function (data, status, headers, config) {
    //                            $scope.Policy.Category = data.CommodityCategoryDescription;
    //                        }).error(function (data, status, headers, config) {
    //                        });
    //                    }
    //                }).error(function (data, status, headers, config) {
    //                });
    //            }
    //        });
    //    });
    //}
    ////--------------------Other Methods------------------------------------------//  
    //$scope.Submit = function () {
    //    $scope.Policy.ModifiedUser = $localStorage.LoggedInUserId;
    //    $scope.Policy.Id = "00000000-0000-0000-0000-000000000000";
    //    if ($scope.Policy.CancelationComment == "" || $scope.Policy.CancelationComment == undefined)
    //        return false
    //    else {
    //        SweetAlert.swal({
    //            title: "Policy Cancelation",
    //            text: "Please Conform Policy Cancelation!",
    //            confirmButtonColor: "#007AFF",
    //            type: "warning",
    //            confirmButtonColor: "rgb(221, 107, 85)",
    //            showCancelButton: true,
    //            confirmButtonClass: "btn-danger",
    //            confirmButtonText: "Approve Policy Cancelation.",
    //            closeOnConfirm: false
    //        },
    //        function (isConfirm) {
    //            if (isConfirm) {
    //                document.getElementById("loading").style.display = "block";
    //                $http({
    //                    method: 'POST',
    //                    url: '/TAS.Web/api/PolicyReg/PolicyCancelation',
    //                    data: $scope.Policy,
    //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
    //            }).success(function (data, status, headers, config) {
    //                    $scope.Ok = data;
    //                    if (data == "OK") {
    //                        SweetAlert.swal({
    //                            title: "Policy Cancelation",
    //                            text: "Successfully Saved and Canceled!",
    //                            confirmButtonColor: "#007AFF"
    //                        });
    //                        clearPolicyControls();
    //                    }
    //                    return false;
    //                }).error(function (data, status, headers, config) {
    //                    SweetAlert.swal({
    //                        title: "Policy Cancelation",
    //                        text: "Error occured while saving data!",
    //                        type: "warning",
    //                        confirmButtonColor: "rgb(221, 107, 85)"
    //                    });
    //                    document.getElementById("loading").style.display = "none";
    //                    return false;
    //                });
    //            }
    //            else {
    //                return false;
    //            }
    //        });
    //    }
    //}
});
