app.controller('UserManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, FileUploader, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "User Management";
        $scope.savebuttonName = "CREATE ACCOUNT";

        $scope.UserSaveBtnIconClass = "";
        $scope.UserSaveBtnDisabled = false;

        $scope.UserSearchGridloadAttempted = false;
        $scope.UserSearchGridloading = false;

        $scope.formAction = true;//true for add new

        var UsersearchPopup;

        $scope.User = {
            Id: "00000000-0000-0000-0000-000000000000",
            UserName: '',
            NationalityId: '',
            CountryId: '',
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
            IDTypeId: '',
            DLIssueDate: '',
            ProfilePicture: '',
            Gender: "M",
            UserRoleMappings: [],
            UserBranches: [],
            RoleId: "00000000-0000-0000-0000-000000000000",
            LanguageId: null,
            IsDealerAccount:false
        };

        $scope.RoleList = [];
        $scope.RoleSelectedList = [];
        $scope.RoleSelectedDList = [];

        $scope.BranchesList = [];
        $scope.BranchesSelectedList = [];
        $scope.BranchesSelectedDList = [];
        $scope.LanguageList = [];
        $scope.maxdate = new Date();


        $scope.settings = {
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false
        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('pages.userManagement.select'),
            dynamicButtonTextSuffix: $filter('translate')('pages.userManagement.ItemSelected'),
        };
        function AddRoles() {
            var index = 0;
            $scope.RoleList = [];
            angular.forEach($scope.UserRoleMappings, function (value) {
                var x = { id: index, code: value.RoleId, label: value.RoleCode };
                $scope.RoleList.push(x);
                index = index + 1;
            });
        }

        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.userManagement.error'), msg);
        };

        function AddBranches() {
            var index = 0;
            $scope.BranchesList = [];
            angular.forEach($scope.UserBranchesMappings, function (value) {
                var x = { id: index, code: value.Id, label: value.BranchName };
                $scope.BranchesList.push(x);
                index = index + 1;
            });
        }

        function LoadRoles() {
            $scope.RoleSelectedList = [];
            $scope.RoleSelectedDList = [];
            angular.forEach($scope.User.UserRoleMappings, function (valueOut) {
                angular.forEach($scope.RoleList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.RoleSelectedList.push(x);
                        $scope.RoleSelectedDList.push(valueIn.label);
                    }
                });
            });
        }

        function LoadBranches() {
            $scope.BranchesSelectedList = [];
            $scope.BranchesSelectedDList = [];
            angular.forEach($scope.User.UserBranches, function (valueOut) {
                angular.forEach($scope.BranchesList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.BranchesSelectedList.push(x);
                        $scope.BranchesSelectedDList.push(valueIn.label);
                    }
                });
            });
        }

        $scope.SendRoles = function () {
            $scope.RoleSelectedDList = [];
            $scope.User.UserRoleMappings = [];
            angular.forEach($scope.RoleSelectedList, function (valueOut) {
                angular.forEach($scope.RoleList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.User.UserRoleMappings.push(valueIn.code);
                        $scope.RoleSelectedDList.push(valueIn.label);
                    }
                });
            });
        }

        $scope.SendBranches = function () {
            $scope.BranchesSelectedDList = [];
            $scope.User.UserBranches = [];
            angular.forEach($scope.BranchesSelectedList, function (valueOut) {
                angular.forEach($scope.BranchesList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.User.UserBranches.push(valueIn.code);
                        $scope.BranchesSelectedDList.push(valueIn.label);
                    }
                });
            });
        }


        $scope.validation = {
            userName: false,
            password: false
        }



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
                url: '/TAS.Web/api/Customer/GetAllIdTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.IdTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUsers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Users = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUserRoles',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UserRoleMappings = data;
                AddRoles();
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/TPABranch/GetTPABranches',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.UserBranchesMappings = data;
                AddBranches();
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'GET',
                url: '/TAS.Web/api/Language/GetAllLanguages',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.LanguageList = data;
            }).error(function (data, status, headers, config) {
            });

        }
        $scope.errorTab1 = "";

        //------------------------------ User Search -------------------------------------//

        $scope.UserSearchGridSearchCriterias = {
            firstName: "",
            lastName: "",
            email: "",
            mobileNo: ""
        };


        var paginationOptionsUserSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };



        $scope.gridOptionsUser = {

            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.userManagement.firstName'), field: 'FirstName', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.userManagement.lastName'), field: 'LastName', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.userManagement.mobileNo'), field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.userManagement.ëmail'), field: 'Email', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadUser(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsUserSearchGrid.sort = null;
                    } else {
                        paginationOptionsUserSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getUserSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsUserSearchGrid.pageNumber = newPage;
                    paginationOptionsUserSearchGrid.pageSize = pageSize;
                    getUserSearchPage();
                });
            }
        };
        $scope.refresUserSearchGridData = function () {
            getUserSearchPage();
        }

        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        $scope.searchPopupReset = function () {
            $scope.UserSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                email: '',
                mobileNo: ''
            };

        }

        $scope.searchPopup = function () {
            $scope.UserSearchGridSearchCriterias = {
                firstName: '',
                lastName: '',
                email: '',
                mobileNo: ''
            };
            var paginationOptionsUserSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getUserSearchPage();
            UsersearchPopup = ngDialog.open({
                template: 'popUpSearchUser',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };

        $scope.loadUser = function (Id) {
            if (isGuid(Id)) {
                UsersearchPopup.close();
                $scope.formAction = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetUsersById',
                    data: { "Id": Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.User.Id = data.Id;
                    $scope.User.UserName = data.UserName;
                    $scope.User.NationalityId = data.NationalityId;
                    $scope.User.CountryId = data.CountryId;
                    $scope.User.MobileNo = data.MobileNo;
                    $scope.User.OtherTelNo = data.OtherTelNo;
                    $scope.User.InternalExtension = data.InternalExtension;
                    $scope.User.FirstName = data.FirstName;
                    $scope.User.LastName = data.LastName;
                    $scope.User.DateOfBirth = data.DateOfBirth;
                    $scope.User.Email = data.Email;
                    $scope.User.Password = data.Password;
                    $scope.User.Address1 = data.Address1;
                    $scope.User.Address2 = data.Address2;
                    $scope.User.Address3 = data.Address3;
                    $scope.User.Address4 = data.Address4;
                    $scope.User.IDNo = data.IDNo;
                    $scope.User.IDTypeId = data.IDTypeId;
                    $scope.User.DLIssueDate = data.DLIssueDate;
                    $scope.User.ProfilePicture = data.ProfilePicture;
                    $scope.User.Gender = data.Gender;
                    $scope.passwordAgain = data.Password;
                    $scope.User.ProfilePictureByte = "";
                    $scope.User.UserRoleMappings = data.UserRoleMappings;
                    $scope.User.UserBranches = data.UserBranches;
                    $scope.User.LanguageId = data.LanguageId;
                    $scope.User.IsDealerAccount = data.IsDealerAccount;
                    $scope.showProfilePicture();
                    LoadRoles();
                    LoadBranches();
                }).error(function (data, status, headers, config) {
                    // clearCustomerControls();
                });
            }
        }

        var getUserSearchPage = function () {

            $scope.UserSearchGridloading = true;
            $scope.UserSearchGridloadAttempted = false;

            var UserSearchGridParam =
            {
                'paginationOptionsUserSearchGrid': paginationOptionsUserSearchGrid,
                'UserSearchGridSearchCriterias': $scope.UserSearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetAllUserForSearchGrid',
                data: UserSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);
                $scope.gridOptionsUser.data = response_arr.data;
                $scope.gridOptionsUser.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.UserSearchGridloadAttempted = true;
                $scope.UserSearchGridloading = false;

            });
        }
        //$scope.gridOptionsUser.onRegisterApi = function (gridApi) {
        //    $scope.gridApi = gridApi;
        //    gridApi.selection.on.rowSelectionChanged($scope, SetUserValues);
        //}
        //$scope.gridOptionsUser.multiSelect = false;

        function clearUserControls() {
            $scope.User.Id = "00000000-0000-0000-0000-000000000000";
            $scope.User.UserName = "";
            $scope.User.NationalityId = "";
            $scope.User.CountryId = "";
            $scope.User.MobileNo = "";
            $scope.User.OtherTelNo = "";
            $scope.User.InternalExtension = "";
            $scope.User.FirstName = "";
            $scope.User.LastName = "";
            $scope.User.DateOfBirth = "";
            $scope.User.Email = "";
            $scope.User.Password = "";
            $scope.User.Address1 = "";
            $scope.User.Address2 = "";
            $scope.User.Address3 = "";
            $scope.User.Address4 = "";
            $scope.User.IDNo = "";
            $scope.User.IDTypeId = "";
            $scope.User.DLIssueDate = "";
            $scope.User.ProfilePicture = "";
            $scope.User.Gender = "M";
            $scope.User.RoleId = "00000000-0000-0000-0000-000000000000";
            $scope.User.LanguageId = null;
            $scope.User.IsDealerAccount = false;
            $scope.passwordAgain = "";
            $scope.savebuttonName = "CREATE ACCOUNT";
            $scope.RoleSelectedList = [];
            $scope.RoleSelectedDList = [];
            $scope.BranchesSelectedList = [];
            $scope.BranchesSelectedDList = [];
            $scope.formAction = true;//true for add new
        };

        $scope.resetAll = function () {
            $scope.User.Id = "00000000-0000-0000-0000-000000000000";
            $scope.User.UserName = "";
            $scope.User.NationalityId = "";
            $scope.User.CountryId = "";
            $scope.User.MobileNo = "";
            $scope.User.OtherTelNo = "";
            $scope.User.InternalExtension = "";
            $scope.User.FirstName = "";
            $scope.User.LastName = "";
            $scope.User.DateOfBirth = "";
            $scope.User.Email = "";
            $scope.User.Password = "";
            $scope.User.Address1 = "";
            $scope.User.Address2 = "";
            $scope.User.Address3 = "";
            $scope.User.Address4 = "";
            $scope.User.IDNo = "";
            $scope.User.IDTypeId = "";
            $scope.User.DLIssueDate = "";
            $scope.User.ProfilePicture = "";
            $scope.User.Gender = "M";
            $scope.User.RoleId = "00000000-0000-0000-0000-000000000000";
            $scope.User.LanguageId = null;
            $scope.passwordAgain = "";
            $scope.savebuttonName = "CREATE ACCOUNT";
            $scope.RoleSelectedList = [];
            $scope.RoleSelectedDList = [];
            $scope.BranchesSelectedList = [];
            $scope.BranchesSelectedDList = [];
            $scope.formAction = true;//true for add new
            $scope.errorTab1 = "";
            $scope.User.IsDealerAccount = false;
        }
        function SetUserValues() {
            $scope.errorTab1 = "";
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            angular.forEach($scope.mySelectedRows, function (value, key) {
                if (value.Id != null) {
                    $scope.User.Id = value.Id;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/GetUsersById',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { "Id": value.Id }
                    }).success(function (data, status, headers, config) {
                        $scope.User.Id = data.Id;
                        $scope.User.UserName = data.UserName;
                        $scope.User.NationalityId = data.NationalityId;
                        $scope.User.CountryId = data.CountryId;
                        $scope.User.MobileNo = data.MobileNo;
                        $scope.User.OtherTelNo = data.OtherTelNo;
                        $scope.User.InternalExtension = data.InternalExtension;
                        $scope.User.FirstName = data.FirstName;
                        $scope.User.LastName = data.LastName;
                        $scope.User.DateOfBirth = data.DateOfBirth;
                        $scope.User.Email = data.Email;
                        $scope.User.Password = data.Password;
                        $scope.User.Address1 = data.Address1;
                        $scope.User.Address2 = data.Address2;
                        $scope.User.Address3 = data.Address3;
                        $scope.User.Address4 = data.Address4;
                        $scope.User.IDNo = data.IDNo;
                        $scope.User.IDTypeId = data.IDTypeId;
                        $scope.User.DLIssueDate = data.DLIssueDate;
                        $scope.User.ProfilePicture = data.ProfilePicture;
                        $scope.User.Gender = data.Gender;
                        $scope.passwordAgain = data.Password;
                        $scope.User.ProfilePictureByte = "";
                        $scope.User.UserRoleMappings = data.UserRoleMappings;
                        $scope.User.UserBranches = data.UserBranches;
                        $scope.User.LanguageId = data.LanguageId;
                        $scope.showProfilePicture();

                        LoadRoles();
                        LoadBranches();

                        $scope.savebuttonName = "UPDATE ACCOUNT";
                    }).error(function (data, status, headers, config) {
                        clearUserControls();
                    });
                }
                else {
                    clearUserControls();
                }
            });
        }

        //.................................. End of Search ..............................................//

        $scope.checkUserNameAvailability = function () {

            if ($scope.User.UserName == "" || $scope.User.UserName == null) {
                $scope.validation.userName = false;
            } else {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetUserByUserName',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "UserName": $scope.User.UserName }
                }).success(function (data, status, headers, config) {
                    //if (data!=null && data.IsUserExists == true) {
                    //    $scope.validation.userName = false;
                    //} else {
                    //    $scope.validation.userName = true;

                    $scope.validation.userName = false;
                }).error(function (data, status, headers, config) {
                });
            }
        };
        $scope.checkPasswordMatch = function () {

            if ($scope.User.Password != null && $scope.passwordAgain != null) {
                if ($scope.User.Password == $scope.passwordAgain) {
                    $scope.validation.password = false;
                } else {
                    $scope.validation.password = true;
                }
            }
        };
        $scope.SearchUser = function () {
            $scope.myUserSearch = [];
            angular.forEach($scope.Users, function (value, key) {
                $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
            });
            $scope.tempList = [];
            if ($rootScope.searchUser.firstName != "") {
                angular.copy($scope.myUserSearch, $scope.tempList);
                $scope.myUserSearch = [];
                angular.forEach($scope.tempList, function (value, key) {
                    if ($rootScope.searchUser.firstName.toUpperCase() == value.FirstName.toUpperCase()) {
                        $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
                    }
                });
            }
            if ($rootScope.searchUser.lastName != "") {
                angular.copy($scope.myUserSearch, $scope.tempList);
                $scope.myUserSearch = [];
                angular.forEach($scope.tempList, function (value, key) {
                    if ($rootScope.searchUser.lastName.toUpperCase() == value.LastName.toUpperCase()) {
                        $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
                    }
                });
            }
            if ($rootScope.searchUser.email != "") {
                angular.copy($scope.myUserSearch, $scope.tempList);
                $scope.myUserSearch = [];
                angular.forEach($scope.tempList, function (value, key) {
                    if ($rootScope.searchUser.email.toUpperCase() == value.Email.toUpperCase()) {
                        $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
                    }
                });
            }
            if ($rootScope.searchUser.mobileNo != "") {
                angular.copy($scope.myUserSearch, $scope.tempList);
                $scope.myUserSearch = [];
                angular.forEach($scope.tempList, function (value, key) {
                    if ($rootScope.searchUser.mobileNo.toUpperCase() == value.MobileNo.toUpperCase()) {
                        $scope.myUserSearch.push({ Id: value.Id, FirstName: value.FirstName, LastName: value.LastName, MobileNo: value.MobileNo, Email: value.Email })
                    }
                });
            }
        }


        var PhoneCode = "";
        $scope.SetPhoneCode = function () {
            angular.forEach($scope.countries, function (valueC, key) {
                if ($scope.User.CountryId == valueC.Id) {
                    PhoneCode = valueC.PhoneCode;
                    if ($scope.User.MobileNo == "")
                        $scope.User.MobileNo = PhoneCode;
                }
            });
        }

        $scope.validateUser = function () {

            var isValid = true;

            if ($scope.User.Email == "" || $scope.User.Email == undefined) {
                $scope.validate_Email = "has-error";
                isValid = false;
            } else {
                $scope.validate_Email = "";
            }
            if ($scope.User.FirstName == "" || $scope.User.FirstName == undefined) {
                $scope.validate_FirstName = "has-error";
                isValid = false;
            } else {
                $scope.validate_FirstName = "";
            }
            if ($scope.User.NationalityId == "" || $scope.User.NationalityId == undefined) {
                $scope.validate_NationalityId = "has-error";
                isValid = false;
            } else {
                $scope.validate_NationalityId = "";
            }
            if ($scope.User.CountryId == "" || $scope.User.CountryId == undefined) {
                $scope.validate_CountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryId = "";
            }
            if ($scope.User.MobileNo == "" || $scope.User.MobileNo == undefined) {
                $scope.validate_MobileNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_MobileNo = "";
            }
            if ($scope.User.UserName == "") {
                $scope.validate_UserName = "has-error";
                isValid = false;
            } else {
                $scope.validate_UserName = "";
            }
            if ($scope.User.Password == "" || $scope.User.Password == undefined) {
                $scope.validate_Password = "has-error";
                isValid = false;
            } else {
                $scope.validate_Password = "";
            }
            if ($scope.passwordAgain == "" || $scope.passwordAgain == undefined) {
                $scope.validate_passwordAgain = "has-error";
                isValid = false;
            } else {
                $scope.validate_passwordAgain = "";
            }

            if ($scope.User.LanguageId == undefined || $scope.User.LanguageId == null || $scope.User.LanguageId == "") {
                $scope.validate_LanguageId = "has-error";
                isValid = false;
            } else {
                $scope.validate_LanguageId = "";
            }

            return isValid;
        }

        $scope.UserSubmit = function () {
            var phoneno = /^\d{10,15}$/;
            $scope.User.IDTypeId = 0;
            if ($scope.validateUser()) {
                $scope.errorTab1 = "";
                if (!$scope.validateCharacter($scope.User.FirstName)) {
                    customErrorMessage($filter('translate')('pages.userManagement.specialcharacters'))
                    //$scope.errorTab1 = "";
                    return false;
                }

                if ($scope.User.Password.length <= 5) {
                    customErrorMessage($filter('translate')('pages.userManagement.minimumsixCharacters'))
                    //$scope.errorTab1 = "Minimum 6 Characters for Password";
                    return false;
                }

                //if (!$scope.User.MobileNo.match(phoneno) ||
                //   !$scope.User.OtherTelNo.match(phoneno)
                //   ) {
                //    $scope.errorTab2 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
                //    return false;
                //}
                if (uploader1.queue.length > 0) {
                    uploader1.queue[0].upload();
                }
                $scope.errorTab1 = "";
                // alert($scope.User.Id);
                if ($scope.User.Id == null || $scope.User.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.UserSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.UserSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/AddUser',
                        data: $scope.User,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.UserSaveBtnIconClass = "";
                        $scope.UserSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.userManagement.userInformation'),
                                text: $filter('translate')('pages.userManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/User/GetUsers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Users = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearUserControls();
                        } else {
                            var msg = $filter('translate')('pages.userManagement.erroroccured');

                            if (data.hasOwnProperty("AlreadyExistEmail") && data.AlreadyExistEmail) {
                                msg = $filter('translate')('pages.userManagement.erroroccuredAlreadyExistEmail');
                            }
                            if (data.hasOwnProperty("AlreadyExistUsername") && data.AlreadyExistUsername) {
                                msg = $filter('translate')('pages.userManagement.erroroccuredAlreadyExistUsername');
                            }

                            SweetAlert.swal({
                                title: $filter('translate')('pages.userManagement.userInformation'),
                                text: msg,
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            return false;
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.userManagement.userInformation'),
                            text: $filter('translate')('pages.userManagement.erroroccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.UserSaveBtnIconClass = "";
                        $scope.UserSaveBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.UserSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.UserSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/User/UpdateUser',
                        data: $scope.User,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.UserSaveBtnIconClass = "";
                        $scope.UserSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.userManagement.userInformation'),
                                text: $filter('translate')('pages.userManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/User/GetUsers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Users = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearUserControls();
                        }
                        else {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.userManagement.userInformation'),
                                text: $filter('translate')('pages.userManagement.erroroccured'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            return false;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.userManagement.userInformation'),
                            text: $filter('translate')('pages.userManagement.erroroccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.userManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.UserSaveBtnIconClass = "";
                        $scope.UserSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('pages.userManagement.fillvalidfeild'),)
            }
        }
        var today = new Date();
        $scope.MaxDate = new Date(today.getFullYear() - 18, today.getMonth(), today.getDate());
        $scope.showProfilePicture = function () {
            if ($scope.User.ProfilePicture != null && $scope.User.ProfilePicture != "00000000-0000-0000-0000-000000000000") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Image/GetImageById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { 'ImageId': $scope.User.ProfilePicture },
                }).success(function (data, status, headers, config) {
                    $scope.User.ProfilePictureByte = data;
                }).error(function (data, status, headers, config) {
                });
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
                    alert($filter('translate')('pages.userManagement.invalidfileformat'));
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
                    alert($filter('translate')('pages.userManagement.selectedfileexceeds'));
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
                    alert($filter('translate')('pages.userManagement.exceededthelimit'));
                    return false;
                }
            }
        });
        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader1.onAfterAddingFile = function (fileItem) {
            // alert('Files ready for upload.');
        };
        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            $scope.uploader1.queue = [];
            $scope.uploader1.progress = 0;
            $scope.User.ProfilePicture = response.replace(/\"/g, "");
        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            alert($filter('translate')('pages.userManagement.uploadyourfile'));
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
    });



