app.controller('CustomerProCtrl',
    function ($scope, $rootScope, $http, flowFactory, ngDialog, FileUploader, $localStorage, SweetAlert, toaster) {

        $scope.customerId = $localStorage.LoggedInCustomerId;
        $scope.removeImage = function () {
            $scope.noImage = true;
        };

        $scope.obj = new Flow();

        $scope.Customer = {
            FirstName: '',
            LastName: '',
            Email: '',
            CustomerType: '',
            Gender: 'M',
            Nationality: '',
            Country: '',
            Address1: '',
            Address2: '',
            Address3: '',
            Address4: '',
            IsActive: '',
            ProfilePictureSrc: '',
            DateOfBirth: '',
            IDType: '',
            IdNo: '',
            City: '',
            BusinessTelNo: ''
        };

    

        $scope.changePasswordModel = {
            oldPassword: "",
            newPassword: "",
            confirmPassword: "",
            userId: $localStorage.LoggedInCustomerId
        };

        if ($scope.Customer.ProfilePictureSrc == '') {
            $scope.noImage = true;
        }

        $scope.validation = {
            userName: false,
            password: false
        }

        var uploader1 = $scope.obj.flow = new FileUploader({
            url: window.location.protocol + '//' + window.location.host + '/TAS.Web/api/Customer/UploadImage'
        });

        LoadDetails();

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllNationalities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Nationalities = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomerTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CustomerTypes = data;
                $scope.loadCustomer($scope.customerId);
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UsageTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Cities = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllIdTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.IdTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetOccupations',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Occupations = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetTitles',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Titles = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetMarritalStatuses',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.MaritalStatuses = data;
            }).error(function (data, status, headers, config) {
            });
           
        }

        $scope.loadCustomer = function (customerId) {
            //$scope.customerId = 'DB3E1758-78B5-4CEA-A8C9-2D8CE392C2CF';
            if (isGuid(customerId)) {
               // SearchCustomerPopup.close();
                swal({ title: "TAS Information", text: "Requesting customer information..", showConfirmButton: false });
                $scope.Customer.CustomerSearchDisabled = true;
                $scope.submitBtnDisabled = true;
                $scope.formAction = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetCustomerById',
                    data: { "Id": customerId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Customer.CustomerSearchDisabled = false;
                    $scope.submitBtnDisabled = false;
                    $scope.Customer.Id = data.Id;
                    $scope.Customer.NationalityId = data.NationalityId;
                    $scope.Customer.Nationality = data.Nationality;
                    $scope.Customer.CountryId = data.CountryId;
                    $scope.Customer.Country = data.Country;
                    $scope.Customer.MobileNo = data.MobileNo;
                    $scope.Customer.OtherTelNo = data.OtherTelNo;
                    $scope.Customer.FirstName = data.FirstName;
                    $scope.Customer.LastName = data.LastName;
                    $scope.Customer.DateOfBirth = data.DateOfBirth;
                    $scope.Customer.Email = data.Email;
                    $scope.Customer.CustomerTypeId = data.CustomerTypeId;
                    $scope.Customer.CustomerType = data.CustomerType;
                    $scope.Customer.UsageTypeId = data.UsageTypeId;
                    $scope.SetCountryValue();
                    $scope.selectedCustomerTypeIdChanged();
                    $scope.Customer.CityId = data.CityId;
                    $scope.Customer.Email = data.Email;
                    $scope.Customer.Address1 = data.Address1;
                    $scope.Customer.Address2 = data.Address2;
                    $scope.Customer.Address3 = data.Address3;
                    $scope.Customer.Address4 = data.Address4;
                    $scope.Customer.IDNo = data.IDNo;
                    $scope.Customer.IDTypeId = data.IDTypeId;
                    $scope.Customer.DLIssueDate = data.DLIssueDate;
                    $scope.Customer.Gender = data.Gender;
                    if (data.Gender == "F") {
                        $scope.Customer.GenderN = "Female";
                    } else {
                        $scope.Customer.GenderN = "Male";
                    }
                    $scope.Customer.BusinessName = data.BusinessName;
                    $scope.Customer.BusinessAddress1 = data.BusinessAddress1;
                    $scope.Customer.BusinessAddress2 = data.BusinessAddress2;
                    $scope.Customer.BusinessAddress3 = data.BusinessAddress3;
                    $scope.Customer.BusinessAddress4 = data.BusinessAddress4;
                    $scope.Customer.BusinessTelNo = data.BusinessTelNo;
                    $scope.Customer.TitleId = data.TitleId
                    $scope.Customer.Title = data.Title;
                    $scope.Customer.OccupationId = data.OccupationId;
                    $scope.Customer.Occupation = data.Occupation;
                    $scope.Customer.MaritalStatusId = data.MaritalStatusId;
                    $scope.Customer.MobileNo = data.MobileNo;
                    $scope.Customer.OtherTelNo = data.OtherTelNo;
                    $scope.Customer.PostalCode = data.PostalCode;
                    $scope.Customer.Password = data.Password;
                    $scope.Customer.passwordAgain = data.Password;
                    $scope.getcustomerpolicy();

                }).error(function (data, status, headers, config) {
                    // clearCustomerControls();
                }).finally(function () {
                    $scope.Customer.CustomerSearchDisabled = false;
                    $scope.submitBtnDisabled = false;
                    swal.close();
                });
            }
        }

        // ---- upload ----
        $scope.uploadImage = function () {
            if (uploader1.queue.length > 0) {
                uploader1.queue[0].upload();
            }
        }
        var dialogProfilePic;
        $scope.ChangePicturePopup = function () {

            dialogProfilePic = ngDialog.open({
                template: 'pop-changeProfilePic',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };
        var dialogChangePassword;
        $scope.ChangePasswordPopup = function () {
            dialogChangePassword = ngDialog.open({
                template: 'pop-changePassword',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };
        $scope.changePassword = function () {
            if ($scope.changePasswordModel.oldPassword != ""
                && $scope.changePasswordModel.newPassword != ""
                && $scope.changePasswordModel.confirmPassword != "") {
                $scope.changePasswordErrorMsg = "";
                //validation is good
                if ($scope.changePasswordModel.confirmPassword != $scope.changePasswordModel.newPassword) {
                    $scope.changePasswordErrorMsg = "Password confirmation failed";
                }
                else {
                    //we are good to go
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/ChangePassword',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: $scope.changePasswordModel,
                    }).success(function (data, status, headers, config) {
                        if (data) {
                            SweetAlert.swal({
                                title: "User Information",
                                text: "Password successfully changed!",
                                confirmButtonColor: "#007AFF"
                            });
                            dialogChangePassword.close();
                            $scope.changePasswordModel = {
                                oldPassword: "",
                                newPassword: "",
                                confirmPassword: "",
                                userId: $rootScope.LoggedInUserId
                            };
                        } else {
                            $scope.changePasswordErrorMsg = "Old passsword is invalid."
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        $scope.changePasswordErrorMsg = "Error occured.Please retry."
                        return false;
                    });
                }

            } else {
                $scope.changePasswordErrorMsg = "* All fields are mandatory";
            }

        }

        var uploader1 = $scope.uploader1 = new FileUploader({
            url: '/TAS.Web/api/Image/UploadImage',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
        });
        uploader1.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "png" || extension == "jpg" || extension == "bmp" || extension == "gif")
                    return true;
                else {
                    alert('Invalid file format. Please select a file with jpg/png/bmp or gif format  and try again.');
                    return false;
                }
            }
        });
        uploader1.filters.push({
            name: 'sizeFilter',
            fn: function (item, options) {
                var fileSize = item.size;
                fileSize = parseInt(fileSize) / (1024 * 1024);
                if (fileSize <= 5)
                    return true;
                else {
                    alert('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                    return false;
                }
            }
        });
        uploader1.filters.push({
            name: 'itemResetFilter',
            fn: function (item, options) {
                if (this.queue.length < 5)
                    return true;
                else {
                    alert('You have exceeded the limit of uploading files.');
                    return false;
                }
            }
        });
        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            $scope.uploader1.queue = [];
            $scope.uploader1.progress = 0;
            //$scope.User.ProfilePicture = response.replace(/\"/g, "");
            // LoadDetails();
            if (response != null) {
                response = response.replace(/\"/g, "");
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/UpdateUserProfilePic',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { 'ImageId': response, 'Id': $scope.user.Id },
                }).success(function (data, status, headers, config) {
                    dialogProfilePic.close();
                    $localStorage.currentProfilePic = data;
                    $rootScope.user.picture = data;
                    $scope.userProfilePic = $rootScope.user.picture;
                    if (!$rootScope.$$phase) {
                        $rootScope.$apply();
                    }
                    angular.element("input[type='file']").val(null);
                    SweetAlert.swal({
                        title: "User Information",
                        text: "Successfully Saved!",
                        confirmButtonColor: "#007AFF"
                    });
                }).error(function (data, status, headers, config) {
                });
            }
        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            alert('We were unable to upload your file. Please try again.');
        };
        uploader1.onCancelItem = function (fileItem, response, status, headers) {
            // alert('File uploading has been cancelled.');
        };
        uploader1.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader1.onBeforeUploadItem = function (item) {
            console.info('onBeforeUploadItem', item);
        };
        uploader1.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader1.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader1.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader1.onCompleteAll = function () {
            console.info('onCompleteAll');
        };

        $scope.selectedCustomerTypeIdChanged = function () {
            if (parseInt($scope.Customer.CustomerTypeId)) {
                angular.forEach($scope.CustomerTypes, function (value) {
                    if ($scope.Customer.CustomerTypeId == value.Id) {
                        $scope.selectedCustomerTypeName = value.CustomerTypeName;
                        return false;
                    }
                });
                if (parseInt($scope.Customer.CustomerTypeId)) {

                    angular.forEach($scope.CustomerTypes, function (value) {
                        if ($scope.Customer.CustomerTypeId == value.Id) {

                            if (value.CustomerTypeName == "Corporate") {
                                $scope.isCommodityUsageTypeFreez = true;
                                angular.forEach($scope.UsageTypes, function (valuec) {
                                    if (valuec.UsageTypeName == "Commercial") {
                                        $scope.Customer.UsageTypeId = valuec.Id;

                                    }
                                });

                            } else {
                                $scope.isCommodityUsageTypeFreez = false;
                            }
                        }
                    });
                }

            } else {
                $scope.selectedCustomerTypeName = '';
            }
        }

        var PhoneCode = "";
        $scope.SetCountryValue = function () {
            $scope.Customer.MobileNo = "";
            $scope.Customer.OtherTelNo = "";
            $scope.Customer.CityId = "";
            $scope.Customer.CityDisabled = true;
            if ($scope.Customer.CountryId != null) {
                angular.forEach($scope.countries, function (valueC, key) {
                    if ($scope.Customer.CountryId == valueC.Id) {
                        PhoneCode = valueC.PhoneCode;
                        if ($scope.Customer.MobileNo == "") {
                            $scope.Customer.MobileNo = PhoneCode;
                            $scope.Customer.OtherTelNo = PhoneCode;
                        }
                    }
                });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                    data: { "countryId": $scope.Customer.CountryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Cities = data;
                    $scope.Customer.CityDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.Cities = null;
                    $scope.Customer.CityDisabled = false;
                    //$scope.message = 'Unexpected Error';
                });
            }
            else {
                $scope.Cities = null;
                $scope.Customer.CityDisabled = false;
            }
        }

        // --- Update Customer Details ---
        $scope.validateCustomer = function () {
            var isValid = true;

            if ($scope.Customer.CustomerTypeId == "" || $scope.Customer.CustomerTypeId == undefined || $scope.Customer.CustomerTypeId == null) {
                $scope.validate_CustomerTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CustomerTypeId = "";
            }
            if ($scope.Customer.UsageTypeId == "") {
                $scope.validate_UsageTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_UsageTypeId = "";
            }

            if ($scope.Customer.FirstName == "" || $scope.Customer.FirstName == undefined || $scope.Customer.FirstName == null) {
                $scope.validate_FirstName = "has-error";
                isValid = false;
            } else {
                $scope.validate_FirstName = "";
            }

            if ($scope.selectedCustomerTypeName === "Corporate") {

            } else {
                if ($scope.Customer.TitleId == "" || $scope.Customer.TitleId == undefined || $scope.Customer.TitleId == null) {
                    $scope.validate_TitleId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_TitleId = "";
                }
                if ($scope.Customer.OccupationId == "" || $scope.Customer.OccupationId == undefined || $scope.Customer.OccupationId == null) {
                    $scope.validate_OccupationId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_OccupationId = "";
                }
                if ($scope.Customer.MaritalStatusId == "" || $scope.Customer.MaritalStatusId == undefined || $scope.Customer.MaritalStatusId == null) {
                    $scope.validate_MaritalStatusId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_MaritalStatusId = "";
                }
                if ($scope.Customer.NationalityId == "" || $scope.Customer.NationalityId == undefined || $scope.Customer.NationalityId == null) {
                    $scope.validate_NationalityId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_NationalityId = "";
                }
                if ($scope.Customer.IDTypeId == "" || $scope.Customer.IDTypeId == undefined || $scope.Customer.IDTypeId == null) {
                    $scope.validate_IDTypeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_IDTypeId = "";
                }
                if ($scope.Customer.IDNo == "" || $scope.Customer.IDNo == undefined) {
                    $scope.validate_IDNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_IDNo = "";
                }
            }

            if ($scope.Customer.CountryId == "" || $scope.Customer.CountryId == undefined || $scope.Customer.CountryId == null) {
                $scope.validate_CountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryId = "";
            }
            if ($scope.Customer.CityId == "" || $scope.Customer.CityId == undefined || $scope.Customer.CityId == null) {
                $scope.validate_CityId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CityId = "";
            }

            if ($scope.Customer.MobileNo == "" || $scope.Customer.MobileNo == undefined) {
                $scope.validate_MobileNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_MobileNo = "";
            }


            if ($scope.Customer.Password == "" || $scope.Customer.Password == undefined || $scope.Customer.Password == null) {
                $scope.validate_Password = "has-error";
                isValid = false;
            } else {
                $scope.validate_Password = "";
            }
            if ($scope.Customer.passwordAgain == "" || $scope.Customer.passwordAgain == undefined || $scope.Customer.passwordAgain == null) {
                $scope.validate_passwordAgain = "has-error";
                isValid = false;
            } else {
                $scope.validate_passwordAgain = "";
            }
            if ($scope.Customer.Email == "" || $scope.Customer.Email == undefined || $scope.Customer.Email == null) {
                $scope.validate_Email = "has-error";
                isValid = false;
            } else {
                $scope.validate_Email = "";
            }
            return isValid
        }

        $scope.submit = function () {
            var phoneno = /^\d{10,15}$/;
            if ($scope.validateCustomer()) {
                if (!$scope.Customer.MobileNo.replace(/[\s]/g, '').match(phoneno)) { //|| !$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(phoneno)) {
                    customErrorMessage("Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16.")
                    //$scope.errorTab1 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
                    return false;
                }

                if ($scope.selectedCustomerTypeName != "Corporate") {
                    if (!$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(PhoneCode) && (!$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(phoneno))) {
                        customErrorMessage("Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16.")
                        //$scope.errorTab1 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
                        return false;
                    }
                }


                $scope.checkUserNameAvailability();
                $scope.Customer.UserName = $scope.Customer.Email;
                $scope.Customer.IsActive = true;
                if ($scope.validation.userName) {
                    SweetAlert.swal({
                        title: "Customer Information",
                        text: "Email Already Registered!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    return false;
                }
                if ($scope.validation.password) {
                    SweetAlert.swal({
                        title: "Customer Information",
                        text: "Password do not Match!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    return false;
                }
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/UpdateCustomer',
                        data: $scope.Customer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.submitBtnIconClass = "";
                        $scope.submitBtnDisabled = false;
                        if (data == "OK") {
                            customInfoMessage("Successfully Saved!")
                            //SweetAlert.swal({
                            //    title: "Customer Information",
                            //    text: "Successfully Saved!",
                            //    confirmButtonColor: "#007AFF"
                            //});
                        } else {;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        customErrorMessage("Error occured while saving data.")
                        //SweetAlert.swal({
                        //    title: "Customer Information",
                        //    text: "Error occured while saving data!",
                        //    type: "warning",
                        //    confirmButtonColor: "rgb(221, 107, 85)"
                        //});
                        $scope.submitBtnIconClass = "";
                        $scope.submitBtnDisabled = false;
                        return false;
                    });
                

            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }

        $scope.checkUserNameAvailability = function () {

            if ($scope.Customer.Email == "" || $scope.Customer.Email == null) {
                $scope.validation.userName = false;
            } else {
                angular.forEach($scope.Customers, function (value) {
                    if (value.Email == $scope.Customer.Email && $scope.Customer.Id != value.Id) {
                        $scope.validation.userName = true;
                    } else {
                        $scope.validation.userName = false;
                    }
                });

            }
        };

        $scope.getcustomerpolicy = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetPolicesByCustomerId',
                data: { "Id": $scope.Customer.Id },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Policies = data;
            }).error(function (data, status, headers, config) {
            });          
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

    });



