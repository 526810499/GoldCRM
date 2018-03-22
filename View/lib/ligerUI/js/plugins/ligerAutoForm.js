/**
* jQuery ligerUI 1.1.1 up by xhdcrm 
* 
* Author leoxie [ gd_star@163.com ] 
* 
*/

(function ($) {
    $.ligerui = $.ligerui || {};
    $.ligerui.addManager = function (dom, manager) {
        if (dom.id == undefined || dom.id == "")
            dom.id = "ligerui" + (1000 + $.ligerui.ManagerCount);
        $.ligerui.ManagerCount++;
        $.ligerui.Managers[dom.id] = manager;
        dom.applyligerui = true;
    };
    $.ligerui.getManager = function (domArr) {
        if (domArr.length == 0) return null;
        return $.ligerui.Managers[domArr[0].id];
    };
    $.ligerui.Managers = $.ligerui.Managers || {};
    $.ligerui.ManagerCount = $.ligerui.ManagerCount || 0;

    $.fn.ligerGetFormManager = function () {
        return $.ligerui.getManager(this);
    };

    $.ligerDefaults = $.ligerDefaults || {};
    $.ligerDefaults.Form = {
        inputWidth: 180,//控件宽度        
        labelWidth: 90,//标签宽度        
        space: 40,//间隔宽度
        rightToken: '：',
        labelAlign: 'left', //标签对齐方式        
        align: 'left',//控件对齐方式        
        fields: [],//字段        
        appendID: true,//创建的表单元素是否附加ID        
        prefixID: "",//生成表单元素ID的前缀       
        toJSON: $.ligerui.toJSON //json解析函数
    };

    $.ligerManagers = $.ligerManagers || {};
    $.ligerManagers.Form = function (options, po) {
        this.options = options;
        this.po = po;
    };

    $.ligerManagers.Form.prototype = {
        getType: function () {
            return 'Form';
        },
        idPrev: function () {
            return 'Form';
        },
        render: function (form) {
            var g = this, p = this.options;

            var out = [];

            if (p.fields && p.fields.length) {
                $(p.fields).each(function (i, item) {
                    if (item.type == "group") {
                        out.push('<fieldset class="l-group">');
                        if (item.display) {
                            out.push('<legend >' + item.display + '</legend>')
                            //out.push('<div class="l-group-header"><span>' + item.display + '</span></div>');
                        }
                        var dom = g.buildDom(item.rows);
                        out.push(dom);
                        out.push('</fieldset>')
                    }
                })
            }
            form.append(out.join(''));
            form.ligerForm();
        },
        buildDom: function (field) {
            var g = this, p = this.options;
            var out = [];
            $(field).each(function (i, rows) {
                out.push('<dl>');
                $(rows).each(function (i, item) {
                    var label = item.label || item.display;
                    var labelWidth = item.labelWidth || item.labelwidth || p.labelWidth;
                    if (item.type == "icon") labelWidth = 0;
                    var labelAlign = item.labelAlign || p.labelAlign;
                    if (label) label += p.rightToken;

                    //label
                    if (item.type != "html" && item.type != "hidden") {
                        out.push('<dt style="');
                        if (labelWidth) {
                            out.push('width:' + labelWidth + 'px;');
                        }
                        if (labelAlign) {
                            out.push('text-align:' + labelAlign + ';');
                        }
                        out.push('">');
                        if (label) {
                            out.push(label);
                        }
                        out.push('</dt>');
                    }
                    else if (item.type == "html") {
                        out.push('<dt style="');
                        if (item.width) {
                            out.push('width:' + item.width + 'px;');
                        }

                        out.push('">');
                        if (item.html) {
                            out.push(item.html);
                        }
                        out.push('</dt>');
                    }


                    //input
                    var width = item.width || p.inputWidth;
                    if (item.type == "icon") width = 18;
                    if (item.type == "hidden") width = 1;
                    var align = item.align || item.textAlign || item.textalign || p.align;
                    if (item.type != "html" && item.type != "hidden") {
                        out.push('<dt style="');
                        if (width) {
                            out.push('width:' + (width + 5) + 'px;');
                        }

                        if (align) {
                            out.push('text-align:' + align + ';');
                        }
                        out.push('">');
                        out.push(g.buildInput(item));

                        out.push('</dt>');
                        out.push(g.buildSpace(item));
                    }

                });
                out.push('</dl>');
            })
            return out.join('');
        },
        buildInput: function (field) {
            var g = this, p = this.options;
            var width = field.width || p.inputWidth;
            var name = field.name || field.id;
            var out = [];
            if (field.type == "hidden") {
                out.push('<input type="hidden" id="' + name + '" name="' + name + '" />');
                return out.join('');
            }
            if (field.type == "html") {
                out.push(field.content);
                return out.join('');
            }
            if (field.textarea || field.type == "textarea") {
                out.push('<textarea ');
                field.cols && out.push('cols="' + field.cols + '" ');
                field.rows && out.push('rows="' + field.rows + '" ');
            }
            else if (field.type == "checkbox") {
                out.push('<input type="checkbox" style="margin-top:2px;" ');
            }
            else if (field.type == "radio") {
                out.push('<input type="radio" ');
            }
            else if (field.type == "password") {
                out.push('<input type="password" ');
            }
            else {
                out.push('<input type="text" ');
            }
            if (field.cssClass) {
                out.push('class="' + field.cssClass + '" ');
            }
            if (field.type) {
                out.push('ltype="' + field.type + '" ');
            }
            if (field.comboboxName && field.type == "select") {
                out.push('name="' + field.comboboxName + '"');
                if (p.appendID) {
                    out.push(' id="' + p.prefixID + field.comboboxName + '" ');
                }
            }
            else {
                out.push('name="' + name + '"');
                if (p.appendID) {
                    out.push(' id="' + name + '" ');
                }
            }
            //参数
            var fieldOptions = $.extend({
                width: width - 2
            }, field.options || {});

            if (field.options)
                out.push(" ligerui=\"" + field.options + "\" ");
            //验证参数
            if (field.validate) {
                out.push(" validate=\"" + field.validate + "\" ");
            }
            if (field.type == "textarea") {
                out.push(">");
                if (field.initValue)
                    out.push(field.initValue);
                out.push("</textarea>");
            }
            else {
                if (field.initValue)
                    out.push(" value = \"" + field.initValue + "\"");
                out.push(' />');
            }

            return out.join('');
        },
        buildSpace: function (field) {
            var g = this, p = this.options;
            var spaceWidth = field.space || field.spaceWidth || p.space;
            var out = [];
            out.push('<dt style="color:red;');
            if (spaceWidth) {
                out.push('width:' + spaceWidth + 'px;');
            }
            out.push('">');
            if (field.validate) {
                var p = field.validate;
                if (p.indexOf('{') != 0) p = "{" + p + "}";
                eval("p = " + p + ";");
                if (p.required)
                    out.push('*');
            }
            out.push('</dt>');
            return out.join('');
        }
    }


    ///	<param name="$" type="jQuery"></param>
    $.fn.ligerAutoForm = function (p) {
        this.each(function () {
            p = $.extend({}, $.ligerDefaults.Form, p || {});
            var po = {};
            var g = new $.ligerManagers.Form(p, po);

            g.render($(this));

            $.ligerui.addManager(this, g);
        })
        return $.ligerui.getManager(this);
    };
    $.fn.ligerForm = function (p) {
        p = $.extend({}, $.ligerDefaults.Form, p || {});
        return this.each(function () {
            $("input[ltype=text],input[ltype=password]", this).ligerTextBox();

            $("input[ltype=select],select[ltype=select]", this).ligerComboBox();

            $("input[ltype=spinner]", this).ligerSpinner();

            $("input[ltype=date]", this).ligerDateEditor();

            //$("input[ltype=radio]", this).ligerRadio();

            //$('input[ltype=checkbox]', this).ligerCheckBox();
        });
    }

})(jQuery);