<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="App.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录适航工程中心取证数据管理系统</title>
    <link rel="Stylesheet" href="/resources/css/login.css" />
    <link rel="Stylesheet" href="/resources/css/reset.css" />
    <script type="text/javascript" src="/resources/js/jquery.app.js"></script>
    <script type="text/javascript" src="/resources/js/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="topnav">
        <div>
            <img src="/resources/img/flogo.jpg" alt="适航工程中心数据管理系统" />
            <span class="title">适航工程中心取证数据管理系统</span>
        </div>
    </div>
    <div id="login-container">
        <div id="login-header">
            <h3>
                登录适航工程中心取证数据管理系统</h3>
        </div>
        <!-- /login-header -->
        <div id="login-content" class="clearfix">
            <p class="message hidden">
                xxxx</p>
            <fieldset>
                <div class="control-group">
                    <label class="control-label" for="username">
                        用户名</label>
                    <div class="controls">
                        <input type="text" class="" id="username" />
                    </div>
                </div>
                <div class="control-group">
                    <label class="control-label" for="password">
                        密码</label>
                    <div class="controls">
                        <input type="password" class="" id="password" />
                    </div>
                </div>
            </fieldset>
            <div class="pull-right">
                <button type="submit" class="btn btn-warning btn-large">
                    登录
                </button>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var rtnUrl = request("rtnUrl");
            var rattrid = request("rattrid");
            //console.log(rattrid);
            $("button[type=submit]").click(function (e) {
                e.preventDefault();
                var username = $("#username").val();
                var password = $("#password").val();
                if (username == "" || password == "") {
                    $(".message").removeClass("hidden").text("请输入用户名和密码。");
                } else {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/user/login.ashx",
                        data: { username: username, password: password },
                        dataType: "",
                        success: function (res) {
                            if (res == "0") {
                                $(".message").removeClass("hidden").text("您输入的用户名和密码不匹配。");
                            } else {
                                window.location.href = "/auth.aspx?uid=" + res + (rtnUrl == "" ? "" : "&rtnUrl=" + rtnUrl) + (rattrid == "" ? "" : "&rattrid=" + rattrid);
                            }
                        }
                    });
                }
            });
        });
    </script>
    </form>
</body>
</html>
