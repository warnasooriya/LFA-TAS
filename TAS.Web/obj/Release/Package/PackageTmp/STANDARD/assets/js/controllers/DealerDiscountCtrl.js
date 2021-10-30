app.controller('DealerDiscountCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster) {
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };

        $scope.discountSchemes = [];
        $scope.countries = [];
        $scope.make = [];
        $scope.dealerDiscountPart = {
            id: emptyGuid(),
            itemType: 'P',
            countryId: emptyGuid(),
            dealerId: emptyGuid(),
            makeId: emptyGuid(),
            discounSchemeId: emptyGuid(),
            startDate: '',
            endDate: '',
            discountRate: 0.00,
            goodwillRate: 0.00,
            isActive: true
        };

        $scope.loadInitailData = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDiscountSchemes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.discountSchemes = data;
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
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.make = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
                getPolicySearchPage();
            }).error(function (data, status, headers, config) {
            });


        }


        var paginationOptionsDealerDiscountSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.gridDealerDiscounts = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'Type', field: 'Type', enableSorting: false, cellClass: 'columCss' },
              { name: 'Country', field: 'Country', enableSorting: false, cellClass: 'columCss', },
              { name: 'Dealer', field: 'Dealer', enableSorting: false, cellClass: 'columCss' },
              { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss' },
              { name: 'From', field: 'From', enableSorting: false, cellClass: 'columCss', minWidth: 90 },
              { name: 'To', field: 'To', enableSorting: false, cellClass: 'columCss', minWidth: 90 },
              { name: 'Discount', field: 'Discount', enableSorting: false, cellClass: 'columCss' },
              { name: 'Goodwill', field: 'Goodwill', enableSorting: false, cellClass: 'columCss' },
               { name: 'Scheme', field: 'Scheme', enableSorting: false, cellClass: 'columCss' },
              { name: 'Active', field: 'Active', enableSorting: false, cellClass: 'columCss', width: 60 },

              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadDealerDiscountForUpdate(row.entity.Id)" class="btn btn-xs btn-warning">Update</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsDealerDiscountSearchGrid.sort = null;
                    } else {
                        paginationOptionsDealerDiscountSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getPolicySearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsDealerDiscountSearchGrid.pageNumber = newPage;
                    paginationOptionsDealerDiscountSearchGrid.pageSize = pageSize;
                    getPolicySearchPage();
                });
            }
        };
        $scope.refresSearchGridData = function () {
            getPolicySearchPage();
        }

        function getPolicySearchPage() {
            if (!isGuid($scope.dealerDiscountPart.id)) {
                $scope.gridDealerDiscountsloading = true;
                $scope.gridDealerDiscountsloadAttempted = false;
                var policySearchGridParam =
                {
                    'paginationOptionsDealerDiscountSearchGrid': paginationOptionsDealerDiscountSearchGrid,
                    'dealerDiscountSearchGridSearchCriterias': $scope.dealerDiscountPart
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/SearchDealerDiscountSchemes',
                    data: JSON.stringify(policySearchGridParam),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function(data, status, headers, config) {
                    var responseArr = JSON.parse(data);
                    if (responseArr != null) {
                        $scope.gridDealerDiscounts.data = responseArr.data;
                        $scope.gridDealerDiscounts.totalItems = responseArr.totalRecords;
                    } else {
                        // $scope.gridDealerDiscounts.data = [];
                        // $scope.gridDealerDiscounts.totalItems = 0;
                    }

                }).error(function(data, status, headers, config) {
                }).finally(function() {
                    $scope.gridDealerDiscountsloading = false;
                    $scope.gridDealerDiscountsloadAttempted = true;

                });
            }
        };

        $scope.saveDealerDiscount = function () {
            if ($scope.validateDealerDiscount()) {
                //good to go
                swal({ title: 'Saving Changes...', text: 'Dealer discount information', showConfirmButton: false });
                $scope.dealerDiscountPart.userId = $localStorage.LoggedInUserId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/SaveDealerDiscount',
                    data: $scope.dealerDiscountPart,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data === "ok") {
                        swal("TAS Information", "Dealer discount changes saved successfully.", "success");
                        $scope.resetAfterSave();
                        getPolicySearchPage();
                    } else {
                        swal("TAS Information", data, "error");
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                });

            } else {
                customErrorMessage("Please fill valid values for highlighted fields.");
            }
        }
        $scope.resetAfterSave = function () {
            $scope.dealerDiscountPart.goodwillRate = 0.00;
            $scope.dealerDiscountPart.discountRate = 0.00;
            $scope.dealerDiscountPart.startDate = "";
            $scope.dealerDiscountPart.endDate = "";
            $scope.dealerDiscountPart.countryId = emptyGuid();
            $scope.dealerDiscountPart.makeId = emptyGuid();
            $scope.dealerDiscountPart.discounSchemeId = emptyGuid();
            $scope.dealerDiscountPart.dealerId = emptyGuid();
            $scope.dealerDiscountPart.id = emptyGuid();
            $scope.dealerDiscountPart.isActive = true;
        }
        $scope.validateDealerDiscount = function () {
            var isvalid = true;
            if ($scope.dealerDiscountPart.startDate === "") {
                $scope.validate_part_startDate = "has-error";
                isvalid = false;
            } else {
                $scope.validate_part_startDate = "";
            }

            if ($scope.dealerDiscountPart.endDate === "") {
                $scope.validate_part_endDate = "has-error";
                isvalid = false;
            } else {
                $scope.validate_part_endDate = "";
            }
            if (isvalid) {
                if ($scope.dealerDiscountPart.startDate > $scope.dealerDiscountPart.endDate) {
                    $scope.validate_part_endDate = "has-error";
                    $scope.validate_part_startDate = "has-error";
                    isvalid = false;
                    customErrorMessage("Start date cannot be grater than End date.");
                } else {
                    $scope.validate_part_endDate = "";
                    $scope.validate_part_startDate = "";
                }
            }
            if (!isGuid($scope.dealerDiscountPart.countryId)) {
                $scope.validate_part_countryId = "has-error";
                isvalid = false;
            } else {
                $scope.validate_part_countryId = "";
            }



            if (!isGuid($scope.dealerDiscountPart.dealerId)) {
                $scope.validate_part_dealerId = "has-error";
                isvalid = false;
            } else {
                $scope.validate_part_dealerId = "";
            }

            if ($scope.dealerDiscountPart.itemType === "P") {
                if (!isGuid($scope.dealerDiscountPart.makeId)) {
                    $scope.validate_part_makeId = "has-error";
                    isvalid = false;
                } else {
                    $scope.validate_part_makeId = "";
                }
            }

            if (!isGuid($scope.dealerDiscountPart.discounSchemeId)) {
                $scope.validate_part_discounSchemeId = "has-error";
                isvalid = false;
            } else {
                $scope.validate_part_discounSchemeId = "";
            }

            if (!parseFloat($scope.dealerDiscountPart.discountRate) && parseInt($scope.dealerDiscountPart.discountRate !== 0)) {
                $scope.validate_part_discountRate = "has-error";
                isvalid = false;
            } else {
                if (parseFloat($scope.dealerDiscountPart.discountRate) > 100) {
                    $scope.dealerDiscountPart.discountRate = 100;
                }
                $scope.validate_part_discountRate = "";
            }

            if (!parseFloat($scope.dealerDiscountPart.goodwillRate) && parseInt($scope.dealerDiscountPart.goodwillRate !== 0)) {
                $scope.validate_part_goodwillRate = "has-error";
                isvalid = false;
            } else {
                if (parseFloat($scope.dealerDiscountPart.goodwillRate) > 100) {
                    $scope.dealerDiscountPart.goodwillRate = 100;
                }
                $scope.validate_part_goodwillRate = "";
            }
            return isvalid;
        }

        $scope.selectedItemTypeChanged = function () {
            if ($scope.dealerDiscountPart.itemType === "L") {
                $scope.dealerDiscountPart.makeId = emptyGuid();
            }
            getPolicySearchPage();
        }
        $scope.resetAll = function() {
            $scope.resetAfterSave();
            getPolicySearchPage();
        }
        $scope.loadDealerDiscountForUpdate = function (dealerDiscountId) {
            if (isGuid(dealerDiscountId)) {
                swal({ title: 'Reading...', text: 'Dealer discount information', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetDealerDiscountById',
                    data: { 'dealerDiscountId': dealerDiscountId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data !== null && typeof data !== "undefined") {
                        $scope.dealerDiscountPart.id = data.DealerDiscountId;
                        $scope.dealerDiscountPart.itemType = data.ItemType;
                        $scope.dealerDiscountPart.countryId = data.CountryId;
                        $scope.dealerDiscountPart.dealerId = data.DealerId;
                        $scope.dealerDiscountPart.discounSchemeId = data.SchemeId;
                        $scope.dealerDiscountPart.discountRate = data.DiscountRate;
                        $scope.dealerDiscountPart.goodwillRate = data.GoodWillRate;
                        $scope.dealerDiscountPart.makeId = data.MakeId;
                        $scope.dealerDiscountPart.endDate = data.EndDate;
                        $scope.dealerDiscountPart.isActive = data.IsActive;
                        $scope.dealerDiscountPart.startDate = data.StartDate;
                        if (data.ItemType === "L") {
                            $scope.dealerDiscountPart.makeId = emptyGuid();
                        }
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }

        }


        function customErrorMessage(msg) {
            toaster.pop('error', 'Error', msg);
        };

        function customInfoMessage(msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };
    });

