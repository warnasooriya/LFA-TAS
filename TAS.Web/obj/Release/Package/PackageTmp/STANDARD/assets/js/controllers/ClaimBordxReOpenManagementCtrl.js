app.controller('ClaimBordxReOpenManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, uiGridConstants, ngTableParams, $location, toaster) {

        $scope.currentreinsure = emptyGuid();
        $scope.currentinsure = emptyGuid();
        $scope.currentId = emptyGuid();       
        $scope.currentBordxNumber = "";  
        

        $scope.currentBordxYear = "";
        $scope.currentBordxMonth = "";
        $scope.currentTodate = "";
        $scope.currentFromdate = "";
        var SearchBordxPopup;//search popup public var
        $scope.bordxSearchGridloading = false;
        $scope.bordxSearchGridloadAttempted = false
        $scope.bordxAvailableYears = [];
        $scope.bordxNumbers = [];
        
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }

        $scope.ClaimBordxSearchGridSearchCriterias = {
            Insurer: emptyGuid(),
            year: 0,
            month: 0,
            Reinsurer: emptyGuid(),

        };
        $scope.bordxViewGridSearchCriterias = {
            bordxId: emptyGuid(),
            userId: emptyGuid()

        };

        //$scope.openSearchPopup = function () {

        //    SearchBordxPopup = ngDialog.open({
        //        template: 'BordxSearchPopup',
        //        className: 'ngdialog-theme-plain',
        //        closeByEscape: true,
        //        showClose: true,
        //        closeByDocument: true,
        //        scope: $scope
        //    });
        //}


        //var PaginationOptionsClaimbordxSearchGrid = {
        //    pageNumber: 1,
        //    pageSize: 25,
        //    sort: null
        //};
        //$scope.bordxSearchGrid = {
        //    paginationPageSizes: [25, 50, 75],
        //    paginationPageSize: 25,
        //    useExternalPagination: true,
        //    useExternalSorting: true,
        //    enableColumnMenus: false,
        //    columnDefs: [
        //      { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
        //      { name: 'Insurer', field: 'Insurer', enableSorting: false, cellClass: 'columCss' },
        //      { name: 'Reinsurer', field: 'Reinsurer', enableSorting: false, cellClass: 'columCss' },
        //      { name: 'Year', field: 'BordxYear', enableSorting: false, cellClass: 'columCss', width: 75 },
        //      { name: 'Month', field: 'Bordxmonth', enableSorting: false, cellClass: 'columCss', width: 75 },
        //      { name: 'No.', field: 'BordxNumber', enableSorting: false, cellClass: 'columCss', width: 75 },
        //      { name: 'Start Date', field: 'StartDate', enableSorting: false, cellClass: 'columCss' },
        //      { name: 'End Date', field: 'EndDate', enableSorting: false, cellClass: 'columCss' },


        //      {
        //          name: ' ',
        //          cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBordx(' +
        //          'row.entity.Id,row.entity.Insurer,row.entity.Reinsurer,row.entity.BordxYear,row.entity.Bordxmonth,row.entity.BordxNumber,row.entity.StartDate,row.entity.EndDate)" ' +
        //          'class="btn btn-xs btn-warning">Load</button></div>',
        //          width: 60,
        //          enableSorting: false
        //      }
        //    ],
        //    onRegisterApi: function (gridApi) {
        //        $scope.gridApi = gridApi;
        //        $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
        //            if (sortColumns.length == 0) {
        //                PaginationOptionsClaimbordxSearchGrid.sort = null;
        //            } else {
        //                PaginationOptionsClaimbordxSearchGrid.sort = sortColumns[0].sort.direction;
        //            }
        //            getPage();
        //        });
        //        gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
        //            PaginationOptionsClaimbordxSearchGrid.pageNumber = newPage;
        //            PaginationOptionsClaimbordxSearchGrid.pageSize = pageSize;
        //            getPage();
        //        });
        //    }
        //};
        //var paginationOptionsbordxViewGrid = {
        //    pageNumber: 1,
        //    pageSize: 25,
        //    sort: null
        //};

        //$scope.RefreshSearchGridData = function () {
        //    getPage();
        //}

        //var getPage = function () {
        //    $scope.bordxSearchGridloading = true;
        //    $scope.bordxSearchGridloadAttempted = false;

        //    var bordxSearchGridParam =
        //        {
        //            'PaginationOptionsClaimbordxSearchGrid': PaginationOptionsClaimbordxSearchGrid,
        //            'ClaimBordxSearchGridSearchCriterias': $scope.ClaimBordxSearchGridSearchCriterias,
        //            'action': 'bordxreopen'
        //        }
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/ClaimBordxManegement/GetConfirmedClaimBordxForGrid',
        //        data: bordxSearchGridParam,
        //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        //        //value.Dealer = data.DealerName
        //        var response_arr = JSON.parse(data);
        //        $scope.bordxSearchGrid.data = response_arr.data;
        //        $scope.bordxSearchGrid.totalItems = response_arr.totalRecords;
        //    }).error(function (data, status, headers, config) {
        //    }).finally(function () {
        //        $scope.bordxSearchGridloading = false;
        //        $scope.bordxSearchGridloadAttempted = true;

        //    });
        //};

        $scope.SearchReopenBordx = function () {
            $scope.policyTable.data = [];
            $scope.policyTable.reload();
        }

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
                    //bordxId: $scope.currentBordxId
                    Todate: $scope.currentTodate,
                    Fromdate: $scope.currentFromdate,
                    month:$scope.currentBordxMonth,
                    year: $scope.currentBordxYear,
                    number: $scope.currentBordxNumber
                }
                //if () {
                $scope.policyGridloading = true;
                $scope.policyGridloadAttempted = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBordxManegement/GetAllClaimBordxByDateForGrid',
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

        //$scope.loadBordx = function (id, insurer, reinsurer, bordxyear, bordxmonth, bordxnumber, startdate, enddate) {
        //    if (isGuid(id)) {
        //        $scope.currentId = id;
        //        $scope.currentreinsure = insurer;
        //        $scope.currentinsure = reinsurer;
        //        $scope.currentBordxYear = bordxyear;
        //        $scope.currentBordxMonth = bordxmonth;                
        //        $scope.currentBordxNumber = bordxnumber;
        //        $scope.currentTodate = startdate;
        //        $scope.currentFromdate = enddate;
        //        $scope.policyTable.data = [];
        //        $scope.policyTable.reload();
        //        SearchBordxPopup.close();

        //    } else {
        //        SweetAlert.swal({
        //            title: "TAS Information",
        //            text: "Invalid Bordx selection",
        //            type: "warning",
        //            confirmButtonColor: "rgb(221, 107, 85)"
        //        });
        //    }
        //}

       //getPage();
        function LoadDetails() {
            //validity check

           // $scope.openSearchPopup();          

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
                url: '/TAS.Web/api/ClaimBordxManegement/GetConfirmedClaimBordxYears',                
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetClaimBordxBordxNumbers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxNumbers = data;
            }).error(function (data, status, headers, config) {
            });


        }

        LoadDetails();

        $scope.GetBordxNumbersYearsAndMonth = function () {
            if ($scope.currentBordxYear != "" && $scope.currentBordxMonth != "") {
               
                $http({
                    method: 'post',
                    url: '/tas.web/api/Bordx/getBordxNumbersYearsAndMonth',
                    data: JSON.stringify({ 'bordxYear': $scope.currentBordxYear, 'bordxMonth': $scope.currentBordxMonth }),
                    headers: { 'authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.bordxNumbers = data;                    
                }).error(function (data, status, headers, config) {
                });
            }
        }

        
        $scope.ReopenBordx = function (BordxYear, BordxMonth) {
            //if (isGuid($scope.currentBordxId)) {
            if (BordxYear == 0 || BordxMonth == 0 || BordxYear == "" || BordxMonth == "") {
                customErrorMessage("Please select a year ,a month and a bordx number.")
            } else {
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
                        swal({ title: 'Please wait', text: 'Reopening Claim Bordx...', showConfirmButton: false });
                        $scope.BordexReopenRequest =
                            {
                                claimBordxYear: BordxYear,
                                claimBordxMonth: BordxMonth
                            }
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxReopen',
                            data: $scope.BordexReopenRequest,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        }).success(function (data, status, headers, config) {
                            if (data == 'successful') {
                                swal({ title: "TAS Information", text: "Claim Bordx Reopened successfully.", showConfirmButton: true },
                                    function (ok) {
                                        if (ok) {

                                            $scope.currentBordxYear = "";
                                            $scope.currentBordxMonth = "";

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

            //} else {
            //    customErrorMessage("Please select a bordx for reopen");
            //}
            }

              
        }


        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };


    });