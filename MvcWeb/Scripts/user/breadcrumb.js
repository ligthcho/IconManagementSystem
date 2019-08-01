define(["jquery", "../user/home.js", "../plugins/jsTree/jstree.min.js"], function ($, home, jstree) {
	function Breadcrumb(opts) {
		this.opts = $.extend({}, Breadcrumb.DEFAULTX, opts),
		Operation = this.opts.Operation,
		QueryList = this.opts.QueryList,
		State = this.opts.State;
		if (Operation == "restore") {
			$("#move-title").html("还原于");
			$("#move").css("display", "none");
			$("#restore").css("display", "block");
		} else {
			$("#move-title").html("移动于");
			$("#restore").css("display", "none");
			$("#move").css("display", "block");
		}
		///////////////////////////////面包削逻辑
		$('.move').unbind("click").click(function () {
			var UIDs = [], tid = $(this).attr("id");
			$(".icons-content .lightBoxGallery").find(".icon-d").each(function () {
				var _ths = $(this);
				if (_ths.find(".check").prop("checked")) {
					UIDs.push(_ths.attr("data-id"));
				}
			});
			if (UIDs.length == 0) {
				alert("请选中图标");
				return false;
			}
			$('#move-to').off('show.bs.modal').on('show.bs.modal', function () {//赋值
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
					"expand_selected_onload": true,
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
				$("#move-submit").unbind("click").click(function () {
					//console.log($("#sys_tree").jstree("get_checked") + "-" + UIDs.toString());
					var ths = $(this);
					if (tid == "restore") {
						$.ajax({
							type: "POST",
							url: "/Upload/RestoreImages",
							dataType: "json",
							data: { "SysID": ($("#sys_tree").jstree("get_checked")).toString(), "UIDs": UIDs.toString() },
							success: function (json) {
								if (json.code == 1) {
									alert("还原成功");
									window.location.reload();
								}
							},
							error: function (jqXHR) {
								console.log(jqXHR.status);
							}
						});
					} else {
						$.ajax({
							type: "POST",
							url: "/Upload/MoveImages",
							dataType: "json",
							data: { "SysID": ($("#sys_tree").jstree("get_checked")).toString(), "UIDs": UIDs.toString() },
							success: function (json) {
								if (json.code == 1) {
									alert("移动成功");
									window.location.reload();
								}
							},
							error: function (jqXHR) {
								console.log(jqXHR.status);
							}
						});
					}
				});
			});//模态框操作

		});
		$(".breadcrumb .delete").unbind("click").click(function () {
			if (confirm("是否删除？")) {
				var UIDs = [];
				$(".icons-content .lightBoxGallery").find(".icon-d").each(function () {
					var _ths = $(this);
					if (_ths.find(".check").prop("checked")) {
						UIDs.push(_ths.attr("data-id"));
					}
				});
				$.ajax({
					type: "POST",
					url: "/Upload/DeleteImages",
					dataType: "json",
					data: { "UIDs": UIDs.toString() },
					success: function (json) {
						if (json.code == 1) {
							alert("删除成功");
							window.location.reload();
						} else {
							alert("删除失败");
						}
					},
					error: function (jqXHR) {
						console.log(jqXHR.status);
					}
				});
			}
		});//删除图标 
		$(".breadcrumb .choose-all").find('input').click(function () {
			if ($(this).prop("checked")) {
				$(".icons-content .lightBoxGallery").find('.icon-d').each(function () {
					var mark = '<div class="icon-mark">\
                            <input type="checkbox" class="check">\
                            <a href="javascript:;" class="add"><span class="glyphicon glyphicon-plus-sign"></span></a>\
                            <a href="javascript:;" class="modify"><span class="glyphicon glyphicon-edit"></span></a>\
                            <a href="javascript:;" class="download"><span class="glyphicon glyphicon-download-alt"></span></a>\
                            </div>';
					var _ths = $(this);
					if ($(this).find(".icon-mark").length == 0) {

						_ths.append(mark);
						_ths.find(".icon-mark a").css("display", "none");
						_ths.find(".icon-mark").css("background", "none");
						$(".lightBoxGallery").find(".icon-mark").css("border", "2px solid #01cd78");
						$(".lightBoxGallery").find(".check").css("display", "block");
						$(".lightBoxGallery").find(".check").prop("checked", true);
					} else {
						$(".lightBoxGallery").find(".icon-mark").css("border", "2px solid #01cd78");
						$(".lightBoxGallery").find(".check").prop("checked", true);
						$(".lightBoxGallery").find(".check").css("display", "block");
						$(".lightBoxGallery").find(".icon-mark").css("display", "block");
						$(".lightBoxGallery").find(".icon-mark a").css("display", "none");
						$(".lightBoxGallery").find(".icon-mark").css("background", "none");
					}
				});
			} else {
				$(".icons-content .lightBoxGallery").find('.icon-d').each(function () {
					var _ths = $(this);
				});
				$(".lightBoxGallery").find(".icon-mark").css("border", "none");
				$(".lightBoxGallery").find(".icon-mark a").css("display", "none");
				$(".lightBoxGallery").find(".check").css("display", "none");
				$(".lightBoxGallery").find(".check").removeAttr("checked");
			}
		});// 全选按钮

		$("li.dropdown").on("click", "[data-stopPropagation]", function (e) {
			e.stopPropagation();
		});
		var order, cond;
		// 排序的选中效果
		$(".page-heading .breadcrumb .sort ul li").click(function () {
			var ths = $(this),
				thsi = ths.find("i"),
				pre = $(".page-heading .breadcrumb ul");
			if (ths.attr("class") == "up") {
				$(".up i").removeClass("circle c-up");
				thsi.addClass("circle c-up");
			} else {
				$(".down i").removeClass("circle c-down");
				thsi.addClass("circle c-down");//.css({ "background-color": "#01cd78", "box-shadow": "0 0 10px #afeed6" });
			}
			cond = pre.find(".c-up").siblings("a").attr("id");
			order = pre.find(".c-down").siblings("a").attr("id");
			home.Home({
				State: State,
				QueryList: QueryList,
				Order: order,
				Cond: cond,
				Callback: function () { }
			});
		})
		//筛选功能
		$(".filter li").click(function () {
			var ths = $(this),
			   type = ths.attr("type");
			   QueryTemp = [];
			if (type == 0) {
				$(".filter ul li").each(function () {			
					var tis = $(this);
					var QueryModel = {};
					QueryModel.Field = "PictureSize",
                    QueryModel.Operation = "NotEqual",
                    QueryModel.FieldValue = tis.attr("data-info").toString();
					QueryTemp.push(QueryModel);
				});
			} else {
				var QueryModel = {};
				QueryModel.Field = "PictureSize",
				QueryModel.Operation = "Equals",
				QueryModel.FieldValue = ths.attr("data-info");
				QueryTemp.push(QueryModel);
			}
			var concatList = QueryList.concat(QueryTemp);//合并数组
			home.Home({
				State: State,
				Order: order,
				Cond: cond,
				QueryList: concatList,
				Callback: function () { }
			});
		});
	};

	Breadcrumb.DEFAULTX = {
		QueryList: [],
		State:"",
		Operation: "",//restore 还原
		Callback: function () { }
	};

	return {
		Breadcrumb: Breadcrumb
	};
});
