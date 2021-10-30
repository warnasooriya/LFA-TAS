app.controller('PremiumSetupManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, $localStorage, $timeout, $interval, $state, toaster) {
        $scope.contractSearchGridloadAttempted = false;
        $scope.contractSearchGridloading = false;
        $scope.saveBtnDisabled = false;
        $scope.saveBtnClass = "";
        $scope.formAction = true;
        $scope.IsAutomobile = false;
        $scope.IsYellowGood = false;
        $scope.IsContractDatesAutomaticallySet = false;

        $scope.IsOther = false;
        $scope.IsMinMaxVisibleGross = false;
        $scope.IsMinMaxVisibleNett = false;
        $scope.premiumDetailsHideGross = true;
        $scope.premiumDetailsHideNett = true;
        $scope.formActionText = "Setting up new contract";
        $scope.setNewInsuaranceLimitationMsg = "";
        $scope.SelectedContractError = "";
        $scope.eligibilityError = "";
        $scope.totalNRP = 0.00;
        $scope.grossPremium = 0.00;

        $scope.contractIdToUpdate = emptyGuid();
        $scope.contractSearchGridSearchCriterias = {
            countryId: emptyGuid(),
            dealerId: emptyGuid(),
            productId: emptyGuid(),
            dealName: ""
        };
        $scope.currentContract = {
            countryId: emptyGuid(),
            dealerId: emptyGuid(),
            commodityTypeId: emptyGuid(),
            productId: emptyGuid(),
            linkDealId: emptyGuid(),
            dealName: "",
            dealType: emptyGuid(),
            isPromotional: false,
            discountAvailable: false,
            insurerId: emptyGuid(),
            reinsurerContractId: emptyGuid(),
            contractStartDate: "",
            contractEndDate: "",
            remark: "",
            isAutoRenewal: false,
            isActive: false,
            itemStatusId: emptyGuid(),
            commodityUsageTypeId: emptyGuid(),
            claimLimitation: 0.00,
            liabilityLimitation: 0.00,
            InsuaranceLimitationId: emptyGuid(),
            attributeSpecificationId: emptyGuid(),
            attributeSpecificationPrefix: "",
            attributeSpecificationName: "",
            warrantyTypeId: emptyGuid(),
            premiumBasedOnIdGross: emptyGuid(),
            premiumBasedOnIdNett: emptyGuid(),
            isCustAvailableGross: true,
            isCustAvailableNett: true,
            manufacturerWarrantyGross: false,
            manufacturerWarrantyNett: false,
            rsaProviderId: emptyGuid(),
            regionId: emptyGuid(),
            commodityCategoryId: emptyGuid(),
            minValueGross: '',
            maxValueGross: '',
            minValueNett: '',
            maxValueNett: '',
            annualPremiumTotal: 0.00,
            premiumCurrencyId: emptyGuid(),
            eligibilities: [],
            texes: [],
            commissions: [],
            annualInterestRate: 0.00
        }
        $scope.countries = [];
        $scope.Allcountries = [];
        $scope.dealers = [];
        $scope.dealersByCountry = [];
        $scope.products = [];
        $scope.commodityProducts = [];
        $scope.deals = [];
        $scope.commodityTypes = [];
        $scope.linkDeals = [];
        $scope.dealTypes = [];
        $scope.insurers = [];
        $scope.reinsurerContracts = [];
        $scope.allReinsurerContracts = [];
        $scope.itemStatuses = [];
        $scope.commodityUsageTypes = [];
        $scope.insuaranceLimitations = [];
        $scope.rsaProviders = [];
        $scope.currentProductType = "";
        $scope.newInsuaranceLimitation = {
            km: "",
            month: "",
            hours: "",
            unlimitedcheck: false
        }
        $scope.attributeSpecifications = [];
        $scope.warrantyTypes = [];
        $scope.selectedMakeList = [];
        $scope.availableMakes = [];
        $scope.availableMakesDrp = [];//for drop down
        $scope.selectedModelsList = [];
        $scope.availableModels = [];
        $scope.availableModelsDrp = [];//for drop down
        $scope.clinderCounts = [];
        $scope.clinderCountsDrp = [];//for drop down
        $scope.selectedClinderCounts = [];
        $scope.engineCapacitiesDrp = [];//for drop down
        $scope.engineCapacities = [];
        $scope.availableVariantsDrp = [];//for drop down
        $scope.selectedVariantList = [];
        $scope.availableVariants = [];

        $scope.selectedEngineCapacities = [];
        $scope.premiumCurrency = "";
        $scope.premiumCurrencyId = emptyGuid();
        $scope.premiumBasedOnValues = [];
        $scope.premiumAddons = [];
        $scope.regions = [];
        $scope.annualPremium = [];
        $scope.commissionTypes = [];
        $scope.countryTaxes = [];
        $scope.commodityCategories = [];
        $scope.premiumAddonsGross = {};
        $scope.premiumAddonsNett = {};


        $scope.grossVehicleWeights = [];
        $scope.grossWeightDrp = [];
        $scope.selectedGrossWeights = [];

        //update
        $scope.temp_premiumAddons = [];
        $scope.eligibility = {
            ageMin: 0.00,
            ageMax: 0.00,
            milageMin: 0.00,
            milageMax: 0.00,
            monthsMin: 0.00,
            monthsMax: 0.00,
            isPresentage: false,
            plusMinus: 0.00,
            premium: 0.00
        }
        $scope.temp_taxdetails = [];
        $scope.temp_dealName = "";
        $scope.currentContract = {
            countryId: emptyGuid(),
            dealerId: emptyGuid(),
            commodityTypeId: emptyGuid(),
            productId: emptyGuid(),
            linkDealId: emptyGuid(),
            dealName: "",
            dealType: emptyGuid(),
            isPromotional: false,
            discountAvailable: false,
            insurerId: emptyGuid(),
            reinsurerContractId: emptyGuid(),
            contractStartDate: "",
            contractEndDate: "",
            remark: "",
            isAutoRenewal: false,
            isActive: false,
            itemStatusId: emptyGuid(),
            commodityUsageTypeId: emptyGuid(),
            claimLimitation: 0.00,
            liabilityLimitation: 0.00,
            InsuaranceLimitationId: emptyGuid(),
            attributeSpecificationId: emptyGuid(),
            attributeSpecificationPrefix: "",
            attributeSpecificationName: "",
            warrantyTypeId: emptyGuid(),
            premiumBasedOnIdGross: emptyGuid(),
            premiumBasedOnIdNett: emptyGuid(),
            isCustAvailableGross: true,
            isCustAvailableNett: true,
            manufacturerWarrantyGross: false,
            manufacturerWarrantyNett: false,
            rsaProviderId: emptyGuid(),
            regionId: emptyGuid(),
            commodityCategoryId: emptyGuid(),
            minValueGross: '',
            maxValueGross: '',
            minValueNett: '',
            maxValueNett: '',
            annualPremiumTotal: 0.00,
            premiumCurrencyId: emptyGuid(),
            eligibilities: [],
            texes: [],
            commissions: [],
            labourChargeApplicableOnPolicySold: false,
            isPremiumVisibleToDealer: false,
            annualInterestRate:0.00
        }

        $scope.NRPCommissionContractMapping = {
            Id: emptyGuid(),
            ContractId: emptyGuid(),
            Commission: "",
            IsOnNRP: false,
            IsOnGross: false,
            IsPercentage: false,

        };
        var SearchContraactPopup;
        var NewExtentionTypePopup;
        var paginationOptionsContractSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.currencyPeriodCheck = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CurrencyManagement/CurrencyPeriodCheck',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == true) {
                    $scope.loadInitailData();
                } else if (data == false) {
                    swal({ title: 'TAS Security Information', text: 'No curency period is defined for today.Please contact Administrator.', showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    swal({ title: 'TAS Security Information', text: 'Error occured while getting currency period information.Please contact Administrator.', showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                }

            }).error(function (data, status, headers, config) {
            });

        }

        $scope.ValidategrossandnrpValues = function () {
            var haveErrorInputs = false;
            //$scope.commissionTypes = $scope.currentContract.commissions;
            angular.forEach($scope.commissionTypes, function (lvalue) {

                if (lvalue.Commission !== "" && parseFloat(lvalue.Commission) > 0) {
                    //  alert(lvalue.Commission);
                    if (lvalue.IsOnNRP === false && lvalue.IsOnGROSS === false || lvalue.IsOnNRP === true && lvalue.IsOnGROSS === true) {
                        // alert('here');


                        haveErrorInputs = true;
                        // haveErrorInputs = true;
                    }
                } else {
                    $scope.validate_CommissionValue = "";

                }

            });

             return haveErrorInputs;
        }
        $scope.loadInitailData = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
                $scope.Allcountries = data;
                console.log('allcountries', $scope.Allcountries);
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetProducts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.products = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.insurers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.itemStatuses = data;
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllCylinderCounts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.clinderCounts = data;
                // console.log(data);
                $scope.refreshCyllinderCountwithUI();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.engineCapacities = data;
                //  console.log(data);
                $scope.refreshCCwithUI();
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/AutomobileAttributes/GetAllVehicleWeight',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.grossVehicleWeights = data;
                // console.log(data);
                $scope.refreshWWwithUI();
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetPremiumBasedOns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.premiumBasedOnValues = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetWarrantyTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.warrantyTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetRSAProviders',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.rsaProviders = data;

            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetRegions',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.regions = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetCommissionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commissionTypes = data;
                angular.forEach($scope.commissionTypes, function (value) {
                    value.IsOnNRP = false;

                    value.IsOnGROSS = false;
                });
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityUsageTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetDealTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealTypes = data;
            }).error(function (data, status, headers, config) {
            });

        }
        $scope.loadPremiumSearechPopup = function () {
            $scope.ResetPremiumSearchPopup();
            getContractSearchPage();
            SearchContraactPopup = ngDialog.open({
                template: 'ContractSearchPopup',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            $scope.visitedInitially = false;
        }
        $rootScope.$on('ngDialog.closing', function (e, $dialog) {

        });
        $scope.setNettPremium = function () {
            $scope.totalNRP = 0.00;
            for (var i = 0; i < $scope.premiumAddonsNett.length; i++) {
                if (parseFloat($scope.premiumAddonsNett[i].value)) {
                    $scope.premiumAddonsNett[i].value = parseFloat($scope.premiumAddonsNett[i].value);
                } else {
                    $scope.premiumAddonsNett[i].value = 0.00;
                }
                $scope.totalNRP += parseFloat($scope.premiumAddonsNett[i].value);
            }
            $scope.totalNRP = parseFloat($scope.totalNRP).toFixed(2);
        }
        $scope.setGrossPremium = function () {
            $scope.grossPremium = 0.00;
            for (var i = 0; i < $scope.premiumAddonsGross.length; i++) {
                if (parseFloat($scope.premiumAddonsGross[i].Value)) {
                    $scope.premiumAddonsGross[i].Value = parseFloat($scope.premiumAddonsGross[i].Value);
                } else {
                    $scope.premiumAddonsGross[i].Value = 0.00;
                }
                $scope.grossPremium += parseFloat($scope.premiumAddonsGross[i].Value);
            }
            $scope.grossPremium = parseFloat($scope.grossPremium).toFixed(2);
        }

        $scope.$watch('currentContract.commodityTypeId', function () {
            for (var i = 0; i < $scope.commodityTypes.length; i++) {
                if ($scope.commodityTypes[i].CommodityTypeId === $scope.currentContract.commodityTypeId) {
                    if ($scope.commodityTypes[i].CommonCode === 'A') {
                        $scope.IsAutomobile = true;
                        $scope.IsYellowGood = false;
                        $scope.IsOther = false;
                    } else if ($scope.commodityTypes[i].CommonCode === 'B') {
                        $scope.IsAutomobile = true;
                        $scope.IsYellowGood = false;
                        $scope.IsOther = false;
                    } else if ($scope.commodityTypes[i].CommonCode === 'O') {
                        if ($scope.commodityTypes[i].CommodityCode === 'E') {
                            $scope.IsOther = false;
                            $scope.IsAutomobile = false;
                            $scope.IsYellowGood = false;

                        } else {
                            if ($scope.commodityTypes[i].CommodityCode === 'Y') {
                                $scope.IsYellowGood = true;
                                $scope.IsOther = false;
                                $scope.IsAutomobile = false;
                            } else {
                                $scope.IsYellowGood = false;
                                $scope.IsOther = true;
                                $scope.IsAutomobile = false;
                            }

                        }

                    }
                    return false;
                }
            }

        }, true);

        $scope.commonMultiselectSettings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        $scope.RefreshContractSearchGridData = function () {
            getContractSearchPage();
        }
        $scope.rowStatusFormatting = function (value, id) {
            if (value) {
                return "Inactive";
            } else {
                return "Active";
            }
        }
        $scope.updateContractStatus = function (id, isActive) {
            var status = "inactive";
            if (!isActive)
                status = "active";
            swal({
                title: "Are you sure?",
                text: "This will make selected contract " + status,
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#382E1C",
                confirmButtonText: "Yes",
                closeOnConfirm: true
            }, function () {
                if (isGuid(id)) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/UpdateContractStatus',
                        data: { "Id": id, "status": !isActive },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data) {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Successfully updated!",
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.RefreshContractSearchGridData();
                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Error occured.Please retry.",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }

                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });
                }
            });
        }
        $scope.contractSearchGrid = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'CommodityType', width: '18%', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: 'Country Name', field: 'CountryName', width: '16%', enableSorting: false, cellClass: 'columCss' },
                { name: 'Dealer Name', field: 'DealerName', width: '16%', enableSorting: false, cellClass: 'columCss' },
                { name: 'Product name', field: 'Productname', width: '16%',enableSorting: false, cellClass: 'columCss' },
                { name: 'Deal Name', field: 'DealName', width: '16%', enableSorting: false, cellClass: 'columCss' },
                {
                    name: '   ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.updateValidityCheck(row.entity.Id)" class="btn btn-xs btn-info">update</button></div>',
                    width: 60,
                    enableSorting: false
                }
                ,
                {
                    name: '    ',
                    cellTemplate: '<div class="center"><button style="width:100%;margin:2px" ng-click="grid.appScope.updateContractStatus(row.entity.Id,row.entity.IsActive)" class="btn btn-xs btn-beige">{{grid.appScope.rowStatusFormatting(row.entity.IsActive)}}</button></div>',
                    width: 70,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsContractSearchGrid.sort = null;
                    } else {
                        paginationOptionsContractSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getContractSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsContractSearchGrid.pageNumber = newPage;
                    paginationOptionsContractSearchGrid.pageSize = pageSize;
                    getContractSearchPage();
                });
            }
        };
        var searchingStatus = false;
        var getContractSearchPage = function () {
            if (searchingStatus == false) {
                searchingStatus = true;
            $scope.contractSearchGridloadAttempted = false;
            $scope.contractSearchGridloading = true;

            var contractSearchGridParam =
            {
                'paginationOptionsContractSearchGrid': paginationOptionsContractSearchGrid,
                'contractSearchGridSearchCriterias': $scope.contractSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetContractsForSearchGrid',
                data: contractSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                $scope.contractSearchGrid.data = response_arr.data;
                $scope.contractSearchGrid.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.contractSearchGridloadAttempted = true;
                $scope.contractSearchGridloading = false;
                searchingStatus = false;
            });
            }

        }
        $scope.updateValidityCheck = function (contractIdToUpdate) {
            if (isGuid(contractIdToUpdate)) {
                $scope.saveBtnDisabled = false;
                swal({ title: 'TAS Information', text: 'Loading Deal .....', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/ContractsUpdateValidityCheck',
                    data: { "Id": contractIdToUpdate },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (!data) {
                        customWarningMessage("Selected contract already linked with a policy.So it cannot be updated.");
                        $scope.saveBtnDisabled = true;
                    }
                    $scope.loadContract(contractIdToUpdate);
                }).error(function (data, status, headers, config) {
                    $scope.saveBtnDisabled = false;
                    swal.close();
                }).finally(function () {

                });
            } else {
                SweetAlert.swal({
                    title: "TAS Information",
                    text: "Invalid Contract selection",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            }
        }
        $scope.childProductsData_temp = [];
        $scope.loadContract = function (contractId) {
            if (isGuid(contractId)) {
                $scope.setupNewContract();
                $scope.formActionText = "Updating existing contract";
                $scope.formAction = false;
                $scope.contractIdToUpdate = contractId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetContractId',
                    data: { "Id": $scope.contractIdToUpdate },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.currentContract.countryId = data.data.contract.countryId;
                    $scope.selectedCountryChanged();
                    $scope.currentContract.commodityTypeId = data.data.contract.commodityTypeId;
                    $scope.selectedCommodityChanged();
                    $scope.currentContract.productId = data.data.contract.productId;
                    $scope.currentContract.dealerId = data.data.contract.dealerId;
                    $scope.currentContract.insurerId = data.data.contract.insurerId;
                    $scope.currentContract.annualInterestRate = data.data.contract.AnnualInterestRate;
                    $scope.selectedDealerChanged();


                    $scope.currentContract.linkDealId = data.data.contract.linkDealId;
                    $scope.selectedLinkDealChanged();
                    $scope.currentContract.dealName = data.data.contract.dealName;
                    $scope.temp_dealName = data.data.contract.dealName;

                    $scope.currentContract.dealType = data.data.contract.dealType;
                    $scope.currentContract.isPromotional = data.data.contract.isPromotional;
                    $scope.currentContract.discountAvailable = data.data.contract.discountAvailable;

                    $scope.currentContract.reinsurerContractId = data.data.contract.reinsurerContractId;
                    $scope.reinsurerContractId_temp = data.data.contract.reinsurerContractId;

                    //setting start and end dates at last
                    $scope.currentContract.contractStartDate = data.data.contract.startDate;
                    $scope.currentContract.contractEndDate = data.data.contract.endDate;

                    $scope.currentContract.remark = data.data.contract.remark;
                    $scope.currentContract.isAutoRenewal = data.data.contract.isAutoRenewal;
                    $scope.currentContract.isActive = data.data.contract.isActive;
                    $scope.currentContract.itemStatusId = data.data.contract.itemStatusId;
                    $scope.currentContract.commodityUsageTypeId = data.data.contract.commodityUsageTypeId;
                    $scope.currentContract.claimLimitation = data.data.contract.claimLimitation;
                    $scope.currentContract.liabilityLimitation = data.data.contract.liabilityLimitation;
                    $scope.currentContract.isPremiumVisibleToDealer = data.data.contract.isPremiumVisibleToDealer;
                    $scope.currentContract.labourChargeApplicableOnPolicySold = data.data.contract.labourChargeApplicableOnPolicySold;


                    $scope.currentContract.commodityCategoryId = data.data.contract.commodityCategoryId;
                    $scope.selectedCommodityCategoryChanged();

                    $scope.selectedProductChanged();
                    //eligibility criterias
                    $scope.currentContract.eligibilities = [];
                    angular.forEach(data.data.contract.eligibilities, function (value) {
                        var eligibility = {
                            ageMin: value.ageMin,
                            ageMax: value.ageMax,
                            milageMin: value.milageMin,
                            milageMax: value.milageMax,
                            monthsMin: value.monthsMin,
                            monthsMax: value.monthsMax,
                            isPresentage: value.isPresentage,
                            premium: value.premium,
                            plusMinus: value.plusMinus,
                            isMandatory: value.isMandatory
                        }
                        $scope.currentContract.eligibilities.push(eligibility);
                    });
                    //commission details show
                    var commissionSet = false;
                    angular.forEach(data.data.contract.commissions, function (value) {
                        var i = -1;
                        commissionSet = false;
                        angular.forEach($scope.commissionTypes, function (lvalue) {
                            i++;
                            if (value.Id === lvalue.Id && commissionSet === false) {
                                commissionSet = true;
                                lvalue.Commission = value.Commission;
                                lvalue.IsPercentage = value.IsPercentage;
                                lvalue.IsOnNRP = value.IsOnNRP;
                                lvalue.IsOnGROSS = value.IsOnGROSS;
                                $scope.commissionTypes[i] = lvalue;

                            }
                        });
                    });
                    $scope.temp_taxdetails = data.data.contract.texes;
                    $scope.childProductsData_temp = data.data.products;

                }).error(function (data, status, headers, config) {
                    swal.close();
                });
                SearchContraactPopup.close();
            } else {
                swal.close();
                SweetAlert.swal({
                    title: "TAS Information",
                    text: "Invalid Contract selection",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            }
        }

        $scope.selectedCountryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealersByCountryId',
                data: { "Id": $scope.currentContract.countryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealersByCountry = data;
                if (isGuid($scope.currentContract.insurerId) && isGuid($scope.contractIdToUpdate)) {
                    angular.forEach($scope.dealersByCountry, function (value) {
                        if ($scope.currentContract.dealerId == value.Id) {
                            $scope.currentContract.insurerId = value.InsurerId;
                            $scope.premiumCurrencyId = value.CurrencyId;

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContractsByInsurerId',
                                data: { "Id": $scope.currentContract.insurerId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.reinsurerContracts = data;
                                if (!$scope.formAction) {
                                    $scope.currentContract.reinsurerContractId = $scope.reinsurerContractId_temp;
                                    $scope.selectedReinsurerContractChanged();

                                }
                            }).error(function (data, status, headers, config) {
                            });


                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                                data: { "Id": value.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.premiumCurrency = data.Code;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                    data: { "Id": value.CurrencyId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    if (data == false) {
                                        SweetAlert.swal({
                                            title: "TAS Information",
                                            text: "Selected dealer's currency(" + $scope.premiumCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                            type: "warning",
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

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllTaxTypesFromCountryId',
                data: { "Id": $scope.currentContract.countryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countryTaxes = data;

                if (isGuid($scope.contractIdToUpdate)) {
                    angular.forEach($scope.temp_taxdetails, function (value) {
                        angular.forEach($scope.countryTaxes, function (lvalue) {
                            if (value.Id === lvalue.Id) {
                                lvalue.IsSelected = true;
                                return false;
                            }
                        });
                    });
                }
            }).error(function (data, status, headers, config) {
            });

        }
        $scope.selectedDealerChanged = function () {

            if (isGuid($scope.currentContract.dealerId)) {
                angular.forEach($scope.dealersByCountry, function (value) {
                    if ($scope.currentContract.dealerId == value.Id) {
                        $scope.currentContract.insurerId = value.InsurerId;
                        $scope.premiumCurrencyId = value.CurrencyId;
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
                            $scope.premiumCurrency = data.Code;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                data: { "Id": value.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == false) {
                                    SweetAlert.swal({
                                        title: "TAS Information",
                                        text: "Selected dealer's currency(" + $scope.premiumCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                }
                            }).error(function (data, status, headers, config) {
                            });
                            return false;

                        });
                    }
                });
            }
        }

        $scope.selectedCommodityChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityCategories',
                data: { "CommodityTypeId": $scope.currentContract.commodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
                if (!isGuid($scope.contractIdToUpdate)) {
                    $scope.selectedMakeList = [];
                    $scope.selectedModelsList = [];
                }

            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetPremiumAddonTypeByCommodityTypeId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { 'Id': $scope.currentContract.commodityTypeId }
            }).success(function (data, status, headers, config) {
                $scope.premiumAddonsGross = data;
                $scope.premiumAddonsNett = data;

            }).error(function (data, status, headers, config) {
                });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',

                data: { "Id": $scope.currentContract.commodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }

            }).success(function (data, status, headers, config) {
                $scope.commodityProducts = data;
                if ($scope.contractIdToUpdate) {
                    $scope.selectedProductChanged();
                }
            }).error(function (data, status, headers, config) {
            });


        }
        $scope.IsBundleProduct = false;
        $scope.allChildProducts = [];
        $scope.selectedProductChanged = function () {
        $scope.firstCall = false;
            $scope.allChildProducts = [];
            if (isGuid($scope.currentContract.dealerId) && isGuid($scope.currentContract.productId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetLinkContracts',
                    data: { "DealerId": $scope.currentContract.dealerId, "ProductId": $scope.currentContract.productId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.linkDeals = data;
                    swal.close();
                }).error(function (data, status, headers, config) {
                    swal.close();
                });
            }

            for (var i = 0; i < $scope.commodityProducts.length; i++) {
                if ($scope.commodityProducts[i].Id === $scope.currentContract.productId) {
                    $scope.currentProductType = $scope.commodityProducts[i].ProductTypeCode;
                    $scope.IsBundleProduct = $scope.commodityProducts[i].Isbundledproduct;
                    if ($scope.IsBundleProduct) {
                        $scope.loadBundleProductDetailsByBundleProductId($scope.commodityProducts[i].Id);
                    } else {
                        $scope.allChildProducts[0] = $scope.commodityProducts[i];

                        angular.forEach($scope.allChildProducts, function (child) {
                            child['selectedMakeList'] = [];

                            child['selectedModelsList'] = [];
                            child['availableModelsDrp'] = [];

                            child['selectedVariantList'] = [];
                            child['availableVariantsDrp'] = [];



                            child['selectedClinderCounts'] = [];
                            child['selectedEngineCapacities'] = [];
                            child['selectedGrossWeights'] = [];

                            child['premiumAddonsNett'] = angular.copy($scope.premiumAddonsNett);
                            child['premiumAddonsGross'] = angular.copy($scope.premiumAddonsGross);

                        });
                    }
                    if ($scope.contractIdToUpdate) {

                        angular.forEach($scope.childProductsData_temp, function (product) {
                            angular.forEach($scope.allChildProducts, function (childProduct) {
                                if (product.Id === childProduct.Id) {
                                    $scope.visitedInitially = false;
                                    $scope.firstCall = false;
                                    childProduct.attributeSpecificationId = product.attributeSpecificationId;
                                    $scope.getLimitationsByChildProduct(childProduct, product, false);
                                    return false;
                                }
                            });
                        });
                    }
                }
            }

            $scope.watchDynamicVariables();
        }

        $scope.loadBundleProductDetailsByBundleProductId = function (bundleProductId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllChildProducts',
                data: { "Id": bundleProductId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.allChildProducts = data;

                angular.forEach($scope.allChildProducts, function (child) {
                    child['selectedMakeList'] = [];

                    child['selectedModelsList'] = [];
                    child['availableModelsDrp'] = [];

                    child['selectedVariantList'] = [];
                    child['availableVariantsDrp'] = [];

                    child['selectedClinderCounts'] = [];
                    child['selectedEngineCapacities'] = [];
                    child['selectedGrossWeights'] = [];

                    child['premiumAddonsNett'] = angular.copy($scope.premiumAddonsNett);
                    child['premiumAddonsGross'] = angular.copy($scope.premiumAddonsGross);
                });

            }).error(function (data, status, headers, config) {
            });
        }
        $scope.watchDynamicVariables = function () {
            var childProductsCount = $scope.allChildProducts.length;
            var productIndex = 0;
            if (childProductsCount === 1)
                productIndex = 0;

            for (var i = 0; i < $scope.allChildProducts.length; i++) {
                //make
                $scope.$watch("allChildProducts['" + i + "'].selectedMakeList", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.getAllModelsRelatedToSelectedMakes($scope.allChildProducts[productIndex]);
                        if (newVal.length === $scope.availableMakesDrp.length)
                            $scope.allChildProducts[productIndex].isAllMakesSelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllMakesSelected = false;
                    }
                }, true);
                //model
                $scope.$watch("allChildProducts['" + i + "'].selectedModelsList", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        $scope.getAllVariantsRelatedToSelectedModels($scope.allChildProducts[productIndex]);
                        if (typeof $scope.allChildProducts[productIndex].availableModelsDrp !== 'undefined' &&
                            newVal.length === $scope.allChildProducts[productIndex].availableModelsDrp.length)
                            $scope.allChildProducts[productIndex].isAllModelsSelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllModelsSelected = false;
                    }
                }, true);

                //variant
                $scope.$watch("allChildProducts['" + i + "'].selectedVariantList", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        if (typeof $scope.allChildProducts[productIndex].availableVariantsDrp !== 'undefined'
                            && newVal.length === $scope.allChildProducts[productIndex].availableVariantsDrp.length)
                            $scope.allChildProducts[productIndex].isAllVariantSelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllVariantSelected = false;
                    }
                }, true);

                //cc
                $scope.$watch("allChildProducts['" + i + "'].selectedClinderCounts", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        if (typeof $scope.clinderCountsDrp !== 'undefined' &&
                            newVal.length === $scope.clinderCountsDrp.length)
                            $scope.allChildProducts[productIndex].isAllCyllinderCountSelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllCyllinderCountSelected = false;
                    }
                }, true);


                //ec
                $scope.$watch("allChildProducts['" + i + "'].selectedEngineCapacities", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        if (typeof $scope.engineCapacitiesDrp !== 'undefined' &&
                            newVal.length === $scope.engineCapacitiesDrp.length)
                            $scope.allChildProducts[productIndex].isAllEngineCapacitySelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllEngineCapacitySelected = false;
                    }
                }, true);

                //gvw
                $scope.$watch("allChildProducts['" + i + "'].selectedGrossWeights", function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        if (typeof $scope.grossWeightDrp !== 'undefined' &&
                            newVal.length === $scope.grossWeightDrp.length)
                            $scope.allChildProducts[productIndex].isAllGvwSelected = true;
                        else
                            $scope.allChildProducts[productIndex].isAllGvwSelected = false;
                    }
                }, true);
            }

        }

        $scope.getAllModelsRelatedToSelectedMakes = function (childProduct) {
           // console.log(childProduct);
            if (typeof childProduct.availableModelsDrp !== 'undefined')
                childProduct.availableModelsDrp = [];
            // childProduct.selectedModelsList = [];
            // childProduct.selectedVariantList = [];

            var selectedMakeIds = [];
            angular.forEach(childProduct.selectedMakeList, function (value) {
                selectedMakeIds.push(value.id);
            });
            if (selectedMakeIds.length > 0) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeIdsAndCommodityCategoryId',
                    data: JSON.stringify({
                        'data': selectedMakeIds,
                        'commodityCategoryId': $scope.currentContract.commodityCategoryId
                    }),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var modelData = [];
                    angular.forEach(data, function (value) {
                        var x = {
                            "id": value.Id,
                            "label": value.ModelName,
                        };
                        modelData.push(x);
                    });

                    childProduct.availableModelsDrp = modelData;

                }).error(function (data, status, headers, config) {
                });
            } else {

            }
        }

        $scope.loadRsaRelatedDetails = function () {
            if ($scope.currentProductType === "RSA") {

            }
        }
        $scope.selectedLinkDealChanged = function () {
            angular.forEach($scope.linkDeals, function (value) {
                if (value.Id === $scope.currentContract.linkDealId) {
                    //$scope.currentContract.dealName = value.DealName;
                    $scope.currentContract.dealType = value.DealType;
                    $scope.currentContract.isPromotional = value.IsPromotional;
                    $scope.currentContract.discountAvailable = value.DiscountAvailable;
                    $scope.currentContract.insurerId = value.InsurerId;
                    $scope.currentContract.isAutoRenewal = value.IsAutoRenewal;
                    $scope.currentContract.discountAvailable = value.DiscountAvailable;
                    $scope.currentContract.claimLimitation = value.ClaimLimitation;
                    $scope.currentContract.liabilityLimitation = value.LiabilityLimitation;
                    $scope.currentContract.remark = value.Remark;
                    $scope.currentContract.itemStatusId = value.ItemStatusId;
                    //$scope.currentContract.contractStartDate = value.StartDate;
                    //$scope.currentContract.contractEndDate = value.EndDate;
                    $scope.currentContract.commodityUsageTypeId = value.CommodityUsageTypeId;

                    $scope.loadContractForLinkDeal($scope.currentContract.linkDealId);

                    return false;
                }
            });
        }

        $scope.loadContractForLinkDeal = function (contractId) {
            if (isGuid(contractId)) {


                $scope.contractIdToUpdate = contractId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetContractId',
                    data: { "Id": $scope.contractIdToUpdate },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                   // $scope.currentContract.countryId = data.data.contract.countryId;
                    $scope.selectedCountryChanged();
                    //$scope.currentContract.commodityTypeId = data.data.contract.commodityTypeId;
                    $scope.selectedCommodityChanged();

                    $scope.currentContract.insurerId = data.data.contract.insurerId;
                    $scope.currentContract.annualInterestRate = data.data.contract.AnnualInterestRate;
                    $scope.selectedDealerChanged();


                    //$scope.selectedLinkDealChanged();
                    $scope.currentContract.dealName = data.data.contract.dealName;
                    $scope.temp_dealName = data.data.contract.dealName;

                    $scope.currentContract.dealType = data.data.contract.dealType;
                    $scope.currentContract.isPromotional = data.data.contract.isPromotional;
                    $scope.currentContract.discountAvailable = data.data.contract.discountAvailable;

                    $scope.currentContract.reinsurerContractId = data.data.contract.reinsurerContractId;
                    $scope.reinsurerContractId_temp = data.data.contract.reinsurerContractId;

                    //setting start and end dates at last
                    $scope.currentContract.contractStartDate = data.data.contract.startDate;
                    $scope.currentContract.contractEndDate = data.data.contract.endDate;

                    $scope.currentContract.remark = data.data.contract.remark;
                    $scope.currentContract.isAutoRenewal = data.data.contract.isAutoRenewal;
                    $scope.currentContract.isActive = data.data.contract.isActive;
                    $scope.currentContract.itemStatusId = data.data.contract.itemStatusId;
                    $scope.currentContract.commodityUsageTypeId = data.data.contract.commodityUsageTypeId;
                    $scope.currentContract.claimLimitation = data.data.contract.claimLimitation;
                    $scope.currentContract.liabilityLimitation = data.data.contract.liabilityLimitation;
                    $scope.currentContract.isPremiumVisibleToDealer = data.data.contract.isPremiumVisibleToDealer;
                    $scope.currentContract.labourChargeApplicableOnPolicySold = data.data.contract.labourChargeApplicableOnPolicySold;


                    $scope.selectedCommodityCategoryChanged();

                    //$scope.selectedProductChanged();
                    //eligibility criterias
                    $scope.currentContract.eligibilities = [];
                    angular.forEach(data.data.contract.eligibilities, function (value) {
                        var eligibility = {
                            ageMin: value.ageMin,
                            ageMax: value.ageMax,
                            milageMin: value.milageMin,
                            milageMax: value.milageMax,
                            monthsMin: value.monthsMin,
                            monthsMax: value.monthsMax,
                            isPresentage: value.isPresentage,
                            premium: value.premium,
                            plusMinus: value.plusMinus,
                            isMandatory: value.isMandatory
                        }
                        $scope.currentContract.eligibilities.push(eligibility);
                    });
                    //commission details show
                    var commissionSet = false;
                    angular.forEach(data.data.contract.commissions, function (value) {
                        var i = -1;
                        commissionSet = false;
                        angular.forEach($scope.commissionTypes, function (lvalue) {
                            i++;
                            if (value.Id === lvalue.Id && commissionSet === false) {
                                commissionSet = true;
                                lvalue.Commission = value.Commission;
                                lvalue.IsPercentage = value.IsPercentage;
                                lvalue.IsOnNRP = value.IsOnNRP;
                                lvalue.IsOnGROSS = value.IsOnGROSS;
                                $scope.commissionTypes[i] = lvalue;

                            }
                        });
                    });
                    $scope.temp_taxdetails = data.data.contract.texes;
                    $scope.childProductsData_temp = data.data.products;
                    //$scope.currentContract.productId = $scope.currentContract.productId;
                    //$scope.currentContract.dealerId = $scope.currentContract.dealerId;
                    //$scope.currentContract.linkDealId = $scope.currentContract.linkDealId;
                    //$scope.currentContract.commodityCategoryId = data.data.contract.commodityCategoryId;
                    //$scope.contractIdToUpdate = emptyGuid();

                }).error(function (data, status, headers, config) {
                });
                SearchContraactPopup.close();
            } else {
                SweetAlert.swal({
                    title: "TAS Information",
                    text: "Invalid Contract selection",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            }
        }


        $scope.setMilageUnlimited = function () {
            if ($scope.newInsuaranceLimitation.unlimitedcheck) {
                $scope.newInsuaranceLimitation.km = "";
            }
        }
        $scope.selectedReinsurerContractChanged = function () {
            if (!isGuid($scope.currentContract.reinsurerContractId)) {
                $scope.currentContract.contractStartDate = "";
                $scope.currentContract.contractEndDate = "";

            }
            angular.forEach($scope.reinsurerContracts, function (value) {
                if (value.Id === $scope.currentContract.reinsurerContractId) {

                    $scope.currentContract.contractStartDate = value.FromDate;
                    $scope.currentContract.contractEndDate = value.ToDate;

                    return false;
                }
            });
        }
        $scope.setMaxMinDates = function () {
            if (!isGuid($scope.currentContract.reinsurerContractId)) {
                $scope.reinsurerContractMaxDate = "";
                $scope.reinsurerContractMinDate = "";

            }
            angular.forEach($scope.reinsurerContracts, function (value) {
                if (value.Id === $scope.currentContract.reinsurerContractId) {
                    //  alert(value.FromDate)
                    $scope.reinsurerContractMaxDate = value.ToDate;
                    $scope.reinsurerContractMinDate = value.FromDate;
                    return false;
                }
            });
        }
        $scope.calculateAnnualPremium = function (childProduct) {
            childProduct.annualPremiumTotal = 0.00;
            for (var i = 0; i < childProduct.annualPremium.length; i++) {
                if (parseFloat(childProduct.annualPremium[i].Value)) {
                    childProduct.annualPremiumTotal += parseFloat(childProduct.annualPremium[i].Value);
                } else {
                    childProduct.annualPremium[i].Value = 0.00;
                }
            }
            angular.forEach($scope.premiumAddonsNett, function (value) {
                if (value.IndexNo === 1) {
                    value.value = childProduct.annualPremiumTotal;
                    return false;
                }
            });
            angular.forEach($scope.premiumAddonsGross, function (value) {
                if (value.IndexNo === 1) {
                    value.Value = childProduct.annualPremiumTotal;
                    return false;
                }
            });
        }
        $scope.selectedInsuaranceLimitationChanged = function (childProduct) {
            if (childProduct.Productcode === "RSA") {
                angular.forEach(childProduct.insuaranceLimitations, function (value) {
                    if (value.Id === childProduct.InsuaranceLimitationId) {
                        var years = value.Months / 12;
                        childProduct.annualPremium = [];
                        for (var i = 0; i < years; i++) {
                            var annulap = {
                                Year: i + 1
                            };
                            childProduct.annualPremium.push(annulap);
                        }
                    }
                });
            } else {
                if (isGuid(childProduct.InsuaranceLimitationId)) {
                    angular.forEach(childProduct.insuaranceLimitations, function (value) {
                        if (value.Id === childProduct.InsuaranceLimitationId) {

                            $scope.getAllAttributeSpecificationsByInsuranceLimitataionId(childProduct);
                            childProduct.attributeSpecificationPrefix = value.InsuaranceLimitationName;
                            return;
                        }
                    });

                } else {
                    childProduct.attributeSpecificationPrefix = "";
                }
            }
        }
        $scope.selectedAttributeSpecificationChanged = function (childProduct) {
            childProduct.selectedMakeList = [];
            childProduct.selectedModelsList = [];
            childProduct.selectedVariantList = [];
            childProduct.selectedEngineCapacities = [];
            childProduct.selectedClinderCounts = [];
            childProduct.selectedGrossWeights = [];
            childProduct.attributeSpecificationName = "";
            //childProduct.warrantyTypeId = emptyGuid();
            childProduct.itemStatusId = emptyGuid();
            $scope.selectedItemStatusChanged(childProduct);



            angular.forEach(childProduct.attributeSpecifications, function (value) {

                if (value.AttributeSpecificationId === childProduct.attributeSpecificationId) {
                    childProduct.attributeSpecificationName = value.AttributeSpecification;
                    var attribSpecId = value.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/GetAllMakeModelDetailsByExtensionId',
                        data: { "extensionId": attribSpecId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        childProduct.isAllMakesSelected = data.isAllMakesSelected;
                        childProduct.isAllModelsSelected = data.isAllModelsSelected;
                        childProduct.isAllVariantSelected = data.isAllVariantsSelected;
                        childProduct.isAllCyllinderCountSelected = data.isAllCCsSelected;
                        childProduct.isAllEngineCapacitySelected = data.isAllECsSelected;
                        childProduct.isAllGvwSelected = data.isAllGVWsSelected;

                        //make
                        angular.forEach(data.selectedMakeList, function (make) {
                            childProduct.selectedMakeList.push(
                                {
                                    id: make.id,
                                }
                            );
                        });
                        //model
                        angular.forEach(data.selectedModelsList, function (model) {
                            childProduct.selectedModelsList.push(
                                {
                                    id: model.id,
                                }
                            );
                        });
                        //variant
                        angular.forEach(data.selectedVariantList, function (variant) {
                            childProduct.selectedVariantList.push(
                                {
                                    id: variant.id,
                                }
                            );
                        });
                        //ec
                        if (childProduct.isAllEngineCapacitySelected) {
                            angular.forEach($scope.engineCapacitiesDrp, function (ec) {
                                childProduct.selectedEngineCapacities.push(
                                    {
                                        id: ec.id,
                                    });
                            });
                        } else {
                            angular.forEach(data.selectedEngineCapacities, function (ec) {
                                childProduct.selectedEngineCapacities.push(
                                    {
                                        id: ec.id,
                                    }
                                );
                            });
                        }
                        //cc
                        if (childProduct.isAllCyllinderCountSelected) {
                            angular.forEach($scope.clinderCountsDrp, function (cc) {
                                childProduct.selectedClinderCounts.push(
                                    {
                                        id: cc.id,
                                    });
                            });
                        } else {
                            angular.forEach(data.selectedClinderCounts, function (cc) {
                                childProduct.selectedClinderCounts.push(
                                    {
                                        id: cc.id,
                                    }
                                );
                            });
                        }
                        //gvw
                        if (childProduct.isAllGvwSelected) {
                            angular.forEach($scope.grossWeightDrp, function (gvw) {
                                childProduct.selectedGrossWeights.push(
                                    {
                                        id: gvw.id,
                                    });
                            });
                        } else {
                            angular.forEach(data.selectedGrossWeights, function (gvw) {
                                childProduct.selectedGrossWeights.push(
                                    {
                                        id: gvw.id,
                                    }
                                );
                            });
                        }

                    }).error(function (data, status, headers, config) {
                    });

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/GetAllPremiumByExtensionId',
                        data: { "extensionId": attribSpecId },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        childProduct.allAvailablePremiums = data;

                        if (data.length > 0) {
                            //select first premium
                            childProduct.warrantyTypeId = data[0].warrentyTypeId;
                            childProduct.itemStatusId = data[0].itemStatusId;
                            childProduct.premiumBasedOnIdNett = data[0].premiumBasedOnIdNett;
                            $scope.setPremiumBasedOnValue(childProduct, 'NETT');
                            childProduct.premiumBasedOnIdGross = data[0].premiumBasedOnIdGross;
                            $scope.setPremiumBasedOnValue(childProduct, 'GROSS');
                            childProduct.isCustAvailableNett = data[0].isCustAvailableNett;
                            childProduct.isCustAvailableGross = data[0].isCustAvailableGross;

                            childProduct.minValueNett = data[0].minValueNett;
                            childProduct.maxValueNett = data[0].maxValueNett;
                            childProduct.minValueGross = data[0].minValueGross;
                            childProduct.maxValueGross = data[0].maxValueGross;

                            childProduct.totalNRP = data[0].nrp;
                            childProduct.grossPremium = data[0].gross;

                            var foundNett = false;
                            angular.forEach(childProduct.premiumAddonsNett, function (nettAddon) {
                                foundNett = false;
                                angular.forEach(data[0].premiumAddonsNett, function (nettAddonObj) {
                                    if (!foundNett) {
                                        if (nettAddon.Id === nettAddonObj.Id) {
                                            foundNett = true;
                                            nettAddon.value = nettAddonObj.value;
                                        }
                                    }
                                });
                            });

                            var foundGross = false;
                            angular.forEach(childProduct.premiumAddonsGross, function (grossAddon) {
                                foundGross = false;
                                angular.forEach(data[0].premiumAddonsGross, function (grossAddonObj) {
                                    if (!foundGross) {
                                        if (grossAddon.Id === grossAddonObj.Id) {
                                            foundGross = true;
                                            grossAddon.Value = grossAddonObj.Value;
                                        }
                                    }
                                });
                            });
                        } else {
                            customErrorMessage("No premium found for selected criterias.");
                        }

                    }).error(function (data, status, headers, config) {
                    });
                }
            });
        }
        $scope.selectedItemStatusChanged = function (childProduct) {
            if (isGuid($scope.contractIdToUpdate)) {
                var premiumAvailable = false;
                //console.log(childProduct.allAvailablePremiums);
                angular.forEach(childProduct.allAvailablePremiums, function (premium) {

                    if (premium.warrentyTypeId === childProduct.warrantyTypeId
                        && premium.itemStatusId === childProduct.itemStatusId) {
                        premiumAvailable = true;
                        //premium available

                        childProduct.premiumBasedOnIdNett = premium.premiumBasedOnIdNett;
                        $scope.setPremiumBasedOnValue(childProduct, 'NETT');
                        childProduct.premiumBasedOnIdGross = premium.premiumBasedOnIdGross;
                        $scope.setPremiumBasedOnValue(childProduct, 'GROSS');
                        childProduct.isCustAvailableNett = premium.isCustAvailableNett;
                        childProduct.isCustAvailableGross = premium.isCustAvailableGross;

                        childProduct.minValueNett = premium.minValueNett;
                        childProduct.maxValueNett = premium.maxValueNett;
                        childProduct.minValueGross = premium.minValueGross;
                        childProduct.maxValueGross = premium.maxValueGross;

                        childProduct.totalNRP = premium.nrp;
                        childProduct.grossPremium = premium.gross;

                        var foundNett = false;
                        angular.forEach(childProduct.premiumAddonsNett, function (nettAddon) {
                            foundNett = false;
                            angular.forEach(premium.premiumAddonsNett, function (nettAddonObj) {
                                if (!foundNett) {
                                    if (nettAddon.Id === nettAddonObj.Id) {
                                        foundNett = true;
                                        nettAddon.value = nettAddonObj.value;
                                    }
                                }
                            });
                        });

                        var foundGross = false;
                        angular.forEach(childProduct.premiumAddonsGross, function (grossAddon) {
                            foundGross = false;
                            angular.forEach(premium.premiumAddonsGross, function (grossAddonObj) {
                                if (!foundGross) {
                                    if (grossAddon.Id === grossAddonObj.Id) {
                                        foundGross = true;
                                        grossAddon.Value = grossAddonObj.Value;
                                    }
                                }
                            });
                        });
                    }
                });

                if (premiumAvailable === false) {
                    if (childProduct.itemStatusId !== emptyGuid())
                        customErrorMessage("No premium found for selected criterias.");
                    childProduct.premiumBasedOnIdNett = emptyGuid();
                    $scope.setPremiumBasedOnValue(childProduct, 'NETT');
                    childProduct.premiumBasedOnIdGross = emptyGuid();
                    $scope.setPremiumBasedOnValue(childProduct, 'GROSS');
                    childProduct.isCustAvailableNett = false;
                    childProduct.isCustAvailableGross = false;

                    childProduct.minValueNett = 0.00;
                    childProduct.maxValueNett = 0.00;
                    childProduct.minValueGross = 0.00;
                    childProduct.maxValueGross = 0.00;



                    childProduct.totalNRP = 0.00;
                    childProduct.grossPremium = 0.00;

                    var foundNett = false;
                    angular.forEach(childProduct.premiumAddonsNett, function (nettAddon) {
                        foundNett = false;
                        nettAddon.value = 0.00;
                    });

                    var foundGross = false;
                    angular.forEach(childProduct.premiumAddonsGross, function (grossAddon) {
                        foundGross = false;
                        grossAddon.Value = 0.00;
                    });
                }
            }
        }
        $scope.selectedWarrentyTypeChanged = function (childProduct) {
         //   console.log(childProduct.allAvailablePremiums);
            if (isGuid($scope.contractIdToUpdate)) {
                var premiumAvailable = false;
                angular.forEach(childProduct.allAvailablePremiums, function (premium) {

                    if (premium.warrentyTypeId === childProduct.warrantyTypeId
                        && premium.itemStatusId === childProduct.itemStatusId) {
                        premiumAvailable = true;
                        //premium available

                        childProduct.premiumBasedOnIdNett = premium.premiumBasedOnIdNett;
                        $scope.setPremiumBasedOnValue(childProduct, 'NETT');
                        childProduct.premiumBasedOnIdGross = premium.premiumBasedOnIdGross;
                        $scope.setPremiumBasedOnValue(childProduct, 'GROSS');
                        childProduct.isCustAvailableNett = premium.isCustAvailableNett;
                        childProduct.isCustAvailableGross = premium.isCustAvailableGross;

                        childProduct.minValueNett = premium.minValueNett;
                        childProduct.maxValueNett = premium.maxValueNett;
                        childProduct.minValueGross = premium.minValueGross;
                        childProduct.maxValueGross = premium.maxValueGross;



                        childProduct.totalNRP = premium.nrp;
                        childProduct.grossPremium = premium.gross;

                        var foundNett = false;
                        angular.forEach(childProduct.premiumAddonsNett, function (nettAddon) {
                            foundNett = false;
                            angular.forEach(premium.premiumAddonsNett, function (nettAddonObj) {
                                if (!foundNett) {
                                    if (nettAddon.Id === nettAddonObj.Id) {
                                        foundNett = true;
                                        nettAddon.value = nettAddonObj.value;
                                    }
                                }
                            });
                        });

                        var foundGross = false;
                        angular.forEach(childProduct.premiumAddonsGross, function (grossAddon) {
                            foundGross = false;
                            angular.forEach(premium.premiumAddonsGross, function (grossAddonObj) {
                                if (!foundGross) {
                                    if (grossAddon.Id === grossAddonObj.Id) {
                                        foundGross = true;
                                        grossAddon.Value = grossAddonObj.Value;
                                    }
                                }
                            });
                        });
                    }
                });

                if (premiumAvailable === false) {
                    if (childProduct.warrantyTypeId !== emptyGuid())
                        customErrorMessage("No premium found for selected criterias.");
                    childProduct.premiumBasedOnIdNett = emptyGuid();
                    $scope.setPremiumBasedOnValue(childProduct, 'NETT');
                    childProduct.premiumBasedOnIdGross = emptyGuid();
                    $scope.setPremiumBasedOnValue(childProduct, 'GROSS');
                    childProduct.isCustAvailableNett = false;
                    childProduct.isCustAvailableGross = false;

                    childProduct.minValueNett = 0.00;
                    childProduct.maxValueNett = 0.00;
                    childProduct.minValueGross = 0.00;
                    childProduct.maxValueGross = 0.00;
                    childProduct.totalNRP = 0.00;
                    childProduct.grossPremium = 0.00;

                    var foundNett = false;
                    angular.forEach(childProduct.premiumAddonsNett, function (nettAddon) {
                        foundNett = false;
                        nettAddon.value = 0.00;
                    });

                    var foundGross = false;
                    angular.forEach(childProduct.premiumAddonsGross, function (grossAddon) {
                        foundGross = false;
                        grossAddon.Value = 0.00;
                    });
                }
            }
        }
        $scope.getAllAttributeSpecificationsByInsuranceLimitataionId = function (childProduct) {


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetAllAttributeSpecificationsByInsuranceLimitataionId',
                data: {
                    "insuaranceLimitationId": childProduct.InsuaranceLimitationId,
                    "contractId": $scope.contractIdToUpdate
                },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                childProduct.attributeSpecifications = data;
                $scope.selectedAttributeSpecificationChanged(childProduct);
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.currentChildProduct = [];
        $scope.showNewExtentionTypePopup = function (childProduct) {
            NewExtentionTypePopup = ngDialog.open({
                template: 'NewInsuranceLimitationPopup',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            $scope.currentChildProduct = childProduct;
        }
        $scope.validateNewInsuaranceLimitation = function () {
            var isValid = true;

            if ($scope.currentChildProduct.CommodityType === 'A' || $scope.currentChildProduct.CommodityType === 'Y' || $scope.currentChildProduct.CommodityType === 'B') {

                if (!parseFloat($scope.newInsuaranceLimitation.month) || parseFloat($scope.newInsuaranceLimitation.month) <= 0) {
                    isValid = false;
                    $scope.newInsuaranceLimitation.validateMonth = "has-error";
                } else {
                    $scope.newInsuaranceLimitation.validateMonth = "";
                }

                if ($scope.currentChildProduct.Productcode !== 'RSA') {

                    if (!$scope.newInsuaranceLimitation.unlimitedcheck) {
                        if (!parseFloat($scope.newInsuaranceLimitation.km) || parseFloat($scope.newInsuaranceLimitation.km) <= 0) {
                            isValid = false;
                            $scope.newInsuaranceLimitation.validatekm = "has-error";
                        } else {
                            $scope.newInsuaranceLimitation.validatekm = "";
                        }
                    }
                }
            } else {
                if ($scope.currentChildProduct.CommodityType === 'O') {
                    if (!parseFloat($scope.newInsuaranceLimitation.month) || parseFloat($scope.newInsuaranceLimitation.month) <= 0) {
                        isValid = false;
                        $scope.newInsuaranceLimitation.validateMonth = "has-error";
                    } else {
                        $scope.newInsuaranceLimitation.validateMonth = "";
                    }
                } else {
                    if (!parseFloat($scope.newInsuaranceLimitation.hours) || parseFloat($scope.newInsuaranceLimitation.hours) <= 0) {
                        isValid = false;
                        $scope.newInsuaranceLimitation.validatehours = "has-error";
                    } else {
                        $scope.newInsuaranceLimitation.validatehours = "";
                    }
                }
            }
            if (!isValid) {
                customErrorMessage("Please select all the mandetory fields.");
                return false;
            }
            return true;
        }
        $scope.setNewInsuaranceLimitation = function () {

            if (isGuid($scope.currentContract.commodityTypeId)) {
                if ($scope.validateNewInsuaranceLimitation()) {

                    if (typeof $scope.newInsuaranceLimitation.km === 'undefined' ||
                        $scope.newInsuaranceLimitation.km === null || $scope.newInsuaranceLimitation.km === '')
                        $scope.newInsuaranceLimitation.km = 0;
                    if (typeof $scope.newInsuaranceLimitation.hours === 'undefined' ||
                        $scope.newInsuaranceLimitation.hours === null || $scope.newInsuaranceLimitation.hours === '')
                        $scope.newInsuaranceLimitation.hours = 0;
                    if (typeof $scope.newInsuaranceLimitation.month === 'undefined' ||
                        $scope.newInsuaranceLimitation.month === null || $scope.newInsuaranceLimitation.month === '')
                        $scope.newInsuaranceLimitation.month = 0;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/AddNewInsuaranceLimitation',
                        data: {
                            "insuaranceLimitation": $scope.newInsuaranceLimitation,
                            'commodityTypeId': $scope.currentContract.commodityTypeId,
                            'isRsa': $scope.currentChildProduct.Productcode === "RSA",
                            'isOther': $scope.currentChildProduct.CommodityType === 'O',
                            'isAutomobile': $scope.currentChildProduct.CommodityType === 'A',
                            'isYellowGood': $scope.currentChildProduct.CommodityType === 'Y',
                            "loggedInUserId": $localStorage.LoggedInUserId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        if (data.code === "ok") {
                            $scope.setNewInsuaranceLimitationMsg = "";
                            customInfoMessage("Insuarance limitation saved successfully.");
                            angular.forEach($scope.allChildProducts, function (value) {
                                if (value.Id === $scope.currentChildProduct.Id) {
                                    $scope.getLimitationsByChildProduct(value, null, true);
                                }
                            });

                            NewExtentionTypePopup.close();
                            $scope.newInsuaranceLimitation = {
                                km: "",
                                month: "",
                                hours: "",
                                unlimitedcheck: false
                            }
                        } else {

                            $scope.setNewInsuaranceLimitationMsg = data.msg;
                            customErrorMessage(data.msg);
                        }
                    }).error(function (data, status, headers, config) {
                    });

                }

            } else {
                customErrorMessage("Please select a commodity type.");
            }

        }

        $scope.visitedInitially = false;
        $scope.firstCall = false;
        $scope.getLimitationsByChildProduct = function (childProduct, product, onInit) {
            if ($scope.firstCall == false) {
                $scope.firstCall = true;
                $scope.visitedInitially = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetAllInsuaranceLimitaionsByCommodityType',
                    data: {
                        "Id": childProduct.CommodityTypeId,
                        "productType": childProduct.Productcode
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }

                }).success(function (data, status, headers, config) {
                    childProduct.insuaranceLimitations = data;

                    if (isGuid($scope.contractIdToUpdate) && typeof product !== 'undefined' && product != null) {
                        if (childProduct.Id === product.Id) {
                            childProduct.InsuaranceLimitationId = product.InsuaranceLimitationId;
                            $scope.selectedInsuaranceLimitationChanged(childProduct);

                        }
                    }


                    $scope.firstCall = false;
                }).error(function (data, status, headers, config) {
                    $scope.firstCall = false;
                });



            }
        }

        $scope.isAllMakesSelected = false;
        $scope.isAllModelsSelected = false;
        $scope.isAllVariantSelected = false;
        $scope.isAllCyllinderCountSelected = false;
        $scope.isAllEngineCapacitySelected = false;
        $scope.isAllGvwSelected = false;


        $scope.calculateTotalNrp = function (childProduct) {
            var nrp = 0.00;
            angular.forEach(childProduct.premiumAddonsNett, function (addon) {
                if (!parseFloat(addon.value)) addon.value = 0.00;
                nrp += parseFloat(addon.value) ? parseFloat(addon.value) : 0.00;
            });
            childProduct.totalNRP = parseFloat(nrp).toFixed(2);
        }

        $scope.calculateTotalGross = function (childProduct) {
            var gross = 0.00;
            angular.forEach(childProduct.premiumAddonsGross, function (addon) {
                if (!parseFloat(addon.Value)) addon.Value = 0.00;
                gross += parseFloat(addon.Value) ? parseFloat(addon.Value) : 0.00;
            });
            childProduct.grossPremium = parseFloat(gross).toFixed(2);
        }

        $scope.getAllVariantsRelatedToSelectedModels = function (childProduct) {
            if (typeof childProduct.availableVariantsDrp !== 'undefined')
                childProduct.availableVariantsDrp = [];
            var selectedModelIds = [];
            angular.forEach(childProduct.selectedModelsList, function (value) {
                selectedModelIds.push(value.id);
            });
            if (selectedModelIds.length > 0) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllVariantsByModelIds',
                    data: JSON.stringify({
                        'data': selectedModelIds
                    }),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //console.log(data);
                    var variantData = [];
                    angular.forEach(data, function (value) {
                        var x = {
                            "id": value.Id,
                            "label": value.VariantName
                        };
                        variantData.push(x);
                    });
                    childProduct.availableVariantsDrp = variantData;

                }).error(function (data, status, headers, config) {
                });
            } else {

            }
        }


        $scope.setPremiumBasedOnValue = function (childProduct, type) {
            for (var i = 0; i < $scope.premiumBasedOnValues.length; i++) {
                if (type === 'NETT') {
                    if ($scope.premiumBasedOnValues[i].Id === childProduct.premiumBasedOnIdNett) {
                        if ($scope.premiumBasedOnValues[i].Code === 'RP') {
                            childProduct.IsMinMaxVisibleNett = true;
                            childProduct.percentageVisibleNett = true;
                        } else {
                            childProduct.percentageVisibleNett = false;
                            childProduct.IsMinMaxVisibleNett = false;

                        }
                        if ($scope.premiumBasedOnValues[i].Code === 'UE') {
                            childProduct.isCustAvailableNett = false;
                        } else {
                            childProduct.isCustAvailableNett = true;
                        }
                    }
                } else {
                    for (var i = 0; i < $scope.premiumBasedOnValues.length; i++) {
                        if ($scope.premiumBasedOnValues[i].Id === childProduct.premiumBasedOnIdGross) {
                            if ($scope.premiumBasedOnValues[i].Code === 'RP') {
                                childProduct.IsMinMaxVisibleGross = true;
                                childProduct.percentageVisibleGross = true;
                            } else {
                                childProduct.percentageVisibleGross = false;
                                childProduct.IsMinMaxVisibleGross = false;

                            }

                            if ($scope.premiumBasedOnValues[i].Code === 'UE') {
                                childProduct.isCustAvailableGross = false;
                            } else {
                                childProduct.isCustAvailableGross = true;
                            }
                        }
                    }
                }
            }
        }


        $scope.selectedModelChanged = function (childProduct) {
            $scope.getAllVariantsRelatedToSelectedModels(childProduct);
        }
        $scope.selectedModelDeselected = function (childProduct) {
            childProduct.isAllModelsSelected = false;
            $scope.selectedModelChanged(childProduct);
        }
        $scope.allModelsSelected = function (childProduct) {
            childProduct.isAllModelsSelected = true;
        }
        $scope.allModelsDeselected = function (childProduct) {
            childProduct.isAllModelsSelected = false;
        }


        $scope.selectedVariantDeselected = function (childProduct) {
            childProduct.isAllVariantSelected = false;
        }
        $scope.allVariantSelected = function (childProduct) {
            childProduct.isAllVariantSelected = true;
        }
        $scope.allVariantDeselected = function (childProduct) {
            childProduct.isAllVariantSelected = false;
        }


        $scope.cyllindrCountDeselected = function (childProduct) {
            childProduct.isAllCyllinderCountSelected = false;
        }
        $scope.allCyllindrCountSelected = function (childProduct) {
            childProduct.isAllCyllinderCountSelected = true;
        }
        $scope.allCyllindrCountDeselected = function (childProduct) {
            childProduct.isAllCyllinderCountSelected = false;
        }


        $scope.selectedEngineCapacityDeselect = function (childProduct) {
            childProduct.isAllEngineCapacitySelected = false;
        }
        $scope.allEngineCapacitySelected = function (childProduct) {
            childProduct.isAllEngineCapacitySelected = true;
        }
        $scope.allEngineCapacityDeselected = function (childProduct) {
            childProduct.isAllEngineCapacitySelected = false;
        }



        $scope.refreshMakeswithUI = function () {
            $scope.availableMakesDrp = [];
            angular.forEach($scope.availableMakes, function (value) {
                var x = {
                    "id": value.Id,
                    "label": value.MakeName,
                };
                $scope.availableMakesDrp.push(x);
            });
        }


        $scope.refreshCCwithUI = function () {
            $scope.engineCapacitiesDrp = [];
            angular.forEach($scope.engineCapacities, function (value) {
                var x = {
                    "id": value.Id,
                    "label": value.EngineCapacityNumber + ' ' + value.MesureType
                };
                $scope.engineCapacitiesDrp.push(x);
            });
        }

        $scope.refreshWWwithUI = function () {
            $scope.grossWeightDrp = [];
            angular.forEach($scope.grossVehicleWeights, function (value) {
                var x = {
                    "id": value.Id,
                    "label": value.VehicleWeightDescription
                };
                $scope.grossWeightDrp.push(x);
            });
        }


        $scope.refreshVariantswithUI = function () {
            $scope.availableVariantsDrp = [];

        }


        $scope.refreshCyllinderCountwithUI = function () {
            $scope.clinderCountsDrp = [];
            angular.forEach($scope.clinderCounts, function (value) {
                var x = {
                    "id": value.Id,
                    "label": value.Count
                };
                $scope.clinderCountsDrp.push(x);
            });
        }



        $scope.addNewEligibility = function () {
            if ($scope.IsAutomobile) {

                if (
                    parseInt($scope.eligibility.ageMin) >= 0
                    &&
                    parseInt($scope.eligibility.ageMax) >= 0
                    &&
                    parseInt($scope.eligibility.milageMin) >= 0
                    &&
                    parseInt($scope.eligibility.milageMax) >= 0
                    &&
                    $scope.eligibility.plusMinus !== ""

                ) {

                    if ((parseInt($scope.eligibility.ageMin) > parseInt($scope.eligibility.ageMax))
                        ||
                        (parseInt($scope.eligibility.milageMin) > parseInt($scope.eligibility.milageMax))) {
                        $scope.eligibilityError = "Minimum values cannot exceed maximum values";
                    } else {
                        if ($scope.eligibility.isPresentage) {
                            if ($scope.eligibility.premium > 100) {
                                $scope.eligibilityError = "Percentage value cannot exceed 100.";
                            } else {
                                $scope.eligibilityError = "";
                                $scope.currentContract.eligibilities.push($scope.eligibility);
                                $scope.resetCurrentEligibility();
                            }

                        } else {
                            $scope.eligibilityError = "";
                            $scope.currentContract.eligibilities.push($scope.eligibility);
                            $scope.resetCurrentEligibility();
                        }
                    }
                } else {
                    $scope.eligibilityError = "Please fill all fields with valid values";

                }
            } else {
                if (
                    parseInt($scope.eligibility.monthsMin) >= 0
                    &&
                    parseInt($scope.eligibility.monthsMax) >= 0
                    &&
                    $scope.eligibility.plusMinus !== ""

                ) {
                    if (parseInt($scope.eligibility.monthsMin) > parseInt($scope.eligibility.monthsMax)) {
                        $scope.eligibilityError = "Minimum values cannot exceed maximum values";
                    } else {
                        if ($scope.eligibility.isPresentage) {
                            if ($scope.eligibility.premium > 100) {
                                $scope.eligibilityError = "Percentage value cannot exceed 100.";
                            } else {
                                $scope.eligibilityError = "";
                                $scope.currentContract.eligibilities.push($scope.eligibility);
                                $scope.resetCurrentEligibility();
                            }
                        } else {
                            $scope.eligibilityError = "";
                            $scope.currentContract.eligibilities.push($scope.eligibility);
                            $scope.resetCurrentEligibility();
                        }
                    }
                } else {
                    $scope.eligibilityError = "Please fill all fields with valid values";
                }
            }
        }
        $scope.resetCurrentEligibility = function () {
            $scope.eligibility = {
                ageMin: "",
                ageMax: "",
                milageMin: "",
                milageMax: "",
                monthsMin: "",
                monthsMax: "",
                isPresentage: false,
                plusMinus: "",
                premium: 0.00,
                isMandatory: false
            }
        }
        $scope.deleteEligibility = function (id) {
            $scope.currentContract.eligibilities.splice(id, 1);

        }
        $scope.selectedCommodityCategoryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetMakesByCommodityCategoryId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.currentContract.commodityCategoryId }
            }).success(function (data, status, headers, config) {
                $scope.availableMakes = data;
                $scope.refreshMakeswithUI();
                if (!isGuid($scope.contractIdToUpdate)) {
                    $scope.availableModels = [];
                    $scope.selectedMakeList = [];
                    $scope.selectedModelsList = [];
                    //$scope.refreshModelswithUI();
                } else {
                    for (var i = 0; i < $scope.selectedMakeList.length; i++) {
                        var isExist = false;
                        for (var j = 0; j < $scope.availableMakes.length; j++) {
                            if ($scope.selectedMakeList[i].id === $scope.availableMakes[j].Id) {
                                isExist = true;
                                return false;
                            }
                        }
                        if (!isExist) {
                            $scope.selectedMakeList.splice(i, 1);
                        }
                    }
                }
            }).error(function (data, status, headers, config) {
            });

        }


        $scope.savePremiumSetup = function () {
            if (!isGuid($scope.contractIdToUpdate)) {
                //add new premium
                if (!$scope.validateGeneralDetails(false) &&
                    !$scope.validateProductDetails(false)) {

                    $scope.saveBtnDisabled = true;
                    $scope.saveBtnClass = "fa fa-spinner fa-spin";
                    $scope.currentContract.texes = $scope.countryTaxes;
                    $scope.currentContract.commissions = $scope.commissionTypes;
                    $scope.currentContract.netPremium = $scope.totalNRP;
                    $scope.currentContract.grossPremium = $scope.grossPremium;
                    $scope.currentContract.premiumCurrencyId = $scope.premiumCurrencyId;
                    $scope.currentContract.annualPremium = $scope.annualPremium;
                    $scope.currentContract.loggedInUserId = $localStorage.LoggedInUserId;
                    var data = {
                        contract: $scope.currentContract,
                        products: $scope.allChildProducts
                    };
                    //good to go
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/AddNewContract',
                        data: JSON.stringify({ 'data': data }),
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        // $scope.attributeSpecifications = data;
                        if (data === "Success") {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.setupNewContract();

                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        $scope.saveBtnDisabled = false;
                        $scope.saveBtnClass = "";
                    }).error(function (data, status, headers, config) {
                        $scope.saveBtnDisabled = false;
                        $scope.saveBtnClass = "";
                    });
                } else {
                    //customErrorMessage("Please fill all highlighted fields.");
                }
                //return false;
            } else {
                //update premium

                if (!$scope.validateGeneralDetails(true) &&
                    !$scope.validateProductDetails(true)) {

                    $scope.saveBtnDisabled = true;
                    $scope.saveBtnClass = "fa fa-spinner fa-spin";
                    $scope.currentContract.texes = $scope.countryTaxes;
                    $scope.currentContract.commissions = $scope.commissionTypes;
                    $scope.currentContract.netPremium = $scope.totalNRP;
                    $scope.currentContract.grossPremium = $scope.grossPremium;
                    $scope.currentContract.premiumCurrencyId = $scope.premiumCurrencyId;
                    $scope.currentContract.annualPremium = $scope.annualPremium;
                    $scope.currentContract.loggedInUserId = $localStorage.LoggedInUserId;

                    var data = {
                        contract: $scope.currentContract,
                        products: $scope.allChildProducts
                    };

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ContractManagement/UpdateContract',
                        data: JSON.stringify({ 'data': data, 'contractId': $scope.contractIdToUpdate }),
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data === "ok") {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.setupNewContract();
                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        $scope.saveBtnDisabled = false;
                        $scope.saveBtnClass = "";
                    }).error(function (data, status, headers, config) {
                    });
                }
            }
        }
        $scope.validateGeneralDetails = function (isUpdate) {
            var haveErrorInputs = false;
            if (!isGuid($scope.currentContract.countryId)) {
                $scope.validate_country = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");
            } else { $scope.validate_country = ""; }

            if (!isGuid($scope.currentContract.dealerId)) {
                $scope.validate_dealer = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_dealer = ""; }

            if (!isGuid($scope.currentContract.commodityTypeId)) {
                $scope.validate_commodityType = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_commodityType = ""; }

            if (!isGuid($scope.currentContract.commodityCategoryId)) {
                $scope.validate_commodityCategory = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_commodityCategory = ""; }


            if (!isGuid($scope.currentContract.productId)) {
                $scope.validate_product = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_product = ""; }

            if (!isGuid($scope.currentContract.reinsurerContractId)) {
                $scope.validate_ReinsurerContract = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_ReinsurerContract = ""; }

            if (!isGuid($scope.currentContract.commodityUsageTypeId)) {
                $scope.validate_commodityUsageType = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_commodityUsageType = ""; }

            if (!isGuid($scope.currentContract.dealType)) {
                $scope.validate_dealType = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_dealType = ""; }


            if ($scope.currentContract.dealName === "") {
                $scope.validate_dealName = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else {
                if (!isUpdate) {
                    $scope.validate_dealName = "";
                    $scope.dealNameError = "";
                    for (var i = 0; i < $scope.linkDeals.length; i++) {
                        if ($scope.linkDeals[i].DealName === $scope.currentContract.dealName) {
                            $scope.validate_dealName = "has-error";
                            $scope.dealNameError = " already exist";
                            haveErrorInputs = true;
                            return true;
                            customErrorMessage("Please fill all highlighted fields.");
                        }
                    }
                } else {
                    if ($scope.temp_dealName !== $scope.currentContract.dealName) {
                        for (var i = 0; i < $scope.linkDeals.length; i++) {
                            if ($scope.linkDeals[i].DealName === $scope.currentContract.dealName) {
                                $scope.validate_dealName = "has-error";
                                $scope.dealNameError = " already exist";
                                haveErrorInputs = true;
                                return true;
                                customErrorMessage("Please fill all highlighted fields.");
                            }
                        }
                    } else {
                        $scope.validate_dealName = "";
                        $scope.dealNameError = "";
                    }
                }
            }

            if (!isGuid($scope.currentContract.dealType)) {
                $scope.validate_dealType = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_dealType = ""; }

            if (!isGuid($scope.currentContract.insurerId)) {
                $scope.validate_Insurer = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_Insurer = ""; }

            if (!isGuid($scope.currentContract.reinsurerContractId)) {
                $scope.validate_ReinsurerContract = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_ReinsurerContract = ""; }

            if (!IsDate($scope.currentContract.contractStartDate)) {
                $scope.validate_startDate = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_startDate = ""; }


            if (!IsDate($scope.currentContract.contractEndDate)) {
                $scope.validate_endDate = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_endDate = ""; }

            if (IsDate($scope.currentContract.contractEndDate) && IsDate($scope.currentContract.contractStartDate)) {
                if ((new Date($scope.currentContract.contractStartDate).getTime() > new Date($scope.currentContract.contractEndDate).getTime())) {
                    $scope.validate_endDate = "has-error";
                    $scope.validate_startDate = "has-error";
                    $scope.dateValidationError = "Start date should be less than end date.";
                    haveErrorInputs = true;
                    customErrorMessage("Please fill all highlighted fields.");
                } else {
                    $scope.dateValidationError = "";
                    $scope.validate_endDate = "";
                    $scope.validate_startDate = "";
                }
            }

            //if (!isGuid($scope.currentContract.itemStatusId)) {
            //    $scope.validate_itemStatus = "has-error";
            //    haveErrorInputs = true;

            //} else { $scope.validate_itemStatus = ""; }

            if (!isGuid($scope.currentContract.commodityUsageTypeId)) {
                $scope.validate_commodityUsageType = "has-error";
                haveErrorInputs = true;
                customErrorMessage("Please fill all highlighted fields.");

            } else { $scope.validate_commodityUsageType = ""; }
            return haveErrorInputs;
        }

        $scope.validateProductDetails = function (isUpdate) {
            var haveErrorInputs = false;
            if (!isUpdate) {
                angular.forEach($scope.allChildProducts, function (child) {

                    if (child.Productcode !== 'RSA') {
                        if (!isGuid(child.InsuaranceLimitationId)) {
                            haveErrorInputs = true;
                            child.validate_extentionType = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_extentionType = "";
                        }
                        if (!isGuid(child.attributeSpecificationId)) {
                            if (child.attributeSpecificationName === "") {
                                haveErrorInputs = true;
                                child.validate_specificationName = "has-error";
                                customErrorMessage("Please fill all highlighted fields.");
                            } else {
                                child.validate_specificationName = "";
                            }
                        }

                        if (!isGuid(child.warrantyTypeId)) {
                            haveErrorInputs = true;
                            child.validate_warrentyType = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_warrentyType = "";
                        }

                        if (!isGuid(child.itemStatusId)) {
                            haveErrorInputs = true;
                            child.validate_itemStatus = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_itemStatus = "";
                        }

                        if (child.selectedMakeList.length === 0) {
                            haveErrorInputs = true;
                            child.validate_makes = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_makes = "";
                        }

                        if (child.selectedModelsList.length === 0) {
                            haveErrorInputs = true;
                            child.validate_models = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_models = "";
                        }
                        if (child.selectedVariantList.length === 0) {
                            haveErrorInputs = true;
                            child.validate_variant = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_variant = "";
                        }
                        //if (child.selectedClinderCounts.length === 0) {
                        //    haveErrorInputs = true;
                        //    child.validate_cyllinderCount = "has-error";
                        //    customErrorMessage("Please fill all highlighted fields.");
                        //} else {
                        //    child.validate_cyllinderCount = "";
                        //}
                    } else {
                        if (!isGuid(child.rsaProviderId)) {
                            haveErrorInputs = true;
                            child.validate_rsaProvider = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_rsaProvider = "";
                        }

                        if (!isGuid(child.regionId)) {
                            haveErrorInputs = true;
                            child.validate_regionId = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_regionId = "";
                        }

                    }

                    if (!isGuid(child.premiumBasedOnIdNett)) {
                        haveErrorInputs = true;
                        child.validate_premiumBasedOnNett = "has-error";
                        customErrorMessage("Please fill all highlighted fields.");
                    } else {
                        child.validate_premiumBasedOnNett = "";
                    }

                    if (child.IsMinMaxVisibleNett) {
                        if (!parseFloat(child.minValueNett)) {
                            haveErrorInputs = true;
                            child.minValueNett = 0.00;
                            child.validate_minValueNett = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_minValueNett = "";
                        }

                        if (!parseFloat(child.maxValueNett)) {
                            haveErrorInputs = true;
                            child.maxValueNett = 0.00;
                            child.validate_maxValueNett = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_maxValueNett = "";
                        }

                        if (parseFloat(child.maxValueNett) < parseFloat(child.minValueNett)) {
                            haveErrorInputs = true;
                            child.validate_maxValueNett = "has-error";
                            child.validate_minValueNett = "has-error";
                            child.minMaxNettError = "Min value cannot be exceed max value.";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_maxValueNett = "";
                            child.validate_minValueNett = "";
                            child.minMaxNettError = "";
                        }

                        if (!parseFloat(child.totalNRP)) {
                            customErrorMessage("NRP cannot be zero");
                            haveErrorInputs = true;
                        }
                    }
                    if ($scope.ValidategrossandnrpValues()) {

                        haveErrorInputs = true;
                        customErrorMessage("Please Mark Commossion types as Gross or NRP");

                    }

                    if (child.IsMinMaxVisibleGross) {
                        if (!parseFloat(child.minValueGross)) {
                            haveErrorInputs = true;
                            child.minValueGross = 0.00;
                            child.validate_minValueGross = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_minValueGross = "";
                        }

                        if (!parseFloat(child.maxValueGross)) {
                            haveErrorInputs = true;
                            child.maxValueGross = 0.00;
                            child.validate_maxValueGross = "has-error";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_maxValueGross = "";
                        }

                        if (parseFloat(child.maxValueGross) < parseFloat(child.minValueGross)) {
                            haveErrorInputs = true;
                            child.validate_maxValueGross = "has-error";
                            child.validate_minValueGross = "has-error";
                            child.minMaxGrossError = "Min value cannot be exceed max value.";
                            customErrorMessage("Please fill all highlighted fields.");
                        } else {
                            child.validate_maxValueGross = "";
                            child.validate_minValueGross = "";
                            child.minMaxGrossError = "";
                        }

                        if (!parseFloat(child.grossPremium)) {
                            customErrorMessage("Gross Premium cannot be zero");
                            haveErrorInputs = true;
                        }
                    }

                });
                return haveErrorInputs;
            } else {

            }
        }


        $scope.validateNRPDetails = function () {
            var haveErrorInputs = false;
            if ($scope.IsAutomobile) {
                if ($scope.currentProductType === "RSA") {
                    if ($scope.currentContract.InsuaranceLimitationId === "00000000-0000-0000-0000-000000000000") {
                        $scope.validate_extentionType = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_extentionType = ""; }

                    if (!isGuid($scope.currentContract.rsaProviderId)) {
                        $scope.validate_rsaProvider = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_rsaProvider = ""; }

                    if (!isGuid($scope.currentContract.regionId)) {
                        $scope.validate_regionId = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_regionId = ""; }

                } else {
                    if ($scope.currentContract.InsuaranceLimitationId == "00000000-0000-0000-0000-000000000000") {
                        $scope.validate_extentionType = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_extentionType = ""; }

                    if ($scope.currentContract.attributeSpecificationName == "") {
                        $scope.validate_specificationName = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_specificationName = ""; }
                    if (!isGuid($scope.currentContract.warrantyTypeId)) {
                        $scope.validate_warrentyType = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_warrentyType = ""; }

                    if ($scope.selectedMakeList.length == 0) {
                        $scope.validate_makes = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_makes = ""; }

                    if ($scope.selectedModelsList.length == 0) {
                        $scope.validate_models = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_models = ""; }

                    if ($scope.selectedClinderCounts.length == 0) {
                        $scope.validate_cyllinderCount = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_cyllinderCount = ""; }

                    if ($scope.selectedEngineCapacities.length == 0) {
                        $scope.validate_engineCapacities = "has-error";
                        haveErrorInputs = true;
                    } else { $scope.validate_engineCapacities = ""; }

                }
            } else {
                if ($scope.currentContract.InsuaranceLimitationId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_extentionType = "has-error";
                    haveErrorInputs = true;
                } else { $scope.validate_extentionType = ""; }

                if ($scope.currentContract.attributeSpecificationName == "") {
                    $scope.validate_specificationName = "has-error";
                    haveErrorInputs = true;
                } else { $scope.validate_specificationName = ""; }
                if (!isGuid($scope.currentContract.warrantyTypeId)) {
                    $scope.validate_warrentyType = "has-error";
                    haveErrorInputs = true;
                } else { $scope.validate_warrentyType = ""; }

                if ($scope.selectedMakeList.length == 0) {
                    $scope.validate_makes = "has-error";
                    haveErrorInputs = true;
                } else { $scope.validate_makes = ""; }

                if ($scope.selectedModelsList.length == 0) {
                    $scope.validate_models = "has-error";
                    haveErrorInputs = true;
                } else { $scope.validate_models = ""; }

            }
            return haveErrorInputs;
        }

        $scope.setCommissionIsNRPOrGross = function (commissionName, isNrp) {
            angular.forEach($scope.commissionTypes, function (value) {
                if (value.Name === commissionName) {

                    if (value.IsOnNRP && value.IsOnGROSS) {
                        if (isNrp) {
                            value.IsOnGROSS = false;
                            value.IsOnNRP = true;
                        } else {
                            value.IsOnGROSS = true;
                            value.IsOnNRP = false;

                        }
                    } else if (!value.IsOnNRP && !value.IsOnGROSS) {
                        if (isNrp) {
                            value.IsOnNRP = false;
                            value.IsOnGROSS = true;
                        } else {
                            value.IsOnGROSS = false;
                            value.IsOnNRP = true;
                        }
                    }

                }

            });

        }

        $scope.setMilageType = function () {

        }
        $scope.validatePremiumDetails = function () {
            var haveErrorInputs = false;
            if (!isGuid($scope.currentContract.premiumBasedOnIdGross)) {
                $scope.validate_premiumBasedOnGross = "has-error";
                haveErrorInputs = true;
            } else { $scope.validate_premiumBasedOnGross = ""; }


            if (!isGuid($scope.currentContract.premiumBasedOnIdNett)) {
                $scope.validate_premiumBasedOnNett = "has-error";
                haveErrorInputs = true;
            } else { $scope.validate_premiumBasedOnNett = ""; }
            //alert($scope.currentContract.minValueNett);
            if ($scope.IsMinMaxVisibleNett) {
                // alert(parseFloat($scope.currentContract.minValueNett));
                if ($scope.currentContract.minValueNett != 0) {
                    if (!parseFloat($scope.currentContract.minValueNett)) {
                        $scope.validate_minValueNett = "has-error";
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_minValueNett = "";
                    }
                }
                if ($scope.currentContract.maxValueNett != 0) {
                    if (!parseFloat($scope.currentContract.maxValueNett)) {

                        $scope.validate_maxValueNett = "has-error";
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_maxValueNett = "";
                    }
                }
                if (!haveErrorInputs) {
                    if (parseFloat($scope.currentContract.minValueNett) > parseFloat($scope.currentContract.maxValueNett)) {
                        $scope.validate_minValueNett = "has-error";
                        $scope.validate_maxValueNett = "has-error";
                        $scope.minMaxNettError = "Minimum value cannot exceed the Maximum value"
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_minValueNett = "";
                        $scope.validate_maxValueNett = "";
                        $scope.minMaxNettError = "";
                    }
                }

            }
            //alert('here1' + haveErrorInputs);

            if ($scope.IsMinMaxVisibleGross) {
                if ($scope.currentContract.minValueGross != 0) {
                    if (!parseFloat($scope.currentContract.minValueGross)) {
                        $scope.validate_minValueGross = "has-error";
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_minValueGross = "";
                    }
                }
                if ($scope.currentContract.maxValueGross != 0) {
                    if (!parseFloat($scope.currentContract.maxValueGross)) {
                        $scope.validate_maxValueGross = "has-error";
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_maxValueGross = "";
                    }
                }
                if (!haveErrorInputs) {
                    if (parseFloat($scope.currentContract.minValueGross) > parseFloat($scope.currentContract.maxValueGross)) {
                        $scope.validate_minValueGross = "has-error";
                        $scope.validate_maxValueGross = "has-error";
                        $scope.minMaxGrossError = "Minimum value cannot exceed the Maximum value"
                        haveErrorInputs = true;
                    } else {
                        $scope.validate_minValueGross = "";
                        $scope.validate_maxValueGross = "";
                        $scope.minMaxGrossError = "";
                    }
                }

            }
            //alert('here2' + haveErrorInputs);

            if (parseFloat($scope.totalNRP) > 0 && parseFloat($scope.totalNRP) > 0) {
                $scope.GeneralError = "";
            } else {
                $scope.GeneralError = "Premium values cannot be zero";
                haveErrorInputs = true;
            }

            if (parseFloat($scope.grossPremium) && parseFloat($scope.grossPremium) > 0) {
                $scope.GeneralError = "";
            } else {
                $scope.GeneralError = "Premium values cannot be zero";
                haveErrorInputs = true;
            }
            return haveErrorInputs;
        }
        function IsDate(s) {
            if (s == '') {
                return false;
            }
            var d = new Date(s);
            // alert(d);
            return d;
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };
        var customWarningMessage = function (msg) {
            toaster.pop('warning', 'Warning', msg, 12000);
        };

        $scope.resetButton = function () {
            $scope.saveBtnDisabled = false;
        }
        //$scope.setupNewContract = function () {

        //        $scope.contractSearchGridloadAttempted = false;
        //        $scope.contractSearchGridloading = false;
        //        $scope.formAction = true;
        //        $scope.IsAutomobile = false;
        //        $scope.IsYellowGood = false;
        //        $scope.visitedInitially = false;
        //        $scope.IsOther = false;
        //        $scope.IsMinMaxVisibleGross = false;
        //        $scope.IsMinMaxVisibleNett = false;
        //        $scope.premiumDetailsHideGross = true;
        //        $scope.premiumDetailsHideNett = true;
        //        $scope.formActionText = "Setting up new contract";
        //        $scope.setNewInsuaranceLimitationMsg = "";
        //        $scope.eligibilityError = "";
        //        $scope.totalNRP = 0.00;
        //        $scope.grossPremium = 0.00;

        //        $scope.contractIdToUpdate = emptyGuid();
        //        $scope.contractSearchGridSearchCriterias = {
        //            countryId: emptyGuid(),
        //            dealerId: emptyGuid(),
        //            productId: emptyGuid(),
        //            dealName: ""
        //        };
        //        //$scope.countries = [];
        //        //$scope.Allcountries = [];
        //        $scope.dealers = [];
        //        $scope.dealersByCountry = [];
        //        $scope.products = [];
        //        $scope.commodityProducts = [];
        //        $scope.deals = [];
        //        //  $scope.commodityTypes = [];
        //        $scope.linkDeals = [];
        //        // $scope.dealTypes = [];
        //        //$scope.insurers = [];
        //        $scope.reinsurerContracts = [];
        //        $scope.allReinsurerContracts = [];
        //        // $scope.itemStatuses = [];
        //        //  $scope.commodityUsageTypes = [];
        //        $scope.insuaranceLimitations = [];
        //        // $scope.rsaProviders = [];
        //        $scope.currentProductType = "";
        //        $scope.newInsuaranceLimitation = {
        //            km: "",
        //            month: "",
        //            hours: "",
        //            unlimitedcheck: false,
        //            upto: true
        //        }
        //        $scope.attributeSpecifications = [];
        //        $scope.warrantyTypes = [];
        //        $scope.selectedMakeList = [];
        //        $scope.availableMakes = [];
        //        $scope.availableMakesDrp = [];//for drop down
        //        $scope.selectedModelsList = [];
        //        $scope.availableModels = [];
        //        $scope.availableModelsDrp = [];//for drop down
        //        $scope.clinderCounts = [];
        //        $scope.clinderCountsDrp = [];//for drop down
        //        $scope.selectedClinderCounts = [];
        //        $scope.engineCapacitiesDrp = [];//for drop down
        //        $scope.availableVariantsDrp = [];//for drop down
        //        $scope.selectedVariantList = [];
        //        $scope.availableVariants = [];
        //        $scope.engineCapacities = [];

        //        $scope.grossVehicleWeights = [];
        //        $scope.grossWeightDrp = [];
        //        $scope.selectedGrossWeights = [];

        //        $scope.selectedEngineCapacities = [];
        //        $scope.premiumCurrency = "";
        //        $scope.premiumCurrencyId = emptyGuid();
        //        // $scope.premiumBasedOnValues = [];
        //        $scope.premiumAddons = [];
        //        // $scope.regions = [];
        //        $scope.annualPremium = [];
        //        //$scope.commissionTypes = [];
        //        $scope.countryTaxes = [];
        //        $scope.commodityCategories = [];
        //        $scope.premiumAddonsGross = {};
        //        $scope.premiumAddonsNett = {};
        //        $scope.eligibility = {
        //            ageMin: 0.00,
        //            ageMax: 0.00,
        //            milageMin: 0.00,
        //            milageMax: 0.00,
        //            monthsMin: 0.00,
        //            monthsMax: 0.00,
        //            isPresentage: false,
        //            plusMinus: 0.00,
        //            premium: 0.00
        //        }
        //        $scope.currentContract = {
        //            countryId: emptyGuid(),
        //            dealerId: emptyGuid(),
        //            commodityTypeId: emptyGuid(),
        //            productId: emptyGuid(),
        //            linkDealId: emptyGuid(),
        //            dealName: "",
        //            dealType: emptyGuid(),
        //            isPromotional: false,
        //            discountAvailable: false,
        //            insurerId: emptyGuid(),
        //            reinsurerContractId: emptyGuid(),
        //            contractStartDate: "",
        //            contractEndDate: "",
        //            remark: "",
        //            isAutoRenewal: false,
        //            isActive: false,
        //            itemStatusId: emptyGuid(),
        //            commodityUsageTypeId: emptyGuid(),
        //            claimLimitation: 0.00,
        //            liabilityLimitation: 0.00,
        //            InsuaranceLimitationId: emptyGuid(),
        //            attributeSpecificationId: emptyGuid(),
        //            attributeSpecificationPrefix: "",
        //            attributeSpecificationName: "",
        //            warrantyTypeId: emptyGuid(),
        //            premiumBasedOnIdGross: emptyGuid(),
        //            premiumBasedOnIdNett: emptyGuid(),
        //            isCustAvailableGross: true,
        //            isCustAvailableNett: true,
        //            manufacturerWarrantyGross: false,
        //            manufacturerWarrantyNett: false,
        //            rsaProviderId: emptyGuid(),
        //            regionId: emptyGuid(),
        //            commodityCategoryId: emptyGuid(),
        //            minValueGross: '',
        //            maxValueGross: '',
        //            minValueNett: '',
        //            maxValueNett: '',
        //            annualPremiumTotal: 0.00,
        //            premiumCurrencyId: emptyGuid(),
        //            eligibilities: [],
        //            texes: [],
        //            commissions: [],
        //            annualInterestRate: 0.00
        //        }

        //        $scope.isAllMakesSelected = false;
        //        $scope.isAllModelsSelected = false;
        //        $scope.isAllVariantSelected = false;
        //        $scope.isAllCyllinderCountSelected = false;
        //        $scope.isAllEngineCapacitySelected = false;
        //        $scope.childProductsData_temp = [];

        //    }

        $scope.ResetPremiumSearchPopup = function () {
            $scope.contractSearchGridSearchCriterias.countryId = emptyGuid();
            $scope.contractSearchGridSearchCriterias.dealName = '';
            $scope.contractSearchGridSearchCriterias.dealerId = emptyGuid();
            $scope.contractSearchGridSearchCriterias.productId = emptyGuid();
            $scope.RefreshContractSearchGridData();
        }

        $scope.setupNewContract = function () {
            $scope.contractSearchGridloadAttempted = false;
            $scope.childProductsData_temp = [];
            $scope.visitedInitially = false;
            $scope.contractSearchGridloading = false;
            $scope.formAction = true;
            $scope.IsAutomobile = false;
            $scope.IsYellowGood = false;
            $scope.IsOther = false;
            $scope.IsMinMaxVisibleGross = false;
            $scope.IsMinMaxVisibleNett = false;
            $scope.premiumDetailsHideGross = true;
            $scope.premiumDetailsHideNett = true;
            $scope.formActionText = "Setting up new contract";
            $scope.setNewInsuaranceLimitationMsg = "";
            $scope.eligibilityError = "";
            $scope.totalNRP = 0.00;
            $scope.grossPremium = 0.00;

            $scope.contractIdToUpdate = emptyGuid();
            $scope.contractSearchGridSearchCriterias = {
                countryId: emptyGuid(),
                dealerId: emptyGuid(),
                productId: emptyGuid(),
                dealName: ""
            };
            $scope.countries = [];
            $scope.Allcountries = [];
            $scope.dealers = [];
            $scope.dealersByCountry = [];
            $scope.products = [];
            $scope.commodityProducts = [];
            $scope.deals = [];
            $scope.commodityTypes = [];
            $scope.linkDeals = [];
            $scope.dealTypes = [];
            $scope.insurers = [];
            $scope.reinsurerContracts = [];
            $scope.allReinsurerContracts = [];
            $scope.itemStatuses = [];
            $scope.commodityUsageTypes = [];
            $scope.insuaranceLimitations = [];
            $scope.rsaProviders = [];
            $scope.currentProductType = "";
            $scope.SelectedContractError = "";
            $scope.newInsuaranceLimitation = {
                km: "",
                month: "",
                hours: ""
            }
            $scope.attributeSpecifications = [];
            $scope.warrantyTypes = [];
            $scope.selectedMakeList = [];
            $scope.availableMakes = [];
            $scope.availableMakesDrp = [];//for drop down
            $scope.selectedModelsList = [];
            $scope.availableModels = [];
            $scope.availableModelsDrp = [];//for drop down
            $scope.availableVariantsDrp = [];//for drop down
            $scope.selectedVariantList = [];
            $scope.availableVariants = [];
            $scope.clinderCounts = [];
            $scope.clinderCountsDrp = [];//for drop down
            $scope.selectedClinderCounts = [];
            $scope.engineCapacitiesDrp = [];//for drop down
            $scope.engineCapacities = [];
            $scope.selectedEngineCapacities = [];
            $scope.premiumCurrency = "";
            $scope.premiumCurrencyId = emptyGuid();
            $scope.premiumBasedOnValues = [];
            $scope.premiumAddons = [];
            $scope.regions = [];
            $scope.annualPremium = [];
            $scope.commissionTypes = [];
            $scope.countryTaxes = [];
            $scope.commodityCategories = [];
            $scope.premiumAddonsGross = {};
            $scope.premiumAddonsNett = {};
            $scope.eligibility = {
                ageMin: 0.00,
                ageMax: 0.00,
                milageMin: 0.00,
                milageMax: 0.00,
                monthsMin: 0.00,
                monthsMax: 0.00,
                isPresentage: false,
                plusMinus: 0.00,
                premium: 0.00
            }
            $scope.currentContract = {
                countryId: emptyGuid(),
                dealerId: emptyGuid(),
                commodityTypeId: emptyGuid(),
                productId: emptyGuid(),
                linkDealId: emptyGuid(),
                dealName: "",
                dealType: emptyGuid(),
                isPromotional: false,
                discountAvailable: false,
                insurerId: emptyGuid(),
                reinsurerContractId: emptyGuid(),
                contractStartDate: "",
                contractEndDate: "",
                remark: "",
                isAutoRenewal: false,
                isActive: false,
                itemStatusId: emptyGuid(),
                commodityUsageTypeId: emptyGuid(),
                claimLimitation: 0.00,
                liabilityLimitation: 0.00,
                InsuaranceLimitationId: emptyGuid(),
                attributeSpecificationId: emptyGuid(),
                attributeSpecificationPrefix: "",
                attributeSpecificationName: "",
                warrantyTypeId: emptyGuid(),
                premiumBasedOnIdGross: emptyGuid(),
                premiumBasedOnIdNett: emptyGuid(),
                isCustAvailableGross: true,
                isCustAvailableNett: true,
                manufacturerWarrantyGross: false,
                manufacturerWarrantyNett: false,
                rsaProviderId: emptyGuid(),
                regionId: emptyGuid(),
                commodityCategoryId: emptyGuid(),
                minValueGross: '',
                maxValueGross: '',
                minValueNett: '',
                maxValueNett: '',
                annualPremiumTotal: 0.00,
                premiumCurrencyId: emptyGuid(),
                eligibilities: [],
                texes: [],
                commissions: [],
                annualInterestRate: 0.00
            }

            $scope.allChildProducts = [];

            $scope.isAllMakesSelected = false;
            $scope.isAllModelsSelected = false;
            $scope.isAllVariantSelected = false;
            $scope.isAllCyllinderCountSelected = false;
            $scope.isAllEngineCapacitySelected = false;
            $scope.loadInitailData();

        }
    });
