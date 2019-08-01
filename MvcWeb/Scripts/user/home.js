define(["jquery", "/Scripts/user/bootstrap-paginator.js"], function ($, bootstrapPaginator) {
	function Home(opts) {
		this.opts = $.extend({}, Home.DEFAULTX, opts);
		var TotalPage = this.opts.TotalPage,
		CurrentPage = this.opts.CurrentPage,
		Order = this.opts.Order,
		Cond = this.opts.Cond,
		QueryList = this.opts.QueryList,
		State = this.opts.State,
		QueryModel = {};
		GetData();
		$("#pageSize-Index").change(function () {
			GetData();
		});
		//初始化分页插件
		$('#Paginator-Index').bootstrapPaginator({
			currentPage: CurrentPage,
			totalPages: TotalPage,
			numberOfPages: 1,
			shouldShowPage: true,
			bootstrapMajorVersion: 3
		});
		function GetData() {
			$.ajax({
				type: "POST",
				url: "/Upload/Show",
				dataType: "json",
				data: { "PageIndex": 1, "PageSize": $("#pageSize-Index  option:selected").val(), "Order": Order, "Cond": Cond, "QueryList": JSON.stringify(QueryList), "State": State },
				success: function (json) {
					console.log(json)

					GetDataHtml(json.data.List);
					TotalPage = json.data.TotalPage;
					var options = {
						currentPage: CurrentPage,
						totalPages: TotalPage,
						numberOfPages: $("#pageSize-Index option:selected").val(),
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
							$.post("/Upload/Show", { PageIndex: page, PageSize: $("#pageSize-Index option:selected").val(), "Order": Order, "Cond": Cond, "QueryList": JSON.stringify(QueryList) }, function (json) {
								var json = JSON.parse(json);
								GetDataHtml(json.data.List);
							});
						}
					}
					$('#Paginator-Index').bootstrapPaginator(options);
				},
				error: function (jqXHR) {
					console.log(jqXHR.status);
				}
			});
		}
		function GetDataHtml(mList) {
			bodyData = "";
			$(".icons-container").empty();
			$.each(mList, function (i, val) {
				var tempItem;
				tempItem = "<a href='#' data-gallery='' class='icon-d'><div class='icon-bg'><img data-info=" + escape(JSON.stringify(val)) + " src=" + val.DocumentPath + " class='icon-s' style='max-width:100%;height: auto;'/></div><span class='icon-name'>" + val.DocumentName + "</span></a>";
				bodyData += tempItem;
			});
			$(".icons-container").append(bodyData);
			// 移入图标显示遮罩层和相关操作
			$(".icons-content .lightBoxGallery").find('.icon-d').each(function () {
				var _ths = $(this);
				_ths.attr("data-id", JSON.parse(unescape(_ths.find("img").attr("data-info"))).UID);
				var mark = '<div class="icon-mark">\
                            <input type="checkbox" class="check">\
                            <a href="javascript:;" class="add" id="add-down"  data-down=flase><span class="glyphicon glyphicon-adds"></span></a>\
                            <a href="#" class="modify"><span class="glyphicon glyphicon-edit"></span></a>\
                            <a href="#" class="download" data-toggle="modal" data-target="#icon-down" ><i class="glyphicon fa fa-download"></i></a>\
                            </div>';
				_ths.mouseenter(function () {
					_ths.addClass("selected");
					if (_ths.find(".icon-mark").length == 0) {
						_ths.append(mark);
					} else {
						_ths.find(".icon-mark").css("display", "block");
						_ths.find(".check").css("display", "block");
						_ths.find(".icon-mark a").css("display", "block");
						_ths.find(".icon-mark").css("border", "none").css("background", "rgba(0,0,0,0.7)")
					};
					// 如果图标没有全选，则去掉全选标志
					//$(".icons-content .lightBoxGallery").find(".check").each(function () {
					//alert("23");
					//$(this).click(function () {
					//	if (!$(this).prop("checked")) {
					//		$(".breadcrumb .choose-all input").prop('checked', false);
					//	}
					//});
					//});
				}).mouseleave(function () {
					_ths.removeClass("selected");
					_ths.find(".icon-mark a").css("display", "none");
					_ths.find(".icon-mark").css("background", "none");
					if (_ths.find(".check").prop("checked")) {
						_ths.find(".icon-mark").css("border", "2px solid #01cd78");
					} else {
						_ths.find(".check").css("display", "none");
					}
				});
			});
		}

		$('#icon-down').on('show.bs.modal', function () {//赋值
			var imgData = JSON.parse(unescape($(".selected").find("img").attr("data-info")));
			$('#img-name').html(imgData.DocumentName);
			$("#myModalLabel").html(imgData.DocumentName);
			$('#img-size').html(imgData.PictureSize);
			$('#img-ext').html(imgData.DocumentType);
			$('#img-designer').html(imgData.Creater);
			$('#img-time').html(imgData.CreateTime);
			$('#img-dpi').html(imgData.PictureResolution);
			$('#img-show').attr("src", imgData.DocumentPath);
			$('#icon-down .modify a').attr("href", "/Upload/UploadDetial?UID=" + imgData.UID);
			$('#img-selectsize').change(function () {
				var selID = $('#img-selectsize').val();
				if (selID == 0) {
					$('#img-show').attr("style", "width:32px;height:32px");
				} else if (selID == 1) {
					$('#img-show').attr("style", "width:64px;height:64px");
				} else if (selID == 2) {
					$('#img-show').attr("style", "width:128px;height:128px");
				}
			});
			$.ajax({
				type: "POST",
				url: "/Folder/QueryByDocumentFolderUID",
				dataType: "json",
				data: { "UID": JSON.stringify(imgData.UID) },
				success: function (json) {
					$('#img-sys').empty();
					$.each(json.data, function (i, val) {
						$('#img-sys').append("<li>" + val.FolderName + "</li>");
					});
				},
				error: function (jqXHR) {
					console.log(jqXHR.status);
				}
			});
			//模态框内-删除
			$('#icon-down .del').unbind("click").click(function () {
				if (confirm("是否删除？")) {
					$.ajax({
						type: "POST",
						url: "/Upload/DeleteImage",
						dataType: "json",
						data: { "UID": JSON.stringify(imgData.UID) },
						success: function (json) {
							if (json.code == 1) {
								alert("删除成功");
							} else {
								alert("删除成功");
							}
						},
						error: function (jqXHR) {
							console.log(jqXHR.status);
						}
					});
				}
			});
			//模态框内-添加下载列表
			$('#icon-down .add-to-list').unbind("click").click(function () {
				var bool = false;
				$("#down-iconlist .icon-wrap").each(function () {
					var uid = $(this).find("img").attr("data-id");
					if (uid == imgData.UID) {
						bool = true;
						return;
					}
				});
				if (bool) {
					alert("已添加至下载列表");
					return;
				}
				var html = "<li class='icon-wrap'>" +
			  "<div class='icon'>" +
				"<img src=" + imgData.DocumentPath + " data-id=" + imgData.UID + ">" +
			  "</div>" +
			  "<span class='icon-name'>" + imgData.DocumentName + "</span>" +
			"</li>";
				$("#down-iconlist").append(html);
				$("#down-count").html(parseInt($("#down-count").html()) + 1);
			});
			//$('#img-down').attr("href", "/Upload/GetImageFiles?UID=" + imgData.UID);//$('#img-down').attr("href", "/Upload/GetImageFiles?UID=" + imgData.UID);
			$('#img-down').click(function () {
				var json = [{ "url": imgData.DocumentPath }];//$.cookie('sid')			
				var form = $("<form>");
				form.attr('style', 'display:none');
				form.attr('target', '');
				form.attr('method', 'post'); //请求方式
				form.attr('action', '/Upload/GetImageFiles');//请求地址
				var input1 = $('<input>');//将你请求的数据模仿成一个input表单
				input1.attr('type', 'hidden');
				input1.attr('name', 'ListImagePath');//该输入框的name
				input1.attr('value', JSON.stringify(json));//该输入框的值
				$('body').append(form);
				form.append(input1);

				form.submit();
				form.remove();
			});
		});//模态框操作
		$(document).off('click', "#add-down").on("click", "#add-down", function () {//添加到下载列表			
			var ths = $(this);
			iconID = ths.parents(".icon-d").attr("data-id"),
			datadown = ths.parents(".icons-container").attr("data-down");
			//if ($.inArray(iconID, list) >= 0) {
			//	alert("已添加至下载列表");
			//	return;
			//}
			var bool = false;
			$("#down-iconlist .icon-wrap").each(function () {
				var uid = $(this).find("img").attr("data-id");
				if (uid == iconID) {
					bool = true;
					return;
				}
			});
			if (bool) {
				alert("已添加至下载列表");
				return;
			}
			var list = JSON.parse(datadown);
			list.push(iconID);
			ths.parents(".icons-container").attr("data-down", JSON.stringify(list));
			$("#down-count").html(parseInt($("#down-count").html()) + 1);
			var imgDatas = JSON.parse(unescape($(".selected").find("img").attr("data-info"))),
			    html = "<li class='icon-wrap'>" +
						 "<div class='icon'>" +
						   "<img src=" + imgDatas.DocumentPath + " data-id=" + imgDatas.UID + ">" +
		 	             "</div>" +
			             "<span class='icon-name'>" + imgDatas.DocumentName + "</span>" +
		               "</li>";
			$("#down-iconlist").append(html);
		});
		$('.icons-container').unbind("click").on("click", ".modify", function () {
			var ths = $(this);
			var imgData = JSON.parse(unescape($(".selected").find("img").attr("data-info")));
			ths.attr("href", "/Upload/UploadDetial?UID=" + imgData.UID);
		});//跳转到修改页
	};
	Home.DEFAULTX = {
		TotalPage: 1,
		CurrentPage: 1,
		Order: 0,
		Cond: "DocumentName",
		QueryList: [],
		State: 10,
		Callback: function () { }
	};

	return {
		Home: Home
	};
});
