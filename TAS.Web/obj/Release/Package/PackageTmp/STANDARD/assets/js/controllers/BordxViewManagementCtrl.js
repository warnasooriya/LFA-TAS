app.controller('BordxViewManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, uiGridConstants, ngTableParams, $location, toaster) {

        $scope.ModalName = "Bordereaux View Management";
        $scope.ModalDescription = "View Bordereaux";
        $scope.errorTab1 = "";
        $scope.currentBordxYear = "";
        $scope.currentBordxMonth = "";
        $scope.currentBordxDealer = "";
        var SearchBordxPopup;//search popup public var
        var ViewBordxPopup;//view popup public var
        $scope.bordxSearchGridloading = false;
        $scope.bordxSearchGridloadAttempted = false
        $scope.bordxViewGridloading = false;
        $scope.bordxViewGridloadingloadAttempted = false;
        $scope.policyGridloading = false;
        $scope.policyGridloadAttempted = false;
        $scope.currentBordxId = emptyGuid();
        $scope.bordxSearchGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            year: 0,
            month: 0,
            reinsureId: emptyGuid(),
            insureId: emptyGuid(),
            //countryId: emptyGuid(),

        };
        $scope.bordxViewGridSearchCriterias = {
            bordxId: emptyGuid(),
            userId: emptyGuid()
        };

        $scope.bordxSearchGridSearchCriteriasReset =  function () {
            $scope.bordxSearchGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            year: 0,
            month: 0,
            countryId: emptyGuid(),

            }
        }
        $scope.bordxAvailableYears = [];
        $scope.dealers = [];
        $scope.BordexExportRequest = {};
        $scope.BordexViewRequest = {};
        $scope.openSearchPopup = function () {
            SearchBordxPopup = ngDialog.open({
                template: 'BordxSearchPopup',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        }
        $scope.isExport = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        $scope.bordxReportTemplateId = "";

        LoadDetails();
        $scope.policyTable = new ngTableParams({
            page: 1,
            count: 25,
        }, {
                getData: function ($defer, params) {

                    var page = params.page();
                    var size = params.count();
                    var search = {
                        page: page,
                        pageSize: size,
                        bordxId: $scope.currentBordxId
                    }
                    if (isGuid($scope.currentBordxId)) {
                        $scope.policyGridloading = true;
                        $scope.policyGridloadAttempted = false;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Bordx/GetPoliciesByBordxIdForGrid',
                            data: search,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var response_arr = JSON.parse(data);
                            params.total(response_arr.totalRecords);
                            $defer.resolve(response_arr.data);
                            if (response_arr.data != undefined && response_arr.data != null && response_arr.data.length > 0) {
                                $scope.isExport = true;
                            }
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            $scope.policyGridloading = false;
                            $scope.policyGridloadAttempted = true;
                        });
                    }
                }
            });
        $scope.loadBordx = function (bordxId, year, month, country, commodityType, Number) {
            $scope.isExport = false;
            if (isGuid(bordxId)) {
                $scope.currentBordxYear = year;
                $scope.currentBordxMonth = month;
                $scope.currentBordxCountry = country;
                $scope.currentBordxCommodity = commodityType;
                $scope.currentBordxNumber = Number;
                $scope.currentBordxId = bordxId;
                $scope.policyTable.data = [];
                $scope.policyTable.reload();
                SearchBordxPopup.close();

            } else {
                SweetAlert.swal({
                    title: "TAS Information",
                    text: "Invalid Bordx selection",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            }
        }
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }
        var paginationOptionsbordxSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.bordxSearchGrid = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Commodity', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                //{ name: 'Country', field: 'Country', enableSorting: false, cellClass: 'columCss' },
                { name: 'Insurer', field: 'Insurer', enableSorting: false, cellClass: 'columCss' },
                { name: 'Reinsurer', field: 'Reinsurer', enableSorting: false, cellClass: 'columCss' },
                { name: 'Year', field: 'Year', enableSorting: false, cellClass: 'columCss', width: 75 },
                { name: 'Month', field: 'Month', enableSorting: false, cellClass: 'columCss', width: 75 },
                { name: 'No.', field: 'Number', enableSorting: false, cellClass: 'columCss', width: 75 },
                { name: 'Start Date', field: 'StartDate', enableSorting: false, cellClass: 'columCss' },
                { name: 'End Date', field: 'EndDate', enableSorting: false, cellClass: 'columCss' },


                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBordx(' +
                        'row.entity.Id,row.entity.Year,row.entity.Month,row.entity.Country,row.entity.CommodityType,row.entity.Number)" ' +
                        'class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsbordxSearchGrid.sort = null;
                    } else {
                        paginationOptionsbordxSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsbordxSearchGrid.pageNumber = newPage;
                    paginationOptionsbordxSearchGrid.pageSize = pageSize;
                    getPage();
                });
            }
        };
        var paginationOptionsbordxViewGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.bordxViewGrid = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Coverholder Name', field: 'ContractDetailsCoverholderName', enableSorting: false, cellClass: 'columCss' },
                { name: 'TPA Name', field: 'ContractDetailsTPAName', enableSorting: false, cellClass: 'columCss' },
                { name: 'Agreement No', field: 'ContractDetailsAgreementNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Contract Inception', field: 'ContractDetailsReportingPeriodStartDate', enableSorting: false, cellClass: 'columCss', width: 75, },
                { name: 'Contract Expiry', field: 'ContractDetailsReportingPeriodEndDate', enableSorting: false, cellClass: 'columCss' },
                { name: 'Class of Business', field: 'ContractDetailsClassofBusiness', enableSorting: false, cellClass: 'columCss' },
                { name: 'Original Currency', field: 'ContractDetailsSettlementCurrency', enableSorting: false, cellClass: 'columCss' },
                { name: 'Certificate Reference', field: 'ReferencesCertificateReference', enableSorting: false, cellClass: 'columCss' },
                { name: 'Country', field: 'ClaimantAddressClaimantCountry', enableSorting: false, cellClass: 'columCss' },
                { name: 'Postal code', field: 'ClaimantAddressClaimantPostcode', enableSorting: false, cellClass: 'columCss' },
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsbordxViewGrid.sort = null;
                    } else {
                        paginationOptionsbordxViewGrid.sort = sortColumns[0].sort.direction;
                    }
                    getViewPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsbordxViewGrid.pageNumber = newPage;
                    paginationOptionsbordxViewGrid.pageSize = pageSize;
                    getViewPage();
                });
            }
        };
        $scope.RefreshSearchGridData = function () {
            getPage();
        }
        $scope.RefreshViewGridData = function () {
            getViewPage();
        }
        var getPage = function () {
            $scope.bordxSearchGridloading = true;
            $scope.bordxSearchGridloadAttempted = false;

            var bordxSearchGridParam =
            {
                'paginationOptionsbordxSearchGrid': paginationOptionsbordxSearchGrid,
                'bordxSearchGridSearchCriterias': $scope.bordxSearchGridSearchCriterias,
                'action': 'bordxview'
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetConfirmedBordxForGrid',
                data: bordxSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                $scope.bordxSearchGrid.data = response_arr.data;
                $scope.bordxSearchGrid.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.bordxSearchGridloading = false;
                $scope.bordxSearchGridloadAttempted = true;

            });
        };
        var getViewPage = function () {
            $scope.bordxViewGridloading = true;
            $scope.bordxViewGridloadingloadAttempted = false;
            $scope.bordxViewGridSearchCriterias.bordxId = $scope.currentBordxId;
            $scope.bordxViewGridSearchCriterias.userId = $rootScope.LoggedInUserId;
            var bordxViewGridParam =
            {
                'PaginationOptionsbordxSearchGrid': paginationOptionsbordxViewGrid,
                'bordxViewGridSearchCriterias': $scope.bordxViewGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetConfirmedBordxForViewGrid',
                data: bordxViewGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                $scope.bordxViewGrid.data = response_arr.data;
                $scope.bordxViewGrid.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.bordxViewGridloading = false;
                $scope.bordxViewGridloadingloadAttempted = true;

            });
        }
        getPage();
        function LoadDetails() {
            //validity check
            if ($rootScope.UserType == "RI") {
                if (!isGuid($localStorage.ReinsurerId)) {
                    $location.path('login/signin/' + $localStorage.tpaID);
                }
            } else {

            }

            $scope.openSearchPopup();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
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
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.reinsurers = data;
            }).error(function (data, status, headers, config) {
                });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
            }).error(function (data, status, headers, config) {
                });

            //GetBordxReportTemplates
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetBordxReportTemplates',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxReportTemplates = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetConfirmedBordxYears',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

            if ($rootScope.UserType == "RI") {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetDealerById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { Id: $localStorage.ReinsurerId }
                }).success(function (data, status, headers, config) {
                    //$scope.dealers = data;
                    if (data.Id == emptyGuid()) {
                        SweetAlert.swal({
                            title: "TAS Information",
                            text: "No dealers found",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        SearchBordxPopup.close();
                        //$localStorage.jwt = "";
                        $location.path('app/dashboard/');
                    } else {
                        $scope.dealers[0] = data;
                        $scope.bordxSearchGridSearchCriterias.dealerId = data.Id;
                    }
                }).error(function (data, status, headers, config) {
                });
            } else {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.dealers[0] = { "Id": emptyGuid(), "DealerCode": "All" };
                    for (var i = 1; i < data.length; i++) {
                        $scope.dealers[i] = data[i - 1];
                    }
                    //  $scope.bordxSearchGridSearchCriterias.dealerId = emptyGuid();
                }).error(function (data, status, headers, config) {
                });
            }


        }
        $scope.directExportAsExcel = function () {
            if ($scope.validate() && isGuid($scope.currentBordxId) && isGuid($scope.bordxReportTemplateId)) {
                swal({ title: 'Please wait', text: 'Preparing Bordereaux...', showConfirmButton: false });
                $scope.BordexExportRequest =
                    {
                        bordxId: $scope.currentBordxId,
                        userId: $rootScope.LoggedInUserId,
                        bordxReportTemplateId: $scope.bordxReportTemplateId
                    }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Bordx/ExportPoliciesToExcelByBordxId',
                    data: $scope.BordexExportRequest,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    responseType: "arraybuffer",
                }).success(function (data, status, headers, config) {
                    if (headers('Content-Disposition') != undefined) {
                        var FileName = headers('Content-Disposition').split('=')[1];

                        var defaultFileName = FileName;
                        var type = headers('Content-Type');
                        var disposition = headers('Content-Disposition');

                        defaultFileName = defaultFileName.replace(/[<>:"\/\\|?*]+/g, '');
                        var blob = new Blob([data], { type: type });
                        saveAs(blob, defaultFileName);
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    if (typeof ViewBordxPopup !== 'undefined')
                        ViewBordxPopup.close();
                    swal.close();
                });
            }
        }

        $scope.validate = function () {
            var isValid = true;
            if ($scope.bordxReportTemplateId == undefined || $scope.bordxReportTemplateId == null || $scope.bordxReportTemplateId == "" || (!isGuid($scope.bordxReportTemplateId))) {
                isValid = false;
                customErrorMessage("Please select the report template.")
            }
            return isValid;
        }


        $scope.viewPolicies = function () {
            if (isGuid($scope.currentBordxId)) {
                getViewPage();
            }
            ViewBordxPopup = ngDialog.open({
                template: 'BordxViewPopup',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        }

    });