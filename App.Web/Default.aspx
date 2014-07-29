<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="App.Web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="/resources/jqwidgets/styles/jqx.base.css" />
    <script type="text/javascript" src="resources/jqwidgets/jqxcore.js"></script>
    <script type="text/javascript" src="/resources/jqwidgets/jqxdraw.js"></script>
    <script type="text/javascript" src="/resources/jqwidgets/jqxchart.js"></script>
    <script type="text/javascript" src="/resources/jqwidgets/jqxdata.js"></script>
    <div class="header relative">
        <h1 class="pagetitle">
            系统概况</h1>
    </div>
    <div>
        <div id="docs_div" style="height: 200px;">
        </div>
        <div id="users_div" style="height: 200px;">
        </div>
    </div>
    <div class="list" style="margin-top:10px;">
        <p>
            未上传附件的文档.(列表仅显示文档的部分属性，要查看全部属性，请点击链接查看详情)</p>
        <table>
            <thead>
                <tr>
                    <th style="width:40px;">
                        序号
                    </th>
                    <th style="width:100px;">
                        文档名称
                    </th>
                    <th>
                        文档属性
                    </th>
                    <th style="width:40px;">
                        详情
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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#submenu").remove();
            $("#mainmenu ul li a").eq(0).addClass("selected");
            //get instances
            $.ajax({
                type: "POST",
                url: "/handlers/dashboard/getTopInstances.ashx",
                dataType: "json",
                success: function (res) {
                    var settings = {
                        title: "文档数据概括",
                        description: "",
                        padding: {
                            left: 5,
                            top: 5,
                            right: 5,
                            bottom: 5
                        },
                        titlePadding: {
                            left: 0,
                            top: 0,
                            right: 0,
                            bottom: 10
                        },
                        source: res,
                        xAxis:
                    {
                        dataField: 'Name',
                        showGridLines: true
                    },
                        colorScheme: 'scheme01',
                        seriesGroups:
                    [
                        {
                            type: 'column',
                            valueAxis:
                            {
                                displayValueAxis: true,
                                description: '文档数量'
                            },
                            series: [
                                    { dataField: 'Number', displayText: '文档数量' }
                                ]
                        }
                    ]
                    };
                    $("#docs_div").jqxChart(settings);
                }
            });
            //get users
            $.ajax({
                type: "POST",
                url: "/handlers/dashboard/getTopCreators.ashx",
                dataType: "json",
                success: function (res) {
                    var settings = {
                        title: "用户数据概括",
                        description: "",
                        padding: {
                            left: 5,
                            top: 5,
                            right: 5,
                            bottom: 5
                        },
                        titlePadding: {
                            left: 0,
                            top: 0,
                            right: 0,
                            bottom: 10
                        },
                        source: res,
                        xAxis:
                    {
                        dataField: 'Name',
                        showGridLines: true
                    },
                        colorScheme: 'scheme01',
                        seriesGroups:
                    [
                        {
                            type: 'column',
                            valueAxis:
                            {
                                displayValueAxis: true,
                                description: '创建文档数量'
                            },
                            series: [
                                    { dataField: 'Number', displayText: '创建文档数量' },
                                ]
                        }
                    ]
                    };
                    $("#users_div").jqxChart(settings);
                }
            });
            //files
            var pageIndex = 0;
            var itemIndex = 1;
            function getData() {
                $.ajax({
                    type: "POST",
                    url: "/handlers/dashboard/getUnattachFiles.ashx",
                    data: { pageIndex: pageIndex },
                    dataType: "json",
                    success: function (res) {
                        if (res.length > 0) {
                            $.each(res, function (i, item) {
                                var $tbody = $(".list table tbody");
                                var $tr = $("<tr>");
                                $tr.appendTo($tbody);
                                $("<td>").html(itemIndex).appendTo($tr);
                                itemIndex++;
                                $("<td>").html(item.Document.DocName).appendTo($tr);
                                var str = "";
                                $.each(item.Document.Attrs, function (j, jitem) {
                                    if (j < 2)
                                        str += jitem.AttrName + ":" + jitem.TranValue + ";";
                                });
                                $("<td>").html(str + "...").appendTo($tr);
                                $("<td>").html("<a href='/Doc/Profile.aspx?ID=" + item.ID + "' target='_blank'>详情</a>").appendTo($tr);
                            });
                        } else {
                            if (pageIndex == 0) {
                                $(".list,.more").remove();
                            } else {
                                $(".more").html("没有更多");
                            }
                        }
                    }
                });
            };
            getData();
            //加载更多
            $(".more").click(function () {
                pageIndex++;
                getData();
            });
        });
    </script>
</asp:Content>
