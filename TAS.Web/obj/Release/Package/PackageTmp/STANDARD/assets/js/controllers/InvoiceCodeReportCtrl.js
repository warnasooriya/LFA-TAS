app.controller('InvoiceCodeReportCtrl',
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
        $scope.inviceCode = {
            
            startDate: '',
            endDate: ''
            
        };
        

        $scope.loadInitailData = function () {
           
        }

        

      

        $scope.GenerateInvoiceCode = function () {
            if ($scope.inviceCode.startDate != "" &&
                $scope.inviceCode.endDate != "") {
                var data = {
                    startDate: $scope.inviceCode.startDate,
                    endDate: $scope.inviceCode.endDate
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/DownloadInvoiceCode',
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



