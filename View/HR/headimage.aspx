<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="../CSS/input.css" rel="stylesheet" />

    <title></title>

    <link rel="stylesheet" href="../css/bootstrap.min.css">
    <link rel="stylesheet" href="../css/cropper.css">
    <link rel="stylesheet" href="../css/headimg.css">
    <link href="../CSS/font-awesome.css" rel="stylesheet" />

    <script type="text/javascript" src="../lib/jquery/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="../js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/cropper.min.js"></script>

    <script type="text/javascript">
        var $image;
        $(function () {
            // Import image
            $image = $('#image');
            var $inputImage = $('#fileInput');
            var URL = window.URL || window.webkitURL;
            var blobURL;

            if (URL) {
                $inputImage.change(function () {
                    var files = this.files;
                    var file;

                    if (!$image.data('cropper')) {
                        return;
                    }

                    if (files && files.length) {
                        file = files[0];

                        if (/^image\/\w+$/.test(file.type)) {
                            blobURL = URL.createObjectURL(file);
                            $image.one('built.cropper', function () {

                                // Revoke when load complete
                                URL.revokeObjectURL(blobURL);
                            }).cropper('reset').cropper('replace', blobURL);
                            $inputImage.val('');
                        } else {
                            window.alert('Please choose an image file.');
                        }
                    }
                });
            } else {
                //$inputImage.prop('disabled', true).parent().addClass('disabled');
                alert();
            }

            var $previews = $('.preview');

            $image.cropper({
                preview: '.preview',
                dragMode: 'move',
                aspectRatio: 1,

                build: function (e) {
                    var $clone = $(this).clone();

                    $clone.css({
                        display: 'block',
                        width: '100%',
                        minWidth: 0,
                        minHeight: 0,
                        maxWidth: 'none',
                        maxHeight: 'none'
                    });

                    $previews.css({
                        width: '100%',
                        overflow: 'hidden'
                    }).html($clone);
                },

                crop: function (e) {
                    var imageData = $(this).cropper('getImageData');
                    //var previewAspectRatio = 1;

                    //$previews.each(function () {
                    //    var $preview = $(this);
                    //    var previewWidth = $preview.width();
                    //    var previewHeight = previewWidth / previewAspectRatio;
                    //    var imageScaledRatio = e.width / previewWidth;

                    //    $preview.height(previewHeight).find('img').css({
                    //        width: imageData.naturalWidth / imageScaledRatio,
                    //        height: imageData.naturalHeight / imageScaledRatio,
                    //        marginLeft: -e.x / imageScaledRatio,
                    //        marginTop: -e.y / imageScaledRatio
                    //    });
                    //});
                }
            });
        });

        function f_save() {
            var $imgData = $image.cropper('getCroppedCanvas', {
                width: 120,
                height: 120
            });

            var dataurl = $imgData.toDataURL('image/png');  //dataurl便是base64图片
            console.log(dataurl)

            //$("#aaa").attr("src", dataurl);
          
            return "base64img=" + dataurl;
        }
    </script>
    <style type="text/css">
        fieldset { padding: 8px; }
        legend { font-size: 12px; margin-left: 15px; }
        body { font-size: 12px; }

        input { position: absolute; right: 0; top: 0; font-size: 100px; opacity: 0; filter: alpha(opacity=0); }
        .fileInput { position: absolute; left: 0; top: 0; height: 30px; filter: alpha(opacity=0); opacity: 0; background-color: transparent; cursor: pointer; }
        .btn { width: 200px; height: 30px; margin: 10px; background-color: yellow; text-align: center; line-height: 30px; overflow: hidden; display: block; position: relative; box-shadow: 0 0 5px rgba(0,0,0,0.3); border-radius: 3px; text-shadow: 1px 1px 1px #fff; }
    </style>
</head>
<body>

  <!-- Content -->
  <div class="container" style="padding-top:5px;">
      <div class="col-xs-9" style="margin:5px 0px;">
          <a href="#" class="uploada">
            <input type="file" class="fileInput" id="fileInput" name="fileInput" />
            浏  览
        </a>
          </div>
    <div class="row">
      <div class="col-xs-9">
        <!-- <h3 class="page-header">Demo:</h3> -->
        <div class="img-container">
          <img id="image" src="../images/noheaderlarger.png" alt="Picture">
        </div>
      </div>
      <div class="col-xs-3">
        <!-- <h3 class="page-header">Preview:</h3> -->
        <div class="docs-preview clearfix">
          <div class="preview preview-lg"></div>
        </div>
          <%--<img id="aaa" />--%>
        <!-- <h3 class="page-header">Data:</h3> -->
        <div class="docs-data" style="line-height:28px;">
           <br />
                <b>操作步骤：</b><br />
                1、点击【浏览】按钮。<br />
                2、拖动选择框至合适位置，并选择合适大小，右边会有预览。<br />
                3、点击保存。
        </div>
      </div>
    </div>
 
  </div>

</body>
</html>
