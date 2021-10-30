app.controller('ClaimInquiryCtrl',
    function ($scope, $rootScope, $http, ngDialog, $location, SweetAlert, $localStorage, $cookieStore, $filter, toaster, FileUploader, $state, ngTableParams, $stateParams) {

        $scope.claimInquirySearchGridloading = false;
        $scope.claimInquiryGridloadAttempted = false;

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        $scope.loadInitailData = function () {

            $scope.showClaimInquirySearchPopup();

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ContractManagement/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
                $scope.Allcountries = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;
                //$scope.policySearchInquiryGridSearchCriterias.commodityTypeId = data[2].CommodityTypeId;
                //$scope.selectedCommodityTypeChanged();
            }).error(function (data, status, headers, config) {
            });
        };

        $scope.claimBatch = {
            invoceWarning: '',
            batchGroupWarning: '',
            bordxWarning: '',
            invoiceReceivedDate: '',
            invoiceDate: '',
            invoiceNumber: '',
            batchNumber: '',
            batchEntryDate: '',
            groupName: '',
            insurer: '',
            reinsurer: '',
            bordxYear: '',
            bordxmonth: '',
            bordxNumber: ''
        }

        $scope.claimEndorsement = {

            ExcitingEndoresList: '',
            PolicyNumber: '',
            ClaimNumber: '',
            customer: '',
            Serial: '',
            MakeId: '',
            ModelId: '',
            PolicyDealerId: '',
            ClaimDealer: '',
            TotalClaimAmount: '',
            TotalGrossAmount: '',
            PaidAmount: '',
            ClaimItemList: '',
            complaint: {
                customer: '',
                dealer: '',
                engineer: '',
                conclution: ''
            },

        }

        $scope.newClaimEndorsement = {

            ExcitingEndoresList: '',
            PolicyNumber: '',
            ClaimNumber: '',
            customer: '',
            Serial: '',
            MakeId: '',
            ModelId: '',
            PolicyDealerId: '',
            ClaimDealer: '',
            TotalClaimAmount: '',
            TotalGrossAmount: '',
            PaidAmount: '',
            ClaimItemList: '',
            complaint: {
                customer: '',
                dealer: '',
                engineer: '',
                conclution: ''
            },

        }

        $scope.Claim = {
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
            customerTel: '',
            vinSerial: '',
            make: '',
            model: '',
            entryDate: '',
            approvedDate: '',
            paidAmount: '',
            approvedBy: '',
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

        $scope.showClaimInquirySearchPopupReset = function () {
            $scope.claimInquirySearchGridSearchCriterias = {
                dealerId: emptyGuid(),
                claimNumber: "",
                policyId: "",
                customerId: "",
                claimId: "",
                status: "",
                country: emptyGuid(),

            }
        }

        $scope.showClaimInquirySearchPopup = function () {
            getClaimInquirySearchPage();
            ClaimSelectionPopup = ngDialog.open({
                template: 'popUpClaimInquirySelection',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope
            });
        }

        $scope.claimInquirySearchGridSearchCriterias = {
            dealerId: emptyGuid(),
            claimNumber: "",
            policyId: "",
            customerId: "",
            claimId: "",
            status: "",
            country: emptyGuid(),

        };

        var paginationOptionsClaimInquirySearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };


        $scope.gridOptionsClaimInquiry = {
            paginationPageSizes: [10,25, 50, 75],
            paginationPageSize: 10,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.id'), field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.policyId'), field: 'PolicyId', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.claimNumber'), field: 'ClaimNumber', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.policyNumber'), field: 'PolicyNumber', width: '25%', enableSorting: false, cellClass: 'columCss', },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.make'), field: 'Make', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.model'), field: 'Model', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.vinSerial'), field: 'serialnumber', width: '15%', enableSorting: false, cellClass: 'columCss' },
                { name: $filter('translate')('pages.claimInquiry.searchPopUp.gridClaimInquiry.totalClaimAmount'), field: 'ClaimAmount', enableSorting: false, cellClass: 'columCss' },
                //{ name: 'Date', field: 'EntryDate', enableSorting: false, cellClass: 'columCss' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.selectClaimToInquiry(row.entity.Id,row.entity.PolicyId)" class="btn btn-xs btn-warning">' + $filter('translate')('common.button.load')+'</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                    if (sortColumns.length == 0) {
                        paginationOptionsClaimInquirySearchGrid.sort = null;
                    } else {
                        paginationOptionsClaimInquirySearchGrid.sort = sortColumns[0].sort.direction;
                    }
                    //getClaimInquirySearchPage();
                });
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsClaimInquirySearchGrid.pageNumber = newPage;
                    paginationOptionsClaimInquirySearchGrid.pageSize = pageSize;
                    //getClaimInquirySearchPage();
                });
            }
        };

        $scope.refresSearchGridData = function () {
            getClaimInquirySearchPage();
        }

        var clearFields = function () {
            $scope.claimInquirySearchGridSearchCriterias = {
                dealerId: emptyGuid(),
                claimNumber: "",
                policyId: "",
                customerId: "",
                claimId: "",
                status: "",
                country: emptyGuid(),

            };
        }

        var getClaimInquirySearchPage = function () {
            $scope.claimInquirySearchGridloading = true;
            $scope.claimInquiryGridloadAttempted = false;

            var claimSearchGridParam =
            {
                'paginationOptionsClaimInquirySearchGrid': paginationOptionsClaimInquirySearchGrid,
                'claimInquirySearchGridSearchCriterias': $scope.claimInquirySearchGridSearchCriterias
            }
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllClaimForClaimInquirySearchGrid',
                data: claimSearchGridParam,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                var response_arr = JSON.parse(data);
                $scope.gridOptionsClaimInquiry.data = response_arr.data;
                $scope.gridOptionsClaimInquiry.totalItems = response_arr.totalRecords;
               // clearFields();
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                $scope.claimInquirySearchGridloading = false;
                $scope.claimInquiryGridloadAttempted = true;

            });
        };

        $scope.refresClaimSearchGridData = function () {
            getClaimSearchPage();
        }


        $scope.selectClaimToInquiry = function (claimId, policyid) {
            $scope.claimId = claimId;
            if (isGuid(claimId)) {
                $scope.policyId = policyid;
                swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('pages.claimInquiry.popMessages.claimInquiryInformation'), showConfirmButton: false });
                if (typeof ClaimSelectionPopup !== 'undefined')
                    ClaimSelectionPopup.close();
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetClaimDetailsforInquiryByClaimId',
                    data: {
                        "claimId": claimId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Claim.policyNumber = data.PolicyNumber;
                    $scope.Claim.claimNumber = data.ClaimNumber;
                    $scope.Claim.currencyCode = data.CurrencyCode;
                    $scope.Claim.totalClaimAmount = data.TotalClaimAmount;
                    $scope.Claim.claimDealer = data.DealerName;
                    $scope.Claim.authorizedClaimAmount = data.AuthorizedClaimAmount;
                    $scope.Claim.complaint.customer = data.Complaint.customer;
                    $scope.Claim.complaint.dealer = data.Complaint.dealer;
                    $scope.Claim.complaint.engineer = data.Complaint.engineer;
                    $scope.Claim.complaint.conclution = data.Complaint.conclution;
                    $scope.Claim.paidAmount = data.PaidAmount;
                    $scope.Claim.entryDate = data.EntryDate;
                    $scope.Claim.approvedDate = data.ApprovedDate;
                    $scope.Claim.approvedBy = data.ApprovedBy;

                    $scope.Claim.claimDealerId = data.DealerId;
                    $scope.Claim.country = data.Country;
                    $scope.Claim.claimItemList = data.ClaimItemList;

                    $scope.claimBatch.invoiceReceivedDate = data.InvoiceReceivedDate;
                    $scope.claimBatch.invoiceDate = data.InvoiceDate;
                    $scope.claimBatch.invoiceNumber = data.InvoiceNumber;
                    $scope.claimBatch.batchNumber = data.BatchNumber;
                    $scope.claimBatch.batchEntryDate = data.BatchEntryDate;
                    $scope.claimBatch.groupName = data.GroupName;
                    $scope.claimBatch.insurer = data.Insurer;
                    $scope.claimBatch.reinsurer = data.Reinsurer;
                    $scope.claimBatch.bordxYear = data.BordxYear;
                    $scope.claimBatch.bordxmonth = data.Bordxmonth;
                    $scope.claimBatch.bordxNumber = data.BordxNumber;

                    $scope.makeId = data.MakeId;
                    $scope.modelId = data.ModelId;
                    $scope.dealerId = data.DealerId;

                    $scope.loadExcitingClaimEndorsement($scope.claimId);
                    $scope.loadPolicy($scope.policyId, true);

                    if (data.InvoceWarning != null) {
                        //customInfoMessage("No Invoice Data Found.");
                        $scope.claimBatch.invoceWarning = data.InvoceWarning
                    }
                    if (data.BatchGroupWarning != null) {
                        //customInfoMessage("No Invoice Data Found.");
                        $scope.claimBatch.batchGroupWarning = data.BatchGroupWarning
                    }
                    if (data.BordxWarning != null) {
                        //customInfoMessage("No Invoice Data Found.");
                        $scope.claimBatch.bordxWarning = data.BordxWarning
                    }

                }).error(function (data, status, headers, config) {

                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.loadExcitingClaimEndorsement = function (claimId) {
            if (isGuid(claimId)) {
                $scope.claimId = claimId;

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetExcitingClaimEndorsementDetails',
                    data: { data: claimId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data != null && data.Status === "ok") {

                        $scope.claimEndorsement.ExcitingEndoresList = data.ExcitingCliamEndrosmentData;

                    } else {
                        $scope.claimEndorsement.ExcitingEndoresList = [];
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            } else {
                customErrorMessage($filter('translate')('pages.claimInquiry.errMeassges.claimIdNotFound'));
            }
        }

        $scope.loadPolicy = function (policyId, isExistingPolicy) {
            // alert(policyId);
            if (isGuid(policyId)) {
                if (typeof PolicySelectionPopup != 'undefined')
                    PolicySelectionPopup.close();
                swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('pages.claimInquiry.popMessages.policyInformation'), showConfirmButton: false });
                $scope.policyId = policyId;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetPolicyDetailsForClaimProcess',
                    data: { data: policyId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Status === "ok") {
                        $scope.Claim.policyNumber = data.PolicyNo;
                        if (!isExistingPolicy) {
                            $scope.Claim.claimNumber = 'N/A';
                            $scope.Claim.currencyCode = '';
                            $scope.Claim.totalClaimAmount = '0.00';
                            $scope.Claim.authorizedClaimAmount = '0.00';
                        }

                        $scope.Claim.country = data.PolicyCountry;
                        //$scope.Claim.claimDealer ='';
                        $scope.Claim.policyDealer = data.PolicyDealer;
                        $scope.Claim.customerName = data.CustomerName;
                        $scope.Claim.customerTel = data.CustomerTel;
                        $scope.Claim.vinSerial = data.Serial;
                        $scope.Claim.make = data.Make;
                        $scope.Claim.model = data.Model;
                        $scope.Claim.claimDealerId = data.PolicyDealerId;

                        $scope.makeId = data.MakeId;
                        $scope.modelId = data.ModelId;


                    } else {
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            }
        }

        $scope.showClaimEndrosement = function (ClaimID, CliamEndrosmentId) {

            if (isGuid(CliamEndrosmentId)) {
                $scope.cliamEndrosmentId = CliamEndrosmentId;
                $scope.ClaimID = ClaimID;

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetOldandNewClaimEndorsementDetails',
                    data: {
                        "CliamEndrosmentId": CliamEndrosmentId,
                        "claimId": ClaimID
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.Status === "ok") {

                        if (data.oldCliamEndrosment != null) {
                            $scope.claimEndorsement.PolicyNumber = data.oldCliamEndrosment.PolicyNumber;
                            $scope.claimEndorsement.ClaimNumber = data.oldCliamEndrosment.ClaimNumber;
                            $scope.claimEndorsement.customer = data.oldCliamEndrosment.customer;
                            $scope.claimEndorsement.Serial = data.oldCliamEndrosment.Serial;
                            $scope.claimEndorsement.MakeId = data.oldCliamEndrosment.MakeId;
                            $scope.claimEndorsement.ModelId = data.oldCliamEndrosment.ModelId;
                            $scope.claimEndorsement.PolicyDealerId = data.oldCliamEndrosment.PolicyDealerId;
                            $scope.claimEndorsement.ClaimDealer = data.oldCliamEndrosment.ClaimDealer;
                            $scope.claimEndorsement.TotalClaimAmount = data.oldCliamEndrosment.TotalClaimAmount;
                            $scope.claimEndorsement.TotalGrossAmount = data.oldCliamEndrosment.TotalGrossAmount;
                            $scope.claimEndorsement.PaidAmount = data.oldCliamEndrosment.PaidAmount;
                            $scope.claimEndorsement.ClaimItemList = data.oldCliamEndrosment.ClaimItemList;
                            $scope.claimEndorsement.complaint.customer = data.oldCliamEndrosment.Complaint.customer;
                            $scope.claimEndorsement.complaint.dealer = data.oldCliamEndrosment.Complaint.dealer;
                            $scope.claimEndorsement.complaint.engineer = data.oldCliamEndrosment.Complaint.engineer;
                            $scope.claimEndorsement.complaint.conclution = data.oldCliamEndrosment.Complaint.conclution;
                        } else {
                            $scope.claimEndorsement.PolicyNumber = "";
                            $scope.claimEndorsement.ClaimNumber = "";
                            $scope.claimEndorsement.customer = "";
                            $scope.claimEndorsement.Serial = "";
                            $scope.claimEndorsement.MakeId = "";
                            $scope.claimEndorsement.ModelId = "";
                            $scope.claimEndorsement.PolicyDealerId = "";
                            $scope.claimEndorsement.ClaimDealer = "";
                            $scope.claimEndorsement.TotalClaimAmount = "";
                            $scope.claimEndorsement.TotalGrossAmount = "";
                            $scope.claimEndorsement.PaidAmount = "";
                            $scope.claimEndorsement.ClaimItemList = "";
                            $scope.claimEndorsement.complaint.customer = "";
                            $scope.claimEndorsement.complaint.dealer = "";
                            $scope.claimEndorsement.complaint.engineer = "";
                            $scope.claimEndorsement.complaint.conclution = "";
                        }

                        if (data.newCliamEndrosment != null) {
                            $scope.newClaimEndorsement.PolicyNumber = data.newCliamEndrosment.PolicyNumber;
                            $scope.newClaimEndorsement.ClaimNumber = data.newCliamEndrosment.ClaimNumber;
                            $scope.newClaimEndorsement.customer = data.newCliamEndrosment.customer;
                            $scope.newClaimEndorsement.Serial = data.newCliamEndrosment.Serial;
                            $scope.newClaimEndorsement.MakeId = data.newCliamEndrosment.MakeId;
                            $scope.newClaimEndorsement.ModelId = data.newCliamEndrosment.ModelId;
                            $scope.newClaimEndorsement.PolicyDealerId = data.newCliamEndrosment.PolicyDealerId;
                            $scope.newClaimEndorsement.ClaimDealer = data.newCliamEndrosment.ClaimDealer;
                            $scope.newClaimEndorsement.TotalClaimAmount = data.newCliamEndrosment.TotalClaimAmount;
                            $scope.newClaimEndorsement.TotalGrossAmount = data.newCliamEndrosment.TotalGrossAmount;
                            $scope.newClaimEndorsement.PaidAmount = data.newCliamEndrosment.PaidAmount;
                            $scope.newClaimEndorsement.ClaimItemList = data.newCliamEndrosment.ClaimItemList;
                            $scope.newClaimEndorsement.complaint.customer = data.Complaint.customer;
                            $scope.newClaimEndorsement.complaint.dealer = data.newCliamEndrosment.Complaint.dealer;
                            $scope.newClaimEndorsement.complaint.engineer = data.newCliamEndrosment.Complaint.engineer;
                            $scope.newClaimEndorsement.complaint.conclution = data.newCliamEndrosment.Complaint.conclution;
                        } else {
                            $scope.newClaimEndorsement.PolicyNumber = "";
                            $scope.newClaimEndorsement.ClaimNumber = "";
                            $scope.newClaimEndorsement.customer = "";
                            $scope.newClaimEndorsement.Serial = "";
                            $scope.newClaimEndorsement.MakeId = "";
                            $scope.newClaimEndorsement.ModelId = "";
                            $scope.newClaimEndorsement.PolicyDealerId = "";
                            $scope.newClaimEndorsement.ClaimDealer = "";
                            $scope.newClaimEndorsement.TotalClaimAmount = "";
                            $scope.newClaimEndorsement.TotalGrossAmount = "";
                            $scope.newClaimEndorsement.PaidAmount = "";
                            $scope.newClaimEndorsement.ClaimItemList = "";
                            $scope.newClaimEndorsement.complaint.customer = "";
                            $scope.newClaimEndorsement.complaint.dealer = "";
                            $scope.newClaimEndorsement.complaint.engineer = "";
                            $scope.newClaimEndorsement.complaint.conclution = "";
                        }




                    } else {
                        customErrorMessage(data.Status);
                    }

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });

            } else {
                customErrorMessage($filter('translate')('pages.claimInquiry.errMeassges.claimIdNotFound'));
            }

        }

    });