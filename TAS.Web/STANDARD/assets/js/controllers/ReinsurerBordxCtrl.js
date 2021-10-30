app.controller('ReinsurerBordxCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, $state, toaster) {

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        $scope.Reinsurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            BordxYear: ""
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

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetConfirmedClaimBordxYears',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

        }


        $scope.loadInitailData = function () {


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/UserValidationReinsureBordxSubmission',
                data: { "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.status == 'OK') {
                    
                } else {
                    swal({ title: 'TAS Security Information', text: data.status, showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 8000);
                    $state.go('app.dashboard');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });



            LoadDetails();
        };



        $scope.reinsureBordxReportSearch = [];

        


        $scope.reinsureBordxReportGridOptions = {
            data: 'reinsureBordxReportSearch',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 10,

            columnDefs: [
                {
                    field: "ClaimBordxId",
                    displayName: "ClaimBordxId",
                    visible: false
                }, {
                    field: "ClaimBordxNo",
                    displayName: "Bordx No",
                    width: 100
                }, {
                    field: "ClaimBordxMonth",
                    displayName: "Month",
                    width: 100
                }, {
                    field: "ClaimBordxFromDate",
                    displayName: "From Date",
                    width: 100
                }, {
                    field: "ClaimBordxTodate",
                    displayName: "To Date",
                    width: 100
                }, {
                    field: "ClaimBordxValue",
                    displayName: "Bordx Value",
                    width: 200
                }, {
                    field: "ClaimBordxPaidAmount",
                    displayName: "Paid Amount",
                    width: 200
                }
                
            ],
        };

        $scope.validateReinsureBordx = function () {
            var isValid = true;

            if ($scope.Reinsurer.Id == "00000000-0000-0000-0000-000000000000" || $scope.Reinsurer.Id == null) {
                $scope.validate_reisurer = "has-error";
                isValid = false;
            } else {
                $scope.validate_reisurer = "";
            }

            return isValid
        }

        $scope.searchReinsureBordx = function () {

            if ($scope.validateReinsureBordx()) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerBordxByYearandReinsurerIdForGrid',
                    data: { "ReinsureId": $scope.Reinsurer.Id, "Year": $scope.Reinsurer.BordxYear },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.reinsureBordxReportSearch = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                customErrorMessage("Please fill valid data for highlighted fileds.");
            }
        }

    });