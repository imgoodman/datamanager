<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="MyList.aspx.cs" Inherits="App.Web.Doc.MyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header relative">
        <h1 class="pagetitle">
            列表</h1>
        <a href="/Doc/MyTableList.aspx" class="btn abs">表格视图</a>
    </div>
    <div id="search_div">
        <fieldset class="form relative moreConditions">
            <legend>查询条件</legend>
            <div class="control_temp hidden">
                <input type="hidden" class="attrid" />
                <input type="hidden" class="attrtype" />
                <label class="control_label">
                    attrName</label>
                <div class="controls">
                </div>
            </div>
            <a href="javascript:void(0);" class="btn btn_primary searchBtn abs search" title="根据输入条件查询相关文档数据">
                查询</a> <a href="javascript:void(0);" class="checkMore abs" title="展开查看更多查询条件">更多查询条件</a>
        </fieldset>
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
    <div class="more">
        查看更多
    </div>
    <div id="multiperson_div" class="hidden">
        <input type="hidden" class="sel_userids" />
        <input type="hidden" class="attrid" />
        <input type="hidden" class="sel_type" value="m" />
        <div class="depts_users">
            <div class="pull_left three_col_left sel_title">
                部门结构</div>
            <div class="pull_left its_users three_col_right sel_title">
                部门下属人员列表</div>
            <div id="jstree_dept_m" class="pull_left three_col_left container">
            </div>
            <div class="pull_left its_users three_col_right container">
                <div class="sel_users" id="user_container">
                    <ul>
                        <li class="user_temp hidden"><span class="pull_left" title="选择该人员">人员名称</span><input
                            type="hidden" /></li>
                    </ul>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="selected_users">
            <fieldset class="form">
                <legend>已选择的人员列表</legend>
                <div class="sel_users">
                    <ul>
                        <li class="user_temp hidden"><span class="pull_left username">人员名称</span><span title="删除该人员"
                            class="ui-icon ui-icon-close user_del"></span><input type="hidden" /></li>
                    </ul>
                </div>
            </fieldset>
        </div>
        <span class="user_dis_temp hidden">username<a href="#" title="删除该人员"></a><input type="hidden" /></span>
    </div>
    <script type="text/javascript">
        var docName = "";
        var uid = "";
        var searchAttrIds = "";
        var searchAttrVals = "";
        var totalNum = 0;
        function loadMore(pageIndex, docid) {
            $.ajax({
                type: "POST",
                url: "/handlers/doc/getDocs.ashx",
                data: { pageIndex: pageIndex, docid: docid, userid: uid },
                dataType: "json",
                success: function (res) {
                    if (res != null) {
                        $.each(res, function (i, item) {
                            var $temp = $(".itemtemp").clone();
                            if (docName == "") {
                                docName = item.Document.DocName;
                                $(".pagetitle").text("我的"+docName + "列表");
                            }
                            $temp.find(".abs").attr("href", "/Doc/Profile.aspx?ID=" + item.ID);
                            $.each(item.Document.Attrs, function (j, jitem) {
                                var $attritem = $temp.find(".doc_temp").clone();
                                $attritem.find("label").text(jitem.AttrName + ":");
                                if (j == 0) {
                                    $attritem.find(".docName").html("<a href='/Doc/Profile.aspx?ID=" + item.ID + "' title='点击查看详情' target='_blank'>" + showString(jitem.TranValue) + "</a>");
                                } else {
                                    $attritem.find(".docName").html(jitem.AttrType == "6" ? "<a href='/Avatar/" + jitem.Value + "' target='_blank'>" + showString(jitem.TranValue) + "</a>" : showString(jitem.TranValue));
                                }
                                $attritem.removeClass("hidden doc_temp").addClass("docitem").appendTo($temp.find("ul"));
                            });
                            $temp.removeClass("hidden itemtemp").addClass("listitem relative").appendTo($(".list"));
                        });
                        $(".more").html("查看更多(共" + totalNum + ")");
                    } else {
                        if (pageIndex == 0) {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/docconfig/getitembyid.ashx",
                                data: { id: docid },
                                dataType: "json",
                                success: function (res) {
                                    docName = res.DocName;
                                    $(".pagetitle").text("我的"+docName + "列表");
                                }
                            });
                        }
                        $(".more").html("没有更多" + docName + "(共" + totalNum + ")");
                    }
                }
            });
        };
        //search
        function searchMore(pageIndex, docid) {
            $.ajax({
                type: "POST",
                url: "/handlers/doc/search.ashx",
                data: { pageIndex: pageIndex, docid: docid, userid: uid, attrIds: searchAttrIds, attrVals: searchAttrVals },
                dataType: "json",
                success: function (res) {
                    if (res != null) {
                        $.each(res, function (i, item) {
                            var $temp = $(".itemtemp").clone();
                            if (docName == "") {
                                docName = item.Document.DocName;
                                $(".pagetitle").text("我的"+docName + "列表");
                            }
                            $.each(item.Document.Attrs, function (j, jitem) {
                                var $attritem = $temp.find(".doc_temp").clone();
                                $attritem.find("label").text(jitem.AttrName + ":");
                                if (j == 0) {
                                    $attritem.find(".docName").html("<a href='/Doc/Profile.aspx?ID=" + item.ID + "' title='点击查看详情' target='_blank'>" + jitem.TranValue + "</a>");
                                } else {
                                    $attritem.find(".docName").html(jitem.TranValue);
                                }
                                $attritem.removeClass("hidden doc_temp").addClass("docitem").appendTo($temp.find("ul"));
                            });
                            $temp.removeClass("hidden itemtemp").addClass("listitem").appendTo($(".list"));
                        });
                        $(".more").html("查看更多(共" + totalNum + ")");
                    } else {
                        if (pageIndex == 0) {
                            $.ajax({
                                type: "POST",
                                url: "/handlers/docconfig/getitembyid.ashx",
                                data: { id: docid },
                                dataType: "json",
                                success: function (res) {
                                    docName = res.DocName;
                                    $(".pagetitle").text("我的"+docName + "列表");
                                }
                            });
                        }
                        $(".more").html("没有更多" + docName + "(共" + totalNum + ")");
                    }
                }
            });
        };
        //get total
        function getTotal(attrIds, attrVals) {
            $.ajax({
                type: "POST",
                url: "/handlers/doc/gettotal.ashx",
                data: {
                    attrIds: searchAttrIds,
                    attrVals: searchAttrVals
                },
                success: function (res) {
                    //console.log(res);
                    totalNum = res;
                }
            });
        }


        $(document).ready(function () {
            //check more search conditions
            $(".checkMore").click(function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".form");
                if ($parent.hasClass("moreConditions")) {
                    //$parent.removeClass("moreConditions");
                    $(this).text("收起查询条件");
                } else {
                    //$parent.addClass("moreConditions");
                    $(this).text("更多查询条件");
                }
                $parent.toggleClass("moreConditions");
            });

            var pageIndex = 0;
            var docid = request("id");
            $(".header a").attr({ "href": "/Doc/MyTableList.aspx?ID=" + docid });
            uid = $("#userid").val();
            $.ajax({
                type: "post",
                url: "/handlers/docconfig/getSearchAttrs.ashx",
                data: { id: docid },
                dataType: "json",
                success: function (res) {
                    if (res.length > 0) {
                        $.each(res, function (i, item) {
                            var $temp = $(".control_temp").clone();
                            $temp.find(".control_label").text(item.AttrName);
                            $temp.find(".attrid").val(item.ID);
                            $temp.find(".attrtype").val(item.AttrType);
                            switch (item.AttrType.toString()) {
                                case "1":
                                    //string
                                    $("<input>").attr({ "type": "text" }).addClass("input_medium").appendTo($temp.find(".controls"));
                                    break;
                                case "3":
                                    //date
                                    $("<span>").text("从").appendTo($temp.find(".controls"));
                                    var $dp = $("<input>").attr({ "type": "text" }).addClass("datepickerFrom");
                                    $dp.datepicker({ "dateFormat": "yy-mm-dd" });
                                    $dp.appendTo($temp.find(".controls"));
                                    $("<span>").text("到").appendTo($temp.find(".controls"));
                                    var $dp2 = $("<input>").attr({ "type": "text" }).addClass("datepickerTo");
                                    $dp2.datepicker({ "dateFormat": "yy-mm-dd" });
                                    $dp2.appendTo($temp.find(".controls"));
                                    break;
                                case "4":
                                case "5":
                                    //person
                                    $("<span>").addClass("no_sel").text("暂没有选择任何人").appendTo($temp.find(".controls"));
                                    $("<a>").addClass("btn multiperson").html("选择").attr({ "title": "选择人员" }).appendTo($temp.find(".controls"));
                                    break;
                                case "7":
                                    var $sel = $("<select>").addClass("input_medium");
                                    $("<option>").text("--请选择" + item.AttrName + "--").val("0").appendTo($sel);
                                    $.each(item.AttrVals, function (j, jitem) {
                                        $("<option>").text(jitem.AttrValue).val(jitem.ID).appendTo($sel);
                                    });
                                    $sel.appendTo($temp.find(".controls"));
                                    break;
                            }
                            //append
                            $temp.removeClass("hidden control_temp").addClass("control_group").insertAfter($(".control_temp"));
                        });
                    }
                    else {
                        $("#search_div").remove();
                    }
                }
            });
            loadMore(pageIndex, docid);
            getTotal("", "");
            //加载更多
            $(".more").click(function () {
                pageIndex++;
                if (searchAttrIds == "") {
                    loadMore(pageIndex, docid);
                } else {
                    searchMore(pageIndex, docid);
                }
            });

            //load departments
            $.ajax({
                type: "POST",
                url: "/handlers/dept/getDepts.ashx",
                data: {},
                dataType: "json",
                success: function (res) {
                    $("#jstree_dept_m").jstree({
                        "core": {
                            "data": res
                        }
                    }).on("changed.jstree", function (e, data) {
                        //点击事件
                        var i, j, r = [];
                        for (i = 0, j = data.selected.length; i < j; i++) {
                            r.push(data.instance.get_node(data.selected[i]).id);
                        }
                        //alert('Selected: ' + r.join(', '));
                        var selDeptid = r[0];
                        selDeptid = selDeptid.replace('d', '');
                        //alert(selDeptid);
                        //load its users of a selected department
                        $.ajax({
                            type: "POST",
                            url: "/handlers/user/getUsers.ashx",
                            data: { deptID: selDeptid },
                            dataType: "json",
                            success: function (res_users) {
                                $("#user_container .useritem").remove();
                                $.each(res_users, function (i, item) {
                                    var $temp = $("#user_container .user_temp").clone();
                                    $temp.find("span").html(item.Name);
                                    $temp.find("input[type=hidden]").val(item.ID);
                                    $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#user_container ul"));
                                });
                            }
                        });
                    });
                }
            });
            //select user
            $("body").on("click", "#user_container .useritem", function (e) {
                e.preventDefault();
                var userid = $(this).find("input[type=hidden]").val();
                //console.log(userid);
                var $div = $("#multiperson_div");
                var sel_type = $div.find(".sel_type").val();
                var isExist = ($("#selected_users input[type=hidden][value='" + userid + "']").length > 0 ? true : false);
                //console.log(isExist + sel_type);
                if (!isExist) {
                    var $temp = $("#selected_users .user_temp").clone();
                    $temp.find(".username").html($(this).find("span").text());
                    $temp.find("input[type=hidden]").val(userid);
                    if (sel_type == "m") {
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    } else {
                        $("#selected_users .useritem").remove();
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    }
                }
            });
            //delete select in pop up
            $("body").on("click", "#selected_users .user_del", function (e) {
                e.preventDefault();
                $(this).parent().remove();
            });
            $("body").on("click", ".multiperson", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                var attrid = $parent.find(".attrid").val();
                var $div = $("#multiperson_div");
                $div.find(".sel_type").val("m");
                $div.find(".attrid").val(attrid);
                $("#selected_users .useritem").remove();
                //如果已经选了人,就需要赋值给弹出div
                if ($parent.find(".userspan").length > 0) {
                    $parent.find(".userspan").each(function (i, item) {
                        var username = $(item).text();
                        var userid = $(item).find("input[type=hidden]").val();
                        //console.log(username + userid);
                        var $temp = $("#selected_users .user_temp").clone();
                        $temp.find(".username").text(username);
                        $temp.find("input[type=hidden]").val(userid);
                        $temp.removeClass("hidden user_temp").addClass("useritem").appendTo($("#selected_users ul"));
                    });
                }
                $div.dialog({
                    title: "选择人员",
                    modal: true,
                    width: 800,
                    minHeight: 300,
                    buttons: {
                        "确认选择": function () {
                            $parent.find(".userspan").remove();
                            if ($("#selected_users .useritem").length > 0) {
                                $("#selected_users .useritem").each(function (i, item) {
                                    var username = $(item).find(".username").text();
                                    var userid = $(item).find("input[type=hidden]").val();
                                    //console.log(username + userid);
                                    var $temp = $("<span>").html(username + "<a href='#'></a><input type='hidden' value='" + userid + "' />");
                                    $temp.addClass("userspan").insertAfter($parent.find(".multiperson"));
                                });
                                $parent.find(".no_sel").hide();
                            } else {
                                $parent.find(".no_sel").show();
                            }
                            $div.dialog("close");
                        },
                        "取消": function () { $div.dialog("close"); }
                    }
                });
            });
            //delete selected users in add form
            $("body").on("click", ".userspan a", function (e) {
                e.preventDefault();
                var $parent = $(this).parents(".control_group");
                if ($parent.find(".userspan").length == 1) {
                    $parent.find(".no_sel").show();
                }
                $(this).parent().remove();
            });
            //search
            $(".search").click(function (e) {
                e.preventDefault();
                $(".list .listitem").remove();
                searchAttrIds = "";
                searchAttrVals = "";
                pageIndex = 0;
                $("#search_div .control_group").each(function (i, item) {
                    var attrtype = $(item).find(".attrtype").val();
                    var arrtid = $(item).find(".attrid").val();
                    switch (attrtype.toString()) {
                        case "1":
                            //string
                            if ($(item).find("input[type=text]").val() != "") {
                                //searchStr += "'a" + arrtid + "':'" + $(item).find("input[type=text]").val() + "',";
                                searchAttrIds += arrtid + "$";
                                searchAttrVals += $(item).find("input[type=text]").val() + "$";
                            }
                            break;
                        case "3":
                            //date
                            var dFrom = $(item).find(".datepickerFrom").val();
                            var dTo = $(item).find(".datepickerTo").val();
                            if (dFrom != "" || dTo != "") {
                                //searchStr += "'a" + arrtid + "':'" + (dFrom == "" ? "NA" : dFrom) + "_" + (dTo == "" ? "NA" : dTo) + "',";
                                searchAttrIds += arrtid + "$";
                                searchAttrVals += (dFrom == "" ? "NA" : dFrom) + "_" + (dTo == "" ? "NA" : dTo) + "$";
                            }
                            break;
                        case "4":
                        case "5":
                            //person
                            if ($(item).find(".userspan").length > 0) {
                                var userids = "";
                                $(item).find(".userspan").each(function (j, jitem) {
                                    userids += $(jitem).find("input[type=hidden]").val() + ",";
                                });
                                userids = userids.substr(0, userids.length - 1);
                                //searchStr += "'a" + arrtid + "':'" + userids + "',";
                                searchAttrIds += arrtid + "$";
                                searchAttrVals += userids + "$";
                            }
                            break;
                        case "7":
                            //enum
                            if ($(item).find("select").val() != "0") {
                                searchAttrIds += arrtid + "$";
                                searchAttrVals += $(item).find("select").val() + "$";
                            }
                            break;
                    }
                });
                if (searchAttrIds == "") {
                    loadMore(pageIndex, docid);
                    getTotal("", "");
                } else {
                    searchAttrIds = searchAttrIds.substr(0, searchAttrIds.length - 1);
                    searchAttrVals = searchAttrVals.substr(0, searchAttrVals.length - 1);
                    getTotal(searchAttrIds, searchAttrVals);
                    searchMore(pageIndex, docid);
                }
            });
        });
        
    </script>
</asp:Content>
