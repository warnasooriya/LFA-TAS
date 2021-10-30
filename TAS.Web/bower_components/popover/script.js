var app = angular.module("myApp", ["my.popover", "my.tooltip"]);

app.config(function($popoverProvider, $tooltipProvider) {
  angular.extend($popoverProvider.defaults, {
    html: true,
    animation: true
  });
  angular.extend($tooltipProvider.defaults, {
        trigger: "hover",
        placement: "top",
        animation: true,
        html: true
  });
})

app.controller("FirstController", function($scope) {
  $scope.heading = "Bootstrap Popover to Angular Directive";
  $scope.options = {
    title: "Config object example",
    content: "This uses a <strong>JSON</strong> object from parent <a href='https://docs.angularjs.org/api/ng/directive/ngController'>controller</a> to configure the popover",
    trigger: "click",
    placement: "bottom"
  };
  $scope.size = "43.5 MB";
});
