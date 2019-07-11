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
        $.ajax({
            url: self.urls.query,
            type: 'GET',
            data: $.extend(ko.toJS(self.form), ko.toJS(self.pagination)),
            async: false,
            success: function (d) {
                self.items(ko.mapping.fromJS(d.rows)());
                $('#pp').pagination({
                    total: d.total,
                    pageSize: self.pagination.rows(),
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
};
