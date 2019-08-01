(function ($) {
    var INTEROP_PATH = {
        //swf所在路径
        swf: '/Scripts/webuploader/Uploader.swf',
        //处理文件上传的地址
        server: '../Upload/ChunkUpload',
        //获取已上传文件的块数量
        GetMaxChunk: '../Upload/GetMaxChunk',
        //进行文件合并的地址
        MergeFiles: "../Upload/MergeFiles"
    };

    // 当domReady的时候开始初始化
    $(function () {
        var $wrap = $('#uploader'),

        // 图片容器
            $queue = $('<ul class="filelist"></ul>')
                .appendTo($wrap.find('.queueList')),
			
        // 状态栏，包括进度和控制按钮
            $statusBar = $wrap.find('.statusBar'),

        // 文件总体选择信息。
            $info = $statusBar.find('.info'),

        // 上传按钮
            $upload = $wrap.find('.uploadBtn'),

        // 没选择文件之前的内容。
            $placeHolder = $wrap.find('.placeholder'),

            $progress = $statusBar.find('.progress').hide(),

        // 添加的文件数量
            fileCount = 0,

        // 添加的文件总大小
            fileSize = 0,

        // 优化retina, 在retina下这个值是2
            ratio = window.devicePixelRatio || 1,

        // 缩略图大小
            thumbnailWidth = 110 * ratio,
            thumbnailHeight = 110 * ratio,

        // 可能有pedding, ready, uploading, confirm, done.
            state = 'pedding',

        // 所有文件的进度信息，key为file id
            percentages = {},
        // 判断浏览器是否支持图片的base64
            isSupportBase64 = (function () {
                var data = new Image();
                var support = true;
                data.onload = data.onerror = function () {
                    if (this.width != 1 || this.height != 1) {
                        support = false;
                    }
                }
                data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
                return support;
            })(),

        // 检测是否已经安装flash，检测flash的版本
            flashVersion = (function () {
                var version;

                try {
                    version = navigator.plugins['Shockwave Flash'];
                    version = version.description;
                } catch (ex) {
                    try {
                        version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
                                .GetVariable('$version');
                    } catch (ex2) {
                        version = '0.0';
                    }
                }
                version = version.match(/\d+/g);
                return parseFloat(version[0] + '.' + version[1], 10);
            })(),

            supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                            'WebkitTransition' in s ||
                            'MozTransition' in s ||
                            'msTransition' in s ||
                            'OTransition' in s;
                s = null;
                return r;
            })(),

        // WebUploader实例
            uploader,
            GUID = WebUploader.Base.guid(); //当前页面是生成的GUID作为标示

        if (!WebUploader.Uploader.support('flash') && WebUploader.browser.ie) {

            // flash 安装了但是版本过低。
            if (flashVersion) {
                (function (container) {
                    window['expressinstallcallback'] = function (state) {
                        switch (state) {
                            case 'Download.Cancelled':
                                alert('您取消了更新！')
                                break;

                            case 'Download.Failed':
                                alert('安装失败')
                                break;

                            default:
                                alert('安装已成功，请刷新！');
                                break;
                        }
                        delete window['expressinstallcallback'];
                    };

                    var swf = 'Scripts/expressInstall.swf';
                    // insert flash object
                    var html = '<object type="application/' +
                            'x-shockwave-flash" data="' + swf + '" ';

                    if (WebUploader.browser.ie) {
                        html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                    }

                    html += 'width="100%" height="100%" style="outline:0">' +
                        '<param name="movie" value="' + swf + '" />' +
                        '<param name="wmode" value="transparent" />' +
                        '<param name="allowscriptaccess" value="always" />' +
                    '</object>';

                    container.html(html);

                })($wrap);

                // 压根就没有安转。
            } else {
                $wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" border="0"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
            }

            return;
        } else if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！');
            return;
        }

        // 实例化
        uploader = WebUploader.create({
            pick: {
                id: '#filePicker',
                label: '点击选择文件'
            },
            formData: {uid: 22222},
            dnd: '#dndArea',
            paste: '#uploader',
            swf: INTEROP_PATH.swf,
            chunked: true, //分片处理大文件
            chunkSize: 2 * 1024 * 1024,
            server: INTEROP_PATH.server,
            disableGlobalDnd: true,
            threads: 1, //上传并发数
            //由于Http的无状态特征，在往服务器发送数据过程传递一个进入当前页面是生成的GUID作为标示
            formData: { uid: 333 },
            fileNumLimit: 300,
            compress: false, //图片在上传前不进行压缩
            fileSizeLimit: 1024 * 1024 * 1024,    // 1024 M
            fileSingleSizeLimit: 1024 * 1024 * 1024    // 1024 M
        });

        uploader.on('uploadBeforeSend', function (object, data, headers) {
        	console.info('uploadBeforeSend', object, data, headers);
        	var info = JSON.parse($('#' + data.id).attr("data-info"));
        	data.md5 = Global.GetFileQueued(data.id).md5;
        	data.sysID = info.sysID;
        	data.tag = info.tag;
        	data.ext = info.ext;
        	data.dpi = info.dpi;
        	data.size = info.size;
        	data.definedName = info.name;
            //console.info('uploadBeforeSend', object, data, headers);
        });

        uploader.on('fileQueued', function (file) {

            uploader.md5File(file)

                // 及时显示进度
                .progress(function (percentage) {
                    var fileObj = $('#' + file.id);
                    var spanObj = fileObj.find('.progress_check>span').text(parseInt(percentage * 100));
                    //if (percentage == 1) {
                        //fileObj.find('.progress_check').hide();
                        //fileObj.find('.progress_check').attr('data-checkedcomplete', true).text('验证完成，等待上传').css('color', '#aaa');
                    //}
                    //console.log('Percentage:', percentage, file.getStatus(), file);
                })

                // 完成
                .then(function (val) {
                    ///////console.info('md5 result:', val, file);
                    $.extend(uploader.options.formData, { md5: val });

                    var fileObj = $('#' + file.id);
                    console.log(file.size);
                    $.ajax({
                        url: INTEROP_PATH.GetMaxChunk,
                        async: true,
                        data: { md5: val, ext: file.ext },
                        success: function (response) {
                            //////console.info('response', response);
                            var res = JSON.parse(response);
                            //$.extend(uploader.options.formData, { chunk: res.chunk }); 原来就备注的
                            Global.FileQueueds.push({ id: file.id, md5: val, size: file.size, ext: file.ext, chunk: res.data });
                        	//console.info('fileCheckMaxChunk', file, res.data);                           
                            var info = {};
                            info.name = (file.name).substring(0, (file.name).lastIndexOf('.'));
                            info.sysID = "";
                            info.tag = "";
                            info.ext = "."+file.ext;
                            info.dpi = "0";
                            info.size = file._info.width + "X" + file._info.height;
                            $('#' + file.id).attr('data-info', JSON.stringify(info));
                            fileObj.find('.progress_check').attr('data-checkedcomplete', true).text('验证完成，等待上传').css('color', '#aaa');
                            ////文件验证完成后自动触发上传
                            //uploader.upload();
                        }
                    });
                });

        });

        // 拖拽时不接受 js, txt 文件。
        uploader.on('dndAccept', function (items) {
            var denied = false,
                len = items.length,
                i = 0,
            // 修改js类型
                unAllowed = 'text/plain;application/javascript ';

            for (; i < len; i++) {
                // 如果在列表里面
                if (~unAllowed.indexOf(items[i].type)) {
                    denied = true;
                    break;
                }
            }

            return !denied;
        });

        uploader.on('dialogOpen', function () {
        });

        // uploader.on('filesQueued', function() {
        //     uploader.sort(function( a, b ) {
        //         if ( a.name < b.name )
        //           return -1;
        //         if ( a.name > b.name )
        //           return 1;
        //         return 0;
        //     });
        // });

        // 添加“添加文件”的按钮，
        uploader.addButton({
            id: '#filePicker2',
            label: '继续添加'
        });

        uploader.on('ready', function () {
            window.uploader = uploader;
        });

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {

        	var $li = $('<li id="' + file.id + '" data-info="">' +
                    '<p class="title">' + file.name + '</p>' +
                    '<p class="imgWrap"></p>' +
                    '<p class="progress"><span></span></p>' +
                    '<p class="progress_check" data-checkedcomplete="false">正在验证文件：<span>0</span>%</p>' +
                    '</li>'),
        	


                $btns = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +
                    '<span class="rotateRight">向右旋转</span>' +
                    '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
                $prgress = $li.find('p.progress span'),
                $wrap = $li.find('p.imgWrap'),
                $info = $('<p class="error"></p>'),

                showError = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;

                        case 'interrupt':
                            text = '上传暂停';
                            break;

                        default:
                            text = '上传失败，请重试';
                            break;
                    }

                    $info.text(text).appendTo($li);
                };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                // @todo lazyload
                $wrap.text('预览中');
                uploader.makeThumb(file, function (error, src) {
                    var img;

                    if (error) {
                        $wrap.text('不能预览');
                        $wrap.empty().append($('<img src="/html/lib/webuploader/images/image.png">'));
                        console.info('不能预览', src);
                        return;
                    }

                    if (isSupportBase64) {
                        img = $('<img src="' + src + '">');
                        $wrap.empty().append(img);
                    } else {
                        $.ajax('preview.ashx', {
                            method: 'POST',
                            data: src,
                            dataType: 'json'
                        }).done(function (response) {
                            if (response.result) {
                                img = $('<img src="' + response.result + '">');
                                $wrap.empty().append(img);
                            } else {
                                $wrap.text("预览出错");
                            }
                        });
                    }
                }, thumbnailWidth, thumbnailHeight);

                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    $li.off('mouseenter mouseleave');
                    $btns.remove();
                }

                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    console.log(file.statusText);
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    $info.remove();
                    $prgress.css('display', 'block');
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    $prgress.hide().width(0);
                    $li.append('<span class="success"></span>');
                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            $li.on('mouseenter', function () {
                $btns.stop().animate({ height: 30 });
            });

            $li.on('mouseleave', function () {
                $btns.stop().animate({ height: 0 });
            });

            $btns.on('click', 'span', function () {
                var index = $(this).index(),
                    deg;

                switch (index) {
                    case 0:
                        uploader.removeFile(file);
                        return;

                    case 1:
                        file.rotation += 90;
                        break;

                    case 2:
                        file.rotation -= 90;
                        break;
                }

                if (supportTransition) {
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                } else {
                    $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~ ~((file.rotation / 90) % 4 + 4) % 4) + ')');
                    // use jquery animate to rotation
                    // $({
                    //     rotation: rotation
                    // }).animate({
                    //     rotation: file.rotation
                    // }, {
                    //     easing: 'linear',
                    //     step: function( now ) {
                    //         now = now * Math.PI / 180;

                    //         var cos = Math.cos( now ),
                    //             sin = Math.sin( now );

                    //         $wrap.css( 'filter', "progid:DXImageTransform.Microsoft.Matrix(M11=" + cos + ",M12=" + (-sin) + ",M21=" + sin + ",M22=" + cos + ",SizingMethod='auto expand')");
                    //     }
                    // });
                }
            });

            $li.appendTo($queue);
            $li.on("onload", function () {
            	console.log(file._info.width);
            });
           
            $li.on("click", function () {
            	//////////////////////////////把图片信息填入输入框
            	var info = JSON.parse($('#' + file.id).attr('data-info'));
            	$queue.find('li').removeClass("ssi-choose");
            	$li.addClass("ssi-choose");
            	$(".icon-info").css("display", "block");
            	$("#picSize").html(info.size);
            	$("#picExt").html(info.ext);
            	$("#picName").val(info.name);
            	$("#img-tag").val(info.tag);
            	$("#picResolution").val(info.dpi);
            	tree($("#sys_tree"), $('.ssi-choose'));
            });
        }

        // 负责view的销毁
        function removeFile(file) {
            var $li = $('#' + file.id);

            delete percentages[file.id];
            updateTotalProgress();
            $li.off().find('.file-panel').off().end().remove();
        }

        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });

            percent = total ? loaded / total : 0;


            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        function updateStatus() {
            var text = '', stats;

            if (state === 'ready') {
                text = '选中' + fileCount + '个文件，共' +
                        WebUploader.formatSize(fileSize) + '。';
            } else if (state === 'confirm') {
                stats = uploader.getStats();
                if (stats.uploadFailNum) {
                    text = '已成功上传' + stats.successNum + '个文件，' +
                        stats.uploadFailNum + '个文件上传失败，<a class="retry" href="#">重新上传</a>失败文件或<a class="ignore" href="#">忽略</a>'
                }

            } else {
                stats = uploader.getStats();
                text = '共' + fileCount + '个（' +
                        WebUploader.formatSize(fileSize) +
                        '），已上传' + stats.successNum + '个';

                if (stats.uploadFailNum) {
                    text += '，失败' + stats.uploadFailNum + '个';
                }
            }

            $info.html(text);
        }

        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }

            $upload.removeClass('state-' + state);
            $upload.addClass('state-' + val);
            state = val;

            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $('#filePicker2').removeClass('element-invisible');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'uploading':
                    $('#filePicker2').addClass('element-invisible');
                    $progress.show();
                    $upload.text('暂停上传');
                    break;

                case 'paused':
                    $.each(uploader.getFiles(), function (idx, row) {
                        if (row.getStatus() == "progress") {
                            row.setStatus('interrupt');
                        }
                    })
                    //uploader.getFiles()[0].setStatus('interrupt');
                    $progress.show();
                    $upload.text('继续上传');
                    break;

                case 'confirm':
                    $progress.hide();
                    $('#filePicker2').removeClass('element-invisible');
                    $upload.text('开始上传');

                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        //console.info('finish', uploader, stats);
                        ////alert('上传完成');
                        //if (window.UploadSuccessCallback) {
                        //    window.UploadSuccessCallback();
                        //}
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }

            updateStatus();
        }

        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
            updateTotalProgress();
        };

        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            updateTotalProgress();
        };

        uploader.onFileDequeued = function (file) {
            fileCount--;
            fileSize -= file.size;

            if (!fileCount) {
                setState('pedding');
            }

            removeFile(file);
            updateTotalProgress();

        };

        //all算是一个总监听器
        uploader.on('all', function (type, arg1, arg2) {
            //console.log("all监听：", type, arg1, arg2);
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });

        // 文件上传成功,合并文件。
        uploader.on('uploadSuccess', function (file, response) {
            console.info('uploadSuccess', file, response, file.id);
            if (response && response.code >= 0) {
                var dataObj = response.data;
                var md5 = Global.GetFileQueued(file.id).md5;
                console.info('uploadSuccess2', md5);
                if (dataObj.chunked) {
                	var info = JSON.parse($('#' + file.id).attr("data-info"));
                	$.post(INTEROP_PATH.MergeFiles, { tag: info.tag, ext: info.ext, dpi: info.dpi, size: info.size, md5: md5, definedName: info.name },
                    function (data) {
                        data = $.parseJSON(data);
                        if (data.hasError) {
                            alert('文件合并失败！');
                        } else {
                            //alert(decodeURIComponent(data.savePath));
                            console.info('上传文件完成并合并成功，触发回调事件');
                            if (window.UploadSuccessCallback) {
                                window.UploadSuccessCallback(file, md5);
                            }
                        }
                    });
                }
                else {
                    console.info('上传文件完成，不需要合并，触发回调事件');
                    if (window.UploadSuccessCallback) {
                        window.UploadSuccessCallback(file, md5);
                    }
                }
            }
        });

        uploader.onError = function (code) {
            alert('Eroor: ' + code);
        };

        $upload.on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            if ($queue.find('.progress_check[data-checkedcomplete=false]').length > 0) {
                alert('请等待文件验证完成');
                return false;
            }
            else {
                $queue.find('.progress_check').hide();
            }

            if (state === 'ready') {
                uploader.upload();
            } else if (state === 'paused') {
                uploader.upload();
            } else if (state === 'uploading') {
                uploader.stop();
            }
        });

        $info.on('click', '.retry', function () {
            uploader.retry();
        });

        $info.on('click', '.ignore', function () {
            alert('todo');
        });

        $upload.addClass('state-' + state);
        updateTotalProgress();
    });
	//信息事件
    function tree($id,$imgID) {
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
    	$.jstree.destroy($id);
    	$id.jstree({
    		"core": {
    			"data": function (obj, callback) {
    				var jsonarray = "";
    				//查询项目信息
    				$.ajax({
    					type: "POST",
    					url: "/Folder/Show",
    					dataType: "json",
    					success: function (json) {
    						callback.call(this, getJsonTree(json.data, -1));
    					},
    					error: function (jqXHR) {
    						console.log(jqXHR.status);
    					}
    				});
    			},
    		},
    		"check_callback": true,
    		'types': {//这里就是菜单图标的显示格式
    			"default": {
    				"icon": "fa fa-folder tree-item-icon-color icon-lg"
    			},
    			"file": {
    				"icon": "fa fa-file tree-item-icon-color icon-lg"
    			}
    		},
    		'multiple': true,
    		'plugins': [  //插件，下面是插件的功能
				'types',//可以设置其图标，在上面的一样。
			    'checkbox'
    		],
    		"checkbox": {
    			"keep_selected_style": false,//是否默认选中
    			"three_state": false,//父子级别级联选择
    			"tie_selection": true
    		},
    	});
    	$id.on("changed.jstree", function (e, data) {
    		var checked = $("#sys_tree").jstree("get_checked");
    		addDataInfo($imgID, "data-info", "sysID", checked);
    	});
    	var info=JSON.parse($imgID.attr("data-info"))
    	$id.on("loaded.jstree", function (e, data) {//默认选中
    		var sys = info.sysID;
    		if (sys.length > 0) {
    			for (j = 0; j < sys.length; j++) {
    				var obj = data.instance.get_node(sys[j]);
    				data.instance.select_node(obj);
    			}
    		}
    	});
    }
	//标签改变事件
    $("#uploader-info").on('change', '#img-tag', function () {
    	var _ths = $(this);
    	var str = new RegExp("^[\u4e00-\u9fa5_a-zA-Z0-9\S]+(\\|?[\u4e00-\u9fa5_a-zA-Z0-9\S]+)+$|^[\u4e00-\u9fa5_a-zA-Z0-9\S]+$");//匹配tag格式
    	if (!str.test(_ths.val())) {
    		alert("验证错误，请检查！");
    	} else {
    		addDataInfo($(".ssi-choose"), "data-info", "tag", $(this).val());
    	}
    });
	//名称改变事件
    $("#uploader-info").on('change', '#picName', function () {
    	var _ths = $(this);
    	if (_ths.val() == "") {
    		alert("名称不能为空，请检查！");
    	} else {
    		addDataInfo($(".ssi-choose"), "data-info", "name", $(this).val());
    	}
    });
	//分辨率下拉框事件
    $("#uploader-info").on('change', '#picResolution', function () {
    	addDataInfo($(".ssi-choose"), "data-info", "dpi", $(this).val());
    });
	//对象的属性json值增加对象
    function addDataInfo(target, property, key, val) {
    	var ssiInfo = target,
		ssiData = JSON.parse(ssiInfo.attr(property));
    	ssiData[key] = val;
    	ssiInfo.attr(property, JSON.stringify(ssiData));
    }
})(jQuery);
