app.controller('BrokerCtrl',
    function ($scope, $rootScope, $http, FileUploader, $rootScope, $location, SweetAlert, $modal, $localStorage, toaster, $filter, $translate) {
    
        $scope.saveOrUpdateBtnIconClass = "";
        $scope.saveOrUpdateBtnDisabled = false;
        $scope.dataLoadingMessage = "Loding Data, Please Wait";
        $scope.saveOrUpdateDisabled = false;
        $scope.labelSave = 'pages.brokerManagement.save';
       

        $scope.broker = {
            Id: "",
            Name: "",
            Code: "",
            Status: false,
            CountryId: "00000000-0000-0000-0000-000000000000",
            TelNumber: "",
            Address: "",
        };

        $scope.getBrokersByCountry = function () {
            $scope.allbrokers = null;
            $scope.saveOrUpdateDisabled = true;
            
            if ($scope.broker.CountryId != "0") {
                var countryId = $scope.broker.CountryId;
                $scope.broker = {
                    Id: "",
                    Name: "",
                    Code: "",
                    Status: false,
                    CountryId: countryId,
                    TelNumber: "",
                    Address: "",
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Broker/GetAllBrokersByCountry',
                    data: { "CountryId": $scope.broker.CountryId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.allbrokers = data;
                        $scope.ListOfBrokers = data;
                        $scope.saveOrUpdateDisabled = false; 
                    }).error(function (data, status, headers, config) {
                });
            } 
        }

        $scope.SetBrokerValues = function () {
            var countryId = $scope.broker.CountryId;
            var BrokerId = $scope.broker.Id;
            $scope.allbrokers = "";
            console.log($scope.broker.Id);
            if ($scope.broker.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Broker/GetBrokerDetailsByBrokerId',
                    headers: { 'Authorization': $localStorage.jwt },
                    data: { "Id": $scope.broker.Id }
                }).success(function (data, status, headers, config) {
                    $scope.allbrokers = $scope.ListOfBrokers;
                    $scope.broker = data[0];
                    if (data[0].BrokerStatus === 'True') {
                        $scope.broker.Status = true;
                    } else {
                        $scope.broker.Status = false;
                    }
                     
                    $scope.saveOrUpdateDisabled = false;
                    $scope.validateBrokerCode = $scope.broker.Code;
                    $scope.labelSave = 'pages.brokerManagement.update';
                }).error(function (data, status, headers, config) {
                });
            } else {
                $scope.broker = {
                    Id: "",
                    Name: "",
                    Code: "",
                    Status: false,
                    TelNumber: "",
                    Address: "",

                };
                $scope.allbrokers = "";
            }
        }


         function clearBroker() {
            $scope.broker = {
                Id: "",
                Name: "",
                Code: "",
                Status: false,
                CountryId: "00000000-0000-0000-0000-000000000000",
                TelNumber: "",
                Address: "",
            };
         }
         

       $scope.loadInitailData = function () { 

            $scope.loadCountryData = true;
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.countries = data;
                $scope.loadCountryData = false;
                $scope.dataLoadingMessage = "Select Country";
            }).error(function (data, status, headers, config) {
            }); 
           
        }
         
     
        function validate() {
            
            isValid = true;
            if ($scope.broker.Name == undefined || $scope.broker.Name == "") {
                $scope.validate_brokerName = "has-error";
                isValid = false;                 
            } else {
                $scope.validate_brokerName = "";
            }

            if ($scope.broker.Code == undefined || $scope.broker.Code == "") {
                $scope.validate_brokerCode = "has-error";
                isValid = false;                 
            } else {
                $scope.validate_brokerCode = "";
            }
              
            if ($scope.broker.CountryId == undefined || $scope.broker.CountryId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CountryId = "has-error";
                isValid = false;                
            } else {
                $scope.validate_CountryId = "";
            }
            

            if (!isValid) {
                customErrorMessage($filter('translate')('common.errMessage.validateHighlightedFields'));
            }
            return isValid;
        }
         
        function isValidTaxData() {
            isValid = true;
            angular.forEach($scope.allbrokers, function (value, key) {                 
                if (($scope.broker.Code == value.Code) || ($scope.broker.Name == value.Name)) {
                    console.log($filter('translate')('pages.brokerManagement.nameExists'));
                    isValid = false;
                }
            });
            return isValid;
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', $filter('translate')('common.popUpMessages.error'), msg);
        };

         

        $scope.saveOrUpdateBroker = function () {
    
            if (validate()) {
                if ($scope.broker.Id != "") {
                    if (IsNoExsistsBrokerCode()) {
                        $scope.saveOrUpdateBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.saveOrUpdateBtnDisabled = true;
                        var id = $scope.broker.Id;
                        console.log($scope.broker);
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Broker/UpdateBroker',
                            headers: { 'Authorization': $localStorage.jwt },
                            data: $scope.broker
                        }).success(function (data, status, headers, config) {
                            $scope.saveOrUpdateBtnIconClass = "";
                            $scope.saveOrUpdateBtnDisabled = false;
                            SweetAlert.swal({
                                title: $filter('translate')('pages.brokerManagement.brokerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.allbrokers = "";
                            $scope.broker = {
                                Id: "",
                                Name: "",
                                Code: "",
                                CountryId: "00000000-0000-0000-0000-000000000000",
                                Status: false,
                                TelNumber: "",
                                Address: "",
                            }
                        }).error(function (data, status, headers, config) {
                            $scope.saveOrUpdateBtnIconClass = "";
                            $scope.saveOrUpdateBtnDisabled = false;
                        });
                    }

                } else {
                    if (IsNoExsistsBrokerCodeForInsert()) {
                        $scope.saveOrUpdateBtnIconClass = "fa fa-spinner fa-spin";
                        $scope.saveOrUpdateBtnDisabled = true;
                        $http({
                            method: 'POST',
                            url: '/TAS.Web/api/Broker/SaveBroker',
                            headers: { 'Authorization': $localStorage.jwt },
                            data: $scope.broker
                        }).success(function (data, status, headers, config) {
                            $scope.saveOrUpdateBtnIconClass = "";
                            $scope.saveOrUpdateBtnDisabled = false;
                            SweetAlert.swal({
                                title: $filter('translate')('pages.brokerManagement.brokerInformation'),
                                text: $filter('translate')('common.sucessMessages.successfullySaved'),
                                confirmButtonText: $filter('translate')('common.button.ok'),
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.broker = {
                                Id: "",
                                Name: "",
                                Code: "",
                                CountryId: "00000000-0000-0000-0000-000000000000",
                                Status: "0",
                                TelNumber: "",
                                Address: "",
                            }
                        }).error(function (data, status, headers, config) {
                            $scope.saveOrUpdateBtnIconClass = "";
                            $scope.saveOrUpdateBtnDisabled = false;
                            $scope.allbrokers = "";
                            $scope.broker = {
                                Id: "",
                                Name: "",
                                Code: "",
                                CountryId: "00000000-0000-0000-0000-000000000000",
                                Status: false,
                                TelNumber: "",
                                Address: "",
                            }
                        });
                    }


                }

            } 
        }

         

        function IsNoExsistsBrokerCode() {
            var status = true;
            if ($scope.validateBrokerCode != $scope.broker.Code) {
                angular.forEach($scope.ListOfBrokers, function (value, key) {
                    if ($scope.broker.Code == value.Code) {
                        customErrorMessage($filter('translate')('pages.brokerManagement.brokerAlreadyExists'))
                        status = false;
                    }
                });
            }
            return status;
        }

        function IsNoExsistsBrokerCodeForInsert() {
            var status = true;
            angular.forEach($scope.ListOfBrokers, function (value, key) {
                if ($scope.broker.Code == value.Code) {
                    customErrorMessage($filter('translate')('pages.brokerManagement.brokerAlreadyExists'))
                    status = false;
                }
            });
            return status;
        }
    });