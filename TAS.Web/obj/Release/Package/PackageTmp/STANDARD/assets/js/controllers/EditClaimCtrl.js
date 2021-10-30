app.controller('ClaimEditCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster,
        $state, $stateParams, FileUploader) {
        $scope.claimId = $stateParams.claimId;
        var PartSuggestionPopup;
        $scope.currentPolicy = {
            policyId: emptyGuid(),
            customerId: emptyGuid(),
            commodityCategoryId: emptyGuid(),
            makeId: emptyGuid(),
            modelId: emptyGuid(),
            commodityTypeId: emptyGuid(),
            dealerId: emptyGuid()
        };
        $scope.claimItem = {
            id: 0,
            itemId: emptyGuid(),
            itemName: '',
            itemNumber: '',
            qty: 0,
            unitPrice: 0.00,
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
            allocatedHours: 0
        };
        $scope.labourCharge = {
            chargeType: 'H',
            hourlyRate: 0.00,
            hours: 0,
            totalAmount: 0.00,
            description: ''
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
        $scope.policySaveStatusMsg = "";

        $scope.userType = '';
        $scope.TotalClaimAmount = 0.00;

        $scope.dealer = {
            currencyId: emptyGuid(),
            currencyCode: '',
            hourlyRate: 0.00
        };
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
        $scope.claimSubmission = {
            customerName: '',
            failureDate: '',
            failureMileage: 0.00,
            lastServiceDate: '',
            lastServiceMileage: 0.00

        };

        $scope.dealerInvoiceTireDetails = {
            InvoiceCodeId: emptyGuid(),
            customerComplaint: '',
            dealerComment: '',           
            serialBackRight: '',
            unusedTyreDepthBackRight: '',
            serialBackLeft: '',
            unusedTyreDepthBackLeft: '',
            seriaFrontlLeft: '',
            unusedTyreDepthFrontLeft: '',
            serialFrontRight: '',
            unusedTyreDepthFrontRight: '',            
        }

        $scope.dealerInvoiceTireDetailsarr = {
            frontLeft: {
                claimItemId:'',
                serial: '',
                unusedTyreDepth:''
            },
            backLeft: {
                claimItemId: '',
                serial: '',
                unusedTyreDepth: ''
            },
            frontRight: {
                claimItemId: '',
                serial: '',
                unusedTyreDepth: ''
            },
            backRight: {
                claimItemId: '',
                serial: '',
                unusedTyreDepth: ''
            }
        };

        $scope.frontPositionDisabled = false;
        $scope.backPositionDisabled = false;
        $scope.customerNameDisabled = true;
        $scope.plateNumberDisabled = true;
        $scope.InvoiceCodeId = "";

        $scope.isOtherProductSelected = false;
        $scope.part = {};
        $scope.partAreas = [];
        $scope.parts = [];
        $scope.itemServiceRecords = [];
        $scope.claimItemList = [];
        $scope.partSuggestions = [];
        $scope.isSuggestionsAvailable = false;
        $scope.partId = emptyGuid();
        $scope.uploadedDocIds = [];
        $scope.attachmentData = {};
        $scope.commentDealer = "";
        $scope.claimSubmissionId = "";
        //$scope.backleftPositionDisabled = true;
        //$scope.frontrightPositionDisabled = true;
        //$scope.backrightPositionDisabled = true;
        //$scope.frontleftPositionDisabled = true;

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
        // $scope.removedAttachments = [];
        //initialize uploaders
        $scope.docUploader = new FileUploader({
            url: '/TAS.Web/api/Upload/UploadAttachment',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt, 'Page': 'ClaimSubmission', 'Section': 'Default' }
        });
        $scope.docUploader.onProgressAll = function () {
            swal({ title: "TAS Information", text: 'Uploading documents...', showConfirmButton: false });
        }
        $scope.docUploader.onSuccessItem = function (item, response, status, headers) {
            if (response != 'Failed') {
                $scope.uploadedDocIds.push(response.replace(/['"]+/g, ''));
                // $scope.currentUploadingItem++
                // $scope.policySaveStatusMsg = $scope.currentUploadingItem + " of " + $scope.docUploader.queue.length;
            } else {
                customErrorMessage("Error occured while uploading Attachments.");
                $scope.docUploader.cancelAll();
            }
        }
        $scope.docUploader.onCompleteAll = function () {

            $scope.docUploader.queue = [];

            if ($scope.Products[0].Productcode == "TYRE") {
                $scope.updateOtherTireClaim();
            } else {
                $scope.updateClaim();
            }
           
        }
        $scope.docUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                if ($scope.currentPolicy.docAttachmentType != "" && typeof $scope.currentPolicy.docAttachmentType != 'undefined') {
                    if (item.size > 3000000) {
                        customErrorMessage('Max document size is 3MB');
                        return false;
                    } else {
                        return true;
                    }
                } else {
                    customErrorMessage('Please select a attachment type before select a file.');
                    return false;
                }
            }
        });



        $scope.loadInitailData = function () {
            if (isGuid($scope.claimId)) {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/UserValidationClaimSubmission',
                    data: { "loggedInUserId": $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.status == 'ok') {
                        $scope.dealer.currencyId = data.dealerCurrencyId;
                        $scope.dealer.currencyCode = data.dealerCurrencyCode;
                        $scope.userType = data.userType;
                        if (!parseFloat(data.dealerHourlyRate) || parseFloat(data.dealerHourlyRate) == 0) {
                            swal({ title: 'TAS Security Information', text: "Man-Hour rate not found for dealer.You should update it first in order to access this page.", showConfirmButton: false });
                            setTimeout(function () { swal.close(); }, 8000);
                            $state.go('app.dashboard');

                        } else {
                            $scope.dealer.hourlyRate = data.dealerHourlyRate;
                            $scope.labourCharge.hourlyRate = data.dealerHourlyRate;
                            $scope.getClaimData();

                        }
                    } else {
                        swal({ title: 'TAS Security Information', text: data.status, showConfirmButton: false });
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
            } else {
                swal({ title: 'TAS Security Information', text: "Invalid request.", showConfirmButton: false });
                setTimeout(function () { swal.close(); }, 8000);
                $state.go('app.claimlisting');
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;


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

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllCustomerComplaints',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CustomerComplaints = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllDealerComments',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.DealerComments = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.getClaimData = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;


            }).error(function (data, status, headers, config) {
            });

            swal({ title: 'Loading...', text: 'Reading Claim Information', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/LoadClaimDetailsForClaimEdit',
                data: { "claimId": $scope.claimId, "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.Status == "ok") {
                    $scope.loadPolicy(data.PolicyDetails.PolicyId, data.PolicyDetails.CommodityTypeId,
                        data.PolicyDetails.DealerId, data.PolicyDetails.MakeId, data.PolicyDetails.ModelId,
                        data.PolicyDetails.CustomerId, data.PolicyDetails.CommodityCategoryId);

                    $scope.claimItemList = data.ClaimDetails.ClaimItemList;
                    angular.forEach($scope.claimItemList, function (value) {
                        if (value.qty == 0) {
                            value.qty = "N/A";
                        }
                    });
                    $scope.TotalClaimAmount = data.ClaimDetails.TotalClaimAmount;
                    $scope.complaint.customer = data.ClaimDetails.Complaint.customer;
                    $scope.complaint.dealer = data.ClaimDetails.Complaint.dealer;
                    $scope.dealer.currencyCode = data.ClaimDetails.CurrencyCode;
                    $scope.currentPolicy.PolicyNumber = data.ClaimDetails.PolicyNumber;
                    $scope.claimSubmission.customerName = data.ClaimDetails.CustomerNameCS;
                    $scope.claimSubmission.failureDate = data.ClaimDetails.ClaimDate;
                    $scope.claimSubmission.failureMileage = parseFloat(data.ClaimDetails.ClaimMileage);
                    $scope.claimSubmission.lastServiceDate = data.ClaimDetails.LastServiceDate;
                    $scope.claimSubmission.lastServiceMileage = parseFloat(data.ClaimDetails.LastServiceMileage.replace(/,/g, ''));
                    $scope.calculateTotalClaimAmount();
                    //$scope.claimItemList = data.ClaimDetails;
                    $scope.attachments_temp = data.ClaimDetails.Attachments.Attachments;
                    $scope.docUploader.queue = [];
                    for (var i = 0; i < data.ClaimDetails.Attachments.Attachments.length; i++) {
                        var file = {
                            name: data.ClaimDetails.Attachments.Attachments[i].FileName,
                            size: data.ClaimDetails.Attachments.Attachments[i].AttachmentSizeKB
                        }
                        var attachment = {
                            documentType: data.ClaimDetails.Attachments.Attachments[i].DocumentType,
                            id: data.ClaimDetails.Attachments.Attachments[i].Id,
                            file: file,
                            ref: data.ClaimDetails.Attachments.Attachments[i].FileServerRef
                        }
                        $scope.docUploader.queue.push(attachment);

                    }
                } else {
                    swal({ title: 'TAS Security Information', text: data.Status, showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 8000);
                    $state.go('app.claimlisting');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }

        $scope.clearPartSection = function () {
            $scope.part.selected = undefined;
            $scope.currentPart.partNumber = '';
            $scope.currentPart.partQty = 1;
            $scope.currentPart.unitPrice = 0.00;
            $scope.currentPart.partId = emptyGuid();
            $scope.currentPart.isRelatedPartsAvailable = false;
            $scope.currentPart.allocatedHours = 0;
            $scope.currentPart.remark = '';
        }
        $scope.validatePartDetails = function () {
            var isValid = true;
            if (!isGuid($scope.currentPart.partAreaId)) {
                $scope.validate_partArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partArea = "";

            }

            if (!isGuid($scope.currentPart.partId)) {
                $scope.validatepart = "has-error";
                isValid = false;
            } else {
                $scope.validatepart = "";
            }

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

            return isValid;
        }
        $scope.addPart = function () {
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
                        totalPrice: (parseFloat($scope.currentPart.partQty) * parseFloat($scope.currentPart.unitPrice)).toFixed(2),
                        itemType: 'P',
                        remarks: $scope.currentPart.remark
                    };

                    if (parseFloat($scope.currentPart.allocatedHours) && parseFloat($scope.currentPart.allocatedHours) > 0) {
                        $scope.labourCharge.chargeType = "H";
                        $scope.labourCharge.hours = $scope.currentPart.allocatedHours;
                        $scope.labourCharge.description = "This allocated to part no -" + $scope.currentPart.partNumber;
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

                                    if (inputValue === "" || !parseFloat(inputValue) || parseFloat(inputValue) == 0) {
                                        swal.showInputError("You need to enter an amount!");
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
                            title: "This part is interrelated with some other part(s)",
                            text: "You wish to view and add them?",
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
                                        customErrorMessage('Error occured while reading related part information.');
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
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            } else {
                customErrorMessage('Please select a policy to add parts.');
            }
        }

        $scope.validateLabourPayments = function () {
            var isValid = true;
            if ($scope.labourCharge.chargeType == '') {
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

            if ($scope.labourCharge.chargeType == "H") {
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

            $scope.labourCharge.hourlyRate = 0.00;
            $scope.labourCharge.hours = 0;
            $scope.labourCharge.totalAmount = 0.00;
            $scope.labourCharge.description = '';

        }
        $scope.addLabourCharge = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                if ($scope.validateLabourPayments()) {
                    var nextId = $scope.claimItemList.length;
                    var claimItem = {
                        id: (nextId + 1),
                        partAreaId: emptyGuid(),
                        partId: emptyGuid(),
                        itemName: "Labour Charge",
                        itemNumber: $scope.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                        qty: $scope.labourCharge.chargeType == "H" ? $scope.labourCharge.hours : "N/A",
                        unitPrice: $scope.labourCharge.hourlyRate,
                        totalPrice: $scope.labourCharge.chargeType == "H" ?
                            (parseFloat($scope.labourCharge.hours) * parseFloat($scope.labourCharge.hourlyRate)).toFixed(2) :
                            parseFloat($scope.labourCharge.hourlyRate).toFixed(2),
                        itemType: 'L',
                        remarks: $scope.labourCharge.description
                    };
                    $scope.claimItemList.push(claimItem);
                    $scope.clearLabourChargeSection();
                    //update total
                    $scope.calculateTotalClaimAmount();
                } else {
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            } else {
                customErrorMessage('Please select a policy to add labour charges.');
            }
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
                        qty: "N/A",
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
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            } else {
                customErrorMessage('Please select a policy to add sundires.');
            }
        }
        $scope.validateSundry = function () {
            var isValid = true;
            if ($scope.sundry.name == '') {
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
                            customInfoMessage("Service record added successfully.")
                            $scope.clearServiceRecord();
                            $scope.GetServiceHistory($scope.currentPolicy.policyId);
                        } else {
                            if (data == '') {
                                customErrorMessage("Error occured while saving service history");
                            } else {
                                customErrorMessage(data);
                            }
                        }
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });
                } else {
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            } else {
                customErrorMessage('Please select a policy to add service history.');
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

            if ($scope.serviceRecord.serviceDate == '') {
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
            $scope.TotalClaimAmount = 0.00;
            angular.forEach($scope.claimItemList, function (claimItem) {
                $scope.TotalClaimAmount += parseFloat(claimItem.totalPrice);
            });
        }
        $scope.removeClaimItem = function (claimItemId) {
            if (parseInt(claimItemId)) {
                //remove item
                for (var i = 0; i < $scope.claimItemList.length; i++) {
                    if (claimItemId == $scope.claimItemList[i].id) {
                        $scope.claimItemList.splice(i, 1);
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
                customErrorMessage("Please select a policy.");
            }
        }
        $scope.selectedPartChanged = function (selectedItem) {
            if (isGuid(selectedItem.Id)) {
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

                        if (data.priceComment != "ok") {
                            customInfoMessage("Part's price is not found for your dealer. You can enter any amount and it will be taking as dealers price for selected part");
                        } else {
                            $scope.currentPart.unitPrice = data.price;
                        }

                        if (data.modelComment != 'ok') {
                            customInfoMessage("Item model (" + data.make + "-" + data.model + ") is not mapped with selected part.Once you submit the claim,Selected Part will mapped under " + data.make + "-" + data.model);
                        }
                    }

                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            } else {
                $scope.currentPart.partId = emptyGuid();
                $scope.currentPart.partNumber = '';
            }


        }
        $scope.selectedPartAreaChanged = function () {
            $scope.part.selected = undefined;
            $scope.currentPart.partNumber = '';
            $scope.currentPart.partQty = 1;
            $scope.currentPart.isRelatedPartsAvailable = false;
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
                    if (data == null)
                        customErrorMessage("No parts found for selected category.");
                    $scope.parts = data;
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
                        if (part.partId == partId) {
                            if (!parseFloat(part.price) || parseFloat(part.price) <= 0) {
                                customErrorMessage("Please set a valid value for selected part.");
                                return;
                            } else {
                                var nextId = $scope.claimItemList.length;
                                var claimItem = {
                                    id: (nextId + 1),
                                    partAreaId: part.partAreaId,
                                    partId: part.partId,
                                    itemName: part.partName,
                                    itemNumber: part.partNumber,
                                    qty: part.qty,
                                    unitPrice: parseFloat(part.price),
                                    totalPrice: (parseFloat(part.qty) * parseFloat(part.price)).toFixed(2),
                                    itemType: 'P',
                                    remarks: part.remark
                                };
                                $scope.claimItemList.push(claimItem);
                                if (parseFloat(part.allocatedHours) && parseFloat(part.allocatedHours) > 0) {
                                    $scope.labourCharge.chargeType = "H";
                                    $scope.labourCharge.hours = part.allocatedHours;
                                    $scope.labourCharge.description = "This allocated to part no -" + part.partNumber;
                                    if (parseFloat($scope.dealer.hourlyRate) > 0)
                                        $scope.labourCharge.hourlyRate = $scope.dealer.hourlyRate;

                                    $scope.addLabourCharge();
                                }
                                //update total
                                $scope.calculateTotalClaimAmount();
                                isSuccess = true;
                            }
                        }
                    }
                });
                if (isSuccess) {
                    $scope.partSuggestions.splice(counter, 1);
                }
                if ($scope.partSuggestions.length == 0) {
                    PartSuggestionPopup.close();
                }
            }
        }
        $scope.validateClaimDetails = function () {
            var isValid = true;

            if ($scope.complaint.customer === '') {
                $scope.validate_customerComplaint = "has-error";
                isValid = false;
            } else
                $scope.validate_customerComplaint = "";


            if ($scope.complaint.dealer === '') {
                $scope.validate_dealerComment = "has-error";
                isValid = false;
            } else
                $scope.validate_dealerComment = "";

            return isValid;
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

            //if ($scope.dealerInvoiceTireDetails.customerComplaintId == "") {
            //    $scope.validate_customerComplaintId = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_customerComplaintId = "";
            //}

            if ($scope.dealerInvoiceTireDetails.dealerCommentId == "") {
                $scope.validate_dealerCommentId = "has-error";
                isValid = false;
            } else {
                $scope.validate_dealerCommentId = "";
            }
          

            if ($scope.dealerInvoiceTireDetailsarr.frontRight.unusedTyreDepth != "" && $scope.dealerInvoiceTireDetailsarr.frontRight.unusedTyreDepth != null) {
                if ($scope.dealerInvoiceTireDetailsarr.frontRight.unusedTyreDepth < 3) {
                    customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
                    return false;
                }
            }

            if ($scope.dealerInvoiceTireDetailsarr.backRight.unusedTyreDepth != "" && $scope.dealerInvoiceTireDetailsarr.backRight.unusedTyreDepth != null) {
                if ($scope.dealerInvoiceTireDetailsarr.backRight.unusedTyreDepth < 3) {
                    customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
                    return false;
                }
            }

            if ($scope.dealerInvoiceTireDetailsarr.frontLeft.unusedTyreDepth != "" && $scope.dealerInvoiceTireDetailsarr.frontLeft.unusedTyreDepth != null) {
                if ($scope.dealerInvoiceTireDetailsarr.frontLeft.unusedTyreDepth < 3) {
                    customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
                    return false;
                }
            }

            if ($scope.dealerInvoiceTireDetailsarr.backLeft.unusedTyreDepth != "" && $scope.dealerInvoiceTireDetailsarr.backLeft.unusedTyreDepth != null) {
                if ($scope.dealerInvoiceTireDetailsarr.backLeft.unusedTyreDepth < 3) {
                    customErrorMessage("Tyre minimum legal tread depth is 3 mm.");
                    return false;
                }
            }


            return isValid;

        }

        $scope.updateOtherTireClaim = function () {

            //if (isGuid($scope.currentPolicy.policyId)) {
                if (isGuid($scope.currentPolicy.policyId)) {
                    angular.forEach($scope.claimItemList, function (value) {
                        if (value.qty == "N/A") {
                            value.qty = 0;
                        }
                    });
                if ($scope.validateOtherTireDetails()) {
                    $scope.currentPolicy.lastServiceDate = "1/1/1753";
                    if ($scope.docUploader.queue.length === 0) {
                        swal({ title: 'Processing...!', text: 'Submitting your claim update request...', showConfirmButton: false });
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
                            commentDealer: $scope.commentDealer,
                            otherTireUpdateDetails: $scope.dealerInvoiceTireDetailsarr,
                            InvoiceCodeId: $scope.dealerInvoiceTireDetails.InvoiceCodeId,
                            OtherTireDetails: $scope.dealerInvoiceTireDetails
                        };
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/UpdateOtherTireClaim',
                            dataType: 'json',
                            data: {
                                "claimData": data
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'ok') {
                                swal("TAS Information", "Claim updated successfully.You will redirect to claim listing screen shortly.", "success");
                                $scope.clearAll();
                                //TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                setTimeout(function () { swal.close(); }, 5000);
                                $state.go('app.claimlisting');
                            } else {
                                swal("TAS Information", data, "error");
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
                            if ($scope.Products[0].Productcode == "TYRE") {
                                $scope.updateOtherTireClaim();
                            } else
                            {
                                $scope.updateClaim();
                            }
                            
                        } else {
                            for (var i = 0; i < $scope.docUploader.queue.length; i++) {
                                $scope.docUploader.queue[i].file.name = $scope.docUploader.queue[i].file.name + '@@' + $scope.docUploader.queue[i].documentType;
                            }
                            $scope.docUploader.uploadAll();
                        }
                    }
                } else {
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            } else {
                customErrorMessage("Please select a policy.");
            }
        }



        $scope.updateClaim = function () {
            if (isGuid($scope.currentPolicy.policyId)) {
                angular.forEach($scope.claimItemList, function (value) {
                    if (value.qty == "N/A") {
                        value.qty = 0;
                    }
                });

                if (!$scope.validateClaimDetails()) {
                    customErrorMessage("Please fill complaint details.");
                    return false;
                }
                if ($scope.claimItemList.length > 0) {
                    if ($scope.docUploader.queue.length == 0) {
                        swal({ title: 'Processing...!', text: 'Submitting your claim update request...', showConfirmButton: false });
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
                                swal("TAS Information", "Claim updated successfully.You will redirect to claim listing screen shortly.", "success");
                                $scope.clearAll();
                                //TasNotificationService.getClaimListSyncState($localStorage.LoggedInUserId);
                                setTimeout(function () { swal.close(); }, 5000);
                                $state.go('app.claimlisting');
                            } else {
                                swal("TAS Information", data, "error");
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
                    customErrorMessage("No caim items selected.");
                }
            } else {
                customErrorMessage("Please select a policy.");
            }
        }
        $scope.clearAll = function () {
            $scope.currentPolicy = {
                policyId: emptyGuid(),
                customerId: emptyGuid(),
                commodityCategoryId: emptyGuid(),
                makeId: emptyGuid(),
                modelId: emptyGuid(),
                commodityTypeId: emptyGuid(),
                dealerId: emptyGuid()
            };
            $scope.claimItem = {
                id: 0,
                itemId: emptyGuid(),
                itemName: '',
                itemNumber: '',
                qty: 0,
                unitPrice: 0.00,
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
                allocatedHours: 0
            };
            $scope.labourCharge = {
                chargeType: 'H',
                hourlyRate: 0.00,
                hours: 0,
                totalAmount: 0.00,
                description: ''
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
            $scope.TotalClaimAmount = 0.00;

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
        }




        $scope.loadPolicy = function (PolicyId, CommodityTypeId, DealerId, MakeId, ModelId, CustomerId, CommodityCategoryId) {

            if (isGuid(PolicyId)) {            

                swal({ title: 'Loading...', text: 'Reading Policy Information', showConfirmButton: false });
                $scope.currentPolicy.policyId = PolicyId;
                $scope.currentPolicy.customerId = CustomerId;
                $scope.currentPolicy.commodityCategoryId = CommodityCategoryId;
                $scope.currentPolicy.makeId = MakeId;
                $scope.currentPolicy.modelId = ModelId;
                $scope.currentPolicy.commodityTypeId = CommodityTypeId;
                $scope.currentPolicy.dealerId = DealerId;
                $scope.currentPolicy.failureMileage = $scope.claimSubmission.failureMileage;


                if (isGuid($scope.currentPolicy.commodityTypeId)) {
                    angular.forEach($scope.commodityTypes, function (value) {
                        if ($scope.currentPolicy.commodityTypeId === value.CommodityTypeId) {
                            if (value.CommodityCode.trim() == "A") {
                                $scope.isOtherProductSelected = false;
                            } else if (value.CommodityCode.trim() == "O") {
                                if ($scope.Products[0].Productcode == "TYRE") {
                                    $scope.isOtherProductSelected = true;

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/claim/LoadClaimDetailsForClaimEditOtherTire',
                                        data: { "claimId": $scope.claimId, "loggedInUserId": $localStorage.LoggedInUserId },
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        if (data == null)
                                            customErrorMessage("No parts found for selected category.");
                                        $scope.currentPolicy.plateNumber = data.ClaimDetails.PlateNo;
                                        $scope.currentPolicy.failureMileage = data.PolicyDetails.ClaimMileage;
                                        
                                        $scope.claimSubmissionId = data.ClaimId;
                                        angular.forEach($scope.DealerComments, function (value) {
                                            if (value.Comment == data.PolicyDetails.DealerComment) {
                                                $scope.dealerInvoiceTireDetails.dealerComment = value.Comment;
                                            }
                                        });                                       

                                        angular.forEach(data.ClaimDetailsTire.requestDetailsOtherTires, function (value) {
                                            $scope.dealerInvoiceTireDetails.InvoiceCodeId = value.InvoiceCodeId;
                                            if (value.RemarkPosition == "FL") {

                                                $scope.dealerInvoiceTireDetailsarr.frontLeft.claimItemId = value.claimSubmissionItemId;
                                                $scope.dealerInvoiceTireDetailsarr.frontLeft.serial = value.SerialNumber;
                                                $scope.dealerInvoiceTireDetailsarr.frontLeft.unusedTyreDepth = value.UnUsedTireDepth;
                                                $scope.frontleftPositionDisabled = false;
                                            }
                                            if (value.RemarkPosition == "FR") {
                                                $scope.dealerInvoiceTireDetailsarr.frontRight.claimItemId = value.claimSubmissionItemId
                                                $scope.dealerInvoiceTireDetailsarr.frontRight.serial = value.SerialNumber
                                                $scope.dealerInvoiceTireDetailsarr.frontRight.unusedTyreDepth = value.UnUsedTireDepth;

                                                $scope.frontrightPositionDisabled = false;
                                               
                                            }
                                            if (value.RemarkPosition == "BR") {
                                                $scope.dealerInvoiceTireDetailsarr.backRight.claimItemId = value.claimSubmissionItemId
                                                $scope.dealerInvoiceTireDetailsarr.backRight.serial = value.SerialNumber
                                                $scope.dealerInvoiceTireDetailsarr.backRight.unusedTyreDepth = value.UnUsedTireDepth;

                                                $scope.backrightPositionDisabled = false;
                                            }
                                            if (value.RemarkPosition == "BL") {
                                                $scope.dealerInvoiceTireDetailsarr.backLeft.claimItemId = value.claimSubmissionItemId
                                                $scope.dealerInvoiceTireDetailsarr.backLeft.serial = value.SerialNumber
                                                $scope.dealerInvoiceTireDetailsarr.backLeft.unusedTyreDepth = value.UnUsedTireDepth;
                                                $scope.backleftPositionDisabled = false;
                                                
                                            }
                                        });
                                        
                                    }).error(function (data, status, headers, config) {

                                    }).finally(function () {
                                    });

                                }

                            } else {
                                //other commodities
                                $scope.isOtherProductSelected = false;
                            }
                        } else {
                            $scope.isOtherProductSelected = false;
                        }
                    });
                }


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ReadPolicyInformation',
                    data: { "policyId": PolicyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null) {
                        customErrorMessage("Error occured while reading policy data.");
                        swal.close();
                    } else {
                        // $scope.currentPolicy.policyNumber = data.PolicyNo;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/GetAllPartAreasByCommodityCategoryId',
                            data: { "commodityCategoryId": CommodityCategoryId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == null)
                                customErrorMessage("No part areas found for selected policy's category.");
                            $scope.partAreas = data;
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
                customErrorMessage("Invalid policy selection.");
            }
        };

        $scope.GetServiceHistory = function (PolicyId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllServiceHistoryByPolicyId',
                data: { "policyId": PolicyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null)
                    if ($scope.Products[0].Productcode == "TYRE") {
                        $scope.isOtherProductSelected = true;
                    } else {
                        customErrorMessage("No service records found.");
                    }
                    
                $scope.itemServiceRecords = data;
            }).error(function (data, status, headers, config) {

            }).finally(function () {

            });
        }
        $scope.updateServiceRecord = function (serviceRecordId) {

            angular.forEach($scope.itemServiceRecords, function (value) {
                if (value.Id == serviceRecordId) {
                    $scope.serviceRecord.id = value.Id;
                    $scope.serviceRecord.serviceNumber = value.ServiceNumber;
                    $scope.serviceRecord.milage = value.Milage;
                    $scope.serviceRecord.remarks = value.Remarks;
                    $scope.serviceRecord.serviceDate = value.ServiceDate;
                }
            });

        }
        $scope.downloadAttachment = function (ref) {
            if (ref != '') {
                // swal({ html: true, title: '<h2 class="saving">Processing<span>.</span><span>.</span><span>.</span></h2>', text: 'Preparing your download...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Upload/DownloadAttachment',
                    data: { 'fileRef': ref },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
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
                    //    swal.close();
                });
            }
        }
        $scope.removeAttachment = function (ref) {
            if (ref != '') {
                for (var j = 0; j < $scope.docUploader.queue.length; j++) {
                    if (ref == $scope.docUploader.queue[j].ref) {
                        // $scope.removedAttachments.push($scope.docUploader[i].Id);
                        $scope.docUploader.queue.splice(j, 1);
                    }
                }
            }
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
                } else {
                    $scope.isFrontLeftTireDetailsVisible = true;
                }
            } else if (position === 'F' && elem === 'R') {
                if ($scope.isFrontRightTireDetailsVisible) {
                    $scope.isFrontRightTireDetailsVisible = false;
                } else {
                    $scope.isFrontRightTireDetailsVisible = true;
                }
            }
            else if (position === 'B' && elem === 'L') {
                if ($scope.isBackLeftTireDetailsVisible) {
                    $scope.isBackLeftTireDetailsVisible = false;
                } else {
                    $scope.isBackLeftTireDetailsVisible = true;
                }
            }
            else if (position === 'B' && elem === 'R') {
                if ($scope.isBackRightTireDetailsVisible) {
                    $scope.isBackRightTireDetailsVisible = false;
                } else {
                    $scope.isBackRightTireDetailsVisible = true;
                }
            }
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
                if ($scope.currentPolicy.docAttachmentType == null ||
                    $scope.currentPolicy.docAttachmentType == 'undefined' ||
                    $scope.currentPolicy.docAttachmentType == "") {
                    customErrorMessage("Please Select Attachment Type");
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
                DocattachmentType = $scope.docAttachmentType; console.log(DocattachmentType);
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

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
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

