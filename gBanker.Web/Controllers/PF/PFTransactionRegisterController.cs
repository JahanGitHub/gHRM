using gHRM.Data.CodeFirstMigration.PF;
using gHRM.Service.PF;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.PF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers
{
    public class PFTransactionRegisterController :BaseController
    {
        private readonly ITransactionRegisterService transactionRegisterService;
        public PFTransactionRegisterController(ITransactionRegisterService transactionRegisterService)
        {
            this.transactionRegisterService = transactionRegisterService;
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveVoucher(List<TransactionRegisterViewModel> model)
        {
            string message = string.Empty;
            try
            {

                List<TransactionRegister> objTransactionRegister = new List<TransactionRegister>();

                objTransactionRegister = model.AsEnumerable()
              .Select(row => new TransactionRegister
              {
                 SerialNo = row.SerialNo,
                 AccountCode = row.AccountCode,
                 VoucherNo = row.VoucherNo,
                 Amount = row.Amount,
                 TransactionType = row.TransactionType,
                 TransactionDate = DateTime.Now,
                 Particulars = row.Particulars,
                 CreateUser = Convert.ToInt64(LoggedInEmployeeId.ToString()),
                 CreateDate = DateTime.Now,
                 UpdateUser = Convert.ToInt64(LoggedInEmployeeId.ToString()),
                 UpdateDate = DateTime.Now,
                 IsDeleted = row.IsDeleted,
                 DeletedUser = null,
                 DeleteDate = null

              }).ToList();

                //foreach(var transReg in model)
                //{
                   
                //}

                transactionRegisterService.SaveVoucher(objTransactionRegister);
                message = "Added Successfully";
                //message = model.ID == 0 ? "Added Successfully" : "Updated Successfully";
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
            return GetSuccessMessageResult(message);
        }
 
      
    }
}
