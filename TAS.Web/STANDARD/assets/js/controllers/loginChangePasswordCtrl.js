app.controller('loginChangePasswordCtrl',
function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {
    $scope.requestId = $stateParams.requestId;
    $scope.tpaId = $stateParams.tpaId;
    $scope.tpaName = "";
    $scope.currentUserId = "";
    $scope.err = 0;
    $scope.Errormsg = "";
    $scope.newPassword = "";
    $scope.confirmNewPassword = "";
    $scope.loginBtnClass = "btn btn-primary pull-left";
    $scope.submitBtnClass = "btn btn-primary pull-right";
    $scope.image = "/TAS.Web/STANDARD/assets/images/new_LoginBg.jpg";
    $scope.copyRightYear = new Date().getFullYear();
    $scope.linkData = {
        requestId: $scope.requestId,
        tpaId: $scope.tpaId
    };
    $scope.passwordData = {
        requestId: "",
        tpaId: "",
        password: "",
        systemUserId: ""
    };
    $scope.pageValidityCheck = function () {

        $http({
            method: 'POST',
            url: '/TAS.Web/api/ProductDisplay/GetTPANameById',
            data: { 'tpaId': $scope.tpaId }
        }).success(function (data, status, headers, config) {
            $scope.tpaName = data;
            $scope.ContinentalCheck();
        }).error(function (data, status, headers, config) {

            });

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
    }

    $scope.loginBoxStyleCP = {
        "background": "rgba(0,0,0,0.28)",
        "boxAlignClass": "main-login col-xs-10 col-xl-2 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-3 col-md-offset-2"
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
            }, function () {
                $location.path('login/signin/' + $scope.tpaName);

            });
        }

       // $location.path('login/signin/' + $scope.tpaId);
    }

    function isGuid(stringToTest) {
        var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
        var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
        return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
    }

    $scope.ContinentalCheck = function () {
        if ($scope.tpaName == "continental" || $scope.tpaName == "LasaCon") {

            $scope.loginBtnClass = "btn btn-primaryCon pull-left";
            $scope.submitBtnClass = "btn btn-primaryCon pull-right";
            $scope.loginboxBackground = " background: rgba(0,0,0,0.28)";
            $scope.image = " /TAS.Web/STANDARD/assets/images/new_Loginperkins.jpg";
            $scope.loginBoxStyleCP = {
                "background": "rgba(0,0,0,0.0)",
                "boxAlignClass": "main-login col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2"
            }
            $scope.loginboxBackground = " background:black";
            // $scope.image = "/TAS.Web/STANDARD/assets/images/Newlogincont.jpg";
        }
        else {
            $scope.loginBoxStyle = {
                "color": "box-internal-fore-color-black",
                "colorOrd": "black",
                "loginbox": "1px solid;border-radius:0px",
                "labelHeading": "box-internal-fore-color-black-heading",
                "outerLogo": true,
                "copyrightLinkColor": "color:#000000",
                "loginInputStyle": "background-color: #dae3f3;border: 1px solid;border-radius: 0px;",
                "checkboxColor": "checkbox-color",
                "boxAlignClass": "main-login col-xs-10 col-xl-2 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-3 col-md-offset-2"
            }

            $scope.loginBoxStyleCP = {

                "boxAlignClass": "main-login col-xs-10 col-xl-2 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-3 col-md-offset-2"
            }

            $scope.loginboxBackground = " background: white";
            $scope.forgotPwdLblColor = "color:black"
            $scope.image = "/TAS.Web/STANDARD/assets/images/extenderdWarrentyLoginPage.png";
        }
        //else {
        //    $scope.loginboxBackground = "background: rgba(228,228,228,0.28)";
        //    $scope.image = "/TAS.Web/STANDARD/assets/images/new_LoginBg.jpg";
        //    $scope.bgcolor = "#4e6c86";
        //    $scope.forgotPwdLblColor = "color:#fff"
        //}
    }

    $scope.submitChangePasswordRequest = function () {
        if ($scope.newPassword != "" && $scope.confirmNewPassword != "") {
            if ($scope.newPassword == $scope.confirmNewPassword) {
                if ($scope.currentUserId != "")
                {
                    $scope.passwordData = {
                        requestId: $scope.requestId,
                        tpaId: $scope.tpaId,
                        password: $scope.newPassword,
                        systemUserId: $scope.currentUserId
                    };
                    $scope.err = 0;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/ChangeForgotPasssword',
                        data: $scope.passwordData
                    }).success(function (data, status, headers, config) {
                        if (data == true)
                        {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "You password has successfuly changed. Please login with new credentials.",
                                confirmButtonColor: "#007AFF"
                            });
                            $location.path('login/signin/' + $scope.tpaName);
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