//======================================================================
//
//        Copyright (C) 2010-2011 HEBCCC All rights reserved   
//        Guid: $guid1$
//        CLR Version: $clrversion$
//        Machine Name: $machinename$
//        Registered Organization: $registeredorganization$
//        File Name: $safeitemname$
//        Discription: 
//
//        Created by Hongyuniu at  $time$
//        http://hongyuniu.cnblogs.com
//
//======================================================================
$(document).ready(function () {
    var url = "http://wenfa.hebnews.cn/";
    //var url = "http://localhost:56505/";

    var zxViewModel = {
        items: ko.observableArray()
    };
    $.ajax({
        type: 'GET',
        url: url + 'api/h5/GetJsonp?rows=8',
        async: false,
        dataType: "jsonp",
        jsonp: "callback", //服务端用于接收callback调用的function名的参数
        jsonpCallback: "callback1", //callback的function名称
        success: function (d) {
            zxViewModel.items(ko.mapping.fromJS(d.rows)());
        }
    });

    ko.applyBindings(zxViewModel, document.getElementById('ygwf_zx'));

    $(".ygwf_zxzx").attr("href",url+"h5/list");
    $(".ygwf_home").attr("href", "http://tousu.hebnews.cn/node_117284.htm");
    $(".ygwf_query").attr("href", url + "h5/query");
    $(".ygwf_wyzx").attr("href", url + "h5/wyzx");
    $(".ygwf_lawyer").attr("href", url + "h5/lawyer");
    $(".ygwf_firm").attr("href", url + "h5/firm");
});
function formatDate(v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
    }
    var o = {
        "M+": d.getMonth() + 1,  //month
        "d+": d.getDate(),       //day
        "h+": d.getHours(),      //hour
        "m+": d.getMinutes(),    //minute
        "s+": d.getSeconds(),    //second
        "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
        "S": d.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};
