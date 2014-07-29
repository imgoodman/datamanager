<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserManager.aspx.cs" Inherits="App.Web.UserManager" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Category" TagPrefix="JQPLMWeb" Src="~/Controls/Category.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:Panel ID="plMsg" runat="server" Width="100%" Visible="false">
        <center>
            <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></center>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" Width="100%">
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/add.jpg" /><b>添加用户</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd" style="width: 105px" align="center">
                        用户名
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="colortd" style="width: 105px">
                        真实姓名
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <%--            <tr>
                <td class="colortd" style="width: 105px">
                    性别
                </td>
                <td class="contenttd">
                    <asp:DropDownList ID="ddGender" runat="server">
                        <asp:ListItem Text="--请选择性别--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="男"> </asp:ListItem>
                        <asp:ListItem Text="女"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
                <tr>
                    <td class="colortd" style="width: 105px">
                        所属部门
                    </td>
                    <td align="left">
                        <JQPLMWeb:Category ID="category1" runat="server" GetDataStyle="Dept" AutoPostBack="true"
                            OnTextChanged="TextChanged1"></JQPLMWeb:Category>
                        <asp:Label ID="lb_deptid" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="colortd" style="width: 105px">
                        角色
                    </td>
                    <td class="contenttd">
                        <asp:DropDownList ID="ddlRole" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <%--            <tr>
                <td class="colortd" style="width: 105px">
                    E-mail
                </td>
                <td class="contenttd">
                    <div style="float: left;">
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></div>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="@sina.com">@sina.com</asp:ListItem>
                        <asp:ListItem Value="@sina.vip.com">@sina.vip.com</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="colortd" style="width: 105px">
                    手机
                </td>
                <td class="contenttd">
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="colortd" style="width: 105px">
                    固定电话
                </td>
                <td class="contenttd">
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            </table>
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="添 加" OnClick="btnAdd_Click" Width="80px" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnReset" runat="server" Text="重 置" OnClick="btnReset_Click" Width="80px"/>
            <asp:TextBox ID="TextBox1" runat="server" Style="position: relative" Visible="False"
                Width="48px"></asp:TextBox>
            <br />
            <br />
        </center>
        <p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Manage.jpg" /><b>用户管理</b></p>
        <br />
        <center>
            <table width="80%">
                <tr>
                    <td align="left">
                        <asp:Button ID="btnDel" runat="server" Text="删 除" Width="80px" OnClick="btnDel_Click" OnClientClick="return confirm('确定要删除选定用户吗？');" />
                        <asp:Label ID="lbRemind" runat="server" Text="当前数据库中没有记录" ForeColor="red" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="myGridView" runat="server" Width="80%" DataKeyNames="ID" OnRowCommand="myGridView_RowCommand"
                OnRowDataBound="myGridView_RowDataBound" AllowSorting="True" OnSorting="myGridView_Sorting"
                AutoGenerateColumns="false" OnRowCreated="myGridView_RowCreated">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            选择<asp:CheckBox ID="chkHeader" runat="server" OnClick="javascript: SelectAllCheckBoxInThisPageByID(this);">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelect" runat="server"></asp:CheckBox>
                            <asp:Label ID="ID" runat="server" Text='<%#Eval("ID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="80px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserName" HeaderText="用户名" SortExpression="UserName" />
                    <asp:BoundField DataField="Name" HeaderText="真实姓名" SortExpression="Name" />
                    <asp:BoundField DataField="dataDeptName" HeaderText="所属部门" SortExpression="DeptNameID" />
                    <asp:BoundField DataField="dataRoleName" HeaderText="角色" SortExpression="RoleID" />
                    <asp:ButtonField CommandName="ChooseEdit" HeaderText="编辑" Text="编辑">
                        <ItemStyle Width="50px" />
                    </asp:ButtonField>
                    <asp:TemplateField HeaderText="重置密码">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnResetPwd" runat="server" CommandArgument='<%#Eval("ID")%>'
                                OnCommand="lbtnResetPwd_Command" OnClientClick="return confirm('您确定要将该用户的密码重置为初始密码：111 吗？');">重置密码</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" Width="80%" PageSize="20"
                OnPageChanged="AspNetPager1_PageChanged" ShowPrevNext="true" ShowFirstLast="true"
                ShowInputBox="Always" ShowPageIndex="false" PrevPageText="上一页" NextPageText="下一页"
                FirstPageText="首页" LastPageText="末页" HorizontalAlign="Right" Font-Size="10pt"
                AlwaysShow="true" ShowCustomInfoSection="Left" CustomInfoHTML="<table width='100%'> <tr><td> 共<b style='color:red'>%RecordCount%</b>条记录</td> <td> 第<font color='red'><b>%CurrentPageIndex%</b></font>/%PageCount%页</td> <td>  第%StartRecordIndex% -%EndRecordIndex% 条记录</td></tr>   </table>">
            </webdiyer:AspNetPager>
            <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
        </center>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="100%" Visible="false">
        &nbsp;<p class="titleRow">
            <img alt="" class="NaviImage" src="Img/Edit.jpg" /><b>修改用户信息</b></p>
        <br />
        <center>
            <table class="tableBorder" cellspacing="0" cellpadding="3">
                <tr>
                    <td class="colortd">
                        用户名
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="MtxtUserName" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="colortd">
                        真实姓名
                    </td>
                    <td class="contenttd">
                        <asp:TextBox ID="MtxtName" runat="server" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <%--            <tr>
                <td class="colortd">
                    性别
                </td>
                <td class="contenttd">
                    <asp:DropDownList ID="MddGender" runat="server">
                        <asp:ListItem Text="--请选择性别--" Value="0"></asp:ListItem>
                        <asp:ListItem Text="男"> </asp:ListItem>
                        <asp:ListItem Text="女"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
                <tr>
                    <td class="colortd">
                        角色
                    </td>
                    <td class="contenttd">
                        <asp:DropDownList ID="ddlMRole" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="colortd">
                        所属部门
                    </td>
                    <td class="contenttd">
                        <JQPLMWeb:Category ID="category2" runat="server" GetDataStyle="Dept" AutoPostBack="true"
                            OnTextChanged="TextChanged2"></JQPLMWeb:Category>
                        <asp:Label ID="lb_deptid2" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <%--            <tr>
                <td class="colortd" style="width: 105px">
                    E-mail
                </td>
                <td class="contenttd">
                    <div style="float: left;">
                        <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></div>
                    <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>@sina.com</asp:ListItem>
                        <asp:ListItem>@sina.vip.com</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="colortd" style="width: 105px">
                    手机
                </td>
                <td class="contenttd">
                    <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="colortd" style="width: 105px; height: 30px;">
                    固定电话
                </td>
                <td class="contenttd" style="height: 30px">
                    <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
            </table>
            <br />
            <asp:Button ID="btnModify" runat="server" Text="修 改" OnClick="btnModify_Click" Width="80px"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnReturn" runat="server" Text="返 回" OnClick="btnReturn_Click" Width="80px"/>
            <asp:TextBox ID="txtMdataID" runat="server" Visible="false"></asp:TextBox>
        </center>
    </asp:Panel>
</asp:Content>
