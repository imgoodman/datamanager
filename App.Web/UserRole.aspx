<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserRole.aspx.cs" Inherits="App.Web.UserRole" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        // 点击复选框时触发事件
        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
        function OnTreeNodeChecked() {
            var ele = event.srcElement;
            if (ele.type == 'checkbox') {
                var tblRow = ele.parentNode;

                var txtList = tblRow.getElementsByTagName("div"); // 返回表内嵌的所有 select 控件
                for (var j = 0; j < txtList.length; j++) {
                    window.alert(txtList[j].id);
                }

                var childrenDivID = ele.id.replace('CheckBox', 'Nodes');
                var div = document.getElementById(childrenDivID);
                if (div == null) return;
                var checkBoxs = div.getElementsByTagName('INPUT');
                for (var i = 0; i < checkBoxs.length; i++) {
                    if (checkBoxs[i].type == 'checkbox')
                        checkBoxs[i].checked = ele.checked;
                }

            }
        } 

    </script>
    <asp:Panel runat="server" ID="Panel1" Width="100%">
        <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
            <center>
                <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
        </asp:Panel>
        <center>
            <p class="titleRow">
                <img alt="" class="NaviImage" src="Img/Manage.jpg" />&nbsp;<b>角色-权限配置</b></p>
            <br />
            <asp:GridView ID="myGridView" runat="server" Width="80%" DataKeyNames="ID" OnRowCommand="myGridView_RowCommand"
                OnRowDataBound="myGridView_RowDataBound" AllowSorting="true" OnSorting="myGridView_Sorting"
                AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated" EmptyDataText="当前数据库中没有记录">
                <Columns>
                    <asp:BoundField HeaderText="角色名称" DataField="RoleName" SortExpression="RoleName" />
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="权限" Text="权限">
                        <ItemStyle Width="80px" />
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="80%" PageSize="10"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
        </center>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="100%" Visible="false">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Edit.jpg" />&nbsp;<b>为角色:<asp:Label ID="label1"
                runat="server" Text=""></asp:Label>
                编辑权限信息</b></p>
        <br />
        <center>
            <asp:Table ID="Table1" runat="server" Width="20%" CellSpacing="0" CssClass="clearmargin">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" ShowLines="True" ShowCheckBoxes="All"
                            EnableClientScript="true">
                            <ParentNodeStyle Font-Bold="False" />
                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
            <asp:Button ID="Button1" runat="server" Text="确 定" Height="24px" Width="80px" OnClick="clickadd" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                ID="Button2" runat="server" Text="返 回" Height="24px" Width="80px" OnClick="clickreturn" />
            <asp:TextBox ID="txtMdataID" runat="server" Visible="false"></asp:TextBox>
            <asp:TextBox ID="txtdataTask" runat="server" Visible="false" />
        </center>
    </asp:Panel>
</asp:Content>
