app.controller('ClaimBatchingCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert,$parse, $localStorage, $cookieStore, $filter, toaster, $interval, $timeout) {

        $scope.ClaimBatchingSaveBtnIconClass = "";
        $scope.ClaimBatchingSaveBtnDisabled = false;

        $scope.ClaimBatchingSearchGridloadAttempted = false;
        $scope.ClaimBatchingSearchGridloading = false;

        $scope.tbBatch;
        $scope.tbGroup;
        $scope.selectedBatch = "N/A";
        $scope.ClaimId = [];
        $scope.selected = [];
        $scope.mySelectedRows = [];
        $scope.tab = [{ active: true }, { active: false }, { active: false }];

     
        

        $scope.showGroupTab = function (batch) {
            
            $scope.tab[1].active = true;  
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetClaimBatchingById',
                data: { "Id": batch.Id },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimBatchGroup.CountryId = data.CountryId;
                $scope.claimBatchGroup.DealerId = data.DealerId;
                $scope.claimBatchGroup.InsurerId = data.InsurerId;
                $scope.claimBatchGroup.ReinsurerId = data.ReinsurerId;
                $scope.refreshClaimBatchDetails();
                $scope.claimBatchGroup.ClaimBatchId = batch.Id;
                $scope.selectedClaimBatchChanged();
               // $scope.getNextClaimGroupNumber();
               // $scope.getAllEligibleClaimsByBatchId();
                
                
            }).error(function (data, status, headers, config) {

            });
           
        }
        
       


        $scope.claimBatchGroup = {
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            InsurerId: emptyGuid(),
            ReinsurerId: emptyGuid(),
            ClaimBatchId: emptyGuid(),
            GroupId: emptyGuid(),
            SelectedClaims: [],
            IsGoodwill:false
        }
        $scope.ClaimBatchesInGroup = [];
        $scope.claimGroupsInBatch = [];

        createGrid();

        // Group Seq No..........
        function extractNumberFromString(Stringval) {
            var numb = Stringval.match(/\d/g);
            numb = numb.join("");
            return numb;
        }

        //function getGridData() {
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/ClaimBatching/GetAllClaimDetailsByGroupID',
        //        data: { "ClaimBatchGroup": $scope.ClaimBatchGroup.Id },
        //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        // $scope.claimssBatching = data;
        //$scope.RowstoSelect = data;
        //$http({
        //    method: 'POST',
        //    url: '/TAS.Web/api/ClaimBatching/GetAllClaimDetails',
        //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        //}).success(function (data, status, headers, config) {

        //    angular.forEach(data, function (value) {
        //        $scope.claimssBatching.push(value);
        //    });
        //    $scope.ClaimBatchGroup.ClaimGroupClaims = [];
        //    for (i = 0; i < $scope.RowstoSelect.length; i++) {
        //        $scope.gridApi.selection.selectRow($scope.claimssBatching[i]);
        //    }

        //}).error(function (data, status, headers, config) {
        //});
        //    }).error(function (data, status, headers, config) {
        //    });

        //}

        $scope.setSelectedClaims = function () {
            $scope.ClaimBatchGroup.ClaimGroupClaims = [];
            $scope.selected = [];
            //  console.log($scope.selected);
            // console.log($scope.ClaimBatchGroup.ClaimGroupClaims);
            if ($scope.mySelectedRows.length != 0) {
                angular.forEach($scope.mySelectedRows, function (value) {
                    $scope.selected.push({ "ClaimId": value.Id, "Amount": parseFloat(value.Amount), "Comment": value.Comment });
                });
            }
            $scope.ClaimBatchGroup.ClaimGroupClaims = $scope.selected;
        };

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };

        function emptyGuid() { return "00000000-0000-0000-0000-000000000000"; }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        $scope.claimBatch = {
            Id: emptyGuid(),
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            InsurerId: emptyGuid(),
            ReinsurerId: emptyGuid(),
            BatchNumber: '',
        };

        function clearClaimBatchEntryControls() {
            $scope.claimBatch.Id = "00000000-0000-0000-0000-000000000000";
            $scope.claimBatch.CountryId = "00000000-0000-0000-0000-000000000000";
            $scope.claimBatch.DealerId = "00000000-0000-0000-0000-000000000000";
            $scope.claimBatch.InsurerId = "00000000-0000-0000-0000-000000000000";
            $scope.claimBatch.ReinsurerId = "00000000-0000-0000-0000-000000000000";
            $scope.claimBatch.BatchNumber = "";
            $scope.claimBatch.Prefix = '';
            $scope.batchPrefix = '';
        };

        $scope.loadInitailData = function () {
            LoadDetails();
        }

        $scope.batchPrefix = '';
        $scope.setupClaimBatchPrefix = function () {
            $scope.batchPrefix = '';
            angular.forEach($scope.Countries, function (country) {
                if (isGuid($scope.claimBatch.CountryId)) {
                    if ($scope.claimBatch.CountryId === country.Id) {
                        $scope.batchPrefix += country.CountryCode + "/"
                    }
                }
            });

            angular.forEach($scope.Dealers, function (dealer) {
                if (isGuid($scope.claimBatch.DealerId)) {
                    if ($scope.claimBatch.DealerId === dealer.Id) {
                        $scope.batchPrefix += dealer.DealerCode + "/"
                    }
                }
            });

            angular.forEach($scope.Insurers, function (insurer) {
                if (isGuid($scope.claimBatch.InsurerId)) {
                    if ($scope.claimBatch.InsurerId === insurer.Id) {
                        $scope.batchPrefix += insurer.InsurerCode + "/"
                    }
                }
            });

            angular.forEach($scope.Reinsurers, function (reInsurer) {
                if (isGuid($scope.claimBatch.ReinsurerId)) {
                    if ($scope.claimBatch.ReinsurerId === reInsurer.Id) {
                        $scope.batchPrefix += reInsurer.ReinsurerName + "/"
                    }
                }
            });
            $scope.getNextBatchNumber();
            $scope.getAllClaimBatches();
        }
        $scope.getNextBatchNumber = function () {
            if (isGuid($scope.claimBatch.ReinsurerId) && isGuid($scope.claimBatch.InsurerId) &&
                isGuid($scope.claimBatch.DealerId) && isGuid($scope.claimBatch.CountryId) &&
                isGuid($scope.claimBatch.CountryId)) {
                var claimDetails = {
                    reinsurerId: $scope.claimBatch.ReinsurerId,
                    insurerId: $scope.claimBatch.InsurerId,
                    dealerId: $scope.claimBatch.DealerId,
                    countryId: $scope.claimBatch.CountryId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBatching/GetNextBatchNumber',
                    data: claimDetails,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data === 'invalid') {
                        customErrorMessage('Error occured while reading next batch number.');
                    } else {
                        $scope.claimBatch.BatchNumber = data;
                    }
                }).error(function (data, status, headers, config) {

                });
            }
        }
        $scope.claimsToBatch = [];
        function createGrid() {

            $scope.ColumnList = [{ field: "Id", displayName: "", width: 0, visible: false, enableCellEdit: false },
            { field: "ClaimNumber", displayName: "Claim No", width: 150, enableCellEdit: false },
            { field: "TotalClaimAmount", displayName: "Claim Amount", width: 150, cellFilter: 'number: 2', enableCellEdit: false },
            { field: "Amount", displayName: "Amount", width: 150, cellFilter: 'number: 2' },
            { field: "Comment", displayName: "Comment", width: 200 }
            ]; //, cellEditableCondition: setEnable
            $scope.gridOptions = {
                data: 'claimsToBatch',
                paginationPageSizes: [5, 10, 20],
                paginationPageSize: 10,
                enablePaginationControls: true,
                enableRowSelection: true,
                enableCellSelection: true,
                enableCellEditOnFocus: true,
                enableSelectAll: true,
                enableRowHeaderSelection: true,
                enableColumnResizing: true,
                cellEditableCondition: function (e) {
                    // put your enable-edit code here, using values from $scope.row.entity and/or $scope.col.colDef as you desire
                    //return $scope.row.entity.IsEditable; // in this example, we'll only allow editable rows to be edited
                    $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
                    if ($scope.mySelectedRows.length > 0) if ($filter('filter')($scope.mySelectedRows, { Id: e.row.entity.Id }).length == 1) return true;
                    return false;
                },
                columnDefs: $scope.ColumnList
            };

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, calcualteTotal);
                gridApi.edit.on.afterCellEdit($scope, calcualteTotal, this);
            }
        }

        function calcualteTotal(e) {
            var row = e.entity;
            if (row != undefined) { if (e.isSelected) { row.Amount = row.TotalClaimAmount; } else row.Amount = 0; }
            else { if (isNaN(parseFloat(e.Amount))) e.Amount = 0; if (parseFloat(e.Amount) > e.TotalClaimAmount) e.Amount = e.TotalClaimAmount; }

            $scope.claimBatchGroup.TotalAmount = 0;
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            if ($scope.mySelectedRows.length != 0) {
                $scope.claimBatchGroup.TotalAmount = 0;
                angular.forEach($scope.mySelectedRows, function (value) { $scope.claimBatchGroup.TotalAmount = $scope.claimBatchGroup.TotalAmount + parseFloat(value.Amount); });
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

            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Insurers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Reinsurers = data;
            }).error(function (data, status, headers, config) {
            });

            $scope.getAllClaimBatches();
        }

        $scope.getAllClaimBatches = function () {

            var claimDetails = {
                reinsurerId: $scope.claimBatch.ReinsurerId,
                insurerId: $scope.claimBatch.InsurerId,
                dealerId: $scope.claimBatch.DealerId,
                countryId: $scope.claimBatch.CountryId
            };

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetLast10BatchesBySearchCritera',
                data: claimDetails,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimBatching = data;
            }).error(function (data, status, headers, config) {
            });
        }

        function LoadSavedData() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetAllClaimBatching',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimBatching = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetAllGroupsById',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimBatchingGroup = data;
            }).error(function (data, status, headers, config) {
            });

            //$http({
            //    method: 'POST',
            //    //url: '/TAS.Web/api/ClaimBatching/GetAllClaimDetailsIsBachingFalse',
            //    url: '/TAS.Web/api/ClaimBatching/GetAllClaimDetails', //Load Claims by Group ID == null
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.claimssBatching = data;
            //}).error(function (data, status, headers, config) {
            //});
        }

        $scope.validateClaimBatch = function () {

            var isValid = true;

            if (!isGuid($scope.claimBatch.DealerId)) {
                $scope.validate_claimBatchEntryDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimBatchEntryDealerId = "";
            }

            if (!isGuid($scope.claimBatch.CountryId)) {
                $scope.validate_claimBatchEntryCountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimBatchEntryCountryId == "";
            }

            if (!isGuid($scope.claimBatch.InsurerId)) {
                $scope.validate_claimBatchEntryInsurerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimBatchEntryInsurerId = "";
            }

            if (!isGuid($scope.claimBatch.ReinsurerId)) {
                $scope.validate_claimBatchEntryReinsurerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimBatchEntryReinsurerId = "";
            }

            if ($scope.claimBatch.BatchNumber == "undefined") {
                $scope.validate_claimBatchEntryBatchNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_claimBatchEntryBatchNumber = "";
            }

            return isValid
        }

        $scope.submitClaimBatch = function () {
            //$scope.setSelectedClaims();
            if ($scope.validateClaimBatch()) {

                if ($scope.claimBatch.Id == null || $scope.claimBatch.Id == "00000000-0000-0000-0000-000000000000") {

                    $scope.ClaimBatchingSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ClaimBatchingSaveBtnDisabled = true;
                    $scope.claimBatch.Prefix = $scope.batchPrefix;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimBatching/AddClaimBatching',
                        data: $scope.claimBatch,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ClaimBatchingSaveBtnIconClass = "";
                        $scope.ClaimBatchingSaveBtnDisabled = false;
                        if (data == "ok") {
                            SweetAlert.swal({
                                title: "Claim Batching Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            clearClaimBatchEntryControls();
                            $scope.getAllClaimBatches();
                        } else {
                            SweetAlert.swal({
                                title: "Claim Batch Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Claim Batch Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ClaimBatchingSaveBtnIconClass = "";
                        $scope.ClaimBatchingSaveBtnDisabled = false;

                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }


        $scope.validateClaimBatchGroup = function () {
            var isValid = true;

            if ($scope.selectedBatch == "N/A") {
                $scope.validate_selectedBatch = "has-error";
                isValid = false;
            } else {
                $scope.validate_selectedBatch == "";
            }


            return isValid
        }


        $scope.submitClaimBatchGroup = function () {
            if ($scope.validateClaimBatchGroupDetails()) {
                $scope.claimBatchGroup.SelectedClaims = $scope.mySelectedRows;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBatching/SaveClaimBatchGroup',
                    data: {
                        'data': $scope.claimBatchGroup,
                        'requestedUserId': $rootScope.LoggedInUserId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'ok') {
                        swal({ title: "TAS Information", text: data.msg, showConfirmButton: true });
                        $scope.selectedClaimBatchChanged();
                    } else {
                        swal({ title: "TAS Information", text: data.msg, showConfirmButton: true, type: 'error' });
                    }

                }).error(function (data, status, headers, config) {
                });
            } else {
                customErrorMessage('Please fill all the mandatory fields');
            }

        }
        $scope.validateClaimBatchGroupDetails = function () {
            var isValid = true;
            if (!isGuid($scope.claimBatchGroup.ClaimBatchId)) {
                isValid = false;
                $scope.validate_claimBatchGroupClaimBatchId = "has-error";
            } else
                $scope.validate_claimBatchGroupClaimBatchId = "";

            if (!isGuid($scope.claimBatchGroup.GroupId)) {
                $scope.claimBatchGroup.GroupId = emptyGuid();
            }

            if ($scope.mySelectedRows.length == 0) {
                isValid = false;
                customErrorMessage('Please select atlease one claim to create a batch');
            }
            return isValid;

        }
        $scope.refreshClaimBatchDetails = function () {
            var batchDetails = {
                CountryId: $scope.claimBatchGroup.CountryId,
                DealerId: $scope.claimBatchGroup.DealerId,
                InsurerId: $scope.claimBatchGroup.InsurerId,
                ReinsurerId: $scope.claimBatchGroup.ReinsurerId
            }
            $scope.ClaimBatchesInGroup = [];

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetClaimBatchList',
                data: batchDetails,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ClaimBatchesInGroup = data;

            }).error(function (data, status, headers, config) {

            });
        }

        $scope.selectedClaimBatchChanged = function () {
            $scope.claimGroupsInBatch = [];
            $scope.claimsToBatch = [];
            $scope.claimBatchGroup.GroupName = '';
            $scope.claimBatchGroup.Comment = '';
            $scope.mySelectedRows = [];
            $scope.claimBatchGroup.TotalAmount = 0.00;

            if (isGuid($scope.claimBatchGroup.ClaimBatchId)) {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBatching/GetClaimGroupsByBatchId',
                    data: { 'BatchId': $scope.claimBatchGroup.ClaimBatchId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimGroupsInBatch = data;
                    $scope.getNextClaimGroupNumber();
                    $scope.getAllEligibleClaimsByBatchId();
                }).error(function (data, status, headers, config) {
                });
            } else {


            }
        }
        $scope.getAllEligibleClaimsByBatchId = function () {
            if (isGuid($scope.claimBatchGroup.ClaimBatchId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBatching/GetAllEligibleClaimsByBatchId',
                    data: { 'BatchId': $scope.claimBatchGroup.ClaimBatchId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimsToBatch = data;
                }).error(function (data, status, headers, config) {
                });
            }

        }
        $scope.getNextClaimGroupNumber = function () {
            $scope.removeAllAllocatedGroupsFromGrid();
            if (isGuid($scope.claimBatchGroup.GroupId)) {
                angular.forEach($scope.claimGroupsInBatch, function (value) {
                    if (value.Id === $scope.claimBatchGroup.GroupId) {
                        $scope.claimBatchGroup.GroupName = value.GroupName;
                        $scope.claimBatchGroup.Comment = value.Comment;
                    }
                });
                $scope.getAllAllocatedClaimsByGroupId();
            } else {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBatching/GetNextClaimGroupNumberById',
                    data: { 'BatchId': $scope.claimBatchGroup.ClaimBatchId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimBatchGroup.GroupName = data;
                    $scope.claimBatchGroup.Comment = '';
                    $scope.claimBatchGroup.TotalAmount = 0;


                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.getAllAllocatedClaimsByGroupId = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBatching/GetAllAllocatedClaimsByGroupId',
                data: {
                    'BatchId': $scope.claimBatchGroup.ClaimBatchId,
                    'GroupId': $scope.claimBatchGroup.GroupId

                },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {

                var rowCountEligible = $scope.claimsToBatch.length;
                angular.forEach(data, function (value) {
                    $scope.claimsToBatch.push(value);
                });
                //programmatically select dada

                for (var i = rowCountEligible; i <= $scope.claimsToBatch.length; i++) {
                    $scope.gridApi.grid.modifyRows($scope.claimsToBatch);
                    $scope.gridApi.selection.selectRow($scope.claimsToBatch[i]);
                }

            }).error(function (data, status, headers, config) {
            });


        }

        $scope.removeAllAllocatedGroupsFromGrid = function () {

            for (var i = $scope.claimsToBatch.length - 1; i >= 0; i--) {
                if ($scope.claimsToBatch[i].Allocated) {
                    $scope.gridApi.grid.modifyRows($scope.claimsToBatch);
                    $scope.gridApi.selection.unSelectRow($scope.claimsToBatch[i]);

                    $scope.claimsToBatch.splice(i, 1);
                }
            }


        }

        $scope.IsGoodwillGroupClaim = function () {

            if ($scope.claimBatchGroup.IsGoodwill == true) {

            } else {
                createGrid();
            }
        }

    });