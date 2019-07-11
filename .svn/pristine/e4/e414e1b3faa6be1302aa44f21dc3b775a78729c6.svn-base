var setButton = function (firmCode) {
    com.dialog({
        title: "设置按钮",
        width: 555,
        height: 400,
        html: "#button-template",
        viewModel: function (w) {
            var self = this;
            com.loadCss('/Content/css/metro/css/modern.css', parent.document);
            this.buttons = ko.observableArray();
            this.refresh = function () {
                com.ajax({
                    url: '/api/msg/firm/GetFirmClasses/' + firmCode,
                    type: 'GET',
                    async: false,
                    success: function (d) {
                        self.buttons(ko.mapping.fromJS(d)());
                    }
                });
            };
            this.refresh();
            this.checkAll = ko.observable(false);
            this.checkAll.subscribe(function (value) {
                $.each(self.buttons(), function () {
                    this.Selected(value ? 1 : 0);
                });
            });
            this.buttonClick = function (row) {
                row.Selected(row.Selected() ? 0 : 1);
            };
            this.confirmClick = function () {
                var data = utils.filterProperties($.grep(self.buttons(), function (row) {
                    return row.Selected() > 0;
                }), ['ClassCode']);
                com.ajax({
                    url: '/api/msg/firm/editfirmclasses/' + firmCode,
                    data: ko.toJSON(data),
                    success: function (d) {
                        com.message('success', '保存成功！');
                        self.cancelClick();
                    }
                });
            };
            this.cancelClick = function () {
                w.dialog('close');
            };
        }
    });
};