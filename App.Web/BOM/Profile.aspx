<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Profile.aspx.cs" Inherits="App.Web.BOM.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            清单详情</h1>
        <a href="#" class="btn btn_primary abs" title="删除该文档">编辑</a>
    </div>
    <fieldset class="form">
        <legend>清单基本信息</legend>
        <div class="control_group">
            <label class="control_label">
                清单名称</label>
            <div class="controls" style="padding-top: 4px;">
                <label class="bomname">
                    ddd</label>
            </div>
        </div>
        <div class="control_group">
            <label class="control_label">
                描述</label>
            <div class="controls" style="padding-top: 4px;">
                <label class="desc">
                    ddd</label>
            </div>
        </div>
    </fieldset>
    <fieldset class="form list maindoc">
        <legend>主文档对象</legend>
    </fieldset>
    <fieldset class="form list reldocs">
        <legend>相关文档对象</legend>
    </fieldset>
    <div class="item_temp hidden">
        <ul class="docattrinfo">
            <li>
                <label>
                    文档对象:</label><span class="docname">docname</span></li>
            <li>
                <label>
                    文档属性:</label><span class="surnames"></span> </li>
        </ul>
    </div>
    <div class="form_actions">
        <a href="javascript:void(0);" class="btn btn_primary btn_large btn_del">删除</a>
    </div>
    <div id="confirm_div" class="hidden">
        <p>
        </p>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var id = request("id");
            $(".header a").prop({ "href": "/Bom/Add.aspx?id=" + id });
            $.ajax({
                type: "POST",
                url: "/handlers/bom/getitembyid.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    $(".bomname").html(res.Name);
                    $(".desc").html(showString(res.Description));
                    var str = "";
                    if (res.MainDoc != null) {
                        var $temp = $(".item_temp").clone();
                        $temp.find(".docname").html(res.MainDoc.DocName);
                        str = "";
                        $.each(res.MainDoc.RelatedDocAttrs, function (i, item) {
                            str += item.AttrName + "(" + item.Surname + ");";
                        });
                        $temp.find(".surnames").html(str);
                        $temp.removeClass("item_temp hidden").addClass("listitem").appendTo($(".maindoc"));
                    } else {
                        $("<p>").addClass("red").html("暂无").appendTo($(".maindoc"));
                    }
                    if (res.RelatedDocs.length > 0) {
                        $.each(res.RelatedDocs, function (i, item) {
                            var $temp = $(".item_temp").clone();
                            $temp.find(".docname").html(item.DocName);
                            str = "";
                            $.each(item.RelatedDocAttrs, function (j, jitem) {
                                str += jitem.AttrName + "(" + jitem.Surname + ");";
                            });
                            $temp.find(".surnames").html(str);
                            $temp.removeClass("item_temp hidden").addClass("listitem").appendTo($(".reldocs"));
                        });
                    } else {
                        $("<p>").addClass("red").html("暂无").appendTo($(".reldocs"));
                    }
                }
            });
            //delete
            $(".btn_del").click(function (e) {
                e.preventDefault();
                var $div = $("#confirm_div");
                $div.find("p").html("您确定删除该清单吗?");
                $div.dialog({
                    title: "删除清单",
                    modal: true,
                    width: 600,
                    minHeight: 100,
                    buttons: {
                        "确认": function () {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/bom/deletebom.ashx",
                                data: {
                                    id: id
                                },
                                success: function (res) {
                                    if (res == "1") {
                                        window.location.href = "/BOM/List.aspx";
                                    } else {
                                        InfoTip.showMessage("删除清单失败", "error", 6000);
                                    }
                                }
                            });
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
        });
    </script>
</asp:Content>
