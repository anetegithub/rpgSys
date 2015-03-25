function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#preview').css('display', 'inline');
            $('#preview').attr('src', e.target.result);            
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$("#fileUpload").change(function () {
    readURL(this);
});

$('#fileUploadClean').click(function () {
    $('#preview').removeAttr('src');
    $('#preview').css('display', 'none');
});