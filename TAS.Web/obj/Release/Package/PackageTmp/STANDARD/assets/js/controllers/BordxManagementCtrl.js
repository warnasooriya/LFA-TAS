app.controller('BordxManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, ngTableParams, toaster) {
        $scope.ModalName = "Bordereaux Process Management";
        $scope.ModalDescription = "Create Bordereaux";
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };
        $scope.Years = [];
        $scope.Months = [];
        $scope.bordx = {
            countryId: emptyGuid(),
            commodityTypeId: emptyGuid()
        };
        $scope.newBordx = {};
        $scope.bordxProcess = {
            commodityTypeId: emptyGuid(),
            countryId: emptyGuid()
        };
        $scope.last10Bordx = [];
        $scope.policyTableData = 0;
        var BordxChangePopup;
        $scope.errorTransferPolicy = "";

        $scope.loadInitailData = function () {
            $scope.getLast10Bordx();
            $http({
                method: 'POST',
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.reinsurers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/InsurerManagement/GetAllInsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.insurers = data;
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
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
            }).error(function (data, status, headers, config) {
            });

        }

        $scope.GetBordxAllowedYearsAndMonths = function () {
            if (isGuid($scope.bordx.insurerId) && isGuid($scope.bordx.reinsurerId) && isGuid($scope.bordx.commodityTypeId)) {
                $scope.Years = [];
                $scope.Months = [];
                $scope.bordx.year = '';
                $scope.bordx.month = '';
                $http({
                    method: 'post',
                    url: '/tas.web/api/Bordx/getAllBordxAllowedYearsMonths',
                    data: JSON.stringify({ 'insurerId': $scope.bordx.insurerId, 'reinsurerId': $scope.bordx.reinsurerId ,'commodityTypeId': $scope.bordx.commodityTypeId }),
                    headers: { 'authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Years = data.years;
                    $scope.Months = data.months;
                    $scope.bordx.year = data.years[1];

                    for (var i = 0; i < data.months.length; i++) {
                        if (data.months[i].isDefault == true) {
                            $scope.bordx.month = data.months[i].monthsSeq;

                        }
                    }


                    $scope.loadBordxNumber();
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.selectableEndDate;
        $scope.selectableMinDate;
        $scope.getMaxDate = function (startDate) {
            var month = $scope.bordx.month + '';
            var startDate = $scope.bordx.year + '-' + month.padStart(2, '0') + '-' + '01';
            var ldate = new Date(startDate);
            var lastDayOfMonth = new Date(ldate.getFullYear(), ldate.getMonth() + 1, 0);
            var sed = lastDayOfMonth.getDate() + '';
            $scope.selectableEndDate = $scope.bordx.year + '-' + month.padStart(2, '0') + '-' + sed.padStart(2, '0');
            $scope.selectableMinDate = $scope.bordx.StartDate;
            return startDate;
        }


        $scope.loadStartDateAndEndDates = function () {
            var month = $scope.bordx.month + '';
            var startDate = $scope.bordx.year + '-' + month.padStart(2, '0') + '-' + '01';
            var ldate = new Date(startDate);
            var lastDayOfMonth = new Date(ldate.getFullYear(), ldate.getMonth() + 1, 0);
            var sed = lastDayOfMonth.getDate() + '';
            $scope.selectableEndDate = $scope.bordx.year + '-' + month.padStart(2, '0') + '-' + sed.padStart(2, '0');
            $scope.bordx.StartDate = startDate;
            $scope.bordx.EndDate = $scope.selectableEndDate;
        }


        $scope.GetBordxAllowedYearsAndMonthsProcess = function () {
            if (isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId) && isGuid($scope.bordxProcess.commodityTypeId)) {
                $scope.YearsProcess = [];
                $scope.MonthsProcess = [];
                $scope.bordxProcess.year = '';
                $scope.bordxProcess.month = '';
                $http({
                    method: 'post',
                    url: '/tas.web/api/Bordx/getAllBordxAllowedYearsMonths',
                    data: JSON.stringify({ 'insurerId': $scope.bordxProcess.insurerId, 'reinsurerId': $scope.bordxProcess.reinsurerId, 'commodityTypeId': $scope.bordxProcess.commodityTypeId }),
                    headers: { 'authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.YearsProcess = data.years;
                    $scope.MonthsProcess = data.months;
                    $scope.bordxProcess.year = data.years[1];

                    for (var i = 0; i < data.months.length; i++) {
                        if (data.months[i].isDefault == true) {
                            $scope.bordxProcess.month = data.months[i].monthsSeq;

                        }
                    }
                    $scope.loadBordxNumbers();
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.GetBordxAllowedYearsAndMonthsTransfer = function () {
            if (isGuid($scope.newBordx.insurerId) && isGuid($scope.newBordx.reinsurerId) && isGuid($scope.newBordx.commodityTypeId)) {
                $scope.YearsTransfer = [];
                $scope.MonthsTransfer = [];
                $scope.newBordx.year = '';
                $scope.newBordx.month = '';
                $http({
                    method: 'post',
                    url: '/tas.web/api/Bordx/getAllBordxAllowedYearsMonths',
                    data: JSON.stringify({ 'insurerId': $scope.newBordx.insurerId, 'reinsurerId': $scope.newBordx.reinsurerId, 'commodityTypeId': $scope.newBordx.commodityTypeId }),
                    headers: { 'authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.YearsTransfer = data.years;
                    $scope.MonthsTransfer = data.months;
                    $scope.newBordx.year = data.years[1];

                    for (var i = 0; i < data.months.length; i++) {
                        if (data.months[i].isDefault == true) {
                            $scope.newBordx.month = data.months[i].monthsSeq;

                        }
                    }
                    $scope.getNewBordxNumbers();
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.policyTable = new ngTableParams({
            page: 1,
            count: 10,
        }, {
                getData: function ($defer, params) {


                    if (parseInt($scope.bordxProcess.year) && parseInt($scope.bordxProcess.month) && parseInt($scope.bordxProcess.month) > 0
                        && parseInt($scope.bordxProcess.number) && parseInt($scope.bordxProcess.number) > 0 &&
                        isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId) && isGuid($scope.bordxProcess.commodityTypeId)) {
                       // swal({ title: 'TAS Information', text: 'Loading Policies to Bordereaux Process.', showConfirmButton: false });
                        $scope.policyGridloading = true;
                        $scope.policyGridloadAttempted = false;
                        var page = params.page();
                        var size = params.count();
                        var search = {
                            page: page,
                            pageSize: size,
                            year: $scope.bordxProcess.year,
                            month: $scope.bordxProcess.month,
                            number: $scope.bordxProcess.number,
                            insurerId: $scope.bordxProcess.insurerId,
                            reinsurerId:$scope.bordxProcess.reinsurerId,
                            //countryId: $scope.bordxProcess.countryId,
                            commodityTypeId: $scope.bordxProcess.commodityTypeId

                        }

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Bordx/GetAllBordxDetailsByYearMonth',
                            data: search,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            var response_arr = JSON.parse(data);
                            params.total(response_arr.totalRecords);
                            $scope.policyTableData = response_arr.totalRecords;
                            $defer.resolve(response_arr.data);
                        }).error(function (data, status, headers, config) {
                        }).finally(function () {
                            $scope.policyGridloading = false;
                            $scope.policyGridloadAttempted = true;
                           // swal.close();
                        });
                    } else {

                        //customErrorMessage("Invalid bordx year/month selection");
                        //$scope.policyTable.data = [];
                        //$scope.policyTable.reload();
                        //$defer.resolve($scope.policyTable.data);
                    }
                }
            });

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        $scope.transferPolicy = function (policyId, PolicyNo) {
            BordxChangePopup = ngDialog.open({
                template: 'popUpBordxChange',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            $scope.bordxForTransfer = PolicyNo;
            $scope.policyIdForTransfer = policyId;
            $scope.newBordx.commodityTypeId = $scope.bordxProcess.commodityTypeId;
            $scope.newBordx.countryId = $scope.bordxProcess.countryId;
            $scope.GetBordxAllowedYearsAndMonthsTransfer();
        }

        $scope.loadBordxDetails = function () {

            if (parseInt($scope.bordxProcess.year) && parseInt($scope.bordxProcess.month) &&
                parseInt($scope.bordxProcess.month) > 0 && parseInt($scope.bordxProcess.number) && parseInt($scope.bordxProcess.number) > 0 &&
                isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId) && isGuid($scope.bordxProcess.commodityTypeId)) {
                $scope.policyTable.data = [];
                $scope.policyTable.reload();
            } else {

                customErrorMessage("Invalid bordereaux details selection");

            }
            return false;
        }


        $scope.processBordx = function () {
            if (parseInt($scope.bordxProcess.year) && parseInt($scope.bordxProcess.month) && parseInt($scope.bordxProcess.month) > 0
                && parseInt($scope.bordxProcess.number) && parseInt($scope.bordxProcess.number) > 0 &&
                isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId) && isGuid($scope.bordxProcess.commodityTypeId)) {
                if ($scope.policyTable.data.length > 0) {
                    swal({
                        title: "Are you sure?",
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, Process it!",
                        cancelButtonText: "No, cancel!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });
                            var data = {
                                year: $scope.bordxProcess.year,
                                month: $scope.bordxProcess.month,
                                number: $scope.bordxProcess.number,
                                userId: $localStorage.LoggedInUserId,
                                //countryId: $scope.bordxProcess.countryId,
                                'insurerId': $scope.bordxProcess.insurerId,
                                'reinsurerId': $scope.bordxProcess.reinsurerId,
                                commodityTypeId: $scope.bordxProcess.commodityTypeId
                            }
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Bordx/ProcessBordx',
                                data: data,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == 'successful') {
                                    swal({ title: "TAS Information", text: "Bordereaux processed successfully.", showConfirmButton: true });
                                    $scope.policyTable.data = [];
                                    $scope.policyTable.reload();
                                } else {
                                    swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                                }
                            }).error(function (data, status, headers, config) {
                                swal.close();
                            }).finally(function () {

                            });
                        }
                    });
                } else {
                    customErrorMessage("No policies found for selected bordereaux");
                }
            } else {

                customErrorMessage("Please select a bordereaux for process");

            }
        }

        $scope.confirmBordx = function () {
            if (parseInt($scope.bordxProcess.year) && parseInt($scope.bordxProcess.month) && parseInt($scope.bordxProcess.month) > 0
                && parseInt($scope.bordxProcess.number) && parseInt($scope.bordxProcess.number) > 0 &&
                isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId) && isGuid($scope.bordxProcess.commodityTypeId)) {
                if ($scope.policyTable.data.length > 0) {
                    swal({
                        title: "Are you sure?",
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, Confirm it!",
                        cancelButtonText: "No, cancel!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            swal({ title: 'Please Wait', text: 'Confirming Bordereaux...', showConfirmButton: false });
                            var data = {
                                year: $scope.bordxProcess.year,
                                month: $scope.bordxProcess.month,
                                number: $scope.bordxProcess.number,
                                userId: $localStorage.LoggedInUserId,
                                //countryId: $scope.bordxProcess.countryId,
                                'insurerId': $scope.bordxProcess.insurerId,
                                'reinsurerId': $scope.bordxProcess.reinsurerId,
                                commodityTypeId: $scope.bordxProcess.commodityTypeId
                            }
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Bordx/ConfirmBordx',
                                data: data,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == 'successful') {
                                    swal({ title: "TAS Information", text: "Bordereaux processed successfully.", showConfirmButton: true });
                                    $scope.policyTable.data = [];
                                    $scope.policyTable.reload();
                                } else {
                                    swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                                }
                            }).error(function (data, status, headers, config) {
                                swal.close();
                            }).finally(function () {

                            });
                        }
                    });
                } else {
                    customErrorMessage("No policies found for selected bordereaux");
                }
            } else {

                customErrorMessage("Please select a bordereaux for process");

            }
        }

        $scope.transferPolicyToNewBordx = function () {
            $scope.errorTransferPolicy = "";

            if (parseInt($scope.newBordx.year) && parseInt($scope.newBordx.month) &&
                parseInt($scope.newBordx.month) > 0 && parseInt($scope.newBordx.number) > 0 &&
                parseInt($scope.newBordx.number) > 0 && isGuid($scope.newBordx.insurerId) &&
                isGuid($scope.newBordx.reinsurerId) &&
                isGuid($scope.newBordx.commodityTypeId)) {
                if (isGuid($scope.policyIdForTransfer)) {
                    BordxChangePopup.close();
                    swal({
                        title: "Are you sure?",
                        text: "",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, Transfer it!",
                        cancelButtonText: "No, cancel!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    }, function (isConfirm) {
                        if (isConfirm) {
                            swal({ title: 'Please Wait', text: 'Changing Bordereaux...', showConfirmButton: false });
                            var data = {
                                year: $scope.newBordx.year,
                                month: $scope.newBordx.month,
                                number: $scope.newBordx.number,
                                userId: $localStorage.LoggedInUserId,
                                policyId: $scope.policyIdForTransfer,
                                //countryId: $scope.newBordx.countryId,
                                insurerId: $scope.newBordx.insurerId,
                                reinsurerId: $scope.newBordx.reinsurerId,
                                commodityTypeId: $scope.newBordx.commodityTypeId

                            }
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Bordx/TransferPolicyToBordx',
                                data: data,
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                if (data == 'successful') {
                                    swal({ title: "TAS Information", text: "Bordereaux changed successfully.", showConfirmButton: true });
                                    $scope.policyTable.data = [];
                                    $scope.policyTable.reload();
                                    BordxChangePopup.close();
                                    $scope.errorTransferPolicy = "";
                                } else {
                                      swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });
                                    $scope.errorTransferPolicy = data;
                                    //swal.close();
                                }
                            }).error(function (data, status, headers, config) {
                                swal.close();
                            }).finally(function () {

                            });
                        }
                    });

                } else {
                    $scope.errorTransferPolicy = "Invalid policy selection";
                }

            } else {
                $scope.errorTransferPolicy = "Invalid new bordereaux year/month selection";
            }

        }

        $scope.loadBordxNumber = function () {
            $scope.bordx.number = '';
            if (isGuid($scope.bordx.insurerId) && isGuid($scope.bordx.reinsurerId) && isGuid($scope.bordx.commodityTypeId)) {
                if (parseInt($scope.bordx.year) && parseInt($scope.bordx.month) &&
                    parseInt($scope.bordx.month) > 0 && parseInt($scope.bordx.year) > 0) {
                    var data = {
                        'year': $scope.bordx.year,
                        'month': $scope.bordx.month,
                        'insurerId': $scope.bordx.insurerId,
                        'reinsurerId': $scope.bordx.reinsurerId,
                        //'countryId': $scope.bordx.countryId,
                        'commodityTypeId': $scope.bordx.commodityTypeId
                    };
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Bordx/GetNextBordxNumber',
                        data: data,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.bordx.number = data;
                        $scope.loadStartDateAndEndDates();
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {

                    });
                }
            }
        }

        $scope.loadBordxNumbers = function () {
            //$scope.policyTable.data = [];
            $scope.policyTable.reload();
            $scope.bordxNumbers = [];
            if (parseInt($scope.bordxProcess.year) && parseInt($scope.bordxProcess.month) &&
                parseInt($scope.bordxProcess.month) > 0 && parseInt($scope.bordxProcess.year) > 0 &&
                isGuid($scope.bordxProcess.insurerId) && isGuid($scope.bordxProcess.reinsurerId)
                && isGuid($scope.bordxProcess.commodityTypeId)) {
                var data = {
                    'year': $scope.bordxProcess.year,
                    'month': $scope.bordxProcess.month,
                    'insurerId': $scope.bordxProcess.insurerId,
                    'reinsurerId': $scope.bordxProcess.reinsurerId,
                    //'countryId': $scope.bordxProcess.countryId,
                    'commodityTypeId': $scope.bordxProcess.commodityTypeId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Bordx/GetNextBordxNumbers',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == '') {
                        //$scope.bordxNumbers.push({ 'Number': 'No Bordx Defined' });
                        //$scope.bordxProcess.number = 'No Bordx Defined';
                        customErrorMessage("No bordereaux has defined for selected year/month");
                    } else {
                        $scope.bordxNumbers = data;

                    }
                   // $scope.bordxProcess.number = '0';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });

            }
        }

        $scope.getNewBordxNumbers = function () {
            $scope.newBordxNumbers = [];
            // alert($scope.newBordx.year + '-' + $scope.newBordx.month)
            if (parseInt($scope.newBordx.year) && parseInt($scope.newBordx.month) &&
                parseInt($scope.newBordx.month) > 0 && parseInt($scope.newBordx.year) > 0 &&
                isGuid($scope.newBordx.insurerId) && isGuid($scope.newBordx.reinsurerId) && isGuid($scope.newBordx.commodityTypeId)) {
                var data = {
                    'year': $scope.newBordx.year,
                    'month': $scope.newBordx.month,
                    //'countryId': $scope.newBordx.countryId,
                    'insurerId': $scope.newBordx.insurerId,
                    'reinsurerId': $scope.newBordx.reinsurerId,
                    'commodityTypeId': $scope.newBordx.commodityTypeId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Bordx/GetNextBordxNumbers',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == '') {
                        //$scope.bordxNumbers.push({ 'Number': 'No Bordx Defined' });
                        //$scope.bordxProcess.number = 'No Bordx Defined';
                        customErrorMessage("No bordereaux has defined for selected year/month");
                    } else {
                        $scope.newBordxNumbers = data;

                    }
                    $scope.newBordx.number = '0';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });

            }
        }

        $scope.getLast10Bordx = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Bordx/GetLast10Bordx',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                if (data != null) {
                    $scope.last10Bordx = data;
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });
        }

        $scope.createBordx = function () {
            if (isGuid($scope.bordx.insurerId) && isGuid($scope.bordx.reinsurerId) && isGuid($scope.bordx.commodityTypeId)) {
                if (parseInt($scope.bordx.year) && parseInt($scope.bordx.month) && parseInt($scope.bordx.month) > 0 && parseInt($scope.bordx.year) > 0) {

                    if ($scope.bordx.StartDate != '' && $scope.bordx.EndDate != '' &&
                        typeof $scope.bordx.StartDate !== 'undefined' && typeof $scope.bordx.EndDate !== 'undefined') {

                        if (new Date($scope.bordx.StartDate) <= new Date($scope.bordx.EndDate)) {


                            swal({
                                title: "Are you sure?",
                                text: "",
                                type: "warning",
                                showCancelButton: true,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, Create!",
                                cancelButtonText: "No, cancel!",
                                closeOnConfirm: false,
                                closeOnCancel: true
                            }, function (isConfirm) {
                                if (isConfirm) {
                                    swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                                    var newBordxRequest = {
                                        year: $scope.bordx.year,
                                        month: $scope.bordx.month,
                                        startDate: $scope.bordx.StartDate,
                                        endDate: $scope.bordx.EndDate,
                                        userId: $localStorage.LoggedInUserId,
                                        //countryId: $scope.bordx.countryId,
                                        insurerId: $scope.bordx.insurerId,
                                        reinsurerId: $scope.bordx.reinsurerId,
                                        commodityTypeId: $scope.bordx.commodityTypeId
                                    };

                                    $http({
                                        method: 'POST',
                                        url: '/TAS.Web/api/Bordx/CreateBordx',
                                        data: newBordxRequest,
                                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                    }).success(function (data, status, headers, config) {
                                        if (data == 'successful') {
                                            swal({ title: "TAS Information", text: "Bordereaux created successfully.", showConfirmButton: true });
                                            $scope.resetBordxCreatData();
                                            $scope.getLast10Bordx();
                                        } else {
                                            swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                                        }
                                    }).error(function (data, status, headers, config) {
                                        swal.close();
                                    }).finally(function () {

                                    });
                                }
                            });

                        } else {
                            customErrorMessage("From Date Cannot Exceed To Date.");
                        }
                    } else {
                        customErrorMessage("Please Select From Date & To Date.");
                    }

                } else {
                    customErrorMessage("Invalid Bordereaux Year/Month Selection.");
                }
            } else {
                customErrorMessage("Invalid Commodity Type/Country Selection.");
            }
        }

        $scope.deleteBordx = function (bordxId) {
            if (isGuid(bordxId)) {

                swal({
                    title: "Are you sure?",
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, Delete!",
                    cancelButtonText: "No, cancel!",
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({ title: 'Please Wait', text: 'Processing...', showConfirmButton: false });

                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Bordx/DeleteBordx',
                            data: { 'bordxId': bordxId },
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                        }).success(function (data, status, headers, config) {
                            if (data == 'successful') {
                                swal({ title: "TAS Information", text: "Bordereaux deleted successfully.", showConfirmButton: true });
                                $scope.getLast10Bordx();
                            } else {
                                swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                            }
                        }).error(function (data, status, headers, config) {
                            swal.close();
                        }).finally(function () {
                            //swal.close();
                        });

                    }
                });

            }
        }

        $scope.resetBordxCreatData = function () {
            $scope.bordx.year = "";
            $scope.bordx.month = "";
            $scope.bordx.StartDate = "";
            $scope.bordx.EndDate = "";
            $scope.bordx.number = '';
            $scope.bordx.countryId = emptyGuid();
            $scope.bordx.commodityTypeId = emptyGuid();
        }
        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };


    });