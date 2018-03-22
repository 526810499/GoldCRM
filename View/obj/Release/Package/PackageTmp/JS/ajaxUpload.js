/*2014年9月19日11:11:07 By 王美建*/

function ajaxUpload(opt) {
    /*
        参数说明:
        opt.id : 页面里file控件的ID;
        opt.frameName : iframe的name值;
        opt.url : 文件要提交到的地址;
        opt.format : 文件格式，以数组的形式传递，如['jpg','png','gif','bmp'];
        opt.onsuccess : 上传成功后回调;
        opt.onerror : 上传成功后回调;
    */
    var iName = opt.frameName; //太长了，变短点
    var iframe, form, file, fileParent;
    //创建iframe和form表单
    iframe = $('<iframe name="' + iName + '" style = "width:0;height:0;" />');
    form = $('<form method="post" style="display:none;" target="' + iName + '" action="' + opt.url + '"  name="form_' + iName + '" enctype="multipart/form-data" />');
    file = $('#' + opt.id); //通过id获取flie控件
    fileParent = file.parent(); //存父级
    file.appendTo(form);
    //插入body
    $(document.body).append(iframe).append(form);

    //取得所选文件的扩展名
    var fileFormat = /\.[a-zA-Z]+$/.exec(file.val())[0].substring(1).toLowerCase();
    if (opt.format.join('-').indexOf(fileFormat) != -1) {
        form.submit();//格式通过验证后提交表单;
    } else {
        file.appendTo(fileParent); //将file控件放回到页面
        iframe.remove();
        form.remove();
        //alert('文件格式错误，请重新选择！');
        opt.onerror('文件格式错误，请重新选择！')
    };

    //文件提交完后
    iframe.load(function () {
        var data = $(this).contents().find('body').html();
        file.appendTo(fileParent);
        iframe.remove();
        form.remove();
        opt.onsuccess(data);
    })
}