app.controller('verifyEmailCtrl',
function ($scope, $http, $rootScope, $location, SweetAlert, $localStorage, $stateParams, $cookieStore) {
    $scope.requestId = $stateParams.requestId;
    $scope.tpaId = $stateParams.tpaId;
    $scope.customerId = $stateParams.customerId;
    $scope.tpaName = "";
    $scope.currentUserId = "";
    $scope.err = 0;
    
  
    $scope.pageValidityCheck = function () {
        //if ($scope.requestId != "" && $scope.tpaId != ""
        //    && isGuid($scope.requestId) && isGuid($scope.tpaId)) {
        //    $http({
        //        method: 'POST',
        //        url: '/TAS.Web/api/User/validateChangePassswordLink',
        //        data: $scope.linkData
        //    }).success(function (data, status, headers, config) {
        //        if (data == "") {
        //            $scope.denyAccess("expired");
        //        } else {
        //            $scope.currentUserId = data;
        //        }
        //    }).error(function (data, status, headers, config) {

        //    });
        //} else {
        //    $scope.denyAccess("invalid");
        //}


        //$http({
        //    method: 'POST',
        //    url: '/TAS.Web/api/ProductDisplay/GetTPANameById',
        //    data: { 'tpaId': $scope.tpaId }
        //}).success(function (data, status, headers, config) {
        //    $scope.tpaName = data;
        //}).error(function (data, status, headers, config) {

        //});


    }

    $scope.redirectHomePage = function () {
        //$location.path('http://www.leftfieldassurance.com');
       
            $location.path('login/signin/continental');
       
    }
});