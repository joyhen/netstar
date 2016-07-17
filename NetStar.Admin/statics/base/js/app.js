$(function () {
    //刷新
    $('#refreashBtn').click(function () { location.reload(true); });


});

var commModel = {
    latticeTable: function () {
        $(".mouse:odd").css("background", "#FFFCEA");
    },
};

var loginModel = {
    userlogin: function () {
        var _f = function (msg) {
            $('.tipmsg').html(msg);
        };

        if (inputcheck(_f)) {
            $('.tipmsg').html('&nbsp;');

            var paramdata = {
                action: "934fdb86f88159dd000255872fea60d0",
                arg: $('#myform').serializeJSON()
            };

            doAjaxPost(paramdata, loginModel.logincallback);
        }
    },
    logincallback: function (result) {
        if (!(result.success)) {
            SuperSite.MsgFailed(result.msg);
            $('#imgcode').trigger('click');
            return;
        }

        if (result.data == null || result.data == '') {
            window.location.href = "index.aspx";
            return;
        }

        $("#myform").fadeOut();

        layer.prompt({
            title: result.data,
            formType: 0,
            maxlength: 40,
            move: false,
            closeBtn: false,
            offset: ['240px']
        }, function (pass) {
            var _p = { action: '1e43711651eea8f1b3a6485a59a369ab', arg: { answer: pass } };
            doAjaxPost(_p, function (_result) {
                if (_result.success) {
                    window.location.href = "index.aspx";
                } else {
                    SuperSite.MsgError(_result.msg);
                }
            });
        });
    },
    changeValidCode: function () {
        document.getElementById("code").value = "";

        var dt = new Date().getTime();
        $('#imgcode').attr('src', "control/validatecode.ashx?p=" + dt);
    },
    init: function () {
        var self = this;

        $(function () {
            $('#imgcode').click(self.changeValidCode);

            $('#btnlogin').bind('click', self.userlogin);

            $("input[class*='text']").on("keydown", function (e) {
                if (e.keyCode == 13) {
                    self.userlogin();
                }
            });

            //...
        });
    }
};

