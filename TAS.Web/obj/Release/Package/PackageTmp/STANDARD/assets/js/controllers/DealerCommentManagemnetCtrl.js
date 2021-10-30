
app.controller('DealerCommentManagemnetCtrl',
    function ($scope, $rootScope, $http, ngDialog, $location, SweetAlert, $localStorage, toaster) {

        //supportive functions
        var isGuid = function (stringToTest) {
            var validGuid = /^({|()?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}(}|))?$/;
            var emptyGuid = /^({|()?0{8}-(0{4}-){3}0{12}(}|))?$/;
            return validGuid.test(stringToTest) && !emptyGuid.test(stringToTest);
        };
        var emptyGuid = function () {
            return "00000000-0000-0000-0000-000000000000";
        };
        $scope.clearAll = function () {
            $scope.dealercommentView = {
                CommentCode: '',
                Comment: '',
                isrejectiontype: false
            };
        }


        $scope.dealercommentView = {
            id: emptyGuid(),
            CommentCode: '',
            Comment: '',
            isrejectiontype: false
        };

        $scope.validateDealerComment = function () {

            var isValid = true;
            if (!isGuid($scope.dealercommentView.id)) {
                $scope.validate_dealercommentView = "has-error";
                // isValid = false;
            } else {
                $scope.validate_dealercommentView = "";
            }

            if ($scope.dealercommentView.CommentCode == "") {
                $scope.validate_CommentCode = "has-error";
                isValid = false;
            } else {
                $scope.validate_CommentCode == "";
            }
            if ($scope.dealercommentView.Comment == "") {
                $scope.validate_Comment = "has-error";
                isValid = false;
            } else {
                $scope.validate_Comment == "";
            }

        };


        $scope.submitDealerComment = function () {

           // if ($scope.validateDealerComment()) {

                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/DealerManagement/AddDealerComment',
                    data: $scope.dealercommentView,
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.Ok = data;
                    $scope.clearAll();
                  
                    if (data == "OK") {
                        SweetAlert.swal({
                            title: "Dealer comment Information",
                            text: "Successfully Saved!",
                            confirmButtonColor: "#007AFF"
                        });


                    } else {
                        SweetAlert.swal({
                            title: "Dealer comment Information",
                            text: "Error occured while saving data!",
                            type: "warning",
                            confirmButtonColor: "rgb(221, 107, 85)"
                        });
                        return false;
                    }

                    return false;
                }).error(function (data, status, headers, config) {
                    SweetAlert.swal({
                        title: "Dealer comment Information",
                        text: "Error occured while saving data!",
                        type: "warning",
                        confirmButtonColor: "rgb(221, 107, 85)"
                    });
                  

                    return false;
                });

           // }
        }

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


      
    });


   