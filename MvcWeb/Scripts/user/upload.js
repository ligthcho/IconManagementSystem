$(function () {
	window.Global = window.Global || {};
	Global.FileQueueds = [];
	Global.GetFileQueued = function (id) {
		var res = [];
		$.each(Global.FileQueueds, function (idx, row) {
			if (row.id == id) {
				res = row;
			}
		})
		return res;
	};

	var _chunk = 0;
	WebUploader.Uploader.register({
		"before-send-file": "beforeSendFile",
		"before-send": "beforeSend",
		"after-send-file": "afterSendFile"
	}, {
		beforeSendFile: function (file) {
			console.info('beforeSendFile', Global.FileQueueds, file);
			$.each(Global.FileQueueds, function (idx, row) {
				if (row.id == file.id) {
					_chunk = row.chunk;
				}
			});
			//_chunk = Global.FileQueueds.find(f=>f.id == file.id).chunk;
		},
		beforeSend: function (block) {
			var blob = block.blob.getSource(),
				deferred = $.Deferred();
			console.info('blob', block);

			//根据md5与服务端匹配，如果重复，则跳过。

			if (block.chunk < _chunk) {
				deferred.reject();
			}
			else {
				deferred.resolve();
			}

			return deferred.promise();

		},
		afterSendFile: function (file) {
		}
	});
});


//这是上传文件成功后(文件已合并)的回调事件
function UploadSuccessCallback(file, md5) {
	console.info('UploadSuccessCallback', file);
	var data = {
		Name: file.name,
		Size: file.size,
		Extension: file.ext
	};
	var jsonData = JSON.stringify(data);
	console.info('提交文件', jsonData);
	//$.ajax({
	//    url: '/FileUpload/AddUploadRecord',
	//    type: 'post',
	//    data: { data: jsonData, md5: md5 },
	//    dataType: 'json',
	//    success: function (data) {
	//        console.info(data);
	//        if (data.code < 0) {
	//            alert(data.errmsg);
	//        }
	//        else {
	//            alert('文件[' + file.name + ']已上传并提交，请耐心等待管理员的审核');
	//            $('.pop-window0 .pop-close').click();
	//        }
	//    }
	//});
};