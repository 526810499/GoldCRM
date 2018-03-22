UE.xpath = "";
// 文本框
UE.plugins['text'] = function () {
    var me = this, thePlugins = 'text';
    me.commands[thePlugins] = {
        execCommand: function () {
            f_openDialog(UE.xpath + 'flow_form_textbox.aspx', "单行文本框", 700, 380, "text", 9005);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var xPlugins = el.getAttribute('xtype');
        if (/input/ig.test(el.tagName) && xPlugins == thePlugins) {
            var html = popup.formatHtml(
				'<nobr>文本框: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//日期
UE.plugins['date'] = function () {
    var me = this, thePlugins = 'date';
    me.commands[thePlugins] = {
        execCommand: function () {
            f_openDialog(UE.xpath + 'flow_form_datetime.aspx', "日期文本框", 700, 380, "date", 9005);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var xPlugins = el.getAttribute('xtype');
        if (/input/ig.test(el.tagName) && xPlugins == thePlugins) {
            var html = popup.formatHtml(
				'<nobr>日期时间: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//下拉
UE.plugins['select'] = function () {
    var me = this, thePlugins = 'select';
    me.commands[thePlugins] = {
        execCommand: function () {
            f_openDialog(UE.xpath + 'flow_form_select.aspx', "下拉列表框", 700, 480, "select", 9005);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var xPlugins = el.getAttribute('xtype');
        if (/input/ig.test(el.tagName) && xPlugins == thePlugins) {
            var html = popup.formatHtml(
				'<nobr>下拉列表: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};

//多行文本
UE.plugins['textarea'] = function () {
    var me = this, thePlugins = 'textarea';
    me.commands[thePlugins] = {
        execCommand: function () {
            f_openDialog(UE.xpath + 'flow_form_textarea.aspx', "多行文本框", 700, 380, "textarea", 9005);
        }
    };
    var popup = new baidu.editor.ui.Popup({
        editor: this,
        content: '',
        className: 'edui-bubble',
        _edittext: function () {
            baidu.editor.plugins[thePlugins].editdom = popup.anchorEl;
            me.execCommand(thePlugins);
            this.hide();
        },
        _delete: function () {
            if (window.confirm('确认删除该控件吗？')) {
                baidu.editor.dom.domUtils.remove(this.anchorEl, false);
            }
            this.hide();
        }
    });
    popup.render();
    me.addListener('mouseover', function (t, evt) {
        evt = evt || window.event;
        var el = evt.target || evt.srcElement;
        var xPlugins = el.getAttribute('xtype');
        if (/textarea/ig.test(el.tagName) && xPlugins == thePlugins) {
            var html = popup.formatHtml(
				'<nobr>多行文本: <span onclick=$$._edittext() class="edui-clickable">编辑</span>&nbsp;&nbsp;<span onclick=$$._delete() class="edui-clickable">删除</span></nobr>');
            if (html) {
                popup.getDom('content').innerHTML = html;
                popup.anchorEl = el;
                popup.showAnchor(popup.anchorEl);
            } else {
                popup.hide();
            }
        }
    });
};


var activeDialog = null;
function f_openDialog(url, title, width, height, type, zindex) {
    var z_index = zindex || 9001;
    var buttons = [];
    buttons.push({ text: '保存', onclick: f_ok });

    if (type)
        buttons.push({
            text: '关闭', onclick: function (item, dialog) {
                if (UE.plugins[type].editdom) {
                    delete UE.plugins[type].editdom;
                }
                dialog.close();
            }
        })

    var dialogOptions = {
        zindex: z_index,
        width: width,
        height: height,
        title: title,
        url: url,
        buttons: buttons,
        isResize: true,
        showToggle: true,
        timeParmName: 'a'
    };
    activeDialog = jQuery.ligerDialog.open(dialogOptions);
}
function f_ok(item, dialog) {
    var issave = dialog.frame.f_save();
    if (issave)
        dialog.close();
}

