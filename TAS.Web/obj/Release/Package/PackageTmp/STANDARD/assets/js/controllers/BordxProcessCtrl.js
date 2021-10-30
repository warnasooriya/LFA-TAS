app.controller('BordxProcessCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, toaster) { //, $cookieStore, $filter

        $scope.reinsurers = [];
        $scope.insurers = [];
        $scope.years = [];

        Clear();

        LoadDetails();

        function Clear() {
            $scope.canProcess = false;
            $scope.canConfirm = false;
            $scope.canView = false;

            $scope.errorTab1 = "";
            $scope.bordx = [];

            $scope.Claimbordx = {
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

        function LoadDetails() {
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
                url: '/TAS.Web/api/claimbordxprocess/GetClaimBordxYears',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.years = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.loadBordxNumber = function () {
            $scope.bordx = [];
            if ($scope.Claimbordx.insurerId != emptyGuid() && $scope.Claimbordx.insurerId != "" && $scope.Claimbordx.reinsurerId != emptyGuid() && $scope.Claimbordx.reinsurerId != "" && $scope.Claimbordx.year != "" && $scope.Claimbordx.month != "") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claimbordxprocess/GetClaimBordxByYearAndMonth',
                    data: { 'year': $scope.Claimbordx.year, 'month': $scope.Claimbordx.month, 'insurerid': $scope.Claimbordx.insurerId, 'reinsurerid': $scope.Claimbordx.reinsurerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.bordx = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.checkConfirm = function () { if (!$scope.bordxObj.Isconfirmed) { $scope.canConfirm = $scope.bordxObj.IsProcessed; $scope.canProcess = true; } else { $scope.canView = true; }  }

        function validate() {
            var retVal = true;
            $scope.errorTab1 = "";
            $scope.Claimbordx.claimBordxId = $scope.bordxObj.Id;
            if (!($scope.Claimbordx.insurerId != emptyGuid() && $scope.Claimbordx.insurerId != "" && $scope.Claimbordx.reinsurerId != emptyGuid() && $scope.Claimbordx.reinsurerId != ""
                   && $scope.Claimbordx.claimBordxId != emptyGuid() && $scope.Claimbordx.claimBordxId != "" && $scope.Claimbordx.year != "" && $scope.Claimbordx.month != "")) {
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
                    data: { 'claimbordxId': $scope.bordxObj.Id, 'isProcess': true, 'userId': emptyGuid() },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data) {
                        customInfoMessage("Process done !");
                        $scope.canConfirm = true;
                        Clear();
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
                url: '/TAS.Web/api/claimbordxprocess/ClaimBordxProcessUpdate',
                data: { 'claimbordxId': $scope.bordxObj.Id, 'isConfirm': true, 'userId': emptyGuid() },
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

        $scope.View = function () {

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


    }
);