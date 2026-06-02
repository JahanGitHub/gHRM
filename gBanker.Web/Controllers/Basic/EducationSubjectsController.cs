
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using gHRM.Web.Helpers;
using gHRM.Service.StoreProcedure;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using AutoMapper;

namespace gHRM.Web.Controllers
{
    public class EducationSubjectsController : BaseController
    {
        #region variables

        private readonly IEmployeeEducationService employeeEducationService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IEducationConcentrationService educationConcentrationService;
        private readonly IEducationDegreeService educationDegreeService;

        public EducationSubjectsController(
              IEducationDegreeService educationDegreeService
            , IEducationConcentrationService educationConcentrationService
            , IEmployeeEducationService employeeEducationService
            , IEmployeeSPService employeeSPService

            )
        {
            this.educationDegreeService = educationDegreeService;
            this.educationConcentrationService = educationConcentrationService;
            this.employeeEducationService = employeeEducationService;
            this.employeeSPService = employeeSPService;

        }

        #endregion

        #region Events

        //public ActionResult EducationSubjectEntry()
        public ActionResult Index()
        {
            var model = new EmployeeViewModel();
            MapDropDownList(model);
            return View(model);
        }

        #endregion

        #region HttpRequests

        public JsonResult EducationSubjectList([DataSourceRequest] DataSourceRequest request)
        {
            var param = new { };
            var List = employeeSPService.GetDataWithoutParameter("basic.SP_Education_Subject_entry");
            var CostingList = List.Tables[0].AsEnumerable().Select((row, sl) => new EmployeeViewModel
            {
                rowSl = sl + 1,
                ConcentrationId = row.Field<int>("ConcentrationId"),
                ConcentrationCode = row.Field<string>("ConcentrationCode"),
                ConcentrationName = row.Field<string>("ConcentrationName"),
                DegreeLevel = row.Field<string>("DegreeLevel"),
                DegreeName = row.Field<string>("DegreeName"),
                IsActive = row.Field<bool>("IsActive")
            }).ToList();

            DataSourceResult result = CostingList.ToDataSourceResult(request);
            return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDegreesByDegreeLevel(string degreeLevelId)
        {
            int degreeLvlId = Convert.ToInt32(degreeLevelId);
            var degreeList = educationDegreeService.GetMany(w => w.DegreeLevelId == degreeLvlId && w.IsActive == true).ToList();

            var viewdegreeList = degreeList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DegreeCode.ToString(),
                Text = x.DegreeName.ToString()
            });

            var degree_items = new List<SelectListItem>();
            degree_items.AddRange(viewdegreeList);

