$(function () {
	// 日期插件
	var start ={elem: '#begin-time'};
	var end = {elem: '#end-time'};
	laydate.render(start);
	laydate.render(end);
	//给input赋值
	var myDate = new Date();
	myDate = myDate.getFullYear() + "-" + myDate.getMonth() + 1 + "-" + myDate.getDate()
	$('#begin-time').val(myDate);
	$('#end-time').val(myDate);

	// 头部导航选中高亮色
	$(".navbar-top-links > li").each(function () {
		var _ths = $(this);
		_ths.click(function () {
			$(".navbar-top-links > li").removeClass("active");
			_ths.addClass("active");
		})
	});


	// 项目管理树结构
	//$('#using_json').jstree({
	//	"core": {
	//		"data": [
	//		  {
	//		  	"text": "Empty Folder",
	//		  	"state": {
	//		  		"opened": true
	//		  	},
	//		  	"children": [
	//			  {
	//			  	"text": "animate.css",
	//			  	"icon": "none"
	//			  }
	//		  	]
	//		  },
	//		  {
	//		  	"text": "Resources123",
	//		  	"state": {
	//		  		"opened": true
	//		  	},
	//		  	"children": [
	//			  {
	//			  	"text": "css",
	//			  	"children": [
	//				  {
	//				  	"text": "animate.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "bootstrap.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "main.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "style.css",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  },
	//			  {
	//			  	"text": "js",
	//			  	"children": [
	//				  {
	//				  	"text": "bootstrap.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "inspinia.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "jquery.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "jsTree.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "custom.min.js",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  },
	//			  {
	//			  	"text": "html",
	//			  	"children": [
	//				  {
	//				  	"text": "layout.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "navigation.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "navbar.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "footer.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "sidebar.html",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  }
	//		  	]
	//		  },
	//		  "Fonts",
	//		  "Images",
	//		  "Scripts",
	//		  "Templates"
	//		]
	//	}
	//});

	// 移动于树结构
	//$("#move_tree_json").jstree({
	//	"core": {
	//		"data": [
	//		  {
	//		  	"text": "Empty Folder",
	//		  	"state": {
	//		  		"opened": true
	//		  	},
	//		  	"children": [
	//			  {
	//			  	"text": "animate.css",
	//			  	"icon": "none"
	//			  }
	//		  	]
	//		  },
	//		  {
	//		  	"text": "Resources",
	//		  	"state": {
	//		  		"opened": true
	//		  	},
	//		  	"children": [
	//			  {
	//			  	"text": "css",
	//			  	"children": [
	//				  {
	//				  	"text": "animate.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "bootstrap.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "main.css",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "style.css",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  },
	//			  {
	//			  	"text": "js",
	//			  	"children": [
	//				  {
	//				  	"text": "bootstrap.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "inspinia.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "jquery.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "jsTree.min.js",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "custom.min.js",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  },
	//			  {
	//			  	"text": "html",
	//			  	"children": [
	//				  {
	//				  	"text": "layout.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "navigation.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "navbar.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "footer.html",
	//				  	"icon": "none"
	//				  },
	//				  {
	//				  	"text": "sidebar.html",
	//				  	"icon": "none"
	//				  }
	//			  	],
	//			  	"state": {
	//			  		"opened": true
	//			  	}
	//			  }
	//		  	]
	//		  },
	//		  "Fonts",
	//		  "Images",
	//		  "Scripts",
	//		  "Templates"
	//		]
	//	}
	//});


	// 已完成(具体项鼠标移入显示tips)
	$(".sidebar-container").find(".icon-detail-f").each(function () {
		var _ths = $(this);
		var tips = '<div class="tips">\
                        <i class="triangle-n"></i><i class="triangle-w"></i>\
                        <span class="icon-f">图标</span>\
                        <span class="tips-name tips-n">图片名称：</span><span class="tips-val">基础号表</span>                        <span class="tips-name tips-s">图片尺寸：</span><span class="tips-val">16*16</span>\
                        <span class="tips-name tips-t">完成日期：</span><span class="tips-val">2017/10/20</span>                        <span class="tips-name tips-d">说明：</span><span class="tips-val">业务</span>                        <span class="tips-name tips-b">备注：</span><span class="tips-val">图标应用于珠江，应与珠江医院有所相关</span> \
                    </div>'
		_ths.find(".icon-init a").mouseover(function () {
			if (_ths.find(".tips").length == 0) {
				_ths.append(tips);
			} else {
				_ths.find(".tips").css("display", "block");
			}
		}).mouseleave(function () {
			_ths.find(".tips").css("display", "none");
		})

	})

	// 头部导航中的任务列表（显示则主体内容宽度改变）
	var flag1 = 1,      //变量开关控制
		flag2 = 1,
		zindex = 1100;  //控制任务列表和批量下载的层级
	function changeWidth(obj, value) {
		if ($(".page-heading").length != 0) {
			var headerW = $(".page-heading").outerWidth();
		} else {
			var headerW = 0;
		}
		var wrapperContent = $(".wrapper-content").outerWidth();
		if ($(".page-heading").length != 0) {
			$(".page-heading").outerWidth(headerW - value);
		}
		$(".wrapper-content").outerWidth(wrapperContent - value);

	}
	$(".right-sidebar-toggle").on("click", function () {
		// 改变层级
		$("#right-sidebar").css("z-index", zindex + 1);
		zindex++;
		var workSidebar = $("#right-sidebar").outerWidth();
		if (flag1 == 1 && flag2 == 1) {
			changeWidth($("#right-sidebar"), workSidebar);
		}
		flag1 = 0;

		// 改变上传详细页面的高度（局部内容的宽度）
		if ($(".wrapper-content .up-file").length != 0 && $(".ssi-imgToUploadTable").length != 0) {
			$(".wrapper-content .up-file").outerHeight("810px");
			$(".icon-up-wrapper").css({ "margin-left": "24%", "width": "70%" });
			$(".wrapper-content .icon-info").css({ "margin-left": "24%", "width": "70%" });
		}
		$("#my").css("width","100%");

	})
	$("#right-sidebar").on('click','.close-sidebar',function () {
		flag1 = 1;
		var workSidebar = $("#right-sidebar").outerWidth();
		if (!flag1 == 0 && !flag2 == 0) {
			// changeWidth($("#right-sidebar"),-workSidebar);
			if ($(".page-heading").length != 0) {
				$(".page-heading").outerWidth("100%");
			}
			$(".wrapper-content").outerWidth("100%");

			////恢复上传详细页面的高度(局部内容的宽度)
			//if ($(".wrapper-content .up-file").length != 0) {
			//	$(".wrapper-content .up-file").removeAttr("style");
			//	$(".icon-up-wrapper").css({ "margin-left": "2%", "width": "52%" });
			//	$(".wrapper-content .icon-info").css({ "margin-left": "5%", "width": "40%" });
			//}
		}



	})



	// 头部导航中的批量下载（显示则主体内容宽度改变）
	$(".right-sidebar-down-toggle").on("click", function () {
		// 改变层级
		$("#right-sidebar-down").css("z-index", zindex + 1);
		zindex++;
		var workSidebar = $("#right-sidebar-down").outerWidth();
		if (flag2 == 1 && flag1 == 1) {
			changeWidth($("#right-sidebar-down"), workSidebar);
		}
		flag2 = 0;

		// 改变上传详细页面的高度
		if ($(".wrapper-content .up-file").length != 0 && $(".ssi-imgToUploadTable").length != 0) {
			$(".wrapper-content .up-file").outerHeight("810px");
			$(".icon-up-wrapper").css({ "margin-left": "24%", "width": "70%" });
			$(".wrapper-content .icon-info").css({ "margin-left": "24%", "width": "70%" });
		}

	})
	$("#right-sidebar-down .close-sidebar").on("click", function () {
		flag2 = 1;
		var workSidebar = $("#right-sidebar-down").outerWidth();
		if (!flag1 == 0 && !flag2 == 0) {
			// changeWidth($("#right-sidebar-down"),-workSidebar);
			if ($(".page-heading").length != 0) {
				$(".page-heading").outerWidth("100%");
			}
			$(".wrapper-content").outerWidth("100%");
			//恢复上传详细页面的高度
			//if ($(".wrapper-content .up-file").length != 0) {
			//	$(".wrapper-content .up-file").removeAttr("style")
			//	$(".icon-up-wrapper").css({ "margin-left": "2%", "width": "52%" });
			//	$(".wrapper-content .icon-info").css({ "margin-left": "5%", "width": "40%" });
			//}
		}



	})

	// 批量下载中的一键清除
	$("#right-sidebar-down .clear").click(function () {
		if (confirm("确定一键清除？")) {
			$(this).parents(".sidebar-down").find(".main-icons .icon-wrap").remove();
			$("#down-count").html(0);
		}

	})



	// 批量下载中的删除遮罩层
	$('#right-sidebar-down .main-icons').on("mouseenter", ".icon-wrap", function () {
		var _ths = $(this);
		var mark = '<div class="icon-mark">\
                        <span class="glyphicon glyphicon-trash"></span>\
                    </div>';
		if (_ths.find('.icon-mark').length == 0) {
			_ths.append(mark);
		} else {
			_ths.find('.icon-mark').css("display", "block");
		};
	});

	$('#right-sidebar-down .main-icons').on("mouseleave", ".icon-wrap", function () {
		var _ths = $(this);
		_ths.find('.icon-mark').css("display", "none");
	});
	// 批量下载中的删除某个图标
	$('#right-sidebar-down .main-icons').on("click", ".glyphicon-trash", function () {
		var _ths = $(this),
		     _id = _ths.parent().parent().find(".icon img").attr("data-id"),
		    _list = JSON.parse($('.icons-container').attr("data-down")),
		index =_list.indexOf(_id);
		if (index > -1) {
			_list.splice(index,1);
			$('.icons-container').attr("data-down", JSON.stringify(_list));
			$("#down-count").html(parseInt($("#down-count").html()) - 1);
			_ths.parents(".icon-wrap").remove();
		}
	});
	//批量下载按钮事件
	$('#right-sidebar-down').on("click", "#batch-down", function () {
		var json = [];
			$("#down-iconlist .icon-wrap").each(function () {
				var ths = $(this),
					mod ={},
				    src = ths.find("img").attr("src");
				mod.url = src;
				json.push(mod)
			});
			if (json.length == 0) {
				alert("请选中图片");
				return;
			}
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
// 任务列表
// 全选按钮
//$(".sidebar-container").find(".drop-topfun .choose-all input").each(function(){
//	$(this).click(function(){
//		var _ths = $(this);
//		if (_ths.prop("checked")) {
//			_ths.parents(".finish-work").find('.icon-detail-f input').each(function(){
//				$(this).prop("checked",true);
//			});
//		}else{
//			_ths.parents(".finish-work").find('.icon-detail-f input').each(function(){
//				$(this).prop("checked",false);
//			});
//		}
//	})
//})


// 底部页码切换
$(".text-center .btn-group").find('.page').each(function () {
	$(this).click(function () {
		$(".text-center .btn-group .page").removeClass('active');
		$(this).addClass('active');
	})

});




// 上传图标详细的鼠标移入效果
// $(".detail-content .detail-icons").find(".icon-t").each(function(){
// 	var _ths = $(this);
//        var mark = '<span class="uploader-click">\
//                        <input type="file" name="file-up">\
//                        重新上传\
//                    </span>';
//        _ths.mouseenter(function(){
//            if(_ths.find(".uploader-click").length == 0){
//                _ths.append(mark);
//            }else{
//                _ths.find(".uploader-click").css("display","block");
//            }


//        }).mouseleave(function(){
//            _ths.find(".uploader-click").css("display","none");
//        });

// })
// 排序中的选中效果
$(".sidebar-container .sort li").click(function (e) {
	e.stopPropagation();
	//$(".sidebar-container .sort .circle").css({ "background-color": "#fff", "box-shadow": "0 0 10px #fff" })
	$(this).find(".circle").css({ "background-color": "#01cd78", "box-shadow": "0 0 10px #afeed6" });
});


});