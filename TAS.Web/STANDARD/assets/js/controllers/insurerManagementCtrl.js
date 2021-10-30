app.controller('InsurerManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $filter, $translate) {
        $scope.ModalName = "Insurer Management";
        $scope.ModalDescription = "Add Edit Insurer Management";

        $scope.labelSaveInsurer = 'common.button.save';
        $scope.labelSaveInsurerConsortium = 'common.button.save';

        $scope.ConsortiumSubmitBtnIconClass = "";
        $scope.ConsortiumSubmitBtnDisabled = false;
        $scope.InsurerSubmitBtnIconClass = "";
        $scope.InsurerSubmitBtnDisabled = false;

        $scope.AddInsurerBtnIconClass = "";
        $scope.AddInsurerBtnDisabled = false;

        $scope.PercentageTotal = 0;
        $scope.CountryList = [];
        $scope.SelectedCountryList = [];
        $scope.SelectedCountryDList = [];

        $scope.CommodityTypeList = [];
        $scope.SelectedCommodityTypeList = [];
        $scope.SelectedCommodityTypeDList = [];

        $scope.ProductList = [];
        $scope.SelectedProductList = [];
        $scope.SelectedProductDList = [];

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };

        var customWarningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('common.popUpMessages.warning'), msg, 12000);
        };


        $scope.settings = {
            closeOnBlur: true,
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: true,
            closeOnBlur: false,
            showUncheckAll: true,
            closeOnBlur: true,
            closeOnSelect: true
        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('common.customText.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('common.customText.itemSelected')
        };

        $scope.loadInitailData = function () { }
        $scope.errorTab1 = "";
        $scope.errorTab2 = "";

        $scope.Insurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            InsurerCode: '',
            BookletCode: '',
            InsurerShortName: '',
            InsurerFullName: '',
            Comments: '',
            Countries: [],
            Products: [],
            CommodityTypes:[],
            IsActive: false
        };
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                AddCommodityType();
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.Insurers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
                AddCountry();
            }).error(function (data, status, headers, config) {
            });        
        }

        LoadDetails();
        
        $scope.SetInsurerValues = function () {
            var cId = $scope.Insurer.Id;
            clearInsurerControls();
            $scope.Insurer.Id = cId;
            $scope.errorTab1 = "";
            if ($scope.Insurer.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/InsurerManagement/GetInsurerById',
                    data: { "Id": $scope.Insurer.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                    $scope.Insurer.Id = data.Id;
                    $scope.Insurer.InsurerCode = data.InsurerCode;
                    $scope.Insurer.BookletCode = data.BookletCode;
                    $scope.Insurer.InsurerShortName = data.InsurerShortName;
                    $scope.Insurer.InsurerFullName = data.InsurerFullName;
                    $scope.Insurer.Comments = data.Comments;
                    $scope.Insurer.Countries = data.Countries;
                    $scope.Insurer.Products = data.Products;
                    $scope.Insurer.CommodityTypes = data.CommodityTypes;
                    $scope.Insurer.IsActive = data.IsActive;
                    LoadCountry();
                LoadCommodityType();
                $scope.labelSaveInsurer = 'common.button.update';
                }).error(function (data, status, headers, config) {
                    clearInsurerControls();
                });
            }
            else {
                clearInsurerControls();
            }
        }

        function clearInsurerControls() {
            $scope.Insurer.Id = "00000000-0000-0000-0000-000000000000";
            $scope.Insurer.InsurerCode="";
            $scope.Insurer.BookletCode="";
            $scope.Insurer.InsurerShortName="";
            $scope.Insurer.InsurerFullName="";
            $scope.Insurer.Comments="";
            $scope.Insurer.CommodityTypes = [];
            $scope.Insurer.Products = [];
            $scope.Insurer.Countries = [];
            $scope.Insurer.IsActive = false;
            $scope.SelectedCountryDList = [];
            $scope.SelectedCountryList = [];
            $scope.SelectedCommodityTypeList = [];
            $scope.SelectedCommodityTypeDList = [];
            $scope.ProductList = [];
            $scope.SelectedProductList = [];
            $scope.SelectedProductDList = [];
        };
      
        $scope.validateInsureDetails = function () {
            var isValid = true;
            if ($scope.Insurer.InsurerCode == "" || $scope.Insurer.InsurerCode == undefined) {
                $scope.validate_InsurerCode = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_InsurerCode = "";
            }
            if ($scope.Insurer.InsurerFullName == "" || $scope.Insurer.InsurerFullName == undefined) {
                $scope.validate_InsurerFullName = "has-error";
                isValid = false;
            } else {
                $scope.validate_InsurerFullName = "";
            }
            if ($scope.Insurer.InsurerShortName == "" || $scope.Insurer.InsurerShortName == undefined) {
                $scope.validate_InsurerShortName = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_InsurerShortName = "";
            }
            return isValid
        }

        $scope.InsurerSubmit = function () {
            $scope.SendCountry();
            if ($scope.validateInsureDetails()) {
                $scope.errorTab1 = "";
                if ($scope.Insurer.Id == null || $scope.Insurer.Id == "00000000-0000-0000-0000-000000000000") {
                    $scope.InsurerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.InsurerSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/InsurerManagement/AddInsurer',
                        data: $scope.Insurer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.InsurerSubmitBtnIconClass = "";
                        $scope.InsurerSubmitBtnDisabled = false;
                        if (data == "OK") {
                            $scope.labelSaveInsurer = 'common.button.save';
                            SweetAlert.swal({
                                title: $filter('translate')('pages.insurerManagement.tabInsurerDetails.insurerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Insurers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearInsurerControls();
                        } else {
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.insurerManagement.tabInsurerDetails.insurerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.InsurerSubmitBtnIconClass = "";
                        $scope.InsurerSubmitBtnDisabled = false;
                        return false;
                    });
                }
                else {
                    $scope.InsurerSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.InsurerSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/InsurerManagement/UpdateInsurer',
                        data: $scope.Insurer,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.InsurerSubmitBtnIconClass = "";
                        $scope.InsurerSubmitBtnDisabled = false;
                        if (data == "OK") {
                            $scope.labelSaveInsurer = 'common.button.save';
                            SweetAlert.swal({
                                title: $filter('translate')('pages.insurerManagement.tabInsurerDetails.insurerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Insurers = data;
                            }).error(function (data, status, headers, config) {
                            });
                            clearInsurerControls();
                        } else {;
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.insurerManagement.tabInsurerDetails.insurerInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            type: "warning",
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.InsurerSubmitBtnIconClass = "";
                        $scope.InsurerSubmitBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'))
            }
        }

        function AddCountry() {
            var index = 0;
            $scope.CountryList = [];
            angular.forEach($scope.Countries, function (value) {
                var x = { id: index, code: value.Id, label: value.CountryName };
                $scope.CountryList.push(x);
                index = index + 1;
            });
        }
        function LoadCountry() {
            $scope.SelectedCountryList = [];
            $scope.SelectedCountryDList = [];
            angular.forEach($scope.Insurer.Countries, function (valueOut) {
                angular.forEach($scope.CountryList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedCountryList.push(x);
                        $scope.SelectedCountryDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendCountry=function() {
            $scope.SelectedCountryDList = [];
            $scope.Insurer.Countries = [];
            angular.forEach($scope.SelectedCountryList, function (valueOut) {
                angular.forEach($scope.CountryList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Insurer.Countries.push(valueIn.code);
                        $scope.SelectedCountryDList.push(valueIn.label);
                    }
                });
            });
        }

        function AddProduct() {
            var index = 0;
            $scope.ProductList = [];
            angular.forEach($scope.Products, function (value) {
                var x = { id: index, code: value.Id, label:value.Productcode +' : '+ value.Productname };
                $scope.ProductList.push(x);
                index = index + 1;
            });
        }
        function LoadProduct() {
            $scope.SelectedProductList = [];
            $scope.SelectedProductDList = [];
            angular.forEach($scope.Insurer.Products, function (valueOut) {
                angular.forEach($scope.ProductList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedProductList.push(x);
                        $scope.SelectedProductDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendProduct = function () {
            $scope.SelectedProductDList = [];
            $scope.Insurer.Products = [];
            angular.forEach($scope.SelectedProductList, function (valueOut) {
                angular.forEach($scope.ProductList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Insurer.Products.push(valueIn.code);
                        $scope.SelectedProductDList.push(valueIn.label);
                    }
                });
            });
        }

        function AddCommodityType() {
            var index = 0;
            $scope.CommodityTypeList = [];
            angular.forEach($scope.CommodityTypes, function (value) {
                var x = { id: index, code: value.CommodityTypeId, label: value.CommodityTypeDescription };
                $scope.CommodityTypeList.push(x);
                index = index + 1;
            });
        }
        function LoadCommodityType() {
            $scope.Products = [];
            $scope.SelectedCommodityTypeList = [];
            $scope.SelectedCommodityTypeDList = [];
            angular.forEach($scope.Insurer.CommodityTypes, function (valueOut) {
                angular.forEach($scope.CommodityTypeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedCommodityTypeList.push(x);
                        $scope.SelectedCommodityTypeDList.push(valueIn.label);
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": valueOut }
                }).success(function (data, status, headers, config) {
                    var temp = data;
                    angular.forEach(temp, function (value) {
                        $scope.Products.push(value);
                        AddProduct();
                        LoadProduct();
                    });
                }).error(function (data, status, headers, config) {
                });
            });
        }
        $scope.SendCommodityType = function () {
            $scope.Products = [];
            $scope.SelectedCommodityTypeDList = [];
            $scope.Insurer.CommodityTypes = [];
            angular.forEach($scope.SelectedCommodityTypeList, function (valueOut) {
                angular.forEach($scope.CommodityTypeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Insurer.CommodityTypes.push(valueIn.code);
                        $scope.SelectedCommodityTypeDList.push(valueIn.label);
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                            data: { "Id": valueIn.code }
                        }).success(function (data, status, headers, config) {
                            var temp = data;
                            angular.forEach(temp, function (value) {
                                $scope.Products.push(value);
                                AddProduct();
                            });
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            });
        }

        $scope.RiskTotal = 0;
        $scope.C = {
            ParentInsurerId: "00000000-0000-0000-0000-000000000000",
            InsurerId: "00000000-0000-0000-0000-000000000000"
        }
        $scope.LoadConsortiums = function () {
            $scope.Consortium = [];
            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetInsurerConsortiumsByInsurerId',
                data: { "Id": $scope.C.ParentInsurerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
        }).success(function (data, status, headers, config) {
                $scope.InsurerConsortiums = data;
                $scope.Consortium = [];
                $scope.ConsortiumTemp = [];
                angular.forEach($scope.Insurers, function (valueR) {
                    if (valueR.Id != $scope.C.ParentInsurerId) {
                        $scope.ConsortiumTemp.push(valueR);
                    }
                });
                angular.forEach($scope.ConsortiumTemp, function (value) {
                    var exists = false;
                    angular.forEach($scope.InsurerConsortiums, function (valueC) {
                        if (value.Id == valueC.InsurerId)
                            exists = true;
                    });
                    if (!exists) {
                        $scope.Consortium.push(value);
                    }
                });
                $scope.TotalCalc();
                angular.forEach($scope.InsurerConsortiums, function (value) {
                    angular.forEach($scope.Insurers, function (valueR) {
                        if (valueR.Id == value.InsurerId)
                            value.Insurer = valueR.InsurerFullName;
                    });
                });
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.TotalCalc = function () {
            $scope.RiskTotal = 0;
            $scope.PercentageTotal = 0;
         //   $scope.NRPTotal = 0;
            angular.forEach($scope.InsurerConsortiums, function (value) {
                $scope.RiskTotal = $scope.RiskTotal + value.RiskSharePercentage;
                $scope.PercentageTotal = $scope.PercentageTotal + value.ProfitSharePercentage;
               // $scope.NRPTotal = $scope.NRPTotal + value.NRPPercentage;
            });
        }

        $scope.valideteConsortium = function () {
            var isValid = true;
            if ($scope.C.ParentInsurerId == "00000000-0000-0000-0000-000000000000" || $scope.C.ParentInsurerId == undefined) {
                $scope.validate_InsurerParentInsurerId = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_InsurerParentInsurerId = "";
            }

            if ($scope.C.InsurerId == "00000000-0000-0000-0000-000000000000" || $scope.C.InsurerId == undefined) {
                $scope.validate_InsurerInsurerId = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_InsurerInsurerId = "";
            }

            return isValid
        }
        $scope.AddInsurer = function () {
            if ($scope.valideteConsortium()) {
                var item = {
                    Id: $scope.C.InsurerId,
                    Id2: "10000000-0000-0000-0000-000000000001",
                    ParentInsurerId: $scope.C.ParentInsurerId,
                    InsurerId: $scope.C.InsurerId,
                    RiskSharePercentage: 0,
                    // NRPPercentage: 0,
                    ProfitSharePercentage: 0,
                }
                angular.forEach($scope.Insurers, function (valueR) {
                    if (valueR.Id == $scope.C.InsurerId)
                        item.Insurer = valueR.InsurerFullName;
                });
                $scope.InsurerConsortiums.push(item);
                $scope.Consortium = [];
                $scope.ConsortiumTemp = [];
                $scope.C.InsurerId = "00000000-0000-0000-0000-000000000000";
                angular.forEach($scope.Insurers, function (valueR) {
                    if (valueR.Id != $scope.C.ParentInsurerId) {
                        $scope.ConsortiumTemp.push(valueR);
                    }
                });
                angular.forEach($scope.ConsortiumTemp, function (value) {
                    var exists = false;
                    angular.forEach($scope.InsurerConsortiums, function (valueC) {
                        if (value.Id == valueC.InsurerId)
                            exists = true;
                    });
                    if (!exists) {
                        $scope.Consortium.push(value);

                    }
                });
            } else {
                //SweetAlert.swal({
                //    title: "Insurer Consortium Information",
                //    text: "Please Select Parent Insurer and Insure.",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customErrorMessage($filter('translate')('pages.insurerManagement.errorMessages.selectParentInsurer'))
            }
        }
        function TotalValidation() {
            if ($scope.RiskTotal != 100 || $scope.PercentageTotal != 100) {// || $scope.NRPTotal != 100) {
                //SweetAlert.swal({
                //    title: "Insurer Consortium Information",
                //    text: "Risk and Profit Total Should be 100%.",
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                customWarningMessage($filter('translate')('pages.insurerManagement.errorMessages.riskProfit'))
                return false;
            }
            return true;
        }
        function ClearInsurerConsortium() {
            $scope.RiskTotal = 0;
            $scope.PercentageTotal = 0;
            $scope.C = {
                ParentInsurerId: "00000000-0000-0000-0000-000000000000",
                InsurerId: "00000000-0000-0000-0000-000000000000"
            }
            $scope.InsurerConsortiums = [];
        }
        $scope.InsurerConsortiums = [];
        $scope.Removed = [];
        $scope.Remove = function (val) {
            $scope.Removed.push(val);
            $scope.Temp = [];
            angular.copy($scope.InsurerConsortiums, $scope.Temp);
            $scope.InsurerConsortiums = [];
            angular.forEach($scope.Temp, function (value) {
                if (value.Id != val.Id) {
                    $scope.InsurerConsortiums.push(value);
                }
            });
            $scope.Consortium = [];
            $scope.ConsortiumTemp = [];
            angular.forEach($scope.Insurers, function (valueR) {
                if (valueR.Id != $scope.C.ParentInsurerId) {
                    $scope.ConsortiumTemp.push(valueR);
                }
            });
            angular.forEach($scope.ConsortiumTemp, function (value) {
                var exists = false;
                angular.forEach($scope.InsurerConsortiums, function (valueC) {
                    if (value.Id == valueC.InsurerId)
                        exists = true;
                });
                if (!exists) {
                    $scope.Consortium.push(value);
                }
            });
            $scope.TotalCalc();
             
        }


        $scope.ConsortiumSubmit = function () {
            if ($scope.InsurerConsortiums.length > 0) {

                if( TotalValidation()){
                    angular.forEach($scope.Removed, function (value) {
                        value.RiskSharePercentage = 0;
                        $scope.InsurerConsortiums.push(value);
                    });
                    var Data =
                        {
                            Consortiums: $scope.InsurerConsortiums
                        };
                    $scope.errorTab2 = "";
                    $scope.ConsortiumSubmitBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.ConsortiumSubmitBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/InsurerManagement/AddorUpdateInsurerConsortiums',
                        data: Data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.ConsortiumSubmitBtnIconClass = "";
                        $scope.ConsortiumSubmitBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.insurerManagement.tabInsurerConsortium.insurerConsortiumInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            ClearInsurerConsortium();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.insurerManagement.tabInsurerConsortium.insurerConsortiumInformation'),
                            text: $filter('translate')('common.errMessage.errorOccured'),
                            confirmButtonText: $filter('translate')('common.button.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.ConsortiumSubmitBtnIconClass = "";
                        $scope.ConsortiumSubmitBtnDisabled = false;
                        return false;
                    });
                }else{
                    $scope.errorTab2 = "";
                }
            }
            else {
                //$scope.errorTab2 = "Please Enter Consortium";
                customErrorMessage($filter('translate')('pages.insurerManagement.errorMessages.enterConsortium'))
            }
        }
    });



