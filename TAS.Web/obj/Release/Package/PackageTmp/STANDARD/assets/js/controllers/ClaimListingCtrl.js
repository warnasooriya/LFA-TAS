
app.controller('ClaimListingCtrl',
    function ($scope, $rootScope, $http, ngDialog, $location, SweetAlert, $localStorage,
        $cookieStore, $filter, toaster, $state, ngTableParams, $stateParams, $translate) {
        $scope.userType = "";
        $scope.claimId = $stateParams.claimId;
        $scope.isrefnovalid = false;
        $scope.claimItemList2 = [];
        var ClaimViewPopup, RejectClaimRequestPopup, ClaimHistoryPopup, PopupAssesmentHistory, PopupServiceHistory, PopupViewAttachments;
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
            DealerCode: '',
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
        $scope.claimDealers = [];
        $scope.claimMakes = [];
        $scope.claimStatus = [];
        $scope.claimRejectionTypes = [];
        $scope.dashbordClaims = [];
        $scope.isProductTire = false;
        $scope.isProductIloe = false;
        $scope.userType = '';
        $scope.temporaryNumberlan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.temporaryNumber');
        $scope.claimNolan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.claimNo');
        $scope.amountNolan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.amount');
        $scope.totalPriceNolan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.totalPrice');
        $scope.loanInstallmentlan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.loanInstallment');
        $scope.itemNamelan = $filter('translate')('pages.claimListing.popUpViewClaimRequest.itemName');

        $scope.loadInitailData = function () {

            $http({
                method: 'POST',
                url: '/TAS.Web/api/PolicyReg/GetDealerAccessByUserId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId },
            }).success(function (data, status, headers, config) {
                if (data == '' || data == null) {
                    swal({ title: $filter('translate')('common.redirectingToDashboard'), text: $filter('translate')('common.errMessage.dontHavePermission'), showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 5000);
                    $state.go('app.dashboard');
                } else {
                    if (data == 'NoMapping') {
                        swal({ title: $filter('translate')('common.redirectingToDashboard'), text: $filter('translate')('pages.claimListing.errMessage.notAssignAnyDealer'), showConfirmButton: false });
                        setTimeout(function () { swal.close(); }, 5000);
                        $state.go('app.dashboard');
                    } else if (data == 'Internal') {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.claimDealers = data;
                        }).error(function (data, status, headers, config) {
                        });
                    } else {
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/DealerManagement/GetAllDealersByUserId',
                            data: { "Id": $localStorage.LoggedInUserId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            $scope.claimDealers = data.Dealers;
                            //$scope.claimSearch.claimDealerId = data.Dealers[0].Id;
                        }).error(function (data, status, headers, config) {
                        });
                    }
                }
                //$scope.countries = data;
            }).error(function (data, status, headers, config) {
                });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.commodityTypes = data;


                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "Id": $scope.commodityTypes[0].CommodityTypeId }
                }).success(function (data, status, headers, config) {
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
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimMakes = data;
            }).error(function (data, status, headers, config) {
            });

            

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllClaimStatus',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {


                angular.forEach(data, function (value) {
                    value.Description = $scope.GetStatusByCode(value.StatusCode);

                });

                $scope.claimStatus = data;

            }).error(function (data, status, headers, config) {
            });



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
                    swal({ title: $filter('translate')('common.securityInformation'), text: data.status, showConfirmButton: false });
                    setTimeout(function () { swal.close(); }, 8000);
                    $state.go('app.dashboard');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllClaimRejectionTypes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.claimRejectionTypes = data;
            }).error(function (data, status, headers, config) {
            }).finally(function () {
            });


            if (isGuid($scope.claimId)) {
                $scope.viewClaim($scope.claimId);
            }
        }

        $scope.downloadAuthorizationForm = function (claimId) {
            if (isGuid(claimId)) {

                if ($scope.Products[0].Productcode != "TYRE") {

                    swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('common.preparingDownload'), showConfirmButton: false });
                    var data = {
                        "loggedInUserId": $localStorage.LoggedInUserId,
                        "claimId": claimId
                    }

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/DownloadClaimAuthorizationForm',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: data,
                    }).success(function (datar, status, headers, config) {
                        //alert(datar);
                        if (typeof datar !== "undefined" && datar.length > 0) {
                            var url = $location.protocol() + '://' + $location.host() +
                                '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + $localStorage.jwt;
                            // alert(url);
                            window.open(url, '_blank')
                            //var publicurl = $location.protocol() + '://' + $location.host() + '/TAS.Web/contisurewarranty.pdf';
                            //window.open(publicurl, '_blank')
                            swal.close();
                        } else {
                            customErrorMessage($filter('translate')('common.errMessage.somethingWentWrong'));
                            swal.close();
                        }
                    }).error(function (data, status, headers, config) {
                    });
                } else {
                    swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('common.preparingDownload'), showConfirmButton: false });
                    var data = {
                        "loggedInUserId": $localStorage.LoggedInUserId,
                        "claimId": claimId
                    }

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/claim/DownloadClaimAuthorizationFormforTYER',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: data,
                    }).success(function (datar, status, headers, config) {
                        //alert(datar);
                        if (typeof datar !== "undefined" && datar.length > 0) {
                            var url = $location.protocol() + '://' + $location.host() +
                                '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + $localStorage.jwt;
                            // alert(url);
                            window.open(url, '_blank')
                            //var publicurl = $location.protocol() + '://' + $location.host() + '/TAS.Web/contisurewarranty.pdf';
                            //window.open(publicurl, '_blank')
                            swal.close();
                        } else {
                            customErrorMessage($filter('translate')('common.errMessage.somethingWentWrong'));
                            swal.close();
                        }
                    }).error(function (data, status, headers, config) {
                    });
                }
            } else
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
        }

        $scope.refinepolicySearch = function () {
            $scope.claimTable.data = [];
            $scope.claimTable.reload();
        }

        $scope.resetpolicySearch = function () {
            $scope.claimSearch.commodityTypeId = emptyGuid();
            $scope.claimSearch.policyNo = '';
            $scope.claimSearch.claimNo = '';
            $scope.claimSearch.vinNo = '';
            $scope.claimSearch.claimDealerId = emptyGuid();
            $scope.claimSearch.makeId = emptyGuid();
            $scope.claimSearch.statusId = emptyGuid();
            $scope.refinepolicySearch();
        }

        $scope.$on('doRefreshClaimList', function (event, data) {
            $scope.claimTable.data = [];
            $scope.claimTable.reload();
        });

        $scope.refreshClaimList = function () {
            $scope.claimTable.data = [];
            $scope.claimTable.reload();
        }


        $scope.testWebSocket = function () {
        }
        $scope.sendMsg = function () {

        }

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
                            url: '/TAS.Web/api/claim/GetAllSubmittedClaimsByUserId',
                            data: search,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var i = 0;
                            var responseArr = JSON.parse(data);
                            if (responseArr != null) {
                                angular.forEach(responseArr.data, function (value) {
                                    value.Status = $scope.GetStatusByCode(value.Status);
                                    if ($scope.userType == "DU" && value.isEndorsed) {

                                        delete responseArr.data[i];
                                    }

                                    i++;
                                });
                                params.total(responseArr.totalRecords);
                                $defer.resolve(responseArr.data);
                            } else {
                                customErrorMessage($filter('translate')('pages.claimListing.errMessage.noClaimRequest'));
                            }
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            $scope.policyGridloading = false;
                            $scope.policyGridloadAttempted = true;
                        });
                    }
                }
            });

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
                    case "INP":
                        return "In Progress";
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

        //$scope.navigateToProcessClaim = function () {
        //    var claimId = $scope.claimView.id;
        //    if (isGuid(claimId)) {
        //        ClaimViewPopup.close();
        //        $location.path("app/claim/process/" + claimId);
        //        $route.reload();
        //    } else {
        //        customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
        //    }
        //}
        $scope.viewClaim = function (claimReqId, make, model, dealer, date, claimStatus) {
            if (isGuid(claimReqId)) {
                swal({ html: true, title: '<h2 class="saving">' + $filter('translate')('common.loading') + '<span>.</span><span>.</span><span>.</span></h2>', text: $filter('translate')('pages.claimListing.messages.readingClaims'), showConfirmButton: false });
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

                    $scope.claimView.policyNumber = data.PolicyNumber;
                    $scope.claimView.claimNumber = (data.ClaimNumber == '') ? 'N/A' : data.ClaimNumber;
                    if (data.ClaimNumber == "") {
                        $scope.claimView.claimNumber = data.Wip;
                        $scope.isrefnovalid = true;

                    }
                    else {

                        $scope.isrefnovalid = false;
                    }
                    $scope.claimView.totalClaimAmount = data.TotalClaimAmount + ' ' + data.CurrencyCode;
                    $scope.claimView.currencyCode = data.CurrencyCode;

                    //$scope.claimView.make = make;
                    //$scope.claimView.model = model;
                    //$scope.claimView.dealer = dealer;
                    //$scope.claimView.date = date;

                    $scope.claimView.country = data.Country;
                    $scope.claimView.claimStatusCode = data.ClaimStatus;

                    if ($localStorage.CommodityType == "Tyre") {
                        $scope.claimView.claimItemList = data.ClaimItemList;
                        angular.forEach($scope.claimView.claimItemList, function (it) {

                            it.itemNumber = 'C' + it.itemNumber + '0000';
                        });

                    } else {
                        $scope.claimView.claimItemList = data.ClaimItemList;
                        angular.forEach($scope.claimView.claimItemList, function (claimItem) {

                            angular.forEach($scope.claimView.claimItemList, function (claimItem2) {

                                if (claimItem.parentId == claimItem2.partId) {
                                    claimItem.parentId2 = claimItem2.itemNumber;
                                }

                            });

                        });
                        var i = 0;
                        angular.forEach($scope.claimView.claimItemList, function (claimItem) {

                            if (claimItem.itemType != 'L') {
                                $scope.claimItemList2[i] = claimItem;
                                i++;
                                angular.forEach($scope.claimView.claimItemList, function (claimItem2) {
                                    if (claimItem2.parentId == $scope.claimItemList2[i - 1].partId) {
                                        $scope.claimItemList2[i] = claimItem2;
                                        i++;
                                    }
                                });
                            }
                        })
                        $scope.claimView.claimItemList = $scope.claimItemList2;
                    }

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
                    $scope.claimView.DealerCode = data.DealerCode;


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
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidClaimRequest'));
            }
        }

        $scope.ViewClaimUpdateStatus = function (claimReqId) {


            if (isGuid(claimReqId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ViewClaimUpdateStatus',
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
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.selectValidClaim'));
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

        $scope.downloadAttachmentUploaded = function (ref) {
            if (ref != '') {
                swal({ title: $filter('translate')('common.processing'), text: $filter('translate')('common.preparingDownload'), showConfirmButton: false });
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

        $scope.editClaim = function (claimId) {
            //  alert(claimId);
            if (isGuid(claimId)) {
                if ($scope.claimView.claimStatusCode == 'SUB'
                    || $scope.claimView.claimStatusCode == 'REQ') {

                    //  $location.path("app/claim/edit/" + claimId);
                    //$localStorage.claimId = claimId;
                    $scope.ViewClaimUpdateStatus(claimId);
                    $rootScope.claimId = claimId;
                    $state.go('app.claimsubmission', { claimId: claimId });


                    if (typeof ClaimViewPopup != 'undefined')
                        ClaimViewPopup.close();
                } else {
                    customErrorMessage($filter('translate')('pages.claimListing.errMessage.selectedClaimCantEdit'));
                }
            } else {
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
            }

        }
        $scope.goToProcessClaim = function () {
            var claimId = $scope.claimView.id;

            if ($scope.Products[0].ProductTypeCode == "ILOE") {
                if (isGuid(claimId)) {

                    swal({
                        title: "",
                        text: $filter('translate')('pages.claimListing.confirmatiomMessage.youwantProceesClaim'),
                        type: "info",
                        showCancelButton: true,
                        closeOnConfirm: true
                        // showLoaderOnConfirm: true,
                    },
                        function () {
                            //  alert(claimId);
                            // $location.path("app/claim/process/" + claimId);
                            if (typeof ClaimViewPopup != 'undefined')
                                ClaimViewPopup.close();

                            $state.go('app.claimprocessbank', { claimId: claimId });
                        });
                } else {
                    customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
                }
            } else {
                if (isGuid(claimId)) {

                    swal({
                        title: "",
                        text: $filter('translate')('pages.claimListing.confirmatiomMessage.youwantProceesClaim'),
                        type: "info",
                        showCancelButton: true,
                        closeOnConfirm: true
                        // showLoaderOnConfirm: true,
                    },
                        function () {
                            //  alert(claimId);
                            // $location.path("app/claim/process/" + claimId);
                            if (typeof ClaimViewPopup != 'undefined')
                                ClaimViewPopup.close();

                            $state.go('app.claimprocess', { claimId: claimId });
                        });
                } else {
                    customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
                }
            }

        }
        $scope.loadClaimRequestById = function (claimId) {
            if (isGuid(claimId)) {
                if (typeof ClaimViewPopup != 'undefined')
                    ClaimViewPopup.close();
                $scope.viewClaim(claimId);
            }
        }
        $scope.rejectClaimView = function () {
            var claimId = $scope.claimView.id;
            if (isGuid(claimId)) {
                //if (typeof ClaimViewPopup != 'undefined')
                //    ClaimViewPopup.close();
                swal({
                    title: "",
                    text: $filter('translate')('pages.claimListing.confirmatiomMessage.youWnatRejectClaim'),
                    type: "info",
                    confirmButtonText: $filter('translate')('common.button.yes'),
                    cancelButtonText: $filter('translate')('common.button.no'),
                    showCancelButton: true,
                    closeOnConfirm: true
                    // showLoaderOnConfirm: true,
                },
                    function () {
                        RejectClaimRequestPopup = ngDialog.open({
                            template: 'popUpRejectClaimRequest',
                            className: 'ngdialog-theme-plain',
                            closeByEscape: true,
                            showClose: true,
                            closeByDocument: true,
                            scope: $scope
                        });
                    });
            } else
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
        }
        $scope.rejectClaim = function () {
            if (isGuid($scope.claimView.claimRejectionTypeId)) {
                if ($scope.claimView.claimRejectionComment.trim().length != 0) {
                    if (isGuid($scope.claimView.id)) {
                        swal({ html: true, title: '<h2 class="saving">' + $filter('translate')('common.processing') + '<span>.</span><span>.</span><span>.</span></h2>', text: $filter('translate')('pages.claimListing.messages.rejectingClaims'), showConfirmButton: false });
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/claim/RejectClaimRequest',
                            data: {
                                "claimId": $scope.claimView.id,
                                "rejectionTypeId": $scope.claimView.claimRejectionTypeId,
                                "rejectionComment": $scope.claimView.claimRejectionComment,
                                "loggedInUserId": $localStorage.LoggedInUserId
                            },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data.code === "OK") {
                                customInfoMessage($filter('translate')('pages.claimListing.messages.claimRejectionSuccess'))
                                if (typeof ClaimViewPopup != 'undefined')
                                    ClaimViewPopup.close();
                                $scope.refreshClaimList();
                                $scope.claimView.claimRejectionTypeId = emptyGuid();
                                $scope.claimView.claimRejectionComment = '';


                            } else {
                                customErrorMessage(data.msg);
                            }
                            RejectClaimRequestPopup.close();
                            swal.close();

                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            //swal.close();
                        });
                    } else
                        customErrorMessage($filter('translate')('pages.claimListing.errMessage.selectValidClaim'));
                } else
                    customErrorMessage($filter('translate')('pages.claimListing.errMessage.enterRejectionComment'));
            } else
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.selectRejectionType'));
        }

        $scope.informationRequestView = function () {
            var claimId = $scope.claimView.id;
            if (isGuid(claimId)) {

                swal({
                    title: "",
                    text: $filter('translate')('pages.claimListing.messages.writeRequiredInformation'),
                    //type: "input",
                    //"<textarea id='infoMsg'></textarea>"
                    text:
                        "<div class='form-group'><label for='comment'>" + $filter('translate')('pages.claimListing.messages.requiredInformation') + ":</label><textarea class='form-control' rows='5' id='infoMsg'></textarea></div>",
                    html: true,
                    showCancelButton: true,
                    closeOnConfirm: false

                }, function (inputValue) {
                    var info = document.getElementById('infoMsg').value;
                    // alert(inputValue);
                    if (inputValue === false) return false;
                    if (inputValue === "" || info.length === 0) {
                        swal.showInputError($filter('translate')('pages.claimListing.errMessage.requiredInformation'));
                        return false
                    }
                    // swal("Nice!", "You wrote: " + inputValue, "success");

                    $scope.requestClaimInformation(info);
                    if (typeof ClaimViewPopup != 'undefined')
                        ClaimViewPopup.close();
                });

            } else
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
        }
        $scope.requestClaimInformation = function (requiredInformation) {
            var claimId = $scope.claimView.id;
            if (isGuid(claimId)) {
                swal({
                    html: true, title: '<h2 class="saving">' + $filter('translate')('common.processing') + '<span>.</span><span>.</span><span>.</span></h2>',
                    text: $filter('translate')('pages.claimListing.messages.savingClaim'), showConfirmButton: false
                });
                var data = {
                    claimId: claimId,
                    informationMsg: requiredInformation,
                    loggedInUserId: $localStorage.LoggedInUserId
                }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/ClaimInformationRequest',
                    data: { 'claimInfoRequest': data },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === "success") {
                        customInfoMessage($filter('translate')('pages.claimListing.messages.claimInformationRequestSuccess'));
                    } else
                        customErrorMessage(data.msg);

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });
            } else
                customErrorMessage($filter('translate')('pages.claimListing.errMessage.invalidCalim'));
        }
        $scope.getClaimDetailsByPolicyId = function (type) {
            if (isGuid($scope.claimView.policyId)) {
                swal({ html: true, title: '<h2 class="saving">' + $filter('translate')('common.loading') + '<span>.</span><span>.</span><span>.</span></h2>', text: 'Reading claim history', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetClaimDetailsByPolicyIdAndClaimId',
                    data: { "policyId": $scope.claimView.policyId, "claimId": $scope.claimView.id, "type": type },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

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
            }
        }

        $scope.showClaimHistory = function () {

            swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('common.readingInformation'), showConfirmButton: false });
            var data = {
                claimId: emptyGuid(),
                policyId: emptyGuid()
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

                $scope.claimHistory = data.ClaimData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });
        }

        $scope.showAssesmentHistory = function () {
            swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('common.readingInformation'), showConfirmButton: false });

            var data = {
                claimId: emptyGuid(),
                policyId: emptyGuid()
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

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

        $scope.showServiceHistory = function () {
            swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('common.readingInformation'), showConfirmButton: false });

            var policyId = emptyGuid();

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllServiceHistoryByPolicyId',
                data: { "policyId": policyId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                PopupServiceHistory = ngDialog.open({
                    template: 'popUpServiceHistory',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.serviceHistory = data.AssessmentData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

        $scope.showViewAttachments = function () {
            swal({ title: $filter('translate')('common.loading'), text: $filter('translate')('common.readingInformation'), showConfirmButton: false });

            var data = {
                claimId: emptyGuid(),
                policyId: emptyGuid()
            }

            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetClaimDetailsForView',
                data: data,
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                PopupViewAttachments = ngDialog.open({
                    template: 'popUpViewAttachments',
                    className: 'ngdialog-theme-plain',
                    closeByEscape: true,
                    showClose: true,
                    closeByDocument: true,
                    scope: $scope
                });

                $scope.viewAttachments = data.AssessmentData;

            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

        $scope.uDTValueCalculation = function () {
            $scope.CalculateUDTvalues = '';
            var data = {
                "claimId": $scope.claimView.id === '' ? emptyGuid() : $scope.claimView.id,
                "policyId": typeof $scope.claimView.policyId === 'undefined' ? emptyGuid() : $scope.claimView.policyId,
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

                if ($scope.claimView.claimStatusCode == "REJ") {
                    $scope.customertopayhide = true;
                    $scope.CalculateUDTvalues.OriginalPercentageValue = 0;
                }

            }).error(function (data, status, headers, config) {

            }).finally(function () {
                swal.close();
            });

        }

        $scope.showCalculationPopup = function (itemId) {
            console.log(itemId);
            if (parseInt(itemId)) {
                angular.forEach($scope.claimView.claimItemList, function (claimItem) {
                    if (claimItem.id === itemId) {
                        $scope.partId = claimItem.partId;
                    }
                });
                $scope.uDTValueCalculation();
            }

            AddNewCalculationPopup = ngDialog.open({
                template: 'popUpCalcuationUDT',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope,

            });
        }


        $scope.searchDashBoardPopup = function () {

            $scope.dashBoardClaims();


            ClaimPopup = ngDialog.open({
                template: 'popUpDashBoardClaims',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        };

        $scope.dashBoardClaims = function () {
            $scope.dashbordClaims = [];
            $http({
                method: 'POST',
                url: '/TAS.Web/api/claim/GetAllClaimDashboardStatus',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data == null) customErrorMessage($filter('translate')('pages.claimListing.errMessage.noClaimsFound'));
                angular.forEach(data.data, function (value) {
                    $scope.dashbordClaims.push(value);

                });
                //$scope.claims.push(data);



            }).error(function (data, status, headers, config) {
            });
        }
        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('pages.claimListing.error'), msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', $filter('translate')('pages.claimListing.information'), msg, 12000);
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