
app.service("homeService",
    function($http) {
        return {
            getAllResults: function() {
                return $http.get("../api/result");
            },
            getFails: function(dashId, fromDateUtc, toDateUtc) {
                return $http.get("../api/fails",
                {
                    params: { id: dashId, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getAllByLocation: function(location, fromDateUtc, toDateUtc) {
                return $http.get("../api/allResults",
                {
                    params: { locationName: location, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },

            getFailByLocation: function(locationName, fromDateUtc, toDateUtc) {
                return $http.get("../api/failByLocation",
                {
                    params: { location: locationName, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailByPlan: function(planName, fromDateUtc, toDateUtc) {
                return $http.get("../api/failByPlan",
                {
                    params: { plan: planName, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailsByPlan: function(fromDateUtc, toDateUtc) {
                return $http.get("../api/failsByPlan",
                {
                    params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailsByPeriodChart: function(fromDateUtc, toDateUtc) {

                return $http.get("../api/failsByPeriodChart",
                {
                    params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailsByLocationChart: function(locationName, fromDateUtc, toDateUtc) {
                return $http.get("../api/failsByLocationChart",
                {
                    params: { location: locationName, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailsByPlanChart: function(planName, fromDateUtc, toDateUtc) {
                return $http.get("../api/failsByPlanChart",
                {
                    params: { plan: planName, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getFailsByPlanColumns: function(lang) {
                return [
                    {
                        caption: toTitleCase(lang.plan.caption),
                        dataField: "plan",
                        allowSorting: true
                    },
                    {
                        caption: toTitleCase(lang.numberOfFails.caption),
                        dataField: "numberOfFails",
                        allowSorting: true,
                        width: "auto"
                    }
                ];
            },
            getFailsByLocationColumns: function (lang) {
                return [
                    {
                        caption: toTitleCase(lang.location.caption),
                        dataField: "location",
                        allowSorting: true
                    },
                    {
                        caption: toTitleCase(lang.numberOfFails.caption),
                        dataField: "numberOfFails",
                        allowSorting: true,
                        width: "auto"
                    }
                ];
            },
        };
    });