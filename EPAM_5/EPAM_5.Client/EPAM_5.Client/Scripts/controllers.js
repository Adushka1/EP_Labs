'use strict';
angular.module('smApp.controllers', ['ui.bootstrap', function () {
}])
.controller('bodyController', ['$scope', 'exDialog', '$location', function ($scope, exDialog, $location) {
    $scope.body = {};

    $scope.$on('$locationChangeStart', function (event, newUrl, oldUrl) {
        if (newUrl != oldUrl)
        {
            if ($scope.body.dirty) {
                if (window.confirm("Do you really want to discard data changes\nand leave the page?")) {
                    if (exDialog.hasOpenDialog()) {
                        exDialog.closeAll();
                    }
                    $scope.body.dirty = false;
                }
                else {
                    event.preventDefault();
                }                
            }
            else {
                if (exDialog.hasOpenDialog()) {
                    exDialog.closeAll();
                }
            }            
        }
    });

    window.onbeforeunload = function (event) {
        if ($scope.body.dirty) {
            return "The page will be rediracted to another site but there is unsaved data on this page.";
        }        
    };    
}])

.controller('productListController', ['$scope', '$timeout', '$location', 'ngTableParams', 'exDialog', 'localData', 'productStatusTypes', 'productList', 'deleteProducts', function ($scope, $timeout, $location, ngTableParams, exDialog, localData, productStatusTypes, productList, deleteProducts) {
    $scope.model = {};
    $scope.model.productList = {};
    $scope.search = {};
    $scope.model.productSearchTypes = localData.getProductSearchTypes();

    $scope.model.productStatusTypes = localData.getProductStautsTypes();
    var pageSizeList = localData.getPageSizeList();

    var pageSizeSelectedDef = 5;

    var pageIndex = 0;
    var pageSize = 0;
    var sortBy = "";
    var sortDirection = 0;

    var sorting = {};

    $scope.tableParams = undefined;

    var nonPagerCall = false;
    var bypassGetData = false;
    var reloadType = "";

    $scope.setDefaultSearchItems = function () {
        $scope.model.pSearchType = { selected: "0" };
        $scope.model.pStatusType = { selected: '0' };

        $scope.search.pSearchText = "";
        $scope.search.pPriceLow = "";
        $scope.search.pPriceHigh = "";
        $scope.search.pAvailableFrom = "";
        $scope.search.pAvailableTo = "";

        $scope.errorMessage = undefined;
        $scope.showProductList = false;
    }
    $scope.setDefaultSearchItems();

    var loadProductList = function () {
        pageIndex = 0;
        pageSize = pageSizeSelectedDef;

        nonPagerCall = true;

        if ($scope.tableParams != undefined) {
            if (reloadType == 'refresh') {
                pageIndex = $scope.tableParams.page() - 1;                
            }
            else if (reloadType == 'add') {               
                $scope.setDefaultSearchItems();
            }
            
            pageSize = $scope.tableParams.count();

            sorting = $scope.tableParams.sorting();

            bypassGetData = true;
            $scope.tableParams.count($scope.tableParams.count() + 1);
        }

        $scope.tableParams = new ngTableParams({
            page: pageIndex + 1,   
            count: pageSize,        
            sorting: sorting
        }, {
            defaultSort: 'asc',
            total: 0,
            countOptions: pageSizeList,
            countSelected: pageSize,
            getData: getDataForGrid
        });
    };

    var getDataForGrid = function ($defer, params) {
        if (!bypassGetData) {
            if (!nonPagerCall) {
                pageIndex = params.page() - 1;

                if (pageSize != params.count()) {
                    pageSize = params.count();
                    params.page(1);
                }
                sortBy = Object.getOwnPropertyNames(params.sorting())[0]
                if (sortBy != undefined && sortBy != "") {
                    if (sorting !== params.sorting()) {
                        sorting = params.sorting();
                        sortDirection = sorting[sortBy] == "asc" ? 0 : 1;
                        params.page(1);
                    }
                }
                else {
                    sortBy = "";
                    sortDirection = 0;
                }
            }
            else {
                nonPagerCall = false;
            }
            $scope.errorMessage = undefined;

            var filterJson = getFilterJson();
            if (filterJson.error != undefined && filterJson.error != "") {
                $scope.errorMessage = filterJson.error;
            }
            else {
                productList.post(filterJson.json, function (data) {
                    $scope.model.productList = data.Products;
                    $timeout(function () {
                        if (reloadType == "add") {
                            params.total($scope.model.productList.length);
                            params.settings().addNewLoad = true;
                        }
                        else {
                            params.total(data.TotalCount);

                            if (data.newPageIndex >= 0) {
                                bypassGetData = true;
                                params.page(data.newPageIndex + 1);
                                pageIndex = data.newPageIndex;
                            }                            
                            params.settings().addNewLoad = false;
                        }

                        if (pageIndex == 0) {
                            params.settings().startItemNumber = 1;
                        }
                        else {
                            params.settings().startItemNumber = pageIndex * params.settings().countSelected + 1;
                        }
                        params.settings().endItemNumber = params.settings().startItemNumber + data.Products.length - 1;

                        
                        $defer.resolve($scope.model.productList);
                        
                        $scope.checkboxes.items = [];
                        $scope.checkboxes.topChecked = false;
                        for (var i = 0; i < $scope.model.productList.length; i++) {
                            $scope.checkboxes.items[i] = false;
                        }
                        $scope.hasEditItemChecked = false;

                        $scope.showProductList = true;                        
                    }, 500);
                }, function (error) {
                    exDialog.openMessage($scope, "Error getting product list data.", "Error", "error");
                });
            }
        }
        else {
            bypassGetData = false;
        }
    };

    $scope.clickGo = function () {
        reloadType = "";
        loadProductList();
    };

    var getFilterJson = function () {
        var isValid = false;
        var filterJson = { json: "{", error: "" };

        if ($scope.newProductIds.length > 0) {
            filterJson.json += "\"NewProductIds\": " + JSON.stringify($scope.newProductIds) + ", "
            $scope.newProductIds = [];
        }
        else {
            if ($scope.model.pSearchType.selected != "0" && $scope.search.pSearchText != "") {
                filterJson.json += "\"ProductSearchFilter\": {" +
                   "\"ProductSearchField\": \"" + $scope.model.pSearchType.selected + "\"" +
                   ",\"ProductSearchText\": \"" + $scope.search.pSearchText + "\"" +
                   "}, "
            }
            if ($scope.search.pAvailableFrom != "" || $scope.search.pAvailableTo != "") {
                var dateFrom = getFormattedDate($scope.search.pAvailableFrom);
                if (dateFrom == "error") {
                    filterJson.error += "Invalid Available From date.\n";
                }
                var dateTo = getFormattedDate($scope.search.pAvailableTo);
                if (dateTo == "error") {
                    filterJson.error += "Invalid Available To date.\n";
                }
                if ($scope.search.pAvailableFrom != "" && $scope.search.pAvailableTo != "") {
                    if ($scope.search.pAvailableFrom > $scope.search.pAvailableTo) {
                        filterJson.error += "Available To date should be greater or equal to Available From date.\n";
                    }
                }
                filterJson.json += "\"DateSearchFilter\": {" +
                                   "\"SearchDateFrom\": \"" + dateFrom + "\"" +
                                   ",\"SearchDateTo\": \"" + dateTo + "\"" +
                                   "}, "
            }
            if ($scope.search.pPriceLow != "" || $scope.search.pPriceHigh != "") {
                var priceLow = $scope.search.pPriceLow;
                if (priceLow != "" && !isNumeric(priceLow)) {
                    filterJson.error += "Invalid Price Low value.\n";
                }
                var priceHigh = $scope.search.pPriceHigh;
                if (priceHigh != "" && !isNumeric(priceHigh)) {
                    filterJson.error += "Invalid Price High value.\n";
                }
                if (priceLow != "" && priceHigh != "") {
                    if (parseFloat(priceLow) > parseFloat(priceHigh)) {
                        filterJson.error += "Price High should be greater or equal to Price Low.\n";
                    }
                }
                filterJson.json += "\"PriceSearchFilter\": {" +
                                   "\"SearchPriceLow\": \"" + priceLow + "\"" +
                                   ",\"SearchPriceHigh\": \"" + priceHigh + "\"" +
                                   "}, "
            }
            if ($scope.model.pStatusType.selected != "0") {
                filterJson.json += "\"StatusCode\": " + $scope.model.pStatusType.selected + ","
            }
        }

        filterJson.json +=
            "\"PaginationRequest\": {" +
               "\"PageIndex\": " + pageIndex +
              ",\"PageSize\": " + pageSize +
              ",\"Sort\": {" +
                  "\"SortBy\": \"" + sortBy + "\"" +
                 ",\"SortDirection\": " + sortDirection +
              "}" +
           "}" +
        "}";
        return filterJson;
    };

    $scope.openFrom = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.openedFrom = true;
        $scope.openedTo = false;
    };
    $scope.openTo = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.openedTo = true;
        $scope.openedFrom = false;
    };
    $scope.dateOptions = {
        formatYear: 'yyyy',
        startingDay: 1,
        showWeeks: 'false'
    };
    $scope.format = 'MM/dd/yyyy';

    $scope.checkboxes = {
        'topChecked': false,        
        items: []
    };    
    var hasUnChecked = function () {
        var rtn = false;
        for (var i = 0; i < $scope.checkboxes.items.length; i++) {
            if (!$scope.checkboxes.items[i]) {
                rtn = true;
                break;
            }
        }
        return rtn;
    };
    $scope.topCheckboxChange = function () {        
        angular.forEach($scope.checkboxes.items, function (item, index) {
            $scope.checkboxes.items[index] = $scope.checkboxes.topChecked;
        });
        $scope.hasEditItemChecked = $scope.checkboxes.topChecked;
    };
    $scope.listCheckboxChange = function () {
        $scope.checkboxes.topChecked = !hasUnChecked();
        
        $scope.hasEditItemChecked = false;
        for (var i = 0; i < $scope.checkboxes.items.length; i++) {
            if ($scope.checkboxes.items[i]) {
                $scope.hasEditItemChecked = true;
                break;
            }
        }
    };
    
    $scope.deleteProducts = function () {
        var idsForDelete = [];
        angular.forEach($scope.checkboxes.items, function (item, index) {
            if (item == true) {
                idsForDelete.push($scope.model.productList[index].ProductID);
            }
        });
        if (idsForDelete.length > 0) {
            var temp = "s";
            var temp2 = "s have"
            if (idsForDelete.length == 1) {
                temp = "";
                temp2 = " has";
            }
            exDialog.openConfirm({
                scope: $scope,
                title: "Delete Confirmation",
                message: "Are you sure to delete selected product" + temp + "?"
            }).then(function (value) {
                deleteProducts.post(idsForDelete, function (data) {
                    exDialog.openMessage({
                        scope: $scope,
                        message: "The product" + temp2 + " successfully been deleted.",
                        closeAllDialogs: true
                    });
                    reloadType = "refresh";
                    loadProductList();
                }, function (error) {
                });
            });
        }
    };

    $scope.paging = {};
    $scope.newProductIds = [];

    $scope.paging.openProductForm = function (id) {
        $scope.productId = undefined;
        if (id != undefined) {
            $scope.productId = id;
        }
        exDialog.openPrime({
            scope: $scope,
            template: 'Pages/_product.html',
            controller: 'productController',
            width: '450px',
            beforeCloseCallback: refreshGrid,
            closeByXButton: false,
            closeByClickOutside: false,
            closeByEscKey: false
        });
    };

    var refreshGrid = function () {
        if ($scope.newProductIds.length > 0) {
            reloadType = "add";
            loadProductList();
        }
        else {
            reloadType = "refresh";
            loadProductList();
        }        
        return true;
    };
}])

