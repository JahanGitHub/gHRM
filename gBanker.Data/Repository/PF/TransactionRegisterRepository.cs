using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.CodeFirstMigration.PF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.Repository.PF
{
    public interface ITransactionRegisterRepository : IRepository<TransactionRegister>
    {
        List<TransactionRegister> SaveVoucher(List<TransactionRegister> objTransactionRegisters);
    }
   
    public class TransactionRegisterRepository : RepositoryBaseCodeFirst<TransactionRegister>, ITransactionRegisterRepository
    {
        public TransactionRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<TransactionRegister> SaveVoucher(List<TransactionRegister> objTransactionRegisters)
        {

            try
            {
                   Int64 voucherNo = DataContext.OrganizationSetup.Where(x => x.IsDeleted == false && x.IsActive == true).Max(x => x.VoucherNo);

                    foreach (var transRegister in objTransactionRegisters.ToList())
                    {
                        transRegister.VoucherNo = voucherNo;

                        var cTransRegister = DataContext.TransactionRegister.Where(x => x.SerialNo == transRegister.SerialNo).SingleOrDefault();
                        if (cTransRegister == null)
                        {
                            DataContext.Entry(transRegister).State = EntityState.Added;
                            DataContext.TransactionRegister.Add(transRegister);
                        }
                        else
                        {
                            DataContext.Entry(cTransRegister).State = EntityState.Detached;
                            DataContext.Entry(transRegister).State = EntityState.Modified;
                            DataContext.TransactionRegister.Attach(transRegister);
                        }
                    }

                DataContext.SaveChanges();
            }

            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
            }
            return objTransactionRegisters;

        }
    }
}

