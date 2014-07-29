function showType(type) {
    switch (type) {
        case 1:
            return "单字符串";
        case 2:
            return "多字符串";
        case 3:
            return "日期";
        case 4:
            return "单人";
        case 5:
            return "多人参与";
        case 6:
            return "附件";
        case 7:
            return "枚举";
        default:
            return "单字符串";
    }
};
function setType(type) {
    var text = "Text";
    switch (type) {
        case "1":
            text = "Text";
            break;
        case "2":
            text = "RichText";
            break;
        case "3":
            text = "Date";
            break;
        case "4":
            text = "Person";
            break;
        case "5":
            text = "MultiPerson";
            break;
        case "6":
            text = "File";
            break;
        case "7":
            text = "EnumVal";
            break;
    }
    //alert("type is " + text);
    $("input[value='" + text + "']").prop("checked", true);
};
function request(paras) {
    var url = location.href;  //获取当前url地址
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
};
function showString(str) {
    if (str == "" || str == null)
        return "暂无";
    else
        return str;
};
function getString(str) {
    if (str == "暂无")
        return "";
    else
        return str;
};
function showBool(b) {
    if (b == true || b.toString == "true")
        return "是";
    else
        return "否";
};
function showMessage(msg) {
    $(".message strong").html(msg);
    $(".message").removeClass("hidden");
    //alert(msg);
};
function hideMessage() {
    $(".message").addClass("hidden");
};
function emptyContainer($parent) {
    //清空textbox，textarea,radio,checkbox,hidden
    $parent.find("input[type=text],textarea,input[type=hidden]").val('');
    $parent.find("input[type=radio],input[type=checkbox]").prop("checked", false);
};
function trimLastComma(str) {
    return str.substr(0, str.length - 1);
};
function showLongString(str, count) {
    if (str == "")
        return "暂无";
    else {
        if (str.length > count) {
            return str.substr(0, count)+"...";
        } else {
            return str;
        }
    }
};