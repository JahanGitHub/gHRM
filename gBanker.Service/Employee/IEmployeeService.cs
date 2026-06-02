using gHRM.Core.Common;
using gHRM.Core.Filters.Employee;
using gHRM.Core.Utilities.Constants;
using gHRM.Data.CodeFirstMigration;
using gHRM.Data.CodeFirstMigration.InfrastructureBase;
using gHRM.Data.DBDetailModels.Employee;
using gHRM.Data.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gHRM.Service
{
    public interface IEmployeeService : IServiceBase<Employee>
    {
        Task<Employee> GetEmployeeInfoByUsername(string employeeCode);
        IEnumerable<ValidationResult> IsValidEmployee(string empCode);
        Task<IEnumerable<EmployeeDetailApiModel>> GetEmployeeListByFilter(EmployeeSearchFilter filter);
        Task<Employee> GetEmployeeInfo(int employeeId);
        Employee GetByEmpId(Int64 EmployeeId);
        Employee GetEmployeeById(long employeeId, bool withResignEmployee = true);
        Employee GetByCode(string empCode, bool withResignEmployee = true);
        IEnumerable<Employee> GetByOfficeId(int OfficeId);
        Employee ImportExistingEmployee(Employee newEmployee);
        bool ValidEmployeeCode(string empCode);
        Employee GetEmployeeByEmployeeCode(string empCode);
        bool BulkEmployeesAdd(List<Employee> employees);
        Task<IEnumerable<FixedAssetEmployeeModel>> GetFixedAssetEmployeeByOffice(int officeId);
        DateTime? GetFirstJoiningDateByCode(string Code);
        decimal GetEmployeeBasicSalary(long EmployeeId);
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        Dictionary<string, object> GetEmployeeShortInfoByCode(string EmployeeCode);
        bool IsActive(long EmployeeId);
    }

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository repository;
        private readonly IEmployeeFamilyInfoRepository employeeFamilyInfoRepository;
        private readonly IOfficeRepository offcRepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IEmployeeDesignationRepository employeeDesignationRepository;
        private readonly IEmployeeAddressRepository employeeAddressRepository;
        private readonly IEmployeeOfficeDesignationRepository employeeOfficeDesignationRepository;
        

        public EmployeeService(IEmployeeRepository repository,
            IEmployeeFamilyInfoRepository employeeFamilyInfoRepository,
            IOfficeRepository OffcRepository, 
            IUnitOfWorkCodeFirst unitOfWork,
            IEmployeeDesignationRepository employeeDesignationRepository,
            IEmployeeAddressRepository employeeAddressRepository,
            IEmployeeOfficeDesignationRepository employeeOfficeDesignationRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.offcRepository = OffcRepository;
            this.employeeFamilyInfoRepository = employeeFamilyInfoRepository;
            this.employeeDesignationRepository = employeeDesignationRepository;
            this.employeeAddressRepository = employeeAddressRepository;
            this.employeeOfficeDesignationRepository = employeeOfficeDesignationRepository;
        }

        #region Public Methods

        public async Task<IEnumerable<FixedAssetEmployeeModel>> GetFixedAssetEmployeeByOffice(int officeId)
        {
            var employeeList = new List<FixedAssetEmployeeModel>();

            try
            {
                return await repository.GetFixedAssetEmployeeByOffice(officeId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Employee> GetEmployeeInfoByUsername(string username)
        {
            var employeeInfo = new Employee();

            try
            {
                employeeInfo = await repository.GetEmployeeInfoByUsername(username); 
                if (employeeInfo==null)
                    return employeeInfo;

                var office = await offcRepository.GetOfficeByOfficeId((int)employeeInfo.OfficeId);
                if (office != null) employeeInfo.Office = office;                

                return employeeInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Employee> GetEmployeeInfo(int employeeId)
        {
            var single = new Employee();

            try
            {
                single = await repository.GetEmployeeInfo(employeeId);

                return single;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<IEnumerable<EmployeeDetailApiModel>> GetEmployeeListByFilter(EmployeeSearchFilter filter)
        {
            var filteredList = new List<EmployeeDetailApiModel>();

            try
            {
                return await repository.GetEmployeeListByFilter(filter);
            }
            catch (Exception ex)
            {
                return new List<EmployeeDetailApiModel>();
            }
        }

        public DateTime? GetFirstJoiningDateByCode(string Code)
        {
            return repository.GetFirstJoiningDateByCode(Code);
        }

        public IEnumerable<Employee> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.EmployeeId);
            return entities;
        }

        public Employee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Employee GetByEmpId(Int64 EmployeeId)
        {
            var entity = repository.Get(e => e.EmployeeId == EmployeeId && e.IsActive == true);
            return entity;
        }

        public IEnumerable<Employee> GetByOfficeId(int OfficeId)
        {
            //string[] EmpStatus = { "A", "DP", "LI", "PR", "TR", "EPR", "CNT" };
            //var entity = repository.GetMany(e => e.OfficeId == OfficeId && e.IsActive== true && e.EmployeeStatus=="A");
            var entity = repository.GetMany(e => e.OfficeId == OfficeId && e.IsActive == true);
            return entity;
        }

        public Employee GetEmployeeById(long employeeId, bool withResignEmployee = true)
        {
            if (!withResignEmployee)
                return repository.Get(e => e.EmployeeId == employeeId
                                    && e.IsActive == true
                                    && e.EmployeeStatusId != EmployeeStatusConstants.Resign);

            var entity = repository.Get(e => e.EmployeeId == employeeId && e.IsActive == true);
            return entity;
        }

        public Employee GetByCode(string empCode, bool withResignEmployee = true)
        {
            if (!withResignEmployee)
                return repository.Get(e => e.EmployeeCode == empCode
                                    && e.IsActive == true
                                    && e.EmployeeStatusId != EmployeeStatusConstants.Resign);

            var entity = repository.Get(e => e.EmployeeCode == empCode && e.IsActive == true);
            return entity;
        }

        public Employee Create(Employee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Employee objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public Employee Get(Expression<Func<Employee, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }

        public IEnumerable<Employee> GetMany(Expression<Func<Employee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        #region Asyc
        public virtual async Task<IEnumerable<Employee>> GetAllAsync()
        {

            return await repository.GetAllAsync();
        }


        public virtual async Task<IEnumerable<Employee>> GetManyAsync(Expression<Func<Employee, bool>> where)
        {
            return await repository.GetManyAsync(where);
        }



        public virtual async Task<Employee> GetAsync(Expression<Func<Employee, bool>> where)
        {
            return await repository.GetAsync(where);
        }
        #endregion

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
           
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }

        IEnumerable<ValidationResult> IEmployeeService.IsValidEmployee(string empCode)
        {
            var entity = repository.Get(p => p.EmployeeCode == empCode);
            if (entity != null)
            {
                yield return new ValidationResult("EmployeeCode", "Duplicate Employee.");
            }
        }

        public Employee GetEmployeeByEmployeeCode(string empCode)
        {
            var single = new Employee();

            using (var db = new gHRMDBContext())
            {
                single = db.Employees.FirstOrDefault(p => p.EmployeeCode == empCode && p.IsActive);
            }

            return single;
        }

        public bool ValidEmployeeCode(string empCode)
        {
            var validEmployeeCode = true;

            using (var db = new gHRMDBContext())
            {
                validEmployeeCode = db.Employees.Any(p => p.EmployeeCode == empCode);
            }

            return validEmployeeCode;
        }

        public Employee ImportExistingEmployee(Employee newEmployee)
        {
            try
            {
                using (var db = new gHRMDBContext())
                {
                    var employeeFamilyInfoes = newEmployee.EmployeeFamilyInfoes;
                    var employeeAddressInfos = newEmployee.EmployeeAddresses;

                    //add new employee
                    newEmployee.EmployeeFamilyInfoes = null;
                    newEmployee.EmployeeAddresses = null;
                    db.Employees.Add(newEmployee);

                    //add new employee family info when any record find
                    if (employeeFamilyInfoes.Any())
                    {
                        foreach (var employeeFamilyInfo in employeeFamilyInfoes)
                        {
                            employeeFamilyInfo.EmployeeId = newEmployee.EmployeeId;
                            db.EmployeeFamilyInfoes.Add(employeeFamilyInfo);
                        }
                    }

                    //add new employee address info when any record find
                    if (employeeAddressInfos.Any())
                    {
                        foreach (var employeeAddressInfo in employeeAddressInfos)
                        {
                            employeeAddressInfo.EmployeeId = newEmployee.EmployeeId;
                            db.EmployeeAddresses.Add(employeeAddressInfo);
                        }
                    }

                    db.SaveChanges();
                }

                return newEmployee;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// add bulk employee
        /// </summary>
        /// <param name="employees"></param>
        /// <returns></returns>
        public bool BulkEmployeesAdd(List<Employee> employees)
        {
            var isAddedSuccess = false;

            if (employees == null || !employees.Any())
                return isAddedSuccess;

            var dt = new DataTable();

            //Add Columns
            dt.Columns.Add("SerialId", typeof(int));
            dt.Columns.Add("CompanyId", typeof(int));
            dt.Columns.Add("EmployeeCode", typeof(string));
            dt.Columns.Add("EmployeeName", typeof(string));
            dt.Columns.Add("EmployeeNameBng", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("EmployeeGender", typeof(string));
            dt.Columns.Add("BloodGroup", typeof(string));
            dt.Columns.Add("Religion", typeof(string));
            dt.Columns.Add("NationalId", typeof(string));
            dt.Columns.Add("ContactNo1", typeof(string));
            dt.Columns.Add("FirstJoiningDate", typeof(DateTime));
            dt.Columns.Add("ConfirmationDate", typeof(DateTime));
            dt.Columns.Add("DateOfBirth", typeof(DateTime));
            dt.Columns.Add("EmployeeStatusId", typeof(int));
            dt.Columns.Add("PayrollDesignation", typeof(string));
            dt.Columns.Add("IsActive", typeof(bool));
            dt.Columns.Add("CreateUser", typeof(long));
            dt.Columns.Add("CreateDate", typeof(DateTime));

            //father's information            
            dt.Columns.Add("FatherName", typeof(string));
            dt.Columns.Add("FatherRelation", typeof(string));
            dt.Columns.Add("FatherGender", typeof(string));
            dt.Columns.Add("FatherFamilyInfoType", typeof(int));

            //mother's information
            dt.Columns.Add("MotherName", typeof(string));
            dt.Columns.Add("MotherRelation", typeof(string));
            dt.Columns.Add("MotherGender", typeof(string));
            dt.Columns.Add("MotherFamilyInfoType", typeof(int));

            //present address
            dt.Columns.Add("PresentAddressType", typeof(string));
            dt.Columns.Add("PresentCountryId", typeof(int));
            dt.Columns.Add("PresentStateOrProvinceId", typeof(int));
            dt.Columns.Add("PresentDistrictId", typeof(int));
            dt.Columns.Add("PresentThanaId", typeof(int));
            dt.Columns.Add("PresentUnionId", typeof(int));
            dt.Columns.Add("PresentStreetOrHouse", typeof(string));
            dt.Columns.Add("PresentAddressDetail", typeof(string));

            //permanent address            
            dt.Columns.Add("PermanentAddressType", typeof(string));
            dt.Columns.Add("PermanentCountryId", typeof(int));
            dt.Columns.Add("PermanentStateOrProvinceId", typeof(int));
            dt.Columns.Add("PermanentDistrictId", typeof(int));
            dt.Columns.Add("PermanentThanaId", typeof(int));
            dt.Columns.Add("PermanentUnionId", typeof(int));
            dt.Columns.Add("PermanentStreetOrHouse", typeof(string));
            dt.Columns.Add("PermanentAddressDetail", typeof(string));

            //Add rows
            int count = 0;
            int serialId = 1;
            foreach (var model in employees)
            {
                try
                {
                    //father family info
                    var fatherFamilyInfo = model.EmployeeFamilyInfoes.FirstOrDefault(f => f.FamilyInfoType == FamilyInfoTypeConstants.FatherInfo);
                    fatherFamilyInfo = fatherFamilyInfo != null ? fatherFamilyInfo : new EmployeeFamilyInfo();

                    //father mother info
                    var motherFamilyInfo = model.EmployeeFamilyInfoes.FirstOrDefault(f => f.FamilyInfoType == FamilyInfoTypeConstants.MotherInfo);
                    motherFamilyInfo = motherFamilyInfo != null ? motherFamilyInfo : new EmployeeFamilyInfo();

                    //present info
                    var presentInfo = model.EmployeeAddresses.FirstOrDefault(f => f.AddressType == AddressTypeConstants.PresentAddress);
                    presentInfo = presentInfo != null ? presentInfo : new EmployeeAddress();

                    //permanent info
                    var permanentInfo = model.EmployeeAddresses.FirstOrDefault(f => f.AddressType == AddressTypeConstants.PermanentAddress);
                    permanentInfo = permanentInfo != null ? permanentInfo : new EmployeeAddress();


                    dt.Rows.Add(
                        serialId,
                        model.CompanyId,
                        model.EmployeeCode,
                        model.EmployeeName,
                        model.EmployeeNameBng,
                        model.Email,
                        model.Gender,
                        model.BloodGroup,
                        model.Religion,
                        model.NationalId,
                        model.ContactNo1,
                        model.FirstJoiningDate,
                        model.ConfirmationDate,
                        model.DateOfBirth,
                        model.EmployeeStatusId,
                        model.PayrollDesignation,
                        model.IsActive,
                        model.CreateUser,
                        model.CreateDate,

                        //father's family info
                        fatherFamilyInfo.Name,
                        fatherFamilyInfo.Relation,
                        fatherFamilyInfo.Gender,
                        fatherFamilyInfo.FamilyInfoType,

                        //father's family info
                        motherFamilyInfo.Name,
                        motherFamilyInfo.Relation,
                        motherFamilyInfo.Gender,
                        motherFamilyInfo.FamilyInfoType,

                        //present address info
                        presentInfo.AddressType,
                        presentInfo.CountryId,
                        presentInfo.StateOrProvinceId,
                        presentInfo.DistrictId,
                        presentInfo.ThanaId,
                        presentInfo.UnionId,
                        presentInfo.StreetOrHouse,
                        presentInfo.AddressDetail,

                       //present address info
                       permanentInfo.AddressType,
                       permanentInfo.CountryId,
                       permanentInfo.StateOrProvinceId,
                       permanentInfo.DistrictId,
                       permanentInfo.ThanaId,
                       permanentInfo.UnionId,
                       permanentInfo.StreetOrHouse,
                       permanentInfo.AddressDetail

                        );

                    count++;
                    serialId++;
                }
                catch (Exception ex)
                {
                    // if error don't continue, fall back
                    var exception = new Exception("Adding data rows: " + ex.Message);
                    return false;
                }

                if (count >= 2000)
                {
                    count = 0;

                    // if error don't continue, fall back
                    isAddedSuccess = AddBulkOfEmployees(dt);

                    if (isAddedSuccess == false)
                        return isAddedSuccess;

                    dt.Rows.Clear();
                }
            }

            if (count > 0)
            {
                count = 0;

                // if error don't continue, fall back
                isAddedSuccess = AddBulkOfEmployees(dt);

                if (isAddedSuccess == false)
                    return isAddedSuccess;

                dt.Rows.Clear();
            }

            return true;
        }

        public decimal GetEmployeeBasicSalary(long EmployeeId)
        {
            return repository.GetEmployeeBasicSalary(EmployeeId);
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new BasicDataAccess.gHRMDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public Dictionary<string, object> GetEmployeeShortInfoByCode(string EmployeeCode)
        {
            return repository.GetEmployeeShortInfoByCode(EmployeeCode);
        }

        public bool IsActive(long EmployeeId)
        {
            return repository.IsActive(EmployeeId);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Add a bulk of employees to database
        /// </summary>
        /// <param name="dt"></param>
        private bool AddBulkOfEmployees(DataTable dt)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["gHRMDbContext"].ConnectionString;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand("[dbo].[Employee_InsertBulkForImportEmployee]", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    var dtparam = cmd.Parameters.AddWithValue("@TempBulkEmployeesImport", dt);
                    dtparam.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                string err2 = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    err = eve.Entry.Entity.GetType().Name + eve.Entry.State;
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err2 = "Property Name: " + ve.PropertyName + ", Message:" + ve.ErrorMessage;
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //    ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;
            }
            catch (DbUpdateException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var errorInProperty = entry;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
