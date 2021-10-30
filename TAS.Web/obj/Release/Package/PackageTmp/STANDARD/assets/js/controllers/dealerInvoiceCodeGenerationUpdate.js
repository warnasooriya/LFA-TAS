app.controller('DealerInvoiceCodeGenerationUpdateCtrl',
    function ($scope, $rootScope, $compile, $http, $parse, ngDialog, SweetAlert, $localStorage, toaster, $sce,
        $cookieStore, $filter, toaster, $state, $window, FileUploader, $timeout ,$parse) {

        var w = angular.element($window);
        $scope.windowHeight = w.height();
        $scope.windowWidth = w.width();
        $scope.appendToBody = false;
        $scope.tpaId = $localStorage.tpaID;

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };
        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        var validateEmail = function (email) {
            if (email === "") return false;
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }
        //end of supportive functions
        var CodesearchPopup;
        $scope.isFrontTireDetailsVisible = false;
        $scope.isBackTireDetailsVisible = false;
        $scope.isDownTireDetailsVisible = false;
        //$scope.disabledFrontTireLB = true;
        //$scope.disabledFrontTireRF = true;
        //$scope.disabledFrontTireRB = true;
        //$scope.disabledFrontTireLF = true;

        // read only tyre sizes after load article number and retrive data

        $scope.frontLeftTyreSizeReadOnly=false;
        $scope.rearLeftTyreSizeReadOnly = false;
        $scope.MakeList = [];
        $scope.SelectedMakeList = [];
        $scope.SelectedMakeDList = [];

        $scope.ModelList = [];
        $scope.SelectedModelList = [];
        $scope.SelectedModelDList = [];

        $scope.ModelYearList = [];
        $scope.SelectedModelYearList = [];
        $scope.SelectedModelYearDList = [];
        $scope.Cities = [];

        $scope.additionalPolicyDetailsMake = [];
        $scope.additionalPolicyDetailsModel = [];
        $scope.additionalPolicyDetailsModelYearList = [];
        $scope.additionalSelectedMakeList = [];
        $scope.additionalSelectedModelList = [];
        $scope.filterdSelectedModelList = [];
        $scope.additionalSelectedModelYearList = [];

        $scope.contractDetails = [];
        $scope.isTowContract = false;
        $scope.contractOne = "Contract";
        $scope.contractTow = "Contract Two";
        $scope.contractOneData = [];
        $scope.contractTwoData = [];
        $scope.contractOneModal = "";
        $scope.premiumOneModal = "";

        $scope.additionalPolicyDetailsModelYearList = [];
        $scope.dealerBranches=[];
        $scope.MakeId = emptyGuid();
        $scope.ModelId = emptyGuid();
        $scope.YearId = "";
        $scope.purchaseDate = $filter('date')(new Date(), 'dd-MMM-yyyy');
        $scope.tempInvId = emptyGuid();
        $scope.invoiceUploadProblem = false;
        $scope.dataLoadMode = false;
        $scope.dealerInvoiceDetails = {
            dealerId: emptyGuid(),
            dealerBranchId: emptyGuid(),
            countryId: emptyGuid(),
            cityId: emptyGuid(),
            quantity: '',
            plateNumber: ''
        };
        $scope.gridDealerInvoiceCodeLoading = false;
        $scope.gridDealerInvoiceCodeloadAttempted = false;
        var tyreDetailsArray = [];
        $scope.tyreDetailsValuesForSave = [];
        $scope.initiateUploader = function () {
            //initialize uploaders
            $scope.customerInvoiceUploader = new FileUploader({
                url: '/TAS.Web/api/Upload/UploadAttachment',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'TyreSales', 'Section': 'Customer' },
            });

            $scope.customerInvoiceUploader.onProgressAll = function () {
                swal({ title: 'Uploading invoice...', text: '', showConfirmButton: false });
            }


            $scope.customerInvoiceUploader.onSuccessItem = function (item, response, status, headers) {
                if (response != 'Failed') {
                   // swal.close();
                    $scope.customer.invoiceAttachmentId = response.replace(/['"]+/g, '');
                   // console.log("attachement ID ", $scope.customer.invoiceAttachmentId);


                } else {
                    customErrorMessage("Error occured while uploading attachments.");
                    $scope.customerInvoiceUploader.cancelAll();
                    swal.close();
                }
            }

            $scope.customerInvoiceUploader.onCompleteAll = function () {
                $scope.customerInvoiceUploader.queue = [];
                $scope.generateInvoiceCode();
            }
            $scope.customerInvoiceUploader.filters.push({
                name: 'customFilter',
                fn: function (item, options) {
                    if (item.size > 5000000) {
                        customErrorMessage('Max document size is 5MB');
                        return false;
                    } else {
                        return true;
                    }
                }
            });

        }
        $scope.maxdate = new Date();
        $scope.initiateUploader();

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
                price:0.00
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
                price: 0.00
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
                price: 0.00

            }
        };
        $scope.dealerInvoiceCodeSearchParam = {
            date: '',
            plateNo: '',
            code: ''
        }

        $scope.loggedInCustomer = {
            picture: '',
            name: '',
            id: emptyGuid()
        }

        $scope.product = {
            invoiceCode: '',
            currencyCode: '',
            invoiceNo: '',
            makeId: '',
            modelId: '',
            commodityUsageType: 'Private',
            plateNo: '',
            addMakeId: $scope.additionalSelectedMakeList.id,
            addModelId: $scope.additionalSelectedModelList.id,
            addModelYear: $scope.additionalSelectedModelYearList.id,
            addMileage: '',
            invoiceAttachmentId: emptyGuid(),
            CommodityTypeId:''
        };

        $scope.customer = {
            customerTypeId: emptyGuid(),
            firstName: '',
            lastName: '',
            businessName: '',
            businessTelNo:'',
            usageTypeId: 0,
            mobileNo: '+971',
            email: '',
            invoiceNo: '',
            invoiceAttachmentId:'',
            commodityUsageType: 'Private',
            plateNumber: '',
            addMakeId: $scope.additionalSelectedMakeList.id,
            addModelId: $scope.additionalSelectedModelList.id,
            addModelYear: $scope.additionalSelectedModelYearList.id,
            addMileage: '',

        };

        $scope.resetAll = function () {

            $scope.isFrontTireDetailsVisibleL = false;
            $scope.isFrontTireDetailsVisibleR = false;
            $scope.isBackTireDetailsVisibleL = false;
            $scope.isBackTireDetailsVisibleR = false;
            $scope.isDownTireDetailsVisible = false;
            $scope.dealerInvoiceDetails.dealerId = emptyGuid();
            $scope.dealerInvoiceDetails.dealerBranchId = emptyGuid();
            $scope.dealerInvoiceDetails.countryId = emptyGuid();
            $scope.dealerInvoiceDetails.cityId = emptyGuid();
            $scope.dealerInvoiceDetails.quantity = '';
            $scope.dealerInvoiceDetails.plateNumber = '';
            $scope.dealerInvoiceTireDetails.front.serialLeft = '';
            $scope.dealerInvoiceTireDetails.front.width = '';
            $scope.dealerInvoiceTireDetails.front.cross = '';
            $scope.dealerInvoiceTireDetails.front.diameter = '';
            $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
            $scope.dealerInvoiceTireDetails.front.serialRight = '';
            $scope.dealerInvoiceTireDetails.front.pattern = '';
            $scope.dealerInvoiceTireDetails.back.width = '';
            $scope.dealerInvoiceTireDetails.back.cross = '';
            $scope.dealerInvoiceTireDetails.back.diameter = '';
            $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
            $scope.dealerInvoiceTireDetails.back.serialLeft = '';
            $scope.dealerInvoiceTireDetails.back.serialRight = '';
            $scope.dealerInvoiceTireDetails.back.pattern = '';
            $scope.dealerInvoiceTireDetails.front.dotLeft = '';
            $scope.dealerInvoiceTireDetails.back.dotLeft = '';
            $scope.dealerInvoiceTireDetails.front.dotRight = '';
            $scope.dealerInvoiceTireDetails.back.dotRight = '';
            $scope.dealerInvoiceTireDetails.front.price = 0.00;
            $scope.dealerInvoiceTireDetails.back.price = 0.00;

            $scope.product.invoiceCode = '';
            $scope.product.currencyCode = '';
            $scope.product.invoiceNo = '';
            $scope.product.makeId = '';
            $scope.product.modelId = '';
            $scope.MakeId = emptyGuid();
            $scope.product.commodityUsageType = 'Private';
            $scope.product.plateNo = '';
            $scope.product.addMakeId = $scope.additionalSelectedMakeList.id;
            $scope.product.addModelId = $scope.additionalSelectedModelList.id;
            $scope.product.addModelYear = $scope.additionalSelectedModelYearList.id;
            $scope.product.addMileage = '';
            $scope.product.invoiceAttachmentId = emptyGuid();
            $scope.product.CommodityTypeId = '',
                $scope.customer.customerTypeId = emptyGuid(),
                $scope.customer.firstName = '',
                $scope.customer.lastName = '',
                $scope.customer.businessName = '',
                $scope.customer.businessTelNo = '',
                $scope.customer.usageTypeId = 0,
                $scope.customer.mobileNo = '+971',
                $scope.customer.email = '',
                $scope.customer.invoiceNo = '',
                $scope.customer.invoiceAttachmentId='',
                $scope.customer.makeId = '',
                $scope.customer.modelId = '',
                $scope.customer.commodityUsageType = 'Private',
                $scope.customer.plateNumber = '',
                $scope.customer.addMakeId = $scope.additionalSelectedMakeList.id,
                $scope.customer.addModelId = $scope.additionalSelectedModelList.id,
                $scope.customer.addModelYear = $scope.additionalSelectedModelYearList.id,
                $scope.customer.addMileage = '',
                $scope.additionalPolicyDetailsMake = [];
            $scope.additionalPolicyDetailsModel = [];
            $scope.additionalPolicyDetailsModelYearList = [];
            $scope.additionalSelectedMakeList = [];
            $scope.additionalSelectedModelList = [];
            $scope.filterdSelectedModelList = [];
            $scope.additionalSelectedModelYearList = [];
            $scope.initializeDealerInvoiceCodePage();
            $scope.customerInvoiceUploader.cancelAll();
            angular.element("input[type='file']").val('');
            $scope.tyreDetailsValuesForSave = [];
            tyreDetailsArray = [];
            $scope.selectedvalue = '0';
            $scope.customerInvoiceUploader.queue = [];
            $scope.customer.invoiceAttachmentId = "";
            $scope.customer.PlateRelatedCityId = '';
            $scope.purchaseDate = "";
            $scope.expiryDate = "";
            $scope.premiumOneModal = "";
            $scope.expiryDateOne = "";
            $scope.premiumTwoModal = "";
            $scope.expiryDateOne = "";
            $scope.expiryDateTwo = "";
            $scope.ContractExtensionsIdOne = "";
            $scope.ContractExtensionsIdTwo = "";
            goToStep(1);

        }

        $scope.resetCustomerInformation = function(){
            $scope.customer.customerTypeId = emptyGuid();
            $scope.dealerInvoiceDetails.dealerBranchId = emptyGuid();
            $scope.customer.firstName = '';
            $scope.customer.lastName = '';
            $scope.customer.businessName = '';
            $scope.customer.businessTelNo = '';
            $scope.customer.usageTypeId = 0;
            $scope.customer.mobileNo = '+971';
            $scope.customer.email = '';
            $scope.customer.invoiceNo = '';
            $scope.MakeId = emptyGuid();
            $scope.ModelId = emptyGuid();
            $scope.customer.commodityUsageType = 'Private';
            $scope.customer.plateNumber = '';
            $scope.customer.PlateRelatedCityId = '';
            $scope.customer.addMileage = '';
            $scope.customer.invoiceAttachmentId = '',
            $scope.YearId = 0;
            $scope.customer.businessName = '',
            $scope.customer.businessTelNo= '',
            $scope.customerInvoiceUploader.cancelAll();
            angular.element("input[type='file']").val('');
            $scope.customerInvoiceUploader.queue = [];
            $scope.purchaseDate = "";
            $scope.expiryDate = "";
            $scope.premiumOneModal = "";
            $scope.expiryDateOne = "";
            $scope.premiumTwoModal = "";
            $scope.expiryDateOne = "";
            $scope.expiryDateTwo = "";
            $scope.ContractExtensionsIdOne = "";
            $scope.ContractExtensionsIdTwo = "";

        }

        $scope.selectAllContent = function ($event) {
            $event.target.select();
        };

        $scope.initializeDealerInvoiceCodePage = function () {

            $scope.loadDealerBranches();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllUsageTypes',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                if ($localStorage.CommodityType === "Tyre") {
                    var usageTypes = [];
                    angular.forEach(data, function (value) {
                        if (value.UsageTypeName === 'Private') {
                            usageTypes.push(value);
                            $scope.customer.usageTypeId = value.Id;
                        }
                    });
                    $scope.usageTypes = usageTypes;
                } else {
                    $scope.usageTypes = data;
                }



            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCustomerTypes',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                if ($localStorage.CommodityType === "Tyre") {
                    var cusTypes = [];
                    angular.forEach(data, function (value) {
                        if (value.CustomerTypeName === 'Individual') {

                            cusTypes.push(value);
                        }
                    });
                    $scope.customerTypes = cusTypes;
                } else {
                    $scope.customerTypes = data;
                }

                angular.forEach($scope.customerTypes, function (value) {
                    if (value.CustomerTypeName === 'Individual') {
                        $scope.customer.customerTypeId = value.Id;
                        $scope.selectedCustomerTypeIdChanged();
                        return false;
                    }
                });
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
                if (data.length == 1) {
                    $scope.product.CommodityTypeId = data[0].CommodityTypeId;
                } else {
                    $scope.product.CommodityTypeId = data[2].CommodityTypeId;
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetAllMakesByComodityTypeId',
                    data: { "Id": $scope.product.CommodityTypeId, "tpaId": $localStorage.tpaID == undefined ? tpaId : $localStorage.tpaID }
                }).success(function (data, status, headers, config) {
                    $scope.Makes = data.Makes;
                    AddMake();
                }).error(function (data, status, headers, config) {
                });
                AddModelYear();

                //$scope.selectedCommodityTypeChanged(true);
            }).error(function (data, status, headers, config) {
            });



            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllAdditionalFieldDetails',
                data: {
                    "productCode": "tyre",
                    "tpaId": $scope.tpaId
                }
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    var data_j = JSON.parse(data);
                    console.log(data_j);
                    // make
                    for (var i = 0; i < data_j[0].Value.length; i++) {
                        var make_d = {
                            id: data_j[0].Value[i].Id,
                            label: data_j[0].Value[i].MakeName
                        }
                        $scope.additionalPolicyDetailsMake.push(make_d);
                    }

                    // model
                    for (var i = 0; i < data_j[1].Value.length; i++) {
                        var model_d = {
                            id: data_j[1].Value[i].Id,
                            label: data_j[1].Value[i].ModelName,
                            makeId: data_j[1].Value[i].MakeId
                        }
                        $scope.additionalPolicyDetailsModel.push(model_d);
                    }

                    // model year
                    for (var i = 0; i < data_j[2].Value.length; i++) {
                        var model_year = {
                            id: data_j[2].Value[i].Text,
                            label: data_j[2].Value[i].Value
                        }
                        $scope.additionalPolicyDetailsModelYearList.push(model_year);
                    }
                }
            }).error(function (data, status, headers, config) {
            }).error(function (data, status, headers, config) {
            });

            if (isGuid($localStorage.LoggedInUserId)) {
                swal({ title: 'Authenticating...', text: 'Validating user information', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/UserValidationDealerInvoiceCode',
                    data: { "loggedInUserId": $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'SUCCESS') {
                        swal.close();
                        $scope.dealerInvoiceDetails.dealerId = data.obj.dealerId;
                        $scope.dealerInvoiceDetails.dealerBranchId = data.obj.dealerBranchId;
                        $scope.dealerInvoiceDetails.countryId = data.obj.countryId;
                        $scope.dealerInvoiceDetails.cityId = data.obj.cityId;
                        $scope.loadAvailableTireDetails();
                        $scope.loadCities(data.obj.countryId);
                    } else {
                        swal({ title: 'TAS Security Information', text: data.msg, showConfirmButton: false });
                        setTimeout(function () {
                            swal.close();
                            $state.go('login.signin', { "tpaId": $localStorage.tpaName });
                        }, 8000);
                        // $state.go('app.dashboard');


                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            } else {

                swal({ title: 'TAS Security Information', text: 'Please sign in as a dealer to access this page.', showConfirmButton: false });
                setTimeout(function () {
                    swal.close();
                    $state.go('login.signin', { "tpaId": $localStorage.tpaName });
                }, 8000);
                //$state.go('app.dashboard');

            }


        }
        $scope.availableCrossList = [];
        $scope.availableWidthList = [];
        $scope.availableDiameteList = [];
        $scope.availableLoadSpeedList = [];
        $scope.availablePatternList = [];
        $scope.availableCrossListBack = [];
        $scope.availableWidthListBack = [];
        $scope.availableDiameteListBack = [];
        $scope.availableLoadSpeedListBack = [];
        $scope.availablePatternListBack = [];

        $scope.loadAvailableTireDetails = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.availableWidthList = data.WidthList;
                $scope.availableCrossList = data.CrossSectionList;
                $scope.availableDiameteList = data.DiameterList;
                $scope.availableLoadSpeedList = data.LoadSpeedList;
                $scope.availablePatternList = data.PatternList;
                $scope.availableWidthListBack = data.WidthList;
                $scope.availableCrossListBack = data.CrossSectionList;
                $scope.availableDiameteListBack = data.DiameterList;
                $scope.availableLoadSpeedListBack = data.LoadSpeedList;
                $scope.availablePatternListBack = data.PatternList;
            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });
        }
        $scope.mask = "C99999990000 ?9?9?9?9";
        $scope.selectTyreQuantity = function (qty) {
            $scope.isBackTireDetailsVisibleR = false;
            $scope.isBackTireDetailsVisibleL = false;
            $scope.isFrontTireDetailsVisibleR = false;
            $scope.isFrontTireDetailsVisibleL = false;
            $scope.isDownTireDetailsVisible = false;
            $scope.resetInvoiceTireDetails(true);
            $scope.dealerInvoiceDetails.quantity = parseInt(qty);
            //if ($scope.dealerInvoiceDetails.plateNumber !== '') {
            //    nextStep();
            //} else {
            //    customErrorMessage("Please enter a vehicle plate number.");
            //}
            nextStep();
        }
        $scope.isFrontTireDetailsVisibleL = false;
        $scope.isFrontTireDetailsVisibleR = false;
        $scope.dealerInvoiceTireDetails.back.leftcheckbox = 0;
        $scope.dealerInvoiceTireDetails.front.rightcheckbox = 0;
        $scope.dealerInvoiceTireDetails.back.rightcheckbox = 0;
        $scope.dealerInvoiceTireDetails.front.leftcheckbox = 0;
        $scope.spareWheelcheckbox = 0;

        $scope.selectedvalue = '0';
        $scope.namesofTypes = [];
        $scope.alltyresareSame = true;


        $scope.selectAlltyre = function () {
            //console.log('selected tyre position types',$scope.selectedvalue);
            if ($scope.selectedvalue == '1') {
                $scope.alltyresareSame = true;
            } else if ($scope.selectedvalue == '2') {
                $scope.alltyresareSame = false;
            } else {
                $scope.alltyresareSame = true;
            }
            $scope.tyreDisplayViewDropdown();

        }

        // load cities to customer information

        $scope.loadCities = function (countryId) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                    data: { "countryId": countryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Cities = data;

                }).error(function (data, status, headers, config) {
                    $scope.Cities = null;
                });
        }

        // load dealer locations

        $scope.loadDealerBranches = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByUserId',
                data: { "userId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealerBranches = data;

            }).error(function (data, status, headers, config) {
                $scope.Cities = null;
            });
        }




        // combination sequence SW,FL,FR,BL,BR
        $scope.combinationSourceData = [
            { "combination": "10000", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 1, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11000", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10100", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10010", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10001", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11100", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11010", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11001", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10111", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 4, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10110", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11011", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 4, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11101", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 4, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10101", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "10011", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11110", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 4, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "11111", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 5, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "01000", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 1, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "00100", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 1, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "00010", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 1, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "00001", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 1, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "01100", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "00011", "result": ['isFrontTireDetailsVisibleL'], "optionValues": [1], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }] },
            { "combination": "01010", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }] },
            { "combination": "01001", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "00110", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "00101", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 2, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "01110", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "01011", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "00111", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "01101", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 3, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },
            { "combination": "01111", "result": ['isFrontTireDetailsVisibleL', 'isBackTireDetailsVisibleL'], "optionValues": [1, 2], "tyreQuantity": 4, "namesofTypes": [{ model: 'Please select', color: "0" },{ model: 'All tyres are same size', color: "1" }, { model: 'Front and rear tyres are different', color: '2' }]  },


        ];
        $scope.selectedvalue = '0';
        $scope.tyreDisplayView = function (position, modalName) {
            let combinationString = $scope.spareWheelcheckbox + "" + $scope.dealerInvoiceTireDetails.front.leftcheckbox + "" +
                $scope.dealerInvoiceTireDetails.front.rightcheckbox + "" + $scope.dealerInvoiceTireDetails.back.leftcheckbox + "" +
                $scope.dealerInvoiceTireDetails.back.rightcheckbox;
            //console.log("combination String value", combinationString);
            //check this string value available our combinationSourceData
            var oldSelectedValue = $scope.selectedvalue;

            let selectedCombinationObj = $scope.combinationSourceData.filter(com => com.combination === combinationString);
            //console.log("filtered Object");
            //console.table(selectedCombinationObj);
                    if (selectedCombinationObj.length === 0) {
                        $scope.isBackTireDetailsVisibleL = false;
                        $scope.isFrontTireDetailsVisibleL = false;
                        $scope.namesofTypes = [];
                        $scope.dealerInvoiceDetails.quantity = '';
                        $scope.frontTyreDetailsReset();
                        $scope.backTyreDetailsReset();
                    }
                    else
                    {
                        $scope.dealerInvoiceDetails.quantity = selectedCombinationObj[0].tyreQuantity;
                        $scope.selectedvalue = '0';
                        $scope.namesofTypes = [];
                        $scope.namesofTypes = selectedCombinationObj[0].namesofTypes;

                        angular.forEach(selectedCombinationObj[0].result, function (value) {
                            //  console.log('visible Boxes ', value);
                            var model = $parse(value);
                            model.assign($scope, 1);
                        });
                        //if ($scope.selectedvalue < 2) {
                        //    $scope.isBackTireDetailsVisibleL = false;
                        //}
                      //  $scope.frontTyreDetailsReset();
                      //  $scope.backTyreDetailsReset();
                        $scope.selectedvalue = $scope.selectedvalue;
            }
            console.log('old selected value ', oldSelectedValue);
            console.log('new selected value ', $scope.selectedvalue);
            $scope.ClearTyreDetails(oldSelectedValue);
                $scope.setupTyreDetailsObjectArray(position, modalName);
              }


        $scope.ClearTyreDetails = function (oldSelectedValue) {

            let selVal = $scope.namesofTypes.filter(a => a.color == oldSelectedValue);
            if (selVal.length > 0) {
                $scope.selectedvalue = oldSelectedValue;
            }
            if ($scope.selectedvalue < 2) {
                $scope.isBackTireDetailsVisibleL = false;
            }
            if ($scope.isBackTireDetailsVisibleL == false) {
                $scope.backTyreDetailsReset();
            }
            if ($scope.isFrontTireDetailsVisibleL== false) {
                $scope.frontTyreDetailsReset();
            }





        }

        // dropdown selection function
        $scope.selectedValueCombination = function (selectedCombination) {
            if ($scope.selectedvalue == "0" || $scope.selectedvalue == "1") {

                $scope.isBackTireDetailsVisibleL = false;
                //$scope.frontTyreDetailsReset();
                //$scope.backTyreDetailsReset();

            } else {
                angular.forEach(selectedCombination[1].result, function (value) {
                   // console.log('visible Boxes ', value);
                    var model = $parse(value);
                    model.assign($scope, 1);
                });
            }
        }

        $scope.setupTyreDetailsObjectArray = function (position, modalName) {
            // creating array what selected position and when submitting itterate this array & collect relevent data
            if ($scope.$eval(modalName) == 1) {
                tyreDetailsArray.push({ "position": position });
            } else {
                    tyreDetailsArray = tyreDetailsArray.filter(com => com.position !== position);
            }
        }

        $scope.generateTyreDetails = function () {
            $scope.tyreDetailsValuesForSave = [];
            var tyreDetailsA = [];
            if ($scope.selectedvalue < 2) {
                var priceF = $scope.dealerInvoiceTireDetails.front.price;
                tyreDetailsA.push({ "TireQuantity": $scope.dealerInvoiceDetails.quantity, "Position": "F", "price": priceF });
            }
            if ($scope.selectedvalue == 2) {
                    var frontQty = 0;
                    var backQty = 0;

                if ($scope.dealerInvoiceTireDetails.front.diameter == $scope.dealerInvoiceTireDetails.back.diameter) {
                    var priceF = $scope.dealerInvoiceTireDetails.front.price;
                    if ($scope.dealerInvoiceTireDetails.front.leftcheckbox) {
                        frontQty++;
                    }
                    if ($scope.dealerInvoiceTireDetails.front.rightcheckbox) {
                        frontQty++;
                    }

                    if ($scope.dealerInvoiceTireDetails.back.leftcheckbox) {
                        frontQty++;
                    }
                    if ($scope.dealerInvoiceTireDetails.back.rightcheckbox) {
                        frontQty++;
                    }

                    tyreDetailsA.push({ "TireQuantity": frontQty, "Position": "F", "price": priceF });

                } else {

                    if ($scope.dealerInvoiceTireDetails.front.leftcheckbox) {
                        frontQty++;
                    }
                    if ($scope.dealerInvoiceTireDetails.front.rightcheckbox) {
                        frontQty++;
                    }

                    if ($scope.dealerInvoiceTireDetails.back.leftcheckbox) {
                        backQty++;
                    }
                    if ($scope.dealerInvoiceTireDetails.back.rightcheckbox) {
                        backQty++;
                    }

                    var priceF = $scope.dealerInvoiceTireDetails.front.price;
                    var priceB = $scope.dealerInvoiceTireDetails.back.price;
                    tyreDetailsA.push({ "TireQuantity": frontQty, "Position": "F", "price": priceF });
                    tyreDetailsA.push({ "TireQuantity": backQty, "Position": "B", "price": priceB });
                }
            }

          //  console.table("tire position details array");
          //  console.table(tyreDetailsA);

            angular.forEach(tyreDetailsArray, function (value) {
                if ($scope.selectedvalue < 2) {
                    value["serialNo"] = $scope.dealerInvoiceTireDetails.front.serialLeft;
                    value["dotNumber"] = $scope.dealerInvoiceTireDetails.front.dotLeft;
                    value["width"] = $scope.dealerInvoiceTireDetails.front.width;
                    value["cross"] = $scope.dealerInvoiceTireDetails.front.cross;
                    value["diameter"] = $scope.dealerInvoiceTireDetails.front.diameter;
                    value["loadSpeed"] = $scope.dealerInvoiceTireDetails.front.loadSpeed;
                    value["price"] = $scope.dealerInvoiceTireDetails.front.price;
                    value["positionTemp"] = "F";
                    value["position"] = value.position;

                }
                if ($scope.selectedvalue == 2) {
                        if (value.position === "FL" || value.position === "FR") {
                            value["serialNo"] = $scope.dealerInvoiceTireDetails.front.serialLeft;
                            value["dotNumber"] = $scope.dealerInvoiceTireDetails.front.dotLeft;
                            value["width"] = $scope.dealerInvoiceTireDetails.front.width;
                            value["cross"] = $scope.dealerInvoiceTireDetails.front.cross;
                            value["diameter"] = $scope.dealerInvoiceTireDetails.front.diameter;
                            value["loadSpeed"] = $scope.dealerInvoiceTireDetails.front.loadSpeed;
                            value["price"] = $scope.dealerInvoiceTireDetails.front.price;
                            value["positionTemp"] = "F";
                            value["position"] = value.position;
                        }
                        if (value.position === "BL" || value.position === "BR") {
                            value["serialNo"] = $scope.dealerInvoiceTireDetails.back.serialLeft;
                            value["dotNumber"] = $scope.dealerInvoiceTireDetails.back.dotLeft;
                            value["width"] = $scope.dealerInvoiceTireDetails.back.width;
                            value["cross"] = $scope.dealerInvoiceTireDetails.back.cross;
                            value["diameter"] = $scope.dealerInvoiceTireDetails.back.diameter;
                            value["loadSpeed"] = $scope.dealerInvoiceTireDetails.back.loadSpeed;
                            value["price"] = $scope.dealerInvoiceTireDetails.back.price;
                            value["positionTemp"] = "B";
                            value["position"] = value.position;
                        }

                }
            });
         //   console.table("tire details array");
          //  console.table(tyreDetailsArray);

            if ($scope.selectedvalue == 2 && $scope.dealerInvoiceTireDetails.front.diameter == $scope.dealerInvoiceTireDetails.back.diameter) {
                angular.forEach(tyreDetailsA, function (aVal) {
                    let tyres = tyreDetailsArray;
                    aVal["tyres"] = tyres;
                });
            } else {
                angular.forEach(tyreDetailsA, function (aVal) {
                    let tyres = tyreDetailsArray.filter(com => com.positionTemp === aVal.Position);
                    aVal["tyres"] = tyres;
                });
            }

          //  console.table("separated Tyres Details");
           // console.table(tyreDetailsA);
            $scope.tyreDetailsValuesForSave = tyreDetailsA;
        }

        $scope.tyreDisplayViewDropdown = function () {
            let combinationString = $scope.spareWheelcheckbox + "" + $scope.dealerInvoiceTireDetails.front.leftcheckbox + "" +
                $scope.dealerInvoiceTireDetails.front.rightcheckbox + "" + $scope.dealerInvoiceTireDetails.back.leftcheckbox + "" +
                $scope.dealerInvoiceTireDetails.back.rightcheckbox;
            //console.log("combination String value", combinationString);
            //check this string value available our combinationSourceData
            let selectedCombinationObj = $scope.combinationSourceData.filter(com => com.combination === combinationString);
            //console.log("filtered Object");
            //console.table(selectedCombinationObj);
            //selection on dropdown

            if (selectedCombinationObj.length === 2) {

                $scope.isBackTireDetailsVisibleL = false;
                $scope.isFrontTireDetailsVisibleL = false;
                $scope.namesofTypes = [];
                $scope.dealerInvoiceDetails.quantity = '';
              //  $scope.frontTyreDetailsReset();
            }
            else {
                $scope.dealerInvoiceDetails.quantity = selectedCombinationObj[0].tyreQuantity;
                angular.forEach(selectedCombinationObj[0].result, function (value) {

                    //  console.log('visible Boxes ', value);
                    var model = $parse(value);
                    model.assign($scope, 1);
                });
                //$scope.isBackTireDetailsVisibleL = false;
				$scope.selectedValueCombination();
                //$scope.backTyreDetailsReset();
            }
        }

        $scope.resetTireDetails = function () {
            $scope.selectedvalue = '0';
            if ($scope.isBackTireDetailsVisibleL && $scope.isFrontTireDetailsVisibleL) {
                $scope.selectedvalue = '2';
            }
            if (!$scope.isBackTireDetailsVisibleL && !$scope.isFrontTireDetailsVisibleL) {
                $scope.frontLeftTyreSizeReadOnly = false;
                $scope.rearLeftTyreSizeReadOnly = false;

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
                        price:0.00

                    },
                    back: {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialRight: '',
                        serialLeft: '',
                        pattern: '',
                        dotLeft: '',
                        leftcheckbox: false,
                        rightcheckbox: false,
                        price:0.00
                    }
                };
            }

            if (!$scope.isBackTireDetailsVisibleL && $scope.isFrontTireDetailsVisibleL) {
                $scope.rearLeftTyreSizeReadOnly = false;
                //$scope.dealerInvoiceTireDetails = {
                //    front: {
                //        width: $scope.dealerInvoiceTireDetails.front.width == '' ? $scope.dealerInvoiceTireDetails.back.width : $scope.dealerInvoiceTireDetails.front.width ,
                //        cross: $scope.dealerInvoiceTireDetails.front.cross == '' ? $scope.dealerInvoiceTireDetails.back.cross: $scope.dealerInvoiceTireDetails.front.cross,
                //        diameter: $scope.dealerInvoiceTireDetails.front.diameter == '' ? $scope.dealerInvoiceTireDetails.back.diameter : $scope.dealerInvoiceTireDetails.front.diameter,
                //        loadSpeed: $scope.dealerInvoiceTireDetails.front.loadSpeed == '' ? $scope.dealerInvoiceTireDetails.back.loadSpeed : $scope.dealerInvoiceTireDetails.front.loadSpeed ,
                //        serialLeft: $scope.dealerInvoiceTireDetails.front.serialLeft == '' ? $scope.dealerInvoiceTireDetails.back.serialLeft : $scope.dealerInvoiceTireDetails.front.serialLeft ,
                //        serialRight: $scope.dealerInvoiceTireDetails.front.serialRight == '' ? $scope.dealerInvoiceTireDetails.back.serialRight : $scope.dealerInvoiceTireDetails.front.serialRight,
                //        pattern: $scope.dealerInvoiceTireDetails.front.pattern == '' ? $scope.dealerInvoiceTireDetails.back.pattern : $scope.dealerInvoiceTireDetails.front.pattern,
                //        dotLeft: $scope.dealerInvoiceTireDetails.front.dotLeft == '' ? $scope.dealerInvoiceTireDetails.back.dotLeft : $scope.dealerInvoiceTireDetails.front.dotLeft,
                //         leftcheckbox: $scope.dealerInvoiceTireDetails.front.leftcheckbox,
                //        rightcheckbox: $scope.dealerInvoiceTireDetails.front.rightcheckbox
                //    }, back: {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialRight: '',
                //        serialLeft:'',
                //        pattern: '',
                //        dotLeft: '',
                //        leftcheckbox: $scope.dealerInvoiceTireDetails.back.leftcheckbox,
                //        rightcheckbox: $scope.dealerInvoiceTireDetails.back.rightcheckbox
                //    }
                //};
            }

            if ($scope.isBackTireDetailsVisibleL && !$scope.isFrontTireDetailsVisibleL) {
                $scope.frontLeftTyreSizeReadOnly = false;
                //$scope.dealerInvoiceTireDetails = {
                //    front: {
                //        width: '',
                //        cross: '',
                //        diameter:'',
                //        loadSpeed: '',
                //        serialLeft:'',
                //        serialRight:'',
                //        pattern: '',
                //        dotLeft: '',
                //        leftcheckbox: $scope.dealerInvoiceTireDetails.front.leftcheckbox,
                //        rightcheckbox: $scope.dealerInvoiceTireDetails.front.rightcheckbox
                //    },
                //    back: {
                //        width: $scope.dealerInvoiceTireDetails.back.width == '' ? $scope.dealerInvoiceTireDetails.front.width : $scope.dealerInvoiceTireDetails.back.width,
                //        cross: $scope.dealerInvoiceTireDetails.back.cross == '' ? $scope.dealerInvoiceTireDetails.front.cross : $scope.dealerInvoiceTireDetails.back.cross,
                //        diameter: $scope.dealerInvoiceTireDetails.back.diameter == '' ? $scope.dealerInvoiceTireDetails.front.diameter : $scope.dealerInvoiceTireDetails.back.diameter,
                //        loadSpeed: $scope.dealerInvoiceTireDetails.back.loadSpeed == '' ? $scope.dealerInvoiceTireDetails.front.loadSpeed : $scope.dealerInvoiceTireDetails.back.loadSpeed,
                //        serialRight: $scope.dealerInvoiceTireDetails.back.serialRight == '' ? $scope.dealerInvoiceTireDetails.front.serialRight : $scope.dealerInvoiceTireDetails.back.serialRight,
                //        pattern: $scope.dealerInvoiceTireDetails.back.pattern == '' ? $scope.dealerInvoiceTireDetails.front.pattern : $scope.dealerInvoiceTireDetails.back.pattern,
                //        dotLeft: $scope.dealerInvoiceTireDetails.back.dotLeft == '' ? $scope.dealerInvoiceTireDetails.front.dotLeft : $scope.dealerInvoiceTireDetails.back.dotLeft,
                //        serialLeft: $scope.dealerInvoiceTireDetails.back.serialLeft == '' ? $scope.dealerInvoiceTireDetails.front.serialLeft : $scope.dealerInvoiceTireDetails.back.serialLeft,
                //        leftcheckbox: $scope.dealerInvoiceTireDetails.back.leftcheckbox,
                //        rightcheckbox: $scope.dealerInvoiceTireDetails.back.rightcheckbox
                //    }
                //};
            }

        }

        $scope.frontTyreDetailsReset = function () {
            $scope.dealerInvoiceTireDetails.front.width = "";
            $scope.dealerInvoiceTireDetails.front.cross = "";
            $scope.dealerInvoiceTireDetails.front.diameter = "";
            $scope.dealerInvoiceTireDetails.front.loadSpeed = "";
            $scope.dealerInvoiceTireDetails.front.pattern = "";
            $scope.dealerInvoiceTireDetails.front.serialLeft = "";
            $scope.dealerInvoiceTireDetails.front.dotLeft = "";
            $scope.frontLeftTyreSizeReadOnly = false;
            $scope.dealerInvoiceTireDetails.front.price = 0.00;


        }

        $scope.backTyreDetailsReset = function () {
            $scope.dealerInvoiceTireDetails.back.width = "";
            $scope.dealerInvoiceTireDetails.back.cross = "";
            $scope.dealerInvoiceTireDetails.back.diameter = "";
            $scope.dealerInvoiceTireDetails.back.loadSpeed = "";
            $scope.dealerInvoiceTireDetails.back.pattern = "";
            $scope.dealerInvoiceTireDetails.back.serialLeft = "";
            $scope.dealerInvoiceTireDetails.back.dotLeft = "";
            $scope.rearLeftTyreSizeReadOnly = false;
            $scope.dealerInvoiceTireDetails.back.price= 0.00;
        }

        $scope.resetButtons = function () {
            $scope.dealerInvoiceTireDetails.back.leftcheckbox = 0;
            $scope.dealerInvoiceTireDetails.front.rightcheckbox = 0;
            $scope.dealerInvoiceTireDetails.back.rightcheckbox = 0;
            $scope.dealerInvoiceTireDetails.front.leftcheckbox = 0;
            $scope.spareWheelcheckbox = 0;
            $scope.isBackTireDetailsVisibleL = false;
            $scope.isFrontTireDetailsVisibleL = false;
            $scope.dealerInvoiceDetails.quantity = 0;
        }

        $scope.resetInvoiceTireDetails = function (fullReset) {
            if (fullReset) {
                $scope.frontLeftTyreSizeReadOnly = false;
                $scope.rearLeftTyreSizeReadOnly = false;
                $scope.selectedvalue = '0';
                $scope.dealerInvoiceTireDetails.front.width = "";
                $scope.dealerInvoiceTireDetails.front.cross = "";
                $scope.dealerInvoiceTireDetails.front.diameter = "";
                $scope.dealerInvoiceTireDetails.front.loadSpeed = "";
                $scope.dealerInvoiceTireDetails.front.pattern = "";
                $scope.dealerInvoiceTireDetails.front.serialLeft = "";
                $scope.dealerInvoiceTireDetails.front.dotLeft = "";

                $scope.dealerInvoiceTireDetails.back.width = "";
                $scope.dealerInvoiceTireDetails.back.cross = "";
                $scope.dealerInvoiceTireDetails.back.diameter = "";
                $scope.dealerInvoiceTireDetails.back.loadSpeed = "";
                $scope.dealerInvoiceTireDetails.back.pattern = "";
                $scope.dealerInvoiceTireDetails.back.serialLeft = "";
                $scope.dealerInvoiceTireDetails.back.dotLeft = "";
            }



            $scope.frontTyreWidthValidation = "";
            $scope.frontTyreCrossValidation = "";
            $scope.frontTyreDiameterValidation = "";
            $scope.frontTyreLoadSpeedValidation = "";
            $scope.frontTyreLeftSerialValidation = "";
            $scope.frontTyreRightSerialValidation = "";
            $scope.frontTyreRightpatternValidation = "";
            $scope.backTyreWidthValidation = "";
            $scope.backTyreCrossValidation = "";
            $scope.backTyreDiameterValidation = "";
            $scope.backTyreLoadSpeedValidation = "";
            $scope.backTyreSerialLeftValidation = "";
            $scope.backTyreSerialRightValidation = "";
            $scope.backTyrepatternValidation = "";
            //$scope.disabledFrontTireLB = true;
            //$scope.disabledFrontTireRF = true;
            //$scope.disabledFrontTireRB = true;
            //$scope.disabledFrontTireLF = true;
            //$scope.selectedvalue = '0';
            //$scope.frontLeftTyreSizeReadOnly=false;
            //$scope.rearLeftTyreSizeReadOnly=false;
            $scope.loadAvailableTireDetails();
        }

        $scope.findArticleNoFrontLeft = function (keyEvent) {

            if (keyEvent.which === 13) {
                swal({ title: 'Loading...', text: 'Tyre Size Details', showConfirmButton: false });
                $scope.frontLeftTyreSizeReadOnly = false;
                $scope.dealerInvoiceTireDetails.front.width = '';
                $scope.dealerInvoiceTireDetails.front.cross = '';
                $scope.dealerInvoiceTireDetails.front.diameter = '';
                $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                $scope.dealerInvoiceTireDetails.front.pattern = '';
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreDetailsByArticleNo',
                    data: { "articleNo": 'C'+ $scope.dealerInvoiceTireDetails.front.serialLeft + '0000' },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data!=null && data.AvailableTireSizeId  != null) {
                        $scope.dealerInvoiceTireDetails.front.width = data.Width;
                        $scope.dealerInvoiceTireDetails.front.cross = data.CrossSection;
                        $scope.dealerInvoiceTireDetails.front.diameter = data.Diameter;
                        $scope.dealerInvoiceTireDetails.front.loadSpeed = data.LoadSpeed;
                        $scope.dealerInvoiceTireDetails.front.pattern = data.Pattern;
                        $scope.frontLeftTyreSizeReadOnly = true;
                    } else {
                        customErrorMessage("Article number is incorrect.");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.findArticleNoFrontRight = function (keyEvent) {
            $scope.frontLeftTyreSizeReadOnly=false;
            if (keyEvent.which === 13) {
                swal({ title: 'Loading...', text: 'Tyre Size Details', showConfirmButton: false });
                $scope.dealerInvoiceTireDetails.front.width = '';
                $scope.dealerInvoiceTireDetails.front.cross = '';
                $scope.dealerInvoiceTireDetails.front.diameter = '';
                $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                $scope.dealerInvoiceTireDetails.front.pattern = '';
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreDetailsByArticleNo',
                    data: { "articleNo": 'C' + $scope.dealerInvoiceTireDetails.front.serialRight + '0000' },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null && data.AvailableTireSizeId != null) {
                        $scope.dealerInvoiceTireDetails.front.width = data.Width;
                        $scope.dealerInvoiceTireDetails.front.cross = data.CrossSection;
                        $scope.dealerInvoiceTireDetails.front.diameter = data.Diameter;
                        $scope.dealerInvoiceTireDetails.front.loadSpeed = data.LoadSpeed;
                        $scope.dealerInvoiceTireDetails.front.pattern = data.Pattern;
                        $scope.frontLeftTyreSizeReadOnly = true;
                    } else {
                        customErrorMessage("Article number is incorrect.");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.findArticleNoBackRight = function (keyEvent) {
            $scope.rearLeftTyreSizeReadOnly=false;
            if (keyEvent.which === 13) {
                swal({ title: 'Loading...', text: 'Tyre Size Details', showConfirmButton: false });
                $scope.dealerInvoiceTireDetails.back.width ='';
                $scope.dealerInvoiceTireDetails.back.cross = '';
                $scope.dealerInvoiceTireDetails.back.diameter = '';
                $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                $scope.dealerInvoiceTireDetails.back.pattern = '';
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreDetailsByArticleNo',
                    data: { "articleNo": 'C' + $scope.dealerInvoiceTireDetails.back.serialRight + '0000' },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null && data.AvailableTireSizeId != null) {
                        $scope.dealerInvoiceTireDetails.back.width = data.Width;
                        $scope.dealerInvoiceTireDetails.back.cross = data.CrossSection;
                        $scope.dealerInvoiceTireDetails.back.diameter = data.Diameter;
                        $scope.dealerInvoiceTireDetails.back.loadSpeed = data.LoadSpeed;
                        $scope.dealerInvoiceTireDetails.back.pattern = data.Pattern;
                        $scope.rearLeftTyreSizeReadOnly = true;
                    } else {
                        customErrorMessage("Article number is incorrect.");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.findArticleNoBackLeft = function (keyEvent) {
            $scope.rearLeftTyreSizeReadOnly=false;
            if (keyEvent.which === 13) {
                swal({ title: 'Loading...', text: 'Tyre Size Details', showConfirmButton: false });
                $scope.dealerInvoiceTireDetails.back.width ='';
                $scope.dealerInvoiceTireDetails.back.cross = '';
                $scope.dealerInvoiceTireDetails.back.diameter = '';
                $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                $scope.dealerInvoiceTireDetails.back.pattern = '';
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreDetailsByArticleNo',
                    data: { "articleNo": 'C' + $scope.dealerInvoiceTireDetails.back.serialLeft + '0000' },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null && data.AvailableTireSizeId != null) {
                        $scope.dealerInvoiceTireDetails.back.width = data.Width;
                        $scope.dealerInvoiceTireDetails.back.cross = data.CrossSection;
                        $scope.dealerInvoiceTireDetails.back.diameter = data.Diameter;
                        $scope.dealerInvoiceTireDetails.back.loadSpeed = data.LoadSpeed;
                        $scope.dealerInvoiceTireDetails.back.pattern = data.Pattern;
                        $scope.rearLeftTyreSizeReadOnly=true;
                    } else {
                        customErrorMessage("Article number is incorrect.");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.findArticleNoDownLeft = function (keyEvent) {
            if (keyEvent.which === 13) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreDetailsByArticleNo',
                    data: { "articleNo": 'C' + $scope.dealerInvoiceTireDetails.down.serialLeft + '0000' },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.AvailableTireSizeId != null) {
                        swal.close();
                        $scope.dealerInvoiceTireDetails.down.width = data.Width;
                        $scope.dealerInvoiceTireDetails.down.cross = data.CrossSection;
                        $scope.dealerInvoiceTireDetails.down.diameter = data.Diameter;
                        $scope.dealerInvoiceTireDetails.down.loadSpeed = data.LoadSpeed;
                        $scope.dealerInvoiceTireDetails.down.pattern = data.Pattern;
                    } else {
                        customErrorMessage("Article number is incorrect.");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }
        }

        $scope.frontTireEntryfrontwidth = function () {

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                //    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.front = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }
            if ($scope.dealerInvoiceTireDetails.front.width != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByWidth',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableCrossList = data.CrossSectionList;
                    $scope.dealerInvoiceTireDetails.front.cross = '';
                    $scope.dealerInvoiceTireDetails.front, diameter = '';
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';

                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            }

        }

        $scope.frontTireEntryfrontcross = function () {

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                //    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.front = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }

            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByDiameter',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableDiameteList = data.DiameterList;
                    $scope.dealerInvoiceTireDetails.front.diameter = '';
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }


        }


        $scope.frontTireEntryfrontdiameter = function () {

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                //    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.front = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }
            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != ""
                && $scope.dealerInvoiceTireDetails.front.diameter != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.front.diameter,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByloadSpeed',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availableLoadSpeedList = data.LoadSpeedList;
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }

        }



        $scope.frontTireEntryfrontloadSpeed = function () {

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                //    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.front = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }
            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != ""
                && $scope.dealerInvoiceTireDetails.front.diameter != "" && $scope.dealerInvoiceTireDetails.front.loadSpeed != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.front.diameter,
                    'frontloadSpeed': $scope.dealerInvoiceTireDetails.front.loadSpeed,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByPattern',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availablePatternList = data.PatternList;
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }

        $scope.frontTyreEnterdPttern = function () {
            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != ""
                && $scope.dealerInvoiceTireDetails.front.diameter != "" && $scope.dealerInvoiceTireDetails.front.loadSpeed != "" &&
                $scope.dealerInvoiceTireDetails.front.pattern != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'width': $scope.dealerInvoiceTireDetails.front.width,
                    'cross': $scope.dealerInvoiceTireDetails.front.cross,
                    'diameter': $scope.dealerInvoiceTireDetails.front.diameter,
                    'loadSpeed': $scope.dealerInvoiceTireDetails.front.loadSpeed,
                    'pattern': $scope.dealerInvoiceTireDetails.front.pattern,
                };


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetArticleNoByTyreSize',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null) {
                        var updatedArticleNo = data.ArticleNo.substring(1, 8);
                       // console.log(updatedArticleNo);
                        $scope.dealerInvoiceTireDetails.front.serialLeft = updatedArticleNo;
                    } else {
                        customErrorMessage("There is no article number related to the tire size.");
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });


            }

        }

        $scope.backTyreEnterdPttern = function () {
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "" && $scope.dealerInvoiceTireDetails.back.loadSpeed != "" &&
                $scope.dealerInvoiceTireDetails.back.pattern != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'width': $scope.dealerInvoiceTireDetails.back.width,
                    'cross': $scope.dealerInvoiceTireDetails.back.cross,
                    'diameter': $scope.dealerInvoiceTireDetails.back.diameter,
                    'loadSpeed': $scope.dealerInvoiceTireDetails.back.loadSpeed,
                    'pattern': $scope.dealerInvoiceTireDetails.back.pattern,
                };


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetArticleNoByTyreSize',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null) {
                        var updatedArticleNo = data.ArticleNo.substring(1, 8);
                       // console.log(updatedArticleNo);
                        $scope.dealerInvoiceTireDetails.back.serialLeft = updatedArticleNo;
                    } else {
                        customErrorMessage("There is no article number related to the tire size.");
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });


            }

        }

        $scope.frontTireEntry = function () {
            if($scope.dealerInvoiceTireDetails.front.serialLeft==='0000000' || $scope.dealerInvoiceTireDetails.front.serialRight==='0000000'){
                $scope.frontLeftTyreSizeReadOnly=false;
                $scope.dealerInvoiceTireDetails.front.width = '';
                $scope.dealerInvoiceTireDetails.front.cross = '';
                $scope.dealerInvoiceTireDetails.front.diameter = '';
                $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                $scope.dealerInvoiceTireDetails.front.pattern = '';
            }

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                //    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.front = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }
        }

        $scope.rearTireEntryBackwidth = function () {

            if ($scope.dealerInvoiceDetails.quantity === 2) {
                //if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                //    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                //    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                //    $scope.dealerInvoiceTireDetails.back = {
                //        width: '',
                //        cross: '',
                //        diameter: '',
                //        loadSpeed: '',
                //        serialLeft: '',
                //        serialRight: ''
                //    }
                //}
            }
            if ($scope.dealerInvoiceTireDetails.back.width != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByWidth',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableCrossListBack = data.CrossSectionList;
                    $scope.dealerInvoiceTireDetails.back.cross = '';
                    $scope.dealerInvoiceTireDetails.back, diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }

        }

        $scope.rearTireEntryBackcross = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}

            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByDiameter',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableDiameteListBack = data.DiameterList;
                    $scope.dealerInvoiceTireDetails.back.diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }


        }

        $scope.rearTireEntryBackdiameter = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByloadSpeed',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availableLoadSpeedListBack = data.LoadSpeedList;
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }

        }

        $scope.rearTireEntryBackloadSpeed = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "" && $scope.dealerInvoiceTireDetails.back.loadSpeed != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                    'frontloadSpeed': $scope.dealerInvoiceTireDetails.back.loadSpeed,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByPattern',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availablePatternListBack = data.PatternList;
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }

        $scope.rearTireEntry = function () {
            // this validation function commented , because cannot Enter Rear Tyre Article number -> when first enter front article and then enter rear article number

            // if ($scope.dealerInvoiceDetails.quantity === 2) {
            //     if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //         $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //         customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //         $scope.dealerInvoiceTireDetails.back = {
            //             width: '',
            //             cross: '',
            //             diameter: '',
            //             loadSpeed: '',
            //             serialLeft: '',
            //             serialRight: ''
            //         }
            //     }
            // }

            if($scope.dealerInvoiceTireDetails.back.serialLeft==='0000000' || $scope.dealerInvoiceTireDetails.back.serialRight==='0000000'){
                $scope.rearLeftTyreSizeReadOnly=false;
                $scope.dealerInvoiceTireDetails.back.width = '';
                $scope.dealerInvoiceTireDetails.back.cross = '';
                $scope.dealerInvoiceTireDetails.back.diameter = '';
                $scope.dealerInvoiceTireDetails.back.loadSpeed ='';
                $scope.dealerInvoiceTireDetails.back.pattern = '';
            }

        }

        $scope.downTireEntryBackwidth = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
            if ($scope.dealerInvoiceTireDetails.back.width != "") {

                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByWidth',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableCrossListBack = data.CrossSectionList;
                    $scope.dealerInvoiceTireDetails.back.cross = '';
                    $scope.dealerInvoiceTireDetails.back, diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }

        }

        $scope.downTireEntryBackcross = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}

            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByDiameter',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.availableDiameteListBack = data.DiameterList;
                    $scope.dealerInvoiceTireDetails.back.diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }


        }

        $scope.downTireEntryBackdiameter = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByloadSpeed',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availableLoadSpeedListBack = data.LoadSpeedList;
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }

        }

        $scope.downTireEntryBackloadSpeed = function () {

            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "" && $scope.dealerInvoiceTireDetails.back.loadSpeed != "") {
                swal({ title: 'Loading...', text: 'Loading Tyre Sizes', showConfirmButton: false });
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                    'frontloadSpeed': $scope.dealerInvoiceTireDetails.back.loadSpeed,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByPattern',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availablePatternListBack = data.PatternList;
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }

        $scope.downTireEntry = function () {
            //if ($scope.dealerInvoiceDetails.quantity === 2) {
            //    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
            //        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
            //        customErrorMessage("2 tiers cannot be enterd in both front and rear.");
            //        $scope.dealerInvoiceTireDetails.back = {
            //            width: '',
            //            cross: '',
            //            diameter: '',
            //            loadSpeed: '',
            //            serialLeft: '',
            //            serialRight: ''
            //        }
            //    }
            //}
        }


        $scope.frontTyreWidthValidation = '';
        $scope.frontTyreCrossValidation = '';
        $scope.frontTyreDiameterValidation = '';
        $scope.frontTyreLoadSpeedValidation = '';
        $scope.frontTyreLeftSerialValidation = '';
        $scope.frontTyreRightSerialValidation = '';

        $scope.backTyreWidthValidation = '';
        $scope.backTyreCrossValidation = '';
        $scope.backTyreDiameterValidation = '';
        $scope.backTyreLoadSpeedValidation = '';
        $scope.backTyreSerialRightValidation = '';
        $scope.backTyreSerialLeftValidation = '';

        $scope.validateBackTireDetails = function () {
            var isValid = true;
            if ($scope.dealerInvoiceTireDetails.back.width.length == 0 || $scope.dealerInvoiceTireDetails.back.width == 0) {
                $scope.backTyreWidthValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreWidthValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.cross.length == 0 || $scope.dealerInvoiceTireDetails.back.cross == 0) {
                $scope.backTyreCrossValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreCrossValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.diameter.length == 0 || $scope.dealerInvoiceTireDetails.back.diameter == 0) {
                $scope.backTyreDiameterValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreDiameterValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.loadSpeed.length == 0 || $scope.dealerInvoiceTireDetails.back.loadSpeed == 0) {
                $scope.backTyreLoadSpeedValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreLoadSpeedValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.pattern.length == 0 || $scope.dealerInvoiceTireDetails.back.pattern == 0) {
                $scope.backTyrepatternValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyrepatternValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.price == null || $scope.dealerInvoiceTireDetails.back.price == undefined ||  $scope.dealerInvoiceTireDetails.back.price.length == 0 || $scope.dealerInvoiceTireDetails.back.price == 0) {
                $scope.backTyrePriceValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyrePriceValidation = "";

            //if ($scope.dealerInvoiceTireDetails.back.serialLeft.length == 0) {
            //    $scope.backTyreSerialLeftValidation = "has-error";
            //    isValid = false;
            //} else
            //    $scope.backTyreSerialLeftValidation = "";

            //if ($scope.dealerInvoiceTireDetails.back.serialRight.length == 0) {
            //    $scope.backTyreSerialRightValidation = "has-error";
            //    isValid = false;
            //} else
            //    $scope.backTyreSerialRightValidation = "";

            return isValid;
        }
        $scope.validateFrontTireDetails = function () {
            var isValid = true;
            if ($scope.dealerInvoiceTireDetails.front.width.length == 0 || $scope.dealerInvoiceTireDetails.front.width == 0) {
                $scope.frontTyreWidthValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreWidthValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.cross.length == 0 || $scope.dealerInvoiceTireDetails.front.cross == 0) {
                $scope.frontTyreCrossValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreCrossValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.diameter.length == 0 || $scope.dealerInvoiceTireDetails.front.diameter == 0) {
                $scope.frontTyreDiameterValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreDiameterValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.loadSpeed.length == 0 || $scope.dealerInvoiceTireDetails.front.loadSpeed == 0) {
                $scope.frontTyreLoadSpeedValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreLoadSpeedValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.pattern.length == 0 || $scope.dealerInvoiceTireDetails.front.pattern == 0) {
                $scope.frontTyreRightpatternValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreRightpatternValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.price == null || $scope.dealerInvoiceTireDetails.front.price == undefined || $scope.dealerInvoiceTireDetails.front.price.length == 0 || $scope.dealerInvoiceTireDetails.front.price == 0) {
                $scope.frontTyrePriceValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyrePriceValidation = "";

            //if ($scope.dealerInvoiceTireDetails.front.serialLeft.length == 0) {
            //    $scope.frontTyreLeftSerialValidation = "has-error";
            //    isValid = false;
            //} else
            //    $scope.frontTyreLeftSerialValidation = "";

            //if ($scope.dealerInvoiceTireDetails.front.serialRight.length == 0) {
            //    $scope.frontTyreRightSerialValidation = "has-error";
            //    isValid = false;
            //} else
            //    $scope.frontTyreRightSerialValidation = "";

            return isValid;
        }

        // ------------------------ Customer Validation -------------------------------------
        $scope.validateCustomer = function () {
            var isValid = true;

            if ($scope.customer.plateNumber == "") {
                isValid = false;
                $scope.validate_Plate = "has-error";
            } else {
                $scope.validate_Plate = "";
            }

            if ($scope.customer.PlateRelatedCityId == null || $scope.customer.PlateRelatedCityId == "") {
                isValid = false;
                $scope.validate_city_model = "has-error";
            } else {
                $scope.validate_city_model = "";
            }

            if (!isGuid($scope.MakeId)) {
                $scope.validate_additional_make = "has-error";
                isValid = false;
            } else
                $scope.validate_additional_make = "";

            if (!isGuid($scope.ModelId)) {
                $scope.validate_additional_model = "has-error";
                isValid = false;
            } else
                $scope.validate_additional_model = "";

            if ($scope.customer.invoiceNo == "") {
                $scope.validate_invoiceNo = "has-error";
                isValid = false;
            } else
                $scope.validate_invoiceNo = "";

            //console.log($scope.additionalSelectedModelYearList);
            if (!parseInt($scope.YearId)) {
                $scope.validate_additional_modelYear = "has-error";
                isValid = false;
            } else
                $scope.validate_additional_modelYear = "";

            if (parseFloat($scope.customer.addMileage) && parseFloat($scope.customer.addMileage) > 0) {
                $scope.validate_additional_mileage = "";
            } else {
                $scope.validate_additional_mileage = "has-error";
                isValid = false;
            }


            if ($scope.customer.usageTypeId == "" || $scope.customer.usageTypeId == 0) {
                isValid = false;
                $scope.validate_usageTypeId = "has-error";
            } else {
                $scope.validate_usageTypeId = "";
            }

            if ($scope.customer.customerTypeId == "" || $scope.customer.customerTypeId ==="00000000-0000-0000-0000-000000000000") {
                isValid = false;
                $scope.validate_customerTypeId = "has-error";
            } else {
                $scope.validate_customerTypeId = "";
            }

            if ($scope.dealerInvoiceDetails.dealerBranchId == null || $scope.dealerInvoiceDetails.dealerBranchId == "" || $scope.dealerInvoiceDetails.dealerBranchId === "00000000-0000-0000-0000-000000000000") {
                isValid = false;
                $scope.validate_dealer_branch_model = "has-error";
            } else {
                $scope.validate_dealer_branch_model = "";
            }

            if ($scope.customer.mobileNo == "+971") {
                isValid = false;
                $scope.validate_mobileNo = "has-error";
            } else {
                $scope.validate_mobileNo = "";
            }

            if ($scope.selectedCustomerTypeName == "Corporate") {
                if ($scope.customer.businessName == "") {
                    $scope.validate_businessName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_businessName = "";
                }

                if ($scope.customer.businessTelNo == "") {
                    $scope.validate_businessTelNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_businessTelNo = "";
                }

            } else {
                if ($scope.customer.firstName == "") {
                    isValid = false;
                    $scope.validate_firstName = "has-error";
                } else {
                    $scope.validate_firstName = "";
                }

                if ($scope.customer.lastName == "") {
                    isValid = false;
                    $scope.validate_lastName = "has-error";
                } else {
                    $scope.validate_lastName = "";
                }
            }


            if ($scope.customer.plateNumber == "") {
                isValid = false;
                $scope.validate_Plate = "has-error";
            } else {
                $scope.validate_Plate = "";
            }

            if (!validateEmail($scope.customer.email)) {
                isValid = false;
                $scope.validate_email = "has-error";
            } else {
                $scope.validate_email = "";
            }

            if ($scope.purchaseDate == null || $scope.purchaseDate == undefined ||  $scope.purchaseDate == "") {
                isValid = false;
                $scope.validate_purchaseDate = "has-error";
            } else {
                $scope.validate_purchaseDate = "";
            }

            if ($scope.premiumOneModal == null || $scope.premiumOneModal == undefined || $scope.premiumOneModal == "") {
                isValid = false;
                $scope.validate_contractOne = "has-error";
            } else {
                $scope.validate_contractOne = "";
            }

            if ($scope.isTowContract) {
                if ($scope.premiumTwoModal == null || $scope.premiumTwoModal == undefined || $scope.premiumTwoModal == "") {
                    isValid = false;
                    $scope.validate_contractTow = "has-error";
                } else {
                    $scope.validate_contractTow = "";
                }


            }




            return isValid;
        }

        $scope.nextPage = function () {
            $state.go('app.buyingprocessDemo', { tempInvId: null });
            //$timeout(function () {
            //    $state.go('home.buyingprocessDemo', { tempInvId: data.msg });
            //    swal.close();
            //}, 2000);
        }

        $scope.policySaveStatusTitle = "";
        $scope.policySaveStatusMsg = "";

        $scope.submitTyreSales = function () {
            if ($scope.customerInvoiceUploader.queue.length !== 0) {
                $scope.customerInvoiceUploader.uploadAll();
            } else if ($scope.customer.invoiceAttachmentId.length > 0) {
                $scope.generateInvoiceCode();
            } else {
                customErrorMessage("Please Attach Invoice ");
            }
        }

        $scope.generateInvoiceCode = function () {

            if ($scope.dealerInvoiceDetails.quantity != '') {

                if ($scope.validateCustomer()) {

                    swal({ title: 'Processing...', text: 'Saving policy details', showConfirmButton: false });

                    $scope.dealerInvoiceDetails.plateNumber = $scope.customer.plateNumber;
                    $scope.customer.addMakeId = $scope.MakeId;
                    $scope.customer.addModelId = $scope.ModelId;
                    $scope.customer.addModelYear = $scope.YearId;
                    $scope.customer.tpaId = $localStorage.tpaID;
                    if ($scope.selectedvalue < 2) {
                        $scope.alltyresareSame = true;
                    }
                    $scope.generateContractDetails();
                    var data = {
                        'dealerInvoiceDetails': $scope.dealerInvoiceDetails,
                        'dealerInvoiceTireSaveDetails': $scope.dealerInvoiceTireDetails,
                        'loggedInUserId': $localStorage.LoggedInUserId,
                        'customerDetails': $scope.customer,
                        'customerId': $scope.loggedInCustomer.id,
                        'tempInvId': $scope.tempInvId,
                        'alltyres': $scope.alltyresareSame,
                        'tyreDetails': $scope.tyreDetailsValuesForSave,
                        'contractDetails': $scope.contractDetails
                    };


                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/DealerManagement/SaveTyrePolicyDetails',
                        dataType: 'json',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        if (data.code === "SUCCESS") {
                            SweetAlert.swal({
                                html: true,
                                title: "Success",
                                text: data.msg,
                                type: "success",
                                showCancelButton: false,
                                closeOnConfirm: true,
                                allowEscapeKey: false,
                                confirmButtonText: "Ok, Great !",
                                confirmButtonColor: "#ffa500",
                            }, function () {
                                $scope.resetAll();
                                $scope.resetButtons();
                            });
                        } else {
                            swal.close();
                            customErrorMessage(data.msg);
                            if (data.msg == "This Invoice Number Already Entered in Previous Tyre Sales.") {
                                $scope.validate_invoiceNo = "has-error";
                            }
                        }
                        //if (data.code === "SUCCESS") {
                        //    //swal.close();
                        //    //var txt = "<h3>Your invoice code is :</h3><strong><h2>" + data.msg + "</h2></strong>.";
                        //    $scope.policySaveStatusTitle = "Success!";
                        //    $scope.policySaveStatusMsg = "Policy you entered saved successfully.";
                        //    $scope.policySaveStatusConfirmButtons = true;
                        //    swal({ title: $scope.policySaveStatusTitle, text: $scope.policySaveStatusMsg, showConfirmButton: true });
                        //    $scope.resetAll();
                        //} else {
                        //    swal.close();
                        //    customErrorMessage(data.msg);
                        //}

                    }).error(function (data, status, headers, config) {
                        swal.close();
                    });
                } else {
                    swal.close();
                    customErrorMessage('Please fill all highlighted  details.');
                }


            } else {
                swal.close();
                customErrorMessage('Please select tire quantity.');
            }
        }

        $scope.generateContractDetails = function () {
            $scope.contractDetails = [];
            $scope.contractDetails.push({ 'ContractId': $scope.contractOneModal, 'ContractExtensionPremiumId': $scope.premiumOneModal, 'ContractExtensionsId': $scope.ContractExtensionsIdOne, 'Position': "F", 'purchaseDate': $scope.purchaseDate });
            if ($scope.isTowContract) {
                $scope.contractDetails.push({ 'ContractId': $scope.contractTowModal, 'ContractExtensionPremiumId': $scope.premiumTwoModal, 'ContractExtensionsId': $scope.ContractExtensionsIdTwo, 'Position': "B", 'purchaseDate': $scope.purchaseDate});
            }
        }

        $scope.resetForNewItemEntry = function () {
            $scope.resetInvoiceTireDetails(true);
            $scope.dealerInvoiceDetails.quantity = '';
            $scope.dealerInvoiceDetails.plateNumber = '';
            goToStep(1);
            $scope.dataLoadMode = false;
        }

        $scope.searchPopup = function () {
            CodesearchPopup = ngDialog.open({
                template: 'popUpSearchCode',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            getInvoiceCodeSearchPage();
        };
        var paginationOptionsDealerDealerInvoiceSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.gridOptionsInvoiceCode = {
            paginationPageSizes: [10, 20, 50],
            paginationPageSize: 10,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Generated Date', field: 'Date', enableSorting: false, cellClass: 'columCss' },
                { name: 'Plate Number', field: 'PlateNumber', enableSorting: false, cellClass: 'columCss' },
                { name: 'Tire Quantity', field: 'TireQuantity', enableSorting: false, cellClass: 'columCss' },
                { name: 'Invoice Code', field: 'Code', enableSorting: false, cellClass: 'columCssBold' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadInvoiceCodeDetails(row.entity.Id)" class="btn btn-xs btn-info">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsDealerDealerInvoiceSearchGrid.pageNumber = newPage;
                    paginationOptionsDealerDealerInvoiceSearchGrid.pageSize = pageSize;
                    getInvoiceCodeSearchPage();
                });
            }
        };

        $scope.resetSearchCriterias = function () {
            $scope.dealerInvoiceCodeSearchParam = {
                date: '',
                plateNo: '',
                code: ''
            };
            getInvoiceCodeSearchPage();
        }
        $scope.refresInvoiceCodeSearchGridData = function () {
            getInvoiceCodeSearchPage();
        }
        function getInvoiceCodeSearchPage() {
            if (isGuid($scope.dealerInvoiceDetails.dealerId)) {
                $scope.gridDealerInvoiceCodeLoading = true;
                $scope.gridDealerInvoiceCodeloadAttempted = false;
                var InvoiceCodeSearchGridParam =
                {
                    'paginationOptionsDealerDealerInvoiceSearchGrid': paginationOptionsDealerDealerInvoiceSearchGrid,
                    'dealerInvoiceSearchGridSearchCriterias': $scope.dealerInvoiceCodeSearchParam,
                    'dealerInvoiceFilterInformation': $scope.dealerInvoiceDetails
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/SearchDealerInvoiceCode',
                    data: JSON.stringify(InvoiceCodeSearchGridParam),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var responseArr = JSON.parse(data);
                    if (responseArr != null) {
                        $scope.gridOptionsInvoiceCode.data = responseArr.data;
                        $scope.gridOptionsInvoiceCode.totalItems = responseArr.totalRecords;
                    } else {
                        $scope.gridOptionsInvoiceCode.data = [];
                        $scope.gridOptionsInvoiceCode.totalItems = 0;
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    $scope.gridDealerInvoiceCodeLoading = false;
                    $scope.gridDealerInvoiceCodeloadAttempted = true;
                });
            }
        };

        $scope.showImagePopup = function (itemId) {

            AddNewCalculationPopup = ngDialog.open({
                template: 'popUpImage',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope,

            });
        }


        $scope.validateFrontTyre = function () {
            var isValidated = true;
            if ($scope.dealerInvoiceTireDetails.front.dotLeft == undefined || $scope.dealerInvoiceTireDetails.front.dotLeft === "") {
                customErrorMessage("Please enter a dot number.");
                isValidated = false;
            }

           // console.log("front left serial ", $scope.dealerInvoiceTireDetails.front.serialLeft);
            if ($scope.dealerInvoiceTireDetails.front.serialLeft == undefined || $scope.dealerInvoiceTireDetails.front.serialLeft === "0000000" || $scope.dealerInvoiceTireDetails.front.serialLeft.length == 0) {
                customErrorMessage("Please enter a Article Number.");
                isValidated = false;
            }

            if (!$scope.validateFrontTireDetails()) {
                customErrorMessage("Please fill all highlighted tire details.");
                frontValidationMessagePopuped = true;
                isValidated = false;
            }
            return isValidated;
        }

        $scope.validateRearTyre = function () {
            var isValidated = true;
            if ($scope.dealerInvoiceTireDetails.back.dotLeft == undefined || $scope.dealerInvoiceTireDetails.back.dotLeft === "") {
                customErrorMessage("Please enter a dot number.");
                isValidated = false;
            }

            if ($scope.dealerInvoiceTireDetails.back.serialLeft == undefined || $scope.dealerInvoiceTireDetails.back.serialLeft === "0000000" || $scope.dealerInvoiceTireDetails.back.serialLeft.length == 0) {
                customErrorMessage("Please enter a Article Number.");
                isValidated = false;
            }

            if (!$scope.validateBackTireDetails()) {
                customErrorMessage("Please fill all highlighted tire details.");
                isValidated = false;
            }
            return isValidated;
        }


        $scope.loadInvoiceCodeDetails = function (invoiceId) {
            if (isGuid(invoiceId)) {

                CodesearchPopup.close();
                swal({ title: 'Loading...', text: 'Invoice code information', showConfirmButton: false });

                var data = {
                    'invoiceId': invoiceId,
                    'dealerId': $scope.dealerInvoiceDetails.dealerId,
                    'dealerBranchId': $scope.dealerInvoiceDetails.dealerBranchId,
                    'countryId': $scope.dealerInvoiceDetails.countryId,
                    'cityId': $scope.dealerInvoiceDetails.cityId,
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/LoadInvoceCodeDetailsById',
                    dataType: 'json',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.dealerInvoiceDetails.quantity = data.Quantity;
                    $scope.dealerInvoiceDetails.plateNumber = data.PlateNumber;

                    $scope.dealerInvoiceTireDetails.front.width = data.TireFront.Wide == 0 ? "" : data.TireFront.Wide;
                    $scope.dealerInvoiceTireDetails.front.cross = data.TireFront.Cross == 0 ? "" : data.TireFront.Cross;
                    $scope.dealerInvoiceTireDetails.front.diameter = data.TireFront.Diameter == 0 ? "" : data.TireFront.Diameter;
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = data.TireFront.LoadSpeed;
                    $scope.dealerInvoiceTireDetails.front.serialLeft = data.TireFront.SerialLeft;
                    $scope.dealerInvoiceTireDetails.front.serialRight = data.TireFront.SerialRight;
                    $scope.dealerInvoiceTireDetails.front.pattern = data.TireFront.PatternRight;
                    $scope.dealerInvoiceTireDetails.front.pattern = data.TireFront.PatternLeft;

                    $scope.dealerInvoiceTireDetails.back.width = data.TireBack.Wide == 0 ? "" : data.TireBack.Wide;
                    $scope.dealerInvoiceTireDetails.back.cross = data.TireBack.Cross == 0 ? "" : data.TireBack.Cross;
                    $scope.dealerInvoiceTireDetails.back.diameter = data.TireBack.Diameter == 0 ? "" : data.TireBack.Diameter;
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = data.TireBack.LoadSpeed;
                    $scope.dealerInvoiceTireDetails.back.serialLeft = data.TireBack.SerialLeft;
                    $scope.dealerInvoiceTireDetails.back.serialRight = data.TireBack.SerialRight;
                    $scope.dealerInvoiceTireDetails.back.pattern = data.TireBack.PatternRight;
                    $scope.dealerInvoiceTireDetails.back.pattern = data.TireBack.PatternLeft;

                    $scope.dataLoadMode = true;


                    goToStep(2);

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });;
            } else {
                customErrorMessage("Invalid invoice code selection.");
            }
        }

        $scope.currentStep = 1;
        $scope.form = {

            next: function (form) {

                $scope.toTheTop();
                if (form.$valid) {
                    nextStep();
                }
            },
            prev: function (form) {
                $scope.toTheTop();
                prevStep();
            },
            goTo: function (form, i) {

                if (parseInt($scope.currentStep) < parseInt(i)) {
                    var isValidated = false;
                    if ($scope.dealerInvoiceDetails.quantity > 0) {
                        isValidated = true
                    } else {
                        customErrorMessage("Please select tire details.");
                    }


                    // check front and back two tires are selected
                    if ($scope.isFrontTireDetailsVisibleL && $scope.isBackTireDetailsVisibleL) {
                        isValidated = $scope.validateFrontTyre();
                        if (isValidated) {
                            isValidated = $scope.validateRearTyre();
                        }

                        if ($scope.dealerInvoiceTireDetails.front.serialLeft === $scope.dealerInvoiceTireDetails.back.serialLeft) {
                            isValidated = false;
                            customErrorMessage("Unable to Proceed with Same Article Number For Both Tyres");
                        }

                    }
                    if ($scope.isFrontTireDetailsVisibleL && !$scope.isBackTireDetailsVisibleL) {
                        isValidated = $scope.validateFrontTyre();
                    }
                    if (!$scope.isFrontTireDetailsVisibleL && $scope.isBackTireDetailsVisibleL) {
                        isValidated =  $scope.validateRearTyre();
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
            $scope.generateTyreDetails();
            if ($scope.currentStep == 2) {
                $scope.purchaseDate = $filter('date')(new Date(), 'dd-MMM-yyyy');
                $scope.loadContractDetails();
            }

        };




        //$scope.Make.Id = '';
        $scope.setModel = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                data: { "Id": $scope.MakeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var temp = data;
                angular.forEach(temp, function (value) {
                    $scope.Modeles.push(value);
                    AddModel();
                    LoadModel();
                });
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false,
            closeOnSelect: true,
            selectionLimit: 1,
            buttonClasses: "drop-btn btn btn-default",
            smartButtonMaxItems: 1,
            smartButtonTextConverter: function (itemText, originalItem) {
                return itemText;
            }
        };



        $scope.CustomText = {
            buttonDefaultText: ' Please Select ',
            dynamicButtonTextSuffix: ' Item(s) Selected'
            //smartButtonTextProvider: ''
            //dynamicButtonTextSuffix: ' Item(s) Selected'
            //dynamicButtonTextSuffix : { smartButtonTextProvider(selectionArray) { return selectionArray.lenth },
        };
        $scope.Modeles = [];

        // Make drop down

        function AddMake() {
            var index = 0;
            $scope.MakeList = [];
            angular.forEach($scope.MakesVehi, function (value) {
                var x = { id: index, code: value.Id, label: value.MakeName };
                $scope.MakeList.push(x);
                index = index + 1;
            });
        }
        function LoadMake() {
            $scope.SelectedMakeList = [];
            $scope.SelectedMakeDList = [];
            angular.forEach($scope.product.Makes, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedMakeList.push(x);
                        $scope.SelectedMakeDList.push(valueIn.label);
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": valueOut },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var temp = data;
                    angular.forEach(temp, function (value) {
                        $scope.Modeles.push(value);
                        AddModel();
                        LoadModel();
                    });
                }).error(function (data, status, headers, config) {
                });
            });
        }
        $scope.SendMake = function () {
            $scope.Modeles = [];
            $scope.SelectedMakeDList = [];
            $scope.Country.Makes = [];
            angular.forEach($scope.SelectedMakeList, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Country.Makes.push(valueIn.code);
                        $scope.SelectedMakeDList.push(valueIn.label);
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": valueIn.code },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var temp = data;
                            angular.forEach(temp, function (value) {
                                $scope.Modeles.push(value);
                                AddModel();
                            });
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            });
        }

        // --- Model Dropdown

        function AddModel() {
            var index = 0;
            $scope.ModelList = [];
            angular.forEach($scope.Modeles, function (value) {
                var x = { id: index, code: value.Id, label: value.ModelName };
                $scope.ModelList.push(x);
                index = index + 1;
            });
        }
        function LoadModel() {
            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];
            angular.forEach($scope.Country.Modeles, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedModelList.push(x);
                        $scope.SelectedModelDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendModel = function () {
            $scope.SelectedModelDList = [];
            $scope.Country.Modeles = [];
            angular.forEach($scope.SelectedModelList, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Country.Modeles.push(valueIn.code);
                        $scope.SelectedModelDList.push(valueIn.label);
                    }
                });
            });
        }

        // --- Model year dropdown

        function AddModelYear() {
            var index = 0;
            $scope.ModelYearList = [];
            angular.forEach($scope.ModelYear, function (value) {
                var x = { id: index, code: value.Id, label: value.ModelYear };
                $scope.ModelYearList.push(x);
                index = index + 1;
            });
        }


        $scope.selectedAditionalMakeChange = function () {
            $scope.filterdSelectedModelList = [];
            if (isGuid($scope.MakeId)) {
                for (var i = 0; i < $scope.additionalPolicyDetailsModel.length; i++) {
                    if ($scope.additionalPolicyDetailsModel[i].makeId === $scope.MakeId) {
                        var model_d = {
                            id: $scope.additionalPolicyDetailsModel[i].id,
                            label: $scope.additionalPolicyDetailsModel[i].label,
                            makeId: $scope.additionalPolicyDetailsModel[i].makeId
                        }
                        $scope.filterdSelectedModelList.push(model_d);
                    }
                }
            }

            angular.forEach($scope.additionalPolicyDetailsMake, function (value) {
                if (value.id === $scope.additionalSelectedMakeList.id) {
                    $scope.CustomText.buttonDefaultText = value.label;

                }

            });


        }

        $scope.selectedCustomerTypeIdChanged = function () {
            if (parseInt($scope.customer.customerTypeId)) {
                angular.forEach($scope.customerTypes, function (value) {
                    if ($scope.customer.customerTypeId == value.Id) {
                        $scope.selectedCustomerTypeName = value.CustomerTypeName;
                        return false;
                    }
                });
            }
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

                            angular.forEach($scope.commodityUsageTypes, function (valuec) {
                                if (valuec.Name == "Commercial") {
                                    $scope.product.commodityUsageTypeId = valuec.Id;

                                }
                            });
                        } else if (value.CustomerTypeName == "Individual") {
                            angular.forEach($scope.usageTypes, function (valuec) {
                                if (valuec.UsageTypeName == "Private") {
                                    $scope.customer.usageTypeId = valuec.Id;

                                }
                            });

                        }
                    }
                });
            }

            //} else {
            //    $scope.selectedCustomerTypeName = '';
            //}
        }

        $scope.extPolicyRegAttachmentType = emptyGuid();

        $scope.navigateToBuyingProcess = function () {
            if ($scope.availableTireList.length === 0) {
                customErrorMessage("No tire details found.");
                return;
            }

            if ($scope.customerInvoiceUploader.queue.length === 0) {
                if (!isGuid($scope.product.invoiceAttachmentId)) {
                    customErrorMessage("Please attach invoice to upload.");
                    return;
                }
            }


            if ($scope.validateInvoiceData()) {
                if ($scope.customerInvoiceUploader.queue.length !== 0) {
                    $scope.customerInvoiceUploader.uploadAll();
                } else {
                    $scope.savePolicyDetails();
                }

            } else
                customErrorMessage("Please fill all highlighted fields.");
        }

        // load contract details
        $scope.loadContractDetails = function () {
            if ($localStorage.CommodityType === "Tyre") {

                $scope.isTowContract = false;
                $scope.contractOne = "Contract";
                $scope.contractOneData = [];
                $scope.contractTwoData = [];
                swal({ title: 'Loading...', text: 'Loading Contract Details', showConfirmButton: false });


                $scope.generateTyreDetails();
              //  console.log($scope.tyreDetailsValuesForSave);
                var tyreData = [];
                angular.forEach($scope.tyreDetailsValuesForSave, function (tt) {
                    tyreData.push({ 'diameter': tt.tyres[0].diameter, 'quantity': tt.TireQuantity, 'position': tt.Position });
                });

                var data = {
                    'dealerId': $scope.dealerInvoiceDetails.dealerId,
                    'countryId': $scope.dealerInvoiceDetails.countryId,
                    'CommodityUsageTypeId': $scope.customer.commodityUsageType,
                    'CommodityTypeId': $scope.product.CommodityTypeId,
                    'purchaseDate': $scope.purchaseDate,
                    'tyreData': tyreData
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetTyreContractDetails',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.length > 1) {
                        $scope.isTowContract = true;
                        $scope.contractOne = "Contract One";

                        var frontData = data.filter(a => a.position == "F");
                        var rearData = data.filter(a => a.position == "B");
                        $scope.contractOneData = frontData[0].contract;
                        $scope.contractTwoData = rearData[0].contract;

                        if (data[0].contract.length == 0 || data[1].contract.length == 0) {
                            swal("TAS Validation", "Contract not Found", "error");
                        } else {
                            swal.close();
                        }
                    } else if (data.length == 1) {
                        $scope.contractOneData = data[0].contract;
                        if (data[0].contract.length == 0) {
                            swal("TAS Validation", "Contract not Found", "error");
                            return false;
                        } else {
                            swal.close();
                        }
                    } else {
                        swal.close();
                    }
                    $scope.selectDefaultContractDetails();

                }).error(function (data, status, headers, config) {
                    swal.close();
                }).finally(function () {

                });
            }
        }

        $scope.selectDefaultContractDetails = function () {
            $scope.contractTowModal = "";
            $scope.premiumTwoModal = "";
            $scope.contractOneModal = "";
            $scope.premiumOneModal = "";
            $scope.ContractExtensionsIdOne = "";
            $scope.ContractExtensionsIdTwo = "";
            if ($scope.contractOneData.length ==1) {
                $scope.premiumOneModal = $scope.contractOneData[0].ContractExtensionPremiumId;
                $scope.ContractExtensionsIdOne = $scope.contractOneData[0].ContractExtensionsId;
            }
           if ($scope.contractTwoData.length == 1) {
               $scope.premiumTwoModal = $scope.contractTwoData[0].ContractExtensionPremiumId;
               $scope.ContractExtensionsIdTwo = $scope.contractOneData[0].ContractExtensionsId;
            }
            $scope.selectContractOtherDetails();

        }

        $scope.selectContractOtherDetails = function () {
            $scope.grossOne = "";
            $scope.nrpOne = "";
            $scope.expiryDateOne = "";
            $scope.expiryDateTwo = "";
            $scope.grossTwo = "";
            $scope.nrpTow = "";
            if ($scope.premiumOneModal != "") {
                var contract = $scope.contractOneData.filter(ct => ct.ContractExtensionPremiumId == $scope.premiumOneModal);
                $scope.expiryDateOne = $filter('date')(contract[0].ContractEndDate, 'dd-MMM-yyyy');
                $scope.grossOne = contract[0].Gross;
                $scope.nrpOne = contract[0].NRP;
                $scope.contractOneModal = contract[0].ContractId;
                $scope.ContractExtensionsIdOne = contract[0].ContractExtensionsId;

            }
            if ($scope.premiumTwoModal != "") {
                var contract = $scope.contractTwoData.filter(ct => ct.ContractExtensionPremiumId == $scope.premiumTwoModal);
                $scope.expiryDateTwo = $filter('date')(contract[0].ContractEndDate, 'dd-MMM-yyyy');
                $scope.grossTwo = contract[0].Gross;
                $scope.nrpTow = contract[0].NRP;
                $scope.contractTowModal = contract[0].ContractId;
                $scope.ContractExtensionsIdTwo = contract[0].ContractExtensionsId;
            }

        }

    });



