app.controller('FaultManagementCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {
        $scope.ModalName = "Fault Management";
        $scope.ModalDescription = "You can Add/Update Fault Category/Fault/Fault Area here.";
        $scope.FaultCategorySaveBtnIconClass = "";
        $scope.FaultCategorySaveBtnDisabled = false;
        $scope.FaultAreaSaveBtnIconClass = "";
        $scope.FaultAreaSaveBtnDisabled = false;
        $scope.errorTab1 = "";
        $scope.faultsGridloading = false;
        $scope.faultsGridloadAttempted = false;


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
            toaster.pop('error', 'Error', msg);
        };

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/FaultManagement/GetAllFaultCategory',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.FaultCategorys = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/FaultManagement/GetAllFaultArea',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.FaultAreas = data;
            }).error(function (data, status, headers, config) {
            });

            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/FaultManagement/GetAllFault',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.Faults = data;
            //    getFaultsPage();
            //}).error(function (data, status, headers, config) {
            //});

            getFaultsPage()
        }
        var paginationOptionsgridFaults = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.loadInitailData = function () {
            LoadDetails();
        }

        $scope.gridFaults = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            //useExternalPagination: true,
            //useExternalSorting: true,
            //enableColumnMenus: false,

            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Fault Code', field: 'FaultCode', enableSorting: false, cellClass: 'columCss' },
                { name: 'Fault Name', field: 'FaultName', enableSorting: false, cellClass: 'columCss', },
                { name: 'Fault Category', field: 'FaultCategoryCode', enableSorting: false, cellClass: 'columCss' },
                { name: 'Fault Area', field: 'FaultAreaCode', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.SetFaultValues(row.entity.Id)" class="btn btn-xs btn-warning">Edit</button></div>',
                    enableSorting: true,
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        paginationOptionsgridFaults.sort = null;
                    } else {
                        paginationOptionsgridFaults.sort = sortColumns[0].sort.direction;
                    }
                    getFaultsPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsgridFaults.pageNumber = newPage;
                    paginationOptionsgridFaults.pageSize = pageSize;
                    getFaultsPage();
                });
            }
        };

        $scope.refresSearchGridData = function () {
            getFaultsPage();
        }

        function getFaultsPage() {
            $scope.faultsGridloading = true;
            $scope.faultsGridloadAttempted = false;
            var policySearchGridParam =
                {
                    'page': paginationOptionsgridFaults.pageNumber,
                    'pageSize': paginationOptionsgridFaults.pageSize,
                    'userId': $localStorage.LoggedInUserId
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/FaultManagement/GetAllFaultForSearchGrid',
                data: policySearchGridParam,
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridFaults.data = response_arr.data;
                $scope.gridFaults.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.faultsGridloading = false;
                $scope.faultsGridloadAttempted = true;

            });
        };


        $scope.FaultCategory = {
            Id: "00000000-0000-0000-0000-000000000000",
            FaultCategoryCode: "",
            FaultCategoryName: ""
        };
        $scope.FaultArea = {
            Id: "00000000-0000-0000-0000-000000000000",
            FaultAreaCode: "",
            FaultAreaName: ""
        };
        $scope.Fault = {
            Id: "00000000-0000-0000-0000-000000000000",
            FaultCode: "",
            FaultName: ""
        };
        function clearFaultCategoryControls() {
            $scope.FaultCategory = {
                Id: "00000000-0000-0000-0000-000000000000",
                FaultCategoryCode: "",
                FaultCategoryName: ""
            };
        }
        function clearFaultAreaControls() {
            $scope.FaultArea = {
                Id: "00000000-0000-0000-0000-000000000000",
                FaultAreaCode: "",
                FaultAreaName: ""
            };
        }
        function clearFaultControls() {
            $scope.Fault = {
                Id: "00000000-0000-0000-0000-000000000000",
                FaultCode: "",
                FaultName: ""
            };
            $scope.Failures = [{ CauseOfFailure: '' }];
            $scope.FailuresStr = [];
        }
        $scope.SetFaultValues = function (PartId) {
            $scope.errorTab1 = "";
            if (PartId != "00000000-0000-0000-0000-000000000000" && PartId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/FaultManagement/GetFaultById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": PartId }
                }).success(function (data, status, headers, config) {
                    $scope.Fault = data;
                    setCfailure(data.CFailures);
                }).error(function (data, status, headers, config) {
                    clearFaultControls();
                });
            }
            else {
                clearFaultControls();
            }
        }

        $scope.loadFaultGrid = function () {
            $scope.refresSearchGridData();
        }
        $scope.SetFaultCatValues = function () {
            $scope.errorTab1 = "";
            if ($scope.FaultCategory.Id != "00000000-0000-0000-0000-000000000000" && $scope.FaultCategory.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/FaultManagement/GetFaultCategoryById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.FaultCategory.Id }
                }).success(function (data, status, headers, config) {
                    $scope.FaultCategory = data;
                }).error(function (data, status, headers, config) {
                    clearFaultCategoryControls();
                });
            }
            else {
                clearFaultCategoryControls();
            }
        }

        $scope.SetFaultAreaValues = function () {
            $scope.errorTab1 = "";
            if ($scope.FaultArea.Id != "00000000-0000-0000-0000-000000000000" && $scope.FaultArea.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/FaultManagement/GetFaultAreaById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.FaultArea.Id }
                }).success(function (data, status, headers, config) {
                    $scope.FaultArea = data;
                }).error(function (data, status, headers, config) {
                    clearFaultAreaControls();
                });
            }
            else {
                clearFaultAreaControls();
            }
        }

        $scope.LoadFaults = function () {
            getFaultsPage();
        }

        $scope.FaultCategorySubmit = function () {
            if ($scope.validateFaultCategory()) {
                if ($scope.FaultCategory.Id == null || $scope.FaultCategory.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.FaultCategorySaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultCategorySaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/AddFaultCategory',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.FaultCategory
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultCategorySaveBtnIconClass = "";
                        $scope.FaultCategorySaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "FaultCategory Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/FaultManagement/GetAllFaultCategory',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.FaultCategorys = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearFaultCategoryControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "FaultCategory Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultCategorySaveBtnIconClass = "";
                        $scope.FaultCategorySaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.FaultCategorySaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultCategorySaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/UpdateFaultCategory',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.FaultCategory
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultCategorySaveBtnIconClass = "";
                        $scope.FaultCategorySaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "FaultCategory Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/FaultManagement/GetAllFaultCategory',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.FaultCategorys = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearFaultCategoryControls();
                        }
                        else {
                            ;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "FaultCategory Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultCategorySaveBtnIconClass = "";
                        $scope.FaultCategorySaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }




        $scope.FaultAreaSubmit = function () {
            if ($scope.validateFaultArea()) {
                if ($scope.FaultArea.Id == null || $scope.FaultArea.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.FaultAreaSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultAreaSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/AddFaultArea',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.FaultArea
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultAreaSaveBtnIconClass = "";
                        $scope.FaultAreaSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "FaultArea Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/FaultManagement/GetAllFaultArea',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.FaultAreas = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearFaultAreaControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "FaultArea Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultAreaSaveBtnIconClass = "";
                        $scope.FaultAreaSaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.FaultAreaSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultAreaSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/UpdateFaultArea',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.FaultArea
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultAreaSaveBtnIconClass = "";
                        $scope.FaultAreaSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "FaultArea Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/FaultManagement/GetAllFaultArea',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.FaultAreas = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearFaultAreaControls();
                        }
                        else {
                            ;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "FaultArea Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultAreaSaveBtnIconClass = "";
                        $scope.FaultAreaSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }



        $scope.FaultSubmit = function () {
            if ($scope.validateFault()) {
                if ($scope.Fault.Id == null || $scope.Fault.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.FaultSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultSaveBtnDisabled = true;
                    getCauseOfFail();
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/AddFault',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { 'Fault': $scope.Fault, 'CFailures': $scope.FailuresStr }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultSaveBtnIconClass = "";
                        $scope.FaultSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "Fault Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                         $scope.refresSearchGridData();
                         clearFaultControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Fault Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultSaveBtnIconClass = "";
                        $scope.FaultSaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.FaultSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.FaultSaveBtnDisabled = true;
                    getCauseOfFail();
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/FaultManagement/UpdateFault',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { 'Fault': $scope.Fault, 'CFailures': $scope.FailuresStr }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.FaultSaveBtnIconClass = "";
                        $scope.FaultSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "Fault Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.refresSearchGridData();
                            clearFaultControls();
                        }
                        else {
                            ;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Fault Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.FaultSaveBtnIconClass = "";
                        $scope.FaultSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }

        $scope.validateFaultCategory = function () {
            var isValid = true;
            //if (!isGuid($scope.FaultCategory.Id)) {
            //    $scope.validate_FaultCategoryId = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_FaultCategoryId = "";
            //}

            if ($scope.FaultCategory.FaultCategoryCode == "") {
                $scope.validate_FaultCategoryCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultCategoryCode = "";
            }

            if ($scope.FaultCategory.FaultCategoryName == "") {
                $scope.validate_FaultCategoryName = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultCategoryName = "";
            }

            return isValid;
        }

        $scope.validateFaultArea = function () {
            var isValid = true;
            //if (!isGuid($scope.FaultArea.Id)) {
            //    $scope.validate_FaultAreaId = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_FaultAreaId = "";
            //}

            if ($scope.FaultArea.FaultAreaCode == '') {
                $scope.validate_FaultAreaCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultAreaCode = "";
            }

            if ($scope.FaultArea.FaultAreaName == '') {
                $scope.validate_FaultAreaName = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultAreaName = "";
            }

            return isValid;
        }

        $scope.validateFault = function () {
            var isValid = true;
            if (!isGuid($scope.Fault.FaultAreaId)) {
                $scope.validate_FaultFaultAreaId = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultFaultAreaId = "";
            }

            if (!isGuid($scope.Fault.FaultCategoryId)) {
                $scope.validate_FaultFaultCategoryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultFaultCategoryId = "";
            }

            if ($scope.Fault.FaultCode == "" || $scope.Fault.FaultCode == undefined) {
                $scope.validate_FaultCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultCode = "";
            }

            if ($scope.Fault.FaultName == "" || $scope.Fault.FaultName == undefined) {
                $scope.validate_FaultName = "has-error";
                isValid = false;
            } else {
                $scope.validate_FaultName = "";
            }

            return isValid;
        }

        $scope.Failures = [{ CauseOfFailure: '' }];
        $scope.FailuresStr = [];

        $scope.addNewFailure = function () {
            var CauseOfFailure = $scope.Failures.CauseOfFailure;
            $scope.Failures.push({ 'CauseOfFailure': CauseOfFailure });
        };

        function getCauseOfFail() {
            $scope.FailuresStr = [];
            for (var i = 0; i < $scope.Failures.length; i++) {
                $scope.FailuresStr.push($scope.Failures[i]['CauseOfFailure']);
            }
        }

        $scope.removeFailure = function () {
            var lastItem = $scope.Failures.length - 1;
            $scope.Failures.splice(lastItem);
        };

        function setCfailure(Cfailure) {
            $scope.Failures = [];
            for (var i = 0; i < Cfailure.length; i++) {
                $scope.Failures.push({ 'CauseOfFailure': Cfailure[i] });
            }
        }


    });