app.controller("adminRegistrationController",
    function($scope, userService, language) {
        init();

        $scope.title = "Registration";

        function init() {

            try {
                $scope.language = language[0];
                $scope.maingroup = true;
                


                $scope.unitTokenGridOptions = {
                    bindingOptions: {
                        dataSource: "unitTokenData",
                        columns: "unitTokenColumns"
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
                                    caption: $scope.language.generateToken.caption,
                                    items: ["token", "siteName", "name","isCloudSync"]
                                }
                            ]
                        },
                        texts: {

                            confirmDeleteMessage:  $scope.language.cloudDeleteMessage.caption,
                            confirmDeleteTitle: $scope.language.cloudDeleteMessageTitle.caption

                        }

                    },
                    onRowInserting: function(e) {
                        // update
                        registerToken(e);
                    },
                    onRowRemoving: function(e) {
                        removeToken(e);
                    }

                };


                getColumns();

                getTokens();
            } catch (e) {

            }


        }


        function removeToken(e) {

            userService.getRemoveToken(e.data.token, e.data.creator).then(function(response) {

                    getTokens();
                },
                function(response) {

                    console.log(response);
                }
            );
        }

        function registerToken(e) {

        userService.getRegisterToken(e.data.siteName, e.data.token, e.data.name, e.data.isCloudSync).then(function(response) {
                    $scope.token = response.data;
                    getTokens();
                },
                function(response) {

                    console.log(response);
                }
            );
        }

        function getColumns() {

            userService.getSites().then(function(response) {

                    $scope.unitTokenColumns = getTokenColumns(response.data);
                },
                function(response) {

                    console.log(response);
                }
            );


        }

        function getTokens() {

            userService.getUnitTokens().then(function(response) {

                    $scope.unitTokenData = response.data;

                },
                function(response) {
                    $scope.unitTokenData = [];
                    console.log(response);
                }
            );
        }

        function getTokenColumns(sites) {


            return [
               
                {
                    dataField: "token",
                    width: "auto",
                    caption: window.toTitleCase($scope.language.tokenName.caption),
                    validationRules: [
                        { type: "required", message: $scope.language.tokenRequired.caption },
                        {
                            type: "custom",
                            validationCallback: function (options) {

                                const compare = window.$.grep($scope.unitTokenData,
                                    function (obj) { return obj.token === options.value; });

                                if (compare.length > 0) {
                                    return false;
                                }


                                return true;
                            },
                            message: "Token already exist"
                        }
                    ]
                },
                {
                    dataField: "name",
                    caption: window.toTitleCase($scope.language.description.caption),
                    width: "auto",
                    validationRules: [{ type: "required", message: $scope.language.descriptionRequired.caption }]
                },
                {
                    dataField: "siteName",
                    caption: window.toTitleCase($scope.language.siteName.caption),
                    validationRules: [{ type: "required", message: $scope.language.siteNameRequired.caption }],
                    width: 300,
                    lookup: {
                        valueExpr: "id",
                        displayExpr: "name",
                        title: "Site Name",
                        dataSource: sites
                    },
                    cellTemplate: function(container, options) {

                        $(`<div> ${options.value} </div>`).appendTo(container);

                    }
                },
                {
                    dataField: "creator",
                    caption: window.toTitleCase($scope.language.creator.caption),
                    width: 200
                },
                {
                    dataField: "userId",
                    caption: window.toTitleCase($scope.language.status.caption),
                    width: "auto",
                    cellTemplate: function(container, options) {

                        if (options.value) {
                            window.$("<div class='registeredClass'> " +
                                window.toTitleCase($scope.language.registered.caption) +
                                " </div>").appendTo(container);
                        } else {
                            window.$("<div class='pendingClass'> " + window.toTitleCase($scope.language.pending.caption) + " </div>")
                                .appendTo(container);
                        }
                    }
                },             
             
               
                {
                    dataField: "unitNumber",
                    caption: window.toTitleCase($scope.language.unitNumber.caption),
                    width: "auto"
                },
                {
                    dataField: "isCloudSync",
                    dataType: "boolean",
                    caption: window.toTitleCase($scope.language.syncCloud.caption),
                    width: "auto"
                }
            ];
        }
    });