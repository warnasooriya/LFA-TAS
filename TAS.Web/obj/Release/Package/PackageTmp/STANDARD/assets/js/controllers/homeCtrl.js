app.controller('HomeCtrl',
  function ($scope, $http) {
      $scope.SetCountryValue = function () {

          //alert($scope.country.code);
      };
       
      $scope.GetCommodityCategoriesByCommodityType = function () {

          //alert($scope.commodityType.Id);
          if (typeof $scope.commodityType.Id != 'undefined') {
              var data = { 'ID': $scope.commodityType.Id };
              $.ajax({
                  type: "POST",
                  data: JSON.stringify(data),
                  url: "/api/Home/GetCommodityCategoriesByCommodityType",
                  contentType: "application/json"
              }).success(function (data, status, headers, config) {
                  $scope.commodityCategories = data;
              }).error(function (data, status, headers, config) {
                  //$scope.message = 'Unexpected Error';
              });


          } else {

          }
      };
      $scope.GetModelsByMakeId = function () {

          if (typeof $scope.Manufacturer.Id != 'undefined') {
              var data = { 'ID': $scope.Manufacturer.Id };
              $.ajax({
                  type: "POST",
                  data: JSON.stringify(data),
                  url: "/api/Home/GetModelsByMakeId",
                  contentType: "application/json"
              }).success(function (data, status, headers, config) {
                  $scope.commodityCategories = data;
              }).error(function (data, status, headers, config) {
                  //$scope.message = 'Unexpected Error'; 
              });


          } else {

          }
      };
      $scope.country = {
          code: ""
      };
      $scope.commodityType = {
          Id: ""
      };
     
      var LoadDetails = function () {
          $http({
              method: 'POST',
              url: 'api/Home/GetAllCountries'
          }).success(function (data, status, headers, config) {
              $scope.countries = data;
          }).error(function (data, status, headers, config) {
              //$scope.message = 'Unexpected Error';
          });

          $http({
              method: 'POST',
              url: 'api/Home/GetAllCommodityTypes'
          }).success(function (data, status, headers, config) {
              $scope.commodityTypes = data;
          }).error(function (data, status, headers, config) {
              //$scope.message = 'Unexpected Error';
          });

          $http({
              method: 'POST',
              url: 'api/Home/GetAllMakes'
          }).success(function (data, status, headers, config) {
              $scope.Makes = data;
          }).error(function (data, status, headers, config) {
              //$scope.message = 'Unexpected Error';
          });
      };
      LoadDetails();
  });