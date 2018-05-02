﻿/**
* jQuery ligerUI 1.3.2
* 
* http://ligerui.com
*  
* Author daomi 2015 [ gd_star@163.com ] 
* 
*/
(function ($)
{

    $.fn.ligerToolBar = function (options)
    {
        return $.ligerui.run.call(this, "ligerToolBar", arguments);
    };

    $.fn.ligerGetToolBarManager = function ()
    {
        return $.ligerui.run.call(this, "ligerGetToolBarManager", arguments);
    };

    $.ligerDefaults.ToolBar = {};

    $.ligerMethos.ToolBar = {};

    $.ligerui.controls.ToolBar = function (element, options)
    {
        $.ligerui.controls.ToolBar.base.constructor.call(this, element, options);
    };
    $.ligerui.controls.ToolBar.ligerExtend($.ligerui.core.UIComponent, {
        __getType: function ()
        {
            return 'ToolBar';
        },
        __idPrev: function ()
        {
            return 'ToolBar';
        },
        _extendMethods: function ()
        {
            return $.ligerMethos.ToolBar;
        },
        _render: function ()
        {
            var g = this, p = this.options;
			g.toolbarItemCount = 0;
            g.toolBar = $(this.element);
            g.toolBar.addClass("l-toolbar");
            g.set(p);
        },
        _setItems: function (items)
        {
            var g = this;
            g.toolBar.html("");
            $(items).each(function (i, item)
            {
                g.addItem(item);
            });
        },
		removeItem: function (itemid)
        {
            var g = this, p = this.options;
            $("> .l-toolbar-item[toolbarid=" + itemid + "]", g.toolBar).remove();
		},
		removeAll: function () {
		    var g = this, p = this.options;

		    var idlist = g.getItemIdList();
           
		    $(idlist).each(function (i, item) {                
		        g.removeItem(this);
		    })
		},
		getItemIdList: function ()
		{
		    var g = this, p = this.options;
		    var itemidlist = [];
		    $(".l-toolbar-item", g.toolBar).each(function () {
		        if ($(this).attr("toolbarid"))
		            itemidlist.push($(this).attr("toolbarid"));
		    });
		    return itemidlist;
		},
        setEnabled: function (itemid)
        {
            var g = this, p = this.options;
            $("> .l-toolbar-item[toolbarid=" + itemid + "]", g.toolBar).removeClass("l-toolbar-item-disable");
        },
        setDisabled: function (itemid)
        {
            var g = this, p = this.options;
            $("> .l-toolbar-item[toolbarid=" + itemid + "]", g.toolBar).addClass("l-toolbar-item-disable");
        },
        isEnable: function (itemid)
        {
            var g = this, p = this.options;
            return !$("> .l-toolbar-item[toolbarid=" + itemid + "]", g.toolBar).hasClass("l-toolbar-item-disable");
        },
        addItem: function (item)
        {
            var g = this, p = this.options;
            if (item.type == "line") {
                g.toolBar.append('<div class="l-bar-separator"></div>');
                return;
            }
            else if (item.type == "textbox") {
                if (item.text)
                    g.toolBar.append('<div class="l-toolbar-item  l-panel-label">' + item.text + '</div>');
                g.toolBar.append('<div class="l-toolbar-item"><input type="text" id="' + item.id + '" name="' + item.id + '" style="width:' + item.width + 'px;"></div>');
                return;
            }
            else if (item.type == "text") {
                if (item.text)
                    g.toolBar.append('<div class="l-toolbar-item  l-panel-label">' + item.text + '</div>');
                return;
            }
            else if (item.type == "filter") {
                var ditem = $('<div class="l-toolbar-filter"><span></span><div class="l-panel-btn-l"></div><div class="l-panel-btn-r"></div></div>');
                if (item.icon) {
                    ditem.append("<div class='l-icon'></div>");
                    //ditem.css("background", "url(" + item.icon + ") no-repeat 3px 3px");
                    $(".l-icon", ditem).css({ "background": "url(" + item.icon + ") no-repeat 1px 3px", width: "18px", height: "18px" });
                }

                ditem.hover(function () {
                    $(this).addClass("l-panel-btn-over");
                    $(this).attr("title", item.title)
                }, function () {
                    $(this).removeClass("l-panel-btn-over");
                });

                g.toolBar.filter.append(ditem);
                item.click && ditem.click(function () { item.click(item); });

                return;
            }
            var ditem = $('<div class="l-toolbar-item l-panel-btn"><span></span><div class="l-panel-btn-l"></div><div class="l-panel-btn-r"></div></div>');
            g.toolBar.append(ditem);
            item.id && ditem.attr("toolbarid", item.id);
            if (item.icon) {
                ditem.append("<div class='l-icon'></div>");
                //ditem.css("background", "url(" + item.icon + ") no-repeat 3px 3px");
                $(".l-icon", ditem).css({ "background": "url(" + item.icon + ") no-repeat 1px 1px", width: "18px", height: "18px" });
                ditem.addClass("l-toolbar-item-hasicon");
            }
            item.text && $("span:first", ditem).html(item.text);
            item.expid && $("span:first", ditem).attr("id", item.expid);

            if (!item.disable) {
                ditem.addClass("l-toolbar-item-disable");
                ditem.attr("disabled", true)
            }
            else {
                item.click && ditem.click(function () { item.click(item); });

                if (item.type == "button") {
                    ditem.hover(function () {
                        $(this).addClass("l-panel-btn-over");
                    }, function () {
                        $(this).removeClass("l-panel-btn-over");
                    });
                }

            }
            if (item.type == "serchbtn") {
                ditem.hover(function () {
                    $(this).addClass("l-panel-btn-over");
                }, function () {
                    $(this).removeClass("l-panel-btn-over");
                }).click(function () {
                    var serchpanel = $(".az");
                    serchpanel.css({ 'border-bottom': 'solid 1px #8DB2E3', 'border-top': 'solid 1px #8DB2E3' });
                    serchpanel.css('top', ditem.offset().top + ditem.height() + 4);

                    serchpanel.appendTo($(document.body));
                    if (serchpanel.css('display') == 'none') {
                        $(this).addClass("l-panel-btn-selected");
                        serchpanel.fadeIn(100)
                    }
                    else {
                        $(this).removeClass("l-panel-btn-selected");
                        serchpanel.fadeOut(100)
                    }
                    //this.click;
                })
            }
        }
    });
	//旧写法保留
    $.ligerui.controls.ToolBar.prototype.setEnable = $.ligerui.controls.ToolBar.prototype.setEnabled;
    $.ligerui.controls.ToolBar.prototype.setDisable = $.ligerui.controls.ToolBar.prototype.setDisabled;
})(jQuery);