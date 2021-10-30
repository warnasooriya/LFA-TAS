app.controller('ClaimBordxYearProcessCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster, $translate) {

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
                        title: $filter('translate')('common.areYouSure'),
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: $filter('translate')('pages.bordxManagement.sucessMessages.pocessIt'),
                        cancelButtonText: $filter('translate')('pages.bordxManagement.sucessMessages.noCancel'),
                        closeOnConfirm: false,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            swal({
                                title: $filter('translate')('pages.bordxManagement.sucessMessages.pleaseWait'),
                                text: $filter('translate')('common.processing'),
                                showConfirmButton: false
                            });

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
                                    swal({
                                        title: $filter('translate')('common.alertTitle'),
                                        text: $filter('translate')('pages.claimBordxYearProcess.sucessMessages.yearProcessSuccess'),
                                        showConfirmButton: true
                                    });
                                    clearCliamBordxControls();
                                    $scope.getLast10Bordx();
                                } else {
                                    swal({
                                        title: $filter('translate')('common.alertTitle'),
                                        text: data,
                                        showConfirmButton: true,
                                        type: 'error'
                                    });

                                }
                            }).error(function (data, status, headers, config) {
                                swal.close();
                            }).finally(function () {

                            });
                        }
                        });
                    $scope.validate_BordxYear = "has-error";
                } else {
                    customErrorMessage($filter('translate')('pages.claimBordxYearProcess.errorMessages.invalidClaimBordereaux'));
                    $scope.validate_BordxYear = "has-error";
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.allMandatory'));
            }
            }

        $scope.claimBordxYearlyProcessConfrm = function () {
            if (parseInt($scope.Claimbordx.BordxYear) && parseInt($scope.Claimbordx.BordxYear) > 0) {

                swal({
                    title: $filter('translate')('common.areYouSure'),
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: $filter('translate')('pages.bordxManagement.sucessMessages.conFirmIt'),
                    cancelButtonText: $filter('translate')('pages.bordxManagement.sucessMessages.noCancel'),
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({
                            title: $filter('translate')('pages.bordxManagement.sucessMessages.pleaseWait'),
                            text: $filter('translate')('common.processing'),
                            showConfirmButton: false
                        });

                        var newClaimBordxRequest = {
                            BordxYear: $scope.Claimbordx.BordxYear,
                            Reinsurer: $scope.Claimbordx.reinsurerId,
                            Insurer: $scope.Claimbordx.insurerId
                        };

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxYearProcessConfirm',
                            data: newClaimBordxRequest,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'OK') {
                                swal({
                                    title: $filter('translate')('common.alertTitle'),
                                    text: $filter('translate')('pages.claimBordxYearProcess.sucessMessages.yearProcessConfirm'),
                                    showConfirmButton: true
                                });
                                clearCliamBordxControls();
                                $scope.getLast10Bordx();
                            } else {
                                swal({
                                    title: $filter('translate')('common.alertTitle'),
                                    text: data,
                                    showConfirmButton: true,
                                    type: 'error'
                                });

                            }
                        }).error(function (data, status, headers, config) {
                            swal.close();
                        }).finally(function () {

                        });
                    }
                });
            } else {
                customErrorMessage($filter('translate')('pages.claimBordxYearProcess.errorMessages.invalidClaimBordereaux'));
            }
        }


        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
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