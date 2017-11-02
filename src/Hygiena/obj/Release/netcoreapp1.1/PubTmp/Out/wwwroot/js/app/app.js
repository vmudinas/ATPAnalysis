var app = angular.module("app", ["dx", "ngRoute", "ngSanitize"])
    .directive("disallowSpaces",
        function() {
            return {
                restrict: "A",

                link: function($scope, $element) {
                    $element.bind("input",
                        function() {
                            $(this).val($(this).val().replace(/ /g, ""));
                        });
                }
            };
        })

//Implement ng-route directive later
    .config([
        "$routeProvider", "$locationProvider", function($routeProvider, $locationProvider) {
            $routeProvider
                .when("/home",
                {
                    templateUrl: "../Navigation/HomeView"
                })
                .when("/",
                {
                    templateUrl: "../Navigation/HomeView"
                })
                .when("/result",
                {
                    templateUrl: "../Navigation/ResultsView"
                })
                .when("/reports",
                {
                    templateUrl: "../Navigation/ReportsView"
                })
                .when("/pieChartReport",
                {
                    templateUrl: "../Navigation/ReportsPieChart"
                })
                .when("/adminunit",
                {
                    templateUrl: "../Navigation/AdminUnitView"
                })
                .when("/userLangauge",
                {
                    templateUrl: "../Navigation/AdminUserLanguageView"
                })
                .when("/userManagement",
                {
                    templateUrl: "../Navigation/AdminUserManagementView"
                })
                .when("/units",
                {
                    templateUrl: "../Navigation/UnitsView"
                })
                .when("/locations",
                {
                    templateUrl: "../Navigation/LocationsView"
                })
                .when("/plans",
                {
                    templateUrl: "../Navigation/PlansView"
                })
                .when("/language",
                {
                    templateUrl: "../Navigation/LanguageView"
                })
                .when("/admin",
                {
                    templateUrl: "../Navigation/AdminView"
                })
                .when("/roleManagement",
                {
                    templateUrl: "../Navigation/RoleManagementView"
                })
                .when("/scheduleEmailedReports",
                {
                    templateUrl: "../Navigation/ScheduleEmailedReportsView"
                })
                .when("/manage",
                {
                    templateUrl: "../Manage",
                    controller: "navController"
                })
                .when("/changePassword",
                {
                    templateUrl: "../ChangePassword",
                    controller: "adminController"
                })
                .when("/setPassword",
                {
                    templateUrl: "../SetPassword",
                    controller: "adminController"
                })
                .when("/logOff",
                {
                    templateUrl: "../GetLogOff"
                })
                .otherwise({
                    redirectTo: "/home"
                });
            //$locationProvider.html5Mode({
            //    enabled: true,
            //    requireBase: false
            //});
        }
    ])
    .constant("hygEnum",
    {
        "periodsEnum": {
            "Today": 0,
            "Yesterday": 1,
            "ThisWeek": 2,
            "LastWeek": 3,
            "ThisMonth": 4,
            "LastMonth": 5,
            "ThisQuarter": 6,
            "LastQuarter": 7,
            "ThisYear": 8,
            "LastYear": 9,
            "Custom": 10
        },
        "reportEnum": {
            "PCF": 0,
            "Fail": 1,
            "Pass": 2,
            "Retest": 3,
            "Caution": 4
        },
        "langsEnum": { "English": 1, "Spanish": 2, "Russian": 3 }
    })
    .value("periodsArray",
    [
        { id: 0, period: "today" },
        { id: 1, period: "yesterday" },
        { id: 2, period: "thisWeek" },
        { id: 3, period: "lastWeek" },
        { id: 4, period: "thisMonth" },
        { id: 5, period: "lastMonth" },
        { id: 6, period: "thisQuarter" },
        { id: 7, period: "lastQuarter" },
        { id: 8, period: "thisYear" },
        { id: 9, period: "lastYear" },
        { id: 10, period: "custom" }
    ])
    .value("language", [])
    .value("userRoles", [])
    .value("selectedLanguage", [{ language: "English", culture: "en" }])
    .value("account", [{ id: 0 }])
    .value("userSettings",
    {
        "dateFormatShort": "",
        "dateFormatLong": "",
        "dashType": 0,
        "dashPeriod": 0,
        "dashDateFrom": "",
        "dashDateTo": "",
        "resultsPeriod": 0,
        "resultsDateFrom": "",
        "resultsDateTo": "",
        "preferredLang": 1,
        "reportScheduleCurrentView": ""
    })
    .run([
        "$route", "periodsArray", "hygEnum", "language", "languageService", "$rootScope", "$location",
        function ($route, periodsArray, hygEnum,  language, languageService, $rootScope, $location) {

            // We need to get Account Id and Selected Language Id
            // Get All Translations using those ids
            // Bind translations to the value 'language'
            // Use 'language' and languageService to translate

            $rootScope.baseUrl = window.location.origin +
                window.location.pathname
                .replace("Account/Login", "").replace("Account/Register", "");

            $rootScope.language = [];
            $rootScope.language.register = [];
            $rootScope.language.register.caption = "register";
            $rootScope.language.home = [];
            $rootScope.language.home.caption = "home";
            $rootScope.language.login = [];
            $rootScope.language.login.caption = "log in";
            $rootScope.language.results = [];
            $rootScope.language.results.caption = "results";
            $rootScope.language.locations = [];
            $rootScope.language.locations.caption = "locations";
            $rootScope.language.plans = [];
            $rootScope.language.plans.caption = "plans";
            $rootScope.language.reports = [];
            $rootScope.language.reports.caption = "reports";
            $rootScope.language.units = [];
            $rootScope.language.units.caption = "units";
            $rootScope.language.admin = [];
            $rootScope.language.admin.caption = "admin";
            $rootScope.language.users = [];
            $rootScope.language.users.caption = "users";
            $rootScope.language.logoff = [];
            $rootScope.language.logoff.caption = "Log out";
            $rootScope.language.hello = [];
            $rootScope.language.hello.caption = "hello";
            $rootScope.english = "English";
            $rootScope.spanish = "Español";

            languageService.getTranslation("English")
                .then(function(response) {
                        languageService.translate(language, response.data);
                        languageService.translatePeriods(periodsArray, response.data, hygEnum);
                        $rootScope.language = response.data;
                        $rootScope.language.register.caption = response.data.register.caption;
                        $rootScope.language.login.caption = response.data.login.caption;
                        $rootScope.language.results.caption = response.data.results.caption;
                        $rootScope.language.locations.caption = response.data.locations.caption;
                        $rootScope.language.plans.caption = response.data.plans.caption;
                        $rootScope.language.reports.caption = response.data.reports.caption;
                        $rootScope.language.units.caption = response.data.units.caption;
                        $rootScope.language.admin.caption = response.data.admin.caption;
                        $rootScope.language.users.caption = response.data.users.caption;

                        $route.reload();

                    },
                    function(response) {
                        console.log(response);
                    }
                );


        }
    ])
    .filter("capitalize",
        function() {
            return function(input) {
                return input ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : "";
            };
        });


function toTitleCase(str) {
    if (str === undefined || str === null) {
        return "";
    }
    return str.replace(/\w\S*/g, function(txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}

function toCamelCase(str) {
    if (str === undefined || str === null) {
        return "";
    }
    return str.replace(/\w\S*/g, function(txt) { return txt.charAt(0).toLowerCase() + txt.substr(1); });
}

var contDisp = function() {
    var r = $("html").height();
    return r;
};