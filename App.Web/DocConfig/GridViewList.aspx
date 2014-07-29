<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GridViewList.aspx.cs" Inherits="App.Web.DocConfig.GridViewList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            文档对象列表</h1>
            <a href="/DocConfig/List.aspx" class="btn abs">列表视图</a>
    </div>
    <center>
        <table width="100%" style="border-color: #EFF6FE; border-width: 0px; border: 1px;
            border-collapse: collapse; height: 70px; border-style: solid" cellpadding="1"
            id="tb1" cellspacing="0" runat="server">
            <tr>
                <td style="background-color: #9FD6FF; font-weight: bold; width: 100px; font-family: 华文仿宋;
                    font-size: medium; vertical-align: middle; border-right: solid; border-width: 1px;"
                    align="center">
                    搜 索
                </td>
                <td align="left" style="vertical-align: middle; font-size: small">
                    <table width="95%">
                        <tr style="height: 32px; vertical-align: middle">
                            <td class="contenttd">
                                &nbsp;
                                <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="True">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:Label ID="lb1" runat="server" Font-Bold="true" Text="创建日期:"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text="从"></asp:Label>
                                <asp:TextBox ID="txtDateFrom" runat="server" Width="70px"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text="到"></asp:Label>
                                <asp:TextBox ID="txtDateTo" runat="server" Width="70px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 32px; vertical-align: middle">
                            <td>
                                &nbsp;
                                <asp:Label ID="lb2" runat="server" Font-Bold="true" Text="模糊搜索: 文档对象名称"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Text=""></asp:Label>&nbsp;
                                <asp:TextBox ID="txtfuzzyProjName" runat="server"></asp:TextBox>&nbsp;
                                <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" Width="80px"
                                    Height="24px" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="font-weight: bold; width: 100px; vertical-align: middle; border-left: solid;
                    border-width: 1px;" align="center">
                    <asp:Button ID="btnShowAll" runat="server" Text="显示全部" OnClick="btnShowAll_Click"
                        Width="80px" Height="24px" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="ID" AllowSorting="true"
            BorderWidth="1" GridLines="None" BorderColor="#8A8A8A" OnSorting="GridView1_Sorting"
            AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" EmptyDataText="<font color='red'>没有符合条件的文档对象</font>"
            RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            <RowStyle BackColor="#FFFFFF" Font-Size="Medium" Font-Names="华文仿宋" Height="35px"
                BorderColor="gray" VerticalAlign="Middle" BorderWidth="1" />
            <AlternatingRowStyle BackColor="#E4F1FF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#9FD6FF" Font-Bold="True" Height="35px" CssClass="tbHeader"
                Font-Names="华文仿宋" VerticalAlign="Middle" Font-Size="Medium" />
            <Columns>
                <asp:BoundField HeaderText="序号" DataField="Num" ItemStyle-Width="40px" />
                <asp:BoundField HeaderText="顶层类别名称" DataField="TopFatherTypeName" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="父级类别名称" DataField="FatherTypeName" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="文档对象名称" DataField="DocName" ItemStyle-Width="120px" />
                <asp:BoundField HeaderText="属性列表" DataField="Attrs" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="详情">
                    <ItemTemplate>
                        <a href="/DocConfig/Profile.aspx?ID=<%#Eval("ID") %>" target="_blank" title="点击查看详情"
                            rel="#overlay">查看
                    </ItemTemplate>
                    <ItemStyle Width="60px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="100%" PageSize="25"
            OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
            ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
            FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
            AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
        </webdiyer:AspNetPager>
    </center>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtDateFrom").datepicker();
            $("#ContentPlaceHolder1_txtDateTo").datepicker();
        });


        
    </script>
</asp:Content>
