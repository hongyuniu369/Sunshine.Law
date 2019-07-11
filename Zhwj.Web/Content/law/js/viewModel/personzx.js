/**
* 模块名：com viewModel
* 程序名: com.viewModel.search.js
* Copyright(c) 2013-2015 hongyuniu
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.list = function (data) {
    var self = this;
    this.idField = data.idField || "Code";
    this.urls = data.urls;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;
    this.pagination = ko.mapping.fromJS(data.pagination);
    delete this.pagination.__ko_mapping__;
    this.items = ko.observableArray();
    this.refresh = function () {
        com.ajax({
            url: self.urls.query,
            type: 'GET',
            data: $.extend(ko.toJS(self.form),ko.toJS(self.pagination)),
            async: false,
            success: function (d) {
                self.items(ko.mapping.fromJS(d.rows)());
                $('#pp').pagination({
                    total: d.total, 
                    pageSize: self.pagination.rows(),
                    layout:['list','sep','first','prev','links','next','last','sep','refresh'],
                    onSelectPage: function (pageNumber, pageSize) {
                        self.pagination.page(pageNumber);
                        self.pagination.rows(pageSize); 
                        self.refresh();
                    }
                });
            }
        });
    };
    this.refresh();

    this.searchClick = function () {
        //alert(ko.toJS(self.form).toSource());
       // var param = ko.toJS(this.form);
        // this.grid.queryParams(param);
        this.refresh();
    };

    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };
    //获取回复
    this.getReply = function (a,b) {
        var cnt = $("#reply" + a);
        var btn = $("#btn" + a);
        if (cnt.css("display") == "none") {
            cnt.css("display", "block");
            btn.html("点击收起");
        } else {
            cnt.css("display", "none");
            btn.html("查看律师回复");
        }
        if (cnt.attr("loaded") == "") {
            cnt.attr("loaded", "loaded");
            $.ajax({
                type: "GET",
                url: "/api/law/getAdvisroyReply",
                data: { AdvisoryCode: a },
                dataType: "json",
                success: function (data) {
                    b(ko.mapping.fromJS(data.rows)());
                }
            });
        }
    }
};
