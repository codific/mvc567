(function ($) {
    'use strict';
    $(function () {
        if ($('.application-editable-form').length) {
            $.fn.editable.defaults.mode = "inline";
            $.fn.editable.defaults.send = "always";
            $.fn.editable.defaults.onblur = "submit";
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
                    $('.inline-edit-errors-container p').html('');
                },
                error: function (xhr) {
                    $('.inline-edit-errors-container p').html(xhr.responseText);
                }
            }).on('hidden', function() {
                $('.inline-edit-errors-container p').html('');
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
            $('.x-edit-select').editable({
                success: function (data, config) {
                    $('.inline-edit-errors-container p').html('');
                },
                error: function (xhr) {
                    $('.inline-edit-errors-container p').html(xhr.responseText);
                }
            }).on('hidden', function() {
                $('.inline-edit-errors-container p').html('');
            });

            $('.x-edit-bool').editable({
                success: function (data, newValue) {
                    $('.inline-edit-errors-container p').html('');
                    if (newValue === '0') {
                        $(this).siblings('.bool-cell-editable-value-true').removeClass('selected-bool-value').addClass('d-none');
                        $(this).siblings('.bool-cell-editable-value-false').addClass('selected-bool-value').removeClass('d-none');
                    } else {
                        $(this).siblings('.bool-cell-editable-value-false').removeClass('selected-bool-value').addClass('d-none');
                        $(this).siblings('.bool-cell-editable-value-true').removeClass('d-none').addClass('selected-bool-value');
                    }
                },
                error: function (xhr) {
                    $('.inline-edit-errors-container p').html(xhr.responseText);
                }
            }).on('hidden', function($event, reason) {
                $('.inline-edit-errors-container p').html('');
                $(this).siblings('.bool-cell-editable-value.selected-bool-value').css('display', 'inline-block');   
            });
            $('.bool-cell-editable-value').on('click', function ($event) {
                $event.stopPropagation();
                $(this).css('display', 'none');
                $(this).siblings('.x-edit-bool').click();
            })
        }
    });
})(jQuery);