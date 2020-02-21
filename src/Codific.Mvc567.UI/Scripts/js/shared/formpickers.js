(function ($) {
  'use strict';
  if ($(".timepicker-popup").length) {
      $('.timepicker-popup').datetimepicker({
          format: 'HH:mm'
    });
  }
  if ($(".color-picker").length) {
    $('.color-picker').asColorPicker();
  }
  if ($(".datepicker-popup").length) {
    $('.datepicker-popup').datepicker({
        enableOnReadonly: true,
        todayHighlight: true,
        weekStart: 1
    });
  }
  if ($("#inline-datepicker").length) {
    $('#inline-datepicker').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
      weekStart: 1
      // language: 'en' For English Language
    });
  }
  if ($(".datepicker-autoclose").length) {
    $('.datepicker-autoclose').datepicker({
      weekStart: 1,
      autoclose: true
    });
  }
  if ($('input[name="date-range"]').length) {
    $('input[name="date-range"]').daterangepicker({
      weekStart: 1
    });
  }
  if ($('input[name="date-time-range"]').length) {
    $('input[name="date-time-range"]').daterangepicker({
      timePicker: true,
      timePickerIncrement: 30,
      weekStart: 1,
      locale: {
        format: 'MM/DD/YYYY h:mm A'
      }
    });
  }
})(jQuery);