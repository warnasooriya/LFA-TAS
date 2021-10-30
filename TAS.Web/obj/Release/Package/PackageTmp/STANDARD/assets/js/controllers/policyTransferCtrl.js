

app.controller('PolicyTransferCtrl',
    function ($scope, $rootScope, $http, $parse, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster, $state, FileUploader) {
        $scope.currentStep = 1;
        $scope.currentUploadingItem = 0;
        $scope.formActionText = "Transfer a policy to a new or existing customer";
        $scope.formAction = true;//true for add new
        $scope.selectedCustomerTypeName = '';
        $scope.selectedUsageTypeName = '';
        $scope.serialNumberLength = '';
        $scope.currentCommodityTypeCode = '';
        $scope.discountAvailable = true;
        $scope.isPaymentTypesAvailable = false;
        $scope.isOneBranchAvailable = false;
        $scope.isProductDetailsReadonly = true;
        $scope.isPolicyDetailsDisabled = true;
        $scope.isPaymentDetailsDisabled = true;

        $scope.customerDisabled = false;
        $scope.GrossTotal = 0.0;
        $scope.serialNumber_temp = '';
        $scope.transferFee = "0.00"
        var SearchCustomerPopup, SearchVehiclePopup, SearchElectronicPopup, ViewSumaryPopup, SearchPolicyPopup, SearchOtherPopup, SearchYellowGoodPopup, PremiumBreakdownPopup;
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
        $scope.PolicyBreakdown = {};
        $scope.uploadedDocIds = [];
        $scope.policyHistory = [];
        //$scope.policyHistory = {
        //    ModifiedDate:'',
        //    Customer: '',
        //    PolicySoldDate:''
        //}
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
                customErrorMessage("Error occured while uploading attachments.");
                $scope.customerDocUploader.cancelAll();
            }
        }
        $scope.customerDocUploader.onCompleteAll = function () {
            // swal.close();
            //$scope.tmp_customerDocs = $scope.customerDocUploader.queue;
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
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'PolicyReg', 'Section': 'Policy' },
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
        //mvvm variables
        $scope.customer = {
            customerId: emptyGuid(),
            customerTypeId: emptyGuid(),
            usageTypeId: emptyGuid(),
            firstName: '',
            lastName: '',
            dateOfBirth: '',
            gender: '',
            idTypeId: emptyGuid(),
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
            businessAddress4: ''
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
            itemPurchasedDate: '',
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
            grossWeight: '',
            MWStartDate: ''
        };

        $scope.policy = {
            tpaBranchId: emptyGuid(),
            id: emptyGuid(),
            policySoldDate: '',
            hrsUsedAtPolicySale: 0,
            salesPersonId: emptyGuid(),
            dealerPolicy: 'false',
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
                    swal({ title: 'TAS Security Information', text: 'No curency period is defined for today.Please contact Administrator.', showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    swal({ title: 'TAS Security Information', text: 'Error occured while getting currency period information.Please contact Administrator.', showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                }

            }).error(function (data, status, headers, config) {
            });

        }
        $scope.validityCheck = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/TPABranch/GetTpaBranchesByUserId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId },
            }).success(function (data, status, headers, config) {
                if (data == '' || data == null) {
                    swal({ title: 'TAS Security Information', text: 'You don\'t have assiged to any TPA Branch.Please contact administrator.', showConfirmButton: false });
                    $scope.allowedTpaBranches = [];
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    $scope.PolicySearchPopup();
                    $scope.policy.tpaBranchId = data[0].Id;
                    if (data.length == 1) {
                        $scope.isOneBranchAvailable = true;
                    }
                    $scope.loadInitialData();
                    $scope.allowedTpaBranches = data;

                }
                //$scope.countries = data;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.loadInitialData = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
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
                $scope.product.commodityTypeId = data[0].CommodityTypeId;
                $scope.selectedCommodityTypeChanged();
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

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.itemStatuses = data;
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
                $scope.selectedIdTypeNameName = '';
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
        //$scope.selectedCustomerTypeIdChanged = function () {
        //    if (parseInt($scope.customer.customerTypeId)) {
        //        angular.forEach($scope.customerTypes, function (value) {
        //            if ($scope.customer.customerTypeId == value.Id) {
        //                $scope.selectedCustomerTypeName = value.CustomerTypeName;
        //                return false;
        //            }
        //        });
        //    } else {
        //        $scope.selectedCustomerTypeName = '';
        //    }
        //}
        $scope.selectedCustomerTypeIdChanged = function () {
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
                                angular.forEach($scope.usageTypes, function (valuec) {
                                    if (valuec.UsageTypeName == "Commercial") {
                                        $scope.customer.usageTypeId = valuec.Id;

                                    }
                                });

                                angular.forEach($scope.commodityUsageTypes, function (valuec) {
                                    if (valuec.Name == "Commercial") {
                                        $scope.product.commodityUsageTypeId = valuec.Id;

                                    }
                                });
                            }
                        }
                    });
                }

            } else {
                $scope.selectedCustomerTypeName = '';
            }
        }
        $scope.selectedModelChange = function () {
            $scope.variants = [];
            if (isGuid($scope.product.modelId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                    data: { "Id": $scope.product.modelId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.variants = data;

                    if (!$scope.formAction) {
                        if (loadingPolicyData) {
                            policyLoadingRequests.push({ 'reqName': 'GetContractsByIds' });
                            policyLoadingRequests.push({ 'reqName': 'GetExtensionTypesByContractId' });
                            policyLoadingRequests.push({ 'reqName': 'GetAttributeSpecificationByExtensionId' });
                            policyLoadingRequests.push({ 'reqName': 'GetCoverTypesByExtensionId' });
                        }
                        $scope.policySoldDateChanged();
                        $scope.checkSwalClose('GetAllVariantByModelId');
                    }
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllVariantByModelId');
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
        $scope.selectedCommodityTypeChanged = function () {
            $scope.products = [];
            $scope.commodityCategories = [];
            if (isGuid($scope.product.commodityTypeId)) {
               // $scope.isProductDetailsReadonly = false;
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
            $scope.productFormReset();
        }
        $scope.selectedProdctChanged = function () {
            if (isGuid($scope.product.productId)) {
               // $scope.isProductDetailsReadonly = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllChildProducts',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.product.productId }
                }).success(function (data, status, headers, config) {
                    $scope.productContracts = [];
                    angular.forEach(data, function (childProduct) {
                        var productContract = {
                            Id: emptyGuid(),
                            ProductId: childProduct.Id,
                            ParentProductId: $scope.product.productId,
                            ContractId: emptyGuid(),
                            ExtensionTypeId: emptyGuid(),
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
                       // $scope.policySoldDateChanged();
                    }
                    $scope.checkSwalClose('GetAllChildProducts');

                }).error(function (data, status, headers, config) {
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
                    $scope.checkSwalClose('GetAllDealerLocationsByDealerId');
                    if (!isInitial)
                        $scope.serialNumberCheck();
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAllDealerLocationsByDealerId');
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
                                            title: "TAS Information",
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
           // $scope.isProductDetailsReadonly = false;
            if (isGuid($scope.product.categoryId)) {

                angular.forEach($scope.commodityCategories, function (value) {

                    if ($scope.product.categoryId == value.CommodityCategoryId) {
                        $scope.serialNumberLength = value.Length;
                        $scope.serialNumberLength_temp = value.Length;
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
                    $scope.checkSwalClose('GetMakesByCommodityCategoryId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetMakesByCommodityCategoryId');
                });
            } else {
                $scope.serialNumberLength = '';
                $scope.checkSwalClose('GetMakesByCommodityCategoryId');
            }
        }
        $scope.selectedMakeChanged = function () {
            $scope.models = [];
            if (isGuid($scope.product.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.product.makeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.models = data;
                    $scope.checkSwalClose('GetModelesByMakeId');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetModelesByMakeId');
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
            try {
                $scope.selectedVariant = $.grep($scope.variants, function (val) {
                    return val.Id == $scope.product.variantId;
                })[0].VariantName;
            } catch (e) {
                $scope.selectedVariant = '';
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
                            "UsageTypeId": $scope.product.commodityUsageTypeId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.checkSwalClose('GetContractsByIds');
                        if (data == '')
                            customErrorMessage("No Deals found for selected criterias.");
                        valProd.Contracts = data;
                        if (!$scope.formAction) {
                            $scope.setupProductContractValuesFromUpdate();
                        }
                    }).error(function (data, status, headers, config) {
                        $scope.checkSwalClose('GetContractsByIds');
                    });
                });


            }
        }
        $scope.SetContractValue = function (Name) {
            if (Name != '') {
                angular.forEach($scope.productContracts, function (value) {
                    if (value.Name == Name) {
                        value.ExtensionTypes = [];
                        value.CoverTypes = [];
                        if (parseFloat($scope.GrossTotal < value.Premium)) {
                            $scope.GrossTotal = 0.00;
                        } else {
                            $scope.GrossTotal -= value.Premium;

                        }
                        $scope.GrossTotalTmp = $scope.GrossTotal;
                        $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                        value.Premium = '';
                        value.ExtensionTypeId = emptyGuid();
                        value.CoverTypeId = emptyGuid();
                        angular.forEach(value.Contracts, function (valueC) {
                            if (valueC.Id == value.ContractId && !valueC.DiscountAvailable) {
                                $scope.discountAvailable = false;
                            }
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByContractId',
                            //data: { "Id": value.ContractId },
                            data: {
                                "ContractId": value.ContractId,
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
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            value.ExtensionTypes = data;
                            $scope.checkSwalClose('GetExtensionTypesByContractId');
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ContractManagement/GetAttributeSpecificationByExtensionId',
                                data: {

                                    "ExtensionId": value.ExtensionTypeId,
                                    "ContractId": value.ContractId,
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
                                value.AttributeSpecifications = data;
                                $scope.checkSwalClose('GetAttributeSpecificationByExtensionId');
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/ContractManagement/GetCoverTypesByExtensionId',
                                    data: {
                                        "AttributeSpecificationId": value.AttributeSpecificationId,
                                        "ExtensionId": value.ExtensionTypeId,
                                        "ContractId": value.ContractId,
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
                                    $scope.checkSwalClose('GetCoverTypesByExtensionId');
                                    if (data.length === 0)
                                        customErrorMessage('Item status does not match with any cover type.');
                                    value.CoverTypes = data;

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/ContractManagement/GetPremium',
                                        data: {
                                            "CoverTypeId": value.CoverTypeId,
                                            "AttributeSpecificationId": value.AttributeSpecifications[0].Id,
                                            "ExtensionId": value.ExtensionTypeId,
                                            "ContractId": value.ContractId,
                                            "Usage": $scope.policy.hrsUsedAtPolicySale,
                                            "ItemStatusId": $scope.product.itemStatusId,

                                            "DealerPrice": $scope.product.dealerPrice,
                                            "ItemPurchasedDate": $scope.product.itemPurchasedDate,

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
                                        // contract.CoverTypes = data;
                                        $scope.checkSwalClose('GetPremium');
                                        if (data.Status === 'ok') {
                                            value.Premium = data.TotalPremium;
                                            $scope.GrossTmpPaymentType += parseFloat(data.TotalPremium);
                                            $scope.DealerCurrencyName = data.Currency;
                                            value.Currency = data.Currency;
                                            $scope.calculateAllPremiums();
                                            $scope.selectedPaymentTypeChanged();
                                        } else {
                                            customErrorMessage(data);
                                        }

                                    }).error(function (data, status, headers, config) {
                                        $scope.checkSwalClose('GetPremium');
                                    });

                                }).error(function (data, status, headers, config) {
                                    $scope.checkSwalClose('GetCoverTypesByExtensionId');
                                });
                            }).error(function (data, status, headers, config) {
                                $scope.checkSwalClose('GetAttributeSpecificationByExtensionId');
                            });

                        }).error(function (data, status, headers, config) {
                            $scope.checkSwalClose('GetExtensionTypesByContractId');
                        });
                    }
                });
            }
        }
        //$scope.SetExtensionTypeValue = function (Name) {
        //    angular.forEach($scope.productContracts, function (value) {
        //        if (value.Name == Name) {
        //            value.CoverTypes = [];
        //            value.CoverTypeId = emptyGuid();
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
                        customErrorMessage('Item status does not match with any cover type.');
                    contract.CoverTypes = data;
                }).error(function (data, status, headers, config) {
                });

            } else {
                contract.CoverTypes = [];
            }
        }

        //$scope.SetCoverTypeValue = function (Name) {
        //    var tax = [];
        //    var commissions = [];
        //    $scope.GrossTotal = 0.00;
        //    $scope.GrossTotalTmp = 0.00;
        //    $scope.GrossTmpPaymentType = 0.00;
        //    angular.forEach($scope.productContracts, function (value) {
        //        if (value.Name == Name && value.CoverTypeId != undefined) {
        //            $http({
        //                method: 'POST',
        //                url: '/TAS.Web/api/ContractManagement/GetContractExtensionByIds',
        //                data: {
        //                    "ContractId": value.ContractId, "ExtensionTypeId": value.ExtensionTypeId, "WarrantyTypeId": value.CoverTypeId, "RSA": value.RSA,
        //                    "VariantId": $scope.product.variantId, "ModelId": $scope.product.modelId
        //                },
        //                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //            }).success(function (data, status, headers, config) {
        //                var ContractExtension = data;
        //                //disable gross premium textbox
        //                angular.forEach(value.Contracts, function (con) {
        //                    if (con.Id == value.ContractId) {
        //                        value.DiscountAvailable = !con.DiscountAvailable;
        //                    }
        //                });

        //                 $http({
        //                    method: 'POST',
        //                    url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
        //                    data: { "Id": ContractExtension.PremiumCurrencyId },
        //                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //                }).success(function (data, status, headers, config) {
        //                    value.PremiumCurrencyTypeId = data.Id;
        //                    value.PremiumCurrencyName = data.Code;
        //                    if (parseFloat(ContractExtension.MinGross) > 0 || parseFloat(ContractExtension.MaxGross > 0)
        //                        || ContractExtension.PremiumBasedOnIdGross == '4e42ab9b-20c9-469d-a7b8-56593bda0910') {
        //                        $scope.premiumBasedonGross = true;
        //                        //if (ContractExtension.taxValue == 0) {
        //                        var PremiumForTax = $scope.product.dealerPrice * (parseFloat(ContractExtension.GrossPremium) / 100);
        //                        if (PremiumForTax < parseFloat(ContractExtension.MinGross)) {
        //                            PremiumForTax = parseFloat(ContractExtension.MinGross);
        //                        } else if (PremiumForTax > parseFloat(ContractExtension.MaxGross)) {
        //                            PremiumForTax = parseFloat(ContractExtension.MaxGross);
        //                        }
        //                        $http({
        //                            method: 'POST',
        //                            url: '/TAS.Web/api/ContractManagement/GetContractTaxesByExtensionId',
        //                            data: { "ContractId": value.ContractId, "PremiumForTax": PremiumForTax },
        //                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //                        }).success(function (data, status, headers, config) {
        //                            //get tax data when premium based on rp
        //                            value.Premium = parseFloat(parseFloat(PremiumForTax) + parseFloat(data)).round(2);
        //                            $scope.GrossTotal += parseFloat(PremiumForTax) + parseFloat(data);
        //                            $scope.GrossTotalTmp = $scope.GrossTotal.toFixed(2);
        //                            $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                            $scope.eligibilityCheck(value.ContractId);
        //                            $scope.selectedPaymentTypeChanged();
        //                        }).error(function (data, status, headers, config) {
        //                        });
        //                    } else {
        //                        $scope.premiumBasedonGross = false;
        //                        value.Premium = parseFloat(ContractExtension.GrossPremium).round(2);
        //                        $scope.GrossTotal += ContractExtension.GrossPremium;

        //                    }
        //                    $scope.GrossTotalTmp = $scope.GrossTotal.toFixed(2);
        //                    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //                   // $scope.eligibilityCheck(value.ContractId);
        //                    $scope.selectedPaymentTypeChanged();
        //                }).error(function (data, status, headers, config) {
        //                });

        //            }).error(function (data, status, headers, config) {
        //            });
        //        }

        //    });

        //}

        $scope.SetCoverTypeValue = function (contract) {
            contract.Premium = 0.00;
            if (isGuid(contract.CoverTypeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetPremium',
                    data: {
                        "CoverTypeId": contract.CoverTypeId,
                        "AttributeSpecificationId": contract.AttributeSpecifications[0].Id,
                        "ExtensionId": contract.ExtensionTypeId,
                        "ContractId": contract.ContractId,
                        "Usage": $scope.policy.hrsUsedAtPolicySale,
                        "ItemStatusId": $scope.product.itemStatusId,

                        "DealerPrice": $scope.product.dealerPrice,
                        "ItemPurchasedDate": $scope.product.itemPurchasedDate,

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
                    // contract.CoverTypes = data;
                    if (data.Status === 'ok') {
                        contract.Premium = data.TotalPremium;
                        $scope.GrossTmpPaymentType += parseFloat(data.TotalPremium);
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
        $scope.calculateAllPremiums = function () {
            $scope.PremiumBeforePaymentFees = 0.00;
            $scope.PremiumAfterPaymentFees = 0.00;
            $scope.PremiumAfterPaymentFeesAndDiscounts = 0.00;

            angular.forEach($scope.productContracts, function (contract) {
                $scope.PremiumBeforePaymentFees = $scope.PremiumAfterPaymentFeesAndDiscounts = $scope.PremiumAfterPaymentFees += parseFloat(contract.Premium);
            });

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
        $scope.pub_PaymentType = "Value";
        $scope.pub_PaymentCharg = 0.00;

        $scope.selectedPaymentTypeChanged = function () {

            if (isGuid($scope.payment.paymentTypeId)) {
                $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotalTmp).toFixed(2);
                angular.forEach($scope.paymentTypes, function (paymentType) {
                    if (paymentType.Id === $scope.payment.paymentTypeId) {
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
        //$scope.selectedPaymentTypeChanged = function () {

        //    if (isGuid($scope.payment.paymentTypeId)) {
        //        $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotalTmp).toFixed(2);
        //        angular.forEach($scope.paymentTypes, function (paymentType) {
        //            if (paymentType.Id == $scope.payment.paymentTypeId) {
        //                $scope.GrossTmpPaymentType = (
        //                    (parseFloat(paymentType.PaymentCharge) *
        //                     parseFloat($scope.GrossTotalTmp)
        //                    ) + parseFloat($scope.GrossTotalTmp)).toFixed(2);
        //                $scope.pub_PaymentType = paymentType.PaymentType;
        //                $scope.pub_PaymentCharg = (parseFloat(paymentType.PaymentCharge) * parseFloat($scope.GrossTotalTmp)).toFixed(2);
        //                //alert($scope.pub_PaymentCharg);
        //                return false;
        //            }
        //        });
        //    } else {
        //        $scope.GrossTmpPaymentType = parseFloat($scope.GrossTotal).toFixed(2);
        //        // alert($scope.pub_PaymentCharg);
        //    }
        //}
        var serialCheckStatus = false;
        $scope.serialNumberCheck = function () {
            $scope.validate_dealerId = "";
            if (serialCheckStatus == false) {
                serialCheckStatus = true;
                if (isGuid($scope.product.dealerId)) {

                    if ($scope.formAction) {


                        if (isGuid($scope.product.categoryId) && $scope.product.serialNumber != ''
                            && $scope.product.serialNumber.length == parseInt($scope.serialNumberLength)) {
                            swal({ title: 'Processing...!', text: 'Validate VIN/Serial No...', showConfirmButton: false });
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
                                        //  $scope.isProductDetailsReadonly = false;
                                        //  customInfoMessage("Item you enterd is already allocated for another dealer.");
                                        // $scope.resetItemInformationOnly()
                                        return false;
                                    }
                                    else if (data.IsBordxConfirmed) {
                                        $scope.isProductDetailsReadonly = true;
                                        // customInfoMessage("Item you enterd is in a policy which is allocated to a confirmed Bordx.You cannot update this Item's Information");

                                    } else if (data.IsPolicyApproved) {
                                        if (!data.AllowedToApprove) {
                                            $scope.isProductDetailsReadonly = true;
                                            // customInfoMessage('Another approved policy is related to selected Item.Update allowed only for users who has access to Policy Approval');
                                        } else {
                                            //  $scope.isProductDetailsReadonly = false;
                                        }
                                    } else {
                                        // $scope.isProductDetailsReadonly = false;
                                    }

                                    if ($scope.currentCommodityTypeUniqueCode == "E") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadBnW(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "A") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadVehicleFromSerial(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "B") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadVehicleFromSerial(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "O") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadOther(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "Y") {
                                    }
                                } else {
                                    // $scope.isProductDetailsReadonly = false;
                                    $scope.clearSerialNumberInformation();
                                }

                                serialCheckStatus = false;

                            }).error(function (data, status, headers, config) {

                            }).finally(function () {
                                serialCheckStatus = false;
                            });;
                        }

                    } else {

                        if (isGuid($scope.product.categoryId) && $scope.product.serialNumber != '' && $scope.product.serialNumber.length == parseInt($scope.serialNumberLength_temp)) {
                            swal({ title: 'Processing...!', text: 'Validate VIN/Serial No...', showConfirmButton: false });
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
                                        //  $scope.isProductDetailsReadonly = false;
                                        customInfoMessage("Item you enterd is already allocated for another dealer.");
                                        $scope.resetItemInformationOnly();
                                        return false;
                                    }
                                    if (data.IsBordxConfirmed) {
                                        $scope.isProductDetailsReadonly = true;
                                        customInfoMessage("Item you enterd is in a policy which is allocated to a confirmed Bordx.You cannot update this Item's Information");

                                    } else if (data.IsPolicyApproved) {
                                        //var isAuthorizedToEdit = false;
                                        //for (var i = 0; i < data.ApprovedUserIdsList.length; i++) {
                                        //    if (data.ApprovedUserIdsList[i] == $localStorage.LoggedInUserId) {
                                        //        isAuthorizedToEdit = true;
                                        //        return false;
                                        //    }
                                        //}
                                        if (!data.AllowedToApprove) {
                                            $scope.isProductDetailsReadonly = true;
                                            customInfoMessage('Another approved policy is related to selected Item.Update allowed only for users who has access to Policy Approval');
                                        } else {
                                            // $scope.isProductDetailsReadonly = false;
                                        }
                                    } else {
                                        // $scope.isProductDetailsReadonly = false;
                                    }

                                    if ($scope.currentCommodityTypeUniqueCode == "E") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadBnW(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "A") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadVehicleFromSerial(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "B") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadVehicleFromSerial(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "O") {
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetModelesByMakeId' });
                                        }
                                        if (loadingPolicyData) {
                                            policyLoadingRequests.push({ 'reqName': 'GetAllVariantByModelId' });
                                        }
                                        $scope.loadOther(data.ItemId);
                                    } else if ($scope.currentCommodityTypeUniqueCode == "Y") {
                                        // $scope.loadVehicle(data.ItemId);
                                    }
                                } else {
                                    //$scope.isProductDetailsReadonly = false;
                                    $scope.clearSerialNumberInformation();
                                }
                                serialCheckStatus = false;

                            }).error(function (data, status, headers, config) {
                            }).finally(function () {
                                $scope.checkSwalClose('SerialNumberCheck');
                                serialCheckStatus = false;
                            });;
                        }
                    }
                } else {
                    $scope.validate_dealerId = "has-error";
                    customErrorMessage("Please select a Dealer.");
                    serialCheckStatus = false;
                }
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
            $scope.product.commodityUsageTypeId = emptyGuid();
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
            $scope.product.registrationDate = '';
            $scope.product.grossWeight = '';
            $scope.product.MWStartDate = '';
            $scope.product.MWIsAvailable = false;
        }
        //click events
        $scope.customerFormReset = function () {
            $scope.customerDisabled = false;
            $scope.customer = {
                customerId: emptyGuid(),
                customerTypeId: emptyGuid(),
                usageTypeId: emptyGuid(),
                firstName: '',
                lastName: '',
                dateOfBirth: '',
                gender: '',
                idTypeId: emptyGuid(),
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
                businessAddress4: ''
            };
            $scope.customerDocUploader.queue = [];
        }
        $scope.productFormReset = function () {
            $scope.isProductDetailsReadonly = true;

            $scope.product.id = emptyGuid();
            $scope.product.productId = emptyGuid();
            $scope.product.dealerId = emptyGuid();
            $scope.product.dealerLocationId = emptyGuid();
            $scope.product.categoryId = emptyGuid();
            $scope.product.serialNumber = '';
            $scope.product.makeId = emptyGuid();
            $scope.product.modelId = emptyGuid();
            $scope.product.modelYear = '';
            $scope.product.invoiceNo = '';
            $scope.product.additionalSerial = '';
            $scope.product.itemStatusId = emptyGuid();
            $scope.product.commodityUsageTypeId = emptyGuid();
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
            $scope.product.dealerPaymentCurrencyTypeId = emptyGuid();
            $scope.product.customerPaymentCurrencyTypeId = emptyGuid();
            $scope.product.registrationDate = '';
            $scope.product.grossWeight = '';
            $scope.product.MWStartDate = '';
            $scope.product.MWIsAvailable = false;


            $scope.itemDocUploader.queue = [];
        }
        $scope.policyFormReset = function () {
            $scope.policy.policySoldDate = '';
            $scope.hrsUsedAtPolicySale = '';
            $scope.salesPersonId = emptyGuid();
            $scope.dealerPolicy = 'false';
            $scope.policyDocUploader.queue = [];
        }
        $scope.paymentFormReset = function () {
            $scope.payment = {
                refNo: '',
                isSpecialDeal: false,
                discount: 0.00,
                dealerPayment: 0.00,
                isPartialPayment: false,
                paymentModeId: emptyGuid(),
                customerPayment: 0.00,
                comment: ''
            };
            $scope.paymentDocUploader.queue = [];
        }
        $scope.customerSearchGridSearchreset = function () {
            $scope.customerSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                eMail: '',
                mobileNo: '',
                businessName: ''
            }
        }
        $scope.resetAll = function () {
            $scope.customerFormReset();
            $scope.productFormReset();
            $scope.policyFormReset();
            $scope.paymentFormReset();
            $scope.GrossTmpPaymentType = '';
            $scope.GrossTotalTmp = '';
            $scope.GrossPremium = '';

            //$scope.formActionText = "Registering for new Policy";
            $scope.formAction = true;//true for add new
            $scope.selectedCustomerTypeName = '';
            $scope.selectedUsageTypeName = '';
            $scope.serialNumberLength = '';
            $scope.currentCommodityTypeCode = '';
            $scope.discountAvailable = true;
            $scope.isPaymentTypesAvailable = false;
            $scope.GrossTotal = 0.0;
            goToStep(1);
           // $scope.isProductDetailsReadonly = false;
        }
        $scope.clearSerialNumberInformation = function () {
            $scope.product.makeId = emptyGuid();
            $scope.product.modelId = emptyGuid();
            $scope.product.modelYear = '';
            $scope.product.invoiceNo = '';
            $scope.product.additionalSerial = '';
            $scope.product.itemStatusId = '';
          //  $scope.product.commodityUsageTypeId = '';
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
            $scope.product.commodityUsageTypeId = emptyGuid();
            $scope.product.registrationDate = '';
            $scope.product.grossWeight = '';
            $scope.product.MWStartDate = '';
            $scope.product.MWIsAvailable = false;
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
                eMail: '',
                mobileNo: '',
                businessName: ''
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
        var getPolicySearchPage = function () {
            $scope.policySearchGridloading = true;
            $scope.policySearchGridloadAttempted = false;
            var policySearchGridParam =
                {
                    'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                    'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                    'type': 'forpolicytransfer',
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

            policyLoadingRequests = [
                { 'reqName': 'GetPolicyById' },
                { 'reqName': 'GetCustomerById' },
                { 'reqName': 'CheckCustomerById' },
                { 'reqName': 'GetAllChildProducts' },
                { 'reqName': 'GetAllDealerLocationsByDealerId' },
                { 'reqName': 'GetAttachmentsByPolicyId' },
                { 'reqName': 'GetPolicyTransferHistory' },
                { 'reqName': 'GetMakesByCommodityCategoryId' }

            ];

            // alert(policyId);
            if (isGuid(policyId)) {
                if (typeof SearchPolicyPopup != 'undefined')
                    SearchPolicyPopup.close();
                loadingPolicyData = true;
                swal({ title: 'Processing...!', text: 'Loading Policy Data .....', showConfirmButton: false });
                $scope.customerDocUploader.queue = [];
                $scope.itemDocUploader.queue = [];
                $scope.policyDocUploader.queue = [];
                $scope.paymentDocUploader.queue = [];
                $scope.policyHistory = [];
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

                    if (data == "" || data == null) {
                        customErrorMessage('Policy Loading Problem');
                        policyLoadingRequests = [];
                    }
                    $scope.checkSwalClose('GetPolicyById');
                    $scope.ContractProducts_temp = data.ContractProducts

                    $scope.loadCustomer(data.Customer.Id, true);

                    $scope.product.commodityTypeId = data.CommodityTypeId;
                   // $scope.selectedCommodityTypeChanged();

                    $scope.product.productId = data.ProductId;
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
                        policyLoadingRequests.push({ 'reqName': 'GetVehicleDetailsById' });
                        $scope.serialNumberCheck();
                        $scope.product.makeId = data.Vehicle.MakeId;
                        $scope.product.modelId = data.Vehicle.ModelId;
                        $scope.product.engineCapacityId = data.Vehicle.EngineCapacityId;
                        $scope.product.cylinderCountId = data.Vehicle.CylinderCountId;
                        $scope.product.registrationDate = data.Vehicle.RegistrationDate;
                        $scope.product.grossWeight = data.Vehicle.GrossWeight;

                    } else if ($scope.currentCommodityTypeCode == "B") {
                        $scope.product.id = data.Vehicle.Id;
                        $scope.product.categoryId = data.Vehicle.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.Vehicle.VINNo;
                        policyLoadingRequests.push({ 'reqName': 'GetVehicleDetailsById' });
                        $scope.serialNumberCheck();
                        $scope.product.makeId = data.Vehicle.MakeId;
                        $scope.product.modelId = data.Vehicle.ModelId;
                        $scope.product.engineCapacityId = data.Vehicle.EngineCapacityId;
                        $scope.product.cylinderCountId = data.Vehicle.CylinderCountId;
                        $scope.product.registrationDate = data.Vehicle.RegistrationDate;
                        $scope.product.grossWeight = data.Vehicle.GrossWeight;

                    } else {
                        $scope.product.id = data.BAndW.Id;
                        $scope.product.categoryId = data.BAndW.CategoryId;
                        $scope.serialNumber_temp = data.BAndW.CategoryId;
                        $scope.selectedCommodityCategoryChanged();
                        $scope.product.serialNumber = data.BAndW.SerialNo;
                        $scope.product.makeId = data.BAndW.MakeId;
                        $scope.product.modelId = data.BAndW.ModelId;

                    }

                    $scope.product.MWStartDate = data.MWStartDate;
                    $scope.product.MWIsAvailable = data.MWIsAvailable;

                    $scope.policy.tpaBranchId = data.tpaBranchId;

                    $scope.payment.refNo = data.RefNo;
                    $scope.payment.isSpecialDeal = data.IsSpecialDeal;
                    $scope.payment.discount = data.Discount;
                    $scope.payment.dealerPayment = data.DealerPayment;
                    $scope.payment.isPartialPayment = data.IsPartialPayment;
                    $scope.payment.paymentModeId = data.PaymentModeId;
                    $scope.selectedPaymentModeChanged();
                    $scope.payment.paymentTypeId = data.PaymentTypeId;

                    $scope.payment.customerPayment = data.CustomerPayment;
                    $scope.payment.comment = data.Comment;

                    $scope.policy.id = data.Id;
                    $scope.policy.policySoldDate = data.PolicySoldDate;
                    $scope.policySoldDateChanged();
                    $scope.policy.hrsUsedAtPolicySale = data.HrsUsedAtPolicySale;
                    $scope.policy.salesPersonId = data.SalesPersonId;
                    $scope.policy.dealerPolicy = data.DealerPolicy.toString();

                    customInfoMessage("Please search or enter new customer to transfer policy");
                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetAttachmentsByPolicyId',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.attachments_temp = data.Attachments;
                    $scope.attachments_temp_rebind = data.Attachments;
                    $scope.checkSwalClose('GetAttachmentsByPolicyId');
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
                            $scope.paymentDocUploader.queue.push(attachment);
                        }
                    }

                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetAttachmentsByPolicyId');
                });;
                //get policy transfer history

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/GetPolicyTransferHistory',
                    data: { "Id": policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //$scope.policyHistory = data[0];
                   // alert(data.length);
                    $scope.checkSwalClose('GetPolicyTransferHistory');
                    for (var i = 0; i < data.length; i++) {
                        $scope.policyHistory.push(data[i]);
                    }

                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetPolicyTransferHistory');
                });
            }



        }

        $scope.rebindAttachments = function () {
            for (var i = 0; i < $scope.attachments_temp_rebind.length; i++) {
                var file = {
                    name: $scope.attachments_temp_rebind[i].FileName,
                    size: $scope.attachments_temp_rebind[i].AttachmentSizeKB
                }
                var attachment = {
                    documentType: $scope.attachments_temp_rebind[i].DocumentType,
                    id: $scope.attachments_temp_rebind[i].Id,
                    file: file,
                    ref: $scope.attachments_temp_rebind[i].FileServerRef
                }

                if ($scope.attachments_temp_rebind[i].AttachmentSection == "Customer") {
                    $scope.customerDocUploader.queue.push(attachment);
                } else if ($scope.attachments_temp_rebind[i].AttachmentSection == "Item") {
                    $scope.itemDocUploader.queue.push(attachment);
                } else if ($scope.attachments_temp_rebind[i].AttachmentSection == "Policy") {
                    $scope.policyDocUploader.queue.push(attachment);
                } else if ($scope.attachments_temp_rebind[i].AttachmentSection == "Payment") {
                    $scope.paymentDocUploader.queue.push(attachment);
                }
            }

        }

        $scope.checkSwalClose = async function (requestName) {
            if (loadingPolicyData && requestName != null && requestName != undefined && requestName.length > 1) {
                // console.log('response : ' + requestName);
                policyLoadingRequests = policyLoadingRequests.filter(a => a.reqName !== requestName);
               //  console.table(policyLoadingRequests);
                if (policyLoadingRequests.length == 0) {
                    loadingPolicyData = false;
                    swal.close();
                //    console.log('closed swal');
                }
            } else {
                swal.close();
            }
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
                            for (var i = 0; i < $scope.paymentDocUploader.queue.length; i++) {
                                if ($scope.paymentDocUploader.queue[i].id == id) {
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
                swal({ title: 'Processing...!', text: 'Preparing your download...', showConfirmButton: false });
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
                                $scope.productContracts[j].Contracts = $scope.ContractProducts_temp[i].Contracts;
                                $scope.productContracts[j].ExtensionTypeId = $scope.ContractProducts_temp[i].ExtensionTypeId;
                                $scope.SetExtensionTypeValue($scope.ContractProducts_temp[i].ContractExtensionsId);
                                $scope.productContracts[j].AttributeSpecificationId = $scope.ContractProducts_temp[i].AttributeSpecificationId;
                                $scope.SetAttributeSpecificationValue($scope.productContracts[j].Name);
                                $scope.productContracts[j].CoverTypeId = $scope.ContractProducts_temp[i].CoverTypeId;
                                $scope.SetCoverTypeValue($scope.productContracts[j].Name);

                                break inner_loop;
                            }
                        }
                };
        }

        //endranga
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
                    $scope.selectedModelChange();
                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = data.InvoiceNo;
                    $scope.product.additionalSerial = data.AddnSerialNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.ItemPrice;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;
                    $scope.checkSwalClose('GetVehicleDetailsById');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetVehicleDetailsById');
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
              { name: 'Serial No', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss' },
              { name: 'Model', field: 'Model', enableSorting: false, cellClass: 'columCss', },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadOther(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
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
                    $scope.checkSwalClose('GetVehicleDetailsById');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetVehicleDetailsById');
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
              { name: 'Serial No', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss' },
              { name: 'Model', field: 'Model', enableSorting: false, cellClass: 'columCss', },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadYellowGood(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
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
        $scope.refresVehicleSearchGridData = function () {
            getVehicleSearchPage();
        }
        var getVehicleSearchPage = function () {
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
                    //$scope.product.commodityUsageTypeId = emptyGuid();
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.VehiclePrice;
                    $scope.product.variantId = data.Variant;
                    $scope.product.engineCapacityId = data.EngineCapacityId;
                    $scope.product.cylinderCountId = data.CylinderCountId;
                    $scope.product.grossWeight = data.GrossWeight;
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
                    //$scope.selectedCommodityCategoryChanged();
                    $scope.product.serialNumber = data.VINNo;
                    $scope.product.makeId = data.MakeId;
                    $scope.selectedMakeChanged();
                    $scope.product.modelId = data.ModelId;
                    $scope.selectedModelChange();
                    $scope.product.modelYear = data.ModelYear;
                    $scope.product.invoiceNo = '';
                    $scope.product.additionalSerial = data.PlateNo;
                    $scope.product.itemStatusId = data.ItemStatusId;
                  //  $scope.product.commodityUsageTypeId = emptyGuid();
                    $scope.product.itemPurchasedDate = data.ItemPurchasedDate;
                    $scope.product.dealerPrice = data.DealerPrice;
                    $scope.product.itemPrice = data.VehiclePrice;
                    $scope.product.variantId = data.Variant;
                    $scope.product.engineCapacityId = data.EngineCapacityId;
                    $scope.product.cylinderCountId = data.CylinderCountId;
                    $scope.product.fuelTypeId = data.FuelTypeId;
                    $scope.product.transmissionTypeId = data.TransmissionId;
                    $scope.product.bodyTypeId = data.BodyTypeId;
                    $scope.product.aspirationTypeId = data.AspirationId;
                    $scope.product.commodityUsageTypeId = data.CommodityUsageTypeId;
                    $scope.product.grossWeight = data.GrossWeight;
                    $scope.product.registrationDate = data.RegistrationDate;
                    $scope.checkSwalClose('GetVehicleDetailsById');
                }).error(function (data, status, headers, config) {
                    $scope.checkSwalClose('GetVehicleDetailsById');
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
              { name: 'First Name', field: 'FirstName', enableSorting: false, cellClass: 'columCss' },
              { name: 'Last Name', field: 'LastName', enableSorting: false, cellClass: 'columCss', },
              { name: 'Business Name', field: 'BusinessName', enableSorting: false, cellClass: 'columCss', },
              { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Email', field: 'Email', enableSorting: false, cellClass: 'columCss' },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadCustomer(row.entity.Id,false)" class="btn btn-xs btn-warning">Load</button></div>',
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
        $scope.loadCustomer = function (customerId, isAuto) {

            if (isGuid(customerId)) {
                if (typeof SearchCustomerPopup != 'undefined')
                    SearchCustomerPopup.close();
                if (!isAuto)
                    $scope.customerDocUploader.queue = [];


                swal({ title: 'Processing...!', text: 'Validate Customer Information...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/CheckCustomerById',
                    data: { "CustomerId": customerId, 'LoggedInUserId': $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.checkSwalClose('CheckCustomerById');
                    if (data.IsBordxConfirmed) {
                       //$scope.customerDisabled = true;
                      //  customInfoMessage('Another Bordx is confirmed against selected customer.So customer details are Read-Only');
                    } else {
                        if (data.IsPolicyApproved == true && data.HasAccesstoPolicyApproval == false) {
                         //   $scope.customerDisabled = true;
                        //    customInfoMessage('Another approved policy is related to selected customer.Update allowed only for users who has access to Policy Approval');

                        } else {
                            $scope.customerDisabled = false;
                        }

                    }
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/GetCustomerById',
                        data: { "Id": customerId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        $scope.checkSwalClose('GetCustomerById');
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


                        $scope.selectedCustomerTypeIdChanged();
                        $scope.selectedCountryChanged();
                    }).error(function (data, status, headers, config) {
                        // clearCustomerControls();
                        $scope.checkSwalClose('GetCustomerById');
                    });
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    //swal.close();
                    $scope.checkSwalClose('GetCustomerById');
                    $scope.checkSwalClose('CheckCustomerById');
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
            // return true;
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

            if ($scope.selectedCustomerTypeName === "Corporate") {

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

                //if (!validateEmail($scope.customer.email)) {
                //    isValidated = false;
                //    $scope.validate_email = "has-error";
                //} else {
                //    $scope.validate_email = "";
                //}
            }


            //    $scope.customer.customerId = data.Id;
            //$scope.customer.customerTypeId = data.CustomerTypeId;
            //$scope.customer.usageTypeId = data.UsageTypeId;
            return isValidated;
        }
        $scope.validateProductInfo = function () {
            //  return true;

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
                // alert($scope.product.serialNumber);
                if (!$scope.formAction) {
                    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength_temp)) {
                        $scope.validate_serialNumber = "";
                    } else {
                        $scope.validate_serialNumber = "has-error";
                        isValidated = false;
                        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength_temp + " characters.");
                    }
                } else {
                    if ($scope.product.serialNumber.length == parseInt($scope.serialNumberLength)) {
                        $scope.validate_serialNumber = "";
                    } else {
                        $scope.validate_serialNumber = "has-error";
                        isValidated = false;
                        customErrorMessage("Serial Number length must be " + $scope.serialNumberLength + " characters.");
                    }
                }

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
            }


            if ($scope.product.additionalSerial == "") {
                $scope.validate_additionalSerial = "has-error";
                isValidated = false;
            } else {
                $scope.validate_additionalSerial = "";
            }

            if (!isGuid($scope.product.itemStatusId)) {
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
            return isValidated;
        }
        $scope.validatePolicyInfo = function () {
            //    return true;

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

            if ($scope.policy.policySoldDate == '') {
                $scope.validate_policySoldDate = "has-error";
                isValidated = false;
            } else {
                $scope.validate_policySoldDate = "";
            }

            if ($scope.policy.hrsUsedAtPolicySale == '') {
                $scope.validate_hrsUsedAtPolicySale = "has-error";
                isValidated = false;
            } else {
                $scope.validate_hrsUsedAtPolicySale = "";
            }

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
            //  return true;

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
            //if ($scope.payment.refNo == '') {
            //    $scope.validate_refNo = "has-error";
            //    isValidated = false;
            //} else {
            //    $scope.validate_refNo = "";
            //}

            if ($scope.payment.isSpecialDeal) {
                //if ($scope.payment.dealerPayment == "" || !parseFloat($scope.payment.dealerPayment)) {
                //    $scope.validate_dealerPayment = "has-error";
                //    isValidated = false;
                //    $scope.payment.dealerPayment = parseFloat("0.00").toFixed(2);
                //} else {
                //    $scope.validate_dealerPayment = "";

                //}
                if ($scope.payment.comment == "") {
                    $scope.validate_comment = "has-error";
                    isValidated = false;

                } else {
                    $scope.validate_comment = "";
                }
            } else {
                $scope.validate_comment = "";
                if ($scope.payment.dealerPayment == "" || !parseFloat($scope.payment.dealerPayment)) {
                    $scope.payment.dealerPayment = parseFloat("0.00").toFixed(2);
                }
            }
            //if (!isGuid($scope.payment.paymentModeId)) {
            //    $scope.validate_paymentModeId = "has-error";
            //    isValidated = false;
            //} else {
            //    $scope.validate_paymentModeId = "";
            //}

            //if ($scope.payment.isPartialPayment) {
            //    if ($scope.payment.customerPayment == "" || !parseFloat($scope.payment.customerPayment)) {
            //        $scope.validate_customerPayment = "has-error";
            //        $scope.payment.customerPayment = parseFloat("0.00").toFixed(2);
            //        isValidated = false;
            //    } else {
            //        $scope.validate_customerPayment = "";
            //    }
            //} else {
            //    $scope.validate_customerPayment = "";
            //    $scope.payment.customerPayment = parseFloat("0.00").toFixed(2);
            //}
            //if (parseFloat($scope.PremiumAfterPaymentFeesAndDiscounts) != (parseFloat($scope.payment.dealerPayment) + parseFloat($scope.payment.customerPayment))) {
            //    //customErrorMessage("The sum of dealer payment and customer payment should tally with gross premium.");
            //    $scope.validate_customerPayment = "has-error";
            //    $scope.validate_dealerPayment = "has-error";
            //    isValidated = false;
            //} else {
            //    $scope.validate_customerPayment = "";
            //    $scope.validate_dealerPayment = "";
            //}

            return isValidated;
        }
        $scope.validateTpaBranch = function () {
            //if (!isGuid($scope.policy.tpaBranchId)) {
            //    $scope.validate_tpaBranchId = "has-error";
            //    return false;
            //}
            //$scope.validate_tpaBranchId = "";
            return true;
        }

        $scope.validateTransferFee = function () {
            if ($scope.transferFee=="0.00" || parseFloat($scope.transferFee))
            {
                $scope.validate_transferFee = "";
                return true;
            } else {
                $scope.validate_transferFee = "has-error";
                return false;
            }
        }
        //$scope.discountPercentageChanged = function () {
        //    if (parseFloat($scope.payment.discount)) {

        //        $scope.GrossTotalTmp = (parseFloat($scope.GrossTotal) * ((100 - $scope.payment.discount) / 100)).toFixed(2);
        //        $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;

        //    } else {
        //        $scope.GrossTotalTmp = $scope.GrossTotal;
        //        $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
        //        $scope.payment.discount = parseFloat("0.00").toFixed(2);

        //    }
        //}
        $scope.discountPercentageChanged = function () {
            if (parseFloat($scope.payment.discount)) {

                $scope.PremiumAfterPaymentFeesAndDiscounts = (parseFloat($scope.PremiumAfterPaymentFees) *
                    ((100 - $scope.payment.discount) / 100)).toFixed(2);
                //$scope.GrossTmpPaymentType = $scope.GrossTotalTmp;

            } else {
                $scope.PremiumAfterPaymentFeesAndDiscounts = $scope.PremiumAfterPaymentFees;

            }
        }
        $scope.form = {

            next: function (form) {

                $scope.toTheTop();
                //alert(form);
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
                    for (var j = (i - 1) ; j > 0; j--) {
                        if (j == 1) {
                            if (!$scope.validateCustomerInfo()) {
                                isValidated = false;
                                customErrorMessage("Please fill all mandatory fields before proceeding");
                                return false;
                            }
                        } else if (j == 2) {
                            //if (!$scope.validateProductInfo()) {
                            //    isValidated = false;
                            //    customErrorMessage("Please fill all mandatory fields before proceeding");
                            //    return false;
                            //}
                            //else {
                            //    if ($scope.formAction && parseInt($scope.GrossTotal) == 0) {

                            //        $scope.policy.policySoldDate = GetToday();
                            //        $scope.policySoldDateChanged();
                            //    }

                            //}
                        } else if (j == 3) {
                            //if (!$scope.validatePolicyInfo()) {
                            //    isValidated = false;
                            //    customErrorMessage("Please fill all mandatory fields before proceeding");
                            //    return false;
                            //}
                        } else if (j == 4) {

                            //if (!$scope.validatePaymentInfo()) {
                            //    isValidated = false;
                            //    customErrorMessage("Please fill all mandatory fields before proceeding");
                            //    return false;
                            //} else {
                                if ($scope.validateTpaBranch()) {
                                    if($scope.validateTransferFee())
                                    {
                                        $scope.uploadAttachments();
                                    } else {
                                        customErrorMessage("Please enter valid Transfer Fee");
                                        isValidated = false;
                                        return;
                                    }

                                } else {
                                    customErrorMessage("Please select TPA Branch before proceeding");
                                    isValidated = false;
                                    return;
                                }
                            //}
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
            toaster.pop('error', 'Error', 'please complete the form in this step beforee proceeding');
        };
        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };
        $scope.viewPolicySummary = function () {
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
            if (isGuid($scope.policy.id)) {
                if ($scope.validateCustomerInfo()) {
                    if (!$scope.formAction) {
                        for (var i = 0; i < $scope.attachments_temp.length; i++) {
                            var isRemoved = true;
                            if ($scope.attachments_temp[i].AttachmentSection == "Customer") {
                                for (var j = 0; j < $scope.customerDocUploader.queue.length; j++) {
                                    //alert($scope.customerDocUploader.queue[j].ref);
                                    if (typeof $scope.customerDocUploader.queue[j].ref === 'undefined') {
                                        //new records
                                    } else {
                                        if ($scope.attachments_temp[i].FileServerRef == $scope.customerDocUploader.queue[j].ref) {
                                            isRemoved = false;
                                            //return false;
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


                        $scope.savePolicy();
                    }
                } else {
                    customErrorMessage("Please fill all the highlighted fields.");
                }
            } else {
                customErrorMessage("Please select a policy.");
            }
        }
        $scope.eligibilityCheck = function (contractId) {
            if ($scope.policy.hrsUsedAtPolicySale == '' && !parseFloat($scope.policy.hrsUsedAtPolicySale)) {
                $scope.policy.hrsUsedAtPolicySale = 0;
            } else {
                $scope.policy.hrsUsedAtPolicySale = parseFloat($scope.policy.hrsUsedAtPolicySale);
            }

            var usedUnits = $scope.policy.hrsUsedAtPolicySale;
            var itemPurchesedDate = $scope.product.registrationDate;
            if (isGuid(contractId)) {
                var eligibilityCheckRequest = {
                    itemPurchesedDate: itemPurchesedDate,
                    usedAmount: usedUnits,
                    contractId: contractId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/EligibilityCheckRequest',
                    data: { "eligibilityCheckRequest": eligibilityCheckRequest },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (parseFloat(data.premium) && parseFloat(data.premium) > 0) {
                        angular.forEach($scope.productContracts, function (value) {
                            if (contractId == value.ContractId) {
                                if (data.isPercentage) {
                                    var tempPremium = value.Premium;
                                    value.Premium += tempPremium * (data.premium / 100);
                                    $scope.pub_EligibilityValue = tempPremium * (data.premium / 100);
                                    $scope.GrossTotal += tempPremium * (data.premium / 100);
                                    $scope.GrossTotal = $scope.GrossTotal.toFixed(2)
                                    $scope.GrossTotalTmp = $scope.GrossTotal;
                                    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                                } else {
                                    value.Premium += data.premium;
                                    $scope.pub_EligibilityValue = data.premium;
                                    $scope.GrossTotal = $scope.GrossTotal.toFixed(2)
                                    $scope.GrossTotalTmp = $scope.GrossTotal;
                                    $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                                }
                                return false;
                            }
                        });
                    }

                }).error(function (data, status, headers, config) {
                });
            } else {
                angular.forEach($scope.productContracts, function (value) {
                    var eligibilityCheckRequest = {
                        itemPurchesedDate: itemPurchesedDate,
                        usedAmount: usedUnits,
                        contractId: value.ContractId
                    };
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/PolicyReg/EligibilityCheckRequest',
                        data: { "eligibilityCheckRequest": eligibilityCheckRequest },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (parseFloat(data.premium) && parseFloat(data.premium) > 0) {
                            if (data.isPercentage) {
                                var tempPremium = value.Premium;
                                value.Premium += tempPremium * (data.premium / 100);
                                $scope.GrossTotal += tempPremium * (data.premium / 100);
                                $scope.GrossTotalTmp = $scope.GrossTotal;
                                $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                            } else {
                                value.Premium += data.premium;
                                $scope.GrossTotal += data.premium;
                                $scope.GrossTotalTmp = $scope.GrossTotal;
                                $scope.GrossTmpPaymentType = $scope.GrossTotalTmp;
                            }
                        }

                    }).error(function (data, status, headers, config) {
                    });

                });
            }

        }
        $scope.savePolicy = function () {

            if ($scope.selectedCustomerTypeName == 'Corporate') {
                $scope.customer.gender = "M";
                $scope.customer.idIssueDate = "1/1/1753";
                $scope.customer.dateOfBirth = "1/1/1753";
                $scope.customer.idTypeId = 1;
                $scope.customer.nationalityId = 0;
            }

            $scope.policySaveStatusTitle = "Processing...!";
            $scope.policySaveStatusMsg = "Policy transfer is being processed.";
            swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: false });
            $scope.policy.productContracts = $scope.productContracts;
            $scope.policy.premium = $scope.GrossTotalTmp;

            var policyDetails = {
                customer: $scope.customer,
                product: $scope.product,
                policy: $scope.policy,
                payment: $scope.payment,
                policyDocIds: $scope.uploadedDocIds,
                requestedUser: $localStorage.LoggedInUserId,
                transferFee: $scope.transferFee
            };
            if (!$scope.formAction) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/PolicyReg/TransferPolicy',
                    data: { "policyDetails": policyDetails },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    if (data == 'success') {
                        $scope.policySaveStatusTitle = "Success!";
                        $scope.policySaveStatusMsg = "Policy transfered successfully.";
                        $scope.policySaveStatusConfirmButtons = true;
                        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        $scope.resetAll();


                    } else {
                        $scope.policySaveStatusTitle = "Error!";
                        $scope.policySaveStatusMsg = data;
                        $scope.rebindAttachments();
                        swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                    }
                    goToStep(1);

                }).error(function (data, status, headers, config) {
                    swal.close();
                });
            } else {

                customErrorMessage("Policy is not selected for transfer.");
            }
        }


        $scope.pub_EligibilityValue = 0.00;
        $scope.pub_PaymentCharg = 0.00;
        $scope.viewPremiumBreakdown = function () {

            var premiumBreakdownRequest =
                {
                    productContracts: $scope.productContracts,
                    purchaseDate: $scope.product.itemPurchasedDate,
                    hrsUsed: $scope.product.hrsUsedAtPolicySale,
                    commodityTypeId: $scope.product.commodityTypeId,
                    variantId: $scope.product.variantId

                };
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetPremiumBreakdown',
                data: { "premiumBreakdownRequest": premiumBreakdownRequest },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
               // alert(data.Premium);

                data.BreakdownDetails.push(
                    {
                        "Name": "Eligibility",
                        "Breakdowns": [{ "ItemName": "Value", "Value": $scope.pub_EligibilityValue.toFixed(2), "isBasedonRP": false }
                        ]
                    },
                    {
                        "Name": "Payment Fees",
                        "Breakdowns": [{ "ItemName": $scope.pub_PaymentType, "Value": $scope.pub_PaymentCharg, "isBasedonRP": false }

                        ]
                    }
                );
                if ($scope.premiumBasedonGross) {

                    data.Premium = (parseFloat($scope.product.dealerPrice) * (parseFloat(data.Premium) / 100)).toFixed(2);

                } else {

                }
              //  alert(data.Premium);

                $scope.PolicyBreakdown = data;
                // $scope.PolicyBreakdown.BreakdownDetails = data.BreakdownDetails;


            }).error(function (data, status, headers, config) {
            });
            PremiumBreakdownPopup = ngDialog.open({
                template: 'popUpPremiumBreakdownView',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

            return true;
        }
    });






