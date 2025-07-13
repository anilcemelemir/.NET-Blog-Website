(function ($) {
    'use strict';

    $(function () {
        // Mevcut kodlarýnýzýn yaný sýra dosya yükleme iþlevlerini de ekleyin

        // Dosya yükleme butonuna týklama
        $('.file-upload-browse').on('click', function (event) {
            event.preventDefault();  // Formun otomatik submit edilmesini engelle

            var file = $(this).closest('.form-group').find('.file-upload-default');
            file.trigger('click');
        });

        // Dosya seçildiðinde dosya adýný göster
        $('.file-upload-default').on('change', function () {
            var fileName = $(this).val().replace(/C:\\fakepath\\/i, '');
            $(this).closest('.form-group').find('.file-upload-info').val(fileName);
        });

        // Diðer mevcut script kodlarýnýz burada devam eder
    });

})(jQuery);
