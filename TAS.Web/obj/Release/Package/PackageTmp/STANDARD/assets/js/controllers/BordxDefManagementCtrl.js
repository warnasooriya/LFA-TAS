app.controller('BordxDefManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage) {
        $scope.ModalName = "Bordereaux Format";
        $scope.ModalDescription = "Add Edit Bordereaux Format";
        $scope.errorTab1 = "";
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetBordxColumns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId }
        }).success(function (data, status, headers, config) {
            $scope.Cols = data;
            }).error(function (data, status, headers, config) {
            });
        }
        LoadDetails();
        $scope.gridOptions = {
            data: 'Cols',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 5,
            enablePaginationControls: true,
            enableRowSelection: true,
            enableCellSelection: false,
            multiSelect: true,
            columnDefs: [ {
                field: "DisplayName",
                displayName: "Display Name"
            }, {
                field: "IsActive",
                visible: false
            }, {
                field: "Status",
                displayName: "Status"
            }, {
                field: "ColumnId",
                visible: false
            }, {
                field: "UserId",
                visible: false
            }, {
                field: "Id",
                visible: false
            }
            ]
        };
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        }
        $scope.Enable = function () {
            $scope.myPolicySelectedRows = $scope.gridApi.selection.getSelectedRows();
            angular.forEach($scope.myPolicySelectedRows, function (value) {
                angular.forEach($scope.Cols, function (val) {
                    if (value.ColumnId == val.ColumnId)
                        val.IsActive = true;
                });
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/AddBordxColumns',
                data: { "data": $scope.myPolicySelectedRows },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == "ok") {
                    SweetAlert.swal({
                        title: "Bordx Information",
                        text: "Successfully Saved!",
                        confirmButtonColor: "#007AFF"
                    });
                }
                LoadDetails();
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.Dissable = function () {
            $scope.myPolicySelectedRows = $scope.gridApi.selection.getSelectedRows();
            angular.forEach($scope.myPolicySelectedRows, function (value) {
                angular.forEach($scope.Cols, function (val) {
                    if (value.ColumnId == val.ColumnId)
                        val.IsActive = false;
                });
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/AddBordxColumns',
                data: { "data": $scope.myPolicySelectedRows },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == "ok") {
                    SweetAlert.swal({
                        title: "Bordx Information",
                        text: "Successfully Saved!",
                        confirmButtonColor: "#007AFF"
                    });
                }
                LoadDetails();
            }).error(function (data, status, headers, config) {
            });
        }
    });