app.directive('customPopover', function ($compile, $templateCache) {
    var getTemplate = function (contentType) {
        var template = '';
        switch (contentType) {
            case 'contentType1':
                template = $templateCache.get("customPopoverContent1.html");
                break;
            case 'contentType2':
                template = $templateCache.get("customPopoverContent2.html");
                break;
        }
        return template;
    };

    return {
        restrict: "A",
        scope: {
            contentType: '@',
            title: '@',

        },
        link: function (scope, element, attrs) {
            var contentType = scope.contentType;
            var popOverContent = getTemplate(contentType);
            popOverContent = $compile("<div>" + popOverContent + "</div>")(scope);

            var options = {

                content: popOverContent,
                placement: "left",
                html: true,
                date: scope.date,
                trigger: "click"
            };
            $(element).popover(options);

        }
    };
})


app.config(['$tooltipProvider', function ($tooltipProvider) {
    $tooltipProvider.options({
        appendToBody: true, //
        placement: 'bottom' // Set Default Placement
    });
}]);


app.directive('resize', function ($window) {
    return function (scope, element) {
        scope.appendToBody = true;
        var w = angular.element($window);
        scope.getWindowDimensions = function () {
            return {
                'h': w.height(),
                'w': w.width()
            };
        };
        scope.$watch(scope.getWindowDimensions, function (newValue, oldValue) {
            scope.windowHeight = newValue.h;
            scope.windowWidth = newValue.w;

            scope.appendToBody = (scope.windowWidth > 768) ? true : false;

            scope.style = function () {
                return {
                    'height': (newValue.h - 100) + 'px',
                    'width': (newValue.w - 100) + 'px'
                };
            };

        }, true);

        w.bind('resize', function () {
            scope.$apply();
        });
    }
})