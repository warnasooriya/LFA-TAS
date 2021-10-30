app.controller('DealerManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Dealer Management";
        $scope.ModalDescription = "Add Edit Dealer Information";
        $scope.MakeList = [];
        $scope.MakeId = "00000000-0000-0000-0000-000000000000";
        $scope.UserList = [];
        $scope.UserId = "00000000-0000-0000-0000-000000000000";
        $scope.dealerSaveBtnIconClass = "";
        $scope.dealerSaveBtnDisabled = false;
        $scope.dealerBranchSaveBtnIconClass = "";
        $scope.dealerBranchSaveBtnDisabled = false;
        $scope.DealerBranchId = "00000000-0000-0000-0000-000000000000";

        $scope.labelSaveDealer = 'common.button.save';
        $scope.labelSaveDealerBranch = 'common.button.save';
        $scope.labelSaveDealerStaff = 'common.button.save';

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };

        var customWarningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('common.popUpMessages.warning'), msg, 12000);
        };

        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };

        $scope.selectedpp = {};
        $scope.MakeList = [];
        $scope.SelectedMakeList = [];
        $scope.SelectedMakeDList = [];
        $scope.UserList = [];
        $scope.SelectedUserList = [];

        $scope.BranchesList = [];
        $scope.BranchesSelectedList = [];
        $scope.BranchesSelectedDList = [];

        $scope.SelectedUserDList = [];
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
            buttonDefaultText: $filter('translate')('common.customText.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('common.customText.itemSelected')
        };

        $scope.loadInitailData = function () { }
        $scope.DealerLocation = {
            Id: "00000000-0000-0000-0000-000000000000",
            DealerId: "00000000-0000-0000-0000-000000000000",
            CityId: "00000000-0000-0000-0000-000000000000",
            SalesContactPerson: "",
            SalesTelephone: "",
            DealerAddress:"",
            SalesFax: "",
            SalesEmail: "",
            Location: "",
            ServiceContactPerson: "",
            ServiceTelephone: "",
            ServiceFax: "",
            ServiceEmail: "",
            HeadOfficeBranch: "",
            TpaBranchId: "00000000-0000-0000-0000-000000000000"
        };
        $scope.Dealer = {
            Id: "00000000-0000-0000-0000-000000000000",
            DealerName: "",
            DealerCode: '',
            DealerAliase: '',
            CountryId: '00000000-0000-0000-0000-000000000000',
            Type: '',
            CommodityTypeId: '00000000-0000-0000-0000-000000000000',
            InsurerId: '',
            Makes: [],
            CityId: '00000000-0000-0000-0000-000000000000',
            Location: '',
            CurrencyId:"",
            ManHourRate:0.00,
            IsActive: false,
            IsAutoApproval: false
        };

        $scope.DealerLabourCharge = {
            Id: "00000000-0000-0000-0000-000000000000",
            DealerId: "00000000-0000-0000-0000-000000000000",
            CountryId: "00000000-0000-0000-0000-000000000000",
            MakeId: "00000000-0000-0000-0000-000000000000",
            ModelId: [],
            CurrencyId: "00000000-0000-0000-0000-000000000000",
            CurrencyPeriodId: "00000000-0000-0000-0000-000000000000",
            CurrencyPeriodId: "00000000-0000-0000-0000-000000000000",
            StartDate: "",
            EndDate: "",
            LabourChargeValue: ""
        };

        var paginationOptionsDealerLabourChargeSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        function LoadDetails() {
            getDealerLabourChargeSearchPage();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
            }).error(function (data, status, headers, config) {
            });//CommodityTypes
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
            }).error(function (data, status, headers, config) {
            });//Dealers

            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUsers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Users = data;
                AddUser();
            }).error(function (data, status, headers, config) {
            });//Users
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data;
                AddMake();
            }).error(function (data, status, headers, config) {
            });//Makes
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });//Countries
            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Insurers = data;
            }).error(function (data, status, headers, config) {
            });//Insurers
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/DealerManagement/GetAllDealerLocations',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.DealerLocations = data;
            //    AddBranches();
            //}).error(function (data, status, headers, config) {
            //});//DealerLocations
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetCurrencies',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Currencies = data;
            }).error(function (data, status, headers, config) {
            });//Currencies

            //tpa branches
            $http({
                method: 'POST',
                url: '/TAS.Web/api/TPABranch/GetTPABranches',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.tpaBranches = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.make = data;
            }).error(function (data, status, headers, config) {
            });

        }
        LoadDetails();
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";
        $scope.errorTab3 = "";
        $scope.SetCities = function () {
            $scope.errorTab1 = "";
            if ($scope.Dealer.CountryId != null) {
                $scope.CityDisable = false;
                angular.forEach($scope.Countries, function (value, key) {
                    if (value.Id == $scope.Dealer.CountryId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                            data: { "countryId": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Cities = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }
        $scope.SetDealerValues = function () {
            $scope.errorTab1 = "";
            if ($scope.Dealer.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetDealerById',
                    data: { "Id": $scope.Dealer.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Dealer.Id = data.Id;
                    $scope.Dealer.DealerCode = data.DealerCode;
                    $scope.Dealer.DealerName = data.DealerName;
                    $scope.Dealer.DealerAliase = data.DealerAliase;
                    $scope.Dealer.CountryId = data.CountryId;
                    $scope.Dealer.CurrencyId = data.CurrencyId;
                    $scope.Dealer.Type = data.Type;
                    $scope.Dealer.CommodityTypeId = data.CommodityTypeId;
                    $scope.Dealer.InsurerId = data.InsurerId;
                    $scope.Dealer.Makes = data.Makes;
                    $scope.Dealer.CityId = data.CityId;
                    $scope.Dealer.Location = data.Location;
                    $scope.Dealer.IsActive = data.IsActive;
                    $scope.Dealer.IsAutoApproval = data.IsAutoApproval;
                    $scope.Dealer.ManHourRate = data.ManHourRate;
                    $scope.labelSaveDealer = 'common.button.update';
                    $scope.SetCities();
                    LoadMake();
                }).error(function (data, status, headers, config) {
                    clearDealerControls();
                });
            }
            else {
                clearDealerControls();
            }
        }
        function clearDealerControls() {
            $scope.Dealer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Dealer.DealerCode = "";
            $scope.Dealer.DealerName = "";
            $scope.Dealer.DealerAliase = "";
            $scope.Dealer.CountryId = "";
            $scope.Dealer.CurrencyId = "";
            $scope.Dealer.Type = "";
            $scope.Dealer.ManHourRate = 0.00;

            $scope.Dealer.CommodityTypeId = "";
            $scope.Dealer.InsurerId = "";
            $scope.Dealer.Makes = [];
            $scope.Dealer.CityId = "00000000-0000-0000-0000-000000000000";
            $scope.Dealer.Location = "";
            $scope.Dealer.IsActive = false;
            $scope.Dealer.IsAutoApproval = false;
            $scope.SelectedMakeList = [];
            $scope.SelectedMakeDList = [];
            $scope.labelSaveDealer = 'common.button.save';
        }

        $scope.validateDealer = function () {
            var isValid = true;
            if ($scope.Dealer.DealerName == "" || $scope.Dealer.DealerName == undefined) {
                $scope.validate_DealerName = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerName = "";
            }
            if ($scope.Dealer.DealerCode == "" || $scope.Dealer.DealerCode == undefined) {
                $scope.validate_DealerCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerCode = "";
            }
            if ($scope.Dealer.Type == "" || $scope.Dealer.Type == undefined) {
                $scope.validate_Type = "has-error";
                isValid = false;
            } else {
                $scope.validate_Type = "";
            }
            if ($scope.Dealer.InsurerId == "" || $scope.Dealer.InsurerId == undefined) {
                $scope.validate_InsurerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_InsurerId = "";
            }
            if ($scope.Dealer.CommodityTypeId == "" || $scope.Dealer.CommodityTypeId == undefined || $scope.Dealer.CommodityTypeId == null
                || $scope.Dealer.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CommodityTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityTypeId = "";
            }
            if ($scope.Dealer.CountryId == "" || $scope.Dealer.CountryId == undefined || $scope.Dealer.CountryId == null
                || $scope.Dealer.CountryId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryId = "";
            }
            if ($scope.Dealer.CurrencyId == "" || $scope.Dealer.CurrencyId == undefined || $scope.Dealer.CurrencyId == null) {
                $scope.validate_CurrencyId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CurrencyId = "";
            }

            if ((!parseFloat($scope.Dealer.ManHourRate) || parseFloat($scope.Dealer.ManHourRate) == 0)) {
                $scope.validate_ManHourRate = "has-error";
                isValid = false;
            } else {
                $scope.validate_ManHourRate = "";
            }

            if ($scope.SelectedMakeList.length == 0) {
                $scope.validate_SelectedMakeList = "has-error";
                isValid = false;
            } else {
                $scope.validate_SelectedMakeList = "";
            }

            return isValid
        }

        $scope.DealerSubmit = function () {
            $scope.SendMake();
            if ($scope.validateDealer()) {
                $scope.errorTab1 = "";
                if ($scope.Dealer.Id == null || $scope.Dealer.Id == "00000000-0000-0000-0000-000000000000") {

                    $scope.dealerSaveBtnDisabled = true;
                    $scope.dealerSaveBtnIconClass = "fa fa-spinner fa-spin";

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/DealerManagement/AddDealer',
                        data: $scope.Dealer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.dealerSaveBtnIconClass = "";
                        $scope.dealerSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Dealers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearDealerControls();
                        } else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                                text: data,
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.dealerSaveBtnIconClass = "";
                        $scope.dealerSaveBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.dealerSaveBtnDisabled = true;
                    $scope.dealerSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/DealerManagement/UpdateDealer',
                        data: $scope.Dealer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.dealerSaveBtnIconClass = "";
                        $scope.dealerSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Dealers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearDealerControls();
                        } else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                                text: data,
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.dealerSaveBtnIconClass = "";
                        $scope.dealerSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }

        var PhoneCode = "";
        $scope.SetDealerLocationsValues = function () {
            if ($scope.DealerLocation.DealerId != null) {
                $scope.Disable = false;
                angular.forEach($scope.Dealers, function (value, key) {
                    if (value.Id == $scope.DealerLocation.DealerId) {
                        angular.forEach($scope.Countries, function (valueC, key) {
                            if (value.CountryId == valueC.Id) {
                                PhoneCode = valueC.PhoneCode;
                                $scope.DealerLocation.SalesTelephone = valueC.PhoneCode;
                                $scope.DealerLocation.SalesFax = valueC.PhoneCode;
                                $scope.DealerLocation.ServiceTelephone = valueC.PhoneCode;
                                $scope.DealerLocation.ServiceFax = valueC.PhoneCode;
                            }
                        });
                        swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('pages.dealerManagement.sucessMessages.dealerLoading'), showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByDealerId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.DealerLocations = data;
                            swal.close();
                        }).error(function (data, status, headers, config) {
                            swal.close();
                        });
                        $scope.CityDisableByDealer = false;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                            data: { "countryId": value.CountryId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.DealerLocationCities = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }

                });
            }
        }
        $scope.SetDealerLocationValues = function () {
            $scope.errorTab2 = "";
            if ($scope.DealerLocation.Id != null) {
                swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchLoading'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetDealerContrctById',
                    data: { "Id": $scope.DealerLocation.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.DealerLocation.Id = data.Id;
                    $scope.DealerLocation.DealerId = data.DealerId;
                    $scope.DealerLocation.CityId = data.CityId;
                    $scope.DealerLocation.DealerAddress = data.DealerAddress;
                    $scope.DealerLocation.SalesContactPerson = data.SalesContactPerson;
                    $scope.DealerLocation.SalesTelephone = data.SalesTelephone;
                    $scope.DealerLocation.SalesFax = data.SalesFax;
                    $scope.DealerLocation.SalesEmail = data.SalesEmail;
                    $scope.DealerLocation.Location = data.Location;
                    $scope.DealerLocation.LocationCode = data.LocationCode;
                    $scope.DealerLocation.ServiceContactPerson = data.ServiceContactPerson;
                    $scope.DealerLocation.ServiceTelephone = data.ServiceTelephone;
                    $scope.DealerLocation.ServiceFax = data.ServiceFax;
                    $scope.DealerLocation.ServiceEmail = data.ServiceEmail;
                    $scope.DealerLocation.HeadOfficeBranch = data.HeadOfficeBranch;
                    $scope.DealerLocation.TpaBranchId = data.TpaBranchId;
                    $scope.labelSaveDealerBranch = 'common.button.update';
                    swal.close();
                }).error(function (data, status, headers, config) {
                    clearDealerLocationControls();
                    swal.close();
                });
            }
            else {
                clearDealerLocationControls();
            }
        }
        function clearDealerLocationControls() {
            $scope.DealerLocation.Id = "00000000-0000-0000-0000-000000000000";
            //   $scope.DealerLocation.DealerId ="";
            $scope.DealerLocation.CityId = "";
            $scope.DealerLocation.SalesContactPerson = "";
            $scope.DealerLocation.SalesTelephone = "";
            $scope.DealerLocation.DealerAddress = "";
            $scope.DealerLocation.SalesFax = "";
            $scope.DealerLocation.SalesEmail = "";
            $scope.DealerLocation.Location = "";
            $scope.DealerLocation.LocationCode = "";
            $scope.DealerLocation.ServiceContactPerson = "";
            $scope.DealerLocation.ServiceTelephone = "";
            $scope.DealerLocation.ServiceFax = "";
            $scope.DealerLocation.ServiceEmail = "";
            $scope.DealerLocation.HeadOfficeBranch = false;
            $scope.DealerLocation.TpaBranchId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLocation.SalesTelephone = PhoneCode;
            $scope.DealerLocation.SalesFax = PhoneCode;
            $scope.DealerLocation.ServiceTelephone = PhoneCode;
            $scope.DealerLocation.ServiceFax = PhoneCode;
            $scope.labelSaveDealerBranch = 'common.button.save';
        }
        var IsHeadOfficeBranch = false;
        function CheckHeadOfficeBranch() {
            angular.forEach($scope.DealerLocations, function (value) {
                if ($scope.DealerLocation.HeadOfficeBranch && value.Id != $scope.DealerLocation.Id &&
                    value.HeadOfficeBranch) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchInformation'),
                        text: $filter('translate')('pages.dealerManagement.sucessMessages.headOfficeAlready'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.DealerLocation.HeadOfficeBranch = false;
                    IsHeadOfficeBranch = false;
                    return false;
                }
                else {
                    IsHeadOfficeBranch = true;
                }
            });
        }

        $scope.validateDealerBranch = function () {
            var isValid = true;
            if ($scope.DealerLocation.DealerId == "" || $scope.DealerLocation.DealerId == undefined || $scope.DealerLocation.DealerId == null
                || $scope.DealerLocation.DealerId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_dealerLocationDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_dealerLocationDealerId = "";
            }
            if ($scope.DealerLocation.CityId == "" || $scope.DealerLocation.CityId == undefined || $scope.DealerLocation.CityId == null
                || $scope.DealerLocation.CityId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CityId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CityId = "";
            }
            if ($scope.DealerLocation.Location == "" || $scope.DealerLocation.Location == undefined) {
                $scope.validate_Location = "has-error";
                isValid = false;
            } else {
                $scope.validate_Location = "";
            }
            if ($scope.DealerLocation.SalesContactPerson == "" || $scope.DealerLocation.SalesContactPerson == undefined) {
                $scope.validate_SalesContactPerson = "has-error";
                isValid = false;
            } else {
                $scope.validate_SalesContactPerson = "";
            }
            if ($scope.DealerLocation.SalesTelephone == "" || $scope.DealerLocation.SalesTelephone == undefined) {
                $scope.validate_SalesTelephone = "has-error";
                isValid = false;
            } else {
                $scope.validate_SalesTelephone = "";
            }
            if ($scope.DealerLocation.SalesFax == "" || $scope.DealerLocation.SalesFax == undefined) {
                $scope.validate_SalesFax = "has-error";
                isValid = false;
            } else {
                $scope.validate_SalesFax = "";
            }
            if ($scope.DealerLocation.SalesEmail == "" || $scope.DealerLocation.SalesEmail == undefined) {
                $scope.validate_SalesEmail = "has-error";
                isValid = false;
            } else {
                $scope.validate_SalesEmail = "";
            }
            if ($scope.DealerLocation.ServiceContactPerson == "" || $scope.DealerLocation.ServiceContactPerson == undefined) {
                $scope.validate_ServiceContactPerson = "has-error";
                isValid = false;
            } else {
                $scope.validate_ServiceContactPerson = "";
            }
            if ($scope.DealerLocation.ServiceTelephone == "" || $scope.DealerLocation.ServiceTelephone == undefined) {
                $scope.validate_ServiceTelephone = "has-error";
                isValid = false;
            } else {
                $scope.validate_ServiceTelephone = "";
            }
            if ($scope.DealerLocation.ServiceFax == "" || $scope.DealerLocation.ServiceFax == undefined) {
                $scope.validate_ServiceFax = "has-error";
                isValid = false;
            } else {
                $scope.validate_ServiceFax = "";
            }
            if ($scope.DealerLocation.ServiceEmail == "" || $scope.DealerLocation.ServiceEmail == undefined) {
                $scope.validate_ServiceEmail = "has-error";
                isValid = false;
            } if ($scope.DealerLocation.LocationCode == "" || $scope.DealerLocation.LocationCode == undefined) {
                $scope.validate_LocationCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_ServiceEmail = "";
            }
            if ($scope.DealerLocation.TpaBranchId == "00000000-0000-0000-0000-000000000000" || $scope.DealerLocation.TpaBranchId == undefined
                || $scope.DealerLocation.TpaBranchId == null) {
                $scope.validate_TpaBranchId = "has-error";
                isValid = false;
            } else {
                $scope.validate_TpaBranchId = "";

            }

            return isValid
        }

        $scope.DealerLocationSubmit = function () {
            var phoneno = /^\d{10,15}$/;
            if ($scope.validateDealerBranch()) {

                $scope.errorTab2 = "";

                if ($scope.DealerLocation.Location != "") {
                    if ($scope.DealerLocation.HeadOfficeBranch == "") {
                        $scope.DealerLocation.HeadOfficeBranch = false;

                    }
                    $scope.errorTab1 = "";
                    if ($scope.DealerLocation.Id == null || $scope.DealerLocation.Id == "00000000-0000-0000-0000-000000000000") {

                        $scope.dealerBranchSaveBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.dealerBranchSaveBtnDisabled = true;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/AddDealerLocation',
                            data: $scope.DealerLocation,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.dealerBranchSaveBtnIconClass = "";
                            $scope.dealerBranchSaveBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchInformation'),
                                    text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByDealerId',
                                    data: { "Id": $scope.DealerLocation.DealerId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.DealerLocations = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearDealerLocationControls();
                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchInformation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.dealerBranchSaveBtnIconClass = "";
                            $scope.dealerBranchSaveBtnDisabled = false;
                            return false;
                        });

                    }
                    else {

                        $scope.dealerBranchSaveBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.dealerBranchSaveBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/UpdateDealerLocation',
                            data: $scope.DealerLocation,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.dealerBranchSaveBtnIconClass = "";
                            $scope.dealerBranchSaveBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchInformation'),
                                    text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                    confirmButtonText: $filter('translate')('common.button.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/DealerManagement/GetAllDealerLocationsByDealerId',
                                    data: { "Id": $scope.DealerLocation.DealerId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.DealerLocations = data;
                                }).error(function (data, status, headers, config) {
                                });

                                clearDealerLocationControls();

                            } else {;
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerBranchInformation'),
                                text: $filter('translate')('common.errMessage.errorOccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.dealerBranchSaveBtnIconClass = "";
                            $scope.dealerBranchSaveBtnDisabled = false;
                            return false;
                        });
                    }

                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }

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
            angular.forEach($scope.Dealer.Makes, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedMakeList.push(x);
                        $scope.SelectedMakeDList.push(valueIn.label);
                    }
                });
            });
        }
        function LoadModel() {
            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];
            angular.forEach($scope.DealerLabourCharge.ModelId, function (valueOut) {
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
            $scope.DealerLabourCharge.ModelId = [];
            angular.forEach($scope.SelectedModelList, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.DealerLabourCharge.ModelId.push(valueIn.code);
                        $scope.SelectedModelDList.push(valueIn.label);
                    }
                });
            });
        }

        $scope.SendMake = function () {
            $scope.SelectedMakeDList = [];
            $scope.Dealer.Makes = [];
            angular.forEach($scope.SelectedMakeList, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Dealer.Makes.push(valueIn.code);
                        $scope.SelectedMakeDList.push(valueIn.label);
                    }
                });
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



        $scope.Staffs = [];
        $scope.DealerStaff = {
            UserId: "00000000-0000-0000-0000-000000000000",
            DealerId: "00000000-0000-0000-0000-000000000000",
            Id: "00000000-0000-0000-0000-000000000000"
        };
        function AddUser() {
            var index = 0;
            $scope.UserList = [];
            angular.forEach($scope.Users, function (value) {
                var x = { id: index, code: value.Id, label: value.FirstName + ' ' + value.LastName };
                $scope.UserList.push(x);
                index = index + 1;
            });
        }
        function LoadUser() {
            $scope.SelectedUserList = [];
            $scope.SelectedUserDList = [];
            angular.forEach($scope.DealerStaffs, function (valueOut) {
                angular.forEach($scope.UserList, function (valueIn) {
                    if (valueOut.UserId == valueIn.code) {
                        var x = { id: valueIn.id, StaffId: valueOut.Id };
                        $scope.SelectedUserList.push(x);
                        $scope.SelectedUserDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendUser = function () {
            $scope.SelectedUserDList = [];
            $scope.Staffs = [];
            angular.forEach($scope.SelectedUserList, function (valueOut) {
                angular.forEach($scope.UserList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        var val = {
                            UserId: valueIn.code,
                            DealerId: $scope.DealerStaff.DealerId,
                            Id: "00000000-0000-0000-0000-000000000000"
                        };
                        $scope.Staffs.push(val);
                        $scope.SelectedUserDList.push(valueIn.label);


                    }


                    //  alert(valueIn.code);// alert(valueIn.id);
                    //if (valueOut.id == valueIn.id) {
                    //    if ($scope.DealerStaffs.length == 0) {
                    //        var val = {
                    //            UserId: valueIn.code,
                    //            DealerId: $scope.DealerStaff.DealerId,
                    //            Id: "00000000-0000-0000-0000-000000000000"
                    //        };
                    //        $scope.Staffs.push(val);
                    //        $scope.SelectedUserDList.push(valueIn.label);
                    //    }
                    //    else {
                    //        angular.forEach($scope.DealerStaffs, function (val) {
                    //            if (valueIn.code == val.UserId) {
                    //                var val = {
                    //                    UserId: valueIn.code,
                    //                    DealerId: $scope.DealerStaff.DealerId,
                    //                    Id: val.Id
                    //                };
                    //                $scope.Staffs.push(val);
                    //                $scope.SelectedUserDList.push(valueIn.label);
                    //            } else {
                    //                var val = {
                    //                    UserId: valueIn.code,
                    //                    DealerId: $scope.DealerStaff.DealerId,
                    //                    Id: "00000000-0000-0000-0000-000000000000"
                    //                };
                    //                $scope.Staffs.push(val);
                    //                $scope.SelectedUserDList.push(valueIn.label);
                    //            }
                    //        });
                    //    }
                    // }
                });
            });
        }
        function clearDealerStaffsControls() {
            $scope.DealerStaff.DealerId = "00000000-0000-0000-0000-000000000000";
            $scope.SelectedUserList = [];
            $scope.SelectedUserDList = [];
            $scope.BranchesSelectedList = [];
            $scope.BranchesSelectedDList = [];
        }
        //$scope.LoadStaff = function () {
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/DealerManagement/GetDealerStaffByDealerId',
        //        data: { "Id": $scope.DealerStaff.DealerId },
        //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        //        $scope.DealerStaffs = data;
        //        LoadUser();
        //        //DealerLocationById();
        //    }).error(function (data, status, headers, config) {
        //    });//DealerStaff
        //}

        $scope.LoadStaff = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetDealerStaffByDealerIdandBranchId',
                data: { "DealerId": $scope.DealerStaff.DealerId, "BranchId": $scope.DealerBranchId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerStaffs = data;
                LoadUser();
                //DealerLocationById();
            }).error(function (data, status, headers, config) {
            });//DealerStaff
        }


        $scope.LoadBranchersDetails = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealerStaffLocationsByDealerId',
                data: { "Id": $scope.DealerStaff.DealerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerLocations = data;
                AddBranches();
                LoadBranches();
                $scope.SelectedUserList = [];
                $scope.SelectedUserDList = [];
            }).error(function (data, status, headers, config) {
            });
        }


        $scope.validateDealerStaff = function () {
            var isValid = true;
            if ($scope.DealerStaff.DealerId == "" || $scope.DealerStaff.DealerId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DSDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DSDealerId = "";
            }
            if ($scope.SelectedUserList.length == 0) {
                $scope.validate_SelectedUserList = "has-error";
                isValid = false;
            } else {
                $scope.SelectedUserList = "";
            }

            if ($scope.BranchesSelectedList.length == 0) {
                $scope.validate_BranchesSelectedList = "has-error";
                isValid = false;
            } else {
                $scope.validate_BranchesSelectedList = "";
            }
            return isValid
        }



        $scope.DealerStaffSubmit = function () {
            //if ($scope.DealerStaff.DealerId == "" || $scope.DealerStaff.DealerId == "00000000-0000-0000-0000-000000000000" || $scope.SelectedUserList.length == 0) {
            //    $scope.errorTab3 = "Please Select ";
            //    if ($scope.DealerStaff.DealerId == "" || $scope.DealerStaff.DealerId == "00000000-0000-0000-0000-000000000000") {
            //        $scope.errorTab3 = $scope.errorTab3 + "Dealer, ";
            //    }
            //    if ($scope.SelectedUserList.length == 0) {
            //        $scope.errorTab3 = $scope.errorTab3 + "Users";
            //    }
            //    $scope.errorTab3 = $scope.errorTab3.substring(0, $scope.errorTab3.length - 1);
            //    return false;
            //}
            if ($scope.validateDealerStaff()) {
                //$scope.SendUser();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/AddDealerStaff',
                    data: { "Staff": $scope.Staffs, "DealerBranch": $scope.DealerBranchStaff },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Assigned) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                            text: data.message + "\n ( "+ data.userExistList.toString() + " ) " ,
                            type: "error",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        return false;
                    } else {
                        $scope.Ok = data;
                        SweetAlert.swal({
                            title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                            text: $filter('translate')('common.sucessMessages.successfullySaved'),
                            type: "success",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        clearDealerStaffsControls();
                        $scope.errorTab3 = "";
                        return true;

                    }
                 //   return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerInformation'),
                        text: $filter('translate')('common.errMessage.errorOccured'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    clearDealerStaffsControls();
                    $scope.errorTab3 = "";
                    return false;
                });
            }
            else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }

        function DealerLocationById() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealerStaffLocationsByDealerId',
                data: { "Id": $scope.DealerStaff.DealerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerLocations = data;
                LoadBranches();
            }).error(function (data, status, headers, config) {
            });
        }

        function AddBranches() {
            var index = 0;
            $scope.BranchesList = [];
            angular.forEach($scope.DealerLocations, function (value) {
                for (var j = 0; j < value.length; j++) {
                    var x = { id: index, code: value[j].Id, label: value[j].Location };
                    $scope.BranchesList.push(x);
                    index = index + 1;
                }

            });
        }

        function LoadBranches() {
            $scope.BranchesSelectedList = [];
            $scope.BranchesSelectedDList = [];
            angular.forEach($scope.DealerLocations, function (valueOut) {
                angular.forEach($scope.BranchesList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.BranchesSelectedList.push(x);
                        $scope.BranchesSelectedDList.push(valueIn.label);
                    }
                });
            });
        }


        $scope.DealerBranchStaff = [];
        $scope.SendBranches = function () {
            $scope.BranchesSelectedDList = [];
            $scope.DealerBranchStaff = [];
            angular.forEach($scope.BranchesSelectedList, function (valueOut) {
                angular.forEach($scope.BranchesList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        var val = {
                            BranchId: valueIn.code,
                            DealerId: $scope.DealerStaff.DealerId,
                            Id: "00000000-0000-0000-0000-000000000000"
                        };
                        $scope.DealerBranchId = valueIn.code;
                        $scope.DealerBranchStaff.push(val);
                        $scope.BranchesSelectedDList.push(valueIn.label);
                    }
                });
            });

            $scope.LoadStaff();
        }

        //--------------------------------DEALER LABOUR CHARGE--------------------------



        function clearDealerLabourChargeControls() {

            $scope.DealerLabourCharge.Id = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.DealerId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.CountryId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.MakeId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.ModelId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.CurrencyId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.CurrencyPeriodId = "00000000-0000-0000-0000-000000000000";
            $scope.DealerLabourCharge.StartDate = "";
            $scope.DealerLabourCharge.EndDate = "";
            $scope.DealerLabourCharge.LabourChargeValue = "";
            $scope.ModelList = [];
            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];
        }

        $scope.resetAll = function () {
            clearDealerLabourChargeControls();
        }

        $scope.validateDealerLabourCharge = function () {
            var isValid = true;
            if ($scope.DealerLabourCharge.DealerId == "" || $scope.DealerLabourCharge.DealerId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLabourChargeDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeDealerId = "";
            }
            if ($scope.DealerLabourCharge.CountryId == "" || $scope.DealerLabourCharge.CountryId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLabourChargeCountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeCountryId = "";
            }
            if ($scope.DealerLabourCharge.MakeId == "" || $scope.DealerLabourCharge.MakeId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLabourChargeMakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeMakeId = "";
            }
            if ($scope.DealerLabourCharge.ModelId == "" || $scope.DealerLabourCharge.ModelId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLabourChargeModelId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeModelId = "";
            }
            if ($scope.DealerLabourCharge.CurrencyId == "" || $scope.DealerLabourCharge.CurrencyId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLabourChargeCurrencyId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeCurrencyId = "";
            }
            if ($scope.DealerLabourCharge.StartDate == "") {
                $scope.validate_DealerLabourChargeStartDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeStartDate = "";
            }
            if ($scope.DealerLabourCharge.EndDate == "") {
                $scope.validate_DealerLabourChargeEndDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeEndDate = "";
            }
            if ($scope.DealerLabourCharge.LabourChargeValue == "") {
                $scope.validate_DealerLabourChargeLabourChargeValue = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLabourChargeLabourChargeValue = "";
            }

            if (isValid) {
                if ($scope.DealerLabourCharge.StartDate > $scope.DealerLabourCharge.EndDate) {
                    $scope.validate_DealerLabourChargeEndDate = "has-error";
                    $scope.validate_DealerLabourChargeStartDate = "has-error";
                    isValid = false;
                    customErrorMessage("Start date cannot be grater than End date.");
                } else {
                    $scope.validate_DealerLabourChargeEndDate = "";
                    $scope.validate_DealerLabourChargeStartDate = "";
                }
            }
            return isValid
        }


        $scope.dealerLabourChargeSubmit = function () {
            if ($scope.validateDealerLabourCharge()) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/AddDealerLabourCharge',
                    data: $scope.DealerLabourCharge,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    if (data == "ok") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerLabourChargeInformation'),
                            text: $filter('translate')('common.sucessMessages.successfullySaved'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        clearDealerLabourChargeControls();
                        getDealerLabourChargeSearchPage();

                    } else {
                        swal($filter('translate')('common.alertTitle'), data, "error");
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.dealerManagement.sucessMessages.dealerLabourChargeInformation'),
                        text: $filter('translate')('common.errMessage.errorOccured'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    clearDealerLabourChargeControls();
                    return false;
                });
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'));
            }

        }

        $scope.getDealerCurrency = function () {

            if ($scope.DealerLabourCharge.DealerId != "" || $scope.DealerLabourCharge.DealerId != "00000000-0000-0000-0000-000000000000") {

                angular.forEach($scope.Dealers, function (value) {
                    if ($scope.DealerLabourCharge.DealerId == value.Id) {

                        $scope.DealerLabourCharge.CurrencyId = value.CurrencyId;
                        $scope.DealerLabourCharge.CountryId = value.CountryId;
                    }

                });
                $scope.refresSearchGridData();
                $scope.LoadMakesByDealerId();
            }
        }
        $scope.ModelList = [];
        $scope.SelectedModelList = [];
        $scope.SelectedModelDList = [];

        $scope.SetModel = function () {

            if ($scope.DealerLabourCharge.MakeId != null) {

                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.DealerLabourCharge.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Modeles = data;
                            AddModel();
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }

        //------------------Grid Dealer Labuor Charge



        $scope.gridDealerLabourCharge = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //useExternalPagination: true,
            enablePaginationControls: true,
            //useExternalSorting: true,
            //enableColumnMenus: false,
            enableRowSelection: true,
            enableCellSelection: false,
            enableSorting: false,
           // enableColumnResizing: true,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false },
                { name: $filter('translate')('pages.dealerManagement.tabDealerManagement.dealer'), field: 'Dealers', enableSorting: false, cellClass: 'columCss'},
                { name: $filter('translate')('pages.dealerManagement.tabDealerManagement.country'), field: 'Country', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.dealerManagement.tabDealerManagement.make'), field: 'Makes', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerManagement.tabDealerLabour.model'), field: 'Models', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerManagement.tabDealerLabour.startDate'), field: 'StartDate', enableSorting: false, cellClass: 'columCss', minWidth: 90 },
                { name: $filter('translate')('pages.dealerManagement.tabDealerLabour.endDate'), field: 'EndDate', enableSorting: false, cellClass: 'columCss', minWidth: 90 },
                { name: $filter('translate')('pages.dealerManagement.tabDealerLabour.currency'), field: 'Currencys', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.dealerManagement.tabDealerLabour.value'), field: 'LabourChargeValue', enableSorting: false, cellClass: 'columCss' },

              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadDealerLabourChargeForUpdate(row.entity.Id)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.update') +'</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.handleWindowResize();
                //$scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                //    if (sortColumns.length == 0) {
                //        paginationOptionsDealerLabourChargeSearchGrid.sort = null;
                //    } else {
                //        paginationOptionsDealerLabourChargeSearchGrid.sort = sortColumns[0].sort.direction;
                //    }
                //    getDealerLabourChargeSearchPage();
                //});
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsDealerLabourChargeSearchGrid.pageNumber = newPage;
                    paginationOptionsDealerLabourChargeSearchGrid.pageSize = pageSize;
                    getDealerLabourChargeSearchPage();
                });
            }
        };
        $scope.refresSearchGridData = function () {
            getDealerLabourChargeSearchPage();
        }

        function getDealerLabourChargeSearchPage() {

            $scope.gridDealerLabourChargeloading = true;
            $scope.gridDealerLabourChargeloadAttempted = false;
                var policySearchGridParam =
                {
                    'paginationOptionsDealerLabourChargeSearchGrid': paginationOptionsDealerLabourChargeSearchGrid,
                    'dealerLabourChargeSearchGridSearchCriterias': $scope.DealerLabourCharge
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/SearchDealerLabourChargeSchemes',
                    data: JSON.stringify(policySearchGridParam),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var responseArr = JSON.parse(data);
                    if (responseArr != null) {
                        $scope.gridDealerLabourCharge.data = responseArr.data;
                        $scope.gridDealerLabourCharge.totalItems = responseArr.totalRecords;
                    } else {
                        // $scope.gridDealerDiscounts.data = [];
                        // $scope.gridDealerDiscounts.totalItems = 0;
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    $scope.gridDealerLabourChargeloading = false;
                    $scope.gridDealerLabourChargeloadAttempted = true;

                });

        };

        $scope.loadDealerLabourChargeForUpdate = function (dealerLabourChargeId) {
            if (isGuid(dealerLabourChargeId)) {
                swal({ title: $filter('translate')('common.loading') , text: $filter('translate')('pages.dealerManagement.sucessMessages.dealerLabourChargeInformation'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetDealerLabourChargeById',
                    data: { 'dealerLabourChargeId': dealerLabourChargeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data !== null && typeof data !== "undefined") {
                        $scope.DealerLabourCharge.Id= data.DealerLabourChargeId;
                        $scope.DealerLabourCharge.CurrencyId = data.CurrencyId;
                        $scope.DealerLabourCharge.LabourChargeValue = data.LabourChargeValue;
                        $scope.DealerLabourCharge.DealerId = data.DealerId;
                        $scope.DealerLabourCharge.ModelId = data.ModelId;
                        $scope.DealerLabourCharge.StartDate = data.StartDate;
                        $scope.DealerLabourCharge.EndDate = data.EndDate;
                        $scope.DealerLabourCharge.CountryId = data.CountryId;
                        $scope.LoadMakesByDealerId();
                        $scope.DealerLabourCharge.MakeId = data.MakeId;

                        if ($scope.DealerLabourCharge.MakeId != null) {
                            angular.forEach($scope.Makes, function (value, key) {
                                if (value.Id == $scope.DealerLabourCharge.MakeId) {
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                                        data: { "Id": value.Id },
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.Modeles = data;
                                        AddModel();
                                        LoadModel();
                                    }).error(function (data, status, headers, config) {
                                    });
                                }
                            });
                        }

                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }

        $scope.LoadMakesByDealerId = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllMakesByDealerId',
                data: { "dealerId": $scope.DealerLabourCharge.DealerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerMakes = data;

            }).error(function (data, status, headers, config) {
            });
        }

    });



