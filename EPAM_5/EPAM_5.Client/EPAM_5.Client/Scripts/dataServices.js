'use strict';
var webApiBaseUrl = "http://localhost:10422/api/";

angular.module('smApp.AppServices', ['ngResource'])

//Web API call for categores.
.factory('categories', function ($resource) {
    return $resource(webApiBaseUrl + 'lookupcategories', {}, {
        query: { method: 'GET', isArray: true }
    });
})

//Web API call for product status types.
.factory('productStatusTypes', function ($resource) {
    return $resource(webApiBaseUrl + 'lookupproductstatustypes', {}, {
        query: { method: 'GET', isArray: true }
    });
})
//Web API call for product list.
.factory('productList', function ($resource) {
    return $resource(webApiBaseUrl + 'getproductlist_p', {}, {
        post: {
            method: 'POST', isArray: false,
            headers: { 'Content-Type': 'application/json' }
        }
    });
})
//Web API call for product object.
.factory('productObj', function ($resource) {
    return $resource(webApiBaseUrl + 'products/:id', {}, {
        query: { method: 'GET', isArray: false }
    });
})
//Web API call for adding product.
.factory('addProduct', function ($resource) {
    return $resource(webApiBaseUrl + 'addProduct', {}, {
        post: {
            method: 'POST', isArray: false,
            headers: { 'Content-Type': 'application/json' }
        }
    });
})
//Web API call for updating product.
.factory('updateProduct', function ($resource) {
    return $resource(webApiBaseUrl + 'updateproduct', {}, {
        post: {
            method: 'POST', isArray: false,
            headers: { 'Content-Type': 'application/json' }
        }
    });
})
//Web API call for deleting products.
.factory('deleteProducts', function ($resource) {
    return $resource(webApiBaseUrl + 'deleteProducts', {}, {
        post: {
            method: 'POST', isArray: false,
            headers: { 'Content-Type': 'application/json' }
        }
    });
});

angular.module('smApp.AppServices').service('localData', [function () {
    this.getProductSearchTypes = function () {
        return [
          { id: "0", name: "Please select..." },
          { id: "CategoryId", name: "Category ID" },
          { id: "CategoryName", name: "Category Name" },
          { id: "ProductId", name: "Product ID" },
          { id: "ProductName", name: "Product Name" }
        ];
    }
    this.getProductStautsTypes = function () {
        return [
          { StatusCode: 1, Description: "Available" },
          { StatusCode: 2, Description: "Out of Stock" },
          { StatusCode: 3, Description: "Back Ordered" },
          { StatusCode: 4, Description: "Discontinued" },
          { StatusCode: 5, Description: "Undefined" }
        ];
    }
    this.getPrimaryTypes = function () {
        return [
          { id: 0, name: "Please select..." },
          { id: 1, name: "Phone" },
          { id: 2, name: "Email" }
        ];
    }
    
    this.getPageSizeList = function () {
        return [
          { value: 5, text: "5" },
          { value: 10, text: "10" },
          { value: 25, text: "25" },
          { value: 50, text: "50" },
          { value: 100, text: "100" },
          { value: -1, text: "ALL" }
        ];
    }
}]);

