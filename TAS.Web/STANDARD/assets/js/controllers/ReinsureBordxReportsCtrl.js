app.controller('ReinsureBordxReportsCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster) {

        $scope.reinsureBordxReportSubmitBtnIconClass = "";
        $scope.reinsureBordxReportSubmitBtnDisabled = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        $scope.Reinsurer = {
            Id: "00000000-0000-0000-0000-000000000000",
        };

        $scope.ReinsureBordxReportColumns = {
            Id: "00000000-0000-0000-0000-000000000000",
            ColumnId: "00000000-0000-0000-0000-000000000000"
        };

        function LoadDetails() {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Reinsurers = data;
            }).error(function (data, status, headers, config) {
            });

        }

        $scope.loadInitailData = function () {

            LoadDetails();
        };     


       
        $scope.reinsureBordxReportSearch = [];

        $scope.LoadReinsureBordxReportColumns = function () {

            if ($scope.Reinsurer.Id != '' && $scope.Reinsurer.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerManagement/GetBordxReportColumnsByReinsureId',
                    data: { "ReinsureId": $scope.Reinsurer.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.reinsureBordxReportSearch = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.reinsureBordxReportSearch = [];
            }
        }

        $scope.reinsureBordxReportGridOptions = {
            data: 'reinsureBordxReportSearch',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 10,

            columnDefs: [


                {
                    field: "BordxReportColumnsId",
                    displayName: "BordxReportColumnsId",
                    visible: false
                }, {
                    field: "Sequance",
                    displayName: "Sequance",
                    width: 100
                }, {
                    field: "DisplayName",
                    displayName: "Display Name",
                    width: 600
                }, {
                    field: "Enable",
                    displayname: "Check Box",
                    cellTemplate: '<input type="checkbox" ng-model="row.entity.IsAllowed" ng-click="toggle(row.entity.name,row.entity.IsAllowed)">',
                    width: 100
                }
            ],
        };

        $scope.reinsureBordxReportSubmit = function () {

            var jObject = JSON.stringify($scope.reinsureBordxReportSearch);
            $scope.reinsureBordxReportSubmitBtnIconClass = "fa fa-spinner fa-spin";
            $scope.reinsureBordxReportSubmitBtnDisabled = true;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/AddOrUpdateReinsureBordxReportColumns',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { JObject: $scope.reinsureBordxReportSearch }
            }).success(function (data, status, headers, config) {
                $scope.Ok = data;
                $scope.reinsureBordxReportSubmitBtnIconClass = "";
                $scope.reinsureBordxReportSubmitBtnDisabled = false;
                if (data == "OK") {
                    SweetAlert.swal({
                        title: "Reinsure Bordx Mapping Information",
                        text: "Successfully Saved!",
                        confirmButtonColor: "#007AFF"
                    });

                    $scope.LoadReinsureBordxReportColumns();                   
                } else {
                    SweetAlert.swal({
                        title: "Reinsure Bordx Mapping Information",
                        text: "Save Failed!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                }
                return false;
            }).error(function (data, status, headers, config) {
                SweetAlert.swal({
                    title: "Reinsure Bordx Mapping Information",
                    text: "Error occured while saving data!",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                $scope.reinsureBordxReportSubmitBtnIconClass = "";
                $scope.reinsureBordxReportSubmitBtnDisabled = false;
                return false;
            });

        }

    });