/**
* 模块名：mms viewModel
* 程序名: mms.viewModel.edit.js
* Copyright(c) 2013-2015 liuhuisheng [ liuhuisheng.xm@gmail.com ] 
**/
var com = com || {};
com.viewModel = com.viewModel || {};



com.viewModel.edit = function (data) {
    var self = this;
    
    //常量
    this.tabConst = { grid: 'grid', form: 'form', tab: 'tab', edit: 'gridEdit' };

    //设置
    this.urls = data.urls;                                      //api服务地址
    this.resx = data.resx;                                      //中文资源
    this.form = data.form;
    this.tabs = data.tabs;

    //数据
    this.dataSource = data.dataSource;                          //下拉框的数据源
    this.pageData = ko.mapping.fromJS(data.dataSource.pageData);//页面数据
    //撤销
    this.rejectClick = function () {
        ko.mapping.fromJS(data.dataSource.pageData, {}, self.pageData);
        com.message('success', self.resx.rejected);
    };
    //刷新
    this.refreshClick = function () {
        window.location.reload();
    };

    this.fnIsPageChanged = function () {
        var result = { form: {}, tabs: [], _changed: false };

        result.form = com.formChanges(self.pageData.form, data.dataSource.pageData.form, self.form.primaryKeys);
        result._changed = result._changed || result.form._changed;

        return result;
    };

    this.fnIsPageValid = function () {
        var formValid = com.formValidate();
        if (!formValid)
            return '验证不通过，数据未保存！';

        for (var i in self.tabs) {
            var tab = self.tabs[i], tabData;
            if (tab.type == self.tabConst.grid) {
                var edit = self[self.tabConst.edit + i];
                if (!edit.ended())
                    return '第' + i + '个页签中验证不通过';
            }
            else if (tab.type == self.tabConst.form) {
            }
        }

        return '';
    };

    //初始化tabs
    this.init = function () {
        //pageData
        if (self.fnIsNew()) {
            self.pageData.form = ko.mapping.fromJS(self.form.defaults);
        }
    };

    this.fnIsNew = function () { return data.dataSource.pageData.form == null; };
    this.init();
    this.readonly = ko.observable(false);
    this.isnew = ko.computed(function () { return self.fnIsNew(); });

    //保存
    this.saveClick = function () {
        //取得数据
        var post = self.fnIsPageChanged();
        if (!post._changed) {
            com.message('success', '页面没有数据被修改！');
            return;
        }

        //数据提交
        self.readonly(true);
        com.ajax({
            showLoading:false,
            url: self.urls.edit,
            data: ko.toJSON(post),
            success: function (d) {
                com.message('success', "提交成功");
            },
            error: function (e) {
                self.readonly(false);
                com.message('warning', e.responseText);
            }
        });
    };

    //login
    this.user = {
        usercode: ko.observable(""),
        password: ko.observable(""),
        remember: ko.observable(false)
    };
    this.loginClick = function (form) {
        $.ajax({
            type: "POST",
            url: "/login/doAction",
            data: ko.toJSON(self.user),
            dataType: "json",
            contentType: "application/json",
            success: function (d) {
                if (d.status == 'success') {
                    self.saveClick();
                    //window.location.href = '/admin/';
                } else {
                    com.message('warning', d.message);
                }
            },
            error: function (e) {
                com.message('warning', e.responseText);
            }
        });
    };
    this.submitClick = function () {
        //数据验证
        var validMessage = self.fnIsPageValid();
        if (validMessage) {
            com.message('warning', validMessage);
            return;
        }
        if (!self.user.password())
            self.user.password($('[type=password]').val());
        if (self.user.password() && self.user.usercode()) {
            self.loginClick();
        }
        else {
            self.saveClick();
        }
    };
};
