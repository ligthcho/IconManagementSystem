//$(function () {
	//分页展示主任务信息

//});

var TotalPage;
GetData();
$("#pageSize").change(function () {
	GetData();
});
function GetData() {
	$.ajax({
		type: "POST",
		url: "/Task/ShowFinished",
		dataType: "json",
		data: { "PageIndex": 1, "PageSize": $("#pageSize option:selected").val() },
		success: function (json) {
			GetDataHtml(json.data.List);
			TotalPage = json.data.TotalPage;
			var options = {
				currentPage: 1,
				totalPages: TotalPage,
				numberOfPages: $("#pageSize option:selected").val(),
				bootstrapMajorVersion: 3,
				shouldShowPage: true,
				itemTexts: function (type, page, current) {//设置显示的样式，默认是箭头
					switch (type) {
						case "first":
							return "首页";
						case "prev":
							return "上一页";
						case "next":
							return "下一页";
						case "last":
							return "末页";
						case "page":
							return page;
					}
				}, onPageClicked: function (event, originalEvent, type, page) { //异步换页
					$.post("/Task/ShowFinished", { PageIndex: page, PageSize: $("#pageSize option:selected").val() }, function (json) {
						var json = JSON.parse(json);
						console.log(json.data.List);
						GetDataHtml(json.data.List);
					});
				}
			}
			$('#Paginator').bootstrapPaginator(options);
		},
		error: function (jqXHR) {
			console.log(jqXHR.status);
		}
	});
}
function GetDataHtml(mList) {
	bodyData = "",
	delLine = "<div style='position:absolute;width:100%;padding-top:20px;'><div style='outline:#ff0000 solid 1px; width:96%;'></div></div>";
	$(".management-wrap tbody").empty();
	$.each(mList, function (i, val) {
		var tempItem, row, status = "";
		if (val.State > 10) {
			status = "del";
		}
		tempItem = "<td><a href='/Task/ModifyTasks?UID="+val.UID+"' class='IsModify'><span class='glyphicon glyphicon-pencil'></span>修改</a></td><td><a href='javascript:;'  class='IsDelete'><span class='glyphicon glyphicon-trash'></span>删除</a></td>";
		row = "<tr class='" + status + "'><td>" + val.UID + "</td><td>" + val.TaskName + "</td><td>" + val.Per + "</td><td>" + val.CreateTime + "</td>" + tempItem + "</tr>";
		bodyData += row;
	});
	$(".management-wrap tbody").append(bodyData);
	$(".management-wrap tbody").find(".del").before(delLine).find(".IsDelete").removeClass("IsDelete");
	$(".management-wrap tbody").find(".del").before(delLine).find(".IsModify").attr("disabled", true).attr('href', '#');
	//删除
	$('.IsDelete').unbind("click").click(function (event) {
		if (confirm("是否确认删除该项？")) {
			event.stopPropagation();
			var UID = $(this).parents("tr").find("td").eq(0).html(), ths = $(this);
			$.ajax({
				type: "POST",
				url: "/Task/Delete",
				dataType: "json",
				data: { "UID": UID },
				success: function (json) {
					if (json.code == 1) {
						ths.removeClass("IsDelete");
						ths.parents("tr").eq(0).before(delLine);
					}
				}
			});
		}
	});
}