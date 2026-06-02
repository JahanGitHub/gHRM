$(function () {  
    $("#CompanyPhone_NotUsed").keyup(function (e) {
        var isNumeric = app.checkNumeric(e);

        if (!isNumeric) {
            $(this).val('');
        };        
    });
})

function showMyImage(fileInput, imageSelector, inputImageSelector) {
    var files = fileInput.files;
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var imageType = /image.*/;

        if (file.type.match(imageType)) {
            //continue;
            var size = Math.round(file.size / 1024);
            if (size <= 100) {
                var img = document.getElementById(imageSelector);
                img.file = file;
                var reader = new FileReader();
                reader.onload = (function (aImg) {
                    return function (e) {
                        aImg.src = e.target.result;
                    };
                })(img);
                reader.readAsDataURL(file);
            }
            else
                alert('Image file cannot be greater than 100 KB.');
        }
        else {
            $(`#${inputImageSelector}`).replaceWith($(`#${inputImageSelector}`).clone(true));
            alert('Please select a valid image file');
        }

    }
}