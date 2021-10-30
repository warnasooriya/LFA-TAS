app.controller('PolicyRegCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, toaster, $filter, FileUploader) {
        $scope.ModalName = "Policy Registration";
        $scope.value = true;
        $scope.aa = { bb: "" };
        $scope.customer = { username: "", gender: "M" };
        $scope.validation = { userName: false, password: false };
        $scope.vwVehicleDetails = false;
        $scope.currentProductTypeCode = '';
        $scope.vwBAndWDetails = false;
        $scope.businessInfoDisplay = false;
        $scope.vwPolicyDetails = true;
        $scope.vwPaymentDetails = true;
        $scope.isProductILOE = false;
        $scope.PremiumDissabled = true;
        $scope.DealerCurrencyName = "";
        $scope.PremiumCurrencyName = "";
        $scope.emiContractId = "";
        $scope.EMI = "";
        $scope.VehicleCurrency = "";
        $scope.dealersByCountry = [];
        $scope.Products = [];
        $scope.dealerPrice = "";
        $scope.isPaymentTypesAvailable = false;
        $scope.selectedCustomerTypeName = '';
        $scope.selectedUsageTypeName = '';
        $scope.currentCommodityTypeName = '';
        $scope.OtherItemDetails = false;
        $scope.currentCommodityTypeCode = '';
        $scope.currentProductCode = '';
        $scope.currentProductCodeTire = false;
        $scope.SerialNoV1disable = false;
        $scope.SerialNoV2disable = false;

        $scope.customerAttachmentTypes = [];
        $scope.itemAttachmentTypes = [];
        $scope.policyAttachmentTypes = [];
        $scope.paymentAttachmentTypes = [];
        $scope.PolicyBreakdown = [];
        $scope.uploadedDocIds = [];
        $scope.attachmentData = {};
        $scope.isBackTireDetailsVisibleL = false;
        $scope.isFrontTireDetailsVisibleL = false;
        $scope.frontLeftTyreSizeReadOnly = true;
        $scope.rearLeftTyreSizeReadOnly = true;
        $scope.tireSizesAvailable = false;
        $scope.isInvoiceUploaded = false;
        $scope.invoiceDownloadRef = "";
        $scope.CommodityTypeDesc = "";

        // handle contract & policy words
        if ($localStorage.CommodityType === "Tyre") {
            $scope.comType = "Tyre";
            $scope.plicyWords = {
                "name": "Contract",
                "nameFl": "Contracts",
                "no": "Service Contract No"
            };
            $scope.currentProductCodeTire = true;
            $scope.isDealerPolicy = true;
        } else {
            $scope.plicyWords = {
                "name": "Policy",
                "nameFl": "Policies",
                "no": "Policy No"
            };
        }

        $scope.vehicle = {
            plateNumber: '',
            mileage: '',
            make: '',
            model: '',
            year: '',
            invoiceNo: '',
        }
        $scope.dealerInvoiceTireDetails = {
            front: {
                width: '',
                cross: '',
                diameter: '',
                loadSpeed: '',
                serialLeft: '',
                serialRight: '',
                pattern: '',
                dotLeft: '',
                leftcheckbox: false,
                rightcheckbox: false,
                price: 0
            },
            back: {
                width: '',
                cross: '',
                diameter: '',
                loadSpeed: '',
                serialLeft: '',
                serialRight: '',
                pattern: '',
                dotLeft: '',
                leftcheckbox: false,
                rightcheckbox: false,
                price: 0
            },
            down: {
                width: '',
                cross: '',
                diameter: '',
                loadSpeed: '',
                serialLeft: '',
                serialRight: '',
                pattern: '',
                dotLeft: '',

            }
        };

        var validateEmail = function (email) {
            if (email == "") return false;
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }

        //Scanner Variables
        var uploadUrl = window.location.protocol + '//' + window.location.host + '/TAS.Web/api/Upload/UploadScannerAttachment';
        var DWObject;
        var ScannerPopUp;
        $scope.IsScannedImage = false;
        $scope.tabId = '';
        $scope.ImageArray = [];
        $scope.ImageBatch = {
            documentName: "",
            attachmentType: "",
            page: "",
            section: "",
            encodedImage: []
        };

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
                customErrorMessage("Error occured while uploading attachments.");
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
                        customErrorMessage('Max document size is 3MB');
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage('Please select a attachment type before select a file.');
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
                customErrorMessage("Error occured while uploading attachments.");
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
                        customErrorMessage('Max document size is 3MB');
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage('Please select a attachment type before select a file.');
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
                customErrorMessage("Error occured while uploading attachments.");
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
                        customErrorMessage('Max document size is 3MB');
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage('Please select a attachment type before select a file.');
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
                customErrorMessage("Error occured while uploading attachments.");
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
                        customErrorMessage('Max document size is 3MB');
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage('Please select a attachment type before select a file.');
                    return false;
                }
            }

        });


        $scope.testRepeat = [{ name: "Product 1" }, { name: "Product 2" }]
        $scope.BAndW = {
            Id: "00000000-0000-0000-0000-000000000000",
            ItemPurchasedDate: '',
            MakeId: "00000000-0000-0000-0000-000000000000",
            ModelId: "00000000-0000-0000-0000-000000000000",
            SerialNo: '',
            SerialNoV1: '',
            SerialNoV2: '',
            articleNumberV1: '',
            articleNumberV2: '',
            ItemPrice: 0.0,
            CategoryId: "00000000-0000-0000-0000-000000000000",
            ModelYear: '',
            AddnSerialNo: '',
            ItemStatusId: "00000000-0000-0000-0000-000000000000",
            InvoiceNo: '',
            ModelCode: '',
            DealerPrice: 0.0,
            Variant: "00000000-0000-0000-0000-000000000000",
        };
        $scope.Vehicle = {
            Id: "00000000-0000-0000-0000-000000000000",
            VINNo: '',
            MakeId: "00000000-0000-0000-0000-000000000000",
            ModelId: "00000000-0000-0000-0000-000000000000",
            CategoryId: "00000000-0000-0000-0000-000000000000",
            ItemStatusId: "00000000-0000-0000-0000-000000000000",
            CylinderCountId: "00000000-0000-0000-0000-000000000000",
            BodyTypeId: "00000000-0000-0000-0000-000000000000",
            PlateNo: '',
            ModelYear: '',
            FuelTypeId: "00000000-0000-0000-0000-000000000000",
            AspirationId: "00000000-0000-0000-0000-000000000000",
            Variant: "00000000-0000-0000-0000-000000000000",
            TransmissionId: "00000000-0000-0000-0000-000000000000",
            ItemPurchasedDate: '',
            EngineCapacityId: "00000000-0000-0000-0000-000000000000",
            DriveTypeId: "00000000-0000-0000-0000-000000000000",
            VehiclePrice: 0.0,
            DealerPrice: 0.0,
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            DealerCurrencyId: emptyGuid(),
            currencyPeriodId: emptyGuid(),
            RegistrationDate: '',
            GrossWeight: 0.00
        };
        $scope.Customer = {
            Id: "00000000-0000-0000-0000-000000000000",
            FirstName: '',
            LastName: '',
            NationalityId: '00000000-0000-0000-0000-000000000000',
            DateOfBirth: '',
            CountryId: '00000000-0000-0000-0000-000000000000',
            Gender: '',
            MobileNo: '',
            OtherTelNo: '',
            CustomerTypeId: '00000000-0000-0000-0000-000000000000',
            UsageTypeId: '00000000-0000-0000-0000-000000000000',
            Address1: '',
            Address2: '',
            Address3: '',
            Address4: '',
            IDNo: '',
            IDTypeId: '00000000-0000-0000-0000-000000000000',
            CityId: '00000000-0000-0000-0000-000000000000',
            DLIssueDate: '',
            Email: '',
            BusinessName: '',
            BusinessAddress1: '',
            BusinessAddress2: '',
            BusinessAddress3: '',
            BusinessAddress4: '',
            BusinessTelNo: '',
            Password: '',
            //ProfilePicture: ''
        }
        $scope.Policy = {
            Id: "00000000-0000-0000-0000-000000000000",
            CommodityTypeId: '00000000-0000-0000-0000-000000000000',
            ProductId: '00000000-0000-0000-0000-000000000000',
            DealerId: '00000000-0000-0000-0000-000000000000',
            DealerLocationId: '00000000-0000-0000-0000-000000000000',
            ContractId: '00000000-0000-0000-0000-000000000000',
            ExtensionTypeId: '00000000-0000-0000-0000-000000000000',
            AttributeSpecificationId: '00000000-0000-0000-0000-000000000000',
            Premium: 0.0,
            PremiumCurrencyTypeId: '00000000-0000-0000-0000-000000000000',
            DealerPaymentCurrencyTypeId: '00000000-0000-0000-0000-000000000000',
            CustomerPaymentCurrencyTypeId: '00000000-0000-0000-0000-000000000000',
            CoverTypeId: '00000000-0000-0000-0000-000000000000',
            HrsUsedAtPolicySale: '',
            IsPreWarrantyCheck: false,
            PolicySoldDate: '',
            SalesPersonId: '00000000-0000-0000-0000-000000000000',
            PolicyNo: '',
            IsSpecialDeal: false,
            IsPartialPayment: false,
            DealerPayment: 0.0,
            CustomerPayment: 0.0,
            PaymentModeId: '00000000-0000-0000-0000-000000000000',
            RefNo: '',
            Comment: '',
            CustomerId: "00000000-0000-0000-0000-000000000000",
            ItemId: "00000000-0000-0000-0000-000000000000",
            Type: '',
            Vehicle: {},
            BAndW: {},
            Customer: {},
            PolicyStartDate: '',
        }
        $scope.errorTab1 = "";

        $scope.ProductContract = {
            ProductId: "00000000-0000-0000-0000-000000000000",
            PolicyNo: "",
            ParentProductId: "00000000-0000-0000-0000-000000000000",
            ContractId: "00000000-0000-0000-0000-000000000000",
            ExtensionTypeId: "00000000-0000-0000-0000-000000000000",
            AttributeSpecificationId: '00000000-0000-0000-0000-000000000000',
            CoverTypeId: "00000000-0000-0000-0000-000000000000",
            Contracts: [],
            ExtensionTypes: [],
            CoverTypes: [],
            Premium: 0,
            PremiumCurrencyName: '',
            PremiumCurrencyTypeId: "00000000-0000-0000-0000-000000000000",
            Name: ''
        };

        $scope.ManufacturerWarranty = {
            MWEnddate: '',
            MWStartDate: '',
            WarrantyKm: '',
            MWarrantyMonths: '',
            MWIsAvailable: ''
        };

        $scope.ExtentionWarranty = {
            ExtMonths: '',
            ExtKM: '',
            ExtStartDate: '',
            ExtEndDate: ''

        };

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        //RANGA
        var SearchCustomerPopup;
        var SearchPolicyPopup;
        var VehicleSearchPopup;
        var BnwSeaechPopup;
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
        $scope.customerSearchGridloading = false;
        $scope.customerGridloadAttempted = false;
        $scope.customerSearchGridSearchCriterias = {
            firstName: "",
            lastName: "",
            eMail: "",
            mobileNo: ""
        };

        $scope.vehicalSearchGridSearchCriterias = {
            vinNo: "",
            plateNo: ""
        };
        $scope.vehicleSearchGridloading = false;
        $scope.bnWSearchGridloadAttempted = false;
        $scope.bnWSearchGridSearchCriterias = {
            serialNo: "",
            make: emptyGuid(),
            model: emptyGuid()
        };
        $scope.bnWSearchGridloading = false;
        $scope.bnWGridloadAttempted = false;
        //END RANGA
        $scope.ProductContracts = [];
        $scope.PolicyDetailsEnable = false
        $scope.GrossTotal = 0.0;
        $scope.discountAvailable = true;
        $scope.Vin = false;
        $scope.VinLength = 0;
        $scope.VINNoValidation = function () {
            angular.forEach($scope.Categories, function (value) {
                if (value.CommodityCategoryId == $scope.Vehicle.CategoryId && $scope.vwVehicleDetails) {
                    $scope.VinLength = value.Length;
                    $scope.VINNoValidate();
                }
                else if (value.CommodityCategoryId == $scope.BAndW.CategoryId && $scope.vwBAndWDetails) {
                    $scope.VinLength = value.Length;
                    $scope.VINNoValidate();
                }
            });
        }
        $scope.VINNoValidate = function () {
            angular.forEach($scope.CommodityTypes, function (value) {
                if (value.CommodityTypeId == $scope.Policy.CommodityTypeId && value.CommodityTypeDescription == "Automobile") {
                    if ($scope.Vehicle.VINNo.length == $scope.VinLength) {
                        $scope.Vin = true;
                    }
                    else {
                        $scope.Vin = false;
                    }
                } else if (value.CommodityTypeId == $scope.Policy.CommodityTypeId && value.CommodityTypeDescription == "Automotive") {
                    if ($scope.Vehicle.VINNo.length == $scope.VinLength) {
                        $scope.Vin = true;
                    }
                    else {
                        $scope.Vin = false;
                    }
                } else if (value.CommodityTypeId == $scope.Policy.CommodityTypeId && value.CommodityTypeDescription == "Bank") {
                    if ($scope.Vehicle.VINNo.length == $scope.VinLength) {
                        $scope.Vin = true;
                    }
                    else {
                        $scope.Vin = false;
                    }
                }
                else if (value.CommodityTypeId == $scope.Policy.CommodityTypeId) {
                    if ($scope.BAndW.SerialNo && $scope.BAndW.SerialNo.length == $scope.VinLength) {
                        $scope.Vin = true;
                    }
                    else {
                        $scope.Vin = false;
                    }
                }
            });
        }

        //-----------------Hard Code Combo: Load from DB when available-------------------------------------//
        //$scope.PaymentModes = [{ Id: "b8fa922a-b294-4673-9ff8-10e35488253f", PaymentMode: "Check" }, { Id: "ff07bf9d-660f-4750-80d2-539b1d74f9f5", PaymentMode: "Pay Pal" }, { Id: "15b8a9d8-939d-4d1d-8c51-2dabe9057268", PaymentMode: "Credit Card" }];
        //--------------------------------------------------------------------------------------------------//

        $rootScope.policySearch = {
            CommodityType: '',
            PolicyNo: '',
            SerialNo: '',
            //Status: '',
            MobileNo: '',
            vinSerialName: 'VIN No/Serial No',
            StartDate: '',
            EndDate: ''
        };
        $rootScope.policySearch.CommodityType = '';


        //ranga
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }


        //--------------------Policy Search----------------------------------//
        var paginationOptionsPolicySearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };


        var serchGridColumsn = [];
        if ($localStorage.CommodityType === "Tyre") {

            serchGridColumsn = [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Commodity Type', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: 'Service Contract No', field: 'PolicyNo', width: '30%', enableSorting: false, cellClass: 'columCss', },
                { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Contract Sold Date', field: 'PolicySoldDate', enableSorting: false, cellClass: 'columCss' },

                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }];
        } else {

            serchGridColumsn = [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Commodity Type', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: 'Policy No', field: 'PolicyNo', width: '30%', enableSorting: false, cellClass: 'columCss', },
                { name: 'Vin or Serial', field: 'SerialNo', width: '20%', enableSorting: false, cellClass: 'columCss' },
                { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Contract Sold Date', field: 'PolicySoldDate', enableSorting: false, cellClass: 'columCss' },

                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }];
        }


        $scope.gridOptionsPolicy = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: serchGridColumsn,
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
        var getPolicySearchPage = function () {
            $scope.policySearchGridloading = true;
            $scope.policySearchGridloadAttempted = false;
            var policySearchGridParam =
            {
                'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                'type': 'forapproval',
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
        var loadingPolicyData = false;
        var policyLoadingRequests = [];




        $scope.loadPolicy = function (policyId) {
            clearPolicyControls();
            $scope.GrossTotal = 0.00;
            $scope.GrossTotalTmp = 0.00;
            $scope.GrossTmpPaymentType = 0.00;
            $scope.itemDocUploader.queue = [];
            $scope.isInvoiceUploaded = false;
            // alert(policyId);
            if (typeof $scope.Products[0] != 'undefined' && typeof $scope.Products[0].Productcode != 'undefined' && $scope.Products[0].Productcode == "TYRE - ") {
                //alert(policyId);

                if (isGuid(policyId)) {
                    SearchPolicyPopup.close();

                    policyLoadingRequests = [
                        { 'reqName': 'GetOtherTirePolicyById' },
                        { 'reqName': 'GetAllProductsByCommodityTypeId' },
                        { 'reqName': 'GetAllMakesByComodityTypeId' },
                        { 'reqName': 'GetAllVariant' },
                        { 'reqName': 'GetAllCategories' },
                        { 'reqName': 'GetContractsByCommodityTypeId' },
                        { 'reqName': 'GetAllDealerLocationsByDealerId' },
                        { 'reqName': 'GetModelesByMakeId' },
                        { 'reqName': 'GetAttachmentsByPolicyId' },
                        { 'reqName': 'GetAllChildProducts' }
                    ];
                    loadingPolicyData = true;


                    swal({ title: 'Loading', text: 'Loading Selected Contract ...', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/GetOtherTirePolicyById',
                        data: { "Id": policyId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        if (data == "Error occured") {
                            swal.close();
                            customErrorMessage("Error occured while geting policy data.");
                            return;
                        }

                        $scope.currentProductCodeTire = true;
                        $scope.Policy = data;
                        $scope.Policy.Id = data.Id;
                        $scope.Policy.CommodityTypeId = data.CommodityTypeId;
                        $scope.SetCommodityTypeValue();
                        $scope.Policy.PaymentModeId = data.PaymentModeId;
                        $scope.selectedPaymentModeChanged();
                        $scope.selectedModelChange();
                        $scope.Policy.ProductId = data.ProductId;
                        $scope.Policy.BAndW.DealerPrice = parseFloat(data.BAndW.DealerPrice).toFixed(2);

                        $scope.ProductContracts = data.ContractTireProducts;
                        $scope.loadTyreDetailsToVisualizer(data.ContractTireProducts, data.Vehicle.AllTyresAreSame);
                        // load vehicle details
                        $scope.vehicle.plateNumber = data.Vehicle.PlateNo;
                        $scope.vehicle.city = data.Vehicle.City;
                        $scope.vehicle.mileage = data.BAndW.Milage + " km";
                        $scope.vehicle.make = data.BAndW.MakeCodeV;
                        $scope.vehicle.model = data.BAndW.ModelCodeV;
                        $scope.vehicle.year = data.BAndW.AdditionalModalYear;
                        $scope.vehicle.invoiceNo = data.BAndW.InvoiceNo;

                        $scope.isInvoiceUploaded = false;
                        if (data.BAndW.UploadedInvoiceFileName != undefined && data.BAndW.UploadedInvoiceFileName.length > 0) {
                            $scope.isInvoiceUploaded = true;
                            $scope.invoiceDownloadRef = data.BAndW.UploadedInvoiceFileRef;
                            $scope.UploadedInvoiceFileName = data.BAndW.UploadedInvoiceFileName;
                        }

                        // end load vehicle details
                        angular.forEach($scope.ProductContracts, function (value) {
                            $scope.NoOfDateNoOfDate = value.NoOfDate;

                            if (value.Position == "BL" || value.Position == "BR") {

                                $scope.Policy.BAndW.articleNumberV1 = value.ArticleNumber;
                                $scope.Policy.BAndW.SerialNoV1 = value.SerialNumber;
                                $scope.SerialNoV1disable = true;

                            } else if (value.Position == "FL" || value.Position == "FR") {
                                $scope.Policy.BAndW.articleNumberV1 = value.ArticleNumber;
                                $scope.Policy.BAndW.SerialNoV1 = value.SerialNumber;
                                $scope.SerialNoV2disable = true;
                            }

                            if ($scope.ProductContracts.length == 1) {

                                $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                            }

                            if ($scope.ProductContracts.length == 2) {
                                if (value.OtherTireDetails.length == 2) {

                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.SerialNoV2disable = false;
                                } else if (value.OtherTireDetails.length == 3) {
                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.serialno2 = value.OtherTireDetails[2].Position + " - " + value.OtherTireDetails[2].SerialNumber;
                                    //$scope.serialno3 = value.OtherTireDetails[3].Position + " - " + value.OtherTireDetails[3].SerialNumber;
                                    $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                                    $scope.Policy.BAndW.articleNumberV2 = value.OtherTireDetails[2].ArticleNumber;
                                    $scope.SerialNoV2disable = true;
                                } else if (value.OtherTireDetails.length == 4) {
                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.serialno2 = value.OtherTireDetails[2].Position + " - " + value.OtherTireDetails[2].SerialNumber;
                                    $scope.serialno3 = value.OtherTireDetails[3].Position + " - " + value.OtherTireDetails[3].SerialNumber;
                                    $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                                    $scope.Policy.BAndW.articleNumberV2 = value.OtherTireDetails[2].ArticleNumber;
                                    $scope.SerialNoV2disable = true;
                                }
                            } else if ($scope.ProductContracts.length == 4) {
                                if (value.OtherTireDetails.length == 2) {

                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.SerialNoV2disable = false;
                                }
                                else if (value.OtherTireDetails.length != 2 && value.OtherTireDetails[0].Diameter == value.OtherTireDetails[2].Diameter &&
                                    value.OtherTireDetails[0].Width != value.OtherTireDetails[2].Width && value.OtherTireDetails[0].CrossSection != value.OtherTireDetails[2].CrossSection
                                    && value.OtherTireDetails[0].LoadSpeed != value.OtherTireDetails[2].LoadSpeed) {
                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.serialno2 = value.OtherTireDetails[2].Position + " - " + value.OtherTireDetails[2].SerialNumber;
                                    $scope.serialno3 = value.OtherTireDetails[3].Position + " - " + value.OtherTireDetails[3].SerialNumber;
                                    $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                                    $scope.Policy.BAndW.articleNumberV2 = value.OtherTireDetails[2].ArticleNumber;
                                    $scope.SerialNoV2disable = true;
                                } else {
                                    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                                    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                                    $scope.serialno2 = value.OtherTireDetails[2].Position + " - " + value.OtherTireDetails[2].SerialNumber;
                                    $scope.serialno3 = value.OtherTireDetails[3].Position + " - " + value.OtherTireDetails[3].SerialNumber;
                                    $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                                    $scope.SerialNoV2disable = false;
                                }
                            }

                            //if (value.OtherTireDetails.length == 2) {

                            //    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                            //    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                            //    $scope.SerialNoV2disable = false;
                            //} else {
                            //    $scope.serialno = value.OtherTireDetails[0].Position + "  -  " + value.OtherTireDetails[0].SerialNumber;
                            //    $scope.serialno1 = value.OtherTireDetails[1].Position + " - " + value.OtherTireDetails[1].SerialNumber;
                            //    $scope.serialno2 = value.OtherTireDetails[2].Position + " - " + value.OtherTireDetails[2].SerialNumber;
                            //    $scope.serialno3 = value.OtherTireDetails[3].Position + " - " + value.OtherTireDetails[3].SerialNumber;
                            //    $scope.Policy.BAndW.articleNumberV1 = value.OtherTireDetails[0].ArticleNumber;
                            //    $scope.Policy.BAndW.articleNumberV2 = value.OtherTireDetails[2].ArticleNumber;
                            //    $scope.SerialNoV2disable = true;
                            //}

                        });



                        angular.forEach($scope.CommodityTypes, function (value) {
                            if ($scope.Policy.CommodityTypeId == value.CommodityTypeId) {
                                $scope.currentCommodityTypeCode = value.CommonCode;
                                $scope.currentCommodityTypeName = value.CommodityTypeDescription;
                                $scope.currentCommodityTypeUniqueCode = value.CommodityCode;
                                // $scope, selectedCommodityType
                                return false;
                            }
                        });

                        $scope.GrossTotal = 0;
                        $scope.Policy.DealerId = data.DealerId;
                        $scope.SetDealerLocationsValues();
                        $scope.Policy.DealerLocationId = data.DealerLocationId;
                        $scope.Policy.DealerPaymentCurrencyTypeId = data.DealerPaymentCurrencyTypeId;
                        $scope.Policy.CustomerPaymentCurrencyTypeId = data.CustomerPaymentCurrencyTypeId;
                        $scope.Policy.HrsUsedAtPolicySale = data.HrsUsedAtPolicySale;
                        $scope.Policy.IsPreWarrantyCheck = data.IsPreWarrantyCheck;
                        $scope.Policy.PolicySoldDate = data.PolicySoldDate;
                        $scope.Policy.PolicyStartDate = data.PolicyStartDate;
                        $scope.Policy.SalesPersonId = data.SalesPersonId;
                        $scope.Policy.PolicyNo = data.PolicyNo;
                        $scope.Policy.CoverTypeId = data.CoverTypeId;
                        $scope.Policy.IsSpecialDeal = data.IsSpecialDeal;
                        $scope.Policy.IsPartialPayment = data.IsPartialPayment;
                        $scope.Policy.DealerPayment = data.DealerPayment.toLocaleString("en-US");
                        $scope.Policy.CustomerPayment = data.CustomerPayment.toLocaleString("en-US");

                        $scope.isDealerPolicy = data.DealerPolicy;


                        $scope.Policy.RefNo = data.RefNo;
                        $scope.Policy.Comment = data.Comment;
                        $scope.Policy.CustomerId = data.CustomerId;
                        $scope.Policy.ItemId = data.ItemId;

                        $scope.checkSwalClose('GetOtherTirePolicyById');
                        $scope.Policy.BAndW = data.BAndW;
                        $scope.BAndW = data.BAndW;
                        $scope.SetModelB();
                        $scope.Policy.Customer = data.Customer
                        $scope.Customer = data.Customer;
                        $scope.Customer.Address1 = data.Customer.BusinessAddress1;
                        $scope.Customer.Address2 = data.Customer.BusinessAddress2;
                        $scope.Customer.Address3 = data.Customer.BusinessAddress3;
                        $scope.Customer.Address4 = data.Customer.BusinessAddress4;

                        $scope.selectedCustomerTypeIdChanged();

                        $scope.SetModelV();
                        $scope.SetCountryValue();

                        if ($scope.Policy.IsPreWarrantyCheck == true)
                            $scope.Policy.IsPreWarrantyCheck = 1;
                        else
                            $scope.Policy.IsPreWarrantyCheck = 0;
                        angular.forEach($scope.Dealers, function (value) {
                            if (value.Id == $scope.Policy.DealerId) {
                                $scope.Policy.DealerPaymentCurrencyTypeId = value.CurrencyId;
                                $scope.Policy.CustomerPaymentCurrencyTypeId = value.CurrencyId;
                            }
                        });


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
                                    ref: data.Attachments[i].FileServerRef
                                }

                                if (data.Attachments[i].AttachmentSection === "Customer") {
                                    $scope.customerDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Item") {
                                    $scope.itemDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Policy") {
                                    $scope.policyDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Payment") {
                                    $scope.paymentDocUploader.queue.push(attachment)
                                }
                            }
                            $scope.checkSwalClose('GetAttachmentsByPolicyId');

                        }).error(function (data, status, headers, config) {
                            $scope.checkSwalClose('GetAttachmentsByPolicyId');
                        });;;


                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Product/GetAllChildProducts',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.Policy.ProductId }
                        }).success(function (data, status, headers, config) {
                            $scope.ChildProducts = data;
                            $scope.currentProductCode = data[0].Productcode; //for ui change for tyre product
                            if ($scope.currentProductCode === 'TYRE') {
                                $scope.isItemStatusDisabled = true;
                                angular.forEach($scope.itemStatuses, function (value) {
                                    if (value.Status == "New")
                                        $scope.Policy.ItemStatusId = value.Id;
                                });
                            } else {
                                $scope.isItemStatusDisabled = false;
                            }
                            $scope.checkSwalClose('GetAllChildProducts');
                        }).error(function (data, status, headers, config) {
                            $scope.checkSwalClose('GetAllChildProducts');
                        });

                    }
                    ).error(function (data, status, headers, config) {
                        $scope.checkSwalClose('GetOtherTirePolicyById');
                    });


                }

            } else {
                if (isGuid(policyId)) {
                    policyLoadingRequests = [
                        { 'reqName': 'GetPolicyById' },
                        { 'reqName': 'GetAllProductsByCommodityTypeId' },
                        { 'reqName': 'GetAllMakesByComodityTypeId' },
                        { 'reqName': 'GetAllVariant' },
                        { 'reqName': 'GetAllCategories' },
                        { 'reqName': 'GetContractsByCommodityTypeId' },
                        { 'reqName': 'GetAllDealerLocationsByDealerId' },
                        { 'reqName': 'GetModelesByMakeId' },
                        { 'reqName': 'GetAttachmentsByPolicyId' },
                        { 'reqName': 'GetAllChildProducts' }
                    ];
                    loadingPolicyData = true;
                    SearchPolicyPopup.close();
                    swal({ title: 'Loading', text: 'Loading Selected Policy ...', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/GetPolicyById',
                        data: { "Id": policyId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Policy = data;
                        $scope.Policy.Id = data.Id;
                        $scope.Policy.CommodityTypeId = data.CommodityTypeId;
                        $scope.SetCommodityTypeValue();
                        $scope.Policy.PaymentModeId = data.PaymentModeId;
                        policyLoadingRequests.push({ 'reqName': 'GetAllPaymentTypesByPaymentModeId' })
                        $scope.selectedPaymentModeChanged();
                        $scope.selectedModelChange();
                        $scope.Policy.ProductId = data.ProductId;
                        $scope.Policy.BAndW.DealerPrice = parseFloat(data.BAndW.DealerPrice).toFixed(2);

                        $scope.ProductContracts = data.ContractProducts;
                        angular.forEach($scope.CommodityTypes, function (value) {
                            if ($scope.Policy.CommodityTypeId == value.CommodityTypeId) {
                                $scope.currentCommodityTypeCode = value.CommonCode;
                                $scope.currentCommodityTypeName = value.CommodityTypeDescription;
                                $scope.currentCommodityTypeUniqueCode = value.CommodityCode;
                                // $scope, selectedCommodityType
                                return false;
                            }
                        });
                        if ($scope.currentProductTypeCode == 'ILOE') {
                            $scope.emiContractId = data.ContractId;
                            $scope.dealerPrice = data.Vehicle.DealerPrice;
                            policyLoadingRequests.push({ 'reqName': 'GetEMIValue' })
                            $scope.getEMIValue();
                        }



                        $scope.ManufacturerWarranty.MWStartDate = data.MWStartDate;
                        $scope.ManufacturerWarranty.MWEnddate = data.MWEnddate;
                        $scope.ManufacturerWarranty.WarrantyKm = data.MWKM;
                        $scope.ManufacturerWarranty.MWarrantyMonths = data.MWMonths;
                        $scope.ManufacturerWarranty.MWIsAvailable = data.MWIsAvailable;
                        $scope.ExtentionWarranty.ExtMonths = data.ExtMonths;
                        $scope.ExtentionWarranty.ExtKM = data.ExtKM;
                        $scope.ExtentionWarranty.KMCutOff = data.KMCutOff;
                        if ($scope.Policy.Type == 'Vehicle') {
                            if (data.ExtKM == 0) {
                                $scope.ExtentionWarranty.ExtKM = "Unlimited";
                            } else if ($scope.ExtentionWarranty.KMCutOff == data.ExtKM) {
                                $scope.ExtentionWarranty.ExtKM = "Upto " + data.ExtKM;
                            } else {
                                $scope.ExtentionWarranty.ExtKM = "MW +" + data.ExtKM;
                            }
                        }

                        $scope.ExtentionWarranty.ExtStartDate = data.ExtStartDate;
                        $scope.ExtentionWarranty.ExtEndDate = data.ExtEndDate;



                        //$scope.Policy.Discount = data.ContractProducts.
                        //angular.forEach($scope.ProductContracts, function (value) {
                        //    if (value.RSA)
                        //        value.CoverTypes = value.RSAProviders;
                        //    $scope.GrossTotal = $scope.GrossTotal + value.Premium;
                        //});
                        $scope.GrossTotal = parseFloat(data.Premium).toLocaleString("en-US");
                        $scope.Policy.DealerId = data.DealerId;
                        $scope.SetDealerLocationsValues();
                        $scope.Policy.DealerLocationId = data.DealerLocationId;
                        $scope.Policy.DealerPaymentCurrencyTypeId = data.DealerPaymentCurrencyTypeId;
                        $scope.Policy.CustomerPaymentCurrencyTypeId = data.CustomerPaymentCurrencyTypeId;
                        $scope.Policy.HrsUsedAtPolicySale = data.HrsUsedAtPolicySale;
                        $scope.Policy.IsPreWarrantyCheck = data.IsPreWarrantyCheck;
                        $scope.Policy.PolicySoldDate = data.PolicySoldDate;
                        $scope.Policy.PolicyStartDate = data.PolicyStartDate;
                        $scope.Policy.SalesPersonId = data.SalesPersonId;
                        $scope.Policy.PolicyNo = data.PolicyNo;
                        $scope.Policy.CoverTypeId = data.CoverTypeId;
                        $scope.Policy.IsSpecialDeal = data.IsSpecialDeal;
                        $scope.Policy.IsPartialPayment = data.IsPartialPayment;
                        $scope.Policy.DealerPayment = data.DealerPayment.toLocaleString("en-US");
                        $scope.Policy.CustomerPayment = data.CustomerPayment.toLocaleString("en-US");

                        $scope.isDealerPolicy = data.DealerPolicy;

                        $scope.Policy.RefNo = data.RefNo;
                        $scope.Policy.Comment = data.Comment;
                        $scope.Policy.CustomerId = data.CustomerId;
                        $scope.Policy.ItemId = data.ItemId;
                        //$scope.Policy.Type = data.Type;
                        $scope.Policy.Vehicle = data.Vehicle;
                        $scope.Vehicle = data.Vehicle;

                        $scope.Policy.BAndW = data.BAndW;
                        $scope.BAndW = data.BAndW;
                        $scope.SetModelB();
                        $scope.Policy.Customer = data.Customer
                        $scope.Customer = data.Customer;
                        $scope.selectedCustomerTypeIdChanged();

                        $scope.SetModelV();
                        $scope.SetCountryValue();
                        if ($scope.Policy.IsPreWarrantyCheck == true)
                            $scope.Policy.IsPreWarrantyCheck = 1;
                        else
                            $scope.Policy.IsPreWarrantyCheck = 0;
                        angular.forEach($scope.Dealers, function (value) {
                            if (value.Id == $scope.Policy.DealerId) {
                                $scope.Policy.DealerPaymentCurrencyTypeId = value.CurrencyId;
                                $scope.Policy.CustomerPaymentCurrencyTypeId = value.CurrencyId;
                            }
                        });
                        $scope.selectedVehicleStatusChanged();
                        $scope.VINNoValidation();
                        $scope.VINNoValidate();
                        $scope.calculateAllPremiums();
                        $scope.Policy.PaymentTypeId = data.PaymentTypeId;
                        $scope.selectedPaymentTypeChanged();
                        $scope.SetCustomerTypeValue();


                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/PolicyReg/GetAttachmentsByPolicyId',
                            data: { "Id": policyId },
                            headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {

                            $scope.checkSwalClose('GetAttachmentsByPolicyId');

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
                                    ref: data.Attachments[i].FileServerRef
                                }

                                if (data.Attachments[i].AttachmentSection === "Customer") {
                                    $scope.customerDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Item") {
                                    $scope.itemDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Policy") {
                                    $scope.policyDocUploader.queue.push(attachment)
                                } else if (data.Attachments[i].AttachmentSection === "Payment") {
                                    $scope.paymentDocUploader.queue.push(attachment)
                                }
                            }

                        }).error(function (data, status, headers, config) {
                            $scope.checkSwalClose('GetAttachmentsByPolicyId');
                        });;


                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Product/GetAllChildProducts',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.Policy.ProductId }
                        }).success(function (data, status, headers, config) {
                            $scope.ChildProducts = data;
                            $scope.currentProductCode = data[0].Productcode; //for ui change for tyre product
                            if ($scope.currentProductCode === 'TYRE') {
                                $scope.isItemStatusDisabled = true;
                                angular.forEach($scope.itemStatuses, function (value) {
                                    if (value.Status == "New")
                                        $scope.Policy.ItemStatusId = value.Id;
                                });
                                $scope.checkSwalClose('GetAllChildProducts');
                            } else {
                                $scope.isItemStatusDisabled = false;
                                $scope.checkSwalClose('GetAllChildProducts');
                            }
                        }).error(function (data, status, headers, config) {
                            $scope.checkSwalClose('GetAllChildProducts');
                        });;

                        $scope.checkSwalClose('GetPolicyById');
                    }).error(function (data, status, headers, config) {
                        $scope.checkSwalClose('GetPolicyById');
                    });


                }
            }


        }




        $scope.checkSwalClose = async function (requestName) {
            if (loadingPolicyData && requestName != null && requestName != undefined && requestName.length > 1) {
              //  console.log('response : ' + requestName);
                policyLoadingRequests = policyLoadingRequests.filter(a => a.reqName !== requestName);
               //  console.table(policyLoadingRequests);
                if (policyLoadingRequests.length == 0) {
                    loadingPolicyData = false;
                    swal.close();
                 //  console.log('closed swal');
                }
            } else {
                swal.close();
            }
        }

        $scope.loadTyreDetailsToVisualizer = function (tyreDetails, allTyreSame) {
            $scope.dealerInvoiceTireDetails.front.leftcheckbox = false;
            $scope.dealerInvoiceTireDetails.front.rightcheckbox = false;
            $scope.dealerInvoiceTireDetails.back.leftcheckbox = false;
            $scope.dealerInvoiceTireDetails.back.rightcheckbox = false;
            $scope.spareWheelcheckbox = false;
            $scope.isFrontTireDetailsVisibleL = false;
            $scope.isBackTireDetailsVisibleL = false;

            if (tyreDetails.length > 0) {
                $scope.tireSizesAvailable = true;
                let tireDetails = tyreDetails[0].OtherTireDetails;
                angular.forEach(tireDetails, function (value) {
                    console.log("loading tyre to visualizer");
                    console.log(value);
                    // load check boxes values
                    if (value.Position === "FL") {
                        $scope.dealerInvoiceTireDetails.front.leftcheckbox = true;
                    }
                    if (value.Position === "FR") {
                        $scope.dealerInvoiceTireDetails.front.rightcheckbox = true;
                    }
                    if (value.Position === "BL") {
                        $scope.dealerInvoiceTireDetails.back.leftcheckbox = true;
                    }
                    if (value.Position === "BR") {
                        $scope.dealerInvoiceTireDetails.back.rightcheckbox = true;
                    }
                    if (value.Position === "D") {
                        $scope.spareWheelcheckbox = true;
                    }

                    // load tire sizespanel values
                    if (allTyreSame != null && allTyreSame) {
                        if (value.SerialNumber.length > 5) {
                            $scope.dealerInvoiceTireDetails.front.serialLeft = 'C' + value.SerialNumber + '0000';
                            $scope.dealerInvoiceTireDetails.front.width = value.Width;
                            $scope.dealerInvoiceTireDetails.front.cross = value.CrossSection;
                            $scope.dealerInvoiceTireDetails.front.diameter = value.Diameter;
                            $scope.dealerInvoiceTireDetails.front.loadSpeed = value.LoadSpeed;
                            $scope.dealerInvoiceTireDetails.front.dotLeft = value.DotNumber;
                            $scope.dealerInvoiceTireDetails.front.pattern = value.Pattern;
                            $scope.dealerInvoiceTireDetails.front.price = value.Price;
                            $scope.isFrontTireDetailsVisibleL = true;
                        }
                    } else {
                        if (value.Position === "FL" || value.Position === "FR") {
                            if (value.SerialNumber.length > 5) {
                                $scope.dealerInvoiceTireDetails.front.serialLeft = 'C' + value.SerialNumber + '0000';
                                $scope.dealerInvoiceTireDetails.front.width = value.Width;
                                $scope.dealerInvoiceTireDetails.front.cross = value.CrossSection;
                                $scope.dealerInvoiceTireDetails.front.diameter = value.Diameter;
                                $scope.dealerInvoiceTireDetails.front.loadSpeed = value.LoadSpeed;
                                $scope.dealerInvoiceTireDetails.front.dotLeft = value.DotNumber;
                                $scope.dealerInvoiceTireDetails.front.pattern = value.Pattern;
                                $scope.dealerInvoiceTireDetails.front.price = value.Price;
                                $scope.isFrontTireDetailsVisibleL = true;
                            }
                        }

                        if (value.Position === "BL" || value.Position === "BR") {
                            if (value.SerialNumber.length > 5) {
                                $scope.dealerInvoiceTireDetails.back.serialLeft = 'C' + value.SerialNumber + '0000';
                                $scope.dealerInvoiceTireDetails.back.width = value.Width;
                                $scope.dealerInvoiceTireDetails.back.cross = value.CrossSection;
                                $scope.dealerInvoiceTireDetails.back.diameter = value.Diameter;
                                $scope.dealerInvoiceTireDetails.back.loadSpeed = value.LoadSpeed;
                                $scope.dealerInvoiceTireDetails.back.dotLeft = value.DotNumber;
                                $scope.dealerInvoiceTireDetails.back.pattern = value.Pattern;
                                $scope.dealerInvoiceTireDetails.back.price = value.Price;
                                $scope.isBackTireDetailsVisibleL = true;


                            }
                        }
                    }


                });
            }


        }

        //endranga

        //$scope.manufactureWarrentyCheckForUsedItem = function () {
        //    swal({ title: 'Processing', text: 'Checking manufacturer warrenty availability...', showConfirmButton: false });
        //    var data = {
        //        makeId: $scope.Policy.Vehicle.MakeId,
        //        modelId: $scope.Policy.Vehicle.ModelId,
        //        dealerId: $scope.Policy.Vehicle.DealerId,
        //        commodityTypeId: $scope.Policy.CommodityTypeId,
        //        mwStartDate: $scope.ManufacturerWarranty.MWStartDate,
        //        policySoldDate: $scope.Policy.PolicySoldDate,
        //        usage: $scope.Policy.HrsUsedAtPolicySale
        //    };
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/PolicyReg/ManufacturerWarrentyAvailabilityCheckOnPolicySave',
        //        data: data,
        //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        //        if (data.code === 'No') {
        //            swal({
        //                title: "",
        //                text: "Item is under manufacturer warranty, You want to proceed with existing manufacturer warranty?",
        //                type: "info",
        //                showCancelButton: true,
        //                confirmButtonClass: "btn-danger",
        //                confirmButtonText: "Yes, proceed!",
        //                cancelButtonText: "No",
        //                closeOnConfirm: true,
        //                closeOnCancel: true
        //            },
        //                function (isConfirm) {
        //                    if (isConfirm) {
        //                        $scope.ManufacturerWarranty.MWIsAvailable = true;
        //                    } else {
        //                        $scope.ManufacturerWarranty.MWIsAvailable = false;
        //                    }
        //                });
        //        } else {
        //            $scope.ManufacturerWarranty.MWIsAvailable = false;
        //        }
        //    }).error(function (data, status, headers, config) {
        //        swal.close();
        //    });
        //}

        $scope.selectedVehicleStatusChanged = function () {


            if ($scope.Policy.Vehicle.ItemStatusId != emptyGuid()) {
                angular.forEach($scope.ItemStatuss, function (value) {
                    if ($scope.Policy.Vehicle.ItemStatusId == value.Id) {
                        if (value.Status == "Used")
                            $scope.isUsedItem = true;
                        else
                            $scope.isUsedItem = false;
                    }
                });
            }

            try {
                $scope.selectedVehicleStatus = $.grep($scope.itemStatuses, function (val) {
                    return val.Id == Policy.Vehicle.ItemStatusId;
                })[0].Status;
            } catch (e) {
                $scope.selectedVehicleStatus = '';
            }
        }

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
        function clearPolicyControls() {
            $scope.Vin = false;
            $scope.VinLength = 0;
            $scope.GrossTotal = 0.0;
            $scope.Policy.Id = "00000000-0000-0000-0000-000000000000";
            //  $scope.Policy.CommodityTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.ProductId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.DealerId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.DealerLocationId = "";
            $scope.Policy.ContractId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.ExtensionTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.AttributeSpecificationId = '00000000-0000-0000-0000-000000000000';
            $scope.Policy.Premium = 0;
            $scope.Policy.PremiumCurrencyTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.DealerPaymentCurrencyTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.CustomerPaymentCurrencyTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.CoverTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.HrsUsedAtPolicySale = "";
            $scope.Policy.IsPreWarrantyCheck = false;
            $scope.Policy.PolicySoldDate = "";
            $scope.Policy.SalesPersonId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.PolicyNo = "";
            $scope.Policy.IsSpecialDeal = false;
            $scope.Policy.IsPartialPayment = false;
            $scope.Policy.DealerPayment = 0;
            $scope.Policy.Discount = 0;
            $scope.Policy.CustomerPayment = 0;
            $scope.Policy.PaymentModeId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.RefNo = "";
            $scope.Policy.Comment = "";
            $scope.Policy.CustomerId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.ItemId = "00000000-0000-0000-0000-000000000000";
            $scope.Policy.Type = "";
            $scope.Policy.Vehicle = {};
            $scope.Policy.BAndW = {};
            $scope.Policy.Customer = {};
            clearCustomerControls();
            clearVehicleControls();
            clearBAndWControls();
            $scope.ApprovalEnable();
            $scope.ProductContracts = [];
            $scope.ManufacturerWarranty.MWEnddate = "";
            $scope.ManufacturerWarranty.MWKM = "";
            $scope.ManufacturerWarranty.MWStartDate = "";
            $scope.ManufacturerWarranty.MWarrantyMonths = "";
            $scope.ManufacturerWarranty.WarrantyKm = "";
            $scope.ManufacturerWarranty.MWIsAvailable = false;
            $scope.ExtentionWarranty.ExtEndDate = "";
            $scope.ExtentionWarranty.ExtStartDate = "";
            $scope.ExtentionWarranty.ExtKM = "";
            $scope.ExtentionWarranty.ExtMonths = "";
            $scope.ExtentionWarranty.KMCutOff = "";
            $scope.itemDocUploader.queue = [];
            $scope.customerDocUploader.queue = [];
            $scope.policyDocUploader.queue = [];
            $scope.paymentDocUploader.queue = [];
            $scope.tireSizesAvailable = false;
            $scope.vehicle.plateNumber = '';
            $scope.vehicle.city = '';

            $scope.vehicle.mileage = '';
            $scope.vehicle.make = '';
            $scope.vehicle.model = '';
            $scope.vehicle.year = '';
            $scope.vehicle.invoiceNo = '';
            $scope.isInvoiceUploaded = false;
            $scope.invoiceDownloadRef = "";
            $scope.UploadedInvoiceFileName = "";

        };
        function SetPolicyValues() {
            $scope.errorTab1 = "";
            $scope.myPolicySelectedRows = $scope.gridApiPolicy.selection.getSelectedRows();
            angular.forEach($scope.myPolicySelectedRows, function (value, key) {
                if (value.Id != "00000000-0000-0000-0000-000000000000") {
                    $scope.Policy.Id = value.Id;
                    $scope.ApprovalEnable();
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/GetPolicyById',
                        data: { "Id": value.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.GrossTotal = 0.00;
                        $scope.Policy = data;
                        $scope.Policy.Id = data.Id;
                        $scope.Policy.CommodityTypeId = data.CommodityTypeId;
                        $scope.SetCommodityTypeValue();
                        $scope.Policy.ProductId = data.ProductId;
                        $scope.ProductContracts = data.ContractProducts;
                        angular.forEach($scope.ProductContracts, function (value) {
                            if (value.RSA) {
                                value.CoverTypes = value.RSAProviders;
                            }

                            $scope.GrossTotal = $scope.GrossTotal + value.Premium;
                        });

                        $scope.Policy.Product = data.ProductId;
                        $scope.GetProductById();
                        $scope.GrossTotal = parseFloat($scope.GrossTotal).toLocaleString("en-US");
                        $scope.Policy.DealerId = data.DealerId;
                        $scope.SetDealerLocationsValues();
                        $scope.Policy.DealerLocationId = data.DealerLocationId;
                        $scope.Policy.DealerPaymentCurrencyTypeId = data.DealerPaymentCurrencyTypeId;
                        $scope.Policy.CustomerPaymentCurrencyTypeId = data.CustomerPaymentCurrencyTypeId;
                        $scope.Policy.HrsUsedAtPolicySale = data.HrsUsedAtPolicySale;
                        $scope.Policy.IsPreWarrantyCheck = data.IsPreWarrantyCheck;
                        $scope.Policy.PolicySoldDate = data.PolicySoldDate;
                        $scope.Policy.SalesPersonId = data.SalesPersonId;
                        $scope.Policy.PolicyNo = data.PolicyNo;
                        $scope.Policy.IsSpecialDeal = data.IsSpecialDeal;
                        $scope.Policy.IsPartialPayment = data.IsPartialPayment;
                        $scope.Policy.DealerPayment = data.DealerPayment;
                        $scope.Policy.CustomerPayment = data.CustomerPayment;
                        $scope.Policy.PaymentModeId = data.PaymentModeId;
                        $scope.Policy.RefNo = data.RefNo;
                        $scope.Policy.Comment = data.Comment;
                        $scope.Policy.CustomerId = data.CustomerId;
                        $scope.Policy.ItemId = data.ItemId;
                        $scope.Policy.Type = data.Type;
                        $scope.Policy.Vehicle = data.Vehicle;
                        $scope.Vehicle = data.Vehicle;
                        $scope.SetModelV();
                        $scope.Policy.BAndW = data.BAndW;
                        $scope.BAndW = data.BAndW;
                        $scope.SetModelB();
                        $scope.Policy.Customer = data.Customer
                        $scope.Customer = data.Customer;
                        $scope.SetCountryValue();
                        if ($scope.Policy.IsPreWarrantyCheck == true)
                            $scope.Policy.IsPreWarrantyCheck = 1;
                        else
                            $scope.Policy.IsPreWarrantyCheck = 0;
                        angular.forEach($scope.Dealers, function (value) {
                            if (value.Id == $scope.Policy.DealerId) {
                                $scope.Policy.DealerPaymentCurrencyTypeId = value.CurrencyId;
                                $scope.Policy.CustomerPaymentCurrencyTypeId = value.CurrencyId;
                            }
                        });
                        $scope.VINNoValidation();
                        $scope.VINNoValidate();

                    }).error(function (data, status, headers, config) {
                        clearPolicyControls();
                    });
                }
                else {
                    clearPolicyControls();
                }
            });
        }
        //--------------------Customer Search----------------------------------//
        var paginationOptionsCustomerSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsCustomer = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'First Name', field: 'FirstName', enableSorting: false, cellClass: 'columCss' },
                { name: 'Last Name', field: 'LastName', enableSorting: false, cellClass: 'columCss', },
                { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Email', field: 'Email', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadCustomer(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsCustomerSearchGrid.sort = null;
                    } else {
                        paginationOptionsCustomerSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getCustomerSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsCustomerSearchGrid.pageNumber = newPage;
                    paginationOptionsCustomerSearchGrid.pageSize = pageSize;
                    getCustomerSearchPage();
                });
            }
        };
        $scope.GetProductById = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetProductById',
                data: { "productId": $scope.Policy.ProductId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.currentProductTypeCode = data.ProductTypeCode;
                    if ($scope.currentProductTypeCode == 'ILOE') {
                        $scope.isProductILOE = true;

                    }
                    else {

                        $scope.isProductILOE = false;
                    }


                }
                $scope.checkSwalClose('GetProductById');
            }).error(function (data, status, headers, config) {
                $scope.checkSwalClose('GetProductById');
            });;
        }
        $scope.refresCustomerSearchGridData = function () {
            getCustomerSearchPage();
        }
        $scope.loadCustomer = function (customerId) {
            if (isGuid(customerId)) {
                SearchCustomerPopup.close();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetCustomerById',
                    data: { "Id": customerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Customer.Id = data.Id;
                    $scope.Customer.NationalityId = data.NationalityId;
                    $scope.Customer.CountryId = data.CountryId;
                    $scope.Customer.MobileNo = data.MobileNo;
                    $scope.Customer.OtherTelNo = data.OtherTelNo;
                    $scope.Customer.FirstName = data.FirstName;
                    $scope.Customer.LastName = data.LastName;
                    $scope.Customer.DateOfBirth = data.DateOfBirth;
                    $scope.Customer.Email = data.Email;
                    $scope.Customer.CustomerTypeId = data.CustomerTypeId;
                    $scope.Customer.UsageTypeId = data.UsageTypeId;
                    $scope.SetCountryValue();
                    $scope.Customer.CityId = data.CityId;
                    $scope.Customer.Email = data.Email;
                    $scope.Customer.Address1 = data.Address1;
                    $scope.Customer.Address2 = data.Address2;
                    $scope.Customer.Address3 = data.Address3;
                    $scope.Customer.Address4 = data.Address4;
                    $scope.Customer.IDNo = data.IDNo;
                    $scope.Customer.IDTypeId = data.IDTypeId;
                    $scope.Customer.DLIssueDate = data.DLIssueDate;
                    $scope.Customer.Gender = data.Gender;
                    $scope.Customer.BusinessName = data.BusinessName;
                    $scope.Customer.BusinessAddress1 = data.BusinessAddress1;
                    $scope.Customer.BusinessAddress2 = data.BusinessAddress2;
                    $scope.Customer.BusinessAddress3 = data.BusinessAddress3;
                    $scope.Customer.BusinessAddress4 = data.BusinessAddress4;
                    $scope.Customer.BusinessTelNo = data.BusinessTelNo;
                    $scope.Policy.Customer = $scope.Customer;
                    $scope.Policy.CustomerId = $scope.Customer.Id;
                }).error(function (data, status, headers, config) {
                    // clearCustomerControls();
                });
            }
        }

        $scope.getEMIValue = function () {

            //if ($scope.product.lonePeriod > 48) {
            //    customErrorMessage("Limit Upto 48 Months.");
            //} else {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetEMIValue',
                data: {
                    "LoneAmount": $scope.dealerPrice,
                    "ContractId": $scope.emiContractId
                },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var EMI = data;
                $scope.EMI = EMI.toFixed(2);
                $scope.checkSwalClose('GetEMIValue');
            }).error(function (data, status, headers, config) {
                $scope.checkSwalClose('GetEMIValue');
            });
            //}
        }
        var getCustomerSearchPage = function () {
            $scope.customerSearchGridloading = true;
            $scope.customerGridloadAttempted = false;
            var customerSearchGridParam =
            {
                'paginationOptionsCustomerSearchGrid': paginationOptionsCustomerSearchGrid,
                'customerSearchGridSearchCriterias': $scope.customerSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomersForSearchGrid',
                data: customerSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsCustomer.data = response_arr.data;
                $scope.gridOptionsCustomer.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.customerSearchGridloading = false;
                $scope.customerGridloadAttempted = true;
            });
        };

        $scope.CustomerSearchPopup = function () {
            $scope.customerSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                eMail: '',
                mobileNo: ''
            };
            var paginationOptionsCustomerSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getCustomerSearchPage();
            SearchCustomerPopup = ngDialog.open({
                template: 'popUpSearchCustomer',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });


        };
        function clearCustomerControls() {
            $scope.Customer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.UserName = "";
            $scope.Customer.NationalityId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.CountryId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.MobileNo = "";
            $scope.Customer.OtherTelNo = "";
            $scope.Customer.FirstName = "";
            $scope.Customer.LastName = "";
            $scope.Customer.DateOfBirth = "";
            $scope.Customer.Email = "";
            $scope.Customer.Address1 = "";
            $scope.Customer.Address2 = "";
            $scope.Customer.Address3 = "";
            $scope.Customer.Address4 = "";
            $scope.Customer.IDNo = "";
            $scope.Customer.IDTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.DLIssueDate = "";
            $scope.Customer.Gender = "M";
            $scope.Customer.CustomerTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.UsageTypeId = "";
            $scope.Customer.CityId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.BusinessName = "";
            $scope.Customer.BusinessAddress1 = "";
            $scope.Customer.BusinessAddress2 = "";
            $scope.Customer.BusinessAddress3 = "";
            $scope.Customer.BusinessAddress4 = "";
            $scope.Customer.BusinessTelNo = "";

        };
        function SetCustomerValues() {
            $scope.errorTab1 = "";
            $scope.myCustomerSelectedRows = $scope.gridApiCustomer.selection.getSelectedRows();
            angular.forEach($scope.myCustomerSelectedRows, function (value, key) {
                if (value.Id != "00000000-0000-0000-0000-000000000000") {
                    $scope.Customer.Id = value.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/GetCustomerById',
                        data: { "Id": value.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Customer.Id = data.Id;
                        $scope.Customer.NationalityId = data.NationalityId;
                        $scope.Customer.CountryId = data.CountryId;
                        $scope.Customer.MobileNo = data.MobileNo;
                        $scope.Customer.OtherTelNo = data.OtherTelNo;
                        $scope.Customer.FirstName = data.FirstName;
                        $scope.Customer.LastName = data.LastName;
                        $scope.Customer.DateOfBirth = data.DateOfBirth;
                        $scope.Customer.Email = data.Email;
                        $scope.Customer.CustomerTypeId = data.CustomerTypeId;
                        $scope.Customer.UsageTypeId = data.UsageTypeId;
                        $scope.SetCountryValue();
                        $scope.Customer.CityId = data.CityId;
                        $scope.Customer.Email = data.Email;
                        $scope.Customer.Address1 = data.Address1;
                        $scope.Customer.Address2 = data.Address2;
                        $scope.Customer.Address3 = data.Address3;
                        $scope.Customer.Address4 = data.Address4;
                        $scope.Customer.IDNo = data.IDNo;
                        $scope.Customer.IDTypeId = data.IDTypeId;
                        $scope.Customer.DLIssueDate = data.DLIssueDate;
                        $scope.Customer.Gender = data.Gender;
                        $scope.Customer.BusinessName = data.BusinessName;
                        $scope.Customer.BusinessAddress1 = data.BusinessAddress1;
                        $scope.Customer.BusinessAddress2 = data.BusinessAddress2;
                        $scope.Customer.BusinessAddress3 = data.BusinessAddress3;
                        $scope.Customer.BusinessAddress4 = data.BusinessAddress4;
                        $scope.Customer.BusinessTelNo = data.BusinessTelNo;
                        $scope.Policy.Customer = $scope.Customer;
                        $scope.Policy.CustomerId = $scope.Customer.Id;
                    }).error(function (data, status, headers, config) {
                        clearCustomerControls();
                    });
                }
                else {
                    clearCustomerControls();
                }
            });
        }
        //--------------------Vehicle Search----------------------------------//
        var paginationOptionsVehicleSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsVehicle = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'VIN No', field: 'VINNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Plate No', field: 'PlateNo', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadVehicle(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsVehicleSearchGrid.sort = null;
                    } else {
                        paginationOptionsVehicleSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getVehicleSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsVehicleSearchGrid.pageNumber = newPage;
                    paginationOptionsVehicleSearchGrid.pageSize = pageSize;
                    getVehicleSearchPage();
                });
            }
        };
        $scope.refresCustomerSearchGridData = function () {
            getVehicleSearchPage();
        }
        var getVehicleSearchPage = function () {
            //VehicalSearchGridSearchCriterias
            $scope.vehicleSearchGridloading = true;
            $scope.vehicleGridloadAttempted = false;
            var vehicleSearchGridParam =
            {
                'paginationOptionsVehicleSearchGrid': paginationOptionsVehicleSearchGrid,
                'vehicalSearchGridSearchCriterias': $scope.vehicalSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Vehicle/GetAllVehiclesForSearchGrid',
                data: vehicleSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsVehicle.data = response_arr.data;
                $scope.gridOptionsVehicle.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.vehicleSearchGridloading = false;
                $scope.vehicleGridloadAttempted = true;
            });
        }
        $scope.VehicleSearch = function () {
            $scope.vehicalSearchGridSearchCriterias = {
                vinNo: "",
                plateNo: ""
            };
            var paginationOptionsVehicleSearchGrid = {
                pageNumber: 1,
                //  pageSize: 25,
                sort: null
            };
            getVehicleSearchPage();
            VehicleSearchPopup = ngDialog.open({
                template: 'popUpSearchVehicle',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

        };
        $scope.loadVehicle = function (vehicleId) {
            if (isGuid(vehicleId)) {
                VehicleSearchPopup.close();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                    data: { "Id": vehicleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Vehicle = data;
                    $scope.Vehicle.Id = data.Id;
                    $scope.Vehicle.VINNo = data.VINNo;
                    $scope.Vehicle.MakeId = data.MakeId;
                    $scope.SetModelV();
                    $scope.Vehicle.ModelId = data.ModelId;
                    $scope.Vehicle.CategoryId = data.CategoryId;
                    $scope.Vehicle.ItemStatusId = data.ItemStatusId;
                    $scope.Vehicle.CylinderCountId = data.CylinderCountId;
                    $scope.Vehicle.BodyTypeId = data.BodyTypeId;
                    $scope.Vehicle.PlateNo = data.PlateNo;
                    $scope.Vehicle.ModelYear = data.ModelYear;
                    $scope.Vehicle.FuelTypeId = data.FuelTypeId;
                    $scope.Vehicle.AspirationId = data.AspirationId;
                    $scope.Vehicle.Variant = data.Variant;
                    $scope.Vehicle.VariantId = data.Variant;
                    $scope.Vehicle.TransmissionId = data.TransmissionId;
                    $scope.Vehicle.ItemPurchasedDate = data.ItemPurchasedDate;
                    $scope.Vehicle.EngineCapacityId = data.EngineCapacityId;
                    $scope.Vehicle.DriveTypeId = data.DriveTypeId;
                    $scope.Vehicle.VehiclePrice = data.VehiclePrice;
                    $scope.Vehicle.DealerPrice = data.DealerPrice;
                    $scope.Vehicle.RegistrationDate = data.RegistrationDate;
                    $scope.Vehicle.GrossWeight = data.GrossWeight;
                    $scope.Policy.Vehicle = $scope.Vehicle;
                    $scope.Policy.ItemId = $scope.Vehicle.Id;
                    $scope.Policy.Type = "Vehicle";
                    // $scope.VINNoValidation();
                    // $scope.VINNoValidate();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetModelV = function () {
            $scope.errorTab1 = "";
            if (isGuid($scope.Vehicle.MakeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.Vehicle.MakeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Modeles = data;
                    $scope.checkSwalClose('GetModelesByMakeId');
                    //$scope.loadContractExtension();
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetModelesByMakeId');
                });
            }
        }
        function SetVehicleValues() {
            $scope.Vin = false;
            $scope.VinLength = 0;
            $scope.errorTab1 = "";
            $scope.mySelectedVehicleRows = $scope.gridApiVehicle.selection.getSelectedRows();
            angular.forEach($scope.mySelectedVehicleRows, function (value, key) {
                if (value.Id != "00000000-0000-0000-0000-000000000000") {
                    $scope.Vehicle.Id = value.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                        data: { "Id": value.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Vehicle = data;
                        $scope.Vehicle.Id = data.Id;
                        $scope.Vehicle.VINNo = data.VINNo;
                        $scope.Vehicle.MakeId = data.MakeId;
                        $scope.SetModelV();
                        $scope.Vehicle.ModelId = data.ModelId;
                        $scope.Vehicle.CategoryId = data.CategoryId;
                        $scope.Vehicle.ItemStatusId = data.ItemStatusId;
                        $scope.Vehicle.CylinderCountId = data.CylinderCountId;
                        $scope.Vehicle.BodyTypeId = data.BodyTypeId;
                        $scope.Vehicle.PlateNo = data.PlateNo;
                        $scope.Vehicle.ModelYear = data.ModelYear;
                        $scope.Vehicle.FuelTypeId = data.FuelTypeId;
                        $scope.Vehicle.AspirationId = data.AspirationId;
                        $scope.Vehicle.Variant = data.Variant;
                        $scope.Vehicle.VariantId = data.Variant;
                        $scope.Vehicle.TransmissionId = data.TransmissionId;
                        $scope.Vehicle.ItemPurchasedDate = data.ItemPurchasedDate;
                        $scope.Vehicle.EngineCapacityId = data.EngineCapacityId;
                        $scope.Vehicle.DriveTypeId = data.DriveTypeId;
                        $scope.Vehicle.VehiclePrice = data.VehiclePrice;
                        $scope.Vehicle.DealerPrice = data.DealerPrice;
                        $scope.Vehicle.GrossWeight = data.GrossWeight;
                        $scope.Policy.Vehicle = $scope.Vehicle;
                        $scope.Policy.ItemId = $scope.Vehicle.Id;
                        $scope.Policy.Type = "Vehicle";
                        //$scope.loadContractExtension();
                        $scope.VINNoValidation();
                        $scope.VINNoValidate();
                    }).error(function (data, status, headers, config) {
                        clearVehicleControls();
                    });
                }
                else {
                    clearVehicleControls();
                }
            });
        }
        function clearVehicleControls() {
            $scope.Vin = false;
            $scope.VinLength = 0;
            $scope.Vehicle.CommodityUsageTypeId = "";
            $scope.Vehicle.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.VINNo = "";
            $scope.Vehicle.MakeId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.ModelId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.CategoryId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.ItemStatusId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.CylinderCountId = "";
            $scope.Vehicle.BodyTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.PlateNo = "";
            $scope.Vehicle.ModelYear = "";
            $scope.Vehicle.FuelTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.AspirationId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.Variant = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.TransmissionId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.ItemPurchasedDate = "";
            $scope.Vehicle.EngineCapacityId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.DriveTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Vehicle.RegistrationDate = '';
            $scope.Vehicle.GrossWeight = 0;
            $scope.Vehicle.VehiclePrice = 0
            $scope.Vehicle.DealerPrice = 0

        };
        //--------------------B&W Search----------------------------------//
        //$scope.myBAndWSearch = [];
        //$scope.gridOptionsBAndW = {
        //    data: 'myBAndWSearch',
        //    paginationPageSizes: [5, 10, 20],
        //    paginationPageSize: 5,
        //    enablePaginationControls: true,
        //    enableRowSelection: true,
        //    enableCellSelection: false,
        //    multiSelect: false,
        //    columnDefs: [{
        //        field: "SerialNo",
        //        displayName: "Serial No"
        //    },
        //    {
        //        field: "Make",
        //        displayName: "Make"
        //    },
        //    {
        //        field: "Model",
        //        displayName: "Model"
        //    }, {
        //        field: "Id",
        //        visible: false
        //    }
        //    ]
        //};
        //$scope.gridOptionsBAndW.onRegisterApi = function (gridApi) {
        //    $scope.gridApiBAndW = gridApi;
        //    gridApi.selection.on.rowSelectionChanged($scope, SetBAndWValues);
        //}
        var paginationOptionsBnWSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsBAndW = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Serial No', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss' },
                { name: 'Model', field: 'Model', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBnW(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsBnWSearchGrid.sort = null;
                    } else {
                        paginationOptionsBnWSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getBnwSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsBnWSearchGrid.pageNumber = newPage;
                    paginationOptionsBnWSearchGrid.pageSize = pageSize;
                    getBnwSearchPage();
                });
            }
        };
        $scope.refresBnWGridData = function () {
            getBnwSearchPage();
        }
        var getBnwSearchPage = function () {
            $scope.bnWSearchGridloading = true;
            $scope.bnWSearchGridloadAttempted = false;
            var bnWSearchGridParam =
            {
                'paginationOptionsBnWSearchGrid': paginationOptionsBnWSearchGrid,
                'bnWSearchGridSearchCriterias': $scope.bnWSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllItemsForSearchGrid',
                data: bnWSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsBAndW.data = response_arr.data;
                $scope.gridOptionsBAndW.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.bnWSearchGridloading = false;
                $scope.bnWSearchGridloadAttempted = true;
            });
        }
        $scope.loadBnW = function (bnwId) {

        }
        $scope.BAndWSearchPopup = function () {
            //$scope.myBAndWSearch = [];
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.SMakes = data;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllModels',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.SModeles = data;
                    angular.forEach($scope.BAndWs, function (value, key) {
                        var make = "";
                        var model = "";
                        angular.forEach($scope.SMakes, function (valMk, key) {
                            if (valMk.Id == value.MakeId) {
                                make = valMk.MakeName;
                            }
                        });
                        angular.forEach($scope.SModeles, function (valMl, key) {
                            if (valMl.Id == value.ModelId) {
                                model = valMl.ModelName;
                            }
                        });
                        // $scope.myBAndWSearch.push({ Id: value.Id, SerialNo: value.SerialNo, Make: make, Model: model })
                    });
                    BnwSeaechPopup = ngDialog.open({
                        template: 'popUpSearchBAndW',
                        className: 'ngdialog-theme-plain',
                        closeByEscape: true,
                        showClose: true,
                        closeByDocument: true,
                        scope: $scope
                    });
                    $scope.refresBnWGridData();
                    //$rootScope.searchBAndW = {
                    //    SerialNo: '',
                    //    Make: '',
                    //    Model: ''
                    //};
                }).error(function (data, status, headers, config) {
                });
            }).error(function (data, status, headers, config) {
            });
        };
        $scope.SearchBAndW = function () {
            $scope.myBAndWSearch = [];
            angular.forEach($scope.BAndWs, function (value, key) {
                var make = "";
                var model = "";
                angular.forEach($scope.Makes, function (valMk, key) {
                    if (valMk.Id == value.MakeId) {
                        make = valMk.MakeName;
                    }
                });
                angular.forEach($scope.Modeles, function (valMl, key) {
                    if (valMl.Id == value.ModelId) {
                        model = valMl.ModelName;
                    }
                });
                $scope.myBAndWSearch.push({ Id: value.Id, SerialNo: value.SerialNo, Make: make, Model: model })
            });
            $scope.tempBAndWList = [];
            if ($rootScope.searchBAndW.SerialNo != "") {
                angular.copy($scope.myBAndWSearch, $scope.tempBAndWList);
                $scope.myBAndWSearch = [];
                angular.forEach($scope.tempBAndWList, function (value, key) {
                    if ($rootScope.searchBAndW.SerialNo == value.SerialNo) {
                        $scope.myPolicySearch.push({ Id: value.Id, SerialNo: value.SerialNo, Make: value.Make, Model: value.Model })
                    }
                });
            }
            if ($rootScope.searchBAndW.Make != "") {
                angular.copy($scope.myBAndWSearch, $scope.tempBAndWList);
                $scope.myBAndWSearch = [];
                angular.forEach($scope.tempBAndWList, function (value, key) {
                    if ($rootScope.searchBAndW.Make == value.Make) {
                        $scope.myPolicySearch.push({ Id: value.Id, SerialNo: value.SerialNo, Make: value.Make, Model: value.Model })
                    }
                });
            }
            if ($rootScope.searchBAndW.Model != "") {
                angular.copy($scope.myBAndWSearch, $scope.tempBAndWList);
                $scope.myBAndWSearch = [];
                angular.forEach($scope.tempBAndWList, function (value, key) {
                    if ($rootScope.searchBAndW.Model == value.Model) {
                        $scope.myPolicySearch.push({ Id: value.Id, SerialNo: value.SerialNo, Make: value.Make, Model: value.Model })
                    }
                });
            }
        }
        $scope.SetModelB = function () {
            $scope.errorTab1 = "";
            if ($scope.BAndW.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.BAndW.MakeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Modeles = data;
                    $scope.checkSwalClose('GetModelesByMakeId');
                    //$scope.loadContractExtension();
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetModelesByMakeId');
                });
            }
        }
        function clearBAndWControls() {
            $scope.Vin = false;
            $scope.VinLength = 0;
            $scope.BAndW.CommodityUsageTypeId = "";
            $scope.BAndW.Id = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.ItemPurchasedDate = "";
            $scope.BAndW.MakeId = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.ModelId = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.SerialNo = "";
            $scope.BAndW.ItemPrice = 0;
            $scope.BAndW.CategoryId = "";
            $scope.BAndW.ModelYear = "";
            $scope.BAndW.AddnSerialNo = "";
            $scope.BAndW.ItemStatusId = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.InvoiceNo = "";
            $scope.BAndW.ModelCode = "";
            $scope.BAndW.DealerPrice = 0;
            $scope.serialno = "";
            $scope.serialno1 = "";
            $scope.serialno2 = "";
            $scope.serialno3 = "";
        };
        function SetBAndWValues() {
            $scope.Vin = false;
            $scope.VinLength = 0;
            $scope.errorTab1 = "";
            $scope.mySelectedBAndWRows = $scope.gridApiBAndW.selection.getSelectedRows();
            angular.forEach($scope.mySelectedBAndWRows, function (value, key) {
                if (value.Id != "00000000-0000-0000-0000-000000000000") {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/BrownAndWhiteDetails/GetBrownAndWhiteDetailsById',
                        data: { "Id": value.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.BAndW = data;
                        $scope.BAndW.Id = data.Id;
                        $scope.BAndW.ItemPurchasedDate = data.ItemPurchasedDate;
                        $scope.BAndW.MakeId = data.MakeId;
                        $scope.SetModelB();
                        $scope.BAndW.ModelId = data.ModelId;
                        $scope.BAndW.SerialNo = data.SerialNo;
                        $scope.BAndW.ItemPrice = data.ItemPrice;
                        $scope.BAndW.CategoryId = data.CategoryId;
                        $scope.BAndW.ModelYear = data.ModelYear;
                        $scope.BAndW.AddnSerialNo = data.AddnSerialNo;
                        $scope.BAndW.ItemStatusId = data.ItemStatusId;
                        $scope.BAndW.InvoiceNo = data.InvoiceNo;
                        $scope.BAndW.ModelCode = data.ModelCode;
                        $scope.BAndW.DealerPrice = data.DealerPrice;
                        $scope.Policy.BAndW = $scope.BAndW;
                        $scope.Policy.ItemId = $scope.BAndW.Id;
                        $scope.Policy.Type = "B&W";
                        $scope.VINNoValidation();
                        $scope.VINNoValidate();
                    }).error(function (data, status, headers, config) {
                        clearBAndWControls();
                    });
                }
                else {
                    clearBAndWControls();
                }
            });
        }
        //--------------------Other Methods------------------------------------------//
        $scope.Approval = false;
        $scope.SetApproval = function () {
            if (!$scope.Approval) {
                if ($scope.PolicySearchPopup()) {
                    $scope.Approval = true;
                }
            }
            return false;
        }
        $scope.ApprovalEnable = function () {
            //if ($('#loading').length) {
            //    if ($scope.Policy.Id == '00000000-0000-0000-0000-000000000000') {
            //       // document.getElementById("loading").style.display = "block";
            //    }
            //    else
            //       // document.getElementById("loading").style.display = "none";
            //}

        }

        $scope.SetApproval();






        LoadDetails();
        function LoadDetails() {
            // swal({ title: "Loading ...", text: "Data Loading", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                async: false,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                $scope.Policy.CommodityTypeId = $scope.CommodityTypes[0].CommodityTypeId;
                $scope.CommodityTypeDesc = $scope.CommodityTypes[0].CommodityTypeDescription;

                $scope.SetCommodityTypeValue();
                //   swal.close();

            }).error(function (data, status, headers, config) {
                swal.close();
            });

            //loadInizialDataWithPageLoading();
            //loadInitialDataWithPageLoadingParalel();
            loadInizialDataWithPageLoadingPartByPart();

        }



        function loadInizialDataWithPageLoading() {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDocumentTypesByPageName',
                dataType: 'json',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].AttachmentSectionCode === "Customer") {
                        $scope.customerAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Item") {
                        $scope.itemAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Policy") {
                        $scope.policyAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Payment") {
                        $scope.paymentAttachmentTypes.push(data[i]);
                    }
                }
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityUsageTypes = data;

            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetPremiumBasedOns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.PremiumBasedOns = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetCurrencies',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Currencies = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUsers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.SalesPersons = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Payment/GetAllPaymentModes',
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.PaymentModes = data.PaymetModes;
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllNationalities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Nationalities = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomerTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CustomerTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UsageTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllIdTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.IdTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Customers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Vehicles = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllBrownAndWhiteDetails',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.BAndWs = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllCylinderCounts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CylinderCounts = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllDriveTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DriveTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.EngineCapacities = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllFuelTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.FuelTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleBodyTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.BodyTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Transmissions = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Aspirations = data;
            }).error(function (data, status, headers, config) {
            });

        }

        function loadInitialDataWithPageLoadingParalel() {

            ////  call multiple services data for reduce request queuing time in browser purpose
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationDataSerial',
                //url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationData',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //   console.log(data);

                if (data.hasOwnProperty("GetDocumentTypesByPageName")) {
                    $scope.setDocumentTypesByPageName(data.GetDocumentTypesByPageName);
                    console.log('initializing GetDocumentTypesByPageName', data.GetDocumentTypesByPageName);
                }

                if (data.hasOwnProperty("GetAllCommodityUsageTypes")) {
                    $scope.CommodityUsageTypes = data.GetAllCommodityUsageTypes;
                    console.log('initializing CommodityUsageTypes', $scope.CommodityUsageTypes);
                }

                if (data.hasOwnProperty("GetPremiumBasedOns")) {
                    $scope.PremiumBasedOns = data.GetPremiumBasedOns;
                    console.log('initializing PremiumBasedOns', $scope.PremiumBasedOns);
                }

                if (data.hasOwnProperty("GetCurrencies")) {
                    $scope.Currencies = data.GetCurrencies;
                    console.log('initializing Currencies', $scope.Currencies);
                }

                if (data.hasOwnProperty("GetUsers")) {
                    $scope.SalesPersons = data.GetUsers;
                    console.log('initializing SalesPersons', $scope.SalesPersons);
                }

                if (data.hasOwnProperty("GetAllDealers")) {
                    $scope.Dealers = data.GetAllDealers;
                    console.log('initializing Dealers', $scope.Dealers);
                }

                if (data.hasOwnProperty("GetAllPaymentModes")) {
                    $scope.PaymentModes = data.GetAllPaymentModes.PaymetModes;
                    console.log('initializing PaymentModes', $scope.PaymentModes);
                }

                if (data.hasOwnProperty("GetAllCountries")) {
                    $scope.Countries = data.GetAllCountries;
                    console.log('initializing Countries', $scope.Countries);
                }

                if (data.hasOwnProperty("GetAllNationalities")) {
                    $scope.Nationalities = data.GetAllNationalities;
                    console.log('initializing Nationalities', $scope.Nationalities);
                }

                if (data.hasOwnProperty("GetAllCustomerTypes")) {
                    $scope.CustomerTypes = data.GetAllCustomerTypes;
                    console.log('initializing CustomerTypes', $scope.CustomerTypes);
                }

                if (data.hasOwnProperty("GetAllUsageTypes")) {
                    $scope.UsageTypes = data.GetAllUsageTypes;
                    console.log('initializing UsageTypes', $scope.UsageTypes);
                }

                if (data.hasOwnProperty("GetAllIdTypes")) {
                    $scope.IdTypes = data.GetAllIdTypes;
                    console.log('initializing IdTypes', $scope.IdTypes);
                }
                if (data.hasOwnProperty("GetAllCustomers")) {
                    $scope.Customers = data.GetAllCustomers;
                    console.log('initializing Customers', $scope.Customers);
                }
                if (data.hasOwnProperty("GetAllVehicleDetails")) {
                    $scope.Vehicles = data.GetAllVehicleDetails;
                    console.log('initializing Vehicles', $scope.Vehicles);
                }

                if (data.hasOwnProperty("GetAllBrownAndWhiteDetails")) {
                    $scope.BAndWs = data.GetAllBrownAndWhiteDetails;
                    console.log('initializing BAndWs', $scope.BAndWs);
                }

                if (data.hasOwnProperty("GetAllItemStatuss")) {
                    $scope.ItemStatuss = data.GetAllItemStatuss;
                    console.log('initializing ItemStatuss', $scope.ItemStatuss);
                }

                if (data.hasOwnProperty("GetAllCylinderCounts")) {
                    $scope.CylinderCounts = data.CylinderCounts;
                    console.log('initializing CylinderCounts', $scope.CylinderCounts);
                }

                if (data.hasOwnProperty("GetAllDriveTypes")) {
                    $scope.DriveTypes = data.GetAllDriveTypes;
                    console.log('initializing DriveTypes', $scope.DriveTypes);
                }
                if (data.hasOwnProperty("GetAllEngineCapacities")) {
                    $scope.EngineCapacities = data.GetAllEngineCapacities;
                    console.log('initializing EngineCapacities', $scope.EngineCapacities);
                }

                if (data.hasOwnProperty("GetAllFuelTypes")) {
                    $scope.FuelTypes = data.GetAllFuelTypes;
                    console.log('initializing FuelTypes', $scope.FuelTypes);
                }


                if (data.hasOwnProperty("GetAllVehicleBodyTypes")) {
                    $scope.BodyTypes = data.GetAllVehicleBodyTypes;
                    console.log('initializing BodyTypes', $scope.BodyTypes);
                }


                if (data.hasOwnProperty("GetAllTransmissionTypes")) {
                    $scope.Transmissions = data.GetAllTransmissionTypes;
                    console.log('initializing Transmissions', $scope.Transmissions);
                }

                if (data.hasOwnProperty("GetAllVehicleAspirationTypes")) {
                    $scope.Aspirations = data.GetAllVehicleAspirationTypes;
                    console.log('initializing Aspirations', $scope.Aspirations);
                }

            }).error(function (data, status, headers, config) {
                console.log(data);
            });
            //// end  call multiple services data for reduce request queuing time in browser purpose



        }

        function loadInizialDataWithPageLoadingPartByPart() {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationDataSerialPart01',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //   console.log(data);

                if (data.hasOwnProperty("GetAllCommodityUsageTypes")) {
                    $scope.CommodityUsageTypes = data.GetAllCommodityUsageTypes;
                    //console.log('initializing CommodityUsageTypes', $scope.CommodityUsageTypes);
                }

                if (data.hasOwnProperty("GetPremiumBasedOns")) {
                    $scope.PremiumBasedOns = data.GetPremiumBasedOns;
                    //console.log('initializing PremiumBasedOns', $scope.PremiumBasedOns);
                }

                if (data.hasOwnProperty("GetCurrencies")) {
                    $scope.Currencies = data.GetCurrencies;
                   // console.log('initializing Currencies', $scope.Currencies);
                }


                if (data.hasOwnProperty("GetAllDealers")) {
                    $scope.Dealers = data.GetAllDealers;
                   // console.log('initializing Dealers', $scope.Dealers);
                }

                if (data.hasOwnProperty("GetAllPaymentModes")) {
                    $scope.PaymentModes = data.GetAllPaymentModes.PaymetModes;
                    //console.log('initializing PaymentModes', $scope.PaymentModes);
                }

                if (data.hasOwnProperty("GetAllCountries")) {
                    $scope.Countries = data.GetAllCountries;
                   // console.log('initializing Countries', $scope.Countries);
                }



            }).error(function (data, status, headers, config) {
                console.log(data);
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationDataSerialPart02',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {

                if (data.hasOwnProperty("GetAllNationalities")) {
                    $scope.Nationalities = data.GetAllNationalities;
                  //  console.log('initializing Nationalities', $scope.Nationalities);
                }

                if (data.hasOwnProperty("GetAllCustomerTypes")) {
                    $scope.CustomerTypes = data.GetAllCustomerTypes;
                   // console.log('initializing CustomerTypes', $scope.CustomerTypes);
                }

                if (data.hasOwnProperty("GetAllUsageTypes")) {
                    $scope.UsageTypes = data.GetAllUsageTypes;
                   // console.log('initializing UsageTypes', $scope.UsageTypes);
                }

                if (data.hasOwnProperty("GetAllIdTypes")) {
                    $scope.IdTypes = data.GetAllIdTypes;
                    //console.log('initializing IdTypes', $scope.IdTypes);
                }
                if (data.hasOwnProperty("GetAllCustomers")) {
                    $scope.Customers = data.GetAllCustomers;
                   // console.log('initializing Customers', $scope.Customers);
                }
                if (data.hasOwnProperty("GetAllVehicleDetails")) {
                    $scope.Vehicles = data.GetAllVehicleDetails;
                    //console.log('initializing Vehicles', $scope.Vehicles);
                }


            }).error(function (data, status, headers, config) {
                //console.log(data);

            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationDataSerialPart03',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //   console.log(data);
                if (data.hasOwnProperty("GetAllBrownAndWhiteDetails")) {
                    $scope.BAndWs = data.GetAllBrownAndWhiteDetails;
                    //console.log('initializing BAndWs', $scope.BAndWs);
                }

                if (data.hasOwnProperty("GetAllItemStatuss")) {
                    $scope.ItemStatuss = data.GetAllItemStatuss;
                   // console.log('initializing ItemStatuss', $scope.ItemStatuss);
                }

                if (data.hasOwnProperty("GetAllCylinderCounts")) {
                    $scope.CylinderCounts = data.CylinderCounts;
                   // console.log('initializing CylinderCounts', $scope.CylinderCounts);
                }

                if (data.hasOwnProperty("GetAllDriveTypes")) {
                    $scope.DriveTypes = data.GetAllDriveTypes;
                    //console.log('initializing DriveTypes', $scope.DriveTypes);
                }
                if (data.hasOwnProperty("GetAllEngineCapacities")) {
                    $scope.EngineCapacities = data.GetAllEngineCapacities;
                    //console.log('initializing EngineCapacities', $scope.EngineCapacities);
                }

                if (data.hasOwnProperty("GetAllFuelTypes")) {
                    $scope.FuelTypes = data.GetAllFuelTypes;
                   // console.log('initializing FuelTypes', $scope.FuelTypes);
                }



            }).error(function (data, status, headers, config) {
                console.log(data);
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ApiManagerForPolicyApproval/GetInizializationDataSerialPart04',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //   console.log(data);

                if (data.hasOwnProperty("GetAllVehicleBodyTypes")) {
                    $scope.BodyTypes = data.GetAllVehicleBodyTypes;
                   // console.log('initializing BodyTypes', $scope.BodyTypes);
                }


                if (data.hasOwnProperty("GetAllTransmissionTypes")) {
                    $scope.Transmissions = data.GetAllTransmissionTypes;
                   // console.log('initializing Transmissions', $scope.Transmissions);
                }

                if (data.hasOwnProperty("GetAllVehicleAspirationTypes")) {
                    $scope.Aspirations = data.GetAllVehicleAspirationTypes;
                   // console.log('initializing Aspirations', $scope.Aspirations);
                }

                if (data.hasOwnProperty("GetDocumentTypesByPageName")) {
                    $scope.setDocumentTypesByPageName(data.GetDocumentTypesByPageName);
                   // console.log('initializing GetDocumentTypesByPageName', data.GetDocumentTypesByPageName);
                }

                if (data.hasOwnProperty("GetUsers")) {
                    $scope.SalesPersons = data.GetUsers;
                   // console.log('initializing SalesPersons', $scope.SalesPersons);
                }

            }).error(function (data, status, headers, config) {
                console.log(data);
            });


        }

        $scope.setDocumentTypesByPageName = function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].AttachmentSectionCode === "Customer") {
                        $scope.customerAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Item") {
                        $scope.itemAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Policy") {
                        $scope.policyAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode === "Payment") {
                        $scope.paymentAttachmentTypes.push(data[i]);
                    }
                }
        }



        var PaymentValidated = true;
        $scope.ValidatePayment = function () {
            PaymentValidated = true;

            //if ($scope.selectedCustomerTypeName == "Corporate") {
            //    PaymentValidated = true;
            //} else {
            //    if (!isGuid($scope.Policy.PaymentModeId)) {
            //        PaymentValidated = false;
            //        //SweetAlert.swal({
            //        //    title: "Policy Information",
            //        //    text: "Please select a payment method!",
            //        //    confirmButtonColor: "#007AFF"
            //        //});
            //        customErrorMessage("Please select a payment method!");
            //    }

            //}



            //if (!$scope.Policy.IsSpecialDeal) {
            //    if ($scope.Policy.IsPartialPayment) {
            //        if (parseFloat($scope.Policy.CustomerPayment + $scope.Policy.DealerPayment) != parseFloat($scope.GrossTotal)) {
            //            PaymentValidated = false;
            //            //SweetAlert.swal({
            //            //    title: "Policy Information",
            //            //    text: "Payment Details Error!",
            //            //    confirmButtonColor: "#007AFF"
            //            //});
            //            customErrorMessage("Payment Details Error!");
            //        }
            //    }
            //    else {
            //        if (parseFloat($scope.Policy.DealerPayment) != parseFloat($scope.GrossTotal)) {
            //            PaymentValidated = false;
            //            //SweetAlert.swal({
            //            //    title: "Policy Information",
            //            //    text: "Dealer payment should be equal to the Premium!",
            //            //    confirmButtonColor: "#007AFF"
            //            //});
            //            customErrorMessage("Dealer payment should be equal to the Premium!");
            //        }
            //    }
            //}

            //else {
            //    var total = parseFloat($scope.Policy.DealerPayment) + ((parseFloat($scope.GrossTotal) - $scope.Policy.CustomerPayment) * $scope.Policy.Discount / 100.0)
            //    total = total + $scope.Policy.CustomerPayment;
            //    if (total != $scope.GrossTotal) {
            //        PaymentValidated = false;
            //        SweetAlert.swal({
            //            title: "Policy Information",
            //            text: "Payment Details Error!",
            //            confirmButtonColor: "#007AFF"
            //        });
            //    }
            //}
        }
        var PhoneCode = "";
        $scope.SetCountryValue = function () {
            if ($scope.Policy.Customer.CountryId != null) {
                angular.forEach($scope.Countries, function (valueC, key) {
                    if ($scope.Policy.Customer.CountryId == valueC.Id) {
                        PhoneCode = valueC.PhoneCode;
                        if ($scope.Customer.MobileNo == "") {
                            $scope.Customer.MobileNo = PhoneCode;
                        }
                        if ($scope.Customer.OtherTelNo == "") {
                            $scope.Customer.OtherTelNo = PhoneCode;
                        }
                    }

                });
                if ($scope.currentProductCodeTire == true) {
                    $scope.Policy.Customer.CountryId = $scope.Countries[0].Id;
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                    data: { "countryId": $scope.Policy.Customer.CountryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Cities = data;
                    $scope.checkSwalClose('GetAllCitiesByCountry');

                }).error(function (data, status, headers, config) {
                    $scope.Cities = null;
                    $scope.checkSwalClose('GetAllCitiesByCountry');
                });
            }
            else {
                $scope.Cities = null;
                $scope.checkSwalClose('GetAllCitiesByCountry');
            }
        }
        $scope.GetCommodityCatogatyDetails = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                data: { "Id": $scope.Policy.CommodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Categories = data;
                $scope.VINNoValidation();
                $scope.VINNoValidate();
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.SetCommodityTypeValue = function () {
            var result = filterByCommodityTypeId($scope.CommodityTypes, $scope.Policy.CommodityTypeId)
            if (result != null) {
                if (result.CommodityTypeDescription == "Automobile") {
                    $scope.vwVehicleDetails = true;
                    $scope.vwBAndWDetails = false;
                    $scope.Policy.Type = "Vehicle";
                } else if (result.CommodityTypeDescription == "Automotive") {
                    $scope.vwVehicleDetails = true;
                    $scope.vwBAndWDetails = false;
                    $scope.Policy.Type = "Vehicle";
                } else if (result.CommodityTypeDescription == "Electronic") {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = true;
                    $scope.Policy.Type = "B&W";
                } else if (result.CommodityTypeDescription == "Other") {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = true;
                    $scope.Policy.Type = "B&W";
                }
                else if (result.CommodityTypeDescription == "Bank") {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = false;
                    $scope.isProductILOE = true;
                    $scope.Policy.Type = "B&W";
                } else {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = true
                }
            }
            else {
                $scope.vwVehicleDetails = false;
                $scope.vwBAndWDetails = false;
            }
            if ($scope.Policy.CommodityTypeId != "") {
                var jwt = $cookieStore.get('jwt');
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.Policy.CommodityTypeId }
                }).success(function (data, status, headers, config) {
                    $scope.Products = data;
                    angular.forEach($scope.Products, function (value) {
                        if (value.Productcode == null || value.Productcode == "") {
                            value.Productcode = "";
                        }
                        else {
                            value.Productcode = value.Productcode + " - ";
                        }
                    });
                    $scope.checkSwalClose('GetAllProductsByCommodityTypeId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllProductsByCommodityTypeId');
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                    data: { "Id": $scope.Policy.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Makes = data;
                    $scope.checkSwalClose('GetAllMakesByComodityTypeId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllMakesByComodityTypeId');
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetAllVariant',
                    data: { "Id": $scope.Policy.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Variants = data;
                    $scope.checkSwalClose('GetAllVariant');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllVariant');
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                    data: { "Id": $scope.Policy.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Categories = data;
                    $scope.VINNoValidation();
                    $scope.checkSwalClose('GetAllCategories');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllCategories');
                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetContractsByCommodityTypeId',
                    data: { "Id": $scope.Policy.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.AllContracts = data;
                    //  $scope.Contracts = data;
                    $scope.checkSwalClose('GetContractsByCommodityTypeId');

                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetContractsByCommodityTypeId');
                });


            } else {
                $scope.checkSwalClose('GetAllProductsByCommodityTypeId');
                $scope.checkSwalClose('GetAllMakesByComodityTypeId');
                $scope.checkSwalClose('GetAllVariant');
                $scope.checkSwalClose('GetAllCategories');
                $scope.checkSwalClose('GetContractsByCommodityTypeId');
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
        $scope.ValidatePolicySoldDate = function () {
            angular.forEach($scope.AllContractExtensions, function (valueC) {
                if (valueC.ContractId == $scope.Policy.ContractId && valueC.ExtensionTypeId == $scope.Policy.ExtensionTypeId && valueC.WarrantyTypeId == $scope.Policy.CoverTypeId) {
                    if ($scope.Policy.PolicySoldDate < valueC.StartDate || $scope.Policy.PolicySoldDate > valueC.EndDate) {
                        $scope.PolicySoldDateError = "Contract Preriod: " + valueC.StartDate + "-" + valueC.EndDate;
                        SweetAlert.swal({
                            title: "Policy Information",
                            text: "Policy Sold Date should be within contract valid period!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else
                        $scope.PolicySoldDateError = "";
                }
            });
        }
        function filterByCommodityTypeId(input, id) {
            var i = 0, len = input.length;
            for (; i < len; i++) {
                if (input[i].CommodityTypeId == id) {
                    return input[i];
                }
            }
            return null;
        }
        function filterById(input, id) {
            var i = 0, len = input.length;
            for (; i < len; i++) {
                if (input[i].Id == id) {
                    return input[i];
                }
            }
            return null;
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
        $scope.SetCustomerTypeValue = function () {
            var result = filterById($scope.CustomerTypes, $scope.Customer.CustomerTypeId)
            if (result != null) {
                if (result.CustomerTypeName == "Corporate") {
                    $scope.CustomerTypeName = result.CustomerTypeName;
                    $scope.businessInfoDisplay = true;
                } else {
                    $scope.businessInfoDisplay = false;
                }
            }
            else {
                $scope.businessInfoDisplay = false;
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
                    $scope.checkSwalClose('GetAllDealerLocationsByDealerId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllDealerLocationsByDealerId');
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
                                        title: "TAS Information",
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
        $scope.loadContractExtension = function () {
            if ($scope.AllContracts == undefined || $scope.Products == undefined ||
                $scope.Dealers == undefined || $scope.Makes == undefined || $scope.Modeles == undefined) {
                return false;
            }
            if (
                ($scope.Policy.ProductId != "" && $scope.Policy.ProductId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.ProductId != undefined && $scope.Policy.DealerId != "" && $scope.Policy.DealerId != "00000000-0000-0000-0000-000000000000" && $scope.Policy.DealerId != undefined)
                &&
                (
                    ($scope.vwBAndWDetails && $scope.BAndW.MakeId != "" && $scope.BAndW.MakeId != "00000000-0000-0000-0000-000000000000" && $scope.BAndW.MakeId != undefined && $scope.BAndW.ModelId != "" && $scope.BAndW.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.BAndW.ModelId != undefined)
                    ||
                    ($scope.vwVehicleDetails && $scope.Vehicle.MakeId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.MakeId != "" && $scope.Vehicle.MakeId != undefined && $scope.Vehicle.ModelId != "" && $scope.Vehicle.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.ModelId != undefined && $scope.Vehicle.CylinderCountId != "" && $scope.Vehicle.CylinderCountId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.CylinderCountId != undefined && $scope.Vehicle.EngineCapacityId != "" && $scope.Vehicle.EngineCapacityId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.EngineCapacityId != undefined)
                )
            ) {
                var make = "";
                var model = "";
                var cc = "";
                var ec = "";
                if ($scope.vwBAndWDetails) {
                    make = $scope.BAndW.MakeId;
                    model = $scope.BAndW.ModelId;
                }
                if ($scope.vwVehicleDetails) {
                    make = $scope.Vehicle.MakeId;
                    model = $scope.Vehicle.ModelId;
                    cc = $scope.Vehicle.CylinderCountId;
                    ec = $scope.Vehicle.EngineCapacityId;
                }
                if ($scope.ProductContracts.length > 0) {
                    angular.forEach($scope.ProductContracts, function (valProd) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ContractManagement/GetContractsByIds',
                            data: {
                                //"ProductId": valProd.ProductId,
                                //"DealerId": $scope.Policy.DealerId,
                                //"MakeId": make,
                                //"ModelId": model,
                                //"CylinderCountId": cc,
                                //"EngineCapacityId": ec,
                                //"Date": $scope.Policy.PolicySoldDate

                                "ProductId": valProd.ProductId,
                                "DealerId": $scope.Policy.DealerId,
                                "MakeId": make,
                                "ModelId": model,
                                "VariantId": $scope.Vehicle.Variant,
                                "CylinderCountId": cc,
                                "EngineCapacityId": ec,
                                "Date": $scope.Policy.PolicySoldDate,
                                "grossWeight": $scope.Vehicle.GrossWeight,
                                "UsageTypeId": $scope.product.commodityUsageTypeId,
                                "ItemStatusId": $scope.Vehicle.ItemStatusId
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            valProd.Contracts = data;
                        }).error(function (data, status, headers, config) {
                        });
                    });
                }
            }
            else {
                //SweetAlert.swal({
                //    title: "Policy Information",
                //    text: "Please enter product details before policy details",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customErrorMessage("Please enter product details before policy details");
            }
        }
        $scope.LoadFromProduct = function () {
            $scope.GrossTotal = 0.0;
            $scope.ProductContracts = [];
            angular.forEach($scope.Products, function (ValP) {
                if (ValP.Id == $scope.Policy.ProductId) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Product/GetAllChildProducts',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { "Id": $scope.Policy.ProductId }
                    }).success(function (data, status, headers, config) {
                        $scope.ChildProducts = data;

                        angular.forEach($scope.ChildProducts, function (ValP) {
                            var ProductContract = {
                                ProductId: ValP.Id,
                                ParentProductId: $scope.Policy.ProductId,
                                ContractId: "00000000-0000-0000-0000-000000000000",
                                ExtensionTypeId: "00000000-0000-0000-0000-000000000000",
                                AttributeSpecificationId: '00000000-0000-0000-0000-000000000000',
                                CoverTypeId: "00000000-0000-0000-0000-000000000000",
                                Contracts: [],
                                ExtensionTypes: [],
                                CoverTypes: [],
                                Premium: 0,
                                PremiumCurrencyName: '',
                                PremiumCurrencyTypeId: "00000000-0000-0000-0000-000000000000",
                                Name: ValP.Productcode + ' - ' + ValP.Productname,
                                RSA: ValP.Productcode == "RSA" ? true : false
                            };
                            $scope.ProductContracts.push(ProductContract);
                        });
                    }).error(function (data, status, headers, config) {
                    });
                }
            });

        }
        //$scope.SetContractValue = function (Name) {
        //    angular.forEach($scope.ProductContracts, function (value) {
        //        if (value.Name == Name) {
        //            value.ExtensionTypes = [];
        //            value.CoverTypes = [];
        //            angular.forEach(value.Contracts, function (valueC) {
        //                if (valueC.Id == value.ContractId && !valueC.DiscountAvailable) {
        //                    $scope.discountAvailable = false;
        //                }
        //            });
        //            $http({
        //                method: 'POST',
        //                url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByContractId',
        //                data: { "Id": value.ContractId },
        //                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //            }).success(function (data, status, headers, config) {
        //                value.ExtensionTypes = data;
        //                //$scope.Policy.PremiumCurrencyTypeId = val1.PremiumCurrencyId;
        //                //angular.forEach($scope.Currencies, function (valueC) {
        //                //    if (val1.PremiumCurrencyId == valueC.Id)
        //                //        $scope.PremiumCurrencyName = valueC.Code;
        //                //});
        //                //angular.forEach($scope.AllExtensionTypes, function (val2) {
        //                //    if (val1.ExtensionTypeId == val2.Id) {
        //                //        var result = filterByCommodityTypeId($scope.CommodityTypes, $scope.Policy.CommodityTypeId)
        //                //        if (result != null) {
        //                //            if (result.CommodityTypeDescription == "Automobile") {
        //                //                val2.ExtensionName = val2.Month + " M - " + val2.Km + " Km";
        //                //                $scope.ExtensionTypes.push(val2);
        //                //            } else if (result.CommodityTypeDescription == "Other") {
        //                //                val2.ExtensionName = val2.Month + " M";
        //                //                $scope.ExtensionTypes.push(val2);
        //                //            } else {
        //                //                val2.ExtensionName = val2.Hours + " H";
        //                //                $scope.ExtensionTypes.push(val2);
        //                //            }
        //                //        }
        //                //    }
        //                //});
        //            }).error(function (data, status, headers, config) {
        //            });
        //        }
        //    });
        //}
        //$scope.SetExtensionTypeValue = function (Name) {
        //    angular.forEach($scope.ProductContracts, function (value) {
        //        if (value.Name == Name) {
        //            value.CoverTypes = [];
        //            if (value.RSA) {
        //                $http({
        //                    method: 'POST',
        //                    url: '/TAS.Web/api/ContractManagement/GetRSAProvidersByExtensionTypeId',
        //                    data: { "ContractId": value.ContractId, "ExtensionTypeId": value.ExtensionTypeId },
        //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //                }).success(function (data, status, headers, config) {
        //                    value.CoverTypes = data;
        //                }).error(function (data, status, headers, config) {
        //                });
        //            }
        //            else {
        //                $http({
        //                    method: 'POST',
        //                    url: '/TAS.Web/api/ContractManagement/GetWarrantyTypesByExtensionTypeId',
        //                    data: { "ContractId": value.ContractId, "ExtensionTypeId": value.ExtensionTypeId },
        //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //                }).success(function (data, status, headers, config) {
        //                    value.CoverTypes = data;
        //                }).error(function (data, status, headers, config) {
        //                });
        //            }
        //        }
        //    });
        //}
        $scope.SetCoverTypeValue = function (Name) {
            var tax = [];
            var commissions = [];
            $scope.GrossTotal = 0.0;
            $scope.GrossTotalTmp = 0.00;
            $scope.GrossTmpPaymentType = 0.00;
            angular.forEach($scope.ProductContracts, function (value) {
                if (value.Name == Name && value.CoverTypeId != undefined) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/GetContractExtensionByIds',
                        data: {
                            "ContractId": value.ContractId, "ExtensionTypeId": value.ExtensionTypeId, "WarrantyTypeId": value.CoverTypeId, "RSA": value.RSA,
                            "VariantId": $scope.product.variantId, "ModelId": $scope.product.modelId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        var ContractExtension = data;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                            data: { "Id": ContractExtension.PremiumCurrencyId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            value.PremiumCurrencyTypeId = data.Id;
                            value.PremiumCurrencyName = data.Code;
                            value.Premium = ContractExtension.PremiumTotal;
                            $scope.GrossTotal += ContractExtension.GrossPremium.toLocaleString("en-US");
                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/ContractManagement/GetCommissionTypesByContractId',
                            //    data: { "Id": value.ContractId },
                            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //}).success(function (data, status, headers, config) {
                            //    commissions = data;
                            //    $http({
                            //        method: 'POST',
                            //        url: '/TAS.Web/api/ContractManagement/GetCountryTaxesByContractId',
                            //        data: { "Id": value.ContractId },
                            //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //    }).success(function (data, status, headers, config) {
                            //        tax = data;
                            //        var premiumVal = 0.0;
                            //        value.Premium = 0.0;
                            //        angular.forEach($scope.PremiumBasedOns, function (valueP) {
                            //            if (ContractExtension.PremiumBasedOnId == valueP.Id) {
                            //                var RetailPrice = 0.0;
                            //                var DealerPrice = 0.0;
                            //                if ($scope.vwBAndWDetails) {
                            //                    if ($scope.BAndW.DealerPrice == "")
                            //                        $scope.BAndW.DealerPrice = 0.0;
                            //                    if ($scope.BAndW.ItemPrice == "")
                            //                        $scope.BAndW.ItemPrice = 0.0;
                            //                    if (
                            //                        //($scope.BAndW.DealerPrice == 0.0 && valueP.Code == "DP") ||
                            //                        ($scope.BAndW.ItemPrice == 0.0 && valueP.Code == "RP")) {
                            //                        SweetAlert.swal({
                            //                            title: "Policy Information",
                            //                            text: "Enter Price Details to display premium!",
                            //                            type: "warning",
                            //                            confirmButtonColor: "rgb(221, 107, 85)"
                            //                        });
                            //                        return;
                            //                    }
                            //                    else {
                            //                        DealerPrice = $scope.BAndW.DealerPrice;
                            //                        RetailPrice = $scope.BAndW.ItemPrice;
                            //                    }
                            //                }
                            //                else if ($scope.vwVehicleDetails) {
                            //                    if ($scope.Vehicle.DealerPrice == "")
                            //                        $scope.Vehicle.DealerPrice = 0.0;
                            //                    if ($scope.Vehicle.VehiclePrice == "")
                            //                        $scope.Vehicle.VehiclePrice = 0.0;
                            //                    if (($scope.Vehicle.DealerPrice == 0.0 && valueP.Code == "DP") || ($scope.Vehicle.VehiclePrice == 0.0 && valueP.Code == "RP")) {
                            //                        SweetAlert.swal({
                            //                            title: "Policy Information",
                            //                            text: "Enter Price Details to display premium!",
                            //                            type: "warning",
                            //                            confirmButtonColor: "rgb(221, 107, 85)"
                            //                        });
                            //                        return;
                            //                    }
                            //                    else {
                            //                        DealerPrice = $scope.Vehicle.DealerPrice;
                            //                        RetailPrice = $scope.Vehicle.VehiclePrice;
                            //                    }
                            //                }
                            //                if (valueP.Code == "UE") {
                            //                    premiumValm = ContractExtension.GrossPremium;
                            //                    $scope.PremiumDissabled = false;
                            //                }
                            //                else if (valueP.Code == "FV") {
                            //                    premiumVal = ContractExtension.GrossPremium;
                            //                    $scope.PremiumDissabled = true;
                            //                }
                            //                else if (valueP.Code == "RP") {
                            //                    premiumVal = RetailPrice * ContractExtension.PremiumTotal / 100.0;
                            //                    angular.forEach(commissions, function (val) {
                            //                        if (val.IsPercentage) {
                            //                            premiumVal = premiumVal + (val.Commission * premiumVal / 100.0);
                            //                        }
                            //                        else
                            //                            premiumVal = premiumVal + val.Commission;
                            //                    });
                            //                    angular.forEach(tax, function (val) {
                            //                        premiumVal = premiumVal + (val.TaxValue * premiumVal / 100.0);
                            //                    });
                            //                    if (premiumVal < ContractExtension.Min) {
                            //                        premiumVal = ContractExtension.Min;
                            //                    } else if (premiumVal > ContractExtension.Max) {
                            //                        premiumVal = ContractExtension.Max;
                            //                    }
                            //                    $scope.PremiumDissabled = true;
                            //                }
                            //                //else if (valueP.Code == "DP") {
                            //                //    premiumVal = DealerPrice * ContractExtension.PremiumTotal / 100.0;
                            //                //    angular.forEach(commissions, function (val) {
                            //                //        if (val.IsPercentage) {
                            //                //            premiumVal = premiumVal + (val.Commission * premiumVal / 100.0);
                            //                //        }
                            //                //        else
                            //                //            premiumVal = (premiumVal + val.Commission);
                            //                //    });
                            //                //    angular.forEach(tax, function (val) {
                            //                //        premiumVal = premiumVal + (val.TaxValue * premiumVal / 100.0);
                            //                //    });
                            //                //    if (premiumVal < ContractExtension.Min) {
                            //                //        premiumVal = ContractExtension.Min;
                            //                //    } else if (premiumVal > ContractExtension.Max) {
                            //                //        premiumVal = ContractExtension.Max;
                            //                //    }
                            //                //    $scope.PremiumDissabled = true;
                            //                //}
                            //            }
                            //        });
                            //        value.Premium = (parseFloat(premiumVal));
                            //        angular.forEach($scope.ProductContracts, function (value) {
                            //            var Age = 0;
                            //            if ($scope.vwVehicleDetails) {
                            //                Age = $scope.Vehicle.ItemPurchasedDate;
                            //                $http({
                            //                    method: 'POST',
                            //                    url: '/TAS.Web/api/ContractManagement/GetEligibilitiesByContractId',
                            //                    data: { "Id": value.ContractId, "Mileage": $scope.Policy.HrsUsedAtPolicySale, "Age": Age },
                            //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //                }).success(function (data, status, headers, config) {
                            //                    if (data.IsPercentage) {
                            //                        var val = value.Premium * data.Premium;
                            //                        if (data.PlusMinus == " + ")
                            //                            value.Premium = value.Premium + val;
                            //                        if (data.PlusMinus == " - ")
                            //                            value.Premium = value.Premium - val;
                            //                    }
                            //                    else {
                            //                        var val = data.Premium;
                            //                        if (data.PlusMinus == " + ")
                            //                            value.Premium = value.Premium + val;
                            //                        if (data.PlusMinus == " - ")
                            //                            value.Premium = value.Premium - val;
                            //                    }
                            //                    if (value.Premium > 0) {
                            //                        $scope.GrossTotal = value.Premium + $scope.GrossTotal;
                            //                    }
                            //                }).error(function (data, status, headers, config) {
                            //                });
                            //            }
                            //            if ($scope.vwBAndWDetails) {
                            //                Age = $scope.Policy.HrsUsedAtPolicySale;
                            //                $http({
                            //                    method: 'POST',
                            //                    url: '/TAS.Web/api/ContractManagement/GetEligibilitiesByContractId',
                            //                    data: { "Id": ContractId, "Mileage": $scope.Policy.HrsUsedAtPolicySale, "Age": Age },
                            //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //                }).success(function (data, status, headers, config) {
                            //                    if (data.IsPercentage) {
                            //                        var val = value.Premium * data.Premium;
                            //                        if (data.PlusMinus == " + ")
                            //                            value.Premium = value.Premium + val;
                            //                        if (data.PlusMinus == " - ")
                            //                            value.Premium = value.Premium - val;
                            //                    }
                            //                    else {
                            //                        var val = data.Premium;
                            //                        if (data.PlusMinus == " + ")
                            //                            value.Premium = value.Premium + val;
                            //                        if (data.PlusMinus == " - ")
                            //                            value.Premium = value.Premium - val;
                            //                    }
                            //                    if (value.Premium > 0) {
                            //                        $scope.GrossTotal = value.Premium + $scope.GrossTotal;
                            //                    }
                            //                }).error(function (data, status, headers, config) {
                            //                });
                            //            }

                            //        });

                            //    }).error(function (data, status, headers, config) {
                            //   });
                            //}).error(function (data, status, headers, config) {
                            //});
                        }).error(function (data, status, headers, config) {
                        });

                    }).error(function (data, status, headers, config) {
                    });
                }

            });

        } //calculations done here


        $scope.validateCustomerInfo = function () {
            var isValidated = true;

            if (!parseInt($scope.Customer.CustomerTypeId)) {
                isValidated = false;
                $scope.validate_customerTypeId = "has-error";
            } else {
                $scope.validate_customerTypeId = "";
            }

            if (!parseInt($scope.Customer.UsageTypeId)) {
                isValidated = false;
                $scope.validate_usageTypeId = "has-error";
            } else {
                $scope.validate_usageTypeId = "";
            }

            //if (!isGuid($scope.Customer.CountryId)) {
            //    isValidated = false;
            //    $scope.validate_countryId = "has-error";
            //} else {
            //    $scope.validate_countryId = "";
            //}

            //if (!isGuid($scope.Customer.CityId)) {
            //    isValidated = false;
            //    $scope.validate_cityId = "has-error";
            //} else {
            //    $scope.validate_cityId = "";
            //}

            if ($scope.Customer.MobileNo == "") {
                isValidated = false;
                $scope.validate_mobileNo = "has-error";
            } else {
                $scope.validate_mobileNo = "";
            }



            if ($scope.selectedCustomerTypeName === "Corporate") {
                if ($scope.Customer.BusinessName == "") {
                    isValidated = false;
                    $scope.validate_businessName = "has-error";
                } else {
                    $scope.validate_businessName = "";
                }

                if ($scope.Customer.BusinessTelNo == "") {
                    isValidated = false;
                    $scope.validate_businessTelNo = "has-error";
                } else {
                    $scope.validate_businessTelNo = "";
                }

            } else {
                if ($scope.Customer.FirstName == "") {
                    isValidated = false;
                    $scope.validate_firstName = "has-error";
                } else {
                    $scope.validate_firstName = "";
                }
                //if (!parseInt($scope.Customer.NationalityId)) {
                //    isValidated = false;
                //    $scope.validate_nationalityId = "has-error";
                //} else {
                //    $scope.validate_nationalityId = "";
                //}
            }
            return isValidated;
        }

        $scope.Submit = function () {

            if ($scope.Products[0].Productcode == "TYRE - ") {
                if (!$scope.validateCustomerInfo()) {
                    isValidated = false;
                    customErrorMessage("Please fill all mandatory fields before proceeding");
                    return;
                } else { }

                if ($scope.selectedCustomerTypeName == 'Corporate') {
                    $scope.Customer.Gender = "M";
                    $scope.Customer.DLIssueDate = "1/1/1753";
                    $scope.Customer.DateOfBirth = "1/1/1753";
                    $scope.Customer.IdTypeId = 1;
                    $scope.Customer.NationalityId = 0;
                }


                $scope.uploadAttachments();
                $scope.Policy.ContractProducts = $scope.ProductContracts;
                if ($scope.Policy.CustomerPayment == "")
                    $scope.Policy.CustomerPayment = 0.0;
                if ($scope.Policy.DealerPayment == "")
                    $scope.Policy.DealerPayment = 0.0;

            } else {
                if (!$scope.isUsedItem) {
                    $scope.ManufacturerWarranty.MWIsAvailable = true;
                } else { }

                if (!$scope.validateCustomerInfo()) {
                    isValidated = false;
                    customErrorMessage("Please fill all mandatory fields before proceeding");
                    return;
                } else { }

                if ($scope.vwBAndWDetails && ($scope.BAndW.CategoryId == "" || $scope.BAndW.SerialNo == ""
                    || $scope.BAndW.MakeId == "" || $scope.BAndW.ModelId == ""
                    //|| $scope.BAndW.ModelYear == ""
                    || $scope.BAndW.InvoiceNo == ""
                    || $scope.BAndW.ItemStatusId == "" || $scope.BAndW.ItemPurchasedDate == ""
                    //|| $scope.BAndW.DealerPrice == "" || $scope.BAndW.ItemPrice == "")
                )) {
                    //$scope.errorTab1 = "Please Enter Policy Details";
                    customErrorMessage("Please Enter Policy Details");
                    return;
                }
                if ($scope.vwVehicleDetails && ($scope.Vehicle.CategoryId == "" || $scope.Vehicle.VINNo == ""
                    || $scope.Vehicle.MakeId == "" || $scope.Vehicle.ModelId == ""
                    || $scope.Vehicle.ItemStatusId == "" || $scope.Vehicle.ModelYear == ""
                    || $scope.Vehicle.Variant == "" || $scope.Vehicle.EngineCapacityId == ""
                    || $scope.Vehicle.CylinderCountId == "" || $scope.Vehicle.FuelTypeId == ""
                    || $scope.Vehicle.TransmissionId == "" || $scope.Vehicle.DriveTypeId == ""
                    || $scope.Vehicle.BodyTypeId == "" || $scope.Vehicle.AspirationId == ""
                    || $scope.Vehicle.ItemPurchasedDate == "" || $scope.Vehicle.CommodityUsageTypeId == "" || $scope.Vehicle.CommodityUsageTypeId == "00000000-0000-0000-0000-000000000000"
                    //|| $scope.Vehicle.PlateNo == ""
                    || $scope.Vehicle.VehiclePrice == ""
                )) {
                    //$scope.errorTab1 = "Please Enter Policy Details";
                    customErrorMessage("Please Enter Policy Details");
                    return;
                }

                if ($scope.selectedCustomerTypeName == 'Corporate') {
                    $scope.Customer.Gender = "M";
                    $scope.Customer.DLIssueDate = "1/1/1753";
                    $scope.Customer.DateOfBirth = "1/1/1753";
                    $scope.Customer.IdTypeId = 1;
                    $scope.Customer.NationalityId = 0;
                }

                //if (!$scope.Vin)
                //    return false;


                $scope.uploadAttachments();
                $scope.Policy.ContractProducts = $scope.ProductContracts;
                if ($scope.Policy.CustomerPayment == "")
                    $scope.Policy.CustomerPayment = 0.0;
                if ($scope.Policy.DealerPayment == "")
                    $scope.Policy.DealerPayment = 0.0;
                $scope.ValidatePayment();
                $scope.Policy.MWIsAvailable = $scope.ManufacturerWarranty.MWIsAvailable;
                $scope.Policy.MWStartDate = $scope.ManufacturerWarranty.MWStartDate;

            }



        }

        $scope.savePolicyApprovalOtherTire = function () {

            if ($scope.NoOfDateNoOfDate > 30) {
                customErrorMessage("Your policy has been expired.");
                return false;
            }
            $scope.Policy.jwt = $localStorage.jwt;
            $scope.policySaveStatusTitle = "Processing...!";
            $scope.policySaveStatusMsg = "Policy approval you enterd is being processed.";
            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });

            if ($scope.Policy.Id != null && $scope.Policy.Id != "00000000-0000-0000-0000-000000000000" && $scope.Policy.Id != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/ApproveOtherTirePolicy',
                    data: $scope.Policy,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.Policies = undefined;
                    if (data == "OK") {

                        SweetAlert.swal({
                            title: "Policy Information",
                            text: "Successfully Saved and Approved!",
                            confirmButtonColor: "#007AFF"
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Vehicles = data;
                        }).error(function (data, status, headers, config) {
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetAllCustomers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Customers = data;
                        }).error(function (data, status, headers, config) {
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/PolicyReg/GetPolicies',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Policies = data;
                        }).error(function (data, status, headers, config) {
                        });
                        //$scope.rebindPolicySearchGrid();
                        clearPolicyControls();
                    } else {
                        SweetAlert.swal({
                            title: "Policy Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: "Policy Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    // document.getElementById("loading").style.display = "none";
                    return false;
                });
            }
            else {
                SweetAlert.swal({
                    title: "Policy Information",
                    text: "Cannot Approve New Policy: Select a policy from search!",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                //   document.getElementById("loading").style.display = "none";
                return false;
            }

        }

        $scope.savePolicyApproval = function () {

            $scope.policySaveStatusTitle = "Processing...!";
            $scope.policySaveStatusMsg = "Policy approval you enterd is being processed.";
            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });


            if ($scope.x) {
                $scope.Policy.IsApproved = true;
                $scope.Policy.ModifiedUser = $rootScope.LoggedInUserId;
                if (PaymentValidated) {
                    //document.getElementById("loading").style.display = "block";
                    SweetAlert.swal({
                        title: "Policy Information",
                        text: "Please Conform Policy Approval!",
                        confirmButtonColor: "#007AFF",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)",
                        showCancelButton: true,
                        confirmButtonClass: "btn-danger",
                        confirmButtonText: "Approve Policy.",
                        closeOnConfirm: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.Policy.Customer = $scope.Customer;
                                $scope.Policy.BAndW = $scope.BAndW;
                                $scope.Policy.Vehicle = $scope.Vehicle;
                                if ($scope.Policy.IsPreWarrantyCheck == 1)
                                    $scope.Policy.IsPreWarrantyCheck = true;
                                else
                                    $scope.Policy.IsPreWarrantyCheck = false;
                                if ($scope.Policy.Type == "Vehicle")
                                    $scope.Policy.BAndW.ItemPurchasedDate = "1900/10/10";
                                if ($scope.Policy.Type == "B&W") {
                                    $scope.Policy.Vehicle.ItemPurchasedDate = "1900/10/10";
                                    $scope.Policy.MWStartDate = "1900/10/10";
                                }

                                if ($scope.Policy.ItemId == "")
                                    $scope.Policy.ItemId = "00000000-0000-0000-0000-000000000000";
                                if ($scope.Policy.ProductId != "") {
                                    $scope.errorTab1 = "";
                                    if ($scope.Policy.Id != null && $scope.Policy.Id != "00000000-0000-0000-0000-000000000000" && $scope.Policy.Id != "") {
                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/PolicyReg/ApprovePolicy',
                                            data: $scope.Policy,
                                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            $scope.Ok = data;
                                            $scope.Policies = undefined;
                                            if (data == "OK") {
                                                SweetAlert.swal({
                                                    title: "Policy Information",
                                                    text: "Successfully Saved and Approved!",
                                                    confirmButtonColor: "#007AFF"
                                                });
                                                $http({
                                                    method: 'POST',
                                                    url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.Vehicles = data;
                                                }).error(function (data, status, headers, config) {
                                                });
                                                $http({
                                                    method: 'POST',
                                                    url: '/TAS.Web/api/Customer/GetAllCustomers',
                                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.Customers = data;
                                                }).error(function (data, status, headers, config) {
                                                });
                                                $http({
                                                    method: 'POST',
                                                    url: '/TAS.Web/api/PolicyReg/GetPolicies',
                                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                                }).success(function (data, status, headers, config) {
                                                    $scope.Policies = data;
                                                }).error(function (data, status, headers, config) {
                                                });
                                                //$scope.rebindPolicySearchGrid();
                                                clearPolicyControls();
                                            } else {
                                                SweetAlert.swal({
                                                    title: "Policy Information",
                                                    text: "Error occured while saving data!",
                                                    type: "warning",
                                                    confirmButtonColor: "rgb(221, 107, 85)"
                                                });
                                            }
                                            return false;
                                        }).error(function (data, status, headers, config) {
                                            SweetAlert.swal({
                                                title: "Policy Information",
                                                text: "Error occured while saving data!",
                                                type: "warning",
                                                confirmButtonColor: "rgb(221, 107, 85)"
                                            });
                                            // document.getElementById("loading").style.display = "none";
                                            return false;
                                        });
                                    }
                                    else {
                                        SweetAlert.swal({
                                            title: "Policy Information",
                                            text: "Cannot Approve New Policy: Select a policy from search!",
                                            type: "warning",
                                            confirmButtonColor: "rgb(221, 107, 85)"
                                        });
                                        //   document.getElementById("loading").style.display = "none";
                                        return false;
                                    }
                                }
                                else {
                                    SweetAlert.swal({
                                        title: "Policy Information",
                                        text: "Error occured while saving data!",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                    //document.getElementById("loading").style.display = "none";
                                    return false;
                                }
                            }
                            else {
                                //document.getElementById("loading").style.display = "none";
                                return false;
                            }
                        });
                }
                else {
                    customErrorMessage("Payment Details Validation Failed!");
                    return false;
                }
            }
            else {
                $scope.Policy.IsApproved = false;
                if (PaymentValidated) {
                    $scope.Policy.Customer = $scope.Customer;
                    $scope.Policy.BAndW = $scope.BAndW;
                    $scope.Policy.Vehicle = $scope.Vehicle;
                    if ($scope.Policy.IsPreWarrantyCheck == 1)
                        $scope.Policy.IsPreWarrantyCheck = true;
                    else
                        $scope.Policy.IsPreWarrantyCheck = false;
                    if ($scope.Policy.Type == "Vehicle")
                        $scope.Policy.BAndW.ItemPurchasedDate = "1900/10/10";
                    if ($scope.Policy.Type == "B&W") {
                        $scope.Policy.Vehicle.ItemPurchasedDate = "1900/10/10";
                        $scope.Policy.MWStartDate = "1900/10/10";
                    }

                    if ($scope.Policy.ItemId == "")
                        $scope.Policy.ItemId = "00000000-0000-0000-0000-000000000000";
                    if ($scope.Policy.ProductId != "") {
                        $scope.errorTab1 = "";
                        if ($scope.Policy.Id == null || $scope.Policy.Id == "00000000-0000-0000-0000-000000000000") {
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/PolicyReg/AddPolicy',
                                data: $scope.Policy,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.Policies = undefined;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: "Policy Information",
                                        text: "Successfully Saved!",
                                        confirmButtonColor: "#007AFF"
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Vehicles = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/Customer/GetAllCustomers',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Customers = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/PolicyReg/GetPolicies',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Policies = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    //$scope.rebindPolicySearchGrid();
                                    clearPolicyControls();
                                } else {
                                    SweetAlert.swal({
                                        title: "Policy Information",
                                        text: "Error occured while saving data!",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                }

                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: "Policy Information",
                                    text: "Error occured while saving data!",
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                return false;
                            });
                        }
                        else {

                            if ($scope.Policy.Type == "Vehicle")
                                $scope.Policy.BAndW.ItemPurchasedDate = "1900/10/10";
                            if ($scope.Policy.Type == "B&W") {
                                $scope.Policy.Vehicle.ItemPurchasedDate = "1900/10/10";
                                $scope.Policy.MWStartDate = "1900/10/10";
                            }
                            $scope.Policy.IsApproved = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/PolicyReg/UpdatePolicy',
                                data: $scope.Policy,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.Policies = undefined;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: "Policy Information",
                                        text: "Successfully Saved!",
                                        confirmButtonColor: "#007AFF"
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Vehicles = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/Customer/GetAllCustomers',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Customers = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/PolicyReg/GetPolicies',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Policies = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    //$scope.rebindPolicySearchGrid();
                                    clearPolicyControls();
                                } else {
                                    SweetAlert.swal({
                                        title: "Policy Information",
                                        text: "Error occured while saving data!",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                }
                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: "Policy Information",
                                    text: "Error occured while saving data!",
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                return false;
                            });
                        }
                    } else {
                        //$scope.errorTab1 = "Please Enter Policy Details";
                        customErrorMessage("Please Enter Policy Details");
                    }
                }
                else {
                    customErrorMessage("Please Enter Policy Details");
                    //$scope.errorTab1 = "Please Enter Policy Details";
                }
            }
        }

        $scope.SetContractValue = function (contract) {
            contract.ExtensionTypeId = emptyGuid();
            contract.AttributeSpecificationId = emptyGuid();
            contract.CoverTypeId = emptyGuid();
            contract.Premium = 0.00;

            $scope.emiContractId = contract.ContractId;
            $scope.getEMIValue();


            if (isGuid(contract.ContractId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByContractId',
                    data: {
                        "ContractId": contract.ContractId,
                        "ProductId": $scope.Policy.ProductId,
                        "DealerId": $scope.Policy.DealerId,
                        "MakeId": $scope.Vehicle.MakeId,
                        "ModelId": $scope.Vehicle.ModelId,
                        "VariantId": $scope.Vehicle.Variant,
                        "CylinderCountId": $scope.Vehicle.CylinderCountId,
                        "EngineCapacityId": $scope.Vehicle.EngineCapacityId,
                        "Date": $scope.Policy.PolicySoldDate,
                        "grossWeight": $scope.Vehicle.GrossWeight

                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    contract.ExtensionTypes = data;
                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.ExtensionTypes = [];
            }

        }

        $scope.SetExtensionTypeValue = function (contract) {
            contract.AttributeSpecificationId = emptyGuid();
            contract.CoverTypeId = emptyGuid();
            contract.Premium = 0.00;
            if (isGuid(contract.ExtensionTypeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetAttributeSpecificationByExtensionId',
                    data: {
                        "ExtensionId": contract.ExtensionTypeId,
                        "ContractId": contract.ContractId,
                        "ProductId": $scope.Policy.ProductId,
                        "DealerId": $scope.Policy.DealerId,
                        "MakeId": $scope.Vehicle.MakeId,
                        "ModelId": $scope.Vehicle.ModelId,
                        "VariantId": $scope.Vehicle.Variant,
                        "CylinderCountId": $scope.Vehicle.CylinderCountId,
                        "EngineCapacityId": $scope.Vehicle.EngineCapacityId,
                        "Date": $scope.Policy.PolicySoldDate,
                        "grossWeight": $scope.Vehicle.GrossWeight
                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    contract.AttributeSpecifications = data;
                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.AttributeSpecifications = [];
            }

        }

        $scope.SetAttributeSpecificationValue = function (contract) {
            contract.CoverTypeId = emptyGuid();
            contract.Premium = 0.00;
            if (isGuid(contract.AttributeSpecificationId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetCoverTypesByExtensionId',
                    data: {
                        "AttributeSpecificationId": contract.AttributeSpecificationId,
                        "ExtensionId": contract.ExtensionTypeId,
                        "ContractId": contract.ContractId,
                        "ProductId": $scope.Policy.ProductId,
                        "DealerId": $scope.Policy.DealerId,
                        "MakeId": $scope.Vehicle.MakeId,
                        "ModelId": $scope.Vehicle.ModelId,
                        "VariantId": $scope.Vehicle.Variant,
                        "CylinderCountId": $scope.Vehicle.CylinderCountId,
                        "EngineCapacityId": $scope.Vehicle.EngineCapacityId,
                        "Date": $scope.Policy.PolicySoldDate,
                        "grossWeight": $scope.Vehicle.GrossWeight,
                        "ItemStatusId": $scope.Vehicle.ItemStatusId
                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.length === 0)
                        customErrorMessage('Item status does not match with any cover type.');
                    contract.CoverTypes = data;
                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.CoverTypes = [];
            }
        }

        $scope.SetCoverTypeValue = function (contract) {
            contract.Premium = 0.00;

            $scope.emiContractId = contract.ContractId;
            $scope.dealerPrice = $scope.Vehicle.DealerPrice;
            $scope.getEMIValue();

            if (isGuid(contract.CoverTypeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetPremium',
                    data: {
                        "CoverTypeId": contract.CoverTypeId,
                        "AttributeSpecificationId": contract.AttributeSpecificationId,
                        "ExtensionId": contract.ExtensionTypeId,
                        "ContractId": contract.ContractId,
                        "Usage": $scope.Policy.HrsUsedAtPolicySale,
                        "ItemStatusId": $scope.Vehicle.ItemStatusId,

                        "DealerPrice": $scope.Vehicle.DealerPrice,
                        "ItemPurchasedDate": $scope.Vehicle.ItemPurchasedDate,

                        "ProductId": $scope.Policy.ProductId,
                        "DealerId": $scope.Policy.DealerId,
                        "MakeId": $scope.Vehicle.MakeId,
                        "ModelId": $scope.Vehicle.ModelId,
                        "VariantId": $scope.Vehicle.Variant,
                        "CylinderCountId": $scope.Vehicle.CylinderCountId,
                        "EngineCapacityId": $scope.Vehicle.EngineCapacityId,
                        "Date": $scope.Policy.PolicySoldDate,
                        "grossWeight": $scope.Vehicle.GrossWeight,


                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    // contract.CoverTypes = data;
                    if (data.Status === 'ok') {
                        contract.Premium = data.TotalPremium;
                        $scope.GrossTotal += parseFloat(data.TotalPremium);
                        $scope.DealerCurrencyName = data.Currency;
                        contract.Currency = data.Currency;
                        $scope.calculateAllPremiums();
                    } else {
                        customErrorMessage(data);
                    }

                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.Premium = 0.00;
            }
        }
        $scope.PremiumAfterPaymentFees = 0.00;

        //$scope.discountPercentageChanged = function () {
        //    if (parseFloat($scope.Policy.Discount)) {

        //        $scope.GrossTotal = (parseFloat($scope.Policy.Premium) -
        //            (($scope.Policy.Premium * 10) / 100)).toFixed(2);
        //        //$scope.GrossTmpPaymentType = $scope.GrossTotalTmp;

        //    } else {
        //        $scope.GrossTotal = $scope.PremiumAfterPaymentFees;

        //    }
        //}
        $scope.discountPercentageChanged = function () {
            if (parseFloat($scope.Policy.Discount)) {

                $scope.GrossTotal = (parseFloat($scope.PremiumAfterPaymentFees) *
                    ((100 - $scope.Policy.Discount) / 100)).toFixed(2);
                //$scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                $scope.GrossTotal = $scope.GrossTotal.toLocaleString("en-US");

            } else {
                $scope.GrossTotal = $scope.PremiumAfterPaymentFees.toLocaleString("en-US");
                $scope.GrossTotal = parseFloat($scope.GrossTotal).toLocaleString("en-US");
            }
        }
        $scope.selectedCommodityCategoryChanged = function () {

            $scope.Makes = [];
            $scope.Policy.Vehicle.SerialNumber = "";
            if (isGuid($scope.Policy.Vehicle.CategoryId)) {

                angular.forEach($scope.Categories, function (value) {

                    if ($scope.Policy.Vehicle.CategoryId === value.CommodityCategoryId) {
                        $scope.serialNumberLength = value.Length;
                        $scope.selectedItemCategory = value.CommodityCategoryDescription;
                        return false;
                    }
                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetMakesByCommodityCategoryId',
                    data: { "Id": $scope.Policy.Vehicle.CategoryId },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Makes = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.serialNumberLength = '';
            }
        }
        $scope.selectedPaymentModeChanged = function () {
            if (isGuid($scope.Policy.PaymentModeId)) {
                angular.forEach($scope.PaymentModes, function (paymentMode) {
                    if (paymentMode.Id === $scope.Policy.PaymentModeId) {
                        if (paymentMode.Code === 'CC')//credit card
                        {
                            $scope.isPaymentTypesAvailable = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Payment/GetAllPaymentTypesByPaymentModeId',
                                data: { PaymentModeId: $scope.Policy.PaymentModeId },
                                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.paymentTypes = data.PaymetTypes;
                                $scope.checkSwalClose('GetAllPaymentTypesByPaymentModeId');
                            }).error(function (data, status, headers, config) {
                                $scope.checkSwalClose('GetAllPaymentTypesByPaymentModeId');
                            });

                        } else {
                            $scope.isPaymentTypesAvailable = false;
                            $scope.PremiumAfterPaymentFeesAndDiscounts = parseFloat($scope.PremiumBeforePaymentFees).toFixed(2);
                            $scope.discountPercentageChanged();
                            $scope.checkSwalClose('GetAllPaymentTypesByPaymentModeId');
                        }
                    }
                });
                if ($scope.PaymentModes.length == 0) {
                    $scope.checkSwalClose('GetAllPaymentTypesByPaymentModeId');
                }
            } else {
                $scope.isPaymentTypesAvailable = false;
                $scope.PremiumAfterPaymentFeesAndDiscounts = parseFloat($scope.PremiumBeforePaymentFees).toFixed(2);
                $scope.discountPercentageChanged();
                $scope.checkSwalClose('GetAllPaymentTypesByPaymentModeId');
            }
        }
        $scope.pub_PaymentType = "Value";
        $scope.pub_PaymentCharg = 0.00;

        $scope.selectedPaymentTypeChanged = function () {

            if (isGuid($scope.Policy.PaymentTypeId)) {
                $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotalTmp).toFixed(2);
                angular.forEach($scope.paymentTypes, function (paymentType) {
                    if (paymentType.Id === $scope.Policy.PaymentTypeId) {
                        $scope.PremiumAfterPaymentFees = (
                            (parseFloat(paymentType.PaymentCharge) *
                                parseFloat($scope.PremiumBeforePaymentFees)
                            ) + parseFloat($scope.PremiumBeforePaymentFees)).toFixed(2);
                        $scope.discountPercentageChanged();
                        return false;
                    }
                });
            } else {
                $scope.PremiumAfterPaymentFees = parseFloat($scope.PremiumBeforePaymentFees).toFixed(2);
                $scope.discountPercentageChanged();
            }
        }

        $scope.calculateAllPremiums = function () {
            $scope.PremiumBeforePaymentFees = 0.00;
            $scope.PremiumAfterPaymentFees = 0.00;
            $scope.PremiumAfterPaymentFeesAndDiscounts = 0.00;

            angular.forEach($scope.ProductContracts, function (contract) {
                $scope.PremiumBeforePaymentFees = $scope.PremiumAfterPaymentFeesAndDiscounts = $scope.PremiumAfterPaymentFees += parseFloat(contract.Premium).toFixed(2);
            });

        }

        $scope.RemoveExistingAttachment = function (section, id) {
            // alert(id);
            if (isGuid(id)) {

                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, remove it!",
                    cancelButtonText: "No, cancel!",
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        if (section === "Customer") {
                            for (var i = 0; i < $scope.customerDocUploader.queue.length; i++) {
                                if ($scope.customerDocUploader.queue[i].id === id) {
                                    $scope.customerDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section === "Item") {
                            for (var i = 0; i < $scope.itemDocUploader.queue.length; i++) {
                                if ($scope.itemDocUploader.queue[i].id === id) {
                                    $scope.itemDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section === "Policy") {
                            for (var i = 0; i < $scope.policyDocUploader.queue.length; i++) {
                                if ($scope.policyDocUploader.queue[i].id === id) {
                                    $scope.policyDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section === "Payment") {
                            for (var i = 0; i < $scope.paymentDocUploader.queue.length; i++) {
                                if ($scope.paymentDocUploader.queue[i].id === id) {
                                    $scope.paymentDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        }
                    }
                });
            }
        }

        $scope.downloadAttachment = function (ref) {
            if (ref != '') {
                console.log("ref",ref);
                swal({ title: 'Processing...!', text: 'Preparing your download...', showConfirmButton: false });
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

        $scope.uploadAttachments = function () {
            if (!$scope.formAction) {
                for (var i = 0; i < $scope.attachments_temp.length; i++) {
                    var isRemoved = true;
                    if ($scope.attachments_temp[i].AttachmentSection === "Customer") {
                        for (var j = 0; j < $scope.customerDocUploader.queue.length; j++) {
                            //alert($scope.customerDocUploader.queue[j].ref);
                            if (typeof $scope.customerDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef === $scope.customerDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    //return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection === "Item") {

                        for (var j = 0; j < $scope.itemDocUploader.queue.length; j++) {
                            if (typeof $scope.itemDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef === $scope.itemDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection === "Policy") {
                        for (var j = 0; j < $scope.policyDocUploader.queue.length; j++) {
                            if (typeof $scope.policyDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef === $scope.policyDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection === "Payment") {
                        for (var j = 0; j < $scope.paymentDocUploader.queue.length; j++) {
                            if (typeof $scope.paymentDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef === $scope.paymentDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    }

                    if (isRemoved) {
                        $scope.removedAttachments.push($scope.attachments_temp[i].Id);
                    }
                }
                $scope.attachments_temp = [];
                //removing unwanted items for upload
                for (var j = 0; j < $scope.customerDocUploader.queue.length; j++) {
                    if (typeof $scope.customerDocUploader.queue[j].ref === 'undefined') {
                        //new records
                    } else {
                        $scope.customerDocUploader.queue.splice(j, 1);
                    }
                }

                for (var j = 0; j < $scope.itemDocUploader.queue.length; j++) {
                    if (typeof $scope.itemDocUploader.queue[j].ref === 'undefined') {
                        //new records
                    } else {
                        $scope.itemDocUploader.queue.splice(j, 1);
                    }
                }

                for (var j = 0; j < $scope.policyDocUploader.queue.length; j++) {
                    if (typeof $scope.policyDocUploader.queue[j].ref === 'undefined') {
                        //new records
                    } else {
                        $scope.policyDocUploader.queue.splice(j, 1);
                    }
                }

                for (var j = 0; j < $scope.paymentDocUploader.queue.length; j++) {
                    if (typeof $scope.paymentDocUploader.queue[j].ref === 'undefined') {
                        //new records
                    } else {
                        $scope.paymentDocUploader.queue.splice(j, 1);
                    }
                }
            }


            $scope.policySaveStatusTitle = "";
            $scope.policySaveStatusMsg = "";
            if ($scope.customerDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.customerDocUploader.queue.length; i++) {
                    $scope.customerDocUploader.queue[i].file.name = $scope.customerDocUploader.queue[i].file.name + '@@' + $scope.customerDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = "Uploading Customer Attachments...";
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.customerDocUploader.queue.length;
                $scope.customerDocUploader.uploadAll();
            } else if ($scope.itemDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.itemDocUploader.queue.length; i++) {
                    $scope.itemDocUploader.queue[i].file.name = $scope.itemDocUploader.queue[i].file.name + '@@' + $scope.itemDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = "Uploading Item Attachments...";
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.itemDocUploader.queue.length;
                $scope.itemDocUploader.uploadAll();
            } else if ($scope.policyDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.policyDocUploader.queue.length; i++) {
                    $scope.policyDocUploader.queue[i].file.name = $scope.policyDocUploader.queue[i].file.name + '@@' + $scope.policyDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = "Uploading Policy Attachments...";
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.policyDocUploader.queue.length;
                $scope.policyDocUploader.uploadAll();
            } else if ($scope.paymentDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.paymentDocUploader.queue.length; i++) {
                    $scope.paymentDocUploader.queue[i].file.name = $scope.paymentDocUploader.queue[i].file.name + '@@' + $scope.paymentDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = "Uploading Payment Attachments...";
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.paymentDocUploader.queue.length;
                $scope.paymentDocUploader.uploadAll();
            } else {
                //Upload Scanned Documents
                if ($scope.ImageArray.length > 0) {
                    swal({ title: "Uploading", text: "Uploading Scanned Images...", showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Upload/UploadScannedAttachment',
                        data: { "ImageArray": $scope.ImageArray },
                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        console.log(data);
                        angular.forEach(data, function (value) {
                            $scope.uploadedDocIds.push(value);
                        });
                        $scope.ImageArray = [];
                        swal.close();


                        if ($scope.Products[0].Productcode == "TYRE - ") {

                            $scope.savePolicyApprovalOtherTire();

                        } else {
                            $scope.savePolicyApproval();
                        }

                    }).error(function (data, status, headers, config) {


                    });

                } else {
                    if ($scope.Products[0].Productcode == "TYRE - ") {
                        $scope.savePolicyApprovalOtherTire();

                    } else {
                        $scope.savePolicyApproval();
                    }

                }


            }

        }

        $scope.selectedCustomerTypeIdChanged = function () {
            if (parseInt($scope.Policy.Customer.CustomerTypeId)) {
                angular.forEach($scope.CustomerTypes, function (value) {
                    if ($scope.Policy.Customer.CustomerTypeId == value.Id) {
                        $scope.selectedCustomerTypeName = value.CustomerTypeName;
                        return false;
                    }
                });
                if (parseInt($scope.Policy.Customer.CustomerTypeId)) {

                    angular.forEach($scope.CustomerTypes, function (value) {
                        if ($scope.Policy.Customer.CustomerTypeId == value.Id) {
                            if (value.CustomerTypeName == "Corporate") {
                                angular.forEach($scope.UsageTypes, function (valuec) {
                                    if (valuec.UsageTypeName == "Commercial") {
                                        $scope.Policy.Customer.UsageTypeId = valuec.Id;

                                    }
                                });

                                //angular.forEach($scope.CommodityUsageTypes, function (valuec) {
                                //    if (valuec.Name == "Commercial") {
                                //        $scope.product.commodityUsageTypeId = valuec.Id;

                                //    }
                                //});
                            }
                        }
                    });
                }

            } else {
                $scope.selectedCustomerTypeName = '';
            }
        }

        $scope.selectedUsageTypeChanged = function () {
            if ($scope.Policy.Customer.UsageTypeId > 0) {
                angular.forEach($scope.UsageTypes, function (value) {
                    if ($scope.Policy.Customer.UsageTypeId == value.Id) {
                        $scope.selectedUsageTypeName = value.UsageTypeName;
                        return false;
                    }
                });
            } else {
                $scope.selectedUsageTypeName = '';
            }
        }

        /*----------------------Scanner Implementation----------------------
            Insert Dynamsoft.WebTwainEnv.Load(); to Init Method
            Define global variables
        */

        $scope.scannerPopUp = function (tabid) {
            $scope.tabId = tabid;
            // validation for selecting attachment type
            console.log($scope.customerAttachmentType);
            if ($scope.customerAttachmentType == null || $scope.customerAttachmentType == 'undefined' || $scope.customerAttachmentType == "") {
                customErrorMessage("Please Select Attachment Type");
                return false;
            }
            ScannerPopUp = ngDialog.open({
                template: 'imageScannerPopUp',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        }

        $scope.getSelectedImageFromScanner = function () {
            //$scope.customerAttachmentType = 0; //remove this
            console.log($scope.customerAttachmentType);
            var DocattachmentType;

            console.log("Tab Id " + $scope.tabId);
            var page = 'PolicyReg'; var section;
            if ($scope.tabId == 0) {
                section = 'Customer';
                DocattachmentType = $scope.customerAttachmentType; console.log(DocattachmentType);
            } else if ($scope.tabId == 1) {
                DocattachmentType = "";
                section = 'Item';
                DocattachmentType = $scope.itemAttachmentType; console.log(DocattachmentType);
            } else if ($scope.tabId == 2) {
                DocattachmentType = "";
                section = 'Policy';
                DocattachmentType = $scope.policyAttachmentType; console.log(DocattachmentType);
            } else if ($scope.tabId == 3) {
                DocattachmentType = "";
                section = 'Payment';
                DocattachmentType = $scope.paymentAttachmentType; console.log(DocattachmentType);
            }

            $scope.ImageBatch = {
                documentName: $scope.attachmentData.name,
                attachmentType: DocattachmentType,
                page: page,
                section: section,
                encodedImage: []
            };

            var imagecount = DWObject.HowManyImagesInBuffer;
            console.log('there are' + imagecount + 'in buffer');
            if (imagecount > 0 && imagecount != 'undefined') {
                for (var i = 0; i < imagecount; i++) {
                    var imagedata;
                    DWObject.SelectedImagesCount = imagecount;
                    DWObject.SetSelectedImageIndex(0, i);
                    DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                    imagedata = DWObject.SaveSelectedImagesToBase64Binary();
                    $scope.ImageBatch.encodedImage.push(imagedata);
                }
                $scope.ImageArray.push($scope.ImageBatch);
                console.log($scope.ImageArray);
                Dynamsoft.WebTwainEnv.Unload();
                $scope.attachmentData.name = '';
            }
        }

        $scope.clearScannedImageSections = function (sectionName) {
            for (var i = $scope.ImageArray.length - 1; i >= 0; i--) {
                if ($scope.ImageArray[i].section == sectionName)
                    $scope.ImageArray.splice(i, 1);
            }
        }

        $scope.removeAttachmentFromImageArray = function (index) {
            if (index > -1) {
                $scope.ImageArray.splice(index, 1);
            }
        }

        $scope.Dynamsoft_OnReady = function () {
            // Dynamsoft.WebTwainEnv.Unload();
            Dynamsoft.WebTwainEnv.Load();

            var OnAcquireImageSuccess, OnAcquireImageFailure;
            OnAcquireImageSuccess = OnAcquireImageFailure = function () {
                DWObject.Capability = EnumDWT_Cap.ICAP_AUTODISCARDBLANKPAGES;
                DWObject.CapType = EnumDWT_CapType.TWON_ONEVALUE;
                DWObject.CapValue = -1;//Auto
                if (DWObject.CapSet) {
                    console.log(DWObject.CapSet);
                }
                DWObject.CloseSource();
            };


            setTimeout(function () {
                DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
                // console.log(DWObject);
                if (DWObject) {

                    DWObject.SelectSource();
                    DWObject.CloseSource();
                    DWObject.OpenSource();
                    if (DWObject.Duplex != 0)
                        DWObject.IfDuplexEnabled = true;//Enable duplex
                    DWObject.IfShowUI = true; //hide the user interface of the source
                    DWObject.IfAutoDiscardBlankpages = true;
                    DWObject.IfFeederEnabled = true; // Use the document feeder to scan in batches
                    DWObject.IfDisableSourceAfterAcquire = true;
                    DWObject.IfShowIndicator = false;
                    DWObject.AcquireImage(OnAcquireImageSuccess, OnAcquireImageFailure);

                }
            }, 1000);
        }

        $scope.AcquireImage = function () {
            Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', $scope.Dynamsoft_OnReady());
        }



        $scope.uploadScannerDocuments = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Upload/UploadScannedAttachment',
                data: { "ImageArray": $scope.ImageArray },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                console.log(data);
                angular.forEach(data, function (value) {
                    $scope.uploadedDocIds.push(value);
                });
            }).error(function (data, status, headers, config) {
                swal.close();

            });

        }

        $scope.closedialog = function () {
            ScannerPopUp.close();
        }



        $scope.selectedItemStatus = function () {

            if ($scope.customer.nationalityId == emptyGuid()) {
                angular.forEach($scope.itemStatuses, function (value) {
                    if ($scope.product.itemStatusId === value.Id) {
                        $scope.isAvalable = false;
                        return false;
                    } else {
                        $scope.isAvalable = true;
                    }
                });
            } else {
                $scope.isAvalable = false;
            }

        }

        $scope.selectedModelChange = function () {
            $scope.Variants = [];
            $scope.Policy.BAndW.VariantId = emptyGuid();
            if (isGuid($scope.Policy.BAndW.ModelId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                    data: { "Id": $scope.Policy.BAndW.ModelId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Variants = data;
                    //if (!$scope.formAction) {
                    //    $scope.policySoldDateChanged();
                    //}
                    $scope.checkSwalClose('GetAllVariantByModelId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllVariantByModelId');
                });
            }
        }


    });



