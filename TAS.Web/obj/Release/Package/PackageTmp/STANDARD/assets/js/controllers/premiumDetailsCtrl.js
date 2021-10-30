app.controller('PremiumDetailsCtrl',
    function ($scope, $http, $rootScope, $modal, $stateParams, $localStorage, $cookieStore, SweetAlert, toaster,
        ngDialog, $state, FileUploader, $timeout) {
        $scope.isMobileView = detectmob();

   
        $scope.tpaName = $localStorage.tpaName;

        var customErrorMessage = function (msg) {
            toaster.pop('error', '', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }
        function detectmob() {
            if (window.innerWidth <= 800) {
                return true;
            } else {
                return false;
            }
        }


        var tpaId;
        $scope.customerLoggedIn = false;

        $scope.loggedInCustomer = {
            picture: '',
            name: '',
            id: emptyGuid()
        }

        $scope.tempInvId = emptyGuid();

        $scope.MakeList = [];
        $scope.SelectedMakeList = [];
        $scope.SelectedMakeDList = [];

        $scope.ModelList = [];
        $scope.SelectedModelList = [];
        $scope.SelectedModelDList = [];

        $scope.ModelYearList = [];
        $scope.SelectedModelYearList = [];
        $scope.SelectedModelYearDList = [];

        //tire
        $scope.additionalPolicyDetailsMake = [];
        $scope.additionalPolicyDetailsModel = [];
        $scope.additionalPolicyDetailsModelYearList = [];
        $scope.additionalSelectedMakeList = [];
        $scope.additionalSelectedModelList = [];
        $scope.filterdSelectedModelList = [];
        $scope.additionalSelectedModelYearList = [];
        $scope.availableTireList = [];

        $scope.product = {
            invoiceCode: '',
            currencyCode: '',
            invoiceNo: '',
            makeId: '',
            modelId: '',
            commodityUsageType: 'Private',
            plateNo: '',
            addMakeId: $scope.additionalSelectedMakeList.id,
            addModelId: $scope.additionalSelectedModelList.id,
            addModelYear: $scope.additionalSelectedModelYearList.id,
            addMileage: '',
            invoiceAttachmentId: emptyGuid()
        };

        $scope.getPurchasedPrice = function (price) {
            if ($scope.product.Quantity == 4 && $scope.product.Position == "A") {
                $scope.availableTireList[1].price = price;
                $scope.availableTireList[2].price = price;
                $scope.availableTireList[3].price = price;
                $scope.availableTireList[4].price = price;
            } else if ($scope.product.Quantity == 2 && $scope.product.Position == "F") {
                $scope.availableTireList[1].price = price;
                $scope.availableTireList[2].price = price;
            } else if ($scope.product.Quantity == 2 && $scope.product.Position == "B") {
                $scope.availableTireList[1].price = price;
                $scope.availableTireList[2].price = price;
            }
            //else if ($scope.product.Quantity == 4 && $scope.product.Position == "F") {
            //    $scope.availableTireList[1].price = price;
            //    $scope.availableTireList[2].price = price;
            //} else if ($scope.product.Quantity == 4 && $scope.product.Position == "B") {
            //    $scope.availableTireList[3].price = price;
            //    $scope.availableTireList[4].price = price;
            //}
            

        }

        $scope.getDatabyInvoiceCode = function () {
            if ($scope.product.invoiceCode.length === 6) {
                swal({ title: "TAS Information", text: 'Reading Invoice Code...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetTireDetailsByInvoiceCode',
                    data: {
                        "invoiceCode": $scope.product.invoiceCode,
                        "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                    }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data.code === "SUCCESS") {
                        $scope.validate_invoiceCode = "";
                        $("#txt_invoiceNumber").focus();

                        $scope.product.plateNo = data.obj.PlateNumber;
                        $scope.product.makeId = data.obj.MakeId;
                        $scope.product.modelId = data.obj.ModelId;
                        $scope.product.currencyCode = data.obj.CurrencyCode;
                        $scope.product.commodityUsageType = 'Private';
                        $scope.product.Quantity = data.obj.Quantity;
                        $scope.product.Position = data.obj.Position;

                        $scope.availableTireList = [];
                        if (data.obj.Quantity == 2) {
                            if (data.obj.TireFront.Wide == 0) {
                                //so it is back tire
                                var articleNo = data.obj.TireBack.Wide + "/" + data.obj.TireBack.Cross + "R" +
                                    data.obj.TireBack.Diameter + " " + data.obj.TireBack.LoadSpeed;
                                var tireDetailLeft = {
                                    position: 'Back Left',
                                    price: '',
                                    dot: data.obj.TireBack.SerialLeft,
                                    article: articleNo,
                                    id: data.obj.TireBack.IdLeft
                                }
                                var tireDetailRight = {
                                    position: 'Back Right',
                                    price: '',
                                    dot: data.obj.TireBack.SerialRight,
                                    article: articleNo,
                                    id: data.obj.TireBack.IdRight
                                }
                                $scope.availableTireList.push(tireDetailLeft);
                                $scope.availableTireList.push(tireDetailRight);
                            } else {
                                //front
                                var articleNo = data.obj.TireFront.Wide + "/" + data.obj.TireFront.Cross + "R" +
                                    data.obj.TireFront.Diameter + " " + data.obj.TireFront.LoadSpeed;
                                var tireDetailLeft = {
                                    position: 'Front Left',
                                    price: '',
                                    dot: data.obj.TireFront.SerialLeft,
                                    article: articleNo,
                                    id: data.obj.TireFront.IdLeft
                                }
                                var tireDetailRight = {
                                    position: 'Front Right',
                                    price: '',
                                    dot: data.obj.TireFront.SerialRight,
                                    article: articleNo,
                                    id: data.obj.TireFront.IdRight
                                }
                                $scope.availableTireList.push(tireDetailLeft);
                                $scope.availableTireList.push(tireDetailRight);
                            }
                        } else {
                            var articleNoBack = data.obj.TireBack.Wide + "/" + data.obj.TireBack.Cross + "R" +
                                data.obj.TireBack.Diameter + " " + data.obj.TireBack.LoadSpeed;
                            var tireDetailBackLeft = {
                                position: 'Back Left',
                                price: '',
                                dot: data.obj.TireBack.SerialLeft,
                                article: articleNoBack,
                                id: data.obj.TireBack.IdLeft
                            }
                            var tireDetailBackRight = {
                                position: 'Back Right',
                                price: '',
                                dot: data.obj.TireBack.SerialRight,
                                article: articleNoBack,
                                id: data.obj.TireBack.IdRight
                            }
                            var articleNoFront = data.obj.TireFront.Wide + "/" + data.obj.TireFront.Cross + "R" +
                                data.obj.TireFront.Diameter + " " + data.obj.TireFront.LoadSpeed;
                            var tireDetailFrontLeft = {
                                position: 'Front Left',
                                price: '',
                                dot: data.obj.TireFront.SerialLeft,
                                article: articleNoFront,
                                id: data.obj.TireFront.IdLeft
                            }
                            var tireDetailFrontRight = {
                                position: 'Front Right',
                                price: '',
                                dot: data.obj.TireFront.SerialRight,
                                article: articleNoFront,
                                id: data.obj.TireFront.IdRight
                            }

                            $scope.availableTireList.push(tireDetailFrontLeft);
                            $scope.availableTireList.push(tireDetailFrontRight);
                            $scope.availableTireList.push(tireDetailBackLeft);
                            $scope.availableTireList.push(tireDetailBackRight);
                        }
                        var tireDetails = {
                            position: '',
                            price: '',
                            dot: '',
                            article: ''
                        }

                    } else {
                        customErrorMessage(data.msg);

                        $scope.product.plateNo = '';
                        $scope.product.makeId = emptyGuid();
                        $scope.product.modelId = emptyGuid();
                        $scope.availableTireList = [];

                        $scope.product.addMakeId = emptyGuid();
                        $scope.additionalSelectedMakeList = [];
                        $scope.product.addModelId = emptyGuid();
                        $scope.additionalSelectedModelList = [];
                        $scope.product.addModelYear = emptyGuid();
                        $scope.additionalSelectedModelYearList = [];
                        $scope.product.addMileage = '';
                        $scope.product.invoiceAttachmentId = emptyGuid();
                        $scope.product.invoiceNo = '';
                    }
                }).error(function (data, status, headers, config) {
                    swal.close();
                });

            } else
                $scope.validate_invoiceCode = "";

        }
        $scope.navigateToBuyingProcess = function () {
            if ($scope.availableTireList.length === 0) {
                customErrorMessage("No tire details found.");
                return;
            }

            if ($scope.customerInvoiceUploader.queue.length === 0) {
                if (!isGuid($scope.product.invoiceAttachmentId)) {
                    customErrorMessage("Please attach invoice to upload.");
                    return;
                }
            }


            if ($scope.validateInvoiceData()) {
                if ($scope.customerInvoiceUploader.queue.length !== 0) {
                    $scope.customerInvoiceUploader.uploadAll();
                } else {
                    $scope.savePolicyDetails();
                }

            } else
                customErrorMessage("Please fill all highlighted fields.");
        }

        $scope.validateInvoiceData = function () {
            var isValidated = true;

            if (typeof $scope.product.invoiceCode === 'undefined' || $scope.product.invoiceCode.length !== 6) {
                isValidated = false;
                $scope.validate_invoiceCode = "has-error";
            } else
                $scope.validate_invoiceCode = "";

            if (typeof $scope.product.invoiceNo === 'undefined' || $scope.product.invoiceNo.length === 0) {
                isValidated = false;
                $scope.validate_invoiceNo = "has-error";
            } else
                $scope.validate_invoiceNo = "";

            if (typeof $scope.product.makeId === 'undefined' || !isGuid($scope.product.makeId)) {
                isValidated = false;
                $scope.validate_makeId = "has-error";
            } else
                $scope.validate_makeId = "";

            if (typeof $scope.product.modelId === 'undefined' || !isGuid($scope.product.modelId)) {
                isValidated = false;
                $scope.validate_modelId = "has-error";
            } else
                $scope.validate_modelId = "";

            if (typeof $scope.product.commodityUsageType === 'undefined' || $scope.product.commodityUsageType.length === 0) {
                isValidated = false;
                $scope.validate_commodityUsageTypeId = "has-error";
            } else
                $scope.validate_commodityUsageTypeId = "";

            //tire price validation
            angular.forEach($scope.availableTireList, function (tire) {
                if (parseFloat(tire.price) && parseFloat(tire.price) > 0) {
                    tire.price = parseFloat(tire.price).toFixed(2);
                    tire.validate_price = "";
                } else {
                    isValidated = false;
                    tire.validate_price = "has-error";
                }
            });

            if (!isGuid($scope.additionalSelectedMakeList.id)) {
                $scope.validate_additional_make = "has-error";
                isValidated = false;
            } else
                $scope.validate_additional_make = "";

            if (!isGuid($scope.additionalSelectedModelList.id)) {
                $scope.validate_additional_model = "has-error";
                isValidated = false;
            } else
                $scope.validate_additional_model = "";

            //console.log($scope.additionalSelectedModelYearList);
            if (!parseInt($scope.additionalSelectedModelYearList.id)) {
                $scope.validate_additional_modelYear = "has-error";
                isValidated = false;
            } else
                $scope.validate_additional_modelYear = "";

            if (parseFloat($scope.product.addMileage) && parseFloat($scope.product.addMileage) > 0) {
                $scope.validate_additional_mileage = "";
            } else {
                $scope.validate_additional_mileage = "has-error";
                isValidated = false;
            }
            return isValidated;
        }
        $scope.loadInitailData = function () {
            tpaId = $cookieStore.get('tpaId');
            if (typeof tpaId === 'undefined')
                tpaId = $localStorage.tpaId;
            $scope.initiateUploader();
            $scope.actualMasterDataLoad();

            if (isGuid($scope.tempInvId)) {
                swal({ title: "TAS Information", text: 'Readiing Invoice Data...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/LoadSavedCustomerInvoiceData',
                    data: {
                        'tempInvoiceId': $scope.tempInvId,
                        "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                    }
                }).success(function (dataP, status, headers, config) {
                    if (dataP.InvoiceCode === null) {
                        customErrorMessage("Cannot load invoice entry details. Redirecting to new item entry.");
                        $state.go('home.homePremium', { prodId: $scope.prodId });
                        swal.close();
                    } else {
                        $scope.product.invoiceCode = dataP.InvoiceCode;

                        swal({ title: "TAS Information", text: 'Reading Invoice Code...', showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ProductDisplay/GetTireDetailsByInvoiceCode',
                            data: {
                                "invoiceCode": $scope.product.invoiceCode,
                                "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                            }
                        }).success(function (data, status, headers, config) {
                            swal.close();
                            if (data.code === "SUCCESS") {
                                $scope.validate_invoiceCode = "";
                                $("#txt_invoiceNumber").focus();

                                $scope.product.plateNo = data.obj.PlateNumber;
                                $scope.product.makeId = data.obj.MakeId;
                                $scope.product.modelId = data.obj.ModelId;
                                $scope.product.currencyCode = data.obj.CurrencyCode;
                                $scope.product.commodityUsageType = 'Private';

                                $scope.availableTireList = [];
                                if (data.obj.Quantity == 2) {
                                    if (data.obj.TireFront.Wide == 0) {
                                        //so it is back tire
                                        var articleNo = data.obj.TireBack.Wide + "/" + data.obj.TireBack.Cross + "R" +
                                            data.obj.TireBack.Diameter + " " + data.obj.TireBack.LoadSpeed;
                                        var tireDetailLeft = {
                                            position: 'Back Left',
                                            price: '',
                                            dot: data.obj.TireBack.SerialLeft,
                                            article: articleNo,
                                            id: data.obj.TireBack.IdLeft
                                        }
                                        var tireDetailRight = {
                                            position: 'Back Right',
                                            price: '',
                                            dot: data.obj.TireBack.SerialRight,
                                            article: articleNo,
                                            id: data.obj.TireBack.IdRight
                                        }
                                        $scope.availableTireList.push(tireDetailLeft);
                                        $scope.availableTireList.push(tireDetailRight);
                                    } else {
                                        //front
                                        var articleNo = data.obj.TireFront.Wide + "/" + data.obj.TireFront.Cross + "R" +
                                            data.obj.TireFront.Diameter + " " + data.obj.TireFront.LoadSpeed;
                                        var tireDetailLeft = {
                                            position: 'Front Left',
                                            price: '',
                                            dot: data.obj.TireFront.SerialLeft,
                                            article: articleNo,
                                            id: data.obj.TireFront.IdLeft
                                        }
                                        var tireDetailRight = {
                                            position: 'Front Right',
                                            price: '',
                                            dot: data.obj.TireFront.SerialRight,
                                            article: articleNo,
                                            id: data.obj.TireFront.IdRight
                                        }
                                        $scope.availableTireList.push(tireDetailLeft);
                                        $scope.availableTireList.push(tireDetailRight);
                                    }
                                } else {
                                    var articleNoBack = data.obj.TireBack.Wide + "/" + data.obj.TireBack.Cross + "R" +
                                        data.obj.TireBack.Diameter + " " + data.obj.TireBack.LoadSpeed;
                                    var tireDetailBackLeft = {
                                        position: 'Back Left',
                                        price: '',
                                        dot: data.obj.TireBack.SerialLeft,
                                        article: articleNoBack,
                                        id: data.obj.TireBack.IdLeft
                                    }
                                    var tireDetailBackRight = {
                                        position: 'Back Right',
                                        price: '',
                                        dot: data.obj.TireBack.SerialRight,
                                        article: articleNoBack,
                                        id: data.obj.TireBack.IdRight
                                    }
                                    var articleNoFront = data.obj.TireFront.Wide + "/" + data.obj.TireFront.Cross + "R" +
                                        data.obj.TireFront.Diameter + " " + data.obj.TireFront.LoadSpeed;
                                    var tireDetailFrontLeft = {
                                        position: 'Front Left',
                                        price: '',
                                        dot: data.obj.TireFront.SerialLeft,
                                        article: articleNoFront,
                                        id: data.obj.TireFront.IdLeft
                                    }
                                    var tireDetailFrontRight = {
                                        position: 'Front Right',
                                        price: '',
                                        dot: data.obj.TireFront.SerialRight,
                                        article: articleNoFront,
                                        id: data.obj.TireFront.IdRight
                                    }

                                    $scope.availableTireList.push(tireDetailFrontLeft);
                                    $scope.availableTireList.push(tireDetailFrontRight);
                                    $scope.availableTireList.push(tireDetailBackLeft);
                                    $scope.availableTireList.push(tireDetailBackRight);
                                }


                            } else {
                                customErrorMessage(data.msg);
                                $scope.product.plateNo = '';
                                $scope.product.makeId = emptyGuid();
                                $scope.product.modelId = emptyGuid();
                            }

                            //load data
                            $scope.product.invoiceNo = dataP.InvoiceNumber;
                            $scope.product.invoiceNo = dataP.InvoiceNumber;
                            $scope.product.invoiceAttachmentId = dataP.InvoiceAttachmentId;
                            $scope.product.commodityUsageType = dataP.UsageTypeCode;
                            $scope.product.addMakeId = dataP.AdditionalMakeId;
                            $scope.product.addModelId = dataP.AdditionalModelId;
                            $scope.product.addModelYear = dataP.AdditionalModelYearId;
                            $scope.product.addMileage = dataP.AdditionalMileage;


                            $scope.additionalSelectedMakeList.id = dataP.AdditionalMakeId;
                            $scope.selectedAditionalMakeChange();
                            $scope.additionalSelectedModelList.id = dataP.AdditionalModelId;
                            $scope.additionalSelectedModelYearList.id = dataP.AdditionalModelYearId;

                            angular.forEach($scope.availableTireList, function (tire) {
                                var innerLoopValid = true;
                                angular.forEach(dataP.availableTireList, function (tirep) {
                                    if (tire.id === tirep.id && innerLoopValid) {
                                        tire.price = tirep.price.toFixed(2);
                                        innerLoopValid = false;
                                    }
                                });
                            });

                        }).error(function (data, status, headers, config) {
                            swal.close();
                        });
                    }
                }).error(function (data, status, headers, config) {
                    swal.close();
                });
            } else
                $scope.tempInvId = emptyGuid();
        }
        $scope.extPolicyRegAttachmentType = emptyGuid();

        $scope.initiateUploader = function () {
            //initialize uploaders
            $scope.customerInvoiceUploader = new FileUploader({
                url: '/TAS.Web/api/Upload/UploadAttachmentExternal',
                headers: {
                    'Page': 'ExtPolicyReg',
                    'Section': 'Default',
                    'TpaId': tpaId
                }
            });
            $scope.customerInvoiceUploader.onProgressAll = function () {
                swal({ title: 'Uploading invoice...', text: '', showConfirmButton: false });
            }
            $scope.customerInvoiceUploader.onSuccessItem = function (item, response, status, headers) {
                if (response != 'Failed') {
                    $scope.product.invoiceAttachmentId = response.replace(/['"]+/g, '');
                } else {
                    customErrorMessage("Error occured while uploading attachments.");
                    $scope.customerInvoiceUploader.cancelAll();
                }
            }
            $scope.customerInvoiceUploader.onCompleteAll = function () {
                $scope.customerInvoiceUploader.queue = [];
                $scope.savePolicyDetails(); swal.close();
            }
            $scope.customerInvoiceUploader.filters.push({
                name: 'customFilter',
                fn: function (item, options) {
                    if (item.size > 5000000) {
                        customErrorMessage('Max document size is 5MB');
                        return false;
                    } else {
                        return true;
                    }
                }
            });

        }
        $scope.navigateToProductsPage = function () {
            $state.go('home.products', { tpaId: $localStorage.tpaName });
        }

        $scope.validateInvoiceCodeLength = function () {
            if (typeof $scope.product.invoiceCode !== 'undefined' && $scope.product.invoiceCode.length === 6) {
                $scope.validate_invoiceCode = "";
            } else {
                $scope.validate_invoiceCode = "has-error";
                customErrorMessage("Invoice code length should be six characters.");
            }
        }
        $scope.savePolicyDetails = function () {
            $scope.product.addMakeId = $scope.additionalSelectedMakeList.id;
            $scope.product.addModelId = $scope.additionalSelectedModelList.id;
            $scope.product.addModelYear = $scope.additionalSelectedModelYearList.id;

            var data = {
                product: $scope.product,
                tpaId: tpaId,
                availableTireList: $scope.availableTireList,
                customerId: $scope.loggedInCustomer.id,
                tempInvId: $scope.tempInvId
            };

            swal({ title: "TAS Information", text: 'Saving Invoice Code...', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/SaveCustomerInvoiceDetails',
                data: data
            }).success(function (data, status, headers, config) {

                if (data.code === "SUCCESS") {
                    swal({
                        title: "TAS Information",
                        text: 'Saved successfully. Redirecting next page...',
                        showConfirmButton: false
                    });
                    //redirect
                    $timeout(function () {
                        $state.go('home.buyingprocess', { tempInvId: data.msg });
                        swal.close();
                    }, 2000);

                } else {
                    swal.close();
                    customErrorMessage(data.msg);
                }
            }).error(function (data, status, headers, config) {
                swal.close();
            });
        }
        $scope.$watch('product.invoiceCode', function (newVal, oldVal) {
            if (typeof newVal !== 'undefined') {
                if (newVal.length > 6) {
                    $scope.product.invoiceCode = oldVal;
                }
            }
        });
        var toShortFormat = function (date) {

            var month_names = ["Jan", "Feb", "Mar",
                "Apr", "May", "Jun",
                "Jul", "Aug", "Sep",
                "Oct", "Nov", "Dec"];

            var day = date.getDate();
            var month_index = date.getMonth();
            var year = date.getFullYear();

            return "" + day + "-" + month_names[month_index] + "-" + year;
        }
        var today = new Date();

        $scope.setPurcheaseDate = function () {
            $scope.product.itemPurchasedDate = toShortFormat(today);
        }
        var PopupaddToGrid;
        $scope.addToGridPopup = function () {

            PopupaddToGrid = ngDialog.open({
                template: 'PopAddToGrid',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }

        $scope.BAndW = {
            Id: "00000000-0000-0000-0000-000000000000",
            ItemPurchasedDate: '',
            MakeId: "00000000-0000-0000-0000-000000000000",
            ModelId: "00000000-0000-0000-0000-000000000000",
            SerialNo: '',
            ItemPrice: 0.0,
            CategoryId: "00000000-0000-0000-0000-000000000000",
            ModelYear: '',
            AddnSerialNo: '',
            ItemStatusId: "00000000-0000-0000-0000-000000000000",
            InvoiceNo: '',
            ModelCode: '',
            DealerPrice: 0.0,
            commodityUsageTypeId: "00000000-0000-0000-0000-000000000000"
        };
        $scope.Vehicle = {
            Id: "00000000-0000-0000-0000-000000000000",
            VINNo: '',
            MakeId: "00000000-0000-0000-0000-000000000000",
            ModelId: "00000000-0000-0000-0000-000000000000",
            CategoryId: "00000000-0000-0000-0000-000000000000",
            ItemStatusId: "00000000-0000-0000-0000-000000000000",
            CylinderCountId: "00000000-0000-0000-0000-000000000000",
            BodyTypeId: "00000000-0000-0000-0000-000000000000",
            PlateNo: '',
            ModelYear: '',
            FuelTypeId: "00000000-0000-0000-0000-000000000000",
            AspirationId: "00000000-0000-0000-0000-000000000000",
            Variant: "00000000-0000-0000-0000-000000000000",
            TransmissionId: "00000000-0000-0000-0000-000000000000",
            ItemPurchasedDate: '',
            EngineCapacityId: "00000000-0000-0000-0000-000000000000",
            DriveTypeId: "00000000-0000-0000-0000-000000000000",
            VehiclePrice: 0.0,
            DealerPrice: 0.0,
            VinLength: 0,
            commodityUsageTypeId: "00000000-0000-0000-0000-000000000000"
        };

        $scope.Item = {
            DealerId: "00000000-0000-0000-0000-000000000000",
            DealerLocationId: "00000000-0000-0000-0000-000000000000",
            CountryId: "00000000-0000-0000-0000-000000000000",
            ExtensionTypeId: "00000000-0000-0000-0000-000000000000"
        };

        $scope.prodId = $stateParams.prodId;
        $localStorage.prodId = $stateParams.prodId;
        $scope.tempInvId = $stateParams.tempInvId;

        $scope.premiumDetails = {
            Age: "",
            Gender: "",
            DrivingExperiance: "",
            ContryId: "",
        };

        $scope.actualMasterDataLoad = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetProductByProdId',
                data: { "Id": $scope.prodId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Product = data;
                $cookieStore.put('CommodityType', $scope.Product.CommodityType);
                $cookieStore.put('ProductID', $scope.prodId);
                if ($scope.Product.CommodityType == 'Electronic') {

                    $scope.vwBAndWDetails = true;
                    $scope.vwVehicleDetails = false;
                    loadBandWData();
                } else if ($scope.Product.CommodityType == 'Automobile') {
                    $scope.vwVehicleDetails = true;
                    $scope.vwBAndWDetails = false;
                    loadVehicleData();
                }
                else if ($scope.Product.CommodityType == 'Automotive') {
                    $scope.vwVehicleDetails = true;
                    $scope.vwBAndWDetails = false;
                    loadVehicleData();
                }
                else if ($scope.Product.CommodityType == 'Yellow Goods') {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = false;
                    $scope.vwYellowGoodsDetails = true;
                    LoadYellowGoodData();
                } else if ($scope.Product.CommodityType == 'Other' && $scope.Product.Productcode != 'TYRE') {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = false;
                    $scope.vwYellowGoodsDetails = false;
                    $scope.vwOtherDetails = true;
                    $scope.vwTireDetails = false;

                    LoadOtherData();

                } else if ($scope.Product.Productcode == 'TYRE') {
                    $scope.vwVehicleDetails = false;
                    $scope.vwBAndWDetails = false;
                    $scope.vwYellowGoodsDetails = false;
                    $scope.vwTireDetails = true;
                    $scope.vwOtherDetails = false;

                    LoadOtherData();

                }
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCountriesThatHaveDealers',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.countries = data.Countries;
                if (data.Countries.length === 1) {
                    $scope.isCountryReadonly = true;
                    $scope.Item.CountryId = $scope.countries[0].Id;
                    $scope.SetCountryValue();
                } else
                    $scope.isCountryReadonly = false;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                data: { "ImageId": $cookieStore.get('ImageId'), "tpaId": $cookieStore.get('tpaId'), "test": 0 }
            }).success(function (data, status, headers, config) {
                $scope.TpaLogoSrc = data;


            }).error(function (data, status, headers, config) {
                //clearControls();
                //$scope.message = 'Unexpected Error';
            });
        }
        function LoadOtherData() {
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/Customer/GetAllCustomers',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.Customers = data;
            //}).error(function (data, status, headers, config) {
            //});
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCategories',
                data: {
                    "Id": $scope.Product.CommodityTypeId,
                    "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
                if (data.length === 1) {
                    $scope.product.categoryId = $scope.commodityCategories[0].CommodityCategoryId;
                    $scope.selectedCommodityCategoryChanged();
                }
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllMakesByComodityTypeId',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data.Makes;
                AddMake();
            }).error(function (data, status, headers, config) {
            });
            AddModelYear();
            //load other commodity data
            if ($scope.vwTireDetails) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetAllAdditionalFieldDetails',
                    data: {
                        "productCode": "tyre",
                        "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                    }
                }).success(function (data, status, headers, config) {
                    if (data != null) {
                        var data_j = JSON.parse(data);
                        console.log(data_j);
                        // make
                        for (var i = 0; i < data_j[0].Value.length; i++) {
                            var make_d = {
                                id: data_j[0].Value[i].Id,
                                label: data_j[0].Value[i].MakeName
                            }
                            $scope.additionalPolicyDetailsMake.push(make_d);
                        }

                        // model
                        for (var i = 0; i < data_j[1].Value.length; i++) {
                            var model_d = {
                                id: data_j[1].Value[i].Id,
                                label: data_j[1].Value[i].ModelName,
                                makeId: data_j[1].Value[i].MakeId
                            }
                            $scope.additionalPolicyDetailsModel.push(model_d);
                        }

                        // model year
                        for (var i = 0; i < data_j[2].Value.length; i++) {
                            var model_year = {
                                id: data_j[2].Value[i],
                                label: data_j[2].Value[i]
                            }
                            $scope.additionalPolicyDetailsModelYearList.push(model_year);
                        }
                    }
                }).error(function (data, status, headers, config) {
                });


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetDocumentTypesByPageName',
                    dataType: 'json',
                    data: {
                        PageName: 'ExtPolicyReg',
                        "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId
                    },

                }).success(function (data, status, headers, config) {
                    for (var i = 0; i < data.length; i++) {
                        if (data[i].AttachmentSectionCode == "Default") {
                            $scope.extPolicyRegAttachmentType = data[i].Id;
                        }
                    }
                }).error(function (data, status, headers, config) {
                });
            }
        }
        function LoadYellowGoodData() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCategories',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllMakesByComodityTypeId',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data.Makes;
            }).error(function (data, status, headers, config) {
            });

            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityUsageTypes',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.commodityUsageTypes = data;
            //}).error(function (data, status, headers, config) {
            //});

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllItemStatuss',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });
        }

        function loadBandWData() {


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCategories',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllMakesByComodityTypeId',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data.Makes;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllItemStatuss',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.itemStatuses = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.selectedAditionalMakeChange = function () {
            $scope.filterdSelectedModelList = [];
            if (isGuid($scope.additionalSelectedMakeList.id)) {
                for (var i = 0; i < $scope.additionalPolicyDetailsModel.length; i++) {
                    if ($scope.additionalPolicyDetailsModel[i].makeId === $scope.additionalSelectedMakeList.id) {
                        var model_d = {
                            id: $scope.additionalPolicyDetailsModel[i].id,
                            label: $scope.additionalPolicyDetailsModel[i].label,
                            makeId: $scope.additionalPolicyDetailsModel[i].makeId
                        }
                        $scope.filterdSelectedModelList.push(model_d);
                    }
                }
            }

            angular.forEach($scope.additionalPolicyDetailsMake , function (value){
                if (value.id === $scope.additionalSelectedMakeList.id) {
                    $scope.CustomText.buttonDefaultText = value.label;
                        
                }
            
            });
           
            
        }


        $scope.SetModelB = function () {
            $scope.errorTab1 = "";
            if ($scope.BAndW.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetModelesByMakeId',
                    data: { "Id": $scope.BAndW.MakeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.Modeles = data.Modeles;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.SetVINLength = function () {

            $scope.Vehicle.VinLength = 0;
            if ($scope.Vehicle.CategoryId != null) {
                angular.forEach($scope.Categories, function (value) {
                    if ($scope.Vehicle.CategoryId == value.CommodityCategoryId) {
                        $scope.Vehicle.VinLength = value.Length;
                    }
                });
            }
        }

        function loadVehicleData() {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllItemStatuss',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.ItemStatuss = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCylinderCounts',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.CylinderCounts = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllVehicleBodyTypes',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.BodyTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllCategories',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.commodityCategories = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllFuelTypes',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.FuelTypes = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllVehicleAspirationTypes',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Aspirations = data;
            }).error(function (data, status, headers, config) {
            });



            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllVariant',
                data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Variants = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllTransmissionTypes',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Transmissions = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllEngineCapacities',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.EngineCapacities = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllDriveTypes',
                data: { "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.DriveTypes = data;
            }).error(function (data, status, headers, config) {
            });
        }


        $scope.SetModelV = function () {
            $scope.errorTab1 = "";
            if ($scope.Vehicle.MakeId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetModelesByMakeId',
                    data: { "Id": $scope.Vehicle.MakeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.Modeles = data.Modeles;
                    //$scope.loadContractExtension();
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.SetCountryValue = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ProductDisplay/GetAllDealersByCountryId',
                data: { "Id": $scope.Item.CountryId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data.Dealers;
                if (data.Dealers.length === 1) {
                    $scope.isDealerReadonly = true;
                    $scope.Item.DealerId = $scope.Dealers[0].Id;
                    $scope.SetDealerLocationsValues();
                } else {
                    $scope.isDealerReadonly = false;
                }

                $scope.Dealers.push({ "Id": "65ead0cb-ad86-489e-915d-523292e0f9d9", "DealerCode": "Dealer 1", "DealerName": "Dealer 1", "DealerAliase": "", "CountryId": "6095475a-6347-403b-93b4-6213cdcbb8cc", "CurrencyId": "01f71140-d78e-4b31-a1c6-93f4c7953dbd", "Type": "true", "CommodityTypeId": "b98d3bff-f88a-4d2e-ac35-d99a53363c4a", "InsurerId": "bfdff8d1-e95d-4f89-a249-5dd9ef2b5032", "Makes": ["2371060f-7386-4b98-bc50-fd329288855d"], "CityId": "00000000-0000-0000-0000-000000000000", "Location": "", "IsActive": true, "IsAutoApproval": false, "EntryDateTime": "2018-08-06T00:00:00", "EntryUser": "ba56ec84-1fe0-4385-abe4-182f62caa050", "IsDealerExists": false, "ManHourRate": 0.0 });
                $scope.Dealers.push({ "Id": "65ead0cb-ad86-489e-915d-523213e0f9d9", "DealerCode": "Dealer 2", "DealerName": "Dealer 2", "DealerAliase": "", "CountryId": "6095475a-6347-403b-93b4-6213cdcbb8cc", "CurrencyId": "01f71140-d78e-4b31-a1c6-93f4c7953dbd", "Type": "true", "CommodityTypeId": "b98d3bff-f88a-4d2e-ac35-d99a53363c4a", "InsurerId": "bfdff8d1-e95d-4f89-a249-5dd9ef2b5032", "Makes": ["2371060f-7386-4b98-bc50-fd329288855d"], "CityId": "00000000-0000-0000-0000-000000000000", "Location": "", "IsActive": true, "IsAutoApproval": false, "EntryDateTime": "2018-08-06T00:00:00", "EntryUser": "ba56ec84-1fe0-4385-abe4-182f62caa050", "IsDealerExists": false, "ManHourRate": 0.0 });

            }).error(function (data, status, headers, config) {
            });
        }

        $scope.SetDealerLocationsValues = function () {
            if ($scope.Item.DealerId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetAllDealerLocationsByDealerId',
                    data: { "Id": $scope.Item.DealerId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.DealerLocations = data.DealerLocations;
                    if (data.DealerLocations.length === 1) {
                        $scope.Item.DealerLocationId = $scope.DealerLocations[0].Id;
                    }
                }).error(function (data, status, headers, config) {
                });

                $scope.getExtensionTypeValues();
            }
        }


        $scope.getExtensionTypeValues = function () {
            if ($scope.Item.DealerId != null && (($scope.Vehicle.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.ModelId != "") || ($scope.BAndW.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.BAndW.ModelId != ""))) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetExtensionTypesByDealerId',
                    data: { "dealerId": $scope.Item.DealerId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId, "modelId": $scope.Vehicle.ModelId == "00000000-0000-0000-0000-000000000000" ? $scope.BAndW.ModelId : $scope.Vehicle.ModelId }
                }).success(function (data, status, headers, config) {
                    $scope.ExtensionTypes = data.ExtensionTypes;
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.ExtensionTypes = null;
            }
        }

        $scope.validatePrimiumDetails = function () {
            var isValid = true;

            if ($scope.Item.CountryId == "" || $scope.Item.CountryId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CountryId = "";
            }

            if ($scope.Item.DealerId == "" || $scope.Item.DealerId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerId = "";
            }

            if ($scope.Item.DealerLocationId == "" || $scope.Item.DealerLocationId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLocationId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLocationId = "";
            }

            if ($scope.Item.DealerLocationId == "" || $scope.Item.DealerLocationId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_DealerLocationId = "has-error";
                isValid = false;
            } else {
                $scope.validate_DealerLocationId = "";
            }

            if ($scope.Product.CommodityType == 'Automobile' || $scope.Product.CommodityType == 'Automotive') {

                if ($scope.Vehicle.CategoryId == "" || $scope.Vehicle.CategoryId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleCategoryId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleCategoryId = "";
                }

                if ($scope.Vehicle.ItemStatusId == "" || $scope.Vehicle.ItemStatusId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleItemStatusId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleItemStatusId = "";
                }

                if ($scope.Vehicle.CylinderCountId == "" || $scope.Vehicle.CylinderCountId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleCylinderCountId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleCylinderCountId = "";
                }

                if ($scope.Vehicle.VehiclePrice == 0 || $scope.Vehicle.VehiclePrice == undefined) {
                    $scope.validate_VehicleVehiclePrice = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleVehiclePrice = "";
                }

                if ($scope.Vehicle.VINNo == "" || $scope.Vehicle.VINNo == undefined) {
                    $scope.validate_VehicleVINNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleVINNo = "";
                }

                if ($scope.Vehicle.ModelYear == "" || $scope.Vehicle.ModelYear == undefined) {
                    $scope.validate_VehicleModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleModelYear = "";
                }

                if ($scope.Item.Usage == "" || $scope.Item.Usage == undefined) {
                    $scope.validate_VehicleUsage = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleUsage = "";
                }

                if ($scope.Vehicle.MakeId == "" || $scope.Vehicle.MakeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleMakeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleMakeId = "";
                }

                if ($scope.Vehicle.ItemPurchasedDate == "" || $scope.Vehicle.ItemPurchasedDate == undefined) {
                    $scope.validate_VehicleItemPurchasedDate = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleItemPurchasedDate = "";
                }

                if ($scope.Vehicle.ModelId == "" || $scope.Vehicle.ModelId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleModelId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleModelId = "";
                }

                if ($scope.Vehicle.EngineCapacityId == "" || $scope.Vehicle.EngineCapacityId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_VehicleEngineCapacityId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehicleEngineCapacityId = "";
                }

                if ($scope.Vehicle.PlateNo == "" || $scope.Vehicle.PlateNo == undefined) {
                    $scope.validate_VehiclePlateNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_VehiclePlateNo = "";
                }


            } else if ($scope.Product.CommodityType == 'Electronic') {

                if ($scope.BAndW.CategoryId == "" || $scope.BAndW.CategoryId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_BAndWCategoryId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWCategoryId = "";
                }

                if ($scope.BAndW.ModelYear == "" || $scope.BAndW.ModelYear == undefined) {
                    $scope.validate_BAndWModelYear = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWModelYear = "";
                }

                if ($scope.BAndW.ItemPrice == 0 || $scope.BAndW.ItemPrice == undefined) {
                    $scope.validate_BAndWItemPrice = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWItemPrice = "";
                }

                if ($scope.BAndW.SerialNo == 0 || $scope.BAndW.SerialNo == undefined) {
                    $scope.validate_BAndWSerialNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWSerialNo = "";
                }

                if ($scope.BAndW.InvoiceNo == "" || $scope.BAndW.InvoiceNo == undefined) {
                    $scope.validate_BAndWInvoiceNo = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWInvoiceNo = "";
                }

                if ($scope.BAndW.ItemPurchasedDate == "" || $scope.BAndW.ItemPurchasedDate == undefined) {
                    $scope.validate_BAndWItemPurchasedDate = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWItemPurchasedDate = "";
                }

                if ($scope.BAndW.MakeId == "" || $scope.BAndW.MakeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_BAndWMakeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWMakeId = "";
                }

                if ($scope.BAndW.ModelId == "" || $scope.BAndW.ModelId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_BAndWModelId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWModelId = "";
                }

                if ($scope.BAndW.ItemStatusId == "" || $scope.BAndW.ItemStatusId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_BAndWItemStatusId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWItemStatusId = "";
                }

                if ($scope.Item.Usage == "" || $scope.Item.Usage == undefined) {
                    $scope.validate_Usage = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_Usage = "";
                }



                if ($scope.BAndW.commodityUsageTypeId == "" || $scope.BAndW.commodityUsageTypeId == "00000000-0000-0000-0000-000000000000") {
                    $scope.validate_commodityUsageTypeId = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_commodityUsageTypeId = "";
                }

                if ($scope.BAndW.DealerPrice == 0 || $scope.BAndW.DealerPrice == undefined) {
                    $scope.validate_BAndWDealerPrice = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_BAndWDealerPrice = "";
                }

            }



            return isValid
        }

        $scope.open = function (size) {
            //$scope.validatePrimiumDetails()
            if (true) {
                $scope.Vehicle.DealerPrice = $scope.Vehicle.VehiclePrice;
                $scope.BAndW.DealerPrice = $scope.BAndW.ItemPrice;

                if ($scope.Item.DealerId != undefined && $scope.Item.DealerId != null && $scope.Item.DealerId != "00000000-0000-0000-0000-000000000000" && (($scope.Vehicle.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.Vehicle.ModelId != "") || ($scope.BAndW.ModelId != "00000000-0000-0000-0000-000000000000" && $scope.BAndW.ModelId != "")) &&  // $scope.Item.ExtensionTypeId != "00000000-0000-0000-0000-000000000000" && $scope.Item.ExtensionTypeId != "" &&
                    (($scope.BAndW.DealerPrice != 0 && $scope.BAndW.DealerPrice != '') && ($scope.BAndW.ItemPrice != 0 && $scope.BAndW.ItemPrice != '')) || (($scope.Vehicle.DealerPrice != 0 && $scope.Vehicle.DealerPrice != '') && ($scope.Vehicle.VehiclePrice != 0 && $scope.Vehicle.VehiclePrice != ''))) {
                    swal({ title: "TAS Information", text: "Requesting information..", showConfirmButton: false });

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ProductDisplay/GetPrices',
                        data: { "dealerId": $scope.Item.DealerId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId, "modelId": $scope.Vehicle.ModelId == "00000000-0000-0000-0000-000000000000" ? $scope.BAndW.ModelId : $scope.Vehicle.ModelId, 'dealerPrice': $scope.BAndW.DealerPrice == 0 ? $scope.Vehicle.DealerPrice : $scope.BAndW.DealerPrice, 'itemPrice': $scope.Vehicle.VehiclePrice ? $scope.BAndW.ItemPrice : $scope.Vehicle.VehiclePrice } // , 'extensionTypeId': $scope.Item.ExtensionTypeId
                    }).success(function (data, status, headers, config) {
                        $scope.ContractPrices = data.ContractPrices;
                        $localStorage.ContractPrices = $scope.ContractPrices;

                        $localStorage.tmpVehicle = $scope.Vehicle;
                        $cookieStore.put('tmpVehicle', $scope.Vehicle);

                        $localStorage.tmpBAndW = $scope.BAndW;
                        $cookieStore.put('tmpBAndW', $scope.BAndW);

                        $localStorage.tmpItem = $scope.Item;
                        $cookieStore.put('tmpItem', $scope.Item);

                        $scope.tmpProduct = {
                            Id: $scope.Product.Id,
                            CommodityTypeId: $scope.Product.CommodityTypeId,
                            CommodityType: $scope.Product.CommodityType,
                            Productname: $scope.Product.Productname,
                            Productcode: $scope.Product.Productcode
                        };

                        $localStorage.tmpProduct = $scope.tmpProduct;
                        $cookieStore.put('tmpProduct', $scope.tmpProduct);

                        $localStorage.tmpTpaId = $scope.tpaId;
                        $cookieStore.put('tmpTpaId', $scope.tpaId);

                        if (data.ContractPrices.length > 0) {
                            $scope.items = ['item1', 'item2', 'item3'];
                            var modalScope = $rootScope.$new();
                            modalScope.modalInstance = $modal.open({
                                templateUrl: 'myModalContent.html',
                                controller: 'ModelCtrl',
                                size: size,
                                scope: modalScope

                            });
                        } else {
                            SweetAlert.swal({
                                title: "Policy Information",
                                text: "No matching premium plans to display!",
                                confirmButtonColor: "#007AFF"
                            });
                        }
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {
                        swal.close();
                    });
                } else {
                    $scope.ExtensionTypes = null;
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }


        };

        //new methods added by Ranga
        $scope.product = {};


        $scope.selectedCommodityCategoryChanged = function () {

            $scope.makes = [];
            $scope.product.serialNumber = "";
            $scope.isProductDetailsReadonly = false;
            if (isGuid($scope.product.categoryId)) {

                angular.forEach($scope.commodityCategories, function (value) {

                    if ($scope.product.categoryId == value.CommodityCategoryId) {
                        $scope.serialNumberLength = value.Length;
                        $scope.selectedItemCategory = value.CommodityCategoryDescription;
                        return false;
                    }
                });

                //$http({
                //    method: 'POST',
                //    url: '/TAS.Web/api/MakeAndModelManagement/GetMakesByCommodityCategoryId',
                //    data: { "Id": $scope.product.categoryId },
                //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                //}).success(function (data, status, headers, config) {
                //    $scope.makes = data;
                //}).error(function (data, status, headers, config) {
                //});

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetAllMakesByComodityTypeId',
                    data: { "Id": $scope.Product.CommodityTypeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.makes = data.Makes;
                    if (data.Makes.length === 1) {
                        $scope.product.makeId = $scope.makes[0].Id;
                        $scope.selectedMakeChanged();
                    }
                }).error(function (data, status, headers, config) {
                });

            } else {
                $scope.serialNumberLength = '';
            }
        }

        $scope.selectedMakeChanged = function () {
            if (isGuid($scope.product.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetModelesByMakeId',
                    data: { "Id": $scope.product.makeId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.models = data.Modeles;
                    if (data.Modeles.length === 1) {
                        $scope.product.modelId = $scope.models[0].Id;
                        $scope.selectedModelChange();
                    }

                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.models = [];
            }
        }

        $scope.selectedModelChange = function () {
            if (isGuid($scope.product.modelId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ProductDisplay/GetVariantsByModelId',
                    data: { "Id": $scope.product.modelId, "tpaId": $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId }
                }).success(function (data, status, headers, config) {
                    $scope.variants = data;
                    if (data.length === 1) {
                        $scope.product.variantId = $scope.variants[0].Id;
                    } else
                        $scope.product.variantId = emptyGuid();
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.variants = [];
            }
        }

        $scope.scannerPopUp = function () {

            ScannerPopUp = ngDialog.open({
                template: 'imageScannerPopUp',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        }

       

        //  search drop down 

        $scope.settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false,
            closeOnSelect: true,
            selectionLimit: 1,
            buttonClasses: "drop-btn btn btn-default",
            smartButtonMaxItems:1,
            smartButtonTextConverter: function (itemText, originalItem)
            {
                return itemText;
            }
        };
        $scope.CustomText = {
            buttonDefaultText: ' Please Select ',
            dynamicButtonTextSuffix: ' Item(s) Selected'
            //smartButtonTextProvider: ''
            //dynamicButtonTextSuffix: ' Item(s) Selected'
            //dynamicButtonTextSuffix : { smartButtonTextProvider(selectionArray) { return selectionArray.lenth },
        };
        $scope.Modeles = [];

        // Make drop down

        function AddMake() {
            var index = 0;
            $scope.MakeList = [];
            angular.forEach($scope.MakesVehi, function (value) {
                var x = { id: index, code: value.Id, label: value.MakeName };
                $scope.MakeList.push(x);
                index = index + 1;
            });
        }
        function LoadMake() {
            $scope.SelectedMakeList = [];
            $scope.SelectedMakeDList = [];
            angular.forEach($scope.product.Makes, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedMakeList.push(x);
                        $scope.SelectedMakeDList.push(valueIn.label);
                    }
                });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    data: { "Id": valueOut },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var temp = data;
                    angular.forEach(temp, function (value) {
                        $scope.Modeles.push(value);
                        AddModel();
                        LoadModel();
                    });
                }).error(function (data, status, headers, config) {
                });
            });
        }
        $scope.SendMake = function () {
            $scope.Modeles = [];
            $scope.SelectedMakeDList = [];
            $scope.Country.Makes = [];
            angular.forEach($scope.SelectedMakeList, function (valueOut) {
                angular.forEach($scope.MakeList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Country.Makes.push(valueIn.code);
                        $scope.SelectedMakeDList.push(valueIn.label);
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                            data: { "Id": valueIn.code },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var temp = data;
                            angular.forEach(temp, function (value) {
                                $scope.Modeles.push(value);
                                AddModel();
                            });
                        }).error(function (data, status, headers, config) {
                        });
                    }
                });
            });
        }

        // --- Model Dropdown

        function AddModel() {
            var index = 0;
            $scope.ModelList = [];
            angular.forEach($scope.Modeles, function (value) {
                var x = { id: index, code: value.Id, label: value.ModelName };
                $scope.ModelList.push(x);
                index = index + 1;
            });
        }
        function LoadModel() {
            $scope.SelectedModelList = [];
            $scope.SelectedModelDList = [];
            angular.forEach($scope.Country.Modeles, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedModelList.push(x);
                        $scope.SelectedModelDList.push(valueIn.label);
                    }
                });
            });
        }
        $scope.SendModel = function () {
            $scope.SelectedModelDList = [];
            $scope.Country.Modeles = [];
            angular.forEach($scope.SelectedModelList, function (valueOut) {
                angular.forEach($scope.ModelList, function (valueIn) {
                    if (valueOut.id == valueIn.id) {
                        $scope.Country.Modeles.push(valueIn.code);
                        $scope.SelectedModelDList.push(valueIn.label);
                    }
                });
            });
        }

        // --- Model year dropdown

        function AddModelYear() {
            var index = 0;
            $scope.ModelYearList = [];
            angular.forEach($scope.ModelYear, function (value) {
                var x = { id: index, code: value.Id, label: value.ModelYear };
                $scope.ModelYearList.push(x);
                index = index + 1;
            });
        }

        // --------------------- Customer Login --------------------------

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

        $scope.loginAttempt = function () {

            if ($scope.user.userName == undefined || $scope.user.password == undefined || $scope.user.userName.length == 0 || $scope.user.password.length == 0) {
                $scope.Errormsg = "Please enter login credentials";
                $scope.err = 1;
            } else {
                $scope.loginBtnDisabled = true;
                $scope.loginIconBtnClass = "fa fa-spinner fa-spin";

                var data = JSON.stringify({ 'UserName': $scope.user.userName, 'Password': $scope.user.password, 'tpaID': $localStorage.tpaID, "tpaName": $scope.tpaName });
                var request = $http({
                    method: "post",
                    url: "/TAS.Web/api/BuyingWizard/CustomerLoginAuth",
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt }
                })
                request.success(function (response) {
                    if (response == "Invalid") {
                        $scope.Errormsg = "Invalid Username Or Password";
                        $scope.err = 1;
                        $scope.loginBtnDisabled = false;
                        $scope.loginIconBtnClass = "fa fa-arrow-circle-right";
                    } else if (response == "Error") {
                        $scope.Errormsg = "System Error";
                        $scope.err = 1;
                        $scope.loginBtnDisabled = false;
                        $scope.loginIconBtnClass = "fa fa-arrow-circle-right";
                    } else {

                        $localStorage.LoggedInCustomerId = response.LoggedInCustomerId;
                        $scope.loggedInCustomer.id = response.LoggedInCustomerId;
                        $scope.loggedInCustomer.name = response.LoggedInCustomerName;
                        $scope.customerLoggedIn = true;
                        // $state.go('home.customerPro', { tpaId: $localStorage.tpaName, customerId: $localStorage.LoggedInCustomerId });
                        //$scope.viewEmailSentAndSuccessMsg();
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

    });

function emptyGuid() {
    return "00000000-0000-0000-0000-000000000000";
}

app.controller('ModelCtrl',
    function ($scope, $localStorage, $cookieStore, $location) {
        $scope.ContractPrices = $localStorage.ContractPrices;
        var tpaId = $cookieStore.get('tpaId');
        $scope.tpaID = $localStorage.tpaId == undefined ? tpaId : $localStorage.tpaId

        $scope.signin = function (contractId, price, dealName, coverType, extensionName, extensionID) {
            $cookieStore.put('contractId', contractId);
            $cookieStore.put('displayPrice', price);
            $cookieStore.put('dealName', dealName);
            $cookieStore.put('coverType', coverType);
            $cookieStore.put('extensionType', extensionName);
            $cookieStore.put('extensionTypeId', extensionID);
            $location.path("home/login/" + $scope.tpaID);
            $scope.modalInstance.close();
        }

    });