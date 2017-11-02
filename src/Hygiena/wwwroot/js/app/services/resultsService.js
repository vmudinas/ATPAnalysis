
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

                const data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                const config = {
                    params: data,
                    headers: { 'Accept': "application/json" }
                };

                return $http.get("../api/PagedResultsCount", config);

            },

            getPagedResults: function(fromUtc, toUtc, loadOption) {

                const data = {
                    fromUtc: fromUtc,
                    toUtc: toUtc,
                    loadOption: loadOption
                };

                const config = {
                    params: data,
                    headers: { 'Accept': "application/json" }
                };


                return $http.get("../api/PagedResults", config);
            },
            updateResult: function(clientResult) {
                const data = window.$.param({ cr: clientResult });
                const config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResult", data, config);
            },
            updateResultGridSchema: function(resultGridSchemaJson) {
                const data = window.$.param({ resultGridSchemaJSON: resultGridSchemaJson });
                const config = { headers: { "Content-Type": "application/x-www-form-urlencoded;charset=utf-8;" } };
                return $http.post("../api/UpdateResultGridSchema", data, config);
            },
            getResultGridSchemaJSON: function() {
                return $http.get("../api/GetResultGridSchema");
            },
            getResultColumns: function(userPermissions, lang, userSettings, resultSchemaJsonString) {

                const baseSchema = [];
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
                    var arrRsltSchmaCols = window.angular.fromJson(resultSchemaJsonString);


                    Object.assign(baseSchema, arrRsltSchmaCols);
                }

                setDefaultValues(baseSchema, userSettings, userPermissions);
                baseSchema.forEach(translate.bind(null, lang));

                return baseSchema;
            }
        };

        function setDefaultValues(baseSchema, userSettings, userPermissions) {


            window.$.grep(baseSchema,
                function(b, index) {


                    if (b.dataField === "actionImage") {
                        b.caption = "";
                        b.width = 30;
                        b.cellTemplate = function(container, options) {
                            if (typeof options !== "undefined" && options !== null && options.data !== null) {
                                window.$("<div class='" + options.data.resultState + "'>&nbsp;</div>").appendTo(container);
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
                        b.allowFiltering = false;
                        b.format = userSettings.dateFormatShort;
                        b.sortOrder = "desc";
                        baseSchema[index] = b;
                        b.cellTemplate = function(container, options) {
                            if (typeof options !== "undefined" && options !== null && options.data !== null) {
                                const stillUtc = window.moment.utc(options.data.resultDate).toDate();

                                const localDate = window.moment(stillUtc).local().format("l");
                                const localTime = window.moment(stillUtc).local().format("LT");

                                window.$("<div>" + localDate + " " + localTime + "</div>").appendTo(container);
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
                                    return window.toTitleCase(lang.repeatReading.caption);
                                } else if (data.testType === "reTested") {
                                    return window.toTitleCase(lang.reTested.caption);
                                } else if (data.testType === "reTestResult") {
                                    return window.toTitleCase(lang.reTestResult.caption);
                                } else {
                                    return window.toTitleCase(lang.normal.caption);
                                }
                            }
                            return window.toTitleCase(lang.repeatReading.caption);
                        };
                        baseSchema[index] = b;
                    }
                    if (b.dataField === "resultState") {

                        b.calculateCellValue = function(data) {
                            if (typeof data !== "undefined" && data !== null) {
                                if (data.resultState === "pass") {
                                    return window.toTitleCase(lang.pass.caption);
                                } else if (data.resultState === "fail") {
                                    return window.toTitleCase(lang.fail.caption);
                                } else {
                                    return window.toTitleCase(lang.caution.caption);
                                }
                            }
                            return window.toTitleCase(lang.pass.caption);
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