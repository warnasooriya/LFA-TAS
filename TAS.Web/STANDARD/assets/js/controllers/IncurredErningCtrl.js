app.controller('IncurredErningCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster, $translate) {
        $scope.ModalName = "Incurred Earning";
        $scope.ModalDescription = "";
        $scope.FaultCategorySaveBtnIconClass = "";
        $scope.FaultCategorySaveBtnDisabled = false;
        $scope.FaultAreaSaveBtnIconClass = "";
        $scope.FaultAreaSaveBtnDisabled = false;
        $scope.errorTab1 = "";
        $scope.maxClaimDate = new Date();

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };


        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.reinsurers = data;
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
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/IncurredErningManagement/GetUNWYears',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UNWyears = data;
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
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                angular.forEach($scope.CommodityTypes, function (value) {
                    if (value.CommodityTypeDescription == 'Automobile') {
                        $scope.CommodityTypeId = value.CommodityTypeId;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.CommodityTypeId }
                        }).success(function (data, status, headers, config) {
                            $scope.Makes = data;
                        }).error(function (data, status, headers, config) {
                        });

                        return;
                    } else if (value.CommodityTypeDescription == 'Automotive') {
                        $scope.CommodityTypeId = value.CommodityTypeId;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": $scope.CommodityTypeId }
                        }).success(function (data, status, headers, config) {
                            $scope.Makes = data;
                        }).error(function (data, status, headers, config) {
                        });

                        return;
                    }
                });
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
                url: '/TAS.Web/api/AutomobileAttributes/GetAllEngineCapacities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.EngineCapacities = data;
            }).error(function (data, status, headers, config) {
                });

        }

        LoadDetails();
        $scope.reinsurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            ReinsurerCode: "",
            ReinsurerName: ""
        };
        $scope.insurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            InsurerCode: "",
            InsurerShortName: ""
        };
        $scope.Dealer = {
            Id: "00000000-0000-0000-0000-000000000000",
            DealerCode: "",
            DealerName: ""
        };
        $scope.IncurredEarn = {
            ReInsurerId: "00000000-0000-0000-0000-000000000000",
            DealerId: "",
            insurerId: "00000000-0000-0000-0000-000000000000",
            CountryId: "",
            UNWYear: "",
            MakeId: "",
            ModelId: "",
            InsuaranceLimitationId: "",
            MwMonthId:"",
            CylindercountId: "",
            EngineCapacityId:"",
            BordxStartDate: '',
            BordxEndDate: '',
            ErnedDate: '',
            ClaimedDate:''

        };
        $scope.Countries={
            CountryCode: "",
            CountryName: "",
            CurrencyId: "",
            Id: "",
            IsActive: false,
            IsCountryExists: false,
            Makes: null,
            Modeles: null,
            PhoneCode: ""
        };
        $scope.Modeles = {

            AdditionalPremium: false,
            CategoryId: "",
            ContryOfOrigineId: "",
            EntryDateTime: "",
            EntryUser: "",
            Id: "",
            IsActive: false,
            IsModelExists: false,
            MakeId: "",
            ModelCode: "",
            ModelName: "",
            NoOfDaysToRiskStart: 0,
            WarantyGiven: false
        }
        $scope.CylinderCounts={
            Count: "",
            EntryDateTime: "",
            EntryUser: "",
            Id: "",
            IsCylinderCountExists: false

        }
        $scope.MwMonths = {

            ApplicableFrom: "",
            CommodityTypeId: "",
            Countrys: null,
            EntryDateTime: "",
            EntryUser: "",
            Id: "",
            IsManufacturerWarrantyExists: false,
            IsUnlimited: false,
            MakeId: "",
            Models: null,
            WarrantyKm: "",
            WarrantyMonths:"",
            WarrantyName: ""

        }

        $scope.ExtMonths = {
            ExtensionName: "",
            InsuaranceLimitationId: ""
        }
        $scope.EngineCapacities = {

            EngineCapacityNumber: null,
            EntryDateTime: "",
            EntryUser: "",
            Id: "",
            IsEngineCapacityExists: false,
            MesureType: ""
        }
        $scope.claimSearchParam = {
            commodityTypeId: emptyGuid(),
            policyNo: '',
            claimNo: "all",
            vinNo: '',
            claimDealerId: emptyGuid(),
            makeId: emptyGuid(),
            statusId: emptyGuid()
        };

       
        $scope.SetModel = function () {
            $scope.errorTab1 = "";
            if ($scope.IncurredEarn.MakeId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.IncurredEarn.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Modeles = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }
        $scope.SetMwMonths = function () {
            $scope.errorTab1 = "";
            if ($scope.IncurredEarn.MakeId != null) {

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/IncurredErningManagement/GetAllMwMonths',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: $scope.IncurredEarn
                        }).success(function (data, status, headers, config) {

                            $scope.MwMonths = data;
                        }).error(function (data, status, headers, config) {
                        });

            }
        }
        $scope.SetExMonths = function () {
            $scope.errorTab1 = "";
            if ($scope.IncurredEarn.MakeId != null) {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ContractManagement/GetExtensionTypesByMakeModel',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: $scope.IncurredEarn
                }).success(function (data, status, headers, config) {
                    $scope.ExtMonths = data;
                }).error(function (data, status, headers, config) {
                });

            }
        }

        $scope.IncurredErningProcess = function () {
            if ($scope.validateIncuredSelection()) {
                //if ($scope.BordxPayment.Id != null && $scope.BordxPayment.Id != "00000000-0000-0000-0000-000000000000") {
                    $scope.IncurredErningBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.IncurredErningBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/IncurredErningManagement/IncurredErningProcess',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.IncurredEarn
                    }).success(function (data, status, headers, config) {
                        $scope.IncurredProcessData = data;
                        $scope.IncurredErningBtnIconClass = "";
                        $scope.IncurredErningBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.incurredErning.sucessMessages.erningProcess'),
                                text: $filter('translate')('pages.incurredErning.sucessMessages.success'),
                                confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            //$scope.GetBordxNum();
                            //ClearBordxPaymentControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.incurredErning.sucessMessages.erningProcess'),
                            text: $filter('translate')('pages.incurredErning.errorMessages.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.IncurredErningBtnIconClass = "";
                        $scope.IncurredErningBtnDisabled = false;
                        return false;
                    });
                }

            //} else {
            //    customErrorMessage("Please fill valid data for highlighted fields.")
            //}
        }

        var paginationOptionsClaimSearchInquiryGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        var claimSearch = function () {
            $scope.claimSearchGridloading = true;
            $scope.claimSearchGridloadAttempted = false;
            var policySearchGridParam =
            {
                'paginationOptionsClaimSearchInquiryGrid': paginationOptionsClaimSearchInquiryGrid,
                'ClaimSearchInquiryGridSearchCriterias': $scope.ClaimSearchInquiryGridSearchCriterias,

            }
            if ($scope.IncurredEarn.DealerId != null && $scope.IncurredEarn.DealerId!="") {
                $scope.claimSearchParam.claimDealerId = $scope.IncurredEarn.DealerId;
            }

            var search = {
                page: 1,
                pageSize: 10,
                loggedInUserId: $localStorage.LoggedInUserId,
                userType: "iu",
                claimSearch: $scope.claimSearchParam
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllSubmittedClaimsByUserId',
                data: search,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsClaim.data = response_arr.data;
                $scope.gridOptionsClaim.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.claimSearchGridloading = false;
                $scope.claimSearchGridloadAttempted = true;

            });
        };



        $scope.gridOptionsClaim = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                {
                    name: $filter('translate')('pages.incurredErning.approvedAmount'),
                    field: 'ApprovedAmount',
                    enableSorting: false,
                    cellClass: 'columCss'
                },
                {
                    name: $filter('translate')('pages.incurredErning.claimNumber'),
                    field: 'ClaimNumber',
                    enableSorting: false,
                    width: "*",
                    cellClass: 'columCss'
                },
                {
                    name: $filter('translate')('pages.incurredErning.date'),
                    field: 'Date',
                    enableSorting: false,
                    width: '30%',
                    cellClass: 'columCss',
                },

                {
                    name: 'Is Good Will ',
                    cellTemplate: '<div class="center"><input type="checkbox" ng-model="row.entity.GoodWill"/></div>',
                    width: 100,
                    enableSorting: false
                }

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;

                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimSearchInquiryGrid.pageNumber = newPage;
                    paginationOptionsClaimSearchInquiryGrid.pageSize = pageSize;

                });
            }
        };
        $scope.showClaimSearchPopup = function () {

            var paginationOptionsPolicySearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            $scope.policySearchInquiryGridSearchCriterias = {
                ApprovedAmount: "",
                ClaimNumber: "",
                Date: ""
               };

            claimSearch();

            popUpSearchClaim = ngDialog.open({
                template: 'popUpSearchClaim',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            return true;
        };


        $scope.ClaimSearchInquiryGridSearchCriterias = {
            ApprovedAmount: "",
            ClaimNumber: "",
            Date: ""
        };
        $scope.claimSearchGridloading = false;
        $scope.claimSearchGridloadAttempted = false;


        $scope.IncurredErningExcel = function () {
            if ($scope.validateIncuredSelection()) {
            //if ($scope.BordxPayment.Id != null && $scope.BordxPayment.Id != "00000000-0000-0000-0000-000000000000") {
            $scope.IncurredErningExcelBtnIconClass = "fa fa-spinner fa-spin";
            $scope.IncurredErningExcelBtnDisabled = true;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/IncurredErningManagement/IncurredErningExcel',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: $scope.IncurredEarn,
                responseType: "arraybuffer",
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    var FileName = headers('Content-Disposition').split('=')[1];
                    var defaultFileName = FileName;
                    var type = headers('Content-Type');
                    var disposition = headers('Content-Disposition');

                    defaultFileName = defaultFileName.replace(/[<>:"\/\\|?*]+/g, '');
                    var blob = new Blob([data], { type: type });
                    saveAs(blob, defaultFileName);
                    $scope.IncurredErningExcelBtnIconClass = "";
                    $scope.IncurredErningExcelBtnDisabled = false;
                }
                else {

                }
            }).error(function (data, status, headers, config) {
                SweetAlert.swal({
                    title: $filter('translate')('pages.incurredErning.sucessMessages.erningProcess'),
                    text: $filter('translate')('pages.incurredErning.errorMessages.errorOccured'),
                    type: "warning",
                    confirmButtonText: $filter('translate')('pages.automobileAttributes.ok'),
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                $scope.IncurredErningExcelBtnIconClass = "";
                $scope.IncurredErningExcelBtnDisabled = false;
                return false;
            });
            }
        }

        $scope.validateBordxPayment = function () {
            var isValid = true;

            if ($scope.BordxPayment.PaidAmount == '') {
                $scope.validate_PaidAmount = "has-error";
                isValid = false;
            } else {
                $scope.validate_PaidAmount = "";
            }

            return isValid;
        }

        $scope.validateIncuredSelection = function () {
            var isValid = true;
            if ($scope.IncurredEarn.CountryId == '') {
                $scope.validate_incuredCountry = "has-error";
                isValid = false;
            } else {
                $scope.validate_incuredCountry = "";
            }

            if ($scope.IncurredEarn.DealerId == '') {
                $scope.validate_incuredDealer = "has-error";
                isValid = false;
            } else {
                $scope.validate_incuredDealer = "";
            }

            if ($scope.IncurredEarn.UNWYear== '') {
                $scope.validate_incuredYear = "has-error";
                isValid = false;
            } else {
                $scope.validate_incuredYear = "";
            }

            if (isValid == false) {
                customErrorMessage($filter('translate')('common.errMessage.allMandatory'));
            }
            return isValid;
        }

    });