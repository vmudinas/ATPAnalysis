app.controller("userLanguageController",
    function($route, $scope, userService, language, languageService, gridDefinitionService) {
        init();

        $scope.title = "UserLanguage";

        function init() {

            try {
                $scope.language = language[0];
                $scope.maingroup = true;

                getUserLangaugeData("English");
                getWordsByLanguage("English");
                $scope.Selectedlanguage = "English";
                $scope.userLangageGridOptions = {
                    bindingOptions: {
                        dataSource: "userData",
                        columns: "userLanguageColumns"
                    },
                    paging: {
                        enabled: false
                    },
                    showBorders: true,
                    showColumnLines: true,
                    showRowLines: true,
                    filterRow: { visible: true },
                    searchPanel: { visible: true },
                    rowAlternationEnabled: true,
                    loadPanel: {
                        enabled: false
                    },
                    scrolling: {
                        mode: "virtual"
                    },
                    selection: {
                        mode: "single"
                    },
                    height: function() {
                        return window.innerHeight / 2;
                    },
                    hoverStateEnabled: true,
                    editing: {
                        mode: "form",
                        allowUpdating: false,
                        allowDeleting: true,
                        allowAdding: true,
                        form: {
                            items: [
                                {
                                    itemType: "group",
                                    caption: $scope.language.caption.userLanguageSettings,
                                    items: ["caption", "userCaption", "userToolTip"]
                                }
                            ]
                        },
                        texts: gridDefinitionService.getEditText()
                    },
                    onRowInserting: function(e) {

                        updateAddUserDefinition(e);
                    },
                    onRowRemoving: function(e) {

                        removeUserDefinition(e);
                    },
                    onToolbarPreparing: function(e) {
                        //    var dataGrid = e.component;

                        e.toolbarOptions.items.unshift(
                        {
                            location: "before",
                            widget: "dxSelectBox",
                            options: {
                                width: 200,
                                items: [
                                    {
                                        value: "English",
                                        text: "English"
                                    }, {
                                        value: "Spanish",
                                        text: "Spanish"
                                    }
                                ],
                                displayExpr: "text",
                                valueExpr: "value",
                                value: "English",
                                onValueChanged: function(e) {

                                    getWordsByLanguage(e.value);
                                    getUserLangaugeData(e.value);
                                    $scope.Selectedlanguage = e.value;
                                }
                            }
                        });
                    }

                };


            } catch (e) {

                console.log(e);

            }


        }

        function removeUserDefinition(e) {

            languageService.removeUserLanguage(e.data.language, e.data.logicName).then(function(response) {
                    getWordsByLanguage(e.data.language);
                    getUserLangaugeData(e.data.language);
                },
                function(response) {


                    console.log(response);
                }
            );

        }

        function updateAddUserDefinition(e) {

            languageService.updateAddUserDefinition($scope.Selectedlanguage,
                    e.data.caption,
                    e.data.userCaption,
                    e.data.userToolTip)
                .then(function(response) {

                        $route.reload();
                    },
                    function(response) {


                        console.log(response);
                    }
                );

        }

        function getWordsByLanguage(languageName) {

            languageService.getWordsByLanguage(languageName).then(function(response) {
                    $scope.wordByLanguage = response.data;

                    $scope.userLanguageColumns = getUserLanguageColumns(response.data);

                },
                function(response) {


                    console.log(response);
                }
            );

        }

        function getUserLangaugeData(language) {

            languageService.getUserLanguageDefinition(language).then(function(response) {

                    $scope.userData = response.data;

                },
                function(response) {


                    console.log(response);
                }
            );
        }

        function getUserLanguageColumns(words) {


            return [
                {
                    dataField: "caption",
                    caption: toTitleCase($scope.language.caption.caption),
                    width: "250",
                    validationRules: [{ type: "required", message: $scope.language.caption.captionRequired }],
                    lookup: {
                        valueExpr: "caption",
                        displayExpr: "caption",
                        title: $scope.language.caption.caption,
                        dataSource: words
                    }
                },
                {
                    dataField: "toolTip",
                    width: "250",
                    caption: $scope.language.caption.toolTip,
                    lookup: {
                        valueExpr: "toolTip",
                        displayExpr: "toolTip",
                        title: $scope.language.caption.toolTip,
                        dataSource: words
                    }
                },
                {
                    dataField: "userCaption",
                    caption: $scope.language.caption.userCaption,
                    validationRules: [{ type: "required", message: $scope.language.caption.userCaptionRequired }],
                    width: "auto"
                },
                {
                    dataField: "userToolTip",
                    validationRules: [{ type: "required", message: $scope.language.caption.userToolTipRequired }],
                    caption: $scope.language.caption.userToolTip

                },
                {
                    dataField: "language",
                    visible: false
                },
                {
                    dataField: "logicName",
                    visible: false
                }
            ];
        }
    });