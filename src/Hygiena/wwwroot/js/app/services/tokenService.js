
app.service("tokenService",
    function($http) {
        return {
            getTokens: function() {
                return $http.get("../api/GetTokens");
            },
            generateToken: function () {
                return $http.get("../api/CreateToken");
            },
            allowDebug: function (token, unitSerial) {
                return $http.get("../api/AllowDebug",
                    {
                        params: { token: token, unitSerial: unitSerial }
                    });
            }
        };
    });