.controller('productController', ['$scope', '$rootScope', '$timeout', 'exDialog', 'productObj', 'categories', 'productStatusTypes', 'addProduct', 'updateProduct', function ($scope, $rootScope, $timeout, exDialog, productObj, categories, productStatusTypes, addProduct, updateProduct) {
    $scope.model.Product = {};
    var maxAddPerLoad = 10;

    $scope.setDrag = function (flag) {
        $rootScope.noDrag = flag;
    }
    
    $scope.model.CategoryList = categories.query({}, function (data) {
        $scope.model.ProductStatusTypes = productStatusTypes.query({}, function (data) {
        }, function (error) {
            exDialog.openMessage($scope, "Error getting product status type data.", "Error", "error");
        });
    }, function (error) {
        exDialog.openMessage($scope, "Error getting category list data.", "Error", "error");
    });

    if ($scope.productId == undefined) {
        $scope.productDialogTitle = "Add Product";
        $scope.model.selCategory = { selected: 0 };
        $scope.model.selStatusType = { selected: 0 };
    }
    else {
        $scope.productDialogTitle = "Update Product";

        productObj.query({ id: $scope.productId }, function (data) {
            data.UnitPrice = parseFloat(data.UnitPrice.toFixed(2));     
            $scope.model.selCategory = { selected: data.CategoryID };
            $scope.model.selStatusType = { selected: data.StatusCode };
            var avDate = new Date(data.AvailableSince);
            data.AvailableSince = getFormattedDate(avDate);
            $scope.model.Product = data;
        }, function (error) {
            exDialog.openMessage($scope, "Error getting product data.", "Error", "error");
        });
    }

    $scope.saveProduct = function (isValid) {
        if (!isValid) {
            exDialog.openMessage({
                scope: $scope,
                title: "Error",
                icon: "error",
                message: "Invalid data entry.",
                closeAllDialogs: true
            });
            return false;
        }

        $scope.model.Product.CategoryID = $scope.model.selCategory.selected;
        $scope.model.Product.StatusCode = $scope.model.selStatusType.selected;

        var title, message;
        if ($scope.model.Product.ProductID > 0) {
            title = "Update Confirmation";
            message = "Are you sure to update the product?";
        }
        else {
            title = "Add Confirmation";
            message = "Are you sure to add the product?";
        }
        exDialog.openConfirm({
            scope: $scope,
            title: title,
            message: message
        }).then(function (value) {
            if ($scope.model.Product.ProductID > 0) {
                updateProduct.post($scope.model.Product, function (data) {                    
                    $scope.productForm.$setPristine();

                    exDialog.openMessage({
                        scope: $scope,
                        message: "The product has successfully been updated.",
                        closeAllDialogs: true
                    });                    
                }, function (error) {
                    exDialog.openMessage($scope, "Error updating product data.", "Error", "error");
                });
            }
            else {
                addProduct.post($scope.model.Product, function (data) {
                    $scope.productForm.$setPristine();

                    $scope.newProductIds.push(data.ProductID);

                    if ($scope.newProductIds.length < maxAddPerLoad) {
                        exDialog.openConfirm({
                            scope: $scope,
                            message: "The new product has successfully been added. \n\nWould you like to add another?",
                            messageAddClass: 'ng-with-newlines'
                        }).then(function (value) {
                            clearAddForm();
                        }, function (reason) {
                            exDialog.closeAll();                            
                        });
                    }
                    else {
                        exDialog.openMessage({
                            scope: $scope,
                            message: "The new product has successfully been added. \n\nThis is the last new product that can be added in current data load operation.",
                            messageAddClass: 'ng-with-newlines',
                            closeAllDialogs: true
                        });
                    }
                }, function (error) {
                    exDialog.openMessage($scope, "Error adding product data.", "Error", "error");
                });
            }
        });
    };

    $scope.setVisited = function (baseElementName) {
        if ($scope.productForm[baseElementName]) {
            $scope.productForm[baseElementName]['$visited'] = true;
        }
    };

    var clearAddForm = function () {
        $scope.disableValidation = true;
        $scope.model.Product.ProducID = 0;
        $scope.model.Product.ProductName = '';
        $scope.model.selCategory = { selected: 0 };
        $scope.model.Product.UnitPrice = '';
        $scope.model.selStatusType = { selected: 0 };
        $scope.model.Product.AvailableSince = '';
        
        $scope.ddlCategoryDirty = false;
        $scope.ddlStatusDirty = false;
        $scope.productForm.txtProductName.$setValidity("required", true);
        $scope.productForm.txtUnitPrice.$setValidity("required", true);
        $scope.productForm.txtProductName.$visited = false;

        $timeout(function () {
            angular.element(document.querySelector('#txtProductName'))[0].focus();
        });
    };

    $scope.openDatePicker = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };
    $scope.dateOptions = {
        formatYear: 'yyyy',
        startingDay: 1,
        showWeeks: 'false'
    };
    $scope.format = 'MM/dd/yyyy';

    $scope.categoryChanged = function (selected) {
        if (selected != 0) 
            $scope.ddlCategoryDirty = true;
        else
            $scope.ddlCategoryDirty = false;
    };
    $scope.statusChanged = function (selected) {
        if (selected != 0)
            $scope.ddlStatusDirty = true;
        else
            $scope.ddlStatusDirty = false;
    };

    $scope.$watch("productForm.$dirty", function (newValue, oldValue) {
        if (newValue != oldValue) {            
            $scope.body.dirty = newValue;
        }        
    });

    $scope.cancelAddOrUpdate = function () {
        if ($scope.productForm.$dirty) {
            var temp = "adding";
            if ($scope.model.Product.ProductID > 0)
                temp = "updating";

            exDialog.openConfirm({
                scope: $scope,
                title: "Cancel Confirmation",
                message: "Are you sure to discard changes and cancel " + temp + " product?"
            }).then(function (value) {
                doCancel();
            }, function (leaveIt) {
            });
        }
        else {
            doCancel();
        }        
    };
    var doCancel = function () {
        $scope.productForm.$setPristine();
        $scope.body.dirty = false;
        $scope.closeThisDialog('close');
    };
}])

