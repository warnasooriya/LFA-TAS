
app.controller('ClaimProcessCtrlB2',
    function ($scope, $rootScope, $http, ngDialog, $location, SweetAlert, $localStorage, $cookieStore, $filter, toaster, FileUploader, $state, ngTableParams, $stateParams) {
        var ClaimItemEntryPopup,
            ProcessClaimItemPopup,
            PolicySelectionPopup,
            PolicyDetailsPopup,
            CoverDetailsPopup,
            PopupAssesmentHistory,
            ClaimHistoryPopup,
            InitialPopup,
            ClaimSelectionPopup,
            getPolicySearchPage,
            AddNewPartPopup,
            AttachmnetDetailsPopup,
            FaultSearchPopup;


        $scope.docNotes = [];
        //$scope.itemDocUploader.queue = [];

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };
        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        $scope.claimId = $stateParams.claimId;
        $scope.IsEndorsement = $stateParams.endorse;

        $scope.policyId = emptyGuid();
        $scope.dealerId = emptyGuid();
        $scope.policyDealerId = emptyGuid();
        $scope.policyCountryId = emptyGuid();
        $scope.partAreas = [];
        $scope.part = {};
        $scope.newPart = {};
        $scope.newcommodityTypes = {};
        $scope.makes = [];
        $scope.serviceRecord = {};
        $scope.sundry = {};
        $scope.parts = [];
        $scope.policyDetails = {};
        $scope.assesmentHistory = [];
        $scope.claimHistory = [];
        $scope.uploadedDocIds = [];
        $scope.discountSchemes = [];
        $scope.partRejectionTypes = [];
        $scope.extendedClaimItems = [];
        $scope.claimStatusCommentVisible = false;
        $scope.isGoodwillVisible = false;
        $scope.isProductTire = false;
        $scope.totalauthorizedAmount = "";
        $scope.userType = '';
        $scope.isProductIloe = false;
        $scope.faultSearch = {
            categoryCode: '',
            areaCode: '',
            code: '',
            description: '',
            policyId: emptyGuid(),
            claimId: emptyGuid()
        };
        $scope.isPartDiscountDisabled = false;
        $scope.isLabourchargeTabActive = false;
        $scope.assesmentTabActive = false;
        $scope.newPart = {
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

        $scope.isrefnovalid = false;

        $scope.claimProcess = {
            policyNumber: '',
            claimNumber: 'N/A',
            currencyCode: '',
            totalClaimAmount: '',
            authorizedClaimAmount: '',
            country: '',
            claimDealer: '',
            claimDealerId: '',
            policyDealer: '',
            claimItemList: '',
            discountType: 'NA',
            customerName: '',
            vinSerial: '',
            make: '',
            model: '',
            claimDealers: [],
            complaint: {
                customer: '',
                dealer: 'Testiloe',
                engineer: 'testIloe',
                conclution: ''
            },
            claimStatus: 'NA',
            claimDate: '',
            docAttachmentType: '',
            comment: '',
            countryTaxes: [],
            ClaimAmount: '',
            totalLiability: ''
            
        }
        $scope.currentClaimItem = {
            fault: emptyGuid()
        };

        $scope.claimView = {
            id: emptyGuid(),
            policyNumber: '',
            claimNumber: '',
            claimStatus: '',
            claimStatusCode: '',
            totalClaimAmount: '',
            currencyCode: '',
            make: '',
            model: '',
            dealer: '',
            date: '',
            country: '',
            requestedUser: '',
            attachments: [],
            serviceHistory: [],
            customerName: '',
            vinNo: '',
            plateNo: '',
            lastServiceMileage: 0,
            lastServiceDate: '',
            repairCenter: '',
            repairCenterLocation: '',
            makeCS: '',
            modelCS: '',
            customerNameCS: '',
            Complaint: {
                customer: '',
                dealer: ''
            },
            TotalClaimAmount: '',
            TotalDiscountAmount: '',
            claimRejectionTypeId: emptyGuid(),
            claimRejectionComment: '',
            claimNotes: [],
            claimHistory: [],
            claimAssessment: [],
            claimMessage: [],
            totalLiability: ''

        };

        $scope.extendedClaimItemRow = {
            id: 0,
            partNumber: '',
            partName: '',
            partQty: 1,
            unitPrice: '',
            remark: '',
            goodWillType: 'NA',
            goodWillValue: 0.00,
            goodWillAmount: 0.00,
            discountType: 'NA',
            discountValue: 0.00,
            discountAmount: 0.00,
            fault: '',
            faultId: emptyGuid(),
            partDiscountScheme: 'NA',
            partDiscountSchemeName: '',
            rejectionType: '',
            rejectionTypeId: emptyGuid(),
            nettAmount: 0.00,
            authorizedAmount: 0.00,
            itemStatus: '',

            l_chargeType: 'H',
            l_hourlyRate: 0.00,
            l_hours: 0,
            l_totalAmount: 0.00,
            l_description: '',
            l_goodWillType: 'NA',
            l_goodWillValue: 0.00,
            l_goodWillAmount: 0.00,
            l_discountType: 'NA',
            l_discountValue: 0.00,
            l_discountAmount: 0.00,
            l_labourDiscountScheme: 'NA',
            l_labourDiscountSchemeName: '',
            l_nettAmount: 0.00,
            l_authorizedAmount: 0.00
        };

        $scope.currentPart = {
            id: 0,
            partAreaId: emptyGuid(),
            partId: emptyGuid(),
            partNumber: '',
            partName: '',
            partQty: 1,
            unitPrice: 0.00,
            remark: '',
            isRelatedPartsAvailable: false,
            allocatedHours: 0,
            goodWillType: 'NA',
            goodWillValue: 0.00,
            goodWillAmount: 0.00,
            discountType: 'NA',
            discountValue: 0.00,
            discountAmount: 0.00,
            fault: emptyGuid(),
            serverId: emptyGuid(),
            partDiscountScheme: 'NA',
            rejectionTypeId: emptyGuid(),
            nettAmount: 0.00,
            authorizedAmount: 0.00,
            itemStatus: '',
            causeOfFailureId: ""
        };
        $scope.labourCharge = {
            chargeType: 'H',
            hourlyRate: 0.00,
            hours: 0,
            totalAmount: 0.00,
            description: '',
            partId: emptyGuid(),
            goodWillType: 'NA',
            goodWillValue: 0.00,
            goodWillAmount: 0.00,
            discountType: 'NA',
            discountValue: 0.00,
            discountAmount: 0.00,
            labourDiscountScheme: 'NA',
            nettAmount: 0.00,
            authorizedAmount: 0.00
        };
        $scope.sundry = {
            name: '',
            value: 0.00
        };

        $scope.claimNotes = {
            id: emptyGuid(),
            policyId: emptyGuid(),
            claimId: emptyGuid(),
            submittedUserId: emptyGuid(),
            note: ''
        };

        $scope.claimComment = {

            id: emptyGuid(),
            policyId: emptyGuid(),
            claimId: emptyGuid(),
            comment: '',
            sentFrom: emptyGuid(),
            sentTo: emptyGuid(),
            byTPA: 0,
            seen: 0,
        };

        $scope.GetStatusStyleByCode = function (code) {
            switch (code) {
                case "SUB":
                    return "alert-info";
                    break;
                case "REQ":
                    return "alert-warning";
                    break;
                case "HOL":
                    return "alert-info";
                    break;
                case "REV":
                    return "alert-info";
                    break;
                case "REJ":
                    return "alert-danger";
                    break;
                case "APP":
                    return "alert-success";
                    break;
                    defaut:
                    return "";
            }
        }

        $scope.settings = {
            scrollableHeight: '150px',
            scrollable: true,
            enableSearch: true,
            showCheckAll: false,
            closeOnBlur: false,
            showUncheckAll: false,
            closeOnBlur: true,
            closeOnSelect: true,
            onItemSelect: function (item) { $scope.SendTaxes(item); },
            onItemDeselect: function (item) { $scope.SendTaxess(item); }

        };
        $scope.CustomText = {
            buttonDefaultText: ' Please Select ',
            dynamicButtonTextSuffix: ' Item(s) Selected'
        };

        $scope.TaxesList = [];
        $scope.SelectedTaxesList = [];
        $scope.SelectedTaxesDList = [];

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
            } else {
                customErrorMessage("Error occured while uploading Attachments.");
                $scope.docUploader.cancelAll();

            }
        }
        $scope.docUploader.onCompleteAll = function () {
            $scope.saveAttachmentsToClaim();
        }
        $scope.docUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                // alert($scope.claimProcess.docAttachmentType);
                if ($scope.claimProcess.docAttachmentType !== "" && typeof $scope.claimProcess.docAttachmentType !== 'undefined') {
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
            $http({
                method: 'POST',
                url: '/TAS.Web/api/FaultManagement/GetAllFault',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },

            }).success(function (data, status, headers, config) {
                $scope.faults = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GoodwillAuthorizationByUserId',
                data: { "loggedInUserId": $localStorage.LoggedInUserId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
            }).success(function (data, status, headers, config) {
                $scope.isGoodwillVisible = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDiscountSchemes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.discountSchemes = data;
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
                $scope.newcommodityTypes = data;

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.commodityTypes[0].CommodityTypeId }
                }).success(function (data, status, headers, config) {
                    $scope.Products = data;
                    $scope.Products = data;
                    if ($scope.Products[0].Productcode == "TYRE") {
                        $scope.isProductTire = true;


                    }
                    else if ($scope.Products[0].ProductTypeCode == "ILOE") {
                        $scope.isProductIloe = true;
                    }
                }).error(function (data, status, headers, config) {
                });

            }).error(function (data, status, headers, config) {
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
                url: '/TAS.Web/api/claim/GetAllPartRejectioDescription',
                dataType: 'json',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.partRejectionTypes = data;
            }).error(function (data, status, headers, config) {
            });

            if ($scope.IsEndorsement) {
                //endorsement
                if (!isGuid($scope.claimId)) {
                    customErrorMessage("Invalid claim selection.");
                } else {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidateClaimIdForEndorsement',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: {
                            "claimId": $scope.claimId,
                            "policyId": emptyGuid(),
                            "loggedInUserId": $localStorage.LoggedInUserId
                        }
                    }).success(function (data, status, headers, config) {
                        if (data.msg === "ok") {
                            $scope.selectClaimToProcess($scope.claimId, data.policyId);
                        } else {
                            swal("TAS Information", data.msg, "error");
                            setTimeout(function () { swal.close(); }, 5000);
                            $state.go('app.claimendorsement');
                        }
                    }).error(function (data, status, headers, config) {
                    });
                }
            } else {
                if (!isGuid($scope.claimId)) {
                    //came through menu
                    $scope.showInitialSelection();
                } else {
                    //from listing
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ValidateClaim',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: {
                            "claimId": $scope.claimId,
                            "policyId": $scope.policyId,
                            "loggedInUserId": $localStorage.LoggedInUserId
                        },
                    }).success(function (data, status, headers, config) {
                        if (data.status === "ok") {
                            $scope.selectClaimToProcess($scope.claimId, data.policyId);
                        } else if (data.status === "warning") {
                            swal("TAS Information", data.msg, "error");
                        } else {
                            swal("TAS Information", data.msg, "error");
                        }
                    }).error(function (data, status, headers, config) {
                    });
                }
            }

            //if (isGuid($scope.claimId) || isGuid($scope.policyId)) {
            //    ClaimItemEntryPopup = ngDialog.open({
            //        template: 'popUpAddClaimItem',
            //        className: 'ngdialog-theme-plain',
            //        closeByEscape: true,
            //        showClose: true,
            //        closeByDocument: true,
            //        scope: $scope,
            //        preCloseCallback: function () {
            //            if (isGuid($scope.claimId)) {
            //                if ($location.path() === '/app/claim/process/') {
            //                    $location.path('app/claim/process/' + $scope.claimId);
            //                }
            //            }
            //        }
            //    });
            //}
        };
        $scope.selectedFaultChanged = function () {
            if (isGuid($scope.currentPart.fault)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/FaultManagement/GetAllCauseOfFailuresByFaultId',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { Id: $scope.currentPart.fault },
                }).success(function (data, status, headers, config) {
                    $scope.faultCauseOfFailure = data;
                    if (data.length === 0) {
                        customErrorMessage('No Cause of Failures found for selected fault.');
                    }
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.faultCauseOfFailure = [];
            }
        }


        $scope.viewClaim = function (claimReqId, make, model, dealer, date, claimStatus) {
            if (isGuid(claimReqId)) {
                swal({ html: true, title: '<h2 class="saving">Loading<span>.</span><span>.</span><span>.</span></h2>', text: 'Reading claim information', showConfirmButton: false });
                $scope.getAllOriginalPolicyDetailsByClaimId(claimReqId);
                $scope.GetAllClaimHistoryDetailsByClaimId(claimReqId);
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetClaimRequestByClaimId',
                    data: { "claimId": claimReqId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimView.id = claimReqId;
                    $scope.claimView.policyId = data.PolicyId;
                    $scope.claimView.totalLiability = data.totalLiability.toFixed(2);
                    $scope.claimView.policyNumber = data.PolicyNumber;
                    $scope.claimView.claimNumber = (data.ClaimNumber == '') ? 'N/A' : data.ClaimNumber;
                    $scope.claimView.totalClaimAmount = data.TotalClaimAmount + ' ' + data.CurrencyCode;
                    $scope.claimView.currencyCode = data.CurrencyCode;

                    //$scope.claimView.make = make;
                    //$scope.claimView.model = model;
                    //$scope.claimView.dealer = dealer;
                    //$scope.claimView.date = date;

                    $scope.claimView.country = data.Country;
                    $scope.claimView.claimStatusCode = data.ClaimStatus;
                    $scope.claimView.claimItemList = data.ClaimItemList;
                    $scope.claimView.requestedUser = data.RequestedUser;
                    $scope.claimView.customerName = data.CustomerName;
                    $scope.claimView.vinNo = data.VINNO;
                    $scope.claimView.plateNo = data.PlateNo
                    $scope.claimView.lastServiceMileage = data.LastServiceMileage;
                    $scope.claimView.lastServiceDate = data.LastServiceDate;
                    $scope.claimView.repairCenter = data.RepairCenter;
                    $scope.claimView.repairCenterLocation = data.RepairCenterLocation;
                    $scope.claimView.modelCS = data.Model;
                    $scope.claimView.makeCS = data.Make;
                    $scope.claimView.customerNameCS = data.CustomerNameCS;
                    $scope.claimView.AuthorizedClaimAmount = data.AuthorizedClaimAmount;
                    $scope.claimView.TotalClaimAmount = data.TotalClaimAmount;
                    $scope.claimView.TotalDiscountAmount = data.TotalDiscountAmount;
                    $scope.claimView.Complaint.customer = data.Complaint.customer;
                    $scope.claimView.Complaint.dealer = data.Complaint.dealer;
                    $scope.claimView.ClaimDate = data.ClaimDate;
                    $scope.claimView.claimAttachments = data.Attachments.Attachments;
                    $scope.claimView.serviceHistory = data.ServiceHistory;
                    $scope.claimView.serviceHistory = data.ClaimAmount;
                    if (data.ClaimNumber == '') {
                        $scope.claimView.claimNumber = data.Wip;
                        $scope.isrefnovalid = true;

                    }
                    ClaimViewPopup = ngDialog.open({
                        template: 'popUpViewClaimRequest',
                        className: 'ngdialog-theme-plain',
                        closeByEscape: true,
                        showClose: true,
                        closeByDocument: true,
                        scope: $scope
                    });
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            } else {
                customErrorMessage("Invalid claim request selection.");
            }
        }
        $scope.loadClaimRequestById = function (claimId) {
            if (isGuid(claimId)) {
                ClaimHistoryPopup.close();
                $scope.viewClaim(claimId);
            }
        }
        $scope.getAllOriginalPolicyDetailsByClaimId = function (claimReqId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllOriginalPolicyDetailsByClaimId',
                data: { "claimId": claimReqId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimView.oPolicyNumber = data.PolicyNumber;
                $scope.claimView.oPolicyDealer = data.PolicyDealer;
                $scope.claimView.oUsageType = data.PolicyType;

                $scope.claimView.oClaimNumber = data.ClaimNumber;
                $scope.claimView.oMakeModel = data.MakeModel;
                $scope.claimView.oModelYear = data.ModelYear;

                $scope.claimView.oCustomerName = data.CustomerName;
                $scope.claimView.oMobileNumber = data.MobileNumber;
                $scope.claimView.oReinsurer = data.Reinsurer;
                $scope.claimView.oCedent = data.Cedent;

                $scope.claimView.oPlateNumber = data.PlateNumber;
                $scope.claimView.oCylinderCount = data.CylinderCount;
                $scope.claimView.oEngineCapacity = data.EngineCapacity;

                $scope.claimView.oUWYear = data.UWYear;
                $scope.claimView.oVin = data.VIN;
                $scope.claimView.oSalePrice = data.SalePrice;

                $scope.claimView.oMWStart = data.MWStart;
                $scope.claimView.oExtStart = data.ExtStart;
                $scope.claimView.oMWExp = data.MWEnd;
                $scope.claimView.oExtExp = data.ExtEnd;

                $scope.claimView.oMWMonths = data.MWMonths;
                $scope.claimView.oExtMonths = data.EXTMonths;

                $scope.claimView.oMWMileage = data.MWMileage;
                $scope.claimView.oExtMileage = data.ExtMileage;
                $scope.claimView.oCutoffKm = data.CutoffMileage;


            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });
        }
        $scope.GetAllClaimHistoryDetailsByClaimId = function (claimReqId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllClaimHistoryDetailsByClaimId',
                data: {
                    "claimId": claimReqId,
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
        $scope.selectedPartAreaChanged = function () {
            $scope.part.selected = undefined;

            $scope.currentPart.partQty = 1;
            $scope.currentPart.isRelatedPartsAvailable = false;
            if (isGuid($scope.currentPart.partAreaId) && isGuid($scope.makeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllPartsByPartAreaMakeId',
                    data: {
                        "partAreaId": $scope.currentPart.partAreaId,
                        "makeId": $scope.makeId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        customErrorMessage("No parts found for selected category.");
                    $scope.parts = data;

                    if (isGuid($scope.defaultPartId)) {
                        angular.forEach($scope.parts, function (part) {
                            if (part.Id === $scope.defaultPartId) {
                                $scope.part.selected = part;
                            }
                        });
                    } else {
                        $scope.currentPart.partNumber = '';
                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                });
            } else {

            }
        }
        $scope.selectedPartChanged = function (selectedItem) {
            if (isGuid(selectedItem.Id)) {
                $scope.currentPart.partId = selectedItem.Id;
                $scope.partId = selectedItem.Id;
                $scope.currentPart.partNumber = selectedItem.PartNumber;
                $scope.currentPart.partName = selectedItem.PartName;
                swal({ title: 'Loading...', text: 'Validating loan installment information', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ValidatePartInformation',
                    data: {
                        "partId": selectedItem.Id,
                        "dealerId": $scope.dealerId,
                        "modelId": $scope.modelId,
                        "makeId": $scope.makeId
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
                            $scope.currentPart.unitPrice = $scope.currentPart.authorizedAmount;
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
        $scope.saveAttachmentsToClaim = function () {
            if ($scope.uploadedDocIds.length == 0) {
                customErrorMessage('Document uploading process returned error.');
                return;
            }
            if (isGuid($scope.claimId)) {
                var data = {
                    'claimId': $scope.claimId,
                    'docIds': $scope.uploadedDocIds
                };
                swal({ title: 'Processing...', text: 'Saving attachments against claim', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/SaveAttachmentsToClaim',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    if (data.code === "success") {
                        customInfoMessage("Attachments saved successfully.")
                        //get attachments
                    } else {
                        customErrorMessage('Error occured while saving attachments.');
                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            } else {
                customErrorMessage('Selected claim invalid.');
            }

        }
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

            $scope.currentPart.nettAmount = (parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty))
                - parseFloat($scope.currentPart.goodWillAmount) - parseFloat($scope.currentPart.discountAmount);
            $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
        }
        $scope.discountChanging = function () {
            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                    //part has total price
                    if ($scope.currentPart.discountType == "P") {
                        if (parseFloat($scope.currentPart.discountValue) && parseFloat($scope.currentPart.discountValue) > 0) {
                            var totalPriceOfPart = parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty);
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
                    } else if ($scope.currentPart.discountType == "F") {
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
            $scope.currentPart.nettAmount = (parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty))
                - parseFloat($scope.currentPart.goodWillAmount) - parseFloat($scope.currentPart.discountAmount);
            $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
        }
        $scope.validatePartDetails = function () {
            var isValid = true;
            if (!isGuid($scope.currentPart.partAreaId)) {
                $scope.validate_partArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partArea = "";
            }

            if (!isGuid($scope.currentPart.causeOfFailureId)) {
                $scope.validate_causeOfFailureId = "has-error";
                isValid = false;
            } else {
                $scope.validate_causeOfFailureId = "";
            }

            if (!parseFloat($scope.labourCharge.hourlyRate) && parseInt($scope.labourCharge.hours) > 0) {
                $scope.validate_hourlyRate = "has-error";
                isValid = false;
            } else {
                $scope.validate_hourlyRate = "";
            }

            if (!isGuid($scope.currentPart.partId)) {
                $scope.validate_part = "has-error";
                customErrorMessage("Please select a part.");
                isValid = false;
            } else {
                $scope.validate_part = "";
            }

            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                $scope.validate_partQty = "";
            } else {

                $scope.validate_partQty = "has-error";
                isValid = false;
            }

            if ($scope.Products[0].ProductTypeCode == "TYRE") {

            } else {
                if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                    $scope.validate_unitPrice = "";
                } else {

                    $scope.validate_unitPrice = "has-error";
                    isValid = false;
                }
            }

            //validate discount & Good will
            if ($scope.currentPart.goodWillType != 'NA') {
                if (parseFloat($scope.currentPart.goodWillValue) && parseFloat($scope.currentPart.goodWillValue) > 0) {
                    $scope.validate_goodWillValue = "";
                } else {
                    $scope.validate_goodWillValue = "has-error";
                    isValid = false;
                }

            } else {
                $scope.validate_goodWillValue = "";
                $scope.currentPart.goodWillValue = 0.00;
                $scope.currentPart.goodWillAmount = 0.00;
            }

            if ($scope.currentPart.discountType != 'NA') {
                if (parseFloat($scope.currentPart.discountValue) && parseFloat($scope.currentPart.discountValue) > 0) {
                    $scope.validate_discountValue = "";
                } else {
                    $scope.validate_discountValue = "has-error";
                    isValid = false;
                }

            } else {
                $scope.validate_discountValue = "";
                $scope.currentPart.discountValue = 0.00;
                $scope.currentPart.discountAmount = 0.00;
            }
            //validate discount sum
            if (isValid) {
                var totalPriceOfPart = parseFloat($scope.currentPart.unitPrice) * parseInt($scope.currentPart.partQty);
                var totalDiscounts = parseFloat($scope.currentPart.discountAmount) + parseFloat($scope.currentPart.goodWillAmount);

                if (totalPriceOfPart < totalDiscounts) {
                    $scope.validate_discountValue = "has-error";
                    $scope.validate_goodWillValue = "has-error";
                    customErrorMessage("Good will + Discount cannot be exceed total part(s) amount.");
                    isValid = false;
                }
            }
            //fault
            if (!isGuid($scope.currentPart.fault)) {
                $scope.validate_fault = "has-error";
                isValid = false;
            } else {
                $scope.validate_fault = "";
            }
            //rejection type
            if ($scope.currentPart.itemStatus === 'R') {
                if ($scope.Products[0].Productcode != "TYRE") {
                    if (!isGuid($scope.currentPart.rejectionTypeId)) {
                        $scope.validate_rejectionType = "has-error";
                        isValid = false;
                    } else {
                        $scope.validate_rejectionType = "";
                    }
                } else {
                    if (!isGuid($scope.currentPart.rejectionTypeId)) {
                        $scope.validate_rejectionType = "has-error";
                        isValid = false;
                    } else {
                        $scope.validate_rejectionType = "";
                        $scope.currentPart.authorizedAmount = 0;
                    }
                }

            }
            //authorized amount
            if (parseFloat($scope.currentPart.authorizedAmount) && parseFloat($scope.currentPart.authorizedAmount) > 0) {
                if ((parseFloat($scope.currentPart.nettAmount) + parseFloat($scope.labourCharge.nettAmount)) <
                    parseFloat($scope.currentPart.authorizedAmount)) {

                    //parseFloat($scope.labourCharge.nettAmount)
                    $scope.validate_authorizedAmount = "has-error";
                    customErrorMessage("Authorized amount cannot exceed part and labour nett amounts.");
                    isValid = false;
                } else {
                    $scope.validate_authorizedAmount = "";
                }
            }
            //else {
            //    $scope.validate_authorizedAmount = "has-error";
            //    isValid = false;
            //}


            if ($scope.currentPart.itemStatus === '') {
                customErrorMessage("Please select item status.");
                $scope.validate_itemStatus = "has-error";
                isValid = false;

            } else {
                $scope.validate_itemStatus = "";
            }

            if (!isValid)
                customErrorMessage("Please fill the highlighted fields.");
            return isValid;
        }
        $scope.addPart = function () {
           
                if ($scope.IsEndorsement) {
                    var index = 0;
                    if ($scope.updatePartMode) {
                        angular.forEach($scope.extendedClaimItems, function (displayItem) {

                            if (displayItem.serverId === $scope.currentPart.serverId) {
                                $scope.extendedClaimItems.splice(index, 1);
                            }
                            index++;
                        });
                    }

                    $scope.syncWithExtendedGridData($scope.labourCharge, $scope.currentPart);
                    $scope.clearPartDetails();
                } else {

                    $scope.labourCharge.partId = $scope.currentPart.partId;
                    $scope.currentPart.causeOfFailureId = emptyGuid();
                    var data = {
                        "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                        "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,
                        "part": $scope.currentPart,
                        "labour": $scope.labourCharge,
                        "loggedInUserId": $localStorage.LoggedInUserId,
                        "dealerId": typeof $scope.dealerId === 'undefined' ? emptyGuid() : $scope.dealerId,
                        "isUpdate": $scope.updatePartMode,
                       
                    };

                    swal({ title: 'Processing...', text: 'Saving Part Information', showConfirmButton: false });
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/SavePartWithClaim',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        console.log($scope.extendedClaimItems);
                        if (data.Status === "ok") {
                            $scope.claimProcess.claimNumber = data.ClaimNo;
                            $scope.claimProcess.totalClaimAmount = data.RequestedAmount;
                            $scope.claimProcess.authorizedClaimAmount = data.AuthorizedAmount;
                            $scope.totalauthorizedAmount = data.AuthorizedAmount;
                            $scope.claimId = data.ClaimId;
                            //remove if updated
                            var index = 0;
                            if ($scope.updatePartMode) {
                                angular.forEach($scope.extendedClaimItems, function (displayItem) {

                                    if (displayItem.serverId === $scope.currentPart.serverId) {
                                        $scope.extendedClaimItems.splice(index, 1);
                                        $scope.reorderClaimItemids();
                                    }
                                    index++;
                                });
                            }

                            $scope.syncWithExtendedGridData($scope.labourCharge, $scope.currentPart);

                            customInfoMessage("Part successfully saved.");
                            $scope.clearPartDetails();
                            $scope.updatePartMode = false;
                        } else {
                            customErrorMessage(data.Status);
                        }
                    }).error(function (data, status, headers, config) {

                    }).finally(function () {
                        swal.close();
                    });
                }
            //}
        }
        $scope.reorderClaimItemids = function () {
            for (var i = 0; i < $scope.extendedClaimItems.length; i++) {
                $scope.extendedClaimItems[i].id = parseInt(i + 1);
            }
        }
        $scope.clearPartDetails = function () {
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
                discountAmount: 0.00,
                fault: emptyGuid(),
                serverId: emptyGuid(),
                partDiscountScheme: 'NA',
                rejectionTypeId: emptyGuid(),
                nettAmount: 0.00,
                authorizedAmount: 0.00,
                itemStatus: '',
                partDiscountScheme: emptyGuid()
            };
            $scope.labourCharge = {
                chargeType: 'H',
                hourlyRate: 0.00,
                hours: 0,
                totalAmount: 0.00,
                description: '',
                partId: emptyGuid(),
                goodWillType: 'NA',
                goodWillValue: 0.00,
                goodWillAmount: 0.00,
                discountType: 'NA',
                discountValue: 0.00,
                discountAmount: 0.00,
                labourDiscountScheme: 'NA',
                nettAmount: 0.00,
                authorizedAmount: 0.00
            };
        }
        $scope.fillServerDataForExtendedGrid = function (claimItemList) {
            var originalClaimItemList = claimItemList;
            angular.forEach(claimItemList, function (claimItem) {
                var currentLabourCharge, currentPart, isLabourChargeAvailable = false;
                //parts
                if (claimItem.itemType === "P") {
                    currentPart = {
                        id: claimItem.id,
                        partAreaId: claimItem.partAreaId,
                        partId: claimItem.partId,
                        partNumber: claimItem.itemNumber,
                        partName: claimItem.itemName,
                        partQty: claimItem.qty,
                        unitPrice: claimItem.authorizedAmount,
                        remark: claimItem.remarks,
                        isRelatedPartsAvailable: false,
                        allocatedHours: 0,
                        goodWillType: claimItem.isGoodWillPercentage ? "P" : "F",
                        goodWillValue: claimItem.goodWillRate,
                        goodWillAmount: claimItem.goodWillAmount,
                        discountType: claimItem.isDiscountPercentage ? "P" : "F",
                        discountValue: claimItem.discountRate,
                        discountAmount: claimItem.discountAmount,
                        fault: claimItem.faultId,
                        serverId: claimItem.serverId,
                        partDiscountScheme: claimItem.discountSchemeCode === '' ? 'NA' : claimItem.discountSchemeCode,
                        rejectionTypeId: claimItem.rejectionTypeId,
                        nettAmount: claimItem.totalPrice,
                        authorizedAmount: claimItem.authorizedAmount,
                        itemStatus: claimItem.statusCode,
                        causeOfFailureId: claimItem.causeOfFailureId
                    };

                    for (var i = 0; i < originalClaimItemList.length; i++) {
                        //looking for related labour charges
                        if (originalClaimItemList[i].parentId === claimItem.serverId && originalClaimItemList[i].itemType === "L") {
                            //found related labour charge
                            isLabourChargeAvailable = true;
                            currentLabourCharge = {
                                chargeType: originalClaimItemList[i].itemNumber === "Fixed" ? "F" : "H",
                                hourlyRate: originalClaimItemList[i].unitPrice,
                                hours: originalClaimItemList[i].qty,
                                totalAmount: originalClaimItemList[i].totalPrice,
                                description: originalClaimItemList[i].remarks,
                                partId: emptyGuid(),
                                goodWillType: originalClaimItemList[i].isGoodWillPercentage ? "P" : "F",
                                goodWillValue: originalClaimItemList[i].goodWillRate,
                                goodWillAmount: originalClaimItemList[i].goodWillAmount,
                                discountType: originalClaimItemList[i].isDiscountPercentage ? "P" : "F",
                                discountValue: originalClaimItemList[i].discountRate,
                                discountAmount: originalClaimItemList[i].discountAmount,
                                labourDiscountScheme: claimItem.discountSchemeCode === '' ? 'NA' : claimItem.discountSchemeCode,
                                nettAmount: originalClaimItemList[i].totalPrice,
                                authorizedAmount: originalClaimItemList[i].authorizedAmount
                            }

                        }
                    }
                    if (!isLabourChargeAvailable) {
                        currentLabourCharge = {
                            chargeType: 'H',
                            hourlyRate: 0.00,
                            hours: 0,
                            totalAmount: 0.00,
                            description: '',
                            partId: emptyGuid(),
                            goodWillType: 'NA',
                            goodWillValue: 0.00,
                            goodWillAmount: 0.00,
                            discountType: 'NA',
                            discountValue: 0.00,
                            discountAmount: 0.00,
                            labourDiscountScheme: 'NA',
                            nettAmount: 0.00,
                            authorizedAmount: 0.00
                        }
                    }
                    $scope.syncWithExtendedGridData(currentLabourCharge, currentPart);
                } else if (claimItem.itemType === "S") {
                    currentPart = {
                        id: claimItem.id,
                        partAreaId: claimItem.partAreaId,
                        partId: claimItem.partId,
                        partNumber: claimItem.itemNumber,
                        partName: claimItem.itemName,
                        partQty: claimItem.qty,
                        unitPrice: claimItem.unitPrice,
                        remark: claimItem.comment,
                        isRelatedPartsAvailable: false,
                        allocatedHours: 0,
                        goodWillType: claimItem.isGoodWillPercentage ? "P" : "F",
                        goodWillValue: claimItem.goodWillRate,
                        goodWillAmount: claimItem.goodWillAmount,
                        discountType: claimItem.isDiscountPercentage ? "P" : "F",
                        discountValue: claimItem.discountRate,
                        discountAmount: claimItem.discountAmount,
                        fault: claimItem.faultId,
                        serverId: claimItem.serverId,
                        partDiscountScheme: claimItem.discountSchemeCode === '' ? 'NA' : claimItem.discountSchemeCode,
                        rejectionTypeId: claimItem.rejectionTypeId,
                        nettAmount: claimItem.totalPrice,
                        authorizedAmount: claimItem.authorizedAmount,
                        itemStatus: claimItem.statusCode
                    };
                    currentLabourCharge = {
                        chargeType: 'H',
                        hourlyRate: 0.00,
                        hours: 0,
                        totalAmount: 0.00,
                        description: '',
                        partId: emptyGuid(),
                        goodWillType: 'NA',
                        goodWillValue: 0.00,
                        goodWillAmount: 0.00,
                        discountType: 'NA',
                        discountValue: 0.00,
                        discountAmount: 0.00,
                        labourDiscountScheme: 'NA',
                        nettAmount: 0.00,
                        authorizedAmount: 0.00
                    };
                    $scope.syncWithExtendedGridData(currentLabourCharge, currentPart);
                }
            });
        }
        $scope.syncWithExtendedGridData = function (labourCharge, part) {
            var extendedClaimItemRow = {
                id: $scope.extendedClaimItems.length + 1,
                partAreaId: part.partAreaId,
                partId: part.partId,
                partNumber: part.partNumber,
                partName: part.partName,
                partQty: part.partQty,
                unitPrice: part.unitPrice.toFixed(2),
                remark: part.remark,
                goodWillType: part.goodWillType,
                goodWillValue: part.goodWillValue.toFixed(2),
                goodWillAmount: part.goodWillAmount.toFixed(2),
                discountType: part.discountType,
                discountValue: part.discountValue.toFixed(2),
                discountAmount: part.discountAmount.toFixed(2),
                fault: $scope.getFaultNameById(part.fault),
                faultId: part.fault,
                partDiscountScheme: part.partDiscountScheme,
                partDiscountSchemeName: $scope.getDiscountSchemeNameById(part.partDiscountScheme),
                rejectionType: $scope.getPartRejectionTypeNameById(part.partRejectionTypeId),
                rejectionTypeId: part.partRejectionTypeId,
                nettAmount: part.nettAmount.toFixed(2),
                authorizedAmount: part.authorizedAmount.toFixed(2),
                itemStatus: part.itemStatus,
                itemStatusName: $scope.getItemStatusByCode(part.itemStatus),
                serverId: part.serverId,
                causeOfFailureId: part.causeOfFailureId,

                l_chargeType: labourCharge.chargeType,
                l_chargeTypeName: $scope.getChargeTypeNameByCode(labourCharge.chargeType),
                l_hourlyRate: labourCharge.hourlyRate.toFixed(2),
                l_hours: labourCharge.hours.toFixed(2),
                l_totalAmount: labourCharge.totalAmount.toFixed(2),
                l_description: labourCharge.description,
                l_goodWillType: labourCharge.goodWillType,
                l_goodWillValue: labourCharge.goodWillValue.toFixed(2),
                l_goodWillAmount: labourCharge.goodWillAmount.toFixed(2),
                l_discountType: labourCharge.discountType,
                l_discountValue: labourCharge.discountValue.toFixed(2),
                l_discountAmount: labourCharge.discountAmount.toFixed(2),
                l_labourDiscountScheme: labourCharge.labourDiscountScheme,
                l_labourDiscountSchemeName: $scope.getDiscountSchemeNameById(labourCharge.labourDiscountScheme),
                l_nettAmount: labourCharge.nettAmount.toFixed(2),
                l_authorizedAmount: labourCharge.authorizedAmount.toFixed(2)
            };
            $scope.reorderClaimItemids();
            $scope.extendedClaimItems.push(extendedClaimItemRow);
        }
        $scope.getChargeTypeNameByCode = function (chargeTypeCode) {
            if (chargeTypeCode !== '') {
                if (chargeTypeCode === "F") {
                    return "Fixed";
                } else if (chargeTypeCode === "H") {
                    return "Hourly";
                } else {
                    return "";
                }
            } else {
                return "";
            }
        }
        $scope.getPartRejectionTypeNameById = function (rejectionTypeId) {
            if (isGuid(rejectionTypeId)) {
                angular.forEach($scope.partRejectionTypes, function (rejectionType) {
                    if (rejectionType.Id === rejectionTypeId) {
                        return rejectionType.Description;
                    }
                });
            }
        }
        $scope.getFaultNameById = function (faultId) {
            if (isGuid(faultId)) {
                angular.forEach($scope.faults, function (fault) {
                    if (fault.Id === faultId) {
                        return fault.FaultCode + '->' + fault.FaultName;
                    }
                });
            } else {
                return "";
            }
        }
        $scope.getDiscountSchemeNameById = function (descountSchemeId) {
            if (isGuid(descountSchemeId)) {
                angular.forEach($scope.discountSchemes, function (discountScheme) {
                    if (discountScheme.Id === descountSchemeId) {
                        return discountScheme.SchemeName;
                    }
                });
            }
        }
        $scope.getDiscountSchemeNameById = function (partRejectionTypeId) {
            if (isGuid(partRejectionTypeId)) {
                angular.forEach($scope.partRejectionTypes, function (rejectionType) {
                    if (rejectionType.Id === partRejectionTypeId) {
                        return rejectionType.Description;
                    }
                });
            }
        }
        $scope.getItemStatusByCode = function (itemStatusCode) {
            if (itemStatusCode !== '') {
                if (itemStatusCode === "A") {
                    return "Approve";
                } else if (itemStatusCode === "R") {
                    return "Reject";
                } else {
                    return "";
                }
            } else {
                return "";
            }
        }
        $scope.showAddNewClaimItem = function () {
            if (isGuid($scope.claimId) || isGuid($scope.policyId)) {
                ClaimItemEntryPopup = ngDialog.open({
                    template: 'popUpAddClaimItem',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope,
                    preCloseCallback: function () {
                        if (isGuid($scope.claimId)) {
                            if ($location.path() === '/app/claim/process/') {
                                $location.path('app/claim/process/' + $scope.claimId);
                            }
                        }
                    }
                });
            } else {
                customErrorMessage("Please select a claim or policy before add a claim item.");
            }
        }
        $scope.showAddClaimItem = function (itemId) {
            console.log(itemId);
            
            if (parseInt(itemId)) {
                angular.forEach($scope.extendedClaimItems, function (claimItem) {
                    if (claimItem.id === itemId) {
                        console.log(claimItem);
                        if (claimItem.partName !== "Sundry") {
                            $scope.resetSundryDetails();
                            $scope.updateSundry = false;
                            $scope.defaultPartId = claimItem.partId;
                            $scope.updatePartMode = true;
                            $scope.currentPart = {
                                id: claimItem.id,
                                partAreaId: claimItem.partAreaId,
                                partId: claimItem.partId,
                                partNumber: claimItem.partNumber,
                                partName: claimItem.partName,
                                partQty: claimItem.partQty,
                                unitPrice: parseFloat(claimItem.unitPrice.replace(/\,/g, '')),
                                remark: claimItem.remark,
                                isRelatedPartsAvailable: false,
                                allocatedHours: 0,
                                goodWillType: parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')) ? claimItem.goodWillType : 'NA',
                                goodWillValue: claimItem.goodWillType == 'F' ? parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')) : parseFloat(claimItem.goodWillValue.replace(/\,/g, '')),
                                goodWillAmount: parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')),
                                discountType: parseFloat(claimItem.discountAmount.replace(/\,/g, '')) ? claimItem.discountType : 'NA',
                                discountValue: claimItem.discountType == 'F' ? parseFloat(claimItem.discountAmount.replace(/\,/g, '')) : parseFloat(claimItem.discountValue.replace(/\,/g, '')),
                                discountAmount: parseFloat(claimItem.discountAmount.replace(/\,/g, '')),
                                fault: claimItem.fault == "" ? emptyGuid() : claimItem.faultId,
                                serverId: claimItem.serverId,
                                partDiscountScheme: 'NA',
                                rejectionTypeId: claimItem.rejectionTypeId,
                                nettAmount: parseFloat(claimItem.nettAmount.replace(/\,/g, '')),
                                authorizedAmount: parseFloat(claimItem.unitPrice.replace(/\,/g, '')),
                                itemStatus: claimItem.itemStatus == null ? "" : claimItem.itemStatus,
                                causeOfFailureId: claimItem.causeOfFailureId == "" ? "" : claimItem.causeOfFailureId,

                            };
                            $scope.selectedPart = {
                                Id: claimItem.partId,
                                PartNumber: claimItem.partNumber,
                                PartName: claimItem.partName,
                            };
                            $scope.selectedPartChanged($scope.selectedPart);
                            $scope.selectedPartAreaChanged();
                            $scope.selectedFaultChanged();

                            $scope.labourCharge = {
                                chargeType: claimItem.l_chargeType,
                                hourlyRate: parseFloat(claimItem.l_hourlyRate.replace(/\,/g, '')),
                                hours: parseFloat(claimItem.l_hours),
                                totalAmount: parseFloat(claimItem.l_totalAmount.replace(/\,/g, '')),
                                description: claimItem.l_description,
                                partId: claimItem.l_partId,
                                goodWillType: claimItem.l_goodWillType,
                                goodWillValue: parseFloat(claimItem.l_goodWillValue.replace(/\,/g, '')),
                                goodWillAmount: parseFloat(claimItem.l_goodWillAmount.replace(/\,/g, '')),
                                discountType: claimItem.l_discountType,
                                discountValue: parseFloat(claimItem.l_discountValue.replace(/\,/g, '')),
                                discountAmount: parseFloat(claimItem.l_discountAmount.replace(/\,/g, '')),
                                labourDiscountScheme: claimItem.l_labourDiscountScheme,
                                nettAmount: parseFloat(claimItem.l_nettAmount.replace(/\,/g, '')),
                                authorizedAmount: parseFloat(claimItem.l_authorizedAmount.replace(/\,/g, ''))
                            };
                        } else {
                            $scope.resetPartDetails();
                            $scope.updateSundry = true;
                            $scope.updatePartMode = false;
                            $scope.sundry.value = parseFloat(claimItem.nettAmount.replace(/\,/g, ''));
                            $scope.sundry.name = claimItem.partNumber;
                            $scope.sundry.serverId = claimItem.serverId;
                        }
                    }
                });
            }

            if (isGuid($scope.claimId) || isGuid($scope.policyId)) {
                ClaimItemEntryPopup = ngDialog.open({
                    template: 'popUpAddClaimItem',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope,
                    preCloseCallback: function () {
                        if (isGuid($scope.claimId)) {
                            if ($location.path() === '/app/claim/process/') {
                                $location.path('app/claim/process/' + $scope.claimId);
                            }
                        }
                    }
                });
            } else {
                customErrorMessage("Please select a claim or policy before add a claim item.");
            }
        }

        $scope.faultSearchPopup = function () {
            if (isGuid($scope.claimId) || isGuid($scope.policyId)) {
                ClaimItemEntryPopup.close();

                $scope.faultTable = new ngTableParams({
                    page: 1,
                    count: 7,

                }, {
                        counts: [],
                        getData: function ($defer, params) {

                            var page = params.page();
                            var size = params.count();
                            var searchData = {
                                claimId: $scope.claimId,
                                policyId: $scope.policyId,
                                faultCategoryCode: $scope.faultSearch.categoryCode,
                                faultAreaCode: $scope.faultSearch.areaCode,
                                faultCode: $scope.faultSearch.code,
                                faultDescription: $scope.faultSearch.description
                            };
                            var data = {
                                page: page,
                                pageSize: size,
                                searchDetails: searchData
                            }

                            $scope.faultsGridLoading = true;

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/FaultManagement/SearchFaultsByCriterias',
                                data: data,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                var responseArr = JSON.parse(data);
                                if (responseArr != null) {
                                    if (responseArr.data.length === 0) {
                                        customErrorMessage("No faults found.");

                                    }
                                    params.total(responseArr.totalRecords);
                                    $defer.resolve(responseArr.data);

                                } else {

                                    customErrorMessage("No faults found.");
                                }
                            }).error(function (data, status, headers, config) {
                            }).finally(function () {
                                $scope.faultsGridLoading = false;

                            });
                        }
                    });
                FaultSearchPopup = ngDialog.open({
                    template: 'popUpSearchFault',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope,
                    preCloseCallback: function () {
                        $scope.showAddNewClaimItem();
                    }
                });
            } else {
                customErrorMessage("Please select a claim or policy before add a claim item.");
            }
        }
        $scope.refreshFaultSearchGridData = function () {
            $scope.faultTable.reload();
        }
        $scope.selectFaultFromSearch = function (faultId) {
            FaultSearchPopup.close();
            $scope.currentPart.fault = faultId;
            $scope.selectedFaultChanged();

        }
        $scope.clearRefreshFaultSearchGridData = function () {

            $scope.faultSearch.categoryCode = '';
            $scope.faultSearch.areaCode = '';
            $scope.faultSearch.code = '';
            $scope.faultSearch.description = '';

            $scope.faultTable.reload();
        }
        $scope.showPolicySearchPopup = function () {
            InitialPopup.close();
            getPolicySearchPage();
            PolicySelectionPopup = ngDialog.open({
                template: 'popUpPolicySelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }
        $scope.showProcessClaimItem = function (claimId) {
            ProcessClaimItemPopup = ngDialog.open({
                template: 'popUpProcessClaimItem',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
            $scope.currentClaimItem = {
                fault: emptyGuid()
            };
            $scope.currentProcessingClaimItemId = claimId;
            angular.forEach($scope.claimProcess.claimItemList, function (claimItem) {
                if (claimItem.id === claimId) {
                    $scope.currentClaimItem.itemName = claimItem.itemName;
                    $scope.currentClaimItem.itemNumber = claimItem.itemNumber;
                    $scope.currentClaimItem.claimItem = claimItem.itemType;
                    $scope.currentClaimItem.qty = claimItem.qty;
                    $scope.currentClaimItem.totalPrice = claimItem.totalPrice;
                    $scope.currentClaimItem.remarks = claimItem.remarks;
                    $scope.currentClaimItem.discountAmount = claimItem.discountAmount;
                    $scope.currentClaimItem.goodWillAmount = claimItem.goodWillAmount;
                    $scope.currentClaimItem.unitPrice = claimItem.unitPrice;
                    $scope.currentClaimItem.discountType = (claimItem.isDiscountPercentage) ? "P" : "F";
                    $scope.currentClaimItem.discountRate = claimItem.discountRate;
                    $scope.currentClaimItem.fault = claimItem.faultId;
                    $scope.currentClaimItem.status = (claimItem.statusCode) ? "A" : "R";
                    $scope.currentClaimItem.authorizedAmount = claimItem.authorizedAmount;
                    $scope.currentClaimItem.goodWillType = (claimItem.isGoodWillPercentage) ? "P" : "F";
                    $scope.currentClaimItem.goodwillRate = claimItem.goodWillRate;
                    $scope.currentClaimItem.comment = claimItem.comment;
                }
            });

        }
        $scope.showPolicyDetails = function () {

            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });
            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetPolicyDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //PolicyDetailsPopup = ngDialog.open({
                //    template: 'popUpPolicyDetails',
                //    className: 'ngdialog-theme-plain',
                //    closeByEscape: true,
                //    showClose: true,
                //    closeByDocument: false,
                //    scope: $scope
                //});
                $scope.policyDetails.sumOfAuthorizedClaimedAmount = data.sumOfAuthorizedClaimedAmount;
                $scope.policyDetails.policyNo = data.policyNo;
                $scope.policyDetails.warrentyType = data.warrentyType;
                $scope.policyDetails.customerName = data.customerName;
                $scope.policyDetails.mobileNumber = data.mobileNumber;
                $scope.policyDetails.insurer = data.insurer;
                $scope.policyDetails.reInsurer = data.reInsurer;
                $scope.policyDetails.make = data.make;
                $scope.policyDetails.model = data.model;
                $scope.policyDetails.modelYear = data.modelYear;
                $scope.policyDetails.cyllinderCount = data.cyllinderCount;
                $scope.policyDetails.engineCapacity = data.engineCapacity;
                $scope.policyDetails.salePrice = data.salePrice.split(" ");
                $scope.policyDetails.salePrice = $scope.policyDetails.salePrice[0];
                $scope.policyDetails.status = data.status;
                $scope.policyDetails.uwYear = data.uwYear;
                $scope.policyDetails.vin = data.vin;

                $scope.policyDetails.manfWarrentyStartDate = data.manfWarrentyStartDate;
                $scope.policyDetails.manfWarrentyEndDate = data.manfWarrentyEndDate;
                $scope.policyDetails.extensionStartDate = data.extensionStartDate;
                $scope.policyDetails.extensionEndDate = data.extensionEndDate;

                $scope.policyDetails.manfWarrentyMonths = data.manfWarrentyMonths;
                $scope.policyDetails.extensionPeriod = data.extensionPeriod;
                $scope.policyDetails.extensionMilage = data.extensionMilage;
                $scope.policyDetails.manfWarrentyMilage = data.manfWarrentyMilage;
                $scope.policyDetails.cutoff = data.cutoff;


                //cutoff
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }
        $scope.showCoverDetails = function () {
            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });

            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetPolicyDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                //CoverDetailsPopup = ngDialog.open({
                //    template: 'popUpCoverDetails',
                //    className: 'ngdialog-theme-plain',
                //    closeByEscape: true,
                //    showClose: true,
                //    closeByDocument: true,
                //    scope: $scope
                //});
                $scope.policyDetails.sumOfAuthorizedClaimedAmount = data.sumOfAuthorizedClaimedAmount;
                $scope.policyDetails.policyNo = data.policyNo;
                $scope.policyDetails.warrentyType = data.warrentyType;
                $scope.policyDetails.customerName = data.customerName;
                $scope.policyDetails.mobileNumber = data.mobileNumber;
                $scope.policyDetails.insurer = data.insurer;
                $scope.policyDetails.reInsurer = data.reInsurer;
                $scope.policyDetails.make = data.make;
                $scope.policyDetails.model = data.model;
                $scope.policyDetails.modelYear = data.modelYear;
                $scope.policyDetails.cyllinderCount = data.cyllinderCount;
                $scope.policyDetails.engineCapacity = data.engineCapacity;
                $scope.policyDetails.salePrice = data.salePrice;
                $scope.policyDetails.status = data.status;
                $scope.policyDetails.uwYear = data.uwYear;
                $scope.policyDetails.vin = data.vin;
                $scope.policyDetails.manfWarrentyStartDate = data.s_manfWarrentyStartDate;
                $scope.policyDetails.manfWarrentyEndDate = data.s_manfWarrentyEndDate;
                $scope.policyDetails.extensionStartDate = data.s_extensionStartDate;
                $scope.policyDetails.extensionEndDate = data.s_extensionEndDate;
                $scope.policyDetails.manfWarrentyMonths = data.manfWarrentyMonths;
                $scope.policyDetails.extensionPeriod = data.extensionPeriod;
                $scope.policyDetails.extensionMilage = data.extensionMilage;
                $scope.policyDetails.manfWarrentyMilage = data.manfWarrentyMilage;


            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }
        $scope.showAssesmentHistory = function () {
            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });
            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetClaimDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                PopupAssesmentHistory = ngDialog.open({
                    template: 'popUpAssesmentHistory',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.policyDetails.policyNo = data.policyNo;
                //  $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }
        $scope.showPolicyInformation = function () {
            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });
            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetPolicyDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                PopupAssesmentHistory = ngDialog.open({
                    template: 'popUpPolicyDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.assesmentHistory = data.AssessmentData;
                //  $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }
        $scope.showItemInformation = function () {
            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });
            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetItemDetForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                PopupAssesmentHistory = ngDialog.open({
                    template: 'popUpItemDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.assesmentHistory = data.AssessmentData;
                //  $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

        $scope.showClaimHistory = function () {



            swal({ title: 'Loading...', text: 'Reading information', showConfirmButton: false });
            var data = {
                claimId: $scope.claimId === "" ? emptyGuid() : (typeof $scope.claimId === "undefined" ? emptyGuid() : $scope.claimId),
                policyId: $scope.policyId === "" ? emptyGuid() : (typeof $scope.policyId === "undefined" ? emptyGuid() : $scope.policyId)
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetClaimDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                ClaimHistoryPopup = ngDialog.open({
                    template: 'popUpClaimHistory',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                //$scope.assesmentHistory = data.AssessmentData;
                $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }
        $scope.showInitialSelection = function () {

            InitialPopup = ngDialog.open({
                template: 'popUpInitialSelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });

        }
        $scope.showClaimSearchPopup = function () {
            InitialPopup.close();
            $scope.initiateClaimSearchGrid();
            ClaimSelectionPopup = ngDialog.open({
                template: 'popUpClaimSelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }
        $scope.initiateClaimSearchGrid = function () {
            $scope.UserValidation();
            $scope.claimTable = new ngTableParams({
                page: 1,
                count: 10,
            }, {
                    getData: function ($defer, params) {

                        var page = params.page();
                        var size = params.count();
                        var data = {
                            page: page,
                            pageSize: size,
                            loggedInUserId: $localStorage.LoggedInUserId,
                        }

                        $scope.policyGridloading = true;
                        $scope.policyGridloadAttempted = false;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/GetAllClaimsToProcessByUserId',
                            data: { 'data': data },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var responseArr = JSON.parse(data);
                            if (responseArr != null) {
                                if (responseArr.data.length === 0)
                                    customErrorMessage("No claim requests found.");
                                params.total(responseArr.totalRecords);
                                $defer.resolve(responseArr.data);
                            } else {
                                customErrorMessage("No claim requests found.");
                            }
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            $scope.policyGridloading = false;
                            $scope.policyGridloadAttempted = true;
                        });
                    }
                });

        }

        $scope.UserValidation = function (code) {
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
                swal({ title: 'TAS Security Information', text: data.status, showConfirmButton: false });
                setTimeout(function () { swal.close(); }, 8000);
                $state.go('app.dashboard');
            }
        }).error(function (data, status, headers, config) {
        }).finally(function () {
        });
    }

        $scope.GetStatusByCode = function (code) {
            if ($scope.userType == "DU") {

                switch (code) {
                    case "SUB":
                        return "Submitted";
                        break;
                    case "REQ":
                        return "Pending";
                        break;
                    case "HOL":
                        return "On Hold";
                        break;
                    case "REV":
                        return "On Review";
                        break;
                    case "REJ":
                        return "Rejected";
                        break;
                    case "APP":
                        return "Approved";
                        break;
                    case "INP":
                        return "In Progress";
                        break;
                    case "PEN":
                        return "Pending";
                        break;
                    case "PID":
                        return "Paid";
                        break;
                    case "PIP":
                        return "Payment Progress";
                        break;
                    case "PRC":
                        return "Processed";
                        break;
                    case "UPD":
                        return "Re submitted";
                        break;
                    case "RWP":
                        return "Rejected";
                        break;
                        defaut:
                        return "";
                }
            }
                else {

                    switch (code) {
                        case "SUB":
                            return "New";
                            break;
                        case "REQ":
                            return "Info Requested";
                            break;
                        case "HOL":
                            return "On Hold";
                            break;
                        case "REV":
                            return "On Review";
                            break;
                        case "REJ":
                            return "Rejected";
                            break;
                        case "APP":
                            return "Approved";
                            break;
                        case "PEN":
                            return "Pending";
                            break;
                        case "PID":
                            return "Paid";
                            break;
                        case "PIP":
                            return "Payment Progress";
                            break;
                        case "PRC":
                            return "Processed";
                            break;
                        case "UPD":
                            return "Updated";
                            break;
                        case "RWP":
                            return "Rejected";
                            break;
                            defaut:
                            return "";
                    }
            }
        };
        $scope.selectClaimToProcessByGrid = function (claimId, policyId) {
            if (typeof ClaimSelectionPopup !== 'undefined')
                ClaimSelectionPopup.close();
            $scope.policyId = policyId;
            $location.path('app/claim/process/' + claimId);
        }
        $scope.selectClaimToProcess = function (claimId, policyId) {
            $scope.docUploader.queue = [];
            if (isGuid(claimId)) {
                $scope.policyId = policyId;
                swal({ title: 'Loading...', text: 'Reading claim information', showConfirmButton: false });
                if (typeof ClaimSelectionPopup !== 'undefined')
                    ClaimSelectionPopup.close();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetClaimRequestByClaimId',
                    data: {
                        "claimId": claimId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.claimProcess.policyNumber = data.PolicyNumber;
                    $scope.claimProcess.claimNumber = data.ClaimNumber;
                    $scope.claimProcess.currencyCode = data.CurrencyCode;
                    $scope.claimProcess.totalLiability = data.totalLiability.toFixed(2);
                    $scope.claimProcess.totalClaimAmount = data.TotalClaimAmount;
                    $scope.claimProcess.authorizedClaimAmount = data.AuthorizedClaimAmount;
                    $scope.claimProcess.complaint.customer = data.Complaint.customer;
                    $scope.claimProcess.complaint.dealer = data.Complaint.dealer;
                    $scope.claimProcess.complaint.engineer = data.Complaint.engineer;
                    $scope.claimProcess.complaint.conclution = data.Complaint.conclution;
                    $scope.claimProcess.ClaimAmount = data.ClaimAmount;
                    $scope.claimProcess.claimDealerId = data.DealerId;
                    $scope.claimProcess.country = data.Country;
                    $scope.claimProcess.claimItemList = data.ClaimItemList;
                    if (data.ClaimNumber == '') {
                        $scope.claimProcess.claimNumber = data.Wip;
                        $scope.isrefnovalid = true;

                    }
                    $scope.claimProcess.claimDate = data.ClaimDate;
                    $scope.claimProcess.claimMileage = parseFloat(data.ClaimMileage);
                    $scope.claimProcess.claimStatus = (data.ClaimStatus === null || data.ClaimStatus === 'REV') ? 'NA' : data.ClaimStatus;
                    $scope.claimProcess.isGoodwillClaim = data.IsGoodwillClaim;
                    $scope.countryTax = data.CountryTaxes;
                    AddTaxes();
                    $scope.totalauthorizedAmount = data.AuthorizedClaimAmount;

                    //setup data for extended grid
                    $scope.fillServerDataForExtendedGrid(data.ClaimItemList);

                    $scope.makeId = data.MakeId;
                    $scope.modelId = data.ModelId;
                    $scope.dealerId = data.DealerId;
                    $scope.policyDealerId = data.PolicyDealerId;
                    $scope.policyCountryId = data.PolicyCountryId;



                    $scope.getAllPartAreas(data.CommodityCategoryId);

                    $scope.loadPolicy($scope.policyId, true);

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/GetClaimAndPolicyAttachmentsByPolicyId',
                        data: { "Id": $scope.policyId },
                        headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {



                        $scope.attachments_temp = data.Attachments;

                        for (var i = 0; i < data.Attachments.length; i++) {
                            var file = {
                                name: data.Attachments[i].FileName,
                                size: data.Attachments[i].AttachmentSizeKB
                            }
                            var attachment = {
                                documentType: data.Attachments[i].DocumentType,
                                id: data.Attachments[i].Id,
                                file: file,
                                ref: data.Attachments[i].FileServerRef,
                                attachmentSection: data.Attachments[i].AttachmentSection,
                                policyNumber: $scope.claimProcess.policyNumber,
                                claimNumber: $scope.claimProcess.claimNumber,
                                dateOfAttachment: data.Attachments[i].DateOfAttachment
                            }

                            //if (data.Attachments[i].AttachmentSection === "Customer") {
                            //    $scope.customerDocUploader.queue.push(attachment)
                            //} else if (data.Attachments[i].AttachmentSection === "Item") {
                            $scope.docUploader.queue.push(attachment)
                            //} else if (data.Attachments[i].AttachmentSection === "Policy") {
                            //    $scope.policyDocUploader.queue.push(attachment)
                            //} else if (data.Attachments[i].AttachmentSection === "Payment") {
                            //    $scope.paymentDocUploader.queue.push(attachment)
                            //} else if (data.Attachments[i].AttachmentSection === "Other") {
                            //    $scope.customerDocUploader.queue.push(attachment)
                            //}
                        }

                    });

                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });

                $scope.showPolicyDetails();
            }
        }


        function AddTaxes() {
            var index = 0;
            $scope.TaxesList = [];
            angular.forEach($scope.countryTax, function (value) {
                var x = { id: index, code: value.TaxTypesId, label: value.TaxName, amount: value.TaxValue, isPercentage: value.IsPercentage};
                $scope.TaxesList.push(x);
                index = index + 1;
            });
        }


        $scope.SendTaxes = function (id) {
            var selectId = id;
            if ($scope.validateCountryTaxes()) {
                //$scope.SelectedTaxesDList = [];
                //$scope.claimProcess.countryTaxes = [];
                if ($scope.SelectedTaxesList.length != 0) {
                    //angular.forEach($scope.SelectedTaxesList, function (valueOut) {
                        angular.forEach($scope.TaxesList, function (valueIn) {
                            if (selectId.id == valueIn.id) {
                                $scope.claimProcess.countryTaxes.push(valueIn.code);
                                $scope.SelectedTaxesDList.push(valueIn.label);

                                if (valueIn.isPercentage == true) {
                                    $scope.claimProcess.authorizedClaimAmount = (($scope.totalauthorizedAmount * valueIn.amount) / 100) + $scope.claimProcess.authorizedClaimAmount;
                                } else {
                                    $scope.claimProcess.authorizedClaimAmount = $scope.claimProcess.authorizedClaimAmount + valueIn.amount;
                                }
                                
                            }
                        });
                    //});
                }
                
            }
            
        }

        function LoadTaxes() {
            $scope.SelectedTaxesList = [];
            $scope.SelectedTaxesDList = [];
            angular.forEach($scope.claimProcess.countryTaxes, function (valueOut) {
                angular.forEach($scope.TaxesList, function (valueIn) {
                    if (valueOut === valueIn.code) {
                        var x = { id: valueIn.id };
                        $scope.SelectedTaxesList.push(x);
                        $scope.SelectedTaxesDList.push(valueIn.label);
                    }
                });
            });
        }
        

        $scope.SendTaxess = function (id) {
            var deselectId = id;
            if ($scope.validateCountryTaxes()) {
                $scope.SelectedTaxesDList = [];
                $scope.claimProcess.countryTaxes = [];
                if ($scope.SelectedTaxesList.length != 0) {
                    angular.forEach($scope.TaxesList, function (valueIn) {
                        if (deselectId.id == valueIn.id) {
                            $scope.claimProcess.countryTaxes.push(valueIn.code);
                            $scope.SelectedTaxesDList.push(valueIn.label);

                            if (valueIn.isPercentage == true) {
                                $scope.taxauthorizedClaimAmount = ($scope.totalauthorizedAmount * valueIn.amount) / 100;
                                $scope.claimProcess.authorizedClaimAmount = $scope.claimProcess.authorizedClaimAmount - $scope.taxauthorizedClaimAmount;
                            } else {
                                $scope.claimProcess.authorizedClaimAmount = $scope.claimProcess.authorizedClaimAmount - valueIn.amount;
                            }

                            //$scope.claimProcess.authorizedClaimAmount = $scope.claimProcess.authorizedClaimAmount - valueIn.amount;
                        }
                    });

                } else {
                    $scope.claimProcess.authorizedClaimAmount = $scope.totalauthorizedAmount;
                }

            }

        }

        $scope.validateCountryTaxes = function () {
            var isValid = true;
            var isProcessed = false;
            if (isValid) {
                angular.forEach($scope.extendedClaimItems, function (claimItem) {
                    if (parseFloat(claimItem.authorizedAmount) && parseFloat(claimItem.authorizedAmount) > 0)
                        isProcessed = true;
                });
                if (!isProcessed) {
                    customErrorMessage("Please review claim items before select taxes.");
                    isValid = false;
                }
            }
            return isValid;
        };

        $scope.getAllPartAreas = function (commodityCategoryId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllPartAreasByCommodityCategoryId',
                data: { "commodityCategoryId": commodityCategoryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null)
                    customErrorMessage("No part areas found for selected policy's category.");
                $scope.partAreas = data;
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });
        }
        $scope.discountRateChange = function (currentClaimItem) {
            var discount = 0.00;
            var grossPrice = parseFloat(currentClaimItem.qty) * parseFloat(currentClaimItem.unitPrice);
            if (currentClaimItem.discountType == "F") {
                if (grossPrice < parseFloat(currentClaimItem.discountRate)) {
                    currentClaimItem.discountRate = grossPrice;
                }
                discount = currentClaimItem.discountRate;

            } else if (currentClaimItem.discountType == "P") {
                if (parseFloat(currentClaimItem.discountRate) > 100) {
                    currentClaimItem.discountRate = 100;
                }
                discount = (currentClaimItem.discountRate * grossPrice) / 100;
            }
            currentClaimItem.discountAmount = discount;
            $scope.adjustClaimItemValues(currentClaimItem);
        }
        $scope.goodWillRateChange = function (currentClaimItem) {
            var discount = 0.00;
            var grossPrice = parseFloat(currentClaimItem.qty) * parseFloat(currentClaimItem.unitPrice);
            if (currentClaimItem.goodWillType == "F") {
                if (grossPrice < parseFloat(currentClaimItem.goodwillRate)) {
                    currentClaimItem.goodwillRate = grossPrice;
                }
                discount = currentClaimItem.goodwillRate;

            } else if (currentClaimItem.goodWillType == "P") {
                if (parseFloat(currentClaimItem.goodwillRate) > 100) {
                    currentClaimItem.goodwillRate = 100;
                }
                discount = (currentClaimItem.goodwillRate * grossPrice) / 100;
            }
            currentClaimItem.goodWillAmount = discount;
            $scope.adjustClaimItemValues(currentClaimItem);
        }
        $scope.adjustClaimItemValues = function (currentClaimItem) {
            var discount = 0.00;
            if (parseFloat(currentClaimItem.discountAmount)) {
                discount = parseFloat(currentClaimItem.discountAmount);
            }
            var goodWill = 0.00;
            if (parseFloat(currentClaimItem.goodWillAmount)) {
                goodWill = parseFloat(currentClaimItem.goodWillAmount);
            }

            var grossPrice = parseFloat(currentClaimItem.qty) * parseFloat(currentClaimItem.unitPrice);
            if ((discount + goodWill) > grossPrice) {
                customErrorMessage("Sum of discount and goodwill cannot exceed items gross value");
                return false;
            } else {
                currentClaimItem.totalPrice = parseFloat(grossPrice - (discount + goodWill)).toFixed(2);
                return true;
            }
        }
        $scope.saveClaimItem = function (currentClaimItem) {
            currentClaimItem.validate_status = "";
            currentClaimItem.validate_authorizedAmount = "";
            currentClaimItem.validate_fault = "";
            if (currentClaimItem.status === "A") {
                if (currentClaimItem.claimItem === "P") {
                    if (!isGuid(currentClaimItem.fault)) {
                        currentClaimItem.validate_fault = "has-error";
                        customErrorMessage("Please select the fault type of the part.");
                        return false;
                    }
                }
                if ($scope.adjustClaimItemValues(currentClaimItem)) {
                    if (parseFloat(currentClaimItem.authorizedAmount)) {
                        if (parseFloat(currentClaimItem.totalPrice) < parseFloat(currentClaimItem.authorizedAmount)) {
                            customErrorMessage("Authorized amount cannot exceed total claim item amount.");
                            currentClaimItem.validate_authorizedAmount = "has-error";
                            return false;
                        } else {
                            //setting up data for post
                            var itemForPost;
                            angular.forEach($scope.claimProcess.claimItemList, function (claimItem) {
                                if (claimItem.id === $scope.currentProcessingClaimItemId) {
                                    claimItem.qty = currentClaimItem.qty;
                                    claimItem.totalPrice = currentClaimItem.totalPrice;
                                    claimItem.discountAmount = currentClaimItem.discountAmount;
                                    claimItem.goodWillAmount = currentClaimItem.goodWillAmount;
                                    claimItem.unitPrice = currentClaimItem.unitPrice;
                                    claimItem.isDiscountPercentage = (currentClaimItem.discountType === "P") ? true : false;
                                    claimItem.discountRate = currentClaimItem.discountRate;
                                    claimItem.isGoodWillPercentage = (currentClaimItem.goodWillType === "P") ? true : false;
                                    claimItem.goodWillRate = currentClaimItem.goodwillRate;
                                    claimItem.status = currentClaimItem.status;
                                    claimItem.authorizedAmt = currentClaimItem.authorizedAmount;
                                    claimItem.comment = currentClaimItem.comment;
                                    claimItem.claimId = $scope.claimId;
                                    claimItem.faultId = currentClaimItem.fault;
                                    claimItem.policyId = $scope.policyId;
                                    claimItem.entryBy = $localStorage.LoggedInUserId;

                                    if (claimItem.partId == null) {
                                        claimItem.partId = emptyGuid();
                                    }
                                    itemForPost = claimItem;

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/claim/SaveClaimItem',
                                        data: itemForPost,
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        ProcessClaimItemPopup.close();
                                        if (data.Status === "ok") {
                                            if (data.IsReload) {
                                                if (data.ClaimId === $scope.claimId) {
                                                    $state.reload();
                                                } else {
                                                    $location.path('app/claim/process/' + data.ClaimId);
                                                }
                                            } else {

                                            }
                                        } else {
                                            customErrorMessage(data.Status);
                                        }
                                    }).error(function (data, status, headers, config) {
                                    }).finally(function () {

                                    });
                                }
                            });
                        }
                    } else {
                        customErrorMessage("Please enter authorized amount.");
                        currentClaimItem.validate_authorizedAmount = "has-error";
                        return false;
                    }
                }
            } else if (currentClaimItem.status === "R") {
                if ($scope.adjustClaimItemValues(currentClaimItem)) {
                    //ready
                    angular.forEach($scope.claimProcess.claimItemList, function (claimItem) {
                        if (claimItem.id === $scope.currentProcessingClaimItemId) {
                            claimItem.qty = currentClaimItem.qty;
                            claimItem.totalPrice = currentClaimItem.totalPrice;
                            claimItem.discountAmount = currentClaimItem.discountAmount;
                            claimItem.goodWillAmount = currentClaimItem.goodWillAmount;
                            claimItem.unitPrice = currentClaimItem.unitPrice;
                            claimItem.isDiscountPercentage = (currentClaimItem.discountType === "P") ? true : false;
                            claimItem.discountRate = currentClaimItem.discountRate;
                            claimItem.isGoodWillPercentage = (currentClaimItem.goodWillType === "P") ? true : false;
                            claimItem.goodWillRate = currentClaimItem.goodwillRate;
                            claimItem.status = currentClaimItem.status;
                            claimItem.authorizedAmt = 0.00;
                            claimItem.comment = currentClaimItem.comment;
                            claimItem.claimId = $scope.claimId;
                            claimItem.faultId = currentClaimItem.fault;
                            claimItem.policyId = $scope.policyId;
                            claimItem.entryBy = $localStorage.LoggedInUserId;
                            if (claimItem.partId == null) {
                                claimItem.partId = emptyGuid();
                            }
                            itemForPost = claimItem;

                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/claim/SaveClaimItem',
                                data: itemForPost,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                ProcessClaimItemPopup.close();
                                if (data.Status === "ok") {
                                    if (data.IsReload) {
                                        if (data.ClaimId === $scope.claimId) {
                                            $state.reload();
                                        } else {
                                            $location.path('app/claim/process/' + data.ClaimId);
                                        }
                                    } else {

                                    }
                                } else {
                                    customErrorMessage(data.Status);
                                }
                            }).error(function (data, status, headers, config) {
                            }).finally(function () {

                            });
                        }
                    });
                }
            } else {
                customErrorMessage("Please select item status");
                currentClaimItem.validate_status = "has-error";
            }
        }
        $scope.policySearchGridSearchCriterias = {
            commodityTypeId: emptyGuid(),
            policyNo: "",
            serialNo: "",
            mobileNo: "",
            policyStartDate: "",
            policyEndDate: "",
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
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Commodity Type', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: 'Policy No', field: 'PolicyNo', width: '30%', enableSorting: false, cellClass: 'columCss', },
                { name: 'Vin or Serial', field: 'SerialNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Mobile No', field: 'MobileNo', enableSorting: false, cellClass: 'columCss' },
                { name: 'Policy Sold Date', field: 'PolicySoldDate', enableSorting: false, cellClass: 'columCss' },

                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadPolicy(row.entity.Id,false)" class="btn btn-xs btn-warning">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
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
        $scope.refresSearchGridData = function () {
            getPolicySearchPage();
        }
        getPolicySearchPage = function () {
            $scope.policySearchGridloading = true;
            $scope.policySearchGridloadAttempted = false;
            var policySearchGridParam =
                {
                    'paginationOptionsPolicySearchGrid': paginationOptionsPolicySearchGrid,
                    'policySearchGridSearchCriterias': $scope.policySearchGridSearchCriterias,
                    'type': 'forclaimsub',
                    'userId': $localStorage.LoggedInUserId
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetPoliciesForSearchGrid',
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
        $scope.loadPolicy = function (policyId, isExistingPolicy) {
            // alert(policyId);
            $scope.docUploader.queue = [];
            if (isGuid(policyId)) {
                if (typeof PolicySelectionPopup != 'undefined')
                    PolicySelectionPopup.close();
                swal({ title: 'Processing...', text: 'Reading Policy Information', showConfirmButton: false });
                $scope.policyId = policyId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetPolicyDetailsForClaimProcess',
                    data: { data: policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Status === "ok") {
                        $scope.claimProcess.policyNumber = data.PolicyNo;
                        if (!isExistingPolicy) {
                            $scope.claimProcess.claimNumber = 'N/A';
                            $scope.claimProcess.currencyCode = '';
                            $scope.claimProcess.totalClaimAmount = '0.00';
                            $scope.claimProcess.authorizedClaimAmount = '0.00';
                        }

                        $scope.claimProcess.country = data.PolicyCountry;
                        $scope.claimProcess.policyDealer = data.PolicyDealer;
                        $scope.claimProcess.customerName = data.CustomerName;
                        $scope.claimProcess.vinSerial = data.Serial;
                        $scope.claimProcess.make = data.Make;
                        $scope.claimProcess.model = data.Model;
                        $scope.claimProcess.claimDealers = data.ClaimDealers;
                        $scope.claimProcess.claimDealerId = data.PolicyDealerId;
                        $scope.refreshSelectedClaimDealer();
                        $scope.makeId = data.MakeId;
                        $scope.modelId = data.ModelId;
                        $scope.getAllPartAreas(data.CommodityCategoryId);

                        $scope.policyDealerId = data.PolicyDealerId;
                        $scope.policyCountryId = data.PolicyCountryId;                        
                        $scope.getCommodityTypeByCommodityCategoryId(data.CommodityCategoryId);
                        $scope.getallmakes();
                        $scope.showPolicyDetails();

                    } else {
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            }
        }
        $scope.refreshSelectedClaimDealer = function () {
            angular.forEach($scope.claimProcess.claimDealers, function (claimDealer) {
                if (claimDealer.Id === $scope.claimProcess.claimDealerId) {
                    $scope.claimProcess.currencyCode = claimDealer.CurrencyCode;
                    $scope.dealerId = $scope.claimProcess.claimDealerId;
                    //$scope.claimProcess.claimDealer = claimDealer.Country;
                }
            });
        }
        $scope.saveEngineerComment = function () {
            if ($scope.validateEngineerComments()) {
                swal({ title: 'Processing...', text: 'Saving claim', showConfirmButton: false });
                //$scope.policyId = policyId;
                var data = {
                    policyId: $scope.policyId,
                    claimId: $scope.claimId,
                    dealerId: $scope.dealerId,
                    engineer: $scope.claimProcess.complaint.engineer,
                    conclution: $scope.claimProcess.complaint.conclution,
                    loggedInUserId: $localStorage.LoggedInUserId,
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/SaveClaimEngineerComment',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Status === "ok") {
                        customInfoMessage("Claim successfully updated.");
                    } else {
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            } else {
                customErrorMessage("Please enter all mandatory fields.");
            }
        }
        $scope.validateEngineerComments = function () {
            var isValid = true;
            //if ($scope.claimProcess.complaint.engineer == null || $scope.claimProcess.complaint.engineer.length === 0) {
            //    $scope.validate_engineerComment = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_engineerComment = "";
            //}
            //if ($scope.claimProcess.complaint.conclution == null || $scope.claimProcess.complaint.conclution.length === 0) {
            //    $scope.validate_conclution = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_conclution = "";
            //}

            //if (isValid) {
            //    if (typeof $scope.claimProcess.claimItemList === "undefined" || $scope.claimProcess.claimItemList.length === 0) {
            //        customErrorMessage("Please add claim items before adding the assesment comments");
            //        isValid = false;
            //    }

            //    var isProcessed = false;
            //    if (isValid) {
            //        if ($scope.Products[0].Productcode == "TYRE") {
            //            isProcessed = true;
            //        } else {
            //            angular.forEach($scope.extendedClaimItems, function (claimItem) {
            //                if (parseFloat(claimItem.authorizedAmount) && parseFloat(claimItem.authorizedAmount) > 0)
            //                    isProcessed = true;
            //            });
            //            if (!isProcessed) {
            //                customErrorMessage("Please review claim items before adding the assesment comments");
            //                isValid = false;
            //            }
            //        }
                    
            //    }
            //}
            return isValid;
        }
        $scope.addLabourCharge = function () {
            if ($scope.validateLabourPayments()) {
                if (isGuid($scope.claimId)) {
                    var data = {
                        claimId: $scope.claimId,
                        labourCharge: $scope.labourCharge,
                        loggedInUserId: $localStorage.LoggedInUserId,
                        dealerId: $scope.dealerId
                    }
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/SaveClaimWithLabourCharge',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data.Status === "ok") {
                            ClaimItemEntryPopup.close();
                            if (data.IsReload) {
                                if (data.ClaimId === $scope.claimId) {
                                    $state.reload();
                                } else {
                                    $location.path('app/claim/process/' + data.ClaimId);
                                }

                            } else {

                            }
                        } else {
                            customErrorMessage(data.Status);
                        }

                    }).error(function (data, status, headers, config) {
                    }).finally(function () {
                        swal.close();
                    });
                } else {
                    customErrorMessage("Invalid claim selection.");
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fileds.");
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

            if (!parseFloat($scope.labourCharge.hourlyRate) && parseInt($scope.labourCharge.hours) > 0) {
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

            //discount validation
            var totalAmt, discountAmt, goodWillAmt;
            if (isValid) {
                if ($scope.labourCharge.chargeType === "H") {
                    totalAmt = parseFloat($scope.labourCharge.hours) * parseFloat($scope.labourCharge.hourlyRate);

                } else {
                    totalAmt = parseFloat($scope.labourCharge.hourlyRate);
                }

                if ($scope.labourCharge.goodWillType === "P") {
                    goodWillAmt = (parseFloat($scope.labourCharge.goodWillValue) * totalAmt) / 100;
                } else if ($scope.labourCharge.goodWillType === "F") {
                    goodWillAmt = parseFloat($scope.labourCharge.goodWillValue);
                } else {
                    goodWillAmt = 0.00;
                }

                if ($scope.labourCharge.discountType === "P") {
                    discountAmt = (parseFloat($scope.labourCharge.discountValue) * totalAmt) / 100;
                } else if ($scope.labourCharge.discountType === "F") {
                    discountAmt = parseFloat($scope.labourCharge.discountValue);
                } else {
                    discountAmt = 0.00;
                }

                if ((goodWillAmt + discountAmt) > totalAmt) {
                    $scope.validate_discountLabourValue = "has-error";
                    $scope.validate_goodWillLabourValue = "has-error";
                    customErrorMessage("Sum of discounts and good will cannot exceed laboure charge cost");
                    isValid = false;
                } else {
                    $scope.validate_discountLabourValue = "";
                    $scope.validate_goodWillLabourValue = "";
                }
            }
            if (!isValid) {
                customErrorMessage("Please fill all the mandatory fields.");
            }
            return isValid;
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
                        $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
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
            if ($scope.labourCharge.chargeType === "H") {
                $scope.labourCharge.nettAmount = (parseFloat($scope.labourCharge.hourlyRate) * parseInt($scope.labourCharge.hours))
                    - parseFloat($scope.labourCharge.goodWillAmount) - parseFloat($scope.labourCharge.discountAmount);
                $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
            } else {
                $scope.labourCharge.nettAmount = (parseFloat($scope.labourCharge.hourlyRate))
                    - parseFloat($scope.labourCharge.goodWillAmount) - parseFloat($scope.labourCharge.discountAmount);
                $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
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
                        $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
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
            if ($scope.labourCharge.chargeType === "H") {
                $scope.labourCharge.nettAmount = (parseFloat($scope.labourCharge.hourlyRate) * parseInt($scope.labourCharge.hours))
                    - parseFloat($scope.labourCharge.goodWillAmount) - parseFloat($scope.labourCharge.discountAmount);
                $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
            } else {
                $scope.labourCharge.nettAmount = (parseFloat($scope.labourCharge.hourlyRate))
                    - parseFloat($scope.labourCharge.goodWillAmount) - parseFloat($scope.labourCharge.discountAmount);
                $scope.currentPart.authorizedAmount = $scope.labourCharge.nettAmount + $scope.currentPart.nettAmount;
            }

        }
        $scope.adjustAllDiscountsLabour = function () {
            $scope.goodWillChangingLabour();
            $scope.discountChangingLabour();
        }
        $scope.doRestartPage = function () {
            swal({
                title: "Are you sure?",
                text: "Unsaved data of this page will be lost.",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: true,
                showLoaderOnConfirm: false
            },
                function () {
                    if ($location.path() === '/app/claim/processbank/') {
                        $state.reload();
                    } else {
                        $location.path('app/claim/processbank/');
                    }
                });
        }
        $scope.navigateToClaimListing = function () {
            swal({
                title: "Are you sure?",
                text: "Unsaved data of this page will be lost.",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: true,
                showLoaderOnConfirm: false
            },
                function () {
                    // $location.path('app/claim/listing');
                    $state.go('app.claimlisting');
                });
        }
        $scope.processClaim = function () {
            if ($scope.IsEndorsement) {
                swal({
                    title: "Are you sure?",
                    text: "Selected claim will be endorsed.",
                    type: "info",
                    showCancelButton: true,
                    closeOnConfirm: true,
                    showLoaderOnConfirm: false
                },
                    function () {
                        swal({ title: 'Processing...', text: 'Processing claim endorsement', showConfirmButton: false });
                        var data = {
                            claimId: $scope.claimId,
                            status: $scope.claimProcess.claimStatus,
                            isGoodwillClaim: typeof $scope.claimProcess.isGoodwillClaim === "undefined" ? false : $scope.claimProcess.isGoodwillClaim,
                            claimDate: $scope.claimProcess.claimDate,
                            claimMileage: $scope.claimProcess.claimMileage,
                            policyDocIds: $scope.uploadedDocIds,
                            claimItems: $scope.extendedClaimItems,
                            compalinData: $scope.claimProcess.complaint,
                            loggedInUserId: $localStorage.LoggedInUserId,

                        };

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/EndorseClaim',
                            data: data,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data.code === "error") {
                                swal("TAS Information", data.msg, "error");

                            } else {
                                swal("TAS Information", data.msg, "success");
                                setTimeout(function () { swal.close(); }, 5000);
                                $state.go('app.claimendorsement');
                            }
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {

                        });
                    });

            } else {
                swal({
                    title: "Are you sure?",
                    text: "You are about to save the claim.",
                    type: "info",
                    showCancelButton: true,
                    //closeOnConfirm: true,
                    showLoaderOnConfirm: false
                },
                    function () {
                        if ($scope.claimProcess.claimStatus === 'NA') {
                            customErrorMessage("Please select a claim status.");
                            $scope.validate_claimProcessStatus = "has-error";
                            swal.close();
                            return;
                        } else {
                            $scope.validate_claimProcessStatus = "";
                        }
                        if ($scope.claimProcess.claimStatus === 'APP' || $scope.claimProcess.claimStatus === 'HOL') {
                            $scope.saveProcessedClaim();
                        } else {
                            var isValid = true;
                            if ($scope.claimProcess.comment.trim() === '') {
                                $scope.validate_claimComment = "has-error";
                                customErrorMessage("Please enter a valid comment.");
                                isValid =  false;
                                return isValid;
                            }
                            else {
                                $scope.validate_claimComment = '';
                            }
                            $scope.saveProcessedClaim();
                        }
                    });
            }
        }
        $scope.claimGoodWillChanged = function () {
            if ($scope.claimProcess.isGoodwillClaim) {
                $scope.claimProcess.claimStatus = 'APP';
            } else {
                $scope.claimProcess.claimStatus = 'NA';
            }
        }
        $scope.saveProcessedClaim = function () {
            if ($scope.validateClaimStatus() && $scope.validateClaimItems() && $scope.validateAuthoriedAmount()) {
                if ($scope.validateEngineerComments()) {
                    swal({ title: 'Processing...', text: 'Processing claim', showConfirmButton: false });
                    var data = {
                        claimId: $scope.claimId,
                        status: $scope.claimProcess.claimStatus,
                        isGoodwillClaim: typeof $scope.claimProcess.isGoodwillClaim === "undefined" ? false : $scope.claimProcess.isGoodwillClaim,
                        claimDate: $scope.claimProcess.claimDate,
                        claimMileage: $scope.claimProcess.claimMileage,
                        policyDocIds: $scope.uploadedDocIds,
                        comment: $scope.claimProcess.comment,
                        loggedInUserId: $localStorage.LoggedInUserId,
                        claimtaxIds: $scope.claimProcess.countryTaxes,
                        athorizedAmountbefortax: $scope.totalauthorizedAmount
                    };

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/ProcessClaim',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data.Status === "ok") {
                            swal("TAS Information", "Claim processed successfully.", "success");
                            $location.path('app/claim/processbank/');
                        } else {
                            swal("TAS Information", "Claim processed successfully.", "success");
                            $location.path('app/claim/processbank/');
                            //customErrorMessage(data.Status);
                        }
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {
                        swal.close();
                    });
                } else {
                    customErrorMessage("Please save assessment data");
                }
            }
        }
        $scope.validateClaimStatus = function () {
            if ($scope.claimProcess.claimStatus === "NA") {
                customErrorMessage("Please select a claim status");
                $scope.validate_claimProcessStatus = "has-error";
                return false;
            } else {
                $scope.validate_claimProcessStatus = "";
                var isValid = true;
                if ($scope.claimProcess.claimDate === "") {
                    //alert($scope.claimProcess.claimDate);
                    $scope.validate_claimDate = "has-error";
                    isValid = false;
                } else {
                    $scope.validate_claimDate = "";
                }

                if (parseFloat($scope.claimProcess.claimMileage) && $scope.claimProcess.claimMileage > 0) {
                    $scope.validate_claimMileage = "";
                } else {
                    $scope.validate_claimMileage = "has-error";
                    isValid = false;
                }
                if (!isValid)
                    customErrorMessage("Please fill all the highlighted fields");
                return isValid;
            }
        }
        $scope.validateClaimItems = function () {
            var isValid = true;

            angular.forEach($scope.extendedClaimItems, function (claimItem) {
                if (isValid && claimItem.partName !== "Sundry") {
                    if (claimItem.itemStatus === "A" || claimItem.itemStatus === "R") {

                    } else {
                        $scope.validate_claimItemsTable = "has-error";
                        $scope.assesmentTabActive = true;
                        //customErrorMessage("Please review all the claim items");
                        isValid = true;
                        return isValid;
                    }
                }
            });
            if (isValid) {
                $scope.assesmentTabActive = false;
                $scope.validate_claimItemsTable = "";
            }
            $scope.validate_claimItemsTable = "";
            return isValid;
        }
        $scope.validateAuthoriedAmount = function ()
        {
            var isValid = false;
            if ($scope.totalauthorizedAmount + $scope.policyDetails.sumOfAuthorizedClaimedAmount > $scope.claimProcess.totalLiability) {

                customErrorMessage("Claim Amount is exceeding Total liability value.");
                return isValid;
            }
            else {

                isValid = true;
                return isValid;
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
        
        
        
        

   
        
        
        $scope.documentUploadValidation = function () {
            if ($scope.docUploader.queue.length == 0) {
                customErrorMessage("No documents selected for upload.");
                return;
            }
            if ($scope.preValidationDocUpload()) {
                $scope.uploadDocuments();
            }
        }
        $scope.preValidationDocUpload = function () {
            var isValid = true;
            if (isValid) {
                if (typeof $scope.claimProcess.claimItemList === "undefined" || $scope.claimProcess.claimItemList.length === 0) {
                    customErrorMessage("Please add claim items before upload documents");
                    isValid = false;
                }

                var isProcessed = false;
                if (isValid) {
                    angular.forEach($scope.extendedClaimItems, function (claimItem) {
                        if (parseFloat(claimItem.authorizedAmount) && parseFloat(claimItem.authorizedAmount) > 0)
                            isProcessed = true;
                    });
                    if (!isProcessed) {
                        customErrorMessage("Please review claim items before upload documents");
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        $scope.uploadDocuments = function () {

            if ($scope.docUploader.queue.length > 0) {
                for (var i = 0; i < $scope.docUploader.queue.length; i++) {

                    $scope.docUploader.queue[i].file.name = $scope.docUploader.queue[i].file.name + '@@' + $scope.docUploader.queue[i].documentType;
                }
                $scope.docUploader.uploadAll();
            } else {
                customErrorMessage("Pleae select a document to upload");
            }
        }
        $scope.labourDiscountSchemeChanged = function () {
            var data = {
                "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,
                "type": "L",
                "schemeId": $scope.labourCharge.partDiscountScheme,
                "makeId": $scope.makeId,
                "dealerId": $scope.policyDealerId,
                "countryId": $scope.policyCountryId
            };
            $scope.isLabourDiscountDisabled = false;
            if (data.claimId === emptyGuid() && data.policyId === emptyGuid()) {
                customErrorMessage("Please select a policy or claim first");
                return;
            } else if (data.schemeId === "NA") {

                return;
            }
            swal({ title: 'Processing...', text: 'Getting Dealer Discounts', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetDealerDiscountsByScheme',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.Status === "ok") {

                    $scope.labourCharge.goodWillType = 'P';
                    $scope.labourCharge.goodWillValue = data.GoodWillRate;
                    $scope.labourCharge.discountType = 'P';
                    $scope.labourCharge.discountValue = data.DiscountRate;
                    $scope.discountChangingLabour();
                    $scope.goodWillChangingLabour();
                    $scope.isLabourDiscountDisabled = true;
                } else {

                    $scope.labourCharge.partDiscountScheme = 'NA';
                    $scope.labourCharge.goodWillType = 'NA';
                    $scope.labourCharge.goodWillValue = 0.00;
                    $scope.labourCharge.goodWillAmount = 0.00;
                    $scope.labourCharge.discountType = 'NA';
                    $scope.labourCharge.discountValue = 0.00;
                    $scope.labourCharge.discountAmount = 0.00;
                    $scope.isLabourDiscountDisabled = false;

                    customErrorMessage(data.Status);
                }
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });
        }
        $scope.partDiscountSchemeChanged = function () {
            var data = {
                "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,
                "type": "P",
                "schemeId": $scope.currentPart.partDiscountScheme,
                "makeId": $scope.makeId,
                "dealerId": $scope.policyDealerId,
                "countryId": $scope.policyCountryId
            };
            $scope.isPartDiscountDisabled = false;
            if (data.claimId === emptyGuid() && data.policyId === emptyGuid()) {
                customErrorMessage("Please select a policy or claim first");
                return;
            } else if (data.schemeId === "NA") {

                return;
            }
            swal({ title: 'Processing...', text: 'Getting Dealer Discounts', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetDealerDiscountsByScheme',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data.Status === "ok") {

                    $scope.currentPart.goodWillType = 'P';
                    $scope.currentPart.goodWillValue = data.GoodWillRate;
                    $scope.currentPart.discountType = 'P';
                    $scope.currentPart.discountValue = data.DiscountRate;
                    $scope.discountChanging();
                    $scope.goodWillChanging();
                    $scope.isPartDiscountDisabled = true;
                } else {

                    $scope.currentPart.partDiscountScheme = 'NA';
                    $scope.currentPart.goodWillType = 'NA';
                    $scope.currentPart.goodWillValue = 0.00;
                    $scope.currentPart.goodWillAmount = 0.00;
                    $scope.currentPart.discountType = 'NA';
                    $scope.currentPart.discountValue = 0.00;
                    $scope.currentPart.discountAmount = 0.00;
                    $scope.isPartDiscountDisabled = false;

                    customErrorMessage(data.Status);
                }
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });
        }
        $scope.defaultPartId = emptyGuid();
        $scope.updatePartMode = false;
        $scope.updateSundry = false;
        $scope.editClaimItem = function (itemId) {
            console.log(itemId);
            if (parseInt(itemId)) {
                angular.forEach($scope.extendedClaimItems, function (claimItem) {
                    if (claimItem.id === itemId) {
                        console.log(claimItem);
                        if (claimItem.partName !== "Sundry") {
                            $scope.resetSundryDetails();
                            $scope.updateSundry = false;
                            $scope.defaultPartId = claimItem.partId;
                            $scope.updatePartMode = true;
                            $scope.currentPart = {
                                id: claimItem.id,
                                partAreaId: claimItem.partAreaId,
                                partId: claimItem.partId,
                                partNumber: claimItem.partNumber,
                                partName: claimItem.partName,
                                partQty: claimItem.partQty,
                                unitPrice: parseFloat(claimItem.unitPrice.replace(/\,/g, '')),
                                remark: claimItem.remark,
                                isRelatedPartsAvailable: false,
                                allocatedHours: 0,
                                goodWillType: parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')) ? claimItem.goodWillType : 'NA',
                                goodWillValue: claimItem.goodWillType == 'F' ? parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')) : parseFloat(claimItem.goodWillValue.replace(/\,/g, '')),
                                goodWillAmount: parseFloat(claimItem.goodWillAmount.replace(/\,/g, '')),
                                discountType: parseFloat(claimItem.discountAmount.replace(/\,/g, '')) ? claimItem.discountType : 'NA',
                                discountValue: claimItem.discountType == 'F' ? parseFloat(claimItem.discountAmount.replace(/\,/g, '')) : parseFloat(claimItem.discountValue.replace(/\,/g, '')),
                                discountAmount: parseFloat(claimItem.discountAmount.replace(/\,/g, '')),
                                fault: claimItem.fault == "" ? emptyGuid() : claimItem.faultId,
                                serverId: claimItem.serverId,
                                partDiscountScheme: 'NA',
                                rejectionTypeId: claimItem.rejectionTypeId,
                                nettAmount: parseFloat(claimItem.nettAmount.replace(/\,/g, '')),
                                authorizedAmount: parseFloat(claimItem.unitPrice.replace(/\,/g, '')),
                                itemStatus: claimItem.itemStatus == null ? "" : claimItem.itemStatus,
                                causeOfFailureId: claimItem.causeOfFailureId == "" ? "" : claimItem.causeOfFailureId,

                            };
                            $scope.selectedPart = {
                                Id: claimItem.partId,
                                PartNumber: claimItem.partNumber,
                                PartName: claimItem.partName,
                            };
                            $scope.selectedPartChanged($scope.selectedPart);
                            $scope.selectedPartAreaChanged();
                            $scope.selectedFaultChanged();

                            $scope.labourCharge = {
                                chargeType: claimItem.l_chargeType,
                                hourlyRate: parseFloat(claimItem.l_hourlyRate.replace(/\,/g, '')),
                                hours: parseFloat(claimItem.l_hours),
                                totalAmount: parseFloat(claimItem.l_totalAmount.replace(/\,/g, '')),
                                description: claimItem.l_description,
                                partId: claimItem.l_partId,
                                goodWillType: claimItem.l_goodWillType,
                                goodWillValue: parseFloat(claimItem.l_goodWillValue.replace(/\,/g, '')),
                                goodWillAmount: parseFloat(claimItem.l_goodWillAmount.replace(/\,/g, '')),
                                discountType: claimItem.l_discountType,
                                discountValue: parseFloat(claimItem.l_discountValue.replace(/\,/g, '')),
                                discountAmount: parseFloat(claimItem.l_discountAmount.replace(/\,/g, '')),
                                labourDiscountScheme: 'NA',
                                nettAmount: parseFloat(claimItem.l_nettAmount.replace(/\,/g, '')),
                                authorizedAmount: parseFloat(claimItem.l_authorizedAmount.replace(/\,/g, ''))
                            }; 

                            if ($scope.Products[0].Productcode == "TYRE") {
                                //$scope.currentPart.authorizedAmount = parseFloat(claimItem.unitPrice.replace(/\,/g, ''))
                                $scope.currentPart.authorizedAmount = $scope.currentPart.nettAmount + $scope.labourCharge.nettAmount
                            } else {
                                $scope.currentPart.authorizedAmount = $scope.currentPart.nettAmount + $scope.labourCharge.nettAmount
                            }


                        } else {
                            $scope.resetPartDetails();
                            $scope.updateSundry = true;
                            $scope.updatePartMode = false;
                            $scope.sundry.value = parseFloat(claimItem.nettAmount.replace(/\,/g, ''));
                            $scope.sundry.name = claimItem.partNumber;
                            $scope.sundry.serverId = claimItem.serverId;
                        }
                    }
                });
            }
        }
        $scope.resetPartDetails = function () {

            $scope.defaultPartId = emptyGuid();
            $scope.updatePartMode = false;
            $scope.clearPartDetails();

        }
        $scope.parseFloat = function (value) {
            if (parseFloat(value)) {
                return parseFloat(value);
            } else {
                return 0.00;
            }
        }
        $scope.resetSundryDetails = function () {
            $scope.updateSundry = false;
            $scope.sundry.value = '';
            $scope.sundry.name = '';
        }
        //$scope.showAddNewPartPopup = function () {
        //    ClaimItemEntryPopup.close();
        //    AddNewPartPopup = ngDialog.open({
        //        template: 'popUpAddNewPart',
        //        className: 'ngdialog-theme-plain',
        //        closeByEscape: false,
        //        showClose: true,
        //        closeByDocument: false,
        //        scope: $scope,
        //        preCloseCallback: function () {
        //            ClaimItemEntryPopup = ngDialog.open({
        //                template: 'popUpAddClaimItem',
        //                className: 'ngdialog-theme-plain',
        //                closeByEscape: true,
        //                showClose: true,
        //                closeByDocument: true,
        //                scope: $scope,
        //                preCloseCallback: function () {
        //                    if (isGuid($scope.claimId)) {
        //                        if ($location.path() === '/app/claim/process/') {
        //                            $location.path('app/claim/process/' + $scope.claimId);
        //                        }
        //                    }
        //                }
        //            });
        //        }
        //    });
        //}

        $scope.uDTValueCalculation = function () {
            $scope.CalculateUDTvalues = '';
            var data = {
                "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,
               "partId": $scope.partId,
            };
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/CalculateUDT',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CalculateUDTvalues = data;
                if (data.WithinMonth == true) {
                    $scope.WithinMonthlbl = true;
                } else {
                    $scope.WithinMonthlbl = false;
                }
               
            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });

        }

        $scope.showCalculationPopup = function () {
           // ClaimItemEntryPopup.close();

            $scope.uDTValueCalculation();

            AddNewCalculationPopup = ngDialog.open({
                template: 'popUpCalcuationUDT',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope,
                
            });
        }
        $scope.getCommodityTypeByCommodityCategoryId = function (commodityCategoryId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/CommodityItemAttributes/GetCommodityTypeByCommodityCategoryId',
                data: { "Id": commodityCategoryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.newPart.CommodityId = data.CommodityTypeId;
                $scope.commodityCode = data.CommodityCode;

            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });
        }
        $scope.getallmakes = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllMakesByDealerId',
                dataType: 'json',
                data: { dealerId: $scope.claimProcess.claimDealerId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.makes = data;
                $scope.newPart.MakeId = $scope.makeId;
            }).error(function (data, status, headers, config) {
            });
        }
        $scope.validateSavePartDetails = function () {
            var isValid = true;
            if (!isGuid($scope.newPart.PartAreaId)) {
                $scope.validate_partPartArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partPartArea = "";
            }

            if ($scope.newPart.PartName === '') {
                $scope.validate_partName = "has-error";
                isValid = false;
            } else {
                $scope.validate_partName = "";
            }

            if ($scope.newPart.PartCode === '') {
                $scope.validate_partCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_partCode = "";
            }

            if ($scope.newPart.PartNumber === '') {
                $scope.validate_partNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_partNumber = "";
            }

            if ($scope.newPart.Price === '' || !parseFloat($scope.newPart.Price) || parseFloat($scope.newPart.Price) < 0) {
                $scope.newPart.Price = 0.00;
            }
            if ($scope.newPart.AllocatedHours === '' || !parseFloat($scope.newPart.AllocatedHours || parseFloat($scope.newPart.AllocatedHours) < 0)) {
                $scope.newPart.AllocatedHours = 0.00;
            }
            //alert(isValid);
            return isValid;
        }
        $scope.savePart = function () {
            $scope.newPart.DealerId = $scope.claimProcess.claimDealerId;
            $scope.newPart.MakeId = $scope.makeId;
            $scope.newPart.EntryBy = $localStorage.LoggedInUserId;
            if ($scope.validateSavePartDetails()) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/SaveNewPart',
                    data: $scope.newPart,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'error') {
                        customErrorMessage(data.msg);
                    } else {
                        customInfoMessage('Part successfully saved.');
                        $scope.selectedPartAreaChanged();
                        $scope.clearPartAddDetailsUponSave();
                        AddNewPartPopup.close();

                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {

                });

            } else {
                customErrorMessage('Please fill all the mandatory fields');
            }
        }
        $scope.clearPartAddDetailsUponSave = function () {
            $scope.newPart = {
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
        $scope.showAttachmnetDetails = function () {

            $scope.docUploader.queue = [];
            AttachmnetDetailsPopup = ngDialog.open({
                template: 'popUpAttachmnetDetails',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetClaimAndPolicyAttachmentsByPolicyId',
                data: { "Id": $scope.policyId },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {



                $scope.attachments_temp = data.Attachments;

                for (var i = 0; i < data.Attachments.length; i++) {
                    var file = {
                        name: data.Attachments[i].FileName,
                        size: data.Attachments[i].AttachmentSizeKB
                    }
                    var attachment = {
                        documentType: data.Attachments[i].DocumentType,
                        id: data.Attachments[i].Id,
                        file: file,
                        ref: data.Attachments[i].FileServerRef,
                        attachmentSection: data.Attachments[i].AttachmentSection,
                        policyNumber: $scope.claimProcess.policyNumber,
                        claimNumber: $scope.claimProcess.claimNumber,
                        dateOfAttachment: data.Attachments[i].DateOfAttachment
                    }

                    //if (data.Attachments[i].AttachmentSection === "Customer") {
                    //    $scope.customerDocUploader.queue.push(attachment)
                    //} else if (data.Attachments[i].AttachmentSection === "Item") {
                    $scope.docUploader.queue.push(attachment)
                    //} else if (data.Attachments[i].AttachmentSection === "Policy") {
                    //    $scope.policyDocUploader.queue.push(attachment)
                    //} else if (data.Attachments[i].AttachmentSection === "Payment") {
                    //    $scope.paymentDocUploader.queue.push(attachment)
                    //} else if (data.Attachments[i].AttachmentSection === "Other") {
                    //    $scope.customerDocUploader.queue.push(attachment)
                    //}
                }

            });


        }
        $scope.downloadAttachmentUploaded = function (ref) {
            if (ref != '') {
                swal({ title: 'Processing...!', text: 'Preparing your download...', showConfirmButton: false });
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
        $scope.showNotesDetails = function () {
            AttachmnetDetailsPopup = ngDialog.open({
                template: 'popUpNoteDetails',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetClaimNotesPolicyId',
                data: { "Id": $scope.policyId },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimNotes = data.Notes;
            });
        }
        $scope.addNotes = function () {
            if ($scope.claimNotes.note == null || $scope.claimNotes.note == "") {
                customErrorMessage("Please Enter all required fields.");
            } else {
                var data = {
                    "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                    "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,
                    "note": $scope.claimNotes.note,
                    "submittedUserId": $localStorage.LoggedInUserId
                };

                swal({ title: 'Processing...', text: 'Saving Note Information', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/AddNotes',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data === "ok") {
                        //$scope.showNotesDetails();
                        customInfoMessage("Note successfully saved.");
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Claim/GetClaimNotesPolicyId',
                            data: { "Id": $scope.policyId },
                            headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.claimNotes = data.Notes;
                        });
                        //$scope.clearPartDetails();
                    } else {
                        customErrorMessage(data);
                    }
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            }
        }
        $scope.showCommentsDetails = function () {
            AttachmnetDetailsPopup = ngDialog.open({
                template: 'popUpCommentsDetails',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetClaimPendingCommentsByPolicyId',
                data: { "Id": $scope.policyId },
                headers: { 'Authorization': $localStorage.jwt === undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimPendingComments = data.Comments;
            });
        }
    });