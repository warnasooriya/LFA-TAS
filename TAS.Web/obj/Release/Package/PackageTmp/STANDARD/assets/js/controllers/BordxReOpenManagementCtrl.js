
app.controller('BordxReOpenManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, uiGridConstants, ngTableParams, $location) {

        $scope.ModalName = "Bordereaux Re-Open Management";
        $scope.ModalDescription = "Reopen recent confiremd bordereaux";
        $scope.errorTab1 = "";
        $scope.currentBordxYear = "";
        $scope.currentBordxMonth = "";
        $scope.currentBordxCountry = "";
        $scope.currentBordxCommodity = "";
        $scope.currentBordxNumber = "";

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
        $scope.bordxSearchGridSearchReset = function () {
            $scope.bordxSearchGridSearchCriterias = {
                commodityTypeId: emptyGuid(),
                year: 0,
                month: 0,
                reinsureId: emptyGuid(),
                insureId: emptyGuid(),
                //countryId: emptyGuid(),

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
            //$scope.RefreshSearchGridData();
        }

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
                //if () {
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
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {
                        $scope.policyGridloading = false;
                        $scope.policyGridloadAttempted = true;
                    });
           //     }
            }
        });
        $scope.loadBordx = function (bordxId, year, month, country,commodityType,Number) {
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
                { name: 'Insurer', field: 'Insurer', enableSorting: false, cellClass: 'columCss' },
                { name: 'Reinsurer', field: 'Reinsurer', enableSorting: false, cellClass: 'columCss' },
              { name: 'Year', field: 'Year', enableSorting: false, cellClass: 'columCss', width:75 },
              { name: 'Month', field: 'Month', enableSorting: false, cellClass: 'columCss', width: 75 },
              { name: 'No.', field: 'Number', enableSorting: false, cellClass: 'columCss', width: 75 },
              { name: 'Start Date', field: 'StartDate', enableSorting: false, cellClass: 'columCss' },
              { name: 'End Date', field: 'EndDate', enableSorting: false, cellClass: 'columCss' },


              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBordx('+
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
       
        $scope.RefreshSearchGridData = function () {
            getPage();
        }
      
        var getPage = function () {
            $scope.bordxSearchGridloading = true;
            $scope.bordxSearchGridloadAttempted = false;
         
            var bordxSearchGridParam =
                {
                    'paginationOptionsbordxSearchGrid': paginationOptionsbordxSearchGrid,
                    'bordxSearchGridSearchCriterias': $scope.bordxSearchGridSearchCriterias,
                    'action':'bordxreopen'
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
        
        getPage();
        function LoadDetails() {
            //validity check

            $scope.openSearchPopup();

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
                url: '/TAS.Web/api/DealerManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
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


            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetConfirmedBordxYears',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

            

        }
        $scope.ReopenBordx = function () {
            if (isGuid($scope.currentBordxId)) {


                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Reopen it!",
                    cancelButtonText: "No, cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({ title: 'Please wait', text: 'Reopening Bordx...', showConfirmButton: false });
                        $scope.BordexReopenRequest =
                            {
                                bordxId: $scope.currentBordxId,
                                userId: $rootScope.LoggedInUserId
                            }
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Bordx/BordxReopen',
                            data: $scope.BordexReopenRequest,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        }).success(function (data, status, headers, config) {
                            if (data == 'successful') {
                                swal({ title: "TAS Information", text: "Bordx Reopened successfully.", showConfirmButton: true },
                                    function (ok) {
                                        if (ok) {
                                         
                                            $scope.currentBordxYear = "";
                                            $scope.currentBordxMonth = "";
                                            $scope.currentBordxCountry = "";
                                            $scope.currentBordxCommodity = "";
                                            $scope.currentBordxNumber = "";
                                            $scope.openSearchPopup();
                                            $scope.RefreshSearchGridData();
                                            $scope.currentBordxId = emptyGuid();

                                            $scope.policyTable.data = [];
                                            $scope.policyTable.reload();
                                            
                                        }
                                    });
                               
                            } else {
                                swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                            }
                        }).error(function (data, status, headers, config) {
                            swal({ title: "TAS Information", text: "Error occured", showConfirmButton: true, type: 'error' });
                        }).finally(function () {
                           
                        });

                    }
                });
               
            } else {
                customErrorMessage("Please select a bordx for reopen");
            }
        }


        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
    });