app.controller('DealerChequesCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, toaster) { //, $cookieStore, $filter

        $scope.errorTab1 = "";

        $scope.IsExistsCheque = false;
        $scope.searchGridloading = false;
        $scope.gridloadAttempted = false;

        $scope.ClaimChequePayment = {
            Id: emptyGuid(),
            ChequeNo: '',
            ChequeAmount: '',
            ChequeDate: '',
            Country: '',
            Dealer: '',
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            IssuedDate: '',
            Total: '',
            Comment: '',
            ClaimChequePaymentDetails: []
        };

        createGrid();

        clear();
        $scope.DealerChequeGridloading = false;
        $scope.DealerChequeGridloadAttempted = false;
        LoadDetails();

        //////---------------------------------------------------------------------------------------------------------------------------------------------------------

        function createGrid() {
            $scope.List = [];
            $scope.ColumnList = [{ field: "ClaimBatchGroupId", displayName: "", width: 0, visible: false },
                            { field: "BatchNumber", displayName: "Batch No", width: 250 },
                            { field: "BatchDate", displayName: "Batch Date", width: 200, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\'' },
                            { field: "GroupName", displayName: "Group", width: 300 },
                            { field: "Amount", displayName: "Amount", width: 250, cellFilter: 'number: 2' }];
            $scope.gridOptions = {
                data: 'List',
                paginationPageSizes: [5, 10, 20],
                paginationPageSize: 10,
                enablePaginationControls: true,
                enableRowSelection: true,
                enableCellSelection: true,
                enableCellEditOnFocus: true,
                enableSelectAll: true,
                enableRowHeaderSelection: true,
                enableColumnResizing: true,
                columnDefs: $scope.ColumnList
            };

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, calcualteTotal);
            }
        }

        function calcualteTotal() {
            $scope.ClaimChequePayment.Total = '';
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            if ($scope.mySelectedRows.length != 0) {
                $scope.ClaimChequePayment.Total = 0;
                angular.forEach($scope.mySelectedRows, function (value) {
                    $scope.ClaimChequePayment.Total = $scope.ClaimChequePayment.Total + parseFloat(value.Amount);
                });
            }
        }

        function clear() {
            $scope.errorTab1 = "";
            $scope.IsExistsCheque = false;
            $scope.List = [];
            $scope.ClaimChequePayment = {
                Id: emptyGuid(),
                ChequeNo: '',
                ChequeAmount: '',
                ChequeDate: '',
                Country: '',
                Dealer: '',
                CountryId: emptyGuid(),
                DealerId: emptyGuid(),
                IssuedDate: '',
                Total: '',
                Comment: '',
                ClaimChequePaymentDetails: []
            }
        }

        function LoadDetails() {
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
                url: '/TAS.Web/api/Country/GetAllActiveCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });
        }

        function validate() {
            var retVal = true;
            $scope.errorTab1 = "";
            if (!($scope.ClaimChequePayment.CountryId != "00000000-0000-0000-0000-000000000000" && $scope.ClaimChequePayment.CountryId != "" && $scope.ClaimChequePayment.DealerId != "00000000-0000-0000-0000-000000000000" && $scope.ClaimChequePayment.DealerId != "")) {
                retVal = false;
                $scope.errorTab1 = "Please Select Country And Dealer";
            }
            if (retVal) {
                if (!($scope.ClaimChequePayment.ChequeNo != "" && $scope.ClaimChequePayment.ChequeAmount != "" && $scope.ClaimChequePayment.ChequeAmount != 0 && $scope.ClaimChequePayment.ChequeDate != "" && $scope.ClaimChequePayment.IssuedDate != "")) {
                    retVal = false;
                    $scope.errorTab1 = "Please Enter Cheque Details";
                }
            }

            if (retVal) {
                if ($scope.IsExistsCheque) {
                    retVal = false;
                    $scope.errorTab1 = "Cheque No Is Already Exist";
                }
            }

            if (retVal) {
                $scope.ClaimChequePayment.ClaimChequePaymentDetails = $scope.gridApi.selection.getSelectedRows();
                if ($scope.ClaimChequePayment.ClaimChequePaymentDetails.length == 0) {
                    retVal = false;
                    $scope.errorTab1 = "Please Select at least one row to save";
                }
            }
            if (retVal) {
                if ($scope.ClaimChequePayment.ChequeAmount != $scope.ClaimChequePayment.Total) {
                    retVal = false;
                    $scope.errorTab1 = "Cheque amount and total ammount should be same";
                }
            }
            return retVal;
        }

        $scope.chequeNoIsExsits = function () {
            $scope.errorTab1 = "";
            if ($scope.ClaimChequePayment.ChequeNo != undefined && $scope.ClaimChequePayment.ChequeNo != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ClaimChequeNoIsExists',
                    data: { "chequeNo": $scope.ClaimChequePayment.ChequeNo },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsCheque = data;
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                });
            }
        }

        $scope.GetGetPendingClaimGroups = function () {
            $scope.DealerChequeGridloading = true;
            $scope.DealerChequeGridloadAttempted = false;
            $scope.errorTab1 = "";
            if ($scope.ClaimChequePayment.CountryId != "00000000-0000-0000-0000-000000000000" && $scope.ClaimChequePayment.CountryId != "" && $scope.ClaimChequePayment.DealerId != "00000000-0000-0000-0000-000000000000" && $scope.ClaimChequePayment.DealerId != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetPendingClaimGroupsByDealer',
                    data: { "countryId": $scope.ClaimChequePayment.CountryId, "dealerId": $scope.ClaimChequePayment.DealerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.List = [];
                    if (data == null || data.length == 0) {
                     
                        //customInfoMessage("No Data");
                    }
                    else {
                    $scope.List = data;
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                   
                    $scope.DealerChequeGridloading = false;
                    $scope.DealerChequeGridloadAttempted = true;
                });
            }
        }

        $scope.Save = function () {
            if (validate()) {
                swal({ title: "TAS Information", text: "saving data. Please wait ...", showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/AddClaimChequePayment',
                    data: $scope.ClaimChequePayment,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data == "OK") {
                        customInfoMessage("Successfully Saved!");
                        clear();
                    }
                    else { customErrorMessage(data); }
                }).error(function (data, status, headers, config) {
                    swal.close();
                    customErrorMessage("Save Failed !");
                });
            }

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

        ////------------------------------------------------------------------------------------------------------------------------------------------------------------
        ////-------------------------------------------------  Search --------------------------------------------------------------------------------------------------

        $scope.chequeSearchGridSearchCriterias = {
            chequeNo: "",
            chequeDate: "",
            ChequeAmount: ""
        };

        var paginationOptionsSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.gridOptionsSearch = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'Cheque No', field: 'ChequeNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Cheque Date', field: 'ChequeDate', enableSorting: false, cellClass: 'columCss' },//, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\'' },
              { name: 'Cheque Amount', field: 'ChequeAmount', enableSorting: false, cellClass: 'columCss' },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadCheque(row.entity.Id)" class="btn btn-xs btn-warning">View</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsSearchGrid.sort = null;
                    } else {
                        paginationOptionsSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsSearchGrid.pageNumber = newPage;
                    paginationOptionsSearchGrid.pageSize = pageSize;
                    getSearchPage();
                });
            }
        };

        var getSearchPage = function () {
            $scope.searchGridloading = true;
            $scope.gridloadAttempted = false;
            var searchGridParam =
                {
                    'paginationOptionsSearchGrid': paginationOptionsSearchGrid,
                    'searchGridSearchCriterias': $scope.chequeSearchGridSearchCriterias
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllChequesForSearchGrid',
                data: searchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsSearch.data = response_arr.data;
                $scope.gridOptionsSearch.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.searchGridloading = false;
                $scope.gridloadAttempted = true;
            });
        };

        $scope.refresSearchGridData = function () { getSearchPage(); }

        $scope.loadCheque = function (chequePaymentId) {
            if (isGuid(chequePaymentId)) {
                SearchPopup.close();
                try{
                    $scope.downloadAttachment(chequePaymentId);
                }
                catch (ex) {
                    var test = ex;
                }
                
            }
        }

        $scope.searchPopupReset = function () {
            $scope.chequeSearchGridSearchCriterias = {
                chequeNo: "",
                chequeDate: "",
                ChequeAmount: ""
            }
        }

        $scope.searchPopup = function () {
            $scope.chequeSearchGridSearchCriterias = {
                chequeNo: "",
                chequeDate: "",
                ChequeAmount: ""
            };
            var paginationOptionsSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getSearchPage();
            SearchPopup = ngDialog.open({
                template: 'popUpSearchDealerCheque',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });


        };

        $scope.downloadAttachment = function (chequePaymentId) {

            swal({ title: 'Processing...!', text: 'Preparing your download...', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetChequeAttachmentById',
                data: { "chequePaymentId": chequePaymentId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
               , responseType: 'arraybuffer',
            }).success(function (data, status, headers, config) {
                try {
                    var octetStreamMime = 'application/octet-stream';
                    var success = false;

                    // Get the headers
                    headers = headers();

                    // Get the filename from the x-filename header or default to "download.bin"
                    var filename = headers['x-filename'] || 'Policy.pdf';

                    // Determine the content type from the header or default to "application/octet-stream"
                    var contentType = headers['content-type'] || octetStreamMime;

                    try{
                        var pdfAsDataUri = "data:application/pdf;base64,"+data;
                        window.open(pdfAsDataUri);
                    }
                    catch (ex) {
                        console.log("saveBlob method failed with the following exception:");
                        console.log(ex);
                    }

                    try {
                        // Try using msSaveBlob if supported
                        console.log("Trying saveBlob method ...");
                        var blob = new Blob([data], { type: contentType });
                        if (navigator.msSaveBlob)
                            navigator.msSaveBlob(blob, filename);
                        else {
                            // Try using other saveBlob implementations, if available
                            var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                            if (saveBlob === undefined) throw "Not supported";
                            saveBlob(blob, filename);
                        }
                        console.log("saveBlob succeeded");
                        success = true;
                    } catch (ex) {
                        console.log("saveBlob method failed with the following exception:");
                        console.log(ex);
                    }

                    if (!success) {
                        // Get the blob url creator
                        var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                        if (urlCreator) {
                            // Try to use a download link
                            var link = document.createElement('a');
                            if ('download' in link) {
                                // Try to simulate a click
                                try {
                                    // Prepare a blob URL
                                    console.log("Trying download link method with simulated click ...");
                                    var blob = new Blob([data], { type: contentType });
                                    var url = urlCreator.createObjectURL(blob);
                                    link.setAttribute('href', url);

                                    // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                    link.setAttribute("download", filename);

                                    // Simulate clicking the download link
                                    var event = document.createEvent('MouseEvents');
                                    event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                                    link.dispatchEvent(event);
                                    console.log("Download link method with simulated click succeeded");
                                    success = true;

                                } catch (ex) {
                                    console.log("Download link method with simulated click failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                            if (!success) {
                                // Fallback to window.location method
                                try {
                                    // Prepare a blob URL
                                    // Use application/octet-stream when using window.location to force download
                                    console.log("Trying download link method with window.location ...");
                                    var blob = new Blob([data], { type: octetStreamMime });
                                    var url = urlCreator.createObjectURL(blob);
                                    window.location = url;
                                    console.log("Download link method with window.location succeeded");
                                    success = true;
                                } catch (ex) {
                                    console.log("Download link method with window.location failed with the following exception:");
                                    console.log(ex);
                                }
                            }

                        }
                    }

                    if (!success) {
                        console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                        window.open(httpPath, '_blank', '');
                    }
                }
                catch (ex) {

                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }


        ////----------------------------------------------



    });