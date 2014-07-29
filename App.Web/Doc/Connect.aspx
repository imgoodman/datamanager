<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Connect.aspx.cs" Inherits="App.Web.Doc.Connect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="formContent">
        <div class="header">
            <h1 class="pagetitle">
                查看关联文档</h1>
        </div>
        <div class="list">
            <p>
                列表仅显示文档的部分属性，要查看全部属性，请点击链接查看详情</p>
            <div class="itemtemp hidden">
                <a href="#" class="hidden abs" target="_blank">详情</a>
                <ul class="docinfo">
                    <li class="hidden doc_temp">
                        <label>
                            文档名称：</label>
                        <p class="docName">
                            docName</p>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var rInstID = request("RInstID");
            var rAttrID = request("RAttrID");
            //console.log(rInstID + "," + rAttrID);
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getSourceInstance.ashx",
                data: { rInstID: rInstID, rAttrID: rAttrID },
                success: function (res) {
                    if (res == "") {
                        window.location.href = "/404.aspx";
                    } else {
                        if (res.indexOf(',') < 0) {
                            window.location.href = "/Doc/Profile.aspx?ID=" + res;
                        }
                        else {
                            var ids = res.split(',');
                            $.each(ids, function (i, id) {
                                $.ajax({
                                    type: "POST",
                                    url: "/handlers/doc/getDocById.ashx",
                                    data: { docinstanceid: id },
                                    dataType: "json",
                                    success: function (docitem) {
                                        var $temp = $(".itemtemp").clone();
                                        $temp.find(".abs").attr("href", "/Doc/Profile.aspx?ID=" + docitem.ID);
                                        var $docnameitem = $temp.find(".doc_temp").clone();
                                        $docnameitem.find("label").text("文档对象");
                                        $docnameitem.find(".docName").text(docitem.Document.DocName);
                                        $docnameitem.removeClass("hidden doc_temp").addClass("docitem").appendTo($temp.find("ul"));

                                        $.each(docitem.Document.Attrs, function (j, jitem) {
                                            var $attritem = $temp.find(".doc_temp").clone();
                                            $attritem.find("label").text(jitem.AttrName + ":");
                                            if (j == 0) {
                                                $attritem.find(".docName").html("<a href='/Doc/Profile.aspx?ID=" + docitem.ID + "' title='点击查看详情' target='_blank'>" + showString(jitem.TranValue) + "</a>");
                                            } else {
                                                $attritem.find(".docName").html(jitem.AttrType == "6" ? "<a href='/Avatar/" + jitem.Value + "' target='_blank'>" + showString(jitem.TranValue) + "</a>" : showString(jitem.TranValue));
                                            }
                                            $attritem.removeClass("hidden doc_temp").addClass("docitem").appendTo($temp.find("ul"));
                                        });
                                        $temp.removeClass("hidden itemtemp").addClass("listitem relative").appendTo($(".list"));
                                    }
                                });
                            });
                        }
                    }
                },
                error: function (msg) { }
            });
        });
    </script>
</asp:Content>
