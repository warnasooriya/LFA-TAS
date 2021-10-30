//app.controller('homeLoginCtrl',
//function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {

//    $scope.tpaID = $localStorage.tmpTpaId;
//    var tpaId = $cookieStore.get('tmpTpaId');
//    $scope.link = "";
//    $scope.loadInitailData = function () {
//    }
//    $scope.err = 0;
//    $scope.Errormsg = '';
//    $scope.loginAttempt = function () {

//        if (!$scope.userName && !$scope.password) {
//            $scope.Errormsg = "Please enter login credentials";
//            $scope.err = 1;
//        } else {
//            var data = JSON.stringify({ 'UserName': $scope.userName, 'Password': $scope.password, 'tpaID': $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId });
//            var request = $http({
//                method: "post",
//                url: "/TAS.Web/api/User/LoginAuth",
//                data: data,
//            });
//            request.success(function (response) {
//                if (response == "Invalid") {
//                    $scope.Errormsg = "Invalid Username Or Password";
//                    $scope.err = 1;
//                } else if (response == "Error") {
//                    $scope.Errormsg = "System Error";
//                    $scope.err = 1;
//                } else {



//                    //$cookies.put('jwt', response);
//                    $localStorage.jwt = response.JsonWebToken;
//                    $rootScope.LoggedInUserId = response.LoggedInUserId
//                    $rootScope.UserType = response.UserType

//                    $localStorage.LoggedInUserId = response.LoggedInUserId
//                    $localStorage.UserType = response.UserType

//                    $localStorage.menuType = 2;
//                    $localStorage.tpaID = $scope.tpaID;

//                    $cookieStore.put('jwt', response.JsonWebToken);
//                    $cookieStore.put('LoggedInUserId', response.LoggedInUserId);
//                    $cookieStore.put('UserType', response.UserType);
//                    //var menu =  $http({
//                    //       method: 'POST',
//                    //       url: '/TAS.Web/api/User/GetMenu',
//                    //       headers: { 'Authorization': $localStorage.jwt }
//                    //   }).success(function (data, status, headers, config) {
//                    //       $localStorage.link = data;
//                    //   }).error(function (data, status, headers, config) {
//                    //   });


//                    if ($rootScope.UserType == "IU") {
//                        $http({
//                            method: 'POST',
//                            url: '/TAS.Web/api/User/GetUsersById',
//                            data: { "Id": $rootScope.LoggedInUserId }
//                        }).success(function (data, status, headers, config) {
//                            $localStorage.user = data;
//                            $cookieStore.put('user', data);
//                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
//                        }).error(function (data, status, headers, config) {
//                        });
//                    }
//                    else {
//                        $http({
//                            method: 'POST',
//                            url: '/TAS.Web/api/Customer/GetCustomerById',
//                            data: { "Id": $rootScope.LoggedInUserId }
//                        }).success(function (data, status, headers, config) {
//                            $localStorage.user = data;
//                            $cookieStore.put('user', data);
//                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
//                        }).error(function (data, status, headers, config) {
//                        });
//                    }
//                    //  $location('app/dashboard');
//                    return false;
//                }

//            });
//        }

//    };
//});

