'use strict'

angular.module('smApp', ['ngRoute', 'smApp.controllers', 'smApp.AppServices', 'smApp.directives',  'ui.bootstrap', 'ngTable', 'ngExDialog', 'ajaxLoader', function () {
}])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
    .when('/list', {
        templateUrl: '/Pages/productList.html',
        controller: 'productListController'
    })
    .when('/list_cont', {
        templateUrl: '/Pages/contactList.html',
        controller: 'contactListController'
        })
    .when('/about', {
        templateUrl: '/Pages/about.html',
        controller: 'aboutController'
    })
    ;

    $routeProvider.otherwise({ redirectTo: '/list' });
}])
.config(['exDialogProvider', function (exDialogProvider) {
    exDialogProvider.setDefaults({
        template: 'ngExDialog/commonDialog.html',
        width: '330px',
    });
}]);
;

