$(function(){
data = [
];
	$('#publish-tasklist').jexcel({
	        data: data,
	        colHeaders: ['任务清单名称', '图片尺寸', '图片分辨率', '图片背景色', '文件类型', '备注'],
	        colWidths: [200, 100, 100, 100, 100, 340],
	        colID: ['TaskListName', 'PictureSize', 'PictureResolution', 'PictureBackground', 'DocumentType', 'Remark']
	});

	$("#publishBtn").unbind("click").click(function () {
		var TaskName = $('#taskName').val(),
            Remark = $('#remark').val();
		if (TaskName.length == 0 || Remark.length == 0) {
			alert('任务名称或备注不能为空');
			return;
		}
		//添加主任务信息
		$.ajax({
			type: "POST",
			url: "/Task/Add",
			dataType: "json",
			async: false,
			data: { "TaskName": TaskName, "Remark": Remark },
			success: function (json) {
				console.log(json);
				//拿到主任务Id
				var TaskID = json.data,
					TaskList = [];
				$("#publish-tasklist tr").each(function (index, element) {
					var ths = $(this);
					if (index != 0) {
						var a = $(element).children();//$(element)代表每行tr，后面的children代表tr下面的td，a即这一行所有td的集合
						    pushData = {};
						for (var i = 1; i < a.length; i++) {
							pushData[a.eq(i).attr('data-header')] = a.eq(i).text();
							//console.log(a.eq(i).text());// 取得index为 i的td里面的文本
						}
						pushData.TaskUID = TaskID;
						TaskList.push(pushData);
					}
				});
				//添加子任务列表
				$.ajax({
					type: "POST",
					url: "/TaskList/AddList",
					dataType: "json",
					data: { "TaskList": JSON.stringify(TaskList) },
					success: function (json) {
						console.log(json);
					},
					error: function (jqXHR) {
						console.log(jqXHR.status);
					}
				});
			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});

	});
});
