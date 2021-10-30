app.controller('ClaimBordxReOpenManagementCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, uiGridConstants, ngTableParams, $location, toaster, $filter, $translate) {

        $scope.currentreinsure = emptyGuid();
        $scope.currentinsure = emptyGuid();
        $scope.currentId = emptyGuid();       
        $scope.currentBordxNumber = "";  
        

        $scope.currentBordxYear = "";
        $scope.currentBordxMonth = "";
        $scope.currentTodate = "";
        $scope.currentFromdate = "";
        var SearchBordxPopup;//search popup public var
        $scope.bordxSearchGridloading = false;
        $scope.bordxSearchGridloadAttempted = false
        $scope.bordxAvailableYears = [];
        $scope.bordxNumbers = [];
        
        function isGuid(stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        }
        function emptyGuid() {
            return "00000000-0000-0000-0000-000000000000";
        }

        $scope.ClaimBordxSearchGridSearchCriterias = {
            Insurer: emptyGuid(),
            year: 0,
            month: 0,
            Reinsurer: emptyGuid(),

        };
        $scope.bordxViewGridSearchCriterias = {
            bordxId: emptyGuid(),
            userId: emptyGuid()

        };

        

        $scope.SearchReopenBordx = function () {
            $scope.policyTable.data = [];
            $scope.policyTable.reload();
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
                    Todate: $scope.currentTodate,
                    Fromdate: $scope.currentFromdate,
                    month:$scope.currentBordxMonth,
                    year: $scope.currentBordxYear,
                    number: $scope.currentBordxNumber
                }
                //if () {
                $scope.policyGridloading = true;
                $scope.policyGridloadAttempted = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ClaimBordxManegement/GetAllClaimBordxByDateForGrid',
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
                //     }
            }
        });

       
        function LoadDetails() {
            //validity check

           // $scope.openSearchPopup();          

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
                url: '/TAS.Web/api/ReinsurerManagement/GetAllReinsurers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.reinsurers = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetConfirmedClaimBordxYears',                
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxAvailableYears = data;
            }).error(function (data, status, headers, config) {
            });

            $http({
                method: 'POST',
                url: '/TAS.Web/api/ClaimBordxManegement/GetClaimBordxBordxNumbers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.bordxNumbers = data;
            }).error(function (data, status, headers, config) {
            });


        }

        LoadDetails();

        $scope.GetBordxNumbersYearsAndMonth = function () {
            if ($scope.currentBordxYear != "" && $scope.currentBordxMonth != "") {
               
                $http({
                    method: 'post',
                    url: '/tas.web/api/Bordx/getBordxNumbersYearsAndMonth',
                    data: JSON.stringify({ 'bordxYear': $scope.currentBordxYear, 'bordxMonth': $scope.currentBordxMonth }),
                    headers: { 'authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.bordxNumbers = data;                    
                }).error(function (data, status, headers, config) {
                });
            }
        }

        
        $scope.ReopenBordx = function (BordxYear, BordxMonth) {
            //if (isGuid($scope.currentBordxId)) {
            if (BordxYear == 0 || BordxMonth == 0 || BordxYear == "" || BordxMonth == "") {
                customErrorMessage($filter('translate')('pages.claimBordxReOpenManagement.errorMessages.selectYearandMonths'))
            } else {
                swal({
                    title: $filter('translate')('common.areYouSure'),
                    text: "",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: $filter('translate')('pages.claimBordxReOpenManagement.errorMessages.reopenIt'),
                    cancelButtonText: $filter('translate')('pages.claimBordxReOpenManagement.errorMessages.noCancel'),
                    closeOnConfirm: false,
                    closeOnCancel: true
                }, function (isConfirm) {
                    if (isConfirm) {
                        swal({
                            title: $filter('translate')('pages.claimBordxReOpenManagement.errorMessages.pleaseWait'),
                            text: $filter('translate')('pages.claimBordxReOpenManagement.errorMessages.reOpenBordx'),
                            showConfirmButton: false
                        });
                        $scope.BordexReopenRequest =
                            {
                                claimBordxYear: BordxYear,
                                claimBordxMonth: BordxMonth
                            }
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/ClaimBordxManegement/ClaimBordxReopen',
                            data: $scope.BordexReopenRequest,
                            headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        }).success(function (data, status, headers, config) {
                            if (data == 'successful') {
                                swal({
                                    title: $filter('translate')('common.alertTitle'),
                                    text: $filter('translate')('pages.claimBordxReOpenManagement.inforMessages.reopenSuccess'),
                                    showConfirmButton: true
                                },
                                    function (ok) {
                                        if (ok) {

                                            $scope.currentBordxYear = "";
                                            $scope.currentBordxMonth = "";

                                            $scope.policyTable.data = [];
                                            $scope.policyTable.reload();

                                        }
                                    });

                            } else {
                                swal({
                                    title: $filter('translate')('common.alertTitle'),
                                    text: data,
                                    showConfirmButton: true,
                                    type: 'error'
                                });

                            }
                        }).error(function (data, status, headers, config) {
                            swal({
                                title: $filter('translate')('common.alertTitle'),
                                text: $filter('translate')('pages.claimBordxReOpenManagement.errorMessages.errorOccured'),
                                showConfirmButton: true,
                                type: 'error'
                            });
                        }).finally(function () {

                        });

                    }
                });
            }

              
        }


        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };


    });