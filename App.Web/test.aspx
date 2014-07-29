<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="test.aspx.cs" Inherits="App.Web.test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="Button1" runat="server" Text="手动导出清单数据" OnClick="Button1_Click" />
    <asp:Button ID="btnCreator" runat="server" Text="校正文档创建者" 
        onclick="btnCreator_Click" />
    <div id="jstree">
        <ul>
            <li>/
                <ul>
                    <li>*
                        <ul>
                            <li>F<sub>v</sub> </li>
                            <li>()<sup>3</sup>
                                <ul>
                                    <li>l</li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                    <li>*
                        <ul>
                            <li>*
                                <ul>
                                    <li>4</li>
                                    <li>[w]</li>
                                </ul>
                            </li>
                            <li>*
                                <ul>
                                    <li>E </li>
                                    <li>()<sup>3</sup>
                                        <ul>
                                            <li>t</li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </li>
                </ul>
            </li>
        </ul>
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#jstree").jstree();
        });
    </script>
</asp:Content>
