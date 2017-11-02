app.controller("adminUserManagementController",
    function($scope, userService, language) {

        $scope.title = "User Management";
        $scope.language = language[0];

        init();

        function init() {


            if ($scope.language !== undefined) {
                setContext();
            }

        }

        function setContext() {
            getRolesData();
            getUserData();
            $scope.userManagementGridOptions = {
                bindingOptions: {
                    dataSource: "userData",
                    columns: "userManagerColumns"
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
                height: "inherit",
                hoverStateEnabled: true,
                editing: {
                    mode: "form",
                    allowUpdating: true,
                    allowDeleting: true,
                    allowAdding: true,
                    form: {
                        items: [
                            {
                                itemType: "group",
                                caption: $scope.language.userDetails.caption,
                                items: ["userName", "email", "roleName"]
                            }
                        ]
                    }
                },
                onRowInserting: function(e) {

                    userService.getAddUser(e.data).then(function(response) {

                        console.log(response.data);
                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onRowUpdating: function(e) {

                    userService.getUpdateUser(e.oldData.userName, e.newData.userName, e.newData.email, e.newData.role)
                        .then(function(response) {


                            },
                            function(response) {

                                console.log(response);
                            }
                        );
                },
                onRowRemoving: function(e) {
                    // removeToken(e);

                    userService.getRemoveUser(e.data).then(function(response) {


                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onSelectionChanged: function(selectedItems) {
                    var data = selectedItems.selectedRowsData[0];


                }

            };

            $scope.userRolesGridOptions = {
                bindingOptions: {
                    dataSource: "userRoleData",
                    columns: "userRoleColumns"
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
                height: 350,
                hoverStateEnabled: true,
                editing: {
                    mode: "form",
                    allowUpdating: true,
                    allowDeleting: true,
                    allowAdding: true,
                    form: {
                        items: [
                            {
                                itemType: "group",
                                caption: $scope.language.role.caption,
                                items: ["roleName", "roleDescription"]
                            }
                        ]
                    }
                },
                onRowInserting: function(e) {

                    userService.addRoleForAccount(e.data.__KEY__, e.data.roleName, e.data.roleDescription)
                        .then(function(response) {

                            },
                            function(response) {

                                console.log(response);
                            }
                        );

                },
                onRowUpdating: function(e) {

                    userService.updateRole(e.key.roleId, e.newData.roleName, e.newData.roleDescription)
                        .then(function(response) {

                            },
                            function(response) {

                                console.log(response);
                            }
                        );
                },
                onRowRemoving: function(e) {

                    userService.removeRole(e.data.roleId).then(function(response) {

                        },
                        function(response) {

                            console.log(response);
                        }
                    );
                },
                onSelectionChanged: function(selectedItems) {
                    var data = selectedItems.selectedRowsData[0];
                    getPermissionsData(data.roleName);

                }

            };

            $scope.userPermissionsGridOptions = {
                bindingOptions: {
                    dataSource: "userPermissionData",
                    columns: "userRolePermissionsColumns"
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
                height: 350,
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
                                caption: $scope.language.permission.caption,
                                items: ["permissionName"]
                            }
                        ]
                    }
                },
                onRowInserting: function(e) {
                    // update
                    userService.getInsertPermission($scope.selectedRole, e.data.permissionName)
                        .then(function(response) {
                            },
                            function(response) {
                                console.log(response);
                            }
                        );


                },
                onRowRemoving: function(e) {

                    userService.getRemovePermission(e.data.__KEY__)
                        .then(function(response) {
                            },
                            function(response) {
                                console.log(response);
                            }
                        );
                },
                onSelectionChanged: function(selectedItems) {
                    var data = selectedItems.selectedRowsData[0];

                }

            };

        }

        function getUserManagementColumns(roles) {
            console.log(roles);
            $scope.userManagerColumns = [
                {
                    dataField: "userName",
                    width: "150",
                    sortOrder: "asc",
                    caption: toTitleCase($scope.language.userName.caption),
                    validationRules: [{ type: "required", message: $scope.language.userNameRequired.caption }]
                },
                {
                    dataField: "email",
                    caption: toTitleCase($scope.language.email.caption),
                    validationRules: [
                        { type: "required", message: $scope.language.emailRequired.caption }, { type: "email" }
                    ]
                },
                {
                    dataField: "roleName",
                    caption: toTitleCase($scope.language.role.caption),
                    width: "150",
                    validationRules: [{ type: "required", message: $scope.language.roleRequired.caption }],
                    lookup:
                    {
                        valueExpr: "role",
                        displayExpr: "role",
                        dataSource: roles
                    }
                }
            ];

        }

        function getUserData() {

            userService.getUsersForAccount().then(function(response) {
              
                    $scope.userData = response.data;

                },
                function(response) {

                    console.log(response);
                }
            );


        }

        function getRolesData() {


            userService.getRolesForAccount().then(function(response) {
               
                    $scope.userRoleData = response.data;

                    getUserManagementColumns(response.data);

                },
                function(response) {

                    console.log(response);

                }
            );


        }

    });