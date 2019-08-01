// Creation of data model
Ext.define('StudentDataModel', {
	extend: 'Ext.data.Model',
	fields: [
	{ name: 'TaskListName', mapping: 'TaskListName' },
	{ name: 'Remark', mapping: 'Remark' },
	{ name: 'PictureSize', mapping: 'PictureSize' },
	{ name: 'PictureResolution', mapping: 'PictureResolution' },
	{ name: 'PictureBackground', mapping: 'PictureBackground' },
	]
});

////分页展示任务信息
//$.ajax({
//	type: "POST",
//	url: "/TaskList/Show",
//	dataType: "json",
//	async: true,
//	data: { "PageIndex": 1, "PageSize": 20 },
//	success: function (json) {

//	},
//	error: function (jqXHR) {
//		console.log(jqXHR.status);
//	}
//});

var itemsPerPage = 7;
// Store data
var myData = [
   //{ TaskListName: "", Remark: "" },
   //{ TaskListName: "", Remark: "" },
   //{ TaskListName: "", Remark: "" }
];
// Creation of first grid store


//Ext.Ajax.request({
//	url: "/TaskList/Show",
//	params: { "PageIndex": 1, "PageSize": 20 },
//	method: 'POST',
//	dataType:'json',
//	success: function (json) {
//		var jsonlist = JSON.parse(json.responseText);
//		var pushData = {};
//		$.each(jsonlist.List, function (i, val) {
//			pushData.TaskListName = val.TaskListName;
//			pushData.Remark = val.Remark;
//			myData.push(pushData);
//		});
//		console.log(JSON.stringify(myData));
//		pData = JSON.stringify(myData);
//		console.log(pData);
//	},
//	failure: function (resp, opts) {
//		var respText = Ext.util.JSON.decode(resp.responseText);
//		Ext.Msg.alert('错误', respText.error);
//	}

//});

var gridStore = Ext.create('Ext.data.Store', {
	model: 'StudentDataModel',
	autoLoad: false,
	data: myData
	//pageSize: itemsPerPage,

});


//gridStore.load({
//	params: {
//		start: 0,
//		limit: itemsPerPage//分页条数
//	}
//});
//定义一个combobox，以便于在分页工具中添加
var combo = Ext.create('Ext.form.ComboBox', {
	name: 'pagesize',
	hiddenName: 'pagesize',
	store: new Ext.data.ArrayStore({
		fields: ['text', 'value'],
		data: [['20', 20], ['40', 40], ['60', 60],
				['80', 80], ['100', 100], ['200', 200]]
	}),
	valueField: 'value',
	displayField: 'text',
	emptyText: 20,
	width: 50
});
////给combobox加上change事件
//combo.on("select", function (comboBox) {
//	var pagebar = Ext.getCmp('pagebar');//获取分页工具组件
//	pagebar.pageSize = 20;//parseInt(comboBox.getValue());//修改分页工具组件的pagingsize
//	itemsPerPage = 20;//parseInt(comboBox.getValue());//给分页条数赋值，这里你可以定义一个全局的分页条数，每次在这里更改他的值
//	gridStore.limit = itemsPerPage;//将store里面的limit值也改掉
//	gridStore.pageSize = itemsPerPage;//同理改掉store里面的pagesize值。这个很重要！！！！
//	gridStore.loadPage(1);//不管是在哪个页面，都让store去显示第一个页面，加载第一个页面的数据
//})

//定义分页
var pagebar = Ext.create("Ext.toolbar.Paging", {
	store: gridStore,
	pageSize: itemsPerPage,
	displayInfo: true,
	dock: 'bottom',
	displayMsg: "显示{0}-{1}条,共计{2}条",
	items: ['-', '每页显示', combo, '条'],
	emptyMsg: "没有数据",
	listeners: {
		change: function (paging, pageData) {
			start = (pageData.currentPage - 1) * itemsPerPage;
			total = pageData.total;
		}
	}
});

