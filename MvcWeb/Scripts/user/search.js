define(["jquery", "../user/home.js", "../plugins/jsTree/jstree.min.js"], function ($, home, jstree) {
	function Search(opts) {
		this.opts = $.extend({}, Search.DEFAULTX, opts);

		//阻止冒泡（搜索-所属文件）
		$("#search-folder-tree").on("click", ".jstree-anchor", function (e) {
			e.stopPropagation();
		});
		//填充文件所属下拉框 
		$("#search-menu .select-choose").click(function (e) {
			$("#search-menu .project-tree").toggle();
			var ths = $(this).parents().find("#search-folder-select .project-tree");
			//查询项目信息
			$.ajax({
				type: "POST",
				url: "/Folder/Show",
				dataType: "json",
				success: function (json) {
					console.log(json);
					$.jstree.destroy("#search-folder-tree");
					$("#search-folder-tree").jstree({ "core": { "data": getJsonTree(json.data, -1) } });
					$('#search-folder-tree').on("changed.jstree", function (e, data) {
						$("#search-menu  .select-choose").text(data.node.text).attr("data-info", data.node.id);
						$("#search-folder-tree").empty();
						$("#search-menu .project-tree").css("display", "none");
					});
				},
				error: function (jqXHR) {
					console.log(jqXHR.status);
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
		});
		// 高级搜索中的重置
		$("#search-menu .reset-btn").on("click", function () {
			$("#search-key").val("");
			$("#search-menu  .select-choose").text("").attr("data-info", "");
			$("#begin-time").val("");
			$("#end-time").val("");
		})
		//搜索
		$("#search-submit").click(function () {
			var key = $("#search-key").val(),
			info = $("#search-menu .select-choose").attr("data-info"),
			beginTime = $("#begin-time").val(),
			endTime = $("#end-time").val(),
			QueryList = [];

			if (beginTime > endTime ) {
				alert("开始时间要小于结束时间");
			}
			var QueryModel1 = {};
			QueryModel1.Field = "Tag",
			QueryModel1.Operation = "Contains",
			QueryModel1.FieldValue = key;//标签

			var QueryModel2 = {};
			QueryModel2.Field = "CreateTime",
			QueryModel2.Operation = "GreaterThanOrEqual",
			QueryModel2.FieldValue = beginTime;//开始时间

			var QueryModel3 = {};
			QueryModel3.Field = "CreateTime",
			QueryModel3.Operation = "LessThanOrEqual",
			QueryModel3.FieldValue = endTime;//结束时间

			var QueryModel4 = {};
			QueryModel4.Field = "FolderUID",
			QueryModel4.Operation = "Equals",
			QueryModel4.FieldValue = info;//所属项目

			QueryList.push(QueryModel1);
			QueryList.push(QueryModel2);
			QueryList.push(QueryModel3);
			QueryList.push(QueryModel4);

			home.Home({ 
				QueryList: QueryList,
				Callback: function () { }
			});
		});
		$("#top-search").keydown(function () {
			var QueryList = [],QueryModel = {};
			QueryModel.Field = "DocumentName",
			QueryModel.Operation = "Contains",
			QueryModel.FieldValue = $("#top-search").val();//所属项目

			QueryList.push(QueryModel);
			home.Home({
				QueryList: QueryList,
				Callback: function () { }
			});
		});
	};

	Search.DEFAULTX = {
		Callback: function () { }
	};

	return {
		Search: Search
	};
});
