
app.service("userService",
    function($http) {
        return {
            getAllData: function() {
                return [];
            },
            getRegisterToken: function(siteId, tokenName, unitName) {
                return $http.get("../api/GenerateToken",
                {
                    params: { siteId: siteId, tokenName: tokenName, unitName: unitName }
                });
            },
            getRemoveToken: function(token, userName) {
                return $http.get("../api/RemoveToken",
                {
                    params: { tokenName: token, userName: userName }
                });
            },
            getCurrentUser: function() {
                return $http.get("../api/GetCurrentUser");
            },
            getUserSettings: function() {
                return $http.get("../api/GetUserSettings");
            },
            updateUserSettings: function(section, userSettings, fromDateUtc, toDateUtc) {
                var data = $
                    .param({ whichSection: section, cus: userSettings, fromUtc: fromDateUtc, toUtc: toDateUtc });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateUserSettings", data, config);
            },
            getSites: function() {
                return $http.get("../api/GetAccountSites");
            },
            getUserSites: function(userName) {
                return $http.get("../api/GetUserSites",
                {
                    params: { userName: userName }
                });
            },
            updateUserSite: function(userSite) {
                var data = $.param({ userSite: userSite });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateUserSite", data, config);
            },
            getUnitTokens: function() {
                return $http.get("../api/GetUnitTokens");
            },
            getUsersForAccount: function() {

                return $http.get("../api/GetCurrentUsers");

            },
            getRolesForAccount: function() {
                return $http.get("../api/GetAccountRoles");

            },
            addRoleForAccount: function(roleId, role, roleDescription) {
                return $http.get("../api/AddRole",
                {
                    params:
                    {
                        roleId: roleId,
                        role: role,
                        roleDescription: roleDescription
                    }
                });

            },
            removeRole: function(roleId) {
                return $http.get("../api/RemoveRole",
                {
                    params:
                    {
                        roleId: roleId
                    }
                });

            },
            updateRole: function(roleId, role, roleDescription) {
                return $http.get("../api/UpdateRole",
                {
                    params:
                    {
                        roleId: roleId,
                        role: role,
                        roleDescription: roleDescription
                    }
                });

            },
            getRolePermissions: function(role) {
                return $http.get("../api/GetRolePermissions",
                {
                    params: { role: role }
                });
            },
            getAllRolePermissions: function() {
                return $http.get("../api/GetAllRolePermissions");
            },
            getUserRolePermissions: function() {
                return $http.get("../api/GetUserRolePermissions");
            },
            getAddUser: function(newUser) {
                return $http.get("../api/AddUser",
                {
                    params: { userName: newUser.userName, email: newUser.email, role: newUser.role }
                });
            },
            getUpdateUser: function(oldUserName, newUser, newEmail, newRole) {
                return $http.get("../api/UpdateUser",
                {
                    params: { oldUser: oldUserName, newUser: newUser, newEmail: newEmail, newRole: newRole }
                });
            },
            getRemoveUser: function(userToRemove) {
                return $http.get("../api/RemoveUser",
                {
                    params: { userName: userToRemove.userName }
                });
            },
            getUpdatePermission: function(role, permission, status) {
                return $http.get("../api/AddPermission",
                {
                    params: { role: role, permission: permission, status: status }
                });
            },
            getRemovePermission: function(rolePermissionId) {
                return $http.get("../api/RemovePermission",
                {
                    params: { rolePermissionId: rolePermissionId }
                });
            },
            updatePassword: function(password) {
                //{ model: password }
                var data = $.param(password);
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/ChangePassword", data, config);
            },

        };
    });