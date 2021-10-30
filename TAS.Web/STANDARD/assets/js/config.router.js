'use strict';

/**
 * Config for the router
 */
app.config(['$stateProvider', '$urlRouterProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$ocLazyLoadProvider', 'JS_REQUIRES',
    function ($stateProvider, $urlRouterProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $ocLazyLoadProvider, jsRequires) {

        app.controller = $controllerProvider.register;
        app.directive = $compileProvider.directive;
        app.filter = $filterProvider.register;
        app.factory = $provide.factory;
        app.service = $provide.service;
        app.constant = $provide.constant;
        app.value = $provide.value;

        // LAZY MODULES

        $ocLazyLoadProvider.config({
            debug: false,
            events: true,
            modules: jsRequires.modules
        });

        // APPLICATION ROUTES
        // -----------------------------------
        // For any unmatched url, redirect to /app/dashboard
        $urlRouterProvider.otherwise("/login/signin");
        //
        // Set up the states
        $stateProvider.state('app', {
            url: "/app",
            templateUrl: "assets/views/app.html?" + guid(),
            resolve: loadSequence('modernizr', 'moment', 'angularMoment', 'uiSwitch', 'perfect-scrollbar-plugin',
                'toaster', 'ngAside', 'vAccordion', 'sweet-alert', 'chartjs', 'tc.chartjs', 'oitozero.ngSweetAlert',
                'chatCtrl', 'googlechart'),
            abstract: true
        }).state('app.dashboard', {
            url: "/dashboard",
            templateUrl: "assets/views/dashboard.html?" + guid(),
            resolve: loadSequence('jquery-sparkline', 'dashboardCtrl', 'menuBuilderCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
            title: 'Dashboard',
            ncyBreadcrumb: {
                label: 'Dashboard'
            }
        }).state('app.ui', {
            url: '/ui',
            template: '<div ui-view class="fade-in-up"></div>',
            title: 'UI Elements',
            ncyBreadcrumb: {
                label: 'UI Elements'
            }
        }).state('app.ui.elements', {
            url: '/elements',
            templateUrl: "assets/views/ui_elements.html?" + guid(),
            title: 'Elements',
            icon: 'ti-layout-media-left-alt',
            ncyBreadcrumb: {
                label: 'Elements'
            }
        }).state('app.ui.buttons', {
            url: '/buttons',
            templateUrl: "assets/views/ui_buttons.html?" + guid(),
            title: 'Buttons',
            resolve: loadSequence('spin', 'ladda', 'angular-ladda', 'laddaCtrl'),
            ncyBreadcrumb: {
                label: 'Buttons'
            }
        }).state('app.ui.links', {
            url: '/links',
            templateUrl: "assets/views/ui_links.html?" + guid(),
            title: 'Link Effects',
            ncyBreadcrumb: {
                label: 'Link Effects'
            }
        }).state('app.ui.icons', {
            url: '/icons',
            templateUrl: "assets/views/ui_icons.html?" + guid(),
            title: 'Font Awesome Icons',
            ncyBreadcrumb: {
                label: 'Font Awesome Icons'
            },
            resolve: loadSequence('iconsCtrl')
        }).state('app.ui.lineicons', {
            url: '/line-icons',
            templateUrl: "assets/views/ui_line_icons.html?" + guid(),
            title: 'Linear Icons',
            ncyBreadcrumb: {
                label: 'Linear Icons'
            },
            resolve: loadSequence('iconsCtrl')
        }).state('app.ui.modals', {
            url: '/modals',
            templateUrl: "assets/views/ui_modals.html?" + guid(),
            title: 'Modals',
            ncyBreadcrumb: {
                label: 'Modals'
            },
            resolve: loadSequence('asideCtrl')
        }).state('app.ui.toggle', {
            url: '/toggle',
            templateUrl: "assets/views/ui_toggle.html?" + guid(),
            title: 'Toggle',
            ncyBreadcrumb: {
                label: 'Toggle'
            }
        }).state('app.ui.tabs_accordions', {
            url: '/accordions',
            templateUrl: "assets/views/ui_tabs_accordions.html?" + guid(),
            title: "Tabs & Accordions",
            ncyBreadcrumb: {
                label: 'Tabs & Accordions'
            },
            resolve: loadSequence('vAccordionCtrl')
        }).state('app.ui.panels', {
            url: '/panels',
            templateUrl: "assets/views/ui_panels.html?" + guid(),
            title: 'Panels',
            ncyBreadcrumb: {
                label: 'Panels'
            }
        }).state('app.ui.notifications', {
            url: '/notifications',
            templateUrl: "assets/views/ui_notifications.html?" + guid(),
            title: 'Notifications',
            ncyBreadcrumb: {
                label: 'Notifications'
            },
            resolve: loadSequence('toasterCtrl', 'sweetAlertCtrl')
        }).state('app.ui.treeview', {
            url: '/treeview',
            templateUrl: "assets/views/ui_tree.html?" + guid(),
            title: 'TreeView',
            ncyBreadcrumb: {
                label: 'Treeview'
            },
            resolve: loadSequence('angularBootstrapNavTree', 'treeCtrl')
        }).state('app.ui.media', {
            url: '/media',
            templateUrl: "assets/views/ui_media.html?" + guid(),
            title: 'Media',
            ncyBreadcrumb: {
                label: 'Media'
            }
        }).state('app.ui.nestable', {
            url: '/nestable2',
            templateUrl: "assets/views/ui_nestable.html?" + guid(),
            title: 'Nestable List',
            ncyBreadcrumb: {
                label: 'Nestable List'
            },
            resolve: loadSequence('jquery-nestable-plugin', 'ng-nestable', 'nestableCtrl')
        }).state('app.ui.typography', {
            url: '/typography',
            templateUrl: "assets/views/ui_typography.html?" + guid(),
            title: 'Typography',
            ncyBreadcrumb: {
                label: 'Typography'
            }
        }).state('app.table', {
            url: '/table',
            template: '<div ui-view class="fade-in-up"></div>',
            title: 'Tables',
            ncyBreadcrumb: {
                label: 'Tables'
            }
        }).state('app.table.basic', {
            url: '/basic',
            templateUrl: "assets/views/table_basic.html?" + guid(),
            title: 'Basic Tables',
            ncyBreadcrumb: {
                label: 'Basic'
            }
        }).state('app.table.responsive', {
            url: '/responsive',
            templateUrl: "assets/views/table_responsive.html?" + guid(),
            title: 'Responsive Tables',
            ncyBreadcrumb: {
                label: 'Responsive'
            }
        }).state('app.table.data', {
            url: '/data',
            templateUrl: "assets/views/table_data.html?" + guid(),
            title: 'ngTable',
            ncyBreadcrumb: {
                label: 'ngTable'
            },
            resolve: loadSequence('ngTable', 'ngTableCtrl')
        }).state('app.table.export', {
            url: '/export',
            templateUrl: "assets/views/table_export.html?" + guid(),
            title: 'Table'
        }).state('app.form', {
            url: '/form',
            template: '<div ui-view class="fade-in-up"></div>',
            title: 'Forms',
            ncyBreadcrumb: {
                label: 'Forms'
            }
        }).state('app.form.elements', {
            url: '/elements',
            templateUrl: "assets/views/form_elements.html?" + guid(),
            title: 'Forms Elements',
            ncyBreadcrumb: {
                label: 'Elements'
            },
            resolve: loadSequence('ui.select', 'monospaced.elastic', 'ui.mask', 'touchspin-plugin', 'selectCtrl')
        }).state('app.form.xeditable', {
            url: '/xeditable',
            templateUrl: "assets/views/form_xeditable.html?" + guid(),
            title: 'Angular X-Editable',
            ncyBreadcrumb: {
                label: 'X-Editable'
            },
            resolve: loadSequence('xeditable', 'checklist-model', 'xeditableCtrl')
        }).state('app.form.texteditor', {
            url: '/editor',
            templateUrl: "assets/views/form_text_editor.html?" + guid(),
            title: 'Text Editor',
            ncyBreadcrumb: {
                label: 'Text Editor'
            },
            resolve: loadSequence('ckeditor-plugin', 'ckeditor', 'ckeditorCtrl')
        }).state('app.form.wizard', {
            url: '/wizard',
            templateUrl: "assets/views/form_wizard.html?" + guid(),
            title: 'Form Wizard',
            ncyBreadcrumb: {
                label: 'Wizard'
            },
            resolve: loadSequence('wizardCtrl')
        }).state('app.form.validation', {
            url: '/validation',
            templateUrl: "assets/views/form_validation.html?" + guid(),
            title: 'Form Validation',
            ncyBreadcrumb: {
                label: 'Validation'
            },
            resolve: loadSequence('validationCtrl')
        }).state('app.form.cropping', {
            url: '/image-cropping',
            templateUrl: "assets/views/form_image_cropping.html?" + guid(),
            title: 'Image Cropping',
            ncyBreadcrumb: {
                label: 'Image Cropping'
            },
            resolve: loadSequence('ngImgCrop', 'cropCtrl')
        }).state('app.form.upload', {
            url: '/file-upload',
            templateUrl: "assets/views/form_file_upload.html?" + guid(),
            title: 'Multiple File Upload',
            ncyBreadcrumb: {
                label: 'File Upload'
            },
            resolve: loadSequence('angularFileUpload', 'uploadCtrl')
        }).state('app.pages', {
            url: '/pages',
            template: '<div ui-view class="fade-in-up"></div>',
            title: 'Pages',
            ncyBreadcrumb: {
                label: 'Pages'
            }
        }).state('app.pages.user', {
            url: '/user',
            templateUrl: "assets/views/pages_user_profile.html?" + guid(),
            title: 'User Profile',
            ncyBreadcrumb: {
                label: 'User Profile'
            },
            resolve: loadSequence('flow', 'userCtrl')
        }).state('app.pages.invoice', {
            url: '/invoice',
            templateUrl: "assets/views/pages_invoice.html?" + guid(),
            title: 'Invoice',
            ncyBreadcrumb: {
                label: 'Invoice'
            }
        }).state('app.pages.timeline', {
            url: '/timeline',
            templateUrl: "assets/views/pages_timeline.html?" + guid(),
            title: 'Timeline',
            ncyBreadcrumb: {
                label: 'Timeline'
            },
            resolve: loadSequence('ngMap')
        }).state('app.pages.calendar', {
            url: '/calendar',
            templateUrl: "assets/views/pages_calendar.html?" + guid(),
            title: 'Calendar',
            ncyBreadcrumb: {
                label: 'Calendar'
            },
            resolve: loadSequence('moment', 'mwl.calendar', 'calendarCtrl')
        }).state('app.pages.messages', {
            url: '/messages',
            templateUrl: "assets/views/pages_messages.html?" + guid(),
            resolve: loadSequence('truncate', 'htmlToPlaintext', 'inboxCtrl')
        }).state('app.pages.messages.inbox', {
            url: '/inbox/:inboxID',
            templateUrl: "assets/views/pages_inbox.html?" + guid(),
            controller: 'ViewMessageCrtl'
        }).state('app.pages.blank', {
            url: '/blank',
            templateUrl: "assets/views/pages_blank_page.html?" + guid(),
            ncyBreadcrumb: {
                label: 'Starter Page'
            }
        }).state('app.utilities', {
            url: '/utilities',
            template: '<div ui-view class="fade-in-up"></div>',
            title: 'Utilities',
            ncyBreadcrumb: {
                label: 'Utilities'
            }
        }).state('app.utilities.search', {
            url: '/search',
            templateUrl: "assets/views/utility_search_result.html?" + guid(),
            title: 'Search Results',
            ncyBreadcrumb: {
                label: 'Search Results'
            }
        }).state('app.utilities.pricing', {
            url: '/pricing',
            templateUrl: "assets/views/utility_pricing_table.html?" + guid(),
            title: 'Pricing Table',
            ncyBreadcrumb: {
                label: 'Pricing Table'
            }
        }).state('app.maps', {
            url: "/maps",
            templateUrl: "assets/views/maps.html?" + guid(),
            resolve: loadSequence('ngMap', 'mapsCtrl'),
            title: "Maps",
            ncyBreadcrumb: {
                label: 'Maps'
            }
        }).state('app.charts', {
            url: "/charts",
            templateUrl: "assets/views/charts.html?" + guid(),
            resolve: loadSequence('chartjs', 'tc.chartjs', 'chartsCtrl'),
            title: "Charts",
            ncyBreadcrumb: {
                label: 'Charts'
            }
        }).state('app.documentation', {
            url: "/documentation",
            templateUrl: "assets/views/documentation.html?" + guid(),
            title: "Documentation",
            ncyBreadcrumb: {
                label: 'Documentation'
            }
        }).state('app.home', {
            url: "/home",
            templateUrl: "assets/views/home.html?" + guid(),
            title: "Home",
            ncyBreadcrumb: {
                label: 'Home'
            }
        }).state('app.home.tpaUrl', {
            url: "/home/:tpaURL",
            templateUrl: "assets/views/home.html?" + guid(),
            resolve: loadSequence('flow', 'tpaHomeCtrl'),
            title: "Home",
            ncyBreadcrumb: {
                label: 'Home'
            }
        }).state('app.home.premium.display', {
            url: "/home_premium_display",
            templateUrl: "assets/views/premium_display.html?" + guid(),
            title: "Premium Display",
            ncyBreadcrumb: {
                label: 'Premium Display'
            }
        }).state('app.home.premium.display.login', {
            url: "/home_premium_display/login",
            templateUrl: "assets/views/login_user.html?" + guid(),
            title: "Login User",
            ncyBreadcrumb: {
                label: 'Login User'
            }
        }).state('app.internal.user', {
            url: "/internal_user",
            templateUrl: "assets/views/dashboard.html?" + guid(),
            title: "Home Premium",
            ncyBreadcrumb: {
                label: 'Home Premium'
            }
        }).state('app.internalLogin', {
            url: "/internal",
            templateUrl: "assets/views/login.html?" + guid(),
            resolve: loadSequence('flow', 'LoginCtrl'),
            title: "Login",
            ncyBreadcrumb: {
                label: 'Login'
            }
        }).state('app.user', {
            url: "/Users",
            templateUrl: "assets/views/Users.html?" + guid(),
            resolve: loadSequence('flow', 'UsersCtrl'),
            title: "Users",
            ncyBreadcrumb: {
                label: 'Users'
            }
        })/*.state('app.customerRegistration', {
		url: "/CustomerRegistration",
		templateUrl: "assets/views/CustomerRegistration.html?"+guid(),
		resolve: loadSequence('flow', 'CustomerCtrl'),
		title: "Customer Registration",
		ncyBreadcrumb: {
			label: 'Customer Registration'
		}
	}).state('app.customerProfile', {
		url: "/CustomerProfile",
		templateUrl: "assets/views/CustomerProfile.html?"+guid(),
		resolve: loadSequence('flow', 'CustomerProfileCtrl'),
		title: "Customer Profile",
		ncyBreadcrumb: {
			label: 'Customer Profile'
		}
	})*/.state('app.login', {
            url: "/internal",
            templateUrl: "assets/views/login.html?" + guid(),
            resolve: loadSequence('flow', 'LoginCtrl'),
            title: "Login",
            ncyBreadcrumb: {
                label: 'Login'
            }
        }).state('app.inthome', {
            url: "/internal/home",
            templateUrl: "assets/views/internal_home.html?" + guid(),
            resolve: loadSequence('flow', 'InternalHomeCtrl'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.product', {
            url: "/product",
            templateUrl: "assets/views/AddEditProduct.html?" + guid(),
            resolve: loadSequence('ProductCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngCookies', 'toaster', 'ngDialog'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.automobileAttributes', {
            url: "/automobileAttributes",
            templateUrl: "assets/views/AutomobileAttributes.html?" + guid(),
            resolve: loadSequence('AutomobileAttributesCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.commodityItemAttributes', {
            url: "/commodityItemAttributes",
            templateUrl: "assets/views/CommodityItemAttributes.html?" + guid(),
            resolve: loadSequence('CommodityItemAttributesCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.brownAndWhiteDetails', {
            url: "/brownAndWhiteDetails",
            templateUrl: "assets/views/BrownAndWhiteDetails.html?" + guid(),
            resolve: loadSequence('BrownAndWhiteDetailsCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.vehicleDetails', {
            url: "/vehicleDetails",
            templateUrl: "assets/views/VehicleDetails.html?" + guid(),
            resolve: loadSequence('VehicleDetailsCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.makeAndModelManagement', {
            url: "/makeAndModelManagement",
            templateUrl: "assets/views/MakeAndModelManagement.html?" + guid(),
            resolve: loadSequence('MakeAndModelManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'flow', 'ngDialog', 'ui.select', 'ui.grid', 'underscore', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.insurerManagement', {
            url: "/insurerManagement",
            templateUrl: "assets/views/InsurerManagement.html?" + guid(),
            resolve: loadSequence('InsurerManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.reinsurerManagement', {
            url: "/reinsurerManagement",
            templateUrl: "assets/views/ReinsurerManagement.html?" + guid(),
            resolve: loadSequence('ReinsurerManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bordxManagement', {
            url: "/bordxManagement",
            templateUrl: "assets/views/BordxManagement.html?" + guid(),
            resolve: loadSequence('ngTable', 'BordxManagementCtrl', 'ngDialog', 'ui.grid', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bordxDefManagement', {
            url: "/bordxDefManagement",
            templateUrl: "assets/views/BordxDefManagement.html?" + guid(),
            resolve: loadSequence('BordxDefManagementCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bordxReportTemplate', {
            url: "/bordxReportTemplate",
            templateUrl: "assets/views/BordxReportTemplate.html?" + guid(),
            resolve: loadSequence('BordxReportTemplateCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bordxReOpenManagement', {
            url: "/bordxReOpenManagement",
            templateUrl: "assets/views/BordxReOpenManagement.html?" + guid(),
            resolve: loadSequence('ngTable', 'BordxReOpenManagementCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bordxViewManagement', {
            url: "/bordxViewManagement",
            templateUrl: "assets/views/BordxViewManagement.html?" + guid(),
            resolve: loadSequence('ngTable', 'BordxViewManagementCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.bulkPolicyUpload', {
            url: "/bulkPolicyUpload",
            templateUrl: "assets/views/BulkPolicyUpload.html?" + guid(),
            resolve: loadSequence('BulkPolicyUploadCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert', 'angularFileUpload'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.dealerManagement', {
            url: "/dealerManagement",
            templateUrl: "assets/views/DealerManagement.html?" + guid(),
            resolve: loadSequence('DealerManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'ngDialog', 'ui.select', 'ui.grid', 'underscore', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.dealerPortal', {
            url: "/dealerPortal",
            templateUrl: "assets/views/DealerPortal.html?" + guid(),
            resolve: loadSequence('DealerPortalCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.DealerCommentManagemnet', {
            url: "/dealerCommentManagemnet",
            templateUrl: "assets/views/DealerCommentManagemnet.html?" + guid(),
            resolve: loadSequence('DealerCommentManagemnetCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        }).state('app.InvoiceCodeReport', {
            url: "/InvoiceCodeReport",
            templateUrl: "assets/views/InvoiceCodeReport.html?" + guid(),
            resolve: loadSequence('InvoiceCodeReportCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
            title: "Internal Home",
            ncyBreadcrumb: {
                label: 'Internal Home'
            }
        })
            .state('app.dealerInvoceCodeGeneration', {
                url: "/dealerinvoicecode",
                templateUrl: "assets/views/dealerInvoceCodeGeneration.html?" + guid(),
                resolve: loadSequence('DealerInvoiceCodeGenerationCtrl', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert',
                    'toaster', 'flow', 'ui.select', 'ui.mask'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            })
            .state('app.buyingprocessDemo', {
                url: "/buyingprocess",
                templateUrl: "assets/views/buying_wizardDemo.html?" + guid(),
                resolve: loadSequence('BuyingWizardDemoCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage',
                    'angular-jwt', 'ngCookies', 'ngDialog', 'flow', 'toaster', 'ui.select', 'angularjs-dropdown-multiselect', 'underscore'),
                title: "Home Premium",
                ncyBreadcrumb: {
                    label: 'Home Premium'
                }
            })
            .state('app.manufacturerManagement', {
                url: "/manufacturerManagement",
                templateUrl: "assets/views/ManufacturerManagement.html?" + guid(),
                resolve: loadSequence('ManufacturerManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
                title: "Internal Home",
                ncyBreadcrumb: {
                    label: 'Internal Home'
                }
            }).state('app.warrantyTypeManagement', {
                url: "/warrantyTypeManagement",
                templateUrl: "assets/views/WarrantyTypeManagement.html?" + guid(),
                resolve: loadSequence('WarrantyTypeManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'toaster'),
                title: "Internal Home",
                ncyBreadcrumb: {
                    label: 'Internal Home'
                }
            }).state('app.countryManagement', {
                url: "/countryManagement",
                templateUrl: "assets/views/CountryManagement.html?" + guid(),
                resolve: loadSequence('CountryManagementCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore', 'ui.grid', 'toaster'),
                title: "Internal Home",
                ncyBreadcrumb: {
                    label: 'Internal Home'
                }
            }).state('app.userManagement', {
                url: "/userManagement",
                templateUrl: "assets/views/UserManagement.html?" + guid(),
                resolve: loadSequence('UserManagementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
                title: "Internal Home",
                ncyBreadcrumb: {
                    label: 'Internal Home'
                }
            }).state('app.customerRegistration', {
                url: "/customerRegistration",
                templateUrl: "assets/views/CustomerRegistration.html?" + guid(),
                resolve: loadSequence('CustomerCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
                title: "Customer Registration",
                ncyBreadcrumb: {
                    label: 'Customer Registration'
                }
            }).state('app.bulkCustomerUpload', {
                url: "/bulkCustomerUpload",
                templateUrl: "assets/views/BulkCustomerUpload.html?" + guid(),
                resolve: loadSequence('BulkCustomerUploadCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
                title: "Bulk Upload",
                ncyBreadcrumb: {
                    label: 'Bulk Upload'
                }
            }).state('app.roleManagement', {
                url: "/roleManagement",
                templateUrl: "assets/views/RoleManagement.html?" + guid(),
                resolve: loadSequence('RoleManagementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'angular-touch', 'angular-animate', 'toaster'),
                title: "Internal Home",
                ncyBreadcrumb: {
                    label: 'Internal Home'
                }
            }).state('app.tpa', {
                url: "/tpa",
                templateUrl: "assets/views/tpaManagement.html?" + guid(),
                resolve: loadSequence('tpaCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage'),
                title: "TPA Management",
                ncyBreadcrumb: {
                    label: ' '
                }
            }).state('app.tpabranches', {
                url: "/tpabranches",
                templateUrl: "assets/views/tpaBranchesManagement.html?" + guid(),
                resolve: loadSequence('TpaBranchCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage', 'angular-jwt', 'ngCookies', 'toaster'),
                title: "TPA Branches",
                ncyBreadcrumb: {
                    label: 'TPA Branches'
                }

            }).state('app.policyRegistration', {
                url: "/policyRegistration",
                templateUrl: "assets/views/PolicyRegistration.html?" + guid(),
                resolve: loadSequence('angularFileUpload', 'PolicyRegistrationCtrlV2', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Policy Registration",
                ncyBreadcrumb: {
                    label: 'Policy Registration'
                }
            }).state('app.policyRegistrationDealer', {
                url: "/policyRegistrationDealer",
                templateUrl: "assets/views/PolicyRegistrationDealer.html?" + guid(),
                resolve: loadSequence('PolicyRegDealerCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Dealer Policy Registration",
                ncyBreadcrumb: {
                    label: 'Dealer Policy Registration'
                }
            }).state('app.policyRegistrationDemo', {
                url: "/SalesRegistration",
                templateUrl: "assets/views/PolicyRegistrationDemo.html?" + guid(),
                resolve: loadSequence('PolicyRegCtrlDemo', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'ngTable', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Dealer Policy Registration",
                ncyBreadcrumb: {
                    label: 'Dealer Policy Registration'
                }
            }).state('app.policyTransfer', {
                url: "/policyTransfer",
                templateUrl: "assets/views/PolicyTransfer.html?" + guid(),
                resolve: loadSequence('PolicyTransferCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Policy Transfer",
                ncyBreadcrumb: {
                    label: 'Policy Transfer'
                }
            }).state('app.policyApproval', {
                url: "/policyApproval",
                templateUrl: "assets/views/PolicyApproval.html?" + guid(),
                resolve: loadSequence('PolicyRegCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Policy Approval",
                ncyBreadcrumb: {
                    label: 'Policy Approval'
                }
            }).state('app.policyEndorsement', {
                url: "/policyEndorsement",
                templateUrl: "assets/views/PolicyEndorsement.html?" + guid(),
                resolve: loadSequence('PolicyEndorsementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Policy Endorsement",
                ncyBreadcrumb: {
                    label: 'Policy Endorsement'
                }
            }).state('app.policyEndorsementApproval', {
                url: "/policyEndorsementApproval",
                templateUrl: "assets/views/PolicyEndorsementApproval.html?" + guid(),
                resolve: loadSequence('PolicyEndorsementApprovalCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies'),
                title: "Policy Endorsement Approval",
                ncyBreadcrumb: {
                    label: 'Policy Endorsement Approval'
                }
            }).state('app.policyCancelationApproval', {
                url: "/policyCancelationApproval",
                templateUrl: "assets/views/PolicyCancelationApproval.html?" + guid(),
                resolve: loadSequence('PolicyCancelationApprovalCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies'),
                title: "Policy Cancelation Approval",
                ncyBreadcrumb: {
                    label: 'Policy Cancelation Approval'
                }
            }).state('app.policyCancelation', {
                url: "/policyCancelation",
                templateUrl: "assets/views/PolicyCancelation.html?" + guid(),
                resolve: loadSequence('PolicyCancelationCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies'),
                title: "Policy Cancelation",
                ncyBreadcrumb: {
                    label: 'Policy Cancelation'
                }
            }).state('app.policyInquiry', {
                url: "/policyInquiry",
                templateUrl: "assets/views/PolicyInquiry.html?" + guid(),
                resolve: loadSequence('PolicyInquiryCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'byteToKB','kbToReadableSize'),
                title: "Policy Inquiry",
                ncyBreadcrumb: {
                    label: 'Policy Inquiry'
                }
            }).state('app.policyRenewal', {
                url: "/policyRenewal",
                templateUrl: "assets/views/PolicyRenewal.html?" + guid(),
                resolve: loadSequence('PolicyRenewalCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster', 'byteToKB'),
                title: "Policy Renewal",
                ncyBreadcrumb: {
                    label: 'Policy Renewal'
                }
            }).state('app.currencyManagement', {
                url: "/currencyManagement",
                templateUrl: "assets/views/CurrencyManagement.html?" + guid(),
                resolve: loadSequence('CurrencyManagementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'ngCookies', 'toaster'),
                title: "Currency Management",
                ncyBreadcrumb: {
                    label: 'Currency Management'
                }
            }).state('app.premiumSetup', {
                url: "/premiumSetup",
                templateUrl: "assets/views/PremiumSetup.html?" + guid(),
                resolve: loadSequence('PremiumSetupManagementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow',
                    'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'toaster'),
                title: "Premium Setup",
                ncyBreadcrumb: {
                    label: 'Premium Setup'
                }
            }).state('app.customerProfile', {
                url: "/customerProfile",
                templateUrl: "assets/views/CustomerProfile.html?" + guid(),
                resolve: loadSequence('CustomerProfileCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow'),
                title: "Customer Profile",
                ncyBreadcrumb: {
                    label: 'Customer Profile'
                }
            }).state('app.taxManagement', {
                url: "/taxManagement",
                templateUrl: "assets/views/TaxManagement.html?" + guid(),
                resolve: loadSequence('TaxManagementCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster'),
                title: "Tax Management",
                ncyBreadcrumb: {
                    label: 'Tax Management'
                }
            }).state('app.userProfile', {
                url: "/userProfile",
                templateUrl: "assets/views/UserProfile.html?" + guid(),
                resolve: loadSequence('UserProfileCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow'),
                title: "User Profile",
                ncyBreadcrumb: {
                    label: 'User Profile'
                }
            }).state('home.customerPro', {
                url: "/customerPro",
                templateUrl: "assets/views/CustomerPro.html?" + guid(),
                resolve: loadSequence('CustomerProCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster'),
                title: "User Profile",
                ncyBreadcrumb: {
                    label: 'User Profile'
                }
            })
            //claim module
            .state('app.claimsubmission', {
                url: "/claim/claimsubmission",
                templateUrl: "assets/views/ClaimSubmission.html?" + guid(),
                resolve: loadSequence('angularFileUpload', 'ClaimSubmissionCtrl', 'ngTable', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow',
                    'ngDialog', 'ui.grid', 'toaster', 'ui.select', 'propsFilter', 'byteToKB', 'ui.mask'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimedit', {
                url: "/claim/edit/:claimId",
                templateUrl: "assets/views/ClaimEdit.html?" + guid(),
                resolve: loadSequence('angularFileUpload', 'ClaimEditCtrl', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'toaster', 'ui.select', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.partRejectionType', {
                url: "/claim/partRejectionType",
                templateUrl: "assets/views/PartRejectionType.html?" + guid(),
                resolve: loadSequence('PartRejectionTypeCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.partmanagement', {
                url: "/claim/partmanagement",
                templateUrl: "assets/views/PartManagement.html?" + guid(),
                resolve: loadSequence('PartCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.faultmanagement', {
                url: "/faultmanagement",
                templateUrl: "assets/views/FaultManagement.html?" + guid(),
                resolve: loadSequence('FaultManagementCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimbordxmanagement', {
                url: "/claimbordxmanagement",
                templateUrl: "assets/views/ClaimBordxManagement.html?" + guid(),
                resolve: loadSequence('ClaimBordxManagementCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.reinsurebordxreports', {
                url: "/reinsurebordxreports",
                templateUrl: "assets/views/ReinsureBordxReports.html?" + guid(),
                resolve: loadSequence('ReinsureBordxReportsCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'angular-touch', 'angular-animate', 'toaster'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.reinsurerbordx', {
                url: "/reinsurerbordx",
                templateUrl: "assets/views/ReinsurerBordx.html?" + guid(),
                resolve: loadSequence('ReinsurerBordxCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ngDialog', 'ui.grid', 'angularjs-dropdown-multiselect', 'underscore', 'angular-touch', 'angular-animate', 'toaster'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimbordxreopenmanagement', {
                url: "/claimbordxreopenmanagement",
                templateUrl: "assets/views/ClaimBordxReOpenManagement.html?" + guid(),
                resolve: loadSequence('ClaimBordxReOpenManagementCtrl', 'ngTable', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert', 'toaster'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimbordxyearlyreopenmanagement', {
                url: "/claimbordxyearlyreopenmanagement",
                templateUrl: "assets/views/ClaimBordxYearlyReOpenManagement.html?" + guid(),
                resolve: loadSequence('ClaimBordxYearlyReOpenManagementCtrl', 'ngTable', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert', 'toaster'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.reinsurerreceipt', {
                url: "/reinsurerreceipt",
                templateUrl: "assets/views/ReinsurerReceipt.html?" + guid(),
                resolve: loadSequence('ReinsurerReceiptCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.incurrederning', {
                url: "/incurrederning",
                templateUrl: "assets/views/IncurredErning.html?" + guid(),
                resolve: loadSequence('IncurredErningCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'ui.grid', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.dealercheques', {
                url: "/dealercheques",
                templateUrl: "assets/views/DealerCheques.html?" + guid(),
                resolve: loadSequence('DealerChequesCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'ui.grid', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimlisting', {
                url: "/claim/listing",
                templateUrl: "assets/views/ClaimListing.html?" + guid(),
                resolve: loadSequence('ngTable', 'ClaimListing', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimlistingclaim', {
                url: "/claim/listing/:claimId",
                templateUrl: "assets/views/ClaimListing.html?" + guid(),
                resolve: loadSequence('ngTable', 'ClaimListing', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimbordxyearprocess', {
                url: "/claimbordxyearprocess",
                templateUrl: "assets/views/ClaimBordxYearProcess.html?" + guid(),
                resolve: loadSequence('ClaimBordxYearProcessCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.grid'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claiminvoiceentry', {
                url: "/claim/invoice",
                templateUrl: "assets/views/ClaimInvoiceEntry.html?" + guid(),
                resolve: loadSequence('jquery-sparkline', 'ClaimInvoiceCtrl', 'angularFileUpload', 'menuBuilderCtrl', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.grid', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimbatching', {
                url: "/claim/claimbatching",
                templateUrl: "assets/views/ClaimBatching.html?" + guid(),
                resolve: loadSequence('ngTable', 'ClaimBatchingCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimendorsement', {
                url: "/claim/claimendorsement",
                templateUrl: "assets/views/ClaimEndorsement.html?" + guid(),
                resolve: loadSequence('ngTable', 'angularFileUpload', 'ClaimEndorsementCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claiminquiry', {
                url: "/claim/claiminquiry",
                templateUrl: "assets/views/ClaimInquiry.html?" + guid(),
                resolve: loadSequence('ngTable', 'angularFileUpload', 'ClaimInquiryCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimendorsementapproval', {
                url: "/claim/claimendorsementapproval",
                templateUrl: "assets/views/ClaimEndorsementApproval.html?" + guid(),
                resolve: loadSequence('ngTable', 'angularFileUpload', 'ClaimEndorsementApprovalCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimprocess', {
                url: "/claim/process/:claimId",
                templateUrl: "assets/views/ClaimProcess.html?" + guid(),
                resolve: loadSequence('ngTable', 'angularFileUpload', 'ClaimProcessCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'angularjs-dropdown-multiselect', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimprocessbank', {
                url: "/claim/processbank/:claimId",
                templateUrl: "assets/views/ClaimProcessBank.html?" + guid(),
                resolve: loadSequence('ClaimProcessCtrlB2', 'ngTable', 'angularFileUpload', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'angularjs-dropdown-multiselect', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.claimendorse', {
                url: "/claim/process/:claimId/:endorse",
                templateUrl: "assets/views/ClaimProcess.html?" + guid(),
                resolve: loadSequence('ngTable', 'angularFileUpload', 'ClaimProcessCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid', 'propsFilter', 'byteToKB'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.bordxprocess', {
                url: "/bordxprocess",
                templateUrl: "assets/views/BordxProcess.html?" + guid(),
                resolve: loadSequence('ngTable', 'BordxProcessCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('app.dealerdiscountmanagement', {
                url: "/dealerdiscount",
                templateUrl: "assets/views/DealerDiscountManagement.html?" + guid(),
                resolve: loadSequence('DealerDiscountCtrl', 'angularFileUpload', 'ngDialog', 'sweet-alert', 'oitozero.ngSweetAlert', 'flow', 'toaster', 'ui.select', 'ui.grid'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            }).state('error', {
                url: '/error',
                template: '<div ui-view class="fade-in-up"></div>'
            }).state('error.404', {
                url: '/404',
                templateUrl: "assets/views/utility_404.html?" + guid(),
            }).state('error.500', {
                url: '/500',
                templateUrl: "assets/views/utility_500.html?" + guid(),
            }).state('app.tastpa', {
                url: "/tastpa",
                templateUrl: "assets/views/tasTpaManagement.html?" + guid(),
                resolve: loadSequence('tasTpaCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngCookies'),
                title: "TPA Management",
                ncyBreadcrumb: {
                    label: ' '
                }
            })

            .state('app.broker', {
                url: "/broker",
                templateUrl: "assets/views/brokerManagement.html?" + guid(),
                resolve: loadSequence('brokerCtrl', 'angularFileUpload', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage'),
                title: "Broker Management",
                ncyBreadcrumb: {
                    label: ' '
                }
            })

            .state('app.reportExplorer', {
                url: "/reportexplorer",
                templateUrl: "assets/views/ReportExplorer.html?" + guid(),
                resolve: loadSequence('ReportExplorerCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage'),
                title: "Broker Management",
                ncyBreadcrumb: {
                    label: ' '
                }
            })
            // Login routes
            .state('home', {
                url: "/home",
                templateUrl: "assets/views/empty.html?" + guid(),
                abstract: true,
                resolve: loadSequence('modernizr', 'moment', 'angularMoment', 'uiSwitch', 'perfect-scrollbar-plugin',
                    'toaster', 'ngAside', 'vAccordion', 'sweet-alert', 'chartjs', 'tc.chartjs', 'oitozero.ngSweetAlert',
                    'chatCtrl', 'googlechart'),
                title: "Home",
                ncyBreadcrumb: {
                    label: 'Home'
                }
            }).state('home.products', {
                url: "/products/:tpaId",
                templateUrl: "assets/views/home.html?" + guid(),
                resolve: loadSequence('TpaHomeCtrl', 'bootstrapminjs', 'ngStorage', 'ngCookies', 'mwl.calendar'),  //, 'angular-strap'
                // controller: 'ViewMessageCrtl',
                title: "Home",
                ncyBreadcrumb: {
                    label: 'Home'
                }
            })
            .state('app.dealerInvoceCodeGenerationUpdate', {
                url: "/dealerinvoicecodenew",
                templateUrl: "assets/views/dealerInvoceCodeGenerationUpdate.html?" + guid(),
                resolve: loadSequence('DealerInvoiceCodeGenerationUpdateCtrl', 'angularFileUpload', 'modernizr', 'moment', 'angularMoment' ,
                                            'asideCtrl', 'ngStorage', 'ngCookies', 'sweet-alert', 'ngDialog', 'ui.grid', 'oitozero.ngSweetAlert',
                    'toaster', 'flow', 'ui.select', 'ui.mask', 'angularjs-dropdown-multiselect', 'mwl.calendar'),
                title: "",
                ncyBreadcrumb: {
                    label: ''
                }
            })
            .state('home.homePremium', {
                url: "/homePremium/:prodId",
                templateUrl: "assets/views/home_premium.html?" + guid(),
                resolve: loadSequence('PremiumDetailsCtrl', 'angularFileUpload', 'modernizr', 'moment', 'angularMoment', 'asideCtrl',
                    'ngStorage', 'ngCookies',
                    'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore',
                    'ngDialog', 'mwl.calendar'),
                title: "Home",
                ncyBreadcrumb: {
                    label: 'Home'
                }
            })
            .state('home.homePremiumWithTemp', {
                url: "/homePremium/:prodId/:tempInvId",
                templateUrl: "assets/views/home_premium.html?" + guid(),
                resolve: loadSequence('PremiumDetailsCtrl', 'angularFileUpload', 'modernizr', 'moment', 'angularMoment', 'asideCtrl',
                    'ngStorage', 'ngCookies',
                    'sweet-alert', 'oitozero.ngSweetAlert', 'angularjs-dropdown-multiselect', 'underscore',
                    'ngDialog', 'mwl.calendar'),
                title: "Home",
                ncyBreadcrumb: {
                    label: 'Home'
                }
            })

            .state('home.buyingprocess', {
                url: "/buyingprocess/:tempInvId",
                templateUrl: "assets/views/buying_wizard.html?" + guid(),
                resolve: loadSequence('BuyingWizardCtrl', 'sweet-alert', 'oitozero.ngSweetAlert', 'ngStorage',
                    'angular-jwt', 'ngCookies', 'ngDialog', 'flow', 'toaster', 'ui.select'),
                title: "Home Premium",
                ncyBreadcrumb: {
                    label: 'Home Premium'
                }
            }).state('home.login', {
                url: "/login/:tpaId",
                templateUrl: "assets/views/home_login.html?" + guid(),
                resolve: loadSequence('homeLoginCtrl', 'ngStorage', 'angular-jwt', 'ngCookies', 'sweet-alert', 'oitozero.ngSweetAlert'),
                //resolve: loadSequence('BuyingWizardCtrl'),
                title: "Home Premium",
                ncyBreadcrumb: {
                    label: 'Home Premium'
                }
            })
            .state('login', {
                url: '/login',
                template: '<div ui-view class="fade-in-right-big smooth" ></div>',
                abstract: true,
                resolve: loadSequence('sweet-alert', 'oitozero.ngSweetAlert')
            }).state('login.signin', {
                url: '/signin/:tpaId',
                templateUrl: "assets/views/login_login.html?" + guid(),
                resolve: loadSequence('loginCtrl', 'ngStorage', 'angular-jwt', 'ngCookies')
            }).state('login.tas', {
                url: '/tas',
                templateUrl: "assets/views/main_login.html?" + guid(),
                resolve: loadSequence('mainLoginCtrl', 'ngStorage', 'angular-jwt', 'ngCookies', 'LocationCtrl', 'ui.mask')
            }).state('login.forgot', {
                url: '/forgot/:tpaId',
                templateUrl: "assets/views/login_forgot.html?" + guid(),
                resolve: loadSequence('loginForgetPasswordCtrl', 'ngStorage', 'angular-jwt', 'ngCookies')
            }).state('login.registration', {
                url: '/registration',
                templateUrl: "assets/views/login_registration.html?" + guid(),
            }).state('login.lockscreen', {
                url: '/lock',
                templateUrl: "assets/views/login_lock_screen.html?" + guid(),

            })
            .state('login.Verifyemail', {
                url: '/Verifyemail/:customerId',
                templateUrl: "assets/views/Verifyemail.html?" + guid(),
                resolve: loadSequence('verifyEmailCtrl', 'ngStorage', 'angular-jwt', 'ngCookies')
            })
            .state('login.changepaswordcustomer', {
                url: '/changepaswordcustomer/:requestId/:currentCustomerId/:tpaId',
                templateUrl: "assets/views/login_changepasword_customer.html?" + guid(),
                resolve: loadSequence('loginChangePasswordCustomerCtrl', 'ngStorage', 'angular-jwt', 'ngCookies')
            })
            .state('login.changepasword', {
                url: '/changepasword/:requestId/:tpaId',
                templateUrl: "assets/views/login_changepassword.html?" + guid(),
                resolve: loadSequence('loginChangePasswordCtrl', 'ngStorage', 'angular-jwt', 'ngCookies')

            });

        // Generates a resolve object previously configured in constant.JS_REQUIRES (config.constant.js)
        function loadSequence() {
            var _args = arguments;
            return {
                deps: ['$ocLazyLoad', '$q',
                    function ($ocLL, $q) {
                        var promise = $q.when(1);
                        for (var i = 0, len = _args.length; i < len; i++) {
                            promise = promiseThen(_args[i]);
                        }
                        return promise;

                        function promiseThen(_arg) {
                            if (typeof _arg == 'function')
                                return promise.then(_arg);
                            else
                                return promise.then(function () {
                                    var nowLoad = requiredData(_arg);
                                    if (!nowLoad)
                                        return $.error('Route resolve: Bad resource name [' + _arg + ']');
                                    return $ocLL.load(nowLoad);
                                });
                        }

                        function requiredData(name) {
                            if (jsRequires.modules)
                                for (var m in jsRequires.modules)
                                    if (jsRequires.modules[m].name && jsRequires.modules[m].name === name)
                                        return jsRequires.modules[m];
                            return jsRequires.scripts && jsRequires.scripts[name];
                        }
                    }]
            };
        }


        //create guids

        //create guids
        function guid() {
            //function s4() {
            //    return Math.floor((1 + Math.random()) * 0x10000)
            //      .toString(16)
            //      .substring(1);
            //}
            //return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
            //  s4() + '-' + s4() + s4() + s4();

            return "098A1E73-58FB-4F79-8AB9-74DA52943DB2";
        }
    }]);