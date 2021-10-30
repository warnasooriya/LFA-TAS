app.controller('DealerInvoiceCodeGenerationCtrl',
    function ($scope, $rootScope, $compile, $http, $parse, ngDialog, SweetAlert, $localStorage, toaster, $sce,
        $cookieStore, $filter, toaster, $state, $window) {

        var w = angular.element($window);
        $scope.windowHeight = w.height();
        $scope.windowWidth = w.width();
        $scope.appendToBody =  false;

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
        //end of supportive functions
        var CodesearchPopup;
        $scope.isFrontTireDetailsVisible = false;
        $scope.isBackTireDetailsVisible = false;


        $scope.dataLoadMode = false;
        $scope.dealerInvoiceDetails = {
            dealerId: emptyGuid(),
            dealerBranchId: emptyGuid(),
            countryId: emptyGuid(),
            cityId: emptyGuid(),
            quantity: '',
            plateNumber: ''
        };
        $scope.gridDealerInvoiceCodeLoading = false;
        $scope.gridDealerInvoiceCodeloadAttempted = false;
        $scope.dealerInvoiceTireDetails = {
            front: {
                width: '',
                cross: '',
                diameter: '',
                loadSpeed: '',
                serialLeft: '',
                serialRight: '',
                pattern: '',
                
                
            },
            back: {
                width: '',
                cross: '',
                diameter: '',
                loadSpeed: '',
                serialLeft: '',
                serialRight: '',
                pattern: '',
               
            }
        };
        $scope.dealerInvoiceCodeSearchParam = {
            date: '',
            plateNo: '',
            code: ''
        }

        $scope.initializeDealerInvoiceCodePage = function () {
            if (isGuid($localStorage.LoggedInUserId)) {
                swal({ title: 'Authenticating...', text: 'Validating user information', showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/UserValidationDealerInvoiceCode',
                    data: { "loggedInUserId": $localStorage.LoggedInUserId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === 'SUCCESS') {
                        swal.close();
                        $scope.dealerInvoiceDetails.dealerId = data.obj.dealerId;
                        $scope.dealerInvoiceDetails.dealerBranchId = data.obj.dealerBranchId;
                        $scope.dealerInvoiceDetails.countryId = data.obj.countryId;
                        $scope.dealerInvoiceDetails.cityId = data.obj.cityId;
                        $scope.loadAvailableTireDetails();
                    } else {
                        swal({ title: 'TAS Security Information', text: data.msg, showConfirmButton: false });
                        setTimeout(function () { swal.close(); }, 8000);
                        $state.go('app.dashboard');
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            } else {

                swal({ title: 'TAS Security Information', text: 'Please sign in as a dealer to access this page.', showConfirmButton: false });
                setTimeout(function () { swal.close(); }, 8000);
                $state.go('app.dashboard');
            }
        }
        $scope.availableCrossList = [];
        $scope.availableWidthList = [];
        $scope.availableDiameteList = [];
        $scope.availableLoadSpeedList = [];
        $scope.availablePatternList = [];
        $scope.availableCrossListBack = [];
        $scope.availableWidthListBack = [];
        $scope.availableDiameteListBack = [];
        $scope.availableLoadSpeedListBack = [];
        $scope.availablePatternListBack = [];

        $scope.loadAvailableTireDetails = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizes',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.availableWidthList = data.WidthList;
                $scope.availableCrossList = data.CrossSectionList;
                $scope.availableDiameteList = data.DiameterList;
                $scope.availableLoadSpeedList = data.LoadSpeedList;
                $scope.availablePatternList = data.PatternList;
                $scope.availableWidthListBack = data.WidthList;
                $scope.availableCrossListBack = data.CrossSectionList;
                $scope.availableDiameteListBack = data.DiameterList;
                $scope.availableLoadSpeedListBack = data.LoadSpeedList;
                $scope.availablePatternListBack = data.PatternList;
            }).error(function (data, status, headers, config) {
            }).finally(function () {

            });
        }

        $scope.selectTyreQuantity = function (qty) {
            $scope.dealerInvoiceDetails.quantity = parseInt(qty);
            if ($scope.dealerInvoiceDetails.plateNumber !== '') {
                nextStep();
            } else {
                customErrorMessage("Please enter a vehicle plate number.");
            }
        }
        $scope.openFrontTireDetails = function (elem, position) {
            if (position === 'F') {
                if ($scope.isFrontTireDetailsVisible) {
                    $scope.isFrontTireDetailsVisible = false;
                } else {
                    $scope.isFrontTireDetailsVisible = true;
                }
            }
            else if (position === 'B') {
                if ($scope.isBackTireDetailsVisible) {
                    $scope.isBackTireDetailsVisible = false;
                } else {
                    $scope.isBackTireDetailsVisible = true;
                }
            }
        }

        $scope.resetInvoiceTireDetails = function () {
            $scope.dealerInvoiceTireDetails = {
                front: {
                    width: '',
                    cross: '',
                    diameter: '',
                    loadSpeed: '',
                    serialLeft: '',
                    serialRight: '',
                    pattern: ''
                },
                back: {
                    width: '',
                    cross: '',
                    diameter: '',
                    loadSpeed: '',
                    serialRight: '',
                    pattern: ''
                }
            };

            $scope.frontTyreWidthValidation = "";
            $scope.frontTyreCrossValidation = "";
            $scope.frontTyreDiameterValidation = "";
            $scope.frontTyreLoadSpeedValidation = "";
            $scope.frontTyreLeftSerialValidation = "";
            $scope.frontTyreRightSerialValidation = "";
            $scope.frontTyreRightpatternValidation = "";
            $scope.backTyreWidthValidation = "";
            $scope.backTyreCrossValidation = "";
            $scope.backTyreDiameterValidation = "";
            $scope.backTyreLoadSpeedValidation = "";
            $scope.backTyreSerialLeftValidation = "";
            $scope.backTyreSerialRightValidation = "";
            $scope.backTyrepatternValidation = "";

            $scope.loadAvailableTireDetails();
        }

        $scope.frontTireEntryfrontwidth = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.front = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
            if ($scope.dealerInvoiceTireDetails.front.width != "") {

                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByWidth',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //$scope.availableWidthList = data.WidthList;    
                    $scope.availableCrossList = data.CrossSectionList;
                    $scope.dealerInvoiceTireDetails.front.cross = '';
                    $scope.dealerInvoiceTireDetails.front, diameter = '';
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            } 

        }

        $scope.frontTireEntryfrontcross = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.front = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }

            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != "")
            {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByDiameter',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //$scope.availableCrossList = data.CrossSectionList;
                    $scope.availableDiameteList = data.DiameterList;
                    $scope.dealerInvoiceTireDetails.front.diameter = '';
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }
         

        }


        $scope.frontTireEntryfrontdiameter = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.front = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }          
            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != ""
                && $scope.dealerInvoiceTireDetails.front.diameter != "" ) {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.front.diameter,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByloadSpeed',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availableLoadSpeedList = data.LoadSpeedList;
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }

        }

        $scope.frontTireEntryfrontloadSpeed = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.front = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
            if ($scope.dealerInvoiceTireDetails.front.width != "" && $scope.dealerInvoiceTireDetails.front.cross != ""
                && $scope.dealerInvoiceTireDetails.front.diameter != "" && $scope.dealerInvoiceTireDetails.front.loadSpeed != "") {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.front.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.front.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.front.diameter,
                    'frontloadSpeed': $scope.dealerInvoiceTireDetails.front.loadSpeed,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByPattern',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {                   
                    $scope.availablePatternList = data.PatternList;
                    $scope.dealerInvoiceTireDetails.front.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });

            }

        }

        $scope.frontTireEntry = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width > 0 ||
                    $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross > 0) {
                    customErrorMessage("2 tires cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.front = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
        }

        $scope.rearTireEntryBackwidth = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.back = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
            if ($scope.dealerInvoiceTireDetails.back.width != "") {

                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByWidth',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //$scope.availableWidthList = data.WidthList;    
                    $scope.availableCrossListBack = data.CrossSectionList;
                    $scope.dealerInvoiceTireDetails.back.cross = '';
                    $scope.dealerInvoiceTireDetails.back, diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }

        }

        $scope.rearTireEntryBackcross = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.back = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }

            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != "") {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByDiameter',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    //$scope.availableCrossList = data.CrossSectionList;
                    $scope.availableDiameteListBack = data.DiameterList;
                    $scope.dealerInvoiceTireDetails.back.diameter = '';
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }


        }

        $scope.rearTireEntryBackdiameter = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.back = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "") {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByloadSpeed',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availableLoadSpeedListBack = data.LoadSpeedList;
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = '';
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });
            }

        }

        $scope.rearTireEntryBackloadSpeed = function () {
            // alert($scope.dealerInvoiceTireDetails.back.width.length);
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.back = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }
            }
            if ($scope.dealerInvoiceTireDetails.back.width != "" && $scope.dealerInvoiceTireDetails.back.cross != ""
                && $scope.dealerInvoiceTireDetails.back.diameter != "" && $scope.dealerInvoiceTireDetails.back.loadSpeed != "") {
                var data = {
                    'frontwidth': $scope.dealerInvoiceTireDetails.back.width,
                    'frontcross': $scope.dealerInvoiceTireDetails.back.cross,
                    'frontdiameter': $scope.dealerInvoiceTireDetails.back.diameter,
                    'frontloadSpeed': $scope.dealerInvoiceTireDetails.back.loadSpeed,
                };

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GetAllAvailabelTireSizesByPattern',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.availablePatternListBack = data.PatternList;
                    $scope.dealerInvoiceTireDetails.back.pattern = '';
                }).error(function (data, status, headers, config) {
                }).finally(function () {

                });

            }

        }

        $scope.rearTireEntry = function () {
            if ($scope.dealerInvoiceDetails.quantity === 2) {
                if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width > 0 ||
                    $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross > 0) {
                    customErrorMessage("2 tiers cannot be enterd in both front and rear.");
                    $scope.dealerInvoiceTireDetails.back = {
                        width: '',
                        cross: '',
                        diameter: '',
                        loadSpeed: '',
                        serialLeft: '',
                        serialRight: ''
                    }
                }               
            }            
        }
        $scope.frontTyreWidthValidation = '';
        $scope.frontTyreCrossValidation = '';
        $scope.frontTyreDiameterValidation = '';
        $scope.frontTyreLoadSpeedValidation = '';
        $scope.frontTyreLeftSerialValidation = '';
        $scope.frontTyreRightSerialValidation = '';

        $scope.backTyreWidthValidation = '';
        $scope.backTyreCrossValidation = '';
        $scope.backTyreDiameterValidation = '';
        $scope.backTyreLoadSpeedValidation = '';
        $scope.backTyreSerialRightValidation = '';
        $scope.backTyreSerialLeftValidation = '';

        $scope.validateBackTireDetails = function () {
            var isValid = true;
            if ($scope.dealerInvoiceTireDetails.back.width.length == 0 || $scope.dealerInvoiceTireDetails.back.width == 0) {
                $scope.backTyreWidthValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreWidthValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.cross.length == 0 || $scope.dealerInvoiceTireDetails.back.cross == 0) {
                $scope.backTyreCrossValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreCrossValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.diameter.length == 0 || $scope.dealerInvoiceTireDetails.back.diameter == 0) {
                $scope.backTyreDiameterValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreDiameterValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.loadSpeed.length == 0 || $scope.dealerInvoiceTireDetails.back.loadSpeed == 0) {
                $scope.backTyreLoadSpeedValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreLoadSpeedValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.serialLeft.length == 0) {
                $scope.backTyreSerialLeftValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreSerialLeftValidation = "";

            if ($scope.dealerInvoiceTireDetails.back.serialRight.length == 0) {
                $scope.backTyreSerialRightValidation = "has-error";
                isValid = false;
            } else
                $scope.backTyreSerialRightValidation = "";

            return isValid;
        }
        $scope.validateFrontTireDetails = function () {
            var isValid = true;
            if ($scope.dealerInvoiceTireDetails.front.width.length == 0 || $scope.dealerInvoiceTireDetails.front.width == 0) {
                $scope.frontTyreWidthValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreWidthValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.cross.length == 0 || $scope.dealerInvoiceTireDetails.front.cross == 0) {
                $scope.frontTyreCrossValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreCrossValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.diameter.length == 0 || $scope.dealerInvoiceTireDetails.front.diameter == 0) {
                $scope.frontTyreDiameterValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreDiameterValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.loadSpeed.length == 0 || $scope.dealerInvoiceTireDetails.front.loadSpeed == 0) {
                $scope.frontTyreLoadSpeedValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreLoadSpeedValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.serialLeft.length == 0) {
                $scope.frontTyreLeftSerialValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreLeftSerialValidation = "";

            if ($scope.dealerInvoiceTireDetails.front.serialRight.length == 0) {
                $scope.frontTyreRightSerialValidation = "has-error";
                isValid = false;
            } else
                $scope.frontTyreRightSerialValidation = "";

            return isValid;
        }

        $scope.generateInvoiceCode = function () {
            if ($scope.dealerInvoiceDetails.quantity != '') {
                if ($scope.dealerInvoiceDetails.quantity == 2) {
                    if ($scope.dealerInvoiceTireDetails.front.width.length > 0 || $scope.dealerInvoiceTireDetails.front.width != 0 ||
                        $scope.dealerInvoiceTireDetails.front.cross.length > 0 || $scope.dealerInvoiceTireDetails.front.cross != 0) {
                        $scope.dealerInvoiceTireDetails.back.width = '';
                        $scope.dealerInvoiceTireDetails.back.cross = '';
                        $scope.dealerInvoiceTireDetails.back.diameter = '';


                        if (!$scope.validateFrontTireDetails()) {
                            customErrorMessage("Please fill all highlighted tire details.");
                            return;
                        }
                    } else if ($scope.dealerInvoiceTireDetails.back.width.length > 0 || $scope.dealerInvoiceTireDetails.back.width != 0 ||
                        $scope.dealerInvoiceTireDetails.back.cross.length > 0 || $scope.dealerInvoiceTireDetails.back.cross != 0) {
                        $scope.dealerInvoiceTireDetails.front.width = '';
                        $scope.dealerInvoiceTireDetails.front.cross = '';
                        $scope.dealerInvoiceTireDetails.front.diameter = '';
                        if (!$scope.validateBackTireDetails()) {
                            customErrorMessage("Please fill all highlighted tire details.");
                            return;
                        }
                        
                    } else {
                        customErrorMessage("Please enter tire details.");
                        return;
                    }
                } else {

                    if ($scope.dealerInvoiceTireDetails.back.width >= $scope.dealerInvoiceTireDetails.front.width) {
                        
                    } else {
                        customErrorMessage("Back tyres should be euql or greater than front tyres in width.");
                        return;
                    }
                    if (!$scope.validateFrontTireDetails()) {
                        customErrorMessage("Please fill all highlighted tire details.");
                        return;
                    }
                    if (!$scope.validateBackTireDetails()) {
                        customErrorMessage("Please fill all highlighted tire details.");
                        return;
                    }
                }

                swal({ title: 'Processing...', text: 'Generating new invoice code', showConfirmButton: false });
                 //setting zero

               
                var data = {
                    'dealerInvoiceDetails': $scope.dealerInvoiceDetails,
                    'dealerInvoiceTireDetails': $scope.dealerInvoiceTireDetails,
                    'loggedInUserId': $localStorage.LoggedInUserId
                };
               

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/GenerateInvoiceCode',
                    dataType: 'json',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data.code === "SUCCESS") {
                        var txt = "<h3>Your invoice code is :</h3><strong><h2>" + data.msg + "</h2></strong>.";
                        SweetAlert.swal({
                            html: true,
                            title: "Success",
                            text: txt,
                            type: "success",
                            showCancelButton: false,
                            closeOnConfirm: true,
                            allowEscapeKey: false,
                            confirmButtonText: "OK",
                            confirmButtonColor: "#ffa500",
                        }, function () {
                            $scope.resetForNewItemEntry();
                        });
                    } else {
                        swal.close();
                        customErrorMessage(data.msg);
                    }
                      
                }).error(function (data, status, headers, config) {
                    swal.close();
                });
            } else {
                customErrorMessage('Please select tire quantity.');
            }
        }
        $scope.resetForNewItemEntry = function () {
            $scope.resetInvoiceTireDetails();
            $scope.dealerInvoiceDetails.quantity = '';
            $scope.dealerInvoiceDetails.plateNumber = '';
            goToStep(1);
            $scope.dataLoadMode = false;
        }
        $scope.searchPopup = function () {
            CodesearchPopup = ngDialog.open({
                template: 'popUpSearchCode',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
            getInvoiceCodeSearchPage();
        };
        var paginationOptionsDealerDealerInvoiceSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        $scope.gridOptionsInvoiceCode = {
            paginationPageSizes: [10, 20, 50],
            paginationPageSize: 10,
            useExternalPagination: true,
            useExternalSorting: true,
            enableColumnMenus: false,
            columnDefs: [
                { name: 'Id', field: 'Id', enableSorting: false, visible: false, cellClass: 'columCss' },
                { name: 'Generated Date', field: 'Date', enableSorting: false, cellClass: 'columCss' },
                { name: 'Plate Number', field: 'PlateNumber', enableSorting: false, cellClass: 'columCss' },
                { name: 'Tire Quantity', field: 'TireQuantity', enableSorting: false, cellClass: 'columCss' },
                { name: 'Invoice Code', field: 'Code', enableSorting: false, cellClass: 'columCssBold' },
                {
                    name: ' ',
                    cellTemplate: '<div class="center"><button ng-click="grid.appScope.loadInvoiceCodeDetails(row.entity.Id)" class="btn btn-xs btn-info">Load</button></div>',
                    width: 60,
                    enableSorting: false
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.pagination.on.paginationChanged($scope, function (newPage, pageSize) {
                    paginationOptionsDealerDealerInvoiceSearchGrid.pageNumber = newPage;
                    paginationOptionsDealerDealerInvoiceSearchGrid.pageSize = pageSize;
                    getInvoiceCodeSearchPage();
                });
            }
        };

        $scope.resetSearchCriterias = function () {
            $scope.dealerInvoiceCodeSearchParam = {
                date: '',
                plateNo: '',
                code: ''
            };
            getInvoiceCodeSearchPage();
        }
        $scope.refresInvoiceCodeSearchGridData = function () {
            getInvoiceCodeSearchPage();
        }
        function getInvoiceCodeSearchPage() {
            if (isGuid($scope.dealerInvoiceDetails.dealerId)) {
                $scope.gridDealerInvoiceCodeLoading = true;
                $scope.gridDealerInvoiceCodeloadAttempted = false;
                var InvoiceCodeSearchGridParam =
                    {
                        'paginationOptionsDealerDealerInvoiceSearchGrid': paginationOptionsDealerDealerInvoiceSearchGrid,
                        'dealerInvoiceSearchGridSearchCriterias': $scope.dealerInvoiceCodeSearchParam,
                        'dealerInvoiceFilterInformation': $scope.dealerInvoiceDetails
                    }
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/SearchDealerInvoiceCode',
                    data: JSON.stringify(InvoiceCodeSearchGridParam),
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    var responseArr = JSON.parse(data);
                    if (responseArr != null) {
                        $scope.gridOptionsInvoiceCode.data = responseArr.data;
                        $scope.gridOptionsInvoiceCode.totalItems = responseArr.totalRecords;
                    } else {
                        $scope.gridOptionsInvoiceCode.data = [];
                        $scope.gridOptionsInvoiceCode.totalItems = 0;
                    }
                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    $scope.gridDealerInvoiceCodeLoading = false;
                    $scope.gridDealerInvoiceCodeloadAttempted = true;
                });
            }
        };

        $scope.showImagePopup = function (itemId) {           

            AddNewCalculationPopup = ngDialog.open({
                template: 'popUpImage',
                className: 'ngdialog-theme-plain',
                closeByEscape: false,
                showClose: true,
                closeByDocument: false,
                scope: $scope,

            });
        }

        

        $scope.loadInvoiceCodeDetails = function (invoiceId) {
            if (isGuid(invoiceId)) {
               
                CodesearchPopup.close();
                swal({ title: 'Loading...', text: 'Invoice code information', showConfirmButton: false });
                //loading code information
                var data = {
                    'invoiceId': invoiceId,
                    'dealerId': $scope.dealerInvoiceDetails.dealerId,
                    'dealerBranchId': $scope.dealerInvoiceDetails.dealerBranchId,
                    'countryId': $scope.dealerInvoiceDetails.countryId,
                    'cityId': $scope.dealerInvoiceDetails.cityId,
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/LoadInvoceCodeDetailsById',
                    dataType: 'json',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    $scope.dealerInvoiceDetails.quantity = data.Quantity;
                    $scope.dealerInvoiceDetails.plateNumber = data.PlateNumber;

                    $scope.dealerInvoiceTireDetails.front.width = data.TireFront.Wide == 0 ? "" : data.TireFront.Wide;
                    $scope.dealerInvoiceTireDetails.front.cross = data.TireFront.Cross == 0 ? "" : data.TireFront.Cross;
                    $scope.dealerInvoiceTireDetails.front.diameter = data.TireFront.Diameter == 0 ? "" : data.TireFront.Diameter;
                    $scope.dealerInvoiceTireDetails.front.loadSpeed = data.TireFront.LoadSpeed;
                    $scope.dealerInvoiceTireDetails.front.serialLeft = data.TireFront.SerialLeft;
                    $scope.dealerInvoiceTireDetails.front.serialRight = data.TireFront.SerialRight;
                    $scope.dealerInvoiceTireDetails.front.pattern = data.TireFront.PatternRight;
                    $scope.dealerInvoiceTireDetails.front.pattern = data.TireFront.PatternLeft;

                    $scope.dealerInvoiceTireDetails.back.width = data.TireBack.Wide == 0 ? "" : data.TireBack.Wide;
                    $scope.dealerInvoiceTireDetails.back.cross = data.TireBack.Cross == 0 ? "" : data.TireBack.Cross;
                    $scope.dealerInvoiceTireDetails.back.diameter = data.TireBack.Diameter == 0 ? "" : data.TireBack.Diameter;
                    $scope.dealerInvoiceTireDetails.back.loadSpeed = data.TireBack.LoadSpeed;
                    $scope.dealerInvoiceTireDetails.back.serialLeft = data.TireBack.SerialLeft;
                    $scope.dealerInvoiceTireDetails.back.serialRight = data.TireBack.SerialRight;
                    $scope.dealerInvoiceTireDetails.back.pattern = data.TireBack.PatternRight;
                    $scope.dealerInvoiceTireDetails.back.pattern = data.TireBack.PatternLeft;

                    $scope.dataLoadMode = true;
                    goToStep(2);

                }).error(function (data, status, headers, config) {
                }).finally(function () {
                    swal.close();
                });;
            } else {
                customErrorMessage("Invalid invoice code selection.");
            }
        }

        $scope.currentStep = 1;
        $scope.form = {

            next: function (form) {

                $scope.toTheTop();
                if (form.$valid) {
                    nextStep();
                }
            },
            prev: function (form) {
                $scope.toTheTop();
                prevStep();
            },
            goTo: function (form, i) {

                if (parseInt($scope.currentStep) < parseInt(i)) {
                    var isValidated = false;
                    if ($scope.dealerInvoiceDetails.plateNumber !== '' &&
                        $scope.dealerInvoiceDetails.quantity > 0) {
                        isValidated = true
                    } else {
                        customErrorMessage("Please enter plate number and select a tire quantity.");
                    }
                    if (isValidated) {
                        goToStep(i);
                        $scope.toTheTop();
                    }
                } else {
                    $scope.toTheTop();
                    goToStep(i);
                }
            },
            submit: function () {
            },
            reset: function () {
            }
        };
        var nextStep = function () {
            $scope.currentStep++;
        };
        var prevStep = function () {
            $scope.currentStep--;
        };
        var goToStep = function (i) {
            $scope.currentStep = i;
        };


    });

