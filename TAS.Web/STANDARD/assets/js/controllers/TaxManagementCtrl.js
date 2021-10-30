app.controller('TaxManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Tax Management";
        $scope.ModalDescription = "Add Edit Tax Information";
        $scope.errorTab1 = "";
        $scope.TaxSaveBtnIconClass = "";
        $scope.TaxSaveBtnDisabled = false;
        $scope.IsExistsVarTaxName = false;
        $scope.IsExistsVarTaxCode = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.taxManagement.Error'), msg);
        };

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllTaxTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Taxes = data;
            }).error(function (data, status, headers, config) {
            });
        }
        LoadDetails();
        $scope.TaxTypes = {
            Id: "00000000-0000-0000-0000-000000000000",
            TaxName: "",
            TaxCode: ""
        };
        function clearTaxTypesControls() {
            $scope.TaxTypes = {
                Id: "00000000-0000-0000-0000-000000000000",
                TaxName: "",
                TaxCode: ""
            };
        }
        $scope.SetTaxValues = function () {
            $scope.errorTab1 = "";
            if ($scope.TaxTypes.Id != "00000000-0000-0000-0000-000000000000" && $scope.TaxTypes.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Country/GetTaxById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.TaxTypes.Id }
                }).success(function (data, status, headers, config) {
                    $scope.TaxTypes = data;
                }).error(function (data, status, headers, config) {
                    clearTaxTypesControls();
                });
            }
            else {
                clearTaxTypesControls();
            }
        }

        $scope.TaxSubmit = function () {
            if (validate()) {
                if (isValidTaxData()){
                if ($scope.TaxTypes.Id == null || $scope.TaxTypes.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.TaxSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.TaxSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/AddTax',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.TaxTypes
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.TaxSaveBtnIconClass = "";
                        $scope.TaxSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.taxManagement.taxInformation'), 
                                text: $filter('translate')('pages.taxManagement.SuccessfullySaved'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/GetAllTaxTypes',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Taxes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearTaxTypesControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.taxManagement.taxInformation'),
                            text: $filter('translate')('pages.taxManagement.Erroroccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.TaxSaveBtnIconClass = "";
                        $scope.TaxSaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.TaxSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.TaxSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/UpdateTax',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.TaxTypes
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.TaxSaveBtnIconClass = "";
                        $scope.TaxSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.taxManagement.taxInformation'),
                                text: $filter('translate')('pages.taxManagement.SuccessfullySaved'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/GetAllTaxTypes',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Taxes = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearTaxTypesControls();
                        }
                        else {;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.taxManagement.taxInformation'),
                            text: $filter('translate')('pages.taxManagement.Erroroccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.TaxSaveBtnIconClass = "";
                        $scope.TaxSaveBtnDisabled = false;
                        return false;
                    });
                }
            }
            }
            else {
                
                customErrorMessage($filter('translate')('pages.taxManagement.fillvalidfeild'))
            }
        }

        $scope.IsExsistsTaxName = function () {
            $scope.errorTab1 = "";
           // $scope.TaxSaveBtnDisabled = true;
            if ($scope.TaxTypes.Id != null && $scope.TaxTypes.TaxName != undefined || $scope.TaxTypes.TaxName.trim() != "") {
                swal({ title: $filter('translate')('pages.taxManagement.Processing'), text: $filter('translate')('pages.taxManagement.ValidatingTaxName'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/Country/IsExsistingTaxByName',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.TaxTypes.Id, "TaxName": $scope.TaxTypes.TaxName }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarTaxName = data;
                    swal.close();
                    if ($scope.IsExistsVarTaxName) {
                        customErrorMessage($filter('translate')('pages.taxManagement.validetetaxname'))
                        //customErrorMessage("Tax Name already exists.");
                    }
                   // $scope.TaxSaveBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                   // $scope.TaxSaveBtnDisabled = false;
                });
            }
            return retVal;
        }

        $scope.IsExsistsTaxCode = function () {
            $scope.errorTab1 = "";
           // $scope.TaxSaveBtnDisabled = true;
            if ($scope.TaxTypes.Id != null && $scope.TaxTypes.TaxCode != undefined || $scope.TaxTypes.TaxCode.trim() != "") {
                swal({ title: $filter('translate')('pages.taxManagement.Processing'), text: $filter('translate')('pages.taxManagement.ValidatingTaxCode'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/Country/IsExsistingTaxByCode',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.TaxTypes.Id, "TaxCode": $scope.TaxTypes.TaxCode }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarTaxCode = data;
                    swal.close();
                    if ($scope.IsExistsVarTaxCode) {
                        customErrorMessage($filter('translate')('pages.taxManagement.validetetaxcode'))
                        //customErrorMessage("Tax Code already exists.");
                    }
                  //  $scope.TaxSaveBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                //    $scope.TaxSaveBtnDisabled = false;
                });
            }
            return retVal;
        }

        function validate()
        {
            var isValid = true;
                if ($scope.TaxTypes.TaxName == undefined || $scope.TaxTypes.TaxName == "") {
                    $scope.validate_taxName = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_taxName = "";
                }

                if ($scope.TaxTypes.TaxCode == undefined || $scope.TaxTypes.TaxCode == "") {
                    $scope.validate_taxCode = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_taxCode = "";
                }        
                return isValid
        }

        function isValidTaxData() {
            var isValid = true;
            if (!$scope.validateCharacter($scope.TaxTypes.TaxName)) {
                customErrorMessage($filter('translate')('pages.taxManagement.validetetaxnumeric'))
                //customErrorMessage("Tax Name cannot contain only numeric and special characters.")
                isValid = false;
            }
            if (isValid) {
                if ($scope.IsExistsVarTaxName) {
                    customErrorMessage($filter('translate')('pages.taxManagement.validetetaxname'))
                    //customErrorMessage("Tax Name already exists.")
                    isValid = false;
                }
                if (isValid) {
                    if ($scope.IsExistsVarTaxCode) {
                        customErrorMessage($filter('translate')('pages.taxManagement.validetetaxcode'))
                        //customErrorMessage("Tax Code already exists.")
                        isValid = false;
                    }
                }
            }
            return isValid
        }

     
        //function validate() {
        //    var retVal = true;
        //    $scope.errorTab1 = "";
        //    if ($scope.TaxTypes.TaxName == undefined || $scope.TaxTypes.TaxName.trim() == "" || $scope.TaxTypes.TaxCode == undefined || $scope.TaxTypes.TaxCode.trim() == "") {
        //        $scope.errorTab1 = "Please Enter ";
        //        var vari = "";
        //        if ($scope.TaxTypes.TaxName == undefined || $scope.TaxTypes.TaxName.trim() == "") {
        //            $scope.errorTab1 = $scope.errorTab1 + "Name ";
        //        }
        //        vari = ($scope.errorTab1 != "Please Enter ") ? " ," : "";
        //        if ($scope.TaxTypes.TaxCode == undefined || $scope.TaxTypes.TaxCode.trim() == "") {
        //            $scope.errorTab1 = $scope.errorTab1 + "Code ";
        //        }
        //        $scope.errorTab1 = $scope.errorTab1.substring(0, $scope.errorTab1.length - 1);
        //        retVal = false;
        //    }
        //    else {
        //        if (!$scope.validateCharacter($scope.TaxTypes.TaxName)) {
        //            $scope.errorTab1 = "Tax Name cannot contain only numeric and special characters";
        //            retVal = false;
        //        }
        //        if (retVal) {
        //            if ($scope.IsExistsVarTaxName) {
        //                $scope.errorTab1 = "Tax Name already exists";
        //                retVal = false;
        //            }
        //            if (retVal) {
        //                if ($scope.IsExistsVarTaxCode) {
        //                    $scope.errorTab1 = "Tax Code already exists";
        //                    retVal = false;
        //                }
        //            }
        //        }
        //    }
        //    return retVal;
        //}
    });