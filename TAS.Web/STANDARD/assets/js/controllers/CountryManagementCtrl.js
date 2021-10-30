app.controller('CountryManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Country Management";
        $scope.ModalDescription = "Add Edit Country Information";
        $scope.CountrySaveBtnIconClass = "";
        $scope.CountrySaveBtnDisabled = false;
        $scope.CountryTaxSaveBtnIconClass = "";
        $scope.CountryTaxSaveBtnDisabled = false;
        $scope.loadInitailData = function () { }
        $scope.gridOptionsTaxGridloading = false;
        $scope.TaxGridloadAttempted = false;
        $scope.MakeList = [];
        $scope.SelectedMakeList = [];
        $scope.SelectedMakeDList = [];

        $scope.ModelList = [];
        $scope.SelectedModelList = [];
        $scope.SelectedModelDList = [];

        $scope.dealersByCountry = [];

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.countryManagement.error'), msg);
        };

        $scope.settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: false,
            showUncheckAll: false,
            closeOnBlur: true,
            closeOnSelect: true

        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('pages.countryManagement.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('pages.countryManagement.ItemSelected')
        };
        $scope.Modeles = [];

        function AddMake() {
            var index = 0;
            $scope.MakeList = [];
            angular.forEach($scope.Makes, function (value) {
                var x = { id: index, code: value.Id, label: value.MakeName };
                $scope.MakeList.push(x);
                index = index + 1;
            });
        }
        function LoadMake() {
            $scope.SelectedMakeList = [];
            $scope.SelectedMakeDList = [];
            var selectedMakesList = [];
            angular.forEach($scope.Country.Makes, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedMakeList.push(x);
                        $scope.SelectedMakeDList.push(valueIn.label);
                        selectedMakesList.push(valueIn.code);
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeIds',
                    data: { "makeIdList": selectedMakesList },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var temp = data;
                    $scope.Modeles = [];
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
            swal({ title: $filter('translate')('pages.countryManagement.TASInformation'), text: $filter('translate')('pages.countryManagement.loadingModels'), showConfirmButton: false });
            $scope.Country.Modeles = [];
            $scope.SelectedModelDList = [];

            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];

            $scope.Modeles = [];
            $scope.SelectedMakeDList = [];

            var selectedMakesList = [];
            angular.forEach($scope.SelectedMakeList, function (selectedMake) {
                var orgMake = $scope.MakeList.filter(a => a.id == selectedMake.id);
                selectedMakesList.push(orgMake[0].code);
            });

            $scope.Country.Makes = selectedMakesList;

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeIds',
                data: { "makeIdList": selectedMakesList },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Modeles = data;
                AddModel();
                swal.close();
            }).error(function (data, status, headers, config) {
                swal.close();
            });


        }

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

        $scope.Country = {
            Id: "00000000-0000-0000-0000-000000000000",
            CountryCode: '',
            PhoneCode: '',
            CountryName: '',
            CurrencyId: "00000000-0000-0000-0000-000000000000",
            IsActive: false
        };
        $scope.CountryTax = {
            Id: "00000000-0000-0000-0000-000000000000",
            CountryId: "00000000-0000-0000-0000-000000000000",
            TaxTypeId: "00000000-0000-0000-0000-000000000000",
            TaxValue: 0,
            IsOnPreviousTax: false,
            IsOnNRP: false,
            IsOnGross: true,
            MinimumValue: 0,
            IsPercentage: false,
            TpaCurrencyId: "00000000-0000-0000-0000-000000000000",
            currencyPeriodId: "00000000-0000-0000-0000-000000000000"
        };
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllCountrys',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllTaxTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.AllTaxes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data;
                AddMake();
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetCurrencies',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Currencies = data;
            }).error(function (data, status, headers, config) {
            });
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/Country/GetAllContactTaxes',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.AllContactTaxes = data;
            //}).error(function (data, status, headers, config) {
            //});
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/Country/GetAllContactTaxes',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.AllContactTaxes = data;
            //}).error(function (data, status, headers, config) {
            //});
        }
        LoadDetails();
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";

        $scope.CountryTaxesList = [];// [{ Id: "", TaxValue: "", MinimumValue: "", IsPercentage: "", IsOnPreviousTax: "" }, { Id: "", TaxValue: "", MinimumValue: "", IsPercentage: "", IsOnPreviousTax: "" }];
        $scope.gridOptionsTax = {
            data: 'CountryTaxesList',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            multiSelect: false,
            columnDefs: [{
                field: "TaxName",
                displayName: $filter('translate')('pages.countryManagement.Tax')
            }, {
                field: "TaxValue",
                    displayName: $filter('translate')('pages.countryManagement.TaxValue')
            }, {
                field: "MinimumValue",
                    displayName: $filter('translate')('pages.countryManagement.MinimumValue')
            }, {
                field: "IsPercentage",
                    displayName: $filter('translate')('pages.countryManagement.Percentage')
            }, {
                field: "IsOnPreviousTax",
                    displayName: $filter('translate')('pages.countryManagement.IsOnPreviousTax')
            }, {
                field: "IsOnNRP",
                    displayName: $filter('translate')('pages.countryManagement.IsOnNRP')
            }, {
                field: "IsOnGross",
                    displayName: $filter('translate')('pages.countryManagement.IsOnGross')
            }, {
                name: ' ',
                    displayName: $filter('translate')('pages.countryManagement.RemoveTax'),
                cellTemplate: '<div class="center"><button ng-click="grid.appScope.generateReport(row)" class="btn btn-xs btn-warning">Remove</button></div>' //'<div><button ng-click="clickHandler()"></button></div>'
            }, {
                field: "Id",
                visible: false
            }
            ]
        };
        $scope.generateReport = function (row) {
            // alert("Test " + row.entity.Id);
            angular.forEach($scope.CountryTaxes, function (value) {
                if (value.Id == row.entity.Id) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/DeleteCountryTaxes',
                        data: value,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                                text: $filter('translate')('pages.countryManagement.SuccessfullyRemoved'),
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/GetAllCountryTaxessFromCountryId',
                                data: { "Id": $scope.CountryTax.CountryId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.CountryTaxes = data;
                                LoadTaxGrid();
                            }).error(function (data, status, headers, config) {
                            });
                            clearCountryTaxControls();
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                                text: $filter('translate')('pages.countryManagement.Erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                            text: $filter('translate')('pages.countryManagement.deletingTaxes'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    });
                }
            });
        };

        $scope.gridOptionsTax.onRegisterApi = function (gridApi) {
            $scope.gridApiTaxes = gridApi;
            gridApi.selection.on.rowSelectionChanged($scope, LoadCountryTaxes);
        }
        function LoadCountryTaxes() {
            clearCountryTaxControls();
            var SelectedRows = $scope.gridApiTaxes.selection.getSelectedRows();
            if (SelectedRows.length == 0) {
                var c = $scope.CountryTax.CountryId;
                $scope.CountryTax = {};
                $scope.CountryTax.CountryId = c;
                $scope.CountryTax.Id = "00000000-0000-0000-0000-000000000000";
                return false;
            }
            angular.forEach(SelectedRows, function (value) {
                angular.forEach($scope.AllTaxes, function (valueT) {
                    if (value.TaxName == valueT.TaxName) {
                        $scope.CountryTax.TaxTypeId = valueT.Id;
                        $scope.Taxes.push(valueT);
                        angular.forEach($scope.CountryTaxes, function (valueC) {
                            if (valueT.Id == valueC.TaxTypeId) {
                                $scope.CountryTax.Id = valueC.Id;
                                $scope.CountryTax.TaxValue = valueC.TaxValue;
                                $scope.CountryTax.MinimumValue = valueC.MinimumValue;
                                $scope.CountryTax.IsPercentage = valueC.IsPercentage;
                                $scope.CountryTax.IsOnPreviousTax = valueC.IsOnPreviousTax;
                                $scope.CountryTax.IsOnGross = valueC.IsOnGross;
                                $scope.CountryTax.IsOnNRP = valueC.IsOnNRP;
                            }
                        });
                    }
                });
            });
        }

        $scope.SetCountryValues = function () {
            var cId = $scope.Country.Id;
            clearCountryControls();
            $scope.Country.Id = cId;
            $scope.errorTab1 = "";
            if ($scope.Country.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Country/GetCountryById',
                    data: { "Id": $scope.Country.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Country.Id = data.Id;
                    $scope.Country.CountryCode = data.CountryCode;
                    $scope.Country.CountryName = data.CountryName;
                    $scope.Country.CountryId = data.CountryId;
                    $scope.Country.PhoneCode = parseInt(data.PhoneCode);
                    $scope.Country.Makes = data.Makes;
                    $scope.Country.Modeles = data.Modeles;
                    $scope.Country.CurrencyId = data.CurrencyId;
                    LoadMake();
                    $scope.Country.IsActive = data.IsActive;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/GetAllCountryTaxessFromCountryId',
                        data: { "Id": $scope.Country.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.CountryTaxes = data;
                    }).error(function (data, status, headers, config) {
                    });
                }).error(function (data, status, headers, config) {
                    clearCountryControls();
                });
            }
            else {
                clearCountryControls();
            }
        }
        $scope.SetCountryTaxValues = function () {
            $scope.gridOptionsTaxGridloading = true;
            $scope.TaxGridloadAttempted = false;
            $scope.errorTab1 = "";
            if ($scope.CountryTax.CountryId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Country/GetAllCountryTaxessFromCountryId',
                    data: { "Id": $scope.CountryTax.CountryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CountryTaxes = data;
                    $scope.CountryTax.TaxTypeId = "";
                    LoadTaxGrid();
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.Gross = function () {
            if ($scope.CountryTax.IsOnGross)
                $scope.CountryTax.IsOnNRP = false;
        }
        $scope.NRP = function () {
            if ($scope.CountryTax.IsOnNRP)
                $scope.CountryTax.IsOnGross = false;
        }
        function clearCountryControls() {
            $scope.Country.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Country.CountryCode = "";
            $scope.Country.CountryName = "";
            $scope.Country.CountryId = "";
            $scope.Country.CurrencyId = "00000000-0000-0000-0000-000000000000";
            $scope.Country.IsActive = false;
            $scope.Country.PhoneCode = "";
            $scope.SelectedMakeList = [];
            $scope.SelectedMakeDList = [];
            $scope.ModelList = [];
            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];
        }
        function clearCountryTaxControls() {
            $scope.CountryTax.Id = "00000000-0000-0000-0000-000000000000";
            $scope.CountryTax.TaxTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.CountryTax.TaxValue = 0;
            $scope.CountryTax.IsOnPreviousTax = false;
            $scope.CountryTax.IsOnGross = true;
            $scope.CountryTax.IsOnNRP = false;
            $scope.CountryTax.MinimumValue = 0;
            $scope.CountryTax.IsPercentage = false;
        }
        function LoadTaxGrid() {
            $scope.Index = 0;
            $scope.CountryTaxesList = [];
            angular.forEach($scope.CountryTaxes, function (value) {
                if ($scope.Index < value.Index)
                    $scope.Index = value.Index;
                var tax = {};
                angular.forEach($scope.AllTaxes, function (valueT) {
                    if (value.TaxTypeId == valueT.Id)
                        tax.TaxName = valueT.TaxName;
                });
                tax.Id = value.Id;
                tax.TaxValue = value.TaxValue;
                tax.MinimumValue = value.MinimumValue;
                if (value.IsPercentage)
                    tax.IsPercentage = 'YES';
                else
                    tax.IsPercentage = 'NO';

                if (value.IsOnPreviousTax)
                    tax.IsOnPreviousTax = 'YES';
                else
                    tax.IsOnPreviousTax = 'NO';

                if (value.IsOnNRP)
                    tax.IsOnNRP = 'YES';
                else
                    tax.IsOnNRP = 'NO';

                if (value.IsOnGross)
                    tax.IsOnGross = 'YES';
                else
                    tax.IsOnGross = 'NO';
                $scope.CountryTaxesList.push(tax);
            });
            LoadTaxList();

        }
        function LoadTaxList() {
            $scope.Taxes = [];
            angular.forEach($scope.AllTaxes, function (valueT) {
                var add = true;
                angular.forEach($scope.CountryTaxes, function (value) {
                    if (value.TaxTypeId == valueT.Id)
                        add = false;
                });
                if (add) {
                    $scope.Taxes.push(valueT);
                }
            });
            $scope.gridOptionsTaxGridloading = false;
            $scope.TaxGridloadAttempted = true;

        }


        $scope.validateCountry = function () {

            var isValid = true;
            if ($scope.Country.CountryName == undefined || $scope.Country.CountryName == "") {
                $scope.validate_CountryName = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryName = "";
            }

            if ($scope.Country.CountryCode == undefined || $scope.Country.CountryCode == "") {
                $scope.validate_CountryCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryCode = "";
            }

            if ($scope.Country.PhoneCode == undefined || $scope.Country.PhoneCode == "") {
                $scope.validate_PhoneCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_PhoneCode = "";
            }

            if (!isGuid($scope.Country.CurrencyId)) {
                $scope.validate_CountryCurrency = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryCurrency = "";
            }
            return isValid

        }

        $scope.isValidCountryData = function () {
            var isValid = true;
            if (!$scope.validateCharacter($scope.Country.CountryName)) {
                customErrorMessage($filter('translate')('pages.countryManagement.errornumericcharacters'))
                isValid = false;
            }
            if (!$scope.validateCharacter($scope.Country.CountryCode)) {
                customErrorMessage($filter('translate')('pages.countryManagement.errornumericcharacters'))
                isValid = false;
            }
            return isValid
        }

        $scope.CountrySubmit = function () {
            if ($scope.validateCountry()) {
                if ($scope.isValidCountryData()) {
                    var exists = false;
                    $scope.errorTab1 = "";
                    if ($scope.Country.Id == null || $scope.Country.Id == "00000000-0000-0000-0000-000000000000") {
                        angular.forEach($scope.Countries, function (value) {
                            if (value.CountryName == $scope.Country.CountryName) {
                                exists = true;
                            }
                        });
                        if (!exists) {
                            $scope.CountrySaveBtnIconClass = "fa fa-spinner fa-spin";
                            $scope.CountrySaveBtnDisabled = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/AddCountry',
                                data: $scope.Country,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.CountrySaveBtnIconClass = "";
                                $scope.CountrySaveBtnDisabled = false;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.countryManagement.countryInformation'),
                                        text: $filter('translate')('pages.countryManagement.successfullySaved'),
                                        confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                        confirmButtonColor: "#007AFF"
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/Country/GetAllCountrys',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Countries = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    clearCountryControls();
                                }
                                else {
                                }

                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.countryManagement.countryInformation'),
                                    text: $filter('translate')('pages.countryManagement.Erroroccuredsavingdata'),
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                $scope.CountrySaveBtnIconClass = "";
                                $scope.CountrySaveBtnDisabled = false;
                                return false;
                            });
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryInformation'),
                                text: $filter('translate')('pages.countryManagement.alreadyAdded'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            clearCountryControls();
                        }
                    }
                    else {
                        angular.forEach($scope.Countries, function (value) {
                            if (value.CountryName == $scope.Country.CountryName && value.Id != $scope.Country.Id) {
                                exists = true;
                            }
                        });
                        if (!exists) {
                            $scope.CountrySaveBtnIconClass = "fa fa-spinner fa-spin";
                            $scope.CountrySaveBtnDisabled = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/UpdateCountry',
                                data: $scope.Country,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.CountrySaveBtnIconClass = "";
                                $scope.CountrySaveBtnDisabled = false;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.countryManagement.countryInformation'),
                                        text: $filter('translate')('pages.countryManagement.successfullySaved'),
                                        confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                        confirmButtonColor: "#007AFF"
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/Country/GetAllCountrys',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Countries = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    clearCountryControls();
                                }
                                else {;
                                }
                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.countryManagement.countryInformation'),
                                    text: $filter('translate')('pages.countryManagement.Erroroccuredsavingdata'),
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                $scope.CountrySaveBtnIconClass = "";
                                $scope.CountrySaveBtnDisabled = false;
                                return false;
                            });
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryInformation'),
                                text: $filter('translate')('pages.countryManagement.alreadyAdded'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            clearCountryControls();
                        }
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.countryManagement.fillvalidfeild'))
            }

        }

        $scope.validateCountryTax = function () {
            var validated = true;
            if (!isGuid($scope.CountryTax.CountryId)) {
                //customErrorMessage("Selected country is invalid");
                $scope.validate_CountryId = "has-error";
                validated = false;
            } else {
                $scope.validate_CountryId = "";
            }
            if (validated) {
                if (!isGuid($scope.CountryTax.TaxTypeId)) {
                    //customErrorMessage("Selected TPA is invalid");
                    $scope.validate_TaxTypeId = "has-error";
                    validated = false;
                } else {
                    $scope.validate_TaxTypeId = "";
                }
                if (validated) {
                    if (!parseFloat($scope.CountryTax.TaxValue) || !parseFloat($scope.CountryTax.TaxValue) > 0) {
                        customErrorMessage($filter('translate')('pages.countryManagement.taxvaluegraterthan'));
                        $scope.validate_Taxvalue = "has-error";
                        validated = false;
                    }
                    else {
                        $scope.validate_Taxvalue = "";
                    }
                    if (!parseFloat($scope.CountryTax.MinimumValue)) {
                        $scope.CountryTax.MinimumValue = 0.00;
                    }


                }
                if ($scope.CountryTax.IsPercentage || $scope.CountryTax.IsOnPreviousTax || $scope.CountryTax.IsOnNRP) {



                } else {
                    customErrorMessage($filter('translate')('pages.countryManagement.selectatleastone'));
                    validated = false;
                }
            }
            return validated;

        }
        $scope.CountryTaxSubmit = function () {
            if ($scope.validateCountryTax()) {
                if ($scope.CountryTax.Id == null || $scope.CountryTax.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.CountryTax.Index = $scope.Index + 1;
                    $scope.CountryTaxSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.CountryTaxSaveBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Country/AddCountryTaxes',
                        data: $scope.CountryTax,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }

                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.CountryTaxSaveBtnIconClass = "";
                        $scope.CountryTaxSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                                text: $filter('translate')('pages.countryManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/GetAllCountryTaxessFromCountryId',
                                data: { "Id": $scope.CountryTax.CountryId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.CountryTaxes = data;
                                LoadTaxGrid();
                            }).error(function (data, status, headers, config) {
                            });
                            clearCountryTaxControls();
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                                text: "Selected country doesn't have set the default currency. Please update it first!",
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                            text: $filter('translate')('pages.countryManagement.Erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.CountryTaxSaveBtnIconClass = "";
                        $scope.CountryTaxSaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.CountryTaxSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.CountryTaxSaveBtnDisabled = true;
                    $http(

                        {
                        method: 'POST',
                        url: '/TAS.Web/api/Country/UpdateCountryTaxes',
                        data: $scope.CountryTax,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.CountryTaxSaveBtnIconClass = "";
                        $scope.CountryTaxSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                                text: $filter('translate')('pages.countryManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Country/GetAllCountryTaxessFromCountryId',
                                data: { "Id": $scope.CountryTax.CountryId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.CountryTaxes = data;
                                LoadTaxGrid();
                            }).error(function (data, status, headers, config) {
                            });
                            clearCountryTaxControls();
                        }
                        else {;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.countryManagement.countryTaxInformation'),
                            text: $filter('translate')('pages.countryManagement.Erroroccuredsavingdata'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.countryManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.CountryTaxSaveBtnIconClass = "";
                        $scope.CountryTaxSaveBtnDisabled = false;
                        return false;

                    });
                }
            } else {
                customErrorMessage($filter('translate')('pages.countryManagement.fillvalidfeild'))
            }
        }


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
            //toast ng-scope toast-error
            if ($("#toast-container").children().length == 0) { toaster.pop('error', $filter('translate')('pages.countryManagement.error'), msg); }
        };
    });



