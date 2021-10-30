app.controller('CacheUtilCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Cache Utilities";
        $scope.value = true;
        $scope.serchGridColumsn = [];

        $scope.loadInitailData = function () {
            getCacheSearchPage();
        }

        var paginationOptionsCacheSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };


        $scope.gridOptionsCache = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: false,
            enableColumnMenus: false,
            columnDefs: [
                {
                    name: 'Action',
                    cellTemplate: '<div style="float:left;margin-left:10px"><button  class="btn btn-xs btn-warning">View </button></div>  <div style="float:left ; margin-left:10px"><button  class="btn btn-xs btn-warning">Delete</button></div>',
                    width: 150,
                    enableSorting: false
                },
                { name: 'KEY', field: 'key', enableSorting: false, visible: true, cellClass: 'columCss' },
                { name: 'Value Count', field: 'valueCount', enableSorting: false, cellClass: 'columC' },

            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsCacheSearchGrid.sort = null;
                    } else {
                        paginationOptionsCacheSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getCacheSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsCacheSearchGrid.pageNumber = newPage;
                    paginationOptionsCacheSearchGrid.pageSize = pageSize;
                    getCacheSearchPage();
                });
            }
        };


        var getCacheSearchPage = function () {
            $scope.cacheSearchGridloading = true;
            $scope.cacheSearchGridloadAttempted = false;

            $scope.serchGridColumsn  = [
                { name: 'KEY', field: 'key', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Value Count', field: 'valueCount', enableSorting: false, cellClass: 'columC' },
                {
                    name: 'key',
                    cellTemplate: '<div class="center"><button  class="btn btn-xs btn-warning">View Data</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ];

            var tempdata = [
                { "key": "tenentId", "valueCount": 2 },
                { "key": "dealerId", "valueCount": 5 },
                { "key": "dealerId", "valueCount": 5 },
                { "key": "Dealer", "valueCount": 7 },
            ];

            $scope.gridOptionsCache.data = tempdata;
            $scope.gridOptionsCache.totalItems = tempdata.length;
            $scope.cacheSearchGridloading = false;
            $scope.cacheGridloadAttempted = true;
            //var cacheSearchGridParam =
            //{
            //    'paginationOptionsPolicySearchGrid': paginationOptionsCacheSearchGrid,
            //    'policySearchGridSearchCriterias': $scope.cacheSearchGridSearchCriterias,
            //    'userId': $localStorage.LoggedInUserId
            //}
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/PolicyReg/GetPoliciesForSearchGrid',
            //    data: cacheSearchGridParam,
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    var response_arr = JSON.parse(data);
            //    $scope.gridOptionsCache.data = response_arr.data;
            //    $scope.gridOptionsCache.totalItems = response_arr.totalRecords;



            //}).error(function (data, status, headers, config) {

            //}).finally(function () {

            //    $scope.cacheSearchGridloading = false;
            //    $scope.cacheSearchGridloadAttempted = true;

            //});
        };








    });

