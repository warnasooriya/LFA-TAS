app.controller('ClaimBordxYearlyReOpenManagementCtrl',
    function ($scope, toaster, $rootScope, $http, SweetAlert, $localStorage, ngDialog, uiGridConstants, ngTableParams, $location) {

        $scope.currentBordxYear = "";
        $scope.currentCountry = "";

        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }

        function LoadDetails() {


            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetConfirmedClaimBordxYearlybyYear',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/Country/GetAllActiveCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });

        }

        LoadDetails();
        $scope.validateParams = function () {

            var isValid = true;
            if ($scope.currentCountry == '') {
                isValid = false;
                $scope.validate_Country = "has-error";

            }
            else {
                $scope.validate_Country = "";
            }
            //if ($scope.currentBordxYear == '') {
            //    isValid = false;
            //    $scope.validate_Country = "has-error";
            //}
            //else {
            //    $scope.validate_Country = "";
            //}
            return isValid;
        }

        $scope.SearchReopenBordx = function () {
            if ($scope.validateParams()) {
                $scope.policyTable.data = [];
                $scope.policyTable.reload();
            }
            else {
                customErrorMessage("Please select a Country");
            }
        }

        $scope.policyTable = new ngTableParams({
            page: 1,
            count: 25,
        }, {
            getData: function ($defer, params) {

                var page = params.page();
                var size = params.count();
                var search = {
                    page: page,
                    pageSize: size,
                    //bordxId: $scope.currentBordxId
                    //Todate: $scope.currentTodate,
                    //Fromdate: $scope.currentFromdate,
                    Country: $scope.currentCountry,
                    Year: $scope.currentBordxYear
                }
                //if () {
                if ($scope.currentCountry != '' ) {
                $scope.policyGridloading = true;
                $scope.policyGridloadAttempted = false;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimBordxManegement/GetAllClaimBordxByYearandCountryForGrid',
                        data: search,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        var response_arr = JSON.parse(data);
                        params.total(response_arr.totalRecords);
                        $defer.resolve(response_arr.data);
                    }).error(function (data, status, headers, config) {
                    }).finally(function () {
                        $scope.policyGridloading = false;
                        $scope.policyGridloadAttempted = true;
                    });
                }
            }
        });

        $scope.ReopenBordx = function (BordxYear, BordxCountry) {
            //if (isGuid($scope.currentBordxId)) {


            swal({
                title: "Are you sure?",
                text: "",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, Reopen it!",
                cancelButtonText: "No, cancel!",
                closeOnConfirm: false,
                closeOnCancel: true
            }, function (isConfirm) {
                if (isConfirm) {
                    swal({ title: 'Please wait', text: 'Reopening Claim Bordx...', showConfirmButton: false });
                    $scope.BordexReopenRequest =
                        {
                            claimBordxYear: BordxYear,
                            claimBordxCountry: BordxCountry
                        }
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxYealyReopen',
                        data: $scope.BordexReopenRequest,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    }).success(function (data, status, headers, config) {
                        if (data == 'successful') {
                            swal({ title: "TAS Information", text: "Claim Bordx Reopened successfully.", showConfirmButton: true },
                                function (ok) {
                                    if (ok) {

                                        $scope.currentBordxYear = "";
                                        $scope.currentCountry = "";

                                        $scope.policyTable.data = [];
                                        $scope.policyTable.reload();

                                    }
                                });

                        } else {
                            swal({ title: "TAS Information", text: data, showConfirmButton: true, type: 'error' });

                        }
                    }).error(function (data, status, headers, config) {
                        swal({ title: "TAS Information", text: "Error occured", showConfirmButton: true, type: 'error' });
                    }).finally(function () {

                    });

                }
            });

            //} else {
            //    customErrorMessage("Please select a bordx for reopen");
            //}
        }


        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };

    });