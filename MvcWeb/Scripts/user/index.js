$(function () {
	// 拾色器
	// 改变背景图标颜色
	$('#colorpicker').colorpicker().on('changeColor', function (e) {
		$(".icons-content .icon-bg").css("background-color", e.color.toString(
			'rgba'))
	});


	// 筛选中的选中效果            
	$(".filter .dropdown-menu").find("li").each(function () {
		var _ths = $(this);
		_ths.click(function () {
			$(".filter .dropdown-menu > li").css({ "background-color": "#fff", "color": "#979ca4" });
			_ths.css({ "background-color": "#01cd78", "color": "#fff" });
		})

	})
	// 滑动条初始化插件
	var $range = $("#ionrange_2");
	$range.ionRangeSlider({
		min: 0,
		max: 4,
		from: 0,
		type: 'single',
		step: 1
	});

	// 图标主体居中计算
	changeIconW();
	function changeIconW() {
		var wrapIconsWidth = $('.lightBoxGallery').outerWidth(true),
			iconW = $('.icon-d').outerWidth(true),
			iconNum = Math.floor(wrapIconsWidth / iconW);
		var iconsWrapW = iconNum * iconW;
		$(".icons-container").width(iconsWrapW);
	}

	// 有侧边栏点开时也要动态设置图标主体的宽度
	$(".right-sidebar-toggle").on("click", function () {
		setTimeout(function () { changeIconW(); }, 100);
	})
	$("#right-sidebar .close-sidebar").on("click", function () {
		setTimeout(function () { changeIconW(); }, 100);
	})
	$(".right-sidebar-down-toggle").on("click", function () {
		setTimeout(function () { changeIconW(); }, 100);
	})
	$("#right-sidebar-down .close-sidebar").on("click", function () {
		setTimeout(function () { changeIconW(); }, 100);
	})


	// 滑动条控制图标背景图的大小
	$range.on("change", function () {
		var $this = $(this),
			value = parseInt($this.prop("value"));
		var mark = '<div class="icon-mark" style="display: none;">\
                            <input type="checkbox" class="check">\
                            <a href="javascript:;" class="add"><span class="glyphicon glyphicon-adds"></span></a>\
                            <a href="uploadDetial.html" class="modify"><span class="glyphicon glyphicon-edit"></span></a>\
                            <a href="#" class="download" data-toggle="modal" data-target="#icon-down"><i class="glyphicon fa fa-download"></i></a>\
                    </div>';
		$(".icons-content .lightBoxGallery").find(".icon-d").each(function () {
			if ($(this).find(".icon-mark").length == 0) {
				$(this).append(mark);
			}
		})

		// console.log("Value: " + value);
		if (value == 1) {
			// 关联显示模式背景
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			$(".display-mode").find("li").eq(0).css("background-color", "#01cd78");
			$(".display-mode").find("a").eq(0).css({ "background-color": "#01cd78", "color": "#fff" });
			// 改变图标背景大小
			$(".icons-content .icon-d").removeClass("icon140 icon160 icon180").addClass("icon120");
			changeIconW();


		} else if (value == 2) {
			// 关联显示模式背景
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			$(".display-mode").find("li").eq(1).css("background-color", "#01cd78");
			$(".display-mode").find("a").eq(1).css({ "background-color": "#01cd78", "color": "#fff" });
			// 改变图标背景大小
			$(".icons-content .icon-d").removeClass("icon120 icon160 icon180").addClass("icon140");
			changeIconW();


		} else if (value == 3) {
			// 关联显示模式背景
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			$(".display-mode").find("li").eq(2).css("background-color", "#01cd78");
			$(".display-mode").find("a").eq(2).css({ "background-color": "#01cd78", "color": "#fff" });
			// 改变图标背景大小
			$(".icons-content .icon-d").removeClass("icon120 icon140 icon180").addClass("icon160");
			changeIconW();


		} else if (value == 4) {
			// 关联显示模式背景
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			$(".display-mode").find("li").eq(3).css("background-color", "#01cd78");
			$(".display-mode").find("a").eq(3).css({ "background-color": "#01cd78", "color": "#fff" });
			// 改变图标背景大小
			$(".icons-content .icon-d").removeClass("icon120 icon140 icon160").addClass("icon180");
			changeIconW();


		}
		if (value == 0) {
			// 关联显示模式背景
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			// 恢复图标背景初始大小
			$(".icons-content .icon-d").removeClass("icon120 icon140 icon160 icon180");
			changeIconW();

		}

	});

	// 显示模式下拉框(关联滑动条)
	$(".display-mode li").each(function (i) {
		var _ths = $(this);
		_ths.click(function () {
			// 选中背景高亮
			$(".display-mode li").css("background-color", "#fff");
			$(".display-mode li a").css({ "background-color": "#fff", "color": "#333" });
			_ths.css("background-color", "#01cd78");
			_ths.find("a").css({ "background-color": "#01cd78", "color": "#fff" });
			var index = i + 1;
			// Save slider instance to var
			var slider = $("#ionrange_2").data("ionRangeSlider");

			// Call sliders update method with any params（更新滑动条显示）
			slider.update({
				min: 0,
				max: 4,
				from: index,
				type: 'single',
				step: 1
			});

		})
	})
	//显示模式点击选中效果
});