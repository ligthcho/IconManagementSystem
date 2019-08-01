$(function () {
	//新建文件夹
	$("#folderBtn").unbind("click").click(function (e) {
		e.stopPropagation();
		console.log('新建文件夹操作');
		var FolderName = $('#folder-name').val(),
	     	FolderRemark = $('#folder-remark').val(),
			FatherID = $('#folder-data .select-choose').attr("data-info");
		if (FatherID == "") {
			FatherID = -1;
		}
		//新建文件夹操作
		$.ajax({
			type: "POST",
			url: "/Folder/Add",
			dataType: "json",
			data: { "Folder": JSON.stringify({ "FolderName": FolderName, "FatherID": FatherID, "Remark": FolderRemark }) },
			success: function (json) {
				console.log(json);
				if (json.code == 1) {
					alert("保存成功");
				    $('#folder-name').val("")
				    $('#folder-remark').val("");
				    $('.select-choose').attr("data-info", "");
				} else if (json.code == -1) {
					alert("名称重复，请检查！");
				}
				//$(".modal-select .select-choose").click();
			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});
	});
	//填充文件所属下拉框 
	$(".modal-select .select-choose").click(function (e) {	
		var ths = $(this).parents().find("#folder-data .project-tree");
		//查询项目信息
		$.ajax({
			type: "POST",
			url: "/Folder/Show",
			dataType: "json",
			success: function (json) {
				$.jstree.destroy("#sys_tree");
				//$('#new-folder .folder_tree_json').data('jstree', false).empty();
				$("#folder_tree_content").jstree({ "core": { "data": getJsonTree(json.data, -1) } });
				$('#folder_tree_content').on("changed.jstree", function (e, data) {
					$(".modal-select .select-choose").text(data.node.text).attr("data-info", data.node.id);
					$("#folder_tree_content").empty();
				});

			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});
	});
	//显示隐藏文件夹所属
	//$(".modal-select .select-choose").click(function (e) {
	//	e.stopPropagation();
	//	var ths = $(this);
	//	var _list = ths.parents().find("#folder-data .project-tree");
	//	_list.toggle();
	//});
	//项目管理
	var chooseID = "";
	$("#pro-management").on("click", function () {
		//$('#folder_tree').data('jstree', true).empty();
		$(".modal-select .project-tree").css("display", "block");
		$.jstree.destroy("#folder_tree");
		$('#folder_tree').jstree({
			"core": { "data":function (obj, callback) {
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
			'plugins': [  //插件，下面是插件的功能
				'types'//可以设置其图标，在上面的一样。
			]
		});
		$('#folder_tree').on("changed.jstree", function (e, data) {
			console.log(data);
			if (data.action == "select_node") {
				if (data.node.id !== undefined) {
					chooseID = data.node.id;
				}
			}		
		});
	});
	//项目管理按钮显示
	$("#folder-data .project-tree").on("click", ".jstree-anchor", function (e) {
		e.stopPropagation();
	})
	//项目管理按钮显示
	$("#promagr-tree").on("click", ".jstree-anchor", function (e) {
		e.stopPropagation();
		var _ths = $(this);
		$("#promagr-tree").find(".handle").css("display", "none");
		if (_ths.find(".handle").length == 0) {
			_ths.after("<div class='handle'><button type='button' class='btn btn-modify'>修改</button ><button  type='button' class='btn btn-delete'>删除</button></div>");
		}
	});
	//项目管理修改事件
	$("#promagr-tree").delegate(".btn-modify", "click", function (e) {
		e.stopPropagation();
		var _ths = $(this);
		_ths.attr("class", "btn btn-pro-submit").text("确定");
		_ths.before("<input type=text id='pro-modity-input'></input>");
	});
	//项目管理修改确定事件
	$("#promagr-tree").delegate(".btn-pro-submit", "click", function (e) {
		e.stopPropagation();
		var _ths = $(this),
			pro_input = _ths.parents().eq(0).find("input"),
			fName=pro_input.val();
		pro_input.parents().find(".jstree-clicked").eq(0).text(fName);
		if (confirm("确定要修改数据吗？")) {
			$.ajax({
				type: "POST",
				url: "/Folder/Modify",
				data: { "FID": chooseID.toString(), FName: fName},
				dataType: "json",
				success: function (json) {
					if (json.code == 1) {
						alert("修改成功!");
						$('#folder_tree').jstree(true).refresh();//刷新树
					} else {
						alert("修改失败!");
					}
				},
				error: function (jqXHR) {
					console.log(jqXHR.status);
				}
			});
		}
	});
	//项目管理删除事件
	$("#promagr-tree").delegate(".btn-delete", "click", function () {
		var _ths = $(this);		
		if (_ths.parent().siblings(".jstree-children").length > 0) {
			alert("请先清除其下子文件");
			return false;
		}
		if (confirm("确定要删除数据吗？")) {
			$.ajax({
				type: "POST",
				url: "/Folder/Delete",
				data: { "FID": chooseID.toString() },
				dataType: "json",
				success: function (json) {
					console.log(json)
					if (json.code == 1) {
						alert("删除成功!");
						$('#folder_tree').jstree(true).refresh();//刷新树

					} else {
						alert("删除失败!");
					}
				},
				error: function (jqXHR) {
					console.log(jqXHR.status);
				}
			});
		}
	});
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
	//点击项目管理空白区域
	//$(document).bind("click", function (e) {
	//	var target = $(e.target);
	//	if (target.closest(".handle").length == 0 || target.closest("#new-folder .project-tree").length == 0) {
	//		$(".handle").hide();
	//	}
	//});
	//点击文件夹所属空白区域 清空span
	//$("#folder-tree-box").bind("click", function (e) {
	//	var target = $(e.target);
	//	$("#folder-data  .select-choose").attr("data-info", "").text("");
	//});
	//组织事件传播
	$("#folder-data").on("click", "#folder-tree-box", function (e) {
		e.stopPropagation();
	});

});
