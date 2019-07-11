//首页登录
var viewModel = function () {
    var self = this;
    this.form = {
        usercode: ko.observable(),
        password: ko.observable(),
        remember: ko.observable(false),
        ip: null,
        city: null,
    };
    this.message = ko.observable();
    this.loginClick = function (form) {
        if (!self.form.password())
            self.form.password($('[type=password]').val());
        $.ajax({
            type: "POST",
            url: "/login/doAction",
            data: ko.toJSON(self.form),
            dataType: "json",
            contentType: "application/json",
            success: function (d) {
                if (d.status == 'success') {
                    self.message("登陆成功");
                    self.checkLogin();
                } else {
                    self.message(d.message);
                }
                alert(self.message());
            },
            error: function (e) {
                self.message(e.responseText);
            },
            beforeSend: function () {
                $(form).find("input").attr("disabled", true);
                self.message("正在登陆处理，请稍候...");
            },
            complete: function () {
                $(form).find("input").attr("disabled", false);
            }
        });
    };

    this.resetClick = function () {
        self.form.usercode("");
        self.form.password("");
        self.form.remember(false);
    };

    this.init = function () {
        var ILData = ILData || [];
        self.form.ip = ILData[0];
        $.getJSON("http://api.map.baidu.com/location/ip?ak=F454f8a5efe5e577997931cc01de3974&callback=?", function (d) {
            self.form.city = d.content.address;
        });
        if (top != window) top.window.location = window.location;
        self.checkLogin();
    };


    //点击登录按钮
    this.login = function (para) {
        if (para == 1) {
            self.form.usercode($("#usercode1").val());
            self.form.password($("#password1").val());
        } else if (para == 2) {
            self.form.usercode($("#usercode2").val());
            self.form.password($("#password2").val());
        }

        self.loginClick();
        
    }
    //登录成功
    this.logined = function (para) {
       
        var lgnMsgCnt = $("<div></div");
        lgnMsgCnt.attr("class", "lgnMsgCnt");
        lgnMsgCnt.html(para + " " + "<a href='/law/Person'>个人中心</a>" + "   <a href='/Login/LogoutIndex'>退出登录</a>");
        $("#loginForm").html("");
        lgnMsgCnt.appendTo($("#loginForm"));
    }
    //判断是否登录
    this.checkLogin = function () {
        $.ajax({
            type: "POST",
            url: "/login/checkLogin",
            dataType: "json",
            contentType: "application/json",
            success: function (d) {
                if (d.state) {
                    self.logined(d.usercode);
                } else {
                   
                }
               
            },
            error: function (e) {

            },
            beforeSend: function () {

            },
            complete: function () {
                
            }
        });
    }

    this.init();
};

$(function () { ko.applyBindings(new viewModel(),document.getElementById("loginForm"));});