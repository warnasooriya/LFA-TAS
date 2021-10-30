app.controller('BrownAndWhiteDetailsCtrl',
    function ($scope, $rootScope, $http, SweetAlert, ngDialog, $localStorage, toaster) {
        $scope.ModalName = "Electronic Details";
        $scope.ModalDescription = "Add Edit Electronic Details";

        $scope.BAndWSubmitBtnIconClass = "";
        $scope.BAndWSubmitBtnDisabled = false;
        $scope.formAction = true;//true for add new
        $scope.BnWSearchGridloading = false;
        $scope.BnWSearchGridloadAttempted = false;
        $scope.BnWCurrency = "";
        $scope.dealersByCountry = [];

        $scope.errorTab1 = "";
        $scope.selectedpp = {};
        $scope.loadInitailData = function () { }
        $scope.ModelsForSearch = [];
        setCommodityType();
        function setCommodityType() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                angular.forEach($scope.CommodityTypes, function (value) {
                    if (value.CommodityTypeDescription == 'Electronic') {
                        $scope.CommodityTypeId = value.CommodityTypeId;
                    }
                });
                LoadDetails();
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.BAndW = {
            Id: "00000000-0000-0000-0000-000000000000",
            ItemPurchasedDate: '',
            MakeId: '',
            ModelId: '',
            SerialNo: '',
            ItemPrice: '',
            CategoryId: '',
            ModelYear: '',
            AddnSerialNo: '',
            CommodityUsageTypeId:'',
            ItemStatusId: '',
            InvoiceNo: '',
            ModelCode: '',
            DealerPrice: '',
            CountryId: '',
            DealerId: '',
            //DealerId: emptyGuid(),
            DealerCurrencyId: "00000000-0000-0000-0000-000000000000",
            currencyPeriodId: "00000000-0000-0000-0000-000000000000",
            Variant:'',

        };

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };


        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllActiveCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
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
            $http({
                method: 'POST',
                url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllBrownAndWhiteDetails',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.BAndWs = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetAllItemStatuss',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakesByComodityTypeId',
                data: { "Id": $scope.CommodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.Makes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCategories',
                data: { "Id": $scope.CommodityTypeId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.Categories = data;
            }).error(function (data, status, headers, config) {
            });
            $scope.Modeles = [];
        }


        $scope.SetModelpopup = function () {
            $scope.errorTab1 = "";
            if ($scope.bnWSearchGridSearchCriterias.MakeId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.bnWSearchGridSearchCriterias.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                             $scope.ModelsForSearch = data;

                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }

        $scope.SetModel = function () {
            $scope.errorTab1 = "";
            if ($scope.BAndW.MakeId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Makes, function (value, key) {
                    if (value.Id == $scope.BAndW.MakeId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                            $scope.Modeles = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }
        $scope.SetBAndWValues = function () {
            $scope.errorTab1 = "";
            if ($scope.BAndW.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetBrownAndWhiteDetailsById',
                    data: { "Id": $scope.BAndW.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.status == "Policy") {
                        //clearBAndWControls();
                        $scope.BAndWSubmitBtnDisabled = true;
                        SweetAlert.swal({
                            title: "Electronic Information",
                            text: "You can't modify as it has been added to a policy. Please update from policy approval.",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else if (data.status == "Bordx") {
                        //clearBAndWControls();
                         $scope.BAndWSubmitBtnDisabled = true;
                        SweetAlert.swal({
                            title: "Electronic Information",
                            text: "You can't modify as it has been added to a bordx. Please update from policy endorsement.",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else {
                        $scope.BAndW.Id = data.Id;
                        $scope.BAndW.CategoryId = data.CategoryId;
                        $scope.VINNoValidation();
                        $scope.BAndW.ItemPurchasedDate = data.ItemPurchasedDate;
                        $scope.BAndW.MakeId = data.MakeId;
                        $scope.BAndW.ModelId = data.ModelId;
                        $scope.BAndW.CountryId = data.CountryId;
                        $scope.BAndW.DealerId = data.DealerId;
                        $scope.BAndW.SerialNo = data.SerialNo;
                        $scope.VINNoValidate();
                        $scope.BAndW.ItemPrice = data.ItemPrice;
                        $scope.BAndW.ModelYear = data.ModelYear;
                        $scope.BAndW.AddnSerialNo = data.AddnSerialNo;
                        $scope.BAndW.CommodityUsageTypeId = data.CommodityUsageTypeId;
                        $scope.BAndW.ItemStatusId = data.ItemStatusId;
                        $scope.BAndW.InvoiceNo = data.InvoiceNo;
                        $scope.BAndW.ModelCode = data.ModelCode;
                        $scope.BAndW.DealerPrice = data.DealerPrice;
                        $scope.SetModel();
                    }
                }).error(function (data, status, headers, config) {
                    clearBAndWControls();
                });
            }
            else {
                clearBAndWControls();
            }
        }

        $scope.resetAll = function () {
            $scope.BAndW.Id = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.ItemPurchasedDate = "";
            $scope.BAndW.MakeId = "";
            $scope.BAndW.ModelId = "";
            $scope.BAndW.SerialNo = "";
            $scope.BAndW.ItemPrice = "";
            $scope.BAndW.CategoryId = "";
            $scope.BAndW.DealerId = "";
            $scope.BAndW.CountryId = "";
            $scope.BAndW.ModelYear = "";
            $scope.BAndW.AddnSerialNo = "";
            $scope.BAndW.CommodityUsageTypeId = "";
            $scope.BAndW.ItemStatusId = "";
            $scope.BAndW.InvoiceNo = "";
            $scope.BAndW.ModelCode = "";
            $scope.BAndW.DealerPrice = "";
            $scope.BAndW.Variant = "";
            $scope.VinLength = 0;
            $scope.Vin = false;
            $scope.formAction = true;//true for add new
            $scope.BAndWSubmitBtnDisabled = false;
        }

        function clearBAndWControls() {
            $scope.BAndW.Id = "00000000-0000-0000-0000-000000000000";
            $scope.BAndW.ItemPurchasedDate = "";
            $scope.BAndW.MakeId = "";
            $scope.BAndW.ModelId = "";
            $scope.BAndW.SerialNo = "";
            $scope.BAndW.ItemPrice = "";
            $scope.BAndW.CategoryId = "";
            $scope.BAndW.DealerId = "";
            $scope.BAndW.CountryId = "";
            $scope.BAndW.ModelYear = "";
            $scope.BAndW.AddnSerialNo = "";
            $scope.BAndW.CommodityUsageTypeId = "";
            $scope.BAndW.ItemStatusId = "";
            $scope.BAndW.InvoiceNo = "";
            $scope.BAndW.ModelCode = "";
            $scope.BAndW.DealerPrice = "";
            $scope.BAndW.Variant = "";
            $scope.VinLength = 0;
            $scope.Vin = false;
        };
        $scope.Vin = false;
        $scope.VinLength = 0;
        $scope.VINNoValidation = function () {
            angular.forEach($scope.Categories, function (value) {
                if (value.CommodityCategoryId == $scope.BAndW.CategoryId) {
                    $scope.VinLength = value.Length;
                }
            });
        }
        $scope.VINNoValidate = function () {
            if ($scope.BAndW.SerialNo.length == $scope.VinLength) {
                $scope.Vin = true;
            }
            else {
                $scope.Vin = false;
            }
        }

        $scope.validateBAndW = function () {
            var isValid = true;

            if ($scope.BAndW.CategoryId == "" || $scope.BAndW.CategoryId == null || $scope.BAndW.CategoryId == undefined) {
                $scope.validate_CategoryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CategoryId = "";
            }
            if ($scope.BAndW.MakeId == "" || $scope.BAndW.MakeId == null || $scope.BAndW.MakeId == undefined) {
                $scope.validate_MakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_MakeId = "";
            }
            if ($scope.BAndW.ModelId == "" || $scope.BAndW.ModelId == null || $scope.BAndW.ModelId == undefined) {
                $scope.validate_ModelId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelId = "";
            }
            if ($scope.BAndW.ModelYear == "" || $scope.BAndW.ModelYear == null || $scope.BAndW.ModelYear == undefined) {
                $scope.validate_ModelYear = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelYear = "";
            }
            if ($scope.BAndW.Variant == "" || $scope.BAndW.Variant == undefined || $scope.BAndW.Variant == null) {
                $scope.validate_Variant = "has-error";
                isValid = false;
            } else {
                $scope.validate_Variant = "";
            }
            if ($scope.BAndW.ModelCode == "" || $scope.BAndW.ModelCode == undefined) {
                $scope.validate_ModelCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_ModelCode = "";
            }
            if ($scope.BAndW.ItemStatusId == "" || $scope.BAndW.ItemStatusId == undefined || $scope.BAndW.ItemStatusId == null) {
                $scope.validate_ItemStatusId = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemStatusId = "";
            }
            if ($scope.BAndW.ItemPurchasedDate == "" || $scope.BAndW.ItemPurchasedDate == undefined || $scope.BAndW.ItemPurchasedDate == null) {
                $scope.validate_ItemPurchasedDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemPurchasedDate = "";
            }
            if ($scope.BAndW.SerialNo == "" || $scope.BAndW.SerialNo == undefined || $scope.BAndW.SerialNo == null) {
                $scope.validate_SerialNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_SerialNo = "";
            }
            if ($scope.BAndW.InvoiceNo == "" || $scope.BAndW.InvoiceNo == undefined) {
                $scope.validate_InvoiceNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_InvoiceNo = "";
            }
            if ($scope.BAndW.CommodityUsageTypeId == "" || $scope.BAndW.CommodityUsageTypeId == undefined || $scope.BAndW.CommodityUsageTypeId == null) {
                $scope.validate_CommodityUsageTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityUsageTypeId = "";
            }
            if ($scope.BAndW.ItemPrice == "" || $scope.BAndW.ItemPrice == undefined || $scope.BAndW.ItemPrice == null) {
                $scope.validate_ItemPrice = "has-error";
                isValid = false;
            } else {
                $scope.validate_ItemPrice = "";
            }
            if ($scope.BAndW.DealerPrice == "" || $scope.BAndW.DealerPrice == undefined || $scope.BAndW.DealerPrice == null) {
                $scope.validate_DealerPrice = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerPrice = "";
            }

            return isValid
        }

        $scope.BAndWSubmit = function () {
            if ($scope.validateBAndW()) {


                if ($scope.BAndW.ItemPurchasedDate != "") {
                    $scope.errorTab1 = "";
                    if ($scope.BAndW.Id == null || $scope.BAndW.Id == "00000000-0000-0000-0000-000000000000") {
                        $scope.BAndWSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.BAndWSubmitBtnDisabled = true;
                        //   alert($scope.BAndW.DealerPrice);
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/BrownAndWhiteDetails/AddBrownAndWhiteDetails',
                            data: $scope.BAndW,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.BAndWSubmitBtnIconClass = "";
                            $scope.BAndWSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: "Electronic Information",
                                    text: "Successfully Saved!",
                                    confirmButtonColor: "#007AFF"
                                });
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllBrownAndWhiteDetails',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    clearBAndWControls();
                                    $scope.BAndWs = data;

                                }).error(function (data, status, headers, config) {
                                    clearBAndWControls();
                                });

                            } else {
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: "Electronic Information",
                                text: "Error occured while saving data!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.BAndWSubmitBtnIconClass = "";
                            $scope.BAndWSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                    else {
                        $scope.BAndWSubmitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.BAndWSubmitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/BrownAndWhiteDetails/UpdateBrownAndWhiteDetails',
                            data: $scope.BAndW,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.BAndWSubmitBtnIconClass = "";
                            $scope.BAndWSubmitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: "Electronic Information",
                                    text: "Successfully Update!",
                                    confirmButtonColor: "#007AFF"
                                });

                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllBrownAndWhiteDetails',
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    $scope.BAndWs = data;
                                }).error(function (data, status, headers, config) {
                                });
                                clearBAndWControls();
                            } else {;
                            }
                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: "Electronic Information",
                                text: "Error occured while saving data!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.BAndWSubmitBtnIconClass = "";
                            $scope.BAndWSubmitBtnDisabled = false;
                            return false;
                        });
                    }
                } else {
                    customErrorMessage("Please Enter Electronic Item Details.")
                    //$scope.errorTab1 = "Please Enter Electronic Item Details";
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }


        //----------------------------------------- Search ------------------------------------------


        //$scope.bnWSearchGridSearchCriterias = {
        //    SerialNo: "",
        //    Make: emptyGuid(),
        //    Model: emptyGuid(),
        //    InvoiceNo: ""
        //};

        $scope.SearchItemPopupReset = function () {

            $scope.bnWSearchGridSearchCriterias = {
                SerialNo: '',
                MakeId: emptyGuid(),
                ModelId: emptyGuid(),
                InvoiceNo: ''

            }

        }


        var paginationOptionsBnWSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };


        $scope.SearchBnWPopup = function () {
            $scope.bnWSearchGridSearchCriterias = {
                SerialNo: '',
                Make: emptyGuid(),
                Model: emptyGuid(),
                InvoiceNo: ''

            };
            var paginationOptionsBnWSearchGrid = {
                pageNumber: 1,
                // pageSize: 25,
                sort: null
            };
            getBnWSearchPage();

            BnWSearchPopup = ngDialog.open({
                template: 'popUpBnWVehicle',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

        };

        var getBnWSearchPage = function () {

            $scope.BnWSearchGridloading = true;
            $scope.BnWSearchGridloadAttempted = false;


            var BnWSearchGridParam =
                {
                    'paginationOptionsBnWSearchGrid': paginationOptionsBnWSearchGrid,
                    'bnWSearchGridSearchCriterias': $scope.bnWSearchGridSearchCriterias
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/BrownAndWhiteDetails/GetAllItemsForSearchGrid',
                data: BnWSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //value.Dealer = data.DealerName
                var response_arr = JSON.parse(data);

                $scope.gridOptionsBnw.data = response_arr.data;
                $scope.gridOptionsBnw.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.BnWSearchGridloadAttempted = true;
                $scope.BnWSearchGridloading = false;

            });
        }


        $scope.gridOptionsBnw = {

            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'Serial No', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
              { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss', },
              { name: 'Model', field: 'Model', enableSorting: false, cellClass: 'columCss' },
              { name: 'Invoice No', field: 'InvoiceNo', enableSorting: false, cellClass: 'columCss' },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadBnW(row.entity.Id)" class="btn btn-xs btn-warning">Load</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsBnWSearchGrid.sort = null;
                    } else {
                        paginationOptionsBnWSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getBnWSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsBnWSearchGrid.pageNumber = newPage;
                    paginationOptionsBnWSearchGrid.pageSize = pageSize;
                    getBnWSearchPage();
                });
            }
        };
        $scope.refresBnwSearchGridData = function () {
            getBnWSearchPage();
        }

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }

        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        $scope.loadBnW = function (BnWId) {
            if (isGuid(BnWId)) {
                BnWSearchPopup.close();
                $scope.formAction = false;
                $scope.BAndW.BandWSearchDisabled = true;
                $scope.BAndWSubmitBtnDisabled = true;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/BrownAndWhiteDetails/GetBrownAndWhiteDetailsById',
                    data: { "Id": BnWId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.BAndW.BandWSearchDisabled = false;
                    $scope.BAndWSubmitBtnDisabled = false;
                    $scope.BAndW.Id = data.Id;
                    $scope.BAndW.CategoryId = data.CategoryId;
                    $scope.VINNoValidation();
                    $scope.BAndW.ItemPurchasedDate = data.ItemPurchasedDate;
                    $scope.BAndW.MakeId = data.MakeId;
                    $scope.BAndW.ModelId = data.ModelId;
                    $scope.BAndW.SerialNo = data.SerialNo;
                    $scope.VINNoValidate();
                    $scope.BAndW.ItemPrice = data.ItemPrice;
                    $scope.BAndW.CountryId = data.CountryId;
                    $scope.BAndW.DealerId = data.DealerId;
                    $scope.BAndW.ModelYear = data.ModelYear;
                    $scope.BAndW.AddnSerialNo = data.AddnSerialNo;
                    $scope.BAndW.CommodityUsageTypeId = data.CommodityUsageTypeId;
                    $scope.BAndW.ItemStatusId = data.ItemStatusId;
                    $scope.BAndW.InvoiceNo = data.InvoiceNo;
                    $scope.BAndW.ModelCode = data.ModelCode;
                    $scope.BAndW.DealerPrice = data.DealerPrice;
                    $scope.BAndW.Variant = data.Variant;
                    $scope.selectedDealerChanged();
                    $scope.SetBAndWValues();
                    //$scope.formAction = false;//true for add new


                }).error(function (data, status, headers, config) {
                    $scope.BAndW.BandWSearchDisabled = false;
                    $scope.BAndWSubmitBtnDisabled = false;
                    // clearCustomerControls();
                });
            }
        }


        //----------------------------------- Dealer Currency -------------------------------------

        $scope.selectedDealerChanged = function () {

            if (isGuid($scope.BAndW.DealerId)) {
                angular.forEach($scope.dealersByCountry, function (value) {
                    if ($scope.BAndW.DealerId == value.Id) {
                        $scope.BAndW.currencyPeriodId = value.currencyPeriodId;
                        $scope.BAndW.DealerCurrencyId = value.CurrencyId;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurerContractsByInsurerId',
                            data: { "Id": $scope.currentContract.insurerId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.reinsurerContracts = data;
                        }).error(function (data, status, headers, config) {
                        });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                            data: { "Id": value.CurrencyId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.BnWCurrency = data.Code;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                data: { "Id": value.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == false) {
                                    SweetAlert.swal({
                                        title: "TAS Information",
                                        text: "Selected dealer's currency(" + $scope.BnWCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                        type: "warning",
                                        confirmButtonColor: "rgb(221, 107, 85)"
                                    });
                                }
                            }).error(function (data, status, headers, config) {
                            });
                            return false;

                        });
                    }
                });
            }
        }


        $scope.selectedCountryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealersByCountryId',
                data: { "Id": $scope.BAndW.CountryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealersByCountry = data;
                if (isGuid($scope.BAndW.CountryId)) {
                    angular.forEach($scope.dealersByCountry, function (value) {
                        if ($scope.BAndW.DealerId == value.Id) {
                            //$scope.Vehicle.insurerId = value.InsurerId;
                            $scope.BAndW.DealerCurrencyId = value.CurrencyId;


                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                                data: { "Id": value.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.BnWCurrency = data.Code;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                    data: { "Id": value.CurrencyId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    if (data == false) {
                                        SweetAlert.swal({
                                            title: "TAS Information",
                                            text: "Selected dealer's currency(" + $scope.BnWCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                            type: "warning",
                                            confirmButtonColor: "rgb(221, 107, 85)"
                                        });
                                    }
                                }).error(function (data, status, headers, config) {
                                });

                            }).error(function (data, status, headers, config) {
                            });
                            return false;
                        }
                    });
                }
            }).error(function (data, status, headers, config) {
            });
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/Country/GetAllTaxTypesFromCountryId',
            //    data: { "Id": $scope.Vehicle.CountryId },
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.countryTaxes = data;
            //    if (isGuid($scope.contractIdToUpdate)) {
            //        angular.forEach($scope.temp_taxdetails, function (value) {
            //            angular.forEach($scope.countryTaxes, function (lvalue) {
            //                if (value.TaxTypeId == lvalue.Tax.Id) {
            //                    lvalue.IsSelected = true;
            //                    return false;
            //                }
            //            });
            //        });
            //        temp_taxdetails = [];
            //    }
            //}).error(function (data, status, headers, config) {
            //});

        }

        $scope.SetVariant = function () {
            $scope.errorTab1 = "";
            if ($scope.BAndW.ModelId != null) {
                $scope.ValueDisable = false;
                angular.forEach($scope.Modeles, function (value, key) {
                    if (value.Id == $scope.BAndW.ModelId) {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/VariantManagement/GetAllVariantByModelId',
                            data: { "Id": value.Id },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Variantss = data;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            }
        }

    });



