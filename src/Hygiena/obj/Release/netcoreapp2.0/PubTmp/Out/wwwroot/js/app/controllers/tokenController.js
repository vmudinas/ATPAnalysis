app.controller("tokenController",
    function ($scope, tokenService, userSettings) {

        $scope.title = "Token Generator";  
        $scope.tokenColumns = [
            {
                caption: "Token", // $scope.language.resultDate.caption.toCamelCase,
                dataField: "tokens"
            },
            {
                caption: "Expires On", // $scope.language.rLU.caption.toCamelCase,
                dataField: "createdDate",
                format : userSettings.dateFormatShort,
                cellTemplate : function (container, options) {
                   if (typeof options !== "undefined" && options !== null && options.data !== null) {
                        const stillUtc = window.moment.utc(options.data.createdDate).toDate();

                        const localDate = window.moment(stillUtc).local().format("l");
                        const localTime = window.moment(stillUtc).local().format("LT");

                        window.$("<div>" + localDate + " " + localTime + "</div>").appendTo(container);
                   }
                }
            }
        ];

        $scope.tokenGridOptions = {
            bindingOptions: {
                dataSource: "tokenSources",
                columns: "tokenColumns"
            },           
            showBorders: true,
            showColumnLines: true,
            showRowLines: true,
            rowAlternationEnabled: true,
            loadPanel: {
                enabled: false
            },        
            height: function () {
                return window.innerHeight / 1.5;
            },
            hoverStateEnabled: true
        };


        init();

        function init() {
            tokenService.getTokens()
                .then(function (response) {

                    $scope.tokenSources = response.data;                 

                    },
                    function(response) {

                        console.log(response);
                    }
                );


        }

        $scope.generateToken = function ()
        {
            tokenService.generateToken()
                .then(function (response) {

                    init();

                },
                function (response) {

                    console.log(response);
                }
                );

        };
    });