            return Json(degree_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubjectsByDegree(string degreeCode)
        {
            var degreeList = educationConcentrationService.GetMany(w => w.DegreeCode == degreeCode && w.IsActive == true).ToList();

            var viewdegreeList = degreeList.OrderBy(x => x.ConcentrationId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ConcentrationCode.ToString(),
                Text = x.ConcentrationName.ToString()
            });

            var degree_items = new List<SelectListItem>();
            degree_items.AddRange(viewdegreeList);

            return Json(degree_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEducationSubject(EducationConcentration vmCarType)
        {
            int result = 0;
            string message = string.Empty;

            try
            {
                var isDuplicate = educationConcentrationService.GetMany(p => p.IsActive == true && p.ConcentrationCode.Trim() == vmCarType.ConcentrationCode.Trim() && p.DegreeCode.Trim() == vmCarType.DegreeCode.Trim()).ToList();
                if (isDuplicate.Any())
                {
                    message = "Duplicate Subject Code Found";
                }
                else
                {
                    var entity = new EducationConcentration();
                    entity.DegreeCode = vmCarType.DegreeCode;
                    entity.ConcentrationCode = vmCarType.ConcentrationCode;
                    entity.ConcentrationName = vmCarType.ConcentrationName;
                    entity.IsActive = true;
                    entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                    entity.CreateDate = DateTime.UtcNow;
                    entity.UpdateDate = DateTime.UtcNow;
                    educationConcentrationService.Create(entity);
                    message = "Saved Successfully";
                    result = 1;
                }
            }

            catch (Exception ex)
            {
                // result = ex.InnerException.Message.ToString();
                message = "Update Failed";
            }

            return Json(new { result, message }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult EditEducationSubject(int ConcentrationId)
        {
            var model = new EmployeeViewModel();
            var result = "1";

            try
            {
                var degree_level = new List<SelectListItem>();
                degree_level.Add(new SelectListItem() { Text = "Please Select", Value = "0" });

                var degree_items = new List<SelectListItem>();
                degree_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });

                var detail = educationConcentrationService.GetAll().Where(p => p.ConcentrationId == ConcentrationId).FirstOrDefault();

                if (detail != null)
                {
                    model.ConcentrationId = detail.ConcentrationId;
                    model.ConcentrationCode = detail.ConcentrationCode;
                    model.ConcentrationName = detail.ConcentrationName;
                    model.DegreeCode = detail.DegreeCode.ToString();

                    var degreeLevelList = educationDegreeService.GetMany(w => w.CompanyId == 1).DistinctBy(w => new { w.DegreeLevelId, w.DegreeLevel }).ToList();

                    var viewDegreeLevelList = degreeLevelList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.DegreeLevelId.ToString(),
                        Text = x.DegreeLevel.ToString()
                    });

                    degree_level.AddRange(viewDegreeLevelList);

                    var DegreeLevel = educationDegreeService.GetMany(level => level.DegreeCode == detail.DegreeCode).FirstOrDefault();

                    int DegreeLevelId = DegreeLevel.DegreeLevelId;
                    model.DegreeLevel = DegreeLevelId.ToString();

                    var degreeList = educationDegreeService.GetMany(w => w.DegreeLevelId == DegreeLevelId && w.IsActive == true).ToList();
                    var viewDegreeList = degreeList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.DegreeCode.ToString(),
                        Text = x.DegreeName.ToString()
                    });

                    degree_items.AddRange(viewDegreeList);
                }

