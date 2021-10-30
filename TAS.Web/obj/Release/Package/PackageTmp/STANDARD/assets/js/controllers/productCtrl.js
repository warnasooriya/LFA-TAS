app.controller('ProductCtrl',
    function ($scope, $rootScope, $http, FileUploader, SweetAlert, $localStorage, $cookieStore, toaster, $filter, $translate) {
        $scope.ModalName = "Product Definition";
        $scope.ModalDescription = "Add Edit Products";
        $scope.selectedpp = {};
        $scope.errorTab1 = "";
        $scope.submitBtnIconClass = "";
        $scope.submitBtnDisabled = false;
        $scope.successUpload = true;
        $scope.labelSave = 'pages.productManagement.productSave';

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.productManagement.error'), msg);
        };

        var customWarningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('pages.productManagement.warning'), msg, 12000);
        };


        $scope.loadInitailData = function () {
        }
        $scope.parentProducts = [];
        LoadDetails();
        function LoadDetails() {
            var jwt = $cookieStore.get('jwt');
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetProductTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.ProductTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllParentProducts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.pparentProducts = data;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.SetProductValues = function () {
            var jwt = $cookieStore.get('jwt');
            $scope.Product.image = false;
            if ($scope.Product.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetProductById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "productId": $scope.Product.Id }
                }).success(function (data, status, headers, config) {
                    $scope.Product = data;
                    $scope.Product.productName = data.Productname;
                    $scope.Product.productcode = data.Productcode;
                    $scope.Product.commodityTypeId = data.CommodityTypeId;
                    $scope.Product.productTypeId = data.ProductTypeId;
                    $scope.Product.Ismandatoryproduct = data.Ismandatoryproduct;
                    $scope.Product.Isbundledproduct = data.Isbundledproduct;
                    $scope.Product.productDescription = data.Productdescription;
                    $scope.Product.productShortDescription = data.Productshortdescription;
                    $scope.Product.DisplayImage = data.Displayimage;
                    $scope.Product.DisplayImageSrc = data.DisplayImageSrc;
                    if ($scope.Product.DisplayImage != "00000000-0000-0000-0000-000000000000") {
                        $scope.Product.image = true;
                    }
                    var log = [];
                    angular.forEach(data.BundledProducts, function (value, key) {
                        $scope.selectedpp[value["ParentProductId"]] = true;
                    }, log);
                    $scope.labelSave = 'pages.productManagement.productUpdate';
                }).error(function (data, status, headers, config) {
                    clearControls();
                });

                var log = [];
                $scope.parentProducts = [];
                angular.forEach($scope.Products, function (value, key) {
                    if (value.Id != $scope.Product.Id) {
                        if (value.Isbundledproduct != true) {
                            $scope.parentProducts.push(value);
                        }
                    }
                }, log);
            }
            else {
                $scope.labelSave = 'pages.productManagement.productSave';
                clearControls();
            }
        }
        $scope.SetCommodityTypeValue = function () {
            if ($scope.Product.commodityTypeId != undefined && $scope.Product.commodityTypeId != '' && $scope.Product.commodityTypeId != "00000000-0000-0000-0000-000000000000") {
                //clearControls();
                $scope.parentProducts = [];
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProducts',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.Product.commodityTypeId }
                }).success(function (data, status, headers, config) {
                    $scope.Products = data;

                    var log = [];
                    angular.forEach($scope.Products, function (value, key) {
                        if (value.Id != $scope.Product.Id) {
                            if (value.Isbundledproduct != true) {
                                $scope.parentProducts.push(value);
                            }


                        }
                    }, log);

                }).error(function (data, status, headers, config) {
                    clearControls();
                });

            } else {
                clearControls();
            }
        }



        //$scope.SetParentProductVal = function (val)
        //{
        //    if (val == $scope.Product.Id) {
        //        SweetAlert.swal({
        //            title: "Product Information",
        //            text: "Parent product cannot be the same!",
        //            confirmButtonColor: "#007AFF"
        //        });
        //    }
        //}
        $scope.Product = {
            productName: '',
            productcode : '',
            commodityTypeId : "00000000-0000-0000-0000-000000000000",
            productTypeId : "00000000-0000-0000-0000-000000000000",
            Ismandatoryproduct : false,
            Isbundledproduct : false,
            Isactive : false,
            productDescription : "",
            productShortDescription : "",
            DisplayImageSrc : "",
            DisplayImage : "",
            Id: "00000000-0000-0000-0000-000000000000",
            image : false


        };

        function clearControls() {
            $scope.Product.productName = "";
            $scope.Product.productcode = "";
            $scope.Product.commodityTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Product.productTypeId = "00000000-0000-0000-0000-000000000000";
            $scope.Product.Ismandatoryproduct = false;
            $scope.Product.Isbundledproduct = false;
            $scope.Product.Isactive = false;
            $scope.Product.productDescription = "";
            $scope.Product.productShortDescription = "";
            $scope.Product.DisplayImageSrc = "";
            $scope.Product.DisplayImage = "";
            $scope.selectedpp = {};
            $scope.Product.Id = "00000000-0000-0000-0000-000000000000";
            $scope.parentProducts = [];
            $scope.Products = [];
            angular.element("input[type='file']").val('');
            $scope.Product.image = false;
        }
        var jwtt = $cookieStore.get('jwt');
        var uploader1 = $scope.uploader1 = new FileUploader({
            url: window.location.protocol + '//' + window.location.host + '/TAS.Web/api/Image/UploadImage',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwtt : $localStorage.jwt }
        });
        $scope.aa = { bb: "" };

        $scope.validateProduct = function () {
            var isValid = true;
            if ($scope.Product.productName == "" || typeof $scope.Product.productName == 'undefined') {
                $scope.validate_productname = "has-error";
                isValid = false;
            } else {
                $scope.validate_productname = "";
            }

            if (!isGuid($scope.Product.commodityTypeId)) {
                $scope.validate_commodityTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_commodityTypeId = "";
            }
            if (!isGuid($scope.Product.productTypeId )) {
                $scope.validate_productTypeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_productTypeId = "";
            }
            return isValid
        }

        $scope.submit = function () {
            var jwt = $cookieStore.get('jwt');
            $scope.Product.selectedpp = [];
            var log = [];
            angular.forEach($scope.selectedpp, function (value,key) {
                if (value) {
                    $scope.Product.selectedpp.push(key);
                }
            }, log);

            if (uploader1.queue.length > 0) {
                uploader1.queue[0].upload();

            } else {

                if ($scope.validateProduct()) {
                    if ($scope.isValidProductName()){
                        if ($scope.Product.selectedpp.length < 0) {
                            customErrorMessage($filter('translate')('pages.productManagement.selectParentProducts'));
                            //$scope.errorTab1 = "Please Select Parent Products..";
                            return;
                        } else {
                            $scope.errorTab1 = "";
                        }
                    if ($scope.Product.Id === null || $scope.Product.Id === "00000000-0000-0000-0000-000000000000" || $scope.Product.Id === '') {
                        if ($scope.Product.DisplayImage === "")
                            $scope.Product.DisplayImage = "00000000-0000-0000-0000-000000000000";
                        if ($scope.successUpload) {
                            $scope.submitBtnIconClass = "fa fa-spinner fa-spin";
                            $scope.submitBtnDisabled = true;
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Product/AddProduct',
                                data: $scope.Product,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Ok = data;
                                $scope.submitBtnIconClass = "";
                                $scope.submitBtnDisabled = false;
                                if (data === "OK") {

                                    SweetAlert.swal({
                                        title: $filter('translate')('pages.productManagement.productInformation'),
                                        text: $filter('translate')('pages.productManagement.successfullySaved'),
                                        confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                        confirmButtonColor: "#007AFF"
                                    });
                                    clearControls();
                                    $scope.SetCommodityTypeValue();

                                    //$http({
                                    //    method: 'POST',
                                    //    url: '/TAS.Web/api/Product/GetAllProducts',
                                    //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    //}).success(function (data, status, headers, config) {
                                    //    $scope.Products = data;
                                    //}).error(function (data, status, headers, config) {
                                    //});

                                    //$http({
                                    //    method: 'POST',
                                    //    url: '/TAS.Web/api/Product/GetAllParentProducts',
                                    //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    //}).success(function (data, status, headers, config) {
                                    //    $scope.pparentProducts = data;
                                    //}).error(function (data, status, headers, config) {
                                    //});

                                } else {
                                    alert(data);
                                }

                                return false;
                            }).error(function (data, status, headers, config) {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.productManagement.productInformation'),
                                    text: $filter('translate')('pages.productManagement.Erroroccuredsavingdata'),
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                    type: "warning",
                                    confirmButtonColor: "rgb(221, 107, 85)"
                                });
                                $scope.submitBtnIconClass = "";
                                $scope.submitBtnDisabled = false;
                                return false;
                            });
                             } else {

                            SweetAlert.swal({
                                title: $filter('translate')('pages.productManagement.productInformation'),
                                text: "Choose a Different Type Attchment!",
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });

                        }

                    }
                    else {
                        $scope.submitBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.submitBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Product/UpdateProduct',
                            data: $scope.Product,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.Ok = data;
                            $scope.submitBtnIconClass = "";
                            $scope.submitBtnDisabled = false;
                            if (data == "OK") {
                                SweetAlert.swal({
                                    title: $filter('translate')('pages.productManagement.productInformation'),
                                    text: $filter('translate')('pages.productManagement.successfullySaved'),
                                    confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                    confirmButtonColor: "#007AFF"
                                });
                                clearControls();
                                $scope.SetCommodityTypeValue();

                            } else {
                                alert(data);
                            }

                            return false;
                        }).error(function (data, status, headers, config) {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.productManagement.productInformation'),
                                text: $filter('translate')('pages.productManagement.Erroroccuredsavingdata'),
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                            $scope.submitBtnIconClass = "";
                            $scope.submitBtnDisabled = false;
                            return false;
                        });
                    }
                }
                } else {
                    customErrorMessage($filter('translate')('pages.currencyManagement.fillvalidfeild'))
                }
            }
        }

        $scope.IsExsistingProductName = function () {
            if ($scope.Product.Id != null && $scope.Product.productName != undefined || $scope.Product.productName.trim() != "") {
                $http({
                    method: 'POST',
                    async: false,
                    url: '/TAS.Web/api/Product/IsExsistingProductName',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.Product.Id, "ProductName": $scope.Product.productName }
                }).success(function (data, status, headers, config) {
                    $scope.IsExistsVarProductName = data;
                }).error(function (data, status, headers, config) {

                });
            }
        }

        $scope.isValidProductName = function () {
            var isValid = true;
            if (isValid) {

                if (isValid) {
                    if ($scope.IsExistsVarProductName) {
                        customWarningMessage($filter('translate')('pages.currencyManagement.productNamealreadyexists'))
                        //$scope.errorTab2 = "Engine Capacity Number already exists";
                        isValid = false;
                    }
                }
            }
            return isValid
        }

        uploader1.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "bmp" || extension == "png")
                    return true;
                else {
                    alert($filter('translate')('pages.currencyManagement.invalidfileformat'));
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
                    alert($filter('translate')('pages.currencyManagement.selectedfileexceeds'));
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
                    alert($filter('translate')('pages.currencyManagement.exceededthelimit'));
                    return false;
                }
            }
        });
        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
            $scope.successUpload = false;
        };
        uploader1.onAfterAddingFile = function (fileItem) {
        };
        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            var jwt = $cookieStore.get('jwt');
            $scope.uploader1.queue = [];
            $scope.uploader1.progress = 0;
            $scope.Product.DisplayImage = response.replace(/\"/g, "");
            $scope.Product.selectedpp = [];
            var log = [];
            angular.forEach($scope.selectedpp, function (value, key) {
                if (value) {
                    $scope.Product.selectedpp.push(key);
                }
            }, log);
            if (uploader1.queue.length > 0) {
                uploader1.queue[0].upload();
            } else {
                if ($scope.Product.Id == null || $scope.Product.Id == "00000000-0000-0000-0000-000000000000" || $scope.Product.Id == '') {
                    if ($scope.Product.productName == "" || typeof $scope.Product.productName == 'undefined'
                        || $scope.Product.commodityTypeId == "" || typeof $scope.Product.commodityTypeId == 'undefined') {
                        $scope.errorTab1 = $filter('translate')('pages.currencyManagement.enterCommodityProductName');
                        return;
                    } else {
                        $scope.errorTab1 = "";
                    }
                    if ($scope.Product.selectedpp.length <= 0) {
                        $scope.errorTab1 = $filter('translate')('pages.currencyManagement.selectParentProducts');
                        return;
                    } else {
                        $scope.errorTab1 = "";
                    }
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Product/AddProduct',
                        data: $scope.Product,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        if (data === "OK") {
                            SweetAlert.swal({
                                title: $filter('translate')('pages.productManagement.productInformation'),
                                text: $filter('translate')('pages.productManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/Product/GetAllProducts',
                            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //}).success(function (data, status, headers, config) {
                            //    $scope.Products = data;
                            //}).error(function (data, status, headers, config) {
                            //});

                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/Product/GetAllParentProducts',
                            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //}).success(function (data, status, headers, config) {
                            //    $scope.pparentProducts = data;
                            //}).error(function (data, status, headers, config) {
                            //});

                            clearControls();

                        } else {
                            alert(data);
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: $filter('translate')('pages.productManagement.productInformation'),
                            text: $filter('translate')('pages.productManagement.Erroroccuredsavingdata'),
                            confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    });

                }
                else {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Product/UpdateProduct',
                        data: $scope.Product,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        if (data == "OK") {

                            SweetAlert.swal({
                                title: $filter('translate')('pages.productManagement.productInformation'),
                                text: $filter('translate')('pages.productManagement.successfullySaved'),
                                confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            clearControls();

                        } else {
                            alert(data);
                        }

                        return false;
                    }).error(function (data, status, headers, config) {

                        SweetAlert.swal({
                            title: $filter('translate')('pages.productManagement.productInformation'),
                            text: $filter('translate')('pages.productManagement.Erroroccuredsavingdata'),
                            confirmButtonText: $filter('translate')('pages.currencyManagement.ok'),
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    });
                }
            }
        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            alert($filter('translate')('pages.currencyManagement.unableuploadyour'));
        };
        uploader1.onCancelItem = function (fileItem, response, status, headers) {
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


//supportive functions
var isGuid = function (stringToTest) {
    var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
    var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
    return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
};
var emptyGuid = function () {
    return "00000000-0000-0000-0000-000000000000";
};


