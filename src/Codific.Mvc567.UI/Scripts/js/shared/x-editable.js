(function ($) {
    'use strict';
    $(function () {
        if ($('.application-editable-form').length) {
            $.fn.editable.defaults.mode = "inline";
            $.fn.editable.defaults.send = "always";
            $.fn.editable.defaults.ajaxOptions = { type: "POST" };
            $.fn.editableform.buttons =
                '<button type="submit" class="btn btn-primary editable-submit p-0 ml-2">' +
                '<i class="mdi mdi-check m-0 font-size-20"></i>' +
                '</button>' +
                '<button type="button" class="btn btn-default editable-cancel p-0 ml-2">' +
                '<i class="mdi mdi-cancel m-0 font-size-20"></i>' +
                '</button>';

            $('.application-editable-form-input-text').editable({
                success: function (data, config) {
                    console.log(data);
                },
                error: function (errors) {
                    console.log(errors);
                }
            });

            $('.application-editable-form-input-date').editable({
                format: 'yyyy-mm-dd',
                viewformat: 'dd/mm/yyyy',
                datetimepicker: {
                    todayBtn: 'linked',
                    weekStart: 1
                },
                success: function (data, config) {
                    console.log(data);
                },
                error: function (errors) {
                    console.log(errors);
                }
            });
        }
    });
})(jQuery);