callWithJQuery = (pivotModule) ->
    if typeof exports is "object" and typeof module is "object" # CommonJS
        pivotModule require("jquery")
    else if typeof define is "function" and define.amd # AMD
        define ["jquery"], pivotModule
    # Plain browser env
    else
        pivotModule jQuery
        
callWithJQuery ($) ->

    makeGoogleChart = (chartType, extraOptions) -> (pivotData, opts) ->
        defaults =
            localeStrings:
                vs: "vs"
                by: "by"
            gchart: {}

        opts = $.extend true, defaults, opts
        opts.gchart.width ?= window.innerWidth / 1.4
        opts.gchart.height ?= window.innerHeight / 1.4

        rowKeys = pivotData.getRowKeys()
        rowKeys.push [] if rowKeys.length == 0
        colKeys = pivotData.getColKeys()
        colKeys.push [] if colKeys.length == 0
        fullAggName = pivotData.aggregatorName 
        if pivotData.valAttrs.length
            fullAggName += "(#{pivotData.valAttrs.join(", ")})"
        headers = (h.join("-") for h in rowKeys)
        headers.unshift ""

        numCharsInHAxis = 0
        if chartType == "ScatterChart"
            dataArray = []
            for y, tree2 of pivotData.tree
                for x, agg of tree2
                     dataArray.push [
                        parseFloat(x),
                        parseFloat(y),
                        fullAggName+": \n"+agg.format(agg.value())
                        ]
            dataTable = new google.visualization.DataTable()
            dataTable.addColumn 'number', pivotData.colAttrs.join("-")
            dataTable.addColumn 'number', pivotData.rowAttrs.join("-") 
            dataTable.addColumn type: "string", role: "tooltip"
            dataTable.addRows dataArray
            hAxisTitle = pivotData.colAttrs.join("-")
            vAxisTitle = pivotData.rowAttrs.join("-")
            title = ""
        else
            dataArray = [headers]
            for colKey in colKeys
                row = [colKey.join("-")]
                numCharsInHAxis += row[0].length
                for rowKey in rowKeys
                    agg = pivotData.getAggregator(rowKey, colKey)
                    if agg.value()?
                        val = agg.value()
                        if $.isNumeric val
                            if val < 1
                                row.push parseFloat(val.toPrecision(3))
                            else
                                row.push parseFloat(val.toFixed(3))
                        else
                            row.push val
                    else row.push null
                dataArray.push row

            dataTable = google.visualization.arrayToDataTable(dataArray)

            title = vAxisTitle = fullAggName
            hAxisTitle = pivotData.colAttrs.join("-")
            title += " #{opts.localeStrings.vs} #{hAxisTitle}" if hAxisTitle != ""
            groupByTitle = pivotData.rowAttrs.join("-")
            title += " #{opts.localeStrings.by} #{groupByTitle}" if groupByTitle != ""

        options = 
            title: title
            hAxis: {title: hAxisTitle, slantedText: numCharsInHAxis > 50}
            vAxis: {title: vAxisTitle}
            legend: {position: 'right'}
            width: "100%"
            height: "100%"
            colors: ['#84B2CC', '#0099C6','#FF9900','#109618','#990099','#3B3EAC','#DD4477','#66AA00','#B82E2E','#316395','#994499','#22AA99','#AAAA11','#E67300','#8B0707','#329262','#5574A6',],
            tooltip: { 
                textStyle:{ 
                    fontSize: 12
                }
            }
            textStyle:{ 
                fontSize: 10
            }

        if chartType == "ColumnChart"
            options.vAxis.minValue = 0

        if chartType == "ScatterChart"
            options.legend = position: "none"
            options.chartArea = {'width': '100%', 'height': '100%'}

        if chartType == "LineChart"
            options.chartArea = {
             'width': '80%',
             'left':'10%',
             'height': '70%',
             'top':'5%'
            }
            options.width = "100%"

        if chartType == "PieChart"
            options.legend = {position: 'right'};
            options.chartArea = {
                'width': '90%',
                'height': '90%'
            }
            dataTable.setColumnLabel(0, pivotData.colAttrs.join("-"))
            dataTable.setColumnLabel(1, pivotData.valAttrs.join("-"))

        if chartType == "BarChart"
            options.legend = 'right';
            options.chartArea = {
                'width': '70%',
                'left':'15%',
                'height': '90%'
            }
            options.vAxis = {
                'title': hAxisTitle,
                'slantedText': numCharsInHAxis > 50
            }
            options.hAxis = {
                'title': vAxisTitle
            }

            if not extraOptions.isStacked

                for own i, dataA of dataArray
                    if i > 0
                        dataArray[i][1] = parseFloat(parseFloat(dataArray[i][1]*100).toFixed(2))
                        dataArray[i].push(dataArray[i][1] + "%")
                    else
                        dataArray[0].push({ role: 'annotation' })
                
            dataTable = google.visualization.arrayToDataTable(dataArray);

        if chartType == "AreaChart"
            options.chartArea = {
                'width': '70%',
                'left':'5%',
                'height': '70%',
                'top':'5%'
            }
            options.width = "100%"
            options.legend = {position: "right"}
            options.isStacked = true

        if chartType == "GeoChart"
            options.legend = {textStyle: {color: '#3E718E', fontSize: 12}}
            console.log(extraOptions)
            options.region = extraOptions.region
            if options.region == 'BR'
                options.resolution = 'provinces'
            options.displayMode = 'regions'
            options.colors = ["#3E718E"]
            options.colorAxis = {colors:['#F5F5F5','#3E718E'], minValue: 0}

            dataTable.setColumnLabel(0, pivotData.colAttrs.join("-"))
            dataTable.setColumnLabel(0, pivotData.valAttrs.join("-"))
        
        

        else if dataArray[0].length == 2 and dataArray[0][1] ==  ""
            options.legend = position: "none"


        
        $.extend options, opts.gchart, extraOptions

        result = $("<div>").css(height: "100%")

        if chartType != "BarChart"
            result = $("<div>").css({
              width: "100%",
              height: "100%",
              padding: "0 !important"
            })
        dataTable.setColumnLabel(0, pivotData.colAttrs.join("-"))
        
        if chartType == "LineChart" || chartType == "AreaChart" 
            result = $("<div>").css({
              width: "100%",
              height: "100%",
              padding: "0 !important"
            })

        wrapper = new google.visualization.ChartWrapper {dataTable, chartType, options}
        wrapper.draw(result[0])    
        result.bind "dblclick", -> 
            editor = new google.visualization.ChartEditor()
            google.visualization.events.addListener editor, 'ok', -> 
                editor.getChartWrapper().draw(result[0])
            editor.openDialog(wrapper)
        return result

    $.pivotUtilities.gchart_renderers = 
        "Line Chart": makeGoogleChart("LineChart")
        "Pie Chart": makeGoogleChart("PieChart")
        "Bar Chart": makeGoogleChart("BarChart")
        "Stacked Bar Chart": makeGoogleChart("BarChart", isStacked: true)
        "Column Chart": makeGoogleChart("ColumnChart")
        "Stacked Column Chart": makeGoogleChart("ColumnChart", isStacked: true)
        "Area Chart": makeGoogleChart("AreaChart", isStacked: true)
        "Scatter Chart": makeGoogleChart("ScatterChart")
        "Geographic Chart - Brazil": makeGoogleChart("GeoChart", {region: 'BR'}, false),
        "Geographic Chart - South America": makeGoogleChart("GeoChart", {region: '005'}, false),
        "Geographic Chart - World": makeGoogleChart("GeoChart", {region: 'world'}, false),
