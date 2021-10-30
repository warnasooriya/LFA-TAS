app.controller('PartRejectionTypeCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {

        $scope.PartRejectionSaveBtnIconClass = "";
        $scope.PartRejectionSaveBtnDisabled = false;
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

        $scope.PartRejectionType = {
            Id: emptyGuid(),
            Code: '',
            Description: ''
            
        };

        function ClearPartRejectionType() {
            $scope.PartRejectionType = {
                Id: emptyGuid(),
                Code: '',
                Description: ''
            };
        }

        $scope.loadInitailData = function () {
            LoadDetails();
        }

        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Claim/GetAllPartRejectioDescription',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Description = data;
            }).error(function (data, status, headers, config) {
            });
        }

        $scope.validatePartRejection = function () {
            var isValid = true;
            if ($scope.PartRejectionType.Code == "") {
                $scope.validate_partRejectionCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_partRejectionCode = "";
            }

            if ($scope.PartRejectionType.Description == "") {
                $scope.validate_partRejectionDescription = "has-error";
                isValid = false;
            } else {
                $scope.validate_partRejectionDescription = "";
            }

            //if (!isGuid($scope.PartRejectionType.PartId)) {
            //    $scope.validate_partRejectionPartId = "has-error";
            //    isValid = false;
            //} else {
            //    $scope.validate_partRejectionPartId = "";
            //}

            return isValid;

        }

        $scope.PartRejectionSubmit = function () {

            if ($scope.validatePartRejection()) {

                $scope.PartRejectionSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.PartRejectionSaveBtnDisabled = true;


                if ($scope.PartRejectionType.Id == null || $scope.PartRejectionType.Id == "00000000-0000-0000-0000-000000000000") {
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/AddPartRejection',
                        data: $scope.PartRejectionType,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.PartRejectionSaveBtnIconClass = "";
                        $scope.PartRejectionSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "Part Rejection Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Claim/GetAllPartRejectioDescription',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Description = data;
                            }).error(function (data, status, headers, config) {
                            });

                            ClearPartRejectionType();

                        } else {
                            SweetAlert.swal({
                                title: "Part Rejection Information",
                                text: data,
                                type: "warning",
                                confirmButtonColor: "rgb(221, 107, 85)"
                            });
                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Part Rejection Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.PartRejectionSaveBtnIconClass = "";
                        $scope.PartRejectionSaveBtnDisabled = false;

                        return false;
                    });


                } else {
                    $scope.PartRejectionSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.PartRejectionSaveBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/UpdatePartRejectioDescription',
                        data: $scope.PartRejectionType,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.PartRejectionSaveBtnIconClass = "";
                        $scope.PartRejectionSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: " Part Rejection Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/Claim/GetAllPartRejectioDescription',
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.Description = data;
                            }).error(function (data, status, headers, config) {
                            });
                            ClearPartRejectionType();
                        } else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Part Rejection Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.PartRejectionSaveBtnIconClass = "";
                        $scope.PartRejectionSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }

            }

        $scope.PartRejectionTypeValues = function () {
            
            if ($scope.PartRejectionType.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetPartRejectionTypeById',
                    data: { "Id": $scope.PartRejectionType.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.PartRejectionType.Id = data.Id;
                    $scope.PartRejectionType.Code = data.Code;
                    $scope.PartRejectionType.Description = data.Description;
                }).error(function (data, status, headers, config) {
                    ClearPartRejectionType();
                });
            }
            else {
                ClearPartRejectionType();
            }
        }


    });