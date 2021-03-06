﻿document.write("<script type='text/javascript' src='statics/lib/parseUri.1.2.2.js'></script>");
document.write("<script type='text/javascript' src='statics/lib/jquery.serializeJSON.js'></script>");
document.write("<script type='text/javascript' src='statics/lib/layer-v2.0/layer/layer.js'></script>");
document.write("<script type='text/javascript' src='statics/lib/mustache.min.js'></script>");

//get方式的参数对象（暂时没有用到）
var globalAjaxParamGet = 'ajaxparam';

//固定浮动
$.fn.smartFloat = function (width_p) {

    var position = function (element) {
        var top = element.position().top,
            pos = element.css("position");

        $(window).scroll(function () {

            var scrolls = $(this).scrollTop();

            if (scrolls > top) {
                if (window.XMLHttpRequest) {
                    element.css({
                        position: "fixed",
                        'z-index': 999,
                        width: width_p,
                        top: 0
                    });
                } else {
                    element.css({ top: scrolls });
                }
            } else {
                element.css({
                    position: "", //absolute  
                    top: top
                });
            }
        });
    };

    return $(this).each(function () {
        position($(this));
    });
};

function getValidateMsg($validatedom, currenturl) {
    var msgtip = "can't be empty";                      //out msg
    var msgmodel = '';                                  //smg model
    var msgkey = '';                                    //msg key
    var vtxt = $.trim($validatedom.attr('validate'));   //model$key

    if (vtxt == '') return msgtip;

    var arr = vtxt.split('$');
    if (arr.length >= 2) {
        msgmodel = $.trim(arr[0]);
        msgkey = $.trim(arr[1]);
    }
    else if (arr.length == 1) {
        msgkey = $.trim(arr[0]);

        var $modeltxt = $validatedom.attr('msgmodel');
        if ($modeltxt && $.trim($modeltxt).length > 0) {
            msgmodel = $.trim($modeltxt);
        }
    }

    if (msgkey == '') {
        return msgtip;
    }

    if (msgmodel == '') {
        var myurl = parseUri(currenturl || location.href);
        if (myurl.directory == myurl.path) { //伪静态的情况下
            var _arr = myurl.path.split('/');
            msgmodel = _arr[_arr.length - 1];
        } else {
            var currentfile = myurl.file;
            var idx = currentfile.lastIndexOf('.');
            msgmodel = currentfile.substring(0, idx);
        }
    }

    if (msgmodel == '') return msgtip;

    var tips = $.msg[msgmodel];
    if (tips && tips[msgkey]) {
        msgtip = tips[msgkey];
    }

    return msgtip;
}

//避免重复提交（间隔3秒钟，暂时没有用到,需要修改下）
function avoidSubmit(dom, fn) {
    var avoidbutton = $(dom).attr('avoidfrequent');
    if (avoidbutton != undefined) {
        var txt = $(dom).val(); //需要修改
        var tag = parseInt($(dom).attr('submittag') || '0');

        if (tag == 0) {
            $(dom).attr({ submittag: "1", value: "..." });
            $(dom).unbind('click');

            setTimeout(function () {
                $(dom).attr({ submittag: "0", value: txt });
                $(dom).bind('click', fn);
            }, 1000 * 3);
        }
    }
}

//输入检查
function inputcheck(tipFn, chkitem, currenturl) {
    var tag = false;
    var url = currenturl || location.href;
    var checkitem = chkitem || $('input[validate],textarea[validate]');

    if (checkitem.length > 0) {
        checkitem.each(function (idx, dom) {
            var tar = $(dom);
            if ($.trim(tar.val()) == '') {
                if (typeof tipFn == "function") {
                    tipFn(getValidateMsg(tar, url));
                }

                tar.focus();
                tag = false;
                return false;
            } else {
                tag = true;
            }
        });
    }

    return tag;
};

$(function () {

    layer.config({
        extend: '/extend/layer.ext.js',
        //skin: 'layer-ext-moon', //一旦设定，所有弹层风格都采用此主题。
        //extend:'skin/moon/style.css'
    });

    //输入框焦点样式处理
    $("input[class*='text']").focus(function () {
        $(this).addClass('metfocus');
    }).focusout(function () {
        $(this).removeClass('metfocus');
    });

    //固定面包屑
    $(".metinfotop").smartFloat($(".v52fmbx_tbmax").width());

    //回到顶部
    $('body').append('<div class="gototop" style="display:none;"><span title="回到顶部"></span></div>');
    $('.gototop').click(function () {
        $('html,body').animate({ scrollTop: '0px' }, 300);
    });
    //回到顶部显示与隐藏
    $(window).scroll(function () {
        if ($(window).scrollTop() >= 100) {
            $('.gototop').fadeIn(300);
        } else {
            $('.gototop').fadeOut(300);
        }
    });



});

//鼠标移动到输入框上自动选中内容
function selectAllInputText($dom) {
    $dom.hover(function () {
        $(this).focus();
        $(this).select();
    }, function () {
        $(this).blur();
    });
};

var IndexNavigation = 0;
///导航
function showNavigationPanel() {
    if (IndexNavigation != 0) {
        layer.close(IndexNavigation)
    } else {
        IndexNavigation = layer.open({
            type: 2, //0~2
            title: '管理中心导航',
            skin: 'layui-layer-rim', //加上边框
            area: ['750px', '470px'], //宽高
            shade: false,
            content: 'control/navigation.html',
            cancel: function (_index) {
                IndexNavigation = 0;
            },
            end: function () {
                IndexNavigation = 0;
            }
        });
    }
};

