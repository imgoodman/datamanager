﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Forward.aspx.cs" Inherits="App.Web.BOM.Forward" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            请选择要查看的清单对象</h1>
    </div>
    <div id="search_div">
        <fieldset class="form relative">
            <legend>查询</legend>
            <div class="control_group">
                <label class="control_label">
                    清单名称</label>
                <div class="controls">
                    <input type="text" class="input_medium bomname" />
                </div>
            </div>
            <a href="javascript:void(0);" class="btn btn_primary search abs searchBtn" title="根据名称来查询">
                查询</a>
        </fieldset>
    </div>
    <div class="list">
        <div class="itemtemp hidden" title="查看该清单的数据" style="cursor: pointer;">
            <a href="#" class="hidden abs" target="_blank">查看数据</a>
            <ul class="docinfo">
                <li>
                    <label>
                        清单名称：</label><span></span> </li>
                <li>
                    <label>
                        主文档名称：</label><span></span> </li>
                <li>
                    <label>
                        相关文档及属性：</label><span></span> </li>
            </ul>
        </div>
    </div>
    <div class="more">
        查看更多文档
    </div>
    <script type="text/javascript">
        var totalnum = 0;
        var showAttr = false;
        var action = "";
        function loadMore(pageIndex) {
            $.ajax({
                type: "POST",
                url: "/handlers/bom/List.ashx",
                data: { pageIndex: pageIndex, bomName: $(".bomname").val(), istemp: "0" },
                dataType: "json",
                success: function (res) {
                    if (res.length > 0) {
                        $.each(res, function (i, item) {
                            if (!item.IsTemp) {
                                var $temp = $(".itemtemp").clone();
                                var $span = $temp.find("span");
                                $temp.find(".abs").attr({ "href": "/BOM/" + (action == "s" ? "Sta" : "") + "Instance.aspx?ID=" + item.ID });
                                $span.eq(0).html("<a href='/BOM/" + (action == "s" ? "Sta" : "") + "Instance.aspx?ID=" + item.ID + "' title='点击查看数据' target='_blank'>" + item.Name + "</a>");
                                var str = "";
                                if (item.MainDoc != null) {
                                    //main doc
                                    str += item.MainDoc.DocName;
                                    if (showAttr) {
                                        str += "{";
                                        $.each(item.MainDoc.RelatedDocAttrs, function (j, jitem) {
                                            str += jitem.AttrName + "(" + jitem.Surname + ");";
                                        });
                                        str += "}";
                                    }
                                    $span.eq(1).html(str);
                                }
                                else
                                    $span.eq(1).html("暂无");
                                //related doc
                                str = "";
                                if (item.RelatedDocs.length > 0) {
                                    $.each(item.RelatedDocs, function (j, jitem) {
                                        //each doc
                                        str += jitem.DocName;
                                        if (showAttr) {
                                            str += "{";
                                            $.each(jitem.RelatedDocAttrs, function (k, kitem) {
                                                str += kitem.AttrName + "(" + kitem.Surname + ");";
                                            });
                                            str += "}";
                                        }
                                        str += ",";
                                        $span.eq(2).html(str);
                                    });
                                } else {
                                    $span.eq(2).html("暂无");
                                }
                                $temp.removeClass("hidden itemtemp").addClass("listitem relative").appendTo($(".list"));
                            }
                        });
                        $(".more").html("查看更多,共(" + totalnum + ")个");
                    } else {
                        $(".more").html("所有清单已经显示,共(" + totalnum + ")个");
                    }
                }
            });
        };
        $(document).ready(function () {
            var pageIndex = 0;
            action = request("action");
            $.ajax({
                type: "POST",
                url: "/handlers/bom/getTotal.ashx",
                data: { istemp: "0" },
                success: function (res) {
                    totalnum = res;
                    $(".more").html("查看更多,共(" + totalnum + ")个");
                }
            });
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
