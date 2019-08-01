$(function () {
	//确认登陆
	$("#loginConfirmBtn").click(function () {
		console.log('确认登陆操作');
		var LoginName = $('#login-name').val(),
			Password = $('#login-password').val();

		$.ajax({
			type: "POST",
			url: "/Home/LoginConfirm",
			dataType: "json",
			data: { "LoginName": LoginName, "Password": Password },
			success: function (json) {
				console.log(json);
				if (json.code == 1) {
					window.location.href = '/Home/Index';
				}			
			},
			error: function (jqXHR) {
				console.log(jqXHR.status);
			}
		});
	});
	//退出登陆
	//$("#login-out").click(function () {
	//	console.log('退出登陆操作');
	//	$.ajax({
	//		type: "POST",
	//		url: "/Home/LoginOut",
	//		success: function (data) {
	//			console.log(data);
	//		},
	//		error: function (jqXHR) {
	//			console.log(jqXHR.status);
	//		}
	//	});
	//});

	//设置cookies
	function setCookie(name, set_l) {
		var set_arr = new Array;
		set_arr = [];
		set_arr.push(set_l);
		document.cookie = [name, '=', JSON.stringify(set_arr)].join('');
	};
	function getCookie(name) {
		var Cookie_arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
		if (Cookie_arr !== null) {
			var c_arr = $.parseJSON(Cookie_arr[2]);
			c_class = c_arr[0];
			$(".contant").removeClass("left-layout").addClass(c_class);
		} else {
			if (!$(".contant").hasClass("left-layout")) {
				$(".contant").addClass("left-layout");
			};
		}
	};

});