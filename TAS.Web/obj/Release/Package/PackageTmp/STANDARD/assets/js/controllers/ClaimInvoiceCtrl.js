app.controller('ClaimInvoiceCtrl',
    function ($scope, $rootScope, $http, ngDialog, $localStorage, SweetAlert, toaster, $filter, FileUploader) {

        $scope.ClaimInvoiceSaveBtnIconClass = "";
        $scope.ClaimInvoiceSaveBtnDisabled = false;
        $scope.claimSubmittedSearchClaimNo = "";
        $scope.claimSubmittedSearchInvoiceNo = "";
        $scope.ClaimInvoiceEntrySearchGridloadAttempted = false;
        $scope.ClaimInvoiceEntrySearchGridloading = false;

        $scope.ClaimInvoiceEntryClaimSearchGridloadAttempted = false;
        $scope.ClaimInvoiceEntryClaimSearchGridloading = false;
        $scope.ClaimInvoiceEntryDisable = false;

        $scope.claims = [];
        $scope.claims.prods = [];
        $scope.claims.prods.prod = [];
        $scope.claims.prods.color = [];
        $scope.selected = [];
        $scope.mySelectedRows = [];

        $scope.formAction = true;
        $scope.claimInvoiceSisableConfirm = true;
        $scope.saveUpdateClass = 'btn-info';
        $scope.saveText = "Save";
        $scope.readyToSave = false;
        $scope.readyToUpdate = false;
        $scope.claimInvoiceDisableSave = true;
        $scope.saveUpdateClass = 'btn-info';
        $scope.readyToConfirm = false;
        $scope.ClaimInvoiceEdjustDissable = true;
        $scope.currentAdjustClaimNo = '';
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        $scope.ClaimInvoiceEntry = {
            Id: emptyGuid(),
            DealerId: emptyGuid(),
            InvoiceReceivedDate: '',
            InvoiceDate: '',
            InvoiceNumber: '',
            InvoiceAmount: '',
            CurrencyId: emptyGuid(),
            CurrencyPeriodId: emptyGuid(),
            ConversionRate: 0.0,
            UserAttachmentId: emptyGuid(),
            TotalAmount: 0,
            IsConfirm: false,
            claims: []
        };

        function clearClaimInvoiceEntryControls() {
            $scope.ClaimInvoiceEntry.Id = "00000000-0000-0000-0000-000000000000";
            $scope.ClaimInvoiceEntry.DealerId = "00000000-0000-0000-0000-000000000000";
            $scope.ClaimInvoiceEntry.InvoiceReceivedDate = "";
            $scope.ClaimInvoiceEntry.InvoiceDate = "";
            $scope.ClaimInvoiceEntry.InvoiceNumber = "";
            $scope.ClaimInvoiceEntry.InvoiceAmount = "";
            $scope.ClaimInvoiceEntry.CurrencyId = "";
            $scope.ClaimInvoiceEntry.CurrencyPeriodId = "";
            $scope.ClaimInvoiceEntry.ConversionRate = 0.0;
            $scope.ClaimInvoiceEntry.UserAttachmentId =  emptyGuid();
            $scope.ClaimInvoiceEntry.TotalAmount = 0;
            $scope.ClaimInvoiceEntry.claims = [];
            $scope.claimInvoiceDisableSave = true;
            $scope.saveUpdateClass = 'btn-info';
            $scope.saveText = "Save";
            $scope.claimInvoiceSisableConfirm = true;
            $scope.ClaimInvoiceEdjustDissable = true;
            //$scope.ClaimInvoiceEntry.claims();
            $scope.formAction = true;//true for add new
            $scope.claims = [];
            $scope.gridOptions.data = [];
            $scope.gridOptions.totalItems = 0;

        };



        function clearAdjustmentDetails() {
            $scope.adjustment.beforadjustPartAmount = '';
            $scope.adjustment.beforadjustLabourAmount = '';
            $scope.adjustment.beforadjustSundryAmount = '';
            $scope.adjustment.approvedAmmount = '';
            $scope.adjustment.invoiceAmount = '';
            $scope.adjustment.claimIdAj = emptyGuid();
            $scope.ClaimInvoiceEdjustDissable = true;
            $scope.currentAdjustClaimNo = '';
            $scope.adjustPartAmount = '';
            $scope.adjustLabourAmount = '';
            $scope.adjustSundryAmount = '';

        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        var customWarningMessage = function (msg) {
            toaster.pop('warning', 'Warning', msg, 10000);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        $scope.loadInitailData = function () {
            LoadDetails();
        }

        function LoadDetails() {
            swal({ title: "TAS Information", text: 'Loading Dealer Details...', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
                swal.close();
            }).error(function (data, status, headers, config) {
                swal.close();
            });

            //$scope.refresClaimSearchGridData();
        }


        $scope.validateInvoice = function () {

            var isValid = true;

            if (!isGuid($scope.ClaimInvoiceEntry.DealerId)) {
                $scope.validate_claimInvoiceEntryDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimInvoiceEntryDealerId = "";
            }

            if ($scope.ClaimInvoiceEntry.InvoiceReceivedDate == "") {
                $scope.validate_claimInvoiceEntryInvoiceReceivedDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimInvoiceEntryInvoiceReceivedDate == "";
            }

            if ($scope.ClaimInvoiceEntry.InvoiceDate == "") {
                $scope.validate_claimInvoiceEntryInvoiceDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimInvoiceEntryInvoiceDate = "";
            }

            if ($scope.ClaimInvoiceEntry.InvoiceNumber == "") {
                $scope.validate_claimInvoiceEntryInvoiceNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimInvoiceEntryInvoiceNumber = "";
            }

            if ($scope.ClaimInvoiceEntry.InvoiceAmount == "") {
                $scope.validate_claimInvoiceEntryInvoiceAmount = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimInvoiceEntryInvoiceAmount = "";
            }
            if ($scope.mySelectedRows.length == 0) {
                isValid = false;
                customErrorMessage('Please select atlease one claim to create a Invoice.');
            }
            //if ($scope.ClaimInvoiceEntry.claims = []) {
            //    customErrorMessage("Please Select Claim.");
            //    isValid = false;
            //}
            return isValid
        }


        $scope.submitConfirmInvoice = function () {
            swal({ title: "TAS Information", text: 'Confirming Invoice Entry...', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimInvoice/ConfirmClaimInvoice',
                        data: $scope.ClaimInvoiceEntry,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        swal.close();
                        $scope.Ok = data;
                        if (data == "OK") {
                            clearClaimInvoiceEntryControls();
                            clearAdjustmentDetails()
                            SweetAlert.swal({
                                title: "Claim Invoice Information",
                                text: "Confirmed successfully!",
                                confirmButtonColor: "#007AFF"
                            });
                        } else {
                            SweetAlert.swal({
                                title: "Claim Invoice Information",
                                text: "Error occurred while Confirming !",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            return false;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        swal.close();
                        customErrorMessage("Error occurred while saving data!")
                        return false;
                    });
        }

        $scope.submitInvoice = function () {

            if ($scope.validateInvoice()) {

                if ($scope.ClaimInvoiceEntry.TotalAmount != $scope.ClaimInvoiceEntry.InvoiceAmount) {
                    swal({
                        title: "Are you sure? You want to Continue",
                        text: "Invoice Amount Mismatch with Selected Claim/s Authorized Amount.",
                        type: "warning",
                        showCancelButton: true,
                        //closeOnConfirm: true,
                        showLoaderOnConfirm: false,
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Yes, Continue!'
                    },
                        function () {
                            $scope.invoiceSubmitBody();
                        });
                } else {
                    $scope.invoiceSubmitBody();
                }


            } else {
                swal.close();
                customErrorMessage("Please fill valid data for highlighted fields.")
            }


        }

        $scope.invoiceSubmitBody = function () {

            $scope.ClaimInvoiceEntry.claims = $scope.mySelectedRows;
            if ($scope.readyToSave) {
                $scope.ClaimInvoiceSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ClaimInvoiceSaveBtnDisabled = true;
                swal({ title: "TAS Information", text: 'Saving Invoice Entry...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimInvoice/AddClaimInvoice',
                    data: $scope.ClaimInvoiceEntry,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    $scope.Ok = data;
                    $scope.ClaimInvoiceSaveBtnIconClass = "";
                    $scope.ClaimInvoiceSaveBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: "Claim Invoice Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Dealers = data;
                        }).error(function (data, status, headers, config) {
                        });
                        clearClaimInvoiceEntryControls();
                        clearAdjustmentDetails();

                    } else {
                        swal.close();
                        SweetAlert.swal({
                            title: "Claim Invoice Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    }

                    return false;
                }).error(function (data, status, headers, config) {
                    swal.close();
                    SweetAlert.swal({
                        title: "Claim Invoice Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ClaimInvoiceSaveBtnIconClass = "";
                    $scope.ClaimInvoiceSaveBtnDisabled = false;

                    return false;
                });

            } else {
                $scope.ClaimInvoiceSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ClaimInvoiceSaveBtnDisabled = true;
                swal({ title: "TAS Information", text: 'Updating Invoice Entry...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimInvoice/UpdateClaimInvoice',
                    data: $scope.ClaimInvoiceEntry,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    $scope.Ok = data;
                    $scope.ClaimInvoiceSaveBtnIconClass = "";
                    $scope.ClaimInvoiceSaveBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: "Claim Invoice Information",
                            text: "Successfully Updated!",
                            confirmButtonColor: "#007AFF"
                        });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Dealers = data;
                        }).error(function (data, status, headers, config) {
                        });
                        clearClaimInvoiceEntryControls();
                        clearAdjustmentDetails();
                    }
                    else {

                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    swal.close();
                    SweetAlert.swal({
                        title: "Claim Invoice Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ClaimInvoiceSaveBtnIconClass = "";
                    $scope.ClaimInvoiceSaveBtnDisabled = false;
                    return false;
                });
            }

        }

        //----------------------------------- Dealer Currency -------------------------------------

        $scope.selectedDealerChanged = function () {
            $scope.claims = [];

            if (isGuid($scope.ClaimInvoiceEntry.DealerId)) {
                angular.forEach($scope.Dealers, function (dealer) {
                    if ($scope.ClaimInvoiceEntry.DealerId == dealer.Id) {
                        $scope.ClaimInvoiceEntry.CurrencyPeriodId = dealer.CurrencyPeriodId;
                        $scope.ClaimInvoiceEntry.ConversionRate = dealer.ConversionRate;
                        if (isGuid(dealer.CurrencyId)) {
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                                data: { "Id": dealer.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.partPriceCurrency = data.Code;
                                $scope.ClaimInvoiceEntry.CurrencyId = data.Id;
                            });

                        } else {
                            customErrorMessage("Selected dealer's currency not found.");
                        }

                    }
                });

                if (isGuid($scope.ClaimInvoiceEntry.DealerId)) {
                    swal({ title: "TAS Information", text: 'Reading Claims...', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimInvoice/GetAllClaimByDealerId',
                        data: { "ClaimSubmittedDealerId": $scope.ClaimInvoiceEntry.DealerId, "ClaimNumber": $scope.claimSubmittedSearchClaimNo, "InvoiceNumber": $scope.claimSubmittedSearchInvoiceNo },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == null) customWarningMessage("Invoice Pending claims not found.");
                        $scope.claims = data;

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimInvoice/GetAllSubmittedInvoiceClaimByDealerId',
                            data: { "ClaimSubmittedDealerId": $scope.ClaimInvoiceEntry.DealerId, "ClaimNumber": $scope.claimSubmittedSearchClaimNo, "InvoiceNumber": $scope.claimSubmittedSearchInvoiceNo },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            swal.close();
                            if (data == null) customWarningMessage("Pending Confirm claims not found.");
                            if ($scope.claims == null) {
                                $scope.claims = data;
                            } else {
                                angular.forEach(data, function (value) {
                                    $scope.claims.push(value);
                                });
                            }

                            //$scope.claims.push(data);



                        }).error(function (data, status, headers, config) {
                            swal.close();
                        });


                    }).error(function (data, status, headers, config) {
                        swal.close();
                    });
                }


            } else {
                $scope.ClaimInvoiceEntry.DealerName = '';
                $scope.ClaimInvoiceEntry.CountryId = emptyGuid();
                $scope.partPriceCurrency = ''
            }

        }

        //----------------------------------- Search ----------------------
        var ClaimInvoiceEntryPopup;


        $scope.ClaimInvoiceEntrySearchGridSearchCriterias = {
            dealer: "",
            invoiceNo: "",
            invoiceDate: "",
            mobileNo: ""
        };


        var paginationOptionsClaimInvoiceEntrySearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };



        $scope.gridOptionsClaimInvoiceEntry = {

            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Dealer Name', field: 'DealerId', enableSorting: false, cellClass: 'columCss' },
                { name: 'Invoice Number', field: 'InvoiceNumber', enableSorting: false, cellClass: 'columCss', },
                { name: 'Invoice Date', field: 'InvoiceDate', enableSorting: false, cellClass: 'columCss' },
                { name: 'Invoice Amount', field: 'InvoiceAmount', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadClaimInvoiceEntry(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsClaimInvoiceEntrySearchGrid.sort = null;
                    } else {
                        paginationOptionsClaimInvoiceEntrySearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getClaimInvoiceEntrySearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimInvoiceEntrySearchGrid.pageNumber = newPage;
                    paginationOptionsClaimInvoiceEntrySearchGrid.pageSize = pageSize;
                    getClaimInvoiceEntrySearchPage();
                });
            }
        };


        $scope.refresUserSearchGridData = function () {
            getClaimInvoiceEntrySearchPage();
        }

        $scope.searchPopup = function () {
            getClaimInvoiceEntrySearchPage();

            $scope.ClaimInvoiceEntrySearchGridSearchCriterias = {
                dealer: '',
                invoiceNo: '',
                invoiceDate: '',
                mobileNo: ''
            };

            ClaimInvoiceEntryPopup = ngDialog.open({
                template: 'popUpClaimInvoiceEntry',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };

        $scope.loadClaimInvoiceEntry = function (Id) {
            if (isGuid(Id)) {
                ClaimInvoiceEntryPopup.close();
                $scope.formAction = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimInvoice/GetClaimInvoiceEntryById',
                    data: { "Id": Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.ClaimInvoiceEntry.Id = data.Id;
                    $scope.ClaimInvoiceEntry.DealerId = data.DealerId;
                    $scope.ClaimInvoiceEntry.InvoiceReceivedDate = data.InvoiceReceivedDate;
                    $scope.ClaimInvoiceEntry.InvoiceDate = data.InvoiceDate;
                    $scope.ClaimInvoiceEntry.InvoiceNumber = data.InvoiceNumber;
                    $scope.ClaimInvoiceEntry.InvoiceAmount = data.InvoiceAmount;
                    $scope.ClaimInvoiceEntry.CurrencyId = data.CurrencyId;
                    $scope.ClaimInvoiceEntry.CurrencyPeriodId = data.CurrencyPeriodId;
                    $scope.ClaimInvoiceEntry.ConversionRate = data.ConversionRate;
                    $scope.ClaimInvoiceEntry.UserAttachmentId = data.UserAttachmentId;
                    $scope.ClaimInvoiceEntry.IsConfirm = data.IsConfirm;

                    if ($scope.ClaimInvoiceEntry.IsConfirm == true) {
                        $scope.ClaimInvoiceEntryDisable = true;
                        $scope.ClaimInvoiceSaveBtnDisabled = true;
                    } else {
                        $scope.ClaimInvoiceEntryDisable = false;
                        $scope.ClaimInvoiceSaveBtnDisabled = false;
                    }

                }).error(function (data, status, headers, config) {
                    // clearCustomerControls();
                });
            }
        }


        var getClaimInvoiceEntrySearchPage = function () {

            $scope.ClaimInvoiceEntrySearchGridloadAttempted = true;
            $scope.ClaimInvoiceEntrySearchGridloading = false;

            var UserSearchGridParam =
            {
                'paginationOptionsClaimInvoiceEntrySearchGrid': paginationOptionsClaimInvoiceEntrySearchGrid,
                'ClaimInvoiceEntrySearchGridSearchCriterias': $scope.ClaimInvoiceEntrySearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimInvoice/GetAllClaimInvoiceEntryForSearchGrid',
                data: UserSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                $scope.gridOptionsClaimInvoiceEntry.data = response_arr.data;
                $scope.gridOptionsClaimInvoiceEntry.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.ClaimInvoiceEntrySearchGridloadAttempted = true;
                $scope.ClaimInvoiceEntrySearchGridloading = false;

            });
        }


        //------------------------- Grid for Cliam Details -------------------------

        $scope.showClaimId = function (id, isChecked, index) {
            console.log("index:" + index + " " + isChecked);

            if (isChecked) {
                $scope.selected.push(id);
            } else {
                var _index = $scope.selected.indexOf(id);
                $scope.selected.splice(_index, 1);
            }
        };

        $scope.claimInvoiceEntryClaimSearchGridSearchCriterias = {

            ClaimNumber: "",
            TotalClaimAmount: "",
            LastUpdatedDate: "",
            DealerId: $scope.ClaimInvoiceEntry.DealerId
            //invoiceNo: "",
            //invoiceDate: "",
            //mobileNo: ""
        };


        var paginationOptionsClaimInvoiceEntryClaimSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };



        $scope.gridOptions = {
            data: 'claims',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: true,
            enableSelectAll: true,
            enableRowHeaderSelection: true,
            columnDefs: [
                { field: "Id", displayName: "Id", width: 150, visible: false },
                { field: "ClaimInvoiceEntryId", displayName: "ClaimInvoiceEntryId", width: 150, visible: false },
                { field: "InvoiceNumber", displayName: "Invoice No", width: 100 },
                { field: "ClaimNumber", displayName: "Claim No", width: 110 },
                { field: "TotalClaimAmount", displayName: "Authorized Amount", width: 150, cellFilter: 'number: 2', visible: false },
                { field: "TotalClaimAmount2", displayName: "Authorized Amount", width: 150, cellFilter: 'number: 2' },
                { field: "LastUpdatedDate", displayName: "Authorized Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\'' },
                {
                    name: '   ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadInvoiceDataById(row.entity)" class="btn btn-xs btn-info">adjust</button></div>',
                    width: 60,
                    enableSorting: false
                }
                //{
                //    name: ' ',
                //    cellTemplate: '<div class="center"><input type="checkbox" id="IsInvoiced" ng-model="checked" ng-change="showClaimId(p.Id,checked, $index)" class="checkbox"/></div>',
                //    width: 60,
                //    enableSorting: false
                //}
            ],

            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsClaimInvoiceEntryClaimSearchGrid.sort = null;
                    } else {
                        paginationOptionsClaimInvoiceEntryClaimSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                  //  getClaimInvoiceEntryClaimSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimInvoiceEntryClaimSearchGrid.pageNumber = newPage;
                    paginationOptionsClaimInvoiceEntryClaimSearchGrid.pageSize = pageSize;
                   // getClaimInvoiceEntryClaimSearchPage();
                });
                gridApi.selection.on.rowSelectionChanged($scope, calcualteTotal);
                gridApi.selection.on.rowSelectionChangedBatch($scope, calcualteTotal);
            }
        };

        function calcualteTotal() {
            $scope.ClaimInvoiceEntry.TotalAmount = 0;
            $scope.ClaimInvoiceEntry.claims = [];
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            if ($scope.mySelectedRows.length != 0) {
                $scope.ClaimInvoiceEntry.TotalAmount = 0;


                angular.forEach($scope.mySelectedRows, function (value) {
                    var claimObj = {
                        Id: value.Id,
                        TotalClaimAmount: value.TotalClaimAmount2 ,
                        ClaimNumber: value.ClaimNumber ,
                        Amount: value.InvoiceAmount
                    };
                    $scope.ClaimInvoiceEntry.claims.push(claimObj);
                    $scope.ClaimInvoiceEntry.TotalAmount = $scope.ClaimInvoiceEntry.TotalAmount + parseFloat(value.TotalClaimAmount2);
                    $scope.ClaimCurrencyCode = value.ClaimCurrencyCode;

                });

                //console.log($scope.mySelectedRows);
            } else {
                //$scope.ClaimInvoiceEntry.InvoiceReceivedDate = "";
                //$scope.ClaimInvoiceEntry.InvoiceDate = "";
                //$scope.ClaimInvoiceEntry.InvoiceNumber = "";
                //$scope.ClaimInvoiceEntry.InvoiceAmount = "";
            }
            $scope.buttonVisibilityForSaveUpdateConfirm();
        }

        $scope.buttonVisibilityForSaveUpdateConfirm = function () {
            $scope.readyToSave = false;
            $scope.readyToUpdate = false;
            $scope.claimInvoiceDisableSave = true;
            $scope.readyToConfirm = false;
            $scope.claimInvoiceSisableConfirm = true;
            var len = $scope.mySelectedRows.length;
            //$scope.ClaimInvoiceEntry.InvoiceReceivedDate = "";
            //$scope.ClaimInvoiceEntry.InvoiceDate = "";
            //$scope.ClaimInvoiceEntry.InvoiceNumber = "";
            //$scope.ClaimInvoiceEntry.InvoiceAmount = "";
            //$scope.ClaimInvoiceEntry.Id = "00000000-0000-0000-0000-000000000000";
            var invoiceNumberFilter = $scope.mySelectedRows.filter(a => a.InvoiceNumber == "N/A");

            if (len>0&& len == invoiceNumberFilter.length) {
                console.log('ready to save');
                $scope.saveText = "Save";
                $scope.readyToSave = true;
                $scope.claimInvoiceDisableSave = false;
                $scope.saveUpdateClass = 'btn-info';
            }
            var newClaims = $scope.mySelectedRows.filter(a => a.InvoiceNumber == "N/A");
            var invicedClaims = $scope.mySelectedRows.filter(a => a.InvoiceNumber != "N/A");
            var sameInvoiceNumber = invicedClaims.filter(a => a.InvoiceNumber == invicedClaims[0].InvoiceNumber);
            if (len > 0 && sameInvoiceNumber.length>0 && (newClaims.length + sameInvoiceNumber.length) == len) {
                $scope.saveText = "Update";
                $scope.readyToUpdate = true;
                $scope.claimInvoiceDisableSave = false;
                $scope.saveUpdateClass = 'btn-warning';
                $scope.ClaimInvoiceEntry.InvoiceReceivedDate = sameInvoiceNumber[0].InvoiceReceivedDate;
                $scope.ClaimInvoiceEntry.InvoiceDate = sameInvoiceNumber[0].InvoiceDate;
                $scope.ClaimInvoiceEntry.InvoiceNumber = sameInvoiceNumber[0].InvoiceNumber;
                $scope.ClaimInvoiceEntry.InvoiceAmount = sameInvoiceNumber[0].InvoiceAmount;
                $scope.ClaimInvoiceEntry.Id = sameInvoiceNumber[0].ClaimInvoiceEntryId;
            }

            if (len > 0 && sameInvoiceNumber.length == len) {
                $scope.readyToConfirm = true;
                $scope.claimInvoiceSisableConfirm = false;
            }

        }

        $scope.refresClaimSearchGridData = function () {
            getClaimInvoiceEntryClaimSearchPage();
        }

        var getClaimInvoiceEntryClaimSearchPage = function () {

            $scope.ClaimInvoiceEntryClaimSearchGridloadAttempted = true;
            $scope.ClaimInvoiceEntryClaimSearchGridloading = false;

            $scope.claimInvoiceEntryClaimSearchGridSearchCriterias.DealerId = $scope.ClaimInvoiceEntry.DealerId;

            var UserSearchGridParam =
            {
                'paginationOptionsClaimInvoiceEntryClaimSearchGrid': paginationOptionsClaimInvoiceEntryClaimSearchGrid,
                'claimInvoiceEntryClaimSearchGridSearchCriterias': $scope.claimInvoiceEntryClaimSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimInvoice/GetAllClaimInvoiceEntryClaimForSearchGrid',
                data: UserSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptions.data = response_arr.data;
                $scope.gridOptions.totalItems = response_arr.totalRecords;
                $scope.claims = [];
                $scope.claims = response_arr.data;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.ClaimInvoiceEntryClaimSearchGridloadAttempted = true;
                $scope.ClaimInvoiceEntryClaimSearchGridloading = false;

            });
        }

        //$scope.claims = {};
        $scope.adjustment = {
            beforadjustPartAmount: 0,
            beforadjustLabourAmount: 0,
            beforadjustSundryAmount:0,
            approvedAmmount: '',
            invoiceAmount: '',
            claimIdAj: emptyGuid()
        };

        $scope.adjustPartAmount = 0;
        $scope.adjustLabourAmount = 0;
        $scope.adjustSundryAmount = 0;

        $scope.loadInvoiceDataById = function (entry) {
            if (isGuid(entry.Id)) {
                swal({ title: "TAS Information", text: 'Loading For Adjustment...', showConfirmButton: false });
                $scope.adjustment.beforadjustPartAmount = '';
                $scope.adjustment.beforadjustLabourAmount = '';
                $scope.adjustment.beforadjustSundryAmount = '';
                $scope.adjustment.approvedAmmount = '';
                $scope.adjustment.invoiceAmount ='';
                $scope.adjustment.claimIdAj = emptyGuid();
                $scope.ClaimInvoiceEdjustDissable = true;
                $scope.currentAdjustClaimNo = '';
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimInvoice/GetAllClaimPartDetailsByClaimId',
                    data: { "ClaimId": entry.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data.error != null) {
                        customErrorMessage(data.error);
                    } else {
                        $scope.adjustment.beforadjustPartAmount = data.PartAmount;
                        $scope.adjustment.beforadjustLabourAmount = data.LabourAmount;
                        $scope.adjustment.beforadjustSundryAmount = data.SundryAmount;
                        $scope.adjustment.approvedAmmount = data.ClaimApprovedAmount;
                        $scope.adjustment.invoiceAmount = data.InvoiceAmount;
                        $scope.adjustment.claimIdAj = data.ClaimId;
                        $scope.ClaimInvoiceEdjustDissable = false;
                        $scope.currentAdjustClaimNo = ' - [ Claim No :  ' + entry.ClaimNumber + ' , Invoice No : ' + entry.InvoiceNumber +' ]';
                    }

                    //$scope.claims.push(data);
                }).error(function (data, status, headers, config) {
                    $scope.ClaimInvoiceEdjustDissable = true;
                    swal.close();
                });
            }
        }

        $scope.submitAdjustAmount = function () {
            if ($scope.adjustment.claimIdAj != emptyGuid()) {
                swal({ title: "Processing", text: 'Submitting to Adjustment...', showConfirmButton: false });
                $scope.ClaimInvoiceAjusmentSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.ClaimInvoiceAjusmentBtnDisabled = true;

                data = {
                    'ClaimId': $scope.adjustment.claimIdAj,
                    'adjustPartAmount': $scope.adjustPartAmount,
                    'adjustLabourAmount': $scope.adjustLabourAmount,
                    'adjustSundryAmount': $scope.adjustSundryAmount
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimInvoice/AddAjusments',
                    data: data, //{ 'claims': $scope.selected, 'ClaimInvoiceEntry': $scope.ClaimInvoiceEntry },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    $scope.Ok = data;
                    $scope.ClaimInvoiceAjusmentSaveBtnIconClass = "";
                    $scope.ClaimInvoiceAjusmentBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: "Ajusments",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Dealers = data;
                        }).error(function (data, status, headers, config) {
                        });
                        clearClaimInvoiceEntryControls();
                        $scope.beforadjustPartAmount = 0;
                        $scope.beforadjustLabourAmount = 0;
                        $scope.beforadjustSundryAmount = 0;
                        $scope.adjustPartAmount = 0;
                        $scope.adjustLabourAmount = 0;
                        $scope.adjustSundryAmount = 0;
                        $scope.adjustment.approvedAmmount = '';
                        $scope.adjustment.invoiceAmount = '';
                        $scope.adjustment.beforadjustPartAmount = '';
                        $scope.adjustment.beforadjustLabourAmount = '';
                        $scope.adjustment.beforadjustSundryAmount = '';
                        $scope.currentAdjustClaimNo = '';
                    } else {
                        SweetAlert.swal({
                            title: "Claim Invoice Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    }

                    return false;
                }).error(function (data, status, headers, config) {
                    swal.close();
                    SweetAlert.swal({
                        title: "Claim Invoice Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.ClaimInvoiceAjusmentSaveBtnIconClass = "";
                    $scope.ClaimInvoiceAjusmentBtnDisabled = false;

                    return false;
                });

            }
        }

        //$scope.orders.prods.prod = {};
        $scope.sum = function () {
            return $scope.claims.filter(function (claims) {
                return claims.Id;
            }).reduce(function (subtotal, selectedProd) {
                return subtotal + parseInt(selectedProd.TotalClaimAmount);
            }, 0);
        };



    });