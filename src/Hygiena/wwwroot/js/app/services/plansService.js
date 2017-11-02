
app.service("plansService",
    function($http) {
        return {
            getAllData: function() {
                return [];
            },
            getCurrentPlans: function() {
                return $http.get("../api/GetPlans");
            }
        };
    });