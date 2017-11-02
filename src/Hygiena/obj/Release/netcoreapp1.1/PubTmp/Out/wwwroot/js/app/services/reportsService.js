app.service("reportsService",
    function($http) {
        return {
            generateReport: function(type, fromDateUtc, toDateUtc) {
                return $http.get("../api/GenerateReport",
                {
                    params: { reportType: type, fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getAllData: function (fromDateUtc, toDateUtc) {
                return $http.get("../api/GetReportData",
                    {
                        params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                    });
            },
            getReport: function ( type, id) {
                

               let result =  {
                   bindto: `#${id}`,
                   size: {},
                   data: {
                       type: type,
                        empty: {
                            label: {
                                text: "No Data Available"
                            }
                        }
                        },
                        title: {
                            text: id.replace("_id_", "").replace("_", "")
                        },
                        color:
                        {
                            pattern:  ["red", "green", "orange","blue"]
                          //  pattern: ["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", "#2ca02c", "#98df8a", "#d62728", "#ff9896", "#9467bd", "#c5b0d5", "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7", "#bcbd22", "#dbdb8d", "#17becf", "#9edae5"]
                        }
               }

               //if (reportType.id === 1) {
               //    result.color.pattern = ["red", "green", "orange"];

               //}
               //else if (reportType.id === 2) {

               //    let fails = data.filter(function (value) {
               //        return value.rlu > value.upper;
               //    }).length;

               // result.data.columns = [
               //     ["Failed", fails],
               //     ["Total", data.length]
               // ];
               // result.color.pattern =["red", "green"];

               // }
               //else if (reportType.id === 3) {

               //    let retest = data.filter(function (value) {
               //        return value.repeatedTest !== null;
               //    }).length;

               // result.data.columns = [
               //     ["Retest", retest],
               //     ["Total", data.length]
               // ];
               // result.color.pattern = ["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7", "#bcbd22", "#dbdb8d", "#17becf", "#9edae5"];


               // }
               // else if (reportType.id === 4) {
               // result.data.columns = [
               //     ["Location A", 1, 333, -44, 123, 123, 123, 123, 333, 5555],
               //     ["Location B", 1, 333, -44, 123, 123, 123, 123, 333, 5555]
               // ];
               // result.color.pattern = ["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7", "#bcbd22", "#dbdb8d", "#17becf", "#9edae5"];


               // }
               // else if (reportType.id === 5) {
               // result.data.columns = [
               //     ["User1", 1, 333, -44],
               //     ["User2", 1, 1, 1],
               //     ["User3", 1, 4, -555],
               //     ["User4", 88, 123, 444],
               //     ["User5", 1, 333, -44],
               //     ["User6", 5, 123, 444],
               //     ["User7", 1, 333, -44],
               //     ["User8", 88, 123, 444],
               //     ["User9", 1, 333, -44],
               //     ["User10", 88, 123, 444],
               //     ["User11", 1, 333, -44],
               //     ["User12", 88, 123, 444],
               //     ["User13", 1, 333, -44],
               //     ["User14", 88, 123, 444],
               //     ["User15", 1, 333, -44],
               //     ["User16", 88, 123, 444],
               //     ["User17", 1, 333, -44],
               //     ["User18", 88, 123, 444],
               //     ["User19", 1, 333, -44],
               //     ["User20", 88, 123, 444],
               //     ["User21", 1, 333, -44],
               //     ["User22", 88, 123, 444],
               //     ["User23", 1, 333, -44],
               //     ["User24", 88, 123, 444],
               //     ["User25", 1, 333, -44],
               //     ["User26", 88, 123, 444],
               //     ["User27", 1, 333, -44],
               //     ["User28", 88, 123, 444],
               //     ["User29", 1, 333, -44],
               //     ["User30", 88, 123, 444],
               //     ["User31", 1, 333, -44],
               //     ["User32", 88, 123, 444],
               //     ["User33", 1, 333, -44],
               //     ["User34", 88, 123, 444],
               //     ["User35", 1, 333, -44],
               //     ["User36", 88, 123, 444]

               // ];
               // result.color.pattern = ["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", '#2ca02c', '#98df8a', '#d62728', '#ff9896', '#9467bd', '#c5b0d5', "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7", "#bcbd22", "#dbdb8d", "#17becf", "#9edae5"];


               // }


               return result;

         
            },
            getTestReports: function() {

                var result = [];
                for (var i = 0; i < 20; i++) {
                    result.push({
                        accountId: i,
                        actualIncubationTime: null,
                        controlExperationDate: null,
                        controlLotNumber: null,
                        controlModifiedBy: null,
                        controlModifiedDate: null,
                        controlName: null,
                        correctedTest: null,
                        correctiveAction: null,
                        createdBy: "MPC\vmudinas",
                        createdDate: "2016-11-07T13:45:16.237",
                        customField1: "",
                        customField2: "",
                        customField3: "",
                        customField4: "",
                        customField5: "",
                        customField6: "",
                        customField7: "",
                        customField8: "",
                        customField9: "",
                        customField10: "",
                        deviceCategory: null,
                        deviceName: null,
                        deviceTemprature: null,
                        deviceUOM: null,
                        dilution: null,
                        groupName: "",
                        incubationTime: null,
                        isDeleted: false,
                        locationName: "Location-1" + i,
                        lower: 10,
                        modifiedBy: "MPC\vmudinas",
                        modifiedDate: null,
                        notes: "",
                        personnel: null,
                        rank: 0,
                        rawADCOutput: 0,
                        repeatedTest: null,
                        result: false,
                        resultDate: "2017-02-07T22:27:00",
                        resultId: "68b7eae8-ab4e-4e51-93d8-34ffd90a482b",
                        rlu: 4,
                        roomNumber: null,
                        sampleType: null,
                        siteId: "3bfc272a-9a7f-427f-831a-d44e57b4f1eb",
                        surfaceName: "",
                        planName: "Plan 19",
                        testState: 3,
                        unitAngle: null,
                        unitName: "Unit - 69293",
                        unitNo: 69293,
                        unitSoftware: null,
                        unitType: "EnSURE v2",
                        upper: 44,
                        userName: "User 2",
                        warningId: 0,
                        zone: ""
                    });
                }
                return result;

            }

        };
    });