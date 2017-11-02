app.controller("navController",
    function($scope,
        $route,
        language,
        languageService,
        periodsArray,
        hygEnum,
        $rootScope,
        $location,
        $window,
        userService) {


        var markers = [
            {
                coordinates: [-0.1262, 51.5002],
                attributes: {
                    name: "London"
                }
            },
            {
                coordinates: [-118.2437, 34.0522],
                attributes: {
                    name: "Los Angeles"
                }
            },
            {
                coordinates: [-75.5277, 38.9108],
                attributes: {
                    name: "Los Angeles"
                }
            }
        ];

        $rootScope.vectorMapOptions = {
            tooltip: {
                enabled: true,
                customizeTooltip: function(arg) {
                    if (arg.layer.type === "marker") {
                        return { text: arg.attribute("name") };
                    }
                }
            },
            onClick: function(e) {
                if (e.target && e.target.layer.type === "marker") {
                    e.component.center(e.target.coordinates()).zoomFactor(10);
                }
            },
            bounds: [-180, 85, 180, -60],
            layers: [
                {
                    dataSource: DevExpress.viz.map.sources.world,
                    hoverEnabled: false
                }, {
                    dataSource: markers
                }
            ]
        };

        init();

        function init() {
            try {

                $scope.selectedMenu = "home";
                setTabFromPath($location.path());

                $scope.language = $rootScope.language;
                $scope.selectedLang = $rootScope.baseUrl;


                $scope.selectedLang = $scope.selectedLang.replace("Account/ForgotPassword", "").replace("Manage", "")
                    .replace("ChangePassword", "");

                languageService.getUserLanguage()
                    .then(function(response) {
                            if (response.data === "Spanish") {
                                $scope.selectedLang = $scope.selectedLang + "/css/flag-es.png";

                            }
                            if (response.data === "English") {
                                $scope.selectedLang = $scope.selectedLang + "/css/flag-uk.png";

                            }
                            // + "css/flag-uk.png";


                            languageService.getTranslation(response.data)
                                .then(function(response) {
                                        languageService.translate(language, response.data);
                                        languageService.translatePeriods(periodsArray, response.data, hygEnum);
                                        $scope.language = response.data;
                                        $rootScope.language = response.data;

                                    },
                                    function(response) {
                                        console.log(response);
                                    }
                                );
                        },
                        function(response) {
                            console.log(response);
                        }
                    );
            } catch (e) {
                console.log(e);
            }
        }

        $scope.selectLanguage = function(flagUrl, languageSelected) {
            try {


                var baseUrl = $scope.selectedLang.split("css");
                $scope.selectedLang = baseUrl[0] + flagUrl;

                if (window.location.href.indexOf("Manage") === -1 && window.location.href.indexOf("Account") === -1) {
                    languageService.updateUserLanguage(languageSelected)
                        .then(function(response) {
                                console.log("updateUserLanguage");
                            },
                            function(response) {
                                console.log(response);
                            }
                        );

                }


                languageService.getTranslation(languageSelected)
                    .then(function(response) {
                            languageService.translate(language, response.data);
                            languageService.translatePeriods(periodsArray, response.data, hygEnum);
                            $scope.language = response.data;
                            $rootScope.language = response.data;
                        },
                        function(response) {
                            console.log(response);
                        }
                    );

            } catch (e) {
                console.log(e);
            }
        };


        $scope.go = function(path, canAccess) {
            setTabFromPath(path);
            if (canAccess) {
                $location.path(path);
            }

        };
        $scope.logOff = function() {
            $location.path("/logOff");
        };
        $scope.add = function() {
            console.log("Add");
        };

        function setTabFromPath(path) {


            if (path.length > 0) {
                var menu = path.replace("/", "");
                $scope.selectedMenu = menu;
                $scope.selectedAdminMenu = "";

                if (menu === "userManagement" ||
                    menu === "language" ||
                    menu === "userLangauge" ||
                    menu === "scheduleEmailedReports" ||
                    menu === "adminunit" ||
                    menu === "roleManagement" ||
                    menu === "userManagement"
                ) {
                    $scope.selectedMenu = "admin";
                    $scope.selectedAdminMenu = menu;
                }
            } else {
                $scope.selectedMenu = "home";
            }

            if (window.location.href.includes("ChangePassword")) {
                $scope.selectedMenu = "manage";
                $scope.selectedAdminMenu = "";
            }
        }

    });