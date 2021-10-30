app.controller('ClaimBordxManagementCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {

        $scope.ClaimbordxSaveBtnIconClass = "";
        $scope.ClaimbordxSaveBtnDisabled = false;
        $scope.errorTab1 = "";
        $scope.gridloading = false;

        Clear();

        createGrid();

        function LoadDetails() {
            var date = new Date();
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
                url: '/TAS.Web/api/ClaimBordxManegement/GetAllBordxs',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.last10ClaimBordx = data;
            }).error(function (data, status, headers, config) {
            });

            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/ClaimBordxManegement/GetClaimBordxYears',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.ClaimBordxyears = data;
            //}).error(function (data, status, headers, config) {
            //});


            $http({
                method: 'POST',
                url: '/TAS.Web/api/claimbordxprocess/GetBordxYearsByClaim', //GetBordxYearsByClaim',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //var fdate = new Date(date.getFullYear(), date.getMonth(), 1);
                //$scope.Claimbordx.Fromdate = fdate.toLocaleDateString();
                //var ldate = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                //$scope.Claimbordx.Todate = ldate.toLocaleDateString();
                $scope.ClaimBordxyears = data;

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
        $scope.Claimbordx = {
            Id: "00000000-0000-0000-0000-000000000000",
            //InsurerCode: "",
            //InsurerShortName: ""
        };

        function clearCliamBordxControls() {
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

            $scope.Claimbordx = {
                Id: "00000000-0000-0000-0000-000000000000",
                BordxYear: "",
                Bordxmonth: "",
                BordxNumber: "",
                Fromdate: "",
                Todate: ""
            };
        }

        $scope.createClaimBordx = function () {
            if (isGuid($scope.Claimbordx.insurerId) && isGuid($scope.Claimbordx.reinsurerId)) {
                if (parseInt($scope.Claimbordx.BordxYear) && parseInt($scope.Claimbordx.Bordxmonth) && parseInt($scope.Claimbordx.Bordxmonth) > 0 && parseInt($scope.Claimbordx.BordxYear) > 0) {

                    if ($scope.Claimbordx.Fromdate != '' && $scope.Claimbordx.Todate != '' &&
                        typeof $scope.Claimbordx.Fromdate !== 'undefined' && typeof $scope.Claimbordx.Todate !== 'undefined') {

                        if (new Date($scope.Claimbordx.Fromdate) <= new Date($scope.Claimbordx.Todate)) {


                            swal({
                                title: "Are you sure?",
                                text: "",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, Create!",
                                cancelButtonText: "No, cancel!",
                                closeOnConfirm: false,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                                    var newClaimBordxRequest = {
                                        BordxYear: $scope.Claimbordx.BordxYear,
                                        Bordxmonth: $scope.Claimbordx.Bordxmonth,
                                        BordxNumber: $scope.Claimbordx.BordxNumber,
                                        Fromdate: $scope.Claimbordx.Fromdate,
                                        Todate: $scope.Claimbordx.Todate,
                                        createdby: $localStorage.LoggedInUserId,
                                        Insurer: $scope.Claimbordx.insurerId,
                                        Reinsurer: $scope.Claimbordx.reinsurerId
                                    };

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/ClaimBordxManegement/CreateClaimBordx',
                                        data: newClaimBordxRequest,
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        if (data == 'successful') {
                                            swal({ title: "TAS Information", text: "Bordereaux created successfully.", showConfirmButton: true });
                                            clearCliamBordxControls();
                                            $scope.getLast10Bordx();
                                        } else {
                                            swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                                        }
                                    }).error(function (data, status, headers, config) {
                                        swal.close();
                                    }).finally(function () {

                                    });
                                }
                            });

                        } else {
                            customErrorMessage("From Date Cannot Exceed To Date.");
                        }
                    } else {
                        customErrorMessage("Please Select From Date & To Date.");
                    }

                } else {
                    customErrorMessage("Invalid ClaimBordereaux Year/Month Selection.");
                }
            } else {
                customErrorMessage("Invalid Reinsu/Country Selection.");
            }


        }

        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }

        $scope.getLast10Bordx = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetAllBordxs',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.last10ClaimBordx = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.loadBordxNumber = function () {
            $scope.Claimbordx.BordxNumber = '';
            if (isGuid($scope.Claimbordx.insurerId) && isGuid($scope.Claimbordx.reinsurerId)) {
                if (parseInt($scope.Claimbordx.BordxYear) && parseInt($scope.Claimbordx.Bordxmonth)
                    && parseInt($scope.Claimbordx.Bordxmonth) > 0 && parseInt($scope.Claimbordx.BordxYear) > 0) {
                    var data = {
                        'year': $scope.Claimbordx.BordxYear,
                        'month': $scope.Claimbordx.Bordxmonth,
                        'insurerId': $scope.Claimbordx.insurerId,
                        'ReinsurerId': $scope.Claimbordx.reinsurerId
                    };
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimBordxManegement/GetNextBordxNumber',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Claimbordx.BordxNumber = data;
                        $scope.loadStartDateAndEndDates();
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });
                }
            }
        }
        $scope.selectableEndDate;
        $scope.selectableMinDate;
        $scope.loadStartDateAndEndDates = function () {
            var month = $scope.Claimbordx.Bordxmonth + '';
            var startDate = $scope.Claimbordx.BordxYear + '-' + month.padStart(2, '0') + '-' + '01';
            var ldate = new Date(startDate);
            var lastDayOfMonth = new Date(ldate.getFullYear(), ldate.getMonth() + 1, 0);
            var sed = lastDayOfMonth.getDate() + '';
            $scope.selectableEndDate = $scope.Claimbordx.BordxYear + '-' + month.padStart(2, '0') + '-' + sed.padStart(2, '0');
            $scope.Claimbordx.Fromdate = startDate;
            $scope.Claimbordx.Todate = $scope.selectableEndDate;
        }

        $scope.deleteClaimBordx = function (bordxId) {
            if (isGuid(bordxId)) {

                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Delete!",
                    cancelButtonText: "No, cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimBordxManegement/DeleteClaimBordx',
                            data: { 'bordxId': bordxId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'successful') {
                                swal({ title: "TAS Information", text: "ClaimBordereaux deleted successfully.", showConfirmButton: true });
                                $scope.getLast10Bordx();
                            } else {
                                swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                            }
                        }).error(function (data, status, headers, config) {
                            swal.close();
                        }).finally(function () {
                            //swal.close();
                        });

                    }
                });

            }
        }


        //------------------------------------------------------------------------------------------------------------------------------------


        function Clear() {
            clearButtons();

            $scope.errorTab1 = "";
            $scope.bordx = [];
            $scope.processedBordx = [];
            $scope.gridloading = false;

            $scope.ClaimProcessbordx = {
                id: emptyGuid(),
                year: "",
                month: "",
                claimBordxId: emptyGuid(),
                insurerId: emptyGuid(),
                reinsurerId: emptyGuid(),
                isProcessed: true,
                isConfirmed: false
            }
        }

        function clearButtons() {
            $scope.canProcess = false;
            $scope.canConfirm = false;
            $scope.canView = false;
        }

        $scope.gridOptionsClaim = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Policy Number', field: 'PolicyNumber', width: '20%', enableSorting: false, cellClass: 'columCss', },
                { name: 'Claim Number', field: 'ClaimNumber', enableSorting: false, cellClass: 'columCss' },
                { name: 'Authorized Amount', field: 'AuthorizedAmount', enableSorting: false, cellClass: 'columCss' },
                
                
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsClaimSearchGrid.sort = null;
                    } else {
                        paginationOptionsClaimSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getClaimSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimSearchGrid.pageNumber = newPage;
                    paginationOptionsClaimSearchGrid.pageSize = pageSize;
                    getClaimSearchPage();
                });
            }
        };


        $scope.showClaimSearchPopup = function () {
            getClaimSearchPage();            
            ClaimSelectionPopup = ngDialog.open({
                template: 'popUpClaimSelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }
        var paginationOptionsClaimSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.claimSearchGridSearchCriterias = {
            commodityTypeId: "",
            claimNumber: "",
            policyId: "",
            customerId: "",
            claimId:"" 

        };

        var getClaimSearchPage = function () {
            $scope.claimSearchGridloading = true;
            $scope.claimGridloadAttempted = false;
            $scope.claimSearchGridSearchCriterias.claimId=$scope.ClaimProcessbordx.bordx.Id;
            var claimSearchGridParam =
            {
                'paginationOptionsClaimSearchGrid': paginationOptionsClaimSearchGrid,
                'claimSearchGridSearchCriterias': $scope.claimSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claimbordxprocess/ProcessClaimBordxForSearchGrid',
                data: claimSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsClaim.data = response_arr.data;
                $scope.gridOptionsClaim.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.claimSearchGridloading = false;
                $scope.claimGridloadAttempted = true;

            });
        };


        $scope.loadProcessBordxNumber = function (val) {
            clearButtons();
            $scope.bordx = [];
            if ($scope.ClaimProcessbordx.insurerId != emptyGuid() && $scope.ClaimProcessbordx.insurerId != "" && $scope.ClaimProcessbordx.reinsurerId != emptyGuid() && $scope.ClaimProcessbordx.reinsurerId != "" && $scope.ClaimProcessbordx.year != "" && $scope.ClaimProcessbordx.month != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claimbordxprocess/GetClaimBordxByYearAndMonth',
                    data: { 'year': $scope.ClaimProcessbordx.year, 'month': $scope.ClaimProcessbordx.month, 'insurerid': $scope.ClaimProcessbordx.insurerId, 'reinsurerid': $scope.ClaimProcessbordx.reinsurerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.bordx = data;
                }).error(function (data, status, headers, config) {
                });
            }
            //if (val) $scope.loadProcessedClaimBordx();
        }

        $scope.loadProcessedClaimBordx = function () {
            $scope.processedBordx = [];
            if ($scope.ClaimProcessbordx.insurerId != emptyGuid() && $scope.ClaimProcessbordx.insurerId != "" && $scope.ClaimProcessbordx.reinsurerId != emptyGuid() && $scope.ClaimProcessbordx.reinsurerId != "" && $scope.ClaimProcessbordx.year != "") {
                $scope.gridloading = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claimbordxprocess/ClaimBordxProcessedDetailsByYear',
                    data: { 'year': $scope.ClaimProcessbordx.year, 'insurerid': $scope.ClaimProcessbordx.insurerId, 'reinsurerid': $scope.ClaimProcessbordx.reinsurerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.processedBordx = data;
                }).error(function (data, status, headers, config) {
                }).finally(function () { $scope.gridloading = false; });
            }
        }

        function createGrid() {
            $scope.processedBordx = [];
            $scope.ColumnList = [{ field: "Id", displayName: "", width: 0, visible: false, enableCellEdit: false },
                { field: "BordxNumber", displayName: "Bordx Number", width: 250, enableCellEdit: false },
                { field: "Year", displayName: "Year", width: 150, enableCellEdit: false },
                { field: "Month", displayName: "Month", width: 150, enableCellEdit: false },
                { field: "Amount", displayName: "Amount", width: 200, cellFilter: 'number: 2' },
                { field: "IsConfirmed", displayName: "Confirmed", width: 100, cellTemplate: '<div class="center"><input type="checkbox"  ng-model="row.entity.IsConfirmed" class="checkbox" disabled="disabled" ></div>' }
            ];
            $scope.gridOptions = {
                data: 'processedBordx',
                paginationPageSizes: [5, 10, 20],
                paginationPageSize: 10,
                enablePaginationControls: true,
                enableColumnResizing: true,
                columnDefs: $scope.ColumnList
            };

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
            }
        }

        $scope.checkConfirm = function () {
            clearButtons();
            if (!$scope.ClaimProcessbordx.bordx.IsConfirmed) { $scope.canConfirm = $scope.ClaimProcessbordx.bordx.IsProcessed; $scope.canProcess = true; } else { $scope.canView = true; }
        }

        function validate() {
            var retVal = true;
            $scope.errorTab1 = "";
            $scope.ClaimProcessbordx.claimBordxId = $scope.ClaimProcessbordx.bordx.Id;
            if (!($scope.ClaimProcessbordx.insurerId != emptyGuid() && $scope.ClaimProcessbordx.insurerId != "" && $scope.ClaimProcessbordx.reinsurerId != emptyGuid() && $scope.ClaimProcessbordx.reinsurerId != ""
                   && $scope.ClaimProcessbordx.claimBordxId != emptyGuid() && $scope.ClaimProcessbordx.claimBordxId != "" && $scope.ClaimProcessbordx.year != "" && $scope.ClaimProcessbordx.month != "")) {
                retVal = false;
                $scope.errorTab1 = "Please Select Required Fields";
            }
            return retVal;
        }

        $scope.Process = function () {
            if (validate()) {
                swal({ title: "TAS Information", text: "Processing. Please wait ...", showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claimbordxprocess/ClaimBordxProcess',
                    data: { 'claimbordxId': $scope.ClaimProcessbordx.bordx.Id, 'isProcess': true, 'userId': emptyGuid() },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data) {
                        customInfoMessage("Process done !");
                        //Clear();
                        ClaimSelectionPopup.close();
                        $scope.canView = true;
                        $scope.canConfirm = true;
                        $scope.loadProcessedClaimBordx();
                        
                    }
                    else { customErrorMessage("Process Failed !"); }
                }).error(function (data, status, headers, config) {
                    swal.close();
                    customErrorMessage("Error ! Process Failed !");
                });
            }
        }

        $scope.Confirm = function () {
            swal({ title: "TAS Information", text: "Please wait ...", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claimbordxprocess/ClaimBordxCanConfirmedByYear',
                data: { 'year': $scope.ClaimProcessbordx.year, 'insurerid': $scope.ClaimProcessbordx.insurerId, 'reinsurerid': $scope.ClaimProcessbordx.reinsurerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data) {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claimbordxprocess/ClaimBordxProcessUpdate',
                        data: { 'claimbordxId': $scope.ClaimProcessbordx.bordx.Id, 'isConfirm': true, 'userId': emptyGuid() },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        swal.close();
                        if (data) {
                            customInfoMessage("Confirmed !");
                            Clear();
                        }
                        else { customErrorMessage("Confirmation Failed !"); }
                    }).error(function (data, status, headers, config) {
                        swal.close();
                        customErrorMessage("Error ! Confirmation Failed !");
                    });
                }
                else {
                    swal.close();
                    customErrorMessage("Error ! Can't Process.Please Do year end process for last year !");
                }
            }).error(function (data, status, headers, config) {
                swal.close();
            }).finally(function () { });
        }

        function canConfim() {

        }


        $scope.View = function () {
            swal({ title: "TAS Information", text: "Please wait ...", showConfirmButton: false  });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claimbordxprocess/ExportToExcelClaimBordxById',
                data: { 'claimbordxid': $scope.ClaimProcessbordx.bordx.Id },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
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
                }
                else {

                }
            }).error(function (data, status, headers, config) {
            }).finally(function () { swal.close(); });
        }

        ////supportive functions

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }

        function emptyGuid() { return "00000000-0000-0000-0000-000000000000"; }

        var customInfoMessage = function (msg) { toaster.pop('info', 'Information', msg, 12000); };

        var customErrorMessage = function (msg) { toaster.pop('error', 'Error', msg); };

    });