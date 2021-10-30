app.controller('CustomerProfileCtrl',
    function ($scope, $rootScope, $http, flowFactory, FileUploader, SweetAlert, $localStorage) {
        $scope.ModalName = "Customer Profile";

        $scope.removeImage = function () {
            $scope.noImage = true;
        };

        $scope.obj = new Flow();

        $scope.customer = {
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

        if ($scope.customer.ProfilePictureSrc == '') {
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

        var PhoneCode = "";
        $scope.SetCountryValue = function () {
            if ($scope.customer.countryId != null) {
                angular.forEach($scope.Countries, function (valueC, key) {
                    if ($scope.Customer.CountryId == valueC.Id) {
                        PhoneCode = valueC.PhoneCode;
                        if ($scope.Customer.MobileNo == "")
                            $scope.Customer.MobileNo = PhoneCode;
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                    data: { "countryId": $scope.customer.CountryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    $scope.Cities = data;
                }).error(function (data, status, headers, config) {
                    $scope.Cities = null;
                });
            }
            else {
                $scope.Cities = null;
            }
        }

        $scope.loadCustomer = function () {
            if ($scope.countries != undefined
                && $scope.Nationalities != undefined
                && $scope.IdTypes != undefined
                && $scope.IdTypes != undefined
                && $scope.Occupations != undefined
                && $scope.MaritalStatuses != undefined
                && $scope.Titles != undefined
                && $scope.CustomerTypes != undefined
                && $scope.Cities != undefined
                && $scope.Countries != undefined) {
                return false;
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetCustomerById',
                data: { "Id": $rootScope.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.customer = data;
                $scope.customer.Address = $scope.customer.Address1 + "" + $scope.customer.Address2 + "" + $scope.customer.Address3 + "" + $scope.customer.Address4;
                $scope.customer.g = data.Gender == "M" ? "Male" : "Female";
                $scope.customer.Active = data.IsActive == true ? "Active" : "Inactive";
                $scope.SetDateofBirth();
                $scope.customer.ProfilePictureSrc = data.ProfilePictureSrc;
                $scope.noImage = false;
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetAllCountries'
                //}).success(function (data, status, headers, config) {
                //    $scope.countries = data;
                angular.forEach($scope.countries, function (value) {
                    if (value.Id == $scope.customer.CountryId)
                        $scope.customer.Country = value.CountryName;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetAllNationalities'
                //}).success(function (data, status, headers, config) {
                //    $scope.Nationalities = data;
                angular.forEach($scope.Nationalities, function (value) {
                    if (value.Id == $scope.customer.NationalityId)
                        $scope.customer.Nationality = value.NationalityName;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetAllIdTypes'
                //}).success(function (data, status, headers, config) {
                //    $scope.IdTypes = data;
                angular.forEach($scope.IdTypes, function (value) {
                    if (value.Id == $scope.customer.IDTypeId)
                        $scope.customer.IDType = value.IDTypeName;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetOccupations'
                //}).success(function (data, status, headers, config) {
                //    $scope.Occupations = data;
                angular.forEach($scope.Occupations, function (value) {
                    if (value.Id == $scope.customer.OccupationId)
                        $scope.customer.Occupation = value.Name;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetMarritalStatuses'
                //}).success(function (data, status, headers, config) {
                //    $scope.MaritalStatuses = data;
                angular.forEach($scope.MaritalStatuses, function (value) {
                    if (value.Id == $scope.customer.MaritalStatusId)
                        $scope.customer.MaritalStatus = value.Name;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetTitles'
                //}).success(function (data, status, headers, config) {
                //    $scope.Titles = data;
                angular.forEach($scope.Titles, function (value) {
                    if (value.Id == $scope.customer.TitleId)
                        $scope.customer.Title = value.Name;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetAllCustomerTypes'
                //}).success(function (data, status, headers, config) {
                //    $scope.CustomerTypes = data;
                angular.forEach($scope.CustomerTypes, function (value) {
                    if (value.Id == $scope.customer.CustomerTypeId)
                        $scope.customer.CustomerType = value.CustomerTypeName;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/Customer/GetAllCitiesByCountry',
                //    data: { "countryId": $scope.customer.CountryId }
                //}).success(function (data, status, headers, config) {
                //    $scope.Cities = data;
                angular.forEach($scope.Cities, function (value) {
                    if (value.Id == $scope.customer.CityId)
                        $scope.customer.City = value.CityName;
                });
                //}).error(function (data, status, headers, config) {
                //});
                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/PolicyReg/GetPoliciesFromCustomerId',
                //    data: { "Id": $scope.customer.Id }
                //}).success(function (data, status, headers, config) {
                //    $scope.Policies = data;
                //    $http({
                //        method: 'POST',
                //        url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities'
                //    }).success(function (data, status, headers, config) {
                //        $scope.CommodityTypes = data;
                //        angular.forEach($scope.Policies, function (valueP) {
                //            angular.forEach($scope.CommodityTypes, function (valueW) {
                //                if (valueP.CommodityTypeId == valueW.CommodityTypeId) {
                //                    valueP.CommodityType = valueW.CommodityTypeDescription;
                //                }
                //            });
                //        });
                //    }).error(function (data, status, headers, config) {
                //    });
                //    $http({
                //        method: 'POST',
                //        url: '/TAS.Web/api/ContractManagement/GetWarrantyTypes'
                //    }).success(function (data, status, headers, config) {
                //        $scope.WarrantyTypes = data;
                //        angular.forEach($scope.Policies, function (valueP) {
                //            angular.forEach($scope.WarrantyTypes, function (valueW) {
                //                if (valueP.CoverTypeId == valueW.Id) {
                //                    valueP.CoverType = valueW.WarrantyTypeDescription;
                //                }
                //            });
                //        });
                //    }).error(function (data, status, headers, config) {
                //    });
                //}).error(function (data, status, headers, config) {
                //});
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.SetDateofBirth = function()
        {
            var myDate = $scope.customer.DateOfBirth;
            if (myDate != null) {
                myDate = myDate.split("-");
                var newDate = myDate[1] + "/" + myDate[2].substring(0, 2) + "/" + myDate[0];
                var d = new Date(newDate).getTime();
                $scope.customer.DateOfBirthDisplay = d;
            }
        }

        $scope.aa = { bb: "" };

        $scope.submit = function () {
            if ($scope.customer.Email != "") {
                $scope.customer.UserName = $scope.customer.Email;
                $scope.customer.Gender = $scope.customer.g == "Male" ? "M" : "F";
                $scope.customer.IsActive = $scope.customer.Active == "Active" ? true : false;
                $scope.errorTab1 = "";
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Customer/UpdateCustomerProfile',
                    data:  $scope.customer,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: "Customer Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });
                    } else {;
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: "Customer Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    return false;
                });
            }
        }

        $scope.checkUserNameAvailability = function () {
            if ($scope.customer.Email == "" || $scope.customer.Email == null) {
                $scope.validation.userName = false;
            } else {
                $http({
                    method: 'POST',
                    url: 'api/Customer/GetCustomerByUserName',
                    data: { "username": $scope.customer.Email },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    if (data == true) {
                        $scope.validation.userName = true;
                    } else {
                        $scope.validation.userName = false;
                    }
                }).error(function (data, status, headers, config) {
                });
            }
        };
        $scope.checkPasswordMatch = function () {

            if ($scope.customer.password!=null && $scope.passwordAgain!=null) {
                if ($scope.customer.password == $scope.passwordAgain) {
                    $scope.validation.password = false;
                } else {
                    $scope.validation.password = true;
                }
            }
        };



    });



