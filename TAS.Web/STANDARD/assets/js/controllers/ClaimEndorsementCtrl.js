app.controller('ClaimEndorsementCtrl',
    function ($scope, $rootScope, $http, ngDialog, $location, SweetAlert, $localStorage, $cookieStore, $filter, toaster, FileUploader, $state, ngTableParams, $stateParams) {

        $scope.claimSearchGridloading = false;
        $scope.claimGridloadAttempted = false;


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

        $scope.claimId = "";
        $scope.policyid = "";
        $scope.dealerId = emptyGuid();
        $scope.partAreas = [];
        $scope.part = {};
        $scope.serviceRecord = {};
        $scope.sundry = {};
        $scope.parts = [];
        $scope.policyDetails = {};
        $scope.assesmentHistory = [];
        $scope.claimHistory = [];
        $scope.uploadedDocIds = [];
        $scope.docAttachmentType = "";
        $scope.datapart = [];
        $scope.Labourdetails = [];
        $scope.addSundryDetails = [];

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
            serverId: emptyGuid()
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
            discountAmount: 0.00
        };
        $scope.sundry = {
            partNumber: '',
            unitPrice: 0.00
        };

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

            $scope.docUploader.queue = [];
            //  $scope.submitClaim();
        }
        $scope.docUploader.filters.push({
            name: 'customFilter',
            fn: function (item, options) {
                alert($scope.docAttachmentType);
                if ($scope.docAttachmentType !== "" && typeof $scope.docAttachmentType !== 'undefined') {
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
            $scope.showClaimSearchPopup();

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
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;

            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDocumentTypesByPageName',
                dataType: 'json',
                data: { PageName: 'ClaimEndorsementSubmission' },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.docAttachmentTypes = data;
            }).error(function (data, status, headers, config) {
            });

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
                        "loggedInUserId": $localStorage.LoggedInUserId,
                    },
                }).success(function (data, status, headers, config) {
                    if (data.status === "ok") {
                        $scope.selectClaimToProcess($scope.claimId, data.policyId);
                        //  $scope.policyId = data.policyId;
                    } else if (data.status === "warning") {
                        swal("TAS Information", data.msg, "error");
                    } else {
                        swal("TAS Information", data.msg, "error");
                    }

                }).error(function (data, status, headers, config) {
                });

            }

        };

        $scope.showClaimSearchPopupReset = function () {
          
            $scope.claimSearchGridSearchCriterias = {
                commodityTypeId: "",
                claimNumber: "",
                policyId: "",
                customerId: "",
                claimId: ""

            }
        }

        $scope.showClaimSearchPopup = function () {            
            getClaimSearchPage();
            ClaimSelectionPopup = ngDialog.open({
                template: 'popUpClaimSelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }

        $scope.claimSearchGridSearchCriterias = {
            commodityTypeId: "",
            claimNumber: "",
            policyId: "",
            customerId: "",
            claimId: ""

        };

        var paginationOptionsClaimSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };
        $scope.gridOptionsClaim = {
            paginationPageSizes: [25, 50, 75],
            paginationPageSize: 25,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
              { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'PolicyId', field: 'PolicyId', enableSorting: false, visible: false, cellClass: 'columCss' },
              { name: 'Commodity Type', field: 'CommodityType', enableSorting: false, cellClass: 'columCss' },
                { name: 'Policy Number', field: 'PolicyNumber', width: '20%', enableSorting: false, cellClass: 'columCss', },
              { name: 'Claim Number', field: 'ClaimNumber', enableSorting: false, cellClass: 'columCss' },
                { name: 'Claim Submitted Dealer', field: 'ClaimDealer', width: '25%', enableSorting: false, cellClass: 'columCss' },
              { name: 'Make', field: 'Make', enableSorting: false, cellClass: 'columCss' },
              { name: 'Model', field: 'Model', enableSorting: false, cellClass: 'columCss' },
              { name: 'Total Claim Amount', field: 'ClaimAmount', enableSorting: false, cellClass: 'columCss' },
              {
                  name: ' ',
                  cellTemplate: '<div class="center"><button ng-click="grid.appScope.selectClaimToProcess(row.entity.Id,row.entity.PolicyId)" class="btn btn-xs btn-warning">Load</button></div>',
                  width: 60,
                  enableSorting: false
              }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsClaimSearchGrid.sort = null;
                    } else {
                        paginationOptionsClaimSearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    getClaimSearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimSearchGrid.pageNumber = newPage;
                    paginationOptionsClaimSearchGrid.pageSize = pageSize;
                    getClaimSearchPage();
                });
            }
        };
        $scope.refresSearchGridData = function () {
            getClaimSearchPage();
        }

        var getClaimSearchPage = function () {            
            $scope.claimSearchGridloading = true;
            $scope.claimGridloadAttempted = false;

            var claimSearchGridParam =
                {
                    'paginationOptionsClaimSearchGrid': paginationOptionsClaimSearchGrid,
                    'claimSearchGridSearchCriterias': $scope.claimSearchGridSearchCriterias
                }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllClaimForSearchGrid',
                data: claimSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsClaim.data = response_arr.data;
                $scope.gridOptionsClaim.totalItems = response_arr.totalRecords;
            }).error(function (data, status, headers, config) {
            }).finally(function () {                
                $scope.claimSearchGridloading = false;
                $scope.claimGridloadAttempted = true;

            });
        };

        $scope.refresClaimSearchGridData = function () {
            getClaimSearchPage();
        }
      

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
                dealer: '',
                engineer: '',
                conclution: ''
            },
            claimStatus: ''
        }
        $scope.currentClaimItem = {
            fault: emptyGuid()
        };



        $scope.selectClaimToProcess = function (claimId, policyid) {
            if (isGuid(claimId)) {
                ClaimSelectionPopup.close();
                $location.path('app/claim/process/' + claimId+'/true');
            } else {
                customErrorMessage("Invalid claim selection.");
            }
            //$scope.claimId = claimId;
            //if (isGuid(claimId)) {
            //    $scope.policyId = policyid;
            //    swal({ title: 'Loading...', text: 'Reading claim information', showConfirmButton: false });
            //    if (typeof ClaimSelectionPopup !== 'undefined')
            //        ClaimSelectionPopup.close();
            //    $http({
            //        method: 'POST',
            //        url: '/TAS.Web/api/claim/GetClaimDetilsByClaimId',
            //        data: {
            //            "claimId": claimId
            //        },
            //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //    }).success(function (data, status, headers, config) {
            //        $scope.claimProcess.policyNumber = data.PolicyNumber;
            //        $scope.claimProcess.claimNumber = data.ClaimNumber;
            //        $scope.claimProcess.currencyCode = data.CurrencyCode;
            //        $scope.claimProcess.totalClaimAmount = data.TotalClaimAmount;
            //        $scope.claimProcess.authorizedClaimAmount = data.AuthorizedClaimAmount;
            //        $scope.claimProcess.complaint.customer = data.Complaint.customer;
            //        $scope.claimProcess.complaint.dealer = data.Complaint.dealer;
            //        $scope.claimProcess.complaint.engineer = data.Complaint.engineer;
            //        $scope.claimProcess.complaint.conclution = data.Complaint.conclution;

            //        $scope.claimProcess.claimDealerId = data.DealerId;
            //        $scope.claimProcess.country = data.Country;
            //        $scope.claimProcess.claimItemList = data.ClaimItemList;

            //        $scope.makeId = data.MakeId;
            //        $scope.modelId = data.ModelId;
            //        $scope.dealerId = data.DealerId;

            //        $scope.getAllPartAreas(data.CommodityCategoryId);


            //        $scope.loadPolicy($scope.policyId, true);

            //    }).error(function (data, status, headers, config) {

            //    }).finally(function () {
            //        swal.close();
            //    });
            //}
        }


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


        $scope.loadPolicy = function (policyId, isExistingPolicy) {
            // alert(policyId);
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
                        //$scope.claimProcess.claimDealer ='';
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

                        $scope.GetServiceHistory($scope.policyId);
                    } else {
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        //----------------------- policy Details ----------------------------------

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
                PolicyDetailsPopup = ngDialog.open({
                    template: 'popUpPolicyDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: false,
                    scope: $scope
                });

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
                $scope.policyDetails.manfWarrentyStartDate = data.manfWarrentyStartDate;
                $scope.policyDetails.manfWarrentyEndDate = data.manfWarrentyEndDate;
                $scope.policyDetails.extensionStartDate = data.extensionStartDate;
                $scope.policyDetails.extensionEndDate = data.extensionEndDate;
                $scope.policyDetails.manfWarrentyMonths = data.manfWarrentyMonths;
                $scope.policyDetails.extensionPeriod = data.extensionPeriod;
                $scope.policyDetails.extensionMilage = data.extensionMilage;
                $scope.policyDetails.manfWarrentyMilage = data.manfWarrentyMilage;


            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }

        //---------------------------Cover Details -----------------------------
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
                CoverDetailsPopup = ngDialog.open({
                    template: 'popUpCoverDetails',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });
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

        //---------------------------- Assesmet Details -------------------
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

                $scope.assesmentHistory = data.AssessmentData;
                //  $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

        //------------------------------------- Claim History ---------------------------

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

        $scope.showAddNewClaimItem = function () {

            ClaimItemEntryPopup = ngDialog.open({
                template: 'popUpAddClaimItem',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
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

        $scope.selectedPartAreaChanged = function () {
            $scope.part.selected = undefined;
            $scope.currentPart.partNumber = '';
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
                swal({ title: 'Loading...', text: 'Validating part information', showConfirmButton: false });
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

        $scope.adjustAllDiscounts = function () {
            $scope.goodWillChanging();
            $scope.discountChanging();
        }

        $scope.goodWillChanging = function () {
            if (parseInt($scope.currentPart.partQty) && parseInt($scope.currentPart.partQty) > 0) {
                if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                    //part has total price
                    if ($scope.currentPart.goodWillType == "P") {
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
                    } else if ($scope.currentPart.goodWillType == "F") {
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

            if (parseFloat($scope.currentPart.unitPrice) && parseFloat($scope.currentPart.unitPrice) >= 0) {
                $scope.validate_unitPrice = "";
            } else {

                $scope.validate_unitPrice = "has-error";
                isValid = false;
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
                    customErrorMessage("Good will + Discount cannot be exceed total part(s) amount");
                    isValid = false;
                }
            }

            if (!isGuid($scope.currentPart.fault)) {
                $scope.validate_fault = "has-error";
                isValid = false;
            } else {
                $scope.validate_fault = "";

            }
            return isValid;
        }


        $scope.addPart = function () {
            if ($scope.validatePartDetails()) {

                //$scope.currentPart,


                var currentClaimItem = {
                    itemName: $scope.currentPart.partName,
                    itemNumber: $scope.currentPart.partNumber,
                    claimItem: $scope.currentPart.partName == "sundry" ? "S" : "P",
                    qty: $scope.currentPart.partQty,
                    totalPrice: ($scope.currentPart.unitPrice * $scope.currentPart.partQty),
                    remarks: $scope.currentPart.remark,
                    discountAmount: $scope.currentPart.discountAmount,
                    goodWillAmount: $scope.currentPart.goodWillAmount,
                    unitPrice: $scope.currentPart.unitPrice,
                    discountType: ($scope.currentPart.discountType) ? "P" : "F",
                    discountRate: $scope.currentPart.discountValue,
                    fault: $scope.currentPart.fault,
                    status: "",
                    authorizedAmount: ($scope.currentPart.unitPrice * $scope.currentPart.partQty),
                    goodWillType: ($scope.currentPart.goodWillType) ? "P" : "F",
                    goodwillRate: $scope.currentPart.goodWillValue,
                    comment: $scope.currentPart.remark,

                };


                $scope.claimProcess.claimItemList.push(currentClaimItem)

                $scope.clearcurrentPart();
                ClaimItemEntryPopup.close();

            }

            else {

                customErrorMessage("Please fill valid data for highlighted fileds.");
            }
        }


            $scope.addLabourCharge = function () {
                if ($scope.validateLabourPayments()) {

                    var currentClaimItem = {
                        itemName: "Labour Charge",
                        itemNumber: $scope.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                        claimItem: "",
                        qty: $scope.labourCharge.hours == 0 ? 1 : $scope.labourCharge.hours,
                        totalPrice: ($scope.labourCharge.chargeType == "H" ? ($scope.labourCharge.hourlyRate * $scope.labourCharge.hours) : $scope.labourCharge.hourlyRate) == 0 ?
                        $scope.labourCharge.hours * $scope.labourCharge.discountAmount : 1 * $scope.labourCharge.hourlyRate,
                        remarks: "Added By Claim Engineer",
                        discountAmount: $scope.labourCharge.discountAmount,
                        goodWillAmount: $scope.labourCharge.goodWillAmount,
                        unitPrice: $scope.labourCharge.hourlyRate,
                        discountType: ($scope.labourCharge.discountType) ? "P" : "F",
                        discountRate: $scope.labourCharge.discountValue,
                        fault: emptyGuid(),
                        status: "",
                        authorizedAmount: (($scope.labourCharge.chargeType == "H" ? ($scope.labourCharge.hourlyRate * $scope.labourCharge.hours) : $scope.labourCharge.hourlyRate) - ($scope.labourCharge.discountAmount + $scope.labourCharge.goodWillAmount)),
                        goodWillType: ($scope.labourCharge.goodWillType) ? "P" : "F",
                        goodwillRate: $scope.labourCharge.goodWillType == "F" ? ($scope.labourCharge.goodWillRate) : $scope.labourCharge.goodWillRate,
                        comment: $scope.labourCharge.description,
                        totalGrossPrice: $scope.labourCharge.chargeType == "H" ? ($scope.labourCharge.hourlyRate * $scope.labourCharge.hours) : $scope.labourCharge.hourlyRate,
                        itemCode: $scope.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                    };

                    $scope.claimProcess.claimItemList.push(currentClaimItem);
                    $scope.clearLabourdetails();




                    ClaimItemEntryPopup.close();

                } else {
                    customErrorMessage("Please fill valid data for highlighted fileds.");
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

                if (typeof $scope.labourCharge.partId == 'undefined' || $scope.labourCharge.partId === ""
                    || $scope.labourCharge.partId == null || $scope.labourCharge.partId === emptyGuid()) {

                    $scope.validate_partIdlabourCharge = "has-error";
                    isValid = false;

                } else {
                    $scope.validate_partIdlabourCharge = "";
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


                return isValid;
            }


            $scope.goodWillChangingLabour = function () {
                var totalAmount = 0.00;
                if ($scope.labourCharge.chargeType == 'H') {
                    totalAmount = parseFloat($scope.labourCharge.hourlyRate) * parseFloat($scope.labourCharge.hours);
                } else {
                    totalAmount = parseFloat($scope.labourCharge.hourlyRate);
                }

                if (parseFloat(totalAmount) && parseFloat(totalAmount) > 0) {

                    if ($scope.labourCharge.goodWillType == "P") {
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
                    } else if ($scope.labourCharge.goodWillType == "F") {
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
                if ($scope.labourCharge.chargeType == 'H') {
                    totalAmount = parseFloat($scope.labourCharge.hourlyRate) * parseFloat($scope.labourCharge.hours);
                } else {
                    totalAmount = parseFloat($scope.labourCharge.hourlyRate);
                }

                if (parseFloat(totalAmount) && parseFloat(totalAmount) > 0) {
                    if ($scope.labourCharge.discountType == "P") {
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
                    } else if ($scope.labourCharge.discountType == "F") {
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



            $scope.addSundry = function () {
                if ($scope.validateSundry()) {

                    //$scope.sundry,

                    var currentClaimItem = {
                        itemName: "Sundry",
                        itemNumber: $scope.sundry.partNumber,
                        claimItem: "S",
                        qty: 1,
                        totalPrice: ($scope.sundry.unitPrice * 1),
                        remarks: "Enterd by claim engineer",
                        discountAmount: 0,
                        goodWillAmount: 0,
                        unitPrice: $scope.sundry.unitPrice,
                        discountType: "F",
                        discountRate: 0,
                        fault: emptyGuid(),
                        status: "",
                        authorizedAmount: ($scope.sundry.unitPrice * 1) - (0 + 0),
                        goodWillType: "F",
                        goodwillRate: 0,
                        comment: "Enterd by claim engineer",

                    };

                    $scope.claimProcess.claimItemList.push(currentClaimItem);


                    //$scope.addSundryDetails.push($scope.sundry);
                    ClaimItemEntryPopup.close();
                } else {
                    customErrorMessage('Please fill all the highlighted fields.');
                }
            }
        
        $scope.validateSundry = function () {
            var isValid = true;
            if ($scope.sundry.partNumber == '') {
                $scope.validate_sundryName = "has-error";
                isValid = false;
            } else {
                $scope.validate_sundryName = "";
            }

            if (!parseFloat($scope.sundry.unitPrice)) {
                $scope.validate_sundryValue = "has-error";
                isValid = false;
            } else {
                $scope.validate_sundryValue = "";
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
                customErrorMessage("Pleace select a document to upload");
            }
        }

        $scope.processClaim = function () {
            swal({
                title: "Are you sure?",
                text: "You are about to process the claim.",
                type: "info",
                showCancelButton: true,
                closeOnConfirm: true,
                showLoaderOnConfirm: false
            },
               function () {
                   if ($scope.validateClaimStatus() && $scope.validateClaimItems()) {
                       if ($scope.validateEngineerComments()) {
                           swal({ title: 'Processing...', text: 'Processing claim', showConfirmButton: false });
                           var ClaimEndorsementDetails = {
                               "assesmentDetails": $scope.assesmentDetails,
                               "ClaimList": $scope.claimProcess.claimItemList,
                               "attachmentIds": $scope.uploadedDocIds,                               
                               "claimId": $scope.claimId === '' ? emptyGuid() : $scope.claimId,
                               "policyId": typeof $scope.policyId === 'undefined' ? emptyGuid() : $scope.policyId,                               
                               "loggedInUserId": $localStorage.LoggedInUserId,
                               "dealerId": typeof $scope.dealerId === 'undefined' ? emptyGuid() : $scope.dealerId,          
                              
                               "isGoodwillClaim": typeof $scope.claimProcess.isGoodwillClaim === "undefined" ? false : $scope.claimProcess.isGoodwillClaim
                           }

                           $http({
                               method: 'POST',
                               url: '/TAS.Web/api/claim/ProcessClaimEndorsement',
                               data: { "ClaimEndorsementDetails" : ClaimEndorsementDetails},
                               headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                           }).success(function (data, status, headers, config) {
                               if (data.Status === "ok") {
                                   swal("TAS Information", "Claim Endorsement processed successfully.", "success");
                                   $location.path('app/claim/claimendorsement');
                               } else {
                                   customErrorMessage(data.Status);
                               }

                           }).error(function (data, status, headers, config) {
                           }).finally(function () {
                               swal.close();
                           });
                       } else {
                           customErrorMessage("Please save assessment data");
                       }
                   }
               });
        }

        $scope.assesmentDetails = "";
        $scope.assesmentSaveBtnDisabled = false;

        $scope.saveEngineerComment = function () {
            if ($scope.validateEngineerComments()) {
                //swal({ title: 'Processing...', text: 'Saving claim Endrosment Assesment', showConfirmButton: false });
                //$scope.policyId = policyId;
                var data = {
                    policyId: $scope.policyId,
                    claimId: $scope.claimId,
                    dealerId: $scope.dealerId,
                    engineer: $scope.claimProcess.complaint.engineer,
                    conclution: $scope.claimProcess.complaint.conclution,
                    loggedInUserId: $localStorage.LoggedInUserId,
                }

                $scope.assesmentDetails = data;
                customInfoMessage("Claim Endrosment Assesment added successfully.");
                $scope.assesmentSaveBtnDisabled = true;

                
            } else {
                customErrorMessage("Please enter all mandatory fields.");
            }
        }

        $scope.validateClaimStatus = function () {
            if ($scope.claimProcess.claimStatus === "") {
                customErrorMessage("Please select a claim status");
                $scope.validate_claimProcessStatus = "has-error";
                return false;
            } else {
                $scope.validate_claimProcessStatus = "";
                return true;
            }
        }
        $scope.validateClaimItems = function () {
            var isValid = true;
            angular.forEach($scope.alradyaddclaimItems, function (claimItem) {
                if (isValid) {
                    if (claimItem.status === "A" || claimItem.status === "R") {

                    } else {
                        $scope.validate_claimItemsTable = "has-error";
                        $scope.assesmentTabActive = true;
                        customErrorMessage("Please process all the claim items");
                        isValid = false;
                        return false;
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

        $scope.validateEngineerComments = function () {
            var isValid = true;
            if ($scope.claimProcess.complaint.engineer == null || $scope.claimProcess.complaint.engineer.length === 0) {
                $scope.validate_engineerComment = "has-error";
                isValid = false;
            } else {
                $scope.validate_engineerComment = "";
            }
            if ($scope.claimProcess.complaint.conclution == null || $scope.claimProcess.complaint.conclution.length === 0) {
                $scope.validate_conclution = "has-error";
                isValid = false;
            } else {
                $scope.validate_conclution = "";
            }

            if (isValid) {
                if (typeof $scope.claimProcess.claimItemList === "undefined" || $scope.claimProcess.claimItemList.length === 0) {
                    customErrorMessage("Please add claim items before adding the assesment comments");
                    isValid = false;
                }

                var isProcessed = false;
                if (isValid) {
                    angular.forEach($scope.claimProcess.claimItemList, function (claimItem) {
                        if (parseFloat(claimItem.authorizedAmount) && parseFloat(claimItem.authorizedAmount) > 0)
                            isProcessed = true;

                    });
                    if (!isProcessed) {
                        customErrorMessage("Please review claim items before adding the assesment comments");
                        isValid = false;
                    }
                }

            }
            return isValid;
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

        $scope.alradyaddclaimItems = [];

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
                                    if (claimItem.faultId == null) {
                                        claimItem.faultId = emptyGuid();
                                    }
                                    //claimItem.faultId = currentClaimItem.fault === null ? currentClaimItem.fault : emptyGuid();
                                    claimItem.policyId = $scope.policyId;
                                    claimItem.entryBy = $localStorage.LoggedInUserId;                                    
                                    if (claimItem.partId == null) {
                                        claimItem.partId = emptyGuid();
                                    }
                                    itemForPost = claimItem;

                                    $scope.alradyaddclaimItems.push(itemForPost);

                                    
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
                            //claimItem.faultId = currentClaimItem.fault;
                            if (claimItem.faultId == null) {
                                claimItem.faultId = emptyGuid();
                            }
                            claimItem.policyId = $scope.policyId;
                            claimItem.entryBy = $localStorage.LoggedInUserId;
                            if (claimItem.partId == null) {
                                claimItem.partId = emptyGuid();
                            }
                            itemForPost = claimItem;

                            $scope.alradyaddclaimItems.push(itemForPost);

                            
                        }
                    });
                }
            } else {
                customErrorMessage("Please select item status");
                currentClaimItem.validate_status = "has-error";
            }
        }
        //........................adding service history

        $scope.GetServiceHistory = function (policyId) {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllServiceHistoryByPolicyId',
                data: { "policyId": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null)
                    customErrorMessage("No service records found.");
                $scope.itemServiceRecords = data;
            }).error(function (data, status, headers, config) {

            }).finally(function () {

            });
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

        $scope.addServiceHistory = function () {
            if (isGuid($scope.policyId)) {
                if ($scope.validateServiceHistory()) {
                    //adding service history
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/AddServiceHistory',
                        data: {
                            "policyId": $scope.policyId,
                            "serviceData": $scope.serviceRecord
                        },
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        if (data == 'ok') {
                            customInfoMessage("Service record added successfully.");
                            $scope.clearServiceRecord();
                            $scope.GetServiceHistory($scope.policyId);
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

        $scope.clearcurrentPart = function () {

            $scope.currentPart.id = 0;
            $scope.currentPart.partAreaId = emptyGuid();
            $scope.currentPart.partId = emptyGuid();
            $scope.currentPart.partNumber = '';
            $scope.currentPart.partName = '';
            $scope.currentPart.partQty = 1;
            $scope.currentPart.unitPrice = '';
            $scope.currentPart.remark = '';
            $scope.currentPart.isRelatedPartsAvailable = false;
            $scope.currentPart.allocatedHours = 0;
            $scope.currentPart.goodWillType = 'NA';
            $scope.currentPart.goodWillValue = 0.00;
            $scope.currentPart.goodWillAmount = 0.00;
            $scope.currentPart.discountType = 'NA';
            $scope.currentPart.discountValue = 0.00;
            $scope.currentPart.discountAmount = 0.00;
            $scope.currentPart.fault = emptyGuid();
            $scope.currentPart.serverId = emptyGuid();
            $scope.currentPart.partDiscountScheme; 'NA';
        }

        $scope.clearLabourdetails = function (){

            $scope.labourCharge.chargeType = 'H';
            $scope.labourCharge.hourlyRate = 0.00;
            $scope.labourCharge.hours = 0;
            $scope.labourCharge.totalAmount = 0.00;
            $scope.labourCharge.description = '';
            $scope.labourCharge.partId = emptyGuid();
            $scope.labourCharge.goodWillType = 'NA';
            $scope.labourCharge.goodWillValue = 0.00;
            $scope.labourCharge.goodWillAmount = 0.00;
            $scope.labourCharge.discountType = 'NA';
            $scope.labourCharge.discountValue = 0.00;
            $scope.labourCharge.discountAmount = 0.00;

        }

    });