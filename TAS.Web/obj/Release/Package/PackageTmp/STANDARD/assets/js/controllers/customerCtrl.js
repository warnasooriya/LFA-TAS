app.controller('CustomerCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, $localStorage, toaster) {
        $scope.ModalName = "Customer Registration";

        $scope.submitBtnIconClass = "";
        $scope.submitBtnDisabled = false;
        $scope.CustomertxtDisabled = false;
        $scope.formAction = true;//true for add new
        $scope.customerSearchGridloading = false;
        $scope.customerGridloadAttempted = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };


        $scope.Customer = {
            Id: emptyGuid(),
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
            BusinessTelNo: '',
            OccupationI: '',
            MaritalStatusId: emptyGuid(),
            Title: '',
            PostalCode: '',
            UserRoles: []
        };
        $scope.businessInfoDisplay = false;
        $scope.RoleList = [];
        $scope.RoleSelectedList = [];
        $scope.RoleSelectedDList = [];
        $scope.settings = {
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false
        };
        $scope.CustomText = {
            buttonDefaultText: ' Please Select ',
            dynamicButtonTextSuffix: ' Item(s) Selected'
        };
        function AddRoles() {
            var index = 0;
            $scope.RoleList = [];
            angular.forEach($scope.UserRoles, function (value) {
                var x = { id: index, code: value.Id, label: value.RoleName };
                $scope.RoleList.push(x);
                index = index + 1;
            });
        }
        function LoadRoles() {
            $scope.RoleSelectedList = [];
            $scope.RoleSelectedDList = [];
            angular.forEach($scope.Customer.UserRoles, function (valueOut) {
                angular.forEach($scope.RoleList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.RoleSelectedList.push(x);
                        $scope.RoleSelectedDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendRoles = function () {
            $scope.RoleSelectedDList = [];
            $scope.Customer.UserRoles = [];
            angular.forEach($scope.RoleSelectedList, function (valueOut) {
                angular.forEach($scope.RoleList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Customer.UserRoles.push(valueIn.code);
                        $scope.RoleSelectedDList.push(valueIn.label);
                    }
                });
            });
        }

        $scope.validation = {
            userName: false,
            password: false
        }


        //-------------------------- search -----------------------------//

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }


        $scope.customerSearchGridSearchCriterias = {
            firstName: "",
            lastName: "",
            eMail: "",
            mobileNo: ""
        };

        var paginationOptionsCustomerSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsCustomer = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'First Name', field: 'FirstName', enableSorting: false, cellClass: 'columCss' },
              { name: 'Last Name', field: 'LastName', enableSorting: false, cellClass: 'columCss', },
              { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Email', field: 'Email', enableSorting: false, cellClass: 'columCss' },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadCustomer(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsCustomerSearchGrid.sort = null;
                    } else {
                        paginationOptionsCustomerSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getCustomerSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsCustomerSearchGrid.pageNumber = newPage;
                    paginationOptionsCustomerSearchGrid.pageSize = pageSize;
                    getCustomerSearchPage();
                });
            }
        };


        var getCustomerSearchPage = function () {
            $scope.customerSearchGridloading = true;
            $scope.customerGridloadAttempted = false;
            var customerSearchGridParam =
                {
                    'paginationOptionsCustomerSearchGrid': paginationOptionsCustomerSearchGrid,
                    'customerSearchGridSearchCriterias': $scope.customerSearchGridSearchCriterias
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomersForSearchGrid',
                data: customerSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsCustomer.data = response_arr.data;
                $scope.gridOptionsCustomer.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.customerSearchGridloading = false;
                $scope.customerGridloadAttempted = true;
            });
        };

        $scope.refresCustomerSearchGridData = function () {
            getCustomerSearchPage();
        }

        $scope.loadCustomer = function (customerId) {
            if (isGuid(customerId)) {
                SearchCustomerPopup.close();
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
                    $scope.Customer.CountryId = data.CountryId;
                    $scope.Customer.MobileNo = data.MobileNo;
                    $scope.Customer.OtherTelNo = data.OtherTelNo;
                    $scope.Customer.FirstName = data.FirstName;
                    $scope.Customer.LastName = data.LastName;
                    $scope.Customer.DateOfBirth = data.DateOfBirth;
                    $scope.Customer.Email = data.Email;
                    $scope.Customer.CustomerTypeId = data.CustomerTypeId;
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
                    $scope.Customer.BusinessName = data.BusinessName;
                    $scope.Customer.BusinessAddress1 = data.BusinessAddress1;
                    $scope.Customer.BusinessAddress2 = data.BusinessAddress2;
                    $scope.Customer.BusinessAddress3 = data.BusinessAddress3;
                    $scope.Customer.BusinessAddress4 = data.BusinessAddress4;
                    $scope.Customer.BusinessTelNo = data.BusinessTelNo;
                    $scope.Customer.TitleId = data.TitleId;
                    $scope.Customer.OccupationId = data.OccupationId;
                    $scope.Customer.MaritalStatusId = data.MaritalStatusId;
                    $scope.Customer.MobileNo = data.MobileNo;
                    $scope.Customer.OtherTelNo = data.OtherTelNo;
                    $scope.Customer.PostalCode = data.PostalCode;
                    $scope.Customer.Password = data.Password;
                    $scope.Customer.passwordAgain = data.Password;
                    SetUserValues();
                    $scope.Policy.Customer = $scope.Customer;
                    $scope.Policy.CustomerId = $scope.Customer.Id;
                    if (data.DateOfBirth == null) {
                        $scope.Customer.DateOfBirth = '1753-01-01'
                    }
                    if (data.DLIssueDate == null) {
                        $scope.Customer.DLIssueDate = '1753-01-01'
                    }
                    
                }).error(function (data, status, headers, config) {
                    // clearCustomerControls();
                }).finally(function () {
                    $scope.Customer.CustomerSearchDisabled = false;
                    $scope.submitBtnDisabled = false;
                    swal.close();
                });
            }
        }

        LoadDetails();
        clearCustomerControls();

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
                url: '/TAS.Web/api/Customer/GetAllIdTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.IdTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCustomers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Customers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUserRoles',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UserRoles = data;
                AddRoles();
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

        $scope.CustomerSearchPopupReset = function () {
            $scope.customerSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                eMail: '',
                mobileNo: ''
            }
        }
        $scope.CustomerSearchPopup = function () {
            $scope.customerSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                eMail: '',
                mobileNo: ''
            };
            var paginationOptionsCustomerSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getCustomerSearchPage();
            SearchCustomerPopup = ngDialog.open({
                template: 'popUpSearchCustomer',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });


        };
        //$scope.SearchUser = function () {
        //    $scope.myUserSearch = [];
        //    angular.forEach($scope.Customers, function (value, key) {
        //        $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
        //    });
        //    $scope.tempList = [];

        //    if ($rootScope.searchUser.firstName != "") {
        //        angular.copy($scope.myUserSearch, $scope.tempList);
        //        $scope.myUserSearch = [];
        //        angular.forEach($scope.tempList, function (value, key) {
        //            if ($rootScope.searchUser.firstName == value.FirstName) {
        //                $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
        //            }
        //        });
        //    }
        //    if ($rootScope.searchUser.lastName != "") {
        //        angular.copy($scope.myUserSearch, $scope.tempList);
        //        $scope.myUserSearch = [];
        //        angular.forEach($scope.tempList, function (value, key) {
        //            if ($rootScope.searchUser.lastName == value.LastName) {
        //                $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
        //            }
        //        });
        //    }
        //    if ($rootScope.searchUser.email != "") {
        //        angular.copy($scope.myUserSearch, $scope.tempList);
        //        $scope.myUserSearch = [];
        //        angular.forEach($scope.tempList, function (value, key) {
        //            if ($rootScope.searchUser.email == value.Email) {
        //                $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
        //            }
        //        });
        //    }
        //    if ($rootScope.searchUser.mobileNo != "") {
        //        angular.copy($scope.myUserSearch, $scope.tempList);
        //        $scope.myUserSearch = [];
        //        angular.forEach($scope.tempList, function (value, key) {
        //            if ($rootScope.searchUser.mobileNo == value.MobileNo) {
        //                $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
        //            }
        //        });
        //    }
        //}
        function SetUserValues() {
            $scope.errorTab1 = "";
            //$scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            //angular.forEach($scope.mySelectedRows, function (value, key) {
            if ($scope.Customer.Id != null) {
                   // $scope.Customer.Id = value.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/GetCustomerById',
                        data: { "Id": $scope.Customer.Id },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data.status == "Policy") {
                            //clearCustomerControls();
                            //$scope.submitBtnDisabled = true;
                            $scope.CustomertxtDisabled = true;
                            SweetAlert.swal({
                                title: "Customer Information",
                                text: "Cannot Update Customer. Please Raise a Policy Endorsement!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        else if (data.status == "Bordx") {
                            //clearCustomerControls();
                            //$scope.submitBtnDisabled = true;
                            $scope.submitBtnDisabled = true;
                            $scope.CustomertxtDisabled = true;
                            SweetAlert.swal({
                                title: "Customer Information",
                                text: "Cannot Edit Already Added to Bordx: Please Raise Bordx Endorsement!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }
                        else {
                            $scope.Customer = data;
                            $scope.SetCountryValue();
                        }
                    }).error(function (data, status, headers, config) {
                        clearCustomerControls();
                    });
                }
                else {
                    clearCustomerControls();
                }
            //});
        }
        $scope.errorTab1 = "";

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
        $scope.SetCustomerTypeValue = function () {
            var result = filterById($scope.CustomerTypes, $scope.Customer.CustomerTypeId)
            if (result != null) {
                if (result.CustomerTypeName == "Corporate") {
                    $scope.businessInfoDisplay = true;
                } else {
                    $scope.businessInfoDisplay = false;
                }
            }
            else {
                $scope.businessInfoDisplay = false;
            }
        }

        $scope.resetAll = function () {
            PhoneCode = "";
            $scope.Customer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.UserName = "";
            $scope.Customer.NationalityId = 0;
            $scope.Customer.CountryId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.MobileNo = "";
            $scope.Customer.OtherTelNo = "";
            $scope.Customer.FirstName = "";
            $scope.Customer.LastName = "";
            $scope.Customer.DateOfBirth = "";
            $scope.Customer.Email = "";
            $scope.Customer.Password = "";
            $scope.Customer.Address1 = "";
            $scope.Customer.Address2 = "";
            $scope.Customer.Address3 = "";
            $scope.Customer.Address4 = "";
            $scope.Customer.IDNo = "";
            $scope.Customer.IDTypeId = 1;
            $scope.Customer.DLIssueDate = "";
            $scope.Customer.ProfilePicture = "";
            $scope.Customer.Gender = "M";
            $scope.Customer.IsActive = "";
            $scope.Customer.CustomerTypeId = "";
            $scope.Customer.UsageTypeId = "";
            $scope.Customer.CityId = "";
            $scope.Customer.Email = "";
            $scope.Customer.BusinessName = "";
            $scope.Customer.BusinessAddress1 = "";
            $scope.Customer.BusinessAddress2 = "";
            $scope.Customer.BusinessAddress3 = "";
            $scope.Customer.BusinessAddress4 = "";
            $scope.Customer.BusinessTelNo = "";
            $scope.Customer.UserRoles = [];
            $scope.Customer.passwordAgain = "";
            $scope.Customer.RoleSelectedList = [];
            $scope.Customer.RoleSelectedDList = [];
            $scope.Customer.businessInfoDisplay = false;
            $scope.Customer.PostalCode = "";
            $scope.Customer.OccupationId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.MaritalStatusId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.TitleId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.CityDisabled = true;
            $scope.formAction = true;//true for add new
            $scope.submitBtnDisabled = false;
            $scope.CustomertxtDisabled = false;

        }

        function clearCustomerControls() {
            PhoneCode = "";
            $scope.Customer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.UserName = "";
            $scope.Customer.NationalityId = 0;
            $scope.Customer.CountryId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.MobileNo = "";
            $scope.Customer.OtherTelNo = "";
            $scope.Customer.FirstName = "";
            $scope.Customer.LastName = "";
            $scope.Customer.DateOfBirth = "";
            $scope.Customer.Email = "";
            $scope.Customer.Password = "";
            $scope.Customer.Address1 = "";
            $scope.Customer.Address2 = "";
            $scope.Customer.Address3 = "";
            $scope.Customer.Address4 = "";
            $scope.Customer.IDNo = "";
            $scope.Customer.IDTypeId = 1;
            $scope.Customer.DLIssueDate = "";
            $scope.Customer.ProfilePicture = "";
            $scope.Customer.Gender = "M";
            $scope.Customer.IsActive = "";
            $scope.Customer.CustomerTypeId = "";
            $scope.Customer.UsageTypeId = "";
            $scope.Customer.CityId = "";
            $scope.Customer.Email = "";
            $scope.Customer.BusinessName = "";
            $scope.Customer.BusinessAddress1 = "";
            $scope.Customer.BusinessAddress2 = "";
            $scope.Customer.BusinessAddress3 = "";
            $scope.Customer.BusinessAddress4 = "";
            $scope.Customer.BusinessTelNo = "";
            $scope.Customer.UserRoles = [];
            $scope.Customer.passwordAgain = "";
            $scope.Customer.RoleSelectedList = [];
            $scope.Customer.RoleSelectedDList = [];
            $scope.Customer.businessInfoDisplay = false;
            $scope.Customer.PostalCode = "";
            $scope.Customer.OccupationId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.MaritalStatusId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.TitleId = "00000000-0000-0000-0000-000000000000";
            $scope.Customer.CityDisabled = true;
            $scope.formAction = true;//true for add new
        };
        var today = new Date();
        $scope.MaxDate = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());

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
                $scope.errorTab1 = "";
                if ($scope.Customer.Id == null || $scope.Customer.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.submitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.submitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Customer/AddCustomer',
                        data: $scope.Customer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;

                        $scope.submitBtnIconClass = "";
                        $scope.submitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "Customer Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Customer/GetAllCustomers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Customers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearCustomerControls();
                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Customer Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.submitBtnIconClass = "";
                        $scope.submitBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.submitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.submitBtnDisabled = true;
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
                            SweetAlert.swal({
                                title: "Customer Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Customer/GetAllCustomers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Customers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearCustomerControls();
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
                        $scope.submitBtnIconClass = "";
                        $scope.submitBtnDisabled = false;
                        return false;
                    });
                }

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
        $scope.checkPasswordMatch = function () {

            if ($scope.Customer.Password != null && $scope.Customer.passwordAgain != null) {
                if ($scope.Customer.Password == $scope.Customer.passwordAgain) {
                    $scope.validation.password = false;
                } else {
                    $scope.validation.password = true;
                }
            }
        };
        function filterById(input, id) {
            var i = 0, len = input.length;
            for (; i < len; i++) {
                if (input[i].Id == id) {
                    return input[i];
                }
            }
            return null;
        }

    });



