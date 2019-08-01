$(function (dinfo) {
	// 文件拖拽插件(初始化)

	$('#ssi-upload').ssi_uploader({
		url: '/Upload/UploadImage?key=fileuploadkey',//上传文件地址
		//data: { "Tag": "", "ProID":"" },
		maxFileSize: 6,
		allowed: ['jpg', 'gif', 'png'],
		tag:'1',
		beforeEachUpload: function (fileInfo) {

		},
		beforeUpload: function () {
			//console.log("文件上传前执行的回调函数");
		},
		ajaxOptions: {
			success: function (data) {
				data = $.parseJSON(data);
				console.log(data);
			}
		}
	});

	// 拖拽元素进去后添加背景
	var tips = '<p class="up-detail-tips" style="display:none;">将图片文件拖拽至此，或<span class="uploader-files-click">点击上传</span></p>';
	$("#ssi-DropZoneBack").append(tips);

	//阻止事件传播
	//$(".up-file").on("click", ".project-tree", function (e) {
	//	e.stopPropagation();
	//});
	//递归函数，生成树
	var getJsonTree = function (data, parentId) {
		var itemArr = [];
		for (var i = 0; i < data.length; i++) {
			var node = data[i];
			//data.splice(i, 1)
			if (node.FatherID == parentId) {
				var newNode = { id: node.UID, text: node.FolderName, state: { "opened": true }, children: getJsonTree(data, node.UID) };
				if (newNode.children.length == 0) {
					newNode["icon"] = "none";
				}
				itemArr.push(newNode);
			}
		}
		return itemArr;
	}
	// 文件拖拽进去触发的事件
	$("#ssi-previewBox").on("drop", function () {
		// 把之前的背景隐藏
		$(".up-file > a").css("display", "none");
		$(".up-file > .up-tips").css("display", "none");
		// 显示当前背景
		$("#ssi-previewBox").css({ "min-height": "60px", "height": "60px" })
		$(".up-detail-tips").css("display", "inline-block");
		$(".ssi-uploader .icon-info").css("display", "inline-block");
		$(".ssi-uploader .upload-btn").css("display", "block");
		$(".icon-previewBox").css("display", "block");

		//$(".wrapper-content .up-file").outerHeight("810px");

	})
	//ssi-uploadBtn
	//事件委托（给上传的图标显示删除遮罩层）

	//点击时给图标添加边框，以及填充它的相关信息
	$(".up-file").on('click', '.ssi-imgToUploadTable', function () {
		var _ths = $(this);
		$.jstree.destroy("#sys_tree");
		$('#sys_tree').jstree({
			"core": {
				"data": function (obj, callback) {
					var jsonarray = "";
					//查询项目信息
					$.ajax({
						type: "POST",
						url: "/Folder/Show",
						dataType: "json",
						success: function (json) {
							callback.call(this, getJsonTree(json.data, -1));
						},
						error: function (jqXHR) {
							console.log(jqXHR.status);
						}
					});
				},
			},
			"check_callback": true,
			'types': {//这里就是菜单图标的显示格式
				"default": {
					"icon": "fa fa-folder tree-item-icon-color icon-lg"
				},
				"file": {
					"icon": "fa fa-file tree-item-icon-color icon-lg"
				}
			},
			'multiple': true,
			'plugins': [  //插件，下面是插件的功能
				'types',//可以设置其图标，在上面的一样。
			    'checkbox'
			],
			"checkbox": {
				"keep_selected_style": false,//是否默认选中
				"three_state": false,//父子级别级联选择
				"tie_selection": true
			},
		});
		$('#sys_tree').on("changed.jstree", function (e, data) {
			if ($(".ssi-choose").length == 0) {
				alert("请选择一张图片");
			} else {
				alert($("#sys_tree").jstree("get_checked"));
				var checked = $("#sys_tree").jstree("get_checked");
				//$('#sys_tree').data('jstree', false).empty();
				//$(".ssi-treeBox .select-choose").text(data.node.text).attr("data-info", data.node.id);		
				addDataInfo($(".ssi-choose"), "data-info", "sysID", checked);
			}
		});
		$(".icon-previewBox .ssi-imgToUploadTable").css("border", "2px solid #fff").removeClass("ssi-choose");
		_ths.css("border", "2px solid #01cd78");
		_ths.addClass("ssi-choose");
		// 获取上传时的名称填入详细信息中
		var iconName = _ths.find(".icon-current-name td").text();
		_ths.parents(".icon-previewBox").find(".icon-info .icon-name").val(iconName);
		var info = JSON.parse(_ths.attr("data-info"));
		$("#img-tag").val(info.tag);
		//$("#img-proid").val(info.sysID);
		var image = new Image();
		image.src = _ths.find("img").attr("src");
		
		var imgWidth = 0, imgHeight = 0;
		image.onload = function () {
			imgWidth = image.width;
			imgHeight = image.height;
			$("#picSize").html(imgWidth + "X" + imgHeight);//填充尺寸
			addDataInfo($(".ssi-choose"), "data-info", "size", imgWidth + "X" + imgHeight);
		};
		$("#picSize").html(imgWidth + "X" + imgHeight);
		addDataInfo($(".ssi-choose"), "data-info", "size", imgWidth + "X" + imgHeight);
		addDataInfo($(".ssi-choose"), "data-info", "dpi", $("#picResolution").val());
	});

	$(".up-file").on('mouseenter', '.ssi-imgToUploadTable', function () {
		var _ths = $(this);
		_ths.find(".del-icon").css("display", "block");
	});
	$(".up-file").on('mouseleave', '.ssi-imgToUploadTable', function () {
		var _ths = $(this);
		_ths.find(".del-icon").css("display", "none");
	});
	//上传按钮点击事件
	$(".up-file").on('click', '#submitfilesBtn', function () {
		var boolRes = true;
		$(".ssi-imgToUploadTable").each(function () {
			boolRes = false;
			var _thsJson = JSON.parse($(this).attr("data-info"));
			if (_thsJson.tag == "" || _thsJson.tag == undefined ||
				_thsJson.sysID == "" || _thsJson.sysID == undefined ||
				_thsJson.dpi == "" || _thsJson.dpi == undefined||
				_thsJson.size == "" || _thsJson.size == undefined) {
				return boolRes;
			}
			boolRes = true;
		});
		if (!boolRes) {
			alert("图标信息未填写完成，请检查！");
		} else {
			$("#ssi-uploadBtn").trigger('click');
		}
	});
	// 点击上传事件触发
	$(".uploader-files-click").click(function () {
		$(".ssi-uploadInput").trigger('click');
	});
	//标签改变事件
	$(".up-file").on('change', '#img-tag', function () {
		if ($(".ssi-choose").length == 0) {
			alert("请选择一张图片");
			return;
		}
		addDataInfo($(".ssi-choose"), "data-info", "tag", $(this).val());
		var _ths = $(this);
		var str = new RegExp("^[\u4e00-\u9fa5_a-zA-Z0-9\S]+(\\|?[\u4e00-\u9fa5_a-zA-Z0-9\S]+)+$|^[\u4e00-\u9fa5_a-zA-Z0-9\S]+$");//匹配tag格式
		if (!str.test(_ths.val())) {
			alert("验证错误，请检查！");
		}
	});
	//分辨率下拉框事件
	$(".up-file").on('change', '#picResolution', function () {
		if ($(".ssi-choose").length == 0) {
			alert("请选择一张图片");
			return;
		}
		addDataInfo($(".ssi-choose"), "data-info", "dpi", $(this).val());
	});

});
//对象的属性json值增加对象
function addDataInfo(target, property, key, val) {
	var ssiInfo = target,
	ssiData = JSON.parse(ssiInfo.attr(property));
	ssiData[key] = val;
	ssiInfo.attr(property, JSON.stringify(ssiData));
}
//动态添加自定义参数
function setAgs() {
	var args = {};
	return args;
}