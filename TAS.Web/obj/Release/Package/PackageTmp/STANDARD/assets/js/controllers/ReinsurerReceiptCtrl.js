app.controller('ReinsurerReceiptCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {

        $scope.ModalName = "Reinsurer Receipt";
        $scope.ModalDescription = "You can record reinsure receipt here.";
        $scope.BordrxPaymtSaveBtnIconClass = "";
        $scope.BordrxPaymtSaveBtnDisabled = false;
        $scope.errorTab1 = "";

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };



        function LoadDetails() {
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
                url: '/TAS.Web/api/ReinsurerReceiptManagement/GetAllBordxs',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Bordxs = data;
            }).error(function (data, status, headers, config) {
            });
           
        }

        LoadDetails();
        $scope.reinsurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            ReinsurerCode: "",
            ReinsurerName: ""
        };
        $scope.insurer = {
            Id: "00000000-0000-0000-0000-000000000000",
            InsurerCode: "",
            InsurerShortName: ""
        };
        $scope.Bordx = {
            Id: "00000000-0000-0000-0000-000000000000",
            BordxYear: "",
            Bordxmonth: "",
            BordxNumber: ""
        };
        $scope.BordxNum = {
            Id: "00000000-0000-0000-0000-000000000000",
            BordxNumber: ""
        };
        $scope.BordxPayment = {
            Id: "00000000-0000-0000-0000-000000000000",
            BordxAmount: "",
            BalanceAmount: "",
            PaidAmount: "",
            InvoiceReceivedDate: "",
            RefNo:"",
        };

        function clearCliamBordxControls() {
            $scope.reinsurer = {
                Id: "00000000-0000-0000-0000-000000000000",
                ReinsurerCode: "",
                ReinsurerName: ""
            };
            $scope.insurer = {
                Id: "00000000-0000-0000-0000-000000000000",
                InsurerCode: "",
                InsurerShortName: ""
            };
            $scope.Bordx = {
                Id: "00000000-0000-0000-0000-000000000000",
                BordxYear: "",
                Bordxmonth: "",
                BordxNumber: ""
            };
            $scope.BordxNum = {
                Id: "00000000-0000-0000-0000-000000000000",
                BordxNumber: ""
            };
        }

        function ClearBordxPaymentControls()
        {
            $scope.BordxPayment = {
                Id: "00000000-0000-0000-0000-000000000000",
                BordxAmount: "",
                BalanceAmount: "",
                PaidAmount: "",
                RefNo: ""
            };

        }
        
        $scope.GetBordxNum = function () {
            $scope.errorTab1 = "";
            if ($scope.reinsurer.Id != "00000000-0000-0000-0000-000000000000" && $scope.reinsurer.Id != null
                && $scope.insurer.Id != "00000000-0000-0000-0000-000000000000" && $scope.insurer.Id != null
                && $scope.Bordx.BordxYear != "" && $scope.Bordx.BordxYear != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerReceiptManagement/GetBordxsById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: { "ReId": $scope.reinsurer.Id, "InId": $scope.insurer.Id, "Year": $scope.Bordx.BordxYear }
                }).success(function (data, status, headers, config) {
                    $scope.BordxPayments = data;
                }).error(function (data, status, headers, config) {
                    clearCliamBordxControls();
                });
            }
            else {
                clearCliamBordxControls();
            }
        }

        $scope.GetBordxPayment = function () {
            $scope.errorTab1 = "";
            if ($scope.BordxNum.Id != "00000000-0000-0000-0000-000000000000" && $scope.BordxNum.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/ReinsurerReceiptManagement/GetBordxPaymmentById',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                    data: {'ClaimBordxId': $scope.BordxNum.Id }
                }).success(function (data, status, headers, config) {
                    $scope.BordxPayments = data;
                }).error(function (data, status, headers, config) {
                   clearCliamBordxControls();
                });
            }
            else {
                  clearCliamBordxControls();
            }
        }

        $scope.saveBordxPayment = function () {
            if ($scope.validateBordxPayment()) {
                if ($scope.BordxPayment.Id != null && $scope.BordxPayment.Id != "00000000-0000-0000-0000-000000000000" && parseFloat($scope.BordxPayment.PaidAmount) > 0) {
                    $scope.BordrxPaymtSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.BordrxPaymtSaveBtnDisabled = true;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/ReinsurerReceiptManagement/AddBordxPayment',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                        data: { 'BordxPayment': $scope.BordxPayment, 'ClaimBordxId': $scope.BordxPayment.Id }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.BordrxPaymtSaveBtnIconClass = "";
                        $scope.BordrxPaymtSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "BordxPayment Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $scope.GetBordxNum();
                            ClearBordxPaymentControls();
                        }
                        else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "BordxPayment Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.BordrxPaymtSaveBtnIconClass = "";
                        $scope.BordrxPaymtSaveBtnDisabled = false;
                        return false;
                    });
                }
              
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }

        $scope.validateBordxPayment = function () {
            var isValid = true;
           
            if ($scope.BordxPayment.PaidAmount == '') {
                $scope.validate_PaidAmount = "has-error";
                isValid = false;
            } else {
                $scope.validate_PaidAmount = "";
            }
            if ($scope.BordxPayment.RefNo == '') {
                $scope.validate_RefNo = "has-error";
                isValid = false;
            } else {
                $scope.validate_RefNo = "";
            }

            return isValid;
        }


        $scope.SetBordxPayments = function (Id, BordxAmount,BalanceAmount,PaidAmount,refNo) {
           
            $scope.BordxPayment.Id = Id;
            $scope.BordxPayment.BalanceAmount = parseFloat(PaidAmount) == 0.00 ? 0.00 : BalanceAmount;
            $scope.BordxPayment.BordxAmount = BordxAmount
            $scope.BordxPayment.PaidAmount = parseFloat(PaidAmount) == 0.00 ? BordxAmount : PaidAmount;
            $scope.BordxPayment.RefNo = refNo;

        }

        $scope.calBalance = function () {
            $scope.BordxPayment.BalanceAmount = parseFloat($scope.BordxPayment.PaidAmount) - parseFloat($scope.BordxPayment.BordxAmount) ;
        }
    }); 