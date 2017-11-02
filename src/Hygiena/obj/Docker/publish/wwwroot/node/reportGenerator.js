"use strict";
module.exports = function (callback, data, chartTitle) {
    
    var refreshButton = "<button type='button' class='btn btn-default' onclick='location.reload();'>Close</button>";

    var printButton = "<button type='button' class='btn btn-default' onclick='window.print();'>Print</button>";

    var html = "<div id='genReport' ng-controller='reportsController' class='.table-responsive'><div class='row'>" + printButton + refreshButton + "</div><div class='row'><div id='dataviz-container' ></div></div></div>";
    var d3 = require('d3')

    const jsdom = require("jsdom");
    const { JSDOM } = jsdom;
    const dom = new JSDOM( html);

    var el = dom.window.document.querySelector('#dataviz-container')

            var w = 400;
            var h = 400;
            var r = h / 2;
            var aColor = [
                'red',
                'green',
                'orange'
            ]


            for (var i = 0; i < 7; i++)
            {


                var vis = d3.select(el).append("svg:svg").data([data]).attr("width", w).attr("height", h)
                    .append("svg:g").attr("transform", "translate(" + r + "," + r + ")");

                var pie = d3.layout.pie().value(function(d) { return d.value; });

                // Declare an arc generator function
                var arc = d3.svg.arc().outerRadius(r);

    //            // Select paths, use arc generator to draw
                var arcs = vis.selectAll("g.slice").data(pie).enter().append("svg:g").attr("class", "slice");
                arcs.append("svg:path")
                    .attr("fill", function(d, i) { return aColor[i]; })
                    .attr("d", function(d) { return arc(d); });
                // add the text
                arcs.append("svg:text").attr("transform",
                    function(d) {
                        d.innerRadius = 0;
                        d.outerRadius = r;
                        return "translate(" + arc.centroid(d) + ")";
                    }).attr("text-anchor", "middle").text(function(d, i) {
                       return data[i].label;
                    }
                );

            }

    // pass the html stub to jsDom
    var greet = function (body) {

    

        return body;

    };

    callback(null, greet(dom.window.document.querySelector("#genReport").outerHTML));
    //jsdom.env(htmlStub , function (errors, window) {
    //        // process the html document, like if we were at client side
    //        // code to generate the dataviz and process the resulting html file to be added here
    //        var el = window.document.querySelector('#dataviz-container')

    //        var w = 400;
    //        var h = 400;
    //        var r = h / 2;
    //        var aColor = [
    //            'red',
    //            'green',
    //            'orange'
    //        ]

          
    //        for (var i = 0; i < 7; i++) {


    //            var vis = d3.select(el).append("svg:svg").data([data]).attr("width", w).attr("height", h)
    //                .append("svg:g").attr("transform", "translate(" + r + "," + r + ")");

    //            var pie = d3.layout.pie().value(function(d) { return d.value; });

    //            // Declare an arc generator function
    //            var arc = d3.svg.arc().outerRadius(r);

    //            // Select paths, use arc generator to draw
    //            var arcs = vis.selectAll("g.slice").data(pie).enter().append("svg:g").attr("class", "slice");
    //            arcs.append("svg:path")
    //                .attr("fill", function(d, i) { return aColor[i]; })
    //                .attr("d", function(d) { return arc(d); });
    //            // add the text
    //            arcs.append("svg:text").attr("transform",
    //                function(d) {
    //                    d.innerRadius = 0;
    //                    d.outerRadius = r;
    //                    return "translate(" + arc.centroid(d) + ")";
    //                }).attr("text-anchor", "middle").text(function(d, i) {
    //                    return data[i].label;
    //                }
    //            );

    //        }
      
    //        callback(null, greet(window.document.querySelector('#genReport').outerHTML));

    //    }
//  )
   
}