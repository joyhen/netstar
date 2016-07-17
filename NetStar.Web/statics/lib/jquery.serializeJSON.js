; (function ($) {
    'use strict';
    $.fn.serializeJSON = function () {
        var json = {};
        $.each(this.find('input,select,textarea'), function (i) {
            var el = $(this),
              key = el.attr('name'),
              val = el.val();

            //if (val != '' && val !== undefined && val !== null) {
            if (val !== undefined && val !== null) {
                if (el.is(':checkbox')) {
                    val = el.prop('checked') ? "1" : "0";
                    //el.prop('checked') && ($.isArray(json[key]) ? json[key].push(val) : json[key] = [val]);
                    $.isArray(json[key]) ? json[key] = [val] : json[key] = val;
                } else if (el.is(':radio')) {
                    el.prop('checked') && (json[key] = val);
                } else if (el.is('select')) {
                    json[key] = el.val();
                } else if (el.is('input[type="reset"]') || el.is(':button') || el.is('input[type="file"]')) {

                } else {
                    json[key] = val;
                }
            }
        });

        return json;
    };
})(jQuery);

//(function($){
//    $.fn.serializeJSON=function() {
//        var json = {};
//        jQuery.map($(this).serializeArray(), function(n, i){
//            (json[n['name']] === undefined) ? json[n['name']] = n['value'] : json[n['name']] += ',' + n['value'];
//        });
//        return json;
//    };
//})(jQuery);