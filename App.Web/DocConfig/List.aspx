<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="App.Web.DocConfig.List" %>

<%@ Register Src="../Controls/DocTypeSelect.ascx" TagName="DocTypeSelect" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            文档对象列表</h1>
        <a href="/DocConfig/GridViewList.aspx" class="btn abs">表格视图</a>
    </div>
    <div id="search_div">
        <fieldset class="form relative">
            <legend>查询</legend>
            <div class="control_group">
                <label class="control_label">
                    文档类型</label>
                <div class="controls">
                    <uc1:DocTypeSelect ID="DocTypeSelect1" runat="server" />
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档名称</label>
                <div class="controls">
                    <input type="text" class="input_medium docname" />
                </div>
            </div>
            <a href="javascript:void(0);" class="btn btn_primary search abs searchBtn" title="根据文档类型和名称来查询">查询</a>
        </fieldset>
    </div>
    <div class="list">
        <div class="itemtemp hidden">
            <a href="#" class="hidden abs">详情</a>
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
    <script type="text/javascript">
        function loadMore(pageIndex) {
            $.ajax({
                type: "POST",
                url: "/handlers/docConfig/List.ashx",
                data: { pageIndex: pageIndex, docTypeID: $("#selDocType").val(), docName: $(".docname").val() },
                dataType: "json",
                success: function (res) {
                    if (res != null) {
                        $.each(res, function (i, item) {
                            var $temp = $(".itemtemp").clone();
                            $temp.find(".docType").text(item.DocType.TypeName);
                            $temp.find(".abs").attr({ "href": "/DocConfig/Profile.aspx?ID=" + item.ID });
                            $temp.find(".docName").html("<a href='/DocConfig/Profile.aspx?ID=" + item.ID + "' title='点击查看详情' target='_blank'>" + item.DocName + "</a>");
                            if (item.Attrs.length > 0) {
                                var str = "";
                                $.each(item.Attrs, function (j, jitem) {
                                    str += jitem.AttrName + "(" + showType(jitem.AttrType)+");";
                                    //$("<li><p>" + jitem.AttrName + "(" + showType(jitem.AttrType) + ")</p></li>").appendTo($temp.find(".attrinfo"));
                                });
                                $("<li><p>" + str + "</p></li>").appendTo($temp.find(".attrinfo"));
                            } else {
                                $("<li><p>暂无属性</p></li>").appendTo($temp.find(".attrinfo"));
                            }
                            $temp.removeClass("hidden itemtemp").addClass("listitem relative").appendTo($(".list"));
                        });
                        $(".more").html("查看更多");
                    } else {
                        $(".more").html("所有文档已经显示");
                    }
                }
            });
        };
        $(document).ready(function () {
            $("#isVirtual").val("2");
            var pageIndex = 0;
            loadMore(pageIndex);
            //加载更多
            $(".more").click(function () {
                pageIndex++;
                loadMore(pageIndex);
            });
            $(".search").click(function (e) {
                e.preventDefault();
                $(".list .listitem").remove();
                pageIndex = 0;
                loadMore(pageIndex);
            });
        });
        
    </script>
</asp:Content>
