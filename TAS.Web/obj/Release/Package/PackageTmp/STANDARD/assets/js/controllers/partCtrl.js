app.controller('PartCtrl',
    function ($scope, $rootScope, $http, ngDialog, SweetAlert, $localStorage, $cookieStore, $filter, toaster) {

        $scope.PartAreaSaveBtnIconClass = "";
        $scope.PartAreaSaveBtnDisabled = false;
        $scope.PartSaveBtnIconClass = "";
        $scope.PartSaveBtnDisabled = false;
        $scope.errorTab1 = "";
        $scope.IsAutomobile = true;

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }

        $scope.PartAreasByCommodityType = [];
        $scope.parts = [];
        $scope.partPrices = [];
        $scope.Dealers = [];
        $scope.Countries = [];

        $scope.Part = {
            Id: emptyGuid(),
            PartAreaId: emptyGuid(),
            MakeId: emptyGuid(),
            CommodityId: emptyGuid(),
            PartCode: '',
            PartName: '',
            PartNumber: '',
            AllocatedHours: 0,
            IsActive: true,
            ApplicableForAllModels: true
        };
        $scope.PartSuggestion = {
            PartAreaId: emptyGuid(),
            MakeId: emptyGuid(),
            CommodityId: emptyGuid(),
            PartId: emptyGuid()
        }
        $scope.PartPrice = {
            Id: emptyGuid(),
            CountryId: emptyGuid(),
            DealerId: emptyGuid(),
            CountryName: '',
            CurrencyName: '',
            DealerName: '',
            Price: '',
        };
        $scope.PartArea = {
            Id: emptyGuid(),
            CommodityTypeId: emptyGuid(),
            CommodityCategoryId: emptyGuid(),
            PartAreaCode: '',
            PartAreaName: ''
        };

        $scope.loadInitailData = function () {
            LoadDetails();
        }
        function LoadDetails() {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
            }).error(function (data, status, headers, config) {
            });
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/Claim/GetAllPartArea',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    angular.forEach(data, function (PartArea) {
            //        angular.forEach($scope.CommodityCategories, function (CommodityCategory) {
            //            if (PartArea.CommodityCategoryId == CommodityCategory.CommodityCategoryId) {
            //                PartArea.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
            //            }
            //        });

            //    })
            //    $scope.PartAreas = data;
            //    $scope.PartA = $scope.PartAreas.CommodityTypeId;
            //}).error(function (data, status, headers, config) {
            //});
            $scope.PartAreas = [];
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllMakes',
                headers: { 'Authorization': $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Makes = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealers',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Dealers = data;
            }).error(function (data, status, headers, config) {
            });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Customer/GetAllCountries',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, status, headers, config) {
            });
        }



        $scope.SetCommodityCategoryValues = function () {
            $scope.errorTab1 = "";
            if ($scope.PartArea.CommodityCategoryId != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetCommodityCategoryById',
                    data: $scope.PartArea,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.PartArea.PartAreaCode = '';
                    $scope.PartArea.PartAreaName = '';
                    $scope.CommodityCategory.CommodityCategoryId = data.CommodityCategoryId;
                    $scope.CommodityCategory.CommodityCategoryDescription = data.CommodityCategoryDescription;
                    $scope.CommodityCategory.CommodityCategoryCode = data.CommodityCategoryCode;
                    $scope.CommodityCategory.Length = data.Length;
                    $scope.PartArea.PartAreaCode = '';
                    $scope.PartArea.PartAreaName = '';
                }).error(function (data, status, headers, config) {
                    clearCommodityCategoryControls();
                });
            }
            else {
                clearCommodityCategoryControls();
            }
        }
        $scope.LoadFromComodityType = function () {

            if ($scope.PartArea.CommodityTypeId != undefined && $scope.PartArea.CommodityTypeId != '' && $scope.PartArea.CommodityTypeId != "00000000-0000-0000-0000-000000000000") {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/CommodityItemAttributes/GetAllCommodityCategories',
                    data: { "CommodityTypeId": $scope.PartArea.CommodityTypeId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.CommodityCategories = data;
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/GetAllPartArea',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        angular.forEach(data, function (PartArea) {
                            angular.forEach($scope.CommodityCategories, function (CommodityCategory) {
                                if (PartArea.CommodityCategoryId == CommodityCategory.CommodityCategoryId) {
                                    PartArea.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
                                }
                            });

                        })
                        $scope.PartAreas = data;
                        $scope.PartA = $scope.PartAreas.CommodityTypeId;
                    }).error(function (data, status, headers, config) {
                    });
                }).error(function (data, status, headers, config) {
                    clearCommodityCategoryControls();
                });
            } else {
                clearCommodityCategoryControls();
            }
        }

        $scope.validatePartArea = function () {
            var isValid = true;

            if ($scope.PartArea.PartAreaName == "" || $scope.PartArea.PartAreaName == undefined) {
                $scope.validate_PartAreaName = "has-error";
                isValid = false;
            } else {
                $scope.validate_PartAreaName = "";
            }
            if ($scope.PartArea.PartAreaCode == "" || $scope.PartArea.PartAreaCode == undefined) {
                $scope.validate_PartAreaCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_PartAreaCode = "";
            }
            if ($scope.PartArea.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CommodityTypeId = "has-error";
                isValid = false;
            }
            else {
                $scope.validate_CommodityTypeId = "";
            }
            if ($scope.PartArea.CommodityCategoryId == "00000000-0000-0000-0000-000000000000") {
                $scope.validate_CommodityCategoryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommodityCategoryId = "";
            }

            return isValid
        }

        $scope.PartAreaSubmit = function () {

            if ($scope.validatePartArea()) {

                $scope.errorTab1 = "";
                if ($scope.PartArea.Id == null || $scope.PartArea.Id == "00000000-0000-0000-0000-000000000000") {

                    $scope.PartAreaSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.PartAreaSaveBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/AddPartArea',
                        data: $scope.PartArea,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.PartAreaSaveBtnIconClass = "";
                        $scope.PartAreaSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: "Part Area Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });

                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/Claim/GetAllPartArea',
                            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //}).success(function (data, status, headers, config) {
                            //    angular.forEach(data, function (PartArea) {
                            //        angular.forEach($scope.CommodityCategories, function (CommodityCategory) {
                            //            if (PartArea.CommodityCategoryId == CommodityCategory.CommodityCategoryId) {
                            //                PartArea.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
                            //            }
                            //        });

                            //    })
                            //    $scope.PartAreas = data;
                            //}).error(function (data, status, headers, config) {
                            //});
                            clearPartAreaControls();
                            $scope.clearPartDetails();
                        } else {

                        }

                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Part Area Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.PartAreaSaveBtnIconClass = "";
                        $scope.PartAreaSaveBtnDisabled = false;

                        return false;
                    });


                } else {
                    $scope.PartAreaSaveBtnIconClass = "fa fa-spinner fa-spin";
                    $scope.PartAreaSaveBtnDisabled = true;

                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/Claim/UpdatePartArea',
                        data: $scope.PartArea,
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.Ok = data;
                        $scope.PartAreaSaveBtnIconClass = "";
                        $scope.PartAreaSaveBtnDisabled = false;
                        if (data == "OK") {
                            SweetAlert.swal({
                                title: " Part Area Information",
                                text: "Successfully Saved!",
                                confirmButtonColor: "#007AFF"
                            });
                            //$http({
                            //    method: 'POST',
                            //    url: '/TAS.Web/api/Claim/GetAllPartArea',
                            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            //}).success(function (data, status, headers, config) {
                            //    angular.forEach(data, function (PartArea) {
                            //        angular.forEach($scope.CommodityCategories, function (CommodityCategory) {
                            //            if (PartArea.CommodityCategoryId == CommodityCategory.CommodityCategoryId) {
                            //                PartArea.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
                            //            }
                            //        });

                            //    })
                            //    $scope.PartAreas = data;
                            //}).error(function (data, status, headers, config) {
                            //});
                            $scope.PartAreas = [];
                            clearPartAreaControls();
                            $scope.clearPartDetails();
                        } else {
                        }
                        return false;
                    }).error(function (data, status, headers, config) {
                        SweetAlert.swal({
                            title: "Part Area Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        $scope.PartAreaSaveBtnIconClass = "";
                        $scope.PartAreaSaveBtnDisabled = false;
                        return false;
                    });
                }
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }
        $scope.PartAreaValues = function () {
            $scope.errorTab1 = "";
            if ($scope.PartArea.Id != null) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetPartAreaById',
                    data: { "Id": $scope.PartArea.Id },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.PartArea.Id = data.Id;
                    $scope.PartArea.CommodityTypeId = data.CommodityTypeId;
                    $scope.PartArea.CommodityCategoryId = data.CommodityCategoryId;
                    $scope.PartArea.PartAreaCode = data.PartAreaCode;
                    $scope.PartArea.PartAreaName = data.PartAreaName;
                    $scope.LoadFromComodityType();
                }).error(function (data, status, headers, config) {
                    clearPartAreaControls();
                });
            }
            else {
                clearPartAreaControls();
            }
        }

        function clearPartAreaControls() {
            $scope.PartArea = {
                Id: emptyGuid(),
                CommodityTypeId: emptyGuid(),
                CommodityCategoryId: emptyGuid(),
                PartAreaCode: '',
                PartAreaName: ''
            };
        }

        $scope.selectedCommodityTypeChanged = function () {
            $scope.Part.Id = emptyGuid();
            $scope.Part.PartAreaId = emptyGuid();
            $scope.Part.MakeId = emptyGuid();

            if (isGuid($scope.Part.CommodityId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetAllPartAreasByCommodityTypeId',
                    data: { "commodityTypeId": $scope.Part.CommodityId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        customErrorMessage("No part areas found.");
                    $scope.PartAreasByCommodityType = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.selectedPartAreaChanged = function () {
            $scope.getAllParts();
        }

        $scope.getAllParts = function () {
            if (isGuid($scope.Part.PartAreaId)
                && isGuid($scope.Part.MakeId)
                && isGuid($scope.Part.CommodityId)) {
                var data = {
                    partAreaId: $scope.Part.PartAreaId,
                    makeId: $scope.Part.MakeId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetAllPartsByMakePartArea',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        customErrorMessage("No parts found.");
                    $scope.parts = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.validateExitPart = function () {            
            
            var Res = true;

            angular.forEach($scope.parts, function (eachpart) {
                if ($scope.Part.PartNumber == eachpart.PartNumber) {
                    Res = false;                    
                }
            });
            return Res;
           
        }

        $scope.validatePart = function () {
            var isValid = true;
            if (!isGuid($scope.Part.CommodityId)) {
                $scope.validate_partCommodityType = "has-error";
                isValid = false;
            } else {
                $scope.validate_partCommodityType = "";
            }

            if (!isGuid($scope.Part.PartAreaId)) {
                $scope.validate_partPartArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partPartArea = "";
            }

            if (!isGuid($scope.Part.MakeId)) {
                $scope.validate_partMakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_partMakeId = "";
            }

            if ($scope.Part.PartName == '') {
                $scope.validate_partName = "has-error";
                isValid = false;
            } else {
                $scope.validate_partName = "";
            }

            if ($scope.Part.PartCode == '') {
                $scope.validate_partCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_partCode = "";
            }

            if ($scope.Part.PartNumber == '') {
                $scope.validate_partNumber = "has-error";
                isValid = false;
            } else {
                $scope.validate_partNumber = "";
            }

            if (!parseFloat($scope.Part.AllocatedHours)) {
                $scope.Part.AllocatedHours = 0.00;
            }
            return isValid;

        }
        $scope.savePart = function () {
            if ($scope.validatePart()) {
                //if ($scope.validateExitPart()) {
                $scope.PartSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.PartSaveBtnDisabled = true;

                $http({
                    method: 'POST',
                    url: $scope.Part.Id != emptyGuid() ? '/TAS.Web/api/Claim/UpdatePart' : '/TAS.Web/api/Claim/AddPart',
                    data: {
                        'Part': $scope.Part,
                        'PartPrice': $scope.partPrices,
                        'UserId': $rootScope.LoggedInUserId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.PartSaveBtnIconClass = "";
                    $scope.PartSaveBtnDisabled = false;
                    if (data == "ok") {
                        SweetAlert.swal({
                            title: "Part Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });
                        $scope.resetForAddPartData();
                        $scope.getAllParts();
                    } else if (data == "DuplicatePrtNo") {
                        SweetAlert.swal({
                            title: "Part Information",
                            text: "Part no could not be duplicate",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                    else {
                        SweetAlert.swal({
                            title: "Part Information",
                            text: data,
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }

                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: "Part Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                    $scope.PartSaveBtnIconClass = "";
                    $scope.PartSaveBtnDisabled = false;

                    return false;
                });
                //}
                //else {
                //    customErrorMessage("Part no could not be duplicate.")
                //}
            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }

        $scope.validatePartPrice = function () {
            var isValid = true;
            if (!isGuid($scope.PartPrice.DealerId)) {
                $scope.validate_partPriceDealerId = "has-error";
                isValid = false;
            } else {
                $scope.validate_partPriceDealerId = "";
            }

            if (!isGuid($scope.PartPrice.CountryId)) {
                $scope.validate_partPriceCountryId = "has-error";
                isValid = false;
            } else {
                $scope.validate_partPriceCountryId = "";
            }

            if (parseFloat($scope.PartPrice.Price) && parseFloat($scope.PartPrice.Price) > 0) {
                $scope.validate_partPrice = "";
            } else {
                $scope.validate_partPrice = "has-error";
                isValid = false;
            }

            if ($scope.PartPrice.CurrencyName == '') {
                customErrorMessage("Selected dealer's currency not found.");
                isValid = false;
            } else {

            }

            return isValid;
        }
        $scope.addPartPrice = function () {
            if ($scope.validatePartPrice()) {
                $scope.PartPrice.Id = getNextAvailablePartPriceId();
                var partPriceObj = {
                    Id: $scope.PartPrice.Id,
                    CountryId: $scope.PartPrice.CountryId,
                    DealerId: $scope.PartPrice.DealerId,
                    CountryName: $scope.PartPrice.CountryName,
                    CurrencyName: $scope.PartPrice.CurrencyName,
                    DealerName: $scope.PartPrice.DealerName,
                    Price: $scope.PartPrice.Price,
                };
                $scope.partPrices.push(partPriceObj);
                $scope.clearPartPrice();

            } else {
                customErrorMessage("Please fill valid data for highlighted fields.")
            }
        }
        $scope.deletePartPrice = function (id) {
            if (parseInt(id)) {

                $scope.partPrices.splice((id - 1), 1);
                //resetting id
                angular.forEach($scope.partPrices, function (partPrice) {
                    if (partPrice.Id > id) {
                        partPrice.Id--;
                    }
                });
            }
        }
        $scope.clearPartPrice = function () {
            $scope.PartPrice = {
                Id: emptyGuid(),
                CountryId: emptyGuid(),
                DealerId: emptyGuid(),
                CountryName: '',
                CurrencyName: '',
                DealerName: '',
                Price: '',
            };
        }
        $scope.clearPartDetails = function () {
            $scope.Part = {
                Id: emptyGuid(),
                PartAreaId: emptyGuid(),
                MakeId: emptyGuid(),
                CommodityId: emptyGuid(),
                PartCode: '',
                PartName: '',
                PartNumber: '',
                AllocatedHours: 0,
                IsActive: true,
                ApplicableForAllModels: true
            };
        }

        $scope.resetForAddPartData = function () {
            $scope.Part.Id = emptyGuid();
            $scope.Part.PartCode = '';
            $scope.Part.PartName = '';
            $scope.Part.PartNumber = '';
            $scope.Part.AllocatedHours = 0;
            $scope.Part.IsActive = true;
            $scope.Part.ApplicableForAllModels = true;
            $scope.clearPartPrice();
            $scope.partPrices = [];
        }
        $scope.selectedMakeIdChanged = function () {

            //$scope.Part.PartAreaId = "";
            $scope.Part.Id = "";
            $scope.Part.CommodityTypeId = "";
            $scope.Part.CommodityCategoryId = "";
            $scope.Part.PartAreaCode = false;
            $scope.Part.PartAreaName = false;
            $scope.Part.AllocatedHours = "";
            //$scope.Part.PartAreaId = "";
            $scope.PartDetails = [];

            if ($scope.Part.MakeId != null) {

                //angular.forEach($scope.CommodityTypes, function (value) {
                //    if (value.CommodityTypeDescription == 'Automobile' && $scope.Part.CommodityId == value.CommodityTypeId) {
                //        $scope.IsAutomobile = true;
                //    }
                //});
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetPartByPartAreaId',
                    data: { "Id": $scope.Part.PartAreaId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.PartDetails = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }


        //part suggestion
        $scope.selectedCommodityTypeChangedPartSuggestion = function () {
            $scope.PartSuggestion.PartAreaId = emptyGuid();
            $scope.PartSuggestion.MakeId = emptyGuid();

            if (isGuid($scope.PartSuggestion.CommodityId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetAllPartAreasByCommodityTypeId',
                    data: { "commodityTypeId": $scope.PartSuggestion.CommodityId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        customErrorMessage("No part areas found.");
                    $scope.PartAreasByCommodityTypePartSuggestion = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.selectedPartAreaChangedPartSuggestion = function () {
            $scope.GetPartsForPartSuggestion();
        }

        $scope.selectedPartChangedPartSuggestion = function () {
            $scope.relatedPartsSelectionInPartSuggestion = [];
            var allPartSuggestionData = angular.copy($scope.purePartsInSuggestion);
            if (isGuid($scope.PartSuggestion.PartId)) {
                for (var i = 0; i < allPartSuggestionData.length; i++) {

                    if (allPartSuggestionData[i].Id == $scope.PartSuggestion.PartId) {
                        allPartSuggestionData.splice(i, 1);
                        $scope.relatedPartsSelectionInPartSuggestion = allPartSuggestionData;
                    }
                }

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetAllRelatedPartsByPartId',
                    data: { 'partId': $scope.PartSuggestion.PartId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {

                    angular.forEach($scope.relatedPartsSelectionInPartSuggestion, function (suggestedPart) {
                        suggestedPart.quantity = 1;
                        suggestedPart.isSelected = false;
                        if (data != null) {
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].SuggestedPartId == suggestedPart.Id) {
                                    suggestedPart.quantity = data[i].Quantity;
                                    suggestedPart.isSelected = true;
                                }
                            }
                        }
                    });

                }).error(function (data, status, headers, config) {
                });
            }
        }
        $scope.selectedMakeChangedPartSuggestion = function () {
            $scope.GetPartsForPartSuggestion();
        }

        $scope.GetPartsForPartSuggestion = function () {
            if (isGuid($scope.PartSuggestion.PartAreaId)
                && isGuid($scope.PartSuggestion.MakeId)
                && isGuid($scope.PartSuggestion.CommodityId)) {

                var data = {
                    partAreaId: $scope.PartSuggestion.PartAreaId,
                    makeId: $scope.PartSuggestion.MakeId
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetAllPartsByMakePartArea',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null) {
                        customErrorMessage("No parts found.");

                        $scope.PartSuggestion.PartId = emptyGuid();
                    } else {

                    }
                    $scope.purePartsInSuggestion = data;
                    $scope.partsInSuggestion = data;
                }).error(function (data, status, headers, config) {
                });
            }
        }

        $scope.savePartSuggestion = function () {
            if ($scope.validatePartSuggestions()) {
                $scope.PartSuggestionSaveBtnIconClass = "fa fa-spinner fa-spin";
                $scope.PartSuggestionSaveBtnDisabled = true;
                var data = {
                    partSuggestion: $scope.PartSuggestion,
                    relatedParts: $scope.relatedPartsSelectionInPartSuggestion
                };
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/SavePartSuggestion',
                    data: data,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == "ok") {
                        SweetAlert.swal({
                            title: "TAS Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });
                        $scope.clearPartSuggestions();
                    } else {
                        SweetAlert.swal({
                            title: "TAS Information",
                            text: data,
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                    }
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: "TAS Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                }).finally(function () {
                    $scope.PartSuggestionSaveBtnIconClass = "";
                    $scope.PartSuggestionSaveBtnDisabled = false;
                });
            } else {
                customErrorMessage("Please fill valid values for highlighted.");
            }
        }

        $scope.validatePartSuggestions = function () {
            var isValid = true;
            if (!isGuid($scope.PartSuggestion.CommodityId)) {
                $scope.validate_partSuggestionCommodityType = "has-error";
                isValid = false;
            } else {
                $scope.validate_partSuggestionCommodityType = "";
            }

            if (!isGuid($scope.PartSuggestion.PartAreaId)) {
                $scope.validate_partSuggestionPartArea = "has-error";
                isValid = false;
            } else {
                $scope.validate_partSuggestionPartArea = "";
            }

            if (!isGuid($scope.PartSuggestion.MakeId)) {
                $scope.validate_partSuggestionMakeId = "has-error";
                isValid = false;
            } else {
                $scope.validate_partSuggestionMakeId = "";
            }


            if (!isGuid($scope.PartSuggestion.PartId)) {
                $scope.validate_partSuggestionPartId = "has-error";
                isValid = false;
            } else {
                $scope.validate_partSuggestionPartId = "";
            }

            //validate selected
            angular.forEach($scope.relatedPartsSelectionInPartSuggestion, function (suggestion) {
                if (suggestion.isSelected) {
                    if (!parseInt(suggestion.quantity) || parseInt(suggestion.quantity) == 0) {
                        suggestion.class = "has-error";
                        isValid = false;

                    } else {
                        suggestion.class = "";
                        suggestion.quantity = parseInt(suggestion.quantity);
                    }


                }
            })

            return isValid;
        }

        $scope.clearPartSuggestions = function () {
            $scope.relatedPartsSelectionInPartSuggestion = [];
            $scope.PartSuggestion.PartId = emptyGuid();
        }



        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        }
        var customErrorMessage = function (msg) {
            toaster.pop('error', 'Error', msg);
        };
        var getNextAvailablePartPriceId = function () {
            return $scope.partPrices.length + 1;
        }
        var customInfoMessage = function (msg) {
            toaster.pop('info', 'Information', msg, 12000);
        };

        $scope.selectedPartAreaChanged = function () {
            //$scope.part.selected = undefined;
            //$scope.currentPart.partNumber = '';
            //$scope.currentPart.partQty = 1;
            //$scope.currentPart.isRelatedPartsAvailable = false;
            if (isGuid($scope.Part.PartAreaId) && isGuid($scope.Part.MakeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllPartsByPartAreaMakeId',
                    data: {
                        "partAreaId": $scope.Part.PartAreaId,
                        "makeId": $scope.Part.MakeId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        // customErrorMessage("No parts found for selected category.");
                        $scope.parts = data;
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                });
            } else {

            }
        }

        var paginationOptionsPriceSearchGrid = {
            pageNumber: 1,
            pageSize: 25,
            sort: null
        };

        var PartPopup;
        $scope.viewPartPricePopup = function () {
            PartPopup = ngDialog.open({
                template: 'popUpPartPrice',
                className: 'ngdialog-theme-plain',
                closeByEscape: true,
                showClose: true,
                closeByDocument: true,
                scope: $scope
            });
        }


        $scope.loadPart = function (PartId) {
            if (isGuid(PartId)) {
                $scope.formAction = false;
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/Claim/GetPartById',
                    data: { "Id": PartId },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Part.Id = data.Id;
                    $scope.Part.CommodityId = data.CommodityId;
                    $scope.Part.PartAreaId = data.PartAreaId;
                    $scope.Part.MakeId = data.MakeId;
                    $scope.Part.PartCode = data.PartCode;
                    $scope.Part.PartName = data.PartName;
                    $scope.Part.PartNumber = data.PartNumber;
                    $scope.Part.AllocatedHours = data.AllocatedHours;
                    $scope.Part.IsActive = data.IsActive;
                    $scope.Part.ApplicableForAllModels = data.ApplicableForAllModels;
                    $scope.partPrices = data.PartPrices;
                }).error(function (data, status, headers, config) {

                });
            }
        }

        $scope.selectedPartAreaChanged = function () {

            $scope.Part.PartName = '';
            $scope.Part.PartNumber = '';
            if (isGuid($scope.Part.PartAreaId) && isGuid($scope.Part.MakeId)) {
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/claim/GetAllPartsByPartAreaMakeId',
                    data: {
                        "partAreaId": $scope.Part.PartAreaId,
                        "makeId": $scope.Part.MakeId
                    },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    if (data == null)
                        $scope.errorTab2 = "No parts found for selected category.";

                    $scope.parts = data;
                }).error(function (data, status, headers, config) {

                }).finally(function () {
                });
            } else {

            }
        }


        $scope.dealersByCountry = [];
        $scope.selectedDealerChanged = function () {

            if (isGuid($scope.PartPrice.DealerId)) {
                angular.forEach($scope.Dealers, function (dealer) {
                    if ($scope.PartPrice.DealerId == dealer.Id) {
                        $scope.PartPrice.DealerName = dealer.DealerName;
                        $scope.PartPrice.CountryId = dealer.CountryId;
                        if (isGuid(dealer.CurrencyId)) {
                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                                data: { "Id": dealer.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.partPriceCurrency = data.Code;
                                $scope.PartPrice.CurrencyName = data.Code;
                            });

                        } else {
                            customErrorMessage("Selected dealer's currency not found.");
                        }

                        angular.forEach($scope.Countries, function (country) {
                            if ($scope.PartPrice.CountryId == country.Id) {
                                $scope.PartPrice.CountryName = country.CountryName;
                            }
                        });
                    }
                });
            } else {
                $scope.PartPrice.DealerName = '';
                $scope.PartPrice.CountryId = emptyGuid();
                $scope.partPriceCurrency = ''
            }
        }
        $scope.selectedCountryChanged = function () {
            $http({
                method: 'POST',
                url: '/TAS.Web/api/DealerManagement/GetAllDealersByCountryId',
                data: { "Id": $scope.PartPrice.CountryId },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.dealersByCountry = data;
                if (isGuid($scope.PartPrice.CountryId)) {
                    angular.forEach($scope.dealersByCountry, function (value) {
                        if ($scope.PartPrice.DealerId == value.Id) {
                            //$scope.Vehicle.insurerId = value.InsurerId;
                            $scope.PartPrice.CurrencyId = value.CurrencyId;


                            $http({
                                method: 'POST',
                                url: '/TAS.Web/api/CurrencyManagement/GetCurrencyById',
                                data: { "Id": value.CurrencyId },
                                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                            }).success(function (data, status, headers, config) {
                                $scope.BnWCurrency = data.Code;
                                $http({
                                    method: 'POST',
                                    url: '/TAS.Web/api/CurrencyManagement/GetCurrencyRateAvailabilityByCurrencyId',
                                    data: { "Id": value.CurrencyId },
                                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                                }).success(function (data, status, headers, config) {
                                    if (data == false) {
                                        SweetAlert.swal({
                                            title: "TAS Information",
                                            text: "Selected dealer's currency(" + $scope.BnWCurrency + ") is not defined in the current currency conversion period.Please update it before proceeding.",
                                            type: "warning",
                                            confirmButtonColor: "rgb(221, 107, 85)"
                                        });
                                    }
                                }).error(function (data, status, headers, config) {
                                });

                            }).error(function (data, status, headers, config) {
                            });
                            return false;
                        }
                    });
                }
            }).error(function (data, status, headers, config) {
            });

        }

    });