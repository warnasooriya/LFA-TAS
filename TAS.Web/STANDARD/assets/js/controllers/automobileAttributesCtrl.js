app.controller('AutomobileAttributesCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Automobile Attributes";
        $scope.ModalDescription = "Add Edit Automobile Attributes";
        $scope.selectedpp = {};
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";
        $scope.errorTab3 = "";
        $scope.VehicleKiloWattBtnIconClass = "";
        $scope.VehicleKiloWattBtnDisabled = false;
        $scope.VehicleHorsePowerSubmitBtnIconClass = "";
        $scope.VehicleHorsePowerSubmitBtnDisabled = false;
        $scope.VehicleAspirationTypeSubmitBtnIconClass = "";
        $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
        $scope.TransmissionTypeSubmitBtnIconClass = "";
        $scope.TransmissionTypeSubmitBtnDisabled = false;

        $scope.FuelTypeSubmitBtnIconClass = "";
        $scope.FuelTypeSubmitBtnDisabled = false;
        $scope.EngineCapacitySubmitBtnIconClass = "";
        $scope.EngineCapacitySubmitBtnDisabled = false;
        $scope.CylinderCountSubmitBtnIconClass = "";
        $scope.CylinderCountSubmitBtnDisabled = false;

        $scope.VehicleColorSubmitBtnIconClass = "";
        $scope.VehicleColorSubmitBtnDisabled = false;
        $scope.DriveTypeSubmitBtnIconClass = "";
        $scope.DriveTypeSubmitBtnDisabled = false;
        $scope.VehicleBodyTypeSubmitBtnIconClass = "";
        $scope.VehicleBodyTypeSubmitBtnDisabled = false;

        $scope.VehicleWeightSubmitBtnIconClass = "";
        $scope.VehicleWeightSubmitBtnDisabled = false;
        $scope.disableTo = false;
        $scope.labelSave = 'pages.automobileAttributes.save';
        $scope.bodyTypelabelSave = 'pages.automobileAttributes.tabGeneralAttributes.bodyTypeSave';
        $scope.colorlabelSave = 'pages.automobileAttributes.tabGeneralAttributes.colorTypeSave';
        $scope.cylinderlabelSave = 'pages.automobileAttributes.tabTecAttributes.cylinderTypeSave';
        $scope.enginlabelSave = 'pages.automobileAttributes.tabTecAttributes.engineSave';
        $scope.fuellabelSave = 'pages.automobileAttributes.tabTecAttributes.fuelSave';
        $scope.gvwlabelSave = 'pages.automobileAttributes.tabTecAttributes.gvwSave';
        $scope.transmissionlabelSave = 'pages.automobileAttributes.tabTecAttributes2.transmissionSave';
        $scope.aspirationlabelSave = 'pages.automobileAttributes.tabTecAttributes2.aspirationSave';
        $scope.horsePowerlabelSave = 'pages.automobileAttributes.tabTecAttributes2.horsePowerSave';
        $scope.kiloWattlabelSave = 'pages.automobileAttributes.tabTecAttributes2.kiloWattSave';

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.automobileAttributes.error'), msg);
        };
        var customWarningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('pages.automobileAttributes.warning'), msg, 12000);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', $filter('translate')('pages.automobileAttributes.information'), msg, 12000);
        };

        $scope.loadInitailData = function () { }

        $scope.CylinderCount = {
            Id: "00000000-0000-0000-0000-000000000000",
            Count: ''
        };
        $scope.DriveType = {
            Id: "00000000-0000-0000-0000-000000000000",
            Type: '',
            DriveTypeDescription: ''
        };
        $scope.EngineCapacity = {
            Id: "00000000-0000-0000-0000-000000000000",
            MesureType: '',
            EngineCapacityNumber: ''
        };
        $scope.FuelType = {
            FuelTypeId: "00000000-0000-0000-0000-000000000000",
            FuelTypeCode: '',
            FuelTypeDescription: ''
        };
        $scope.VehicleColor = {
            Id: "00000000-0000-0000-0000-000000000000",
            VehicleColorCode: '',
            Color: ''
        };
        $scope.VehicleBodyType = {
            Id: "00000000-0000-0000-0000-000000000000",
            VehicleBodyTypeCode: '',
            VehicleBodyTypeDescription: ''
        };
        $scope.TransmissionType = {
            Id: "00000000-0000-0000-0000-000000000000",
            TransmissionTypeCode: '',
            Technology: ''
        };
        $scope.VehicleHorsePower = {
            Id: "00000000-0000-0000-0000-000000000000",
            HorsePower: ''
        };
        $scope.VehicleKiloWatt = {
            Id: "00000000-0000-0000-0000-000000000000",
            KiloWatt: ''
        };
        $scope.VehicleAspirationType = {
            Id: "00000000-0000-0000-0000-000000000000",
            AspirationTypeCode: ''
        };

        $scope.VehicleWeight = {
            Id: "00000000-0000-0000-0000-000000000000",
            VehicleWeightDescription: '',
            WeightFrom: 0.00,
            WeightTo: 0.00,
            VehicleWeightCode: ''
        };

        $scope.settings = {
            closeOnBlur: true,
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: true,
            closeOnBlur: false,
            showUncheckAll: true,
            closeOnBlur: true,
            closeOnSelect: true
        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('common.customText.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('common.customText.itemSelected')
        };
        $scope.TechnologyList = [];
        $scope.SelectedTechnologyList = [];
        $scope.SelectedTechnologyDList = [];
        function AddTechnology() {
            var index = 0;
            $scope.TechnologyList = [];
            angular.forEach($scope.Technologies, function (value) {
                var x = { id: index, code: value.Id, label: value.Name };
                $scope.TechnologyList.push(x);
                index = index + 1;
            });
        }
        function LoadTechnology() {
            $scope.SelectedTechnologyList = [];
            $scope.SelectedTechnologyDList = [];
            angular.forEach($scope.TransmissionType.Technology, function (valueOut) {
                angular.forEach($scope.TechnologyList, function (valueIn) {
                    angular.forEach($scope.Technologies, function (value) {
                        if (value.Id === valueIn.code && valueOut === value.Name) {
                            var x = { id: valueIn.id };
                            $scope.SelectedTechnologyList.push(x);
                            $scope.SelectedTechnologyDList.push(valueIn.label);
                        }
                    });
                });
            });
        }
        $scope.SendTechnology = function () {
            $scope.SelectedTechnologyDList = [];
            $scope.TransmissionType.Technology = [];
            angular.forEach($scope.SelectedTechnologyList, function (valueOut) {
                angular.forEach($scope.TechnologyList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.TransmissionType.Technology.push(valueIn.code);
                        $scope.SelectedTechnologyDList.push(valueIn.label);
                    }
                });
            });
        }



        LoadDetails();
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetTransmissionTechnology',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Technologies = data;
                AddTechnology();
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
                $scope.VehicleBodyTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleColors',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.VehicleColors = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.TransmissionTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.VehicleAspirationTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleHorsePowers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.VehicleHorsePowers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleKiloWatts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.VehicleKiloWatts = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleWeight',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.VehicleWeights = data;
            }).error(function (data, status, headers, config) {
            });
        }

        //Cylinder Count//////////////////////////////////////
        $scope.SetCylinderCountValues = function () {
            $scope.errorTab2 = "";
            if ($scope.CylinderCount.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetCylinderCountById',
                    data: { "cylinderCountId": $scope.CylinderCount.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CylinderCount.Id = data.Id;
                    $scope.CylinderCount.Count = parseInt(data.Count);
                    $scope.cylinderlabelSave = 'pages.automobileAttributes.tabTecAttributes.cylinderTypeUpdate';
                }).error(function (data, status, headers, config) {
                    clearCylinderCountControls();
                    //$scope.message = 'Unexpected Error';
                });
            }
            else {
                clearCylinderCountControls();
                $scope.cylinderlabelSave = 'pages.automobileAttributes.tabTecAttributes.cylinderTypeSave';
            }
        }
        function clearCylinderCountControls() {
            $scope.CylinderCount.Count = "";
            $scope.CylinderCount.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearCylinderCountControls();
        function CheckCylinderCountCode() {
            var ret = true;
            angular.forEach($scope.CylinderCounts, function (value) {
                if (value.Count == $scope.CylinderCount.Count && $scope.CylinderCount.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {
                //SweetAlert.swal({
                //    title: "Cylinder Count Information",
                //    text: "Cylinder Count Already Created!",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customWarningMessage($filter('translate')('pages.automobileAttributes.cylinderCountMgs'))
            }
            return ret;
        }

        $scope.validateCylinderCount = function () {
            var isValid = true;
            if ($scope.CylinderCount.Count == null || $scope.CylinderCount.Count == ""
                || $scope.CylinderCount.Count == undefined) {
                $scope.validate_VehicleCylinderCount = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleCylinderCount = "";
            }

            return isValid
        }

        $scope.CylinderCountSubmit = function () {
            if ($scope.validateCylinderCount()) {
                if (CheckCylinderCountCode()) {
                    $scope.errorTab2 = "";
                    if ($scope.CylinderCount.Id == null || $scope.CylinderCount.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.CylinderCountSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CylinderCountSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddCylinderCount',
                            data: $scope.CylinderCount,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CylinderCountSubmitBtnIconClass = "";
                            $scope.CylinderCountSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.cylinderCountInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllCylinderCounts',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CylinderCounts = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearCylinderCountControls();
                            } else {
                                alert(data);
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.cylinderCountInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CylinderCountSubmitBtnIconClass = "";
                            $scope.CylinderCountSubmitBtnDisabled = false;
                            return false;
                        });

                    }
                    else {
                        $scope.CylinderCountSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CylinderCountSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateCylinderCount',
                            data: $scope.CylinderCount,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CylinderCountSubmitBtnIconClass = "";
                            $scope.CylinderCountSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.cylinderCountInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllCylinderCounts',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CylinderCounts = data;
                                }).error(function (data, status, headers, config) {
                                });
                                $scope.cylinderlabelSave = 'pages.automobileAttributes.tabTecAttributes.cylinderTypeSave';
                                clearCylinderCountControls();

                            } else {
                                alert(data);
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.cylinderCountInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CylinderCountSubmitBtnIconClass = "";
                            $scope.CylinderCountSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.enterCylinderCount'))
                //$scope.errorTab2 = "Please Enter Cylinder Count";
            }
        }

        // Drive Type ////////////////////////////
        $scope.SetDriveTypeValues = function () {
            $scope.errorTab1 = "";
            if ($scope.DriveType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetDriveTypeById',
                    data: { "driveTypeId": $scope.DriveType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.DriveType.Id = data.Id;
                    $scope.DriveType.Type = data.Type;
                    $scope.DriveType.DriveTypeDescription = data.DriveTypeDescription;
                    $scope.labelSave = 'pages.automobileAttributes.update';
                }).error(function (data, status, headers, config) {
                    clearDriveTypeControls();
                });
            }
            else {
                $scope.labelSave = 'pages.automobileAttributes.save';
                clearDriveTypeControls();
            }
        }
        function clearDriveTypeControls() {
            $scope.DriveType.Type = "";
            $scope.DriveType.DriveTypeDescription = "";
            $scope.DriveType.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearDriveTypeControls();
        function CheckDriveTypeCode() {
            var ret = true;
            angular.forEach($scope.DriveTypes, function (value) {
                if (value.Type == $scope.DriveType.Type && $scope.DriveType.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {
                customWarningMessage($filter('translate')('pages.automobileAttributes.typeUnique'))
            }
            return ret;
        }



        $scope.validateDriveType = function () {
            var isValid = true;
            if ($scope.DriveType.Type == null || $scope.DriveType.Type == "" || $scope.DriveType.Type == undefined) {
                $scope.validate_DriveType = "has-error";
                isValid = false;
            } else {
                $scope.validate_DriveType = "";
            }

            if ($scope.DriveType.DriveTypeDescription == null || $scope.DriveType.DriveTypeDescription == "" || $scope.DriveType.DriveTypeDescription == undefined) {
                $scope.validate_DriveTypeDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_DriveTypeDescription = "";
            }
            return isValid
        }

        $scope.DriveTypeSubmit = function () {
            if ($scope.validateDriveType()) {
                if (CheckDriveTypeCode()) {
                    $scope.errorTab1 = "";
                    if ($scope.DriveType.Id == null || $scope.DriveType.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.DriveTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.DriveTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddDriveType',
                            data: $scope.DriveType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.DriveTypeSubmitBtnIconClass = "";
                            $scope.DriveTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.driveTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllDriveTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.DriveTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearDriveTypeControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.driveTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.DriveTypeSubmitBtnIconClass = "";
                            $scope.DriveTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.DriveTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.DriveTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateDriveType',
                            data: $scope.DriveType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.DriveTypeSubmitBtnIconClass = "";
                            $scope.DriveTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.driveTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllDriveTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.DriveTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearDriveTypeControls();
                                $scope.labelSave = 'pages.automobileAttributes.save';
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.driveTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.DriveTypeSubmitBtnIconClass = "";
                            $scope.DriveTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                } else {

                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.enterDriveType'))

            }
        }

        // Engine Capacity ////////////////////////////
        $scope.SetEngineCapacityValues = function () {
            $scope.errorTab2 = "";
            if ($scope.EngineCapacity.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetEngineCapacityById',
                    data: { "engineCapacityId": $scope.EngineCapacity.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.EngineCapacity.Id = data.Id;
                    $scope.EngineCapacity.EngineCapacityNumber = data.EngineCapacityNumber;
                    $scope.EngineCapacity.MesureType = data.MesureType;
                    $scope.enginlabelSave = 'pages.automobileAttributes.tabTecAttributes.engineUpdate';
                }).error(function (data, status, headers, config) {
                    clearEngineCapacityControls();
                });
            }
            else {
                clearEngineCapacityControls();
                $scope.enginlabelSave = 'pages.automobileAttributes.tabTecAttributes.engineSave';
            }
        }
        function clearEngineCapacityControls() {
            $scope.EngineCapacity.MesureType = "";
            $scope.EngineCapacity.EngineCapacityNumber = "";
            $scope.EngineCapacity.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearEngineCapacityControls()
        function CheckEngineCapacityCode() {
            var ret = true;
            angular.forEach($scope.EngineCapacities, function (value) {
                if (value.EngineCapacityNumber == $scope.EngineCapacity.EngineCapacityNumber && $scope.EngineCapacity.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.engineNumbeUnique'))
            }
            return ret;
        }

        $scope.validateEngineCapacity = function () {
            var isValid = true;
            if ($scope.EngineCapacity.EngineCapacityNumber == "" || $scope.EngineCapacity.EngineCapacityNumber == undefined
                || $scope.EngineCapacity.EngineCapacityNumber == null) {
                $scope.validate_EngineCapacityNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_EngineCapacityNumber = "";
            }
            if ($scope.EngineCapacity.EngineCapacityNumber == "" || $scope.EngineCapacity.EngineCapacityNumber == undefined
                || $scope.EngineCapacity.EngineCapacityNumber == null) {
                $scope.validate_MesureType = "has-error";
                isValid = false;
            } else {
                $scope.validate_MesureType = "";
            }
            return isValid
        }

        $scope.isValidEngineCapacity = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarEngineCapacityNumber) {
                        customWarningMessage($filter('translate')('pages.automobileAttributes.engineNumberalreadyexists'))
                        //$scope.errorTab2 = "Engine Capacity Number already exists";
                        isValid = false;
                    }
                }
            }
            return isValid
        }
        $scope.EngineCapacitySubmit = function () {
            if ($scope.validateEngineCapacity()) {
                if ($scope.isValidEngineCapacity()) {

                    $scope.errorTab2 = "";
                    if ($scope.EngineCapacity.Id == null || $scope.EngineCapacity.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.EngineCapacitySubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.EngineCapacitySubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddEngineCapacity',
                            data: $scope.EngineCapacity,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.EngineCapacitySubmitBtnIconClass = "";
                            $scope.EngineCapacitySubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.engineCapacityInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.EngineCapacities = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearEngineCapacityControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.engineCapacityInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.EngineCapacitySubmitBtnIconClass = "";
                            $scope.EngineCapacitySubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.EngineCapacitySubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.EngineCapacitySubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateEngineCapacity',
                            data: $scope.EngineCapacity,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.EngineCapacitySubmitBtnIconClass = "";
                            $scope.EngineCapacitySubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.engineCapacityInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.EngineCapacities = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearEngineCapacityControls();
                                $scope.enginlabelSave = 'pages.automobileAttributes.tabTecAttributes.engineSave';
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.engineCapacityInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.EngineCapacitySubmitBtnIconClass = "";
                            $scope.EngineCapacitySubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }

            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
            }
        }

        // Fuel Type ////////////////////////////
        $scope.SetFuelTypeValues = function () {
            $scope.errorTab2 = "";
            if ($scope.FuelType.FuelTypeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetFuelTypeById',
                    data: { "fuelTypeId": $scope.FuelType.FuelTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.FuelType.FuelTypeId = data.FuelTypeId;
                    $scope.FuelType.FuelTypeCode = data.FuelTypeCode;
                    $scope.FuelType.FuelTypeDescription = data.FuelTypeDescription;
                    $scope.fuellabelSave = 'pages.automobileAttributes.tabTecAttributes.fuelUpdate';
                }).error(function (data, status, headers, config) {
                    clearFuelTypeControls();
                });
            }
            else {
                clearFuelTypeControls();
                $scope.fuellabelSave = 'pages.automobileAttributes.tabTecAttributes.fuelSave';
            }
        }
        function clearFuelTypeControls() {
            $scope.FuelType.FuelTypeCode = "";
            $scope.FuelType.FuelTypeDescription = "";
            $scope.FuelType.FuelTypeId = "00000000-0000-0000-0000-000000000000";
        }
        clearFuelTypeControls();
        function CheckFuelTypeCode() {
            var ret = true;
            angular.forEach($scope.FuelTypes, function (value) {
                if (value.FuelTypeCode == $scope.FuelType.FuelTypeCode && $scope.FuelType.FuelTypeId != value.FuelTypeId) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))
            }
            return ret;
        }

        $scope.validateFuelType = function () {
            var isValid = true;
            if ($scope.FuelType.FuelTypeDescription == "" || $scope.FuelType.FuelTypeDescription == undefined || $scope.FuelType.FuelTypeDescription == null) {
                $scope.validate_FuelTypeDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_FuelTypeDescription = "";
            }

            if ($scope.FuelType.FuelTypeCode == "" || $scope.FuelType.FuelTypeCode == undefined || $scope.FuelType.FuelTypeCode == null) {
                $scope.validate_FuelTypeCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_FuelTypeCode = "";
            }

            return isValid
        }

        $scope.FuelTypeSubmit = function () {
            if ($scope.validateFuelType()) {
                if (CheckFuelTypeCode()) {
                    $scope.errorTab2 = "";
                    if ($scope.FuelType.FuelTypeId == null || $scope.FuelType.FuelTypeId == "00000000-0000-0000-0000-000000000000") {
                        $scope.FuelTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.FuelTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddFuelType',
                            data: $scope.FuelType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.FuelTypeSubmitBtnIconClass = "";
                            $scope.FuelTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.fuelTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllFuelTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.FuelTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearFuelTypeControls();
                            } else {
                                alert(data);
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.fuelTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.FuelTypeSubmitBtnIconClass = "";
                            $scope.FuelTypeSubmitBtnDisabled = false;
                            return false;
                        });

                    }
                    else {
                        $scope.FuelTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.FuelTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateFuelType',
                            data: $scope.FuelType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.FuelTypeSubmitBtnIconClass = "";
                            $scope.FuelTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.fuelTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllFuelTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.FuelTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearFuelTypeControls();
                                $scope.fuellabelSave = 'pages.automobileAttributes.tabTecAttributes.fuelUpdate';
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.fuelTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.FuelTypeSubmitBtnIconClass = "";
                            $scope.FuelTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
                // $scope.errorTab2 = "Please Enter a Fuel type Code";
            }
        }

        // Vehicle Body Type ////////////////////////////
        $scope.SetVehicleBodyTypeValues = function () {
            $scope.errorTab1 = "";
            if ($scope.VehicleBodyType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleBodyTypeById',
                    data: { "vehicleBodyTypeId": $scope.VehicleBodyType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleBodyType.Id = data.Id;
                    $scope.VehicleBodyType.VehicleBodyTypeCode = data.VehicleBodyTypeCode;
                    $scope.VehicleBodyType.VehicleBodyTypeDescription = data.VehicleBodyTypeDescription;
                    $scope.bodyTypelabelSave = 'pages.automobileAttributes.tabGeneralAttributes.bodyTypeUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleBodyTypeControls();
                });
            }
            else {
                $scope.bodyTypelabelSave = 'pages.automobileAttributes.tabGeneralAttributes.bodyTypeSave';
                clearVehicleBodyTypeControls();
            }
        }
        function clearVehicleBodyTypeControls() {
            $scope.VehicleBodyType.VehicleBodyTypeCode = "";
            $scope.VehicleBodyType.VehicleBodyTypeDescription = "";
            $scope.VehicleBodyType.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearVehicleBodyTypeControls();
        function CheckBodyTypeCode() {
            var ret = true;
            angular.forEach($scope.VehicleBodyTypes, function (value) {
                if (value.VehicleBodyTypeCode == $scope.VehicleBodyType.VehicleBodyTypeCode && $scope.VehicleBodyType.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))

            }
            return ret;
        }

        $scope.validateVehicleBodyType = function () {
            var isValid = true;
            if ($scope.VehicleBodyType.VehicleBodyTypeCode == null || $scope.VehicleBodyType.VehicleBodyTypeCode == ""
                || $scope.VehicleBodyType.VehicleBodyTypeCode == undefined) {
                $scope.validate_VehicleBodyType = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleBodyType = "";
            }

            if ($scope.VehicleBodyType.VehicleBodyTypeDescription == null || $scope.VehicleBodyType.VehicleBodyTypeDescription == ""
                || $scope.VehicleBodyType.VehicleBodyTypeDescription == undefined) {
                $scope.validate_VehicleBodyTypeDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleBodyTypeDescription = "";
            }
            return isValid
        }

        $scope.VehicleBodyTypeSubmit = function () {
            if ($scope.validateVehicleBodyType()) {
                if (CheckBodyTypeCode()) {
                    $scope.errorTab1 = "";
                    if ($scope.VehicleBodyType.Id == null || $scope.VehicleBodyType.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleBodyTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleBodyTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddVehicleBodyType',
                            data: $scope.VehicleBodyType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleBodyTypeSubmitBtnIconClass = "";
                            $scope.VehicleBodyTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleBodyTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleBodyTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleBodyTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleBodyTypeControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleBodyTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleBodyTypeSubmitBtnIconClass = "";
                            $scope.VehicleBodyTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.VehicleBodyTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleBodyTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateVehicleBodyType',
                            data: $scope.VehicleBodyType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleBodyTypeSubmitBtnIconClass = "";
                            $scope.VehicleBodyTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleBodyTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleBodyTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleBodyTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleBodyTypeControls();
                                $scope.bodyTypelabelSave = 'pages.automobileAttributes.tabGeneralAttributes.bodyTypeSave';
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleBodyTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleBodyTypeSubmitBtnIconClass = "";
                            $scope.VehicleBodyTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            }
            else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.bothBodyTypeandBodyTypeCode'))
                //$scope.errorTab1 = "Please Enter both Body Type and Body Type Code";
            }
        }

        // Vehicle Color ////////////////////////////
        $scope.SetVehicleColorValues = function () {
            if ($scope.VehicleColor.Id != null) {
                $scope.errorTab1 = "";
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleColorById',
                    data: { "vehicleColorId": $scope.VehicleColor.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleColor.Id = data.Id;
                    $scope.VehicleColor.VehicleColorCode = data.VehicleColorCode;
                    $scope.VehicleColor.Color = data.Color;
                    $scope.colorlabelSave = 'pages.automobileAttributes.tabGeneralAttributes.colorTypeUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleColorControls();
                });
            }
            else {
                $scope.colorlabelSave = 'pages.automobileAttributes.tabGeneralAttributes.colorTypeSave';
                clearVehicleColorControls();
            }
        }
        function clearVehicleColorControls() {
            $scope.VehicleColor.Color = "";
            $scope.VehicleColor.VehicleColorCode = "";
            $scope.VehicleColor.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearVehicleColorControls()
        function CheckColorCode() {
            var ret = true;
            angular.forEach($scope.VehicleColors, function (value) {
                if (value.VehicleColorCode == $scope.VehicleColor.VehicleColorCode && $scope.VehicleColor.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))

                return ret;
            }
            else {
                return ret;
            }
        }

        $scope.validateVehicleColor = function () {
            var isValid = true;
            if ($scope.VehicleColor.VehicleColorCode == null || $scope.VehicleColor.VehicleColorCode == ""
                || $scope.VehicleColor.VehicleColorCode == undefined) {
                $scope.validate_VehicleColorCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleColorCode = "";
            }

            if ($scope.VehicleColor.Color == null || $scope.VehicleColor.Color == ""
                || $scope.VehicleColor.Color == undefined) {
                $scope.validate_VehicleColor = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleColor = "";
            }
            return isValid
        }

        $scope.VehicleColorSubmit = function () {
            if ($scope.validateVehicleColor()) {
                if (CheckColorCode()) {
                    $scope.errorTab1 = "";
                    if ($scope.VehicleColor.Id == null || $scope.VehicleColor.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleColorSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleColorSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddVehicleColor',
                            data: $scope.VehicleColor,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleColorSubmitBtnIconClass = "";
                            $scope.VehicleColorSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleColorInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleColors',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleColors = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleColorControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleColorInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleColorSubmitBtnIconClass = "";
                            $scope.VehicleColorSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.VehicleColorSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleColorSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateVehicleColor',
                            data: $scope.VehicleColor,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleColorSubmitBtnIconClass = "";
                            $scope.VehicleColorSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleColorInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleColors',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleColors = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleColorControls();
                                $scope.colorlabelSave = 'pages.automobileAttributes.tabGeneralAttributes.colorTypeSave';
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleColorInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleColorSubmitBtnIconClass = "";
                            $scope.VehicleColorSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.bothColorCodeandColor'))
                //$scope.errorTab1 = "Please Enter both Color Code and Color";
            }
        }

        //Transmission Type//////////////////////////////////////
        $scope.SetTransmissionTypeValues = function () {
            $scope.errorTab3 = "";
            if ($scope.TransmissionType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetTransmissionTypeById',
                    data: { "TransmissionTypeId": $scope.TransmissionType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.TransmissionType.Id = data.Id;
                    $scope.TransmissionType.TransmissionTypeCode = data.TransmissionTypeCode;

                    $scope.TransmissionType.Technology = data.Technology;
                    $scope.transmissionlabelSave = 'pages.automobileAttributes.tabTecAttributes2.transmissionUpdate';
                    LoadTechnology();

                }).error(function (data, status, headers, config) {
                    clearTransmissionTypeControls();
                });
            }
            else {
                clearTransmissionTypeControls();
                $scope.transmissionlabelSave = 'pages.automobileAttributes.tabTecAttributes2.transmissionSave';
            }
        }
        function clearTransmissionTypeControls() {
            $scope.TransmissionType.Technology = [];
            $scope.TransmissionType.TransmissionTypeCode = [];
            $scope.TransmissionType.Id = "00000000-0000-0000-0000-000000000000";
            $scope.SelectedTechnologyList = [];
            $scope.SelectedTechnologyDList = [];
        }
        clearTransmissionTypeControls();
        function CheckTransmissionTypeCode() {
            var ret = true;
            angular.forEach($scope.TransmissionTypes, function (value) {
                if (value.TransmissionTypeCode == $scope.TransmissionType.TransmissionTypeCode && $scope.TransmissionType.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))
            }
            return ret;
        }

        $scope.validateTransmissionType = function () {
            var isValid = true;
            if ($scope.TransmissionType.TransmissionTypeCode == "" || $scope.TransmissionType.TransmissionTypeCode == undefined ||
                $scope.TransmissionType.TransmissionTypeCode == null) {
                $scope.validate_TransmissionTypeCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_TransmissionTypeCode = "";
            }

            return isValid
        }
        $scope.TransmissionTypeSubmit = function () {
            if ($scope.validateTransmissionType()) {
               // if (CheckTransmissionTypeCode()) {
                    $scope.errorTab2 = "";
                if ($scope.TransmissionType.Id == null || $scope.TransmissionType.Id == "00000000-0000-0000-0000-000000000000") {
                    if (CheckTransmissionTypeCode()) {
                        $scope.TransmissionTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.TransmissionTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddTransmissionType',
                            data: $scope.TransmissionType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.TransmissionTypeSubmitBtnIconClass = "";
                            $scope.TransmissionTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.transmissionTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.TransmissionTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearTransmissionTypeControls();
                            } else {
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.transmissionTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.TransmissionTypeSubmitBtnIconClass = "";
                            $scope.TransmissionTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }

                    }
                    else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateTransmissionType',
                            data: $scope.TransmissionType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.TransmissionTypeSubmitBtnIconClass = "";
                            $scope.TransmissionTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.transmissionTypeInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllTransmissionTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.TransmissionTypes = data;
                                }).error(function (data, status, headers, config) {
                                    //$scope.message = 'Unexpected Error';
                                });
                                clearTransmissionTypeControls();
                                $scope.transmissionlabelSave = 'pages.automobileAttributes.tabTecAttributes2.transmissionSave';
                            } else {
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.transmissionTypeInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.TransmissionTypeSubmitBtnIconClass = "";
                            $scope.TransmissionTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
               // }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.enterTransmissionType'))
                //$scope.errorTab3 = "Please Enter Transmission Type";
            }
        }

        //Vehicle Aspiration Type//////////////////////////////////////
        $scope.SetVehicleAspirationTypeValues = function () {
            $scope.errorTab3 = "";
            if ($scope.VehicleAspirationType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleAspirationTypeById',
                    data: { "VehicleAspirationTypeId": $scope.VehicleAspirationType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleAspirationType.Id = data.Id;
                    $scope.VehicleAspirationType.AspirationTypeCode = data.AspirationTypeCode;
                    $scope.aspirationlabelSave = 'pages.automobileAttributes.tabTecAttributes2.aspirationUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleAspirationTypeControls();
                });
            }
            else {
                clearVehicleAspirationTypeControls();
                $scope.aspirationlabelSave = 'pages.automobileAttributes.tabTecAttributes2.aspirationSave';
            }
        }
        function clearVehicleAspirationTypeControls() {
            $scope.VehicleAspirationType.AspirationTypeCode = "";
            $scope.VehicleAspirationType.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearVehicleAspirationTypeControls();
        function CheckAspirationTypeCode() {
            var ret = true;
            angular.forEach($scope.VehicleAspirationTypes, function (value) {
                if (value.AspirationTypeCode == $scope.VehicleAspirationType.AspirationTypeCode && $scope.VehicleAspirationType.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))
            }
            return ret;
        }

        $scope.validateVehicleAspirationType = function () {
            var isValid = true;
            if ($scope.VehicleAspirationType.AspirationTypeCode == "" || $scope.VehicleAspirationType.AspirationTypeCode == undefined ||
                $scope.VehicleAspirationType.AspirationTypeCode == null) {
                $scope.validate_AspirationTypeCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_AspirationTypeCode = "";
            }
            return isValid
        }

        $scope.isValidVehicleAspirationType = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarAspirationTypeCode) {
                        customWarningMessage($filter('translate')('pages.automobileAttributes.aspirationTypeCodealreadyexists'))
                        //$scope.errorTab3 = "Aspiration Type Code already exists";
                        isValid = false;
                    }
                }
            }
            return isValid
        }

        $scope.VehicleAspirationTypeSubmit = function () {
            if ($scope.validateVehicleAspirationType()) {
                if ($scope.isValidVehicleAspirationType()) {
                    $scope.errorTab2 = "";
                    if ($scope.VehicleAspirationType.Id == null || $scope.VehicleAspirationType.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleAspirationTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleAspirationTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddVehicleAspirationType',
                            data: $scope.VehicleAspirationType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleAspirationTypeSubmitBtnIconClass = "";
                            $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                //alert("Product Saved!");

                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleAspirationInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleAspirationTypes = data;
                                }).error(function (data, status, headers, config) {
                                    //$scope.message = 'Unexpected Error';
                                });
                                clearVehicleAspirationTypeControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            //$scope.message = 'Unexpected Error';
                            //alert("Error occured while saving data!");
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleAspirationInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleAspirationTypeSubmitBtnIconClass = "";
                            $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                            return false;
                        });

                    }
                    else {
                        $scope.VehicleAspirationTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleAspirationTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateVehicleAspirationType',
                            data: $scope.VehicleAspirationType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleAspirationTypeSubmitBtnIconClass = "";
                            $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                //alert("Product Saved!");
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleAspirationInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleAspirationTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleAspirationTypes = data;
                                }).error(function (data, status, headers, config) {
                                    //$scope.message = 'Unexpected Error';
                                });
                                $scope.aspirationlabelSave = 'pages.automobileAttributes.tabTecAttributes2.aspirationSave'
                                clearVehicleAspirationTypeControls();

                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            //$scope.message = 'Unexpected Error';
                            //alert("Error occured while saving data!");
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleAspirationInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleAspirationTypeSubmitBtnIconClass = "";
                            $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
            }
        }

        //Vehicle Horse Power//////////////////////////////////////
        $scope.SetVehicleHorsePowerValues = function () {
            $scope.errorTab3 = "";
            if ($scope.VehicleHorsePower.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleHorsePowerById',
                    data: { "VehicleHorsePowerId": $scope.VehicleHorsePower.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleHorsePower.Id = data.Id;
                    $scope.VehicleHorsePower.HorsePower = data.HorsePower;
                    $scope.horsePowerlabelSave = 'pages.automobileAttributes.tabTecAttributes2.horsePowerUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleHorsePowerControls();
                });
            }
            else {
                clearVehicleHorsePowerControls();
                $scope.horsePowerlabelSave = 'pages.automobileAttributes.tabTecAttributes2.horsePowerSave';
            }
        }
        function clearVehicleHorsePowerControls() {
            $scope.VehicleHorsePower.HorsePower = "";
            $scope.VehicleHorsePower.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearVehicleHorsePowerControls();
        function CheckHorsePower() {
            var ret = true;
            angular.forEach($scope.VehicleHorsePowers, function (value) {
                if (value.HorsePower == $scope.VehicleHorsePower.HorsePower && $scope.VehicleHorsePower.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))
            }
            return ret;
        }

        $scope.validateVehicleHorsePower = function () {
            var isValid = true;
            if ($scope.VehicleHorsePower.HorsePower == "" || $scope.VehicleHorsePower.HorsePower == undefined ||
                $scope.VehicleHorsePower.HorsePower == null) {
                $scope.validate_HorsePower = "has-error";
                isValid = false;
            } else {
                $scope.validate_HorsePower = "";
            }
            return isValid
        }

        $scope.isValidVehicleHorsePower = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarHorsePowerByHorse) {
                        customWarningMessage($filter('translate')('pages.automobileAttributes.horsePowerCodealreadyexists'))
                        //$scope.errorTab3 = "Horse Power Code already exists";
                        isValid = false;
                    }
                }
            }
            return isValid
        }

        $scope.VehicleHorsePowerSubmit = function () {
            if ($scope.validateVehicleHorsePower()) {
                if ($scope.isValidVehicleHorsePower()) {
                    $scope.errorTab2 = "";
                    if ($scope.VehicleHorsePower.Id == null || $scope.VehicleHorsePower.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleHorsePowerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleHorsePowerSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddVehicleHorsePower',
                            data: $scope.VehicleHorsePower,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleHorsePowerSubmitBtnIconClass = "";
                            $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleHorsePowerInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleHorsePowers',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleHorsePowers = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleHorsePowerControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleHorsePowerInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleHorsePowerSubmitBtnIconClass = "";
                            $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.VehicleHorsePowerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleHorsePowerSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateVehicleHorsePower',
                            data: $scope.VehicleHorsePower,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleHorsePowerSubmitBtnIconClass = "";
                            $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleHorsePowerInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleHorsePowers',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleHorsePowers = data;
                                }).error(function (data, status, headers, config) {
                                });
                                $scope.horsePowerlabelSave = 'pages.automobileAttributes.tabTecAttributes2.horsePowerSave';
                                clearVehicleHorsePowerControls();

                            } else {
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleHorsePowerInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleHorsePowerSubmitBtnIconClass = "";
                            $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
            }
        }

        //Vehicle Kilo Watt//////////////////////////////////////
        $scope.SetVehicleKiloWattValues = function () {
            $scope.errorTab3 = "";
            if ($scope.VehicleKiloWatt.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleKiloWattById',
                    data: { "VehicleKiloWattId": $scope.VehicleKiloWatt.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleKiloWatt.Id = data.Id;
                    $scope.VehicleKiloWatt.KiloWatt = data.KiloWatt;
                    $scope.kiloWattlabelSave = 'pages.automobileAttributes.tabTecAttributes2.kiloWattUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleKiloWattControls();
                });
            }
            else {
                clearVehicleKiloWattControls();
                $scope.kiloWattlabelSave = 'pages.automobileAttributes.tabTecAttributes2.kiloWattSave';
            }
        }
        function clearVehicleKiloWattControls() {
            $scope.VehicleKiloWatt.KiloWatt = "";
            $scope.VehicleKiloWatt.Id = "00000000-0000-0000-0000-000000000000";
        }
        clearVehicleKiloWattControls();
        function CheckKiloWatt() {
            var ret = true;
            angular.forEach($scope.VehicleKiloWatts, function (value) {
                if (value.KiloWatt == $scope.VehicleKiloWatt.KiloWatt && $scope.VehicleKiloWatt.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {

                customWarningMessage($filter('translate')('pages.automobileAttributes.codeUnique'))
            }
            return ret;
        }

        $scope.validateVehicleKiloWatt = function () {
            var isValid = true;
            if ($scope.VehicleKiloWatt.KiloWatt == "" || $scope.VehicleKiloWatt.KiloWatt == undefined ||
                $scope.VehicleKiloWatt.KiloWatt == null) {
                $scope.validate_KiloWatt = "has-error";
                isValid = false;
            } else {
                $scope.validate_KiloWatt = "";
            }
            return isValid
        }

        $scope.isValidVehicleKiloWatt = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarVehicleKiloWattByKiloWatt) {
                        customWarningMessage($filter('translate')('pages.automobileAttributes.kiloWattCodealreadyexists'))
                        isValid = false;
                    }
                }
            }
            return isValid
        }

        $scope.VehicleKiloWattSubmit = function () {
            if ($scope.validateVehicleKiloWatt()) {
                if ($scope.isValidVehicleKiloWatt()) {
                    $scope.errorTab2 = "";
                    if ($scope.VehicleKiloWatt.Id == null || $scope.VehicleKiloWatt.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.VehicleKiloWattBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleKiloWattBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/AddVehicleKiloWatt',
                            data: $scope.VehicleKiloWatt,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleKiloWattBtnIconClass = "";
                            $scope.VehicleKiloWattBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleKiloWattInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleKiloWatts',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleKiloWatts = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearVehicleKiloWattControls();
                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleKiloWattInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleKiloWattBtnIconClass = "";
                            $scope.VehicleKiloWattBtnDisabled = false;
                            return false;
                        });

                    }
                    else {
                        $scope.VehicleKiloWattBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.VehicleKiloWattBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/UpdateVehicleKiloWatt',
                            data: $scope.VehicleKiloWatt,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.VehicleKiloWattBtnIconClass = "";
                            $scope.VehicleKiloWattBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.automobileAttributes.vehicleKiloWattInformation'),
                                    text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleKiloWatts',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.VehicleKiloWatts = data;
                                }).error(function (data, status, headers, config) {
                                });
                                $scope.kiloWattlabelSave = 'pages.automobileAttributes.tabTecAttributes2.kiloWattSave';
                                clearVehicleKiloWattControls();

                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleKiloWattInformation'),
                                text: $filter('translate')('pages.automobileAttributes.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.VehicleKiloWattBtnIconClass = "";
                            $scope.VehicleKiloWattBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
            }
        }


        $scope.IsExsistingEngineCapacityByEngineCapacityNumber = function () {
            $scope.errorTab1 = "";
            $scope.EngineCapacitySubmitBtnDisabled = true;
            if ($scope.EngineCapacity.Id != null && $scope.EngineCapacity.EngineCapacityNumber != undefined || $scope.EngineCapacity.EngineCapacityNumber.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/AutomobileAttributes/IsExsistingEngineCapacityByEngineCapacityNumber',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.EngineCapacity.Id, "EngineCapacityNumber": $scope.EngineCapacity.EngineCapacityNumber }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarEngineCapacityNumber = data;
                    $scope.EngineCapacitySubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.EngineCapacitySubmitBtnDisabled = false;
                });
            }
        }






        $scope.IsExsistingAspirationTypesByCode = function () {
            $scope.errorTab1 = "";
            $scope.VehicleAspirationTypeSubmitBtnDisabled = true;
            if ($scope.VehicleAspirationType.Id != null && $scope.VehicleAspirationType.AspirationTypeCode != undefined || $scope.VehicleAspirationType.AspirationTypeCode.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/AutomobileAttributes/IsExsistingAspirationTypesByCode',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.VehicleAspirationType.Id, "AspirationTypeCode": $scope.VehicleAspirationType.AspirationTypeCode }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarAspirationTypeCode = data;
                    $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.VehicleAspirationTypeSubmitBtnDisabled = false;
                });
            }
        }


        $scope.IsExsistingHorsePowerByHorsePower = function () {
            $scope.errorTab1 = "";
            $scope.VehicleHorsePowerSubmitBtnDisabled = true;
            if ($scope.VehicleHorsePower.Id != null && $scope.VehicleHorsePower.HorsePower != undefined || $scope.VehicleHorsePower.HorsePower.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/AutomobileAttributes/IsExsistingHorsePowerByHorsePower',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.VehicleHorsePower.Id, "HorsePower": $scope.VehicleHorsePower.HorsePower }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarHorsePowerByHorse = data;
                    $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.VehicleHorsePowerSubmitBtnDisabled = false;
                });
            }
        }


        $scope.IsExsistingVehicleKiloWattByKiloWatt = function () {
            $scope.errorTab1 = "";
            $scope.VehicleKiloWattBtnDisabled = true;
            if ($scope.VehicleKiloWatt.Id != null && $scope.VehicleKiloWatt.KiloWatt != undefined || $scope.VehicleKiloWatt.KiloWatt.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/AutomobileAttributes/IsExsistingVehicleKiloWattByKiloWatt',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.VehicleKiloWatt.Id, "KiloWatt": $scope.VehicleKiloWatt.KiloWatt }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarVehicleKiloWattByKiloWatt = data;
                    $scope.VehicleKiloWattBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.VehicleKiloWattBtnDisabled = false;
                });
            }
        }



        // Vehicle Weight------------

        $scope.validateVehicleWeight = function () {
            var isValid = true;
            if ($scope.VehicleWeight.VehicleWeightDescription == "" || $scope.VehicleWeight.VehicleWeightDescription == undefined || $scope.VehicleWeight.VehicleWeightDescription == null) {
                $scope.validate_VehicleWeightDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleWeightDescription = "";
            }

            if (!parseFloat($scope.VehicleWeight.WeightFrom) || $scope.VehicleWeight.WeightFrom === 0) {
                $scope.validate_VehicleWeightDescription = "has-error";
                customErrorMessage($filter('translate')('pages.automobileAttributes.minimumallowedvalue'));
                isValid = false;
            } else {
                $scope.validate_VehicleWeightDescription = "";
            }
            //if ($scope.VehicleWeight.WeightFrom == "" || $scope.VehicleWeight.WeightFrom == undefined || $scope.VehicleWeight.WeightFrom == null) {
            //    $scope.validate_WeightFrom = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_WeightFrom = "";
            //}
            if ($scope.VehicleWeight.VehicleWeightCode == "" || $scope.VehicleWeight.VehicleWeightCode == undefined || $scope.VehicleWeight.VehicleWeightCode == null) {
                $scope.validate_VehicleWeightCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_VehicleWeightCode = "";
            }
            //if ($scope.VehicleWeight.WeightTo == "" || $scope.VehicleWeight.WeightTo == undefined || $scope.VehicleWeight.WeightTo == null) {
            //    $scope.validate_WeightTo = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_WeightTo = "";
            //}

            return isValid
        }

        $scope.VehicleWeightSubmit = function () {
            if ($scope.validateVehicleWeight()) {

                //swal({ title: 'Saving Changes...', text: $filter('translate')('pages.automobileAttributes.vehicleweightInformation'), showConfirmButton: false });
                //$scope.VehicleWeight.userId = $localStorage.LoggedInUserId;

                $scope.VehicleWeightSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.VehicleWeightSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/SubmitVehicleWeight',
                    data: $scope.VehicleWeight,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.VehicleWeightSubmitBtnIconClass = "";
                    $scope.VehicleWeightSubmitBtnDisabled = false;
                    if (data == "ok") {

                            SweetAlert.swal({
                                title: $filter('translate')('pages.automobileAttributes.vehicleweightInformation'),
                                text: $filter('translate')('pages.automobileAttributes.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleWeight',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.VehicleWeights = data;
                        }).error(function (data, status, headers, config) {
                        });
                        clearVehicleWeightControls();
                    } else {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.automobileAttributes.vehicleweightInformation'),
                            text: data,
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                });

            } else {
                customErrorMessage($filter('translate')('pages.automobileAttributes.fillvalidfeild'))
            }
        }

        function clearVehicleWeightControls() {
            $scope.VehicleWeight.WeightFrom = 0.00;
            $scope.VehicleWeight.WeightTo = 0.00;
            $scope.VehicleWeight.VehicleWeightDescription = "";
            $scope.VehicleWeight.VehicleWeightCode = "";
            $scope.VehicleWeight.Id = "00000000-0000-0000-0000-000000000000";
        }

        $scope.disableToCol = function () {
            //if ($scope.VehicleWeight.WeightFrom == null || $scope.VehicleWeight.WeightFrom == "" || $scope.VehicleWeight.WeightFrom == undefined) {
            //    $scope.validate_WeightFrom = "has-error";
            //    customErrorMessage("Please fill valid data for highlighted fields.")
            //    return false;
            //}
            if ($scope.VehicleWeight.IsGraterThan == true) {

                $scope.disableTo = true;

                $scope.VehicleWeight.VehicleWeightDescription = $scope.VehicleWeight.WeightFrom + " Above  ";
                $scope.VehicleWeight.WeightTo = 0;

            } else {
                $scope.VehicleWeight.VehicleWeightDescription = $scope.VehicleWeight.WeightFrom;
                $scope.disableTo = false;

            }
        }

        $scope.CreateVehicleWeightDescription = function () {
            //if ($scope.VehicleWeight.WeightFrom == null || $scope.VehicleWeight.WeightFrom == "" || $scope.VehicleWeight.WeightFrom == undefined) {
            //    $scope.validate_WeightFrom = "has-error";
            //    customErrorMessage("Please fill valid data for highlighted fields.")
            //    return false;
            //}

            if ($scope.VehicleWeight.WeightFrom != null || $scope.VehicleWeight.WeightFrom != undefined ||
                $scope.VehicleWeight.WeightTo != null || $scope.VehicleWeight.WeightTo != undefined) {

                $scope.VehicleWeight.VehicleWeightDescription = $scope.VehicleWeight.WeightFrom + " T To " + $scope.VehicleWeight.WeightTo + "T";
            }
        }

        $scope.SetVehicleWeightValues = function () {

            if ($scope.VehicleWeight.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/AutomobileAttributes/GetVehicleWeightById',
                    data: { "Id": $scope.VehicleWeight.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.VehicleWeight.Id = data.Id;
                    $scope.VehicleWeight.VehicleWeightDescription = data.VehicleWeightDescription;
                    $scope.VehicleWeight.VehicleWeightCode = data.VehicleWeightCode;
                    $scope.VehicleWeight.WeightFrom = data.WeightFrom;
                    $scope.VehicleWeight.WeightTo = data.WeightTo;
                    $scope.gvwlabelSave = 'pages.automobileAttributes.tabTecAttributes.gvwUpdate';
                }).error(function (data, status, headers, config) {
                    clearVehicleWeightControls();
                });
            }
            else {
                clearVehicleWeightControls();
                $scope.gvwlabelSave = 'pages.automobileAttributes.tabTecAttributes.gvwSave';
            }
        }

    });
