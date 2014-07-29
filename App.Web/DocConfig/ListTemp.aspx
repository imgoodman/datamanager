<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListTemp.aspx.cs" Inherits="App.Web.DocConfig.ListTemp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="header">
        <h1 class="pagetitle">
            临时文档对象列表</h1>
    </div>
    <div class="list">
        <div class="listitem itemtemp hidden">
            <ul class="docinfo">
                <li>
                    <label>
                        文档类别：</label>
                    <p class="docType">
                        docType</p>
                </li>
                <li>
                    <label>
                        文档名称：</label>
                    <p class="docName">
                        docName</p>
                </li>
                <li>
                    <label>
                        文档属性：</label>
                    <ul class="attrinfo">
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <div class="more">
        查看更多文档
    </div>
    <div id="loading_div" class="hidden">
        <p>
            loading.......</p>
    </div>
    <script type="text/javascript">
        function loadMore(pageIndex) {
            $.ajax({
                type: "POST",
                url: "/handlers/tempdocConfig/List.ashx",
                data: { pageIndex: pageIndex },
                dataType: "json",
                success: function (res) {
                    if (res != null) {
                        $.each(res, function (i, item) {
                            var $temp = $(".itemtemp").clone();
                            $temp.find(".docType").text(item.DocType.TypeName);
                            $temp.find(".docName").html("<a href='/DocConfig/StepAdd.aspx?ID=" + item.ID + "' title='点击查看详情' target='_blank'>" + item.DocName + "</a>");
                            if (item.Attrs.length > 0) {
                                $.each(item.Attrs, function (j, jitem) {
                                    $("<li><p>" + jitem.AttrName + "(" + showType(jitem.AttrType) + ")</p></li>").appendTo($temp.find(".attrinfo"));
                                });
                            } else {
                                $("<li><p>暂无属性</p></li>").appendTo($temp.find(".attrinfo"));
                            }
                            $temp.removeClass("hidden itemtemp").appendTo($(".list"));
                        });
                    } else {
                        $(".more").html("所有文档已经显示").unbind("click");
                    }
                }
            });
        };
        $(document).ready(function () {
            var pageIndex = 0;
            loadMore(pageIndex);
            //加载更多
            $(".more").click(function () {
                pageIndex++;
                loadMore(pageIndex);
            });
        }).ajaxStart(function () {
            //$("#loading_div").dialog();
        }).ajaxComplete(function () {
            //$("#loading_div").dialog("close");
        });
        
    </script>
</asp:Content>
