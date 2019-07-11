/**
* 模块名：mms viewModel
* 程序名: link.js
**/

var viewModel = function () {
    var self = this;
    this.tree = {
        method: 'GET',
        url: '/api/cms/link/GetRootlink',
        queryParams: ko.observable(),
        loadFilter: function (d) {
            var filter = utils.filterProperties(d.rows || d, ['LinkCode as id', 'LinkName as text']);
            return [{ id: '', text: '所有类别', children: filter }];
        },
        onSelect: function (node) {
            self.ParentLinkCode(node.id);
        }
    };
    this.ParentLinkCode = ko.observable();
    this.grid = {
        size: { w: 189, h: 40 },
        url: "/api/cms/link",
        queryParams: ko.observable(),
        pagination: true,
        idField: 'LinkCode',
        treeField: 'LinkCode',
        //onBeforeLoad: function (param) {
        //    var treeElemet1 = $(self.tree.$element()[0]);
        //    if (treeElemet1.tree("getSelected").id != "")
        //        return true;
        //    else
        //        return false
        //},
        loadFilter: function (data) {
            data.rows = utils.toTreeData(data.rows, 'LinkCode', 'ParentLinkCode', "children");
            return data;
        }
    };
    this.gridEdit = new com.editTreeGridViewModel(this.grid);
    this.ParentLinkCode.subscribe(function (value) {//设置树形和treeGrid联动
        self.grid.queryParams({ ParentLinkCode: value });
    });

    this.refreshClick = function () {
        window.location.reload();
    };
    this.addClick = function () {
        if (!self.ParentLinkCode()) return com.message('warning', '请先在左边选择要添加的类别！');
        if (self.grid.onClickRow()) {
            com.ajax({
                type: 'GET',
                url: '/api/cms/link/getnewcode',
                success: function (d) {//设置新增行默认属性
                    var row = { ParentLinkCode: self.ParentLinkCode(), LinkCode: d, IsVisible: true, IsEnable: true, LinkOrder: 99 };
                    self.grid.treegrid('append', { parent: '', data: [row] });
                    self.grid.treegrid('select', row.LinkCode);
                    self.grid.$element().data("datagrid").insertedRows.push(row);
                    self.editClick();
                    //self.gridEdit.addnew(row);
                }
            });
        }
    };
    this.editClick = function () {
        var row = self.grid.treegrid('getSelected');
        if (row) {
            //从左侧树取得父节点数据
            var treeElemet = $(self.tree.$element()[0]);
            var rootNode = treeElemet.tree("getRoot");
            var treeData = JSON.parse(JSON.stringify(treeElemet.tree('getChildren', rootNode.target)));
            //var treeData = JSON.parse(JSON.stringify(self.grid.treegrid('getData')).replace(/_id/g, "id").replace(/MenuName/g, "text"));
            treeData.unshift({ "id": "", "text": "" });

            //设置上级菜单下拉树
            var gridOpt = $.data(self.grid.$element()[0], "datagrid").options;
            var col = $.grep(gridOpt.columns[0], function (n) { return n.field == 'ParentLinkCode' })[0];
            col.editor = { type: 'combotree', options: { data: treeData } };
            col.editor.options.onBeforeSelect = function (node) {
                var isChild = utils.isInChild(treeData, row.LinkCode, node.id);
                com.messageif(isChild, 'warning', '不能将自己或下级设为上级节点');
                return !isChild;
            };
            self.gridEdit.begin(row);
            self.edit_id = row.LinkCode; 
            var eds = self.grid.treegrid('getEditors', row.LinkCode);
            var edt = function (field) { return $.grep(eds, function (n) { return n.field == field })[0]; };
            self.afterCreateEditors(edt);
        }
    };
    this.afterCreateEditors = function (editors) {
        var iconInput = editors("LinkPic").target;
        var onShowPanel = function () {
            iconInput.lookup('hidePanel');//隐藏下拉面板
            var finder = new CKFinder();//弹出ckfinder
            finder.basePath = '/Content/js/jquery-plugin/ckfinder/';
            finder.startupPath = 'Images:/';
            finder.selectActionFunction = SetFileField;
            finder.popup();
            function SetFileField(fileUrl, data) {//赋值
                iconInput.lookup('setValue', fileUrl);
            }
        };
        iconInput.lookup({ customShowPanel: true, onShowPanel: onShowPanel, editable: true });
        iconInput.lookup('resize', iconInput.parent().width());
        iconInput.lookup('textbox').unbind();
    };
    this.grid.OnBeforeDestroyEditor = function (editors, row) {
        row.ParentLinkName = editors['ParentLinkCode'].target.combotree('getText');
        row.LinkPic = editors["LinkPic"].target.lookup('textbox').val();
    };
    this.deleteClick = self.gridEdit.deleterow;
    this.saveClick = function () {
        self.gridEdit.ended();
        var post = {};
        post.list = self.gridEdit.getChanges(['LinkCode', 'LinkName','LinkPic','LinkUrl', 'ParentLinkCode', 'LinkRemark', 'IsVisible', 'IsEnable', 'LinkOrder']);
        if (self.gridEdit.isChangedAndValid()) {
            com.ajax({
                url: '/api/cms/link/edit',
                data: ko.toJSON(post),
                success: function (d) {
                    com.message('success', '保存成功！');
                    self.grid.queryParams({ ParentLinkCode: self.ParentLinkCode() });
                    self.gridEdit.accept();
                }
            });
        }
    };
    this.grid.onDblClickRow = self.editClick;
    this.grid.onClickRow = self.gridEdit.ended;
    this.grid.OnAfterCreateEditor = function (edt) {//设置只读等属性
        com.readOnlyHandler('input')(edt["LinkCode"].target, true);
    };

    this.typeClick = function () {
        com.dialog({
            title: "&nbsp;留言类别",
            iconCls: 'icon-node_tree',
            width: 640,
            height: 410,
            html: "#type-template",
            viewModel: function (w) {
                var that = this;
                this.typeGrid = {
                    width: 626,
                    height: 340,
                    pagination: true,
                    pageSize: 10,
                    url: "/api/cms/link/getrootlink",
                    queryParams: ko.observable(),
                    idField: 'LinkCode',
                    treeField: 'LinkCode',
                    loadFilter: function (data) {
                        data.rows = utils.toTreeData(data.rows, 'LinkCode', 'ParentLinkCode', "children");
                        return data;
                    }
                };
                this.typeGridEdit = new com.editTreeGridViewModel(this.typeGrid);
                this.addTypeClick = function () {
                    com.ajax({
                        type: 'GET',
                        url: '/api/cms/link/getnewcode',
                        success: function (d) {//设置新增行默认属性
                            var row = { LinkCode: d, IsVisible: 'true', IsEnable: 'true', LinkOrder: 99 };
                            that.typeGridEdit.addnew(row);
                        }
                    });
                };
                this.editTypeClick = function () {
                    var row = that.typeGrid.treegrid('getSelected');
                    row.IsEnable = row.IsEnable.toString();
                    that.typeGridEdit.begin(row);
                };
                this.typeGrid.onClickRow = that.typeGridEdit.ended;
                this.typeGrid.onDblClickRow = that.editTypeClick;
                this.typeGrid.OnAfterCreateEditor = function (editors, row) {
                    //if (!row._isnew) com.readOnlyHandler('input')(editors["LinkCode"].target, true);
                    com.readOnlyHandler('input')(editors["LinkCode"].target, true);
                };
                this.typeGrid.toolbar = [
                    { text: '新增', iconCls: 'icon-add1', handler: that.addTypeClick }, '-',
                    { text: '编辑', iconCls: 'icon-edit', handler: that.editTypeClick }, '-',
                    { text: '删除', iconCls: 'icon-cross', handler: that.typeGridEdit.deleterow }
                ];
                this.confirmClick = function () {
                    if (that.typeGridEdit.isChangedAndValid()) {
                        var list = that.typeGridEdit.getChanges(['LinkCode', 'LinkName', 'LinkRemark', 'IsVisible', 'IsEnable', 'LinkOrder']);
                        com.ajax({
                            url: '/api/cms/link/edit/',
                            data: ko.toJSON({ list: list }),
                            success: function (d) {
                                that.cancelClick();
                                self.tree.$element().tree('reload');
                                com.message('success', '保存成功！');
                            }
                        });
                    }
                };
                this.cancelClick = function () {
                    w.dialog('close');
                };
            }
        });
    };
};