var cols = [
	//new Ext.grid.RowNumberer(),
					 {
					 	text: '任务清单名称',
					 	width: 250,
					 	dataIndex: 'TaskListName',
					 	editor: {
					 		xtype: 'textfield',
					 		allowBlank: false
					 	}
					 }, {
					 	text: '备注',
					 	width: 200,
					 	dataIndex: 'Remark',
					 	editor: {
					 		xtype: 'textfield',
					 		enableKeyEvents: false,
					 	}
					 }, {
					 	text: '图片尺寸',
					 	width: 200,
					 	dataIndex: 'PictureSize',
					 	editor: {
					 		xtype: 'textfield',
					 		enableKeyEvents: false,
					 	}
					 }, {
					 	text: '图片分辨率',
					 	width: 200,
					 	dataIndex: 'PictureResolution',
					 	editor: {
					 		xtype: 'textfield',
					 		enableKeyEvents: false,
					 	}
					 }, {
					 	text: '图片背景色',
					 	width: 200,
					 	dataIndex: 'PictureBackground',
					 	editor: {
					 		xtype: 'textfield',
					 		enableKeyEvents: false,
					 	}
					 }
];
// Creation of first grid
//Ext.create('Ext.grid.Panel', {
//   id                : 'gridId',
//   store: gridStore,
//   stripeRows        : true,
//   title             : '任务文件管理清单', // Title for the grid
//   renderTo          :'gridTable', // Div id where the grid has to be rendered
//   width             : 600,
//   collapsible       : true, // property to collapse grid
//   enableColumnMove  :true, // property which alllows column to move to different position by dragging that column.
//   enableColumnResize:true, // property which allows to resize column run time.
//   columns           :
//      [{
//         header: "Student Name",
//         dataIndex: 'name',
//         id : 'name',
//         flex:  1,  		 // property defines the amount of space this column is going to take in the grid container with respect to all.
//         sortable: true, // property to sort grid column data.
//         hideable: true,  // property which allows column to be hidden run time on user request.
//         editor:new Ext.Editor(new Ext.form.TextField({
//         	allowBlank:false //不能为空，为空无法保存
//         }))
//      },{
//         header: "Age",
//         dataIndex: 'age',
//         flex: .5,
//         sortable: true,
//          hideable: false // this column will not be available to be hidden.
//      },{
//         header: "Marks",
//         dataIndex: 'marks',
//         flex: .5,
//         sortable: true,
//         // renderer is used to manipulate data based on some conditions here who ever has marks greater than 75 will be displayed as 'Distinction' else 'Non Distinction'.
//         renderer :  function (value, metadata, record, rowIndex, colIndex, store) {
//            if (value > 75) {
//               return "Distinction";
//            } else {
//               return "Non Distinction";
//            }
//         }
//      }],
//	bbar: pagebar
//});
var rowEditing = Ext.create('Ext.grid.plugin.RowEditing', {
	saveBtnText: '保存',
	cancelBtnText: "取消",
	autoCancel: false,
	clicksToMoveEditor: 1, //双击进行修改  1-单击   2-双击    0-可取消双击/单击事件
	listeners: {
		edit: function (editor, e) {
			//e.record.commit();
			/*var myMask = new Ext.LoadMask(Ext.getBody(), {
						   msg: '正在修改，请稍后...',
						   removeMask: true     //完成后移除
			 });
			myMask.show();*/
			//console.info(e.record);
			//e.context.record为更改的这行的数据，某个值可以用get方法，比如下面
			var id = e.record.get('TaskListName'); //比如修改了id，在这里就可以获取id
			//e.context.record.fields.items为修改的这行字段名，这是一个数组集合,e.context.record.fields.items[0].name为第一列的名称，以此类推
			// 更新提示界面(供调试使用)
			Ext.Msg.alert('您成功修改信息', "修改的内容是:" + e.record + "n 修改的字段是:" + e.record.fields.items[1].name + " 修改的id为" + id);//取得更新内容  
		}
		//当然这里你也可以自定义一个ajax来提交到后台，大家自由发挥，这里不多写。
	}
});

