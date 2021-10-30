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
            $scope.image = " /TAS.Web/STANDARD/assets/images/new_Loginperkins.jpg";
            $scope.loginBoxStyle = {
                "background": "black",
                "boxAlignClass": "main-login col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-2"
            }
            // $scope.image = "/TAS.Web/STANDARD/assets/images/Newlogincont.jpg";
        }
        else if ($scope.tpaName == "perkinsdemo" || $scope.tpaName == "Perkinsdemo" || $scope.tpaName == "PERKINSDEMO" || $scope.tpaName == "leftfielddemo") {
            $scope.image = " /TAS.Web/STANDARD/assets/images/new_Loginperkins.jpg";
            $scope.loginboxBackground = " background: rgba(228,228,228,0.45)";

        }
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