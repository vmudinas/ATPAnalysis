app.controller("navController",
    function($scope, language, languageService, periodsArray, hygEnum, $rootScope, $location) {

      
        var markers = [
            {
                coordinates: [-0.1262, 51.5002],
                attributes: {
                    name: "London"
                }
            },          
            {
                coordinates: [-118.2437, 34.0522 ],
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
                customizeTooltip: function (arg) {
                    if (arg.layer.type === "marker") {
                        return { text: arg.attribute("name") };
                    }
                }
            },
            onClick: function (e) {
                if (e.target && e.target.layer.type === "marker") {
                    e.component.center(e.target.coordinates()).zoomFactor(10);
                }
            },
            bounds: [-180, 85, 180, -60],
            layers: [{
                dataSource: DevExpress.viz.map.sources.world,
                hoverEnabled: false
            }, {
                dataSource: markers
            }]
        };

        init();

        function init() {
            try {
                $scope.tabEnum = { home: 1, results: 2, locs: 3, plans: 4, reports: 5, admin: 6 };
                $scope.tabActive = { isHome: false, isResults: false, isLocs: false, isPlans: false, isReports: false, isAdmin: false };

                setTabFromPath($location.path());

                $scope.language = $rootScope.language;
                $scope.selectedLang = $rootScope.baseUrl + "css/flag-uk.png";

                languageService.getUserLanguage()
                    .then(function(response) {
                            if (response.data === "Spanish") {
                                $scope.selectedLang = $rootScope.baseUrl + "css/flag-es.png";
                            }
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
                $scope.selectedLang = $rootScope.baseUrl + flagUrl;

                setActiveFlags($scope.tabEnum.home);

                languageService.updateUserLanguage(languageSelected)
                    .then(function(response) {
                            console.log("updateUserLanguage");
                        },
                        function(response) {
                            console.log(response);
                        }
                    );

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

            if (canAccess) { $location.path(path); }
        };

        function setTabFromPath(urlPath)
        {
            urlPath = urlPath.replace("/", "");

            if (urlPath === "home") { setActiveFlags($scope.tabEnum.home); }
            else if (urlPath === "result") { setActiveFlags($scope.tabEnum.results); }
            else if (urlPath === "locations") { setActiveFlags($scope.tabEnum.locs); }
            else if (urlPath === "plans") { setActiveFlags($scope.tabEnum.plans); }
            else if (urlPath === "reports") { setActiveFlags($scope.tabEnum.reports); }
            else if (urlPath === "userManagement" || urlPath === "roleManagement" || urlPath === "userLangauge" ||
                     urlPath === "adminunit" || urlPath === "language" || urlPath === "scheduleEmailedReports")
            { setActiveFlags($scope.tabEnum.admin); }
            else { setActiveFlags($scope.tabEnum.home); }
        }

        function setActiveFlags(newTab)
        {
            $scope.tabActive.isHome = $scope.tabActive.isResults = $scope.tabActive.isLocs = $scope.tabActive.isPlans = $scope.tabActive.isReports = $scope.tabActive.isAdmin = false;

            if (typeof newTab === "undefined") {
                $scope.tabActive.isHome = true;
            }
            else
            {
                if (newTab === $scope.tabEnum.home) { $scope.tabActive.isHome = true; }
                else if (newTab === $scope.tabEnum.results) { $scope.tabActive.isResults = true; }
                else if (newTab === $scope.tabEnum.locs) { $scope.tabActive.isLocs = true;}
                else if (newTab === $scope.tabEnum.plans) { $scope.tabActive.isPlans = true;}
                else if (newTab === $scope.tabEnum.reports) { $scope.tabActive.isReports = true; }
                else if (newTab === $scope.tabEnum.admin) { $scope.tabActive.isAdmin = true;}
                else { $scope.tabActive.isHome = true; }
            }
        }
    });