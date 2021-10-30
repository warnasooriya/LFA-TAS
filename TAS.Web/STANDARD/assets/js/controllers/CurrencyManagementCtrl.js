app.controller('CurrencyManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $filter, $localStorage, toaster, $translate) {
        $scope.ModalName = "Currency Setup";
        $scope.CurrencyPeriodBtnIconClass = "";
        $scope.CurrencyPeriodBtnDisabled = false;
        $scope.CurrencyAddBtnIconClass = "";
        $scope.CurrencyAddBtnDisabled = false;

        $scope.AddCurrencyEmailBtnIconClass = "";
        $scope.AddCurrencyEmailBtnDisabled = false;


        $scope.filterValue = function ($event) {
            //if (isNaN(String.fromCharCode($event.keyCode))) {
            //    $event.preventDefault();
            //}
            //$scope.Conversion.Rate = $scope.Conversion.Rate.replace(/\D/g, '');
            $scope.Conversion.Rate = "";
        };


        function validateEmail($email) {
            var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
            return emailReg.test($email);
        }

        $scope.Periods = [];// [{From:"",To:"",Description:"",Id:1}];
        $scope.gridOptionsPeriod = {
            data: 'Periods',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            multiSelect: false,
            enableHiding: false,
            //enableSorting: false,
            columnDefs: [{
                field: "From",
                displayName: $filter('translate')('pages.currencyManagement.from')
            }, {
                field: "To",
                    displayName: $filter('translate')('pages.currencyManagement.to')
            }, {
                field: "Description",
                    displayName: $filter('translate')('pages.currencyManagement.description')
            }, {
                field: "Id",
                visible: false
            }
            ]
        };
        $scope.gridOptionsPeriod.onRegisterApi = function (gridApi) {
            $scope.gridApiPeriod = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, SetPeriodValues);
        }

        $scope.Conversions = [];// [{ Currency: "USD", X: "X", Rate: "1", Comment: "USD Conversion", Id: 1 }];
        $scope.gridOptionsCurrencyConversion = {
            data: 'Conversions',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            enableSorting: false,
            multiSelect: false,
            enableHiding: false,
            columnDefs: [{
                field: "Currency",
                displayName: $filter('translate')('pages.currencyManagement.tabCurrencyDefinition.currency')
            }, {
                field: "X",
                displayName: ""
            }, {
                field: "Rate",
                displayName: $filter('translate')('pages.currencyManagement.tabCurrencyDefinition.rate')
            }, {
                field: "Comment",
                displayName: $filter('translate')('pages.currencyManagement.tabCurrencyDefinition.comment')
            }, {
                field: "Id",
                visible: false
            }
            ]
        };
        $scope.gridOptionsCurrencyConversion.onRegisterApi = function (gridApi) {
            $scope.gridApiCurrencyConversion = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, SetConversionValues);
        }

        LoadDetails();

        $scope.Period = {
            Id: "00000000-0000-0000-0000-000000000000",
            FromDate: "",
            ToDate: "",
            Description: ""
        }
        $scope.Conversion = {
            Id: "00000000-0000-0000-0000-000000000000",
            Rate: "",
            CurrencyId: "",
            CurrencyConversionPeriodId: "",
            Comment: ""
        }
        $scope.CurrencyEmail =
            {
                Id: "00000000-0000-0000-0000-000000000000",
                TPAEmail: "",
                AdminEmail: "",
                FirstEmailDuration: "",
                FirstDurationType: "",
                FirstMailSubject: "",
                FirstMailBody: "",
                SecoundEmailDuration: "",
                SecoundDurationType: "",
                SecoundMailSubject: "",
                SecoundMailBody: "",
                LastEmailDuration: "",
                LastDurationType: "",
                LastMailSubject: "",
                LastMailBody: "",
            }
        $scope.DurationType = [{ Name: 'Hours' }, { Name: 'Days' }];
        function ClearPeriod() {
            $scope.Period.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Period.FromDate = "";
            $scope.Period.ToDate = "";
            $scope.Period.Description = "";
            $scope.Periods = [];
            angular.forEach($scope.CurrencyConversionPeriods, function (value) {
                var p = { From: $filter('date')(value.FromDate, "yyyy-MMM-dd"), To: $filter('date')(value.ToDate, "yyyy-MMM-dd"), Description: value.Description, Id: value.Id, Fromdate: value.FromDate, Todate: value.ToDate }
                $scope.Periods.push(p)
            });
        }
        function ClearCurrencyConversion() {
            $scope.Conversion.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Conversion.Rate = "";
            $scope.Conversion.CurrencyId = "";
            $scope.Conversion.Comment = "";
            $scope.Conversions = [];
            //angular.forEach($scope.AllCurrencyConversions, function (value) {
            //    if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
            //        var Currency = "";
            //        angular.forEach($scope.Currencies, function (valueC) {
            //            if (valueC.Id == value.CurrencyId) {
            //                Currency = valueC.Code;
            //            }
            //        });

            //        var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
            //        $scope.Conversions.push(p);
            //    }
            //});
        }
        function SetPeriodValues() {
            $scope.CurrencyConversions = [];
            var SelectedRows = $scope.gridApiPeriod.selection.getSelectedRows();

            if (SelectedRows.length > 0) {
                angular.forEach(SelectedRows, function (value) {
                    $scope.Conversion.CurrencyConversionPeriodId = value.Id;
                    $scope.Period.Id = value.Id;
                    $scope.Period.FromDate = value.Fromdate;
                    $scope.Period.ToDate = value.Todate;
                    $scope.Period.Description = value.Description;
                    angular.forEach($scope.AllCurrencyConversions, function (valueIn) {
                        if (value.Id == valueIn.CurrencyConversionPeriodId) {
                            $scope.CurrencyConversions.push(valueIn);
                        }
                    });
                });
                ClearCurrencyConversion();

                angular.forEach($scope.AllCurrencyConversions, function (value) {
                    if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
                        var Currency = "";
                        angular.forEach($scope.Currencies, function (valueC) {
                            if (valueC.Id == value.CurrencyId) {
                                Currency = valueC.Code;
                            }
                        });

                        var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
                        $scope.Conversions.push(p);
                    }
                });

            } else {

                $scope.CurrencyConversions = [];
                ClearCurrencyConversion();
                ClearPeriod();

            }
        }
        function SetConversionValues() {
            var SelectedRows = $scope.gridApiCurrencyConversion.selection.getSelectedRows();

            if (SelectedRows.length > 0) {
                angular.forEach(SelectedRows, function (value) {
                    angular.forEach($scope.Currencies, function (valueC) {
                        if (valueC.Code == value.Currency) {
                            $scope.Conversion.CurrencyId = valueC.Id;
                        }
                    });
                    $scope.Conversion.Id = value.Id;
                    $scope.Conversion.Rate = value.Rate;
                    $scope.Conversion.CurrencyConversionPeriodId = $scope.Period.Id;
                    $scope.Conversion.Comment = value.Comment;

                });
            } else {
                ClearCurrencyConversion();
                angular.forEach($scope.AllCurrencyConversions, function (value) {
                    if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
                        var Currency = "";
                        angular.forEach($scope.Currencies, function (valueC) {
                            if (valueC.Id == value.CurrencyId) {
                                Currency = valueC.Code;
                            }
                        });

                        var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
                        $scope.Conversions.push(p);
                    }
                });
            }
        }
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyEmails',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CurrencyEmail = data[0];
                if ($scope.CurrencyEmail == undefined) {
                    $scope.CurrencyEmail = {};
                }

            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/GetCurrencies',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Currencies = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversions',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.AllCurrencyConversions = data;
                ClearCurrencyConversion();

                angular.forEach($scope.AllCurrencyConversions, function (value) {
                    if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
                        var Currency = "";
                        angular.forEach($scope.Currencies, function (valueC) {
                            if (valueC.Id == value.CurrencyId) {
                                Currency = valueC.Code;
                            }
                        });

                        var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
                        $scope.Conversions.push(p);
                    }
                });
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversionPeriods',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CurrencyConversionPeriods = data;
                ClearPeriod();
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.validateCurrencyPeriod = function () {
            var isValidated = true;

            if ($scope.Period.FromDate == "") {
                isValidated = false;
                $scope.validate_policyPeriodFromDate = "has-error";
            } else {
                $scope.validate_policyPeriodFromDate = "";
            }

            if ($scope.Period.ToDate == "") {

                isValidated = false;
                $scope.validate_policyPeriodToDate = "has-error";
            } else {
                $scope.validate_policyPeriodToDate = "";
            }

            if ($scope.Period.Description == "") {
                isValidated = false;
                $scope.validate_policyPeriodDescription = "has-error";
            } else {
                $scope.validate_policyPeriodDescription = "";
            }

            return isValidated;
        }


        $scope.AddPeriods = function () {
            if (!$scope.validateCurrencyPeriod()) {
                customErrorMessage($filter('translate')('pages.currencyManagement.fillvalidfeild'));

            } else {

                //date validation
                if ($scope.Period.FromDate > $scope.Period.ToDate) {
                    customErrorMessage($filter('translate')('pages.currencyManagement.fromDateLarger'));
                    $scope.validate_policyPeriodFromDate = "has-error";
                    $scope.validate_policyPeriodToDate = "has-error";
                } else {
                    $scope.CurrencyPeriodBtnIconClass = "fa fa-spinner fa-spin";;
                    $scope.CurrencyPeriodBtnDisabled = true;

                    $scope.validate_policyPeriodToDate = "";
                    $scope.validate_policyPeriodFromDate = "";
                    //validate for time period overlaps

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/CurrencyManagement/ValidateCurrencyPeriodOverlaps',
                        data: $scope.Period,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data != "") {
                            customErrorMessage(data);
                            $scope.CurrencyPeriodBtnIconClass = "";
                            $scope.CurrencyPeriodBtnDisabled = false;
                            return false;

                        } else {
                            if ($scope.Period.Id == null || $scope.Period.Id == "00000000-0000-0000-0000-000000000000") {
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/AddCurrencyConversionPeriod',
                                    data: $scope.Period,
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.Ok = data;
                                    if (data == "OK") {
                                        SweetAlert.swal({
                                            title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                            text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                            confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                            confirmButtonColor: "#007AFF"
                                        });
                                        $scope.CurrencyPeriodBtnIconClass = "";
                                        $scope.CurrencyPeriodBtnDisabled = false;
                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversionPeriods',
                                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            $scope.CurrencyConversionPeriods = data;
                                            ClearPeriod();

                                        }).error(function (data, status, headers, config) {
                                        });
                                    } else {
                                    }
                                    return false;
                                }).error(function (data, status, headers, config) {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                        text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                        type: "warning",
                                        confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                    $scope.CurrencyPeriodBtnIconClass = "";
                                    $scope.CurrencyPeriodBtnDisabled = false;
                                    return false;
                                });
                            }
                            else {
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/UpdateCurrencyConversionPeriod',
                                    data: $scope.Period,
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.Ok = data;
                                    if (data == "OK") {
                                        SweetAlert.swal({
                                            title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                            text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                            confirmButtonColor: "#007AFF",
                                            confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                        });
                                        $scope.CurrencyPeriodBtnIconClass = "";
                                        $scope.CurrencyPeriodBtnDisabled = false;
                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversionPeriods',
                                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            $scope.CurrencyConversionPeriods = data;
                                            ClearPeriod();
                                        }).error(function (data, status, headers, config) {
                                        });
                                    } else {;
                                    }
                                    return false;
                                }).error(function (data, status, headers, config) {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                        text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                        type: "warning",
                                        confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                    $scope.CurrencyPeriodBtnIconClass = "";
                                    $scope.CurrencyPeriodBtnDisabled = false;
                                    return false;
                                });
                            }
                        }
                    }).error(function (data, status, headers, config) {
                    });
                }
            }
            return false;
        }
        $scope.validateCurrencyConversion = function () {
            var isValidated = true;

            if ($scope.Conversion.Rate == "") {
                isValidated = false;
                $scope.validate_conversionRate = "has-error";
            } else {
                $scope.validate_conversionRate = "";
            }

            if ($scope.Conversion.CurrencyId == "") {
                isValidated = false;
                $scope.validate_conversionCurencyId = "has-error";
            } else {
                $scope.validate_conversionCurencyId = "";
            }

            //if ($scope.Conversion.Comment == "") {
            //    isValidated = false;
            //    $scope.validate_conversionComment = "has-error";
            //} else {
            //    $scope.validate_conversionComment = "";
            //}
            return isValidated;

        }
        $scope.AddCurrencyConversion = function () {
            var exists = false;
            if ($scope.Conversion.CurrencyConversionPeriodId == "") {
                customErrorMessage($filter('translate')('pages.currencyManagement.applicableCurrency'));
                return false;
            }
            if ($scope.validateCurrencyConversion()) {
                if ($scope.Conversion.Id == null || $scope.Conversion.Id == "00000000-0000-0000-0000-000000000000") {
                    angular.forEach($scope.Conversions, function (value) {
                        angular.forEach($scope.Currencies, function (valueC) {
                            if (valueC.Id == $scope.Conversion.CurrencyId) {
                                if (value.Currency == valueC.Code) {
                                    exists = true;
                                }
                            }

                        });
                    });
                    if (exists) {

                        customErrorMessage($filter('translate')('pages.currencyManagement.selectedCurrency'));
                        $scope.Conversion.Id = "00000000-0000-0000-0000-000000000000";
                        $scope.Conversion.Rate = "";
                        $scope.Conversion.CurrencyId = "";
                        $scope.Conversion.Comment = "";
                    }
                    else {
                        $scope.CurrencyAddBtnIconClass = "fa fa-spinner fa-spin";;
                        $scope.CurrencyAddBtnDisabled = true;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/AddCurrencyConversion',
                            data: $scope.Conversion,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                    text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                    confirmButtonColor: "#007AFF",
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                });
                                $scope.CurrencyAddBtnIconClass = "";
                                $scope.CurrencyAddBtnDisabled = false;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversions',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.AllCurrencyConversions = data;
                                    ClearCurrencyConversion();

                                    angular.forEach($scope.AllCurrencyConversions, function (value) {
                                        if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
                                            var Currency = "";
                                            angular.forEach($scope.Currencies, function (valueC) {
                                                if (valueC.Id == value.CurrencyId) {
                                                    Currency = valueC.Code;
                                                }
                                            });

                                            var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
                                            $scope.Conversions.push(p);
                                        }
                                    });
                                }).error(function (data, status, headers, config) {
                                });
                            } else {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                    text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CurrencyAddBtnIconClass = "";
                            $scope.CurrencyAddBtnDisabled = false;
                            return false;
                        });
                    }
                }
                else {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/CurrencyManagement/UpdateCurrencyConversion',
                        data: $scope.Conversion,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.currencyManagement.currencySetup'),
                                text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                confirmButtonColor: "#007AFF",
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyConversions',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.AllCurrencyConversions = data;
                                ClearCurrencyConversion();
                       

                                angular.forEach($scope.AllCurrencyConversions, function (value) {
                                    if (value.CurrencyConversionPeriodId == $scope.Conversion.CurrencyConversionPeriodId) {
                                        var Currency = "";
                                        angular.forEach($scope.Currencies, function (valueC) {
                                            if (valueC.Id == value.CurrencyId) {
                                                Currency = valueC.Code;
                                            }
                                        });

                                        var p = { Currency: Currency, X: "*", Rate: value.Rate, Comment: value.Comment, Id: value.Id }
                                        $scope.Conversions.push(p);
                                    }
                                });
                            }).error(function (data, status, headers, config) {
                            });
                        } else {;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.currencyManagement.currencySetup'),
                            text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('pages.currencyManagement.fillvalidfeild'));
            }



            var exists = false
            //if ($scope.Conversion.CurrencyConversionPeriodId == "") {
            //    SweetAlert.swal({
            //        title: "Currency Setup Information",
            //        text: "Please select a applicable period from above grid",
            //        type: "warning",
            //        confirmButtonColor: "rgb(221, 107, 85)"
            //    });
            //    return false;
            //}
            //if ($scope.Conversion.Rate != "" && $scope.Conversion.CurrencyId != "" && $scope.Conversion.Comment != "") {
            //    $scope.errorTab1 = "Please Enter ";
            //    if ($scope.Conversion.Rate == "") {
            //        $scope.errorTab1 = $scope.errorTab1 + "Rate, ";
            //    }
            //    if ($scope.Conversion.CurrencyId == "") {
            //        $scope.errorTab1 = $scope.errorTab1 + "Currency, ";
            //    }
            //    if ($scope.Conversion.Comment == "") {
            //        $scope.errorTab1 = $scope.errorTab1 + "Comment, ";
            //    }
            //    $scope.errorTab1 = $scope.errorTab1.substring(0, $scope.errorTab1.length - 1);
            //    return false;
            //}
            //$scope.errorTab1 = "";

        }
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";

        $scope.validateCurrencyEmail = function () {

            var isValid = true;

            if ($scope.CurrencyEmail.TPAEmail == "" || $scope.CurrencyEmail.TPAEmail == null) {
                $scope.validate_TPAEmail = "has-error";
                isValid = false;
            } else {
                $scope.validate_TPAEmail = "";
            }

            if ($scope.CurrencyEmail.AdminEmail == "" || $scope.CurrencyEmail.AdminEmail == null) {
                $scope.validate_AdminEmail = "has-error";
                isValid = false;
            }else{
                $scope.validate_AdminEmail = "";
            }

            if ($scope.CurrencyEmail.FirstEmailDuration == "" || $scope.CurrencyEmail.FirstEmailDuration == undefined) {
                $scope.validate_FirstEmailDuration = "has-error";
                isValid = false;
            } else {
                $scope.validate_FirstEmailDuration = "";
            }

            if ($scope.CurrencyEmail.FirstDurationType == "" || $scope.CurrencyEmail.FirstDurationType == undefined) {
                $scope.validate_FirstDurationType = "has-error";
                isValid = false;
            } else {
                $scope.validate_FirstDurationType = "";
            }

            if ($scope.CurrencyEmail.SecoundEmailDuration == "" || $scope.CurrencyEmail.SecoundEmailDuration == undefined) {
                $scope.validate_SecoundEmailDuration = "has-error";
                isValid = false;
            } else {
                $scope.validate_SecoundEmailDuration = "";
            }

            if ($scope.CurrencyEmail.SecoundDurationType == "" || $scope.CurrencyEmail.SecoundDurationType == undefined) {
                $scope.validate_SecoundDurationType = "has-error";
                isValid = false;
            } else {
                $scope.validate_SecoundDurationType = "";
            }

            if ($scope.CurrencyEmail.LastEmailDuration == "" || $scope.CurrencyEmail.LastEmailDuration == undefined) {
                $scope.validate_LastEmailDuration = "has-error";
                isValid = false;
            } else {
                $scope.validate_LastEmailDuration = "";
            }

            if ($scope.CurrencyEmail.LastDurationType == "" || $scope.CurrencyEmail.LastDurationType == undefined) {
                $scope.validate_LastDurationType = "has-error";
                isValid = false;
            } else {
                $scope.validate_LastDurationType = "";
            }

            return isValid;

        }

        $scope.isValidEmail = function () {

            var isValid = true;

            if ($scope.CurrencyEmail.TPAEmail != undefined) {
                if (!validateEmail($scope.CurrencyEmail.TPAEmail)) {
                    customErrorMessage($filter('translate')('pages.currencyManagement.validTPAemail'))
                    isValid = false;                   
                }

            }
            if ($scope.CurrencyEmail.AdminEmail != undefined) {
                if (!validateEmail($scope.CurrencyEmail.AdminEmail)) {
                    customErrorMessage($filter('translate')('pages.currencyManagement.validAdminemail'))
                    isValid = false;                    
                }
            }

            return isValid;
        }

        $scope.AddCurrencyEmail = function () {
            if ($scope.validateCurrencyEmail()) {
                if ($scope.isValidEmail()) {

                    $scope.errorTab2 = "";
                    if ($scope.CurrencyEmail.Id == null || $scope.CurrencyEmail.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.AddCurrencyEmailBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.AddCurrencyEmailBtnDisabled = true;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/AddCurrencyEmail',
                            data: $scope.CurrencyEmail,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.AddCurrencyEmailBtnIconClass = "";
                            $scope.AddCurrencyEmailBtnDisabled = false;

                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.currencyManagement.currencyEmailSetup'),
                                    text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                    confirmButtonColor: "#007AFF",
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyEmails',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CurrencyEmail = data[0];
                                }).error(function (data, status, headers, config) {
                                });
                            } else {
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.currencyManagement.currencyEmailSetup'),
                                text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.AddCurrencyEmailBtnIconClass = "";
                            $scope.AddCurrencyEmailBtnDisabled = false;

                            return false;
                        });
                    }
                    else {
                        $scope.AddCurrencyEmailBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.AddCurrencyEmailBtnDisabled = true;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/UpdateCurrencyEmail',
                            data: $scope.CurrencyEmail,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.AddCurrencyEmailBtnIconClass = "";
                            $scope.AddCurrencyEmailBtnDisabled = false;

                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.currencyManagement.currencyEmailSetup'),
                                    text: $filter('translate')('pages.currencyManagement.successfullySaved'),
                                    confirmButtonColor: "#007AFF",
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetAllCurrencyEmails',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CurrencyEmail = data[0];
                                }).error(function (data, status, headers, config) {
                                });
                            } else {;
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.currencyManagement.currencyEmailSetup'),
                                text: $filter('translate')('pages.currencyManagement.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.AddCurrencyEmailBtnIconClass = "";
                            $scope.AddCurrencyEmailBtnDisabled = false;

                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.currencyManagement.fillvalidfeild'))
            }
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.currencyManagement.error'), msg);
        };

    });



