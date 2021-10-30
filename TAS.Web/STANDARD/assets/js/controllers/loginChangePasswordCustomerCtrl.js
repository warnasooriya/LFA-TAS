app.controller('loginChangePasswordCustomerCtrl',
function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {
    $scope.requestId = $stateParams.requestId;
    $scope.tpaId = $stateParams.tpaId;
    $scope.currentCustomerId = $stateParams.currentCustomerId;
    $scope.tempInvId = $stateParams.tempInvId;
    $scope.tpaName = "";
    $scope.currentUserId = "";
    $scope.err = 0;
    $scope.Errormsg = ""
    $scope.newPassword = ""
    $scope.confirmNewPassword = ""
    $scope.linkData = {
        requestId: $scope.requestId,
        tpaId: $scope.tpaId,
        currentCustomerId: $scope.currentCustomerId
    };
    $scope.passwordData = {
        requestId: "",
        tpaId: "",
        password: "",
        systemUserId: "",
        currentCustomerId:""
    };
    $scope.pageValidityCheck = function () {
        if ($scope.requestId != "" && $scope.tpaId != ""
            && isGuid($scope.requestId) && isGuid($scope.tpaId)) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/validateChangePassswordLink',
                data: $scope.linkData
            }).success(function (data, status, headers, config) {
                if (data == "") {
                    $scope.denyAccess("expired");
                } else {
                    $scope.currentUserId = data;
                }
            }).error(function (data, status, headers, config) {

            });
        } else {
            $scope.denyAccess("invalid");
        }


        $http({
            method: 'POST',
            url: '/TAS.Web/api/ProductDisplay/GetTPANameById',
            data: { 'tpaId': $scope.tpaId }
        }).success(function (data, status, headers, config) {
            $scope.tpaName = data;
        }).error(function (data, status, headers, config) {

        });


    }

    $scope.navigateToLogin = function () {
        $location.path('login/signin/' + $scope.tpaName);
    }
    $scope.denyAccess = function (state) {
        if (state == "invalid") {
            SweetAlert.swal({
                title: "TAS Information",
                text: "You don't have access to requested page.",
                type: "warning",
                confirmButtonColor: "rgb(221, 107, 85)"
            });
        }
        else if (state == "expired") {
            SweetAlert.swal({
                title: "TAS Information",
                text: "The page you request is exipred or invalid.",
                type: "warning",
                confirmButtonColor: "rgb(221, 107, 85)"
            });
        }

        $location.path('login/signin/' + $scope.tpaId);
    }

    function isGuid(stringToTest) {
        var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
    }

    $scope.submitChangePasswordRequest = function () {
        if ($scope.newPassword != "" && $scope.confirmNewPassword != "") {
            if ($scope.newPassword == $scope.confirmNewPassword) {
                if ($scope.currentUserId != "") {
                    $scope.passwordData = {
                        requestId: $scope.requestId,
                        tpaId: $scope.tpaId,
                        password: $scope.newPassword,
                        systemUserId: $scope.currentUserId,
                        currentCustomerId:$scope.currentCustomerId
                    };
                    $scope.err = 0;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/ChangeForgotPasssword',
                        data: $scope.passwordData
                    }).success(function (data, status, headers, config) {
                        if (data == true) {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "You password has successfuly changed. Please login with new credentials.",
                                confirmButtonColor: "#007AFF"
                            });
                            //$location.path('login/signin/' + $scope.tpaName);
                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Error occured while saving data!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                    }).error(function (data, status, headers, config) {

                    });
                } else {
                    $scope.denyAccess("expired");
                }

            } else {
                $scope.err = 1;
                $scope.Errormsg = "Confirm password doesnt match";
            }
        } else {
            $scope.err = 1;
            $scope.Errormsg = "Please fill all the fields";
        }
    }
});