                model.DegreeLevelList = degree_level;
                model.DegreeList = degree_items;
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return Json(new { result, model }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateEducationSubject(EducationConcentration vmCarType)
        {

            int result = 0;
            string message = string.Empty;
            try
            {
                var isDuplicate = educationConcentrationService.GetMany(p => p.IsActive == true && p.DegreeCode.Trim() == vmCarType.DegreeCode.Trim() && p.ConcentrationCode.Trim() == vmCarType.ConcentrationCode.Trim() && p.ConcentrationId != vmCarType.ConcentrationId).ToList();

                if (isDuplicate.Any())
                {
                    message = "Duplicate Subject Code Found";
                    return Json(new { result, message }, JsonRequestBehavior.AllowGet);
                }

                var previousInfo = educationConcentrationService.GetById(vmCarType.ConcentrationId);
                var isUsed = employeeEducationService.GetMany(p => p.IsActive == true && p.DegreeTitle == previousInfo.DegreeCode && p.Concentration.Trim() == previousInfo.ConcentrationCode.Trim()).ToList();

                if (isUsed.Any())
                {
                    message = "This Concentration Code Already Used Cannot Be Changed";
                    return Json(new { result, message }, JsonRequestBehavior.AllowGet);
                }

                var isUsedDegreeCode = employeeEducationService.GetMany(p => p.IsActive == true && p.DegreeTitle == previousInfo.DegreeCode).ToList();

                if (isUsedDegreeCode.Any())
                {
                    message = "This Concentration Degree Code Already Used Cannot Be Deleted";
                    return Json(new { result, message }, JsonRequestBehavior.AllowGet);
                }

                if (vmCarType != null)
                {
                    var entity = educationConcentrationService.GetAll().Where(p => p.ConcentrationId == vmCarType.ConcentrationId).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.ConcentrationId = vmCarType.ConcentrationId;
                        entity.DegreeCode = vmCarType.DegreeCode;
                        entity.ConcentrationCode = vmCarType.ConcentrationCode;
                        entity.ConcentrationName = vmCarType.ConcentrationName;

                        entity.IsActive = true;
                        entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                        entity.CreateDate = DateTime.UtcNow;
                        entity.UpdateDate = DateTime.UtcNow;
                        educationConcentrationService.Update(entity);
                        message = "Updated Successfully";
                        result = 1;
                    }
                    else
                    {
                        message = "No records found";
                    }
                }
                else
                {
                    message = "Please insert all required fields";
                }
            }

            catch (Exception ex)
            {
                message = "Update Failed";
            }
            return Json(new { result, message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteEducationSubject(int Id)
        {
            var result = 0;
            var message = "";
            try
            {
                var previousInfo = educationConcentrationService.GetById(Id);
                var isUsed = employeeEducationService.GetMany(p => p.IsActive == true && p.DegreeTitle == previousInfo.DegreeCode && p.Concentration.Trim() == previousInfo.ConcentrationCode.Trim()).ToList();

                if (isUsed.Any())
                {
                    message = "This Concentration Code Already Used Cannot Be Deleted";
                    return Json(new { result, message }, JsonRequestBehavior.AllowGet);
                }

                var isUsedDegreeCode = employeeEducationService.GetMany(p => p.IsActive == true && p.DegreeTitle == previousInfo.DegreeCode).ToList();

                if (isUsedDegreeCode.Any())
                {
                    message = "This Concentration Degree Code Already Used Cannot Be Deleted";
                    return Json(new { result, message }, JsonRequestBehavior.AllowGet);
                }
                var model = educationConcentrationService.GetById(Id);
                model.IsActive = false;
                model.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                model.CreateDate = DateTime.UtcNow;
                model.UpdateDate = DateTime.UtcNow;
                educationConcentrationService.Update(model);
                result = 1;
                message = "Deleted Successfully";
            }
            catch (Exception)
            {
                result = 0;
                message = "Delete Failed";

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Methods

        private void MapDropDownList(EmployeeViewModel model)
        {
            var degreeLevelList = educationDegreeService.GetMany(w => w.CompanyId == 1).DistinctBy(w => new { w.DegreeLevelId, w.DegreeLevel }).ToList();

            var viewdegreeList = degreeLevelList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DegreeLevelId.ToString(),
                Text = x.DegreeLevel.ToString()
            });

            var degree_items = new List<SelectListItem>();
            degree_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            degree_items.AddRange(viewdegreeList);
            model.DegreeLevelList = degree_items;

            //concentration

            var DegreeList = new List<SelectListItem>();
            DegreeList.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            model.DegreeList = DegreeList;

            var concentration_items = new List<SelectListItem>();
            concentration_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            model.ConcentrationList = concentration_items;
        }

        #endregion

        #region EducationDegree

        public ActionResult AddEducationDegree(int? DegreeId)
        {
            var model = new EducationDegreeViewModel();
            if ((DegreeId ?? 0) > 0)
            {
                int Id = (int)Convert.ToInt64(DegreeId);
                var _model = educationDegreeService.GetById(Id);
                model = Mapper.Map<EducationDegree, EducationDegreeViewModel>(_model);
            }

            //else
            //{

            var degreeLevelList = educationDegreeService.GetMany(w => w.CompanyId == 1).DistinctBy(w => new { w.DegreeLevelId, w.DegreeLevel }).ToList();

            var viewdegreeList = degreeLevelList.OrderBy(x => x.DegreeLevelId).Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DegreeLevelId.ToString(),
                Text = x.DegreeLevel.ToString(),
                Selected = (model.DegreeLevelId > 0 && x.DegreeLevelId == model.DegreeLevelId ? true : false)
            });

            var degree_items = new List<SelectListItem>();
            degree_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            degree_items.AddRange(viewdegreeList);
            model.DegreeLevelList = degree_items;
            //}

            return View(model);
        }

        public async Task<JsonResult> SaveEducationDegree(int? DegreeId, int? DegreeLevelId, string DegreeLevel, string DegreeCode, string DegreeName)
        {
            int result = 0;
            string message = string.Empty;
            var entity = new EducationDegree();

            try
            {
                EducationDegree edObj = new EducationDegree();
                if (string.IsNullOrEmpty(DegreeCode))
                {
                    message = "Please Insert Degree Code";
                }
                else if (string.IsNullOrEmpty(DegreeName))
                {
                    message = "Please Insert Degree Name";
                }
                else if (string.IsNullOrEmpty(DegreeLevel))
                {
                    message = "Please Insert Degree Level";
                }
                else if ((DegreeId ?? 0) > 0 && (DegreeLevelId ?? 0) == 0)
                {

                    message = "Don't have Degree Level Id";
                }
                // Level Required --in update time
                else if ((DegreeLevelId ?? 0) > 0)
                {
                    edObj = educationDegreeService.GetMany(p => p.DegreeLevelId == DegreeLevelId.Value).FirstOrDefault();
                    if (edObj.DegreeLevel != DegreeLevel)
                    {
                        message = "Wrong Degree Level Name";
                    }
                }
                // Update time

                if (message == "") // add update
                {   
                       
                    //// message
                    //else
                    //{
                        if ((DegreeId ?? 0) > 0)
                        {
                            var isDuplicate = educationDegreeService.GetMany(p =>p.DegreeId != DegreeId && (p.DegreeCode == DegreeCode || p.DegreeName == DegreeName)).ToList();
                            if (isDuplicate.Any())
                            {
                                message = "Duplicate Degree Name or Code found";
                            }
                            else
                            {
                                int Id = (int)Convert.ToInt64(DegreeId);
                                var model = educationDegreeService.GetById(Id);
                                model.DegreeLevelId = (int)DegreeLevelId;
                                model.DegreeLevel = DegreeLevel;
                                model.DegreeCode = DegreeCode;
                                model.DegreeName = DegreeName;
                                model.IsActive = true;
                                model.CompanyId = (int)SessionHelper.CompanyID;

                                educationDegreeService.Update(model);
                                message = "Updated Successfully";
                            }

                        }
                        else //add
                        {
                            var isDuplicate = educationDegreeService.GetMany(p => (p.DegreeCode == DegreeCode || p.DegreeName == DegreeName)).ToList();
                            if (isDuplicate.Any())
                            {
                                message = "Duplicate Degree Name or Code found";
                            }
                            else
                            {
                                var isDuplicateDegreeLevel = educationDegreeService.GetMany(p => p.IsActive == true && p.DegreeLevel.Trim() == DegreeLevel.Trim()).ToList();

                                if (isDuplicateDegreeLevel.Any())
                                {
                                    entity.DegreeLevelId = isDuplicateDegreeLevel.FirstOrDefault().DegreeLevelId;
                                    entity.DegreeLevel = isDuplicateDegreeLevel.FirstOrDefault().DegreeLevel;

                                    //SessionHelper.CompanyID
                                    //SessionHelper.CompanyInfo
                                }
                                else
                                {
                                    var DegreeLevelList = educationDegreeService.GetMany(p => p.IsActive == true).OrderByDescending(x => x.DegreeLevelId).ToList();
                                    var MaxDegreeLevelId = DegreeLevelList.FirstOrDefault().DegreeLevelId;
                                    entity.DegreeLevelId = MaxDegreeLevelId + 1;
                                    entity.DegreeLevel = DegreeLevel;
                                }

                                entity.DegreeCode = DegreeCode;
                                entity.DegreeName = DegreeName;
                                entity.IsActive = true;
                                entity.CompanyId = (int)SessionHelper.CompanyID;
                                entity.CreateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                                entity.CreateDate = DateTime.UtcNow;
                                entity.UpdateDate = DateTime.UtcNow;
                                educationDegreeService.Create(entity);
                                message = "Saved Successfully";
                                result = 1;

                            }
                        }
                    }               
            }


            catch (Exception ex)
            {
                // result = ex.InnerException.Message.ToString();
                message = "Update Failed";
            }

            return Json(new { result, message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEducationDegree([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                List<EducationDegreeViewModel> List_ViewModel = new List<EducationDegreeViewModel>();
                var param = new { };
                var List = employeeSPService.GetDataWithoutParameter("basic.SP_Education_Degree");
                var CostingList = List.Tables[0].AsEnumerable().Select((row, sl) => new EducationDegreeViewModel
                {
                    rowSl = sl + 1,
                    DegreeId = row.Field<int>("DegreeId"),
                    DegreeLevel = row.Field<string>("DegreeLevel"),
                    DegreeCode = row.Field<string>("DegreeCode"),
                    DegreeName = row.Field<string>("DegreeName"),

                    IsActive = row.Field<bool>("IsActive")
                }).ToList();

                DataSourceResult result = CostingList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetDegreesByDegreeLevelId(int? degreeLevelId)
        {
            var degreeLevel = "";
            var degreeList = educationDegreeService.GetMany(w => w.DegreeLevelId == degreeLevelId && w.IsActive == true).ToList();

            if (degreeList.Any())
            {
                degreeLevel = degreeList.FirstOrDefault().DegreeLevel;
            }

            return Json(degreeLevel, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
