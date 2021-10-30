app.controller('ManufacturerManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Manufacturer Management";
        $scope.ModalDescription = "Add Edit Manufacturer Manufacturer Information";
        $scope.CommodityTypeList = [];
        $scope.SelectedCommodityTypeList = [];
        $scope.SelectedCommodityTypeDList = [];
        $scope.errorTab1 = "";
        $scope.labelSave = 'pages.manufacturerManagement.save';

        $scope.ManufacturerSubmitBtnIconClass = "";
        $scope.ManufacturerSubmitBtnDisabled = false;

        $scope.settings = {
            scrollableHeight: '100px',
            scrollable: false,
            closeOnBlur: true,
            showCheckAll: false,
            showUncheckAll: false
          
        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('pages.manufacturerManagement.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('pages.manufacturerManagement.itemSelected')
        };

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.manufacturerManagement.error'), msg);
        };
        $scope.loadInitailData = function () { }
        $scope.Manufacturer = {
            Id: "00000000-0000-0000-0000-000000000000",
            ManufacturerCode: '',
            ManufacturerName: '',
            ManufacturerId: '',
            ComodityTypeId:'',
            ComodityTypes: [],
            IsWarrentyGiven: false,
           // ManufacturerClassId:'',
            IsActive: false
        };           

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.Manufacturer.CommodityTypes = data;
            $scope.CommodityTypes = data;
                AddCommodityType();
            }).error(function (data, status, headers, config) {
            });
              
        }
        LoadDetails();

        $scope.LoadManufacturers = function()
        {
            clearManufacturerControls();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Manufacturer/GetAllManufacturers',
                data: { "Id": $scope.CommodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Manufactures = data;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.SetManufacturerValues = function () {
            $scope.errorTab1 = "";
            if ($scope.Manufacturer.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Manufacturer/GetManufacturerById',
                    data: { "Id": $scope.Manufacturer.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    $scope.Manufacturer.Id = data.Id;
                    $scope.Manufacturer.ManufacturerCode = data.ManufacturerCode;
                    $scope.Manufacturer.ManufacturerName = data.ManufacturerName;
                    $scope.Manufacturer.ManufacturerId = data.ManufacturerId;
                    $scope.Manufacturer.ComodityTypes = data.ComodityTypes;
                    $scope.Manufacturer.IsWarrentyGiven = data.IsWarrentyGiven;
                    $scope.Manufacturer.IsActive = data.IsActive;
                LoadCommodityType();
                $scope.labelSave = 'pages.manufacturerManagement.update';
                   // $scope.Manufacturer.ManufacturerClassId = data.ManufacturerClassId;
                }).error(function (data, status, headers, config) {
                    clearManufacturerControls();

                });
            }
            else {
                clearManufacturerControls();
                $scope.labelSave = 'pages.manufacturerManagement.save';
            }
        }
        clearManufacturerControls();
        function clearManufacturerControls() {
            $scope.Manufacturer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Manufacturer.ManufacturerCode = "";
            $scope.Manufacturer.ManufacturerName = "";
            $scope.Manufacturer.ManufacturerId = "";
            $scope.Manufacturer.ComodityTypeId = "";
            $scope.Manufacturer.IsWarrentyGiven = "";
            $scope.Manufacturer.IsActive = "";
            // $scope.Manufacturer.ManufacturerClassId = "";
            $scope.SelectedCommodityTypeList = [];
            $scope.SelectedCommodityTypeDList = [];
            $scope.Manufacturer.ComodityTypes = [];

        }
        function CheckManufacturer() {
            var ret = true;
            angular.forEach($scope.Manufactures, function (value) {
                if ((value.ManufacturerName == $scope.Manufacturer.ManufacturerName || value.ManufacturerCode == $scope.Manufacturer.ManufacturerCode) && $scope.Manufacturer.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {
                SweetAlert.swal({
                    title: $filter('translate')('pages.manufacturerManagement.manufacturerInformation'),
                    text: $filter('translate')('pages.manufacturerManagement.codeNameUnique'),
                    type: "warning",
                    confirmButtonText: $filter('translate')('pages.manufacturerManagement.ok'),
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                return ret;
            }
            else {
                return ret;
            }
        }

        $scope.validateManufacturer = function () {

            var isValid = true;

            if ($scope.CommodityTypeId == "00000000-0000-0000-0000-000000000000" || $scope.CommodityTypeId == undefined) {
                $scope.validate_CommodityTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityTypeId = "";
            }

            if ($scope.Manufacturer.ManufacturerName == "" || $scope.Manufacturer.ManufacturerName == undefined) {
                $scope.validate_ManufacturerName = "has-error";
                isValid = false;
            } else {
                $scope.validate_ManufacturerName = "";
            }
            if ($scope.Manufacturer.ManufacturerCode == "" || $scope.Manufacturer.ManufacturerCode == undefined) {
                $scope.validate_ManufacturerCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_ManufacturerCode = "";
            }
            if ($scope.Manufacturer.ComodityTypes.length == 0) {
                $scope.validate_ComodityTypes = "has-error";
                isValid = false;                
            } else {
                $scope.validate_ComodityTypes = "";                
            }

            return isValid
        }
        $scope.ManufacturerSubmit = function () {
            $scope.SendCommodityType();
            if ($scope.validateManufacturer()) {


                $scope.errorTab1 = "";
                if (!CheckManufacturer()) {
                    return false;
                }
                if ($scope.Manufacturer.Id == null || $scope.Manufacturer.Id == "00000000-0000-0000-0000-000000000000") {
                    if ($scope.Manufacturer.IsWarrentyGiven == "") {
                        $scope.Manufacturer.IsWarrentyGiven = false;
                    }

                    if ($scope.Manufacturer.IsActive == "") {
                        $scope.Manufacturer.IsActive = false;
                    }

                    $scope.ManufacturerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ManufacturerSubmitBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Manufacturer/AddManufacturer',
                        data: $scope.Manufacturer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ManufacturerSubmitBtnIconClass = "";
                        $scope.ManufacturerSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.manufacturerManagement.manufacturerInformation'),
                                text: $filter('translate')('pages.manufacturerManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.manufacturerManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Manufacturer/GetAllManufacturers',
                                data: { "Id": $scope.CommodityTypeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Manufactures = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearManufacturerControls();
                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.manufacturerManagement.manufacturerInformation'),
                            text: $filter('translate')('pages.manufacturerManagement.erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.manufacturerManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ManufacturerSubmitBtnIconClass = "";
                        $scope.ManufacturerSubmitBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.ManufacturerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ManufacturerSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Manufacturer/UpdateManufacturer',
                        data: $scope.Manufacturer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ManufacturerSubmitBtnIconClass = "";
                        $scope.ManufacturerSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.manufacturerManagement.manufacturerInformation'),
                                text: $filter('translate')('pages.manufacturerManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.manufacturerManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Manufacturer/GetAllManufacturers',
                                data: { "Id": $scope.CommodityTypeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Manufactures = data;
                            }).error(function (data, status, headers, config) {
                            });

                            clearManufacturerControls();

                        } else {;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.manufacturerManagement.manufacturerInformation'),
                            text: $filter('translate')('pages.manufacturerManagement.erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.manufacturerManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ManufacturerSubmitBtnIconClass = "";
                        $scope.ManufacturerSubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('pages.manufacturerManagement.fillvalidfeild'))
            }
        }

        function AddCommodityType() {
            var index = 0;
            $scope.CommodityTypeList = [];
            angular.forEach($scope.CommodityTypes, function (value) {
                var x = { id: index, code: value.CommodityTypeId, label: value.CommodityTypeDescription };
                $scope.CommodityTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadCommodityType() {
            $scope.SelectedCommodityTypeList = [];
            $scope.SelectedCommodityTypeDList = [];
            angular.forEach($scope.Manufacturer.ComodityTypes, function (valueOut) {
                angular.forEach($scope.CommodityTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedCommodityTypeDList.push(valueIn.label);
                        $scope.SelectedCommodityTypeList.push(x);
                    }
                });
            });
        }
        $scope.SendCommodityType = function () {
            $scope.SelectedCommodityTypeDList = [];
            $scope.Manufacturer.ComodityTypes = [];
            angular.forEach($scope.SelectedCommodityTypeList, function (valueOut) {
                angular.forEach($scope.CommodityTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.SelectedCommodityTypeDList.push(valueIn.label);
                        $scope.Manufacturer.ComodityTypes.push(valueIn.code);
                    }
                });
            });
        }
    });



