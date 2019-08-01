$(function () {
	//新建项目
	$("#projectBtn").click(function () {
		console.log('新建项目操作');
		var ProjectName = $('#pro-name').val(),
	     	ProjectRemark = $('#pro-remark').val();
		//添加主任务信息
		$.ajax({
			type: "POST",
			url: "/Project/Add",
			dataType: "json",
			data: { "Project": JSON.stringify({ "ProjectName": ProjectName, "Remark": ProjectRemark }) },
			success: function (json) {
				console.log(json);
			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});
	});
		////填充文件所属下拉框 不用填充 那里填充树状文件夹
		//var ths = $("#project-data");
		////查询项目信息
		//$.ajax({
		//	type: "POST",
		//	url: "/Project/Show",
		//	dataType: "json",
		//	success: function (json) {
		//		console.log(json);
		//		//	var obj = JSON.parse(json);
		//		$.each(json.data, function (i, val) {
		//			ths.append("<option value='" + val.UID + "' >" + val.ProjectName + "</option>");
		//		});
		//	},
		//	error: function (jqXHR) {
		//		console.log(jqXHR.status);
		//	}
		//});
});