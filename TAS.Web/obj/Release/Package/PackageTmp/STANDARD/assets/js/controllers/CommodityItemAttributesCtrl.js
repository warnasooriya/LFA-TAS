app.controller('CommodityItemAttributesCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Commodity Item Attributes";
        $scope.ModalDescription = "Add Edit Commodity Item Attributes";
        $scope.ItemStatusSubmitBtnIconClass = "";
        $scope.ItemStatusSubmitBtnDisabled = false;
        $scope.CommodityCategorySubmitBtnIconClass = "";
        $scope.CommodityCategorySubmitBtnDisabled = false;
        $scope.CommodityUsageTypeSubmitBtnIconClass = "";
        $scope.CommodityUsageTypeSubmitBtnDisabled = false;
        $scope.IsExistsVarStatus = false;
        $scope.isItemStatusExisting = false;
        $scope.IsCommodityCategoryExist = false;
        $scope.IsCommodityUsageexist = false;

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.commodityItemAttributes.Error'), msg);
        };

        $scope.loadInitailData = function () { }
        $scope.errorTab1 = "";
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityUsageTypes = data;
            }).error(function (data, status, headers, config) {
            });
        }
        LoadDetails();

        // Item Status ////////////////////////////
        $scope.ItemStatus = {
            Id: "00000000-0000-0000-0000-000000000000",
            Status: '',
            ItemStatusDescription: ''
        };
        $scope.SetItemStatusValues = function () {
            $scope.errorTab1 = "";
            if ($scope.ItemStatus.Id == null || $scope.ItemStatus.Id == "00000000-0000-0000-0000-000000000000") {
                $scope.isItemStatusExisting = false;
            }
            else {

                $scope.isItemStatusExisting = true;
            }
            if ($scope.ItemStatus.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetItemStatusById',
                    data: { "Id": $scope.ItemStatus.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.ItemStatus.Id = data.Id;
                    $scope.ItemStatus.Status = data.Status;
                    $scope.ItemStatus.ItemStatusDescription = data.ItemStatusDescription;

                }).error(function (data, status, headers, config) {
                    clearItemStatusControls();
                });
            }
            else {
                clearItemStatusControls();
            }
        }
        function clearItemStatusControls() {
            $scope.ItemStatus.Status = "";
            $scope.ItemStatus.ItemStatusDescription = "";
            $scope.ItemStatus.Id = "00000000-0000-0000-0000-000000000000";
        }
        $scope.ItemStatusSubmit = function () {
            if ($scope.validateItemStaus()) {
                if ($scope.isValidItemStaust()) {
                    if ($scope.ItemStatus.Status != "" && $scope.ItemStatus.ItemStatusDescription != "") {
                        $scope.errorTab1 = "";
                        if ($scope.ItemStatus.Id == null || $scope.ItemStatus.Id == "00000000-0000-0000-0000-000000000000") {
                            $scope.ItemStatusSubmitBtnIconClass = "fa fa-spinner fa-spin";
                            $scope.ItemStatusSubmitBtnDisabled = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CommodityItemAttributes/AddItemStatus',
                                data: $scope.ItemStatus,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.ItemStatusSubmitBtnIconClass = "";
                                $scope.ItemStatusSubmitBtnDisabled = false;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.commodityItemAttributes.itemStatusInformation'),
                                        text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                        confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                        confirmButtonColor: "#007AFF"
                                    });
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.ItemStatuss = data;
                                    }).error(function (data, status, headers, config) {
                                    });
                                    clearItemStatusControls();
                                } else {
                                    alert(data);
                                }

                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.itemStatusInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                $scope.ItemStatusSubmitBtnIconClass = "";
                                $scope.ItemStatusSubmitBtnDisabled = false;
                                return false;
                            });

                        }
                        else {
                            $scope.ItemStatusSubmitBtnIconClass = "fa fa-spinner fa-spin";
                            $scope.ItemStatusSubmitBtnDisabled = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CommodityItemAttributes/UpdateItemStatus',
                                data: $scope.ItemStatus,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.ItemStatusSubmitBtnIconClass = "";
                                $scope.ItemStatusSubmitBtnDisabled = false;
                                if (data == "OK") {
                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.commodityItemAttributes.itemStatusInformation'),
                                        text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                        confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                        confirmButtonColor: "#007AFF"
                                    });

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        $scope.ItemStatuss = data;
                                    }).error(function (data, status, headers, config) {
                                    });

                                    clearItemStatusControls();

                                } else {;
                                }

                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.itemStatusInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                    type: "warning",
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                $scope.ItemStatusSubmitBtnIconClass = "";
                                $scope.ItemStatusSubmitBtnDisabled = false;
                                return false;
                            });
                        }

                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.commodityItemAttributes.fillvalidfeild'))
            }
        }

        // Product Category //////////////////////////
        $scope.CommodityCategory = {
            CommodityCategoryId: "00000000-0000-0000-0000-000000000000",
            CommodityCategoryDescription: '',
            CommodityCategoryCode: '',
            Length: 0,
            CommodityTypeId: "00000000-0000-0000-0000-000000000000"//"6823c00e-860b-4869-b811-2a8637c742c8"
        };
        $scope.SetCommodityCategoryValues = function () {
            $scope.errorTab1 = "";
            if ($scope.CommodityCategory.CommodityCategoryId == null || $scope.CommodityCategory.CommodityCategoryId == "00000000-0000-0000-0000-000000000000") {
                $scope.IsCommodityCategoryExist = false;
            }
            else {
                $scope.IsCommodityCategoryExist = true;

            }
            if ($scope.CommodityCategory.CommodityCategoryId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetCommodityCategoryById',
                    data: $scope.CommodityCategory,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CommodityCategory.CommodityCategoryId = data.CommodityCategoryId;
                    $scope.CommodityCategory.CommodityCategoryDescription = data.CommodityCategoryDescription;
                    $scope.CommodityCategory.CommodityCategoryCode = data.CommodityCategoryCode;
                    $scope.CommodityCategory.Length = data.Length;
                }).error(function (data, status, headers, config) {
                    clearCommodityCategoryControls();
                });
            }
            else {
                clearCommodityCategoryControls();
            }
        }
        $scope.LoadFromComodityType = function () {
           
            if ($scope.CommodityCategory.CommodityTypeId != undefined && $scope.CommodityCategory.CommodityTypeId != '' && $scope.CommodityCategory.CommodityTypeId != "00000000-0000-0000-0000-000000000000") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityCategories',
                    data: { "CommodityTypeId": $scope.CommodityCategory.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CommodityCategories = data;
                }).error(function (data, status, headers, config) {
                    clearCommodityCategoryControls();
                });
                clearCommodityCategoryControls();
            } else {
                clearCommodityCategoryControls();
            }
        }
        function clearCommodityCategoryControls() {
            $scope.CommodityCategory.CommodityCategoryDescription = "";
            $scope.CommodityCategory.CommodityCategoryCode = "";
            $scope.CommodityCategory.Length = "";
            $scope.CommodityCategory.CommodityCategoryId = "00000000-0000-0000-0000-000000000000";
           // $scope.CommodityCategory.CommodityTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.CommodityCategories = [];
        }
        $scope.CommodityCategorySubmit = function () {
            if ($scope.validateProductCategory()) {
                if ($scope.isValidCommodityCategory()) {

                    $scope.errorTab1 = "";
                    if ($scope.CommodityCategory.CommodityCategoryId == null || $scope.CommodityCategory.CommodityCategoryId == "00000000-0000-0000-0000-000000000000") {
                        $scope.CommodityCategorySubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CommodityCategorySubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CommodityItemAttributes/AddCommodityCategory',
                            data: $scope.CommodityCategory,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CommodityCategorySubmitBtnIconClass = "";
                            $scope.CommodityCategorySubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.productCategoryInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $scope.LoadFromComodityType();
                                clearCommodityCategoryControls();
                            } else {
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.commodityItemAttributes.productCategoryInformation'),
                                text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CommodityCategorySubmitBtnIconClass = "";
                            $scope.CommodityCategorySubmitBtnDisabled = false;
                            return false;

                        });
                    }
                    else {
                        $scope.CommodityCategorySubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CommodityCategorySubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CommodityItemAttributes/UpdateCommodityCategory',
                            data: $scope.CommodityCategory,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CommodityCategorySubmitBtnIconClass = "";
                            $scope.CommodityCategorySubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.productCategoryInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $scope.LoadFromComodityType();
                                clearCommodityCategoryControls();

                            } else {;
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.commodityItemAttributes.productCategoryInformation'),
                                text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                type: "warning",
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CommodityCategorySubmitBtnIconClass = "";
                            $scope.CommodityCategorySubmitBtnDisabled = false;
                            return false;

                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.commodityItemAttributes.fillvalidfeild'))
            }


        }

        // CommodityUsageType /////////////////////
        $scope.CommodityUsageType = {
            Id: "00000000-0000-0000-0000-000000000000",
            Name: '',
            Description: ''
        };
        $scope.SetCommodityUsageTypeValues = function () {
            $scope.errorTab1 = "";
            if ($scope.CommodityUsageType.Id == null || $scope.CommodityUsageType.Id == "00000000-0000-0000-0000-000000000000") {
                $scope.IsCommodityUsageexist = false;
            }
            else{
                $scope.IsCommodityUsageexist = true;
            }
            if ($scope.CommodityUsageType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetCommodityUsageTypeById',
                    data: { "Id": $scope.CommodityUsageType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CommodityUsageType.Id = data.Id;
                    $scope.CommodityUsageType.Name = data.Name;
                    $scope.CommodityUsageType.Description = data.Description;

                }).error(function (data, status, headers, config) {
                    clearCommodityUsageTypeControls();
                });
            }
            else {
                clearCommodityUsageTypeControls();
            }
        }
        function clearCommodityUsageTypeControls() {
            $scope.CommodityUsageType.Name = "";
            $scope.CommodityUsageType.Description = "";
            $scope.CommodityUsageType.Id = "00000000-0000-0000-0000-000000000000";
        }
        $scope.CommodityUsageTypeSubmit = function () {
            if ($scope.validateUseageType()) {
                if ($scope.isValidUseageType()) {
                    $scope.errorTab1 = "";
                    if ($scope.CommodityUsageType.Id == null || $scope.CommodityUsageType.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.CommodityUsageTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CommodityUsageTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CommodityItemAttributes/AddCommodityUsageType',
                            data: $scope.CommodityUsageType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CommodityUsageTypeSubmitBtnIconClass = "";
                            $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.commodityUsageTypeInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CommodityUsageTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearCommodityUsageTypeControls();
                            } else {
                                alert(data);
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.commodityItemAttributes.commodityUsageTypeInformation'),
                                text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CommodityUsageTypeSubmitBtnIconClass = "";
                            $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.CommodityUsageTypeSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.CommodityUsageTypeSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CommodityItemAttributes/UpdateCommodityUsageType',
                            data: $scope.CommodityUsageType,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.CommodityUsageTypeSubmitBtnIconClass = "";
                            $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.commodityItemAttributes.commodityUsageTypeInformation'),
                                    text: $filter('translate')('pages.commodityItemAttributes.SuccessfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.CommodityUsageTypes = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearCommodityUsageTypeControls();
                            } else {;
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.commodityItemAttributes.commodityUsageTypeInformation'),
                                text: $filter('translate')('pages.commodityItemAttributes.Erroroccuredsavingdata'),
                                confirmButtonText: $filter('translate')('pages.taxManagement.ok'),
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.CommodityUsageTypeSubmitBtnIconClass = "";
                            $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                }
            } else {
                customErrorMessage($filter('translate')('pages.commodityItemAttributes.fillvalidfeild'))
            }
        }

        $scope.IsExsistingItemStatusByStatus = function () {
            $scope.errorTab1 = "";
            $scope.ItemStatusSubmitBtnDisabled = true;
            if ($scope.ItemStatus.Id != null && $scope.ItemStatus.Status != undefined || $scope.ItemStatus.Status.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/CommodityItemAttributes/IsExsistingItemStatusByStatus',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.ItemStatus.Id, "Status": $scope.ItemStatus.Status }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarStatus = data;
                    $scope.ItemStatusSubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.ItemStatusSubmitBtnDisabled = false;
                });
            }
        }


        $scope.validateItemStaus = function ()
        {
            var isValid = true;

            if ($scope.ItemStatus.Status == "") {
                $scope.validate_Status = "has-error";
                isValid = false;
            } else {
                $scope.validate_Status = "";
            }
            if ($scope.ItemStatus.ItemStatusDescription == "") {
                $scope.validate_ItemStatusDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemStatusDescription = "";
            }

            return isValid;
        }

        $scope.isValidItemStaust = function () {

            var isValid = true;

            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarStatus) {
                        customErrorMessage($filter('translate')('pages.commodityItemAttributes.fillvalidfeild'))                  
                        isValid = false;
                    }
                }
            }

            return isValid;

        }


        $scope.IsExsistingCommodityCategoryByDescription = function () {
            $scope.errorTab1 = "";
            $scope.CommodityCategorySubmitBtnDisabled = true;
            if ($scope.CommodityCategory.CommodityCategoryId != null && $scope.CommodityCategory.CommodityCategoryDescription != undefined || $scope.CommodityCategory.CommodityCategoryDescription.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/CommodityItemAttributes/IsExsistingCommodityCategoryByDescription',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "CommodityCategoryId": $scope.CommodityCategory.CommodityCategoryId, "CommodityCategoryDescription": $scope.CommodityCategory.CommodityCategoryDescription }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarDescription = data;
                    $scope.CommodityCategorySubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.CommodityCategorySubmitBtnDisabled = false;
                });
            }
        }

        $scope.validateProductCategory = function () {
            var isValid = true;

            
            if ($scope.CommodityCategory.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CommodityTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityTypeId = "";
            }

            if ($scope.CommodityCategory.CommodityCategoryDescription == "") {
                $scope.validate_CommodityCategoryDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityCategoryDescription = "";
            }

            if ($scope.CommodityCategory.CommodityCategoryCode == "") {
                $scope.validate_CommodityCategoryCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityCategoryCode = "";
            }
            if ($scope.CommodityCategory.Length == "") {
                $scope.validate_Length = "has-error";
                isValid = false;
            } else {
                $scope.validate_Length = "";
            }
            return isValid;
        }

        $scope.isValidCommodityCategory = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarDescription) {
                        customErrorMessage($filter('translate')('pages.commodityItemAttributes.valideteDescription'))
                       // $scope.errorTab1 = "Description already exists";
                        isValid = false;
                    }
                }
            }
            return isValid;
        }      


        $scope.IsExsistingCommodityUsageTypeByName = function () {
            $scope.errorTab1 = "";
            $scope.CommodityUsageTypeSubmitBtnDisabled = true;
            if ($scope.CommodityUsageType.Id != null && $scope.CommodityUsageType.Name != undefined || $scope.CommodityUsageType.Name.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/CommodityItemAttributes/IsExsistingCommodityUsageTypeByName',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.CommodityUsageType.Id, "Name": $scope.CommodityUsageType.Name }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarName = data;
                    $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                }).error(function (data, status, headers, config) {
                    $scope.CommodityUsageTypeSubmitBtnDisabled = false;
                });
            }
        }

        $scope.validateUseageType =  function(){
            var isValid = true;

            if ($scope.CommodityUsageType.Name == "") {
                $scope.validate_Name = "has-error";
                isValid = false;
            } else {
                $scope.validate_Name = "";
            }
            if ($scope.CommodityUsageType.Description == "") {
                $scope.validate_Description = "has-error";
                isValid = false;
            } else {
                $scope.validate_Description = "";
                
            }

            return isValid;
        }

        $scope.isValidUseageType = function () {
            var isValid = true;

            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarName) {
                        customErrorMessage($filter('translate')('pages.commodityItemAttributes.valideteName'))                   
                        isValid = false;
                    }
                }
            }
            return isValid;
        }


    });



