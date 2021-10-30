app.controller('MakeAndModelManagementCtrl',
    function ($modal, $scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, Idle, Keepalive, $filter, toaster, $interval, $translate) {
        $scope.ModalName = "Make And Model Management";
        $scope.ModalDescription = "Add Edit Make And Model Information";

        $scope.labelSaveMake = 'pages.makeAndModelManagement.saveMake';
        $scope.labelSaveModel = 'pages.makeAndModelManagement.saveModel';
        $scope.labelSaveVarient = 'pages.makeAndModelManagement.saveVarient';


        $scope.MakeSubmitBtnIconClass = "";
        $scope.MakeSubmitBtnDisabled = false;

        $scope.ModelSubmitBtnIconClass = "";
        $scope.ModelSubmitBtnDisabled = false;

        $scope.VariantSubmitBtnIconClass = "";
        $scope.VariantSubmitBtnDisabled = false;

        $scope.ManufacturerWarrantySubmitBtnIconClass = "";
        $scope.ManufacturerWarrantySubmitBtnDisabled = false;

        $scope.gridManufacturerWarrantyloading = false;
        $scope.gridManufacturerWarrantyloadAttempted = true;

        $scope.disableWarrantyKm = false;

        $scope.selectedpp = {};
        $scope.settings = {
            scrollableHeight: '150px',
            scrollable: true,
            closeOnBlur: true,
            closeOnSelect: true
        };
        $scope.CustomText = { 
            buttonDefaultText: $filter('translate')('common.customText.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('common.customText.itemSelected')
        };

        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };
        var customWarningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('common.popUpMessages.warning'), msg, 12000);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', $filter('translate')('common.popUpMessages.information'), msg, 12000);
        };

        $scope.ManufacturerWarranty = {
            Id: "00000000-0000-0000-0000-000000000000",
            ApplicableFrom: '',
            WarrantyName: '',
            WarrantyMonths: '',
            WarrantyKm: 0,
            MakeId: "00000000-0000-0000-0000-000000000000",
            Models: [],
            Countrys: [],
            Expired: [],
            IsUnlimited: false

        };
        $scope.ManufacturerWarranty.CommodityTypeId = "00000000-0000-0000-0000-000000000000";
        var paginationOptionsManufacturerWarrentySearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.loadInitailData = function () { }
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";
        $scope.errorTab3 = "";
        $scope.errorTab4 = "";
        function LoadDetails() {
            getPolicySearchPage();
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
                url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.EngineCapacities = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
            }).error(function (data, status, headers, config) {
            });
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/MakeAndModelManagement/GetAllManufacturers'
            //}).success(function (data, status, headers, config) {
            //    $scope.Manufacturers = data;
            //}).error(function (data, status, headers, config) {
            //});
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ModelMakes = data;
                $scope.ManufacturerWarrantyMakes = data;
                AddModel();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                //url: '/TAS.Web/api/Customer/GetAllCountries',
                url: '/TAS.Web/api/Country/GetAllActiveCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
                AddCountry();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Transmissions = data;
                AddTransmission();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Aspirations = data;
                AddAspiration();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllFuelTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.FuelTypes = data;
                AddFuelType();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleBodyTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.BodyTypes = data;
                AddBodyType();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllDriveTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DriveTypes = data;
                AddDriveType();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllPremiumAddonType',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.PremiumAddonType = data;
                AddPremiumAddonType();
            }).error(function (data, status, headers, config) {
            });
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/VariantManagement/GetAllVariant'
            //}).success(function (data, status, headers, config) {
            //    $scope.Variants = data;
            //}).error(function (data, status, headers, config) {
            //});
            $scope.Categories = [];
        }
        $scope.refreshMakes = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ModelMakes = data;
                $scope.ManufacturerWarrantyMakes = data;
            }).error(function (data, status, headers, config) {
            });
        }

        LoadDetails();
        //-------------------Make-----------------//
        $scope.Make = {
            Id: "00000000-0000-0000-0000-000000000000",
            MakeCode: '',
            MakeName: '',
            ManufacturerId: '',
            CommodityTypeId: '',
            WarantyGiven: false,
            IsActive: false
        };
        $scope.SetManufacturers = function () {
            $scope.Manufacturers = [];
            $scope.Make.ManufacturerId = "";
            $scope.errorTab1 = "";
            if ($scope.Make.CommodityTypeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Manufacturer/GetAllManufacturersByCommodityTypeId',
                    data: { "Id": $scope.Make.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Manufacturers = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetMake = function () {
            $scope.errorTab1 = "";
            if ($scope.Make.ManufacturerId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByManufacturerId',
                    data: { "Id": $scope.Make.ManufacturerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Makes = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetMakeValues = function () {
            $scope.errorTab1 = "";
            if ($scope.Make.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetMakeById',
                    data: { "Id": $scope.Make.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Make.Id = data.Id;
                    $scope.Make.MakeCode = data.MakeCode;
                    $scope.Make.MakeName = data.MakeName;
                    //$scope.Make.ManufacturerId = data.ManufacturerId;
                    //$scope.Make.CommodityTypeId = data.CommodityTypeId;
                    $scope.Make.WarantyGiven = data.WarantyGiven;                    
                    $scope.labelSaveMake = 'pages.makeAndModelManagement.updateMake';
                    $scope.Make.IsActive = data.IsActive;
                }).error(function (data, status, headers, config) {
                    clearMakeControls();
                });
            }
            else {
                clearMakeControls();
            }
        }
        function clearMakeControls() {
            $scope.Make.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Make.MakeCode = "";
            $scope.Make.MakeName = "";

            $scope.Make.WarantyGiven = false;
            $scope.Make.IsActive = false;
            $scope.Makes = [];

        }

        $scope.validateMake = function () {
            var isValid = true;
            if ($scope.Make.MakeName == null || $scope.Make.MakeName == ""
                || $scope.Make.MakeName == undefined) {
                $scope.validate_MakeName = "has-error";
                isValid = false;
            } else {
                $scope.validate_MakeName = "";
            }
            if ($scope.Make.CommodityTypeId == null || $scope.Make.CommodityTypeId == ""
                || $scope.Make.CommodityTypeId == undefined) {
                $scope.validate_CommodityTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityTypeId = "";
            }
            if ($scope.Make.ManufacturerId == null || $scope.Make.ManufacturerId == ""
                || $scope.Make.ManufacturerId == undefined) {
                $scope.validate_ManufacturerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ManufacturerId = "";
            }
            if ($scope.Make.MakeCode == null || $scope.Make.MakeCode == ""
                || $scope.Make.MakeCode == undefined) {
                $scope.validate_MakeCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_MakeCode = "";
            }

            return isValid
        }

        $scope.MakeSubmit = function () {
            if ($scope.validateMake()) {

                $scope.errorTab1 = "";
                if ($scope.Make.Id == null || $scope.Make.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.MakeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.MakeSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/MakeAndModelManagement/AddMake',
                        data: $scope.Make,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.MakeSubmitBtnIconClass = "";
                        $scope.MakeSubmitBtnDisabled = false;
                        if (data.MakeInsertion) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            clearMakeControls();

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByManufacturerId',
                                data: { "Id": $scope.Make.ManufacturerId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Makes = data;
                                //$scope.ManufacturerWarrantyMakes = data;
                                //$scope.ModelMakes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            $scope.refreshMakes();
                        } else if (data.IsMakeExists) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                                text: $filter('translate')('pages.makeAndModelManagement.errorMessages.makeAlreadyExits'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            return false;
                        } else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.MakeSubmitBtnIconClass = "";
                            $scope.MakeSubmitBtnDisabled = false;
                            return false;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.MakeSubmitBtnIconClass = "";
                        $scope.MakeSubmitBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.MakeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.MakeSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/MakeAndModelManagement/UpdateMake',
                        data: $scope.Make,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.MakeSubmitBtnIconClass = "";
                        $scope.MakeSubmitBtnDisabled = false;
                        if (data == "OK") {
                            $scope.labelSaveMake = 'pages.makeAndModelManagement.saveMake';                            
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });



                            clearMakeControls();
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByManufacturerId',
                                data: { "Id": $scope.Make.ManufacturerId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Makes = data;
                                //$scope.ManufacturerWarrantyMakes = data;
                                //$scope.ModelMakes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            $scope.refreshMakes();

                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.makeInforMation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.MakeSubmitBtnIconClass = "";
                        $scope.MakeSubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }
        //----------------Model-----------------//
        $scope.Model = {
            Id: "00000000-0000-0000-0000-000000000000",
            ModelCode: '',
            ModelName: '',
            MakeId: '',
            CategoryId: '',
            NoOfDaysToRiskStart: '',
            WarantyGiven: '',
            IsActive: '',
            ContryOfOrigineId: '',
            AdditionalPremium: ''
        };
        $scope.SetCategory = function () {
            $scope.Model.CategoryId = "";
            $scope.Categories = [];
            $scope.errorTab2 = "";
            if ($scope.Model.MakeId != null) {
                var m = $scope.Model.MakeId;
                $scope.Model = {
                    Id: "00000000-0000-0000-0000-000000000000",
                    ModelCode: '',
                    ModelName: '',
                    MakeId: '',
                    CategoryId: '',
                    //RiskStartDate: '',
                    NoOfDaysToRiskStart: '',
                    WarantyGiven: false,
                    IsActive: false,
                    ContryOfOrigineId: '',
                    AdditionalPremium: false
                };
                $scope.Model.MakeId = m;
                $scope.CategoryDisable = false;
                angular.forEach($scope.ModelMakes, function (value, key) {
                    if (value.Id == $scope.Model.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                            data: { "Id": value.CommodityTypeId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Categories = data;
                        }).error(function (data, status, headers, config) {
                        });                        
                    }
                });
            }
        }
        $scope.SetModelValues = function () {
            $scope.errorTab3 = "";
            if ($scope.Model.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelById',
                    data: { "Id": $scope.Model.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Model.Id = data.Id;
                    $scope.Model.ModelCode = data.ModelCode;
                    $scope.Model.ModelName = data.ModelName;
                    $scope.Model.MakeId = data.MakeId;
                    $scope.Model.CategoryId = data.CategoryId;
                    //$scope.Model.RiskStartDate = data.RiskStartDate;
                    $scope.Model.NoOfDaysToRiskStart = data.NoOfDaysToRiskStart;
                    $scope.Model.WarantyGiven = data.WarantyGiven;
                    $scope.Model.IsActive = data.IsActive;
                    $scope.Model.ContryOfOrigineId = data.ContryOfOrigineId;
                    $scope.Model.EntryDateTime = data.EntryDateTime;
                    $scope.Model.EntryUser = data.EntryUser;
                    $scope.Model.AdditionalPremium = data.AdditionalPremium;                      
                    $scope.labelSaveModel = 'pages.makeAndModelManagement.updateModel';
                }).error(function (data, status, headers, config) {
                    clearModelControls();
                    $scope.SetCategory();
                });
            }
            else {
                clearModelControls();
                $scope.SetCategory();
            }
        }
        function clearModelControls() {
            $scope.Model.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Model.ModelName = "";
            $scope.Model.ModelCode = "";
            $scope.Model.ContryOfOrigineId = "";
            //$scope.Model.RiskStartDate = "";
            $scope.Model.NoOfDaysToRiskStart = "";
            $scope.Model.WarantyGiven = false;
            $scope.Model.AdditionalPremium = false;
            $scope.Model.IsActive = false;
            $scope.Modeles = [];
        }

        $scope.validateModel = function () {
            var isValid = true;

            if ($scope.Model.MakeId == "" || $scope.Model.MakeId == "00000000-0000-0000-0000-000000000000" || $scope.Model.MakeId == undefined || $scope.Model.MakeId == null) {
                $scope.validate_ModelMakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelMakeId = "";
            }

            if ($scope.Model.CategoryId == "" || $scope.Model.CategoryId == "00000000-0000-0000-0000-000000000000" || $scope.Model.CategoryId == undefined || $scope.Model.CategoryId == null) {
                $scope.validate_CategoryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CategoryId = "";
            }

            if ($scope.Model.ModelName == "" || $scope.Model.ModelName == undefined || $scope.Model.ModelName == null) {
                $scope.validate_ModelName = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelName = "";
            }
            if ($scope.Model.ModelCode == "" || $scope.Model.ModelCode == undefined
                || $scope.Model.ModelCode == null) {
                $scope.validate_ModelCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelCode = "";
            }
            if ($scope.Model.ContryOfOrigineId == "" || $scope.Model.ContryOfOrigineId == undefined
                || $scope.Model.ContryOfOrigineId == null) {
                $scope.validate_ContryOfOrigineId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ContryOfOrigineId = "";
            }
            if ($scope.Model.NoOfDaysToRiskStart == "" || $scope.Model.NoOfDaysToRiskStart == undefined
                || $scope.Model.NoOfDaysToRiskStart == null) {
                $scope.validate_NoOfDaysToRiskStart = "has-error";
                isValid = false;
            } else {
                $scope.validate_NoOfDaysToRiskStart = "";
            }
            return isValid
        }

        $scope.ModelSubmit = function () {
            if ($scope.validateModel()) {


                $scope.errorTab2 = "";
                if ($scope.Model.Id == null || $scope.Model.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.ModelSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ModelSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/MakeAndModelManagement/AddModel',
                        data: $scope.Model,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ModelSubmitBtnIconClass = "";
                        $scope.ModelSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.modelInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                                data: { "Id": $scope.Model.MakeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Modeles = data;
                                // $scope.ManufacturerWarrantyModeles = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearModelControls();
                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.modelInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                        $scope.ModelSubmitBtnIconClass = "";
                        $scope.ModelSubmitBtnDisabled = false;
                    });

                }
                else {
                    $scope.ModelSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ModelSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/MakeAndModelManagement/UpdateModel',
                        data: $scope.Model,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ModelSubmitBtnIconClass = "";
                        $scope.ModelSubmitBtnDisabled = false;
                        if (data == "OK") {
                            $scope.labelSaveModel = 'pages.makeAndModelManagement.saveModel';
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.modelInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                                data: { "Id": $scope.Model.MakeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Modeles = data;
                                //  $scope.ManufacturerWarrantyModeles = data;
                            }).error(function (data, status, headers, config) {
                            });

                            clearModelControls();

                        } else {
                            ;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.modelInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                        $scope.ModelSubmitBtnIconClass = "";
                        $scope.ModelSubmitBtnDisabled = false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }
        //----------------Variant-----------------//
        $scope.Variant = {
            Id: "00000000-0000-0000-0000-000000000000",
            MakeId: '',
            ModelId: '',
            CommodityTypeId: '',
            VariantName: '',
            FromModelYear: '',
            ToModelYear: '',
            EngineCapacityId: "00000000-0000-0000-0000-000000000000",
            CylinderCountId: "00000000-0000-0000-0000-000000000000",
            BodyCode: '',
            BodyTypes: [],
            Countrys: [],
            FuelTypes: [],
            Aspirations: [],
            Transmissions: [],
            DriveTypes: [],
            PremiumAddonType: [],
            GrossWeight: 0,
            IsActive: false,
            IsSports: false,
            IsForuByFour: false
        };
        $scope.IsAutomobile = true;
        $scope.SetVariantMake = function () {
            $scope.Variant.ModelId = "";
            $scope.Variant.MakeId = "";
            $scope.VariantMakes = [];
            $scope.VariantModeles = [];
            $scope.Variants = [];
            $scope.errorTab3 = "";
            if ($scope.Variant.CommodityTypeId != null) {
                $scope.IsAutomobile = false;
                angular.forEach($scope.CommodityTypes, function (value) {
                    if (value.CommodityTypeDescription == 'Automobile' && $scope.Variant.CommodityTypeId == value.CommodityTypeId) {
                        $scope.IsAutomobile = true;
                    } else if (value.CommodityTypeDescription == 'Automotive' && $scope.Variant.CommodityTypeId == value.CommodityTypeId) {
                        $scope.IsAutomobile = true;
                    } else if (value.CommodityTypeDescription == 'Bank' && $scope.Variant.CommodityTypeId == value.CommodityTypeId) {
                        $scope.IsAutomobile = true;
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                    data: { "Id": $scope.Variant.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VariantMakes = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetModel = function () {
            if ($scope.Variant.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.Variant.MakeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VariantModeles = data;
                    clearVariantControls();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetVariant = function () {
            if ($scope.Variant.ModelId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                    data: { "Id": $scope.Variant.ModelId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Variants = data;
                    clearVariantControls();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetVariantValues = function () {
            $scope.errorTab3 = "";
            if ($scope.Variant.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VariantManagement/GetVariantById',
                    data: { "Id": $scope.Variant.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Variant.Id = data.Id;
                    $scope.Variant.CommodityTypeId = data.CommodityTypeId;
                    // $scope.SetVariantMake();
                    var m = data.ModelId;
                    $scope.Variant.VariantName = data.VariantName;
                    $scope.Variant.FromModelYear = data.FromModelYear;
                    $scope.Variant.ToModelYear = data.ToModelYear;
                    $scope.Variant.EngineCapacityId = data.EngineCapacityId;
                    $scope.Variant.CylinderCountId = data.CylinderCountId;
                    $scope.Variant.BodyCode = data.BodyCode;
                    $scope.Variant.BodyTypes = data.BodyTypes;
                    $scope.Variant.Countrys = data.Countrys;
                    $scope.Variant.FuelTypes = data.FuelTypes;
                    $scope.Variant.Aspirations = data.Aspirations;
                    $scope.Variant.Transmissions = data.Transmissions;
                    $scope.Variant.DriveTypes = data.DriveTypes;
                    $scope.Variant.PremiumAddonType = data.PremiumAddonType;
                    $scope.Variant.GrossWeight = data.GrossWeight;
                    $scope.Variant.IsActive = data.IsActive;
                    $scope.Variant.IsSports = data.IsSports;
                    $scope.Variant.IsForuByFour = data.IsForuByFour;
                    $scope.labelSaveVarient = 'pages.makeAndModelManagement.updateVarient';
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/MakeAndModelManagement/GetMakeIdByModelId',
                        data: { "Id": m },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Variant.MakeId = data;
                        //  $scope.SetModel();
                        $scope.Variant.ModelId = m;
                    }).error(function (data, status, headers, config) {
                    });
                    LoadAspiration();
                    LoadDriveType();
                    LoadFuelType();
                    LoadTransmission();
                    LoadCountry();
                    LoadBodyType();
                    LoadPremiumAddonType();


                }).error(function (data, status, headers, config) {
                    clearVariantControls();
                });
            }
            else {
                clearVariantControls();
            }
        }
        function clearVariantControls() {
            $scope.Variant.Id = "00000000-0000-0000-0000-000000000000";

            $scope.Variant.VariantName = "";
            $scope.Variant.FromModelYear = "";
            $scope.Variant.ToModelYear = "";
            $scope.Variant.EngineCapacityId = "00000000-0000-0000-0000-000000000000";
            $scope.Variant.CylinderCountId = "00000000-0000-0000-0000-000000000000";
            $scope.Variant.BodyCode = "";
            $scope.Variant.BodyTypes = [];
            $scope.Variant.Countrys = [];
            $scope.Variant.FuelTypes = [];
            $scope.Variant.Aspirations = [];
            $scope.Variant.Transmissions = [];
            $scope.Variant.DriveTypes = [];
            $scope.Variant.PremiumAddonType = [];
            $scope.Variant.IsActive = false;
            $scope.Variant.IsSports = false;
            $scope.Variant.IsForuByFour = false;
            $scope.Variant.GrossWeight = 0;
            $scope.SelectedAspirationList = [];
            $scope.SelectedBodyTypeList = [];
            $scope.SelectedCountryList = [];
            $scope.SelectedTransmissionList = [];
            $scope.SelectedDriveTypeList = [];
            $scope.SelectedFuelTypeList = [];
            $scope.SelectedPremiumAddonTypeList = [];

            $scope.AspirationDList = [];
            $scope.BodyTypeDList = [];
            $scope.CountryDList = [];
            $scope.TransmissionDList = [];
            $scope.DriveTypeDList = [];
            $scope.FuelTypeDList = [];
            $scope.PremiumAddonTypeDList = [];


        }


        $scope.validateVariant = function () {
            var isValid = true;
            if ($scope.IsAutomobile) {
                if ($scope.Variant.VariantName == "" || $scope.Variant.VariantName == undefined
                    || $scope.Variant.VariantName == null) {
                    $scope.validate_VariantName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantName = "";
                }

                if (!isGuid($scope.Variant.Id)) {
                    angular.forEach($scope.Variants, function (variant) {
                        if (variant.VariantName.trim() === $scope.Variant.VariantName.trim()) {
                            $scope.validate_VariantName = "has-error";
                            isValid = false;
                            customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.variantAlreadyExits'))
                            return false;
                        }
                    });
                }

                if ($scope.Variant.CommodityTypeId == emptyGuid() || $scope.Variant.CommodityTypeId == "" || $scope.Variant.CommodityTypeId == undefined
                    || $scope.Variant.CommodityTypeId == null) {
                    $scope.validate_VariantCommodityTypeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantCommodityTypeId = "";
                }

                if ($scope.Variant.MakeId == emptyGuid() || $scope.Variant.MakeId == "" || $scope.Variant.MakeId == undefined
                    || $scope.Variant.MakeId == null) {
                    $scope.validate_VariantMakeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantMakeId = "";
                }

                if ($scope.Variant.ModelId == emptyGuid() || $scope.Variant.ModelId == "" || $scope.Variant.ModelId == undefined
                    || $scope.Variant.ModelId == null) {
                    $scope.validate_VariantModelId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantModelId = "";
                }

                if ($scope.Variant.EngineCapacityId == emptyGuid() || $scope.Variant.EngineCapacityId == "" || $scope.Variant.EngineCapacityId == undefined
                    || $scope.Variant.EngineCapacityId == null) {
                    $scope.validate_VariantEngineCapacityId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantEngineCapacityId = "";
                }
                if ($scope.Variant.CylinderCountId == emptyGuid() || $scope.Variant.CylinderCountId == "" || $scope.Variant.CylinderCountId == undefined
                    || $scope.Variant.CylinderCountId == null) {
                    $scope.validate_VariantCylinderCountId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantCylinderCountId = "";
                }
                if ($scope.Variant.BodyCode == "" || $scope.Variant.BodyCode == undefined
                    || $scope.Variant.BodyCode == null) {
                    $scope.validate_VariantBodyCode = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantBodyCode = "";
                }

                if ($scope.Variant.FromModelYear == "" || $scope.Variant.FromModelYear == undefined
                    || $scope.Variant.FromModelYear == null) {
                    $scope.validate_VariantFromModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantFromModelYear = "";
                }

                if ($scope.Variant.GrossWeight == "" || $scope.Variant.GrossWeight == undefined
                    || $scope.Variant.GrossWeight == null) {
                    $scope.validate_VariantGrossWeight = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantGrossWeight = "";
                }

                if ($scope.Variant.ToModelYear == "" || $scope.Variant.ToModelYear == undefined
                    || $scope.Variant.ToModelYear == null) {
                    $scope.validate_VariantToModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantToModelYear = "";
                }
            } else if (!$scope.IsAutomobile) {
                if ($scope.Variant.VariantName == "" || $scope.Variant.VariantName == undefined
                    || $scope.Variant.VariantName == null) {
                    $scope.validate_VariantName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantName = "";
                }

                if ($scope.Variant.FromModelYear == "" || $scope.Variant.FromModelYear == undefined
                    || $scope.Variant.FromModelYear == null) {
                    $scope.validate_VariantFromModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantFromModelYear = "";
                }

                if ($scope.Variant.ToModelYear == "" || $scope.Variant.ToModelYear == undefined
                    || $scope.Variant.ToModelYear == null) {
                    $scope.validate_VariantToModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VariantToModelYear = "";
                }
            }

            return isValid
        }

        $scope.validateVariant2 = function () {
            var isValid = true;

            return isValid
        }

        $scope.VariantSubmit = function () {
            $scope.SendAspiration();
            $scope.SendBodyType();
            $scope.SendCountry();
            $scope.SendDriveType();
            $scope.SendFuelType();
            $scope.SendTransmission();
            $scope.SendPremiumAddonType();

            if ($scope.validateVariant()) {
                //if ($scope.validateVariant()) {

                //} else {
                //    customErrorMessage("Please fill valid data for highlighted fields.")
                //}

                var d = new Date();
                var n = d.getFullYear();
                //if ($scope.Variant.FromModelYear == undefined || $scope.Variant.FromModelYear == null || $scope.Variant.FromModelYear == "") {
                //    $scope.Variant.FromModelYear = null;

                //} else if (!($scope.Variant.FromModelYear >= 1900 && $scope.Variant.FromModelYear <= n)) {
                //    customErrorMessage( "Enter From Year As 2019 or beyond");
                //    return false;
                //}


                if ($scope.Variant.ToModelYear == undefined || $scope.Variant.ToModelYear == null || $scope.Variant.ToModelYear == "") {
                    $scope.Variant.ToModelYear = null;
                }
                else if ($scope.Variant.FromModelYear > n) {
                    customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.currentYear'));
                    return false;
                }
                if ($scope.Variant.FromModelYear == undefined || $scope.Variant.FromModelYear == null || $scope.Variant.FromModelYear == "" ||
                    $scope.Variant.ToModelYear == undefined || $scope.Variant.ToModelYear == null || $scope.Variant.ToModelYear == "") {


                }
                else if ($scope.Variant.ToModelYear < $scope.Variant.FromModelYear) {
                    customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.largerYear'));
                    return false;
                }


                $scope.errorTab3 = "";
                if ($scope.Variant.Id == null || $scope.Variant.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.VariantSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.VariantSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/VariantManagement/AddVariant',
                        data: $scope.Variant,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.VariantSubmitBtnIconClass = "";
                        $scope.VariantSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.variantInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/VariantManagement/GetAllVariant'
                            //}).success(function (data, status, headers, config) {
                            //    $scope.Variants = data;
                            //}).error(function (data, status, headers, config) {
                            //});
                            clearVariantControls();
                            $scope.SetVariant();

                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.variantInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.VariantSubmitBtnIconClass = "";
                        $scope.VariantSubmitBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.VariantSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.VariantSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/VariantManagement/UpdateVariant',
                        data: $scope.Variant,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.VariantSubmitBtnIconClass = "";
                        $scope.VariantSubmitBtnDisabled = false;
                        if (data == "OK") {
                            $scope.labelSaveVarient = 'pages.makeAndModelManagement.updateVarient';
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.variantInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/VariantManagement/GetAllVariant'
                            //}).success(function (data, status, headers, config) {
                            //    $scope.Variants = data;
                            //}).error(function (data, status, headers, config) {
                            //});

                            clearVariantControls();
                            $scope.SetVariant();

                        } else {
                            ;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.variantInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.VariantSubmitBtnIconClass = "";
                        $scope.VariantSubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }
        //----------------Variant:Body Type-----------------//
        $scope.BodyTypeList = [];
        $scope.BodyTypeDList = [];
        $scope.SelectedBodyTypeList = [];
        function AddBodyType() {
            var index = 0;
            $scope.BodyTypeList = [];
            angular.forEach($scope.BodyTypes, function (value) {
                var x = { id: index, code: value.Id, label: value.VehicleBodyTypeDescription };
                $scope.BodyTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadBodyType() {
            $scope.SelectedBodyTypeList = [];
            angular.forEach($scope.Variant.BodyTypes, function (valueOut) {
                angular.forEach($scope.BodyTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.BodyTypeDList.push(valueIn.label);
                        $scope.SelectedBodyTypeList.push(x);
                    }
                });
            });
        }
        $scope.SendBodyType = function () {
            $scope.Variant.BodyTypes = [];
            $scope.BodyTypeDList = [];
            angular.forEach($scope.SelectedBodyTypeList, function (valueOut) {
                angular.forEach($scope.BodyTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.BodyTypeDList.push(valueIn.label);
                        $scope.Variant.BodyTypes.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Country-----------------//
        $scope.CountryList = [];
        $scope.CountryDList = [];
        $scope.SelectedCountryList = [];
        function AddCountry() {
            var index = 0;
            $scope.CountryList = [];
            angular.forEach($scope.Countries, function (value) {
                var x = { id: index, code: value.Id, label: value.CountryName };
                $scope.CountryList.push(x);
                index = index + 1;
            });
        }
        function LoadCountry() {
            $scope.SelectedCountryList = [];
            angular.forEach($scope.Variant.Countrys, function (valueOut) {
                angular.forEach($scope.CountryList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.CountryDList.push(valueIn.label);
                        $scope.SelectedCountryList.push(x);
                    }
                });
            });
        }
        $scope.SendCountry = function () {
            $scope.Variant.Countrys = [];
            $scope.CountryDList = [];
            angular.forEach($scope.SelectedCountryList, function (valueOut) {
                angular.forEach($scope.CountryList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.CountryDList.push(valueIn.label);
                        $scope.Variant.Countrys.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Fuel Type-----------------//
        $scope.FuelTypeList = [];
        $scope.FuelTypeDList = [];
        $scope.SelectedFuelTypeList = [];
        function AddFuelType() {
            var index = 0;
            $scope.FuelTypeList = [];
            angular.forEach($scope.FuelTypes, function (value) {
                var x = { id: index, code: value.FuelTypeId, label: value.FuelTypeDescription };
                $scope.FuelTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadFuelType() {
            $scope.SelectedFuelTypeList = [];
            angular.forEach($scope.Variant.FuelTypes, function (valueOut) {
                angular.forEach($scope.FuelTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.FuelTypeDList.push(valueIn.label);
                        $scope.SelectedFuelTypeList.push(x);
                    }
                });
            });
        }
        $scope.SendFuelType = function () {
            $scope.Variant.FuelTypes = [];
            $scope.FuelTypeDList = [];
            angular.forEach($scope.SelectedFuelTypeList, function (valueOut) {
                angular.forEach($scope.FuelTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.FuelTypeDList.push(valueIn.label);
                        $scope.Variant.FuelTypes.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Asporation-----------------//
        $scope.AspirationList = [];
        $scope.AspirationDList = [];
        $scope.SelectedAspirationList = [];
        function AddAspiration() {
            var index = 0;
            $scope.AspirationList = [];
            angular.forEach($scope.Aspirations, function (value) {
                var x = { id: index, code: value.Id, label: value.AspirationTypeCode };
                $scope.AspirationList.push(x);
                index = index + 1;
            });
        }
        function LoadAspiration() {
            $scope.SelectedAspirationList = [];
            angular.forEach($scope.Variant.Aspirations, function (valueOut) {
                angular.forEach($scope.AspirationList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.AspirationDList.push(valueIn.label);
                        $scope.SelectedAspirationList.push(x);
                    }
                });
            });
        }
        $scope.SendAspiration = function () {
            $scope.Variant.Aspirations = [];
            $scope.AspirationDList = [];
            angular.forEach($scope.SelectedAspirationList, function (valueOut) {
                angular.forEach($scope.AspirationList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.AspirationDList.push(valueIn.label);
                        $scope.Variant.Aspirations.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Transmission-----------------//
        $scope.TransmissionList = [];
        $scope.TransmissionDList = [];
        $scope.SelectedTransmissionList = [];
        function AddTransmission() {
            var index = 0;
            $scope.TransmissionList = [];
            angular.forEach($scope.Transmissions, function (value) {
                var x = { id: index, code: value.Id, label: value.TransmissionTypeCode };
                $scope.TransmissionList.push(x);
                index = index + 1;
            });
        }
        function LoadTransmission() {
            $scope.SelectedTransmissionList = [];
            angular.forEach($scope.Variant.Transmissions, function (valueOut) {
                angular.forEach($scope.TransmissionList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.TransmissionDList.push(valueIn.label);
                        $scope.SelectedTransmissionList.push(x);
                    }
                });
            });
        }
        $scope.SendTransmission = function () {
            $scope.Variant.Transmissions = [];
            $scope.TransmissionDList = [];
            angular.forEach($scope.SelectedTransmissionList, function (valueOut) {
                angular.forEach($scope.TransmissionList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.TransmissionDList.push(valueIn.label);
                        $scope.Variant.Transmissions.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Drive Type-----------------//
        $scope.DriveTypeList = [];
        $scope.DriveTypeDList = [];
        $scope.SelectedDriveTypeList = [];
        function AddDriveType() {
            var index = 0;
            $scope.DriveTypeList = [];
            angular.forEach($scope.DriveTypes, function (value) {
                var x = { id: index, code: value.Id, label: value.Type };
                $scope.DriveTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadDriveType() {
            $scope.SelectedDriveTypeList = [];
            $scope.DriveTypeDList = [];
            angular.forEach($scope.Variant.DriveTypes, function (valueOut) {
                angular.forEach($scope.DriveTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.DriveTypeDList.push(valueIn.label);
                        $scope.SelectedDriveTypeList.push(x);
                    }
                });
            });
        }
        $scope.SendDriveType = function () {
            $scope.Variant.DriveTypes = [];
            $scope.DriveTypeDList = [];
            angular.forEach($scope.SelectedDriveTypeList, function (valueOut) {
                angular.forEach($scope.DriveTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.DriveTypeDList.push(valueIn.label);
                        $scope.Variant.DriveTypes.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Variant:Premium Addon Type-----------------//
        $scope.PremiumAddonTypeList = [];
        $scope.PremiumAddonTypeDList = [];
        $scope.SelectedPremiumAddonTypeList = [];
        function AddPremiumAddonType() {
            var index = 0;
            $scope.PremiumAddonTypeList = [];
            angular.forEach($scope.PremiumAddonType, function (value) {
                var x = { id: index, code: value.Id, label: value.Description };
                $scope.PremiumAddonTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadPremiumAddonType() {
            $scope.SelectedPremiumAddonTypeList = [];
            $scope.PremiumAddonTypeDList = [];
            angular.forEach($scope.Variant.PremiumAddonType, function (valueOut) {
                angular.forEach($scope.PremiumAddonTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.PremiumAddonTypeDList.push(valueIn.label);
                        $scope.SelectedPremiumAddonTypeList.push(x);
                    }
                });
            });
        }
        $scope.SendPremiumAddonType = function () {
            $scope.Variant.PremiumAddonType = [];
            $scope.PremiumAddonTypeDList = [];
            angular.forEach($scope.SelectedPremiumAddonTypeList, function (valueOut) {
                angular.forEach($scope.PremiumAddonTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.PremiumAddonTypeDList.push(valueIn.label);
                        $scope.Variant.PremiumAddonType.push(valueIn.code);
                    }
                });
            });
        }
        //----------------Manufacturer Warranty-----------------//

        function clearManufacturerWarrantyControls() {
            $scope.ManufacturerWarranty.Id = "00000000-0000-0000-0000-000000000000";
            $scope.ManufacturerWarranty.ApplicableFrom = "";
            $scope.ManufacturerWarranty.ApplicableTo = "";
            $scope.ManufacturerWarranty.WarrantyName = "";
            $scope.ManufacturerWarranty.WarrantyMonths = "";
            $scope.ManufacturerWarranty.WarrantyKm = 0;
            $scope.ManufacturerWarranty.Expired = [];
            $scope.ManufacturerWarranty.MakeId = "00000000-0000-0000-0000-000000000000";
            $scope.ManufacturerWarranty.Countrys = [];
            $scope.ManufacturerWarranty.Models = [];
            $scope.ManufacturerWarranty.IsUnlimited = false;
            $scope.CountryListM = [];
            $scope.CountryDListM = [];
            $scope.SelectedCountryListM = [];
            $scope.ModelList = [];
            $scope.ModelDList = [];
            $scope.SelectedModelList = [];
        }

        $scope.resetAllManufacture = function () {
            $scope.ManufacturerWarranty.Id = "00000000-0000-0000-0000-000000000000";
            $scope.ManufacturerWarranty.ApplicableFrom = "";
            $scope.ManufacturerWarranty.ApplicableTo = "";
            $scope.ManufacturerWarranty.WarrantyName = "";
            $scope.ManufacturerWarranty.WarrantyMonths = "";
            $scope.ManufacturerWarranty.WarrantyKm = 0;
            $scope.ManufacturerWarranty.Expired = [];
            $scope.ManufacturerWarranty.Countrys = [];
            $scope.ManufacturerWarranty.Models = [];
            $scope.ManufacturerWarranty.MakeId = "00000000-0000-0000-0000-000000000000";
            $scope.CountryListM = [];
            $scope.CountryDListM = [];
            $scope.SelectedCountryListM = [];
            $scope.ModelList = [];
            $scope.ModelDList = [];
            $scope.SelectedModelList = [];

        }


        $scope.SetManufacturerWarrantyValues = function () {
            $scope.errorTab4 = "";
            if ($scope.ManufacturerWarranty.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ManufacturerWarrantyManagement/GetManufacturerWarrantyById',
                    data: { "Id": $scope.ManufacturerWarranty.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.ManufacturerWarranty.Id = data.Id;
                    $scope.ManufacturerWarranty.ApplicableFrom = data.ApplicableFrom;
                    $scope.ManufacturerWarranty.WarrantyName = data.WarrantyName;
                    $scope.ManufacturerWarranty.WarrantyMonths = data.WarrantyMonths;
                    $scope.ManufacturerWarranty.WarrantyKm = data.WarrantyKm;
                    $scope.Expired = data.Expired;                  

                }).error(function (data, status, headers, config) {
                    clearManufacturerWarrantyControls();
                });
            }
            else {
                clearManufacturerWarrantyControls();
            }
        }

        $scope.validateManufacturerWarranty = function () {
            var isValid = true;
            if ($scope.WarantyKm) {
                if ($scope.SelectedCountryListM.length == 0) {
                    $scope.validate_ManufacturerWarrantyCountryId = "has-error";
                    isValid = false;
                }
                else {
                    $scope.validate_ManufacturerWarrantyCountryId = "";
                }
                if ($scope.SelectedModelList.length == 0) {
                    $scope.validate_ManufacturerWarrantyModels = "has-error";
                    isValid = false;
                }
                else {
                    $scope.validate_ManufacturerWarrantyModels = "";
                }
                if ($scope.ManufacturerWarranty.WarrantyName == "") {
                    $scope.validate_ManufacturerWarrantyWarrantyName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyWarrantyName = "";
                }
                if ($scope.ManufacturerWarranty.WarrantyMonths == "") {
                    $scope.validate_ManufacturerWarrantyMonths = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyMonths = "";
                }
                //if ($scope.ManufacturerWarranty.WarrantyKm == "" || $scope.ManufacturerWarranty.WarrantyKm == null
                //    || $scope.ManufacturerWarranty.WarrantyKm == null) {
                //    $scope.validate_ManufacturerWarrantyKm = "has-error";
                //    isValid = false;
                //} else {
                //    $scope.validate_ManufacturerWarrantyKm = "";
                //}
                if ($scope.ManufacturerWarranty.ApplicableFrom == "") {
                    $scope.validate_ManufacturerWarrantyApplicableFrom = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyApplicableFrom = "";
                }
                if ($scope.ManufacturerWarranty.CommodityTypeId == "" || $scope.ManufacturerWarranty.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_ManufacturerWarrantyCommodityTypeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyCommodityTypeId = "";
                }

                if ($scope.ManufacturerWarranty.MakeId == "" || $scope.ManufacturerWarranty.MakeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_ManufacturerWarrantyMakeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyMakeId = "";
                }


            } else {

                if ($scope.ManufacturerWarranty.WarrantyName == "") {
                    $scope.validate_ManufacturerWarrantyWarrantyName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyWarrantyName = "";
                }
                if ($scope.ManufacturerWarranty.WarrantyMonths == "") {
                    $scope.validate_ManufacturerWarrantyMonths = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyMonths = "";
                }

                if ($scope.ManufacturerWarranty.ApplicableFrom == "") {
                    $scope.validate_ManufacturerWarrantyApplicableFrom = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyApplicableFrom = "";
                }
                if ($scope.ManufacturerWarranty.CommodityTypeId == "" || $scope.ManufacturerWarranty.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_ManufacturerWarrantyCommodityTypeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyCommodityTypeId = "";
                }

                if ($scope.ManufacturerWarranty.MakeId == "" || $scope.ManufacturerWarranty.MakeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_ManufacturerWarrantyMakeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyMakeId = "";
                }
                if ($scope.SelectedCountryListM.length == 0) {
                    $scope.validate_ManufacturerWarrantyCountryId = "has-error";
                    isValid = false;
                }
                else {
                    $scope.validate_ManufacturerWarrantyCountryId = "";
                }
                if ($scope.SelectedModelList.length == 0) {
                    $scope.validate_ManufacturerWarrantyModels = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_ManufacturerWarrantyModels = "";
                }

                angular.forEach($scope.ManufacturerWarranties, function (valmw) {
                    if ($scope.ManufacturerWarranty.WarrantyName == valmw.WarrantyName) {

                        isValid = false;
                        customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.manufacturerAlreadyExits'))
                    }

                });
            }




            return isValid
        }


        $scope.ManufacturerWarrantySubmit = function () {
            if ($scope.validateManufacturerWarranty()) {





                $scope.errorTab4 = "";
                if ($scope.ManufacturerWarranty.Id == null || $scope.ManufacturerWarranty.Id == "00000000-0000-0000-0000-000000000000") {
                    swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'), showConfirmButton: false });
                    angular.forEach($scope.ManufacturerWarranties, function (valmw) {
                        if ($scope.ManufacturerWarranty.WarrantyName == valmw.WarrantyName) {
                            isValid = false;
                            customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.manufacturerAlreadyExits'))
                            return false;
                        }

                    });

                    $scope.ManufacturerWarrantySubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ManufacturerWarrantySubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ManufacturerWarrantyManagement/AddManufacturerWarranty',
                        data: $scope.ManufacturerWarranty,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ManufacturerWarrantySubmitBtnIconClass = "";
                        $scope.ManufacturerWarrantySubmitBtnDisabled = false;

                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.SetManufacturerWarranties();

                            clearManufacturerWarrantyControls();
                            getPolicySearchPage();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ManufacturerWarrantySubmitBtnIconClass = "";
                        $scope.ManufacturerWarrantySubmitBtnDisabled = false;
                        return false;
                    });
                }
                else {

                    $scope.ManufacturerWarrantySubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ManufacturerWarrantySubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ManufacturerWarrantyManagement/UpdateManufacturerWarranty',
                        data: $scope.ManufacturerWarranty,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ManufacturerWarrantySubmitBtnIconClass = "";
                        $scope.ManufacturerWarrantySubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.SetManufacturerWarranties();
                            clearManufacturerWarrantyControls();
                            getPolicySearchPage();
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ManufacturerWarrantySubmitBtnIconClass = "";
                        $scope.ManufacturerWarrantySubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }
        $scope.WarantyKm = true;
        $scope.WarantyDuration = $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyMonths');
        $scope.SetManufacturerWarrantyModel = function () {
            $scope.errorTab4 = "";
            $scope.ManufacturerWarranties = [];
            if ($scope.ManufacturerWarranty.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": $scope.ManufacturerWarranty.MakeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.ManufacturerWarrantyModeles = data;
                    AddModel();
                    LoadModel();
                    // $scope.SetManufacturerWarranties();
                   //  getPolicySearchPage();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetManufacturerWarrantyMakes = function () {
            clearManufacturerWarrantyControls();
           // getPolicySearchPage();

            angular.forEach($scope.CommodityTypes, function (valc) {
                if ($scope.ManufacturerWarranty.CommodityTypeId == valc.CommodityTypeId) {
                    if (valc.CommodityTypeDescription == "Automobile") {
                        $scope.WarantyKm = true;
                        $scope.WarantyDuration = $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyMonths');
                    } else if (valc.CommodityTypeDescription == "Bank") {
                        $scope.WarantyKm = true;
                        $scope.WarantyDuration = $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyMonths');
                    } else if (valc.CommodityTypeDescription == "Automotive") {
                        $scope.WarantyKm = true;
                        $scope.WarantyDuration = $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyMonths');
                    }
                    else {
                        $scope.WarantyKm = false;
                        $scope.WarantyDuration = $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyHours');
                    }

                }

            });
            $scope.tempMake = [];
            angular.forEach($scope.ManufacturerWarrantyMakes, function (val) {
                if (val.CommodityTypeId == $scope.ManufacturerWarranty.CommodityTypeId) {
                    $scope.tempMake.push(val);
                }
            });
        }
        $scope.SetManufacturerWarranties = function () {
            $scope.errorTab4 = "";
            if ($scope.ManufacturerWarranty.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ManufacturerWarrantyManagement/GetManufacturerWarrantiesByCountryIdAndMakeId',
                    data: {
                        //"CountryId": $scope.ManufacturerWarranty.CountryId,
                        "MakeId": $scope.ManufacturerWarranty.MakeId
                        //  "ModelId": $scope.ManufacturerWarranty.ModelId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.ManufacturerWarranties = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetCountries = function () {
            $scope.errorTab4 = "";
            if ($scope.ManufacturerWarranty.ModelId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Country/GetCountrysByMakeNModel',
                    data: { "MakeId": $scope.ManufacturerWarranty.MakeId, "ModelId": $scope.ManufacturerWarranty.ModelId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.MWCountries = data;

                    if ($scope.MWCountries.length == 0) {
                        customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.countryList'))
                        // $scope.errorTab4 = "Please add the Make and Model selected to the relavant country in order to load the country list";
                    }
                    AddCountryM();
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.SetModel2 = function () {
            if ($scope.Model.MakeId != null && $scope.Model.CategoryId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelByMakeIdAndCatogaryId',
                    data: {
                        "MakeId": $scope.Model.MakeId,
                        "CommodityCategoryId": $scope.Model.CategoryId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Modeles = data;
                    //clearVariantControls();
                }).error(function (data, status, headers, config) {
                });
            }
        }



        //------------------------------ New ManufacterWarrenty -----------------------------------------------



        $scope.gridManufacturerWarranty = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [

                { name: $filter('translate')('pages.makeAndModelManagement.tabMakeManagement.make'), field: 'Makes', enableSorting: false, cellClass: 'columCss',width:100 },
                { name: $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyName'), field: 'WarrantyName', enableSorting: false, cellClass: 'columCss', width: 115 },
                { name: $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyMonths'), field: 'WarrantyMonths', enableSorting: false, cellClass: 'columCss', width: 125 },
                { name: $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.warrantyKm'), field: 'WarrantyKm', enableSorting: false, cellClass: 'columCss', width: 100 },
                { name: $filter('translate')('pages.makeAndModelManagement.tabManufactureWarrenty.applicableFrom'), field: 'ApplicableFrom', enableSorting: false, cellClass: 'columCss', width: 120 },

                // { name: 'Scheme', field: 'Scheme', enableSorting: false, cellClass: 'columCss' },
                //{ name: 'Active', field: 'Active', enableSorting: false, cellClass: 'columCss', width: 60 },

                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadManufacturerWarrantyForUpdate(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.update') + '</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.handleWindowResize();
                //$interval(function () {
                //    $scope.gridApi.core.handleWindowResize();
                //}, 0, 1);
                //$scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                //    if (sortColumns.length == 0) {
                //        paginationOptionsManufacturerWarrentySearchGrid.sort = null;
                //    } else {
                //        paginationOptionsManufacturerWarrentySearchGrid.sort = sortColumns[0].sort.direction;
                //    }
                //    getPolicySearchPage();
                //    $interval(function () {
                //        $scope.gridApi.selection.selectRow($scope.gridManufacturerWarranty);
                //    }, 0, 1);
                //});
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsManufacturerWarrentySearchGrid.pageNumber = newPage;
                    paginationOptionsManufacturerWarrentySearchGrid.pageSize = pageSize;
                    getPolicySearchPage();
                });
            }
        };
        $scope.refresSearchGridData = function () {
            getPolicySearchPage();

        }

        function getPolicySearchPage() {

            $scope.gridManufacturerWarrantyloading = true;
            $scope.gridManufacturerWarrantyloadAttempted = false;
            var policySearchGridParam =
                {
                    'paginationOptionsManufacturerWarrentySearchGrid': paginationOptionsManufacturerWarrentySearchGrid,
                    'manufacturerWarrentySearchGridSearchCriterias': $scope.ManufacturerWarranty,
                    'CommodityTypeId': $scope.ManufacturerWarranty.CommodityTypeId
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ManufacturerWarrantyManagement/SearchManufacturerWarrantySchemes',
                data: JSON.stringify(policySearchGridParam),
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var responseArr = JSON.parse(data);
                if (responseArr != null) {
                    //$scope.gridDealerDiscounts.data = responseArr.data;

                    $scope.gridManufacturerWarranty.data = responseArr.data;

                    angular.forEach($scope.gridManufacturerWarranty.data, function (value) {
                        if (value.IsUnlimited == true) {
                            value.WarrantyKm = "Unlimited"
                        }
                    });


                    $scope.gridManufacturerWarranty.totalItems = responseArr.totalRecords;

                    $scope.SetManufacturerWarrantyModel();
                    $scope.SetManufacturerWarranties();

                } else {
                    // $scope.gridDealerDiscounts.data = [];
                    // $scope.gridDealerDiscounts.totalItems = 0;
                }

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.gridManufacturerWarrantyloading = false;
                $scope.gridManufacturerWarrantyloadAttempted = true;

            });

        };

        $scope.dealerWarrentydrpload = function () {
            if (isGuid($scope.ManufacturerWarranty.Id)) {
                swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ManufacturerWarrantyManagement/GetManufacturerWarrantyById',
                    data: { "Id": $scope.ManufacturerWarranty.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.ManufacturerWarranty.CommodityTypeId = data.CommodityTypeId;
                    $scope.SetManufacturerWarrantyMakes();
                    $scope.ManufacturerWarranty.MakeId = data.MakeId;

                    $scope.SetManufacturerWarrantyModel();
                    $scope.ManufacturerWarranty.Models = data.Models;
                    $scope.ManufacturerWarranty.Countries = data.Countrys;
                    $scope.ManufacturerWarranty.Id = data.Id;
                    $scope.ManufacturerWarranty.ApplicableFrom = data.ApplicableFrom;
                    $scope.ManufacturerWarranty.WarrantyName = data.WarrantyName;
                    $scope.ManufacturerWarranty.WarrantyMonths = data.WarrantyMonths;
                    $scope.ManufacturerWarranty.WarrantyKm = data.WarrantyKm;
                    $scope.Expired = data.Expired;

                    AddModel();
                    LoadModel();
                    $scope.selectedModelChanged();

                    LoadCountryM();

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            } else {
                clearManufacturerWarrantyControls();
            }
        }

        $scope.loadManufacturerWarrantyForUpdate = function (dealerDiscountId) {
            if (isGuid(dealerDiscountId)) {
                swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('pages.makeAndModelManagement.sucessMessages.manufacturerInformation'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ManufacturerWarrantyManagement/GetManufacturerWarrantyById',
                    data: { "Id": dealerDiscountId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.ManufacturerWarranty.CommodityTypeId = data.CommodityTypeId;
                    $scope.SetManufacturerWarrantyMakes();
                    $scope.ManufacturerWarranty.MakeId = data.MakeId;
                    $scope.SetManufacturerWarrantyModel();

                    $scope.ManufacturerWarranty.Models = data.Models;

                    $scope.ManufacturerWarranty.Countrys = data.Countrys;
                    $scope.ManufacturerWarranty.ApplicableFrom = data.ApplicableFrom;
                    $scope.ManufacturerWarranty.WarrantyName = data.WarrantyName;
                    $scope.ManufacturerWarranty.WarrantyMonths = data.WarrantyMonths;
                    $scope.ManufacturerWarranty.WarrantyKm = Number(data.WarrantyKm);
                    if (data.WarrantyKm == "Unlimited") {
                        $scope.ManufacturerWarranty.WarrantyKm = 0;
                    }
                    $scope.ManufacturerWarranty.Id = data.Id;
                    $scope.ManufacturerWarranty.IsUnlimited = data.IsUnlimited;
                    $scope.Expired = data.Expired;

                    AddModel();
                    LoadModel();
                    $scope.selectedModelChanged();

                    LoadCountryM();


                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }


        //----------------Manufacture Warrenty:Country-----------------//
        $scope.CountryListM = [];
        $scope.CountryDListM = [];
        $scope.SelectedCountryListM = [];


        function AddCountryM() {
            var index = 0;
            $scope.CountryListM = [];
            angular.forEach($scope.MWCountries, function (value) {
                var x = { id: index, code: value.Id, label: value.CountryName };
                $scope.CountryListM.push(x);
                index = index + 1;
            });
        }
        function LoadCountryM() {
            $scope.SelectedCountryListM = [];
            $scope.CountryDListM = [];
            angular.forEach($scope.ManufacturerWarranty.Countrys, function (valueOut) {
                angular.forEach($scope.CountryListM, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedCountryListM.push(x);
                        $scope.CountryDListM.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendCountryM = function () {
            $scope.ManufacturerWarranty.Countrys = [];
            $scope.CountryDListM = [];
            angular.forEach($scope.SelectedCountryListM, function (valueOut) {
                angular.forEach($scope.CountryListM, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.CountryDListM.push(valueIn.label);
                        $scope.ManufacturerWarranty.Countrys.push(valueIn.code);
                    }
                });
            });
        }

        $scope.ModelList = [];
        $scope.ModelDList = [];
        $scope.SelectedModelList = [];


        function AddModel() {
            var index = 0;
            $scope.ModelList = [];
            angular.forEach($scope.ManufacturerWarrantyModeles, function (value) {
                var x = { id: index, code: value.Id, label: value.ModelName };
                $scope.ModelList.push(x);
                index = index + 1;
            });
        }

        function LoadModel() {
            $scope.SelectedModelList = [];
            $scope.ModelDList = [];
            angular.forEach($scope.ManufacturerWarranty.Models, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedModelList.push(x);
                        // $scope.MakeDList.push(valueIn.label);
                    }
                });
            });
        }

        $scope.SendModel = function () {
            $scope.ManufacturerWarranty.Models = [];
            $scope.ModelDList = [];
            angular.forEach($scope.SelectedModelList, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.ModelDList.push(valueIn.label);
                        $scope.ManufacturerWarranty.Models.push(valueIn.code);
                    }
                });
            });
            $scope.selectedModelChanged();
        }



        $scope.selectedModelChanged = function () {

            if ($scope.ManufacturerWarranty.Models.length > 0) {
                var data = [];

                //data.push($scope.ManufacturerWarranty.Models);

                if ($scope.ManufacturerWarranty.Models.length > 0) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/GetAllCountrysByMakeNModelIdsNew',
                        data: JSON.stringify({ 'data': $scope.ManufacturerWarranty.Models, "MakeId": $scope.ManufacturerWarranty.MakeId }),
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.MWCountries = data;

                        if ($scope.MWCountries.length == 0) {

                            customErrorMessage($filter('translate')('pages.makeAndModelManagement.errorMessages.countryList'))
                            //$scope.errorTab4 = "Please add the Make and Model selected to the relavant country in order to load the country list";
                        }
                        AddCountryM();
                        LoadCountryM();

                    }).error(function (data, status, headers, config) {
                    });
                }
            }
        }

        $scope.isunlimitedWarrenty = function () {

            if ($scope.ManufacturerWarranty.IsUnlimited == true) {
                $scope.ManufacturerWarranty.WarrantyKm = 0;
                $scope.disableWarrantyKm = true;
            } else {
                $scope.disableWarrantyKm = false;
            }
        }
        //$scope.selectedModelDeselected = function () {
        //    $scope.isAllModelsSelected = false;
        //    $scope.selectedModelChanged();
        //}
        //$scope.allModelsSelected = function () {
        //    $scope.isAllModelsSelected = true;
        //}
        //$scope.allModelsDeselected = function () {
        //    $scope.isAllModelsSelected = false;
        //}


    });

