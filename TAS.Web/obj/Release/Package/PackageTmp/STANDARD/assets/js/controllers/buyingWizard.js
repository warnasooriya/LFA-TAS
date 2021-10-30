'use strict';
app.controller('BuyingWizardCtrl',
    function ($scope, $http, SweetAlert, $localStorage, $cookieStore, $stateParams, $state, ngDialog, toaster) {

        $scope.tempInvId = $stateParams.tempInvId;
        $scope.nationalities = [];
        $scope.countries = [];
        $scope.cities = [];
        $scope.usageTypes = [];
        $scope.customerTypes = [];
        $scope.isMobileView = detectmob();
        $scope.customerLoggedIn = false;
        function detectmob() {
            if (window.innerWidth <= 800) {
                return true;
            } else {
                return false;
            }
        }
        $scope.tpaId = $cookieStore.get('tpaId');
        $scope.initiateScreen = function () {
            if (!isGuid($scope.tempInvId)) {
                customErrorMessage("Something went wrong! Please complete the previous step.");
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllNationalities',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.nationalities = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCountriesThatHaveDealers',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.countries = data.Countries;
                if (data.Countries.length == 1) {
                    $scope.customer.countryId = data.Countries[0].Id;
                    $scope.selectedCountryChanged();
                }
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllUsageTypes',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.usageTypes = data;
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCustomerTypes',
                data: { 'tpaId': $scope.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.customerTypes = data;

                angular.forEach($scope.customerTypes, function (value) {
                    if (value.CustomerTypeName === 'Individual') {
                        $scope.customer.customerTypeId = value.Id;
                        $scope.selectedCustomerTypeIdChanged();
                        return false;
                    }
                });
            }).error(function (data, status, headers, config) {
            });

        }
        $scope.navigateToPreviousPage = function () {
            $state.go('home.homePremiumWithTemp', { prodId: $localStorage.prodId, tempInvId: $scope.tempInvId });
        }
        $scope.selectedCountryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCitiesByCountryId',
                data: {
                    'tpaId': $scope.tpaId,
                    'countryId': $scope.customer.countryId
                }
            }).success(function (data, status, headers, config) {
                $scope.cities = data;

            }).error(function (data, status, headers, config) {
            });
        }

        var SearchReturningPopup;
        var ForgetPasswordReturningPopup;
        $scope.tpaName = $localStorage.tpaName;
        $localStorage.tpaName = $scope.tpaName;
        $scope.tpaID = null;
        $scope.loginIconBtnClass = "fa fa-arrow-circle-right";
        $scope.loginBtnDisabled = false;

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        var validateEmail = function (email) {
            if (email === "") return false;
            var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        }

        var sqlDateTimeMinValue = function () {
            return "1/1/1753";
        }


        $http({
            method: 'POST',
            url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
            data: {
                "ImageId": $cookieStore.get('ImageId'),
                "tpaId": $cookieStore.get('tpaId'), "test": 0
            }
        }).success(function (data, status, headers, config) {
            $scope.TpaLogoSrc = data;
            $scope.selectedCustomerTypeIdChanged();

        }).error(function (data, status, headers, config) {

        });

        var customErrorMessage = function (msg) {
            toaster.pop('error', '', msg);
        };


        $scope.customer = {
            customerId: emptyGuid(),
            customerTypeId: emptyGuid(),
            firstName: '',
            lastName: '',
            dateOfBirth: '1950-01-01',
            idIssueDate: '1950-01-01',
            usageTypeId: 0,
            gender: '',
            idTypeId: emptyGuid(),
            idNo: '',
            nationalityId:0,
            countryId: emptyGuid(),
            cityId: emptyGuid(),
            mobileNo: '+971 ',
            otherTelNo: '',
            email: '',
            address1: '',
            address2: '',
            address3: '',
            address4: '',
            businessName: '',
            businessTelNo: '',
            businessAddress1: '',
            businessAddress2: '',
            businessAddress3: '',
            businessAddress4: '',
            password: '',
            cpassword: ''
        };


        //------------------------ Returning Customer Login -------------------------------

        $scope.loadTPAId = function () {
            $scope.customerId = '';
            //swal({ title: 'Processing...!', text: "Validating Customer User Name.", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/BuyingWizard/GetCustomerIdByName',
                data: { "customerUserName": $scope.user.userName, "tpaName": $scope.tpaName }
                //async: false
            }).success(function (data, status, headers, config) {
                $scope.customerdata = data[0];
                $scope.customerId = $scope.customerdata.Id;
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                //  swal.close();
                //if (!isGuid($scope.customerId)) {
                //    customErrorMessage("Customer User Name Invalid")
                //    //swal("TAS Information", "Customer User Name Invalid.", "error");
                //    swal.close();
                //} else {
                //    swal.close();
                //}
            });;
        }
        $scope.loginAttempt = function () {

            if ($scope.user.userName == undefined || $scope.user.password == undefined || $scope.user.userName.length == 0 || $scope.user.password.length == 0) {
                $scope.Errormsg = "Please enter login credentials";
                $scope.err = 1;
            } else {
                $scope.loginBtnDisabled = true;
                $scope.loginIconBtnClass = "fa fa-spinner fa-spin";

                var data = JSON.stringify({ 'UserName': $scope.user.userName, 'Password': $scope.user.password, 'tpaID': $localStorage.tpaID, "tpaName": $scope.tpaName });
                //var data = JSON.stringify({ 'UserName': $scope.user.userName, 'Password': $scope.user.password, 'customerID': $scope.customerId, 'tpaID': $localStorage.tpaID });
                var request = $http({
                    method: "post",
                    url: "/TAS.Web/api/BuyingWizard/CustomerLoginAuth",
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt }
                })

                request.success(function (response) {
                    if (response == "Invalid") {
                        customErrorMessage("Invalid Username Or Password")
                        //$scope.Errormsg = "Invalid Username Or Password";
                        //$scope.err = 1;
                        //$scope.loginBtnDisabled = false;
                        //$scope.loginIconBtnClass = "fa fa-arrow-circle-right";
                    } else if (response == "Error") {
                        $scope.Errormsg = "System Error";
                        $scope.err = 1;
                        $scope.loginBtnDisabled = false;
                        $scope.loginIconBtnClass = "fa fa-arrow-circle-right";
                    } else {

                        $localStorage.LoggedInCustomerId = response.LoggedInCustomerId;
                        $scope.customer.customerId = response.LoggedInCustomerId;
                        $scope.customerLoggedIn = true;
                        $scope.customer.password = $scope.user.password;

                        //$localStorage.LoggedInCustomerId = response.LoggedInCustomerId;
                        //$state.go('home.customerPro', { tpaId: $localStorage.tpaName, customerId: $localStorage.LoggedInCustomerId });
                        $scope.viewEmailSentAndSuccessMsg();
                        if (typeof SearchReturningPopup != 'undefined')
                            SearchReturningPopup.close();
                    }

                });
                request.error(function (response) {
                    $scope.Errormsg = "Error Occured.";
                    $scope.loginBtnDisabled = false;
                    $scope.loginIconBtnClass = "fa fa-arrow-circle-right";
                });

            }

        }


        $scope.selectedCustomerTypeIdChanged = function () {
            if (parseInt($scope.customer.customerTypeId)) {
                angular.forEach($scope.customerTypes, function (value) {
                    if ($scope.customer.customerTypeId == value.Id) {
                        $scope.selectedCustomerTypeName = value.CustomerTypeName;
                        return false;
                    }
                });
            }
            if (parseInt($scope.customer.customerTypeId)) {

                angular.forEach($scope.customerTypes, function (value) {
                    if ($scope.customer.customerTypeId == value.Id) {

                        if (value.CustomerTypeName == "Corporate") {
                            $scope.isCommodityUsageTypeFreez = true;
                            angular.forEach($scope.usageTypes, function (valuec) {
                                if (valuec.UsageTypeName == "Commercial") {
                                    $scope.customer.usageTypeId = valuec.Id;

                                }
                            });

                            angular.forEach($scope.commodityUsageTypes, function (valuec) {
                                if (valuec.Name == "Commercial") {
                                    $scope.product.commodityUsageTypeId = valuec.Id;

                                }
                            });
                        } else if (value.CustomerTypeName == "Individual") {
                            angular.forEach($scope.usageTypes, function (valuec) {
                                if (valuec.UsageTypeName == "Private") {
                                    $scope.customer.usageTypeId = valuec.Id;

                                }
                            });

                        }
                    }
                });
            }

            //} else {
            //    $scope.selectedCustomerTypeName = '';
            //}
        }

        $scope.navigateToProductsPage = function () {
            $state.go('home.products', { tpaId: $localStorage.tpaName });
        }

        $scope.ReturningSearchPopup = function () {
            //$scope.loadTPAId();
            SearchReturningPopup = ngDialog.open({
                template: 'popUpSearchReturning',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            return true;
        };

        $scope.ReturningForgetPasswordPopup = function () {
            if (typeof SearchReturningPopup != 'undefined')
                SearchReturningPopup.close();
            ForgetPasswordReturningPopup = ngDialog.open({
                template: 'popUpForgetPassword',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            return true;
        };

        $scope.navigateToLogin = function () {
            ForgetPasswordReturningPopup.close();

            SearchReturningPopup = ngDialog.open({
                template: 'popUpSearchReturning',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            return true;
        };

        $scope.getUserName = function () {

            if ($scope.customer.email != "" || $scope.customer.email != undefined || $scope.customer.email != null) {
                var userNameEmail;
                userNameEmail = $scope.customer.email;
                $scope.customer.userName = userNameEmail;
                $scope.customerUserDisabled = true;
            }

        }

        // ------------------------ Customer Validation -------------------------------------
        $scope.validateCustomer = function () {
            var isValid = true;

            if ($scope.customer.usageTypeId == "" || $scope.customer.usageTypeId == 0) {
                isValid = false;
                $scope.validate_usageTypeId = "has-error";
            } else {
                $scope.validate_usageTypeId = "";
            }

            if (!isGuid($scope.customer.countryId)) {
                isValid = false;
                $scope.validate_countryId = "has-error";
            } else {
                $scope.validate_countryId = "";
            }

            if (!isGuid($scope.customer.cityId)) {
                isValid = false;
                $scope.validate_cityId = "has-error";
            } else {
                $scope.validate_cityId = "";
            }

            if ($scope.customer.mobileNo == "") {
                isValid = false;
                $scope.validate_mobileNo = "has-error";
            } else {
                $scope.validate_mobileNo = "";
            }
            if ($scope.customer.firstName == "") {
                isValid = false;
                $scope.validate_firstName = "has-error";
            } else {
                $scope.validate_firstName = "";
            }

            if (!validateEmail($scope.customer.email)) {
                isValid = false;
                $scope.validate_email = "has-error";
            } else {
                $scope.validate_email = "";
            }

            if ($scope.customer.dateOfBirth == "dd-MMM-yyyy" || $scope.customer.dateOfBirth == undefined) {
                isValid = false;
                $scope.validate_dateOfBirth = "has-error";
            } else {
                $scope.validate_dateOfBirth = "";
            }

            if ($scope.customer.password.length <= 5) {
                isValid = false;
                $scope.validate_password = "has-error";
                customErrorMessage("Password should have at least six characters.");
            } else {
                $scope.validate_password = "";
            }

            if ($scope.customer.password !== $scope.customer.cpassword) {
                isValid = false;
                $scope.validate_cpassword = "has-error";
                customErrorMessage("Password dosen't match Confirm Password.");
            } else {
                $scope.validate_cpassword = "";
            }

            if ($scope.selectedCustomerTypeName == "Corporate") {
                if ($scope.customer.businessName == "") {
                    isValid = false;
                    $scope.validate_businessName = "has-error";
                } else {
                    $scope.validate_businessName = "";
                }

                if ($scope.customer.businessTelNo == "") {
                    isValid = false;
                    $scope.validate_businessTelNo = "has-error";
                } else {
                    $scope.validate_businessTelNo = "";
                }

            } else {
                if (!parseInt($scope.customer.nationalityId)) {
                    isValid = false;
                    $scope.validate_nationalityId = "has-error";
                } else {
                    $scope.validate_nationalityId = "";
                }
            }

            return isValid;
        }

        $scope.viewEmailSentAndSuccessMsg = function () {
            if ($scope.customerLoggedIn == true || $scope.validateCustomer()) {
                swal({ title: "TAS Information", text: "Saving policy details.", showConfirmButton: false });
               
                var data = {
                    'customer': $scope.customer,
                    'tempInvId': $scope.tempInvId
                }
                $http({
                    method: 'POST',
                    url: '/TAS.WEB/api/ProductDisplay/SaveCustomerEnterdPolicy',
                    data: {
                        "data": data,
                        "tpaId": $cookieStore.get('tpaId')
                    }
                }).success(function (data, status, headers, config) {
                    if (data.code === "SUCCESS") {
                        SweetAlert.swal({
                            html: true,
                            title: "Success",
                            text: data.msg,
                            type: "success",
                            showCancelButton: false,
                            closeOnConfirm: true,
                            allowEscapeKey: false,
                            confirmButtonText: "Ok, Great !",
                            confirmButtonColor: "#ffa500",
                        }, function () {
                            $scope.clearAll();
                            $state.go('home.products', { tpaId: $localStorage.tpaName });
                        });
                    } else {
                        swal.close();
                        customErrorMessage(data.msg);
                    }
                       
                    
                }).error(function (data, status, headers, config) {

                });

            } else
                customErrorMessage("Please fill all mandatory fields before proceeding");
        }

        $(function () {
            $('[data-toggle="tooltip"]').popover({
                container: 'body'
                
            })
            
        })

        $scope.clearAll = function () {
            $scope.user.userName = '';
            $scope.user.password = '';
        }

        $scope.email = "";
        $scope.submitForgotPasswordRequest = function (email) {
            //alert($scope.email);
            $scope.email = email;
            if ($scope.email != "") {
                if (!validateEmail($scope.email)) {
                    $scope.err = 1;
                    $scope.Errormsg = "Please enter a valid email";
                } else {
                    $scope.err = 0;
                    //we are good to goo
                    if ($scope.tpaId != "") {
                        $scope.forgotPassswordRequest = {
                            tpaId: $scope.tpaId,
                            email: $scope.email,
                        };
                        // var data = JSON.stringify({ 'forgotPassswordRequest': $scope.forgotPassswordRequest });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Customer/ForgotPasswordCustomer',
                            data: $scope.forgotPassswordRequest,
                        }).success(function (data, status, headers, config) {
                            if (data == true) {
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

    });
