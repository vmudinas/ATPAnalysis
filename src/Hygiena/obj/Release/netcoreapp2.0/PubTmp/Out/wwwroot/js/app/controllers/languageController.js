app.controller("languageController",
    function($scope, language, languageService, gridDefinitionService, $route) {

        init();

        function init() {
            $scope.saveAddBtn = false;
            $scope.language = language[0];
            $scope.languageName = "English";
            $scope.logicalNameDisabled = false;
            $scope.selectBox = {
                simple: {
                    items: languageService.getAvailableLanguages(),
                    bindingOptions: {
                        value: "languageName"
                    }
                }
            };
            $scope.languageToUpdate = [];

        }

        $scope.onChange = function(name, identity) {

            $("." + name + identity).css({ background: "red" });
            $scope.saveAddBtn = true;
            $scope.logicalNameDisabled = true;

            if ($scope.languageToUpdate.indexOf(name) === -1) {
                $scope.languageToUpdate.push(name);
            }


        };
        $scope.saveBtn = function() {

            var languageChange = [];
            $scope.languageToUpdate.forEach(function(entry) {

                $scope.languageForEdit[entry].name = entry;
                languageChange.push($scope.languageForEdit[entry]);

            });

            languageService.updateLanguages($scope.languageName, languageChange)
                .then(function(response) {
                        $route.reload();
                    },
                    function(response) {

                        console.log(response);
                    });
            $route.reload();
            //Call service pass updated language with language identification
        };
        $scope.removeSpace = function() {
            $scope.logicalName = toCamelCase($scope.logicalName.replace(/\s+/g, "").trim());
        };
        $scope.checkIfExist = function() {
            return $scope.logicalNames.filter(function(el) {
                    return el.logicName === $scope.logicalName;
                })
                .length;
        };
        $scope.addRow = function() {

            if ($scope.checkIfExist() !== 0) {
                alert("Logical Name: " + $scope.logicalName + " already exists!");

            } else {

                $scope.logicalNames.push({ logicName: $scope.logicalName });
                languageService.updateLogicalName($scope.logicalName).then(function(response) {

                        $route.reload();

                    },
                    function(response) {
                        alert("Value not updated");

                    });
                $scope.logicalName = "";
            }

        };
        $scope.removeLogicName = function(name) {

            languageService.deleteLogicalName(name).then(function(response) {

                    $route.reload();

                },
                function(response) {

                    console.log(response);
                });
        };
        $scope.resetBtn = function() {
            $route.reload();
        };
        $scope.showAddBtn = function() {

            if ($scope.logicalName === undefined) {
                return false;
            }
            if ($scope.checkIfExist() !== 0) {
                return false;
            }
            if ($scope.saveAddBtn === true) {
                return false;
            }
            if ($scope.logicalName === "") {
                return false;
            }
            return true;
        };
        $scope.$watch("languageName",
            function(e) {
                try {
                    $scope.logicalNameDisabled = false;
                    $scope.saveAddBtn = false;
                    languageService.getTranslation($scope.languageName)
                        .then(function(response) {
                                $scope.languageForEdit = response.data;

                            },
                            function(response) {

                                console.log(response);
                            });


                    languageService.getLogicalName().then(function(response) {

                            $scope.logicalNames = response.data;

                        },
                        function(response) {

                            console.log(response);
                        });
                } catch (e) {
                    console.log("Failed on languageName");
                }
            });


    });