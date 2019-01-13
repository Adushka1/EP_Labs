'use strict'

angular.module('smApp.directives', function () {
})
.directive('optionsClass', function ($parse) {
    return {
        link: function (scope, elem, attrs) {
            if (elem[0].tagName == "SELECT")
            {
                var optionsSourceStr = attrs.ngOptions.split(' ').pop(),

                getOptionsClass = $parse(attrs.optionsClass);

                scope.$watch(optionsSourceStr, function (items) {
                    angular.forEach(items, function (item, index) {
                        var classes = getOptionsClass(item);

                        var option = elem.children()[index];

                        angular.forEach(classes, function (type, className) {
                            if ((type == "placeholder" && index == 0) ||
                                (type != "placeholder" && index > 0)) {
                                angular.element(option).addClass(className);
                            }
                        });
                    });
                });
            }
            else if (elem[0].tagName == "OPTION") {
                getOptionsClass = $parse(attrs.optionsClass);
                var classes = getOptionsClass();
                angular.forEach(classes, function (type, className) {
                    if ((type == "placeholder" && elem[0].parentElement.children.length == 1) ||
                        (type != "placeholder" && elem[0].parentElement.children.length > 1)) {
                        angular.element(elem).addClass(className);
                    }
                });
            } 
        }
    };
})
.directive('optionsClass', function ($parse) {
    return {
        link: function (scope, elem, attrs) {
            if (elem[0].tagName == "SELECT")
            {
                var optionsSourceStr = attrs.ngOptions.split(' ').pop(),

                getOptionsClass = $parse(attrs.optionsClass);

                scope.$watch(optionsSourceStr, function (items) {
                    angular.forEach(items, function (item, index) {
                        var classes = getOptionsClass(item);

                        var option = elem.children()[index];

                        angular.forEach(classes, function (type, className) {
                            if ((type == "placeholder" && index == 0) ||
                                (type != "placeholder" && index > 0)) {
                                angular.element(option).addClass(className);
                            }
                        });
                    });
                });
            }
            else if (elem[0].tagName == "OPTION") {
                getOptionsClass = $parse(attrs.optionsClass);
                var classes = getOptionsClass();
                angular.forEach(classes, function (type, className) {
                    if ((type == "placeholder" && elem[0].parentElement.children.length == 1) ||
                        (type != "placeholder" && elem[0].parentElement.children.length > 1)) {
                        angular.element(elem).addClass(className);
                    }
                });
            } 
        }
    };
})

.directive('autoFocus', function($timeout) {
    return {
        restrict: 'AC',
        link: function(scope, element) {
            $timeout(function(){
                element[0].focus();
            }, 0);
        }
    };
})
.directive('setNameObject', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, iElement, iAttrs, ctrls) {
            var name = iElement[0].name;
            var baseName = name.split("_")[0];                       
            var scopeForm = scope[iElement[0].form.name];

            scope.$watch(scopeForm, function () {
                $timeout(function () {
                    if (scopeForm[name] != undefined) {
                        scopeForm[baseName + '_' + scope.$index] = scopeForm[name];
                        scopeForm[baseName + '_' + scope.$index].$name = baseName + '_' + scope.$index;
                    }
                });
            });     
        }
    };
})

