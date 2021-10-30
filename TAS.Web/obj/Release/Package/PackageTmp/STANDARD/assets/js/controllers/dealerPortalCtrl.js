app.controller('DealerPortalCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, toaster, $location) {

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        //end of supportive function
        $scope.dealerInvoiceRequest = {
            dealerId: emptyGuid(),
            year: '',
            month: '',
            bordxId: emptyGuid()
        };

        $scope.dealers = [];
        $scope.Years = [];
        $scope.Months = [];
        $scope.bordxList = [];

        $scope.loadInitailData = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealersByUserId',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $localStorage.LoggedInUserId },
            }).success(function (data, status, headers, config) {
                if (data == '' || data == null) {
                    customErrorMessage("You haven't assign to any dealer.");
                } else {
                    $scope.dealers = data.Dealers;
                    if (data.Dealers.length == 1) {
                        $scope.dealerInvoiceRequest.dealerId = $scope.dealers[0].Id;
                    }
                }
            }).error(function (data, status, headers, config) {
            });


            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetYearsMonthsForDealerInvoices',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },

            }).success(function (data, status, headers, config) {
                $scope.Years = data.years;
                $scope.Months = data.months;
                //$scope.dealerInvoiceRequest.year = data.years[0];

                //for (var i = 0; i < data.months.length; i++) {
                //    if (data.months[i].isDefault == true) {
                //        $scope.dealerInvoiceRequest.month = data.months[i].monthsSeq;
                //        $scope.loadBordxNumbers();
                //    }
                //}
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.GenerateInvoices = function () {

            if (isGuid($scope.dealerInvoiceRequest.dealerId)
                && parseInt($scope.dealerInvoiceRequest.year)
                && parseInt($scope.dealerInvoiceRequest.year) > 0
                && parseInt($scope.dealerInvoiceRequest.month)
                && parseInt($scope.dealerInvoiceRequest.month) > 0) {
                swal({ title: 'Please wait', text: 'Preparing Dealer Invoices...', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GenerateDealerInvoices',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: $scope.dealerInvoiceRequest,
                    responseType: "arraybuffer",
                }).success(function (data, status, headers, config) {
                    if (status == 204)
                        customErrorMessage("No policies found for selected Year/Month");
                    var FileName = headers('Content-Disposition').split('=')[1];

                    var defaultFileName = FileName;
                    var type = headers('Content-Type');
                    var disposition = headers('Content-Disposition');

                    defaultFileName = defaultFileName.replace(/[<>:"\/\\|?*]+/g, '');
                    var blob = new Blob([data], { type: type });
                    saveAs(blob, defaultFileName);

                }).error(function (data, status, headers, config) {

                    customErrorMessage("No policies found for selected Year/Month");
                }).finally(function () {
                    swal.close();
                });

            } else {
                customErrorMessage("Please select all the required fields");
            }
            return false;

        }

        $scope.loadBordxNumbers = function () {
            if (isGuid($scope.dealerInvoiceRequest.dealerId)
                && parseInt($scope.dealerInvoiceRequest.year)
                && parseInt($scope.dealerInvoiceRequest.year) > 0
                && parseInt($scope.dealerInvoiceRequest.month)
                && parseInt($scope.dealerInvoiceRequest.month) > 0) {

                var data = {
                    year: $scope.dealerInvoiceRequest.year,
                    month: $scope.dealerInvoiceRequest.month
                }
                $scope.bordxList = [];
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetConfirmedBordxByYearAndMonth',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: data,
                }).success(function (data, status, headers, config) {
                    $scope.bordxList = data;
                }).error(function (data, status, headers, config) {
                });
            } else {
                customErrorMessage("Please select all the required fields");
            }
        }

        $scope.GenerateInvoiceSummary = function () {
            if (isGuid($scope.dealerInvoiceRequest.dealerId) &&
                isGuid($scope.dealerInvoiceRequest.bordxId)) {
                var data = {
                    dealerId: $scope.dealerInvoiceRequest.dealerId,
                    bordxId: $scope.dealerInvoiceRequest.bordxId
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/DownloadInvoiceSummary',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: data,
                }).success(function (datar, status, headers, config) {
                    //alert(datar);
                    if (typeof datar !== "undefined" && datar.length > 0) {
                        var url = $location.protocol() + '://' + $location.host() +
                            '/TAS.Web/ReportExplorer.aspx?key=' + datar + "&jwt=" + $localStorage.jwt;
                        // alert(url);
                        
                        window.open(url, '_blank')
                    } else {
                        customErrorMessage("Someting went wrong. Pelase contract administrator.");
                    }
                }).error(function (data, status, headers, config) {
                });
            } else {
                customErrorMessage("Please select all the required fields");
            }
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };


    });