//需要修改
function resetDataTable(msg) {
    var _loading = '<tr><td class="td_nodata">没有数据</td></tr>';

    var _tb = $('.table');
    var _tr = _tb.find('tr');
    if (_tr.length <= 0) {
        _tb.html(_loading);
    } else if (_tr.length == 2) {
        _tb.find('tr:last').remove();
        _tb.html(_loading);
    }
};

//渲染表格
function renderTable(param, $table, template, callback, injectfn) {
    getJsonData(param, function (result) {
        if (!(result.success)) {
            SuperSite.MsgFailed(result.msg);
            return;
        }

        $table.find('tr:gt(0)').remove();

        if (result.data) {
            if (typeof injectfn == "function") { injectfn(); }
            renderTemplate(result, template, function (view) {
                $table.append(view);
            });
        } else {
            $table.append('<tr><td colspan="15" class="list td_nodata">没有数据</td></tr>');
        }
        if (typeof callback == "function") { callback(); }
    });
}

//渲染模板
function renderTemplate(data, template, callback) {
    Mustache.parse(template);
    var view = Mustache.render(template, data);
    if (typeof callback == "function") { callback(view); }
}

//获取Json数据
function getJsonData(param, callback) {
    var p = param || {};

    $.getJSON(globalRequestUrl, { ajaxparam: JSON.stringify(p) }, function (result) {
        if (typeof callback == "function") { callback(result); }
    });
};

//post提交
function doAjaxPost(paramdata, callback, errfn) {
    $.ajax({
        type: "post",
        dataType: "json",
        data: JSON.stringify(paramdata),
        url: globalRequestUrl,
        eache: false,
        success: function (result) {
            if (typeof callback == "function") {
                callback(result);
            }
        },
        error: errfn || function () {
            SuperSite.MsgError('error, please contact the administrator');
        }
    });
};

//获取html
function loadHtml($dom, url, callback, param) {
    var _p = param || {};

    $dom.load(url, _p, function () {
        if (typeof callback == "function") { callback(); }
    });
};

//Confirm窗口
function confirmLayerNormal(msg, callback) {
    layer.confirm(msg, { icon: 3, title: '提示', shadeClose: true, closeBtn: false }, function (_index) {
        if (typeof callback == "function") {
            callback(_index);
        }
    });
};
function confirmLayer(msg, btnarr, f1, f2) {
    layer.confirm(msg, {
        icon: 0,
        title: '提示',
        btn: btnarr, //['确定', '取消']
        closeBtn: false,
        shadeClose: true
    }, function (_index) {
        if (typeof f1 == "function") { f1(_index); }
    }, function (__index) {
        if (typeof f2 == "function") { f2(__index); }
    });
};

//打开iframe
function OpeniframeLayer(opentitle, openurl, layerwh, isclose, showmaxmin, isfull) {
    var _index = layer.open({
        type: 2,
        skin: 'layui-layer-lan',
        title: opentitle,
        fix: true,
        maxmin: (showmaxmin || false),
        shadeClose: (isclose || false),
        area: layerwh, //['535px', '340px']
        content: openurl
    });

    if (isfull) {
        layer.full(_index);
    }
};

var SuperSite = {
    //0感叹，1对号，2差号，3问号，4凸号，5苦脸，6笑脸
    MsgWarning: function (msg) {
        layer.msg(msg || 'Warning', { icon: 0 });
    },
    MsgOK: function (msg) {
        layer.msg(msg || 'OK', { icon: 1 });
    },
    MsgError: function (msg) {
        layer.msg(msg || 'Error', { icon: 2 });
    },
    MsgConfirm: function (msg) {
        layer.msg(msg || 'Confirm', { icon: 3 });
    },
    MsgLock: function (msg) {
        layer.msg(msg || 'Lock', { icon: 4 });
    },
    MsgFailed: function (msg) {
        layer.msg(msg || 'Failed', { icon: 5 });
    },
    MsgSuccess: function (msg) {
        layer.msg(msg || 'Success', { icon: 6 });
    }


    //...

};

//当前日期和时间
function setTime($dom) {
    var day = "";
    var month = "";
    var ampm = "";
    var ampmhour = "";
    var myweekday = "";
    var year = "";
    var myHours = "";
    var myMinutes = "";
    var mySeconds = "";
    mydate = new Date();
    myweekday = mydate.getDay();
    mymonth = mydate.getMonth() + 1;
    myday = mydate.getDate();
    myyear = mydate.getYear();
    myHours = mydate.getHours();
    myMinutes = mydate.getMinutes();
    mySeconds = mydate.getSeconds();
    myHours = parseInt(myHours) < 10 ? "0" + myHours : myHours;
    myMinutes = parseInt(myMinutes) < 10 ? "0" + myMinutes : myMinutes;
    mySeconds = parseInt(mySeconds) < 10 ? "0" + mySeconds : mySeconds;
    year = (myyear > 200) ? myyear : 1900 + myyear;

    if (myweekday == 0) {
        weekday = "星期日";
    } else if (myweekday == 1) {
        weekday = "星期一";
    } else if (myweekday == 2) {
        weekday = "星期二";
    } else if (myweekday == 3) {
        weekday = "星期三";
    } else if (myweekday == 4) {
        weekday = "星期四";
    } else if (myweekday == 5) {
        weekday = "星期五";
    } else if (myweekday == 6) {
        weekday = "星期六";
    }
    $dom.html(year + "年" + mymonth + "月" + myday + "日&nbsp;" + weekday + "&nbsp;" + myHours + ":" + myMinutes + ":" + mySeconds);
    setTimeout("setTime()", 1000);
};
