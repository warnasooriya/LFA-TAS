app.controller('loginForgetPasswordCtrl',
function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {
    $scope.tpaID = $stateParams.tpaId;
    $scope.tpaName = $localStorage.tpaName;
    $scope.loginBtnClass = "btn btn-primary pull-left";
    $scope.submitBtnClass = "btn btn-primary pull-right";
    $scope.err = 0;
    $scope.email = "";
    $scope.Errormsg = ""
    $scope.image = "/TAS.Web/STANDARD/assets/images/new_LoginBg.jpg";
    $scope.copyRightYear = new Date().getFullYear();
    $scope.forgotPassswordRequest = {
        tpaId: "",
        email:"",
    };
    $scope.navigateToLogin = function () {
        $location.path('login/signin/' + $localStorage.tpaName);

    }
    $scope.loginBoxStyle = {
        "background": "rgba(0,0,0,0.28)",
        "boxAlignClass": "main-login col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4"
    }

    $scope.ContinentalCheck = function () {
        if ($scope.tpaName == "continental" || $scope.tpaName == "LasaCon") {

            $scope.loginBtnClass = "btn btn-primaryCon pull-left";
            $scope.submitBtnClass = "btn btn-primaryCon pull-right";
            $scope.loginboxBackground = " background:black";
            $scope.forgotPwdLblColor ="color:#fff"
            $scope.image = " /TAS.Web/STANDARD/assets/images/new_Loginperkins.jpg";
            $scope.loginBoxStyle = {
                "background": "black",
                "loginbox":"1px solid #eaeaea;",
                "boxAlignClass": "main-login col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2",
                "colorOrd": "white",
                "outerLogo":false
            }
            // $scope.image = "/TAS.Web/STANDARD/assets/images/Newlogincont.jpg";
        }
        else if ($scope.tpaName == "perkinsdemo" || $scope.tpaName == "Perkinsdemo" || $scope.tpaName == "PERKINSDEMO" ) {
            $scope.image = " /TAS.Web/STANDARD/assets/images/new_Loginperkins.jpg";
            $scope.loginboxBackground = " background: rgba(228,228,228,0.45)";
            $scope.forgotPwdLblColor = "color:#fff"

        } else {
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

    $scope.submitForgotPasswordRequest = function () {
        //alert($scope.email);
        if ($scope.email != "")
        {
            if (!validateEmail($scope.email))
            {
                $scope.err = 1;
                $scope.Errormsg = "Please enter a valid email";
            } else {
                $scope.err = 0;
                //we are good to goo
                if ($scope.tpaID != "")
                {
                    $scope.forgotPassswordRequest = {
                        tpaId: $scope.tpaID,
                        email: $scope.email,
                    };
                   // var data = JSON.stringify({ 'forgotPassswordRequest': $scope.forgotPassswordRequest });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/ForgotPassword',
                        data: $scope.forgotPassswordRequest,
                    }).success(function (data, status, headers, config) {
                        if (data == true)
                        {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "A link has been successfully sent to your email in order to reset your password.",
                                confirmButtonColor: "#007AFF"
                            });
                            $location.path('login/signin/' + $localStorage.tpaName);
                        } else {
                            SweetAlert.swal({
                                title: "TAS Information",
                                text: "Error occured. Please retry.",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                    }).error(function (data, status, headers, config) {
                    });
                } else {

                }
            }
        } else {
            $scope.err = 1;
            $scope.Errormsg = "Please enter a email";
        }
    }
    function validateEmail(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }
});