app.controller('homeLoginCtrl',
function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {

    //$scope.tpaID = $stateParams.tpaId;
    $scope.tpaID = $stateParams.tpaId;


    $scope.confirmEmail = false;
    $scope.confirmPassword = false;
    $scope.link = "";
    $scope.loadInitailData = function () {
    }
    $scope.err = 0;
    $scope.Errormsg = '';


    $scope.user.Email = "";
    $scope.user.ConfirmEmail = "";
    $scope.user.Password = "";
    $scope.user.ConfirmPassword = "";

    $scope.User = {
        Id: "00000000-0000-0000-0000-000000000000",
        UserName: '',
        NationalityId: 1,
        CountryId: "00000000-0000-0000-0000-000000000000",
        MobileNo: '',
        OtherTelNo: '',
        InternalExtension: '',
        FirstName: '',
        LastName: '',
        DateOfBirth: '',
        Email: '',
        Password: '',
        Address1: '',
        Address2: '',
        Address3: '',
        Address4: '',
        IDNo: '',
        IDTypeId: 1,
        DLIssueDate: '',
        ProfilePicture: '',
        Gender: "M",
        UserRoles: [],
        TPAID : $cookieStore.get('tpaId')
    };

    $scope.loginAttempt = function () {

        if (!$scope.userName && !$scope.password) {
            $scope.Errormsg = "Please enter login credentials";
            $scope.err = 1;
        } else {
            var data = JSON.stringify({ 'UserName': $scope.userName, 'Password': $scope.password, 'TPAID': $scope.tpaID });
            var request = $http({
                method: "post",
                url: "/TAS.Web/api/User/LoginAuth",
                data: data,
            });
            request.success(function (response) {
                if (response == "Invalid") {
                    SweetAlert.swal({
                        title: "Invalid Login!",
                        text: "Oppzz.. Invalid Username Or Password! Please check them again!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });

                    //$scope.Errormsg = "Invalid Username Or Password";
                    $scope.err = 1;
                } else if (response == "Error") {
                    $scope.Errormsg = "System Error";
                    $scope.err = 1;
                } else {



                    //$cookies.put('jwt', response);
                    $localStorage.jwt = response.JsonWebToken;
                    $rootScope.LoggedInUserId = response.LoggedInUserId;
                    $rootScope.UserType = response.UserType;

                    $localStorage.LoggedInUserId = response.LoggedInUserId;
                    $localStorage.UserType = response.UserType;

                    $localStorage.menuType = 2;
                    $localStorage.tpaID = $scope.tpaID;

                    $cookieStore.put('tpaID', $scope.tpaID);
                    $cookieStore.put('jwt', response.JsonWebToken);
                    $cookieStore.put('LoggedInUserId', response.LoggedInUserId);
                    $cookieStore.put('UserType', response.UserType);
                    //var menu =  $http({
                    //       method: 'POST',
                    //       url: '/TAS.Web/api/User/GetMenu',
                    //       headers: { 'Authorization': $localStorage.jwt }
                    //   }).success(function (data, status, headers, config) {
                    //       $localStorage.link = data;
                    //   }).error(function (data, status, headers, config) {
                    //   });


                    if ($rootScope.UserType == "IU") {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/User/GetUsersById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $localStorage.user = data;
                            $cookieStore.put('user', data);
                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetCustomerById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $localStorage.user = data;
                            $cookieStore.put('user', data);
                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    //  $location('app/dashboard');
                    return false;
                }

            });
        }

    };

    $scope.RegisterLogin = function () {

        //if ($scope.checkEmails() && $scope.checkPasswords()) {


        $scope.User.Email = $scope.user.Email;
        $scope.User.Password = $scope.user.Password;
        $scope.User.TPAID = $scope.tpaID;

        if ($scope.User.Id == null || $scope.User.Id == "00000000-0000-0000-0000-000000000000") {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/AddTempUserAcc',
                data: $scope.User
            }).success(function (data, status, headers, config) {
                $scope.Ok = data;
                

                if (data == "EmailExists") {
                    $scope.Errormsg = "Email Already Exist!";
                    $scope.err = 1;
                } else if (data == "Error") {
                    $scope.Errormsg = "System Error!";
                    $scope.err = 1;
                } else {



                    //$cookies.put('jwt', response);
                    $localStorage.jwt = data.JsonWebToken;
                    $rootScope.LoggedInUserId = data.LoggedInUserId
                    $rootScope.UserType = data.UserType

                    $localStorage.LoggedInUserId = data.LoggedInUserId
                    $localStorage.UserType = data.UserType

                    $localStorage.menuType = 2;
                    $localStorage.tpaID = $scope.tpaID;

                    $cookieStore.put('tpaID', $scope.tpaID);
                    $cookieStore.put('jwt', data.JsonWebToken);
                    $cookieStore.put('LoggedInUserId', data.LoggedInUserId);
                    $cookieStore.put('UserType', data.UserType);
                    //var menu =  $http({
                    //       method: 'POST',
                    //       url: '/TAS.Web/api/User/GetMenu',
                    //       headers: { 'Authorization': $localStorage.jwt }
                    //   }).success(function (data, status, headers, config) {
                    //       $localStorage.link = data;
                    //   }).error(function (data, status, headers, config) {
                    //   });
                    var jwt = $cookieStore.get('jwt');

                    if ($rootScope.UserType == "IU") {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/User/GetUsersById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : data.JsonWebToken }
                        }).success(function (data, status, headers, config) {
                            $localStorage.user = data;
                            $cookieStore.put('user', data);
                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/GetCustomerById',
                            data: { "Id": $rootScope.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : data.JsonWebToken }
                        }).success(function (data, status, headers, config) {
                            $localStorage.user = data;
                            $cookieStore.put('user', data);
                            window.location.href = '/TAS.Web/STANDARD/index.html#/home/buyingprocess';
                        }).error(function (data, status, headers, config) {
                        });
                    }
                    //  $location('app/dashboard');
                    return false;
                }


                return false;
            }).error(function (data, status, headers, config) {
                SweetAlert.swal({
                    title: "E-mail Already Exists!",
                    text: "Please User Another E-mail!",
                    type: "warning",
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
                return false;
            });
        }

       // }
    }

    $scope.checkEmails= function () {
        if ($scope.user.Email != "" && $scope.user.ConfirmEmail != "") {
            if ($scope.user.Email == $scope.user.ConfirmEmail) {
                $scope.confirmEmail = false;
               
            } else {
                $scope.confirmEmail = true;
            }
        }else
        {
            $scope.confirmEmail = false;
        }
    }



    $scope.checkPasswords = function () {
        if ($scope.user.Password != "" && $scope.user.ConfirmPassword != "") {
            if ($scope.user.Password == $scope.user.ConfirmPassword) {
                $scope.confirmPassword = false;

            } else {
                $scope.confirmPassword = true;
            }
        } else {
            $scope.confirmPassword = false;
        }
    }
});


