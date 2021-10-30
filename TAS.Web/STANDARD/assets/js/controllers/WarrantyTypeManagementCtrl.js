app.controller('WarrantyTypeManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster ,$filter, $translate) {
        $scope.ModalName = "Warranty Type Management";
        $scope.ModalDescription = "Add Edit Warranty Type Information";
        $scope.CommodityTypeList = [];
        $scope.SelectedCommodityTypeList = [];
        $scope.SelectedCommodityTypeDList = [];
        $scope.errorTab1 = "";

        $scope.SubmitBtnIconClass = "";
        $scope.SubmitBtnDisabled = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.warrantyTypeManagement.Error'), msg);
        };

        var customWarningMessage = function (msg) {
            toaster.pop('warning', 'Warning', msg, 12000);
        };

        $scope.loadInitailData = function () { }
        $scope.WarrantyType = {
            Id: "00000000-0000-0000-0000-000000000000",
            WarrantyTypeDescription: ''
        };           

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/WarrantyType/GetAllWarrantyTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.WarrantyTypes = data;
            }).error(function (data, status, headers, config) {
            });
        }
        LoadDetails();

        $scope.SetWarrantyTypeValues = function () {
            $scope.errorTab1 = "";
            if ($scope.WarrantyType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/WarrantyType/GetWarrantyTypeById',
                    data: { "Id": $scope.WarrantyType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.WarrantyType.Id = data.Id;
                $scope.WarrantyType.WarrantyTypeDescription = data.WarrantyTypeDescription;
                }).error(function (data, status, headers, config) {
                    clearWarrantyTypeControls();
                });
            }
            else {
                clearWarrantyTypeControls();
            }
        }
        clearWarrantyTypeControls();
        function clearWarrantyTypeControls() {
            $scope.WarrantyType.Id = "00000000-0000-0000-0000-000000000000";
            $scope.WarrantyType.WarrantyTypeDescription = "";
        }
        function CheckWarrantyType() {
            var ret = true;
            angular.forEach($scope.WarrantyTypes, function (value) {
                if (value.WarrantyTypeDescription == $scope.WarrantyType.WarrantyTypeDescription && $scope.WarrantyType.Id != value.Id) {
                    ret = false;
                }
            });
            if (!ret) {
                //SweetAlert.swal({
                //    title: "Warranty Type Information",
                //    text: "Name Should be Unique!",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customWarningMessage($filter('translate')('pages.warrantyTypeManagement.valideteName'))  
                //customWarningMessage("Name Should be Unique!")
                return ret;
            }
            else {
                return ret;
            }
        }

        

        $scope.validateWarrantyType = function () {
            var isValid = true;
            if ($scope.WarrantyType.WarrantyTypeDescription == "" || $scope.WarrantyType.WarrantyTypeDescription == undefined
                || $scope.WarrantyType.WarrantyTypeDescription == null) {
                $scope.validate_WarrantyTypeDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_WarrantyTypeDescription = "";
            }
            return isValid
        }

        $scope.Submit = function () {
            
            if ($scope.validateWarrantyType()) {

                $scope.errorTab1 = "";
                if (!CheckWarrantyType()) {
                    return false;
                }
                if ($scope.WarrantyType.Id == null || $scope.WarrantyType.Id == "00000000-0000-0000-0000-000000000000") {

                    $scope.SubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.SubmitBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/WarrantyType/AddWarrantyType',
                        data: $scope.WarrantyType,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.SubmitBtnIconClass = "";
                        $scope.SubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.warrantyTypeManagement.warrantyTypeInformation'),
                                text: $filter('translate')('pages.warrantyTypeManagement.SuccessfullySaved'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/WarrantyType/GetAllWarrantyTypes',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.WarrantyTypes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearWarrantyTypeControls();
                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.warrantyTypeManagement.warrantyTypeInformation'),
                            text: $filter('translate')('pages.warrantyTypeManagement.Erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.SubmitBtnIconClass = "";
                        $scope.SubmitBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.SubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.SubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/WarrantyType/UpdateWarrantyType',
                        data: $scope.WarrantyType,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.SubmitBtnIconClass = "";
                        $scope.SubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.warrantyTypeManagement.warrantyTypeInformation'),
                                text: $filter('translate')('pages.warrantyTypeManagement.SuccessfullySaved'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/WarrantyType/GetAllWarrantyTypes',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.WarrantyTypes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearWarrantyTypeControls();
                        } else {;
                            SweetAlert.swal({
                                title: $filter('translate')('pages.warrantyTypeManagement.warrantyTypeInformation'),
                                text: $filter('translate')('pages.warrantyTypeManagement.Erroroccuredsavingdata'),                                
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.warrantyTypeManagement.warrantyTypeInformation'),
                            text: $filter('translate')('pages.warrantyTypeManagement.Erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.SubmitBtnIconClass = "";
                        $scope.SubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('pages.warrantyTypeManagement.fillvalidfeild'))  
            }
        }
    });



