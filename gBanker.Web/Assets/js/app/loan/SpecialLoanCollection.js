
function clearTH() {
    $("#th_dis_dt,#th_loan_amt,#th_loan_paid_amt,#th_loan_due_amt,#th_interest_amt,#th_interest_paid_amt,#th_interest_due_amt, #th_total_due_amt,#th_upto_interest_charge,#th_total_due_amt_after_charge").html("");
}

$(document).ready(function () {
    //$("#TransactionDate,#InterestUptoDate").datepicker({
        $("#InterestUptoDate").datepicker({
        dateFormat: "dd-M-yy",
        showAnim: "scale",
        changeMonth: true,
        yearRange: "",
        changeYear: true
    });
    $("#EmployeeCode").blur(function () {
        clearTH();
        var obj = GenerateAjaxRequist('/Employee/GetEmployeeByEmployeeCode', { employeeCode: $(this).val() }, "GET");
        $("#LoanID").html('')
        if (obj.EmployeeId) {
            $("#EmployeeCode").val(obj.EmployeeCode);
            $("#thEmployeeName").text(obj.EmployeeName);

            var ddlObj = GenerateAjaxRequist('/LoanGTT/GetLoanNoByEmployeeForDropdown', { employeeId: obj.EmployeeId, isClose: false }, "GET");
            var htm = "<option value>Please Select</option>";
            $.each(ddlObj, function (key, val) {
                htm += `<option value="${val.Value}">${val.Text}</option>`
            });
            $("#LoanID").html(htm);
        }
    });
    $("#InterestUptoDate").change(function () { $("#TransactionDate").val($(this).val()) })
    $("#btnView").click(function (e) {
        e.preventDefault();
        clearTH();
        if (!$("#LoanID").val()) $.alert.open("Warning", "Loan No. is required.");
        else if (!$("#InterestUptoDate").val()) $.alert.open("Warning", "Interest Upto date is required.");
        else if ($("#LoanID").val()) {
            var json_Obj = GenerateAjaxRequist('/LoanGTT/GetLoanDetailsById', { loanid: $("#LoanID").val(), uptodate: $("#InterestUptoDate").val() }, "GET");
            if (json_Obj.Message) $.alert.open("Warning", json_Obj.Message);
            else {
                var re = /-?\d+/;
                var m = re.exec(json_Obj.dis.DisburseDate);
                var d = new Date(parseInt(m[0]));
                const month = d.toLocaleString('default', { month: 'short' });
                //var dt = `${(d.getDay() < 10 ? ("0" + d.getDay()) : d.getDay())} - ${month}-${d.getFullYear()}`;
                var dt = `${(d.getDate() < 10 ? ("0" + d.getDate()) : d.getDate())} - ${month}-${d.getFullYear()}`;  // Tazdik
                $("#th_dis_dt").html(dt);
                $("#th_loan_amt").html(json_Obj.dis.DisburseAmount);
                $("#th_loan_paid_amt").html(json_Obj.dis.LoanPaid);
                var principal_due = json_Obj.dis.DisburseAmount - json_Obj.dis.LoanPaid;
                $("#th_loan_due_amt").html(principal_due);

                $("#th_interest_amt").html(json_Obj.dis.InterestCharge);
                $("#th_interest_paid_amt").html(json_Obj.dis.InterestPaid);
                
                var interest_due =json_Obj.interest_due //json_Obj.dis.InterestCharge - parseFloat(json_Obj.dis.InterestPaid);
                $("#th_interest_due_amt").html(interest_due);

                $("#th_total_due_amt").html(principal_due + interest_due);
                $("#th_upto_interest_charge").html(json_Obj.charge);

                $("#th_total_due_amt_after_charge").html(principal_due + interest_due + json_Obj.charge)
            }
        }
    });
    $("#btnSave").click(function (e) {
        e.preventDefault();
        var trn_amt = parseFloat($("#TransactionAmount").val());
        var total_due_amt = parseFloat($("#th_total_due_amt_after_charge").html());
        var message = isNaN(trn_amt) | trn_amt <= 0 ? "Transaction Amount is Required" : isNaN(total_due_amt) | total_due_amt <= 0 ? "Due Amount not found" : trn_amt > total_due_amt ? "Transaction amount is never greater than total due amount" : !$("#Narration").val() ? "Naration is required." : $("#TransactionDate").val() != $("#InterestUptoDate").val() ? "Upto date and Transaction date same date is required" : "";
        if (message) $.alert.open("Warning", message);
        else {
            var dataObject = {
                LoanId: $("#LoanID").val(),
                TransactionDate: $("#TransactionDate").val(),
                UptoDate: $("#InterestUptoDate").val(),
                TransactionAmount: $("#TransactionAmount").val(),
                Narration: $("#Narration").val(),
                TotalDue: $("#th_total_due_amt").text(),
                InterestCharge: $("#th_upto_interest_charge").text(),
            };
            var json_Obj = GenerateAjaxRequist('/LoanGTT/PostSpecialCollection', JSON.stringify({ model: dataObject }), "POST");
            if (json_Obj) {
                $.alert.open((json_Obj.status == 0 ? "Warning" : "Success"), json_Obj.msg);
                if (json_Obj.status == 1) {
                    clearTH();
                    $("#TransactionAmount,#Narration,#InterestUptoDate,#TransactionDate").val('')
                }
            }
            
        }
        //$("#Narration").val()
    });
});