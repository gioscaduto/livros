function FormatarCampoDateTimePicker() {

    $('.data').datetimepicker({
        ignoreReadonly: false,
        format: 'DD/MM/YYYY',
        locale: 'pt-br'
    });

}