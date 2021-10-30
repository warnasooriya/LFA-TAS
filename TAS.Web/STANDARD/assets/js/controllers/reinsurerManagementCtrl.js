app.controller('ReinsurerManagementCtrl', function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
    $scope.ModalName = "Reinsurer Management";
    $scope.ModalDescription = "Add Edit Reinsurer Information";

    $scope.labelSaveReinsurer = 'common.button.save';
    $scope.labelSaveReinsurerContract = 'common.button.save';
    $scope.labelSaveReinsurerConsortium = 'common.button.save';

    $scope.saveStaffMappingsBtnIconClass = "";
    $scope.saveStaffMappingsBtnDisabled = false;

    $scope.ConsortiumSubmitBtnIconClass = "";
    $scope.ConsortiumSubmitBtnDisabled = false;

    $scope.ReinsurerContractSubmitBtnIconClass = "";
    $scope.ReinsurerContractSubmitBtnDisabled = false;

    $scope.ReinsurerSubmitBtnIconClass = "";
    $scope.ReinsurerSubmitBtnDisabled = false;

    $scope.loadInitailData = function () { };
    $scope.selectedpp = {};
    $scope.errorTab1 = "";
    $scope.errorTab2 = "";
    $scope.errorTab3 = "";
    $scope.reinsurerStaff = {
        reinsurerId: emptyGuid(),
    }
    $scope.RiskTotal = 0;
    $scope.PercentageTotal = 0;
    //$scope.NRPTotal = 0;
    $scope.drpsettings = {
        scrollableHeight: '250px',
        width: '300px',
        scrollable: true,
        enableSearch: true
    };
    $scope.C = {
        ParentReinsurerId: "00000000-0000-0000-0000-000000000000",
        ReinsurerId: "00000000-0000-0000-0000-000000000000"
    }
    function emptyGuid() {
        return "00000000-0000-0000-0000-000000000000";
    }
    function isGuid(stringToTest) {
        var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
    }

    var customErrorMessage = function (msg) {
        toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
    };

    var customWarningMessage = function (msg) {
        toaster.pop('warning', $filter('translate')('common.popUpMessages.warning'), msg, 12000);
    };

    $scope.UserList = [];
    $scope.SelectedUserList = [];
    $scope.reinsurerStaffs = [];
    $scope.ReinsurerContract = {
        Id: "00000000-0000-0000-0000-000000000000",
        ReinsurerId: "",
        UWYear: '',
        CountryId: '',
        CommodityTypeId: '',
        InsurerId: '',
        FromDate: '',
        ToDate: '',
        ContractNo: '',
        IsActive: '',
        BrokerId: emptyGuid()
    };
    $scope.Reinsurer = {
        Id: "00000000-0000-0000-0000-000000000000",
        ReinsurerName: "",
        ReinsurerCode: '',
        IsActive: false,
        CurrencyId:"00000000-0000-0000-0000-000000000000"
    };

    function LoadDetails() {
        $http({
            method: 'POST',
            url: '/TAS.Web/api/ReinsurerManagement/GetAllCommodities',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.CommodityTypes = data;
        }).error(function (data, status, headers, config) {
        });
        $http({
            method: 'POST',
            url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.Reinsurers = data;
        }).error(function (data, status, headers, config) {
        });
        $http({
            method: 'POST',
            url: '/TAS.Web/api/Customer/GetAllCountries',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.Countries = data;
        }).error(function (data, status, headers, config) {
        });
        $http({
            method: 'POST',
            url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContracts',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.ReinsurerContracts = data;
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
            url: '/TAS.Web/api/User/GetUsers',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
            $scope.Users = data;
            AddUser();
        }).error(function (data, status, headers, config) {
        });//Users
    }

    LoadDetails();

    $scope.LoadConsortiums = function () {
        $scope.Consortium = [];
        $http({
            method: 'POST',
            url: '/TAS.Web/api/ReinsurerManagement/GetReinsurerConsortiumsByReinsurerId',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
            data: { "Id": $scope.C.ParentReinsurerId }
        }).success(function (data, status, headers, config) {
            $scope.ReinsurerConsortiums = data;
            $scope.Consortium = [];
            $scope.ConsortiumTemp = [];
            angular.forEach($scope.Reinsurers, function (valueR) {
                if (valueR.Id != $scope.C.ParentReinsurerId) {
                    $scope.ConsortiumTemp.push(valueR);
                }
            });
            angular.forEach($scope.ConsortiumTemp, function (value) {
                var exists = false;
                angular.forEach($scope.ReinsurerConsortiums, function (valueC) {
                    if (value.Id == valueC.ReinsurerId)
                        exists = true;
                });
                if (!exists) {
                    $scope.Consortium.push(value);
                }
            });
            $scope.TotalCalc();
            angular.forEach($scope.ReinsurerConsortiums, function (value) {
                angular.forEach($scope.Reinsurers, function (valueR) {
                    if (valueR.Id == value.ReinsurerId)
                        value.Reinsurer = valueR.ReinsurerName;
                });
            });
        }).error(function (data, status, headers, config) {
        });
    }
    $scope.TotalCalc = function () {
        $scope.RiskTotal = 0;
           $scope.PercentageTotal = 0;
        //   $scope.NRPTotal = 0;
        angular.forEach($scope.ReinsurerConsortiums, function (value) {
            $scope.RiskTotal = $scope.RiskTotal + value.RiskSharePercentage;
            $scope.PercentageTotal = $scope.PercentageTotal + value.ProfitSharePercentage;
            //   $scope.NRPTotal = $scope.NRPTotal + value.NRPPercentage;
        });
    }

    $scope.validateConsortium = function () {
        var isValid = true;

        if ($scope.C.ParentReinsurerId == "00000000-0000-0000-0000-000000000000" || $scope.C.ParentReinsurerId == undefined || $scope.C.ParentReinsurerId == null) {
            $scope.validate_RParentReinsurerId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RParentReinsurerId = "";
        }

        if ($scope.C.ReinsurerId == "00000000-0000-0000-0000-000000000000" || $scope.C.ReinsurerId == undefined || $scope.C.ReinsurerId == null) {
            $scope.validate_RCReinsurerId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RCReinsurerId = "";
        }
        return isValid
    }

    $scope.AddReinsurer = function () {
        if ($scope.validateConsortium()) {
            var item = {
                Id: $scope.C.ReinsurerId,
                Id2: "10000000-0000-0000-0000-000000000001" ,
                ParentReinsurerId: $scope.C.ParentReinsurerId,
                ReinsurerId: $scope.C.ReinsurerId,
                RiskSharePercentage: 0,
                ProfitSharePercentage: 0
            }
            // check already exist

            var exist = $scope.ReinsurerConsortiums.filter(a => a.Id == item.Id);
            if (exist.length == 0) {
                var reIns = $scope.Reinsurers.filter(a => a.Id == $scope.C.ReinsurerId);
                if (reIns.length > 0) {
                    item.Reinsurer = reIns[0].ReinsurerName;
                    $scope.ReinsurerConsortiums.push(item);
                    $scope.Consortium = $scope.Consortium.filter(a => a.Id != item.Id);

                }

            } else {
                customWarningMessage($filter('translate')('pages.reInsurerManagement.errorMessages.alreadyAddReinsurer'))
            }

            //angular.forEach($scope.Reinsurers, function (valueR) {
            //    if (valueR.Id == $scope.C.ReinsurerId)
            //        item.Reinsurer = valueR.ReinsurerName;
            //});

            //$scope.ReinsurerConsortiums.push(item);
            //$scope.Consortium = [];
            //$scope.ConsortiumTemp = [];
            //$scope.C.ReinsurerId = "00000000-0000-0000-0000-000000000000";
            //angular.forEach($scope.Reinsurers, function (valueR) {
            //    if (valueR.Id != $scope.C.ParentReinsurerId) {
            //        $scope.ConsortiumTemp.push(valueR);
            //    }
            //});
            //angular.forEach($scope.ConsortiumTemp, function (value) {
            //    var exists = false;
            //    angular.forEach($scope.ReinsurerConsortiums, function (valueC) {
            //        if (value.Id == valueC.ReinsurerId)
            //            exists = true;
            //    });
            //    if (!exists) {
            //        $scope.Consortium.push(value);
            //    }
            //});
        } else {            
            customErrorMessage($filter('translate')('pages.reInsurerManagement.errorMessages.pleaseSelectReinsurer'))
        }
    }
    function TotalValidation() {
        if ($scope.RiskTotal != 100 || $scope.PercentageTotal != 100) { //|| $scope.NRPTotal != 100) {
            {
                //SweetAlert.swal({
                //    title: "Reinsurer Consortium Information",
                //    text: "Risk and Percentage Total Should be 100%.",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customWarningMessage($filter('translate')('pages.reInsurerManagement.errorMessages.riskPercentageTotal'))
                return false;
            }
        }
        return true;
    }
    function ClearReinsurerConsortium() {
        $scope.RiskTotal = 0;
        $scope.PercentageTotal = 0;
        $scope.C = {
            ParentReinsurerId: "00000000-0000-0000-0000-000000000000",
            ReinsurerId: "00000000-0000-0000-0000-000000000000"
        }
        $scope.ReinsurerConsortiums = [];
    }
    $scope.ReinsurerConsortiums = [];
    $scope.Removed = [];
    $scope.Remove = function (val) {
        var sc = $scope.Consortium;
        val.ReinsurerName = val.Reinsurer;
        val.Id = val.ReinsurerId;
        val.IsReinsurerConsortiumExists = false;
        val.NRPPercentage = 0;
        val.ProfitSharePercentage = 0;
        val.RiskSharePercentage = 0;
        sc.push(val);
        $scope.Consortium = sc;
        $scope.ReinsurerConsortiums = $scope.ReinsurerConsortiums.filter(a => a.Id != val.Id);
        $scope.TotalCalc();
    }
    $scope.ConsortiumSubmit = function () {
        if ($scope.ReinsurerConsortiums.length > 0 ) {
            if (TotalValidation()) {
                //angular.forEach($scope.Removed, function (value) {
                //    value.RiskSharePercentage = 0;
                //    $scope.ReinsurerConsortiums.push(value);
                //});

                var Data =
                {
                    Consortiums: $scope.ReinsurerConsortiums
                };
                $scope.errorTab3 = "";
                $scope.ConsortiumSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ConsortiumSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerManagement/AddorUpdateReinsurerConsortiumsNew',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: Data
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.ConsortiumSubmitBtnIconClass = "";
                    $scope.ConsortiumSubmitBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.reInsurerManagement.tabReinsurerConsortium.reinsurerConsortiumInformation'),
                            text: $filter('translate')('common.sucessMessages.successfullySaved'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        ClearReinsurerConsortium();
                    }
                    else {
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.reInsurerManagement.tabReinsurerConsortium.reinsurerConsortiumInformation'),
                        text: $filter('translate')('common.errMessage.errorOccured'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ConsortiumSubmitBtnIconClass = "";
                    $scope.ConsortiumSubmitBtnDisabled = false;
                    return false;
                });
            }
        }
        else {
            customErrorMessage($filter('translate')('pages.reInsurerManagement.errorMessages.enterConsortium'))
           // $scope.errorTab3 = "Please Enter Consortium";
        }
    }

    $scope.SetReinsurerValues = function () {
        $scope.errorTab1 = "";
        if ($scope.Reinsurer.Id != null) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetReinsurerById',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.Reinsurer.Id }
            }).success(function (data, status, headers, config) {
                $scope.Reinsurer.Id = data.Id;
                $scope.Reinsurer.ReinsurerCode = data.ReinsurerCode;
                $scope.Reinsurer.ReinsurerName = data.ReinsurerName;
                $scope.Reinsurer.CurrencyId = data.CurrencyId;
                $scope.Reinsurer.IsActive = data.IsActive;
                $scope.labelSaveReinsurer = 'common.button.update';
            }).error(function (data, status, headers, config) {
                clearReinsurerControls();
            });
        }
        else {
            clearReinsurerControls();
        }
    }
    function clearReinsurerControls() {
        $scope.Reinsurer.Id = "00000000-0000-0000-0000-000000000000";
        $scope.Reinsurer.ReinsurerCode = "";
        $scope.Reinsurer.ReinsurerName = "";
        $scope.Reinsurer.IsActive = false;
        $scope.Reinsurer.CurrencyId = "00000000-0000-0000-0000-000000000000";
        $scope.labelSaveReinsurer = 'common.button.save';
    }

    $scope.validateReinsureDetails = function () {
        var isValid = true;

        if ($scope.Reinsurer.ReinsurerName == "" || $scope.Reinsurer.ReinsurerName == undefined) {
            $scope.validate_ReinsurerName = "has-error";
            isValid = false;
        }
        else {
            $scope.validate_ReinsurerName = "";
        }
        if ($scope.Reinsurer.ReinsurerCode == "" || $scope.Reinsurer.ReinsurerCode == undefined) {
            $scope.validate_ReinsurerCode = "has-error";
            isValid = false;
        } else {
            $scope.validate_ReinsurerCode = "";
        }
        if ($scope.Reinsurer.CurrencyId == "00000000-0000-0000-0000-000000000000" || $scope.Reinsurer.CurrencyId == null
            || $scope.Reinsurer.CurrencyId == undefined) {
            $scope.validate_CurrencyId = "has-error";
            isValid = false;
        } else {
            $scope.validate_CurrencyId = "";
        }

        return isValid

    }

    $scope.ReinsurerSubmit = function () {
        if ($scope.validateReinsureDetails()) {
            $scope.errorTab1 = "";
            if ($scope.Reinsurer.Id == null || $scope.Reinsurer.Id == "00000000-0000-0000-0000-000000000000") {
                $scope.ReinsurerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ReinsurerSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerManagement/AddReinsurer',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: $scope.Reinsurer
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.ReinsurerSubmitBtnIconClass = "";
                    $scope.ReinsurerSubmitBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.reInsurerManagement.tabReInsurerDetails.reinsurerInformation'),
                            text: $filter('translate')('common.sucessMessages.successfullySaved'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Reinsurers = data;
                        }).error(function (data, status, headers, config) {
                        });
                        clearReinsurerControls();
                    }
                    else {
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.reInsurerManagement.tabReInsurerDetails.reinsurerInformation'),
                        text: $filter('translate')('common.errMessage.errorOccured'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ReinsurerSubmitBtnIconClass = "";
                    $scope.ReinsurerSubmitBtnDisabled = false;
                    return false;
                });

            }
            else {
                $scope.ReinsurerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ReinsurerSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerManagement/UpdateReinsurer',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: $scope.Reinsurer
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.ReinsurerSubmitBtnIconClass = "";
                    $scope.ReinsurerSubmitBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.reInsurerManagement.tabReInsurerDetails.reinsurerInformation'),
                            text: $filter('translate')('common.sucessMessages.successfullySaved'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "#007AFF"
                        });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Reinsurers = data;
                        }).error(function (data, status, headers, config) {
                        });

                        clearReinsurerControls();

                    } else {;
                    }

                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.reInsurerManagement.tabReInsurerDetails.reinsurerInformation'),
                        text: $filter('translate')('common.errMessage.errorOccured'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ReinsurerSubmitBtnIconClass = "";
                    $scope.ReinsurerSubmitBtnDisabled = false;
                    return false;
                });
            }
        } else {
            customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
        }
    }

    $scope.SetInsurer = function () {
        $scope.errorTab1 = "";
        if ($scope.ReinsurerContract.CountryId != null) {
            $scope.InsurerDisable = false;
            angular.forEach($scope.Countries, function (value, key) {
                if (value.Id == $scope.ReinsurerContract.CountryId) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/InsurerManagement/GetAllInsurersByCountryId',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { "Id": value.Id }
                    }).success(function (data, status, headers, config) {
                        $scope.Insurers = data;
                    }).error(function (data, status, headers, config) {
                    });
                }
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Broker/GetAllBrokersByCountry',
                data: { "CountryId": $scope.ReinsurerContract.CountryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.allbrokers = data;
                //$scope.ListOfBrokers = data;
                //$scope.saveOrUpdateDisabled = false;
            }).error(function (data, status, headers, config) {
            });
        }
    }
    $scope.SetReinsurerContractsValues = function () {
        $scope.errorTab1 = "";
        if ($scope.ReinsurerContract.ReinsurerId != null) {
            angular.forEach($scope.Reinsurers, function (value, key) {
                if (value.Id == $scope.ReinsurerContract.ReinsurerId) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContractsByReinsurerId',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { "Id": value.Id }
                    }).success(function (data, status, headers, config) {
                        $scope.ReinsurerContracts = data;
                    }).error(function (data, status, headers, config) {
                    });
                }
            });
        }
    }
    $scope.SetReinsurerContractValues = function () {
        $scope.errorTab2 = "";
        if ($scope.ReinsurerContract.Id != null) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetReinsurerContrctById',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.ReinsurerContract.Id }
            }).success(function (data, status, headers, config) {
                $scope.ReinsurerContract.Id = data.Id;
                $scope.ReinsurerContract.LinkContractId = data.LinkContractId;
                $scope.ReinsurerContract.ReinsurerId = data.ReinsurerId;
                $scope.ReinsurerContract.UWYear = data.UWYear;
                $scope.ReinsurerContract.CountryId = data.CountryId;
                $scope.ReinsurerContract.InsurerId = data.InsurerId;
                $scope.ReinsurerContract.CommodityTypeId = data.CommodityTypeId;
                $scope.ReinsurerContract.FromDate = data.FromDate;
                $scope.ReinsurerContract.ToDate = data.ToDate;
                $scope.ReinsurerContract.ContractNo = data.ContractNo;
                $scope.ReinsurerContract.IsActive = data.IsActive;
                $scope.ReinsurerContract.BrokerId = data.BrokerId;
                $scope.labelSaveReinsurerContract = 'common.button.update';
                $scope.SetInsurer();
            }).error(function (data, status, headers, config) {
                clearReinsurerContractControls();
            });
        }
        else {
            clearReinsurerContractControls();
        }
    }
    $scope.SetReinsurerLinkContractValues = function () {
        $scope.errorTab1 = "";
        if ($scope.ReinsurerContract.Id != null) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetReinsurerContrctById',
                data: { "Id": $scope.ReinsurerContract.LinkContractId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ReinsurerContract.Id = "00000000-0000-0000-0000-000000000000";
                $scope.ReinsurerContract.ReinsurerId = data.ReinsurerId;
                $scope.ReinsurerContract.UWYear = data.UWYear;
                $scope.ReinsurerContract.CountryId = data.CountryId;
                $scope.ReinsurerContract.InsurerId = data.InsurerId;
                $scope.ReinsurerContract.CommodityTypeId = data.CommodityTypeId;
                $scope.ReinsurerContract.FromDate = data.FromDate;
                $scope.ReinsurerContract.ToDate = data.ToDate;
                $scope.ReinsurerContract.ContractNo = data.ContractNo;
                $scope.ReinsurerContract.IsActive = data.IsActive;
                $scope.ReinsurerContract.BrokerId = data.BrokerId;
                $scope.SetInsurer();
            }).error(function (data, status, headers, config) {
                clearReinsurerContractControls();
            });
        }
        else {
            clearReinsurerContractControls();
        }
    }
    function clearReinsurerContractControls() {
        $scope.ReinsurerContract.Id = "00000000-0000-0000-0000-000000000000";
        $scope.ReinsurerContract.LinkContractId = "00000000-0000-0000-0000-000000000000";
        $scope.ReinsurerContract.ReinsurerId = "";
        $scope.ReinsurerContract.UWYear = "";
        $scope.ReinsurerContract.CountryId = "";
        $scope.ReinsurerContract.InsurerId = "";
        $scope.ReinsurerContract.CommodityTypeId = "";
        $scope.ReinsurerContract.FromDate = "";
        $scope.ReinsurerContract.ToDate = "";
        $scope.ReinsurerContract.ContractNo = "";
        $scope.ReinsurerContract.IsActive = "";
        $scope.ReinsurerContract.BrokerId = emptyGuid();
        $scope.labelSaveReinsurerContract = 'common.button.save';
    }

    $scope.validateReinsurerContract = function () {
        var isValid = true;

        if ($scope.ReinsurerContract.ReinsurerId == "" || $scope.ReinsurerContract.ReinsurerId == undefined || $scope.ReinsurerContract.ReinsurerId == null) {
            $scope.validate_RReinsurerId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RReinsurerId = "";
        }
        if ($scope.ReinsurerContract.UWYear == "" || $scope.ReinsurerContract.UWYear == undefined) {
            $scope.validate_UWYear = "has-error";
            isValid = false;
        } else {
            $scope.validate_UWYear = "";
        }
        if ($scope.ReinsurerContract.ContractNo == "" || $scope.ReinsurerContract.ContractNo == undefined) {
            $scope.validate_ContractNo = "has-error";
            isValid = false;
        } else {
            $scope.validate_ContractNo = "";
        }
        if ($scope.ReinsurerContract.CountryId == "" || $scope.ReinsurerContract.CountryId == undefined || $scope.ReinsurerContract.CountryId == null) {
            $scope.validate_RCountryId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RCountryId = "";
        }
        if ($scope.ReinsurerContract.InsurerId == "" || $scope.ReinsurerContract.InsurerId == undefined || $scope.ReinsurerContract.InsurerId == null) {
            $scope.validate_RInsurerId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RInsurerId = "";
        }
        if ($scope.ReinsurerContract.CommodityTypeId == "" || $scope.ReinsurerContract.CommodityTypeId == undefined || $scope.ReinsurerContract.CommodityTypeId == null) {
            $scope.validate_RCommodityTypeId = "has-error";
            isValid = false;
        } else {
            $scope.validate_RCommodityTypeId = "";
        }
        if ($scope.ReinsurerContract.ToDate == "" || $scope.ReinsurerContract.ToDate == undefined || $scope.ReinsurerContract.ToDate == null) {
            $scope.validate_RToDate = "has-error";
            isValid = false;
        }
        else {
            $scope.validate_RToDate = "";
        }
        if ($scope.ReinsurerContract.FromDate == "" || $scope.ReinsurerContract.FromDate == undefined || $scope.ReinsurerContract.FromDate == null) {
            $scope.validate_RFromDate = "has-error";
            isValid = false;
        }
        else {
            $scope.validate_RFromDate = "";
        }
        return isValid
    }

    $scope.ReinsurerContractSubmit = function () {
        if ($scope.validateReinsurerContract()) {
            $scope.errorTab2 = "";
            if ($scope.ReinsurerContract.FromDate < $scope.ReinsurerContract.ToDate) {
                if ($scope.ReinsurerContract.Id == null || $scope.ReinsurerContract.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.ReinsurerContractSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ReinsurerContractSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ReinsurerManagement/AddReinsurerContract',
                        data: $scope.ReinsurerContract,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ReinsurerContractSubmitBtnIconClass = "";
                        $scope.ReinsurerContractSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.reInsurerManagement.tabReInsurerContract.reinsurerContractInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContracts',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.ReinsurerContracts = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearReinsurerContractControls();
                        } else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.reInsurerManagement.tabReInsurerContract.reinsurerContractInformation'),
                                text: $filter('translate')('pages.reInsurerManagement.errorMessages.contractNoExist'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });

                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.reInsurerManagement.tabReInsurerContract.reinsurerContractInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ReinsurerContractSubmitBtnIconClass = "";
                        $scope.ReinsurerContractSubmitBtnDisabled = false;
                        return false;
                    });

                }
                else {
                    $scope.ReinsurerContractSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ReinsurerContractSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ReinsurerManagement/UpdateReinsurerContract',
                        data: $scope.ReinsurerContract,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ReinsurerContractSubmitBtnIconClass = "";
                        $scope.ReinsurerContractSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.reInsurerManagement.tabReInsurerContract.reinsurerContractInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContracts',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.ReinsurerContracts = data;
                            }).error(function (data, status, headers, config) {
                            });

                            clearReinsurerContractControls();

                        } else {;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.reInsurerManagement.tabReInsurerContract.reinsurerContractInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ReinsurerContractSubmitBtnIconClass = "";
                        $scope.ReinsurerContractSubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                SweetAlert.swal({
                    title: $filter('translate')('pages.reInsurerManagement.errorMessages.shouldbegreater'),
                    text: $filter('translate')('common.errMessage.errorOccured'),
                    type: "warning",
                    confirmButtonText: $filter('translate')('common.button.ok'),
                    confirmButtonColor: "rgb(221, 107, 85)"
                });

            }
        } else {
            customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
        }
    }

    $scope.setReinsurerStaffValues = function () {
        $scope.SelectedUserList = [];
        $scope.SelectedUserIDList = [];
        if (isGuid($scope.reinsurerStaff.reinsurerId)) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetAllStaffByReinsurerId',
                data: { "Id": $scope.reinsurerStaff.reinsurerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                for (var i = 0; i < data.length; i++) {
                    angular.forEach($scope.UserList, function (valueIn) {
                        if (data[i] == valueIn.code) {
                            var user = {
                                id: valueIn.id
                            };
                            $scope.SelectedUserList.push(user);
                        }
                    });
                }
                $scope.SendUser();
            }).error(function (data, status, headers, config) {
            });
        } else {

        }
    }

    $scope.SendUser = function () {
        $scope.SelectedUserIDList = [];
        $scope.Staffs = [];
        angular.forEach($scope.SelectedUserList, function (valueOut) {
            //alert(valueOut.UserId);
            angular.forEach($scope.UserList, function (valueIn) {
                //alert(valueIn.code);
                if (valueOut.id == valueIn.id) {
                    if ($scope.reinsurerStaffs.length == 0) {
                        var val = {
                            UserId: valueIn.code,
                            ReinusrerId: $scope.reinsurerStaff.reinsurerId,
                            Id: "00000000-0000-0000-0000-000000000000"
                        };
                        $scope.Staffs.push(val);
                        $scope.SelectedUserIDList.push(valueIn.label);
                    }
                    else {
                        angular.forEach($scope.reinsurerStaffs, function (val) {
                            if (valueIn.code == val.UserId) {
                                var val = {
                                    UserId: valueIn.code,
                                    DealerId: $scope.DealerStaff.DealerId,
                                    Id: val.Id
                                };
                                $scope.Staffs.push(val);
                                $scope.SelectedUserIDList.push(valueIn.label);
                            }
                        });
                    }
                }
            });
        });
    }

    function AddUser() {
        var index = 0;
        $scope.UserList = [];
        angular.forEach($scope.Users, function (value) {
            var x = { id: index, code: value.Id, label: value.FirstName + ' ' + value.LastName };
            $scope.UserList.push(x);
            index = index + 1;
        });
    }
    $scope.validate_RSTInsurerId = "";
    $scope.saveStaffMappings = function () {
        var request = {
            ReinsurerStaff: $scope.Staffs,
            ReinusrerId: $scope.reinsurerStaff.reinsurerId
        }
        if (isGuid($scope.reinsurerStaff.reinsurerId)) {
            $scope.validate_RSTInsurerId = "";
            $scope.saveStaffMappingsBtnIconClass = "fa fa-spinner fa-spin";
            $scope.saveStaffMappingsBtnDisabled = true;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/SaveReinsurerStaff',
                data: { "data": request },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.saveStaffMappingsBtnIconClass = "";
                $scope.saveStaffMappingsBtnDisabled = false;
                if (data) {
                    SweetAlert.swal({
                        title: $filter('translate')('common.alertTitle'),
                        text: $filter('translate')('common.sucessMessages.successfullySaved'),
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "#007AFF"
                    });
                    $scope.clearReinsurerStaffMapping();

                } else {
                    SweetAlert.swal({
                        title: $filter('translate')('common.alertTitle'),
                        text: $filter('translate')('pages.reInsurerManagement.errorMessages.reinsureStaff'),
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.saveStaffMappingsBtnIconClass = "";
                    $scope.saveStaffMappingsBtnDisabled = false;
                }
            }).error(function (data, status, headers, config) {
            });
        } else {
            customErrorMessage($filter('translate')('pages.reInsurerManagement.errorMessages.enterReinsurer'))
            $scope.validate_RSTInsurerId = "has-error";
        }
    }



    $scope.clearReinsurerStaffMapping = function () {
        $scope.SelectedUserList = [];
        $scope.SelectedUserIDList = [];
        $scope.Staffs = [];
        $scope.reinsurerStaff.reinsurerId = emptyGuid();
    }

    //-------------------- date validation ------------------------

    //$scope.checkErr = function ( FromDate, ToDate) {
    //    $scope.errMessage = '';
    //    var curDate = new Date();

    //    if (new Date(FromDate) > new Date(ToDate)) {
    //        $scope.errMessage = 'End Date should be greater than start date';
    //        return false;
    //    }
    //    //if (new Date(fromDate) < curDate) {
    //    //    $scope.errMessage = 'Start date should not be before today.';
    //    //    return false;
    //    //}
    //};

});



