app.controller('ClaimBordxYearProcessCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {

        $scope.ClaimbordxProcessBtnIconClass = "";
        $scope.ClaimbordxProcessBtnDisabled = false;
        $scope.errorTab1 = "";

        
        $scope.loadInitialDetails=function () {
            
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetClaimBordxYearsForProcess',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ClaimBordxyears = data;
            }).error(function (data, status, headers, config) {
            });
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
        }
   
        $scope.Claimbordx = {
            Id: "00000000-0000-0000-0000-000000000000",
            reinsurerId: "00000000-0000-0000-0000-000000000000",
            insurerId: "00000000-0000-0000-0000-000000000000",
        };
        $scope.validateClaimBordxYear = function () {

            var isValid = true;
           
            if (!isGuid($scope.Claimbordx.reinsurerId)) {
                $scope.validate_reinsurerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_reinsurerId = "";
            }
            if (!isGuid($scope.Claimbordx.insurerId)) {
                $scope.validate_insurerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_insurerId = "";
            }
            return isValid;

        }
       
        $scope.claimBordxYearlyProcess = function () {
            if ($scope.validateClaimBordxYear()) {
                if (parseInt($scope.Claimbordx.BordxYear) && parseInt($scope.Claimbordx.BordxYear) > 0) {

                    swal({
                        title: "Are you sure?",
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, Process!",
                        cancelButtonText: "No, cancel!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                            var newClaimBordxRequest = {
                                BordxYear: $scope.Claimbordx.BordxYear,
                                Reinsurer: $scope.Claimbordx.reinsurerId,
                                Insurer: $scope.Claimbordx.insurerId,
                            };

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxYearProcess',
                                data: newClaimBordxRequest,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == 'OK') {
                                    swal({ title: "TAS Information", text: "Year Process Success.", showConfirmButton: true });
                                    clearCliamBordxControls();
                                    $scope.getLast10Bordx();
                                } else {
                                    swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                                }
                            }).error(function (data, status, headers, config) {
                                swal.close();
                            }).finally(function () {

                            });
                        }
                        });
                    $scope.validate_BordxYear = "has-error";
                } else {
                    customErrorMessage("Invalid ClaimBordereaux Year Selection.");
                    $scope.validate_BordxYear = "has-error";
                }
            } else {
                customErrorMessage("Fill all the highlighted fields.");
            }
            } 

        $scope.claimBordxYearlyProcessConfrm = function () {
            if (parseInt($scope.Claimbordx.BordxYear) && parseInt($scope.Claimbordx.BordxYear) > 0) {

                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Confirm!",
                    cancelButtonText: "No, cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                        var newClaimBordxRequest = {
                            BordxYear: $scope.Claimbordx.BordxYear,
                        };

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxYearProcessConfirm',
                            data: newClaimBordxRequest,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'OK') {
                                swal({ title: "TAS Information", text: "Year Process Confirm Success.", showConfirmButton: true });
                                clearCliamBordxControls();
                                $scope.getLast10Bordx();
                            } else {
                                swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                            }
                        }).error(function (data, status, headers, config) {
                            swal.close();
                        }).finally(function () {

                        });
                    }
                });
            } else {
                customErrorMessage("Invalid ClaimBordereaux Year Selection.");
            }
        }
        

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };

        //var isGuid = function (stringToTest) {
        //    var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        //    var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        //    return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        //}
        
    });