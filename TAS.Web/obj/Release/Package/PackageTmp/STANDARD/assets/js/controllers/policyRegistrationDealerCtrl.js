app.controller('PolicyRegDealerCtrl',
    function ($scope, $rootScope, $http, $parse, ngDialog, SweetAlert, $location, $localStorage,
        $cookieStore, $filter, toaster, $state, FileUploader, $translate) {
        $scope.currentStep = 1;
        $scope.currentUploadingItem = 0;
        $scope.userType = '';
        $scope.formActionText = "Registering for new Policy";
        $scope.formAction = true;//true for add new
        $scope.formActionCustomer = true;
        $scope.selectedCustomerTypeName = '';
        $scope.selectedUsageTypeName = '';
        $scope.serialNumberLength = '';
        $scope.currentCommodityTypeCode = '';
        $scope.currentProductCode = '';
        $scope.discountAvailable = true;
        $scope.isPaymentTypesAvailable = false;
        $scope.isOneBranchAvailable = false;
        $scope.isProductDetailsReadonly = false;
        $scope.isPolicyDetailsReadonly = false;
        $scope.customerDisabled = false;
        $scope.IsCustomerNoExist = false
        $scope.isHeaderSectionReadonly = false;
        $scope.isCommodityUsageTypeFreez = false;
        //$scope.isEligible = false;
        $scope.updateMode = false;
        $scope.GrossTotal = 0.0;
        $scope.serialNumber_temp = '';
        $scope.attachmentData = {};
        var SearchCustomerPopup, SearchVehiclePopup, SearchElectronicPopup, ViewSumaryPopup, SearchPolicyPopup, SearchOtherPopup, SearchYellowGoodPopup;
        //master data
        $scope.customerTypes = [];
        $scope.usageTypes = [];
        $scope.nationalities = [];
        $scope.idTypes = [];
        $scope.countries = [];
        $scope.cities = [];
        $scope.commodityTypes = [];
        $scope.products = [];
        $scope.dealers = [];
        $scope.dealerLocation = [];
        $scope.commodityCategories = [];
        $scope.makes = [];
        $scope.models = [];
        $scope.itemStatuses = [];
        $scope.commodityUsageTypes = [];
        $scope.variants = [];
        $scope.engineCapacities = [];
        $scope.cylinderCounts = [];
        $scope.fuelTypes = [];
        $scope.transmissionTypes = [];
        $scope.bodyTypes = [];
        $scope.aspirationTypes = [];
        $scope.productContracts = [];
        $scope.salesPersons = [];
        $scope.paymentModes = [];
        $scope.currencies = [];
        $scope.PaymentTypes = [];
        $scope.allowedTpaBranches = [];
        $scope.customerAttachmentTypes = [];
        $scope.itemAttachmentTypes = [];
        $scope.policyAttachmentTypes = [];
        $scope.paymentAttachmentTypes = [];
        $scope.uploadedDocIds = [];
        $scope.isProductILOE = false;
        $scope.emiContractId = "00000000-0000-0000-0000-000000000000";
        $scope.customerResetforComodityTypeChenge = false;
        $scope.policyDetailsPopupKm = '';
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
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Customer' },
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
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadingAttachments'));
                $scope.customerDocUploader.cancelAll();
            }
        }
        $scope.customerDocUploader.onCompleteAll = function () {
            $scope.customerDocUploader.queue = [];
            $scope.uploadAttachments();
        }
        $scope.customerDocUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                if ($scope.customerAttachmentType != "" && typeof $scope.customerAttachmentType != 'undefined') {
                    if (item.size > 3000000) {
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.maxSize'));
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.attachmentType'));
                    return false;
                }


            }
        });

        $scope.itemDocUploader = new FileUploader({
            url: '/TAS.Web/api/Upload/UploadAttachment',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Item' },
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
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadingAttachments'));
                $scope.itemDocUploader.cancelAll();
            }
        }
        $scope.itemDocUploader.onCompleteAll = function () {
            $scope.itemDocUploader.queue = [];
            $scope.uploadAttachments();
        }
        $scope.itemDocUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                if ($scope.itemAttachmentType != "" && typeof $scope.itemAttachmentType != 'undefined') {

                    if (item.size > 3000000) {
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.maxSize'));
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.attachmentType'));
                    return false;
                }
            }
        });


        $scope.policyDocUploader = new FileUploader({
            url: '/TAS.Web/api/Upload/UploadAttachment',
            headers: {
                'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt,
                'Page': 'PolicyReg', 'Section': 'Policy'
            },
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
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadingAttachments'));
                $scope.policyDocUploader.cancelAll();
            }
        }
        $scope.policyDocUploader.onCompleteAll = function () {
            $scope.policyDocUploader.queue = [];
            $scope.uploadAttachments();
        }
        $scope.policyDocUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                if ($scope.policyAttachmentType != "" && typeof $scope.policyAttachmentType != 'undefined') {

                    if (item.size > 3000000) {
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.maxSize'));
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.attachmentType'));
                    return false;
                }
            }
        });




        $scope.paymentDocUploader = new FileUploader({
            url: '/TAS.Web/api/Upload/UploadAttachment',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Payment' },
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
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadingAttachments'));
                $scope.paymentDocUploader.cancelAll();
            }
        }
        $scope.paymentDocUploader.onCompleteAll = function () {
            $scope.paymentDocUploader.queue = [];
            $scope.uploadAttachments();
        }
        $scope.paymentDocUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                if ($scope.paymentAttachmentType != "" && typeof $scope.paymentAttachmentType != 'undefined') {
                    if (item.size > 3000000) {
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.maxSize'));
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.attachmentType'));
                    return false;
                }
            }

        });
        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        var validateEmail = function (email) {
            if (email == "") return false;
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }


        $scope.getMaxDate = function () {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            if (dd < 10) {
                dd = '0' + dd
            }

            if (mm < 10) {
                mm = '0' + mm
            }
            return yyyy + '-' + mm + '-' + dd;
        }
        //mvvm variables
        $scope.customer = {
            customerId: emptyGuid(),
            customerTypeId: emptyGuid(),
            usageTypeId: emptyGuid(),
            firstName: '',
            lastName: '',
            dateOfBirth: '',
            gender: 'M',
            idTypeId: 1,
            idNo: '',
            idIssueDate: '',
            nationalityId: emptyGuid(),
            countryId: emptyGuid(),
            cityId: emptyGuid(),
            mobileNo: '',
            otherTelNo: '',
            email: '',
            address1: '',
            address2: '',
            address3: '',
            address4: '',
            businessName: '',
            businessTelNo: '',
            businessAddress1: '',
            businessAddress2: '',
            businessAddress3: '',
            businessAddress4: '',
            PostalCode: ''
        };
        $scope.product = {
            id: emptyGuid(),
            commodityTypeId: emptyGuid(),
            productId: emptyGuid(),
            dealerId: emptyGuid(),
            dealerLocationId: emptyGuid(),
            categoryId: emptyGuid(),
            serialNumber: '',
            makeId: emptyGuid(),
            modelId: emptyGuid(),
            modelYear: '',
            invoiceNo: '',
            additionalSerial: '',
            itemStatusId: emptyGuid(),
            commodityUsageTypeId: emptyGuid(),
            itemPurchasedDate: GetToday(),
            dealerPrice: '',
            itemPrice: '',
            variantId: emptyGuid(),
            engineCapacityId: emptyGuid(),
            cylinderCountId: emptyGuid(),
            fuelTypeId: emptyGuid(),
            transmissionTypeId: emptyGuid(),
            bodyTypeId: emptyGuid(),
            aspirationTypeId: emptyGuid(),
            dealerPaymentCurrencyTypeId: emptyGuid(),
            customerPaymentCurrencyTypeId: emptyGuid(),
            registrationDate: '',
            grossWeight: 0,
            MWStartDate: '',
            MWIsAvailable: false,
            driveTypeId: emptyGuid()

        };
        $scope.product.MWStartDate = GetToday();
        $scope.product.MWStartDate = '';
        $scope.product.registrationDate = GetToday();
        $scope.product.registrationDate = '';

        $scope.policy = {
            tpaBranchId: emptyGuid(),
            id: emptyGuid(),
            policySoldDate: '',
            hrsUsedAtPolicySale: 0,
            salesPersonId: emptyGuid(),
            dealerPolicy: 'true',
        };
        $scope.payment = {
            refNo: '',
            isSpecialDeal: false,
            discount: 0.00,
            dealerPayment: 0.00,
            isPartialPayment: false,
            paymentModeId: emptyGuid(),
            paymentTypeId: emptyGuid(),
            customerPayment: 0.00,
            comment: ''
        };
        $scope.customerSearchGridloading = false;
        $scope.customerGridloadAttempted = false;
        $scope.customerSearchGridSearchCriterias = {
            firstName: "",
            lastName: "",
            eMail: "",
            mobileNo: ""
        };

        $scope.bnWSearchGridSearchCriterias = {
            serialNo: "",
            make: emptyGuid(),
            model: emptyGuid()
        };
        $scope.bnWSearchGridloading = false;
        $scope.bnWGridloadAttempted = false;

        $scope.currencyPeriodCheck = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/CurrencyPeriodCheck',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == true) {
                    $scope.validityCheck();
                } else if (data == false) {
                    swal({ title: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.securityInformation'), text: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.curencyPeriodDefined'), showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    swal({ title: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.securityInformation'), text: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.errorCurencyPeriodDefined'), showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                }

            }).error(function (data, status, headers, config) {
            });

        }
        $scope.validityCheck = function () {
            swal({ title: $filter('translate')('pages.dealerPolicyRegistration.loadingData'), text: $filter('translate')('pages.dealerPolicyRegistration.pleaseWait'), showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDealerAccessByUserId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId },
            }).success(function (data, status, headers, config) {
                if (data == '' || data == null) {
                    swal({ title: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.redirecting'), text: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.dontAccess'), showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    if (data == 'NoMapping') {
                        swal({ title: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.redirecting'), text: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.notAssignedDealer'), showConfirmButton: false });
                        setTimeout(function () { swal.close(); }, 5000);
                        $state.go('app.dashboard');
                    } else if (data == 'Internal') {
                        $scope.loadInitialData();
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.dealers = data;
                            $scope.product.dealerId = data[0].Id;
                            $scope.selectedDealerChanged(true);
                        }).error(function (data, status, headers, config) {
                        });
                    } else {
                        $scope.loadInitialData();
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealersByUserId',
                            data: { "Id": $localStorage.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.dealers = data.Dealers;
                            $scope.product.dealerId = data.Dealers[0].Id;
                            $scope.selectedDealerChanged(false);
                        }).error(function (data, status, headers, config) {
                        });
                    }

                }
                swal.close();
                //$scope.countries = data;
            }).error(function (data, status, headers, config) {
                swal.close();
            });

        }
        $scope.loadInitialData = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
                if (data.length == 1) {
                    $scope.customer.countryId = data[0].Id;
                    $scope.selectedCountryChanged();
                }
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllNationalities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.nationalities = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomerTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.customerTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.usageTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllIdTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.idTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
                if (data.length == 1) {
                    $scope.product.commodityTypeId = data[0].CommodityTypeId;
                } else {
                    $scope.product.commodityTypeId = data[2].CommodityTypeId;
                }

                $scope.selectedCommodityTypeChanged(true);
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.itemStatuses = data;

                if (data.length == 1) {
                    $scope.product.itemStatusId = data[0].Id;
                } else {
                    $scope.product.itemStatusId = data[2].Id;
                }
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
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityUsageTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.engineCapacities = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllCylinderCounts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.cylinderCounts = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllFuelTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.fuelTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.transmissionTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleBodyTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bodyTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.aspirationTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUsers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.salesPersons = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Payment/GetAllPaymentModes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.paymentModes = data.PaymetModes;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetCurrencies',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.currencies = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDocumentTypesByPageName',
                dataType: 'json',
                data: { PageName: 'PolicyReg' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].AttachmentSectionCode == "Customer") {
                        $scope.customerAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode == "Item") {
                        $scope.itemAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode == "Policy") {
                        $scope.policyAttachmentTypes.push(data[i]);
                    } else if (data[i].AttachmentSectionCode == "Payment") {
                        $scope.paymentAttachmentTypes.push(data[i]);
                    }
                }
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/UserValidationClaimListing',
                data: { "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.status == 'ok') {
                    $scope.userType = data.type;
                    //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                } else {

                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
            });
        }


        $scope.selectedNationaltyChanged = function () {
            if ($scope.customer.nationalityId > 0) {
                angular.forEach($scope.nationalities, function (value) {
                    if ($scope.customer.nationalityId == value.Id) {
                        $scope.selectedNationaltyName = value.NationalityName;
                        return false;
                    }
                });
            } else {
                $scope.selectedNationaltyName = '';
            }
        }


        $scope.selectedIdTypeChanged = function () {
            if (parseInt($scope.customer.idTypeId)) {
                angular.forEach($scope.idTypes, function (value) {
                    if ($scope.customer.idTypeId == value.Id) {
                        $scope.selectedIdTypeNameName = value.IdTypeName;
                        return false;
                    }
                });
            } else {
                $scope.selectedUsageTypeName = '';
            }
        }
        $scope.selectedUsageTypeChanged = function () {
            if ($scope.customer.usageTypeId > 0) {
                angular.forEach($scope.usageTypes, function (value) {
                    if ($scope.customer.usageTypeId == value.Id) {
                        $scope.selectedUsageTypeName = value.UsageTypeName;
                        return false;
                    }
                });
            } else {
                $scope.selectedUsageTypeName = '';
            }
        }
        $scope.selectedCustomerTypeIdChanged = function () {

            $scope.resetWithCustomerTypes();
            if (parseInt($scope.customer.customerTypeId)) {
                angular.forEach($scope.customerTypes, function (value) {
                    if ($scope.customer.customerTypeId == value.Id) {
                        $scope.selectedCustomerTypeName = value.CustomerTypeName;
                        return false;
                    }
                });
                if (parseInt($scope.customer.customerTypeId)) {

                    angular.forEach($scope.customerTypes, function (value) {
                        if ($scope.customer.customerTypeId == value.Id) {

                            if (value.CustomerTypeName == "Corporate") {
                                $scope.isCommodityUsageTypeFreez = true;
                                angular.forEach($scope.usageTypes, function (valuec) {
                                    if (valuec.UsageTypeName == "Commercial") {
                                        $scope.customer.usageTypeId = valuec.Id;


                                    }
                                });


                            }
                            else if (value.CustomerTypeName == "Individual") {
                                $scope.isCommodityUsageTypeFreez = true;
                                angular.forEach($scope.usageTypes, function (valuec) {
                                    if (valuec.UsageTypeName == "Private") {
                                        $scope.customer.usageTypeId = valuec.Id;

                                    }
                                });

                            }
                            else {
                                $scope.isCommodityUsageTypeFreez = false;
                            }
                        }
                    });
                }


                if ($scope.customer.customerTypeId == 1) {
                    angular.forEach($scope.commodityUsageTypes, function (val) {
                        if (val.Name == "Commercial") {
                            $scope.product.commodityUsageTypeId = val.Id;
                            $scope.customerResetforComodityTypeChenge = true;
                        }

                    });
                }
                else if ($scope.customer.customerTypeId == 2) {


                    angular.forEach($scope.commodityUsageTypes, function (val) {
                        if (val.Name == "Private") {
                            $scope.product.commodityUsageTypeId = val.Id;
                            console.log($scope.customer.usageTypeId);
                            $scope.customerResetforComodityTypeChenge = true;
                        }

                    });

                }

                if (!$scope.customerResetforComodityTypeChenge) {
                    $scope.customerReset();
                }

            } else {
                $scope.selectedCustomerTypeName = '';
            }

            console.log($scope.customer.usageTypeId);

        }
        $scope.selectedModelChange = function () {
            $scope.variants = [];
            $scope.product.variantId = emptyGuid();
            if (isGuid($scope.product.modelId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                    data: { "Id": $scope.product.modelId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.variants = data;
                    if ($scope.currentProductTypeCode == 'ILOE') {
                        $scope.product.variantId = data[0].Id;
                        $scope.getVariantDetailsById($scope.product.variantId);
                    }
                    if (!$scope.formAction) {
                        $scope.policySoldDateChanged();
                    }
                }).error(function (data, status, headers, config) {
                });
            }
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

        $scope.resetWithCustomerTypes = function () {
            if (!$scope.updateMode){
                $scope.customer.Id = emptyGuid();;
                $scope.customer.firstName = '';
                $scope.customer.lastName = '';
                $scope.customer.dateOfBirth = '';
                $scope.customer.gender = 'M';
                $scope.customer.idTypeId = '1';
                $scope.customer.idNo = '';
                $scope.customer.idIssueDate = '';
                $scope.customer.nationalityId = emptyGuid();;
                $scope.customer.countryId = emptyGuid();;
                $scope.customer.cityId = emptyGuid();;
                $scope.customer.PostalCode = '';
                $scope.customer.mobileNo = '';
                $scope.customer.otherTelNo = '';
                $scope.customer.email = '';
                $scope.customer.address1 = '';
                $scope.customer.address2 = '';
                $scope.customer.address3 = '';
                $scope.customer.address4 = '';
                $scope.customer.businessName = '';
                $scope.customer.businessTelNo = '';
                $scope.customer.businessAddress1 = '';
                $scope.customer.businessAddress2 = '';
                $scope.customer.businessAddress3 = '';
                $scope.customer.businessAddress4 = '';
            }
        }

        $scope.selectedCityChanged = function () {
            if (isGuid($scope.customer.cityId)) {
                angular.forEach($scope.cities, function (value) {
                    if ($scope.customer.cityId == value.Id) {
                        $scope.selectedCity = value.CityName;
                        return false;
                    }
                });

            } else {
                $scope.selectedCity = '';
            }
        }
        $scope.selectedCommodityTypeChanged = function (isLoading = false) {
            //if (!isLoading) {
            swal({ title: $filter('translate')('pages.dealerPolicyRegistration.loadingData'), text: $filter('translate')('pages.dealerPolicyRegistration.pleaseWait'), showConfirmButton: false });
            //}
            $scope.products = [];
            $scope.commodityCategories = [];
            if (isGuid($scope.product.commodityTypeId)) {
                $scope.isProductDetailsReadonly = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId2',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.product.commodityTypeId }
                }).success(function (data, status, headers, config) {
                    $scope.products = data;
                    if (data.length > 0) {
                        $scope.product.productId = data[0].Id;
                        $scope.productId = data[0].Id;
                        $scope.selectedProdctChanged();
                    }
                    else {
                        $scope.product.productId = emptyGuid();
                    }
                    //if (!isLoading) {
                    swal.close();
                    //}
                }).error(function (data, status, headers, config) {
                    //if (!isLoading) {
                    swal.close();
                    //}
                });


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                    data: { "Id": $scope.product.commodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.commodityCategories = data;

                    if (!$scope.formAction) {

                        angular.forEach($scope.commodityCategories, function (value) {

                            if ($scope.product.categoryId == value.CommodityCategoryId) {
                                //$scope.serialNumberLength = value.Length;
                                $scope.serialNumberLength_temp = value.Length;
                                $scope.serialNumberCheck();
                                return false;
                            }
                        });

                    } else {
                        if ($scope.commodityCategories.length == 1) {
                            $scope.product.categoryId = $scope.commodityCategories[0].CommodityCategoryId;
                            $scope.selectedCommodityCategoryChanged();
                        }
                    }
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
            $scope.productFormReset();
        }
        $scope.selectedProdctChanged = function () {

            if (isGuid($scope.product.productId)) {
                swal({ title: $filter('translate')('pages.dealerPolicyRegistration.loadingData'), text: $filter('translate')('pages.dealerPolicyRegistration.pleaseWait'), showConfirmButton: false });
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
                        $scope.isItemStatusDisabled = true;
                        $scope.isProductILOE = false;
                        angular.forEach($scope.itemStatuses, function (value) {
                            if (value.Status == "New")
                                $scope.product.itemStatusId = value.Id;
                        });
                    } else if ($scope.currentProductTypeCode === 'ILOE') {
                        $scope.isProductILOE = true;
                    } else {
                        $scope.isItemStatusDisabled = false;
                        $scope.isProductILOE = false;
                    }

                    angular.forEach(data, function (childProduct) {
                        var productContract = {
                            Id: emptyGuid(),
                            ProductId: childProduct.Id,
                            ParentProductId: $scope.product.productId,
                            ContractId: emptyGuid(),
                            ExtensionTypeId: emptyGuid(),
                            AttributeSpecificationId: emptyGuid(),
                            CoverTypeId: emptyGuid(),
                            Contracts: [],
                            ExtensionTypes: [],
                            CoverTypes: [],
                            Premium: 0,
                            PremiumCurrencyName: '',
                            PremiumCurrencyTypeId: emptyGuid(),
                            Name: childProduct.Productcode + ' - ' + childProduct.Productname,
                            RSA: childProduct.Productcode == "RSA" ? true : false,
                            PolicyNo: ''
                        };
                        $scope.productContracts.push(productContract);

                    });


                    if (!$scope.formAction) {
                        //  $scope.policySoldDateChanged();
                    }
                    swal.close();
                }).error(function (data, status, headers, config) {
                    swal.close();
                });

                angular.forEach($scope.products, function (value) {
                    if ($scope.product.productId == value.Id) {
                        $scope.selectedProductName = value.Productname;
                        return false;
                    }
                });

            } else {
                $scope.selectedProductName = '';
            }
        }
        $scope.selectedDealerChanged = function (isInitial) {
            $scope.dealerLocations = [];
            if (isGuid($scope.product.dealerId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByDealerId',
                    data: { "Id": $scope.product.dealerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.dealerLocations = data;

                    if ($scope.dealerLocations.length === 1) {
                        $scope.product.dealerLocationId = data[0].Id;
                    }

                    if (!isInitial)
                        $scope.serialNumberCheck();
                }).error(function (data, status, headers, config) {
                });

                angular.forEach($scope.dealers, function (value) {
                    if (value.Id == $scope.product.dealerId) {
                        $scope.policy.dealerPaymentCurrencyTypeId = value.CurrencyId;
                        $scope.product.dealerPaymentCurrencyTypeId = value.CurrencyId;
                        $scope.selectedDealer = value.DealerName;
                        $scope.policy.customerPaymentCurrencyTypeId = value.CurrencyId;
                        $scope.product.customerPaymentCurrencyTypeId = value.CurrencyId;
                        angular.forEach($scope.currencies, function (valueC) {
                            if (value.CurrencyId == valueC.Id) {
                                $scope.DealerCurrencyName = valueC.Code;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                    data: { "Id": value.CurrencyId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    if (data == false) {
                                        SweetAlert.swal({
                                            title: $filter('translate')('pages.dealerPolicyRegistration.errorMessages.tasInformation'),
                                            text: "Selected dealer's currency(" + $scope.premiumCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                            type: "warning",
                                            confirmButtonColor: "rgb(221, 107, 85)"
                                        });
                                    }
                                }).error(function (data, status, headers, config) {
                                });
                            }
                        });
                    }
                });
            } else {
                $scope.selectedDealer = '';
            }

        }
        $scope.selectedDealerBrancheChanged = function () {
            try {
                $scope.selectedDealerBranch = $.grep($scope.dealerLocations, function (val) {
                    return val.Id == $scope.product.dealerLocationId;
                })[0].Location;
            } catch (e) {
                $scope.selectedDealerBranch = '';
            }
        }
        $scope.selectedCommodityCategoryChanged = function () {

            $scope.makes = [];
            $scope.product.serialNumber = "";
            $scope.isProductDetailsReadonly = false;
            if (isGuid($scope.product.categoryId)) {

                angular.forEach($scope.commodityCategories, function (value) {

                    if ($scope.product.categoryId == value.CommodityCategoryId) {
                        $scope.serialNumberLength = value.Length;
                        $scope.selectedItemCategory = value.CommodityCategoryDescription;
                        return false;
                    }
                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetMakesByCommodityCategoryId',
                    data: { "Id": $scope.product.categoryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.makes = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.serialNumberLength = '';
            }
        }
        $scope.selectedMakeChanged = function () {
            $scope.models = [];
            $scope.variants = [];
            $scope.product.modelId = emptyGuid();
            $scope.product.variantId = emptyGuid();

            if (isGuid($scope.product.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByCategoryAndMakeId',
                    data: {
                        "makeId": $scope.product.makeId,
                        "categoryId": $scope.product.categoryId,
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.models = data;
                }).error(function (data, status, headers, config) {
                });

                try {
                    $scope.selectedMake = $.grep($scope.makes, function (val) {
                        return val.Id == $scope.product.makeId;
                    })[0].MakeName;
                } catch (e) {
                    $scope.selectedMake = '';
                }
            }
        }
        $scope.isUsedItem = false;
        $scope.selectedModelChanged = function () {

            try {
                $scope.selectedModel = $.grep($scope.models, function (val) {
                    return val.Id == $scope.product.modelId;
                })[0].ModelName;
            } catch (e) {
                $scope.selectedModel = '';
            }
        }


        $scope.selectedVehicleStatusChanged = function () {


            if ($scope.product.itemStatusId != emptyGuid()) {
                angular.forEach($scope.itemStatuses, function (value) {
                    if ($scope.product.itemStatusId == value.Id) {
                        if (value.Status == "Used")
                            $scope.isUsedItem = true;
                        else
                            $scope.isUsedItem = false;
                    }
                });
            }

            try {
                $scope.selectedVehicleStatus = $.grep($scope.itemStatuses, function (val) {
                    return val.Id == $scope.product.itemStatusId;
                })[0].Status;
            } catch (e) {
                $scope.selectedVehicleStatus = '';
            }
        }
        $scope.selectedEngineCapacityChanged = function () {
            try {
                var engineCC = $.grep($scope.engineCapacities, function (val) {
                    return val.Id == $scope.product.engineCapacityId;
                })[0];
                $scope.selectedEngineCapacity = engineCC.EngineCapacityNumber + ' ' + engineCC.MesureType;
            } catch (e) {
                $scope.selectedEngineCapacity = '';
            }
        }

        //$scope.isProductILOE = function () {

        //    if ($scope.currentProductCode === 'ILOE') {
        //        return true;
        //    }
        //    else false;
        //}

        $scope.selectedCylinderCountChanged = function () {
            try {
                $scope.selectedCylinderCount = $.grep($scope.cylinderCounts, function (val) {
                    return val.Id == $scope.product.cylinderCountId;
                })[0].Count;
            } catch (e) {
                $scope.selectedCylinderCount = '';
            }
        }
        $scope.selectedVariantChanged = function () {
            $scope.getVariantDetailsById($scope.product.variantId);
            try {
                $scope.selectedVariant = $.grep($scope.variants, function (val) {

                    return val.Id == $scope.product.variantId;
                })[0].VariantName;
            } catch (e) {
                $scope.selectedVariant = '';
            }
        }

        $scope.getVariantDetailsById = function (variantId) {
            if (isGuid(variantId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetVariantById',
                    data: { "Id": variantId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.product.engineCapacityId = data.EngineCapacityId;
                    $scope.product.cylinderCountId = data.CylinderCountId;
                    $scope.product.fuelTypeId = data.FuelTypes[0];
                    $scope.product.transmissionTypeId = data.Transmissions[0];
                    $scope.product.driveTypeId = data.DriveTypes[0];
                    $scope.product.bodyTypeId = data.BodyTypes[0];
                    $scope.product.aspirationTypeId = data.Aspirations[0];
                    $scope.product.grossWeight = data.GrossWeight;


                }).error(function (data, status, headers, config) {

                });
            } else {
                $scope.product.engineCapacityId = emptyGuid();
                $scope.product.cylinderCountId = emptyGuid();
                $scope.product.fuelTypeId = emptyGuid();
                $scope.product.transmissionTypeId = emptyGuid();
                $scope.product.driveTypeId = emptyGuid();
                $scope.product.bodyTypeId = emptyGuid();
                $scope.product.aspirationTypeId = emptyGuid();
                $scope.product.grossWeight = 0.00;
            }
        }

        $scope.selectedFuelTypeChanged = function () {
            try {
                $scope.selectedFuelType = $.grep($scope.fuelTypes, function (val) {
                    return val.FuelTypeId == $scope.product.fuelTypeId;
                })[0].FuelTypeCode;
            } catch (e) {
                $scope.selectedFuelType = '';
            }
        }
        $scope.selectedTransmissionTypeChanged = function () {
            try {
                $scope.selectedTransmissionType = $.grep($scope.transmissionTypes, function (val) {
                    return val.Id == $scope.product.transmissionTypeId;
                })[0].TransmissionTypeCode;
            } catch (e) {
                $scope.selectedTransmissionType = '';
            }
        }
        $scope.selectedBodyTypeChanged = function () {
            try {
                $scope.selectedBodyType = $.grep($scope.bodyTypes, function (val) {
                    return val.Id == $scope.product.bodyTypeId;
                })[0].VehicleBodyTypeCode;
            } catch (e) {
                $scope.selectedBodyType = '';
            }
        }
        $scope.selectedAspirationTypeChanged = function () {
            try {
                $scope.selectedAspirationType = $.grep($scope.aspirationTypes, function (val) {
                    return val.Id == $scope.product.aspirationTypeId;
                })[0].AspirationTypeCode;
            } catch (e) {
                $scope.selectedAspirationType = '';
            }
        }
        $scope.selectedCommodityUsageTypeChanged = function () {
            try {
                $scope.selectedCommodityUsageType = $.grep($scope.commodityUsageTypes, function (val) {
                    return val.Id == $scope.product.commodityUsageTypeId;
                })[0].Name;
            } catch (e) {
                $scope.selectedCommodityUsageType = '';
            }
        }
        $scope.selectedSalesPersonChanged = function () {
            try {
                var salesPerson = $.grep($scope.salesPersons, function (val) {
                    return val.Id == $scope.policy.salesPersonId;
                })[0];
                $scope.selectedSalesPersonName = salesPerson.FirstName + ' ' + salesPerson.LastName;
            } catch (e) {
                $scope.selectedSalesPersonName = '';
            }
        }
        //selectedCommodityUsageType
        //selectedAspirationType
        $scope.selectedMaleChangedOnSearch = function () {
            $scope.SModels = [];
            if (isGuid($scope.bnWSearchGridSearchCriterias.make)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.bnWSearchGridSearchCriterias.make },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.SModels = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllModels',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.SModels = data;
                });
            }
        }

        $scope.policySoldDateChanged = function () {

            if ($scope.policy.policySoldDate != '') {
                $scope.GrossTotal = 0.00;
                $scope.GrossTotalTmp = 0.00;
                $scope.GrossTmpPaymentType = 0.00;

                angular.forEach($scope.productContracts, function (valProd) {
                    valProd.Contracts = [];
                    valProd.ContractId = emptyGuid();
                    valProd.ExtensionTypeId = emptyGuid();
                    valProd.AttributeSpecificationId = emptyGuid();
                    valProd.CoverTypeId = emptyGuid();
                    valProd.Premium = 0.00;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/GetContractsByIds',
                        data: {
                            "ProductId": $scope.product.productId,
                            "DealerId": $scope.product.dealerId,
                            "MakeId": $scope.product.makeId,
                            "ModelId": $scope.product.modelId,
                            "VariantId": $scope.product.variantId,
                            "CylinderCountId": $scope.product.cylinderCountId,
                            "EngineCapacityId": $scope.product.engineCapacityId,
                            "Date": $scope.policy.policySoldDate,
                            "grossWeight": $scope.product.grossWeight,
                            "UsageTypeId": $scope.product.commodityUsageTypeId,
                            "ItemStatusId": $scope.product.itemStatusId
                        },
                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data.length === 0 || data === '' || !data)
                            customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.noDealsFound'));

                        valProd.Contracts = data;

                        if ($scope.currentProductTypeCode == 'ILOE') {
                            $scope.emiContractId = data[0].Id;
                            $scope.getEMIValue();
                        }


                        if (data.length == 1) {
                            valProd.ContractId = data[0].Id;
                            $scope.SetContractValue(valProd)
                        }
                        if (!$scope.formAction) {

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByContractId',
                                data: {
                                    "ContractId": $scope.ContractProducts_temp[0].ContractId,
                                    "ProductId": $scope.product.productId,
                                    "DealerId": $scope.product.dealerId,
                                    "MakeId": $scope.product.makeId,
                                    "ModelId": $scope.product.modelId,
                                    "VariantId": $scope.product.variantId,
                                    "CylinderCountId": $scope.product.cylinderCountId,
                                    "EngineCapacityId": $scope.product.engineCapacityId,
                                    "Date": $scope.policy.policySoldDate,
                                    "grossWeight": $scope.product.grossWeight
                                },
                                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                valProd.ExtensionTypes = data;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/ContractManagement/GetAttributeSpecificationByExtensionId',
                                    data: {
                                        "ExtensionId": $scope.ContractProducts_temp[0].ContractExtensionsId,
                                        "ContractId": $scope.ContractProducts_temp[0].ContractId,
                                        "ProductId": $scope.product.productId,
                                        "DealerId": $scope.product.dealerId,
                                        "MakeId": $scope.product.makeId,
                                        "ModelId": $scope.product.modelId,
                                        "VariantId": $scope.product.variantId,
                                        "CylinderCountId": $scope.product.cylinderCountId,
                                        "EngineCapacityId": $scope.product.engineCapacityId,
                                        "Date": $scope.policy.policySoldDate,
                                        "grossWeight": $scope.product.grossWeight
                                    },
                                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    valProd.AttributeSpecifications = data;

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/ContractManagement/GetCoverTypesByExtensionId',
                                        data: {
                                            "AttributeSpecificationId": $scope.ContractProducts_temp[0].AttributeSpecificationId,
                                            "ExtensionId": $scope.ContractProducts_temp[0].ContractExtensionsId,
                                            "ContractId": $scope.ContractProducts_temp[0].ContractId,
                                            "ProductId": $scope.product.productId,
                                            "DealerId": $scope.product.dealerId,
                                            "MakeId": $scope.product.makeId,
                                            "ModelId": $scope.product.modelId,
                                            "VariantId": $scope.product.variantId,
                                            "CylinderCountId": $scope.product.cylinderCountId,
                                            "EngineCapacityId": $scope.product.engineCapacityId,
                                            "Date": $scope.policy.policySoldDate,
                                            "grossWeight": $scope.product.grossWeight,
                                            "ItemStatusId": $scope.product.itemStatusId
                                        },
                                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        if (data.length === 0)
                                            customErrorMessage('Item status does not match with any cover type.');
                                        valProd.CoverTypes = data;

                                        var dealerPrice = $scope.product.dealerPrice ;

                                        if ($scope.isProductILOE) {
                                            dealerPrice= $scope.product.dealerPrice - $scope.product.itemPrice;
                                        }

                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/ContractManagement/GetPremium',
                                            data: {
                                                "CoverTypeId": $scope.ContractProducts_temp[0].CoverTypeId,
                                                "AttributeSpecificationId": $scope.ContractProducts_temp[0].AttributeSpecificationId,
                                                "ExtensionId": $scope.ContractProducts_temp[0].ContractExtensionsId,
                                                "ContractId": $scope.ContractProducts_temp[0].ContractId,
                                                "Usage": $scope.policy.hrsUsedAtPolicySale,
                                                "ItemStatusId": $scope.product.itemStatusId,

                                                "DealerPrice": dealerPrice,
                                                "ItemPurchasedDate": $scope.product.itemPurchasedDate,

                                                "ProductId": $scope.product.productId,
                                                "DealerId": $scope.product.dealerId,
                                                "MakeId": $scope.product.makeId,
                                                "ModelId": $scope.product.modelId,
                                                "VariantId": $scope.product.variantId,
                                                "CylinderCountId": $scope.product.cylinderCountId,
                                                "EngineCapacityId": $scope.product.engineCapacityId,
                                                "Date": $scope.policy.policySoldDate,
                                                "grossWeight": $scope.product.grossWeight,

                                            },
                                            headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            // contract.CoverTypes = data;
                                            if (data.Status === 'ok') {
                                                valProd.Premium = data.TotalPremium;
                                                $scope.GrossTmpPaymentType += parseFloat(data.TotalPremium);
                                                $scope.DealerCurrencyName = data.Currency;
                                                valProd.Currency = data.Currency;
                                                $scope.GrossTotal = data.TotalPremium;
                                                $scope.setupProductContractValuesFromUpdate();
                                                // $scope.calculateAllPremiums();
                                                $scope.selectedPaymentTypeChanged();
                                            } else {
                                                customErrorMessage(data);
                                            }

                                        }).error(function (data, status, headers, config) {
                                        });


                                    }).error(function (data, status, headers, config) {
                                    });


                                }).error(function (data, status, headers, config) {
                                });



                            }).error(function (data, status, headers, config) {
                            });


                        }
                    }).error(function (data, status, headers, config) {
                    });
                })

                    ;


            }
        }
        $scope.SetContractValue = function (contract) {
            contract.ExtensionTypeId = emptyGuid();
            contract.AttributeSpecificationId = emptyGuid();
            contract.CoverTypeId = emptyGuid();
            contract.Premium = 0.00;
            contract.IsPremiumVisibleToDealer = false;
            $scope.emiContractId = contract.ContractId;

            if ($scope.currentProductTypeCode == 'ILOE') {
                $scope.getEMIValue();
            }

            if (isGuid(contract.ContractId)) {

                angular.forEach(contract.Contracts, function (contractObj) {
                    if (contract.ContractId === contractObj.Id) {
                        contract.IsPremiumVisibleToDealer = contractObj.IsPremiumVisibleToDealer;
                    }
                });


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByContractId',
                    data: {
                        "ContractId": contract.ContractId,
                        "ProductId": $scope.product.productId,
                        "DealerId": $scope.product.dealerId,
                        "MakeId": $scope.product.makeId,
                        "ModelId": $scope.product.modelId,
                        "VariantId": $scope.product.variantId,
                        "CylinderCountId": $scope.product.cylinderCountId,
                        "EngineCapacityId": $scope.product.engineCapacityId,
                        "Date": $scope.policy.policySoldDate,
                        "grossWeight": $scope.product.grossWeight
                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    contract.ExtensionTypes = data;
                    if (data.length == 1) {
                        contract.ExtensionTypeId = data[0].Id;;
                        $scope.SetExtensionTypeValue(contract)

                    }

                    angular.forEach($scope.productContracts, function (contractObj) {
                        if (contract.ProductId === contractObj.ProductId) {
                            contractObj.ExtensionTypeId = contract.ExtensionTypeId;
                        }
                    });
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
                        "ProductId": $scope.product.productId,
                        "DealerId": $scope.product.dealerId,
                        "MakeId": $scope.product.makeId,
                        "ModelId": $scope.product.modelId,
                        "VariantId": $scope.product.variantId,
                        "CylinderCountId": $scope.product.cylinderCountId,
                        "EngineCapacityId": $scope.product.engineCapacityId,
                        "Date": $scope.policy.policySoldDate,
                        "grossWeight": $scope.product.grossWeight
                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    contract.AttributeSpecifications = data;
                    contract.AttributeSpecificationId = data[0].Id;
                    $scope.SetAttributeSpecificationValue(contract);

                    angular.forEach($scope.productContracts, function (contractObj) {
                        if (contract.ProductId === contractObj.ProductId) {
                            contractObj.AttributeSpecifications = contract.AttributeSpecifications;
                            contractObj.AttributeSpecificationId = contract.AttributeSpecificationId;
                        }
                    });
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
                        "ProductId": $scope.product.productId,
                        "DealerId": $scope.product.dealerId,
                        "MakeId": $scope.product.makeId,
                        "ModelId": $scope.product.modelId,
                        "VariantId": $scope.product.variantId,
                        "CylinderCountId": $scope.product.cylinderCountId,
                        "EngineCapacityId": $scope.product.engineCapacityId,
                        "Date": $scope.policy.policySoldDate,
                        "grossWeight": $scope.product.grossWeight,
                        "ItemStatusId": $scope.product.itemStatusId
                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.length === 0)                        
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.itemStatusNotMatch'));
                    contract.CoverTypes = data;
                    if (data.length == 1) {
                        contract.CoverTypeId = data[0].Id;;
                        $scope.SetCoverTypeValue(contract)
                    }
                    angular.forEach($scope.productContracts, function (contractObj) {
                        if (contract.ProductId === contractObj.ProductId) {
                            contractObj.CoverTypeId = contract.CoverTypeId;

                        }
                    });
                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.CoverTypes = [];
            }
        }

        $scope.SetCoverTypeValue = function (contract) {
            contract.Premium = 0.00;
            if ($scope.currentCommodityTypeCode == "O") {
                $scope.product.registrationDate = $scope.product.itemPurchasedDate;
            }
            if (isGuid(contract.CoverTypeId)) {


                var dealerPrice = $scope.product.dealerPrice;

                if ($scope.isProductILOE) {
                    dealerPrice = $scope.product.dealerPrice - $scope.product.itemPrice;
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetPremium',
                    data: {
                        "CoverTypeId": contract.CoverTypeId,
                        "AttributeSpecificationId": contract.AttributeSpecificationId,
                        "ExtensionId": contract.ExtensionTypeId,
                        "ContractId": contract.ContractId,
                        "Usage": $scope.policy.hrsUsedAtPolicySale,
                        "ItemStatusId": $scope.product.itemStatusId,

                        "DealerPrice": dealerPrice,
                        "ItemPurchasedDate": $scope.product.registrationDate,

                        "ProductId": $scope.product.productId,
                        "DealerId": $scope.product.dealerId,
                        "MakeId": $scope.product.makeId,
                        "ModelId": $scope.product.modelId,
                        "VariantId": $scope.product.variantId,
                        "CylinderCountId": $scope.product.cylinderCountId,
                        "EngineCapacityId": $scope.product.engineCapacityId,
                        "Date": $scope.policy.policySoldDate,
                        "grossWeight": $scope.product.grossWeight,

                    },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    // contract.CoverTypes = data;
                    if (data.Status === 'ok') {
                        if (data.TotalPremium != 0) {
                            contract.Premium = data.TotalPremium;
                        }
                        else {
                            contract.Premium = '0';
                        }
                        $scope.GrossTmpPaymentType += parseFloat(data.TotalPremium);
                        $scope.DealerCurrencyName = data.Currency;
                        contract.Currency = data.Currency;
                        $scope.GrossTotal = data.TotalPremium;

                        //$scope.calculateAllPremiums();
                        //apply to main array
                        angular.forEach($scope.productContracts, function (contractObj) {
                            if (contract.ProductId === contractObj.ProductId) {
                                contractObj.Premium = contract.Premium;
                                contractObj.Currency = contract.Currency;
                                if ($scope.userType == "du") {
                                    contractObj.IsPremiumVisibleToDealer = contract.IsPremiumVisibleToDealer;
                                }
                                else {
                                    contractObj.IsPremiumVisibleToDealer = true;
                                }
                            }
                        });
                        console.log(contract);
                    } else {
                        customErrorMessage(data);
                    }

                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.Premium = 0.00;
            }

        }
        $scope.selectedPaymentModeChanged = function () {
            if (isGuid($scope.payment.paymentModeId)) {
                angular.forEach($scope.paymentModes, function (paymentMode) {
                    if (paymentMode.Id == $scope.payment.paymentModeId) {
                        if (paymentMode.Code == 'CC')//credit card
                        {
                            $scope.isPaymentTypesAvailable = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Payment/GetAllPaymentTypesByPaymentModeId',
                                data: { PaymentModeId: $scope.payment.paymentModeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.paymentTypes = data.PaymetTypes;
                            });
                        } else {
                            $scope.isPaymentTypesAvailable = false;
                            $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotal).toFixed(2);

                        }
                    }
                });
            } else {
                $scope.isPaymentTypesAvailable = false;
                $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotal).toFixed(2);
            }
        }
        $scope.selectedPaymentTypeChanged = function () {
            if (isGuid($scope.payment.paymentTypeId)) {
                $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotalTmp).toFixed(2);
                angular.forEach($scope.paymentTypes, function (paymentType) {
                    if (paymentType.Id == $scope.payment.paymentTypeId) {
                        $scope.GrossTmpPaymentType = (
                            (parseFloat(paymentType.PaymentCharge) *
                                parseFloat($scope.GrossTotalTmp)
                            ) + parseFloat($scope.GrossTotalTmp)).toFixed(2);
                        $scope.pub_PaymentType = paymentType.PaymentType;
                        $scope.pub_PaymentCharg = (parseFloat(paymentType.PaymentCharge) * parseFloat($scope.GrossTotalTmp)).toFixed(2);
                        //alert($scope.pub_PaymentCharg);
                        return false;
                    }
                });
            } else {
                $scope.GrossTmpPaymentType = $scope.GrossTotal;
            }
        }
        $scope.serialNumberCheck = function () {
            $scope.validate_dealerId = "";
            if (isGuid($scope.product.dealerId)) {
                if ($scope.formAction) {


                    if (isGuid($scope.product.categoryId) && $scope.product.serialNumber != ''
                        //&& $scope.product.serialNumber.length == parseInt($scope.serialNumberLength)
                    ) {                        
                        swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.validateVin'), showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/PolicyReg/SerialNumberCheck',
                            data: {
                                'SerialNumber': $scope.product.serialNumber,
                                'CommodityCode': $scope.currentCommodityTypeUniqueCode,
                                'LoggedInUserId': $localStorage.LoggedInUserId,
                                'DealerId': $scope.product.dealerId
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            document.getElementById("btn-nextStep").focus();
                            if (data.IsSerialExist) {
                                if (data.IsDealerInvalid) {
                                    $scope.isProductDetailsReadonly = false;
                                    customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.allocatedAnotherDealer'));
                                    $scope.resetItemInformationOnly()
                                    return false;
                                }
                                else if (data.IsBordxConfirmed) {
                                    $scope.isProductDetailsReadonly = true;
                                    customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.allocatedConfirmBordx'));

                                } else if (data.IsPolicyApproved) {
                                    if (!data.AllowedToApprove) {
                                        $scope.isProductDetailsReadonly = true;
                                        customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.anotherApprovedPolicy'));
                                    } else {
                                        $scope.isProductDetailsReadonly = false;
                                    }
                                } else {
                                    $scope.isProductDetailsReadonly = false;
                                }

                                if ($scope.currentCommodityTypeUniqueCode == "E") {
                                    $scope.loadBnW(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "A") {
                                    $scope.loadVehicleFromSerial(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "B") {
                                    $scope.loadVehicleFromSerial(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "O") {
                                    $scope.loadOther(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "Y") {
                                }
                            } else {
                                $scope.isProductDetailsReadonly = false;
                                $scope.clearSerialNumberInformation();
                            }

                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            swal.close();
                        });;
                    }

                } else {

                    if (isGuid($scope.product.categoryId) && $scope.product.serialNumber != '' && $scope.product.serialNumber.length == parseInt($scope.serialNumberLength_temp)) {
                        swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.validateVin'), showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/PolicyReg/SerialNumberCheck',
                            data: {
                                'SerialNumber': $scope.product.serialNumber,
                                'CommodityCode': $scope.currentCommodityTypeUniqueCode,
                                'LoggedInUserId': $localStorage.LoggedInUserId,
                                'DealerId': $scope.product.dealerId
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            document.getElementById("btn-nextStep").focus();
                            if (data.IsSerialExist) {
                                if (data.IsBordxConfirmed) {
                                    $scope.isProductDetailsReadonly = true;
                                    customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.allocatedConfirmBordx'));

                                } else if (data.IsPolicyApproved) {
                                    if (!data.AllowedToApprove) {
                                        $scope.isProductDetailsReadonly = true;
                                        customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.anotherApprovedPolicy'));
                                    } else {
                                        $scope.isProductDetailsReadonly = false;
                                    }
                                } else {
                                    $scope.isProductDetailsReadonly = false;
                                }

                                if ($scope.currentCommodityTypeUniqueCode == "E") {
                                    $scope.loadBnW(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "A") {
                                    $scope.loadVehicleFromSerial(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "B") {
                                    $scope.loadVehicleFromSerial(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "O") {
                                    $scope.loadOther(data.ItemId);
                                } else if ($scope.currentCommodityTypeUniqueCode == "Y") {
                                    // $scope.loadVehicle(data.ItemId);
                                }
                            } else {
                                $scope.isProductDetailsReadonly = false;
                                $scope.clearSerialNumberInformation();
                            }

                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            swal.close();
                        });;
                    }
                }
            } else {
                $scope.validate_dealerId = "has-error";
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.selectDealer'));
            }

        }
        $scope.resetItemInformationOnly = function () {
            $scope.product.serialNumber = "";
            $scope.product.makeId = emptyGuid();
            $scope.product.modelId = emptyGuid();
            $scope.product.modelYear = '';
            $scope.product.invoiceNo = '';
            $scope.product.additionalSerial = '';
            $scope.product.itemStatusId = emptyGuid();
            $scope.product.itemPurchasedDate = '';
            $scope.product.dealerPrice = '';
            $scope.product.itemPrice = '';
            $scope.product.variantId = emptyGuid();
            $scope.product.engineCapacityId = emptyGuid();
            $scope.product.cylinderCountId = emptyGuid();
            $scope.product.fuelTypeId = emptyGuid();
            $scope.product.transmissionTypeId = emptyGuid();
            $scope.product.bodyTypeId = emptyGuid();
            $scope.product.aspirationTypeId = emptyGuid();
            $scope.product.grossWeight = 0.00;
            $scope.product.MWStartDate = GetToday();
            $scope.product.MWStartDate = '';
            $scope.product.MWIsAvailable = false;
            $scope.product.registrationDate = GetToday();
            $scope.product.registrationDate = '';

            $scope.itemAttachmentType = '';
            $scope.product.MWIsAvailable = false;
            $scope.product.commodityUsageTypeId = emptyGuid();
            //$scope.itemDocUploader.queue = [];
            $scope.clearScannedImageSections('Item')
        }

        //click events
        $scope.customerFormReset = function (withAttachments) {
            $scope.customerDisabled = false;
            $scope.customer = {
                customerId: emptyGuid(),
                customerTypeId: emptyGuid(),
                usageTypeId: emptyGuid(),
                firstName: '',
                lastName: '',
                dateOfBirth: '',
                gender: 'M',
                idTypeId: 1,
                idNo: '',
                idIssueDate: '',
                nationalityId: emptyGuid(),
                countryId: emptyGuid(),
                cityId: emptyGuid(),
                mobileNo: '',
                otherTelNo: '',
                email: '',
                address1: '',
                address2: '',
                address3: '',
                address4: '',
                businessName: '',
                businessTelNo: '',
                businessAddress1: '',
                businessAddress2: '',
                businessAddress3: '',
                businessAddress4: '',
                PostalCode: ''
            };

            $scope.customerAttachmentType = '';
            if (withAttachments)
                $scope.customerDocUploader.queue = [];
            $scope.clearScannedImageSections('Customer')
        }

        $scope.customerReset = function () {
            $scope.customerDisabled = false;

            $scope.customer.customerId = emptyGuid();
                //customerTypeId: emptyGuid(),
            $scope.customer.usageTypeId = emptyGuid();
            $scope.customer.firstName = '';
            $scope.customer.lastName = '';
            $scope.customer.dateOfBirth = '';
            $scope.customer.gender = 'M';
            $scope.customer.idTypeId = 1;
            $scope.customer.idNo = '';
            $scope.customer.idIssueDate = '';
            $scope.customer.nationalityId = emptyGuid();
            $scope.customer.countryId = emptyGuid();
            $scope.customer.cityId = emptyGuid();
            $scope.customer.mobileNo = '';
            $scope.customer.otherTelNo = '';
            $scope.customer.email = '';
            $scope.customer.address1 = '';
            $scope.customer.address2 = '';
            $scope.customer.address3 = '';
            $scope.customer.address4 = '';
            $scope.customer.businessName = '';
            $scope.customer.businessTelNo = '';
            $scope.customer.businessAddress1 = '';
            $scope.customer.businessAddress2 = '';
            $scope.customer.businessAddress3 = '';
            $scope.customer.businessAddress4 = '';
            $scope.customer.PostalCode = '';
            $scope.customerDocUploader.queue = [];

        }

        $scope.productFormReset = function (withAttachments) {
            $scope.isProductDetailsReadonly = false;
            $scope.isPolicyDetailsReadonly = false;

            $scope.product.id = emptyGuid();
            //$scope.product.productId = emptyGuid();
            //$scope.product.dealerId = emptyGuid();
            //$scope.product.dealerLocationId = emptyGuid();
            $scope.product.categoryId = emptyGuid();
            $scope.product.serialNumber = '';
            $scope.product.makeId = emptyGuid();
            $scope.product.modelId = emptyGuid();
            $scope.product.modelYear = '';
            $scope.product.invoiceNo = '';
            $scope.product.additionalSerial = '';
            $scope.product.itemStatusId = emptyGuid();
            // $scope.product.commodityUsageTypeId = emptyGuid();
            $scope.product.itemPurchasedDate = GetToday();
            $scope.product.dealerPrice = '';
            $scope.product.itemPrice = '';
            $scope.product.variantId = emptyGuid();
            $scope.product.engineCapacityId = emptyGuid();
            $scope.product.cylinderCountId = emptyGuid();
            $scope.product.fuelTypeId = emptyGuid();
            $scope.product.transmissionTypeId = emptyGuid();
            $scope.product.bodyTypeId = emptyGuid();
            $scope.product.aspirationTypeId = emptyGuid();
            //$scope.product.dealerPaymentCurrencyTypeId = emptyGuid();
            // $scope.product.customerPaymentCurrencyTypeId = emptyGuid();
            $scope.product.grossWeight = 0.00;
            $scope.product.MWStartDate = GetToday();
            $scope.product.MWStartDate = '';

            $scope.product.registrationDate = GetToday();
            $scope.product.registrationDate = '';
            $scope.itemAttachmentType = '';
            $scope.product.MWIsAvailable = false;
            // $scope.product.commodityUsageTypeId = emptyGuid();
            if (withAttachments)
                $scope.itemDocUploader.queue = [];
            $scope.clearScannedImageSections('Item')
        }
        $scope.policyFormReset = function (withAttachments) {
            $scope.policy.policySoldDate = '';
            $scope.policy.hrsUsedAtPolicySale = 0;
            $scope.policy.salesPersonId = emptyGuid();
            $scope.policy.dealerPolicy = 'true';
            if (withAttachments)
                $scope.policyDocUploader.queue = [];
            $scope.clearScannedImageSections('Policy');
            $scope.policyAttachmentType = '';
        }

        $scope.resetAll = function () {
            $scope.updateMode = false;
            $scope.ImageArray = [];
            $scope.customerFormReset(true);
            $scope.productFormReset(true);
            $scope.policyFormReset(true);
            // $scope.paymentFormReset();
            $scope.GrossTmpPaymentType = '';
            $scope.GrossTotalTmp = '';
            $scope.GrossPremium = '';

            $scope.formActionText = "Registering for new Policy";
            $scope.formAction = true;//true for add new
            $scope.selectedCustomerTypeName = '';
            $scope.selectedUsageTypeName = '';
            $scope.serialNumberLength = '';
            //  $scope.currentCommodityTypeCode = '';
            $scope.discountAvailable = true;
            $scope.isPaymentTypesAvailable = false;
            $scope.GrossTotal = 0.0;
            goToStep(1);
            $scope.isProductDetailsReadonly = false;
            $scope.isPolicyDetailsReadonly = false;
            $scope.isHeaderSectionReadonly = false;
        }
        $scope.clearSerialNumberInformation = function () {
            $scope.product.makeId = emptyGuid();
            $scope.product.modelId = emptyGuid();
            $scope.product.modelYear = '';
            $scope.product.invoiceNo = '';
            $scope.product.additionalSerial = '';
            //$scope.product.itemStatusId = emptyGuid();
            // $scope.product.commodityUsageTypeId = '';
            $scope.product.itemPurchasedDate = GetToday();
            $scope.product.dealerPrice = '';
            $scope.product.itemPrice = '';
            $scope.product.variantId = emptyGuid();
            $scope.product.engineCapacityId = emptyGuid();
            $scope.product.cylinderCountId = emptyGuid();
            $scope.product.fuelTypeId = emptyGuid();
            $scope.product.transmissionTypeId = emptyGuid();
            $scope.product.bodyTypeId = emptyGuid();
            $scope.product.aspirationTypeId = emptyGuid();
            //   $scope.product.commodityUsageTypeId = emptyGuid();
        }
        $scope.customerSearchPopup = function () {
            SearchCustomerPopup = ngDialog.open({
                template: 'popUpSearchCustomer',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            $scope.customerSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                businessName: '',
                eMail: '',
                mobileNo: ''
            };
            var paginationOptionsCustomerSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getCustomerSearchPage();

        }
        $scope.ProductSearchPopup = function () {
            if ($scope.currentCommodityTypeUniqueCode == "A") {
                SearchVehiclePopup = ngDialog.open({
                    template: 'popUpSearchVehicle',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.vehicalSearchGridSearchCriterias = {
                    vinNo: "",
                    plateNo: ""
                };
                getVehicleSearchPage();
            } else if ($scope.currentCommodityTypeUniqueCode == "B") {
                SearchVehiclePopup = ngDialog.open({
                    template: 'popUpSearchVehicle',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.vehicalSearchGridSearchCriterias = {
                    vinNo: "",
                    plateNo: ""
                };
                getVehicleSearchPage();
            } else if ($scope.currentCommodityTypeUniqueCode == "E") {
                $scope.bnWSearchGridSearchCriterias = {
                    serialNo: "",
                    make: emptyGuid(),
                    model: emptyGuid()
                };
                SearchElectronicPopup = ngDialog.open({
                    template: 'popUpSearchElectronic',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
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
                        $scope.SModels = data;
                    });
                });
                $scope.refresBnWGridData();
            } else if ($scope.currentCommodityTypeUniqueCode == "O") {
                $scope.OtherSearchGridSearchCriterias = {
                    serialNo: "",
                    make: emptyGuid(),
                    model: emptyGuid()
                };
                SearchOtherPopup = ngDialog.open({
                    template: 'popUpSearchOther',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
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
                        $scope.SModels = data;
                    });
                });
                $scope.refresOtherGridData();
            } else if ($scope.currentCommodityTypeUniqueCode == "Y") {
                $scope.YellowGoodSearchGridSearchCriterias = {
                    serialNo: "",
                    make: emptyGuid(),
                    model: emptyGuid()
                };
                SearchYellowGoodPopup = ngDialog.open({
                    template: 'popUpSearchYellowGood',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
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
                        $scope.SModels = data;
                    });
                });
                $scope.refresYellowGoodGridData();
            }
        }


        //policy search grid

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
                { name: $filter('translate')('pages.dealerPolicyRegistration.commodityType'), field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabPolicy.policyNo'), field: 'PolicyNo', width: '30%', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.dealerPolicyRegistration.popUpSearchPolicy.vINSerial'), field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.popUpSearchPolicy.mobileNo'), field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabPolicy.policySoldDate'), field: 'PolicySoldDate', enableSorting: false, cellClass: 'columCss' },

                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') + '</button></div> ',
                    width: 60,
                    enableSorting: false
                },
                {
                    name: 'Statement',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.downloadStatement(row.entity.Id)" class="btn btn-xs btn-info">' + $filter('translate')('common.button.download') + '</button></div>',
                    width: 80,
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
        var getPolicySearchPage = function () {
            $scope.policySearchGridloading = true;
            $scope.policySearchGridloadAttempted = false;
            var policySearchGridParam =
            {
                'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                'type': 'forpolicydealerreg',
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
        $scope.GetProductById = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetProductById',
                data: { "productId": $scope.product.productId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.currentProductTypeCode = data.Productcode;
                    if ($scope.currentProductTypeCode == 'ILOE') {
                        $scope.isProductILOE = true;

                    }


                }
            });
        }

        $scope.loadPolicy = function (policyId) {
            // alert(policyId);
            // $scope.resetAll();
            if (isGuid(policyId)) {

                if (typeof SearchPolicyPopup != 'undefined')
                    SearchPolicyPopup.close();


                $scope.customerDocUploader.queue = [];
                $scope.itemDocUploader.queue = [];
                $scope.policyDocUploader.queue = [];
                $scope.paymentDocUploader.queue = [];
                $scope.GrossTotal = 0.00;
                $scope.GrossTotalTmp = 0.00;
                $scope.GrossTmpPaymentType = 0.00;
                $scope.formActionText = "Updating an existing Policy";
                $scope.formAction = false;//true for add new
                goToStep(1);
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetPolicyById',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.updateMode = true;
                    $scope.customerResetforComodityTypeChenge = true;
                    $scope.ContractProducts_temp = data.ContractProducts
                    $scope.loadCustomer(data.Customer.Id);

                    $scope.product.commodityTypeId = data.CommodityTypeId;
                   // $scope.selectedCommodityTypeChanged(true);

                    $scope.product.productId = data.ProductId;
                    $scope.GetProductById();
                    $scope.selectedProdctChanged();
                    $scope.product.dealerPaymentCurrencyTypeId = data.DealerPaymentCurrencyTypeId;
                    $scope.product.customerPaymentCurrencyTypeId = data.CustomerPaymentCurrencyTypeId;
                    $scope.product.dealerId = data.DealerId;
                    $scope.selectedDealerChanged(true);
                    $scope.product.dealerLocationId = data.DealerLocationId;
                    if ($scope.currentCommodityTypeCode == "A") {
                        $scope.product.id = data.Vehicle.Id;
                        $scope.product.categoryId = data.Vehicle.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.Vehicle.VINNo;
                        //$scope.serialNumberCheck();
                        $scope.loadVehicleFromSerial(data.Vehicle.Id);
                        $scope.product.makeId = data.Vehicle.MakeId;
                        $scope.product.modelId = data.Vehicle.ModelId;
                        $scope.product.engineCapacityId = data.Vehicle.EngineCapacityId;
                        $scope.product.cylinderCountId = data.Vehicle.CylinderCountId;
                        $scope.product.registrationDate = data.Vehicle.RegistrationDate;
                        $scope.product.grossWeight = data.Vehicle.GrossWeight;
                        $scope.product.driveTypeId = data.Vehicle.DriveTypeId;

                    } else if ($scope.currentCommodityTypeCode == "B") {
                        $scope.product.id = data.Vehicle.Id;
                        $scope.product.categoryId = data.Vehicle.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.Vehicle.VINNo;
                        //$scope.serialNumberCheck();
                        $scope.loadVehicleFromSerial(data.Vehicle.Id);
                        $scope.product.makeId = data.Vehicle.MakeId;
                        $scope.product.modelId = data.Vehicle.ModelId;
                        $scope.product.engineCapacityId = data.Vehicle.EngineCapacityId;
                        $scope.product.cylinderCountId = data.Vehicle.CylinderCountId;
                        $scope.product.registrationDate = data.Vehicle.RegistrationDate;
                        $scope.product.grossWeight = data.Vehicle.GrossWeight;
                        $scope.product.driveTypeId = data.Vehicle.DriveTypeId;

                    } else if ($scope.currentCommodityTypeCode == "E") {
                        $scope.product.id = data.BAndW.Id;
                        $scope.product.categoryId = data.BAndW.CategoryId;
                        // $scope.serialNumber_temp = data.BAndW.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.BAndW.SerialNo;
                        // $scope.serialNumberCheck();
                    } else if ($scope.currentCommodityTypeCode == "O") {

                        $scope.loadOther(data.BAndW.Id);

                        $scope.product.makeId = data.BAndW.MakeId;
                        $scope.product.modelId = data.BAndW.ModelId;
                        $scope.selectedModelChange();
                        $scope.product.id = data.BAndW.Id;
                        $scope.product.categoryId = data.BAndW.CategoryId;
                        // $scope.serialNumber_temp = data.BAndW.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.BAndW.SerialNo;
                        $scope.product.invoiceNo = data.BAndW.InvoiceNo;
                        $scope.product.itemPurchasedDate = data.BAndW.ItemPurchasedDate;
                        $scope.product.variantId = data.BAndW.Variant;
                    }

                    $scope.policy.salesPersonId = data.SalesPersonId;

                    $scope.product.MWStartDate = data.MWStartDate;
                    $scope.product.MWIsAvailable = data.MWIsAvailable;
                    $scope.product.productId = data.ProductId;
                    $scope.policy.id = data.Id;
                    $scope.policy.policySoldDate = data.PolicySoldDate;
                    // $scope.policySoldDateChanged();
                    $scope.policy.hrsUsedAtPolicySale = data.HrsUsedAtPolicySale;
                    $scope.policy.dealerPolicy = 'true';
                    $scope.policy.isApproved = data.IsApproved;

                    if ($scope.policy.isApproved) {
                        $scope.isProductDetailsReadonly = true;
                        $scope.isPolicyDetailsReadonly = true;
                        $scope.isHeaderSectionReadonly = true;
                    }

                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetAttachmentsByPolicyId',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.attachments_temp = data.Attachments;
                    $scope.customerDocUploader.queue = [];
                    $scope.itemDocUploader.queue = [];
                    $scope.policyDocUploader.queue = [];
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

                        if (data.Attachments[i].AttachmentSection == "Customer") {
                            $scope.customerDocUploader.queue.push(attachment);
                        } else if (data.Attachments[i].AttachmentSection == "Item") {
                            $scope.itemDocUploader.queue.push(attachment);
                        } else if (data.Attachments[i].AttachmentSection == "Policy") {
                            $scope.policyDocUploader.queue.push(attachment);
                        } else if (data.Attachments[i].AttachmentSection == "Payment") {
                            //  $scope.paymentDocUploader.queue.push(attachment)
                        }
                    }

                });

            }
        }
        $scope.RemoveExistingAttachment = function (section, id) {
            // alert(id);
            if (isGuid(id)) {

                swal({
                    title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.areYouSure'),
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.removeIt'),
                    cancelButtonText: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.noCancel'),
                    closeOnConfirm: true,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        if (section == "Customer") {
                            for (var i = 0; i < $scope.customerDocUploader.queue.length; i++) {
                                if ($scope.customerDocUploader.queue[i].id == id) {
                                    $scope.customerDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section == "Item") {
                            for (var i = 0; i < $scope.itemDocUploader.queue.length; i++) {
                                if ($scope.itemDocUploader.queue[i].id == id) {
                                    $scope.itemDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section == "Policy") {
                            for (var i = 0; i < $scope.policyDocUploader.queue.length; i++) {
                                if ($scope.policyDocUploader.queue[i].id == id) {
                                    $scope.policyDocUploader.queue.splice(i, 1);
                                    return false;
                                }
                            }
                        } else if (section == "Payment") {

                        }
                    }
                });
            }
        }

        $scope.downloadAttachment = function (ref) {
            if (ref != '') {
                swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: 'Preparing your download...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Upload/DownloadAttachment',
                    data: { 'fileRef': ref },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
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

        $scope.setupProductContractValuesFromUpdate = function () {
            outer_loop:
            for (var i = 0; i < $scope.ContractProducts_temp.length; i++) {
                inner_loop:
                for (var j = 0; j < $scope.productContracts.length; j++) {

                    if ($scope.ContractProducts_temp[i].ProductId == $scope.productContracts[j].ProductId) {
                        //alert('here');
                        $scope.productContracts[j].Id = $scope.ContractProducts_temp[i].Id;
                        $scope.productContracts[j].PolicyNo = $scope.ContractProducts_temp[i].PolicyNo;
                        $scope.productContracts[j].ContractId = $scope.ContractProducts_temp[i].ContractId;
                        $scope.SetContractValue($scope.productContracts[j].Name);
                        $scope.productContracts[j].ExtensionTypeId = $scope.ContractProducts_temp[i].ExtensionTypeId;
                        $scope.SetExtensionTypeValue($scope.productContracts[j].Name);
                        $scope.productContracts[j].CoverTypeId = $scope.ContractProducts_temp[i].CoverTypeId;
                        $scope.SetCoverTypeValue($scope.productContracts[j].Name);
                        $scope.productContracts[j].BookletNumber = $scope.ContractProducts_temp[i].BookletNumber;
                        $scope.productContracts[j].AttributeSpecificationId = $scope.ContractProducts_temp[i].AttributeSpecificationId;
                        break inner_loop;
                    }
                }
            };
        }

        //endranga
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

        //electronic search grid
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
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.serialNo'), field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.make'), field: 'Make', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.model'), field: 'Model', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBnW(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
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
            if (isGuid(bnwId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetBrownAndWhiteDetailsById',
                    data: { "Id": bnwId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.product.id = data.Id;
                    $scope.product.categoryId = data.CategoryId;
                    $scope.selectedCommodityCategoryChanged();
                    $scope.product.serialNumber = data.SerialNo;
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    if (!$scope.formAction) {
                        $scope.selectedModelChange();
                    }
                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = data.InvoiceNo;
                    $scope.product.additionalSerial = data.AddnSerialNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.ItemPrice;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;

                }).error(function (data, status, headers, config) {
                });
            }
        }
        //other item search grid

        var paginationOptionsOtherSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsOther = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.serialNo'), field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.make'), field: 'Make', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.model'), field: 'Model', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadOther(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') + '</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsOtherSearchGrid.sort = null;
                    } else {
                        paginationOptionsOtherSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getOtherSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsOtherSearchGrid.pageNumber = newPage;
                    paginationOptionsOtherSearchGrid.pageSize = pageSize;
                    getOtherSearchPage();
                });
            }
        };
        $scope.refresOtherGridData = function () {
            getOtherSearchPage();
        }
        var getOtherSearchPage = function () {
            $scope.OtherSearchGridloading = true;
            $scope.OtherSearchGridloadAttempted = false;
            var OtherSearchGridParam =
            {
                'paginationOptionsOtherSearchGrid': paginationOptionsOtherSearchGrid,
                'OtherSearchGridSearchCriterias': $scope.OtherSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/OtherItem/GetAllItemsForSearchGrid',
                data: OtherSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsOther.data = response_arr.data;
                $scope.gridOptionsOther.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.OtherSearchGridloading = false;
                $scope.OtherSearchGridloadAttempted = true;
            });
        }
        $scope.loadOther = function (OtherId) {
            if (isGuid(OtherId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/OtherItem/GetOtherDetailsById',
                    data: { "Id": OtherId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.product.id = data.Id;
                    $scope.product.categoryId = data.CategoryId;
                    $scope.selectedCommodityCategoryChanged();
                    $scope.product.serialNumber = data.SerialNo;
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    $scope.selectedModelChange();
                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = data.InvoiceNo;
                    $scope.product.additionalSerial = data.AddnSerialNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.ItemPrice;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;

                }).error(function (data, status, headers, config) {
                });
            }
        }
        //YellowGood search grid

        var paginationOptionsYellowGoodsSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsYellowGood = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.serialNo'), field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.make'), field: 'Make', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.model'), field: 'Model', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadYellowGood(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsYellowGoodsSearchGrid.sort = null;
                    } else {
                        paginationOptionsYellowGoodsSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getYellowGoodSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsYellowGoodsSearchGrid.pageNumber = newPage;
                    paginationOptionsYellowGoodsSearchGrid.pageSize = pageSize;
                    getYellowGoodSearchPage();
                });
            }
        };
        $scope.refresYellowGoodGridData = function () {
            getYellowGoodSearchPage();
        }
        var getYellowGoodSearchPage = function () {
            $scope.YellowGoodSearchGridloading = true;
            $scope.YellowGoodSearchGridloadAttempted = false;
            var YellowGoodSearchGridParam =
            {
                'paginationOptionsYellowGoodsSearchGrid': paginationOptionsYellowGoodsSearchGrid,
                'YellowGoodSearchGridSearchCriterias': $scope.YellowGoodSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/YellowGood/GetAllItemsForSearchGrid',
                data: YellowGoodSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsYellowGood.data = response_arr.data;
                $scope.gridOptionsYellowGood.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.YellowGoodSearchGridloading = false;
                $scope.YellowGoodSearchGridloadAttempted = true;
            });
        }
        $scope.loadYellowGood = function (YellowGoodId) {
            if (isGuid(OtherId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetBrownAndWhiteDetailsById',
                    data: { "Id": OtherId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.product.id = data.Id;
                    $scope.product.categoryId = data.CategoryId;
                    $scope.selectedCommodityCategoryChanged();
                    $scope.product.serialNumber = data.SerialNo;
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    $scope.selectedModelChange();
                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = data.InvoiceNo;
                    $scope.product.additionalSerial = data.AddnSerialNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.ItemPrice;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;

                }).error(function (data, status, headers, config) {
                });
            }
        }

        //vehicle search grid
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
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.vinNo'), field: 'VINNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.tabProduct.plateNo'), field: 'PlateNo', enableSorting: false, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadVehicle(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
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
        $scope.refresVehicleSearchGridData = function () {
            getVehicleSearchPage();
        }
        var getVehicleSearchPage = function () {
            $scope.validate_dealerId = "";
            if (isGuid($scope.product.dealerId)) {
                $scope.vehicleSearchGridloading = true;
                $scope.vehicleGridloadAttempted = false;
                var vehicleSearchGridParam =
                {
                    'paginationOptionsVehicleSearchGrid': paginationOptionsVehicleSearchGrid,
                    'vehicalSearchGridSearchCriterias': $scope.vehicalSearchGridSearchCriterias,
                    'dealerId': $scope.product.dealerId
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Vehicle/GetAllVehiclesForSearchGridByDealerId',
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
            } else {
                $scope.validate_dealerId = "has-error";
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.selectDealer'));
            }
        }
        $scope.loadVehicle = function (vehicleId) {
            if (isGuid(vehicleId)) {
                if (typeof SearchVehiclePopup != 'undefined')
                    SearchVehiclePopup.close();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                    data: { "Id": vehicleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.product.id = data.Id;
                    $scope.product.categoryId = data.CategoryId;
                    $scope.selectedCommodityCategoryChanged();
                    $scope.product.serialNumber = data.VINNo;
                    $scope.serialNumberCheck();
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    $scope.selectedModelChange();

                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = '';
                    $scope.product.additionalSerial = data.PlateNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.selectedVehicleStatusChanged();
                    //$scope.product.commodityUsageTypeId = emptyGuid();
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.VehiclePrice;
                    $scope.product.variantId = data.Variant == '' ? emptyGuid() : data.Variant;
                    $scope.selectedVariantChanged();
                    $scope.product.engineCapacityId = data.EngineCapacityId;
                    $scope.product.cylinderCountId = data.CylinderCountId;
                    $scope.product.fuelTypeId = data.FuelTypeId;
                    $scope.product.transmissionTypeId = data.TransmissionId;
                    $scope.product.bodyTypeId = data.BodyTypeId;
                    $scope.product.aspirationTypeId = data.AspirationId;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;
                    $scope.product.registrationDate = data.RegistrationDate;

                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.loadVehicleFromSerial = function (vehicleId) {
            if (isGuid(vehicleId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                    data: { "Id": vehicleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.product.id = data.Id;
                    $scope.product.categoryId = data.CategoryId;
                    $scope.product.serialNumber = data.VINNo;
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    $scope.selectedModelChange();

                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = '';
                    $scope.product.additionalSerial = data.PlateNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.selectedVehicleStatusChanged();
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.VehiclePrice;
                    $scope.product.variantId = data.Variant == '' ? emptyGuid() : data.Variant;
                    $scope.product.engineCapacityId = data.EngineCapacityId;
                    $scope.product.cylinderCountId = data.CylinderCountId;
                    $scope.product.fuelTypeId = data.FuelTypeId;
                    $scope.product.transmissionTypeId = data.TransmissionId;
                    $scope.product.bodyTypeId = data.BodyTypeId;
                    $scope.product.aspirationTypeId = data.AspirationId;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;

                }).error(function (data, status, headers, config) {
                });
            }
        }
        //customer search grid
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
                { name: $filter('translate')('pages.dealerPolicyRegistration.customer.firstName'), field: 'FirstName', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.customer.lastName'), field: 'LastName', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.dealerPolicyRegistration.customer.businessName'), field: 'BusinessName', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.dealerPolicyRegistration.customer.mobilePhone'), field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerPolicyRegistration.customer.emailAddress'), field: 'Email', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadCustomer(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
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
        $scope.refresCustomerSearchGridData = function () {
            getCustomerSearchPage();
        }
        $scope.loadCustomer = function (customerId) {
            if (isGuid(customerId)) {
                if (typeof SearchCustomerPopup != 'undefined')
                    SearchCustomerPopup.close();
                swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: 'Validate Customer Information...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/CheckCustomerById',
                    data: { "CustomerId": customerId, 'LoggedInUserId': $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.IsBordxConfirmed) {
                        $scope.customerDisabled = true;
                        customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.confirmedBordxCustomer'));
                    } else {
                        $scope.updateMode = true;
                        if (data.IsPolicyApproved == true && data.HasAccesstoPolicyApproval == false) {
                            $scope.customerDisabled = true;
                            customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.anotherApprovedPolicyCustomer'));

                        } else {
                            $scope.customerDisabled = false;
                        }

                    }

                    if ($scope.currentProductTypeCode == 'ILOE') {

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetCustomerByIdforIloe',
                            data: { "Id": customerId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.customer.customerId = data.Id;
                            $scope.customer.customerTypeId = data.CustomerTypeId;
                            $scope.customer.usageTypeId = data.UsageTypeId;
                            $scope.customer.firstName = data.FirstName;
                            $scope.customer.lastName = data.LastName;
                            $scope.customer.dateOfBirth = data.DateOfBirth;
                            $scope.customer.gender = data.Gender;
                            $scope.customer.idTypeId = data.IDTypeId;
                            $scope.selectedIdTypeChanged();
                            $scope.customer.idNo = data.IDNo;
                            $scope.customer.idIssueDate = data.DLIssueDate;
                            $scope.customer.nationalityId = data.NationalityId;
                            $scope.selectedNationaltyChanged();
                            $scope.customer.countryId = data.CountryId;
                            $scope.customer.cityId = data.CityId;
                            $scope.customer.mobileNo = data.MobileNo;
                            $scope.customer.otherTelNo = data.OtherTelNo;
                            $scope.customer.email = data.Email;
                            $scope.customer.address1 = data.Address1;
                            $scope.customer.address2 = data.Address2;
                            $scope.customer.address3 = data.Address3;
                            $scope.customer.address4 = data.Address4;
                            $scope.customer.businessName = data.BusinessName;
                            $scope.customer.businessTelNo = data.BusinessTelNo;
                            $scope.customer.businessAddress1 = data.BusinessAddress1;
                            $scope.customer.businessAddress2 = data.BusinessAddress2;
                            $scope.customer.businessAddress3 = data.BusinessAddress3;
                            $scope.customer.businessAddress4 = data.BusinessAddress4;
                            $scope.customer.PostalCode = data.PostalCode;
                            $scope.formActionCustomer = false;
                            $scope.PolicyId = data.PolicyId;
                            $scope.calculateAge(data.DateOfBirth);

                            $scope.selectedCustomerTypeIdChanged();
                            $scope.selectedCountryChanged();
                            $scope.selectedUsageTypeChanged();

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/PolicyReg/GetAttachmentsByPolicyId',
                                data: { "Id": $scope.PolicyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {

                                $scope.attachments_temp = data.Attachments;
                                $scope.customerDocUploader.queue = [];
                                $scope.itemDocUploader.queue = [];
                                $scope.policyDocUploader.queue = [];
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

                                    if (data.Attachments[i].AttachmentSection == "Customer") {
                                        $scope.customerDocUploader.queue.push(attachment);
                                        $scope.uploadedDocIds.push(data.Attachments[i].Id);
                                    }
                                }

                            });


                        }).error(function (data, status, headers, config) {
                            // clearCustomerControls();
                        });
                    } else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetCustomerById',
                            data: { "Id": customerId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.customer.customerId = data.Id;
                            $scope.customer.customerTypeId = data.CustomerTypeId;
                            $scope.customer.usageTypeId = data.UsageTypeId;
                            $scope.customer.firstName = data.FirstName;
                            $scope.customer.lastName = data.LastName;
                            $scope.customer.dateOfBirth = data.DateOfBirth;
                            $scope.customer.gender = data.Gender;
                            $scope.customer.idTypeId = data.IDTypeId;
                            $scope.selectedIdTypeChanged();
                            $scope.customer.idNo = data.IDNo;
                            $scope.customer.idIssueDate = data.DLIssueDate;
                            $scope.customer.nationalityId = data.NationalityId;
                            $scope.selectedNationaltyChanged();
                            $scope.customer.countryId = data.CountryId;
                            $scope.customer.cityId = data.CityId;
                            $scope.customer.mobileNo = data.MobileNo;
                            $scope.customer.otherTelNo = data.OtherTelNo;
                            $scope.customer.email = data.Email;
                            $scope.customer.address1 = data.Address1;
                            $scope.customer.address2 = data.Address2;
                            $scope.customer.address3 = data.Address3;
                            $scope.customer.address4 = data.Address4;
                            $scope.customer.businessName = data.BusinessName;
                            $scope.customer.businessTelNo = data.BusinessTelNo;
                            $scope.customer.businessAddress1 = data.BusinessAddress1;
                            $scope.customer.businessAddress2 = data.BusinessAddress2;
                            $scope.customer.businessAddress3 = data.BusinessAddress3;
                            $scope.customer.businessAddress4 = data.BusinessAddress4;
                            $scope.customer.PostalCode = data.PostalCode;
                            $scope.customerResetforComodityTypeChenge = true;

                            $scope.selectedCustomerTypeIdChanged();
                            $scope.selectedCountryChanged();
                            $scope.selectedUsageTypeChanged();
                        }).error(function (data, status, headers, config) {
                            // clearCustomerControls();
                        });
                    }


                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });;

            }
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
        $scope.validateCustomerInfo = function () {
            var isValidated = true;

            if (!isGuid($scope.product.commodityTypeId)) {
                $scope.validate_commodityTypeId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_commodityTypeId = "";
            }

            if (!isGuid($scope.product.productId)) {
                $scope.validate_productId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_productId = "";
            }


            if (!isGuid($scope.product.dealerId)) {
                $scope.validate_dealerId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_dealerId = "";
            }

            if (!isGuid($scope.product.dealerLocationId)) {
                $scope.validate_dealerLocationId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_dealerLocationId = "";
            }
            if (!parseInt($scope.customer.customerTypeId)) {
                isValidated = false;
                $scope.validate_customerTypeId = "has-error";
            } else {
                $scope.validate_customerTypeId = "";
            }

            if (!parseInt($scope.customer.usageTypeId)) {
                isValidated = false;
                $scope.validate_usageTypeId = "has-error";
            } else {
                $scope.validate_usageTypeId = "";
            }

            if (!isGuid($scope.customer.countryId)) {
                isValidated = false;
                $scope.validate_countryId = "has-error";
            } else {
                $scope.validate_countryId = "";
            }

            if (!isGuid($scope.customer.cityId)) {
                isValidated = false;
                $scope.validate_cityId = "has-error";
            } else {
                $scope.validate_cityId = "";
            }

            if ($scope.customer.mobileNo == "") {
                isValidated = false;
                $scope.validate_mobileNo = "has-error";
            } else {
                $scope.validate_mobileNo = "";
            }
            if ($scope.customer.firstName == "") {
                isValidated = false;
                $scope.validate_firstName = "has-error";
            } else {
                $scope.validate_firstName = "";
            }

            //if ($scope.IsCustomerNoExist) {
            //    isValidated = false;
            //    $scope.validate_mobileNo = "has-error";
            //}
            //else {
            //    $scope.validate_businessName = "";
            //}

            if ($scope.selectedCustomerTypeName === "Corporate") {
                if ($scope.customer.businessName == "") {
                    isValidated = false;
                    $scope.validate_businessName = "has-error";
                } else {
                    $scope.validate_businessName = "";
                }

                if ($scope.customer.businessTelNo == "") {
                    isValidated = false;
                    $scope.validate_businessTelNo = "has-error";
                } else {
                    $scope.validate_businessTelNo = "";
                }

            } else {

                //if (!parseInt($scope.customer.idTypeId)) {
                //    isValidated = false;
                //    $scope.validate_idTypeId = "has-error";
                //} else {
                //    $scope.validate_idTypeId = "";
                //}

                //if ($scope.customer.idNo == "") {
                //    isValidated = false;
                //    $scope.validate_idNo = "has-error";
                //} else {
                //    $scope.validate_idNo = "";
                //}

                if (!parseInt($scope.customer.nationalityId)) {
                    isValidated = false;
                    $scope.validate_nationalityId = "has-error";
                } else {
                    $scope.validate_nationalityId = "";
                }
                if ($scope.isProductILOE) {
                    if ($scope.customer.dateOfBirth == "") {
                        isValidated = false;
                        $scope.validate_DOB = "has-error";
                    }
                } else {
                    $scope.validate_DOB = "";
                }
            }
            return isValidated;
        }
        $scope.validateProductInfo = function () {
            var isValidated = true;
            if ($scope.currentCommodityTypeCode == 'O' && $scope.currentProductCode == 'TYRE' && $scope.currentProductTypeCode != 'ILOE') {
                if (!isGuid($scope.product.commodityTypeId)) {
                    $scope.validate_commodityTypeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_commodityTypeId = "";
                }

                if (!isGuid($scope.product.productId)) {
                    $scope.validate_productId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_productId = "";
                }

                if (!isGuid($scope.product.dealerId)) {
                    $scope.validate_dealerId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerId = "";
                }

                if (!isGuid($scope.product.dealerLocationId)) {
                    $scope.validate_dealerLocationId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerLocationId = "";
                }

                if (!isGuid($scope.product.categoryId)) {
                    $scope.validate_categoryId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_categoryId = "";
                }

                if ($scope.product.serialNumber == "") {
                    $scope.validate_serialNumber = "has-error";
                    isValidated = false;
                } else {
                    //if (!$scope.formAction) {
                    //    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength_temp)) {
                    //        $scope.validate_serialNumber = "";
                    //    } else {
                    //        $scope.validate_serialNumber = "has-error";
                    //        isValidated = false;
                    //        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength_temp + " characters.");
                    //    }
                    //} else {
                    //    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength)) {
                    //        $scope.validate_serialNumber = "";
                    //    } else {
                    //        $scope.validate_serialNumber = "has-error";
                    //        isValidated = false;
                    //        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength + " characters.");
                    //    }
                    //}
                }

                if (!isGuid($scope.product.makeId)) {
                    $scope.validate_makeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_makeId = "";
                }


                if (!isGuid($scope.product.modelId)) {
                    $scope.validate_modelId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_modelId = "";
                }

                //if (!isGuid($scope.product.variantId) || $scope.product.variantId === "") {
                //    $scope.validate_variant = "has-error";
                //    isValidated = false;
                //} else {
                //    $scope.validate_variant = "";
                //}


                //if ($scope.product.modelYear == "") {
                //    $scope.validate_modelYear = "has-error";
                //    isValidated = false;
                //} else {
                //    $scope.validate_modelYear = "";
                //}
                if ($scope.currentCommodityTypeCode == 'O') {
                    if ($scope.product.invoiceNo == "") {
                        $scope.validate_invoiceNo = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_invoiceNo = "";
                    }
                }

                //if ($scope.currentCommodityTypeCode == 'A') {
                //    if (!isGuid($scope.product.cylinderCountId)) {
                //        $scope.validate_cylinderCountId = "has-error";
                //        isValidated = false;
                //    } else {
                //        $scope.validate_cylinderCountId = "";
                //    }

                //    if (!isGuid($scope.product.engineCapacityId)) {
                //        $scope.validate_engineCapacityId = "has-error";
                //        isValidated = false;
                //    } else {
                //        $scope.validate_engineCapacityId = "";
                //    }
                //    if ($scope.product.MWStartDate == "") {
                //        $scope.validate_MWStartDate = "has-error";
                //        isValidated = false;
                //    } else {
                //        $scope.validate_MWStartDate = "";
                //    }
                //}


                if (!isGuid($scope.product.itemStatusId) || $scope.product.itemStatusId === "") {
                    $scope.validate_itemStatusId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_itemStatusId = "";
                }


                if (!isGuid($scope.product.commodityUsageTypeId)) {
                    $scope.validate_commodityUsageTypeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_commodityUsageTypeId = "";
                }

                if ($scope.product.itemPurchasedDate == "") {
                    $scope.validate_itemPurchasedDate = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_itemPurchasedDate = "";
                }

                if ($scope.product.dealerPrice != "" && parseFloat($scope.product.dealerPrice)) {
                    $scope.validate_dealerPrice = "";
                    $scope.product.itemPrice = $scope.product.dealerPrice;

                } else {
                    $scope.validate_dealerPrice = "has-error";
                    isValidated = false;
                }

                //if ($scope.product.itemPrice != "" && parseFloat($scope.product.itemPrice)) {

                //    $scope.validate_itemPrice = "";
                //} else {
                //    $scope.validate_itemPrice = "has-error";
                //    isValidated = false;
                //}

                //if (parseFloat($scope.product.itemPrice) >= parseFloat($scope.product.dealerPrice)) {

                //    $scope.validate_itemPrice = "";
                //    $scope.validate_dealerPrice = "";
                //} else {
                //    //customErrorMessage('Dealer price cannot exceed the Market price.');
                //    $scope.validate_itemPrice = "has-error";
                //    $scope.validate_dealerPrice = "has-error";
                //    isValidated = false;
                //}
            } else if ($scope.currentProductTypeCode == 'ILOE') {
                if (!isGuid($scope.product.commodityTypeId)) {
                    $scope.validate_commodityTypeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_commodityTypeId = "";
                }

                if (!isGuid($scope.product.productId)) {
                    $scope.validate_productId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_productId = "";
                }

                if (!isGuid($scope.product.dealerId)) {
                    $scope.validate_dealerId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerId = "";
                }

                if (!isGuid($scope.product.dealerLocationId)) {
                    $scope.validate_dealerLocationId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerLocationId = "";
                }

                if (!isGuid($scope.product.categoryId)) {
                    $scope.validate_categoryId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_categoryId = "";
                }

                if ($scope.product.serialNumber == "") {
                    $scope.validate_serialNumber = "has-error";
                    isValidated = false;
                } else {

                }

                if (!isGuid($scope.product.makeId)) {
                    $scope.validate_makeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_makeId = "";
                }


                if (!isGuid($scope.product.modelId)) {
                    $scope.validate_modelId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_modelId = "";
                }

            } else {
                if (!isGuid($scope.product.commodityTypeId)) {
                    $scope.validate_commodityTypeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_commodityTypeId = "";
                }

                if (!isGuid($scope.product.productId)) {
                    $scope.validate_productId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_productId = "";
                }

                if (!isGuid($scope.product.dealerId)) {
                    $scope.validate_dealerId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerId = "";
                }

                if (!isGuid($scope.product.dealerLocationId)) {
                    $scope.validate_dealerLocationId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_dealerLocationId = "";
                }

                if (!isGuid($scope.product.categoryId)) {
                    $scope.validate_categoryId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_categoryId = "";
                }

                if ($scope.product.serialNumber == "") {
                    $scope.validate_serialNumber = "has-error";
                    isValidated = false;
                } else {
                    //if (!$scope.formAction) {
                    //    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength_temp)) {
                    //        $scope.validate_serialNumber = "";
                    //    } else {
                    //        $scope.validate_serialNumber = "has-error";
                    //        isValidated = false;
                    //        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength_temp + " characters.");
                    //    }
                    //} else {
                    //    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength)) {
                    //        $scope.validate_serialNumber = "";
                    //    } else {
                    //        $scope.validate_serialNumber = "has-error";
                    //        isValidated = false;
                    //        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength + " characters.");
                    //    }
                    //}
                }

                if (!isGuid($scope.product.makeId)) {
                    $scope.validate_makeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_makeId = "";
                }


                if (!isGuid($scope.product.modelId)) {
                    $scope.validate_modelId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_modelId = "";
                }

                //if (!isGuid($scope.product.variantId) || $scope.product.variantId === "") {
                //    $scope.validate_variant = "has-error";
                //    isValidated = false;
                //} else {
                //    $scope.validate_variant = "";
                //}


                if ($scope.product.modelYear == "") {
                    $scope.validate_modelYear = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_modelYear = "";
                }
                if ($scope.currentCommodityTypeCode == 'O') {
                    if ($scope.product.invoiceNo == "") {
                        $scope.validate_invoiceNo = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_invoiceNo = "";
                    }
                }

                if ($scope.currentCommodityTypeCode == 'A') {
                    if (!isGuid($scope.product.cylinderCountId)) {
                        $scope.validate_cylinderCountId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_cylinderCountId = "";
                    }

                    if (!isGuid($scope.product.engineCapacityId)) {
                        $scope.validate_engineCapacityId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_engineCapacityId = "";
                    }
                    if ($scope.product.MWStartDate == "") {
                        $scope.validate_MWStartDate = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_MWStartDate = "";
                    }
                }

                if ($scope.currentCommodityTypeCode == 'B') {
                    if (!isGuid($scope.product.cylinderCountId)) {
                        $scope.validate_cylinderCountId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_cylinderCountId = "";
                    }

                    if (!isGuid($scope.product.engineCapacityId)) {
                        $scope.validate_engineCapacityId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_engineCapacityId = "";
                    }
                    if ($scope.product.MWStartDate == "") {
                        $scope.validate_MWStartDate = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_MWStartDate = "";
                    }
                }

                if ($scope.currentCommodityTypeCode == 'B') {
                    if (!isGuid($scope.product.cylinderCountId)) {
                        $scope.validate_cylinderCountId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_cylinderCountId = "";
                    }

                    if (!isGuid($scope.product.engineCapacityId)) {
                        $scope.validate_engineCapacityId = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_engineCapacityId = "";
                    }
                    if ($scope.product.MWStartDate == "") {
                        $scope.validate_MWStartDate = "has-error";
                        isValidated = false;
                    } else {
                        $scope.validate_MWStartDate = "";
                    }
                }

                if (!isGuid($scope.product.itemStatusId) || $scope.product.itemStatusId === "") {
                    $scope.validate_itemStatusId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_itemStatusId = "";
                }


                if (!isGuid($scope.product.commodityUsageTypeId)) {
                    $scope.validate_commodityUsageTypeId = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_commodityUsageTypeId = "";
                }

                if ($scope.product.itemPurchasedDate == "") {
                    $scope.validate_itemPurchasedDate = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_itemPurchasedDate = "";
                }

                if ($scope.product.dealerPrice != "" && parseFloat($scope.product.dealerPrice)) {
                    $scope.validate_dealerPrice = "";
                    $scope.product.itemPrice = $scope.product.dealerPrice;

                } else {
                    $scope.validate_dealerPrice = "has-error";
                    isValidated = false;
                }

                if ($scope.product.itemPrice != "" && parseFloat($scope.product.itemPrice)) {

                    $scope.validate_itemPrice = "";
                } else {
                    $scope.validate_itemPrice = "has-error";
                    isValidated = false;
                }

                if (parseFloat($scope.product.itemPrice) >= parseFloat($scope.product.dealerPrice)) {

                    $scope.validate_itemPrice = "";
                    $scope.validate_dealerPrice = "";
                } else {
                    //customErrorMessage('Dealer price cannot exceed the Market price.');
                    $scope.validate_itemPrice = "has-error";
                    $scope.validate_dealerPrice = "has-error";
                    isValidated = false;
                }
                //if ($scope.product.registrationDate == "") {
                //    $scope.validate_RegistrationDate = "has-error";
                //    isValidated = false;
                //} else {

                //    $scope.validate_RegistrationDate = "";
                //}
            }

            return isValidated;
        }
        $scope.validatePolicyInfo = function () {
            var isValidated = true;
            if (!isGuid($scope.product.commodityTypeId)) {
                $scope.validate_commodityTypeId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_commodityTypeId = "";
            }

            if (!isGuid($scope.product.productId)) {
                $scope.validate_productId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_productId = "";
            }


            if (!isGuid($scope.product.dealerId)) {
                $scope.validate_dealerId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_dealerId = "";
            }

            if (!isGuid($scope.product.dealerLocationId)) {
                $scope.validate_dealerLocationId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_dealerLocationId = "";
            }
            if (!parseInt($scope.customer.customerTypeId)) {
                isValidated = false;
                $scope.validate_customerTypeId = "has-error";
            } else {
                $scope.validate_customerTypeId = "";
            }
            if (!$scope.isProductILOE) {
                if ($scope.policy.policySoldDate == '') {
                    $scope.validate_policySoldDate = "has-error";
                    isValidated = false;
                } else {
                    $scope.validate_policySoldDate = "";
                }
            }
            else {
                $scope.validate_policySoldDate = "";
            }
            //if ($scope.policy.hrsUsedAtPolicySale == "") {
            //    $scope.validate_hrsUsedAtPolicySale = "has-error";
            //    isValidated = false;
            //} else {
            //    $scope.validate_hrsUsedAtPolicySale = 0;
            //}

            if (!isGuid($scope.policy.salesPersonId)) {
                $scope.validate_salesPersonId = "has-error";
                isValidated = false;
            } else {
                $scope.validate_salesPersonId = "";
            }

            angular.forEach($scope.productContracts, function (contract) {
                if (contract.PolicyNo == '' || typeof contract.PolicyNo == 'undefined') {
                    contract.validatePolicyNo = "has-error";
                    isValidated = false;
                } else {
                    contract.validatePolicyNo = "";
                }

                if (!isGuid(contract.ContractId)) {
                    contract.validateContract = "has-error";
                    isValidated = false;
                } else {
                    contract.validateContract = "";
                }

                if (!isGuid(contract.ExtensionTypeId)) {
                    contract.validateExtentionType = "has-error";
                    isValidated = false;
                } else {
                    contract.validateExtentionType = "";
                }

                if (!isGuid(contract.CoverTypeId)) {
                    contract.validateCoverType = "has-error";
                    isValidated = false;
                } else {
                    contract.validateCoverType = "";
                }

                if (contract.Premium == '' || typeof contract.Premium == 'undefined') {
                    contract.validatePremium = "has-error";
                    isValidated = false;
                } else {
                    contract.validatePremium = "";
                }
            });
            return isValidated;
        }
        $scope.validatePaymentInfo = function () {
            return true;

        }
        $scope.validateTpaBranch = function () {
            return true;
        }
        $scope.discountPercentageChanged = function () {
            if (parseFloat($scope.payment.discount)) {

                $scope.GrossTotalTmp = (parseFloat($scope.GrossTotal) * ((100 - $scope.payment.discount) / 100)).toFixed(2);
                $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
            } else {
                $scope.GrossTotalTmp = $scope.GrossTotal;
                $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                $scope.payment.discount = parseFloat("0.00").toFixed(2);
            }
        }
        $scope.form = {

            next: function (form) {

                $scope.toTheTop();
                if (form.$valid) {
                    nextStep();
                } else {
                    var field = null, firstError = null;
                    for (field in form) {
                        if (field[0] != '$') {
                            if (firstError === null && !form[field].$valid) {
                                firstError = form[field].$name;
                            }

                            if (form[field].$pristine) {
                                form[field].$dirty = true;
                            }
                        }
                    }

                    angular.element('.ng-invalid[name=' + firstError + ']').focus();
                    errorMessage();
                }
            },
            prev: function (form) {
                $scope.toTheTop();
                prevStep();
            },
            goTo: function (form, i) {
                if (parseInt($scope.currentStep) < parseInt(i)) {
                    var isValidated = true;
                    for (var j = (i - 1); j > 0; j--) {
                        if (j == 1) {
                            if (!$scope.validateCustomerInfo()) {
                                isValidated = false;
                                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.allMandatory'));
                                return;
                            }
                        } else if (j == 2) {
                            if (!$scope.validateProductInfo()) {
                                $scope.product.itemPurchasedDate = GetToday();
                                isValidated = false;
                                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.allMandatory'));
                                return;
                            } else {



                                if (i !== 4) {

                                    if ($scope.formAction) {
                                        $scope.policy.policySoldDate = GetToday();
                                        $scope.policy.salesPersonId = $localStorage.LoggedInUserId;
                                    }
                                    $scope.policySoldDateChanged();
                                }


                                angular.forEach($scope.productContracts, function (contract) {
                                    var branchIdValue = $scope.policy.tpaBranchId;
                                    if ($localStorage.tpaName != "CycleandCarriage") {
                                        branchIdValue=$scope.product.dealerLocationId;
                                    }
                                    var data = {
                                        branchId: branchIdValue,
                                        dealerId: $scope.product.dealerId,
                                        productId: $scope.product.productId,
                                        tpaId: $localStorage.tpaID,
                                        commodityTypeId: $scope.product.commodityTypeId
                                    };

                                    if ($scope.formAction) {
                                        $scope.EMI = '';


                                        angular.forEach($scope.productContracts, function (value) {

                                            value.BookletNumber = '';
                                        });

                                        if ($scope.currentProductTypeCode === 'ILOE') {
                                            $scope.getEMIValue();
                                        }


                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/PolicyReg/GetPolicyNumber',
                                            data: data,
                                            headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            contract.PolicyNo = data;
                                        }).error(function (data, status, headers, config) {
                                        });
                                    }
                                });

                            }
                        } else if (j == 3) {
                            if (!$scope.validatePolicyInfo()) {
                                isValidated = false;
                                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.allMandatory'));
                                return;
                            } else {
                                if ($scope.currentProductTypeCode === 'ILOE') {
                                    // $scope.checkValidILOE();
                                    $scope.ValidationILOEPopup();
                                    if (!$scope.checkValidILOE()) {
                                        customErrorMessage("Please upload the attachments.");
                                        var idValidd = false;
                                        return;
                                    } else {
                                        return;
                                    }

                                } else {
                                    if ($scope.isUsedItem)
                                        $scope.manufactureWarrentyCheckForUsedItem();
                                    else {
                                        $scope.product.MWIsAvailable = true;
                                        $scope.uploadAttachments();
                                    }

                                }

                            }
                        } else if (j == 4) {

                        }
                    }
                    if (isValidated) {
                        goToStep(i);
                        $scope.toTheTop();
                    }
                } else {
                    $scope.toTheTop();
                    goToStep(i);
                }
            },
            submit: function () {
            },
            reset: function () {
            }
        };
        var nextStep = function () {
            $scope.currentStep++;
        };
        var prevStep = function () {
            $scope.currentStep--;
        };
        var goToStep = function (i) {
            $scope.currentStep = i;
        };
        var errorMessage = function (i) {
            toaster.pop('error', $filter('translate')('pages.dealerPolicyRegistration.error'), $filter('translate')('pages.dealerPolicyRegistration.errorMessages.pleaseCompleteSteps'));
        };
        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.dealerPolicyRegistration.error'), msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', $filter('translate')('pages.dealerPolicyRegistration.information'), msg, 12000);
        };

        $scope.employeeApprovedYes = false;
        $scope.isbankApproved = false;
        $scope.customerGrossSalaryYes = false;
        $scope.impendingunemploymentRisvisible = false;
        $scope.ageforToday = 0;

        $scope.checkValidILOE = function () {
            $scope.passportRisvisible = false;
            $scope.EmploymentVerifcationLetterRisvisible = false;
            $scope.VehicleInvoiceCopyRisvisible = false;
            $scope.customerGrossSalaryRisvisible = false;
            $scope.EmiratesIdCopyRisvisible = false;
            $scope.ageforTodayRisvisible = false;

            var idValidd = true;

            //angular.forEach($scope.customerAttachmentTypes, function (customerAttachment) {
            if ($scope.customerDocUploader.queue.length != 0) {
                angular.forEach($scope.customerDocUploader.queue, function (customerDocUpload) {

                    if (customerDocUpload.documentType == "UAEResidentVisaCopy") {
                        $scope.passportRisvisible = true;
                    } else if (customerDocUpload.documentType == "EmploymentVerifcationLetter") {
                        $scope.EmploymentVerifcationLetterRisvisible = true;
                    } else if (customerDocUpload.documentType == "EmiratesIdCopy") {
                        $scope.EmiratesIdCopyRisvisible = true;
                    }



                });
            } else {
                idValidd = false;
            }

            if ($scope.passportRisvisible != true) {
                idValidd = false;
            }
            if ($scope.EmploymentVerifcationLetterRisvisible != true) {
                idValidd = false;
            }
            if ($scope.EmiratesIdCopyRisvisible != true) {
                idValidd = false;
            }

            if ($scope.itemDocUploader.queue.length != 0) {
                angular.forEach($scope.itemDocUploader.queue, function (itemDocUpload) {

                    if (itemDocUpload.documentType == "VehicleInvoiceCopyFromDealer") {
                        $scope.VehicleInvoiceCopyRisvisible = true;

                    }



                });
            } else {
                idValidd = false;
            }
            if ($scope.VehicleInvoiceCopyRisvisible != true) {
                idValidd = false;
            }

            if ($scope.ageforToday >= 18 && $scope.ageforToday < 59) {
                $scope.ageforTodayRisvisible = true;
            } else {
                idValidd = false;
            }

            return idValidd;

        }
        $scope.selectedMobilechanged = function () {
            swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.checkingCustomerExistance'), showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/checkCustomerExist',
                data: {
                    "mobileNo": $scope.customer.mobileNo
                },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {

                if (data) {
                    $scope.IsCustomerNoExist = true;
                    customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.customerNoExisting'))
                }
                else {
                    $scope.IsCustomerNoExist = false;
                }
                swal.close();
            }).error(function (data, status, headers, config) {
            });

        }
        $scope.ConfirmButtonInILOE = function () {
            // $scope.checkValidILOE();
            if ($scope.checkValidILOE()) {
                customInfoMessage($filter('translate')('pages.dealerPolicyRegistration.inforMessages.successfullyConfirmed'))

                if ($scope.isUsedItem)
                    $scope.manufactureWarrentyCheckForUsedItem();
                else {

                    if (!$scope.formActionCustomer) {
                        $scope.product.MWIsAvailable = true;
                        $scope.uploadAttachmentsUpload();
                    } else {
                        $scope.product.MWIsAvailable = true;
                        $scope.uploadAttachments();
                    }


                }
                dialogChangePassword.close();
            } else {
                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadAttachments'));
            }
        }

        $scope.calculateAge = function calculateAge(birthday) {
            var date = new Date(birthday);
            var ageDifMs = Date.now() - date.getTime();
            var ageDate = new Date(ageDifMs); // miliseconds from epoch
            $scope.ageforToday = Math.abs(ageDate.getUTCFullYear() - 1970);
            //return result;
        }
        $scope.customerGrossSalarycheckBoxChangesYES = function (customerGrossSalaryapproved) {
            if (customerGrossSalaryapproved == true) {
                $scope.customerGrossSalaryYes = true;
            } else {
                $scope.customerGrossSalaryYes = false;
            }

        }
        $scope.customerGrossSalarycheckBoxChangesNo = function (customerGrossSalaryisaapproved) {
            if (customerGrossSalaryisaapproved == true) {
                $scope.customerGrossSalaryYes = false;
            } else {
                $scope.customerGrossSalaryYes = false;
            }

        }

        $scope.checkBoxChangesYES = function (visaapproved) {
            if (visaapproved == true) {
                $scope.employeeApprovedYes = true;
            } else {
                $scope.employeeApprovedYes = false;
            }

        }
        $scope.checkBoxChangesNO = function (isaapprovedno) {
            if (isaapprovedno == true) {
                $scope.employeeApprovedYes = false;
            } else {
                $scope.employeeApprovedYes = false;
            }

        }

        $scope.checkBoxBankApprovedChangesYES = function (bankapproved) {
            if (bankapproved == true) {
                $scope.isbankApproved = true;
            } else {
                $scope.isbankApproved = false;
            }

        }

        $scope.checkBoxBankApprovedChangesNO = function (bankapprovedNo) {
            if (bankapprovedNo == false) {
                $scope.isbankApproved = false;
            } else {
                $scope.isbankApproved = false;
            }
        }

        $scope.checkBoximpendingunemploymentApprovedChangesYES = function (impendingunemploymentapproved) {
            if (impendingunemploymentapproved == true) {
                $scope.impendingunemploymentRisvisible = true;
            } else {
                $scope.impendingunemploymentRisvisible = false;
            }
        }

        $scope.checkBoximpendingunemploymentApprovedChangesNO = function (impendingunemploymentapprovedNo) {
            if (impendingunemploymentapprovedNo == true) {
                $scope.impendingunemploymentRisvisible = false;
            } else {
                $scope.impendingunemploymentRisvisible = false;
            }
        }

        $scope.loanAmount = 1;
        //$scope.interestRate = 0.0399‬;
        $scope.loanTenure = 48;
        $scope.EMI = 0.0;
        $scope.ishidden = !true;

        //this function contains the EMI logic
        $scope.calculateEMI = function () {

            var loanamt = $scope.product.dealerPrice; // loanAmount
            var intrest = (3.99 / 100);
            var repaytrm = $scope.loanTenure;

            //EMI calculation logic
            var rate1 = (parseFloat(intrest) / 100) / 12;
            var rate = 1 + rate1;
            var interestRate = Math.pow(rate, repaytrm);
            var E1 = loanamt * rate1 * interestRate;
            var E2 = interestRate - 1;
            var EMI = (E1 / E2);
            var total_payable = EMI * repaytrm;
            var total_interest = (total_payable - loanamt);

            //Values to display
            $scope.EMI = EMI.toFixed(2);
            $scope.interestPayable = total_interest;
            $scope.totalPayable = total_payable;
            $scope.ishidden = !false;
        } //end function calculateEMI

        $scope.getEMIValue = function () {

            $scope.productContracts[0].FinanceAmount = $scope.product.dealerPrice - $scope.product.itemPrice;

            //if ($scope.product.lonePeriod > 48) {
            //    customErrorMessage("Limit Upto 48 Months.");
            //} else {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetEMIValue',
                data: {
                    "LoneAmount": $scope.productContracts[0].FinanceAmount,
                    "ContractId": $scope.emiContractId
                },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var EMI = data;
                if (EMI != null) {
                    $scope.EMI = EMI.toFixed(2);
                }
            }).error(function (data, status, headers, config) {
            });
            //}
        }




        $scope.ValidationILOEPopup = function () {
            $scope.impendingunemploymentRisvisible = false;
            $scope.customerGrossSalaryYes = false;
            $scope.employeeApprovedYes = false;
            $scope.isbankApproved = false;
            dialogChangePassword = ngDialog.open({
                template: 'popUpValidationILOE',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };

        $scope.iloeValidateteList = []

        $scope.viewPolicySummary = function () {

            var selectedIdType = $scope.idTypes.filter(a => a.Id == $scope.customer.idTypeId);
            var selectedUsageType = $scope.usageTypes.filter(a => a.Id == $scope.customer.usageTypeId);

            $scope.selectedIdTypeNameName = selectedIdType[0].IdTypeName;
            $scope.selectedUsageTypeName = selectedUsageType[0].UsageTypeName;
            $scope.policyDetailsPopupKm = $scope.currentCommodityTypeCode == 'A' ? 'km at Policy Sale' : ' hrs used'
            $scope.birthDayShowable = true;
            $scope.idIssueDateShowable = true;
            $scope.registrationDateShowable = true;
            $scope.fuelTypeForPopup = '';
            $scope.BodyTypeForPopup = '';
            var selFTypes = $scope.fuelTypes.filter(a => a.FuelTypeId === $scope.product.fuelTypeId);
            if (selFTypes.length > 0) {
                $scope.fuelTypeForPopup = selFTypes[0].FuelTypeDescription;
            }
            var bodyTypes = $scope.bodyTypes.filter(a => a.Id === $scope.product.bodyTypeId);
            if (bodyTypes.length > 0) {
                $scope.BodyTypeForPopup = bodyTypes[0].VehicleBodyTypeDescription;
            }

            if ($scope.customer.dateOfBirth == undefined || $scope.customer.dateOfBirth == null || $scope.customer.dateOfBirth == '') {
                $scope.birthDayShowable = false;
            }
            if ($scope.customer.idIssueDate == undefined || $scope.customer.idIssueDate == null || $scope.customer.idIssueDate == '') {
                $scope.idIssueDateShowable = false;
            }
            if ($scope.product.itemPurchasedDate == undefined || $scope.product.itemPurchasedDate == null || $scope.product.itemPurchasedDate == '') {
                $scope.product.itemPurchasedDate = GetToday();
            }
            if ($scope.product.registrationDate == undefined || $scope.product.registrationDate == null || $scope.product.registrationDate == '') {
                $scope.registrationDateShowable = false;
            }






            ViewSumaryPopup = ngDialog.open({
                template: 'popUpSummaryView',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            $scope.selectedDealerBrancheChanged();
            $scope.selectedModelChanged();
            $scope.selectedVehicleStatusChanged();
            $scope.selectedEngineCapacityChanged();
            $scope.selectedCylinderCountChanged();
            $scope.selectedVariantChanged();
            $scope.selectedFuelTypeChanged()
            $scope.selectedTransmissionTypeChanged();
            $scope.selectedBodyTypeChanged();
            $scope.selectedAspirationTypeChanged();
            $scope.selectedCommodityUsageTypeChanged()
        }
        $scope.removedAttachments = [];
        $scope.uploadAttachments = function () {
            if (!$scope.formAction) {
                for (var i = 0; i < $scope.attachments_temp.length; i++) {
                    var isRemoved = true;
                    if ($scope.attachments_temp[i].AttachmentSection == "Customer") {
                        for (var j = 0; j < $scope.customerDocUploader.queue.length; j++) {
                            if (typeof $scope.customerDocUploader.queue[j].ref === 'undefined') {
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.customerDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Item") {

                        for (var j = 0; j < $scope.itemDocUploader.queue.length; j++) {
                            if (typeof $scope.itemDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.itemDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Policy") {
                        for (var j = 0; j < $scope.policyDocUploader.queue.length; j++) {
                            if (typeof $scope.policyDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.policyDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Payment") {
                        for (var j = 0; j < $scope.paymentDocUploader.queue.length; j++) {
                            if (typeof $scope.paymentDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.paymentDocUploader.queue[j].ref) {
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

            } else {
                console.log($scope.ImageArray);
                if ($scope.ImageArray.length > 0) {
                    var json = JSON.stringify($scope.ImageArray);
                    console.log(json.length);
                    swal({ title: "Uploading", text: "Uploading Scanned Images...", showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Upload/UploadScannedAttachment',
                        contentType: 'application/json',
                        processData: false,
                        data: { "ImageArray": $scope.ImageArray },
                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        console.log(data);
                        if (data !== "Failed") {
                            angular.forEach(data, function (value) {
                                $scope.uploadedDocIds.push(value);
                            });

                        } else
                            customErrorMessage("Document upload failed.");


                        $scope.savePolicy();
                    }).error(function (data, status, headers, config) {
                        swal.close();

                    });

                } else {
                    $scope.savePolicy();

                }

                //$scope.savePolicy();
            }

        }


        $scope.uploadAttachmentsUpload = function () {
            if (!$scope.formAction) {
                for (var i = 0; i < $scope.attachments_temp.length; i++) {
                    var isRemoved = true;
                    if ($scope.attachments_temp[i].AttachmentSection == "Customer") {
                        for (var j = 0; j < $scope.customerDocUploader.queue.length; j++) {
                            if (typeof $scope.customerDocUploader.queue[j].ref === 'undefined') {
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.customerDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Item") {

                        for (var j = 0; j < $scope.itemDocUploader.queue.length; j++) {
                            if (typeof $scope.itemDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.itemDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Policy") {
                        for (var j = 0; j < $scope.policyDocUploader.queue.length; j++) {
                            if (typeof $scope.policyDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.policyDocUploader.queue[j].ref) {
                                    isRemoved = false;
                                    // return false;
                                }
                            }
                        }
                    } else if ($scope.attachments_temp[i].AttachmentSection == "Payment") {
                        for (var j = 0; j < $scope.paymentDocUploader.queue.length; j++) {
                            if (typeof $scope.paymentDocUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                if ($scope.attachments_temp[i].FileServerRef == $scope.paymentDocUploader.queue[j].ref) {
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

            $scope.customerDocUploader.queue = [];
            $scope.policySaveStatusTitle = "";
            $scope.policySaveStatusMsg = "";
            if ($scope.customerDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.customerDocUploader.queue.length; i++) {
                    $scope.customerDocUploader.queue[i].file.name = $scope.customerDocUploader.queue[i].file.name + '@@' + $scope.customerDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.uploadingCustomerAttachments');
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.customerDocUploader.queue.length;
                $scope.customerDocUploader.uploadAll();
            } else if ($scope.itemDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.itemDocUploader.queue.length; i++) {
                    $scope.itemDocUploader.queue[i].file.name = $scope.itemDocUploader.queue[i].file.name + '@@' + $scope.itemDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.uploadingItemAttachments');
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.itemDocUploader.queue.length;
                $scope.itemDocUploader.uploadAll();
            } else if ($scope.policyDocUploader.queue.length > 0) {
                for (var i = 0; i < $scope.policyDocUploader.queue.length; i++) {
                    $scope.policyDocUploader.queue[i].file.name = $scope.policyDocUploader.queue[i].file.name + '@@' + $scope.policyDocUploader.queue[i].documentType;
                }
                $scope.currentUploadingItem = 0;
                $scope.currentUploadingItem++;
                $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.uploadingPolicyAttachments');
                $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.policyDocUploader.queue.length;
                $scope.policyDocUploader.uploadAll();

            } else {
                console.log($scope.ImageArray);
                if ($scope.ImageArray.length > 0) {
                    var json = JSON.stringify($scope.ImageArray);
                    console.log(json.length);
                    swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.uploading'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.uploadingImages'), showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Upload/UploadScannedAttachment',
                        contentType: 'application/json',
                        processData: false,
                        data: { "ImageArray": $scope.ImageArray },
                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        console.log(data);
                        if (data !== "Failed") {
                            angular.forEach(data, function (value) {
                                $scope.uploadedDocIds.push(value);
                            });

                        } else
                            customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.uploadfailed'));


                        $scope.savePolicy();
                    }).error(function (data, status, headers, config) {
                        swal.close();

                    });

                } else {
                    $scope.savePolicy();

                }

                //$scope.savePolicy();
            }

        }



        //$scope.eligibilityCheck = function (contractId) {
        //    if ($scope.policy.hrsUsedAtPolicySale == '' && !parseFloat($scope.policy.hrsUsedAtPolicySale)) {
        //        $scope.policy.hrsUsedAtPolicySale = 0;
        //    } else {
        //        $scope.policy.hrsUsedAtPolicySale = parseFloat($scope.policy.hrsUsedAtPolicySale);
        //    }

        //    var usedUnits = $scope.policy.hrsUsedAtPolicySale;
        //    var itemPurchesedDate = $scope.product.registrationDate;
        //    if (isGuid(contractId)) {
        //        var eligibilityCheckRequest = {
        //            itemPurchesedDate: itemPurchesedDate,
        //            policySoldDate: $scope.policy.policySoldDate,
        //            usedAmount: usedUnits,
        //            contractId: contractId
        //        };
        //        $http({
        //            method: 'POST',
        //            url: '/TAS.Web/api/PolicyReg/EligibilityCheckRequest',
        //            data: { "eligibilityCheckRequest": eligibilityCheckRequest },
        //            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //        }).success(function (data, status, headers, config) {
        //            if (data.status === "YES") {
        //                $scope.isEligible = true;
        //                if (parseFloat(data.premium) && parseFloat(data.premium) > 0) {
        //                    angular.forEach($scope.productContracts, function (value) {
        //                        if (contractId == value.ContractId) {
        //                            //if (data.isPercentage) {
        //                            //    var tempPremium = value.Premium;
        //                            //    value.Premium += tempPremium * (data.premium / 100);
        //                            //    $scope.GrossTotal += tempPremium * (data.premium / 100);
        //                            //    $scope.GrossTotalTmp = $scope.GrossTotal;
        //                            //    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                            //} else {
        //                            //    value.Premium += data.premium;
        //                            //    $scope.GrossTotal += data.premium;
        //                            //    $scope.GrossTotalTmp = $scope.GrossTotal;
        //                            //    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                            //}
        //                            return false;
        //                        }
        //                    });
        //                }
        //            } else {
        //                $scope.isEligible = false;
        //                customErrorMessage("Mandatory eligibility criterias not satisfied.");
        //            }
        //        }).error(function (data, status, headers, config) {
        //        });
        //    } else {
        //        angular.forEach($scope.productContracts, function (value) {
        //            var eligibilityCheckRequest = {
        //                itemPurchesedDate: itemPurchesedDate,
        //                policySoldDate: $scope.policy.policySoldDate,
        //                usedAmount: usedUnits,
        //                contractId: value.ContractId
        //            };
        //            $http({
        //                method: 'POST',
        //                url: '/TAS.Web/api/PolicyReg/EligibilityCheckRequest',
        //                data: { "eligibilityCheckRequest": eligibilityCheckRequest },
        //                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //            }).success(function (data, status, headers, config) {
        //                if (data.status === "YES") {
        //                    $scope.isEligible = true;
        //                    if (parseFloat(data.premium) && parseFloat(data.premium) > 0) {
        //                        //if (data.isPercentage) {
        //                        //    var tempPremium = value.Premium;
        //                        //    value.Premium += tempPremium * (data.premium / 100);
        //                        //    $scope.GrossTotal += tempPremium * (data.premium / 100);
        //                        //    $scope.GrossTotalTmp = $scope.GrossTotal;
        //                        //    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                        //} else {
        //                        //    value.Premium += data.premium;
        //                        //    $scope.GrossTotal += data.premium;
        //                        //    $scope.GrossTotalTmp = $scope.GrossTotal;
        //                        //    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                        //}
        //                    }
        //                } else {
        //                    $scope.isEligible = false;
        //                    //customErrorMessage("Mandatory eligibility criterias not satisfied.");
        //                }
        //            }).error(function (data, status, headers, config) {
        //            });

        //        });
        //    }

        //}

        $scope.manufactureWarrentyCheckForUsedItem = function () {
            swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.manufacturerAvailability'), showConfirmButton: false });
            var data = {
                makeId: $scope.product.makeId,
                modelId: $scope.product.modelId,
                dealerId: $scope.product.dealerId,
                commodityTypeId: $scope.product.commodityTypeId,
                mwStartDate: $scope.product.MWStartDate,
                policySoldDate: $scope.policy.policySoldDate,
                usage: $scope.policy.hrsUsedAtPolicySale
            };
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/ManufacturerWarrentyAvailabilityCheckOnPolicySave',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.code === 'No') {
                    swal({
                        title: "",
                        text: "Item is under manufacturer warranty, You want to proceed with existing manufacturer warranty?",
                        type: "info",
                        showCancelButton: true,
                        confirmButtonClass: "btn-danger",
                        confirmButtonText: "Yes, proceed!",
                        cancelButtonText: "No",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.product.MWIsAvailable = true;
                            } else {
                                $scope.product.MWIsAvailable = false;
                            }
                            $scope.uploadAttachments();
                        });
                } else {
                    $scope.product.MWIsAvailable = false;
                    $scope.uploadAttachments();
                }
            }).error(function (data, status, headers, config) {
                swal.close();
            });
        }

        $scope.downloadStatement = function (currentPolicyBundleId) {

            swal({ title: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing'), text: $filter('translate')('pages.dealerPolicyRegistration.inforMessages.yourDownload'), showConfirmButton: false });

            if ($scope.currentProductCode === 'TYRE') {
                var data = {
                    "policyid": currentPolicyBundleId
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/DownloadPolicyStatementforTYER',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: data,
                }).success(function (datar, status, headers, config) {
                    //alert(datar);
                    if (typeof datar !== "undefined" && datar.length > 0) {
                        var url = $location.protocol() + '://' + $location.host() +
                            '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + $localStorage.jwt;
                        // alert(url);
                        window.open(url, '_blank')
                        //var publicurl = $location.protocol() + '://' + $location.host() + '/TAS.Web/contisurewarranty.pdf';
                        //window.open(publicurl, '_blank')
                        swal.close();
                    } else {
                        customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.contactAdministrator'));
                        swal.close();
                    }
                }).error(function (data, status, headers, config) {
                });

            } else {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetPolicyAttachmentById',
                    data: { "policyid": currentPolicyBundleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    responseType: 'arraybuffer',
                }).success(function (data, status, headers, config) {
                    var success = false;
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
                        success = true;
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

        $scope.savePolicy = function () {

            //if (!$scope.isEligible && !$scope.formAction) {
            //    customErrorMessage("Mandatory eligibility criterias not satisfied. Please check usage and registration date.");
            //    $scope.validate_hrsUsedAtPolicySale = "has-error";
            //    goToStep(2);
            //    swal.close();
            //    return false;
            //} else {
            //    $scope.validate_hrsUsedAtPolicySale = 0;
            //}

            $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.processing');
            $scope.policySaveStatusMsg = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.policySaving');
            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
            $scope.policy.productContracts = $scope.productContracts;
            $scope.policy.premium = $scope.GrossTotalTmp;
            //dynamic binding issues
            angular.forEach($scope.policy.productContracts, function (contractObj) {
                if (contractObj.Contracts.length === 1) {
                    contractObj.ContractId = contractObj.Contracts[0].Id;
                }

                if (contractObj.ExtensionTypes.length === 1) {
                    contractObj.ExtensionTypeId = contractObj.ExtensionTypes[0].Id;
                }

                if (contractObj.AttributeSpecifications.length === 1) {
                    contractObj.AttributeSpecificationId = contractObj.AttributeSpecifications[0].Id;
                }

                if (contractObj.CoverTypes.length === 1) {
                    contractObj.CoverTypeId = contractObj.CoverTypes[0].Id;
                }
            });
            console.log($scope.policy.productContracts);
            //setting up nessasary data for cooperate user
            if ($scope.selectedCustomerTypeName == 'Corporate') {
                $scope.customer.gender = "M";
                $scope.customer.idIssueDate = "1/1/1753";
                $scope.customer.dateOfBirth = "1/1/1753";
                $scope.customer.idTypeId = 1;
                $scope.customer.nationalityId = 0;
            }

            if ($scope.currentProductTypeCode == 'ILOE') {
                $scope.product.MWStartDate = "1/1/1753";
                //$scope.product.itemPrice = 0.00;
                $scope.policy.Emi = $scope.EMI;
            }

            if ($scope.currentCommodityTypeCode == "O") {
                //hardcoded for tyre coz this are not required

                $scope.product.MWStartDate = "1/1/1753";
                if (!parseInt($scope.customer.idTypeId) || parseInt($scope.customer.idTypeId) === 0)
                    $scope.customer.idTypeId = 1;
                if ($scope.customer.idIssueDate == '' || typeof $scope.customer.idIssueDate === 'undefined')
                    $scope.customer.idIssueDate = "1/1/1753";
                if ($scope.customer.dateOfBirth == '' || typeof $scope.customer.dateOfBirth === 'undefined')
                    $scope.customer.dateOfBirth = "1/1/1753";
                if ($scope.customer.gender == '' || typeof $scope.customer.gender === 'undefined')
                    $scope.customer.gender = "M";
            }

            var policyDetails = {
                customer: $scope.customer,
                product: $scope.product,
                policy: $scope.policy,
                payment: $scope.payment,
                policyDocIds: $scope.uploadedDocIds
            };
            if ($scope.formAction) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/SavePolicy',
                    data: { "policyDetails": policyDetails },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == 'success') {
                        $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.success');
                        $scope.policySaveStatusMsg = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.policyEnteredSucc');
                        $scope.policySaveStatusConfirmButtons = true;
                        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        $scope.resetAll();
                        goToStep(1);

                    } else {
                        $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.error');
                        $scope.policySaveStatusMsg = data;
                        prevStep();
                        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                    }

                }).error(function (data, status, headers, config) {
                    swal.close();
                });
            } else {
                //update policy
                if (isGuid($scope.policy.id)) {
                    //deleting removed documents
                    if ($scope.removedAttachments.length > 0) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Upload/DeleteAttachments',
                            data: { "AttachmentIds": $scope.removedAttachments },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == "Failed") {
                                customErrorMessage($filter('translate')('pages.dealerPolicyRegistration.errorMessages.removedAttachments'))
                            }

                        }).error(function (data, status, headers, config) {
                        });
                    }

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/UpdatePolicyV2',
                        data: { "policyDetails": policyDetails },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == 'success') {
                            $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.success');
                            $scope.policySaveStatusMsg = $filter('translate')('pages.dealerPolicyRegistration.inforMessages.policyEnteredUpdate');
                            $scope.policySaveStatusConfirmButtons = true;
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                            $scope.resetAll();
                            goToStep(1);

                        } else {
                            $scope.policySaveStatusTitle = $filter('translate')('pages.dealerPolicyRegistration.error');
                            $scope.policySaveStatusMsg = data;
                            prevStep();
                            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        }

                    }).error(function (data, status, headers, config) {
                        swal.close();
                    });
                }
            }
        }



        /*----------------------Scanner Implementation----------------------
           Insert Dynamsoft.WebTwainEnv.Load(); to Init Method
           Define global variables
        */

        $scope.scannerPopUp = function (tabid) {
            $scope.tabId = tabid;
            // validation for selecting attachment type
            //console.log($scope.customerAttachmentType);
            //if (($scope.customerAttachmentType == null ||
            //    $scope.customerAttachmentType == 'undefined' ||
            //    $scope.customerAttachmentType == "") && tabid == 0) {

            //    customErrorMessage("Please Select Attachment Type");
            //    return false;
            //}
            if (tabid == 0) {
                if ($scope.customerAttachmentType == null ||
                    $scope.customerAttachmentType == 'undefined' ||
                    $scope.customerAttachmentType == "") {
                    customErrorMessage("Please Select Customer Attachment Type");
                    return false;
                }
            }

            if (tabid == 1) {
                if ($scope.itemAttachmentType == null ||
                    $scope.itemAttachmentType == 'undefined' ||
                    $scope.itemAttachmentType == "") {
                    customErrorMessage("Please Select Item Attachment Type");
                    return false;
                }
            }

            if (tabid == 2) {
                if ($scope.policyAttachmentType == null ||
                    $scope.policyAttachmentType == 'undefined' ||
                    $scope.policyAttachmentType == "") {
                    customErrorMessage("Please Select Item Attachment Type");
                    return false;
                }
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
            // console.log($scope.customerAttachmentType);
            var DocattachmentType;

            //console.log("Tab Id " + $scope.tabId);
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
            console.log(imagecount);
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

        $scope.removeAttachmentFromImageArray = function (index) {
            if (index > -1) {
                $scope.ImageArray.splice(index, 1);
            }
        }

        $scope.Dynamsoft_OnReady = function () {
            // Dynamsoft.WebTwainEnv.Unload();
            Dynamsoft.WebTwainEnv.Load();
            // console.log('here');
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

        $scope.clearScannedImageSections = function (sectionName) {
            for (var i = $scope.ImageArray.length - 1; i >= 0; i--) {
                if ($scope.ImageArray[i].section == sectionName)
                    $scope.ImageArray.splice(i, 1);
            }
        }

        //$scope.uploadScannerDocuments = function () {
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/Upload/UploadScannedAttachment',
        //        data: { "ImageArray": $scope.ImageArray },
        //        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        //        console.log(data);
        //        if (data !== "Failed") {
        //            angular.forEach(data, function (value) {
        //                $scope.uploadedDocIds.push(value);
        //            });
        //        }
        //    }).error(function (data, status, headers, config) {
        //        swal.close();

        //    });

        //}

        $(function () {
            $('[data-toggle="tooltip"]').popover({
                container: 'body'

            })
        })

        $scope.closedialog = function () {
            ScannerPopUp.close();
        }

        $scope.selectedItemStatus = function () {
            //    angular.forEach($scope.itemStatuses, function (value) {
            //        if ($scope.product.itemStatusId === value.Id) {
            //            $scope.isAvalable = false;
            //            return false;
            //        } else {
            //            $scope.isAvalable = true;
            //        }
            //    });
            //} else {
            //    $scope.isAvalable = false;
            //}

        }


    });
