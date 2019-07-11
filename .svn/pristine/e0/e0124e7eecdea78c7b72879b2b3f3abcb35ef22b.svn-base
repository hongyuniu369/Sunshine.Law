/**
* 模块名：com viewModel
* 程序名: com.viewModel.search.js
* Copyright(c) 2013-2015 hongyuniu
**/
var com = com || {};
com.viewModel = com.viewModel || {};

com.viewModel.razorList = function (data) {
    var self = this;
    this.urls = data.urls;
    this.dataSource = data.dataSource;
    this.form = ko.mapping.fromJS(data.form);
    delete this.form.__ko_mapping__;
    this.pagination = ko.mapping.fromJS(data.pagination);
    delete this.pagination.__ko_mapping__;

    this.refresh = function () {
        $('#pp').pagination({
            total: Number(self.dataSource.total),
            pageSize: Number(self.pagination.rows()),
            pageNumber: Number(self.pagination.page()) > Math.ceil(Number(self.dataSource.total) / Number(self.pagination.rows())) ? 1 : Number(self.pagination.page()),
            layout: ['list', 'sep', 'first', 'prev', 'links', 'next', 'last', 'sep', 'refresh'],
            onSelectPage: function (pageNumber, pageSize) {
                self.pagination.page(pageNumber);
                self.pagination.rows(pageSize);
                self.doSearch();
            }
        });
    };
    self.refresh();

    this.doSearch = function () {
        var param = $.extend(ko.toJS(self.form), ko.toJS(self.pagination));
        var str = $.param(param);
        window.location.href = self.urls.query + "?" + str;
    };

    this.searchClick = function () {
        self.pagination.page(1);
        self.doSearch();
    };

    this.clearClick = function () {
        $.each(self.form, function () { this(''); });
        $.each(self.pagination, function () { this(''); });
        this.searchClick();
    };

    this.refreshClick = function () {
        window.location.reload();
    };
};
