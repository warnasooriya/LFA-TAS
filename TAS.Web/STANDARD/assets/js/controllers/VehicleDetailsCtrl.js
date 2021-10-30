app.controller('VehicleDetailsCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Vehicle Details";
        $scope.ModalDescription = "Add Edit Vehicle Details";

        $scope.labelSave = 'common.button.save';

        $scope.selectedpp = {};
        $scope.loadInitailData = function () { }
        $scope.errorTab1 = "";
        $scope.formAction = true;//true for add new
        $scope.VehicleSubmitBtnIconClass = "";
        $scope.VehicleSubmitBtnDisabled = false;
        $scope.ModelsForSearch = [];

        $scope.VehicleSearchGridloadAttempted = false;
        $scope.VehicleSearchGridloading = false;
        $scope.EngineCapacityDisable = false;
        $scope.CylinderCountDisable = false;
        $scope.GrossWeightDisable = false;
        $scope.FuelTypeDisable = false;
        $scope.TransmissionsDisable = false;
        $scope.DriveTypesDisable = false;
        $scope.BodyTypesDisable = false;
        $scope.AspirationsDisable = false;
        $scope.VehicleCurrency = "";
        $scope.dealersByCountry = [];
        //$scope.DealerCurrencyId = emptyGuid();

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };


        $scope.Modeles = [];
        $scope.Variantss = [];

        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        $scope.Vehicle = {
            Id: emptyGuid(),
            VINNo: '',
            MakeId: emptyGuid(),
            ModelId: emptyGuid(),
            CategoryId: emptyGuid(),
            ItemStatusId: emptyGuid(),
            CylinderCountId: emptyGuid(),
            BodyTypeId: emptyGuid(),
            PlateNo: '',
            ModelYear: '',
            FuelTypeId: emptyGuid(),
            AspirationId: emptyGuid(),
            Variant: emptyGuid(),
            TransmissionId: emptyGuid(),
            ItemPurchasedDate: '',
            EngineCapacityId: emptyGuid(),
            DriveTypeId: emptyGuid(),
            VehiclePrice: 0.00,
            DealerPrice: 0.00,
            CommodityUsageTypeId: emptyGuid(),
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            DealerCurrencyId: emptyGuid(),
            currencyPeriodId: emptyGuid(),
            RegistrationDate: '',
            GrossWeight: '',
            EngineNumber:''
        };

        function LoadDetails() {
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
                url: '/TAS.Web/api/Country/GetAllActiveCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
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
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                angular.forEach($scope.CommodityTypes, function (value) {
                    if (value.CommodityTypeDescription == 'Automobile' || value.CommodityTypeDescription == 'Automotive') {
                        $scope.CommodityTypeId = value.CommodityTypeId;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.CommodityTypeId }
                        }).success(function (data, status, headers, config) {
                            $scope.Makes = data;
                        }).error(function (data, status, headers, config) {
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VariantManagement/GetAllVariant',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.CommodityTypeId }
                        }).success(function (data, status, headers, config) {
                            $scope.Variants = data;
                        }).error(function (data, status, headers, config) {
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.CommodityTypeId }
                        }).success(function (data, status, headers, config) {
                            $scope.Categories = data;
                        }).error(function (data, status, headers, config) {
                        });
                        return;
                    }
                });
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
            $scope.Modeles = [];
            $scope.Variantss = [];
        }

        LoadDetails();

        $scope.SetVariant = function () {
            $scope.errorTab1 = "";
            if ($scope.Vehicle.ModelId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Modeles, function (value, key) {
                    if (value.Id == $scope.Vehicle.ModelId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Variantss = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }

        $scope.SetModel = function () {
            $scope.errorTab1 = "";
            if ($scope.Vehicle.MakeId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.Vehicle.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Modeles = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }

        $scope.SetModelPopup = function () {
            $scope.errorTab1 = "";
            if ($scope.vehicalSearchGridSearchCriterias.MakeId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.vehicalSearchGridSearchCriterias.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.ModelsForSearch = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }
        $scope.SetVehicleValues = function () {
            $scope.errorTab1 = "";
            if ($scope.Vehicle.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.Vehicle.Id }
                }).success(function (data, status, headers, config) {
                    if (data.status == "Policy") {
                        //  clearVehicleControls();
                        $scope.VehicleSubmitBtnDisabled = true;
                        SweetAlert.swal({
                            title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                            text: $filter('translate')('pages.vehicleManagement.inForMessages.cantModify'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else if (data.status == "Bordx") {
                        // clearVehicleControls();
                        $scope.VehicleSubmitBtnDisabled = true;
                        SweetAlert.swal({
                            title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                            text: $filter('translate')('pages.vehicleManagement.inForMessages.cantModifyBordereaux'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else {
                        $scope.Vehicle.Id = data.Id;
                        $scope.Vehicle.CommodityUsageTypeId = data.CommodityUsageTypeId;
                        $scope.Vehicle.CategoryId = data.CategoryId;
                        $scope.VINNoValidation();
                        $scope.Vehicle.VINNo = data.VINNo;
                        $scope.VINNoValidate();
                        $scope.Vehicle.MakeId = data.MakeId;
                        $scope.Vehicle.ModelId = data.ModelId;
                        $scope.Vehicle.ItemStatusId = data.ItemStatusId;
                        $scope.Vehicle.CylinderCountId = data.CylinderCountId;
                        $scope.Vehicle.BodyTypeId = data.BodyTypeId;
                        $scope.Vehicle.DealerId = data.DealerId;
                        $scope.Vehicle.CountryId = data.CountryId;
                        $scope.Vehicle.PlateNo = data.PlateNo;
                        $scope.Vehicle.ModelYear = data.ModelYear;
                        $scope.Vehicle.FuelTypeId = data.FuelTypeId;
                        $scope.Vehicle.AspirationId = data.AspirationId;
                        $scope.Vehicle.Variant = data.Variant;
                        $scope.Vehicle.TransmissionId = data.TransmissionId;
                        $scope.Vehicle.ItemPurchasedDate = data.ItemPurchasedDate;
                        $scope.Vehicle.EngineCapacityId = data.EngineCapacityId;
                        $scope.Vehicle.DriveTypeId = data.DriveTypeId;
                        $scope.Vehicle.VehiclePrice = data.VehiclePrice;
                        $scope.Vehicle.DealerPrice = data.DealerPrice;
                        $scope.Vehicle.RegistrationDate = data.RegistrationDate;
                        $scope.Vehicle.GrossWeight = data.GrossWeight;
                        $scope.Vehicle.EngineNumber = data.EngineNumber;

                        $scope.SetModel();
                        $scope.SetVariant();
                    }
                }).error(function (data, status, headers, config) {
                    clearVehicleControls();
                });
            }
            else {
                clearVehicleControls();
            }
        }

        function clearVehicleControls() {
            $scope.Vehicle.Id = emptyGuid();
            $scope.Vehicle.VINNo ="";
            $scope.Vehicle.MakeId = emptyGuid();
            $scope.Vehicle.ModelId = emptyGuid();
            $scope.Vehicle.CategoryId = emptyGuid();
            $scope.Vehicle.ItemStatusId = emptyGuid();
            $scope.Vehicle.CylinderCountId = emptyGuid();
            $scope.Vehicle.BodyTypeId = emptyGuid();
            $scope.Vehicle.CountryId = emptyGuid();
            $scope.Vehicle.DealerId = emptyGuid();
            $scope.Vehicle.PlateNo = "";
            $scope.Vehicle.ModelYear = "";
            $scope.Vehicle.FuelTypeId = emptyGuid();
            $scope.Vehicle.AspirationId = emptyGuid();
            $scope.Vehicle.Variant = emptyGuid();
            $scope.Vehicle.TransmissionId = emptyGuid();
            $scope.Vehicle.ItemPurchasedDate = "";
            $scope.Vehicle.EngineCapacityId = emptyGuid();
            $scope.Vehicle.DriveTypeId = emptyGuid();
            $scope.Vehicle.VehiclePrice = 0.00;
            $scope.Vehicle.DealerPrice = 0.00;
            $scope.VinLength = 0;
            $scope.Vin = false;
            $scope.Vehicle.CommodityUsageTypeId = emptyGuid();
            $scope.Vehicle.RegistrationDate = "";
            $scope.Vehicle.GrossWeight = "";
            $scope.formAction = true;//true for add new
            $scope.EngineCapacityDisable = false;
            $scope.CylinderCountDisable = false;
            $scope.GrossWeightDisable = false;
            $scope.FuelTypeDisable = false;
            $scope.TransmissionsDisable = false;
            $scope.DriveTypesDisable = false;
            $scope.BodyTypesDisable = false;
            $scope.AspirationsDisable = false;
            $scope.Vehicle.EngineNumber = "";
        };

        $scope.resetAll = function () {
            $scope.Vehicle.Id = emptyGuid();
            $scope.Vehicle.VINNo = "";
            $scope.Vehicle.MakeId = emptyGuid();
            $scope.Vehicle.ModelId = emptyGuid();
            $scope.Vehicle.CategoryId = emptyGuid();
            $scope.Vehicle.ItemStatusId = emptyGuid();
            $scope.Vehicle.CylinderCountId = emptyGuid();
            $scope.Vehicle.BodyTypeId = emptyGuid();
            $scope.Vehicle.CountryId = emptyGuid();
            $scope.Vehicle.DealerId = emptyGuid();
            $scope.Vehicle.PlateNo = "";
            $scope.Vehicle.ModelYear = "";
            $scope.Vehicle.FuelTypeId = emptyGuid();
            $scope.Vehicle.AspirationId = emptyGuid();
            $scope.Vehicle.Variant = emptyGuid();
            $scope.Vehicle.TransmissionId = emptyGuid();
            $scope.Vehicle.ItemPurchasedDate = "";
            $scope.Vehicle.EngineCapacityId = emptyGuid();
            $scope.Vehicle.DriveTypeId = emptyGuid();
            $scope.Vehicle.VehiclePrice = 0.00;
            $scope.Vehicle.DealerPrice = 0.00;
            $scope.VinLength = 0;
            $scope.Vin = false;
            $scope.Vehicle.CommodityUsageTypeId = emptyGuid();
            $scope.Vehicle.RegistrationDate = "";
            $scope.Vehicle.GrossWeight = "";
            $scope.formAction = true;//true for add new
            $scope.VehicleSubmitBtnDisabled = false;
            $scope.EngineCapacityDisable = false;
            $scope.CylinderCountDisable = false;
            $scope.GrossWeightDisable = false;
            $scope.FuelTypeDisable = false;
            $scope.TransmissionsDisable = false;
            $scope.DriveTypesDisable = false;
            $scope.BodyTypesDisable = false;
            $scope.AspirationsDisable = false;
            $scope.Vehicle.EngineNumber = "";
        }

        $scope.Vin = false;
        $scope.VinLength = 0;
        $scope.VINNoValidation = function()
        {
            angular.forEach($scope.Categories, function (value) {
                if (value.CommodityCategoryId == $scope.Vehicle.CategoryId)
                {
                    $scope.VinLength = value.Length;
                }
            });
        }
        $scope.VINNoValidate = function () {
            if ($scope.Vehicle.VINNo.length == $scope.VinLength) {
                $scope.Vin = true;
            }
            else {
                $scope.Vin = false;
            }
        }


        //$scope.validateVehicle = function () {
        //    $scope.errorTab1 = "Please Enter ";
        //}

        $scope.validateVehicleDetails = function () {
            var isValid = true;

            if ($scope.Vehicle.VINNo == "" || $scope.Vehicle.VINNo == undefined || $scope.Vehicle.VINNo == null) {
                $scope.validate_VINNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_VINNo = "";
            }
            if ($scope.Vehicle.CategoryId == "" || $scope.Vehicle.CategoryId == "00000000-0000-0000-0000-000000000000"
                || $scope.Vehicle.CategoryId == null) {
                $scope.validate_CategoryId = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_CategoryId = "";
            }
            if ($scope.Vehicle.ModelId == "" || $scope.Vehicle.ModelId == "00000000-0000-0000-0000-000000000000"
                || $scope.Vehicle.ModelId == null) {
                $scope.validate_ModelId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelId = "";
            }
            if ($scope.Vehicle.MakeId == "" || $scope.Vehicle.MakeId == "00000000-0000-0000-0000-000000000000" ||
                $scope.Vehicle.MakeId == null) {
                $scope.validate_MakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_MakeId = "";
            }
            if ($scope.Vehicle.ItemStatusId == "" || $scope.Vehicle.ItemStatusId == "00000000-0000-0000-0000-000000000000" ||
                $scope.Vehicle.ItemStatusId == null) {
                $scope.validate_ItemStatusId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemStatusId = "";
            }
            if ($scope.Vehicle.ModelYear == "" || $scope.Vehicle.ModelYear == null || $scope.Vehicle.ModelYear == undefined) {
                $scope.validate_ModelYear = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelYear = "";
            }
            if ($scope.Vehicle.EngineNumber == "" || $scope.Vehicle.EngineNumber == null || $scope.Vehicle.EngineNumber == undefined) {
                $scope.validate_EngineNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_EngineNumber = "";
            }
            if ($scope.Vehicle.EngineCapacityId == "" || $scope.Vehicle.EngineCapacityId == "00000000-0000-0000-0000-000000000000"
                || $scope.Vehicle.EngineCapacityId == undefined) {
                $scope.validate_EngineCapacityId = "has-error";
                isValid = false;
            } else {
                $scope.validate_EngineCapacityId = "";
            }
            if ($scope.Vehicle.CylinderCountId == "" || $scope.Vehicle.CylinderCountId == "00000000-0000-0000-0000-000000000000"
                || $scope.Vehicle.CylinderCountId == undefined) {
                $scope.validate_CylinderCountId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CylinderCountId = "";
            }
            //if ($scope.Vehicle.CommodityUsageTypeId == "" || $scope.Vehicle.CommodityUsageTypeId == "00000000-0000-0000-0000-000000000000"
            //    || $scope.Vehicle.CommodityUsageTypeId == undefined) {
            //    $scope.validate_CommodityUsageTypeId = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_CommodityUsageTypeId = "";
            //}
            if ($scope.Vehicle.ItemPurchasedDate == "" || $scope.Vehicle.ItemPurchasedDate == undefined || $scope.Vehicle.ItemPurchasedDate == null) {
                $scope.validate_ItemPurchasedDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemPurchasedDate = "";
            }
            //if ($scope.Vehicle.VehiclePrice == "" || $scope.Vehicle.VehiclePrice == undefined || $scope.Vehicle.VehiclePrice == null) {
            //    $scope.validate_VehiclePrice = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_VehiclePrice = "";
            //}
            if ($scope.Vehicle.DealerPrice == "" || $scope.Vehicle.DealerPrice == undefined || $scope.Vehicle.DealerPrice == null) {
                $scope.validate_DealerPrice = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerPrice = "";
            }
            //if ($scope.Vehicle.PlateNo == "" || $scope.Vehicle.PlateNo == undefined || $scope.Vehicle.PlateNo == null) {
            //    $scope.validate_PlateNo = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_PlateNo = "";
            //}
            if ($scope.Vehicle.CountryId == "" || $scope.Vehicle.CountryId == "00000000-0000-0000-0000-000000000000" || $scope.Vehicle.CountryId == undefined ||
                $scope.Vehicle.CountryId == null) {
                $scope.validate_CountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryId = "";
            }
            if ($scope.Vehicle.DealerId == "" || $scope.Vehicle.DealerId == "00000000-0000-0000-0000-000000000000" || $scope.Vehicle.DealerId == null
                || $scope.Vehicle.DealerId == undefined) {
                $scope.validate_DealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerId = "";
            }
            if ($scope.Vehicle.RegistrationDate == "" || $scope.Vehicle.RegistrationDate == undefined || $scope.Vehicle.RegistrationDate == null) {
                $scope.validate_RegistrationDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_RegistrationDate = "";
            }

            return isValid
        }

        $scope.VehicleSubmit = function () {
            if ($scope.validateVehicleDetails()) {


                if ($scope.Vehicle.VINNo != "") {
                    $scope.errorTab1 = "";
                    if ($scope.Vehicle.Id == null || $scope.Vehicle.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VehicleDetails/AddVehicleDetails',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: $scope.Vehicle
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleSubmitBtnIconClass = "";
                            $scope.VehicleSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                    text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                //    $http({
                                //        method: 'POST',
                                //        url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                                //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                //}).success(function (data, status, headers, config) {
                                //        $scope.Vehicles = data;
                                //    }).error(function (data, status, headers, config) {
                                //    });
                                clearVehicleControls();
                            } else {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                    text: data,
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleSubmitBtnIconClass = "";
                            $scope.VehicleSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.VehicleSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VehicleDetails/UpdateVehicleDetails',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: $scope.Vehicle
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleSubmitBtnIconClass = "";
                            $scope.VehicleSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                    text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                //    $http({
                                //        method: 'POST',
                                //        url: '/TAS.Web/api/VehicleDetails/GetAllVehicleDetails',
                                //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                //}).success(function (data, status, headers, config) {
                                //        $scope.Vehicles = data;
                                //    }).error(function (data, status, headers, config) {
                                //    });
                                clearVehicleControls();
                            } else {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                    text: data,
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.vehicleManagement.inForMessages.vehicleInformation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleSubmitBtnIconClass = "";
                            $scope.VehicleSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }

        //-------------------------------- Search -----------------------------------

        $scope.vehicalSearchGridSearchCriterias = {
            VINNo: "",
            Make: emptyGuid(),
            Model: emptyGuid(),
            PlateNo: ""
        };

        var paginationOptionsVehicleSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.SearchVehiclePopupReset = function () {

            $scope.vehicalSearchGridSearchCriterias = {
                VINNo: '',
                Make: emptyGuid(),
                Model: emptyGuid(),
                PlateNo: ''

            }

        }

        $scope.SearchVehiclePopup = function () {
            $scope.vehicalSearchGridSearchCriterias = {
                VINNo: '',
                Make: emptyGuid(),
                Model: emptyGuid(),
                PlateNo:''

            };
            var paginationOptionsVehicleSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
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

        var getVehicleSearchPage = function () {

            $scope.VehicleSearchGridloading = true;
            $scope.VehicleSearchGridloadAttempted = false;


            var VehicleSearchGridParam =
                {
                    'paginationOptionsVehicleSearchGrid': paginationOptionsVehicleSearchGrid,
                    'vehicalSearchGridSearchCriterias': $scope.vehicalSearchGridSearchCriterias
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Vehicle/GetAllVehiclesForSearchGrid',
                data: VehicleSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                //alert(message);
                $scope.gridOptionsVehicle.data = response_arr.data;
                $scope.gridOptionsVehicle.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.VehicleSearchGridloadAttempted = true;
                $scope.VehicleSearchGridloading = false;

            });
        }


        $scope.gridOptionsVehicle = {

            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.vehicleManagement.vINNo'), field: 'VINNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.vehicleManagement.make'), field: 'Make', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.vehicleManagement.model'), field: 'Model', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.vehicleManagement.plateNo'), field: 'PlateNo', enableSorting: false, cellClass: 'columCss' },
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

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }


        $scope.loadVehicle = function (ItemId) {
            if (isGuid(ItemId)) {
                VehicleSearchPopup.close();
                $scope.formAction = false;
                $scope.Vehicle.VehicleSearchDisabled = true;
                $scope.VehicleSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/VehicleDetails/GetVehicleDetailsById',
                    data: { "Id": ItemId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.Vehicle.VehicleSearchDisabled = false;
                    $scope.VehicleSubmitBtnDisabled = false;
                    $scope.Vehicle.Id = data.Id;
                    $scope.Vehicle.MakeId = data.MakeId;
                    $scope.Vehicle.ModelId = data.ModelId;
                    $scope.Vehicle.CategoryId = data.CategoryId;
                    $scope.VINNoValidation();
                    $scope.Vehicle.VINNo = data.VINNo;
                    $scope.VINNoValidate();
                    $scope.Vehicle.ItemStatusId = data.ItemStatusId;
                    $scope.Vehicle.CylinderCountId = data.CylinderCountId;
                    $scope.Vehicle.BodyTypeId = data.BodyTypeId;
                    $scope.Vehicle.PlateNo = data.PlateNo;
                    $scope.Vehicle.ModelYear = data.ModelYear;
                    $scope.Vehicle.FuelTypeId = data.FuelTypeId;
                    $scope.Vehicle.AspirationId = data.AspirationId;
                    $scope.Vehicle.Variant = data.Variant;
                    $scope.Vehicle.TransmissionId = data.TransmissionId;
                    $scope.Vehicle.ItemPurchasedDate = data.ItemPurchasedDate;
                    $scope.Vehicle.EngineCapacityId = data.EngineCapacityId;
                    $scope.Vehicle.DriveTypeId = data.DriveTypeId;
                    $scope.Vehicle.VehiclePrice = data.VehiclePrice;
                    $scope.Vehicle.DealerPrice = data.DealerPrice
                    $scope.Vehicle.CommodityUsageTypeId = data.CommodityUsageTypeId;
                    $scope.Vehicle.CountryId = data.CountryId;
                    $scope.selectedCountryChanged();
                    $scope.Vehicle.DealerId = data.DealerId;
                    $scope.Vehicle.RegistrationDate = data.RegistrationDate;
                    $scope.Vehicle.EngineNumber = data.EngineNumber;
                    $scope.SetModel();
                    $scope.SetVehicleValues();

                    //$scope.selectedDealerChanged();

                    //$scope.Policy.ItemId = $scope.Vehicle.Id;
                    //$scope.Model.ModelId = $scope.Vehicle.ModelId


                }).error(function (data, status, headers, config) {
                    $scope.Vehicle.VehicleSearchDisabled = false;
                    $scope.VehicleSubmitBtnDisabled = false;
                    // clearCustomerControls();
                });
            }
        }


        //----------------------------------- Dealer Currency -------------------------------------

        $scope.selectedDealerChanged = function () {

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
                                        title: $filter('translate')('common.alertTitle'),
                                        text: "Selected dealer's currency(" + $scope.VehicleCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                }
                            }).error(function (data, status, headers, config) {
                            });
                            return false;

                        });
                    } else {
                        if ($scope.Vehicle.DealerId != value.Id) {

                        }
                    }

                    //SweetAlert.swal({
                    //    title: "TAS Information",
                    //    text: "Selected dealer currency is not present in current conversion period. Please update it",
                    //    type: "warning",
                    //    confirmButtonColor: "rgb(221, 107, 85)"
                    //});
                });
            }
        }


        $scope.selectedCountryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealersByCountryId',
                data: { "Id": $scope.Vehicle.CountryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealersByCountry = data;
                if (isGuid($scope.Vehicle.CountryId)) {
                    angular.forEach($scope.dealersByCountry, function (value) {
                        if ($scope.Vehicle.DealerId == value.Id) {
                            //$scope.Vehicle.insurerId = value.InsurerId;
                            $scope.Vehicle.DealerCurrencyId = value.CurrencyId;


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
                                            title: $filter('translate')('common.alertTitle'),
                                            text: "Selected dealer's currency(" + $scope.VehicleCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                            type: "warning",
                                            confirmButtonText: $filter('translate')('common.button.ok'),
                                            confirmButtonColor: "rgb(221, 107, 85)"
                                        });
                                    }
                                }).error(function (data, status, headers, config) {
                                });

                            }).error(function (data, status, headers, config) {
                            });
                            return false;
                        }
                    });
                }
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.SetVariantD = function () {
            if ($scope.Vehicle.Variant != null || $scope.Vehicle.Variant != undefined) {
                //$scope.ValueDisable = false;
                angular.forEach($scope.Variantss, function (value, key) {
                    if (value.Id == $scope.Vehicle.Variant) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetCylinderCountEnginCapacityByVariantId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Vehicle.EngineCapacityId = data.EngineCapacityId;
                            $scope.Vehicle.CylinderCountId = data.CylinderCountId;
                            $scope.Vehicle.GrossWeight = data.GrossWeight;
                            $scope.Vehicle.FuelTypeId = data.FuelTypes[0];
                            $scope.Vehicle.TransmissionId = data.Transmissions[0];
                            $scope.Vehicle.DriveTypeId = data.DriveTypes[0];
                            $scope.Vehicle.BodyTypeId = data.BodyTypes[0];
                            $scope.Vehicle.AspirationId = data.Aspirations[0];
                            $scope.EngineCapacityDisable = true;
                            $scope.CylinderCountDisable = true;
                            $scope.GrossWeightDisable = true;
                            $scope.FuelTypeDisable = true;
                            $scope.TransmissionsDisable = true;
                            $scope.DriveTypesDisable = true;
                            $scope.BodyTypesDisable = true;
                            $scope.AspirationsDisable = true;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            } else {
                $scope.EngineCapacityDisable = false;
                $scope.CylinderCountDisable = false;
                $scope.GrossWeightDisable = false;
                $scope.FuelTypeDisable = false;
                $scope.TransmissionsDisable = false;
                $scope.DriveTypesDisable = false;
                $scope.BodyTypesDisable = false;
                $scope.AspirationsDisable = false;
            }
        }
    });



