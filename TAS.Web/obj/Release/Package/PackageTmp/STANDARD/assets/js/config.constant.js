/// <reference path="../../../bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js" />
/// <reference path="../../../bower_components/angular-strap/angular-strap.min.js" />
/// <reference path="controllers/menuBuilderCtrl.js" />
'use strict';

/**
 * Config constant
 */
app.constant('APP_MEDIAQUERY', {
    'desktopXL': 1200,
    'desktop': 992,
    'tablet': 768,
    'mobile': 480
});
app.constant('JS_REQUIRES', {
    //*** Scripts
    scripts: {
        //*** Javascript Plugins
        'modernizr': ['../bower_components/components-modernizr/modernizr.js?' + guid()],
        'moment': ['../bower_components/moment/min/moment.min.js?' + guid()],
        'spin': '../bower_components/spin.js/spin.js?' + guid(),

        //*** jQuery Plugins
        'perfect-scrollbar-plugin': ['../bower_components/perfect-scrollbar/js/min/perfect-scrollbar.jquery.min.js?' + guid(), '../bower_components/perfect-scrollbar/css/perfect-scrollbar.min.css?' + guid()],
        'ladda': ['../bower_components/ladda/dist/ladda.min.js?' + guid(), '../bower_components/ladda/dist/ladda-themeless.min.css?' + guid()],
        'sweet-alert': ['../bower_components/sweetalert/lib/sweet-alert.min.js?' + guid(), '../bower_components/sweetalert/lib/sweet-alert.css?' + guid()],
        'chartjs': '../bower_components/chartjs/Chart.min.js?' + guid(),
        'jquery-sparkline': '../bower_components/jquery.sparkline.build/dist/jquery.sparkline.min.js?' + guid(),
        'ckeditor-plugin': '../bower_components/ckeditor/ckeditor.js?' + guid(),
        'jquery-nestable-plugin': ['../bower_components/jquery-nestable/jquery.nestable.js?' + guid()],
        'touchspin-plugin': ['../bower_components/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.js?' + guid(), '../bower_components/bootstrap-touchspin/dist/jquery.bootstrap-touchspin.min.css?' + guid()],
        'bootstrapminjs': ['../bower_components/bootstrap/dist/js/bootstrap.min.js?' + guid()],

        //*** Controllers
        'dashboardCtrl': 'assets/js/controllers/dashboardCtrl.js?' + guid(),
        'iconsCtrl': 'assets/js/controllers/iconsCtrl.js?' + guid(),
        'vAccordionCtrl': 'assets/js/controllers/vAccordionCtrl.js?' + guid(),
        'ckeditorCtrl': 'assets/js/controllers/ckeditorCtrl.js?' + guid(),
        'laddaCtrl': 'assets/js/controllers/laddaCtrl.js?' + guid(),
        'ngTableCtrl': 'assets/js/controllers/ngTableCtrl.js?' + guid(),
        'cropCtrl': 'assets/js/controllers/cropCtrl.js?' + guid(),
        'asideCtrl': 'assets/js/controllers/asideCtrl.js?' + guid(),
        'toasterCtrl': 'assets/js/controllers/toasterCtrl.js?' + guid(),
        'sweetAlertCtrl': 'assets/js/controllers/sweetAlertCtrl.js?' + guid(),
        'mapsCtrl': 'assets/js/controllers/mapsCtrl.js?' + guid(),
        'chartsCtrl': 'assets/js/controllers/chartsCtrl.js?' + guid(),
        'calendarCtrl': 'assets/js/controllers/calendarCtrl.js?' + guid(),
        'nestableCtrl': 'assets/js/controllers/nestableCtrl.js?' + guid(),
        'validationCtrl': ['assets/js/controllers/validationCtrl.js?' + guid()],
        'userCtrl': ['assets/js/controllers/userCtrl.js?' + guid()],
        'selectCtrl': 'assets/js/controllers/selectCtrl.js?' + guid(),
        'wizardCtrl': 'assets/js/controllers/wizardCtrl.js?' + guid(),
        'uploadCtrl': 'assets/js/controllers/uploadCtrl.js?' + guid(),
        'treeCtrl': 'assets/js/controllers/treeCtrl.js?' + guid(),
        'inboxCtrl': 'assets/js/controllers/inboxCtrl.js?' + guid(),
        'xeditableCtrl': 'assets/js/controllers/xeditableCtrl.js?' + guid(),
        'chatCtrl': 'assets/js/controllers/chatCtrl.js?' + guid(),
        'menuBuilderCtrl': 'assets/js/controllers/menuBuilderCtrl.js?' + guid(),
        //ranga
        'loginCtrl': 'assets/js/controllers/loginCtrl.js?' + guid(),
        'homeLoginCtrl': 'assets/js/controllers/homeLoginCtrl.js?' + guid(),
        'mainLoginCtrl': 'assets/js/controllers/mainLoginCtrl.js?' + guid(),
        'tpaCtrl': 'assets/js/controllers/tpaCtrl.js?' + guid(),
        'tasTpaCtrl': 'assets/js/controllers/tasTpaCtrl.js?' + guid(),
        'TpaBranchCtrl': 'assets/js/controllers/tpaBranchCtrl.js?' + guid(),
        'ProductCtrl': 'assets/js/controllers/productCtrl.js?' + guid(),
        'TpaHomeCtrl': 'assets/js/controllers/tpaHomeCtrl.js?' + guid(),
        'CustomerCtrl': 'assets/js/controllers/customerCtrl.js?' + guid(),
        'PolicyRegCtrl': 'assets/js/controllers/policyRegistrationCtrl.js?' + guid(),
        'PolicyRegistrationCtrlV2': 'assets/js/controllers/PolicyRegistrationCtrlV2.js?' + guid(),
        'CustomerProfileCtrl': 'assets/js/controllers/customerProfileCtrl.js?' + guid(),
        'BuyingWizardCtrl': 'assets/js/controllers/buyingWizard.js?' + guid(),
        'PremiumDetailsCtrl': 'assets/js/controllers/premiumDetailsCtrl.js?' + guid(),
        'loginForgetPasswordCtrl': 'assets/js/controllers/loginForgetPasswordCtrl.js?' + guid(),
        'loginChangePasswordCtrl': 'assets/js/controllers/loginChangePasswordCtrl.js?' + guid(),
        'DealerPortalCtrl': 'assets/js/controllers/dealerPortalCtrl.js?' + guid(),
        'DealerInvoiceCodeGenerationCtrl': 'assets/js/controllers/dealerInvoiceCodeGeneration.js?' + guid(),
        'AutomobileAttributesCtrl': 'assets/js/controllers/automobileAttributesCtrl.js?' + guid(),
        'CommodityItemAttributesCtrl': 'assets/js/controllers/commodityItemAttributesCtrl.js?' + guid(),
        'MakeAndModelManagementCtrl': 'assets/js/controllers/MakeAndModelManagementCtrl.js?' + guid(),
        'BrownAndWhiteDetailsCtrl': 'assets/js/controllers/brownAndWhiteDetailsCtrl.js?' + guid(),
        'VehicleDetailsCtrl': 'assets/js/controllers/vehicleDetailsCtrl.js?' + guid(),
        'InsurerManagementCtrl': 'assets/js/controllers/InsurerManagementCtrl.js?' + guid(),
        'ReinsurerManagementCtrl': 'assets/js/controllers/ReinsurerManagementCtrl.js?' + guid(),
        'DealerManagementCtrl': 'assets/js/controllers/DealerManagementCtrl.js?' + guid(),
        'ManufacturerManagementCtrl': 'assets/js/controllers/ManufacturerManagementCtrl.js?' + guid(),
        'UserManagementCtrl': 'assets/js/controllers/UserManagementCtrl.js?' + guid(),
        'PremiumSetupManagementCtrl': 'assets/js/controllers/PremiumSetupManagementCtrl.js?' + guid(),
        'underscore': 'assets/js/directives/underscore.js?' + guid(),
        'UserProfileCtrl': 'assets/js/controllers/userProfileCtrl.js?' + guid(),
        'RoleManagementCtrl': 'assets/js/controllers/RoleManagementCtrl.js?' + guid(),
        'PolicyEndorsementCtrl': 'assets/js/controllers/PolicyEndorsementCtrl.js?' + guid(),
        'PolicyTransferCtrl': 'assets/js/controllers/PolicyTransferCtrl.js?' + guid(),
        'PolicyEndorsementApprovalCtrl': 'assets/js/controllers/PolicyEndorsementApprovalCtrl.js?' + guid(),
        'PolicyCancelationApprovalCtrl': 'assets/js/controllers/PolicyCancelationApprovalCtrl.js?' + guid(),
        'PolicyCancelationCtrl': 'assets/js/controllers/PolicyCancelationCtrl.js?' + guid(),
        'PolicyRenewalCtrl': 'assets/js/controllers/PolicyRenewalCtrl.js?' + guid(),
        'PolicyInquiryCtrl': 'assets/js/controllers/PolicyInquiryCtrl.js?' + guid(),
        'CurrencyManagementCtrl': 'assets/js/controllers/CurrencyManagementCtrl.js?' + guid(),
        'TaxManagementCtrl': 'assets/js/controllers/TaxManagementCtrl.js?' + guid(),
        'LocationCtrl': '../bower_components/angular-location/location.js?' + guid(),
        'CountryManagementCtrl': 'assets/js/controllers/CountryManagementCtrl.js?' + guid(),
        'BordxManagementCtrl': 'assets/js/controllers/BordxManagementCtrl.js?' + guid(),
        'BordxDefManagementCtrl': 'assets/js/controllers/BordxDefManagementCtrl.js?' + guid(),
        'BordxReOpenManagementCtrl': 'assets/js/controllers/BordxReOpenManagementCtrl.js?' + guid(),
        'BordxViewManagementCtrl': 'assets/js/controllers/BordxViewManagementCtrl.js?' + guid(),
        'WarrantyTypeManagementCtrl': 'assets/js/controllers/WarrantyTypeManagementCtrl.js?' + guid(),
        'PolicyRegDealerCtrl': 'assets/js/controllers/policyRegistrationDealerCtrl.js?' + guid(),
        'BulkPolicyUploadCtrl': 'assets/js/controllers/BulkPolicyUploadCtrl.js?' + guid(),
        'ClaimSubmissionCtrl': 'assets/js/controllers/ClaimSubmissionCtrl.js?' + guid(),
        'PartCtrl': 'assets/js/controllers/PartCtrl.js?' + guid(),
        'ClaimListing': 'assets/js/controllers/ClaimListingCtrl.js?' + guid(),
        'ClaimEditCtrl': 'assets/js/controllers/EditClaimCtrl.js?' + guid(),
        'ClaimInvoiceCtrl': 'assets/js/controllers/ClaimInvoiceCtrl.js?' + guid(),
        'ClaimInvoiceBatchingCtrl': 'assets/js/controllers/ClaimInvoiceBatchingCtrl.js?' + guid(),
        'ClaimProcessCtrl': 'assets/js/controllers/ClaimProcessCtrl.js?' + guid(),
        'ClaimBordxYearProcessCtrl': 'assets/js/controllers/ClaimBordxYearProcessCtrl.js?' + guid(),
        'DealerDiscountCtrl': 'assets/js/controllers/DealerDiscountCtrl.js?' + guid(),
        'ReportExplorerCtrl': 'assets/js/controllers/ReportExplorerCtrl.js?' + guid(),


        //Lasanthi
        'PartRejectionTypeCtrl': 'assets/js/controllers/PartRejectionTypeCtrl.js?' + guid(),
        'FaultManagementCtrl': 'assets/js/controllers/FaultManagementCtrl.js?' + guid(),
        'ClaimBordxManagementCtrl': 'assets/js/controllers/ClaimBordxManagementCtrl.js?' + guid(),
        'ReinsurerReceiptCtrl': 'assets/js/controllers/ReinsurerReceiptCtrl.js?' + guid(),
        'DealerChequesCtrl': 'assets/js/controllers/DealerChequesCtrl.js?' + guid(),
        'ClaimBatchingCtrl': 'assets/js/controllers/ClaimBatchingCtrl.js?' + guid(),
        'ClaimBordxReOpenManagementCtrl': 'assets/js/controllers/ClaimBordxReOpenManagementCtrl.js?' + guid(),
        'ClaimBordxYearlyReOpenManagementCtrl': 'assets/js/controllers/ClaimBordxYearlyReOpenManagementCtrl.js?' + guid(),
        'ClaimEndorsementCtrl': 'assets/js/controllers/ClaimEndorsementCtrl.js?' + guid(),
        'ClaimEndorsementApprovalCtrl': 'assets/js/controllers/ClaimEndorsementApprovalCtrl.js?' + guid(),
        'ClaimInquiryCtrl': 'assets/js/controllers/ClaimInquiryCtrl.js?' + guid(),
        'ReinsureBordxReportsCtrl': 'assets/js/controllers/ReinsureBordxReportsCtrl.js?' + guid(),
        'IncurredErningCtrl': 'assets/js/controllers/IncurredErningCtrl.js?' + guid(),
        'ReinsurerBordxCtrl': 'assets/js/controllers/ReinsurerBordxCtrl.js?' + guid(),
        'CustomerProCtrl': 'assets/js/controllers/CustomerProCtrl.js?' + guid(),
        'verifyEmailCtrl': 'assets/js/controllers/verifyEmailCtrl.js?' + guid(),
        'loginChangePasswordCustomerCtrl': 'assets/js/controllers/loginChangePasswordCustomerCtrl.js?' + guid(),
        'InvoiceCodeReportCtrl': 'assets/js/controllers/InvoiceCodeReportCtrl.js?' + guid(),
        'ClaimProcessCtrlB2': 'assets/js/controllers/ClaimProcessCtrlB2.js?' + guid(),


        //Sachith
        'BulkCustomerUploadCtrl': 'assets/js/controllers/BulkCustomerUploadCtrl.js?' + guid(),
        'BordxProcessCtrl': 'assets/js/controllers/BordxProcessCtrl.js?' + guid(),
        'BordxReportTemplateCtrl': 'assets/js/controllers/BordxReportTemplateCtrl.js?' + guid(),

        //gihan
        'brokerCtrl': 'assets/js/controllers/brokerController.js?' + guid(),

        //nayana
        'DealerCommentManagemnetCtrl': 'assets/js/controllers/DealerCommentManagemnetCtrl.js?' + guid(),
        'PolicyRegCtrlDemo': 'assets/js/controllers/policyRegistrationDemoCtrl.js?' + guid(),

        //Demo
        'DealerInvoiceCodeGenerationUpdateCtrl': 'assets/js/controllers/dealerInvoiceCodeGenerationUpdate.js?' + guid(),
        'BuyingWizardDemoCtrl': 'assets/js/controllers/buyingWizardDemo.js?' + guid(),

        //*** Filters
        'htmlToPlaintext': 'assets/js/filters/htmlToPlaintext.js?' + guid(),
        'byteToKB': 'assets/js/filters/byteToKB.js?' + guid(),
        'kbToReadableSize': 'assets/js/filters/kbToReadableSize.js?' + guid(),
        'propsFilter': 'assets/js/filters/propsFilter.js?' + guid()


    },
    //*** angularJS Modules
    modules: [{
        name: 'angularMoment',
        files: ['../bower_components/angular-moment/angular-moment.min.js?' + guid()]
    }, {
        name: 'toaster',
        files: ['../bower_components/AngularJS-Toaster/toaster.js?' + guid(), '../bower_components/AngularJS-Toaster/toaster.css?' + guid()]
    }, {
        name: 'angularBootstrapNavTree',
        files: ['../bower_components/angular-bootstrap-nav-tree/dist/abn_tree_directive.js?' + guid(), '../bower_components/angular-bootstrap-nav-tree/dist/abn_tree.css?' + guid()]
    }, {
        name: 'angularBootstrapTpls',
        files: ['../bower_components/angular-bootstrap/ui-bootstrap-tpls.min.js?' + guid()]
    }, {
        name: 'angular-ladda',
        files: ['../bower_components/angular-ladda/dist/angular-ladda.min.js?' + guid()]
    }, {
        name: 'angularjs-dropdown-multiselect',
        files: ['../bower_components/angularjs-dropdown-multiselect/dist/angularjs-dropdown-multiselect.min.js?' + guid(),
        '../bower_components/angularjs-dropdown-multiselect/lodash.js?' + guid(),
        '../bower_components/angularjs-dropdown-multiselect/lodash.core.js?' + guid(),
        '../STANDARD/assets/js/directives/underscore.js?' + guid()]
    }, {
        name: 'ngDialog',
        files: ['../bower_components/ng-Dialog/ngDialog.js?' + guid(), '../bower_components/ng-Dialog/ngDialog.css?' + guid(), '../bower_components/ng-Dialog/ngDialog-theme-plain.css?' + guid(), '../bower_components/ng-Dialog/ngDialog-theme-default.css?' + guid()]
    }, {
        name: 'ui.grid',
        files: ['../bower_components/angular-ui-grid/ui-grid.js?' + guid(), '../bower_components/angular-ui-grid/ui-grid.css?' + guid(), '../bower_components/angular-ui-grid/ui-grid.svg', '../bower_components/angular-ui-grid/ui-grid.ttf', '../bower_components/angular-ui-grid/ui-grid.woff', , '../bower_components/angular-ui-grid/ui-grid.eot']
    }, {
        name: 'ngTable',
        files: ['../bower_components/ng-table/dist/ng-table.min.js?' + guid(), '../bower_components/ng-table/dist/ng-table.min.css?' + guid()]
    }, {
        name: 'ui.select',
        files: ['../bower_components/angular-ui-select/dist/select.min.js?' + guid(), '../bower_components/angular-ui-select/dist/select.min.css?' + guid(), '../bower_components/select2/dist/css/select2.min.css?' + guid(), '../bower_components/select2-bootstrap-css/select2-bootstrap.min.css?' + guid(), '../bower_components/selectize/dist/css/selectize.bootstrap3.css?' + guid()]
    }, {
        name: 'ui.mask',
        files: ['../bower_components/angular-ui-utils/mask.min.js?' + guid()]
    }, {
        name: 'ngImgCrop',
        files: ['../bower_components/ngImgCrop/compile/minified/ng-img-crop.js?' + guid(), '../bower_components/ngImgCrop/compile/minified/ng-img-crop.css?' + guid()]
    }, {
        name: 'angularFileUpload',
        files: ['../bower_components/angular-file-upload/angular-file-upload.min.js?' + guid()]
    }, {
        name: 'ngAside',
        files: ['../bower_components/angular-aside/dist/js/angular-aside.min.js?' + guid(), '../bower_components/angular-aside/dist/css/angular-aside.min.css?' + guid()]
    }, {
        name: 'truncate',
        files: ['../bower_components/angular-truncate/src/truncate.js?' + guid()]
    }, {
        name: 'oitozero.ngSweetAlert',
        files: ['../bower_components/angular-sweetalert-promised/SweetAlert.min.js?' + guid()]
    }, {
        name: 'monospaced.elastic',
        files: ['../bower_components/angular-elastic/elastic.js?' + guid()]
    }, {
        name: 'ngMap',
        files: ['../bower_components/ngmap/build/scripts/ng-map.min.js?' + guid()]
    }, {
        name: 'tc.chartjs',
        files: ['../bower_components/tc-angular-chartjs/dist/tc-angular-chartjs.min.js?' + guid()]
    }, {
        name: 'flow',
        files: ['../bower_components/ng-flow/dist/ng-flow-standalone.min.js?' + guid()]
    }, {
        name: 'uiSwitch',
        files: ['../bower_components/angular-ui-switch/angular-ui-switch.min.js?' + guid(), '../bower_components/angular-ui-switch/angular-ui-switch.min.css?' + guid()]
    }, {
        name: 'ckeditor',
        files: ['../bower_components/angular-ckeditor/angular-ckeditor.min.js?' + guid()]
    }, {
        name: 'mwl.calendar',
        files: ['../bower_components/angular-bootstrap-calendar/dist/js/angular-bootstrap-calendar.js?' + guid(), '../bower_components/angular-bootstrap-calendar/dist/js/angular-bootstrap-calendar-tpls.js?' + guid(), '../bower_components/angular-bootstrap-calendar/dist/css/angular-bootstrap-calendar.min.css?' + guid()]
    }, {
        name: 'ng-nestable',
        files: ['../bower_components/ng-nestable/src/angular-nestable.js?' + guid()]
    }, {
        name: 'vAccordion',
        files: ['../bower_components/v-accordion/dist/v-accordion.min.js?' + guid(), '../bower_components/v-accordion/dist/v-accordion.min.css?' + guid()]
    }, {
        name: 'xeditable',
        files: ['../bower_components/angular-xeditable/dist/js/xeditable.min.js?' + guid(), '../bower_components/angular-xeditable/dist/css/xeditable.css?' + guid(), 'assets/js/config/config-xeditable.js?' + guid()]
    }, {
        name: 'checklist-model',
        files: ['../bower_components/checklist-model/checklist-model.js?' + guid()]
    }, {
        name: 'ngStorage',
        files: ['../bower_components/ngStorage-master/ngStorage.min.js?' + guid()]
    }, {
        name: 'angular-jwt',
        files: ['../bower_components/angularJwt/angular-jwt.min.js?' + guid()]
    }, {
        name: 'angular-strap',
        files: ['../bower_components/angular-strap/angular-strap.min.js?' + guid(), '../bower_components/angular-strap/angular-strap.tpl.min.js?' + guid(), '../bower_components/angular-strap/popover.min.js?' + guid()]
    }, {
        name: 'ngCookies',
        files: ['../bower_components/ngCookies/Cookies.js?' + guid()]
    }, {
        name: 'angular-touch',
        files: ['../bower_components/angular-touch/angular-touch.min.js?' + guid()]
    }, {
        name: 'angular-animate',
        files: ['../bower_components/angular-animate/angular-animate.min.js?' + guid()]
    },
    {
        name: 'googlechart',
        files: ['../bower_components/googlechart/googlechart.js?' + guid()]
    },
    {
        name: 'angular-sanitize',
        files: ['../bower_components/angular-sanitize/angular-sanitize.min.js?' + guid()]
    },
    {
        name: 'angular-multi-select',
        files: [
            '../bower_components/angular-multi-select-master/dist/prod/angular-multi-select.min.js?' + guid(),
            '../bower_components/angular-multi-select-master/dist/prod/angular-multi-select.min.css?' + guid()
        ]
    }

    ]

});

//create guids
function guid() {
    //function s4() {
    //    return Math.floor((1 + Math.random()) * 0x10000)
    //      .toString(16)
    //      .substring(1);
    //}
    //return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
    //  s4() + '-' + s4() + s4() + s4();

    return "42019912-d34b-ddsaQQssLMd9b";
}