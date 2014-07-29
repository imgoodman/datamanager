<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StepAdd.aspx.cs" Inherits="App.Web.DocConfig.StepAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header">
        <h1 class="pagetitle">
            逐步新增文档对象</h1>
    </div>
    <p class="message warning hidden">
        <span class="ui-icon ui-icon-info" style="float: left; margin-right: .3em;"></span>
        <strong>Hey!</strong></p>
    <div id="steps">
        <ul>
            <li class="selected">第一步：输入文档对象基本信息</li>
            <li>第二步：配置文档对象的属性信息</li>
            <li>第三步：确认保存</li>
        </ul>
        <div class="clear">
        </div>
    </div>
    <div id="step_1">
        <fieldset class="form">
            <legend>文档对象基本信息</legend>
            <div class="control_group">
                <label class="control_label">
                    文档对象类别</label>
                <div class="controls">
                    <div id="jstree_doctype" style="max-height: 100px;">
                    </div>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    文档对象名称</label>
                <div class="controls">
                    <input type="text" class="input_medium" id="docName" />
                    <p class="help_block">
                        文档名称比如是“会议纪要”，“审定信函”等。</p>
                </div>
            </div>
            <div class="control_group">
                <label class="control_label">
                    描述</label>
                <div class="controls">
                    <textarea class="input_large multitext" id="desc"></textarea>
                </div>
            </div>
        </fieldset>
        <div class="btnsteps right">
            <a href="#" class="btn btn_primary btn_next_1">下一步</a></div>
    </div>
    <div id="step_2" class="hidden">
        <fieldset class="form">
            <legend>文档对象属性信息</legend>
            <div class="attritem relative">
                <div class="control_group">
                    <label class="control_label">
                        属性名称</label>
                    <div class="controls">
                        <input type="text" class="input_medium attrname" />
                        <p class="help_block">
                            属性名称比如是“编号”，“标题”等。</p>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        属性类型</label>
                    <div class="controls" style="padding-top: 3px; margin-right: 20px;">
                        <input type="radio" title="内容简单文本" name="attrType" value="Text" /><span>单字符串</span>
                        <input type="radio" title="内容为复杂文本" name="attrType" value="RichText" /><span>多字符串</span>
                        <input type="radio" title="仅一个人参与" name="attrType" value="Person" /><span>单人</span>
                        <input type="radio" title="多个人参与" name="attrType" value="MultiPerson" /><span>多人参与</span>
                        <input type="radio" title="日期时间" name="attrType" value="Date" /><span>日期</span>
                        <input type="radio" title="文件" name="attrType" value="File" /><span>附件</span>
                        <input type="radio" title="枚举型属性值" name="attrType" value="EnumVal" /><span>枚举</span>
                        <input type="radio" title="关联其它文档对象" name="attrType" value="Obj" class="hidden" /><span
                            class="hidden">对象</span>
                        <div class="attr_content hidden" style="margin-top: 5px;">
                            <input type="text" title="输入枚举类型的值" class="hidden attrval" /><a href="#" class="btn attrval_add hidden"
                                title="新增属性值">新增</a><a href="#" class="btn attrval_obj hidden" title="选择关联的文档对象">选择</a>
                        </div>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        是否内容必填</label>
                    <div class="controls" style="padding-top: 3px;">
                        <input type="radio" title="内容不能为空" name="isRequired" value="true" /><span>是</span>
                        <input type="radio" title="内容允许为空" name="isRequired" value="false" /><span>否</span>
                    </div>
                </div>
                <div class="control_group">
                    <label class="control_label">
                        是否关键搜索属性</label>
                    <div class="controls" style="padding-top: 3px;">
                        <input type="radio" title="通常是单文本，日期，单人，多人属性" name="isSearch" value="true" /><span>是</span>
                        <input type="radio" title="通常是多文本和附件属性" name="isSearch" value="false" /><span>否</span>
                    </div>
                </div>
                <div class="abs order_action ui-state-default">
                    <span class="ui-icon ui-icon-arrowthick-1-n orderup" title="上移该属性"></span><span class="ui-icon ui-icon-close attr_del"
                        title="删除该属性"></span><span class="ui-icon ui-icon-arrowthick-1-s orderdown" title="下移该属性">
                        </span>
                </div>
            </div>
            <div class="more">
                增加属性
            </div>
        </fieldset>
        <div class="btnsteps right">
            <a href="#" class="btn btn_pre_1">上一步</a> <a href="#" class="btn btn_primary btn_next_2">
                下一步</a></div>
    </div>
    <div id="step_3" class="hidden">
        <div class="btnsteps right">
            <a href="#" class="btn btn_prev_2">上一步</a> <a href="#" class="btn btn_primary">完成</a></div>
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            var docID = request("id");
            var selDocTypeID = "";
            //console.log(docID);
            if (docID != "") {
                //加载文档信息 
                $.ajax({
                    type: "POST",
                    url: "/handlers/tempdocConfig/getItemById.ashx",
                    data: { id: docID },
                    dataType: "json",
                    success: function (res) {
                        if (res != null) {
                            $("#docName").val(res.DocName);
                            selDocTypeID = res.DocType.ID;
                            $("#desc").val(res.Description);
                            $.each(res.Attrs, function (i, item) {
                                var $temp = $(".itemtemp").clone();
                                $temp.find(".attrname").text(item.AttrName);
                                if (showType(item.AttrType) == "枚举") {
                                    var attrvals = showType(item.AttrType) + "{枚举值为:";
                                    $.each(item.AttrVals, function (j, jitem) {
                                        attrvals += jitem.AttrValue + ",";
                                    });
                                    attrvals = attrvals.substr(0, attrvals.length - 1);
                                    attrvals += "}";
                                    $temp.find(".attrtype").text(attrvals);
                                } else
                                    $temp.find(".attrtype").text(showType(item.AttrType));

                                $temp.find(".isrequired").text(showBool(item.IsRequired));
                                $temp.find(".issearch").text(showBool(item.IsSearch));
                                $temp.find("input[type=hidden]").val(item.ID);
                                lastOrder = item.VerticalOrder;
                                $temp.removeClass("hidden itemtemp").appendTo($(".listattr"));
                            });
                        } else {
                            window.location.href = "/404.aspx";
                        }
                    }
                });
            }
            /********************************methods in first step**********************************/
            //load doc types
            var selDocTypeID = "";
            $.ajax({
                type: "POST",
                url: "/handlers/docType/getDocTypes.ashx",
                data: {isVirtual:$("#isVirtual").val()},
                dataType: "json",
                success: function (res) {
                    $("#jstree_doctype").jstree({
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
                        selDocTypeID = r[0];
                        selDocTypeID = selDocTypeID.replace('d', '');
                        //console.log(selDocTypeID);
                    });
                }
            });
            var isExist = false;
            //验证该文档名称是否已经存在
            $("#docName").blur(function () {
                $.ajax({
                    type: "POST",
                    url: "/handlers/tempdocConfig/IsDocExist.ashx",
                    data: { docName: $(this).val() },
                    dataType: "json",
                    success: function (res) {
                        if (res == "1") {
                            showMessage("该文档名称已经存在，请修改。");
                            InfoTip.showMessage("该文档名称已经存在，请修改。", "warning", 6000);
                            isExist = true;
                        } else {
                            hideMessage();
                            isExist = false;
                        }
                    }
                });
            });
            //next step in first step
            $(".btn_next_1").click(function (e) {
                e.preventDefault();
                if (selDocTypeID == "") {
                    showMessage("请选择文档类别");
                    InfoTip.showMessage("请选择文档类别", "warning", 6000);
                    return false;
                }
                var docName = $("#docName").val();
                if (docName == "") {
                    showMessage("请输入文档名称。");
                    InfoTip.showMessage("请输入文档名称", "warning", 6000);
                    return false;
                }
                if (isExist) {
                    showMessage("该文档名称已经存在，请修改。");
                    InfoTip.showMessage("该文档名称已经存在，请修改。", "warning", 6000);
                    return false;
                }
                var desc = $("#desc").val();
                var userid = $("#userid").val();
                if (docID == "") {
                    $.ajax({
                        type: "POST",
                        url: "/handlers/tempdocConfig/AddDoc.ashx",
                        data: { docName: docName, desc: desc, userid: userid, docType: selDocTypeID },
                        dataType: "json",
                        success: function (res) {
                            if (res != "0" && res != "-1") {
                                $("#step_1").slideUp(function () {
                                    $("#steps li").removeClass("selected").eq(1).addClass("selected");
                                    $("#step_2").show();
                                });
                            } else {
                                showMessage("操作失败");
                                InfoTip.showMessage("操作失败", "error", 6000);
                            }
                        }
                    });
                } else {
                    //update current doc
                    $.ajax({
                        type: "POST",
                        url: "/handlers/tempdocConfig/UpdateDoc.ashx",
                        data: { id: docID, docName: docName, desc: desc, userid: userid, docType: selDocTypeID },
                        dataType: "json",
                        success: function (res) {
                            if (res != "0" && res != "-1") {
                                $("#step_1").slideUp(function () {
                                    $("#steps li").removeClass("selected").eq(1).addClass("selected");
                                    $("#step_2").show();
                                });
                            } else {
                                InfoTip.showMessage("操作失败", "error", 6000);
                                showMessage("操作失败");
                            }
                        }
                    });
                }
            });

            /***********************************methods in second step*****************************************************/
            //previous step
            $(".btn_pre_1").click(function (e) {
                e.preventDefault();
                $("#step_2").slideUp(function () {
                    $("#steps li").removeClass("selected").eq(0).addClass("selected");
                    $("#step_1").show();
                });
            });
            //next step
            $(".btn_next_2").click(function (e) {
                e.preventDefault();
                $("#step_2").slideUp(function () {
                    $("#steps li").removeClass("selected").eq(2).addClass("selected");
                    $("#step_3").show();
                });
            });
            /***********************************methods in final step******************************************************/
            //previou step
            $(".btn_prev_2").click(function (e) {
                e.preventDefault();
                $("#step_3").slideUp(function () {
                    $("#steps li").removeClass("selected").eq(1).addClass("selected");
                    $("#step_2").show();
                });
            });
        });
    </script>
</asp:Content>
