<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StaInstance.aspx.cs" Inherits="App.Web.BOM.StaInstance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            xx�嵥�����б�</h1>
        <div style="border: 1px solid #ccc; padding: 5px; margin-bottom: 5px; background-color: #f3f3f3;">
            <span>createtime</span>
            <asp:Button ID="btnExport" runat="server" Text="����Excel" ToolTip="������ǰ�嵥���ݵ�excel�ļ�"
                CssClass="btn btn_primary" Visible="false" OnClick="btnExport_Click" />
                <a href="#" class="btn btn_primary">����Excel</a>
                </div>
    </div>
    <div id="search_div" class="hidden">
        <fieldset class="form relative">
            <legend>��ѯ</legend>
            <div class="control_group">
                <label class="control_label">
                    �嵥����</label>
                <div class="controls">
                    <input type="text" class="input_medium bomname" />
                </div>
            </div>
            <a href="javascript:void(0);" class="btn btn_primary search abs searchBtn" title="�����ĵ����ͺ���������ѯ">
                ��ѯ</a>
        </fieldset>
    </div>
    <div class="list" style="width: 750px; overflow: auto;">
        <table class="instance">
            <thead>
                <tr>
                    <th rowspan="2">
                        ���
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <div class="more">
        �鿴�����ĵ�
    </div>
    <script type="text/javascript">
        var totalnum = 0;
        var currentNum = 1;
        var main_attr_ids = "";
        var related_doc_ids = "";
        var related_attr_ids = "";
        //get its info

        //main doc id
        var maindocid = "";

        function loadMore(pageIndex, bomid) {
            $.ajax({
                type: "POST",
                url: "/handlers/bom/getStaInstances.ashx",
                data: { pageIndex: pageIndex, id: bomid },
                dataType: "json",
                success: function (res) {
                    if (res.length > 0) {
                        $.each(res, function (i, item) {
                            //each instance
                            var $tr = $(".temptr").clone();
                            var $tds = $tr.find("td");
                            var $tbody = $(".instance tbody");
                            $tds.each(function (tdindex, tditem) {
                                var clsname = $(tditem).prop("class");
                                if (clsname == "") {
                                    $(tditem).html(currentNum);
                                } else {
                                    var idArray = clsname.split('_');
                                    var docid = idArray[1];
                                    var attr_id = idArray[2];
                                    if (maindocid == docid) {
                                        //set main doc attrs
                                        $.each(item.MainDocData.RelatedDocAttr, function (j, jitem) {
                                            //console.log(jitem.ID + "," + attr_id);
                                            if (jitem.ID == attr_id) {
                                                //console.log(attr_index+",jitem id is"+jitem.ID+", attr id is:"+attr_id);
                                                if (j == 0) {
                                                    $(tditem).html("<a target='_blank' title='����鿴ʵ������' href='/Doc/Profile.aspx?ID=" + item.MainDocData.DocInstanceID + "'>" + showLongString(jitem.TranValue, 10) + "</a><input type='hidden' value='" + attr_id + "' />");
                                                } else {
                                                    $(tditem).html(showLongString(jitem.TranValue, 10) + "<input type='hidden' value='" + attr_id + "' />");
                                                }
                                            }
                                        });
                                    } else {
                                        //set relative doc attrs
                                        $.each(item.RelatedDocDatas, function (j, jitem) {
                                            if (jitem.RelatedDocInstances.length > 0) {
                                                if (jitem.RelatedDoc.ID == docid) {
                                                    var str = "";
                                                    $.each(jitem.RelatedDocInstances, function (k, kitem) {
                                                        //each instance
                                                        $.each(kitem.RelatedDocAttr, function (m, mitem) {
                                                            //each instance attrs
                                                            if (mitem.ID == attr_id) {
                                                                if (m == 0) {
                                                                    str += "<a target='_blank' title='����鿴ʵ������' href='/Doc/Profile.aspx?ID=" + kitem.DocInstanceID + "'>" + showLongString(mitem.TranValue, 10) + "</a><br>";
                                                                } else {
                                                                    str += showLongString(mitem.TranValue, 10) + "<br>";
                                                                }
                                                            }
                                                        });
                                                    });
                                                    $(tditem).html(str);
                                                }
                                                //ss
                                            }
                                            //ss
                                        });
                                    }
                                }
                            });


                            $tr.removeClass("hidden temptr");
                            $tr.appendTo($tbody);
                            currentNum++;
                        });
                        $(".more").removeClass("momore");
                    } else {
                        $(".more").html("û�и����嵥����,��(" + totalnum + ")").addClass("nomore");
                    }
                    //console.log(res.length);
                }
            });
        };
        $(document).ready(function () {
            var pageIndex = 0;
            var id = request("id");
            //get its info and set its headers
            //var test = '{"name":"jan"}';
            //var testjson = JSON.parse(test);
            //console.log(testjson.name);
            $.ajax({
                type: "POST",
                url: "/handlers/bom/getitembyid.ashx",
                data: { id: id },
                dataType: "json",
                success: function (res) {
                    $(".header h1").html(res.Name + "�����б�");
                    var $thead = $(".list table thead");
                    var $doctr = $thead.find("tr");

                    var $temptr = $("<tr>").addClass("temptr hidden");
                    $("<td>").html("&nbsp;").appendTo($temptr);
                    maindocid = res.MainDoc.ID;
                    $("<th>").html(res.MainDoc.DocName + "<input type='hidden' value='" + res.MainDoc.ID + "' />").prop({ "colspan": res.MainDoc.RelatedDocAttrs.length }).appendTo($doctr);
                    $.each(res.RelatedDocs, function (i, item) {
                        $("<th>").html(item.DocName + "<input type='hidden' value='" + item.ID + "' />").prop({ "colspan": item.RelatedDocAttrs.length }).appendTo($doctr);
                        related_doc_ids += item.ID + ",";

                    });
                    related_doc_ids = related_doc_ids.substr(0, related_doc_ids.length - 1);

                    //attr tr
                    var $attrtr = $("<tr>").appendTo($thead);
                    $.each(res.MainDoc.RelatedDocAttrs, function (i, item) {
                        $("<th>").html(item.Surname + "<input type='hidden' value='" + item.ID + "' />").appendTo($attrtr);
                        main_attr_ids += item.ID + ",";
                        $("<td>").addClass("td_" + res.MainDoc.ID + "_" + item.ID).html("&nbsp;").appendTo($temptr);
                    });
                    $.each(res.RelatedDocs, function (i, item) {
                        $.each(item.RelatedDocAttrs, function (j, jitem) {
                            $("<th>").html(jitem.Surname + "<input type='hidden' value='" + jitem.ID + "' />").appendTo($attrtr);
                            related_attr_ids += jitem.ID + ",";
                            $("<td>").addClass("td_" + item.ID + "_" + jitem.ID).html("&nbsp;").appendTo($temptr);
                        });
                    });
                    $temptr.appendTo($(".instance tbody"));
                    main_attr_ids = main_attr_ids.substr(0, main_attr_ids.length - 1);
                    related_attr_ids = related_attr_ids.substr(0, related_attr_ids.length - 1);
                    //console.log(main_attr_ids + "-----" + related_attr_ids);
                    var symindex = res.FilePath.indexOf('.');
                    $(".header a").prop({ "href": "/data/excel/" + res.FilePath.substr(0,symindex) + ".xlsx" });
                }
            });
            //get total num
            $.ajax({
                type: "POST",
                url: "/handlers/bom/getTotalXML.ashx",
                data: { id: id },
                success: function (res) {
                    var symindex = res.indexOf('_');
                    totalnum = res.substr(0, symindex);
                    var createtime = res.substr(symindex + 1, res.length - symindex);
                    $(".header span").html("����ʱ��:" + createtime);
                    $(".more").html("�鿴����,��(" + totalnum + ")��");
                }
            });
            loadMore(pageIndex, id);
            //���ظ���
            $(".more").click(function () {
                if ($(this).hasClass("nomore") || totalnum == currentNum) {
                    InfoTip.showMessage("û�и����嵥����", "warning", 6000);
                    return false;
                }
                pageIndex++;
                loadMore(pageIndex, id);
            });
            $(".search").click(function (e) {
                e.preventDefault();
                $(".list .listitem").remove();
                pageIndex = 0;
                loadMore(pageIndex, id);
            });
        });
        
    </script>
</asp:Content>
