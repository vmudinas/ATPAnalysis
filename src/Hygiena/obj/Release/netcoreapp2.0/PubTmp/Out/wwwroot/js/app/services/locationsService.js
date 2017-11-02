
app.service("locationsService",
    function($http) {
        return {
            getCurrentLocations: function() {
                return $http.get("../api/GetLocations");
            }
        };
    });