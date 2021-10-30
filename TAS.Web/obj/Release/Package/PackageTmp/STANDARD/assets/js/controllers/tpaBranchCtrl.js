app.controller('TpaBranchCtrl',
    function ($scope, $rootScope, $http, $rootScope, $location, SweetAlert, $localStorage, $cookieStore, toaster, $filter, $translate) {

        $scope.errorTab1 = "";
        $scope.TpaBranchBtnIconClass = "";
        $scope.TpaBranchBtnDisabled = false;
        $scope.IsExistsVarBranchCode = false;
        //$scope.TimeZone = [];
        $scope.selectedTimeZoneName = '';
        $scope.tpaBranch = {
            TpaId: "",
            Id: "00000000-0000-0000-0000-000000000000",
            BranchName: "",
            BranchCode: "",
            ContryId: "",
            CityId: "",
            State: "",
            TimeZone: "",
            Address: "",
            IsHeadOffice: false,
        }
        $scope.resetTpaBranch = function () {
            $scope.tpaBranch = {
                TpaId: "",
                Id: "00000000-0000-0000-0000-000000000000",
                BranchName: "",
                BranchCode: "",
                ContryId: "",
                CityId: "",
                State: "",
                TimeZone: "",
                Address: "",
                IsHeadOffice: false,
            }
        }
        $scope.resetTpaBranchDetails = function () {
            $scope.tpaBranch = {
                TpaId: $localStorage.tpaID,
                Id: "00000000-0000-0000-0000-000000000000",
                BranchName: "",
                BranchCode: "",
                ContryId: "",
                CityId: "",
                State: "",
                TimeZone: "",
                Address: "",
                IsHeadOffice: false,
            }
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.tpaBranchManagement.Error'), msg);
        };

        $scope.loadInitailData = function () {

            var jwt = $cookieStore.get('jwt');

            $http({
                method: 'POST',
                url: '/TAS.Web/api/TPA/GetAllTPAs',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.tpas = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
            }).error(function (data, status, headers, config) {
                //$scope.message = 'Unexpected Error';
            });
            $scope.SetTpa();

            $http({
                method: 'POST',
                url: '/TAS.Web/api/TPABranch/GetAllTimezones',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.timeZones = data;
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.SetTpa = function () {
            $scope.tpaBranch.TpaId = $localStorage.tpaID;
            if ($scope.tpaBranch.TpaId != "" && typeof ($scope.tpaBranch.TpaId) !== "undefined" && $scope.tpaBranch.TpaId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TPABranch/GetTPABranchesByTpaId',
                    data: { "tpaId": $scope.tpaBranch.TpaId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.tpaBranches = data;

                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.resetTpaBranch();
               
            }
        }

        $scope.SetTimeZone = function () {            
            //$scope.tpaBranch.TimeZone = $localStorage.tpaID;
            if ($scope.tpaBranch.TimeZone != "" && typeof ($scope.tpaBranch.TimeZone) !== "undefined" && $scope.tpaBranch.TimeZone != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TPABranch/GetTimeZoneById',
                    data: { "TimeZone": $scope.tpaBranch.TimeZone },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.TimeZones = data;

                }).error(function (data, status, headers, config) {
                });
            } else {
                //$scope.resetTpaBranch();

            }
        }

        $scope.SetTpaBranch = function () {
            if ($scope.tpaBranch.Id != "" && $scope.tpaBranch.Id != null) {
                for (var i = 0; i < $scope.tpaBranches.length; i++) {
                    if ($scope.tpaBranches[i].Id == $scope.tpaBranch.Id) {
                       
                        $scope.tpaBranch = {
                            TpaId: $localStorage.tpaID,
                            Id: $scope.tpaBranch.Id,
                            BranchName: $scope.tpaBranches[i].BranchName,
                            BranchCode: $scope.tpaBranches[i].BranchCode,
                            ContryId: $scope.tpaBranches[i].ContryId,
                            CityId: $scope.tpaBranches[i].CityId,
                            State: $scope.tpaBranches[i].State,
                            TimeZone: $scope.tpaBranches[i].TimeZone,
                            Address: $scope.tpaBranches[i].Address,
                            IsHeadOffice: $scope.tpaBranches[i].IsHeadOffice,
                            
                        }
                        $scope.SetCountryValue();

                        var str = $scope.tpaBranches[i].CityId;
                        var CityValue = $scope.Cities;
                        $scope.tpaBranch.CityId = $scope.tpaBranches[i].CityId;

                        var  str =$scope.tpaBranches[i].TimeZone;
                        var timeZoneVlue = $scope.TimeZones;
                        //alert(timeZoneVlue);
                        $scope.tpaBranch.TimeZone = $scope.tpaBranches[i].TimeZone; //$scope.$apply();
                        //alert(timeZoneVlue);
                        //document.getElementById('drptimezone').value = timeZoneVlue;
                    }
                }
            } else {
                $scope.resetTpaBranchDetails();
            }
        }
        $scope.saveOrUpdateTpa = function () {
           
                if ($scope.tpa.Id != "") {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/TPA/UpdateTPA',
                        data: $scope.tpa,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.resetTpa();
                        $scope.loadExistingTPAs();
                        SweetAlert.swal({
                            title: $filter('translate')('pages.tpaBranchManagement.TASInformation'),
                            text: $filter('translate')('pages.tpaBranchManagement.SuccessfullyUpdated'),
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                    }).error(function (data, status, headers, config) {

                    });
                } else {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/TPA/SaveTPA',
                        data: $scope.tpa,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.resetTpa();
                        $scope.loadExistingTPAs();
                        SweetAlert.swal({
                            title: $filter('translate')('pages.tpaBranchManagement.TASInformation'),
                            text: $filter('translate')('pages.tpaBranchManagement.SuccessfullySaved'),
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                    }).error(function (data, status, headers, config) {

                    });
                }
            
        }
        $scope.SetCountryValue = function () {
            $scope.Cities = null;
            $scope.tpaBranchCityDisabled = true;
            $scope.tpaBranch.CityId = "";
            if ($scope.tpaBranch.ContryId != null && $scope.tpaBranch.ContryId != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                    data: { "countryId": $scope.tpaBranch.ContryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Cities = data;
                    $scope.tpaBranchCityDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.tpaBranchCityDisabled = false;
                   // $scope.Cities = null;
                    //$scope.message = 'Unexpected Error';
                });
            }
            else {
                $scope.Cities = null;
                $scope.tpaBranchCityDisabled = false;
            }

        }
        $scope.saveOrUpdateTpaBranch = function () {
            //alert($scope.tpaBranch.BranchName);
            //var retVal = true;
            if (validate()) {
                if ($scope.isValidTpaCode()) {
                    if ($scope.tpaBranch.Id != null && $scope.tpaBranch.Id != "00000000-0000-0000-0000-000000000000") {
                        $scope.TpaBranchBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.TpaBranchBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/TPABranch/UpdateTPABranch',
                            data: $scope.tpaBranch,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.resetTpaBranch();
                            //  $scope.loadInitailData();
                            $scope.SetTpa();
                            $scope.TpaBranchBtnIconClass = "";
                            $scope.TpaBranchBtnDisabled = false;
                            SweetAlert.swal({
                                title: $filter('translate')('pages.tpaBranchManagement.TASInformation'),
                                text: $filter('translate')('pages.tpaBranchManagement.SuccessfullyUpdated'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                        }).error(function (data, status, headers, config) {

                            $scope.TpaBranchBtnIconClass = "";
                            $scope.TpaBranchBtnDisabled = false;
                        });
                    } else {
                        $scope.TpaBranchBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.TpaBranchBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/TPABranch/SaveTPABranch',
                            data: $scope.tpaBranch,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.resetTpaBranch();
                            // $scope.loadInitailData();
                            $scope.SetTpa();
                            $scope.SetTpa();
                            $scope.TpaBranchBtnIconClass = "";
                            $scope.TpaBranchBtnDisabled = false;
                            SweetAlert.swal({
                                title: $filter('translate')('pages.tpaBranchManagement.TASInformation'),
                                text: $filter('translate')('pages.tpaBranchManagement.SuccessfullySaved'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                        }).error(function (data, status, headers, config) {
                            $scope.TpaBranchBtnIconClass = "";
                            $scope.TpaBranchBtnDisabled = false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.tpaBranchManagement.fillvalidfeild'))
            }            
        }

        $scope.IsExsistingTpaByCode = function () {
            $scope.errorTab1 = "";
            $scope.TpaBranchBtnDisabled = true;
            if ($scope.tpaBranch.Id != null && $scope.tpaBranch.BranchCode != undefined || $scope.tpaBranch.BranchCode.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/TPABranch/IsExsistingTpaByCode',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.tpaBranch.Id, "BranchCode": $scope.tpaBranch.BranchCode }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarBranchCode = data;
                    $scope.TpaBranchBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.TpaBranchBtnDisabled = false;
                });
            }
        }


        function validate() {
            var isValid = true;

            if ($scope.tpaBranch.BranchName == undefined || $scope.tpaBranch.BranchName == "") {
                $scope.validate_BranchName = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_BranchName = "";
            }
            if ($scope.tpaBranch.BranchCode == undefined || $scope.tpaBranch.BranchCode == "") {
                $scope.validate_BranchCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_BranchCode = "";
            }
            if ($scope.tpaBranch.ContryId == undefined || $scope.tpaBranch.ContryId == "") {
                $scope.validate_ContryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ContryId = "";
            }
            if ($scope.tpaBranch.CityId == undefined || $scope.tpaBranch.CityId == "") {
                $scope.validate_CityId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CityId = "";
            }
            if ($scope.tpaBranch.TimeZone == undefined || $scope.tpaBranch.TimeZone == "") {
                $scope.validate_TimeZone = "has-error";
                isValid = false;
            } else {
                $scope.validate_TimeZone = "";
            }

            return isValid;
           
            
        }

        $scope.isValidTpaCode = function (){

            var isValid = true;
            if (isValid) {
                   
                if (isValid) {
                    if ($scope.IsExistsVarBranchCode) {
                        customErrorMessage($filter('translate')('pages.tpaBranchManagement.validetebranchCode'))                       
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
           

    });