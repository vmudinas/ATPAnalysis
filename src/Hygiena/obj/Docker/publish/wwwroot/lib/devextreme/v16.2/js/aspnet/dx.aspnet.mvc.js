(function($, DX) {

    var ui = DX.ui;
    
    var outerHtml = function(element) {
        element = $(element);

        var templateTag = element.length && element[0].nodeName.toLowerCase();
        if(templateTag === "script") {
            return element.html();
        } else {
            element = $("<div>").append(element);
            return element.html();
        }
    };

    var compileTemplate = function(content) {

        var patterns = {
            evaluate: /<%([\s\S]+?)%>/g,
            interpolate: /<%=([\s\S]+?)%>/g,
            escape: /<%-([\s\S]+?)%>/g
        };

        var matcher = RegExp([
            patterns.escape.source,
            patterns.interpolate.source,
            patterns.evaluate.source
        ].join('|') + '|$', 'g');

        var escapeRegExp = /\\|'|\r|\n|\u2028|\u2029/g;

        var escapes = {
            "'": "'",
            "\\": "\\",
            "\r": "r",
            "\n": "n",
            "\u2028": "u2028",
            "\u2029": "u2029"
        };

        var escapeChar = function(match) {
            return "\\" + escapes[match];
        };

        var compileTmplSource = function(content) {
            var index = 0;
            var source = "_res_+='";
            content.replace(matcher, function(match, escape, interpolate, evaluate, offset) {
                source += content.slice(index, offset).replace(escapeRegExp, escapeChar);
                index = offset + match.length;
                
                if(escape) {
                    source += "'+\n((_tmp_=(" + escape + "))==null?'':DevExpress.AspNet.escape(_tmp_))+\n'";
                } 
                else if(interpolate) {
                    source += "'+\n((_tmp_=(" + interpolate + "))==null?'':_tmp_)+\n'";
                }
                else if(evaluate) {
                    source += "';\n" + evaluate + "\n_res_+='";
                }

                return match;
            });

            source += "';\n";

            source = "with(obj||{}){\n" + source + "}\n";
            source = "var _tmp_,_res_='';\n" + source + "return _res_;\n";
            
            return source;
        };

        var process = function(content) {
            var source = compileTmplSource(content),
                render;

            try {
                render = new Function("obj", source);
            } catch (e) {
                e.source = source;
                throw e;
            }

            var template = function(data) {
                return render.call(this, data);
            };

            template.source = "function(obj){\n" + source + "}";

            return template;
        };

        DX.AspNet.escape = function(string) {
            string = string === null ? "" : "" + string;
            var escapeMap = {
                "&": "&amp;",
                "<": "&lt;",
                ">": "&gt;",
                '"': "&quot;",
                "'": "&#x27;",
                "`": "&#x60;"
            };
            var escapeMapPattern = "(?:" + Object.keys(escapeMap).join("|") + ")";
            
            if(RegExp(escapeMapPattern).test(string)) {
                return string.replace(RegExp(escapeMapPattern, "g"), function(match) {
                    return escapeMap[match];
                });
            }
            return string;
        };

        return process(content);
    };

    $.extend(DX, {
        AspNet: {
            renderComponent: function(name, options, id) {
                id = id || ("dx-" + new DX.data.Guid());
                var templateRendered = ui.templateRendered;

                var render = function(_, container) {
                    $("#" + id, container)[name](options);
                    templateRendered.remove(render);
                };

                templateRendered.add(render);

                return "<div id=\"" + id + "\"></div>";
            },
			
			getComparisonTargetValue: function(name) {
				var $widget = $("input[name='" + name + "']").closest(".dx-widget");
				if($widget.length) {
					var dxComponents = $widget.data("dxComponents"),
						widget = $widget.data(dxComponents[0]);

					if(widget) {
						return widget.option("value");
                    }                                            
                }
			},

            template: function(content) {
                return compileTemplate(content);
            }
        }
    });

    ui.setTemplateEngine({
        compile: function(element) {
            return DX.AspNet.template(outerHtml(element));
        },
        render: function(template, data) {
            return template(data);
        }
    });

})(jQuery, DevExpress);