app.directive('customPopover', function ($compile, $templateCache) {
    var getTemplate = function (contentType) {
        var template = '';
        switch (contentType) {
            case 'contentType1':
                template = $templateCache.get("customPopoverContent1.html");
                break;
            case 'contentType2':
                template = $templateCache.get("customPopoverContent2.html");
                break;
        }
        return template;
    };

    return {
        restrict: "A",
        scope: {
            contentType: '@',
            title: '@',

        },
        link: function (scope, element, attrs) {
            var contentType = scope.contentType;
            var popOverContent = getTemplate(contentType);
            popOverContent = $compile("<div>" + popOverContent + "</div>")(scope);
            // scope.$apply();  
            var options = {
                //title       : title,
                content: popOverContent,
                placement: "left",
                html: true,
                date: scope.date,
                trigger: "click"
            };
            $(element).popover(options);

        }
    };
})


app.config(['$tooltipProvider', function ($tooltipProvider) {
    $tooltipProvider.options({
        appendToBody: true, // 
        placement: 'bottom' // Set Default Placement
    });
}]);


app.directive('resize', function ($window) {
    return function (scope, element) {
        scope.appendToBody = true;
        var w = angular.element($window);
        scope.getWindowDimensions = function () {
            return {
                'h': w.height(),
                'w': w.width()
            };
        };
        scope.$watch(scope.getWindowDimensions, function (newValue, oldValue) {
            scope.windowHeight = newValue.h;
            scope.windowWidth = newValue.w;

            scope.appendToBody = (scope.windowWidth > 768) ? true : false;

            scope.style = function () {
                return {
                    'height': (newValue.h - 100) + 'px',
                    'width': (newValue.w - 100) + 'px'
                };
            };

        }, true);

        w.bind('resize', function () {
            scope.$apply();
        });
    }
})