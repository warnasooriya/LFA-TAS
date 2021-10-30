app.controller('TpaHomeCtrl',
  function ($scope, $http, $stateParams, $localStorage, $cookieStore, $location) {
      //$scope.SetCountryValue = function () {

      //    //alert($scope.country.code);
      //};


      //$localStorage.tpaID = $stateParams.tpaId;
      //$cookieStore.put('tpaId', $stateParams.tpaId);


      //var LFA = angular.module('LFA', ['angular.filter']);

      //$scope.tpaID = "8C7D9A24-BD99-4971-8747-F5FA87984BE6";
      //$scope.tpaID = $stateParams.tpaId;
      $scope.ModalName = "Left Field";
      $scope.tpa = "";
      $scope.tpa.Banner = '00000000-0000-0000-0000-000000000000';
      $scope.tpa.Banner2 = '00000000-0000-0000-0000-000000000000';
      $scope.tpa.Banner3 = '00000000-0000-0000-0000-000000000000';
      $scope.tpa.Banner4 = '00000000-0000-0000-0000-000000000000';
      $scope.tpa.Banner5 = '00000000-0000-0000-0000-000000000000';
      //$('.app-navbar-fixed').css('padding-top','0 !important');
      document.getElementById("app").style.paddingTop = "10px";


      $scope.myInterval = 1000000;
      $scope.tpaName = $stateParams.tpaId;
      $localStorage.tpaName = $stateParams.tpaId;
      $scope.tpaID = null;
      //$scope.isProductTire = false;

      loadTPAId();

      function loadTPAId() {
          $http({
              method: 'POST',
              url: '/TAS.Web/api/ProductDisplay/GetTPAIdByName',
              data: { "tpaName": $scope.tpaName },
              async: false
          }).success(function (data, status, headers, config) {
              $scope.tpa = data[0];
              $scope.tpaID = $scope.tpa.Id;
              $localStorage.tpaID = $scope.tpa.Id;
              $cookieStore.put('tpaId', $scope.tpa.Id);
              if ($scope.tpa.Logo != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Logo != null) {
                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Logo, "tpaId": $scope.tpaID, "test": 0 }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaLogoSrc = data;
                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }

              LoadDetails();

          }).error(function (data, status, headers, config) {
          });
      }



      
      function LoadDetails() {

          //$http({
          //    method: 'POST',
          //    url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
          //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
          //}).success(function (data, status, headers, config) {
          //    $scope.commodityTypes = data;


          //    $http({
          //        method: 'POST',
          //        url: '/TAS.Web/api/Product/GetAllProductsByCommodityTypeId',
          //        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
          //        data: { "Id": $scope.commodityTypes[0].CommodityTypeId }
          //    }).success(function (data, status, headers, config) {
          //        $scope.Products = data;
          //        if ($scope.Products[0].Productcode == "TYRE") {
          //            //$scope.isProductTire = true;
          //        }
          //    }).error(function (data, status, headers, config) {
          //    });

          //}).error(function (data, status, headers, config) {
          //});
          //if ($scope.URL != null) {
          $http({
              method: 'POST',
              url: '/TAS.WEB/api/ProductDisplay/GetProductsByTPA',
              data: { "tpaId": $scope.tpaID }
          }).success(function (data, status, headers, config) {
              $scope.Products = data;
             // productPopover();
              var log = [];
              angular.forEach(data.BundledProducts, function (value, key) {
                  //if (key == "ParentProductId") {
                  $scope.selectedpp[value["ParentProductId"]] = true;
                  //}

              }, log);
             

          }).error(function (data, status, headers, config) {
              //clearControls();
              //$scope.message = 'Unexpected Error';
          });


          // if ($scope.tpa.Id != "" && typeof ($scope.tpa.Id) !== "undefined" && $scope.tpa.Id != null) {
          $http({
              method: 'POST',
              url: '/TAS.Web/api/ProductDisplay/GetTPADetailsById',
              data: { "tpaId": $scope.tpaID },
              async: false
          }).success(function (data, status, headers, config) {
              $scope.tpa = data[0];

              if ($scope.tpa.Logo != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Logo != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Logo, "tpaId": $scope.tpaID, "test": 0 }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaLogoSrc = data;


                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }

              $cookieStore.put('ImageId', $scope.tpa.Logo);

              if ($scope.tpa.Banner != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Banner != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Banner, "tpaId": $scope.tpaID }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaImageSrc = data;
                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }

              if ($scope.tpa.Banner2 != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Banner2 != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Banner2, "tpaId": $scope.tpaID }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaImageSrc2 = data;


                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }
              if ($scope.tpa.Banner3 != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Banner3 != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Banner3, "tpaId": $scope.tpaID }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaImageSrc3 = data;


                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }
              if ($scope.tpa.Banner4 != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Banner4 != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Banner4, "tpaId": $scope.tpaID }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaImageSrc4 = data;


                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }
              if ($scope.tpa.Banner5 != '00000000-0000-0000-0000-000000000000' && $scope.tpa.Banner5 != null) {


                  $http({
                      method: 'POST',
                      url: '/TAS.WEB/api/ProductDisplay/GetTPAImageById',
                      data: { "ImageId": $scope.tpa.Banner5, "tpaId": $scope.tpaID }
                  }).success(function (data, status, headers, config) {
                      $scope.TpaImageSrc5 = data;


                  }).error(function (data, status, headers, config) {
                      //clearControls();
                      //$scope.message = 'Unexpected Error';
                  });
              }

             

          }).error(function (data, status, headers, config) {
          });




          //} else {
          //   // $scope.resetTpa();
          //    //$scope.publicLink = "";
          //}

      }
      //added by ranga as angular
      $scope.RedirectToProduct = function (productId) {
          $location.path("home/homePremium/" + productId);
      }


      //else {
      //    //clearControls();
      //}
      //}
  });


//app.controller('ViewTPACrtl', ['$scope', '$stateParams',
//function ($scope, $stateParams) {
//    $stateParams.tpaId;

//    alert($stateParams.tpaId);

//}]);