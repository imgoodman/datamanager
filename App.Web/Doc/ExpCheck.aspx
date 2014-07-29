<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ExpCheck.aspx.cs" Inherits="App.Web.Doc.ExpCheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            包含“所属试验”的文档实例列表</h1>
    </div>
    <div class="list" style="max-height: 370px; overflow: auto; padding: 5px;">
        <p>
            列表仅显示文档的部分属性，要查看全部属性，请点击链接查看详情</p>
        <table>
            <thead>
                <tr>
                    <th>
                        序号
                    </th>
                    <th>
                        文档实例名称
                    </th>
                    <th>
                        属性列表
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div class="more">
        查看更多
    </div>
    <div class="form_actions">
        <a href="#" class="btn btn_primary btn_large exp_update">一键更新</a>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var pIndex = 0;
            var rowIndex = 1;
            //get data
            function getData(pageIndex) {
                $.ajax({
                    type: "POST",
                    url: "/handlers/doc/getExpCheck.ashx",
                    data: { pageIndex: pageIndex },
                    dataType: "json",
                    success: function (res) {
                        if (res.length > 0) {
                            $.each(res, function (i, item) {
                                var $tb = $(".list table tbody");
                                var attrs = "";
                                $.each(item.Document.Attrs, function (j, jitem) {
                                    if (j == 0)
                                        attrs += jitem.AttrName + ":<a href='/doc/profile.aspx?id=" + item.ID + "' target='_blank'>" + jitem.TranValue + "</a>;";
                                    if (jitem.AttrName == "所属试验")
                                        attrs += jitem.AttrName + ":" + jitem.TranValue + ";";
                                });
                                $("<tr>").html("<td>" + rowIndex + "</td><td>" + item.Document.DocName + "</td><td>" + attrs + "...</td>").appendTo($tb);
                                rowIndex++;
                            });
                        }
                    }
                });
            }
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getExpChecktotal.ashx",
                success: function (res) {
                    $(".more").html("查看更多共(" + res + ")个");
                }
            });
            getData(0);
            //加载更多
            $(".more").click(function () {
                pIndex++;
                getData(pIndex);
            });
            //update
            $(".exp_update").click(function (e) {
                e.preventDefault();
                $.ajax({
                    type: "POST",
                    url: "/handlers/doc/updateExpCheck.ashx",
                    success: function (res) {
                        if (res == "1")
                            window.location.href = "/doc/expcheck.aspx";
                        else
                            InfoTip.showMessage("一键更新失败", "error", 3000);
                    }
                });
            });
        });
    </script>
</asp:Content>
