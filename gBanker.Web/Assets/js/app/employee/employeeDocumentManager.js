
var documentTypeEnum = {
    Signature: "SIG",
    SpecialSymbol: "SPS",
    FingerPrint: "FGR"
}

var empDocumentManager = {
    init: function () {

    },

    LoadEmpInfo: function (employee_Code) {
        $.ajax({
            type: 'GET',
            contentType: "application/json; charset=utf-8",
            url: '/EmployeeSignature/GetEmployeeListByCode',
            data: { employee_Code: employee_Code },
            dataType: 'json',
            async: true,
            success: function (List_EmployeeViewModel) {
                if (List_EmployeeViewModel != "") {
                    $.each(List_EmployeeViewModel, function (index, emp) {
                        $('#EmployeeId').val(emp.EmployeeId);
                        $("#EmployeeName").val(emp.EmployeeName);
                        empDocumentManager.GetSignatureUploadImage(emp.EmployeeId);
                    });
                }
                else {
                    $.alert.open('alert', 'Invalid code');
                }
            },
            error: function (request, status, error) {
                alert(request.statusText + "/" + request.statusText + "/" + error);
            }
        });
    },

    showMyImage: function (fileInput) {
        var files = fileInput.files;
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var imageType = /image.*/;

            if (file.type.match(imageType)) {
                //continue;
                var size = Math.round(file.size / 1024);
                if (size <= 100) {
                    var img = document.getElementById("thumbnil");
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
                $("#ImgFile").replaceWith($("#ImgFile").clone(true));
                alert('Please select a valid image file');
            }
        }
    },
    GetSignatureUploadImage: function (EmployeeId) {
        var documentType = $('#DocumentType').val();
        $('#thumbnil').attr('src', '');
        $('#DocumentRemarks').val('');

        if (documentType == documentTypeEnum.Signature) {
            var urlString = 'url(/EmployeeSignature/RetrieveSignatureUploadImage/' + EmployeeId + ')';
            document.getElementById('thumbnil').src = '/EmployeeSignature/RetrieveSignatureUploadImage/' + EmployeeId;
        }
        else {
            $.ajax({
                type: 'GET',
                contentType: "application/json; charset=utf-8",
                url: '/EmployeeSignature/GetDocumentPartailPath?employeeId=' + EmployeeId + '&documentType=' + documentType,
                //data: { id: id },
                dataType: 'json',
                async: true,
                success: function (data) {
                    $('#thumbnil').attr('src', data.documentPartialPath);
                    $('#DocumentRemarks').val(data.remark);
                }
            });
        }
    }
}

$(document).ready(function () {
    $("#txtEmployeeCode").blur(function () {
        var employee_Code = $("#txtEmployeeCode").val();
        $("#EmployeeCode").val(employee_Code);
        empDocumentManager.LoadEmpInfo(employee_Code);
    });

    $("#btnSignatureSave").click(function (e) {
        e.preventDefault();

        if ($("#DocumentType").val() == "" || $("#DocumentType").val() == null) {
            $.alert.open("error", "Please Select Document type.")
            return;
        }

        if (!$("#ImgFile").val() && $("#DocumentType").val() == documentTypeEnum.Signature) {
            $.alert.open("error", "Signature not found.")
            return;
        }

        if ($("#txtEmployeeCode").val() != "" && $("#EmployeeName").val() != "" 
            && $("#DocumentType").val() != null) {

            var EmployeeId = $("#EmployeeId").val();
            var documentType = $("#DocumentType").val();

            $('#AjaxLoader').show();
            $('#SignatureUpload').ajaxSubmit({
                type: "POST",
                target: '#thumbnil',
                data: { EmployeeId: EmployeeId, DocumentType: documentType },
                dataType: 'json',
                resetForm: false,
                success: function (data) {
                    $('#AjaxLoader').hide();
                    $("#txtEmployeeCode").val("");
                    $("#EmployeeName").val("");
                    $("#ImgFile").val("");
                    //$.alert.open("alert", "Success, Upload Completed!")
                    
                    if (data.Result == "ERROR")
                        $.alert.open("alert", "Document not Upload!")
                    else
                        $.alert.open("alert", "Success, Upload Completed!")
                },
                error: function () {
                    $("#dialog-message").html('<p>Error, Fail to Save.</p>');
                    $("#dialog-message").dialog({
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                }
            });
        }
        else {
            $.alert.open("error", "All Information required")
        }
    });

    $("#DocumentType").on('change', function () {
        $("#EmployeeCode").val('');
        $("#EmployeeId").val('');
        $("#ImgFile").val('');
        $("#DocumentRemarks").val('');
        $("#thumbnil").attr('scr','');
    })
});

