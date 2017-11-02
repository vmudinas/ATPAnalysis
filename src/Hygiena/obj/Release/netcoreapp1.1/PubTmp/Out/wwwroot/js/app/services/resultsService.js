
app.service("resultService",
    function($http, $filter) {
        return {
            getResults: function(fromDateUtc, toDateUtc) {
                return $http.get("../api/result",
                {
                    params: { fromUtc: fromDateUtc, toUtc: toDateUtc }
                });
            },
            getPagedResultsCount: function(fromUtc, toUtc, loadOption) {

                var data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                var config = {
                    params: data,
                    headers: { 'Accept': "application/json" }
                };

                return $http.get("../api/PagedResultsCount", config);

            },

            getPagedResults: function(fromUtc, toUtc, loadOption) {

                var data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                var config = {
                    params: data,
                    headers: { 'Accept': "application/json" }
                };


                return $http.get("../api/PagedResults", config);
            },
            updateResult: function(clientResult) {
                var data = $.param({ cr: clientResult });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResult", data, config);
            },
            updateResultGridSchema: function(resultGridSchemaJSON) {
                var data = $.param({ resultGridSchemaJSON: resultGridSchemaJSON });
                var config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResultGridSchema", data, config);
            },
            getResultGridSchemaJSON: function() {
                return $http.get("../api/GetResultGridSchema");
            },
            getResultColumns: function(userPermissions, lang, userSettings, resultSchemaJsonString) {

                var baseSchema = [];
                baseSchema.length = 0;
                getBaseColObj("actionImage", "string", baseSchema);
                getBaseColObj("resultDate", "date", baseSchema);
                getBaseColObj("locationName", "string", baseSchema);
                getBaseColObj("unitNo", "number", baseSchema);
                getBaseColObj("unitName", "string", baseSchema);
                getBaseColObj("planName", "string", baseSchema);
                getBaseColObj("userName", "string", baseSchema);
                getBaseColObj("lower", "number", baseSchema);
                getBaseColObj("upper", "number", baseSchema);
                getBaseColObj("rlu", "number", baseSchema);
                getBaseColObj("notes", "string", baseSchema);
                getBaseColObj("groupName", "string", baseSchema);
                getBaseColObj("surfaceName", "string", baseSchema);
                getBaseColObj("createdBy", "string", baseSchema);
                getBaseColObj("rank", "number", baseSchema);
                getBaseColObj("zone", "string", baseSchema);
                getBaseColObj("unitType", "string", baseSchema);
                getBaseColObj("roomNumber", "number", baseSchema);
                getBaseColObj("personnel", "string", baseSchema);
                getBaseColObj("correctiveAction", "string", baseSchema);
                getBaseColObj("customColumn1", "string", baseSchema);
                getBaseColObj("customColumn2", "string", baseSchema);
                getBaseColObj("customColumn3", "string", baseSchema);
                getBaseColObj("customColumn4", "string", baseSchema);
                getBaseColObj("customColumn5", "string", baseSchema);
                getBaseColObj("customColumn6", "string", baseSchema);
                getBaseColObj("customColumn7", "string", baseSchema);
                getBaseColObj("customColumn8", "string", baseSchema);
                getBaseColObj("customColumn9", "string", baseSchema);
                getBaseColObj("customColumn10", "string", baseSchema);
                getBaseColObj("isDeleted", "boolean", baseSchema);


                if (resultSchemaJsonString) {
                    var arrRsltSchmaCols = angular.fromJson(resultSchemaJsonString);


                    Object.assign(baseSchema, arrRsltSchmaCols);
                }

                setDefaultValues(baseSchema, userSettings, userPermissions);
                baseSchema.forEach(translate.bind(null, lang));

                return baseSchema;
            }
        };

        function setDefaultValues(baseSchema, userSettings, userPermissions) {


            $.grep(baseSchema,
                function(b, index) {


                    if (b.dataField === "actionImage") {
                        b.caption = "";
                        b.width = 30;
                        b.cellTemplate = function(container, options) {
                            if (typeof options !== "undefined" && options !== null && options.data !== null) {
                                $("<div class='" + options.data.resultState + "'>&nbsp;</div>").appendTo(container);
                            }
                        };
                        b.allowFiltering = false;
                        b.allowReordering = false;
                        b.allowGrouping = false;
                        b.allowSorting = false;
                        b.showInColumnChooser = false;
                        b.allowResizing = false;
                        baseSchema[index] = b;
                    }

                    if (b.dataField === "resultDate") {

                        b.allowHiding = false;
                        b.format = userSettings.dateFormatShort;
                        baseSchema[index] = b;
                        b.cellTemplate = function(container, options) {
                            if (typeof options !== "undefined" && options !== null && options.data !== null) {
                                var stillUtc = moment.utc(options.data.resultDate).toDate();

                                var localDate = moment(stillUtc).local().format("l");
                                var localTime = moment(stillUtc).local().format("LT");

                                $("<div>" + localDate + " " + localTime + "</div>").appendTo(container);
                            }
                        };
                    }
                    if (b.dataField === "locationName") {

                        b.allowEditing = userPermissions.canEditResultLocation;
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "testType") {

                        b.calculateCellValue = function(data) {
                            if (typeof data !== "undefined" && data !== null) {
                                if (data.testType === "repeatReading") {
                                    return toTitleCase(lang.repeatReading.caption);
                                } else if (data.testType === "reTested") {
                                    return toTitleCase(lang.reTested.caption);
                                } else if (data.testType === "reTestResult") {
                                    return toTitleCase(lang.reTestResult.caption);
                                } else {
                                    return toTitleCase(lang.normal.caption);
                                }
                            }
                        };
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "resultState") {

                        b.calculateCellValue = function(data) {
                            if (typeof data !== "undefined" && data !== null) {
                                if (data.resultState === "pass") {
                                    return toTitleCase(lang.pass.caption);
                                } else if (data.resultState === "fail") {
                                    return toTitleCase(lang.fail.caption);
                                } else {
                                    return toTitleCase(lang.caution.caption);
                                }
                            }
                        };
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "groupName") {

                        b.allowEditing = userPermissions.canEditResultGroup;
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "surfaceName") {

                        b.allowEditing = userPermissions.canEditResultSurface;
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "notes") {

                        b.allowEditing = userPermissions.canEditResultNotes;
                        baseSchema[index] = b;
                    }

                    if (b.dataField === "rank") {

                        b.allowEditing = userPermissions.canEditResultRank;
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "zone") {

                        b.allowEditing = userPermissions.canEditResultZone;
                        baseSchema[index] = b;
                    }

                    if (b.dataField === "isDeleted") {

                        b.allowEditing = userPermissions.canMarkResultsDeleted;
                        b.width = 100;
                        b.allowFiltering = true;
                        b.allowHeaderFiltering = true;
                        baseSchema[index] = b;


                    }

                });
        }

        // assigns most common fields. if different, assign individually after this call.
        function getBaseColObj(dataField, dataType, baseObject) {

            var objColumn = {};
            //  objColumn.caption = "";
            objColumn.dataField = dataField;
            objColumn.dataType = dataType;
            objColumn.width = "auto";
            objColumn.allowEditing = false;
            objColumn.showInColumnChooser = true;
            objColumn.allowResizing = true;
            objColumn.allowHiding = true;
            baseObject.push(objColumn);


        };

        function translate(lang, element, index, array) {

            var languageElement = lang[element.dataField];
            if (languageElement) {
                element.caption = toTitleCase(toTitleCase(languageElement.caption));
            }

        };


    });