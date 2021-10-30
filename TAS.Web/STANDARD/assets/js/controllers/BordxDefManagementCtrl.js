app.controller('BordxDefManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, $filter, $translate) {
        $scope.ModalName = "Bordereaux Format";
        $scope.ModalDescription = "Add Edit Bordereaux Format";
        $scope.errorTab1 = "";

        $scope.ProductType = emptyGuid()


        loadProductTypes();

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
                displayName: $filter('translate')('pages.bordereauxFormat.dispalyName')
            }, {
                field: "IsActive",
                visible: false
            }, {
                field: "Status",
                    displayName: $filter('translate')('pages.bordereauxFormat.status')
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
                data: {
                    "data": $scope.myPolicySelectedRows},
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == "ok") {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.bordereauxFormat.bordxInformation'),
                        text: $filter('translate')('common.sucessMessages.successfullySaved'),
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "#007AFF"
                    });
                }
                $scope.LoadDetails();
            }).error(function (data, status, headers, config) {
            });
        }

        function loadProductTypes() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetProductTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.productsTypes = data;
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
                        title: $filter('translate')('pages.bordereauxFormat.bordxInformation'),
                        text: $filter('translate')('common.sucessMessages.successfullySaved'),
                        confirmButtonText: $filter('translate')('common.button.ok'),
                        confirmButtonColor: "#007AFF"
                    });
                }
                $scope.LoadDetails();
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.LoadDetails = function() {
            var bordxColumnCriteria = {
                id: $localStorage.LoggedInUserId,
                ProductType: $scope.ProductType
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetBordxColumns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: bordxColumnCriteria
            }).success(function (data, status, headers, config) {
                $scope.Cols = data;
            }).error(function (data, status, headers, config) {
            });
        }

    });



var emptyGuid = function () {
    return "00000000-0000-0000-0000-000000000000";
};

