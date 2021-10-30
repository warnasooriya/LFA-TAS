app.controller('RoleManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Role Management";
        $scope.UserRoleSubmitBtnIconClass = "";
        $scope.UserRoleSubmitBtnDisabled = false;
        $scope.RoleMenuSubmitBtnIconClass = "";
        $scope.RoleMenuSubmitBtnDisabled = false;
        $scope.DashboardSubmitBtnIconClass = "";
        $scope.DashboardSubmitBtnDisabled = false;
        $scope.gridOptionsRoleGridloading = false;
        $scope.RoleGridloadAttempted = false;
        $scope.gridOptionsDashboardGridloading = false;
        $scope.DashBoardGridloadAttempted = false;
        $scope.UserRole = {
            RoleId: "00000000-0000-0000-0000-000000000000",
            RoleCode: '',
            RoleName: '',
            IsGoodWillAuthorized: false,
            IsClaimAuthorized:false
        };
        $scope.RoleMenu = {
            RoleId: "00000000-0000-0000-0000-000000000000",
            MenuId: "00000000-0000-0000-0000-000000000000",
            Description: ''
        };

        $scope.RoleDashboard = {
            RoleId: "00000000-0000-0000-0000-000000000000",
            MenuId: "00000000-0000-0000-0000-000000000000",
            Description: ''
        };
        $scope.RoleMenus = [];
        $scope.CheckedLevels = [];
        $scope.LevelList = [];
        $scope.LevelSelectedList = [];
        $scope.LevelSelectedDList = [];
        $scope.settings = {
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: false,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false
        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('pages.roleManagement.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('pages.roleManagement.itemSelected')
        };

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.roleManagement.error'), msg);
        };

        function AddLevels() {
            var index = 0;
            $scope.LevelList = [];
            angular.forEach($scope.Levels, function (value) {
                var x = { id: index, code: value.Id, label: value.LevelName };
                $scope.LevelList.push(x);
                index = index + 1;
            });
        }
        function LoadLevels() {
            $scope.LevelSelectedList = [];
            $scope.LevelSelectedDList = [];
            angular.forEach($scope.CheckedLevels, function (valueOut) {
                angular.forEach($scope.LevelList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.LevelSelectedList.push(x);
                        $scope.LevelSelectedDList.push(valueIn.label);
                    }
                });
            });
        }





        $scope.SendLevel = function () {
            $scope.LevelSelectedDList = [];
            $scope.CheckedLevels = [];
            angular.forEach($scope.LevelSelectedList, function (valueOut) {
                angular.forEach($scope.LevelList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.CheckedLevels.push(valueIn.code);
                        $scope.LevelSelectedDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";

        LoadDetails();
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetUserRoles',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.UserRoles = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetAllPriviledgeLevels',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.Levels = data;
                AddLevels();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/User/GetAllMenus',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.Menus = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.SetUserRoleValues = function () {
            $scope.errorTab1 = "";
            if ($scope.UserRole.RoleId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetUserRoleById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.UserRole.RoleId }
                }).success(function (data, status, headers, config) {
                    $scope.UserRole.RoleCode = data.RoleCode;
                    $scope.UserRole.RoleName = data.RoleName;
                    $scope.UserRole.RoleId = data.RoleId;
                    $scope.UserRole.IsGoodWillAuthorized = data.IsGoodWillAuthorized;
                    $scope.UserRole.IsClaimAuthorized = data.IsClaimAuthorized;

                }).error(function (data, status, headers, config) {
                    clearUserRoleControls();
                });
            }
            else {
                clearUserRoleControls();
            }
        }
        function clearUserRoleControls() {
            $scope.UserRole.RoleId = "00000000-0000-0000-0000-000000000000";
            $scope.UserRole.RoleName = "";
            $scope.UserRole.RoleCode = "";
            $scope.UserRole.IsGoodWillAuthorized =false;
            $scope.UserRole.IsClaimAuthorized = false;
        }

        function CheckUserRole() {
            var ret = true;
            angular.forEach($scope.UserRoles, function (value) {
                if ((value.RoleName == $scope.UserRole.RoleName || value.RoleCode == $scope.UserRole.RoleCode) && $scope.UserRole.Id != value.Id) {
                    ret= false;
                }
            });
            if(!ret)
            {
                SweetAlert.swal({
                    title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                    text: $filter('translate')('pages.roleManagement.userRoleAlreadyCreated'),
                    type: "warning",
                    confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                    confirmButtonColor: "rgb(221, 107, 85)"
                });
            }
            return ret;
        }

        $scope.validateRaole = function () {
            var isValid = true;

            if ($scope.UserRole.RoleCode == "" || $scope.UserRole.RoleCode == undefined) {
                $scope.validate_RoleCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_RoleCode = "";
            }

            if ($scope.UserRole.RoleName == "" || $scope.UserRole.RoleName == undefined) {
                $scope.validate_RoleName = "has-error";
                isValid = false;
            } else {
                $scope.validate_RoleName = "";
            }

            return isValid;
        }

        $scope.UserRoleSubmit = function () {
            var exists = false
            if ($scope.validateRaole()) {
                $scope.errorTab1 = "";
                if (!CheckUserRole()) {
                    return false;
                }
                if ($scope.UserRole.RoleId == null || $scope.UserRole.RoleId == "00000000-0000-0000-0000-000000000000") {
                    angular.forEach($scope.UserRoles, function (value) {
                        if (value.RoleCode == $scope.UserRole.RoleCode) {
                            exists = true;
                        }
                    });
                    if (exists) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                            text: $filter('translate')('pages.roleManagement.roleAlreadyAdded'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else {
                        $scope.UserRoleSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.UserRoleSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/User/AddUserRole',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: $scope.UserRole
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.UserRoleSubmitBtnIconClass = "";
                            $scope.UserRoleSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                                    text: $filter('translate')('pages.roleManagement.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/User/GetUserRoles',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.UserRoles = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearUserRoleControls();
                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                                text: $filter('translate')('pages.roleManagement.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.UserRoleSubmitBtnIconClass = "";
                            $scope.UserRoleSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
                else {
                    angular.forEach($scope.UserRoles, function (value) {
                        if (value.RoleCode == $scope.UserRole.RoleCode && value.RoleId != $scope.UserRole.RoleId) {
                            exists = true;
                        }
                    });
                    if (exists) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                            text: $filter('translate')('pages.roleManagement.roleAlreadyAdded'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else {
                        $scope.UserRoleSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.UserRoleSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/User/UpdateUserRole',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: $scope.UserRole
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.UserRoleSubmitBtnIconClass = "";
                            $scope.UserRoleSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                                    text: $filter('translate')('pages.roleManagement.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/User/GetUserRoles',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.UserRoles = data;
                                }).error(function (data, status, headers, config) {
                                });

                                clearUserRoleControls();

                            } else {;
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.roleManagement.userRoleInformation'),
                                text: $filter('translate')('pages.roleManagement.erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            return false;
                            $scope.UserRoleSubmitBtnIconClass = "";
                            $scope.UserRoleSubmitBtnDisabled = false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.roleManagement.fillvalidfeild'))
            }
        }

        $scope.LoadLevels = function()
        {
            if($scope.RoleMenu.MenuId !="00000000-0000-0000-0000-000000000000" && $scope.RoleMenu.RoleId !="00000000-0000-0000-0000-000000000000")
            {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetRoleMenuLevelsById',
                    data: { "MenuId": $scope.RoleMenu.MenuId, "RoleId": $scope.RoleMenu.RoleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    $scope.CheckedLevels = data;
                    LoadLevels();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        function clearRoleMenuControls() {
            $scope.RoleMenu = {
                RoleId: "00000000-0000-0000-0000-000000000000",
                MenuId: "00000000-0000-0000-0000-000000000000",
                Description: ''
            };
            $scope.CheckedLevels = [];
            $scope.LevelSelectedList = [];
            $scope.LevelSelectedDList = [];
            $scope.myUserSearch = [];

        }


        $scope.validateRoleMenu = function () {
            var isValid = true;

            if ($scope.RoleMenu.RoleId == "" || $scope.RoleMenu.RoleId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_RoleMenuRoleId = "has-error";
                isValid = false;
            } else {
                $scope.validate_RoleMenuRoleId = "";
            }

            return isValid;
        }

        $scope.RoleMenuSubmit = function () {

            if ($scope.validateRoleMenu()) {
                var jObject = JSON.stringify($scope.myUserSearch);
                $scope.RoleMenuSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.RoleMenuSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/AddOrUpdateRoleMenuMapping',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { JObject: $scope.myUserSearch }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.RoleMenuSubmitBtnIconClass = "";
                    $scope.RoleMenuSubmitBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.roleMapping'),
                            text: $filter('translate')('pages.roleManagement.successfullySaved'),
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        $scope.RoleMenu.RoleId = '';
                        $scope.myUserSearch = [];

                        //$scope.LoadGridMenuLevels();
                        //clearRoleMenuControls();
                    } else {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.roleMapping'),
                            text: $filter('translate')('pages.roleManagement.saveFailed'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.roleManagement.roleMapping'),
                        text: $filter('translate')('pages.roleManagement.erroroccuredsavingdata'),
                        type: "warning",
                        confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.RoleMenuSubmitBtnIconClass = "";
                    $scope.RoleMenuSubmitBtnDisabled = false;
                    return false;
                });

            } else {
                customErrorMessage($filter('translate')('pages.roleManagement.fillvalidfeild'))
            }




            //$scope.RoleMenus = [];
            //angular.forEach($scope.CheckedLevels, function (value) {
            //    var item = {
            //        Id:"00000000-0000-0000-0000-000000000000",
            //        RoleId: $scope.RoleMenu.RoleId,
            //        MenuId: $scope.RoleMenu.MenuId,
            //        Description: $scope.RoleMenu.Description,
            //        LevelId:value
            //    };
            //    $scope.RoleMenus.push(item);
            //});

            //if ($scope.CheckedLevels.length > 0 && $scope.RoleMenu.MenuId != "00000000-0000-0000-0000-000000000000"
            //    && $scope.RoleMenu.RoleId != "00000000-0000-0000-0000-000000000000") {
            //    $scope.errorTab1 = "";
            //    var temp = {
            //        RoleMenus: $scope.RoleMenus
            //    };
            //    $http({
            //        method: 'POST',
            //        url: '/TAS.Web/api/User/AddOrUpdateRoleMenuMapping',
            //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
            //        data: temp
            //    }).success(function (data, status, headers, config) {
            //        $scope.Ok = data;
            //        if (data == "OK") {
            //            SweetAlert.swal({
            //                title: "Role Menu Mapping Information",
            //                text: "Successfully Saved!",
            //                confirmButtonColor: "#007AFF"
            //            });
            //            clearRoleMenuControls();
            //        }
            //        return false;
            //    }).error(function (data, status, headers, config) {
            //        SweetAlert.swal({
            //            title: "Role Menu Mapping Information",
            //            text: "Error occured while saving data!",
            //            type: "warning",
            //            confirmButtonColor: "rgb(221, 107, 85)"
            //        });
            //        return false;
            //    });
            //}
            //else {
            //    $scope.errorTab2 = "Please Enter ";
            //    if ($scope.RoleMenu.MenuId == "" || $scope.RoleMenu.MenuId == "00000000-0000-0000-0000-000000000000") {
            //        $scope.errorTab2 = $scope.errorTab2 + "Menu, ";
            //    }
            //    if ($scope.RoleMenu.RoleId == "" || $scope.RoleMenu.RoleId == "00000000-0000-0000-0000-000000000000") {
            //        $scope.errorTab2 = $scope.errorTab2 + "Role, ";
            //    }
            //    $scope.errorTab2 = $scope.errorTab2.substring(0, $scope.errorTab2.length - 1);
            //    return false;
            //}
        }


        $scope.myUserSearch = [];

        $scope.LoadGridMenuLevels = function () {
            $scope.gridOptionsRoleGridloading = true;
            $scope.RoleGridloadAttempted = false;
            if ($scope.RoleMenu.RoleId != '' && $scope.RoleMenu.RoleId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetRoleMenuLevelsByRoleId',
                    data: { "RoleId": $scope.RoleMenu.RoleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.myUserSearch = data;
                    $scope.gridOptionsRoleGridloading = false;
                    $scope.RoleGridloadAttempted = true;
                }).error(function (data, status, headers, config) {
                    });

            } else {
                $scope.myUserSearch = [];
                $scope.gridOptionsRoleGridloading = false;
                $scope.RoleGridloadAttempted = false;
            }
        }


        //$scope.myUserSearch = [{
        //    MenuId: "984235iuyh987234890uw12",
        //    Menu: "aoroni",
        //    Read: false,
        //    Create: true,
        //    Update: true,
        //    Delete: false,
        //    Comment: "asdfsdfasdfasdfsadfsf"
        //}, {
        //    MenuId: "984235iuyh987234890uw12",
        //    Menu: "aoroni",
        //    Read: true,
        //    Create: true,
        //    Update: false,
        //    Delete: false,
        //    Comment: "asdfsdfasdfasdfsadfsf"
        //}, {
        //    MenuId: "984235iuyh987234890uw12",
        //    Menu: "aoroni",
        //    Read: true,
        //    Create: false,
        //    Update: true,
        //    Delete: true,
        //    Comment: "asdfsdfasdfasdfsadfsf"
        //}];


        $scope.gridOptions = {
            data: 'myUserSearch',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 10,
            enableRowSelection: true,
            primaryKey: "Email",
            enableCellSelection: false,
            columnDefs: [


            {
                field: "MenuId",
                displayName: "MenuId",
                visible: false
            }, {
                field: "Menu",
                displayName: $filter('translate')('pages.roleManagement.menu'),
                width: 500
            }, {
                field: $filter('translate')('pages.roleManagement.read'),
                displayname: "Check Box",
                cellTemplate: '<input type="checkbox" ng-model="row.entity.Read" ng-click="toggle(row.entity.name,row.entity.Read)">',
                width: 100
            }, {
                field: $filter('translate')('pages.roleManagement.create'),
                displayname: "Check Box",
                cellTemplate: '<input type="checkbox" ng-model="row.entity.Create" ng-click="toggle(row.entity.name,row.entity.Create)">',
                width: 100
            }, {
                field: $filter('translate')('pages.roleManagement.update'),
                displayname: "Check Box",
                cellTemplate: '<input type="checkbox" ng-model="row.entity.Update" ng-click="toggle(row.entity.name,row.entity.Update)">',
                width: 100
            }, {
                field: $filter('translate')('pages.roleManagement.delete'),
                displayname: "Check Box",
                cellTemplate: '<input type="checkbox" ng-model="row.entity.Delete" ng-click="toggle(row.entity.name,row.entity.Delete)">',
                width: 100
            }
            //, {
            //    field: "Comment",
            //    displayName: "Comment",
            //    width: 400,
            //    cell
            //}

            ],
        };

        //$scope.toggle = function (name, value) {
        //    //do something usefull here, you have the name and the new value of dude. Write it to a db or else.
        //    alert(name + ':' + value);
        //}

        //-- DASHBOARD MAPPING -----------------

        $scope.myDashBoardSearch = [];

        $scope.LoadDashBoardGridMenuLevels = function () {
            $scope.gridOptionsDashboardGridloading = true;
            $scope.DashBoardGridloadAttempted = false;
            if ($scope.RoleDashboard.RoleId != '' && $scope.RoleDashboard.RoleId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/GetDashBoardMenuLevelsByRoleId',
                    data: { "RoleId": $scope.RoleDashboard.RoleId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.myDashBoardSearch = data;
                    $scope.gridOptionsDashboardGridloading = false;
                    $scope.DashBoardGridloadAttempted = true;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.myDashBoardSearch = [];
                $scope.gridOptionsDashboardGridloading = false;
                $scope.DashBoardGridloadAttempted = false;
            }
        }

        $scope.dashBoardGridOptions = {
            data: 'myDashBoardSearch',
            paginationPageSizes: [5, 10, 20],
            paginationPageSize: 10,
            //enableRowSelection: false,
            //primaryKey: "Email",
            //enableCellSelection: false,
            columnDefs: [


                {
                    field: "DashboardChartId",
                    displayName: "DashboardChartId",
                    visible: false
                }, {
                    field: "Section",
                    displayName: $filter('translate')('pages.roleManagement.section'),
                    width: 250
                }, {
                    field: "ChartDisplayName",
                    displayName: $filter('translate')('pages.roleManagement.chartName'),
                    width: 400
                }, {
                    field: $filter('translate')('pages.roleManagement.allowed'),
                    displayname: "Check Box",
                    cellTemplate: '<input type="checkbox" ng-model="row.entity.IsAllowed" ng-click="toggle(row.entity.name,row.entity.IsAllowed)">',
                    width: 100
                }, {
                    field: $filter('translate')('pages.roleManagement.allBranches'),
                    displayname: "Check Box",
                    cellTemplate: '<input type="checkbox" ng-model="row.entity.IsAllBranches" ng-click="toggle(row.entity.name,row.entity.IsAllBranches)">',
                    width: 100
                }


            ],
        };

        $scope.validateRoleDashBoard = function () {
            var isValid = true;

            if ($scope.RoleDashboard.RoleId == "" || $scope.RoleDashboard.RoleId == "00000000-0000-0000-0000-000000000000" ||
                $scope.RoleDashboard.RoleId == null) {
                $scope.validate_RoleDashboardRoleId = "has-error";
                isValid = false;
            } else {
                $scope.validate_RoleDashboardRoleId = "";
            }

            return isValid;
        }

        $scope.DashBoardChartSubmit = function () {

            if ($scope.validateRoleDashBoard()) {
                var jObject = JSON.stringify($scope.myDashBoardSearch);
                $scope.DashboardSubmitBtnIconClass = "fa fa-spinner fa-spin";
                $scope.DashboardSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/User/AddOrUpdateDashBoardChartMapping',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { JObject: $scope.myDashBoardSearch }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.DashboardSubmitBtnIconClass = "";
                    $scope.DashboardSubmitBtnDisabled = false;
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.roleDashbordMapping'),
                            text: $filter('translate')('pages.roleManagement.successfullySaved'),
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            confirmButtonColor: "#007AFF"
                        });
                        $scope.RoleDashboard.RoleId = '';
                        $scope.myDashBoardSearch = [];
                     //   $scope.LoadDashBoardGridMenuLevels();

                    } else {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.roleManagement.roleDashbordMapping'),
                            text: $filter('translate')('pages.roleManagement.saveFailed'),
                            confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: $filter('translate')('pages.roleManagement.roleDashbordMapping'),
                        text: $filter('translate')('pages.roleManagement.erroroccuredsavingdata'),
                        confirmButtonText: $filter('translate')('pages.roleManagement.ok'),
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.DashboardSubmitBtnIconClass = "";
                    $scope.DashboardSubmitBtnDisabled = false;
                    return false;
                });

            } else {
                customErrorMessage($filter('translate')('pages.roleManagement.fillvalidfeild'))
            }


        }


        //$scope.gridOptions = {};

        //$scope.gridOptions.columnDefs = [
        //  { name: 'id', enableCellEdit: false },
        //  { name: 'name', displayName: 'Name (editable)' },
        //  { name: 'gender' },
        //  { name: 'age', displayName: 'Age', type: 'number' },
        //  { name: 'registered', displayName: 'Registered', type: 'date', cellFilter: 'date:"yyyy-MM-dd"' },
        //  { name: 'address', displayName: 'Address', type: 'object', cellFilter: 'address' },
        //  {
        //      name: 'address.city', displayName: 'Address (even rows editable)',
        //      cellEditableCondition: function ($scope) {
        //          return $scope.rowRenderIndex % 2
        //      }
        //  },
        //  { name: 'isActive', displayName: 'Active', type: 'boolean', cellTemplate: '<input type="checkbox" ng-model="row.entity.isActive">' }
        //];

        //$scope.saveRow = function (rowEntity) {
        //    // create a fake promise - normally you'd use the promise returned by $http or $resource
        //    var promise = $q.defer();
        //    $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise.promise);

        //    // fake a delay of 3 seconds whilst the save occurs, return error if gender is "male"
        //    $interval(function () {
        //        if (rowEntity.gender === 'male') {
        //            promise.reject();
        //        } else {
        //            promise.resolve();
        //        }
        //    }, 3000, 1);
        //};

        //$scope.gridOptions.onRegisterApi = function (gridApi) {
        //    //set gridApi on scope
        //    $scope.gridApi = gridApi;
        //    gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
        //};

        //$http.get('https://cdn.rawgit.com/angular-ui/ui-grid.info/gh-pages/data/500_complex.json')
        //.success(function (data) {
        //    for (i = 0; i < data.length; i++) {
        //        data[i].registered = new Date(data[i].registered);
        //    }
        //    $scope.gridOptions.data = data;
        //});
    });