var cellEditing = Ext.create('Ext.grid.plugin.CellEditing', {
	clicksToEdit: 1
});
var internalId;

function rowdblclickFn(grid, rowindex, e) {
	grid.getSelectionModel().each(function (rec) {
		alert(rec.get(TaskListName)); //fieldName，记录中的字段名     
	});
}

// 添加行
function onAddRow() {
	gridTable.add({});
};

// 删除行
function onDelRow() {
	gridTable.removeAt(gridStore.count() - 1);
};

// 添加列
function onAddColumn() {
	cols.push({
		text: '列',
		dataIndex: 'col',
		width: 120,
		sortable: false,
		menuDisabled: true,
	});

	gridTable.reconfigure(gridStore, cols);
};

// 删除列
function onDelColumn() {
	cols.pop()
	gridTable.reconfigure(gridStore, cols);
};
Ext.application({
	name: 'Ext6 Grid示例',
	launch: function () {
		var grid = Ext.create('Ext.grid.Panel', {
			id: 'grid',
			renderTo: gridTable,
			//selType: 'rowmodel',//'cellmodel',
			selType: 'cellmodel',
			plugins: [cellEditing],//rowEditing
			store: gridStore,
			columnLines: true,
			width: "100%",
			height:"250px",
			//frame: true,
			forceFit: true,
			fixed: true,
			//height: 200,
			title: '子任务列表',
			columns: cols,
			stripeRows: true,
			viewConfig: {
			markDirty: false
		},
			//分页功能
			//bbar: pagebar,
			tbar: [{
				text: '添加',
				handler: function () {
					var data = [{
						'TaskListName': '',
						'Remark': '',
					}];
					//console.log(Ext.getCmp('grid').getSelected().getSelection().data);
					gridStore.insert(0, data);//可以自定义在stroe的某个位置插入一行数据。
					//gridStore.loadData(data, true);//在store的最后添加一行数据
					//rowEditing.cancelEdit();
					//rowEditing.startEdit(0, 0);
				}
			},
			{
				itemId: 'removeUser',
				text: '删除',
				handler: function () {
					Ext.MessageBox.confirm('Confirm', '确定删除该记录?', function (btn) {
						if (btn != 'yes') {
							return;
						}
						var sm = Ext.getCmp('grid').getSelectionModel();
						//rowEditing.cancelEdit();
						gridStore.remove(sm.getSelection());
						if (gridStore.getCount() > 0) {
							sm.select(0);
						}
					});
				}
			}],
			//selModel: Ext.create('Ext.selection.RowModel', { mode: "SIMPLE" }),
			listeners: {
				selectionchange: function (model, records) {
					if (records != 'undefined' && records.length > 0) {
						internalId = records[0].internalId;      //获取行数据  
						//console.log(internalId);

						var Read = records[0].data.Read;        //获取行数据  
						if (Read == "true") {
							//alert("true");
						} else {
							//alert("false");
						}
					}
				}
			}
			//分页功能-效果同上
			//dockedItems: [{
			//	xtype: 'pagingtoolbar',
			//	store: gridStore,
			//	dock: 'bottom',
			//	displayInfo: true,
			//}]
		});

	}
});

Ext.get("publishBtn").on("click", add);

function add() {
	var store = Ext.getCmp('grid').getStore(),
		TaskList = [],
        TaskName = Ext.get('taskName').getValue(),
		Remark = Ext.get('remark').getValue();

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
			var TaskID = json.data;
			for (var i = 0; i < store.getCount() ; i++) {//遍历每一行
				var TaskListName = store.getAt(i).data.TaskListName,
					Remark = store.getAt(i).data.Remark;
				if (TaskListName.length != 0 && Remark.length !=0) {
					var pushData = {};
					pushData.TaskListName = TaskListName;
					pushData.Remark = Remark;
					pushData.TaskUID = TaskID;
					TaskList.push(pushData);
				}
			}
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
}
