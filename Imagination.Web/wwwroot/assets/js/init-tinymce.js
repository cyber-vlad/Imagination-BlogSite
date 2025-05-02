function initTinyEditor() {
    tinymce.init({
        selector: '#myTextarea',
        setup: function (editor) {
            editor.on('change', function () {
                $('#myTextarea').val(editor.getContent());
            });
        },
        menubar: false,
        plugins: 'link image code lists',
        toolbar: 'undo redo | bold italic | alignleft aligncenter alignright | bullist numlist | link image',
        height: 300
    });
}
