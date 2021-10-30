app.controller('ClaimSubmissionCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter,
        toaster, $state, ngTableParams, $stateParams, FileUploader, $parse, $location, $translate) {


        var SearchPolicyPopup, PartSuggestionPopup, PartsSuggestionForLabourCost,
            MultipleEligiblePolicySelection, AddNewPartPopup;
        $scope.claimId = $rootScope.claimId;
        if ($scope.claimId == undefined || $scope.claimId == null) {
            if ($scope.isEditClaim) {
                $scope.claimId = $localStorage.claimSubmissionTempId;
            }
        }

        $scope.required = {
            lastServiceDate: true,
            lastServiceMileage: true
        }

        $scope.product = {
            Id : '00000000-0000-0000-0000-000000000000'
        }

        $scope.currentPolicy = {
            policyId: emptyGuid(),
            commodityCategoryId: emptyGuid(),
            makeId: emptyGuid(),
            modelId: emptyGuid(),
            commodityTypeId: emptyGuid(),
            dealerId: emptyGuid(),
            policyNo: "",
            vinNo: "",
            plateNumber: "",
            customerName: "",
            failureDate: "",
            failureMileage: "",
            lastServiceMileage: 0.00,
            lastServiceDate: "",
            policyMilege: 0.00,
            Milage: 0.00
        };

        $scope.claimSubmission = {
            customerName: '',
            plateNumber :'',
            failureDate: '',
            failureMileage: 0.00,
            lastServiceDate: '',
            lastServiceMileage: 0.00

        };

        $scope.claimItem = {
            id: 0,
            itemId: emptyGuid(),
            itemName: '',
            itemNumber: '',
            qty: 0,
            unitPrice: 0.00,
            totalGrossPrice: 0.00,
            discountRate: 0.00,
            discountAmount: 0.00,
            isDiscountPercentage: false,
            goodWillRate: 0.00,
            goodWillAmount: 0.00,
            isGoodWillPercentage: false,
            parentId: emptyGuid(),
            totalPrice: 0.00,
            itemType: '',
            remarks: ''
        };

        $scope.currentPart = {
            id: 0,
            partAreaId: emptyGuid(),
            partId: emptyGuid(),
            partNumber: '',
            partName: '',
            partQty: 1,
            unitPrice: '',
            remark: '',
            isRelatedPartsAvailable: false,
            allocatedHours: 0,
            goodWillType: 'NA',
            goodWillValue: 0.00,
            goodWillAmount: 0.00,
            discountType: 'NA',
            discountValue: 0.00,
            discountAmount: 0.00

        };
        $scope.labourCharge = {
            chargeType: 'H',
            hourlyRate: 0.00,
            hours: 0,
            totalAmount: 0.00,
            description: '',
            partId: 0,
            goodWillType: 'NA',
            goodWillValue: 0.00,
            goodWillAmount: 0.00,
            discountType: 'NA',
            discountValue: 0.00,
            discountAmount: 0.00
        };
        $scope.sundry = {
            name: '',
            value: 0.00
        };

        $scope.policyDetails = {
            customerName: '',
            commodityType: '',
            insuaranceProductName: '',
            policyNo: '',
            startDate: '',
            endDate: ''
        };
        $scope.Part = {
            CommodityId: emptyGuid(),
            PartAreaId: emptyGuid(),
            MakeId: emptyGuid(),
            PartName: '',
            PartCode: '',
            PartNumber: '',
            AllocatedHours: 0,
            Price: 0,
            IsActive: true,
            ApplicableForAllModels: true,
            DealerId: emptyGuid(),
            EntryBy: emptyGuid()
        };

        $scope.frontLeftPositionDisabled = true;
        $scope.frontRightPositionDisabled = true;
        $scope.backLeftPositionDisabled = true;
        $scope.backRightPositionDisabled = true;
        $scope.sparePositionDisable = true;

        $scope.frontLeftClaimed = false;
        $scope.frontRightClaimed = false;
        $scope.backLeftClaimed = false;
        $scope.backRightClaimed = false;
        $scope.spareClaimed = false;


        $scope.frontLeftArticleNo = "";
        $scope.frontRightArticleNo = "";
        $scope.backLeftArticleNo = "";
        $scope.backRightArticleNo = "";
        $scope.downArticleNo = "";
        $scope.customerType = 0;
        $scope.customerName = "Customer Name";
        $scope.policySaveStatusMsg = "";
        $scope.emivalue = '';
        $scope.attachments_temp = [];
        $scope.hourlyRatelan = $filter('translate')('pages.claimSubmission.hourlyRate');
        $scope.amountlan = $filter('translate')('pages.claimSubmission.amount');

        $scope.userType = '';
        $scope.totalClaimAmount = 0.00;
        $scope.selectAlltyreLabel = "Select All Tyres";
        $scope.dealer = {
            id: emptyGuid(),
            currencyId: emptyGuid(),
            currencyCode: '',
            hourlyRate: 0.00
        };
        $scope.discountTypesOptions = [
            //{ name: '%', value: 'P' },

        ];
        $scope.serviceRecord = {
            id: emptyGuid(),
            serviceNumber: '',
            milage: '',
            remarks: '',
            serviceDate: ''
        };
        $scope.complaint = {
            customer: '',
            dealer: ''
        };

        $scope.dealerInvoiceTireDetails = {
            InvoiceCodeId: emptyGuid(),
            customerComplaint: '',
            dealerComment: '',
            serialFrontRight: '',
            unusedTyreDepthFrontRight: '',
            serialBackRight: '',
            unusedTyreDepthBackRight: '',
            serialBackLeft: '',
            unusedTyreDepthBackLeft: '',
            seriaFrontlLeft: '',
            unusedTyreDepthFrontLeft: '',
            frontPositionDisabled: false,
            backPositionDisabled: false,
            backPositionDisabled: false,
            unusedTyreDepthDown: '',
            downSerial:''
        }

        $scope.claimItemList = [];
        $scope.claimItemList2 = [];
        $scope.part = {};
        $scope.partAreas = [];
        $scope.parts = [];
        $scope.makes = [];
        $scope.models = [];
        $scope.commodityTypes = [];
        $scope.eligiblePolicies = [];
        $scope.attachmentData = {};

        $scope.isIloeProductSelected = false;
        $scope.isOtherProductSelected = false;
        $scope.itemServiceRecords = [];
        $scope.partSuggestions = [];
        $scope.isSuggestionsAvailable = false;
        $scope.partId = emptyGuid();
        $scope.uploadedDocIds = [];
        $scope.addedSeperately = false;
        $scope.isOtherEngineChange = false;

        $scope.validateRequirmentTyre = false;
        $scope.validateRequirmentNumberPlate = false;
        $scope.allTyresSelected = false;
        $scope.frontPositionDisabled = false;
        $scope.backPositionDisabled = false;
        $scope.customerNameDisabled = false;
        $scope.plateNumberDisabled = false;
        $scope.InvoiceCodeId = "";
        $scope.isEditClaim = false;
        $scope.commentDealer = "";
        $scope.selectAllTyresVisible = false;
        $scope.isTyre = false;
        $scope.tyreWording = {
            "search": "Search Policy",
            "no":"Policy No"
        }



        var columnForSearchGrid = [
            { name: 'Policy No', field: 'PolicyNo', enableSorting: false, cellClass: 'columCss' },
            { name: 'Customer Name', field: 'CustomerName', enableSorting: false, cellClass: 'columCss' },
            { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss', width: 150 },
            { name: 'Plate No', field: 'PlateNo', enableSorting: false, cellClass: 'columCss', width: 150 },
            {
                name: ' ',
                cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicyNew(' +
                    'row.entity.PolicyNo)"' +
                    ' class="btn btn-xs btn-warning">Load</button></div>',
                width: 60,
                enableSorting: false
            }

        ];

        if ($localStorage.CommodityType === "Tyre") {
            $scope.isTyre = true;
            $scope.tyreWording = {
                "search": "Search Contract",
                "no": "Contract No"
            }

            $scope.isOtherProductSelected = true;
            $scope.customerNameDisabled = true;
            $scope.plateNumberDisabled = true;

            columnForSearchGrid = [
                { name: 'Service Contract No', field: 'PolicyNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Customer Name', field: 'CustomerName', enableSorting: false, cellClass: 'columCss' },
                { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss', width: 150 },
                { name: 'Plate No', field: 'PlateNo', enableSorting: false, cellClass: 'columCss', width: 150 },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicyNew(' +
                        'row.entity.PolicyNo)"' +
                        ' class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') + '</button></div>',
                    width: 60,
                    enableSorting: false
                }

            ];

        }

        //  search drop down
        $scope.settings = {
            scrollableHeight: '200px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: true,
            showUncheckAll: false,
            closeOnBlur: true,
            closeOnSelect: true,
            selectionLimit: 1,
            buttonClasses: "drop-btn btn btn-default"

        };
        $scope.CustomText = {
            buttonDefaultText: $filter('translate')('pages.claimSubmission.pleaseSelect'),
            dynamicButtonTextSuffix: $filter('translate')('pages.claimSubmission.itemSelected')
        };

        $scope.$watch('currentPolicy.failureDate', function (newValue) {
            $scope.currentPolicy.failureDate = $filter('date')(newValue, 'dd-MMM-yyyy');
        });

        $scope.loadPolicyDetails = function (keycode) {
            if (keycode == 13 || keycode == 110) {
                angular.element('#mobilePhoneNumber').focus()
            }
        }

        //Scanner Variables
        var uploadUrl = window.location.protocol + '//' + window.location.host + '/TAS.Web/api/Upload/UploadScannerAttachment';
        var DWObject;
        var ScannerPopUp;
        $scope.IsScannedImage = false;
        $scope.tabId = '';
        $scope.ImageArray = [];
        $scope.ImageBatch = {
            documentName: "",
            attachmentType: "",
            page: "",
            section: "",
            encodedImage: []
        };
        $scope.claimSearch = {
            commodityTypeId: emptyGuid(),
            policyNo: '',
            claimNo: '',
            vinNo: '',
            claimDealerId: emptyGuid(),
            makeId: emptyGuid(),
            statusId: emptyGuid()
        };
        //app.config(['$tooltipProvider', function ($tooltipProvider) {
        //    $tooltipProvider.options({
        //        appendToBody: true, //
        //        placement: 'bottom' // Set Default Placement
        //    });
        //}]);

        $scope.maxdate = new Date();
        //initialize uploaders
        $scope.docUploader = new FileUploader({
            url: '/TAS.Web/api/Upload/UploadAttachment',
            headers: {
                'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt,
                'Page': 'ClaimSubmission', 'Section': 'Default'
            }
        });

        $scope.docUploader.onProgressAll = function () {
            $scope.uploadProgress = true;
            swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), text: $filter('translate')('pages.claimSubmission.inforMessages.uploadingDocuments'), showConfirmButton: false });
        }
        $scope.docUploader.onSuccessItem = function (item, response, status, headers) {
            if (response != 'Failed') {
                $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
                // $scope.currentUploadingItem++
                // $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.docUploader.queue.length;
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.errorUploadingAttachments'));
                $scope.docUploader.cancelAll();
                $scope.uploadProgress = false;
            }
        }
        $scope.docUploader.onCompleteAll = function () {
            let product = $scope.getSelectedProduct();
            $scope.uploadProgress = false;
            $scope.docUploader.queue = [];

            if (product != undefined && product != null && product.ProductTypeCode == "TYRE") {
                if ($scope.isEditClaim) {
                    $scope.updateOtherTyreClaim();
                } else {
                    $scope.submitOtherTireClaim();
                }


            } else if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                $scope.ConfirmButtonInILOE();

            } else {
                if ($scope.isEditClaim) {
                    $scope.updateClaim();
                } else {
                    $scope.submitClaim();
                }
            }

        }
        $scope.claimView = {
            claimNotes: [],
            claimHistory: [],
            claimAssessment: [],
            claimMessage: []
        };
        $scope.maxdate = new Date();
        $scope.docUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                //alert($scope.currentPolicy.docAttachmentType1);
                if ($scope.currentPolicy.docAttachmentType1 !== "" && typeof $scope.currentPolicy.docAttachmentType1 !== 'undefined') {
                    var fileType = item.type.split("/")[0];
                    if (fileType == "video" || fileType == "audio") {
                        if (item.size > 104857600) {  // Max video Size 100MB
                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.maxVideoSize'));
                            return false;
                        } else {
                            return true;
                        }
                    } else {
                        if (item.size > 3000000) {
                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.maxSize'));
                            return false;
                        } else {
                            return true;
                        }
                    }

                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.attachmentType'));
                    return false;
                }
            }
        });

        $scope.loadInitailData = function () {

            if ($scope.claimId == null || $scope.claimId==undefined) {

            } else {
                $scope.getClaimData();

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllClaimHistoryDetailsByClaimId',
                    data: {
                        "claimId": $scope.claimId,
                        "LoggedInUserId": $localStorage.LoggedInUserId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.claimNotes != null)
                        $scope.claimView.claimNotes = data.claimNotes.Notes;
                    if (data.assessmentAndClaimHistory != null)
                        $scope.claimView.claimHistory = data.assessmentAndClaimHistory.ClaimData;
                    if (data.assessmentAndClaimHistory != null)
                        $scope.claimView.claimAssessment = data.assessmentAndClaimHistory.AssessmentData;
                    if (data.claimComments != null)
                        $scope.claimView.claimMessage = data.claimComments.Comments;



                    //console.log($scope.userType);
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });

            }
            //$scope.getClaimData();

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/UserValidationClaimListing',
                data: { "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.status == 'ok') {
                    $scope.userType = data.type;
                    $scope.claimTable.reload();
                    //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                } else {
                    swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.securityInformation'), text: data.status, showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 8000);
                    $state.go('app.dashboard');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/UserValidationClaimSubmission',
                data: { "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.status === 'ok') {
                    $scope.dealer.currencyId = data.dealerCurrencyId;
                    $scope.dealer.currencyCode = data.dealerCurrencyCode;
                    $scope.discountTypesOptions.push({ name: data.dealerCurrencyCode, value: 'F' });

                    $scope.userType = data.userType;
                    if (!parseFloat(data.dealerHourlyRate) || parseFloat(data.dealerHourlyRate) === 0) {
                        swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.securityInformation'), text: $filter('translate')('pages.claimSubmission.inforMessages.manHour'), showConfirmButton: false });
                        setTimeout(function () { swal.close(); }, 8000);
                        $state.go('app.dashboard');

                    } else {
                        $scope.dealer.hourlyRate = data.dealerHourlyRate;
                        $scope.dealer.id = data.dealerId;
                        $scope.currentPolicy.dealerId = data.dealerId;

                        $scope.labourCharge.hourlyRate = data.dealerHourlyRate;
                        $scope.getDetailsPerDealer();

                    }
                } else {
                    swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.securityInformation'), text: data.status, showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 8000);
                    $state.go('app.dashboard');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDocumentTypesByPageName',
                dataType: 'json',
                data: { PageName: 'ClaimSubmission' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.docAttachmentTypes = data;
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllProducts',
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Products = data;


            }).error(function (data, status, headers, config) {
            });
            if (!$scope.isEditClaim) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.commodityTypes = data;
                    if (data.length === 1) {

                        $scope.currentPolicy.commodityTypeId = data[0].CommodityTypeId;
                        $scope.vinNumberChanged();
                    }

                }).error(function (data, status, headers, config) {
                });
            }
             $scope.GetAllCustomerComplaints();
             $scope.GetAllDealerComments();

        }
        $scope.getDetailsPerDealer = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllMakesByDealerId',
                dataType: 'json',
                data: { dealerId: $scope.dealer.id },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.makes = data;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.selectedMakeChanged = function () {
            $scope.models = [];
            $scope.currentPart.partAreaId = emptyGuid();
            if (isGuid($scope.currentPolicy.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                    dataType: 'json',
                    data: { Id: $scope.currentPolicy.makeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.models = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.selectedModelChanged = function () {
            $scope.part.selected = false;
        }



        //$scope.function = function () {
        //    $scope.part.selected = undefined;
        //    $scope.currentPart.partNumber = '';
        //    $scope.currentPart.partQty = 1;
        //    $scope.currentPart.unitPrice = 0.00;
        //    $scope.currentPart.partId = emptyGuid();
        //    $scope.currentPart.isRelatedPartsAvailable = false;
        //    $scope.currentPart.allocatedHours = 0;
        //    $scope.currentPart.remark = '';
        //}

        $scope.adjustAllDiscounts = function () {
            $scope.goodWillChanging();
            $scope.discountChanging();
        }

        $scope.goodWillChanging = function () {
            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                    //part has total price
                    if ($scope.currentPart.goodWillType === "P") {
                        if (parseFloat($scope.currentPart.goodWillValue) && parseFloat($scope.currentPart.goodWillValue) > 0) {
                            var totalPriceOfPart = parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty);
                            if (parseFloat($scope.currentPart.goodWillValue) > 100) {
                                $scope.currentPart.goodWillValue = 100;
                                $scope.currentPart.goodWillAmount = totalPriceOfPart;
                            } else {
                                $scope.currentPart.goodWillAmount = totalPriceOfPart * parseFloat($scope.currentPart.goodWillValue) / 100;
                            }
                        } else {
                            $scope.currentPart.goodWillAmount = 0.00;
                            $scope.currentPart.goodWillValue = 0.00;
                        }
                    } else if ($scope.currentPart.goodWillType === "F") {
                        if (parseFloat($scope.currentPart.goodWillValue) && parseFloat($scope.currentPart.goodWillValue) > 0) {
                            $scope.currentPart.goodWillAmount = parseFloat($scope.currentPart.goodWillValue);
                        } else {
                            $scope.currentPart.goodWillAmount = 0.00;
                            $scope.currentPart.goodWillValue = 0.00;
                        }
                    } else {
                        $scope.currentPart.goodWillAmount = 0.00;
                        $scope.currentPart.goodWillValue = 0.00;
                    }
                } else {
                    $scope.currentPart.goodWillAmount = 0.00;
                }
            } else {
                $scope.currentPart.goodWillAmount = 0.00;
            }
        }
        $scope.discountChanging = function () {
            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                    //part has total price
                    if ($scope.currentPart.discountType === "P") {
                        if (parseFloat($scope.currentPart.discountValue) && parseFloat($scope.currentPart.discountValue) > 0) {
                            var totalPriceOfPart = parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty) - parseFloat($scope.currentPart.goodWillAmount);

                            if (parseFloat($scope.currentPart.discountValue) > 100) {
                                $scope.currentPart.discountValue = 100;
                                $scope.currentPart.discountAmount = totalPriceOfPart;
                            } else {
                                $scope.currentPart.discountAmount = totalPriceOfPart * parseFloat($scope.currentPart.discountValue) / 100;
                            }
                        } else {
                            $scope.currentPart.discountAmount = 0.00;
                            $scope.currentPart.discountValue = 0.00;
                        }
                    } else if ($scope.currentPart.discountType === "F") {
                        if (parseFloat($scope.currentPart.discountValue) && parseFloat($scope.currentPart.discountValue) > 0) {
                            $scope.currentPart.discountAmount = parseFloat($scope.currentPart.discountValue);
                        } else {
                            $scope.currentPart.discountAmount = 0.00;
                            $scope.currentPart.discountValue = 0.00;
                        }
                    } else {
                        $scope.currentPart.discountAmount = 0.00;
                        $scope.currentPart.discountValue = 0.00;
                    }
                } else {
                    $scope.currentPart.discountAmount = 0.00;
                }
            } else {
                $scope.currentPart.discountAmount = 0.00;
            }
        }

        $scope.goodWillChangingLabour = function () {
            var totalAmount = 0.00;
            if ($scope.labourCharge.chargeType === 'H') {
                totalAmount = parseFloat($scope.labourCharge.hourlyRate) * parseFloat($scope.labourCharge.hours);
            } else {
                totalAmount = parseFloat($scope.labourCharge.hourlyRate);
            }

            if (parseFloat(totalAmount) && parseFloat(totalAmount) > 0) {

                if ($scope.labourCharge.goodWillType === "P") {
                    if (parseFloat($scope.labourCharge.goodWillValue) && parseFloat($scope.labourCharge.goodWillValue) > 0) {
                        if (parseFloat($scope.labourCharge.goodWillValue) > 100) {
                            $scope.labourCharge.goodWillValue = 100;
                            $scope.labourCharge.goodWillAmount = totalAmount;
                        } else {
                            $scope.labourCharge.goodWillAmount = totalAmount * parseFloat($scope.labourCharge.goodWillValue) / 100;
                        }
                    } else {
                        $scope.labourCharge.goodWillAmount = 0.00;
                        $scope.labourCharge.goodWillValue = 0.00;
                    }
                } else if ($scope.labourCharge.goodWillType === "F") {
                    if (parseFloat($scope.labourCharge.goodWillValue) && parseFloat($scope.labourCharge.goodWillValue) > 0) {
                        $scope.labourCharge.goodWillAmount = parseFloat($scope.labourCharge.goodWillValue);
                    } else {
                        $scope.labourCharge.goodWillAmount = 0.00;
                        $scope.labourCharge.goodWillValue = 0.00;
                    }
                } else {
                    $scope.labourCharge.goodWillAmount = 0.00;
                    $scope.labourCharge.goodWillValue = 0.00;
                }

            } else {
                $scope.labourCharge.goodWillValue = 0.00;
            }

        }
        $scope.discountChangingLabour = function () {
            var totalAmount = 0.00;
            if ($scope.labourCharge.chargeType === 'H') {
                totalAmount = parseFloat($scope.labourCharge.hourlyRate) * parseFloat($scope.labourCharge.hours);
            } else {
                totalAmount = parseFloat($scope.labourCharge.hourlyRate);
            }

            if (parseFloat(totalAmount) && parseFloat(totalAmount) > 0) {
                if ($scope.labourCharge.discountType === "P") {
                    if (parseFloat($scope.labourCharge.discountValue) && parseFloat($scope.labourCharge.discountValue) > 0) {
                        if (parseFloat($scope.labourCharge.discountValue) > 100) {
                            $scope.labourCharge.discountValue = 100;
                            $scope.labourCharge.discountAmount = totalAmount;
                        } else {
                            $scope.labourCharge.discountAmount = totalAmount * parseFloat($scope.labourCharge.discountValue) / 100;
                        }
                    } else {
                        $scope.labourCharge.discountAmount = 0.00;
                        $scope.labourCharge.discountValue = 0.00;
                    }
                } else if ($scope.labourCharge.discountType === "F") {
                    if (parseFloat($scope.labourCharge.discountValue) && parseFloat($scope.labourCharge.discountValue) > 0) {
                        $scope.labourCharge.discountAmount = parseFloat($scope.labourCharge.discountValue);
                    } else {
                        $scope.labourCharge.discountAmount = 0.00;
                        $scope.labourCharge.discountValue = 0.00;
                    }
                } else {
                    $scope.labourCharge.discountAmount = 0.00;
                    $scope.labourCharge.discountValue = 0.00;
                }
            } else {
                $scope.labourCharge.discountAmount = 0.00;
            }
        }

        $scope.adjustAllDiscountsLabour = function () {
            $scope.goodWillChangingLabour();
            $scope.discountChangingLabour();
        }


        $scope.validatePartDetails = function () {
            var isValid = true;
            if (!isGuid($scope.currentPart.partAreaId)) {
                $scope.validate_partArea = "has-error";
                isValid = false;
            } else
                $scope.validate_partArea = "";



            //if (!isGuid($scope.currentPart.partId)) {
            //    $scope.validate_part = "has-error";
            //    isValid = false;
            //} else
            //    $scope.validate_part = "";


            if ($scope.currentPart.partNumber === '') {
                $scope.validate_partNumber = "has-error";
                isValid = false;
            } else
                $scope.validate_partNumber = "";

            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                $scope.validate_partQty = "";
            } else {

                $scope.validate_partQty = "has-error";
                isValid = false;
            }

            if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                $scope.validate_unitPrice = "";
            } else {

                $scope.validate_unitPrice = "has-error";
                isValid = false;
            }
            //validate discount & Good will
            if ($scope.currentPart.goodWillType !== 'NA') {
                if (parseFloat($scope.currentPart.goodWillValue) && parseFloat($scope.currentPart.goodWillValue) > 0) {
                    $scope.validate_goodWillValue = "";
                } else {
                    $scope.validate_goodWillValue = "has-error";
                    isValid = false;
                }

            } else {
                $scope.currentPart.goodWillValue = 0.00;
                $scope.currentPart.goodWillAmount = 0.00;
            }

            if ($scope.currentPart.discountType !== 'NA') {
                if (parseFloat($scope.currentPart.discountValue) && parseFloat($scope.currentPart.discountValue) > 0) {
                    $scope.validate_discountValue = "";
                } else {
                    $scope.validate_discountValue = "has-error";
                    isValid = false;
                }

            } else {
                $scope.currentPart.discountValue = 0.00;
                $scope.currentPart.discountAmount = 0.00;
            }
            //validate discount sum


            return isValid;
        }
        $scope.validateTotPartExceeding = function () {

            var totalPriceOfPart = parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty);
            var totalDiscounts = parseFloat($scope.currentPart.discountAmount) + parseFloat($scope.currentPart.goodWillAmount);

            if (totalPriceOfPart < totalDiscounts) {
                $scope.validate_discountValue = "has-error";
                $scope.validate_goodWillValue = "has-error";
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.goodwillAmount'));
                return false;
            }
            else
                return true;

        }

        $scope.validateTyreDepth = function (modalName) {
            var model = $parse(modalName);
            if (Number($scope.$eval(modalName)) <= 3) {
                customWorningMessage($filter('translate')('pages.claimSubmission.errorMessages.claimRejectedRef'));
               // model.assign($scope, '');

            }
        }
        $scope.validateClaimDetails = function () {
            var isValid = true;

            if ($scope.currentPolicy.policyNo === "") {
                $scope.validate_policyNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_policyNumber = "";
            }

            if ($scope.currentPolicy.plateNumber === "") {
                $scope.validate_plateNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_plateNumber = "";
            }

            if ($scope.currentPolicy.customerName === "") {
                $scope.validate_customerName = "has-error";
                isValid = false;
            } else {
                $scope.validate_customerName = "";
            }

            if ($scope.currentPolicy.failureDate === "") {
                $scope.validate_failureDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_failureDate = "";
            }

            if ($scope.currentPolicy.failureMileage === "") {
                $scope.validate_failureMileage = "has-error";
                isValid = false;
            } else {
                $scope.validate_failureMileage = "";
            }

            if ($scope.currentPolicy.failureMileage === "") {
                $scope.validate_failureMileage = "has-error";
                isValid = false;
            } else {
                $scope.validate_failureMileage = "";
            }

            if ($scope.currentPolicy.lastServiceDate === "" && $scope.required.lastServiceDate == true) {
                $scope.validate_lastServiceDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_lastServiceDate = "";
            }



            if ($scope.currentPolicy.lastServiceMileage === "" && $scope.required.lastServiceMileage==true) {
                $scope.validate_lastServiceMileage = "has-error";
                isValid = false;
            } else {
                $scope.validate_lastServiceMileage = "";
            }

            if (!isGuid($scope.currentPolicy.makeId)) {
                $scope.validate_makeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_makeId = "";
            }

            if (!isGuid($scope.currentPolicy.modelId)) {
                $scope.validate_modelId = "has-error";
                isValid = false;
            } else {
                $scope.validate_modelId = "";
            }

            return isValid;
        }
        //$scope.addOtherPart = function () {
        //    if (isGuid($scope.currentPolicy.policyId)) {

        //        if ($scope.OtherTireDetails.dealerInvoiceTireDetails.serialFrontRight == "") {
        //            var nextId = $scope.claimItemList.length;
        //            var claimItem = {
        //                id: (nextId + 1),
        //                partAreaId: emptyGuid(),
        //                partId: emptyGuid(),
        //                itemName: $scope.dealerInvoiceTireDetails.serialFrontRight,
        //                itemNumber: $scope.dealerInvoiceTireDetails.serialFrontRight,
        //                qty: 1,
        //                unitPrice: 0.00,
        //                totalGrossPrice: 0.00,
        //                discountRate: 0.00,
        //                discountAmount: 0.00,
        //                isDiscountPercentage: false,
        //                goodWillRate: $scope.currentPart.goodWillValue,
        //                goodWillAmount: $scope.currentPart.goodWillAmount,
        //                isGoodWillPercentage: $scope.currentPart.goodWillType === "P" ? true : false,
        //                parentId: emptyGuid(),
        //                totalPrice: (
        //                    (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)) -
        //                    (parseFloat($scope.currentPart.discountAmount) + parseFloat($scope.currentPart.goodWillAmount))
        //                ).toFixed(2),
        //                itemType: 'P',
        //                remarks: $scope.currentPart.remark
        //            };

        //        } else if ($scope.OtherTireDetails.dealerInvoiceTireDetails.seriaFrontlLeft == "") {

        //        } else if ($scope.OtherTireDetails.dealerInvoiceTireDetails.serialBackLeft == "") {

        //        } else if ($scope.OtherTireDetails.dealerInvoiceTireDetails.serialBackRight == "") {

        //        }


        //    } else {
        //        customErrorMessage('Please select a policy to add parts.');
        //    }
        //}
        $scope.loaninstallmentChange = function () {
            console.log($scope.currentPart.partName);
            if ($scope.currentPart.partName.trim() == "") {
                $scope.currentPart.unitPrice = 0.00;
            } else {
                $scope.currentPart.unitPrice = $scope.emivalue;
            }

        }

        $scope.addPart = function () {
            let product = $scope.getSelectedProduct();
            if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                //$scope.isIloeProductSelected = true;
                $scope.currentPart.partName;

                if ($scope.currentPart.unitPrice > 5000) {
                    $scope.currentPart.unitPrice = $scope.claimLimitation;
                    customInfoMessage("Loan monthly limitation of AED 5,000 has been applied.");
                }

            } else {
                if (!$scope.validateClaimDetails()) {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                    return false;
                }
            }

            if (product != undefined && product != null && product.ProductTypeCode  == "ILOE") {
                //$scope.isIloeProductSelected = true;
                if (isGuid($scope.currentPolicy.policyId)) {
                    if ($scope.validatePartDetails()) {
                        var nextId = $scope.claimItemList.length;

                        var claimItem = {
                            id: (nextId + 1),
                            partAreaId: $scope.currentPart.partAreaId,
                            partId: $scope.currentPart.partId,
                            itemName: $scope.currentPart.partName,
                            itemNumber: $scope.currentPart.partNumber,
                            qty: $scope.currentPart.partQty,
                            unitPrice: $scope.currentPart.unitPrice,
                            totalGrossPrice: (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)).toFixed(2),
                            discountRate: $scope.currentPart.discountValue,
                            discountAmount: $scope.currentPart.discountAmount,
                            isDiscountPercentage: $scope.currentPart.discountType === "P" ? true : false,
                            goodWillRate: $scope.currentPart.goodWillValue,
                            goodWillAmount: $scope.currentPart.goodWillAmount,
                            isGoodWillPercentage: $scope.currentPart.goodWillType === "P" ? true : false,
                            parentId: emptyGuid(),
                            totalPrice: (
                                (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)) -
                                (parseFloat($scope.currentPart.discountAmount) + parseFloat($scope.currentPart.goodWillAmount))
                            ).toFixed(2),
                            itemType: 'P',
                            remarks: $scope.currentPart.remark
                        };
                        $scope.claimItemList.push(claimItem);
                        //$scope.addLabourCharge();
                        $scope.clearPartSection();
                        //update total
                        $scope.calculateTotalClaimAmount();

                        if ($scope.isSuggestionsAvailable) {
                            swal({
                                title: $filter('translate')('pages.claimSubmission.inforMessages.partInterrelated'),
                                text: $filter('translate')('pages.claimSubmission.inforMessages.youWishAdd'),
                                type: "info",
                                showCancelButton: true,
                                closeOnConfirm: false,
                                showLoaderOnConfirm: true,
                            },
                                function () {
                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/claim/GetAllRelatedParts',
                                        data: {
                                            "partId": claimItem.partId,
                                            "dealerId": $scope.currentPolicy.dealerId
                                        },
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        if (data == null)
                                            customErrorMessage($filter('translate')('pages.claimSubmission.inforMessages.errorPartInformation'));
                                        for (var i = 0; i < data.length; i++) {
                                            data[i].discountType = "P";
                                            data[i].discount = 0.00;
                                            data[i].goodWillType = "P";
                                            data[i].goodWill = 0.00;
                                        }
                                        $scope.partSuggestions = data;
                                        PartSuggestionPopup = ngDialog.open({
                                            template: 'popUpSuggestedPartDetails',
                                            className: 'ngdialog-theme-plain',
                                            closeByEscape: true,
                                            showClose: true,
                                            closeByDocument: true,
                                            scope: $scope
                                        });
                                        return true;
                                    }).error(function (data, status, headers, config) {
                                    }).finally(function () {
                                        swal.close();
                                    });
                                });
                        }

                    } else {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.policyToAddPart'));
                }

            } else {
                if (isGuid($scope.currentPolicy.policyId)) {
                    if ($scope.validatePartDetails()) {
                        if ($scope.validateTotPartExceeding()) {
                            var nextId = $scope.claimItemList.length;

                            var claimItem = {
                                id: (nextId + 1),
                                partAreaId: $scope.currentPart.partAreaId,
                                partId: $scope.currentPart.partId,
                                itemName: $scope.currentPart.partName,
                                itemNumber: $scope.currentPart.partNumber,
                                qty: $scope.currentPart.partQty,
                                unitPrice: $scope.currentPart.unitPrice,
                                totalGrossPrice: (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)).toFixed(2),
                                discountRate: $scope.currentPart.discountValue,
                                discountAmount: $scope.currentPart.discountAmount,
                                isDiscountPercentage: $scope.currentPart.discountType === "P" ? true : false,
                                goodWillRate: $scope.currentPart.goodWillValue,
                                goodWillAmount: $scope.currentPart.goodWillAmount,
                                isGoodWillPercentage: $scope.currentPart.goodWillType === "P" ? true : false,
                                parentId: emptyGuid(),
                                totalPrice: (
                                    (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)) -
                                    (parseFloat($scope.currentPart.discountAmount) + parseFloat($scope.currentPart.goodWillAmount))
                                ).toFixed(2),
                                itemType: 'P',
                                remarks: $scope.currentPart.remark
                            };

                            if (parseFloat($scope.currentPart.allocatedHours) && parseFloat($scope.currentPart.allocatedHours) > 0) {
                                $scope.labourCharge.chargeType = "H";
                                $scope.labourCharge.hours = $scope.currentPart.allocatedHours;
                                $scope.labourCharge.description = "This allocated to part no -" + $scope.currentPart.partNumber;
                                $scope.labourCharge.partId = claimItem.id;
                                if (parseFloat($scope.dealer.hourlyRate) > 0) {
                                    $scope.labourCharge.hourlyRate = $scope.dealer.hourlyRate;

                                    $scope.claimItemList.push(claimItem);
                                    $scope.addLabourCharge();
                                    $scope.clearPartSection();
                                    //update total
                                    $scope.calculateTotalClaimAmount();
                                } else {
                                    swal({
                                        title: "",
                                        text: "Please enter dealer hourly rate (" + $scope.dealer.currencyCode + ")",
                                        type: "input",
                                        showCancelButton: true,
                                        closeOnConfirm: true,
                                        animation: "slide-from-top",
                                        inputPlaceholder: "hourly rate amount"
                                    },
                                        function (inputValue) {
                                            if (inputValue === false) return false;

                                            if (inputValue === "" || !parseFloat(inputValue) || parseFloat(inputValue) === 0) {
                                                swal.showInputError($filter('translate')('pages.claimSubmission.errorMessages.enterAmount'));
                                                return false;
                                            }
                                            $scope.dealer.hourlyRate = parseFloat(inputValue);
                                            $scope.addPart();
                                        });
                                }

                            } else {
                                $scope.claimItemList.push(claimItem);
                                $scope.clearPartSection();
                                //update total
                                $scope.calculateTotalClaimAmount();
                            }

                            if ($scope.isSuggestionsAvailable) {
                                swal({
                                    title: $filter('translate')('pages.claimSubmission.inforMessages.partInterrelated'),
                                    text: $filter('translate')('pages.claimSubmission.inforMessages.youWishAdd'),
                                    type: "info",
                                    showCancelButton: true,
                                    closeOnConfirm: false,
                                    showLoaderOnConfirm: true,
                                },
                                    function () {
                                        $http({
                                            method: 'POST',
                                            url: '/TAS.Web/api/claim/GetAllRelatedParts',
                                            data: {
                                                "partId": claimItem.partId,
                                                "dealerId": $scope.currentPolicy.dealerId
                                            },
                                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                        }).success(function (data, status, headers, config) {
                                            if (data == null)
                                                customErrorMessage($filter('translate')('pages.claimSubmission.inforMessages.errorPartInformation'));
                                            for (var i = 0; i < data.length; i++) {
                                                data[i].discountType = "P";
                                                data[i].discount = 0.00;
                                                data[i].goodWillType = "P";
                                                data[i].goodWill = 0.00;
                                            }
                                            $scope.partSuggestions = data;
                                            PartSuggestionPopup = ngDialog.open({
                                                template: 'popUpSuggestedPartDetails',
                                                className: 'ngdialog-theme-plain',
                                                closeByEscape: true,
                                                showClose: true,
                                                closeByDocument: true,
                                                scope: $scope
                                            });
                                            return true;
                                        }).error(function (data, status, headers, config) {
                                        }).finally(function () {
                                            swal.close();
                                        });
                                    });
                            }
                        }
                    } else {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.policyToAddPart'));
                }
            }



        }
        $scope.validateLabourPayments = function () {
            var isValid = true;
            if ($scope.labourCharge.chargeType === '') {
                $scope.validate_chargeType = "has-error";
                isValid = false;
            } else {
                $scope.validate_chargeType = "";
            }

            if (!parseFloat($scope.labourCharge.hourlyRate)) {
                $scope.validate_hourlyRate = "has-error";
                isValid = false;
            } else {
                $scope.validate_hourlyRate = "";
            }

            if ($scope.labourCharge.chargeType === "H") {
                if (!parseFloat($scope.labourCharge.hours)) {
                    $scope.validate_hours = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_hours = "";
                }
            }

            return isValid;

        }
        $scope.clearLabourChargeSection = function () {

            $scope.labourCharge.hours = 0;
            $scope.labourCharge.totalAmount = 0.00;
            $scope.labourCharge.totalGrossPrice = 0.00;
            $scope.labourCharge.partId = 0;
            $scope.labourCharge.goodWillType = 'NA';
            $scope.labourCharge.goodWillValue = 0.00;
            $scope.labourCharge.goodWillAmount = 0.00;
            $scope.labourCharge.discountType = 'NA';
            $scope.labourCharge.discountValue = 0.00;
            $scope.labourCharge.discountAmount = 0.00;

        }
        $scope.addLabourCharge = function (addedSeperately) {
            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateLabourPayments()) {
                    if ($scope.labourCharge.partId == 0) {
                        if ($scope.claimItemList.length === 0) {
                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.atleastLabourCost'));
                        } else {
                            PartsSuggestionForLabourCost = ngDialog.open({
                                template: 'popUpSuggestedPartDetailsForLabourCost',
                                className: 'ngdialog-theme-plain',
                                closeByEscape: false,
                                showClose: true,
                                closeByDocument: false,
                                scope: $scope
                            });
                        }

                    } else {
                        var nextId = $scope.claimItemList.length;
                        if (addedSeperately) {
                            nextId = $scope.labourCharge.partId;
                            angular.forEach($scope.claimItemList, function (claims) {
                                if (claims.id == nextId) {
                                    $scope.currentPart.partId = claims.partId;
                                }
                            });
                        }

                        //alert(addedSeperately);



                        var totalPriceTemp= ($scope.labourCharge.chargeType === "H" ?
                            (parseFloat($scope.labourCharge.hours) * parseFloat($scope.labourCharge.hourlyRate)).toFixed(2) :
                            parseFloat($scope.labourCharge.hourlyRate).toFixed(2)) - (parseFloat($scope.labourCharge.discountAmount) + parseFloat($scope.labourCharge.goodWillAmount));
                        var claimItem = {
                            id: (nextId + 1),
                            partAreaId: emptyGuid(),
                            partId: emptyGuid(),
                            itemName: "Labour Charge",
                            itemNumber: $scope.labourCharge.chargeType === "H" ? "Hourly" : "Fixed",
                            qty: $scope.labourCharge.chargeType === "H" ? $scope.labourCharge.hours : 1,
                            unitPrice: $scope.labourCharge.hourlyRate,
                            totalGrossPrice: $scope.labourCharge.chargeType === "H" ?
                                (parseFloat($scope.labourCharge.hours) * parseFloat($scope.labourCharge.hourlyRate)).toFixed(2) :
                                parseFloat($scope.labourCharge.hourlyRate).toFixed(2),
                            discountRate: $scope.labourCharge.discountValue,
                            discountAmount: $scope.labourCharge.discountAmount,
                            isDiscountPercentage: $scope.labourCharge.discountType === "P" ? true : false,
                            goodWillRate: $scope.labourCharge.goodWillValue,
                            goodWillAmount: $scope.labourCharge.goodWillAmount,
                            isGoodWillPercentage: $scope.labourCharge.goodWillType === "P" ? true : false,

                            //parentId: emptyGuid(),
                            parentId: $scope.currentPart.partId,

                            itemType: 'L',
                            totalPrice: totalPriceTemp.toFixed(2),
                            remarks: $scope.labourCharge.description
                        };
                        $scope.claimItemList.push(claimItem);
                        $scope.clearLabourChargeSection();
                        //update total
                        $scope.calculateTotalClaimAmount();
                    }


                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.selectPolicyLabourCharge'));
            }
        }
        $scope.selectPartToLabourCost = function (id) {
            if ($scope.validatePartUopnAddLabourCharge(id)) {
                $scope.labourCharge.partId = id;
                PartsSuggestionForLabourCost.close();
                $scope.shiftingClaimListingIndexesWithLabourChargeAdd(id);
                $scope.addLabourCharge(true);
                $scope.claimItemList = $filter('orderBy')($scope.claimItemList, 'id', false);
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.selectAnotherPart'));
                return false;
            }
        }
        $scope.validatePartUopnAddLabourCharge = function (partId) {
            var isPartOk = true;
            angular.forEach($scope.claimItemList, function (claim) {

                if (claim.id === (partId + 1) && claim.itemType === 'L') { isPartOk = false; }
            });
            return isPartOk;
        }

        $scope.shiftingClaimListingIndexesWithLabourChargeAdd = function (partId) {
            //update index
            angular.forEach($scope.claimItemList, function (claimItem) {
                if (partId < claimItem.id) {
                    claimItem.id++;
                }

            });
        }

        $scope.addSundry = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateSundry()) {
                    var nextId = $scope.claimItemList.length;
                    var claimItem = {
                        id: (nextId + 1),
                        partAreaId: emptyGuid(),
                        partId: emptyGuid(),
                        itemName: "Sundry",
                        itemNumber: $scope.sundry.name,
                        qty: 1,
                        unitPrice: $scope.sundry.value,
                        totalPrice: parseFloat($scope.sundry.value).toFixed(2),
                        itemType: 'S',
                        remarks: ''
                    };
                    $scope.claimItemList.push(claimItem);
                    $scope.clearSundrySection();
                    //update total
                    $scope.calculateTotalClaimAmount();
                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.addSundires'));
            }
        }
        $scope.validateSundry = function () {
            var isValid = true;
            if ($scope.sundry.name === '') {
                $scope.validate_sundryName = "has-error";
                isValid = false;
            } else {
                $scope.validate_sundryName = "";
            }

            if (!parseFloat($scope.sundry.value)) {
                $scope.validate_sundryValue = "has-error";
                isValid = false;
            } else {
                $scope.validate_sundryValue = "";
            }

            return isValid;
        }
        $scope.clearSundrySection = function () {
            $scope.sundry.name = '';
            $scope.sundry.value = '';
        }

        $scope.addServiceHistory = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateServiceHistory()) {
                    //adding service history
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/AddServiceHistory',
                        data: {
                            "policyId": $scope.currentPolicy.policyId,
                            "serviceData": $scope.serviceRecord
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == 'ok') {
                            customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.serviceRecordSucc'));
                            $scope.clearServiceRecord();
                            $scope.GetServiceHistory($scope.currentPolicy.policyId);
                        } else {
                            if (data == '') {
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.errorServiceHistory'));
                            } else {
                                customErrorMessage(data);
                            }
                        }
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });
                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.addServiceHistory'));
            }
        }
        $scope.validateServiceHistory = function () {
            var isValid = true;
            if (!parseInt($scope.serviceRecord.serviceNumber)) {
                $scope.validate_serviceNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_serviceNumber = "";
            }

            if (!parseFloat($scope.serviceRecord.milage)) {
                $scope.validate_milage = "has-error";
                isValid = false;
            } else {
                $scope.validate_milage = "";
            }

            if ($scope.serviceRecord.serviceDate === '') {
                $scope.validate_serviceDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_serviceDate = "";
            }

            return isValid;

        }
        $scope.clearServiceRecord = function () {

            $scope.serviceRecord.id = emptyGuid();
            $scope.serviceRecord.serviceNumber = '';
            $scope.serviceRecord.milage = '';
            $scope.serviceRecord.remarks = '';
            $scope.serviceRecord.serviceDate = '';

        }


        $scope.calculateTotalClaimAmount = function () {
            $scope.totalClaimAmount = 0.00;
            angular.forEach($scope.claimItemList, function (claimItem) {
                $scope.totalClaimAmount += parseFloat(claimItem.totalPrice);
            });
        }
        $scope.removeClaimItem = function (claimItemId) {
            if (parseInt(claimItemId)) {
                //remove item
                for (var i = 0; i < $scope.claimItemList.length; i++) {
                    if (claimItemId === $scope.claimItemList[i].id) {
                        if ($scope.isIloeProductSelected) {
                            $scope.claimItemList.splice(i, 1);
                        } else {
                            if ($scope.claimItemList[i].itemType === 'P' && $scope.claimItemList[i + 1].itemType === 'L') {
                                $scope.claimItemList.splice(i, 2);
                            } else {
                                $scope.claimItemList.splice(i, 1);
                            }
                        }

                        break;
                    }
                }
                //update index
                angular.forEach($scope.claimItemList, function (claimItem) {
                    if (claimItemId < claimItem.id) {
                        claimItem.id--;
                    }

                });
                //update total
                $scope.calculateTotalClaimAmount();
            }
        }


        $scope.viewPolicyDetails = function () {
            if (isGuid($scope.currentPolicy.policyId)) {

                SearchPolicyPopup = ngDialog.open({
                    template: 'popUpPolicyDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
                return true;
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectPolicy'));
            }
        }

        $scope.viewProductDetails = function () {
            var paginationOptionsProductSearchGrid = {
                pageNumber: 1,
                sort: null
            };



            $scope.refreshProductSearch();

                SearchPolicyPopup = ngDialog.open({
                    template: 'popUpProductDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
                return true;
        }

        $scope.iloeLoanInstallment = [];

        $scope.vinNumberChanged2 = function (policyId,calculateEMI,vinNo) {
            if (!isGuid($scope.currentPolicy.commodityTypeId)) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectCommodityType'));
                return false;
            }
            $scope.isOtherProductSelected = false;
            $scope.customerNameDisabled = false;
            $scope.plateNumberDisabled = false;
            $scope.isOtherEngineChange = false;
            $scope.currentPolicy.vinNo = vinNo;
            $scope.emivalue = calculateEMI.toFixed(2);


            if ($scope.currentPolicy.vinNo !== '') {
                SearchPolicyPopup.close();
                swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.validatingVin'), showConfirmButton: false });

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidateVinSerialNumber',
                        data: {
                            "vinNo": $scope.currentPolicy.vinNo,
                            "commodityTypeId": $scope.currentPolicy.commodityTypeId,
                            "dealerId": $scope.dealer.id,
                            "productId": $scope.product.Id
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        $scope.currentPolicy.policyId = emptyGuid();
                        $scope.currentPolicy.commodityCategoryId = emptyGuid();
                        $scope.currentPolicy.makeId = emptyGuid();
                        $scope.currentPolicy.modelId = emptyGuid();
                        $scope.currentPolicy.policyNo = "";
                        $scope.currentPolicy.plateNumber = "";
                        $scope.currentPolicy.customerName = "";
                        $scope.currentPolicy.failureDate = "";
                        $scope.currentPolicy.failureMileage = "";
                        $scope.currentPolicy.lastServiceMileage = "";
                        $scope.currentPolicy.lastServiceDate = "";
                        $scope.currentPolicy.policyMilege = 0.00;
                        $scope.eligiblePolicies = [];
                        $scope.partAreas = [];
                        $scope.claimItemList = [];

                        if (data.code === 'error') {
                            customErrorMessage(data.msg);
                        } else {
                            if (data.obj.length === 0) {
                                //ok and no policies
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.policiesFound'));
                            }
                            else if (data.obj.length === 1) {
                                $scope.currentPolicy.policyId = data.obj[0].Id;
                                $scope.GetServiceHistory($scope.currentPolicy.policyId);
                                $scope.currentPolicy.commodityCategoryId = data.obj[0].CommodityCategoryId;
                                $scope.currentPolicy.policyNo = data.obj[0].PolicyNo;
                                $scope.currentPolicy.makeId = data.obj[0].MakeId;
                                $scope.currentPolicy.customerName = data.obj[0].CustomerName;

                                if ($scope.isIloeProductSelected) {
                                    $scope.claimLimitation = data.obj[0].ClaimLimitation.toFixed(0);
                                    $scope.emivalue = data.obj[0].CalculateEMI.toFixed(2);
                                    $scope.iloeLoanInstallment = data.obj[0].LoneInstallmentValue;
                                    //$scope.currentPart.partName = data.obj[0].LoneInstallmentValue;
                                    //if (data.obj[0].LoneInstallmentValue != 0 || data.obj[0].LoneInstallmentValue != 1) {
                                    //    $scope.iloeLoanInstallment = [];
                                    //    var val1 = data.obj[0].LoneInstallmentValue - 1;
                                    //    var val2 = data.obj[0].LoneInstallmentValue - 2;
                                    //    $scope.iloeLoanInstallment.push(val1);
                                    //    $scope.iloeLoanInstallment.push(val2);
                                    //    $scope.iloeLoanInstallment.push(data.obj[0].LoneInstallmentValue);
                                    //}

                                }

                                $scope.selectedMakeChanged();
                                $scope.currentPolicy.modelId = data.obj[0].ModelId;
                                $scope.selectedModelChanged();
                                $scope.getAllPartAreas();
                            } else {
                                //multiple policies found
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.activePolicies'));
                                $scope.eligiblePolicies = data.obj;
                                MultipleEligiblePolicySelection = ngDialog.open({
                                    template: 'popUpMultipleEligiblePoliciesForVIN',
                                    className: 'ngdialog-theme-plain',
                                    closeByEscape: false,
                                    showClose: true,
                                    closeByDocument: false,
                                    scope: $scope
                                });
                            }

                        }
                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });

            } else {
                $scope.currentPolicy.policyId = emptyGuid();
                $scope.currentPolicy.commodityCategoryId = emptyGuid();
                $scope.currentPolicy.makeId = emptyGuid();
                $scope.currentPolicy.modelId = emptyGuid();
                $scope.currentPolicy.policyNo = "";
                $scope.currentPolicy.plateNumber = "";
                $scope.currentPolicy.customerName = "";
                $scope.currentPolicy.failureDate = "";
                $scope.currentPolicy.failureMileage = "";
                $scope.currentPolicy.lastServiceMileage = 0.00;
                $scope.currentPolicy.lastServiceDate = "";
                $scope.currentPolicy.policyMilege = 0.00;
                $scope.eligiblePolicies = [];
                $scope.partAreas = [];
                $scope.claimItemList = [];
            }
        }

        $scope.productSearchGridSearchCriterias = {
            policyNo: "",
            serialNo: "",
            customerName: "",
            dealerId: emptyGuid(),
        };

        $scope.productSearchGridloading = false;
        $scope.productSearchGridloadAttempted = false;
        var paginationOptionsProductSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsProductsIloe = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Service Contract No', field: 'PolicyNo', enableSorting: false, cellClass: 'columCss', },
               // { name: 'Vin or Serial', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Customer Name', field: 'CustomerName', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.vinNumberChanged2(' +
                        'row.entity.PolicyNo,row.entity.CalculateEMI,row.entity.SerialNo)"' +
                        ' class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load') +'</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        paginationOptionsProductSearchGrid.sort = null;
                    } else {
                        paginationOptionsProductSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getProductSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsProductSearchGrid.pageNumber = newPage;
                    paginationOptionsProductSearchGrid.pageSize = pageSize;
                    getProductSearchPage();
                });
            }
        };

        $scope.refreshProductSearch = function () {
            getProductSearchPage();
        }
        var getProductSearchPage = function () {
            $scope.productSearchGridloading = true;
            $scope.productSearchGridloadAttempted = false;
            var productSearchGridParam =
            {
                'paginationOptionsProductSearchGrid': paginationOptionsProductSearchGrid,
                'productSearchGridSearchCriterias': $scope.productSearchGridSearchCriterias,
                'userId': $localStorage.LoggedInUserId,
                'userType': $scope.userType,
                'productId': $scope.product.Id
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetProductsForSearchGrid',
                data: productSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsProductsIloe.data = response_arr.data;
                $scope.gridOptionsProductsIloe.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.productSearchGridloading = false;
                $scope.productSearchGridloadAttempted = true;

            });
        };


        $scope.selectedPartChanged = function (selectedItem) {
            $scope.currentPart.unitPrice = 0.00;
            if (!isGuid($scope.currentPolicy.modelId)) {
                customErrorMessage("Please select a model.");
                $scope.part.selected = false;
                return;
            }
            //console.log($scope.part.selected);
            let product = $scope.getSelectedProduct();
            if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                if (isGuid(selectedItem)) {
                    $scope.partNumberDisabled = true;
                    $scope.currentPart.partId = selectedItem;
                    $scope.partId = selectedItem;
                    //swal({ title: 'Loading...', text: 'Validating part information', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidatePartInformation',
                        data: {
                            "partId": selectedItem,
                            "dealerId": $scope.currentPolicy.dealerId,
                            "modelId": $scope.currentPolicy.modelId,
                            "makeId": $scope.currentPolicy.makeId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == null) {
                            customErrorMessage("Error occured.");
                        } else {
                            $scope.currentPart.isRelatedPartsAvailable = data.isRelatedPartsAvailable;
                            $scope.isSuggestionsAvailable = data.isRelatedPartsAvailable;
                            $scope.currentPart.allocatedHours = data.allocatedHours;

                            if (data.priceComment !== "ok") {
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.partPriceNotFound'));
                            } else {
                                if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                                    //$scope.emivalue = 0.00;

                                } else {
                                    $scope.currentPart.unitPrice = data.price;
                                }

                            }

                            if (data.modelComment !== 'ok') {
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.itemModel') + "(" + data.make + "-" + data.model + ")" + $filter('translate')('pages.claimSubmission.inforMessages.isNotMapPart') + data.make + "-" + data.model);
                            }
                        }

                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });
                } else {

                    $scope.currentPart.partId = emptyGuid();
                    $scope.currentPart.partNumber = '';
                    if (selectedItem !== '') {
                        $scope.partNumberDisabled = false;
                        $scope.currentPart.allocatedHours = 0;
                        $scope.currentPart.partName = selectedItem;
                        $scope.currentPart.partNumber = selectedItem;
                        $scope.currentPart.partId = emptyGuid();
                        $scope.part.selected = {
                            AllocatedHours: 0,
                            ApplicableForAllModels: true,
                            Id: emptyGuid(),
                            PartCode: '',
                            PartName: selectedItem,
                            PartNumber: ''
                        };

                    }
                }
            } else {
                if (isGuid(selectedItem.Id)) {
                    $scope.partNumberDisabled = true;
                    $scope.currentPart.partId = selectedItem.Id;
                    $scope.partId = selectedItem.Id;
                    $scope.currentPart.partNumber = selectedItem.PartNumber;
                    $scope.currentPart.partName = selectedItem.PartName;
                    swal({ title: 'Loading...', text: 'Validating part information', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidatePartInformation',
                        data: {
                            "partId": selectedItem.Id,
                            "dealerId": $scope.currentPolicy.dealerId,
                            "modelId": $scope.currentPolicy.modelId,
                            "makeId": $scope.currentPolicy.makeId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == null) {
                            customErrorMessage("Error occured.");
                        } else {
                            $scope.currentPart.isRelatedPartsAvailable = data.isRelatedPartsAvailable;
                            $scope.isSuggestionsAvailable = data.isRelatedPartsAvailable;
                            $scope.currentPart.allocatedHours = data.allocatedHours;

                            if (data.priceComment !== "ok") {
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.partPriceNotFound'));
                            } else {
                                if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                                    //$scope.emivalue = 0.00;
                                    $scope.currentPart.unitPrice = $scope.emivalue;
                                } else {
                                    $scope.currentPart.unitPrice = data.price;
                                }

                            }

                            if (data.modelComment !== 'ok') {
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.itemModel') + "(" + data.make + "-" + data.model + ")" + $filter('translate')('pages.claimSubmission.inforMessages.isNotMapPart') + data.make + "-" + data.model);
                            }
                        }

                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });
                } else {

                    $scope.currentPart.partId = emptyGuid();
                    $scope.currentPart.partNumber = '';
                    if (selectedItem !== '') {
                        $scope.partNumberDisabled = false;
                        $scope.currentPart.allocatedHours = 0;
                        $scope.currentPart.partName = selectedItem;
                        $scope.currentPart.partNumber = selectedItem;
                        $scope.currentPart.partId = emptyGuid();
                        $scope.part.selected = {
                            AllocatedHours: 0,
                            ApplicableForAllModels: true,
                            Id: emptyGuid(),
                            PartCode: '',
                            PartName: selectedItem,
                            PartNumber: ''
                        };

                    }
                }
            }


        }
        $scope.partNumberDisabled = true;
        $scope.getPartsByText = function (search) {
            var newSupes = $scope.parts.slice();
            if (search && newSupes.indexOf(search) === -1) {
                newSupes.unshift(search);
            }
            return newSupes;
        }

        $scope.selectedPartAreaChanged = function () {
            $scope.part.selected = undefined;
            $scope.currentPart.unitPrice = 0.00;
            $scope.currentPart.partNumber = '';
            $scope.currentPart.partQty = 1;
            $scope.currentPart.isRelatedPartsAvailable = false;
            let product = $scope.getSelectedProduct();
            if (!isGuid($scope.currentPolicy.makeId)) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.selectMake'));
                return false;
            }

            if (isGuid($scope.currentPart.partAreaId) && isGuid($scope.currentPolicy.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllPartsByPartAreaMakeId',
                    data: {
                        "partAreaId": $scope.currentPart.partAreaId,
                        "makeId": $scope.currentPolicy.makeId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null) {
                        $scope.parts = data;
                    }
                    else {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noPartsCategory'));
                        $scope.part.selected = "";
                        $scope.parts = [];

                    }
                    if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {
                        if ($scope.parts.length != 0) {
                            $scope.currentPart.partId = $scope.parts[0].Id;
                            $scope.currentPart.partNumber = $scope.parts[0].PartNumber;
                            $scope.selectedPartChanged($scope.currentPart.partId);
                            //$scope.selectedPartAreaChanged();
                        }
                    }

                }).error(function (data, status, headers, config) {

                }).finally(function () {
                });
            } else {

            }
        }

        $scope.addPartSuggestionToClaimList = function (partId) {
            if (isGuid(partId)) {
                var counter = -1;
                var isSuccess = false;
                angular.forEach($scope.partSuggestions, function (part) {
                    if (!isSuccess) {
                        counter++;
                        if (part.partId === partId) {
                            if (!parseFloat(part.price) || parseFloat(part.price) <= 0) {
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.setValidPart'));
                                return;
                            }
                            else if ((parseFloat(part.discount) && parseFloat(part.discount) > 0) ||
                                (parseFloat(part.goodWill) && parseFloat(part.goodWill) > 0)) {
                                var totalPrice = parseFloat(part.qty) * parseFloat(part.price);
                                var discountAmount = 0, goodWillAmount = 0;
                                //discount
                                if (parseFloat(part.discount) && parseFloat(part.discount) > 0) {
                                    if (part.discountType == "F") {
                                        if (totalPrice < parseFloat(part.discount)) {
                                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.discountAmount'));
                                            return;
                                        } else {
                                            discountAmount = parseFloat(part.discount);
                                        }
                                    } else {
                                        if (parseFloat(part.discount) > 100) {
                                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.discountPercentage'));
                                            return;
                                        } else {
                                            discountAmount = parseFloat(part.discount) / 100;
                                        }
                                    }
                                }
                                //good will
                                if (parseFloat(part.goodWill) && parseFloat(part.goodWill) > 0) {
                                    if (part.goodWillType == "F") {
                                        if (totalPrice < parseFloat(part.goodWill)) {
                                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.discountAmount'));
                                            return;
                                        } else {
                                            goodWillAmount = parseFloat(part.goodWill);
                                        }
                                    } else {
                                        if (parseFloat(part.goodWill) > 100) {
                                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.discountPercentage'));
                                            return;
                                        } else {
                                            goodWillAmount = parseFloat(part.goodWill) / 100;
                                        }
                                    }
                                }
                                //counts validation
                                if (totalPrice < (goodWillAmount + discountAmount)) {
                                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.sumOfDiscount'));
                                    return;
                                }
                            }

                            var grossPrice = parseFloat(part.qty) * parseFloat(part.price);
                            var discountAmount = part.discountType === "P" ? (grossPrice * part.discount / 100) : part.discount;
                            var goodWillAmount = part.goodWillType === "P" ? (grossPrice * part.goodWill / 100) : part.goodWill;


                            var nextId = $scope.claimItemList.length;
                            var claimItem = {
                                id: (nextId + 1),
                                partAreaId: part.partAreaId,
                                partId: part.partId,
                                itemName: part.partName,
                                itemNumber: part.partNumber,
                                qty: part.qty,
                                unitPrice: parseFloat(part.price),
                                totalGrossPrice: grossPrice.toFixed(2),
                                discountRate: part.discount,
                                discountAmount: discountAmount,
                                isDiscountPercentage: part.discountType === "P" ? true : false,
                                goodWillRate: part.goodWill,
                                goodWillAmount: goodWillAmount,
                                isGoodWillPercentage: part.goodWillType === "P" ? true : false,
                                parentId: emptyGuid(),
                                totalPrice: (grossPrice -
                                    (parseFloat(discountAmount) + parseFloat(goodWillAmount))
                                ).toFixed(2),
                                itemType: 'P',
                                remarks: part.remark
                            };
                            $scope.claimItemList.push(claimItem);
                            if (parseFloat(part.allocatedHours) && parseFloat(part.allocatedHours) > 0) {
                                $scope.labourCharge.chargeType = "H";
                                $scope.labourCharge.hours = part.allocatedHours;
                                $scope.labourCharge.description = "This allocated to part no -" + part.partNumber;
                                $scope.labourCharge.partId = claimItem.partId;
                                if (parseFloat($scope.dealer.hourlyRate) > 0)
                                    $scope.labourCharge.hourlyRate = $scope.dealer.hourlyRate;

                                $scope.addLabourCharge();
                            }
                            //update total
                            $scope.calculateTotalClaimAmount();
                            isSuccess = true;
                        }
                    }
                });
                if (isSuccess) {
                    $scope.partSuggestions.splice(counter, 1);
                }
                if ($scope.partSuggestions.length === 0) {
                    PartSuggestionPopup.close();
                }
            }
        }

        $scope.validateOtherTireDetails = function () {
            var isValid = true;

            if ($scope.currentPolicy.failureDate == "") {
                $scope.validate_failureDate = "has-error";
                isValid = false;
            } else {
                $scope.validate_failureDate = "";
            }

            if ($scope.currentPolicy.failureMileage == "") {
                $scope.validate_failureMileage = "has-error";
                isValid = false;
            } else {
                $scope.validate_failureMileage = "";
            }

            if ($scope.dealerInvoiceTireDetails.customerComplaintId == "") {
                $scope.validate_customerComplaintId = "has-error";
                isValid = false;
            } else {
                $scope.validate_customerComplaintId = "";
            }

            if ($scope.dealerInvoiceTireDetails.dealerCommentId == "") {
                $scope.validate_dealerCommentId = "has-error";
                isValid = false;
            } else {
                $scope.validate_dealerCommentId = "";
            }

            // validate dept
            $scope.validate_unusedTyreDepthFrontLeft = "";
            $scope.validate_unusedTyreDepthFrontRight = "";
            $scope.validate_unusedTyreDepthBackLeft = "";
            $scope.validate_unusedTyreDepthBackRight = "";
            $scope.validate_unusedTyreDepthDown = "";


            $scope.validate_seriaFrontlLeft = "";
            $scope.validate_serialFrontRight = "";
            $scope.validate_serialBackLeft = "";
            $scope.validate_serialBackRight = "";
            $scope.validate_serialDown = "";

            if ($scope.isFrontLeftTireDetailsVisible) {
                if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft.length == 0) {
                    $scope.validate_unusedTyreDepthFrontLeft = "has-error";
                    isValid = false;
                }
                if ($scope.dealerInvoiceTireDetails.seriaFrontlLeft == 0) {
                    $scope.validate_seriaFrontlLeft = "has-error";
                    isValid = false;
                }
            }

            if ($scope.isFrontRightTireDetailsVisible) {
                if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight.length == 0) {
                    $scope.validate_unusedTyreDepthFrontRight = "has-error";
                    isValid = false;
                }
                if ($scope.dealerInvoiceTireDetails.serialFrontRight == 0) {
                    $scope.validate_serialFrontRight = "has-error";
                    isValid = false;
                }
            }
            if ($scope.isBackLeftTireDetailsVisible) {
                if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft.length == 0) {
                    $scope.validate_unusedTyreDepthBackLeft = "has-error";
                    isValid = false;
                }
                if ($scope.dealerInvoiceTireDetails.serialBackLeft == 0) {
                    $scope.validate_serialBackLeft = "has-error";
                    isValid = false;
                }
            }
            if ($scope.isBackRightTireDetailsVisible) {
                if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight.length == 0) {
                    $scope.validate_unusedTyreDepthBackRight = "has-error";
                    isValid = false;
                }
                if ($scope.dealerInvoiceTireDetails.serialBackRight == 0) {
                    $scope.validate_serialBackRight = "has-error";
                    isValid = false;
                }

            }
            if ($scope.isDownTireDetailsVisible ) {
                if ($scope.dealerInvoiceTireDetails.unusedTyreDepthDown.length == 0) {
                    $scope.validate_unusedTyreDepthDown = "has-error";
                    isValid = false;
                }
                if ($scope.dealerInvoiceTireDetails.downSerial == 0) {
                    $scope.validate_serialDown = "has-error";
                    isValid = false;
                }

            }


            if (!$scope.isFrontLeftTireDetailsVisible && !$scope.isFrontRightTireDetailsVisible && !$scope.isBackLeftTireDetailsVisible && !$scope.isBackRightTireDetailsVisible && !$scope.isDownTireDetailsVisible) {
                customErrorMessage("Please Enter Tyre Details");
                isValid = false;
            }


            if (!$scope.isEditClaim) {
                if ($scope.currentPolicy.docAttachmentType1 == null ||
                    $scope.currentPolicy.docAttachmentType1 == 'undefined' ||
                    $scope.currentPolicy.docAttachmentType1 == "") {
                    customErrorMessage("Please Select Attachment Type");
                    return false;
                }
            }


            $scope.validateOtherTireMilege = function () {
                var isValid = true;
                if ($scope.currentPolicy.policyMilege > $scope.currentPolicy.failureMileage) {

                    return false;
                }

                return isValid;
            }


            //if (!$scope.frontPositionDisabled) {

            //    if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft != "" && $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft != null) {
            //        if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft < 3) {
            //            customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
            //            return false;
            //        }
            //    }

            //    if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight != "" && $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight != null) {
            //        if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight < 3) {
            //            customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
            //            return false;
            //        }
            //    }


            //    if ($scope.dealerInvoiceTireDetails.seriaFrontlLeft == "" && $scope.dealerInvoiceTireDetails.serialFrontRight == "" &&
            //        $scope.dealerInvoiceTireDetails.serialBackLeft == "" && $scope.dealerInvoiceTireDetails.serialBackRight == "") {
            //        customErrorMessage("Please enter tyre Article Number.");
            //        return false;
            //    }

            //    //if ($scope.dealerInvoiceTireDetails.seriaFrontlLeft == "") {
            //    //    $scope.validate_seriaFrontlLeft = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_seriaFrontlLeft = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft == "") {
            //    //    $scope.validate_unusedTyreDepthFrontLeft = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_unusedTyreDepthFrontLeft = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.serialFrontRight == "") {
            //    //    $scope.validate_serialFrontRight = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_serialFrontRight = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight == "") {
            //    //    $scope.validate_unusedTyreDepthFrontRight = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_unusedTyreDepthFrontRight = "";
            //    //}

            //}
            //if (!$scope.backPositionDisabled) {

            //    if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft != "" && $scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft != null) {
            //        if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft < 3) {
            //            customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
            //            return false;
            //        }
            //    }
            //    if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight != "" && $scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight != null) {
            //        if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight < 3) {
            //            customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
            //            return false;
            //        }
            //    }
            //    //if ($scope.dealerInvoiceTireDetails.serialBackLeft == "") {
            //    //    $scope.validate_serialBackLeft = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_serialBackLeft = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft == "") {
            //    //    $scope.validate_unusedTyreDepthBackLeft = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_unusedTyreDepthBackLeft = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.serialBackRight == "") {
            //    //    $scope.validate_serialBackRight = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_serialBackRight = "";
            //    //}

            //    //if ($scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight == "") {
            //    //    $scope.validate_unusedTyreDepthBackRight = "has-error";
            //    //    isValid = false;
            //    //} else {
            //    //    $scope.validate_unusedTyreDepthBackRight = "";
            //    //}

            //}


            return isValid;

        }


        $scope.updateOtherTyreClaim = function () {
            for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                if ($scope.docUploader.queue[i].documentType === "TYRE" || $scope.docUploader.queue[i].documentType === "Tyre") {
                    $scope.validateRequirmentTyre = true;
                }
                if ($scope.docUploader.queue[i].documentType === "NUMBER PLATE" || $scope.docUploader.queue[i].documentType === "Number Plate") {
                    $scope.validateRequirmentNumberPlate = true;
                }
            }

            if (!$scope.validateRequirmentNumberPlate || !$scope.validateRequirmentTyre) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.attachImages'));
                return false;
            }

            var rejectStatus = false;
            rejectStatus = $scope.checkRejected();

            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateOtherTireDetails()) {
                    if ($scope.validateOtherTireMilege()) {
                        $scope.currentPolicy.lastServiceDate = "1/1/1753";
                        if ($scope.docUploader.queue.length === 0) {
                            swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.submittingClaim'), showConfirmButton: false });
                            $scope.generateClaimTyreDetails();
                            var data = {
                                Id:$scope.claimId,
                                requestedUserId: $rootScope.LoggedInUserId,
                                dealerId: $scope.currentPolicy.dealerId,
                                policyId: $scope.currentPolicy.policyId,
                                policyDetails: $scope.currentPolicy,
                                attachmentIds: $scope.uploadedDocIds,
                                claimDate: new Date().toISOString().slice(0, 10),
                                OtherTireDetails: $scope.dealerInvoiceTireDetails,
                                InvoiceCodeId: $scope.dealerInvoiceTireDetails.InvoiceCodeId,
                                tyreDetails: $scope.tyreDetails,
                                Reject: rejectStatus
                            };

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/claim/UpdateOtherTireClaim',
                                data: {
                                    "claimData": data
                                },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data === 'ok') {
                                    swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), $filter('translate')('pages.claimSubmission.inforMessages.claimUpdated'), "success");
                                    $scope.clearAll();
                                    $rootScope.claimId = undefined;
                                    $scope.claimId = undefined;
                                    $scope.isEditClaim = false;
                                    $localStorage.claimSubmissionTempId = undefined;
                                    $scope.loadInitailData();
                                    $scope.validateRequirmentTyre = false;
                                    $scope.validateRequirmentNumberPlate = false;
                                    //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                } else {
                                    swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                                }
                            }).error(function (data, status, headers, config) {
                                swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                            }).finally(function () {

                            });
                        } else {
                            angular.forEach($scope.docUploader.queue, function (doc) {
                                if (doc.isUploaded != undefined && doc.isUploaded) {
                                    $scope.uploadedDocIds.push(doc.id);
                                    console.log('befor filter');
                                    console.log($scope.docUploader.queue);
                                    $scope.docUploader.queue = $scope.docUploader.queue.filter(que => que.id != doc.id);
                                    console.log('after filter');
                                    console.log($scope.docUploader.queue);
                                } else {
                                    doc.file.name = doc.file.name + '@@' + doc.documentType;
                                }
                            });


                            if ($scope.docUploader.queue.length > 0) {
                                $scope.docUploader.uploadAll();
                            } else {
                                $scope.updateOtherTyreClaim();
                            }

                        }
                    }
                    else {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.milegeCannotLower'));
                    }

                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectPolicy'));
            }

        }


        $scope.submitOtherTireClaim = function () {

            for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                if ($scope.docUploader.queue[i].documentType === "TYRE" || $scope.docUploader.queue[i].documentType === "Tyre") {
                    $scope.validateRequirmentTyre = true;
                }
                if ($scope.docUploader.queue[i].documentType === "NUMBER PLATE" || $scope.docUploader.queue[i].documentType === "Number Plate" ) {
                    $scope.validateRequirmentNumberPlate = true;
                }
            }

            if (!$scope.validateRequirmentNumberPlate || !$scope.validateRequirmentTyre) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.attachImages'));
                return false;
            }

            if ($scope.currentPolicy.commodityTypeId == emptyGuid() || $scope.currentPolicy.commodityTypeId == undefined) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectCommodityType'));
                return false;
            }

            var rejectStatus = false;
            rejectStatus = $scope.checkRejected();


            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateOtherTireDetails()) {
                    if ($scope.validateOtherTireMilege()) {
                        $scope.currentPolicy.lastServiceDate = "1/1/1753";
                        if ($scope.docUploader.queue.length === 0) {
                            swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.claimSubmit'), showConfirmButton: false });
                            $scope.generateClaimTyreDetails();
                            var data = {
                                requestedUserId: $rootScope.LoggedInUserId,
                                dealerId: $scope.currentPolicy.dealerId,
                                policyId: $scope.currentPolicy.policyId,
                                policyDetails: $scope.currentPolicy,
                                attachmentIds: $scope.uploadedDocIds,
                                claimDate: new Date().toISOString().slice(0, 10),
                                OtherTireDetails: $scope.dealerInvoiceTireDetails,
                                InvoiceCodeId: $scope.dealerInvoiceTireDetails.InvoiceCodeId,
                                tyreDetails:  $scope.tyreDetails,
                                Reject: rejectStatus
                            };

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/claim/SubmitOtherTireClaim',
                                data: {
                                    "claimOtherData": data
                                },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data === 'ok') {
                                    swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), $filter('translate')('pages.claimSubmission.inforMessages.saveClaims'), "success");
                                    $scope.clearAll();
                                    $scope.loadInitailData();
                                    $scope.validateRequirmentTyre = false;
                                    $scope.validateRequirmentNumberPlate = false;
                                    //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                } else {
                                    swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                                }
                            }).error(function (data, status, headers, config) {
                            }).finally(function () {

                            });
                        } else {

                            angular.forEach($scope.docUploader.queue, function (doc) {
                                    doc.file.name = doc.file.name + '@@' + doc.documentType;
                            });
                            $scope.docUploader.uploadAll();


                        }
                    }
                    else {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.milegeCannotLower'));
                    }

                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectPolicy'));
            }
        }



        $scope.generateClaimTyreDetails = function () {
            $scope.tyreDetails = [];
            var selectedTyreDetails = [];
            if ($scope.isFrontLeftTireDetailsVisible) {
                selectedTyreDetails.push({
                    "Position": "FL",
                    "ArticleNo": $scope.dealerInvoiceTireDetails.seriaFrontlLeft,
                    "UnUsedTireDepth": $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft
                });
            }
            if ($scope.isFrontRightTireDetailsVisible ) {
                selectedTyreDetails.push({
                    "Position": "FR",
                    "ArticleNo": $scope.dealerInvoiceTireDetails.serialFrontRight,
                    "UnUsedTireDepth": $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight
                });
            }
            if ($scope.isBackLeftTireDetailsVisible) {
                selectedTyreDetails.push({
                    "Position": "BL",
                    "ArticleNo": $scope.dealerInvoiceTireDetails.serialBackLeft,
                    "UnUsedTireDepth": $scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft
                });
            }
            if ($scope.isBackRightTireDetailsVisible ) {
                selectedTyreDetails.push({
                    "Position": "BR",
                    "ArticleNo": $scope.dealerInvoiceTireDetails.serialBackRight,
                    "UnUsedTireDepth": $scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight
                });
            }
            if ($scope.isDownTireDetailsVisible) {
                selectedTyreDetails.push({
                    "Position": "D",
                    "ArticleNo": $scope.dealerInvoiceTireDetails.downSerial,
                    "UnUsedTireDepth": $scope.dealerInvoiceTireDetails.unusedTyreDepthDown
                });
            }
            console.table(selectedTyreDetails);
            $scope.tyreDetails = selectedTyreDetails;
        }


        $scope.checkRejected = function () {
            var status = false;
            if ($scope.isFrontLeftTireDetailsVisible) {
                if (Number($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft) <= 3) {
                    status = true;
                }
            }

            if ($scope.isFrontRightTireDetailsVisible) {
                if (Number($scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight) <= 3) {
                    status = true;
                }
            }

            if ($scope.isBackLeftTireDetailsVisible) {
                if (Number($scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft) <= 3) {
                    status = true;
                }
            }

            if ($scope.isBackRightTireDetailsVisible) {
                if (Number($scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight) <= 3) {
                    status = true;
                }
            }

            if ($scope.isDownTireDetailsVisible) {
                if (Number($scope.dealerInvoiceTireDetails.unusedTyreDepthDown) <= 3) {
                    status = true;
                }
            }
            return status;

        }


        $scope.customerGrossSalarycheckBoxChangesYES = function (customerGrossSalaryapproved) {
            if (customerGrossSalaryapproved == true) {
                $scope.customerGrossSalaryYes = true;
            } else {
                $scope.customerGrossSalaryYes = false;
            }

        }

        $scope.customerGrossSalarycheckBoxChangesNo = function (customerGrossSalaryisaapproved) {
            if (customerGrossSalaryisaapproved == true) {
                $scope.customerGrossSalaryYes = false;
            } else {
                $scope.customerGrossSalaryYes = false;
            }

        }

        $scope.resetAll = function () {
            $scope.clearAll();
        }
        $scope.checkBoxChangesYES = function (visaapproved) {
            if (visaapproved == true) {
                $scope.employeeApprovedYes = true;
            } else {
                $scope.employeeApprovedYes = false;
            }

        }
        $scope.checkBoxChangesNO = function (isaapprovedno) {
            if (isaapprovedno == true) {
                $scope.employeeApprovedYes = false;
            } else {
                $scope.employeeApprovedYes = false;
            }

        }

        $scope.checkBoxBankApprovedChangesYES = function (bankapproved) {
            if (bankapproved == true) {
                $scope.isbankApproved = true;
            } else {
                $scope.isbankApproved = false;
            }

        }

        $scope.checkBoxBankApprovedChangesNO = function (bankapprovedNo) {
            if (bankapprovedNo == false) {
                $scope.isbankApproved = false;
            } else {
                $scope.isbankApproved = false;
            }
        }

        $scope.checkBoximpendingunemploymentApprovedChangesYES = function (impendingunemploymentapproved) {
            if (impendingunemploymentapproved == true) {
                $scope.impendingunemploymentRisvisible = true;
            } else {
                $scope.impendingunemploymentRisvisible = false;
            }
        }

        $scope.checkBoximpendingunemploymentApprovedChangesNO = function (impendingunemploymentapprovedNo) {
            if (impendingunemploymentapprovedNo == true) {
                $scope.impendingunemploymentRisvisible = false;
            } else {
                $scope.impendingunemploymentRisvisible = false;
            }
        }

        $scope.checkSalaryslipsforlast6monthsYES = function (Salaryslipsforlast6monthsapproved) {
            if (Salaryslipsforlast6monthsapproved == true) {
                $scope.salaryslipsforlast6monthsRisvisible = true;
            } else {
                $scope.salaryslipsforlast6monthsRisvisible = false;
            }
        }

        $scope.checkSalaryslipsforlast6monthsNO = function (Salaryslipsforlast6monthsNo) {
            if (Salaryslipsforlast6monthsNo == true) {
                $scope.salaryslipsforlast6monthsRisvisible = false;
            } else {
                $scope.salaryslipsforlast6monthsRisvisible = false;
            }
        }

        $scope.checkSupportingbankstatementsYES = function (Salaryslipsforlast6monthsapproved) {
            if (Salaryslipsforlast6monthsapproved == true) {
                $scope.supportingbankstatementsRisvisible = true;
            } else {
                $scope.supportingbankstatementsRisvisible = false;
            }
        }

        $scope.checkSupportingbankstatementsNO = function (Salaryslipsforlast6monthsNo) {
            if (Salaryslipsforlast6monthsNo == true) {
                $scope.supportingbankstatementsRisvisible = false;
            } else {
                $scope.supportingbankstatementsRisvisible = false;
            }
        }

        $scope.checkProofofpaymentYES = function (Proofofpaymentapproved) {
            if (Proofofpaymentapproved == true) {
                $scope.proofofpaymentRisvisible = true;
            } else {
                $scope.proofofpaymentRisvisible = false;
            }
        }

        $scope.checkProofofpaymentNO = function (ProofofpaymentapprovedNo) {
            if (ProofofpaymentapprovedNo == true) {
                $scope.proofofpaymentRisvisible = false;
            } else {
                $scope.proofofpaymentRisvisible = false;
            }
        }

        $scope.checkCopyofPassportYES = function (copyofPassportapproved) {
            if (copyofPassportapproved == true) {
                $scope.copyofPassportRisvisible = true;
            } else {
                $scope.copyofPassportRisvisible = false;
            }
        }

        $scope.checkCopyofPassportNO = function (copyofPassportapprovedNo) {
            if (copyofPassportapprovedNo == true) {
                $scope.copyofPassportRisvisible = false;
            } else {
                $scope.copyofPassportRisvisible = false;
            }
        }

        $scope.checkletterforinsurancecompanyYES = function (letterforinsurancecompanyapproved) {
            if (letterforinsurancecompanyapproved == true) {
                $scope.letterforinsurancecompanyRisvisible = true;
            } else {
                $scope.letterforinsurancecompanyRisvisible = false;
            }
        }

        $scope.checkletterforinsurancecompanyNO = function (letterforinsurancecompanyapprovedNo) {
            if (letterforinsurancecompanyapprovedNo == true) {
                $scope.letterforinsurancecompanyRisvisible = false;
            } else {
                $scope.letterforinsurancecompanyRisvisible = false;
            }
        }

        $scope.checkreportfromImmigrationYES = function (reportfromImmigrationapproved) {
            if (reportfromImmigrationapproved == true) {
                $scope.reportfromImmigrationRisvisible = true;
            } else {
                $scope.reportfromImmigrationRisvisible = false;
            }
        }

        $scope.checkreportfromImmigrationNO = function (reportfromImmigrationapprovedNo) {
            if (reportfromImmigrationapprovedNo == true) {
                $scope.reportfromImmigrationRisvisible = false;
            } else {
                $scope.reportfromImmigrationRisvisible = false;
            }
        }

        $scope.checkNewemploymentcontractYES = function (Newemploymentcontractapproved) {
            if (Newemploymentcontractapproved == true) {
                $scope.newemploymentcontractRisvisible = true;
            } else {
                $scope.newemploymentcontractRisvisible = false;
            }
        }

        $scope.checkNewemploymentcontractNO = function (NewemploymentcontractapprovedNo) {
            if (NewemploymentcontractapprovedNo == true) {
                $scope.newemploymentcontractRisvisible = false;
            } else {
                $scope.newemploymentcontractRisvisible = false;
            }
        }
        $scope.ValidateIloeAttchments = function () {

            var isValid = true;
            if ($scope.customerGrossSalaryYes == undefined) {
                isValid = false;
            }
            else if (!$scope.employeeApprovedYes) {
                isValid = false;
            }
            else if ($scope.isbankApproved == undefined) {
                isValid = false;
            }
            else if ($scope.impendingunemploymentRisvisible == undefined) {

                isValid = false;
            }
            else if ($scope.salaryslipsforlast6monthsRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.supportingbankstatementsRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.proofofpaymentRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.copyofPassportRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.letterforinsurancecompanyRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.reportfromImmigrationRisvisible == undefined) {
                isValid = false;
            }
            else if ($scope.newemploymentcontractRisvisible == undefined) {
                isValid = false;
            }
            return isValid;
        }

        $scope.ConfirmButtonInILOE = function () {
            if ($scope.ValidateIloeAttchments()) {
                if ($scope.docUploader.queue.length === 0) {
                    swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.claimSubmit'), showConfirmButton: false });
                    $scope.currentPolicy.failureMileage = 1;
                    $scope.currentPolicy.failureDate = "1/1/1753";
                    $scope.currentPolicy.lastServiceDate = "1/1/1753";
                    $scope.currentPolicy.lastServiceMileage = 1;


                    var data = {
                        claimItemList: $scope.claimItemList,
                        totalClaimAmount: $scope.totalClaimAmount,
                        requestedUserId: $rootScope.LoggedInUserId,
                        dealerId: $scope.currentPolicy.dealerId,
                        policyId: $scope.currentPolicy.policyId,
                        complaint: $scope.complaint,
                        attachmentIds: $scope.uploadedDocIds,
                        claimMileage: $scope.currentPolicy.failureMileage,
                        claimDate: new Date().toISOString().slice(0, 10),
                        policyDetails: $scope.currentPolicy
                    };


                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/SubmitClaim',
                        data: {
                            "claimData": data
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data === 'ok') {
                            swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), $filter('translate')('pages.claimSubmission.inforMessages.saveClaims'), "success");
                            $scope.clearAll();
                            dialogChangePassword.close();
                            //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                        } else {
                            swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                            dialogChangePassword.close();
                        }
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });

                } else {

                    for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                        $scope.docUploader.queue[i].file.name = $scope.docUploader.queue[i].file.name + '@@' + $scope.docUploader.queue[i].documentType;
                    }
                    $scope.docUploader.uploadAll();
                }
            }
            else {

                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.uploadNeccessary'));
            }
        }
        $scope.partListExcistanceCheck = function () {
            $scope.partListExcistances = false;
            angular.forEach($scope.claimItemList, function (value) {
                if (value.itemType == 'P') {
                    $scope.partListExcistances= true;
                }
                return $scope.partListExcistances;
            });
        }

        $scope.getSelectedProduct = function () {
            let currentProduct = null;
            angular.forEach($scope.Products, function (pValue) {
                if ($scope.product.Id === pValue.Id) {
                    currentProduct = pValue;
                }
            });
            return currentProduct;
        }


        $scope.submitClaim = function () {

            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.claimItemList.length > 0) {
                    $scope.partListExcistanceCheck()
                    if ($scope.partListExcistances) {

                        let product = $scope.getSelectedProduct();
                        if (product != undefined && product != null && product.ProductTypeCode == "ILOE") {

                        $scope.ValidationILOEPopup();

                    } else {
                        if ($scope.complaint.customer.trim().length > 0
                            && $scope.complaint.dealer.trim().length > 0) {

                            // validate mileage less than  policy sale mileage
                                if ($scope.currentPolicy.policyMilege > $scope.currentPolicy.failureMileage){
                                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.milegeCannotLower'));
                                    return false;
                                }

                            if ($scope.docUploader.queue.length === 0) {
                                swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.claimSubmit'), showConfirmButton: false });


                                if ($scope.currentPolicy.lastServiceMileage == "" && $scope.required.lastServiceMileage==false) {
                                    $scope.currentPolicy.lastServiceMileage = 0;
                                }

                                if ($scope.currentPolicy.lastServiceDate == "" && $scope.required.lastServiceDate == false) {
                                    $scope.currentPolicy.lastServiceDate = "1/1/1753";
                                }


                                var data = {
                                    claimItemList: $scope.claimItemList,
                                    totalClaimAmount: $scope.totalClaimAmount,
                                    requestedUserId: $rootScope.LoggedInUserId,
                                    dealerId: $scope.currentPolicy.dealerId,
                                    policyId: $scope.currentPolicy.policyId,
                                    complaint: $scope.complaint,
                                    attachmentIds: $scope.uploadedDocIds,
                                    claimMileage: $scope.currentPolicy.failureMileage,
                                    claimDate: new Date().toISOString().slice(0, 10),
                                    policyDetails: $scope.currentPolicy
                                };
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/claim/SubmitClaim',
                                    data: {
                                        "claimData": data
                                    },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    if (data === 'ok') {
                                        swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), $filter('translate')('pages.claimSubmission.inforMessages.saveClaims'), "success");
                                        $scope.clearAll();
                                        //   TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                    } else {
                                        swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                                    }
                                }).error(function (data, status, headers, config) {
                                }).finally(function () {

                                });
                            } else {

                                for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                                    $scope.docUploader.queue[i].file.name = $scope.docUploader.queue[i].file.name + '@@' + $scope.docUploader.queue[i].documentType;
                                }
                                $scope.docUploader.uploadAll();
                            }
                        } else {
                            customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.enterCustomerComplaint'));
                        }
                    }

                    }
                else{

                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.addClaimItems'));
                }

                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.addClaimItems'));
                }

            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectPolicy'));
            }
        }
        $scope.downloadAttachmentUploaded = function (ref) {
            if (ref != '') {
                swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.preparingDownload'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Upload/DownloadAttachment',
                    data: { 'fileRef': ref },
                    headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt },
                    responseType: 'arraybuffer',
                }).success(function (data, status, headers, config) {
                    headers = headers();
                    var filename = headers['x-filename'];
                    var contentType = headers['content-type'];
                    var linkElement = document.createElement('a');
                    try {
                        var blob = new Blob([data], { type: contentType });
                        var url = window.URL.createObjectURL(blob);

                        linkElement.setAttribute('href', url);
                        linkElement.setAttribute("download", filename);

                        var clickEvent = new MouseEvent("click", {
                            "view": window,
                            "bubbles": true,
                            "cancelable": false
                        });
                        linkElement.dispatchEvent(clickEvent);
                    } catch (ex) {
                        console.log(ex);
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.clearAll = function () {
            $scope.frontLeftPositionDisabled = true;
            $scope.frontRightPositionDisabled = true;
            $scope.backLeftPositionDisabled = true;
            $scope.backRightPositionDisabled = true;
            $scope.sparePositionDisable = true;

            $scope.frontLeftClaimed = false;
            $scope.frontRightClaimed = false;
            $scope.backLeftClaimed = false;
            $scope.backRightClaimed = false;
            $scope.spareClaimed = false;


            $scope.customerType = 0;
            $scope.selectAlltyreLabel = "Select All Tyres";
            $scope.selectAllTyresVisible = false;
            $scope.isEditClaim = false;
            $scope.docUploader.queue = [];

            $scope.currentPolicy = {
                policyId: emptyGuid(),
                commodityCategoryId: emptyGuid(),
                commodityTypeId: emptyGuid(),
                makeId: emptyGuid(),
                modelId: emptyGuid(),
                dealerId: emptyGuid(),
                claimMileage: "",
                claimDate: "",
                policyNo: "",
                vinNo: "",
                plateNumber: "",
                customerName: "",
                failureDate: "",
                failureMileage: "",
                lastServiceMileage: 0.00,
                lastServiceDate: "",
                policyMilege: 0.00

            };
            $scope.claimItem = {
                id: 0,
                itemId: emptyGuid(),
                itemName: '',
                itemNumber: '',
                qty: 0,
                unitPrice: 0.00,
                totalGrossPrice: 0.00,
                discountRate: 0.00,
                discountAmount: 0.00,
                isDiscountPercentage: false,
                goodWillRate: 0.00,
                goodWillAmount: 0.00,
                isGoodWillPercentage: false,
                parentId: emptyGuid(),
                totalPrice: 0.00,
                itemType: '',
                remarks: ''
            };


            $scope.currentPart = {
                id: 0,
                partAreaId: emptyGuid(),
                partId: emptyGuid(),
                partNumber: '',
                partName: '',
                partQty: 1,
                unitPrice: '',
                remark: '',
                isRelatedPartsAvailable: false,
                allocatedHours: 0,
                goodWillType: 'NA',
                goodWillValue: 0.00,
                goodWillAmount: 0.00,
                discountType: 'NA',
                discountValue: 0.00,
                discountAmount: 0.00

            };
            $scope.labourCharge = {
                chargeType: 'H',
                hourlyRate: 0.00,
                hours: 0,
                totalAmount: 0.00,
                description: '',
                partId: 0,
                goodWillType: 'NA',
                goodWillValue: 0.00,
                goodWillAmount: 0.00,
                discountType: 'NA',
                discountValue: 0.00,
                discountAmount: 0.00
            };
            $scope.sundry = {
                name: '',
                value: 0.00
            };
            $scope.policyDetails = {
                customerName: '',
                commodityType: '',
                insuaranceProductName: '',
                policyNo: '',
                startDate: '',
                endDate: ''
            };

            $scope.userType = '';
            $scope.totalClaimAmount = 0.00;

            $scope.serviceRecord = {
                id: emptyGuid(),
                serviceNumber: '',
                milage: '',
                remarks: '',
                serviceDate: ''
            };
            $scope.complaint = {
                customer: '',
                dealer: ''
            };



            $scope.part = {};
            $scope.partAreas = [];
            $scope.parts = [];
            $scope.itemServiceRecords = [];
            $scope.claimItemList = [];
            $scope.partSuggestions = [];
            $scope.isSuggestionsAvailable = false;
            $scope.partId = emptyGuid();


            $scope.dealerInvoiceTireDetails = {
                InvoiceCodeId: emptyGuid(),
                customerComplaint: '',
                dealerComment: '',
                serialFrontRight: '',
                unusedTyreDepthFrontRight: '',
                serialBackRight: '',
                unusedTyreDepthBackRight: '',
                serialBackLeft: '',
                unusedTyreDepthBackLeft: '',
                seriaFrontlLeft: '',
                unusedTyreDepthFrontLeft: '',
                frontPositionDisabled: false,
                backPositionDisabled: false,
                unusedTyreDepthDown: '',
                downSerial: ''
            }

            $scope.isFrontLeftTireDetailsVisible = false;
            $scope.isFrontRightTireDetailsVisible = false;

            $scope.isBackRightTireDetailsVisible = false;
            $scope.isBackLeftTireDetailsVisible = false;
            $scope.isDownTireDetailsVisible = false;

            $scope.frontPositionDisabled = false;
            $scope.backPositionDisabled = false;
        }
        $scope.policySearchGridSearchCriterias = {
            policyNo: "",
            customerName: "",
            mobileNumber: "",
            plateNo: ""
        };


        $scope.policySearchGridloading = false;
        $scope.policySearchGridloadAttempted = false;
        var paginationOptionsPolicySearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };



        $scope.gridOptionsPolicy = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: columnForSearchGrid,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length === 0) {
                        paginationOptionsPolicySearchGrid.sort = null;
                    } else {
                        paginationOptionsPolicySearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getPolicySearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsPolicySearchGrid.pageNumber = newPage;
                    paginationOptionsPolicySearchGrid.pageSize = pageSize;
                    getPolicySearchPage();
                });
            }
        };

        $scope.refreshPolicySearch = function () {
            getPolicySearchPage();
        }
        var getPolicySearchPage = function () {
            $scope.policySearchGridloading = true;
            $scope.policySearchGridloadAttempted = false;
            var policySearchGridParam =
            {
                'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                'userId': $localStorage.LoggedInUserId,
                'userType': $scope.userType
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetPoliciesNewForSearchGrid',
                data: policySearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsPolicy.data = response_arr.data;
                $scope.gridOptionsPolicy.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.policySearchGridloading = false;
                $scope.policySearchGridloadAttempted = true;

            });
        };
        $scope.PolicySearchPopup = function () {
            var paginationOptionsPolicySearchGrid = {
                pageNumber: 1,
                sort: null
            };
            $scope.policySearchGridSearchCriterias = {
                policyNo: "",
                customerName: "",
                mobileNumber: "",
                plateNo:"",
                dealerId: emptyGuid(),
            };
            getPolicySearchPage();
            SearchPolicyPopup = ngDialog.open({
                template: 'popUpSearchPolicy',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            return true;
        };

        $scope.loadPolicyNew = function (policyNo) {
            SearchPolicyPopup.close();
            $scope.currentPolicy.policyNo = policyNo;
            angular.element('#policyNumber').focus();
            $scope.vinNumberChanged();
        }

        $scope.resetSearch = function () {
            $scope.policySearchGridSearchCriterias.policyNo= '';
            $scope.policySearchGridSearchCriterias.customerName = '';
            $scope.policySearchGridSearchCriterias.mobileNumber = '';
            $scope.policySearchGridSearchCriterias.plateNo = '';
        }

        $scope.selectAllTyresPolicy = function () {

                if ($scope.isFrontLeftTireDetailsVisible) {
                    $scope.openFrontTireDetails('L', 'F');
                }
                if ($scope.isFrontRightTireDetailsVisible) {
                    $scope.openFrontTireDetails('R', 'F');
                }
                if ($scope.isBackLeftTireDetailsVisible) {
                    $scope.openFrontTireDetails('L', 'B');
                }
                if ($scope.isBackRightTireDetailsVisible) {
                    $scope.openFrontTireDetails('R', 'B');
                }
                if ($scope.isDownTireDetailsVisible) {
                    $scope.openFrontTireDetails('D', 'D');
                }

            if ($scope.allTyresSelected) {
                if (!$scope.frontLeftPositionDisabled) {
                    $scope.openFrontTireDetails('L', 'F');
                }
                if (!$scope.frontRightPositionDisabled) {
                    $scope.openFrontTireDetails('R', 'F');
                }
                if (!$scope.backLeftPositionDisabled) {
                    $scope.openFrontTireDetails('L', 'B');
                }
                if (!$scope.backRightPositionDisabled) {
                    $scope.openFrontTireDetails('R', 'B');
                }
                if (!$scope.sparePositionDisable) {
                    $scope.openFrontTireDetails('D', 'D');
                }
                $scope.selectAlltyreLabel = "Deselect All Tyres";
            } else {
                $scope.selectAlltyreLabel = "Select All Tyres";
            }


        }

        $scope.loadPolicy = function (PolicyId, CommodityTypeId, DealerId, MakeId, ModelId, CustomerId, CommodityCategoryId) {

            if (isGuid(PolicyId)) {
                if (typeof SearchPolicyPopup != 'undefined')
                    SearchPolicyPopup.close();
                swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.loading'), text: $filter('translate')('pages.claimSubmission.inforMessages.policyInformation'), showConfirmButton: false });
                $scope.currentPolicy.policyId = PolicyId;
                $scope.currentPolicy.customerId = CustomerId;
                $scope.currentPolicy.commodityCategoryId = CommodityCategoryId;
                $scope.currentPolicy.makeId = MakeId;
                $scope.currentPolicy.modelId = ModelId;
                $scope.currentPolicy.commodityTypeId = CommodityTypeId;
                $scope.currentPolicy.dealerId = DealerId;

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ReadPolicyInformation',
                    data: { "policyId": PolicyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null) {
                        customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.errorReadingPolicyData'));
                        swal.close();
                    } else {
                        $scope.currentPolicy.policyNumber = data.PolicyNo;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/GetAllPartAreasByCommodityCategoryId',
                            data: { "commodityCategoryId": $scope.currentPolicy.commodityCategoryId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == null)
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noPartAreas'));
                            $scope.partAreas = data;
                            if ($scope.partAreas.length >0) {
                                $scope.currentPart.partAreaId = $scope.partAreas[0].Id;
                                $scope.selectedPartAreaChanged();
                            }
                        }).error(function (data, status, headers, config) {

                        }).finally(function () {
                            swal.close();
                        });

                        $scope.GetServiceHistory(PolicyId);

                        //setup policy view data

                        $scope.policyDetails.customerName = data.CustomerName;
                        $scope.policyDetails.commodityType = data.CommodityType;
                        $scope.policyDetails.insuaranceProductName = data.InsuaranceProductName;
                        $scope.policyDetails.policyNo = data.PolicyNo;
                        $scope.policyDetails.startDate = data.StartDate;
                        $scope.policyDetails.endDate = data.EndDate;

                    }

                }).error(function (data, status, headers, config) {
                    swal.close();
                }).finally(function () {

                });

            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.invalidPolicy'));
            }
        };

        $scope.ValidationILOEPopup = function () {
            dialogChangePassword = ngDialog.open({
                template: 'popUpValidationILOE',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };

        $scope.GetServiceHistory = function (PolicyId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllServiceHistoryByPolicyId',
                data: { "policyId": PolicyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null && $localStorage.CommodityType !== "Tyre")
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noServiceRecord'));
                $scope.itemServiceRecords = data;
            }).error(function (data, status, headers, config) {

            }).finally(function () {

            });
        }
        $scope.updateServiceRecord = function (serviceRecordId) {

            angular.forEach($scope.itemServiceRecords, function (value) {
                if (value.Id === serviceRecordId) {
                    $scope.serviceRecord.id = value.Id;
                    $scope.serviceRecord.serviceNumber = value.ServiceNumber;
                    $scope.serviceRecord.milage = value.Milage;
                    $scope.serviceRecord.remarks = value.Remarks;
                    $scope.serviceRecord.serviceDate = value.ServiceDate;
                }
            });
        }
        $scope.policyNumberChanged = function () {
            //alert($scope.currentPolicy.policyNo);

            if ($scope.currentPolicy.policyNo !== '') {
                swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.validatingPolicy'), showConfirmButton: false });

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ValidatePolicyNumberOnClaimSubmission',
                    data: {
                        "policyNo": $scope.currentPolicy.policyNo,

                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'error') {
                        customErrorMessage(data.msg);
                        $scope.currentPolicy.vinNo = "";
                        $scope.currentPolicy.policyId = emptyGuid();
                        $scope.currentPolicy.commodityCategoryId = emptyGuid();
                    } else {
                        $scope.currentPolicy.policyId = data.obj.policyId;
                        $scope.GetServiceHistory($scope.currentPolicy.policyId);
                        $scope.currentPolicy.commodityCategoryId = data.obj.commodityCategoryId;
                        $scope.currentPolicy.vinNo = data.msg;
                        $scope.getAllPartAreas();
                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.vinNumberChanged = function () {
            if (!isGuid($scope.currentPolicy.commodityTypeId)) {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectCommodityType'));
                return false;
            }
            $scope.isOtherProductSelected = false;
            $scope.customerNameDisabled = false;
            $scope.plateNumberDisabled = false;
            $scope.isOtherEngineChange = false;


            if (isGuid($scope.currentPolicy.commodityTypeId)) {
                angular.forEach($scope.commodityTypes, function (value) {
                    if ($scope.currentPolicy.commodityTypeId === value.CommodityTypeId) {

                        angular.forEach($scope.Products, function (pValue) {
                            if ($scope.product.Id === pValue.Id) {
                                if (value.CommodityCode.trim() == "A") {
                                    if (pValue.ProductTypeCode == "ADT") {
                                        $scope.required.lastServiceDate = false;
                                        $scope.required.lastServiceMileage = false;
                                    }


                                    if (pValue.ProductTypeCode == "ILOE") {
                                        $scope.isIloeProductSelected = true;

                                    } else {
                                        $scope.isOtherProductSelected = false;
                                        $scope.isIloeProductSelected = false;
                                    }
                                } else if (value.CommodityCode.trim() == "B") {
                                    if (pValue.ProductTypeCode == "ILOE") {
                                        $scope.isIloeProductSelected = true;

                                    } else {
                                        $scope.isOtherProductSelected = false;
                                        $scope.isIloeProductSelected = false;
                                    }
                                } else if (value.CommodityCode.trim() == "O") {
                                    if (pValue.Productcode == "TYRE") {
                                        $scope.isOtherProductSelected = true;
                                        $scope.customerNameDisabled = true;
                                        $scope.plateNumberDisabled = true;
                                    } else {
                                        $scope.isOtherEngineChange = true;
                                    }

                                } else {
                                    //other commodities
                                    $scope.isOtherProductSelected = false;
                                }

                            }
                        });


                    } else {
                        $scope.isOtherProductSelected = false;
                    }
                });
            }
            let product = $scope.getSelectedProduct();
            if ($scope.currentPolicy.policyNo !== '' || $scope.currentPolicy.vinNo!=='') {
                if (product != undefined && product != null && product.ProductTypeCode == "TYRE") {
                    swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.validatingPolicy'), showConfirmButton: false });

                    $scope.currentPolicy.policyId = emptyGuid();
                    $scope.currentPolicy.commodityCategoryId = emptyGuid();
                    $scope.currentPolicy.makeId = emptyGuid();
                    $scope.currentPolicy.modelId = emptyGuid();

                    $scope.currentPolicy.plateNumber = "";
                    $scope.currentPolicy.customerName = "";
                    $scope.currentPolicy.failureDate = "";
                    $scope.currentPolicy.failureMileage = "";
                    $scope.currentPolicy.lastServiceMileage = 0.00;
                    $scope.currentPolicy.lastServiceDate = "";
                    $scope.currentPolicy.policyMilege = 0.00;
                    $scope.currentPolicy.mobileNo = "";
                    $scope.frontLeftArticleNo ="";
                    $scope.frontRightArticleNo = "";
                    $scope.backLeftArticleNo = "";
                    $scope.backRightArticleNo = "";
                    $scope.downArticleNo = "";
                    $scope.eligiblePolicies = [];
                    $scope.isFrontLeftTireDetailsVisible = false;
                    $scope.isFrontRightTireDetailsVisible = false;
                    $scope.isBackRightTireDetailsVisible = false;
                    $scope.isBackLeftTireDetailsVisible = false;

                    $scope.frontLeftPositionDisabled = true;
                    $scope.frontRightPositionDisabled = true;
                    $scope.backLeftPositionDisabled = true;
                    $scope.backRightPositionDisabled = true;
                    $scope.sparePositionDisable = true;


                    $scope.sparePositionDisable = true;
                    $scope.selectAllTyresVisible = false;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/GetTyreDetailsByPolicyNumber',
                        data: {
                            "policyNumber": $scope.currentPolicy.policyNo,
                            "commodityTypeId": $scope.currentPolicy.commodityTypeId,
                            "dealerId": $scope.dealer.id,
                            "userId": $localStorage.LoggedInUserId
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        $scope.frontLeftClaimed = false;
                        $scope.frontRightClaimed = false;
                        $scope.backLeftClaimed = false;
                        $scope.backRightClaimed = false;
                        $scope.spareClaimed = false;

                        $scope.currentPolicy.policyNo = "";

                        angular.forEach(data.obj, function (tyres) {

                            if (tyres.Position === "FL") {
                                $scope.frontLeftPositionDisabled = false;
                                if (tyres.ClaimInformation != undefined && tyres.ClaimInformation != null && tyres.ClaimInformation.NoOfSubmissions == 1) {
                                    $scope.frontLeftPositionDisabled = true;
                                    $scope.frontLeftClaimed = true;
                                }
                                $scope.frontLeftArticleNo = 'C'+tyres.SerialNumber+'0000';
                            }
                            if (tyres.Position === "FR") {
                                $scope.frontRightPositionDisabled = false;
                                if (tyres.ClaimInformation != undefined && tyres.ClaimInformation != null && tyres.ClaimInformation.NoOfSubmissions == 1) {
                                    $scope.frontRightPositionDisabled = true;
                                    $scope.frontRightClaimed  = true;
                                }
                                $scope.frontRightArticleNo = 'C'+tyres.SerialNumber+'0000';
                            }

                            if (tyres.Position === "BL") {
                                $scope.backLeftPositionDisabled = false;
                                if (tyres.ClaimInformation != undefined && tyres.ClaimInformation != null && tyres.ClaimInformation.NoOfSubmissions == 1) {
                                    $scope.backLeftPositionDisabled = true;
                                    $scope.backLeftClaimed = true;
                                }

                                $scope.backLeftArticleNo = 'C' + tyres.SerialNumber + '0000';
                            }
                            if (tyres.Position === "BR") {
                                $scope.backRightPositionDisabled = false;
                                if (tyres.ClaimInformation != undefined && tyres.ClaimInformation != null && tyres.ClaimInformation.NoOfSubmissions == 1) {
                                    $scope.backRightPositionDisabled = true;
                                    $scope.backRightClaimed = true;
                                }
                                $scope.backRightArticleNo = 'C' + tyres.SerialNumber + '0000';
                            }
                            if (tyres.Position === "D") {
                                $scope.sparePositionDisable = false;
                                if (tyres.ClaimInformation != undefined && tyres.ClaimInformation != null && tyres.ClaimInformation.NoOfSubmissions == 1) {
                                    $scope.sparePositionDisable = true;
                                    $scope.spareClaimed  = true;
                                }
                                $scope.downArticleNo = 'C' + tyres.SerialNumber+'0000';
                            }
                        });



                        if (data.code === 'error') {
                            customErrorMessage(data.msg);
                        } else {
                            if (data.obj.length === 0) {
                                //ok and no policies
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noPoliciesFound'));
                            }
                            else if (data.obj.length === 1) {
                                $scope.currentPolicy.policyId = data.obj[0].Id;
                                $scope.GetServiceHistory($scope.currentPolicy.policyId);
                                $scope.currentPolicy.commodityCategoryId = data.obj[0].CommodityCategoryId;
                                $scope.currentPolicy.policyNo = data.obj[0].PolicyNo;
                                $scope.currentPolicy.makeId = data.obj[0].MakeId;
                                $scope.currentPolicy.failureDate = new Date();
                                $scope.selectedMakeChanged();
                                $scope.currentPolicy.modelId = data.obj[0].ModelId;
                                $scope.selectedModelChanged();
                              //  $scope.currentPolicy.customerName = data.obj[0].CustomerName;
                                $scope.currentPolicy.mobileNo = data.obj[0].MobileNumber;
                                $scope.currentPolicy.plateNumber = data.obj[0].PlateNumber;
                                $scope.currentPolicy.position = data.obj[0].Position;
                                $scope.currentPolicy.tireQuantity = data.obj[0].TireQuantity;
                                $scope.currentPolicy.policyMilege = data.obj[0].Milage;
                                $scope.loadCustomerDetails(data.obj[0].CustomerDetails);
                                $scope.selectAllTyresVisible = true;
                                $scope.dealerInvoiceTireDetails.InvoiceCodeId = data.obj[0].InvoiceCodeId;
                            }
                            else if (data.obj.length === 2) {
                                $scope.currentPolicy.policyId = data.obj[0].Id;
                                $scope.GetServiceHistory($scope.currentPolicy.policyId);
                                $scope.currentPolicy.commodityCategoryId = data.obj[0].CommodityCategoryId;
                                $scope.currentPolicy.policyNo = data.obj[0].PolicyNo;
                                $scope.currentPolicy.makeId = data.obj[0].MakeId;
                                $scope.currentPolicy.failureDate = new Date();
                                $scope.selectedMakeChanged();
                                $scope.currentPolicy.modelId = data.obj[0].ModelId;
                                $scope.selectedModelChanged();
                                //$scope.currentPolicy.customerName = data.obj[0].CustomerName;
                                $scope.currentPolicy.plateNumber = data.obj[0].PlateNumber;
                                $scope.currentPolicy.position = data.obj[0].Position;
                                $scope.currentPolicy.tireQuantity = data.obj[0].TireQuantity;
                                $scope.currentPolicy.policyMilege = data.obj[0].Milage;
                                $scope.currentPolicy.mobileNo = data.obj[0].MobileNumber;
                                $scope.loadCustomerDetails(data.obj[0].CustomerDetails);
                                $scope.dealerInvoiceTireDetails.InvoiceCodeId = data.obj[0].InvoiceCodeId;
                                $scope.selectAllTyresVisible = true;
                                //if (data.obj[0].Position == "F") {
                                //    $scope.frontPositionDisabled = false;
                                //    $scope.backPositionDisabled = true;
                                //} else {
                                //    $scope.frontPositionDisabled = true;
                                //    $scope.backPositionDisabled = false;
                                //}

                            } else {
                                //multiple policies found
                                if (product != undefined && product != null && product.ProductTypeCode == "TYRE") {
                                    if (data.obj[0].PolicyNo == data.obj[2].PolicyNo) {
                                        $scope.currentPolicy.policyId = data.obj[0].Id;
                                        $scope.GetServiceHistory($scope.currentPolicy.policyId);
                                        $scope.currentPolicy.commodityCategoryId = data.obj[0].CommodityCategoryId;
                                        $scope.currentPolicy.policyNo = data.obj[0].PolicyNo;
                                        $scope.currentPolicy.makeId = data.obj[0].MakeId;
                                        $scope.currentPolicy.failureDate = new Date();
                                        $scope.selectedMakeChanged();
                                        $scope.currentPolicy.modelId = data.obj[0].ModelId;
                                        $scope.selectedModelChanged();
                                        //$scope.currentPolicy.customerName = data.obj[0].CustomerName;
                                        $scope.currentPolicy.plateNumber = data.obj[0].PlateNumber;
                                        $scope.currentPolicy.position = data.obj[0].Position;
                                        $scope.currentPolicy.tireQuantity = data.obj[0].TireQuantity;
                                        $scope.currentPolicy.policyMilege = data.obj[0].Milage;
                                        $scope.dealerInvoiceTireDetails.InvoiceCodeId = data.obj[0].InvoiceCodeId;
                                        $scope.currentPolicy.mobileNo = data.obj[0].MobileNumber;
                                        $scope.loadCustomerDetails(data.obj[0].CustomerDetails);
                                        $scope.selectAllTyresVisible = true;
                                    }

                                    else {
                                        customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.multipleActivePolicies'));
                                        $scope.eligiblePolicies = data.obj;
                                        MultipleEligiblePolicySelection = ngDialog.open({
                                            template: 'popUpMultipleEligiblePoliciesForInvoice',
                                            className: 'ngdialog-theme-plain',
                                            closeByEscape: false,
                                            showClose: true,
                                            closeByDocument: false,
                                            scope: $scope
                                        });
                                    }



                                } else {
                                    customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.multiplePoliciesFound'));
                                    $scope.eligiblePolicies = data.obj;
                                    MultipleEligiblePolicySelection = ngDialog.open({
                                        template: 'popUpMultipleEligiblePoliciesForVIN',
                                        className: 'ngdialog-theme-plain',
                                        closeByEscape: false,
                                        showClose: true,
                                        closeByDocument: false,
                                        scope: $scope
                                    });
                                }


                            }

                        }
                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });

                } else {
                    swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.validatingVin'), showConfirmButton: false });

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidateVinSerialNumber',
                        data: {
                            "vinNo": $scope.currentPolicy.vinNo,
                            "commodityTypeId": $scope.currentPolicy.commodityTypeId,
                            "dealerId": $scope.dealer.id,
                            "productId": $scope.product.Id
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {

                        $scope.currentPolicy.policyId = emptyGuid();
                        $scope.currentPolicy.commodityCategoryId = emptyGuid();
                        $scope.currentPolicy.makeId = emptyGuid();
                        $scope.currentPolicy.modelId = emptyGuid();
                        $scope.currentPolicy.policyNo = "";
                        $scope.currentPolicy.plateNumber = "";
                        $scope.currentPolicy.customerName = "";
                        $scope.currentPolicy.failureDate = "";
                        $scope.currentPolicy.failureMileage = "";
                        $scope.currentPolicy.lastServiceMileage = "";
                        $scope.currentPolicy.lastServiceDate = "";
                        $scope.currentPolicy.policyMilege = 0.00;
                        $scope.eligiblePolicies = [];
                        $scope.partAreas = [];
                        $scope.claimItemList = [];

                        if (data.code === 'error') {
                            customErrorMessage(data.msg);
                        } else {
                            if (data.obj.length === 0) {
                                //ok and no policies
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noPoliciesFound'));
                            }
                            else if (data.obj.length === 1) {
                                $scope.currentPolicy.policyId = data.obj[0].Id;
                                $scope.GetServiceHistory($scope.currentPolicy.policyId);
                                $scope.currentPolicy.commodityCategoryId = data.obj[0].CommodityCategoryId;
                                $scope.currentPolicy.policyNo = data.obj[0].PolicyNo;
                                $scope.currentPolicy.makeId = data.obj[0].MakeId;
                                $scope.currentPolicy.customerName = data.obj[0].CustomerName;
                                $scope.currentPolicy.plateNumber = data.obj[0].plateNo;
                                $scope.currentPolicy.dealerId = $scope.dealer.id;
                                $scope.currentPolicy.policyMilege = data.obj[0].Milage;

                                if (product != undefined && product != null && product.ProductTypeCode == "TYRE") {
                                    $scope.loadCustomerDetails(data.obj[0].CustomerDetails);
                                }


                                if ($scope.isIloeProductSelected) {
                                    $scope.claimLimitation = data.obj[0].ClaimLimitation.toFixed(0);
                                    $scope.emivalue = data.obj[0].CalculateEMI.toFixed(2);
                                    //$scope.currentPart.partName = data.obj[0].LoneInstallmentValue;
                                    $scope.iloeLoanInstallment = data.obj[0].LoneInstallmentValue;
                                    //if (data.obj[0].LoneInstallmentValue != 0 || data.obj[0].LoneInstallmentValue != 1) {
                                    //    $scope.iloeLoanInstallment = [];
                                    //    var val1 = data.obj[0].LoneInstallmentValue - 1;
                                    //    var va12 = data.obj[0].LoneInstallmentValue - 2;
                                    //    $scope.iloeLoanInstallment.push(val1);
                                    //    $scope.iloeLoanInstallment.push(val2);
                                    //    $scope.iloeLoanInstallment.push(data.obj[0].LoneInstallmentValue);
                                    //}
                                }




                                $scope.selectedMakeChanged();
                                $scope.currentPolicy.modelId = data.obj[0].ModelId;
                                $scope.selectedModelChanged();
                                $scope.getAllPartAreas();
                            } else {
                                //multiple policies found
                                customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.multiplePoliciesFound'));
                                $scope.eligiblePolicies = data.obj;
                                MultipleEligiblePolicySelection = ngDialog.open({
                                    template: 'popUpMultipleEligiblePoliciesForVIN',
                                    className: 'ngdialog-theme-plain',
                                    closeByEscape: false,
                                    showClose: true,
                                    closeByDocument: false,
                                    scope: $scope
                                });
                            }

                        }
                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });
                }
            } else {
                $scope.currentPolicy.policyId = emptyGuid();
                $scope.currentPolicy.commodityCategoryId = emptyGuid();
                $scope.currentPolicy.makeId = emptyGuid();
                $scope.currentPolicy.modelId = emptyGuid();
                $scope.currentPolicy.policyNo = "";
                $scope.currentPolicy.plateNumber = "";
                $scope.currentPolicy.customerName = "";
                $scope.currentPolicy.failureDate = "";
                $scope.currentPolicy.failureMileage = "";
                $scope.currentPolicy.lastServiceMileage = 0.00;
                $scope.currentPolicy.lastServiceDate = "";
                $scope.currentPolicy.policyMilege = 0.00;
                $scope.eligiblePolicies = [];
                $scope.partAreas = [];
                $scope.claimItemList = [];
            }
        }
        $scope.loadCustomerDetails = function (customerDetails) {
            $scope.customerType = customerDetails.CustomerTypeId;
            if (customerDetails.CustomerTypeId == "2") {
                $scope.customerName = "Customer Name";
                $scope.currentPolicy.customerName = customerDetails.FirstName + ' ' + customerDetails.LastName;
            } else if (customerDetails.CustomerTypeId == "1") {
                $scope.customerName = "Business Name";
                $scope.currentPolicy.customerName = customerDetails.BusinessName;
            }
        }

        $scope.selectPolicyForSubmitClaim = function (policyId) {
            let product = $scope.getSelectedProduct();

            if (product != undefined && product != null && product.ProductTypeCode == "TYRE") {
                $scope.frontLeftPositionDisabled = true;
                $scope.frontRightPositionDisabled = true;
                $scope.backLeftPositionDisabled = true;
                $scope.backRightPositionDisabled = true;
                $scope.sparePositionDisable = true;
                $scope.selectAllTyresVisible = true;

                angular.forEach($scope.eligiblePolicies, function (policy) {
                    if (policy.InvoiceCodeDetailsId === policyId) {
                        $scope.currentPolicy.policyId = policy.Id;
                        $scope.currentPolicy.commodityCategoryId = policy.CommodityCategoryId;
                        $scope.currentPolicy.policyNo = policy.PolicyNo;
                        $scope.currentPolicy.makeId = policy.MakeId;
                        $scope.selectedMakeChanged();
                        $scope.currentPolicy.failureDate = new Date();
                        $scope.currentPolicy.modelId = policy.ModelId;
                        $scope.selectedModelChanged();
                        $scope.currentPolicy.customerName = policy.CustomerName;
                        $scope.currentPolicy.mobileNo = policy.MobileNumber;
                        $scope.currentPolicy.plateNumber = policy.PlateNumber;
                        $scope.currentPolicy.position = policy.Position;
                        $scope.currentPolicy.tireQuantity = policy.TireQuantity;
                        $scope.dealerInvoiceTireDetails.InvoiceCodeId = policy.InvoiceCodeId;



                        if ($scope.currentPolicy.position === "FL") {
                            $scope.frontLeftPositionDisabled = false;

                            if (policy.ClaimInformation != undefined && policy.ClaimInformation != null && policy.ClaimInformation.NoOfSubmissions == 1) {
                                $scope.frontLeftPositionDisabled = true;
                                $scope.frontLeftClaimed = true;
                            }
                            $scope.frontLeftArticleNo = 'C'+policy.SerialNumber+'0000';
                        }
                        if ($scope.currentPolicy.position === "FR") {
                            $scope.frontRightPositionDisabled = false;
                            if (policy.ClaimInformation != undefined && policy.ClaimInformation != null && policy.ClaimInformation.NoOfSubmissions == 1) {
                                $scope.frontRightPositionDisabled = true;
                                $scope.frontRightClaimed = true;
                            }
                            $scope.frontRightArticleNo = 'C'+policy.SerialNumber+'0000';
                        }

                        if ($scope.currentPolicy.position === "BL") {
                            $scope.backLeftPositionDisabled = false;
                            if (policy.ClaimInformation != undefined && policy.ClaimInformation != null && policy.ClaimInformation.NoOfSubmissions == 1) {
                                $scope.backLeftPositionDisabled = true;
                                $scope.backLeftClaimed = true;
                            }
                            $scope.backLeftArticleNo = 'C'+policy.SerialNumber+'0000';
                        }
                        if ($scope.currentPolicy.position === "BR") {
                            $scope.backRightPositionDisabled = false;
                            if (policy.ClaimInformation != undefined && policy.ClaimInformation != null && policy.ClaimInformation.NoOfSubmissions == 1) {
                                $scope.backRightPositionDisabled = true;
                                $scope.backRightClaimed = true;
                            }
                            $scope.backRightArticleNo = 'C' +policy.SerialNumber+'0000';
                        }
                        if ($scope.currentPolicy.position === "D") {
                            $scope.sparePositionDisable = false;
                            if (policy.ClaimInformation != undefined && policy.ClaimInformation != null && policy.ClaimInformation.NoOfSubmissions == 1) {
                                $scope.sparePositionDisable = true;
                                $scope.spareClaimed = true;
                            }
                            $scope.downArticleNo = 'C' + policy.SerialNumber + '0000';
                        }


                        MultipleEligiblePolicySelection.close();
                    }
                });
            } else {
                angular.forEach($scope.eligiblePolicies, function (policy) {
                    if (policy.Id === policyId) {
                        $scope.currentPolicy.policyId = policy.Id;
                        $scope.currentPolicy.commodityCategoryId = policy.CommodityCategoryId;
                        $scope.currentPolicy.policyNo = policy.PolicyNo;
                        $scope.currentPolicy.makeId = policy.MakeId;
                        $scope.selectedMakeChanged();
                        $scope.currentPolicy.modelId = policy.ModelId;
                        $scope.selectedModelChanged();
                        $scope.getAllPartAreas();
                        $scope.claimItemList = [];
                        MultipleEligiblePolicySelection.close();
                    }
                });
            }

        }

        $scope.showAddNewPartPopup = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                AddNewPartPopup = ngDialog.open({
                    template: 'popUpAddNewPart',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: false,
                    showClose: true,
                    closeByDocument: false,
                    scope: $scope
                });
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.selectPolicy'));
            }
        }
        $scope.savePart = function () {
            if ($scope.validateSavePartDetails()) {
                $scope.Part.DealerId = $scope.dealer.id;
                $scope.Part.CommodityId = $scope.currentPolicy.commodityTypeId;
                $scope.Part.MakeId = $scope.currentPolicy.makeId;
                $scope.Part.EntryBy = $localStorage.LoggedInUserId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/SaveNewPart',
                    data: $scope.Part,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'error') {
                        customErrorMessage(data.msg);
                    } else {
                        customInfoMessage($filter('translate')('pages.claimSubmission.inforMessages.savePart'));
                        $scope.selectedPartAreaChanged();
                        $scope.clearPartAddDetailsUponSave();
                        AddNewPartPopup.close();

                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {

                });

            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillallMandatory'));
            }
        }
        $scope.clearPartAddDetailsUponSave = function () {
            $scope.Part = {
                CommodityId: emptyGuid(),
                PartAreaId: emptyGuid(),
                MakeId: emptyGuid(),
                PartName: '',
                PartCode: '',
                PartNumber: '',
                AllocatedHours: 0,
                Price: 0,
                IsActive: true,
                ApplicableForAllModels: true,
                DealerId: emptyGuid(),
                EntryBy: emptyGuid()
            };
        }
        $scope.validateSavePartDetails = function () {
            var isValid = true;
            if (!isGuid($scope.Part.PartAreaId)) {
                $scope.validate_partPartArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partPartArea = "";
            }

            if ($scope.Part.PartName === '') {
                $scope.validate_partName = "has-error";
                isValid = false;
            } else {
                $scope.validate_partName = "";
            }

            if ($scope.Part.PartCode === '') {
                $scope.validate_partCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_partCode = "";
            }

            if ($scope.Part.PartNumber === '') {
                $scope.validate_partNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_partNumber = "";
            }

            if ($scope.Part.Price === '' || !parseFloat($scope.Part.Price) || parseFloat($scope.Part.Price) < 0) {
                $scope.Part.Price = 0.00;
            }
            if ($scope.Part.AllocatedHours === '' || !parseFloat($scope.Part.AllocatedHours || parseFloat($scope.Part.AllocatedHours) < 0)) {
                $scope.Part.AllocatedHours = 0.00;
            }
            //alert(isValid);
            return isValid;
        }

        $scope.getAllPartAreas = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllPartAreasByCommodityCategoryId',
                data: { "commodityCategoryId": $scope.currentPolicy.commodityCategoryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null)
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noPartAreas'));
                $scope.partAreas = data;
                if ($scope.partAreas.length > 0) {
                    $scope.currentPart.partAreaId = $scope.partAreas[0].Id;
                    $scope.selectedPartAreaChanged();
                }
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.claimSubmission.errorMessages.error'), msg);
        };

        var customWorningMessage = function (msg) {
            toaster.pop('warning', $filter('translate')('pages.claimSubmission.inforMessages.warning'), msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', $filter('translate')('pages.claimSubmission.inforMessages.information'), msg, 12000);
        };


        $scope.clearPartSection = function () {
            $scope.part.selected = undefined;
            $scope.currentPart.partNumber = '';
            $scope.currentPart.partQty = 1;
            $scope.currentPart.unitPrice = 0.00;
            $scope.currentPart.partId = emptyGuid();
            $scope.currentPart.isRelatedPartsAvailable = false;
            $scope.currentPart.allocatedHours = 0;
            $scope.currentPart.remark = '';
            $scope.currentPart.isRelatedPartsAvailable = false;
            $scope.currentPart.allocatedHours = 0;
            $scope.currentPart.goodWillType = 'NA';
            $scope.currentPart.goodWillValue = 0.00;
            $scope.currentPart.goodWillAmount = 0.00;
            $scope.currentPart.discountType = 'NA';
            $scope.currentPart.discountValue = 0.00;
            $scope.currentPart.discountAmount = 0.00;
            $scope.currentPart.partName = '';
        }

        /*----------------------Scanner Implementation----------------------
         Insert Dynamsoft.WebTwainEnv.Load(); to Init Method
         Define global variables
      */

        $scope.scannerPopUp = function (tabid) {
            $scope.tabId = tabid;
            // validation for selecting attachment type
            //console.log($scope.customerAttachmentType);
            //if (($scope.customerAttachmentType == null ||
            //    $scope.customerAttachmentType == 'undefined' ||
            //    $scope.customerAttachmentType == "") && tabid == 0) {

            //    customErrorMessage("Please Select Attachment Type");
            //    return false;
            //}
            if (tabid == 0) {
                if ($scope.currentPolicy.docAttachmentType1 == null ||
                    $scope.currentPolicy.docAttachmentType1 == 'undefined' ||
                    $scope.currentPolicy.docAttachmentType1 == "") {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectAttachment'));
                    return false;
                }
            }

            ScannerPopUp = ngDialog.open({
                template: 'imageScannerPopUp',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

        }

        $scope.getSelectedImageFromScanner = function () {

            var DocattachmentType;
            var page = 'Claim Submission'; var section;
            if ($scope.tabId == 0) {
                section = 'ClaimSAttachment';
                DocattachmentType = $scope.currentPolicy.docAttachmentType1; console.log(DocattachmentType);
            }

            $scope.ImageBatch = {
                documentName: $scope.attachmentData.name,
                attachmentType: DocattachmentType,
                page: page,
                section: section,
                encodedImage: []
            };

            var imagecount = DWObject.HowManyImagesInBuffer;
            console.log(imagecount);
            if (imagecount > 0 && imagecount != 'undefined') {
                for (var i = 0; i < imagecount; i++) {
                    var imagedata;
                    DWObject.SelectedImagesCount = imagecount;
                    DWObject.SetSelectedImageIndex(0, i);
                    DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                    imagedata = DWObject.SaveSelectedImagesToBase64Binary();
                    $scope.ImageBatch.encodedImage.push(imagedata);
                }
                $scope.ImageArray.push($scope.ImageBatch);
                console.log($scope.ImageArray);
                Dynamsoft.WebTwainEnv.Unload();
                $scope.attachmentData.name = '';
            }
        }

        $scope.removeAttachmentFromImageArray = function (index) {
            if (index > -1) {
                $scope.ImageArray.splice(index, 1);
            }
        }

        $scope.removeAttachment = function (refId) {
            console.log('removing item ', refId);
            $scope.docUploader.queue = $scope.docUploader.queue.filter(image => image.ref != refId);
        }

        $scope.Dynamsoft_OnReady = function () {
            // Dynamsoft.WebTwainEnv.Unload();
            Dynamsoft.WebTwainEnv.Load();
            // console.log('here');
            var OnAcquireImageSuccess, OnAcquireImageFailure;
            OnAcquireImageSuccess = OnAcquireImageFailure = function () {
                DWObject.Capability = EnumDWT_Cap.ICAP_AUTODISCARDBLANKPAGES;
                DWObject.CapType = EnumDWT_CapType.TWON_ONEVALUE;
                DWObject.CapValue = -1;//Auto
                if (DWObject.CapSet) {
                    console.log(DWObject.CapSet);
                }
                DWObject.CloseSource();
            };

            setTimeout(function () {
                DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
                // console.log(DWObject);
                if (DWObject) {

                    DWObject.SelectSource();
                    DWObject.CloseSource();
                    DWObject.OpenSource();
                    if (DWObject.Duplex != 0)
                        DWObject.IfDuplexEnabled = true;//Enable duplex
                    DWObject.IfShowUI = true; //hide the user interface of the source
                    DWObject.IfAutoDiscardBlankpages = true;
                    DWObject.IfFeederEnabled = true; // Use the document feeder to scan in batches
                    DWObject.IfDisableSourceAfterAcquire = true;
                    DWObject.IfShowIndicator = false;
                    DWObject.AcquireImage(OnAcquireImageSuccess, OnAcquireImageFailure);

                }
            }, 1000);
        }

        $scope.AcquireImage = function () {
            Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', $scope.Dynamsoft_OnReady());
        }

        $scope.clearScannedImageSections = function (sectionName) {
            for (var i = $scope.ImageArray.length - 1; i >= 0; i--) {
                if ($scope.ImageArray[i].section == sectionName)
                    $scope.ImageArray.splice(i, 1);
            }
        }

        //$scope.uploadScannerDocuments = function () {
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/Upload/UploadScannedAttachment',
        //        data: { "ImageArray": $scope.ImageArray },
        //        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
        //    }).success(function (data, status, headers, config) {
        //        console.log(data);
        //        if (data !== "Failed") {
        //            angular.forEach(data, function (value) {
        //                $scope.uploadedDocIds.push(value);
        //            });
        //        }
        //    }).error(function (data, status, headers, config) {
        //        swal.close();

        //    });

        //}

        $scope.closedialog = function () {
            ScannerPopUp.close();
        }


        //claim ui tyre
        $scope.isFrontLeftTireDetailsVisible = false;
        $scope.isFrontRightTireDetailsVisible = false;

        $scope.isBackRightTireDetailsVisible = false;
        $scope.isBackLeftTireDetailsVisible = false;

        $scope.dataLoadMode = false;
        $scope.openFrontTireDetails = function (elem, position) {
            if (position === 'F' && elem === 'L') {
                if ($scope.isFrontLeftTireDetailsVisible) {
                    $scope.isFrontLeftTireDetailsVisible = false;
                    $scope.dealerInvoiceTireDetails.seriaFrontlLeft = '';

                } else {
                    $scope.isFrontLeftTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.seriaFrontlLeft = $scope.frontLeftArticleNo != "" ? $scope.frontLeftArticleNo : $scope.frontRightArticleNo  ;
                }
            }

            if (position === 'F' && elem === 'R') {
                if ($scope.isFrontRightTireDetailsVisible) {
                    $scope.isFrontRightTireDetailsVisible = false;
                    $scope.dealerInvoiceTireDetails.serialFrontRight = '';
                } else {
                    $scope.isFrontRightTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.serialFrontRight = $scope.frontRightArticleNo != "" ? $scope.frontRightArticleNo : $scope.frontLeftArticleNo ;
                }
            }

            if (position === 'B' && elem === 'L') {
                if ($scope.isBackLeftTireDetailsVisible) {
                    $scope.isBackLeftTireDetailsVisible = false;
                    $scope.dealerInvoiceTireDetails.serialBackLeft = '';

                } else {
                    $scope.isBackLeftTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.serialBackLeft = $scope.backLeftArticleNo != "" ? $scope.backLeftArticleNo : $scope.backRightArticleNo;
                }
            }
            if (position === 'B' && elem === 'R') {
                if ($scope.isBackRightTireDetailsVisible) {
                    $scope.isBackRightTireDetailsVisible = false;
                    $scope.dealerInvoiceTireDetails.serialBackRight = '';
                } else {
                    $scope.isBackRightTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.serialBackRight = $scope.backRightArticleNo != "" ? $scope.backRightArticleNo : $scope.backLeftArticleNo;
                }
            }

            if (position === 'D' && elem === 'D') {
                if ($scope.isDownTireDetailsVisible) {
                    $scope.isDownTireDetailsVisible = false;
                    $scope.dealerInvoiceTireDetails.downSerial = '';
                } else {
                    $scope.isDownTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.downSerial = $scope.downArticleNo;
                }
            }

        }

        // Claim Submission Edit
        $scope.claimTable = new ngTableParams({
            page: 1,
            count: 10,
        }, {
                getData: function ($defer, params) {

                    var page = params.page();
                    var size = params.count();
                    var search = {
                        page: page,
                        pageSize: size,
                        loggedInUserId: $localStorage.LoggedInUserId,
                        userType: $scope.userType,
                        claimSearch: $scope.claimSearch
                    }
                    if ($scope.userType != '') {
                        $scope.policyGridloading = true;
                        $scope.policyGridloadAttempted = false;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/GetAllSubmittedClaimsForEditByUserId',
                            data: search,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var responseArr = JSON.parse(data);
                            if (responseArr != null) {
                                params.total(responseArr.totalRecords);
                                $defer.resolve(responseArr.data);
                            } else {
                                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noClaimRequest'));
                            }
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            $scope.policyGridloading = false;
                            $scope.policyGridloadAttempted = true;
                        });
                    }
                }
            });



        $scope.GetAllCustomerComplaints =async function() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllCustomerComplaints',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CustomerComplaints = data;
              //  console.log('loaded GetAllCustomerComplaints');
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.GetAllDealerComments = async function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllDealerComments',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerComments = data;
              //  console.log('loaded GetAllDealerComments');
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.GetAllCommodities = async function() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
              //  console.log('loaded GetAllCommodities');
                if (data.length === 1) {

                    //$scope.currentPolicy.commodityTypeId = data[0].CommodityTypeId;
                }

            }).error(function (data, status, headers, config) {
            });
        }




        $scope.getClaimData =async function () {
            $scope.isEditClaim = true;
            //$scope.claimId = claimId;
            await $scope.GetAllCustomerComplaints();
            await $scope.GetAllCommodities();
            await $scope.GetAllDealerComments();

               // console.log('promis called now then ');
            swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.loading'), text: $filter('translate')('pages.claimSubmission.inforMessages.readingClaimInformation'), showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/LoadClaimDetailsForClaimEdit',
                    data: { "claimId": $scope.claimId, "loggedInUserId": $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Status == "ok") {
                        $scope.claimId = data.ClaimId;
                        $scope.loadPolicy(data.PolicyDetails.PolicyId, data.PolicyDetails.CommodityTypeId,
                            data.PolicyDetails.DealerId, data.PolicyDetails.MakeId, data.PolicyDetails.ModelId,
                            data.PolicyDetails.CustomerId, data.PolicyDetails.CommodityCategoryId);

                        $scope.claimItemList = data.ClaimDetails.ClaimItemList;
                        var i = 0;
                        angular.forEach($scope.claimItemList, function (claimItem) {

                            if (claimItem.itemType != 'L') {
                                $scope.claimItemList2[i] = claimItem;
                                i++;
                                angular.forEach($scope.claimItemList, function (claimItem2) {
                                    if (claimItem2.parentId == $scope.claimItemList2[i - 1].partId) {
                                        $scope.claimItemList2[i] = claimItem2;
                                        i++;
                                    }
                                });
                            }
                        })
                        $scope.claimItemList = $scope.claimItemList2;
                        angular.forEach($scope.claimItemList, function (value) {
                            if (value.qty == 0) {
                                value.qty = "N/A";
                            }
                        });
                        $scope.currentPolicy.policyNo = data.ClaimDetails.PolicyNumber;
                        $scope.currentPolicy.commodityTypeId = data.PolicyDetails.CommodityTypeId;
                        $scope.currentPolicy.vinNo = data.ClaimDetails.VINNO;
                        $scope.dealer.id = data.ClaimDetails.DealerId;
                        $scope.currentPolicy.makeId = data.PolicyDetails.MakeId;
                        $scope.currentPolicy.modelId = data.PolicyDetails.ModelId;
                        $scope.currentPolicy.customerName = data.ClaimDetails.CustomerName;
                        $scope.currentPolicy.mobileNo = data.ClaimDetails.CustomeMobileNo;
                        $scope.currentPolicy.plateNumber = data.ClaimDetails.PlateNo;
                        $scope.currentPolicy.failureDate = data.ClaimDetails.ClaimDate;
                        $scope.currentPolicy.failureMileage = parseFloat(data.ClaimDetails.ClaimMileage);
                        $scope.currentPolicy.lastServiceDate = data.ClaimDetails.LastServiceDate;
                        $scope.currentPolicy.lastServiceMileage = parseFloat(data.ClaimDetails.LastServiceMileage.replace(/,/g, ''));
                        $scope.currentPart.dealerComments = data.PolicyDetails.DealerComment;
                        $scope.isOtherProductSelected = false;

                        //$scope.vinNumberChanged();
                        if ($localStorage.CommodityType === "Tyre") {
                            var dcomments = $scope.DealerComments;
                            var comment = dcomments.filter(com => com.Comment === data.PolicyDetails.DealerComment);
                            $scope.dealerInvoiceTireDetails.dealerCommentId = comment[0].Id;
                            $scope.dealerInvoiceTireDetails.dealerComment = data.PolicyDetails.DealerComment;
                            $scope.loadTyreDepthLoading(data.ClaimDetails.ClaimItemList);
                            $scope.customerDisabled = false;
                            $scope.isOtherProductSelected = true;

                        }
                        $scope.TotalClaimAmount = data.ClaimDetails.TotalClaimAmount;
                        $scope.complaint.customer = data.ClaimDetails.Complaint.customer;
                        $scope.complaint.dealer = data.ClaimDetails.Complaint.dealer;
                        $scope.dealer.currencyCode = data.ClaimDetails.CurrencyCode;
                        $scope.currentPolicy.PolicyNumber = data.ClaimDetails.PolicyNumber;
                        $scope.claimSubmission.customerName = data.ClaimDetails.CustomerNameCS;

                        if (isGuid($scope.currentPolicy.makeId)) {
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/MakeAndModelManagement/GetModelesByMakeId',
                                dataType: 'json',
                                data: { Id: $scope.currentPolicy.makeId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.models = data;
                            }).error(function (data, status, headers, config) {
                            });
                        }
                        $scope.calculateTotalClaimAmount();
                        //$scope.claimItemList = data.ClaimDetails;
                        $scope.attachments_temp = data.ClaimDetails.Attachments.Attachments;
                        $scope.claimView.claimAttachments = data.ClaimDetails.Attachments.Attachments;


                        $scope.docUploader.queue = [];
                        for (var i = 0; i < data.ClaimDetails.Attachments.Attachments.length; i++) {
                            var file = {
                                name: data.ClaimDetails.Attachments.Attachments[i].FileName,
                                size: data.ClaimDetails.Attachments.Attachments[i].AttachmentSizeKB * 1000
                            }
                            var attachment = {
                                documentType: data.ClaimDetails.Attachments.Attachments[i].DocumentType,
                                id: data.ClaimDetails.Attachments.Attachments[i].Id,
                                file: file,
                                ref: data.ClaimDetails.Attachments.Attachments[i].FileServerRef,
                                isUploaded:true
                            }
                            $scope.docUploader.queue.push(attachment);

                        }


                    } else {
                        swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.securityInformation'), text: data.Status, showConfirmButton: false });
                        setTimeout(function () { swal.close(); }, 8000);
                        $state.go('app.claimlisting');
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });





        }

        $scope.updateClaim = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                angular.forEach($scope.claimItemList, function (value) {
                    if (value.qty == "N/A") {
                        value.qty = 0;
                    }
                });

                if (!$scope.validateClaimDetailsEdit()) {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.fillComplaint'));
                    return false;
                }


                $scope.claimSubmission.customerName = $scope.currentPolicy.customerName;
                $scope.claimSubmission.plateNumber  = $scope.currentPolicy.plateNumber;
                $scope.claimSubmission.failureDate = $scope.currentPolicy.failureDate;
                $scope.claimSubmission.failureMileage = $scope.currentPolicy.failureMileage;
                $scope.claimSubmission.lastServiceDate = $scope.currentPolicy.lastServiceDate;
                $scope.claimSubmission.lastServiceMileage = $scope.currentPolicy.lastServiceMileage;


                if ($scope.claimItemList.length > 0) {
                    if ($scope.docUploader.queue.length == 0) {
                        swal({ title: $filter('translate')('pages.claimSubmission.inforMessages.processing'), text: $filter('translate')('pages.claimSubmission.inforMessages.submittingClaimUpdate'), showConfirmButton: false });

                        if ($scope.currentPolicy.lastServiceMileage == "" && $scope.required.lastServiceMileage == false) {
                            $scope.currentPolicy.lastServiceMileage = 0;
                        }

                        if ($scope.currentPolicy.lastServiceDate == "" && $scope.required.lastServiceDate == false) {
                            $scope.currentPolicy.lastServiceDate = "1/1/1753";
                        }

                        var data = {
                            id: $scope.claimId,
                            claimItemList: $scope.claimItemList,
                            totalClaimAmount: $scope.TotalClaimAmount,
                            requestedUserId: $rootScope.LoggedInUserId,
                            dealerId: $scope.currentPolicy.dealerId,
                            policyId: $scope.currentPolicy.policyId,
                            complaint: $scope.complaint,
                            attachmentIds: $scope.uploadedDocIds,
                            policyDetails: $scope.claimSubmission,
                            commentDealer: $scope.commentDealer
                        };
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/UpdateClaim',
                            data: {
                                "claimData": data
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'ok') {
                                swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), $filter('translate')('pages.claimSubmission.inforMessages.redirectClaimListing'), "success");
                                $scope.clearAll();
                                //TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                setTimeout(function () { swal.close(); }, 5000);
                                $state.go('app.claimlisting');
                            } else {
                                swal($filter('translate')('pages.claimSubmission.inforMessages.tasInformation'), data, "error");
                            }


                        }).error(function (data, status, headers, config) {
                        }).finally(function () {

                        });
                    } else {



                        for (var i = 0; i < $scope.attachments_temp.length; i++) {
                            var isRemoved = true;
                            for (var j = 0; j < $scope.docUploader.queue.length; j++) {
                                //alert($scope.customerDocUploader.queue[j].ref);
                                if (typeof $scope.docUploader.queue[j].ref === 'undefined') {
                                    //new records
                                } else {
                                    if ($scope.attachments_temp[i].FileServerRef == $scope.docUploader.queue[j].ref) {
                                        isRemoved = false;
                                        //return false;
                                    }
                                }
                            }
                            if (isRemoved) {
                                // $scope.removedAttachments.push($scope.attachments_temp[i].Id);
                            } else {
                                $scope.uploadedDocIds.push($scope.attachments_temp[i].Id);
                            }

                        }

                        $scope.attachments_temp = [];
                        //removing unwanted items for upload

                        for (var j = $scope.docUploader.queue.length - 1; j >= 0; j--) {
                            if (typeof $scope.docUploader.queue[j].ref === 'undefined') {
                                //new records
                            } else {
                                $scope.docUploader.queue.splice(j, 1);
                            }
                        }

                        if ($scope.docUploader.queue.length == 0) {

                            $scope.updateClaim();
                        } else {
                            for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                                $scope.docUploader.queue[i].file.name = $scope.docUploader.queue[i].file.name + '@@' + $scope.docUploader.queue[i].documentType;
                            }
                            $scope.docUploader.uploadAll();
                        }
                    }
                } else {
                    customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.noClaimItem'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimSubmission.errorMessages.pleaseSelectPolicy'));
            }
        }

        $scope.UpdateClaimUpdateStatus = function (claimReqId) {


            if (isGuid(claimReqId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/UpdateClaimUpdateStatus',
                    data: { "claimId": claimReqId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === "OK") {


                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    //swal.close();
                });
            } else {
                customErrorMessage("Please select a valid claim.");
            }
        }


        $scope.validateClaimDetailsEdit = function () {
            var isValid = true;

            //if ($scope.complaint.customer === '') {
            //    $scope.validate_customerComplaint = "has-error";
            //    isValid = false;
            //} else
            //    $scope.validate_customerComplaint = "";


            //if ($scope.complaint.dealer === '') {
            //    $scope.validate_dealerComment = "has-error";
            //    isValid = false;
            //} else
            //    $scope.validate_dealerComment = "";

            return isValid;
        }

        $scope.$watch(function () {
            return $location.path();
        }, function (newPath, oldPath) {
                if (newPath != oldPath) {
                    $scope.claimId = undefined;
                    $rootScope.claimId = undefined;
                    console.log('unset claim id');
                    console.log($scope.claimId);
                    $scope.isEditClaim = false;
                    $localStorage.claimSubmissionTempId = undefined;
                } else {
                    $localStorage.claimSubmissionTempId = $scope.claimId;
                }
        })

        $scope.loadTyreDepthLoading = function (ClaimItemList) {
            angular.forEach(ClaimItemList, function (cItem) {
                if (cItem.remarks === "FL") {
                    $scope.frontLeftPositionDisabled = false;
                    $scope.isFrontLeftTireDetailsVisible = true;
                    $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontLeft = cItem.UnUsedTireDepth;
                    $scope.dealerInvoiceTireDetails.seriaFrontlLeft = cItem.itemNumber;
                }
                if (cItem.remarks === "FR") {
                    $scope.frontRightPositionDisabled = false;
                    $scope.isFrontRightTireDetailsVisible  = true;
                    $scope.dealerInvoiceTireDetails.unusedTyreDepthFrontRight = cItem.UnUsedTireDepth;
                    $scope.dealerInvoiceTireDetails.serialFrontRight = cItem.itemNumber;
                }
                if (cItem.remarks === "BL") {
                    $scope.backLeftPositionDisabled = false;
                    $scope.isBackLeftTireDetailsVisible  = true;
                    $scope.dealerInvoiceTireDetails.unusedTyreDepthBackLeft = cItem.UnUsedTireDepth;
                    $scope.dealerInvoiceTireDetails.serialBackLeft = cItem.itemNumber;
                }
                if (cItem.remarks === "BR") {
                    $scope.backRightPositionDisabled = false;
                    $scope.isBackRightTireDetailsVisible  = true;
                    $scope.dealerInvoiceTireDetails.unusedTyreDepthBackRight= cItem.UnUsedTireDepth;
                    $scope.dealerInvoiceTireDetails.serialBackRight = cItem.itemNumber;
                }
                if (cItem.remarks === "D") {
                    $scope.sparePositionDisable = false;
                    $scope.isDownTireDetailsVisible  = true;
                    $scope.dealerInvoiceTireDetails.unusedTyreDepthDown= cItem.UnUsedTireDepth;
                    $scope.dealerInvoiceTireDetails.downSerial = cItem.itemNumber;
                }
            });
        }
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
var expand = function expand(paneToExpand) {
    if (isDisabled || !paneToExpand) { return; }

    if (!$scope.allowMultiple) {
        ctrl.collapseAll(paneToExpand);
    }

    if (!paneToExpand.isExpanded) {
        paneToExpand.isExpanded = true;
        $scope.expandCb({ index: ctrl.getPaneIndex(paneToExpand), id: paneToExpand.id, pane: paneToExpand });
    }
};

var collapse = function collapse(paneToCollapse) {
    if (isDisabled || !paneToCollapse) { return; }

    if (paneToCollapse.isExpanded) {
        paneToCollapse.isExpanded = false;
        $scope.collapseCb({ index: ctrl.getPaneIndex(paneToCollapse), id: paneToCollapse.id, pane: paneToCollapse });
    }
};

var expandAll = function expandAll() {
    if (isDisabled) { return; }

    if ($scope.allowMultiple) {
        angular.forEach($scope.panes, function (iteratedPane) {
            ctrl.expand(iteratedPane);
        });
    } else {
        throw new Error('The `multiple` attribute can\'t be found');
    }
};

var collapseAll = function collapseAll(exceptionalPane) {
    if (isDisabled) { return; }

    angular.forEach($scope.panes, function (iteratedPane) {
        if (iteratedPane !== exceptionalPane) {
            ctrl.collapse(iteratedPane);
        }
    });
};

