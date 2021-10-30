app.controller('BordxReportTemplateCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, $localStorage, toaster) {
        $scope.ModalName = "Bordereaux Report Template";
        $scope.ModalDescription = "Add Edit Bordereaux Report Template";
        $scope.errorTab1 = "";
        $scope.validate_Name = "";
        $scope.isEdit = false;


        $scope.submitBtnIconClass = "";
        $scope.submitBtnDisabled = false;
        $scope.CustomertxtDisabled = false;
        $scope.formAction = true;//true for add new
        $scope.searchGridloading = false;
        $scope.gridloadAttempted = false;

        $scope.model = {
            Id: emptyGuid(),
            Name: '',
            IsActive: true,
            BordxReportTemplateDetails: [],
            searchDisabled: false
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        LoadDetails();

        clear();

        function LoadDetails() {
            swal({ title: "TAS Information", text: "Please wait..", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetBordxReportTemplateColumns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId }
            }).success(function (data, status, headers, config) {
                $scope.Cols = data;
            }).error(function (data, status, headers, config) {
            }).finally(function () { swal.close(); });
        }

        function clear() {
            $scope.errorTab1 = "";
            $scope.validate_Name = "";

            $scope.model.Id = emptyGuid();
            $scope.model.Name = '';
            $scope.model.IsActive = true;
            $scope.model.BordxReportTemplateDetails = [];
            $scope.model.searchDisabled = false;
            $scope.formAction = true;
            LoadDetails();
        }

        $scope.reset = function () {
            clear();
            $scope.submitBtnDisabled = false;
            $scope.CustomertxtDisabled = false;
            $scope.isEdit = false;
        }

        $scope.save = function () {
            swal({ title: "TAS Information", text: "Please wait..", showConfirmButton: false });
            $scope.model.BordxReportTemplateDetails = [];
            angular.forEach($scope.Cols, function (value) {
                if (value.IsEnable) {
                    value.EntryUser = $rootScope.LoggedInUserId;
                    if ($scope.isEdit) { value.UpdateUser = $rootScope.LoggedInUserId; }
                    $scope.model.BordxReportTemplateDetails.push(value);
                }
            });
            if ($scope.validate()) {
                $scope.model.EntryUser = $rootScope.LoggedInUserId;
                if ($scope.isEdit) { $scope.model.UpdateUser = $rootScope.LoggedInUserId; }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Bordx/SaveBordxReportTemplate',
                    data: $scope.model,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data === true) {
                        SweetAlert.swal({
                            title: "Bordx Report Template Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });
                        $scope.reset();
                    }
                    else { customErrorMessage(data); }
                }).error(function (data, status, headers, config) {
                }).finally(function () { swal.close(); });
            }
            else { swal.close(); }
        }

        $scope.validate = function () {
            var isValid = true;
            $scope.validate_Name = "";
            $scope.errorTab1 = "";
            if ($scope.model.Name == undefined || $scope.model.Name == null || $scope.model.Name == "") {
                $scope.validate_Name = "has-error";
                isValid = false;
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
            if (!$scope.model.BordxReportTemplateDetails.length > 0) {
                isValid = false;
                customErrorMessage("Please Select at least one row to save.")
            }

            return isValid;
        }

        var cellTemplate = '<div><input type="checkbox" ng-model="row.entity.IsEnable" ng-checked="row.entity.IsEnable" ng-input="COL_FIELD" /></div>';
        var cellStatusTemplate = '<div><span ng-cell-text>{{COL_FIELD==true ? "Enable" : "Disable"}}</span></div>'

        $scope.gridOptions = {
            data: 'Cols',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            multiSelect: true,
            columnDefs: [
                { field: "DisplayName", displayName: "Display Name" },
                { field: "IsActive", visible: false },
                { field: "IsEnable", displayName: "Status", enableCellEdit: false, cellTemplate: cellStatusTemplate },
                {
                    field: "IsEnable", displayName: "Value", enableCellEdit: true, cellTemplate: cellTemplate,
                    editableCellTemplate: cellTemplate
                },
                { field: "BordxReportColumnsId", visible: false },
                { field: "Id", visible: false }
            ]
        };

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        }

        $scope.loadBordxReporptTemplate = function (bordxReportTemplteId) {
            if (isGuid(bordxReportTemplteId)) {
                searchPopup.close();
                swal({ title: "TAS Information", text: "Requesting borderaux report template information..", showConfirmButton: false });
                $scope.model.searchDisabled = true;
                $scope.submitBtnDisabled = true;
                $scope.formAction = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Bordx/GetBordxReportTemplateById',
                    data: { "Id": bordxReportTemplteId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.errorTab1 = "";
                    $scope.validate_Name = "";
                    $scope.model.searchDisabled = false;
                    $scope.submitBtnDisabled = false;
                    $scope.model.Id = data.Id;
                    $scope.model.Name = data.Name;
                    $scope.model.IsActive = data.IsActive;
                    $scope.bindGridData(data.BordxReportTemplateDetails);
                    $scope.isEdit = true;
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    $scope.model.searchDisabled = false;
                    $scope.submitBtnDisabled = false;
                    swal.close();
                });
            }
        }

        $scope.bindGridData = function (bordxReportTemplateDetails) {
            angular.forEach($scope.Cols, function (value) {
                if (bordxReportTemplateDetails != undefined && bordxReportTemplateDetails != null && bordxReportTemplateDetails.length > 0) {
                    value.IsEnable = (bordxReportTemplateDetails.find(x => x.BordxReportColumnsId == value.BordxReportColumnsId) != undefined) ? true : false;
                }
                else { value.IsEnable = false; }
            });
        }


        //-------------------------- search -----------------------------//

        $scope.bordxReportTemplateSearchGridSearchCriterias = {
            name: "",
        };

        var paginationOptionsSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.gridOptionsBordxReportTemplate = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Template Name', field: 'Name', enableSorting: true, cellClass: 'columCss' },
                { name: 'Active', field: 'IsActive', enableSorting: true, cellClass: 'columCss', },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBordxReporptTemplate(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
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
                    getBordxReportTemplateSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsSearchGrid.pageNumber = newPage;
                    paginationOptionsSearchGrid.pageSize = pageSize;
                    getBordxReportTemplateSearchPage();
                });
            }
        };

        var getBordxReportTemplateSearchPage = function () {
            $scope.searchGridloading = true;
            $scope.gridloadAttempted = false;
            var customerSearchGridParam =
            {
                'paginationOptionsSearchGrid': paginationOptionsSearchGrid,
                'bordxReportTemplateSearchGridSearchCriterias': $scope.bordxReportTemplateSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetAllBordxReportTemplateForSearchGrid',
                data: customerSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsBordxReportTemplate.data = response_arr.data;
                $scope.gridOptionsBordxReportTemplate.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.searchGridloading = false;
                $scope.gridloadAttempted = true;
            });
        };

        $scope.refresCustomerSearchGridData = function () {
            getBordxReportTemplateSearchPage();
        }

        $scope.searchPopup = function () {
            $scope.bordxReportTemplateSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                eMail: '',
                mobileNo: ''
            };
            var paginationOptionsSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getBordxReportTemplateSearchPage();
            searchPopup = ngDialog.open({
                template: 'popUpSearchBordxReportTemplate',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });


        };

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }

        function emptyGuid() { return "00000000-0000-0000-0000-000000000000"; }
    });