
app.service("languageService",
    function($http, $route, userSettings) {
        return {
            //translate function get translated language and maps to the value word passed
            // we assume that correct translation already passed.
            translate: function(language, languageData) {
                language.length = 0;
                language.push(languageData);

                $route.reload();
            },
            translatePeriods: function(periodsArray, language, hygEnum) {
             
                for (var i = 0; i < periodsArray.length; i++) {
                    if (periodsArray[i].id === hygEnum.periodsEnum.LastWeek) {
                        periodsArray[i].period = toTitleCase(language.lastWeek.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.LastMonth) {
                        periodsArray[i].period = toTitleCase(language.lastMonth.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.LastYear) {
                        periodsArray[i].period = toTitleCase(language.lastYear.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.Custom) {
                        periodsArray[i].period = toTitleCase(language.custom.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.ThisWeek) {
                        periodsArray[i].period = toTitleCase(language.thisWeek.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.ThisMonth) {
                        periodsArray[i].period = toTitleCase(language.thisMonth.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.ThisYear) {
                        periodsArray[i].period = toTitleCase(language.thisYear.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.Today) {
                        periodsArray[i].period = toTitleCase(language.today.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.Yesterday) {
                        periodsArray[i].period = toTitleCase(language.yesterday.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.ThisQuarter) {
                        periodsArray[i].period = toTitleCase(language.thisQuarter.caption);
                    } else if (periodsArray[i].id === hygEnum.periodsEnum.LastQuarter) {
                        periodsArray[i].period = toTitleCase(language.lastQuarter.caption);
                    }
                }

                return periodsArray;
            },
            getTranslation: function(language) {
                if (Globalize.locale() !== undefined && language !== undefined) {
                    var cultureCode = "en";

                    if (language == "English") {
                        cultureCode = "en";
                        userSettings.dateFormatShort = "MM/dd/yyyy hh:mm:ss a";
                    } else if (language == "Spanish") {
                        cultureCode = "es";
                        userSettings.dateFormatShort = "dd/MM/yyyy HH:mm:ss";
                    } else if (language == "German") {
                        cultureCode = "de";
                        userSettings.dateFormatShort = "dd.MM.yyyy HH:mm:ss";
                    } // TODO: figure out what additional language(s) we will ACTUALLY support. German for now.
                    else {
                        cultureCode = "en";
                        userSettings.dateFormatShort = "MM/dd/yyyy hh:mm:ss a";
                    }

                    moment.locale(cultureCode);

                    var cldrPrefix = "/js/app/language/globalization/";
                    $.when(
                        $.getJSON(cldrPrefix + cultureCode + "/ca-gregorian.json"),
                        $.getJSON(cldrPrefix + cultureCode + "/numbers.json"),
                        $.getJSON(cldrPrefix + cultureCode + "/currencies.json"),
                        $.getJSON(cldrPrefix + "likelySubtags.json"),
                        $.getJSON(cldrPrefix + "timeData.json"),
                        $.getJSON(cldrPrefix + "weekData.json"),
                        $.getJSON(cldrPrefix + "currencyData.json"),
                        $.getJSON(cldrPrefix + "numberingSystems.json")
                    ).then(function() {
                        return [].slice.apply(arguments, [0]).map(function(result) {
                            return result[0];
                        });
                    }).then(
                        Globalize.load
                    ).then(function() {
                        Globalize.locale(cultureCode);
                    });
                }

                return $http.get("../api/GetTranslation",
                {
                    params: { language: language }
                });
            },
            getLogicalName: function() {
                return $http.get("../api/GetLogicalName");
            },
            getUserLanguage: function() {
                return $http.get("../api/UserLanguage");
            },
            updateUserLanguage: function(language) {
                var data = $.param({
                    language: language
                });
                return $http.put("../api/UpdateUserLanguage?" + data);
            },
            updateLogicalName: function(logicName) {
                var data = $.param({
                    logicName: logicName
                });
                return $http.put("../api/AddLogicalName?" + data);
            },
            deleteLogicalName: function(logicName) {
                var data = $.param({
                    logicName: logicName
                });
                return $http.delete("../api/DeleteLogicalName?" + data);
            },
            getAvailableLanguages: function() {
                return ["English", "Spanish"];
            },
            updateLanguages: function(language, languageData) {
                var data = $.param({
                    language: language,
                    languageData: languageData
                });
                var config = {
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;"
                    }
                };
                return $http.post("../api/UpdateLanguageData", data, config);
            },
            getUserLanguageDefinition: function(language) {
                return $http.get("../api/GetUserLanguageDefinition",
                {
                    params: { language: language }
                });
            },
            removeUserLanguage: function(language, logicName) {
                return $http.get("../api/RemoveUserLanguage",
                {
                    params: { language: language, logicName: logicName }
                });
            },
            getWordsByLanguage: function(language) {
                return $http.get("../api/GetWordsByLanguage",
                {
                    params: { language: language }
                });
            },
            updateAddUserDefinition: function(language, caption, userCaption, userToolTip) {
                return $http.get("../api/updateAddUserDefinition",
                {
                    params: { language: language, caption: caption, userCaption: userCaption, userToolTip: userToolTip }
                });
            }
        };
    });