(function ($) {
    'use strict';

    $(function () {
        // Mevcut kodlar�n�z�n yan� s�ra dosya y�kleme i�levlerini de ekleyin

        // Dosya y�kleme butonuna t�klama
        $('.file-upload-browse').on('click', function (event) {
            event.preventDefault();  // Formun otomatik submit edilmesini engelle

            var file = $(this).closest('.form-group').find('.file-upload-default');
            file.trigger('click');
        });

        // Dosya se�ildi�inde dosya ad�n� g�ster
        $('.file-upload-default').on('change', function () {
            var fileName = $(this).val().replace(/C:\\fakepath\\/i, '');
            $(this).closest('.form-group').find('.file-upload-info').val(fileName);
        });

        // Di�er mevcut script kodlar�n�z burada devam eder
    });

})(jQuery);