.directive('ngValidator', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {            
            var DOMForm = angular.element(element)[0];

            var form_name = DOMForm.attributes['name'].value;
            var scopeForm = scope[form_name];

            scopeForm.submitted = false;

            scope.$watch(function () { return DOMForm.length; }, function () {
                setupWatches(DOMForm);
            });
           
            element.on('submit', function (event) {
                event.preventDefault();
                scope.$apply(function () {
                    scopeForm.submitted = true;
                });

                if (scopeForm.$valid) {
                    scope.$apply(function () {
                        scope.$eval(DOMForm.attributes["ng-validator-submit"].value);
                    });
                }
            });

            scopeForm.reset = function () {
                for (var i = 0; i < DOMForm.length; i++) {
                    if (DOMForm[i].name) {
                        scopeForm[DOMForm[i].name].$setViewValue("");
                        scopeForm[DOMForm[i].name].$render();
                    }
                }
                scopeForm.submitted = false;
                scopeForm.$setPristine();
            };

            function setupWatches(formElement) {
                for (var i = 0; i < formElement.length; i++) {
                    if (i in formElement) 
                        setupWatch(formElement[i]);                    
                }
            }

            function setupWatch(elementToWatch) {
                if (elementToWatch.isWatchedByValidator) {
                    return;
                }
                elementToWatch.isWatchedByValidator = true;

                if ("validate-on" in elementToWatch.attributes && elementToWatch.attributes["validate-on"].value === "blur") {
                    angular.element(elementToWatch).on('blur', function (event) {
                        updateValidationMessage(elementToWatch);
                        updateValidationClass(elementToWatch);                        
                    });
                }

                if ("clear-on" in elementToWatch.attributes && elementToWatch.attributes["clear-on"].value === "focus") {
                    angular.element(elementToWatch).on('focus', function (event) {
                        if (!(elementToWatch.name in scopeForm)) {
                            return;
                        }
                        var validationMessageElement = isValidationMessagePresent(elementToWatch);
                        if (validationMessageElement) {
                            validationMessageElement.remove();
                        }
                        angular.element(elementToWatch.parentNode).removeClass('has-error');
                    });
                }

                scope.$watch(function () {
                    return elementToWatch.value + elementToWatch.required + scopeForm.submitted + checkElementValidity(elementToWatch) + getDirtyValue(scopeForm[elementToWatch.name]) + getValidValue(scopeForm[elementToWatch.name]);
                }, function () {
                    if (scopeForm.submitted) {
                        updateValidationMessage(elementToWatch);
                        updateValidationClass(elementToWatch);
                    }
                    else {
                        var isDirtyElement = "validate-on" in elementToWatch.attributes && elementToWatch.attributes["validate-on"].value === "dirty";

                        if (isDirtyElement) {
                            updateValidationMessage(elementToWatch);
                            updateValidationClass(elementToWatch);
                        }
                        else if (scopeForm[elementToWatch.name] && scopeForm[elementToWatch.name].$pristine) {
                            updateValidationMessage(elementToWatch);
                            updateValidationClass(elementToWatch);
                        }
                    }
                });
            }
            function getDirtyValue(element) {
                if (element) {
                    if ("$dirty" in element) 
                        return element.$dirty;                    
                }
            }
            function getValidValue(element) {
                if (element) {
                    if ("$valid" in element) 
                        return element.$valid;                   
                }
            }

            function checkElementValidity(element) {
                if ("validator" in element.attributes) {
                    var isElementValid = scope.$eval(element.attributes.validator.value);
                    scopeForm[element.name].$setValidity("ngValidator", isElementValid);
                    return isElementValid;
                }
            }

            function updateValidationMessage(element) {
                if (!(element.name in scopeForm)) {
                    return;
                }

                var scopeElementModel = scopeForm[element.name];

                var validationMessageElement = isValidationMessagePresent(element);
                if (validationMessageElement) {
                    validationMessageElement.remove();
                }

                var msgText = "";
                var isCustom = false;

                if (scopeElementModel.$visited || scopeElementModel.$dirty || (scope[element.form.name] && scope[element.form.name].submitted)) {
                    if (scopeElementModel.$error.required && 
                         (scopeElementModel.$viewValue == "" || scopeElementModel.$viewValue == undefined)) {
                        if ("required-message" in element.attributes)
                            msgText = element.attributes['required-message'].value;
                        else
                            msgText = "'Field is required'";
                    }
                    else if (scopeElementModel.$error.maxlength) {
                        if ("max-length-message" in element.attributes)
                            msgText = element.attributes['max-length-message'].value;
                        else
                            msgText = "'Field is too long'";
                    }
                    else if ("number" in element.attributes) {
                        if (!isFinite(scopeElementModel.$viewValue)) {
                            scopeElementModel.$setValidity("number", false);                           

                            if (element.attributes['invalid-number-message'].value)
                                msgText = element.attributes['invalid-number-message'].value;
                            else
                                msgText = "Invalid number'";
                        }
                        else {
                            if (scopeElementModel.$error.number)
                                scopeElementModel.$setValidity("number", true);
                        }
                        if ("max-number" in element.attributes &&
                                Number(scopeElementModel.$viewValue) > Number(element.attributes["max-number"].value)) {
                            scopeElementModel.$setValidity("max", false);

                            if ("max-number-message" in element.attributes)
                                msgText = element.attributes["max-number-message"].value;
                            else
                                msgText = "'Too large number'";
                        }
                        else {
                            if (scopeElementModel.$error.max)
                                scopeElementModel.$setValidity("max", true);
                        }
                        if ("min-number" in element.attributes &&
                                Number(scopeElementModel.$viewValue) < Number(element.attributes["min-number"].value)) {
                            scopeElementModel.$setValidity("min", false);

                            if ("min-number-message" in element.attributes)
                                msgText = element.attributes["min-number-message"].value;
                            else
                                msgText = "'Too small number'";
                        }
                        else {
                            if (scopeElementModel.$error.min)
                                scopeElementModel.$setValidity("min", true);
                        }
                    }
                    else if ("date" in element.attributes &&
                            (scopeElementModel.$viewValue != "" && scopeElementModel.$viewValue != undefined)) {
                        if (isNaN(Date.parse(scopeElementModel.$viewValue)) || !isValidDate(scopeElementModel.$viewValue)) {
                            scopeElementModel.$setValidity("date", false);

                            if (element.attributes['invalid-date-message'].value)
                                msgText = element.attributes['invalid-date-message'].value;
                            else
                                msgText = "'Invalid date'";
                        }
                        else {
                            if (scopeElementModel.$error.date)
                                scopeElementModel.$setValidity("date", true);
                        }
                        if ("max-date" in element.attributes &&
                                Date.parse(scopeElementModel.$viewValue) > Date.parse(element.attributes["max-date"].value.replace(/'/g, ""))) {
                            scopeElementModel.$setValidity("maxDate", false);

                            if ("max-date-message" in element.attributes)
                                msgText = element.attributes["max-date-message"].value;
                            else
                                msgText = "'Date exceeds maximum value'";
                        }
                        else {
                            if (scopeElementModel.$error.maxDate)
                                scopeElementModel.$setValidity("maxDate", true);
                        }
                        if ("min-date" in element.attributes &&
                                Date.parse(scopeElementModel.$viewValue) < Date.parse(element.attributes["min-date"].value.replace(/'/g, ""))) {
                            scopeElementModel.$setValidity("minDate", false);

                            if ("min-date-message" in element.attributes)
                                msgText = element.attributes["min-date-message"].value;
                            else
                                msgText = "'Date below minimum value'";
                        }
                        else {
                            if (scopeElementModel.$error.minDate)
                                scopeElementModel.$setValidity("minDate", true);
                        }
                    }

                    else if (!scopeElementModel.$valid) {
                        if ("invalid-message" in element.attributes)
                            msgText = element.attributes['invalid-message'].value;
                        else
                            msgTextg = "'Field is invalild'";
                    }

                    if (msgText != "") {
                        var msgAfterElem;
                        if ("message-after" in element.attributes)
                            msgAfterElem = document.getElementById(element.attributes["message-after"].value);
                        else msgAfterElem = element;
                            
                        angular.element(msgAfterElem).after(generateErrorMessage(msgText, element));
                    }
                    else {
                        scopeElementModel.$visited = false;
                    }
                }
            }

            Date.prototype.valid = function () {
                return isFinite(this);
            }
            function isValidDate(value) {
                var d = new Date(value);                
                if (typeof value === 'string' || value instanceof String) {
                    return d.valid() && value.split('/')[0] == (d.getMonth() + 1); 
                }
                else {
                    return d.valid();
                }                
            }

            function generateErrorMessage(messageText, element) {                
                var displayClass = "";
                if ("message-display-class" in element.attributes) {
                    displayClass = element.attributes["message-display-class"].value;                    
                }
                return "<span class='help-block has-error validationMessage " + displayClass + "' >" + scope.$eval(messageText) + "</span>";
            }

            function isValidationMessagePresent(element) {
                var elementSiblings = angular.element(element).parent().children();
                for (var i = 0; i < elementSiblings.length; i++) {
                    if (angular.element(elementSiblings[i]).hasClass("validationMessage")) {
                        return angular.element(elementSiblings[i]);
                    }
                }
                return false;
            }

            function updateValidationClass(element) {
                if (!(element.name in scopeForm)) {
                    return;
                }
                var formField = scopeForm[element.name];
                angular.element(element.parentNode).removeClass('has-error');

                if (formField.$visited || formField.$dirty || (scope[element.form.name] && scope[element.form.name].submitted)) {
                    if (formField.$invalid) {
                        angular.element(element.parentNode).addClass('has-error');
                    }
                }                
            }
        }
    };
})
;
