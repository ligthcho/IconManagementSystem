//$(function () {
	
	//添加主任务信息
	//$("#publishBtn").click(function () {
	//	console.log('添加主任务信息');
	//	var TaskName = $('#taskName').val(),
	//	Remark = $('#remark').val();
	//	//添加主任务信息
	//	$.ajax({
	//		type: "POST",
	//		url: "/Task/Add",
	//		dataType: "json",
	//		data: { "TaskName": TaskName, "Remark": Remark },
	//		success: function (json) {
	//			console.log(json);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	//添加主任务和子任务信息
	//$("#publishBtn").click(function () {
	//	console.log('添加主任务信息');
	//	var TaskName = $('#taskName').val(),
	//	Remark = $('#remark').val();
	//	//添加主任务信息
	//	$.ajax({
	//		type: "POST",
	//		url: "/Task/Add",
	//		dataType: "json",
	//		async:false,
	//		data:{ "TaskName": TaskName, "Remark": Remark },
	//		success: function (json) {
	//			console.log(json);
	//			//拿到主任务Id
	//			var TaskID = json.id;
	//			//添加子任务列表
	//			$.ajax({
	//				type: "POST",
	//				url: "/TaskList/AddList",
	//				dataType: "json",
	//				data: { "TaskList": JSON.stringify([{ "TaskListName": "TaskListName1", "Remark": "Remark", "TaskUID": TaskID }, { "TaskListName": "TaskListName2", "Remark": "Remark", "TaskUID": TaskID }])},
	//				success: function (json) {
	//					console.log(json);
	//				},
	//				error: function (jqXHR) {
	//					console.log(jqXHR.status);
	//				}
	//			});
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	////确认登陆
	//$("#loginConfirmBtn").click(function () {
	//	console.log('确认登陆操作');
	//	var LoginName = $('#login-name').val(),
	//     	Password = $('#login-password').val();

	//	$.ajax({
	//		type: "POST",
	//		url: "/Home/LoginConfirm",
	//		dataType: "json",
	//		data: { "LoginName": LoginName, "Password": Password },
	//		success: function (json) {
	//			console.log(json);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	////新建项目
	//$("#projectBtn").click(function () {
	//	console.log('新建项目操作');
	//	var ProjectName = $('#pro-name').val(),
	//     	ProjectRemark = $('#pro-remark').val();
	//	//添加主任务信息
	//	$.ajax({
	//		type: "POST",
	//		url: "/Project/Add",
	//		dataType: "json",
	//		data: { "Project":JSON.stringify({ "ProjectName": ProjectName, "Remark": ProjectRemark })},
	//		success: function (json) {
	//			console.log(json);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	////新建文件夹
	//$("#folderBtn").click(function () {
	//	console.log('新建项目操作');
	//	var ProjectName = $('#folder-name').val(),
	//     	ProjectRemark = $('#pro-remark').val();
	//	//添加主任务信息
	//	$.ajax({
	//		type: "POST",
	//		url: "/Project/Add",
	//		dataType: "json",
	//		data: { "Project": JSON.stringify({ "ProjectName": ProjectName, "Remark": ProjectRemark }) },
	//		success: function (json) {
	//			console.log(json);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	////填充文件所属下拉框
	//var ths = $("#project-data");
	////查询项目信息
	//$.ajax({
	//	type: "POST",
	//	url: "/Project/Show",
	//	dataType: "json",
	//	//data: {},
	//	success: function (json) {
	//		console.log(json);
	//		//	var obj = JSON.parse(json);
	//		$.each(json.data, function (i, val) {
	//			ths.append("<option value='" + val.ProjectNo + "'>" + val.ProjectName + "</option>");
	//		});
	//	},
	//	error: function (jqXHR) {
	//		console.log(jqXHR.status);
	//	}
	//});
	////保存文件夹
	//$('#folderBtn').click(function () {
	//	console.log('新建文件夹操作');
	//	var FolderName = $('#folder-name').val(),
	//     	ProjectData = $('#project-data').val(),
	//     	FolderRemark = $('#folder-remark').val();
	//	//添加新建文件夹
	//	$.ajax({
	//		type: "POST",
	//		url: "/Folder/Add",
	//		dataType: "json",
	//		data: { "Folder": JSON.stringify({ "FolderName": FolderName, "ProjectData": ProjectData, "FolderRemark": FolderRemark })},
	//		success: function (json) {
	//			console.log(json);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	////修改任务
	////$('.folderBtn').click(function () {
	////	console.log('任务操作');
	////	var FolderName = $('#folder-name').val(),
	////     	ProjectData = $('#project-data').val(),
	////     	FolderRemark = $('#folder-remark').val();
	////	//修改任务
	////	$.ajax({
	////		type: "POST",
	////		url: "/Folder/Update",
	////		dataType: "json",
	////		data: { "Folder": JSON.stringify({ "FolderName": FolderName, "ProjectData": ProjectData, "FolderRemark": FolderRemark }) },
	////		success: function (json) {
	////			console.log(json);
	////		},
	////		error: function (jqXHR) {
	////			console.log(jqXHR.status);
	////		}
	////	});
	////});

	////分页展示子任务信息
	//$.ajax({
	//	type: "POST",
	//	url: "/TaskList/Show",
	//	dataType: "json",
	//	data: { "PageIndex": 1, "PageSize": 20},
	//	success: function (json) {
	//		console.log(json);
	//	},
	//	error: function (jqXHR) {
	//		console.log(jqXHR.status);
	//	}
	//});

	////分页展示主任务信息
	//$.ajax({
	//	type: "POST",
	//	url: "/Task/Show",
	//	dataType: "json",
	//	data: { "PageIndex": 1, "PageSize": 20 },
	//	success: function (json) {
	//		console.log(json);
	//	},
	//	error: function (jqXHR) {
	//		console.log(jqXHR.status);
	//	}
	//});
	//分页展示主任务信息
	//$.ajax({
	//	type: "POST",
	//	url: "/Task/Show",
	//	dataType: "json",
	//	data: { "PageIndex": 1, "PageSize": 20 },
	//	success: function (json) {
	//		//console.log(json);
	//		$.each(json.data.List, function (i, val) {//加载未完成主任务列表
	//			$("#side-menu11").append("<li class='unfinish' data-info=" + val.UID + "><a class='task-menu-level' href='javascript:;'></i><span class='nav-label'>" + val.TaskName + "</span><span class='label label-primary'>28</span><span class='menu-content'>完成度：30%</span> <span class='fa arrow'></span></a><ul class='nav nav-second-level1 collapse'></ul></li>");
	//		});
	//		//点击某个主任务加载未完成子任务列表
	//		$("#side-menu11 .unfinish").unbind('click').click(function () {
	//			var ths = $(this);
	//			var fieldValue = ths.attr("data-info");
	//			$.ajax({
	//				type: "POST",
	//				url: "/TaskList/Show",
	//				dataType: "json",
	//				data: { "PageIndex": 1, "PageSize": 100, "Field": "TaskUID", "FieldValue": fieldValue },
	//				success: function (json) {
	//					console.log(json);
	//					ths.find(".nav").empty();
	//					$.each(json.List, function (i, val) {//加载未完成主任务列表
	//						ths.find(".nav").append("<li class='icon-detail'><ul><li class='icon-name'><a href='javascript:;'>"+val.TaskListName+"</a></li><li class='icon-num'>16</li></ul></li>");
	//					});
	//				}
	//			});
	//		});
	//	}
	//});
	//展示左边菜单项目列表
	//$.ajax({
	//	type: "POST",
	//	url: "/Task/Show",
	//	dataType: "json",
	//	data: { "PageIndex": 1, "PageSize": 20 },
	//	success: function (json) {
	//		//console.log(json);
	//		$.each(json.data.List, function (i, val) {//加载未完成主任务列表
	//			$("#side-menu11").append("<li class='unfinish' data-info=" + val.UID + "><a class='task-menu-level' href='javascript:;'></i><span class='nav-label'>" + val.TaskName + "</span><span class='label label-primary'>28</span><span class='menu-content'>完成度：30%</span> <span class='fa arrow'></span></a><ul class='nav nav-second-level1 collapse'></ul></li>");
	//		});
	//		//点击某个主任务加载未完成子任务列表
	//		$("#side-menu11 .unfinish").unbind('click').click(function () {
	//			var ths = $(this);
	//			var fieldValue = ths.attr("data-info");
	//			$.ajax({
	//				type: "POST",
	//				url: "/TaskList/Show",
	//				dataType: "json",
	//				data: { "PageIndex": 1, "PageSize": 100, "Field": "TaskUID", "FieldValue": fieldValue },
	//				success: function (json) {
	//					//console.log(json);
	//					ths.find(".nav").empty();
	//					$.each(json.List, function (i, val) {//加载未完成主任务列表
	//						ths.find(".nav").append("<li class='icon-detail'><ul><li class='icon-name'><a href='javascript:;'>" + val.TaskListName + "</a></li><li class='icon-num'>16</li></ul></li>");
	//					});
	//				}
	//			});
	//		});
	//	}
	//});

//});

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
		$("#side-menu").find(".rubbish-box").before(temp.join(""));
		$('#side-menu').metisMenu();
	},
	error: function (jqXHR) {
		console.log(jqXHR.status);
	}
});
$("#side-menu").on("click", "li a:not('#rubbish-click')", function () {
	var ths = $(this).attr("data-id"), QueryList = [], QueryModel = {};
	$("#side-menu>li a").css("background-color", "");
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




