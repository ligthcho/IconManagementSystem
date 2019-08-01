window.itop = window.itop || {};
window.itop.WebUploader = window.itop.WebUploader || {};

window.itop.WebUploader.Init = function (pick, callback) {
    console.info('window.itop.WebUploader.Init_pick', pick);
    //加载webuploader脚本
    $('body').append('<script src="/Lib/webuploader/webuploader.min.js"><\/script>');
    $('body').append('<script src="/Lib/webuploader/demo/webuploader.demo.js"><\/script>');
    //初始化加载上传组件
    window.itop.WebUploader.Load(pick, callback);
    if (callback != null) {
        callback();
    }
};

window.itop.WebUploader.Load = function webUploaderInit(pick, callback) {
    //添加上传区域html
    if ($('#' + pick).length == 0) {
        var pickerHtml = '<div id="' + pick + '" style="display:none;">选择图片</div>';
        $('body').append(pickerHtml);
    }

    //上传图片
    // 初始化Web Uploader
    var uploader = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        // swf文件路径
        swf: '../Uploader.swf',
        // 文件接收服务端。
        server: '/Upload/UploadImage',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#' + pick,
        // 只允许选择图片文件。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        }
    });

    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        var $list = $("#fileList"),
            $li = $(
                '<div id="' + file.id + '" class="file-item thumbnail">' +
                '<img>' +
                '<div class="info">' + file.name + '</div>' +
                '</div>'
            ),
            $img = $li.find('img');


        // $list为容器jQuery实例
        $list.append($li);

        // 创建缩略图
        // 如果为非图片文件，可以不用调用此方法。
        // thumbnailWidth x thumbnailHeight 为 100 x 100
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, 100, 100);
    });
    //当某个文件上传到服务端响应后，会派送此事件来询问服务端响应是否有效。
    uploader.on("uploadAccept", function (file, response) {
        console.info('uploadAccept', file, response);
        if (response.code < 0) {
            alert('上传失败');
        }
        else {
            alert('上传成功');
            $("#fileList").find('div.info').text(response.data.SaveFileName);
        }
    });

    if (callback != null) {
        callback(uploader);
    }

    return uploader;
};