.controller('contactListController', ['$scope', '$timeout', '$location', 'ngTableParams', 'exDialog', 'contactList', 'localData', 'addContacts', 'updateContacts', 'deleteContacts', function ($scope, $timeout, $location, ngTableParams, exDialog, contactList, localData, addContacts, updateContacts, deleteContacts) {
    $scope.formLoaded = false;
    $scope.model = {};
    $scope.model.contactList = [];
    $scope.model.contactList_0 = [];
    $scope.tableParams = undefined;
    $scope.model.primaryTypes = localData.getPrimaryTypes();

    $scope.maxAddNumber = 5;     
    $scope.addRowCount = 0;
    $scope.editRowCount = 0;
       
    $scope.isEditDirty = false;
    $scope.isAddDirty = false;
    $scope.rowDisables = [];
    $scope.checkboxes = {
        'topChecked': false,
        'topDisabled': false,
        items: []
    };
    var bypassWatch = false;    
    var maxEditableIndex = 0;    
    var seqNumber = 0;
    var loadingCount = 0;

    $scope.model.displayCount = function () {
        return loadingCount + $scope.addRowCount;
    };

    $scope.validateAtBlur = function (invalid) {
        $scope.inputInvalid = invalid;
    };
    
    var loadContactList = function () {
        $scope.tableParams = new ngTableParams({
            page: 1, 
            count: 0
        }, {
            getData: getDataForGrid
        });
    };    

    var getDataForGrid = function ($defer, params) {
        $scope.errorMessage = undefined;
        
        contactList.query({}, function (data) {
            $timeout(function () {
                $scope.model.contactList = data.Contacts;

                maxEditableIndex = $scope.model.contactList.length - 1;

                for (var i = 0; i < $scope.model.contactList.length; i++) {
                    $scope.checkboxes.items[i] = false;
                    $scope.rowDisables[i] = false;
                }

                $scope.model.contactList_0 = angular.copy(data.Contacts);

                loadingCount = $scope.model.contactList.length;
                
                $scope.addRowCount = 0;
                $scope.editRowCount = 0;
                $scope.isAddDirty = false;
                $scope.isEditDirty = false;

                $defer.resolve($scope.model.contactList);

                $scope.showContactList = true;                    
            }, 500);
        }, function (error) {
            exDialog.openMessage($scope, "Error getting Contact list data.", "Error", "error");
        });                
    };

    loadContactList();

    $scope.contactTitle = {
        0: "No contact item",
        1: "Contact List (total 1 contact)",
        other: "Contact List (total {} contacts)"
    };

    var hasUnChecked = function () {
        var rtn = false;
        for (var i = 0; i <= maxEditableIndex; i++) {
            if (!$scope.checkboxes.items[i]) {
                rtn = true;
                break;
            }
        }
        return rtn;
    };

    $scope.topCheckboxChange = function () {
        if ($scope.checkboxes.topChecked) {
            for (var i = 0; i <= maxEditableIndex ; i++) {
                if (!$scope.checkboxes.items[i]) {
                    $scope.checkboxes.items[i] = true;
                }
            }
            $scope.editRowCount = $scope.checkboxes.items.length;
        } 
        else {
            if ($scope.addRowCount > 0 && $scope.editRowCount == 0) {                
                cancelAllAddRows("topCheckbox");
            }
            else if ($scope.addRowCount == 0 && $scope.editRowCount > 0) {
                cancelAllEditRows("topCheckbox");
            }                       
        }  
    };

    $scope.listCheckboxChange = function (listIndex) {        
        if ($scope.checkboxes.items[listIndex]) {
            $scope.editRowCount += 1;            
        }
        else {
            if (listIndex > maxEditableIndex) {
                if (dataChanged($scope.model.contactList[listIndex],
                                $scope.model.contactList_0[listIndex])) {                
                    exDialog.openConfirm({
                        scope: $scope,
                        title: "Cancel Confirmation",
                        message: "Are you sure to discard changes and remove this new row?"
                    }).then(function (value) {
                        cancelAddRow(listIndex);
                    }, function (forCancel) {
                        undoCancelRow(listIndex);
                    });
                }
                else {
                    cancelAddRow(listIndex);
                }
            }
            else {
                if (dataChanged($scope.model.contactList[listIndex],
                                $scope.model.contactList_0[listIndex])) {
                    exDialog.openConfirm({
                        scope: $scope,
                        title: "Cancel Confirmation",
                        message: "Are you sure to discard changes and cancel editing for this row?"
                    }).then(function (value) {
                        cancelEditRow(listIndex, true);
                    }, function (forCancel) {
                        undoCancelRow(listIndex);
                    });
                }
                else {                    
                    cancelEditRow(listIndex);
                }                
            }
        }        
        if ($scope.addRowCount > 0 && $scope.editRowCount == 0)        
            $scope.checkboxes.topChecked = true;
        else if ($scope.addRowCount == 0 && $scope.editRowCount > 0)
            $scope.checkboxes.topChecked = !hasUnChecked();
    };
    
    var cancelAddRow = function (listIndex) {
        if (listIndex == $scope.checkboxes.items.length - 1) {
            for (var i = listIndex; i > maxEditableIndex; i--) {
                $scope.model.contactList_0.splice(i, 1);
                $scope.model.contactList.splice(i, 1);
                $scope.checkboxes.items.splice(i, 1);

                if (i == maxEditableIndex + 1) {
                    $scope.addRowCount = 0;

                    seqNumber = 0;
                }
                else {
                    $scope.addRowCount -= 1;

                    if ($scope.model.contactList[i - 1] != undefined) {
                        break;
                    }
                }
            }
        }
        else {
            $scope.model.contactList_0[listIndex] = undefined;
            $scope.model.contactList[listIndex] = undefined;
            $scope.checkboxes.items[listIndex] = undefined;

            $scope.addRowCount -= 1;
        }        
    };

    var cancelAllAddRows = function (callFrom) {
        if ($scope.isAddDirty) {
            exDialog.openConfirm({
                scope: $scope,
                title: "Cancel Confirmation",
                message: "Are you sure to discard changes and cancel adding for all rows?"
            }).then(function (value) {
                if (callFrom == "topCheckbox") 
                    cancelAllAddRowsRun();
                else if (callFrom == "cancelButton")
                {
                    $scope.contactForm.$setPristine();
                    $scope.body.dirty = false;
                }   

                $scope.tableParams.count($scope.tableParams.count() + 1);

            }, function (forCancel) {
                if (callFrom == "topCheckbox")
                    $scope.checkboxes.topChecked = true;
            });
        }
        else {
            if (callFrom == "topCheckbox")
                cancelAllAddRowsRun();
            else if (callFrom == "cancelButton")
                $scope.tableParams.count($scope.tableParams.count() + 1);
        }
    }
    var cancelAllAddRowsRun = function () {
        for (var i = $scope.checkboxes.items.length - 1; i > maxEditableIndex; i--) {
            $scope.model.contactList_0.splice(i, 1);
            $scope.model.contactList.splice(i, 1);
            $scope.checkboxes.items.splice(i, 1);
        }
        $scope.addRowCount = 0;

        seqNumber = 0;

        $scope.contactForm.$setPristine();
        $scope.body.dirty = false;
    };

    var cancelEditRow = function (listIndex, copyBack) {
        if (copyBack) {
            $scope.model.contactList[listIndex] = angular.copy($scope.model.contactList_0[listIndex]);
        }
        $scope.editRowCount -= 1;
    };

    var cancelAllEditRows = function (callFrom) {
        if ($scope.isEditDirty) {
            var temp = "";
            if (callFrom == "topCheckbox") {
                temp = "all rows";
            }
            else if (callFrom == "cancelButton") {
                if ($scope.editRowCount == 1)
                    temp = "the selected row";
                else
                    temp = "selected rows";
            }

            exDialog.openConfirm({
                scope: $scope,
                title: "Cancel Confirmation",
                message: "Are you sure to discard changes and cancel editing for " + temp + "?"
            }).then(function (value) {
                for (var i = 0; i <= maxEditableIndex ; i++) {
                    if ($scope.checkboxes.items[i]) {
                        $scope.checkboxes.items[i] = false;

                        $scope.model.contactList[i] = angular.copy($scope.model.contactList_0[i]);
                    }
                }
                $scope.editRowCount = 0;

                $scope.checkboxes.topChecked = false;

                $scope.contactForm.$setPristine();
                $scope.body.dirty = false;

            }, function (forCancel) {
                if (callFrom == "topCheckbox") {
                    $scope.checkboxes.topChecked = true;
                }                
            });
        }
        else {
            for (var i = 0; i <= maxEditableIndex ; i++) {
                if ($scope.checkboxes.items[i]) {
                    $scope.checkboxes.items[i] = false;

                    $scope.editRowCount = 0;
                }
            }
            $scope.checkboxes.topChecked = false;
        }
    }

    var undoCancelRow = function (listIndex) {
        $scope.checkboxes.items[listIndex] = true;
        $scope.checkboxes.topChecked = !hasUnChecked();
    }; 
    
    $scope.setVisited = function (baseElementName, listIndex) {
        if (listIndex > maxEditableIndex) {
            $scope.contactForm[baseElementName + '_' + listIndex]['$visited'] = true;
        }
    };

    $scope.addNewContact = function () {        
        if ($scope.addRowCount + 1 == $scope.maxAddNumber) {
            exDialog.openMessage({
                scope: $scope,
                title: "Warning",
                icon: "warning",
                message: "The maximum number (" + $scope.maxAddNumber + ") of added rows for one submission is approached."
            });            
        }         
        bypassWatch = true;        

        var newContact = {
            ContactID: 0,
            ContactName: '',
            Phone: '',
            Email: '',
            PrimaryType: 0
        };
        $scope.model.contactList.push(newContact);

        $scope.model.contactList_0.push(angular.copy(newContact));        

        seqNumber += 1;
        $scope.checkboxes.items[maxEditableIndex + seqNumber] = true;

        $scope.addRowCount += 1;                
    };

    $scope.deleteContacts = function () {
        var idsForDelete = [];
        angular.forEach($scope.checkboxes.items, function (item, index) {
            if (item == true) {
                idsForDelete.push($scope.model.contactList[index].ContactID);
            }
        });
        if (idsForDelete.length > 0) {
            var temp = "s";
            var temp2 = "s have"
            if (idsForDelete.length == 1) {
                temp = "";
                temp2 = " has";
            }
            exDialog.openConfirm({
                scope: $scope,
                title: "Delete Confirmation",
                message: "Are you sure to delete selected contact" + temp + "?"
            }).then(function (value) {
                deleteContacts.post(idsForDelete, function (data) {
                    exDialog.openMessage({
                        scope: $scope,
                        message: "The " + temp2 + " successfully been deleted."
                    });
                    $scope.tableParams.count($scope.tableParams.count() + 1);

                }, function (forCancel) {
                    exDialog.openMessage($scope, "Error deleting contact data.", "Error", "error");
                });
            });
        }
    };

    $scope.SaveChanges = function () {        
        var title, message, temp, temp2;
        temp = "s";
        temp2 = "s have";
        if ($scope.addRowCount == 1 || $scope.editRowCount == 1) {
            temp = "";
            temp2 = " has"
        }

        if ($scope.isEditDirty) {            
            title = "Update Confirmation";
            message = "Are you sure to update selected contact" + temp + "?";
        }
        else if ($scope.isAddDirty) {
            title = "Add Confirmation";
            message = "Are you sure to add the contact" + temp + "?";
        }
        exDialog.openConfirm({
            scope: $scope,
            title: title,
            message: message
        }).then(function (value) {
            if ($scope.isEditDirty) {
                updateContacts.post($scope.model.contactList, function (data) {
                    $scope.contactForm.$setPristine();
                    $scope.body.dirty = false;

                    exDialog.openMessage($scope, "Selected contact" + temp2 + " successfully been updated.");
                    $scope.tableParams.count($scope.tableParams.count() + 1);

                }, function (error) {
                    exDialog.openMessage($scope, "Error updating contact data.", "Error", "error");
                });
            }
            else if ($scope.isAddDirty) {
                var activeAddItems = [];
                for (var i = maxEditableIndex + 1; i < $scope.model.contactList.length; i++) {
                    if ($scope.model.contactList[i] != undefined) {
                        activeAddItems.push($scope.model.contactList[i]);
                    }
                }
                
                addContacts.post(activeAddItems, function (data) {                
                    $scope.contactForm.$setPristine();
                    $scope.body.dirty = false;

                    exDialog.openMessage($scope, "The new contact" + temp2 + " successfully been added.");

                    $scope.tableParams.count($scope.tableParams.count() + 1);

                }, function (error) {
                    exDialog.openMessage($scope, "Error adding contact data.", "Error", "error");
                });
            }
        });
    };

    $scope.CanelChanges = function () {        
        if ($scope.isEditDirty || (!$scope.isEditDirty && $scope.editRowCount > 0)) {            
            cancelAllEditRows("cancelButton");            
        }
        else if ($scope.isAddDirty || (!$scope.isAddDirty && $scope.addRowCount > 0)) {            
            cancelAllAddRows("cancelButton");            
        }
    };

    $scope.$watch("addRowCount", function (newValue, oldValue) {        
        if (oldValue == 0 && newValue > 0) {
            disableEditRows(true);
            $scope.checkboxes.topChecked = true;
        }
        else if (oldValue > 0 && newValue == 0) {
            $scope.isAddDirty = false;            
            disableEditRows(false);
            $scope.checkboxes.topChecked = false;

            $scope.body.dirty = $scope.isAddDirty;
        }
    });

    $scope.$watch("model.contactList", function (newValue, oldValue) {
        if (bypassWatch) {
            bypassWatch = false;
        } 
        else {
            if (oldValue.length != undefined) {
                if ($scope.model.contactList.length - 1 > maxEditableIndex) {
                    if (dataChanged($scope.model.contactList[maxEditableIndex + 1],
                                    $scope.model.contactList_0[maxEditableIndex + 1])) {
                        $scope.isAddDirty = true;
                    }
                    else {
                        $scope.isAddDirty = false;
                    }
                    $scope.body.dirty = $scope.isAddDirty;
                }
                else {
                    if (dataChanged($scope.model.contactList, $scope.model.contactList_0)) {
                        $scope.isEditDirty = true;
                    }
                    else {
                        $scope.isEditDirty = false;
                    }
                    $scope.body.dirty = $scope.isEditDirty;
                }
            }
        }       
    }, true);
        
    var dataChanged = function (data_1, data_2) {
        var isChanged = false;
        if (angular.isArray(data_1)) {
            for (var idx = 0; idx < data_1.length; idx++) {
                for (var propName in data_1[idx]) {
                    if (propName != "$$hashKey") {
                        if (data_1[idx][propName] != data_2[idx][propName]) {
                            isChanged = true;
                            break;
                        }
                    }
                }
                if (isChanged) break;
            }
        }
        else {
            for (var propName in data_1) {
                if (propName != "$$hashKey") {
                    if (data_1[propName] != data_2[propName]) {
                        isChanged = true;
                        break;
                    }
                }
            }
        }        
        return isChanged;        
    };

    var disableEditRows = function (flag) {
        for (var i = 0; i <= maxEditableIndex; i++) {
            $scope.rowDisables[i] = flag;
        }
    };

    $scope.ddlPrimaryTypeDirty = [];
    $scope.primaryTypeChanged = function (index, selected) {
        if (selected != $scope.model.contactList_0[index].PrimaryType)
            $scope.ddlPrimaryTypeDirty[index] = true;
        else
            $scope.ddlPrimaryTypeDirty[index] = false;
    };      
}])

.controller('aboutController', ['$scope', function ($scope) {
    $scope.message = 'This is an example.';
}])
;
function getFormattedDate(date) {
    if (date == "") return "";
    try {
        var year = date.getFullYear();
        var month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }
    catch (err) {
        return "error";
    }
}
function isNumeric(value) {
    return !isNaN(parseFloat(value)) && isFinite(value);
}

