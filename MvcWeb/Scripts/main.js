requirejs(["lib/require.config"], function (config) {
	requirejs(["jquery", "bootstrap", "/Scripts/user/home.js", "/Scripts/user/Breadcrumb.js", "/Scripts/user/search.js", "/Scripts/plugins/metisMenu/jquery.metisMenu.js"], function ($, bootstrap, home, breadcrumb, search, metisMenu) {
		//requirejs(["jquery", "/Scripts/user/home.js", "/Scripts/user/Breadcrumb.js"], function ($, home, breadcrumb) {
		$('.dropdown-toggle').dropdown();//激活
		home.Home({
			TotalPage: 1,
			CurrentPage: 1,
			Order: 0,
			Cond: "DocumentName",
			Callback: function () { }
		});
		breadcrumb.Breadcrumb({});
		search.Search({});
		getMenu();
		function getMenu() {//加载左边菜单		
		$.ajax({
			type: "POST",
			url: "/Folder/Show",
			dataType: "json",
			data: { "PageIndex": 1, "PageSize": 100 },
			success: function (json) {
				//子孙树，从顶级往下找到是有的子子孙孙
				function sonsTree(arr, id) {
					var temp = [], lev = 0;
					var forFn = function (arr, id, lev) {
						for (var i = 0; i < arr.length; i++) {
							var item = arr[i];
							if (item.FatherID == id) {
								item.lev = lev;
								temp.push(item);
								forFn(arr, item.UID, lev + 1);
							}
						}
					};
					forFn(arr, id, lev);
					return temp;
				}
				var tree = sonsTree(json.data, -1);
				var temp = [];
				for (var i = 0; i < tree.length; i++) {
					var item = tree[i], u = "";
					if (i > 0) {
						u = "</ul>";
					}
					if (item['lev'] == 0) {
						temp.push(u + "<li class=''><a href='javascript:void(0);' data-id=" + item.UID + " ><i class='nav-level1'></i><span class='nav-label'>" + item.FolderName + "</span> <span class='fa arrow'></span></a><ul class='nav nav-second-level collapse' >");
					} else {
						temp.push("<li><a href='javascript:void(0);' data-id=" + item.UID + ">" + item.FolderName + "</a></li>")
					}
					if (i + 1 == tree.length) {
						temp.push("</ul>")
					}
				}
				$("#side-menu").append("<li class='rubbish-box'><a href='#' id='rubbish-click'><i class='nav-del1'></i> <span class='nav-label'>回收站</span></a></li>");
				$("#side-menu").find(".rubbish-box").before(temp.join(""));
				$('#side-menu').metisMenu();
				$("#rubbish-click").click(function () {
					home.Home({
						State: 20,
						Callback: function () { }
					});
					breadcrumb.Breadcrumb({
						State: 20,
						Operation: "restore"
					});
				});
			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});
		$("#side-menu").on("click", "li a:not('#rubbish-click')", function () {
			var ths = $(this).attr("data-id"), QueryList = [], QueryModel = {};
			$("#side-menu>li a").css("background-color","");
			$(this).css("background-color", "#52635c3d");
			QueryModel.Field = "FolderUID",
			QueryModel.Operation = "Equals",
			QueryModel.FieldValue = ths;//所属项目
			QueryList.push(QueryModel);
			home.Home({
					QueryList: QueryList,
					Callback: function () { }
			});
			breadcrumb.Breadcrumb({
				QueryList: QueryList
			});
		});

		}

	});
});