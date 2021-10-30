app.controller('BulkCustomerUploadCtrl',
    function ($scope, $rootScope, $http, SweetAlert, $localStorage, ngDialog, FileUploader, toaster) {
        $scope.ModalName = "Bulk Upload";
        $scope.ModalDescription = "";
        $scope.DateFieldDisabled = false;
        document.getElementById("loading").style.display = "none";
        $scope.errorTab1 = "";
        $scope.CommodityType = "";
        $scope.DataList = [];
        $scope.Product = "";
        createGrid();
        function LoadDetails() {
            swal({ title: "TAS Information", text: "Loading. Please wait ...", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/MakeAndModelManagement/GetAllCommodities',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            }).success(function (data, status, headers, config) {
                $scope.CommodityTypes = data;
                if (data.length > 0) { $scope.CommodityType = data[0]; loadProducts(); }
                else { createGrid(); }
            }).error(function (data, status, headers, config) {
            });
        }

        function loadProducts() {
            swal({ title: "TAS Information", text: "Loading. Please wait ...", showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/Product/GetAllProducts',
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt },
                data: { "Id": $scope.CommodityType.CommodityTypeId }
            }).success(function (data, status, headers, config) {
                $scope.Products = data;
                if (data.length > 0) $scope.Product = data[0];
            }).error(function (data, status, headers, config) {
            }).finally(function (data, status, headers, config) { createGrid(); });
        }

        LoadDetails();
        $scope.HeaderData = { UserId: $localStorage.LoggedInUserId, TPAId: $localStorage.tpaID, StartDate: '', EndDate: '', CommodityTypeId: $scope.CommodityType.CommodityTypeId, CommodityCode: "", Path: "" };
        $scope.checkDate = function (startDate, endDate) {
            $scope.errorTab1 = "";
            var curDate = new Date();
            if (startDate != "" && endDate != "") {
                if (new Date(startDate) > new Date(endDate)) {
                    $scope.errorTab1 = 'End date should be greater than start date';
                    return false;
                }
                else return true;
            }
            else return true;
        }
        $scope.CommodityChange = function () { loadProducts(); }
        $scope.Path = "";
        var customErrorMessage = function (msg) { toaster.pop('error', 'Error', msg); };
        var customInfoMessage = function (msg) { toaster.pop('info', 'Information', msg, 12000); };
        function createGrid() {
            $scope.List = [];
            $scope.rowFormatterRed = function (row) {
                return (!(row.entity.ValidationError === null || row.entity.ValidationError === ""));
            };
            $scope.rowFormatter = function (row) {
                return (row.entity.ValidationError === null || row.entity.ValidationError === "");
            };
            $scope.ColumnList = [{ field: "TempBulkUploadId", displayName: "", width: 0, visible: false },
            {
                field: "Colour", displayName: "", width: 30, enableColumnResizing: false, enableCellEdit: false
                , cellTemplate: '<div><div style="display: block; border-radius: 50%; height: 29px; width: 100%; margin: 0;" ng-class="{ \'green\': grid.appScope.rowFormatter(row) , \'red\': grid.appScope.rowFormatterRed(row)}"></div>'
            },
            { field: "ProductCode", displayName: "Product Code", width: 150 },
            { field: "DealerCode", displayName: "Dealer Code", width: 150 },
            { field: "DealerLocationCode", displayName: "Dealer Location", width: 150 },
            { field: "PolicyNo", displayName: "Policy No", width: 150 },
            { field: "ManufacturerWarrantyMonths", displayName: "Manufacturer Warranty Months", width: 150 },
            { field: "ManufacturerWarrantyKm", displayName: "Manufacturer Warranty Km", width: 150 },
            { field: "ManufacturerWarrantyApplicableFrom", displayName: "Manufacturer Warranty Applicable From", width: 150 },
            { field: "ExtensionPeriod", displayName: "Extension Period", width: 150 },
            { field: "ExtensionMileage ", displayName: "Extension Mileage", width: 150 },
            { field: "ExtensionTypeCode", displayName: "Extension Type", width: 150 },
            { field: "CoverTypeCode", displayName: "Warranty Type", width: 150 },
            {
                field: "MWStartDate", displayName: "MW Start Date", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.MWStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },
            //{
            //    field: "MWStartDate", displayName: "MW Start Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
            //    , cellTemplate: '<div><input ng-model="row.entity.MWStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            //},
            {
                field: "PolicySoldDate", displayName: "Policy Sold Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.PolicySoldDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },
            {
                field: "PolicyStartDate", displayName: "Policy Start Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.PolicyStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },
            {
                field: "PolicyEndDate", displayName: "Policy End Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.PolicyEndDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },
            { field: "CustomerType", displayName: "Customer Type", width: 100 },
            { field: "UsageType", displayName: "Usage Type", width: 100 },
            { field: "Premium", displayName: "Gross Premium (USD)", width: 150, cellFilter: 'number: 2' },
            { field: "SalesPersonCode", displayName: "Sales Person", width: 150 },

            { field: "BusinessName", displayName: "Business Name", width: 150 },
            { field: "BusinessTelephoneNo", displayName: "Business Telephone No", width: 150 },
            { field: "BusinessAddress1", displayName: "Business Address 1", width: 150 },
            { field: "BusinessAddress2", displayName: "Business Address 2", width: 150 },
            { field: "BusinessAddress3", displayName: "Business Address 3", width: 150 },
            { field: "BusinessAddress4", displayName: "Business Address 4", width: 150 },
            { field: "ContactPersonFirstName", displayName: "Contact Person First Name", width: 150 },
            { field: "ContactPersonLastName", displayName: "Contact Person Last Name", width: 150 },
            { field: "ContactPersonMobileNo", displayName: "Contact Person Mobile No", width: 150 },
            { field: "Country", displayName: "Country", width: 100 },
            { field: "City", displayName: "City", width: 100 },
            { field: "Email", displayName: "Email Address", width: 100 },

            { field: "FirstName", displayName: "First Name", width: 150 },
            { field: "LastName", displayName: "Last Name", width: 150 },
            { field: "Title", displayName: "Title", width: 50 },
            { field: "Occupation", displayName: "Occupation", width: 100 },
            { field: "MaritalStatus", displayName: "Marital Status", width: 60 },

            { field: "Nationality", displayName: "Nationality", width: 100 },
            { field: "PostalCode", displayName: "Postal Code", width: 100 },

            { field: "MobilePhone", displayName: "Mobile Phone", width: 100 },
            { field: "OtherPhone", displayName: "Other Phone", width: 100 },
            {
                field: "DateOfBirth", displayName: "Date Of Birth", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.DateOfBirth" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },

            { field: "Gender", displayName: "Gender", width: 50 },


            { field: "Address1", displayName: "Address 1", width: 100 },
            { field: "Address2", displayName: "Address 2", width: 100 },
            { field: "Address3", displayName: "Address 3", width: 100 },
            { field: "Address4", displayName: "Address 4", width: 100 },
            { field: "IDType", displayName: "ID Type", width: 100 },
            { field: "IDNo", displayName: "ID No", width: 100 },
            {
                field: "DrivingLicenseIssueDate", displayName: "Driving  License Issue Date", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                , cellTemplate: '<div><input ng-model="row.entity.DrivingIssueDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
            },
            ];

            if ($scope.CommodityType != "") {
                if ($scope.CommodityType.CommodityCode == "A") {
                    $scope.AutomobileColumnList = [
                        { field: "ContractCode", displayName: "Contract", width: 150 },
                        { field: "KmAtPolicySale", displayName: "Km At Policy Sale", width: 150 },
                        { field: "VINNo", displayName: "VINNo", width: 150 },
                        { field: "MakeCode", displayName: "Make", width: 150 },
                        { field: "ModelCode", displayName: "Model", width: 150 },
                        { field: "CategoryCode", displayName: "Category", width: 150 },
                        { field: "ItemStatusCode", displayName: "Item Status", width: 150 },
                        { field: "CylinderCountCode", displayName: "Cylinder Count", width: 150 },
                        { field: "BodyTypeCode", displayName: "Body Type", width: 150 },
                        { field: "FuelTypeCode", displayName: "Fuel Type", width: 150 },
                        { field: "AspirationCode", displayName: "Aspiration", width: 150 },
                        { field: "TransmissionCode", displayName: "Transmission Type", width: 150 },
                        {
                            field: "ItemPurchasedDate", displayName: "Vehicle Purchased Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.ItemPurchasedDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        {
                            field: "VehicleRegistrationDate", displayName: "Vehicle Registration Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.VehicleRegistrationDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },

                        { field: "EngineCapacityCode", displayName: "Engine Capacity  (CC)", width: 150 },
                        { field: "DriveTypeCode", displayName: "Drive Type", width: 150 },
                        { field: "CommodityUsageType", displayName: "Commodity Usage Type", width: 150 },
                        { field: "VariantCode", displayName: "Variant", width: 150 },
                        { field: "PlateNo", displayName: "PlateNo", width: 150 },
                        { field: "ModelYear", displayName: "ModelYear", width: 80, cellFilter: 'number: 0' },
                        { field: "VehiclePrice", displayName: "Vehicle Price (USD)", width: 150, cellFilter: 'number: 2' },
                        { field: "IsSpecialDeal", displayName: "IsSpecialDeal", width: 150 },
                        { field: "GrossWeight", displayName: "Gross Vehicle Weight (Tonnage)", width: 150, cellFilter: 'number: 2' },
                    ];
                    $scope.ColumnList = $scope.ColumnList.concat($scope.AutomobileColumnList);

                }
                else if ($scope.CommodityType.CommodityCode == "B") {

                    if ($scope.Product.ProductTypeCode = "ILOE") {
                        $scope.ColumnList = [{ field: "TempBulkUploadId", displayName: "", width: 0, visible: false },
                        {
                            field: "Colour", displayName: "", width: 30, enableColumnResizing: false, enableCellEdit: false
                            , cellTemplate: '<div><div style="display: block; border-radius: 50%; height: 29px; width: 100%; margin: 0;" ng-class="{ \'green\': grid.appScope.rowFormatter(row) , \'red\': grid.appScope.rowFormatterRed(row)}"></div>'
                        },
                        { field: "Dealer", displayName: "Finance Company", width: 150 },
                        { field: "FirstName", displayName: "First Name", width: 150 },
                        { field: "LastName", displayName: "Last Name", width: 150 },
                        {
                            field: "DateOfBirth", displayName: "Date Of Birth", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.DateOfBirth" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        { field: "IDType", displayName: "ID Type", width: 100 },
                        { field: "IDNo", displayName: "ID No", width: 100 },
                        { field: "Nationality", displayName: "Nationality", width: 100 },
                        { field: "Email", displayName: "Email Address", width: 100 },
                        { field: "VINNo", displayName: "VINNo", width: 150 },
                        { field: "Make", displayName: "Make", width: 150 },
                        { field: "Model", displayName: "Model", width: 150 },
                        { field: "Country", displayName: "Country", width: 100 },
                        { field: "PlateNo", displayName: "Registration Number", width: 150 },
                        {
                            field: "ItemPurchasedDate", displayName: "Vehicle Purchased Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.ItemPurchasedDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        { field: "VehiclePrice", displayName: "Vehicle Price (USD)", width: 150, cellFilter: 'number: 2' },
                        { field: "Deposit", displayName: "Deposit", width: 150, cellFilter: 'number: 2' },
                        { field: "FinanceAmount", displayName: "Finance Amount", width: 150, cellFilter: 'number: 2' },
                        { field: "LoanPeriod", displayName: "Loan Period(Months)", width: 150, cellFilter: 'number: 0' },
                        { field: "PeriodOfCover", displayName: "Period Of Cover(Months)", width: 150, cellFilter: 'number: 2' },
                        { field: "MonthlyEMI", displayName: "Monthly EMI", width: 150, cellFilter: 'number: 2' },
                        { field: "InterestRate", displayName: "Interest Rate (%)", width: 150, cellFilter: 'number: 2' },
                        { field: "GrossPremiumExcludingTAX", displayName: "Gross Premium Excluding TAX", width: 150, cellFilter: 'number: 2' },
                        ]
                    }
                }
                else if ($scope.CommodityType.CommodityCode == "E") {
                    $scope.ElectronicColumnList = [

                        { field: "ProductCode", displayName: "ProductCode", width: 150 },
                        { field: "Dealer", displayName: "Dealer", width: 150 },
                        { field: "DealerLocation", displayName: "DealerLocation", width: 150 },
                        { field: "ContractCode", displayName: "ContractCode", width: 150 },
                        { field: "ExtensionTypeCode", displayName: "ExtensionTypeCode", width: 150 },
                        { field: "WarrantyType", displayName: "WarrantyType", width: 150 },
                        { field: "SalesPerson", displayName: "SalesPerson", width: 150 },
                        { field: "HrsAtPolicySale", displayName: "HrsAtPolicySale", width: 150 },
                        //{ field: "Comment", displayName: "Comment", width: 150 },
                        { field: "Premium", displayName: "Gross Premium (USD)", width: 150, cellFilter: 'number: 2' },
                        {
                            field: "PolicySoldDate", displayName: "PolicySoldDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.PolicySoldDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        {
                            field: "PolicyStartDate", displayName: "PolicyStartDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.PolicyStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        {
                            field: "PolicyEndDate", displayName: "PolicyEndDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.PolicyEndDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        //{ field: "Discount", displayName: "Discount", width: 150, cellFilter: 'number: 2' },

                        //----------------------------------------------------------------------------------

                        { field: "SerialNo", displayName: "SerialNo", width: 150 },
                        { field: "Make", displayName: "Make", width: 150 },
                        { field: "Model", displayName: "Model", width: 150 },
                        { field: "Category", displayName: "Category", width: 150 },
                        { field: "ItemStatus", displayName: "ItemStatus", width: 150 },
                        {
                            field: "ItemPurchasedDate", displayName: "Vehicle Purchased Date", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.ItemPurchasedDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        { field: "CommodityUsageTypeCode", displayName: "Usage Type", width: 150 },
                        { field: "ModelYear", displayName: "ModelYear", width: 150 },
                        { field: "VehiclePrice", displayName: "Vehicle Price (USD)", width: 150, cellFilter: 'number: 2' },
                        { field: "IsSpecialDeal", displayName: "IsSpecialDeal", width: 150 },
                        //{ field: "DealerPolicy", displayName: "DealerPolicy", width: 150 },
                        {
                            field: "MWStartDate", displayName: "MWStartDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
                            , cellTemplate: '<div><input ng-model="row.entity.MWStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
                        },
                        { field: "PolicyNo", displayName: "PolicyNo", width: 150 }

                    ];
                    $scope.ColumnList = $scope.ColumnList.concat($scope.ElectronicColumnList);
                }
                else {

                }
                $scope.ColumnList.push({ field: "ValidationError", displayName: "Error Message", width: 400, enableCellEdit: false });
            }

            $scope.gridOptions = {
                data: 'List',
                paginationPageSizes: [5, 10, 20],
                paginationPageSize: 10,
                enablePaginationControls: true,
                enableRowSelection: true,
                enableCellSelection: true,
                enableCellEditOnFocus: true,
                enableSelectAll: true,
                enableRowHeaderSelection: true,
                enableColumnResizing: true,
                columnDefs: $scope.ColumnList
            };

            $scope.gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
            }

            swal.close();
            //}
        }

        //function validate(row) {
        //    var retVal = true;
        //    var phoneno = /^\d{10,15}$/;
        //    var validationMessage = "";
        //    // Customer Validation-------------------------------------------------------------------

        //    if (!(row.FirstName != null && row.FirstName.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "First name can't be empty"; retVal = false; }
        //    if (!(row.Title != null && row.Title.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Title can't be empty"; retVal = false; }
        //    if (!(row.MaritalStatus != null && row.MaritalStatus.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Marital status can't be empty"; retVal = false; }
        //    if (!(row.Country != null && row.Country.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Country can't be empty"; retVal = false; }
        //    if (!(row.City != null && row.City.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "City can't be empty"; retVal = false; }
        //    if (!(row.Email != null && row.Email.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Email can't be empty"; retVal = false; }
        //    if (!(row.MobilePhone != null && row.MobilePhone.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Mobile phone number can't be empty"; retVal = false; }
        //    if (!(row.Gender != null && row.Gender.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Gender can't be empty"; retVal = false; }
        //    if (!(row.CustomerType != null && row.CustomerType.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Customer type can't be empty"; retVal = false; }
        //    if (!(row.UsageType != null && row.UsageType.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Usage type can't be empty"; retVal = false; }
        //    if (!(row.IDType != null && row.IDType.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "ID type can't be empty"; retVal = false; }
        //    if (!(row.IDNo != null && row.IDNo.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "ID number can't be empty"; retVal = false; }
        //    if (!row.MobilePhone.replace(/[\s]/g, '').match(phoneno)) { //|| !$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(phoneno)) {
        //        //$scope.errorTab1 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
        //        validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Please enter valid Mobile Phone Number / Length should be greater than or equal to 10 and less than 16 ";
        //        retVal = false;
        //    }
        //    if ((row.OtherPhone != null && row.OtherPhone.trim() != "")) {
        //        if (!row.OtherPhone.replace(/[\s]/g, '').match(phoneno)) {
        //            //&& (!$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(phoneno))) {
        //            //$scope.errorTab1 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Please enter valid Other Phone Number / Length should be greater than or equal to 10 and less than 16 ";
        //            retVal = false;
        //        }
        //    }
        //    if ((row.BusinessTelephoneNo != null && row.BusinessTelephoneNo.trim() != "")) {
        //        if (!row.BusinessTelephoneNo.replace(/[\s]/g, '').match(phoneno)) {
        //            //&& (!$scope.Customer.OtherTelNo.replace(/[\s]/g, '').match(phoneno))) {
        //            //$scope.errorTab1 = "Please enter valid Telephone / Fax No: Length should be greater than or equal to 10 and less than 16";
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Please enter valid Business Phone Number / Length should be greater than or equal to 10 and less than 16 ";
        //            retVal = false;
        //        }
        //    }

        //    //if (!(row.DealerPolicy != null && row.DealerPolicy != "")) {
        //    //    { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Dealer Policy can't be empty"; retVal = false; }
        //    //}

        //    if (!(row.MWStartDate != null && row.MWStartDate.trim() != "")) {
        //        { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Manufature Warrenty Start Date can't be empty"; retVal = false; }
        //    }
        //    if (!(row.PolicyNo != null && row.PolicyNo.trim() != "")) {
        //        { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy No can't be empty"; retVal = false; }
        //    }
        //    //-------------------------------------------------------------------------------------------------------------------------------------

        //    if ($scope.CommodityType.CommodityCode == "A") {
        //        if (!(row.ProductCode != null && row.ProductCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Product Code can't be empty"; retVal = false; }
        //        if (!(row.DealerCode != null && row.DealerCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Dealer Code can't be empty"; retVal = false; }
        //        if (!(row.DealerLocationCode != null && row.DealerLocationCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Dealer Location Code can't be empty"; retVal = false; }
        //        if (!(row.ContractCode != null && row.ContractCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Contract Code can't be empty"; retVal = false; }
        //        if (!(row.ExtensionTypeCode != null && row.ExtensionTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Extension Type Code can't be empty"; retVal = false; }
        //        if (!(row.CoverTypeCode != null && row.CoverTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Cover Type Code can't be empty"; retVal = false; }
        //        if (!(row.SalesPersonCode != null && row.SalesPersonCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Sales Person Code  can't be empty"; retVal = false; }
        //        if (!(row.KmAtPolicySale != null && row.KmAtPolicySale.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "KmAtPolicySale can't be empty"; retVal = false; }
        //        //if (!(row.Comment != null && row.Comment.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Comment can't be empty"; retVal = false; }
        //        //if (!(row.Premium != null && row.Premium.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium can't be empty"; retVal = false; }
        //        if (!(row.PolicySoldDate != null && row.PolicySoldDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy Sold Date can't be empty"; retVal = false; }
        //        if (!(row.PolicyStartDate != null && row.PolicyStartDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy Start Date can't be empty"; retVal = false; }
        //        if (!(row.PolicyEndDate != null && row.PolicyEndDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy End Date can't be empty"; retVal = false; }
        //        if (!(row.Discount != null)) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Discount can't be empty"; retVal = false; }
        //        //if (!(row.ProviderCode != null && row.ProviderCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Provider Code Code can't be empty"; retVal = false; }
        //        if (!(row.VINNo != null && row.VINNo.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "VIN No  can't be empty"; retVal = false; }
        //        if (!(row.MakeCode != null && row.MakeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Make Code can't be empty"; retVal = false; }
        //        if (!(row.ModelCode != null && row.ModelCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Model Code can't be empty"; retVal = false; }
        //        if (!(row.CategoryCode != null && row.CategoryCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Category Code can't be empty"; retVal = false; }
        //        if (!(row.ItemStatusCode != null && row.ItemStatusCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Item Status Code can't be empty"; retVal = false; }
        //        if (!(row.CylinderCountCode != null && row.CylinderCountCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Cylinder Count Code can't be empty"; retVal = false; }
        //        if (!(row.BodyTypeCode != null && row.BodyTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Body Type Code can't be empty"; retVal = false; }
        //        if (!(row.FuelTypeCode != null && row.FuelTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Fuel Type Code can't be empty"; retVal = false; }
        //        if (!(row.AspirationCode != null && row.AspirationCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Aspiration Code can't be empty"; retVal = false; }
        //        if (!(row.TransmissionCode != null && row.TransmissionCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Transmission Code can't be empty"; retVal = false; }
        //        if (!(row.ItemPurchasedDate != null && row.ItemPurchasedDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Item Purchased Date can't be empty"; retVal = false; }
        //        if (!(row.EngineCapacityCode != null && row.EngineCapacityCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Engine Capacity Code  can't be empty"; retVal = false; }
        //        if (!(row.DriveTypeCode != null && row.DriveTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Drive Type Code can't be empty"; retVal = false; }
        //        if (!(row.CommodityUsageTypeCode != null && row.CommodityUsageTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Commodity Usage Type Code can't be empty"; retVal = false; }
        //        if (!(row.VariantCode != null && row.VariantCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Variant Code can't be empty"; retVal = false; }
        //        if (!(row.PlateNo != null && row.PlateNo.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Plate No can't be empty"; retVal = false; }
        //        if (!(row.GrossWeight != null)) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Gross Weight can't be empty"; retVal = false; }
        //        if (!(row.ModelYear != null && row.ModelYear.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Model Year can't be empty"; retVal = false; }
        //        if (!(row.Premium != null)) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium can't be empty"; retVal = false;
        //        }
        //        else if (row.Premium == 0) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium  can't be zero"; retVal = false;
        //        }
        //        if (!(row.VehiclePrice != null)) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Price can't be empty"; retVal = false;
        //        }
        //        else if (row.VehiclePrice == 0) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Price  can't be zero"; retVal = false;
        //        }
        //        if (!(row.IsSpecialDeal != null && row.IsSpecialDeal != "")) {
        //            { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "IsSpecialDeal can't be empty"; retVal = false; }
        //        }

        //    }
        //    else if ($scope.CommodityType.CommodityCode == "E") {

        //        if (!(row.ProductCode != null && row.ProductCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Product Code can't be empty"; retVal = false; }
        //        if (!(row.Dealer != null && row.Dealer.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Dealer Code can't be empty"; retVal = false; }
        //        if (!(row.DealerLocation != null && row.DealerLocation.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Dealer Location Code can't be empty"; retVal = false; }
        //        if (!(row.ContractCode != null && row.ContractCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Contract Code can't be empty"; retVal = false; }
        //        if (!(row.ExtensionTypeCode != null && row.ExtensionTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Extension Type Code can't be empty"; retVal = false; }
        //        if (!(row.WarrantyType != null && row.WarrantyType.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Cover Type Code can't be empty"; retVal = false; }
        //        if (!(row.SalesPerson != null && row.SalesPerson.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Sales Person Code  can't be empty"; retVal = false; }
        //        if (!(row.HrsAtPolicySale != null && row.HrsAtPolicySale.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "HrsAtPolicySale can't be empty"; retVal = false; }
        //        //if (!(row.Comment != null && row.Comment.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Comment can't be empty"; retVal = false; }
        //        //if (!(row.Premium != null && row.Premium.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium can't be empty"; retVal = false; }
        //        if (!(row.PolicySoldDate != null && row.PolicySoldDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy Sold Date can't be empty"; retVal = false; }
        //        if (!(row.PolicyStartDate != null && row.PolicyStartDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy Start Date can't be empty"; retVal = false; }
        //        if (!(row.PolicyEndDate != null && row.PolicyEndDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Policy End Date can't be empty"; retVal = false; }
        //        if (!(row.Discount != null)) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Discount can't be empty"; retVal = false; }
        //        //if (!(row.ProviderCode != null && row.ProviderCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Provider Code Code can't be empty"; retVal = false; }
        //        if (!(row.SerialNo != null && row.SerialNo.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Serial No  can't be empty"; retVal = false; }
        //        if (!(row.Make != null && row.Make.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Make Code can't be empty"; retVal = false; }
        //        if (!(row.Model != null && row.Model.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Model Code can't be empty"; retVal = false; }
        //        if (!(row.Category != null && row.Category.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Category Code can't be empty"; retVal = false; }
        //        if (!(row.ItemStatus != null && row.ItemStatus.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Item Status Code can't be empty"; retVal = false; }
        //        //if (!(row.CylinderCountCode != null && row.CylinderCountCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Cylinder Count Code can't be empty"; retVal = false; }
        //        //if (!(row.BodyTypeCode != null && row.BodyTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Body Type Code can't be empty"; retVal = false; }
        //        //if (!(row.FuelTypeCode != null && row.FuelTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Fuel Type Code can't be empty"; retVal = false; }
        //        //if (!(row.AspirationCode != null && row.AspirationCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Aspiration Code can't be empty"; retVal = false; }
        //        //if (!(row.TransmissionCode != null && row.TransmissionCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Transmission Code can't be empty"; retVal = false; }
        //        if (!(row.ItemPurchasedDate != null && row.ItemPurchasedDate.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Item Purchased Date can't be empty"; retVal = false; }
        //        //if (!(row.EngineCapacityCode != null && row.EngineCapacityCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Engine Capacity Code  can't be empty"; retVal = false; }
        //        //if (!(row.DriveTypeCode != null && row.DriveTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Drive Type Code can't be empty"; retVal = false; }
        //        if (!(row.CommodityUsageTypeCode != null && row.CommodityUsageTypeCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Commodity Usage Type Code can't be empty"; retVal = false; }
        //        //if (!(row.VariantCode != null && row.VariantCode.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Variant Code can't be empty"; retVal = false; }
        //        //if (!(row.PlateNo != null && row.PlateNo.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Plate No can't be empty"; retVal = false; }
        //        if (!(row.ModelYear != null && row.ModelYear.trim() != "")) { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Model Year can't be empty"; retVal = false; }
        //        if (!(row.Premium != null)) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium can't be empty"; retVal = false;
        //        }
        //        else if (row.Premium == 0) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Premium  can't be zero"; retVal = false;
        //        }
        //        if (!(row.VehiclePrice != null)) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Price can't be empty"; retVal = false;
        //        }
        //        else if (row.VehiclePrice == 0) {
        //            validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "Price  can't be zero"; retVal = false;
        //        }
        //        if (!(row.IsSpecialDeal != null && row.IsSpecialDeal != "")) {
        //            { validationMessage = validationMessage + (validationMessage == "" ? "" : ",") + "IsSpecialDeal can't be empty"; retVal = false; }
        //        }
        //    }
        //    if (!retVal) { row.ValidationError = validationMessage; row.Colour = "red"; }

        //    return retVal;
        //}

        function SetValues() {
            var retVal = true;
            $scope.errorTab1 = "";
            $scope.DataList = [];
            $scope.mySelectedRows = $scope.gridApi.selection.getSelectedRows();
            if ($scope.mySelectedRows.length != 0) {
                angular.forEach($scope.mySelectedRows, function (value) {
                    //  if (validate(value)) {
                    $scope.DataList.push(value);
                    value.AutoApproval = "No";
                    //  }
                });
            }
            else {
                retVal = false;
                $scope.errorTab1 = "Please Select at least one row to upload";
            }
            angular.forEach($scope.List, function (value) {

                value.AutoApproval = "No";
            });
            return retVal;
        }

        function clear() {
            $scope.errorTab1 = "";
            $scope.DataList = [];
            $scope.List = [];
            angular.element("input[type='file']").val(null);
            uploader1.queue = [];
            $scope.HeaderData.StartDate = "";
            $scope.HeaderData.EndDate = "";
        }

        function validateUpload() {
            var retVal = true;
            $scope.errorTab1 = "";
            if ($scope.CommodityType != "") {
                var a = $scope.CommodityType;//JSON.parse($scope.CommodityType);
                $scope.HeaderData.CommodityTypeId = a.CommodityTypeId;
                $scope.HeaderData.CommodityCode = a.CommodityCode;
            }

            if ($scope.Product != "") {
                var a = $scope.Product;//JSON.parse($scope.CommodityType);
                $scope.HeaderData.ProductId = a.Id;
                $scope.HeaderData.Productcode = a.Productcode;
            }

            if ($scope.HeaderData.CommodityTypeId == undefined || $scope.HeaderData.CommodityTypeId == '' || $scope.HeaderData.CommodityTypeId == "00000000-0000-0000-0000-000000000000") {
                retVal = false;
                $scope.errorTab1 = "Please select commodity type";
            }
            else {
                if (retVal && ($scope.HeaderData.StartDate == "" || $scope.HeaderData.EndDate == "")) {
                    retVal = false;
                    $scope.errorTab1 = "Please select start date and end date";
                }
                else { retVal = $scope.checkDate($scope.HeaderData.StartDate, $scope.HeaderData.EndDate); }
            }

            if ($scope.CommodityType.CommodityCode == "B") {
                if ($scope.HeaderData.ProductId == undefined || $scope.HeaderData.ProductId == '' || $scope.HeaderData.ProductId == "00000000-0000-0000-0000-000000000000") {
                    retVal = false;
                    $scope.errorTab1 = "Please select product";
                }
            }

            if (retVal && uploader1.queue.length == 0) {
                retVal = false;
                $scope.errorTab1 = "Please select file to upload";
            }
            return retVal;
        }

        $scope.Upload = function () {
            if (validateUpload()) {
                var length = uploader1.queue.length;
                if (length > 0) {
                    uploader1.queue[length - 1].formData = [];
                    uploader1.queue[length - 1].formData.push("HeaderData", $scope.HeaderData);
                    uploader1.queue[length - 1].upload();
                }
                else {
                    $scope.Path = "";
                    $http({
                        method: 'POST',
                        url: '/TAS.Web/api/TempBulkUpload/ConvertData',
                        headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                    }).success(function (data, status, headers, config) {
                        $scope.List = data;
                    }).error(function (data, status, headers, config) {
                    });
                }
            }
        }

        $scope.Save = function () {
            if (SetValues()) {
                swal({ title: "TAS Information", text: "saving data. Please wait ...", showConfirmButton: false });
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TempBulkUpload/SaveBulkUpload',
                    data: { "BulkData": $scope.DataList },
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    swal.close();
                    if (data == "Error !") {
                        customErrorMessage(data);
                    } else {
                        if (data == null || data.length == 0) {
                            customInfoMessage("Successfully Saved!");
                            clear();
                        }
                        else {
                            $scope.List = data;
                            customErrorMessage("No valid data to save");
                            //SweetAlert.swal({
                            //    title: "Policy Bulk Upload",
                            //    text: "No valid data to save",
                            //    type: "warning",
                            //    confirmButtonColor: "rgb(221, 107, 85)"
                            //});
                        }
                    }

                    //if (data == null || data.length == 0) {
                    //    //SweetAlert.swal({
                    //    //    title: "Policy Bulk Upload",
                    //    //    text: "Successfully Saved!",
                    //    //    confirmButtonColor: "#007AFF"
                    //    //});
                    //    customInfoMessage("Successfully Saved!");
                    //    clear();
                    //}
                    //else {
                    //    if (data == "Error !") {
                    //        //SweetAlert.swal({
                    //        //    title: "Policy Bulk Upload",
                    //        //    text: data,
                    //        //    type: "warning",
                    //        //    confirmButtonColor: "rgb(221, 107, 85)"
                    //        //});
                    //        customErrorMessage(data);
                    //    }
                    //    else {
                    //        $scope.List = data;
                    //       // customErrorMessage("No valid data to save");
                    //        //SweetAlert.swal({
                    //        //    title: "Policy Bulk Upload",
                    //        //    text: "No valid data to save",
                    //        //    type: "warning",
                    //        //    confirmButtonColor: "rgb(221, 107, 85)"
                    //        //});
                    //    }
                    //}
                }).error(function (data, status, headers, config) {
                    swal.close();
                    //SweetAlert.swal({
                    //    title: "Policy Bulk Upload",
                    //    text: "Save Failed !",
                    //    type: "warning",
                    //    confirmButtonColor: "rgb(221, 107, 85)"
                    //});
                    customErrorMessage("Save Failed !");
                });

            }
        }
        var uploader1 = $scope.uploader1 = new FileUploader({
            url: window.location.protocol + '//' + window.location.host + '/TAS.Web/api/TempBulkUpload/Upload',
            headers: { 'Authorization': $localStorage.jwt == undefined ? jwtt : $localStorage.jwt }
        });

        uploader1.filters.push({
            name: 'extensionFilter',
            fn: function (item, options) {
                var filename = item.name;
                var extension = filename.substring(filename.lastIndexOf('.') + 1).toLowerCase();
                if (extension == "xlx" || extension == "xlsx")
                    return true;
                else {
                    customErrorMessage('Invalid file format. Please select a Excel file with xlx or xlsx format and try again.');
                    return false;
                }
            }
        });
        uploader1.filters.push({
            name: 'sizeFilter',
            fn: function (item, options) {
                var fileSize = item.size;
                fileSize = parseInt(fileSize) / (1024 * 1024);
                if (fileSize <= 5)
                    return true;
                else {
                    customErrorMessage('Selected file exceeds the 5MB file size limit. Please choose a new file and try again.');
                    return false;
                }
            }
        });
        uploader1.filters.push({
            name: 'itemResetFilter',
            fn: function (item, options) {
                if (this.queue.length < 2)
                    return true;
                else {
                    customErrorMessage('You have exceeded the limit of uploading files.');
                    return false;
                }
            }
        });
        uploader1.onWhenAddingFileFailed = function (item, filter, options) {
            console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader1.onAfterAddingFile = function (fileItem) {
        };
        uploader1.onSuccessItem = function (fileItem, response, status, headers) {
            swal.close();
            if (response == "Ok") {
                angular.element("input[type='file']").val(null);
                uploader1.queue = [];
                $http({
                    method: 'POST',
                    url: '/TAS.Web/api/TempBulkUpload/ConvertData',
                    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                }).success(function (data, status, headers, config) {
                    $scope.List = data;
                }).error(function (data, status, headers, config) {
                });
            }
            else {
                customErrorMessage(response);
                //SweetAlert.swal({
                //    title: "Policy Bulk Upload",
                //    text: response,
                //    type: "warning",
                //    confirmButtonColor: "rgb(221, 107, 85)"
                //});
                //customErrorMessage(response);
            }
        };
        uploader1.onErrorItem = function (fileItem, response, status, headers) {
            customErrorMessage('We were unable to upload your file. Please try again.');
        };
        uploader1.onCancelItem = function (fileItem, response, status, headers) {
        };
        uploader1.onAfterAddingAll = function (addedFileItems) {
            console.info('onAfterAddingAll', addedFileItems);
        };
        uploader1.onBeforeUploadItem = function (item) {
            swal({ title: "TAS Information", text: "Uploading ..", showConfirmButton: false });
            console.info('onBeforeUploadItem', item);
        };
        uploader1.onProgressItem = function (fileItem, progress) {
            console.info('onProgressItem', fileItem, progress);
        };
        uploader1.onProgressAll = function (progress) {
            console.info('onProgressAll', progress);
        };
        uploader1.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader1.onCompleteAll = function () {
            console.info('onCompleteAll');
            //$http({
            //    method: 'POST',
            //    url: '/TAS.Web/api/TempBulkUpload/ConvertData',
            //    headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
            //}).success(function (data, status, headers, config) {
            //    $scope.List = data;

            //}).error(function (data, status, headers, config) {
            //}).finally(function () {
            //    swal.close();
            //});
        };

        $scope.downloadAttachment = function () {
            swal({ title: 'Processing...!', text: 'Preparing your download...', showConfirmButton: false });
            $http({
                method: 'POST',
                url: '/TAS.Web/api/TempBulkUpload/GetBulkUploadTemplate',
                data: { "commodityCode": $scope.CommodityType.CommodityCode },
                headers: { 'Authorization': $localStorage.jwt == undefined ? jwt : $localStorage.jwt }
                , responseType: 'arraybuffer',
            }).success(function (data, status, headers, config) {
                if (data.length != 0) {
                    try {
                        var octetStreamMime = 'application/octet-stream';
                        var success = false;

                        // Get the headers
                        headers = headers();

                        var fileName = "";
                        switch ($scope.CommodityType.CommodityCode) {
                            case "A": fileName = "BulkUploadAutomobile.xlsx"; break;
                            case "B": fileName = "BulkUploadBank.xlsx"; break;
                            case "E": fileName = "BulkUploadElectronic.xlsx"; break;
                        }

                        // Get the filename from the x-filename header or default to "download.bin"
                        var filename = headers['x-filename'] || fileName;

                        // Determine the content type from the header or default to "application/octet-stream"
                        var contentType = headers['content-type'] || octetStreamMime;

                        try {
                            // Try using msSaveBlob if supported
                            console.log("Trying saveBlob method ...");
                            var blob = new Blob([data], { type: contentType });
                            if (navigator.msSaveBlob)
                                navigator.msSaveBlob(blob, filename);
                            else {
                                // Try using other saveBlob implementations, if available
                                var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                                if (saveBlob === undefined) throw "Not supported";
                                saveBlob(blob, filename);
                            }
                            console.log("saveBlob succeeded");
                            success = true;
                        } catch (ex) {
                            console.log("saveBlob method failed with the following exception:");
                            console.log(ex);
                        }

                        if (!success) {
                            // Get the blob url creator
                            var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
                            if (urlCreator) {
                                // Try to use a download link
                                var link = document.createElement('a');
                                if ('download' in link) {
                                    // Try to simulate a click
                                    try {
                                        // Prepare a blob URL
                                        console.log("Trying download link method with simulated click ...");
                                        var blob = new Blob([data], { type: contentType });
                                        var url = urlCreator.createObjectURL(blob);
                                        link.setAttribute('href', url);

                                        // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                                        link.setAttribute("download", filename);

                                        // Simulate clicking the download link
                                        var event = document.createEvent('MouseEvents');
                                        event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                                        link.dispatchEvent(event);
                                        console.log("Download link method with simulated click succeeded");
                                        success = true;

                                    } catch (ex) {
                                        console.log("Download link method with simulated click failed with the following exception:");
                                        console.log(ex);
                                    }
                                }

                                if (!success) {
                                    // Fallback to window.location method
                                    try {
                                        // Prepare a blob URL
                                        // Use application/octet-stream when using window.location to force download
                                        console.log("Trying download link method with window.location ...");
                                        var blob = new Blob([data], { type: octetStreamMime });
                                        var url = urlCreator.createObjectURL(blob);
                                        window.location = url;
                                        console.log("Download link method with window.location succeeded");
                                        success = true;
                                    } catch (ex) {
                                        console.log("Download link method with window.location failed with the following exception:");
                                        console.log(ex);
                                    }
                                }

                            }
                        }

                        if (!success) {
                            console.log("No methods worked for saving the arraybuffer, using last resort window.open");
                            window.open(httpPath, '_blank', '');
                        }
                    }
                    catch (ex) {

                    }
                }
                else {
                    swal.close();
                    customErrorMessage('No File to download');
                }
            }).error(function (data, status, headers, config) {
            }).finally(function () {
                swal.close();
            });

        }

    })

//[
//                    { field: "TempBulkUploadId", displayName: "", width: 0, visible: false },
//                    { field: "FirstName", displayName: "First Name", width: 150 },
//                    { field: "LastName", displayName: "Last Name", width: 150 },
//                    { field: "Title", displayName: "Title", width: 50 },
//                    { field: "Occupation", displayName: "Occupation", width: 100 },
//                    { field: "MaritalStatus", displayName: "Marital Status", width: 60 },
//                    { field: "Country", displayName: "Country", width: 100 },
//                    { field: "City", displayName: "City", width: 100 },
//                    { field: "Nationality", displayName: "Nationality", width: 100 },
//                    { field: "PostalCode", displayName: "Postal Code", width: 100 },
//                    { field: "Email", displayName: "Email Address", width: 100 },
//                    { field: "MobilePhone", displayName: "Mobile Phone", width: 100 },
//                    { field: "OtherPhone", displayName: "Other Phone", width: 100 },
//                    {
//                        field: "DateOfBirth", displayName: "Date Of Birth", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
//                        , cellTemplate: '<div><input ng-model="row.entity.DateOfBirth" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
//                    },
//                    { field: "Gender", displayName: "Gender", width: 50 },
//                    { field: "CustomerType", displayName: "Customer Type", width: 100 },
//                    { field: "UsageType", displayName: "Usage Type", width: 100 },
//                    { field: "Address1", displayName: "Address 1", width: 100 },
//                    { field: "Address2", displayName: "Address 2", width: 100 },
//                    { field: "Address3", displayName: "Address 3", width: 100 },
//                    { field: "Address4", displayName: "Address 4", width: 100 },
//                    { field: "IDType", displayName: "ID Type", width: 100 },
//                    { field: "IDNo", displayName: "ID No", width: 100 },
//                    {
//                        field: "DrivingIssueDate", displayName: "Driving Issue Date", width: 100, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
//                        , cellTemplate: '<div><input ng-model="row.entity.DrivingIssueDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
//                    },
//                    { field: "BusinessName", displayName: "Business Name", width: 150 },
//                    { field: "BusinessTelephoneNo", displayName: "Business Telephone No", width: 150 },
//                    { field: "BusinessAddress1", displayName: "Business Address 1", width: 150 },
//                    { field: "BusinessAddress2", displayName: "Business Address 2", width: 150 },
//                    { field: "BusinessAddress3", displayName: "Business Address 3", width: 150 },
//                    { field: "BusinessAddress4", displayName: "Business Address 4", width: 150 },
//                    //{ field: "Password", displayName: "Password", width: 150 },
//                    //{ field: "ConfirmPassword", displayName: "Confirm Password", width: 150 }

//                    //----------------------------------------------------------------------------------

//                    { field: "CommodityTypeCode", displayName: "CommodityTypeCode", width: 150 },
//                    { field: "ProductCode", displayName: "ProductCode", width: 150 },
//                    { field: "DealerCode", displayName: "DealerCode", width: 150 },
//                    { field: "DealerLocationCode", displayName: "DealerLocationCode", width: 150 },
//                    { field: "ContractCode", displayName: "ContractCode", width: 150 },
//                    { field: "ExtensionTypeCode", displayName: "ExtensionTypeCode", width: 150 },
//                    { field: "CoverTypeCode", displayName: "CoverTypeCode", width: 150 },
//                    { field: "SalesPersonCode", displayName: "SalesPersonCode", width: 150 },
//                    { field: "KmAtPolicySale", displayName: "KmAtPolicySale", width: 150 },
//                    { field: "HrsAtPolicySale", displayName: "HrsAtPolicySale", width: 150 },
//                    { field: "Comment", displayName: "Comment", width: 150 },
//                    { field: "Premium", displayName: "Premium", width: 150 },
//                    {
//                        field: "PolicySoldDate", displayName: "PolicySoldDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
//                        , cellTemplate: '<div><input ng-model="row.entity.PolicySoldDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
//                    },
//                    {
//                        field: "PolicyStartDate", displayName: "PolicyStartDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
//                        , cellTemplate: '<div><input ng-model="row.entity.PolicyStartDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
//                    },
//                    {
//                        field: "PolicyEndDate", displayName: "PolicyEndDate", width: 150, type: 'date', cellFilter: 'date:\'dd-MM-yyyy\''
//                        , cellTemplate: '<div><input ng-model="row.entity.PolicyEndDate" ng-change="grid.appScope.addModifyFlag(row.entity)" ng-click="opened = true;" datepicker-popup="dd-MM-yyyy" is-open="opened" datepicker-options="grid.appScope.dateOptions" datepicker-append-to-body="true" type="text" /></div>'
//                    },
//                    { field: "Discount", displayName: "Discount", width: 150 },
//                    { field: "ProviderCode", displayName: "ProviderCode", width: 150 },

//                    //----------------------------------------------------------------------------------

//                    { field: "SerialNo", displayName: "SerialNo", width: 150 },
//                    { field: "VINNo", displayName: "VINNo", width: 150 },
//                    { field: "MakeCode", displayName: "MakeCode", width: 150 },
//                    { field: "ModelCode", displayName: "ModelCode", width: 150 },
//                    { field: "CategoryCode", displayName: "CategoryCode", width: 150 },
//                    { field: "ItemStatusCode", displayName: "ItemStatusCode", width: 150 },
//                    { field: "CylinderCountCode", displayName: "CylinderCountCode", width: 150 },
//                    { field: "BodyTypeCode", displayName: "BodyTypeCode", width: 150 },
//                    { field: "FuelTypeCode", displayName: "FuelTypeCode", width: 150 },
//                    { field: "AspirationCode", displayName: "AspirationCode", width: 150 },
//                    { field: "TransmissionCode", displayName: "TransmissionCode", width: 150 },
//                    { field: "ItemPurchasedDate", displayName: "ItemPurchasedDate", width: 150 },
//                    { field: "EngineCapacityCode", displayName: "EngineCapacityCode", width: 150 },
//                    { field: "DriveTypeCode", displayName: "DriveTypeCode", width: 150 },
//                    { field: "CommodityUsageTypeCode", displayName: "CommodityUsageTypeCode", width: 150 },
//                    { field: "VariantCode", displayName: "VariantCode", width: 150 },
//                    { field: "PlateNo", displayName: "PlateNo", width: 150 },
//                    { field: "ModelYear", displayName: "ModelYear", width: 150 },
//                    { field: "VehiclePrice", displayName: "VehiclePrice", width: 150 },
//                    { field: "ValidationError", displayName: "Error Message", width: 400, enableCellEdit: false }

//                    //----------------------------------------------------------------------------------

//                    /*

//                    { field: "CommodityTypeCode", displayName: "CommodityTypeCode", width: 150 },
//                    { field: "ProductCode", displayName: "ProductCode", width: 150 },
//                    { field: "DealerCode", displayName: "DealerCode", width: 150 },
//                    { field: "DealerLocationCode", displayName: "DealerLocationCode", width: 150 },
//                    { field: "ContractCode", displayName: "ContractCode", width: 150 },
//                    { field: "ExtensionTypeCode", displayName: "ExtensionTypeCode", width: 150 },
//                    { field: "PremiumCurrencyTypeCode", displayName: "PremiumCurrencyTypeCode", width: 150 },
//                    { field: "CoverTypeCode", displayName: "CoverTypeCode", width: 150 },
//                    { field: "SalesPersonCode", displayName: "SalesPersonCode", width: 150 },
//                    { field: "DealerPaymentCurrencyTypeCode", displayName: "DealerPaymentCurrencyTypeCode", width: 150 },
//                    { field: "CustomerPaymentCurrencyTypeCode", displayName: "CustomerPaymentCurrencyTypeCode", width: 150 },
//                    { field: "PaymentModeCode", displayName: "PaymentModeCode", width: 150 },
//                    { field: "PaymentTypeCode", displayName: "PaymentTypeCode", width: 150 },
//                    { field: "CustomerCode", displayName: "CustomerCode", width: 150 },
//                    { field: "TPABranchCode", displayName: "TPABranchCode", width: 150 },
//                    { field: "BordxCode", displayName: "BordxCode", width: 150 },
//                    { field: "BordxCountryCode", displayName: "BordxCountryCode", width: 150 },
//                    { field: "HrsUsedAtPolicySale", displayName: "HrsUsedAtPolicySale", width: 150 },
//                    { field: "PolicyNo", displayName: "PolicyNo", width: 150 },
//                    { field: "RefNo", displayName: "RefNo", width: 150 },
//                    { field: "Comment", displayName: "Comment", width: 150 },
//                    { field: "Premium", displayName: "Premium", width: 150 },
//                    { field: "DealerPayment", displayName: "DealerPayment", width: 150 },
//                    { field: "CustomerPayment", displayName: "CustomerPayment", width: 150 },
//                    { field: "IsPreWarrantyCheck", displayName: "IsPreWarrantyCheck", width: 150 },
//                    { field: "IsSpecialDeal", displayName: "IsSpecialDeal", width: 150 },
//                    { field: "IsPartialPayment", displayName: "IsPartialPayment", width: 150 },
//                    { field: "EntryDateTime", displayName: "EntryDateTime", width: 150 },
//                    { field: "EntryUser", displayName: "EntryUser", width: 150 },
//                    { field: "PolicySoldDate", displayName: "PolicySoldDate", width: 150 },
//                    { field: "IsApproved", displayName: "IsApproved", width: 150 },
//                    { field: "IsPolicyCanceled", displayName: "IsPolicyCanceled", width: 150 },
//                    { field: "IsPolicyRenewed", displayName: "IsPolicyRenewed", width: 150 },
//                    { field: "PolicyStartDate", displayName: "PolicyStartDate", width: 150 },
//                    { field: "PolicyEndDate", displayName: "PolicyEndDate", width: 150 },
//                    { field: "Discount", displayName: "Discount", width: 150 },
//                    { field: "TransferFee", displayName: "TransferFee", width: 150 },
//                    { field: "Year", displayName: "Year", width: 150 },
//                    { field: "Month", displayName: "Month", width: 150 },
//                    { field: "BordxNumber", displayName: "BordxNumber", width: 150 },
//                    { field: "ForwardComment", displayName: "ForwardComment", width: 150 },
//                    { field: "DealerPolicy", displayName: "DealerPolicy", width: 150 },
//                    { field: "PolicyApprovedBy", displayName: "PolicyApprovedBy", width: 150 },
//                    { field: "PaymentMethodFee", displayName: "PaymentMethodFee", width: 150 },
//                    { field: "PaymentMethodFeePercentage", displayName: "PaymentMethodFeePercentage", width: 150 },
//                    { field: "GrossPremiumBeforeTax", displayName: "GrossPremiumBeforeTax", width: 150 },
//                    { field: "NRP", displayName: "NRP", width: 150 },
//                    { field: "TotalTax", displayName: "TotalTax", width: 150 },
//                    { field: "EligibilityFee", displayName: "EligibilityFee", width: 150 },

//                    //----------------------------------------------------------------------------------------

//                    { field: "VINNo", displayName: "VINNo", width: 150 },
//                    { field: "MakeCode", displayName: "MakeCode", width: 150 },
//                    { field: "ModelCode", displayName: "ModelCode", width: 150 },
//                    { field: "CategoryCode", displayName: "CategoryCode", width: 150 },
//                    { field: "ItemStatusCode", displayName: "ItemStatusCode", width: 150 },
//                    { field: "CylinderCountCode", displayName: "CylinderCountCode", width: 150 },
//                    { field: "BodyTypeCode", displayName: "BodyTypeCode", width: 150 },
//                    { field: "FuelTypeCode", displayName: "FuelTypeCode", width: 150 },
//                    { field: "AspirationCode", displayName: "AspirationCode", width: 150 },
//                    { field: "TransmissionCode", displayName: "TransmissionCode", width: 150 },
//                    { field: "ItemPurchasedDate", displayName: "ItemPurchasedDate", width: 150 },
//                    { field: "EngineCapacityCode", displayName: "EngineCapacityCode", width: 150 },
//                    { field: "DriveTypeCode", displayName: "DriveTypeCode", width: 150 },
//                    { field: "CommodityUsageTypeCode", displayName: "CommodityUsageTypeCode", width: 150 },
//                    { field: "DealerCurrencyCode", displayName: "DealerCurrencyCode", width: 150 },
//                    { field: "CountryCode", displayName: "CountryCode", width: 150 },
//                    { field: "VariantCode", displayName: "VariantCode", width: 150 },
//                    { field: "DealerPrice", displayName: "DealerPrice", width: 150 },
//                    { field: "PlateNo", displayName: "PlateNo", width: 150 },
//                    { field: "ModelYear", displayName: "ModelYear", width: 150 },
//                    { field: "VehiclePrice", displayName: "VehiclePrice", width: 150 }

//                    //----------------------------------------------------------------------------------------

//                    */
//]

//$scope.UploadData = {
//    NationalityId: '',
//    CountryId: '',
//    MobileNo: '',
//    OtherTelNo: '',
//    FirstName: '',
//    LastName: '',
//    DateOfBirth: '',
//    Email: '',
//    CustomerTypeId: '',
//    UsageTypeId: '',
//    CityId: '',
//    Address1: '',
//    Address2: '',
//    Address3: '',
//    Address4: '',
//    IDNo: '',
//    IDTypeId: '',
//    DLIssueDate: '',
//    Gender: '',
//    BusinessName: '',
//    BusinessAddress1: '',
//    BusinessAddress2: '',
//    BusinessAddress3: '',
//    BusinessAddress4: '',
//    BusinessTelNo: '',
//    TitleId: '',
//    OccupationId: '',
//    MaritalStatusId: '',
//    MobileNo: '',
//    OtherTelNo: '',
//    PostalCode: '',

//    //-----------------------------

//    CommodityTypeCode: '',
//    ProductCode: '',
//    DealerCode: '',
//    DealerLocationCode: '',
//    ContractCode: '',
//    ExtensionTypeCode: '',
//    PremiumCurrencyTypeCode: '',
//    CoverTypeCode: '',
//    SalesPersonCode: '',
//    DealerPaymentCurrencyTypeCode: '',
//    CustomerPaymentCurrencyTypeCode: '',
//    PaymentModeCode: '',
//    PaymentTypeCode: '',
//    CustomerCode: '',
//    TPABranchCode: '',
//    BordxCode: '',
//    BordxCountryCode: '',
//    HrsUsedAtPolicySale: '',
//    PolicyNo: '',
//    RefNo: '',
//    Comment: '',
//    Premium: '',
//    DealerPayment: '',
//    CustomerPayment: '',
//    IsPreWarrantyCheck: '',
//    IsSpecialDeal: '',
//    IsPartialPayment: '',
//    EntryDateTime: '',
//    EntryUser: '',
//    PolicySoldDate: '',
//    IsApproved: '',
//    IsPolicyCanceled: '',
//    IsPolicyRenewed: '',
//    PolicyStartDate: '',
//    PolicyEndDate: '',
//    Discount: '',
//    TransferFee: '',
//    Year: '',
//    Month: '',
//    BordxNumber: '',
//    ForwardComment: '',
//    DealerPolicy: '',
//    PolicyApprovedBy: '',
//    PaymentMethodFee: '',
//    PaymentMethodFeePercentage: '',
//    GrossPremiumBeforeTax: '',
//    NRP: '',
//    TotalTax: '',
//    EligibilityFee: '',


//    //-----------------------------

//    VINNo: '',
//    MakeCode: '',
//    ModelCode: '',
//    CategoryCode: '',
//    ItemStatusCode: '',
//    CylinderCountCode: '',
//    BodyTypeCode: '',
//    FuelTypeCode: '',
//    AspirationCode: '',
//    TransmissionCode: '',
//    ItemPurchasedDate: '',
//    EngineCapacityCode: '',
//    DriveTypeCode: '',
//    CommodityUsageTypeCode: '',
//    DealerCurrencyCode: '',
//    CountryCode: '',
//    VariantCode: '',
//    DealerPrice: '',
//    PlateNo: '',
//    ModelYear: '',
//    VehiclePrice: ''

//    //------------------------------------


//};