var noticeModel = {
    callback: function (result) {
        if (result.success) {
            location.href = 'noticetype.aspx';
        } else {
            parent.SuperSite.MsgFailed(result.msg);
        }
    },
    doNoticeAction: function (_action, _callback) {
        var _f = function (msg) {
            parent.SuperSite.MsgFailed(msg);
        };
        var _chkitem = $('input[validate],textarea[validate]');
        var _url = location.href;

        if (parent.inputcheck(_f, _chkitem, _url)) {
            var paramdata = {
                action: _action,
                arg: $('#myform').serializeJSON()
            };
            parent.doAjaxPost(paramdata, _callback || noticeModel.callback);
        }
    },
    doNoticeMsg: function (actionname) {
        noticeModel.doNoticeAction(actionname, function (result) {
            if (result.success) {
                location.href = 'noticemsg.aspx';
            } else {
                parent.SuperSite.MsgFailed(result.msg);
            }
        });
    },
    getAppVersion: function (target) {
        var tag = parseInt(target.val());
        target.next('span').remove();

        var _param = {
            action: "e08907d788d5a9ceafafe8d2029dd8cc",
            arg: { typeTag: tag }
        };
        parent.getJsonData(_param, function (result) {
            parent.renderTemplate(result, $('#template').html(), function (view) {
                target.after(view);
            });
        });
    },
    add: function () {
        $(function () {
            $('.submit').click(function () {
                noticeModel.doNoticeAction("6a24d4bf13f76fea4c5c5066d10cb2d3");
            });

            $('.ccaddconfigtime').click(function () {
                var __t = $('#template').html();
                $(this).parent().parent().parent().before(__t);
            });

            $('#myform').delegate('.ccsubconfigtime', 'click', function () {
                $(this).parent().parent().parent().remove();
            });
        });
    },
    edit: function () {
        var myurl = parent.parseUri(location.href);
        var gid = myurl.queryKey.id;

        $(function () {
            var param = {
                action: "5e0ed7d868e051e38bcbeb7b9517828f",
                arg: { id: gid }
            };
            parent.getJsonData(param, function (result) {
                if (result.success) {
                    parent.renderTemplate(result.data, $('#template').html(), function (view) {
                        $('.v52fmbx').html(view);
                        $('.ccselect').val(result.data.category); //设置父级
                    });
                } else {
                    parent.SuperSite.MsgFailed(result.msg);
                }
            });

            //修改
            $('#myform').delegate('.submit', 'click', function () {
                noticeModel.doNoticeAction("6a1ceea89d096ee2e8b28e6ca37235e6");
            });
        });
    },
    init: function () {
        $(function () {
            //加载数据
            parent.renderTable(
                { action: "4ae1109be457ba23957be22267add1a3" },
                $('.table'),
                $('#template').html(),
                commModel.latticeTable
            );

            //全选
            $("#chkAll").click(function () {
                var chks = $('.table tr:gt(0)').find("input[type='checkbox']");

                if ($(this).attr('checked') == 'checked') {
                    chks.attr('checked', true);
                } else {
                    chks.attr('checked', false);
                }
            });

            //批量删除
            $('#delnoticetype').click(function () {
                var chklist = $('.table tr:gt(0)').find(':checked');
                if (chklist.length == 0) {
                    parent.SuperSite.MsgFailed(parent.$.msg.common.deleteitems);
                    return;
                }

                parent.confirmLayerNormal("确定删除吗？", function (index) {
                    var ids = chklist.map(function (idx, dom) { return $(dom).attr('value'); });
                    var paramdata = {
                        action: "b747b1ed5ca8316d652f3caad22ce2db",
                        arg: { dids: ids.get().join('&') }
                    };

                    parent.doAjaxPost(paramdata, function (result) {
                        if (result.success) {
                            parent.layer.close(index);
                            location.reload(true);
                        } else {
                            parent.SuperSite.MsgError(result.msg);
                        }
                    });
                });
            });

            //配置、停用、启用、删除
            $('.table').delegate('.td_delete, .ccbtnconfig, .ccbtnsuccess, .ccbtnerror', 'click', function () {
                var __did = $(this).attr('ccvalue');
                var __tag = $(this).attr('tag');
                var __msg = parseInt(__tag) == 0 ? '停用此通知类型吗？' : (parseInt(__tag) == 1 ? '启用此通知类型吗？' : '删除此通知类型吗？');

                if (parseInt(__tag) == 3) {
                    parent.SuperSite.MsgWarning('此功能尚未开放');
                    //var thisname = $(this).attr('ccname');
                    //parent.OpeniframeLayer('配置 - ' + thisname, 'noticetype_conf.aspx?id=' + __did, ['800px', '450px'], false);
                    return;
                };

                parent.confirmLayerNormal(__msg, function (index) {
                    var paramdata = {
                        action: "20b496ec015e5e0b0ab15207ceb95aa2",
                        arg: { id: __did, tp: __tag }
                    };

                    parent.doAjaxPost(paramdata, function (result) {
                        if (result.success) {
                            parent.layer.close(index);
                            location.reload(true);
                        } else {
                            parent.SuperSite.MsgError(result.msg);
                        }
                    });
                });
            });

            //...

        });
    },
    msginit: function () {
        $(function () {
            //加载数据
            parent.renderTable(
                { action: "baa96e6444090469d64a4b030f64557f", arg: { idx: $('#hidpageindex').val(), tp: $('#nttype').val() } },
                $('.table'),
                $('#template').html(),
                commModel.latticeTable
            );

            //分页
            $('#pagefrist').click(function () { $('#hidpageindex').val('1'); loadPageData(0); });
            $('#pageprev').click(function () { loadPageData(-1) });
            $('#pagenext').click(function () { loadPageData(1) });
            $('.submit').click(function () { loadPageData(); });

            function loadPageData(idx) {
                var $datatable = $('.table');
                var $pageidxdom = $('#hidpageindex');
                var $st = $('#sttime').val();
                var $et = $('#edtime').val();
                if (($.trim($st) == '' && $.trim($et) != '') || ($.trim($st) != '' && $.trim($et) == '')) {
                    parent.SuperSite.MsgWarning('查询时段不完整'); return;
                }

                var _idx = parseInt($pageidxdom.val()) + (idx || 0);
                if (_idx <= 0) _idx = 1;

                parent.renderTable(
                    {
                        action: "baa96e6444090469d64a4b030f64557f", arg: {
                            idx: _idx,
                            st: $st,
                            et: $et,
                            tp: $('#nttype').val()
                        }
                    },
                    $datatable,
                    $('#template').html(),
                    commModel.latticeTable,
                    function () {
                        $pageidxdom.val(_idx);
                    }
                );
            };

            //...

        });
    },
    msgadd: function () {
        var _target = $('select[name="clienttype"]');
        noticeModel.getAppVersion(_target); //初始化

        _target.change(function () { noticeModel.getAppVersion($(this)); }); //版本   

        //新增
        $('.submit').click(function () {
            var _target = $('#url');
            if (parseInt($('select[name="noticetype"]').val()) == 4) {
                if ($.trim(_target.val()) == '') {
                    _target.focus();
                    parent.SuperSite.MsgWarning('您尚未填写活动链接'); return;
                }
            };
            noticeModel.doNoticeMsg('e5162d95e23b7592863f57b2a66741ae');
        });
    },
    msgedit: function ($versionTick) {
        //版本
        $versionTick.change(function () { noticeModel.getAppVersion($(this)); });
        //编辑
        $('.submit').click(function () {
            var _target = $('#url');
            if (parseInt($('select[name="noticetype"]').val()) == 4) {
                if ($.trim(_target.val()) == '') {
                    _target.focus();
                    parent.SuperSite.MsgWarning('您尚未填写活动链接'); return;
                }
            };
            noticeModel.doNoticeMsg('2f31296c6b2a06ace7dba43aa3cc6b1c');
        });
    }
};