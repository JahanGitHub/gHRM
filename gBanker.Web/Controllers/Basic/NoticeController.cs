using AutoMapper;
using gHRM.Data.CodeFirstMigration.Basic;
using gHRM.Service.Basic;
using gHRM.Web.Helpers;
using gHRM.Web.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gHRM.Core.Utilities.Constants;
using gHRM.Service;
using gHRM.Web.Filters;

namespace gHRM.Web.Controllers
{
    public class NoticeController : Controller
    {

        #region Variables

        private readonly INoticeService noticeService;
        private readonly IAspNetRoleService roleService;
        private readonly IOfficeTypeService officeTypeService;
        public NoticeController(INoticeService noticeService, IAspNetRoleService roleService, IOfficeTypeService officeTypeService)
        {
            this.noticeService = noticeService;
            this.roleService = roleService;
            this.officeTypeService = officeTypeService;
        }

        [SessionExpireFilter]
        [DisableCache]
        public ActionResult Notice()
        {
            MapDropdownListValues();
            return View();
        }

        #endregion

        #region Methods

        public JsonResult NoticeDelete(string NoticeId)
        {
            var entity = noticeService.GetById(Convert.ToInt32(NoticeId));
            string Result = "OK";
            if (ModelState.IsValid)
            {
                entity.IsActive = false;
                entity.UpdateUser = Convert.ToInt64(SessionHelper.LoggedInEmployeeID);
                entity.UpdateDate = DateTime.Now;
                noticeService.Update(entity);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNoticeListInfo(int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            try
            {
                var noticeList = noticeService.GetAll()
                    .Where(x => x.IsActive == true)
                    .OrderByDescending(x => x.NoticeId)
                    .ToList();

                var List_NoticeViewModel = noticeList.Select((x, index) => new NoticeViewModel
                {
                    rowSl = index + 1,
                    NoticeId = x.NoticeId,
                    Title = x.Title,
                    NoticeText = x.NoticeText,
                    PublishDateMsg = x.PublishDate.ToString("dd-MMM-yyyy"),
                    LiveFromMsg = x.LiveFrom.ToString("dd-MMM-yyyy"),
                    LiveToMsg = x.LiveTo.ToString("dd-MMM-yyyy")
                }).ToList();

                var currentPageRecords = List_NoticeViewModel
                    .Skip(jtStartIndex)
                    .Take(jtPageSize);

                return Json(new
                {
                    Result = "OK",
                    Records = currentPageRecords,
                    TotalRecordCount = List_NoticeViewModel.Count()
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Events

        //
        // GET: /Notice/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZOOfficeList"] = items;
            ViewData["ZAOOfficeList"] = items;
            ViewData["AOOfficeList"] = items;
            ViewData["BOOfficeList"] = items;
            return View();
        }

        //
        // GET: /Notice/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Notice/Create
        public ActionResult Create()
        {
            NoticeViewModel model = new NoticeViewModel();

            model.RoleList = GetRoleList();
            model.OfficeTypeList = GetOfficeTypeList();

            return View(model);
        }
        private IEnumerable<SelectListItem> GetRoleList()
        {
            var roleList = roleService.GetAll()
                .Where(x => x.IsActive == true)
                .Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList();

            return roleList;
        }
        private IEnumerable<SelectListItem> GetOfficeTypeList()
        {
            var officeTypeList = officeTypeService.GetAll()
                .Where(x => x.IsActive == true)
                .Select(r => new SelectListItem
                {
                    Value = r.OfficeTypeId.ToString(),
                    Text = r.OfficeTypeName
                }).ToList();

            return officeTypeList;
        }

        //
        // POST: /Notice/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NoticeViewModel model)
        {
            try
            {
                var entity = Mapper.Map<NoticeViewModel, Notice>(model);

                entity.IsActive = true;
                entity.Title = model.Title;
                entity.NoticeText = model.NoticeText;
                entity.CreateUser = SessionHelper.LoginUserEmployeeId;
                entity.CreateDate = DateTime.Now;

                entity.PublishDate = Convert.ToDateTime(model.PublishDateMsg);
                entity.LiveFrom = Convert.ToDateTime(model.LiveFromMsg);
                entity.LiveTo = Convert.ToDateTime(model.LiveToMsg);
                entity.RoleId = Convert.ToInt32(model.RoleId);
                entity.OfficeTypeId = Convert.ToInt32(model.OfficeTypeId);

                var notice = noticeService.Create(entity);

                model.NoticeId = notice.NoticeId;

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            var notice = noticeService.GetById(Convert.ToInt32(id));
            var entity = Mapper.Map<Notice, NoticeViewModel>(notice);

            NoticeViewModel model = new NoticeViewModel();

            //  model.EmbMode = "U";//entity.AuditFromMsg = String.Format("{0:dd-MMM-yyyy}", caseEntry.AuditFrom);
            model.Title = notice.Title;
            model.NoticeText = notice.NoticeText;
            model.PublishDateMsg = String.Format("{0:dd-MMM-yyyy}", notice.PublishDate);
            model.LiveFromMsg = String.Format("{0:dd-MMM-yyyy}", notice.LiveFrom);
            model.LiveToMsg = String.Format("{0:dd-MMM-yyyy}", notice.LiveTo);
            model.RoleId = notice.RoleId.ToString();
            model.OfficeTypeId = notice.OfficeTypeId.ToString();
            model.NoticeId = notice.NoticeId;

            model.RoleList = roleService.GetMany(x => x.IsActive == true)
                .Where(x => x.Name != UserRoleConstants.Super_Admin)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            model.OfficeTypeList = officeTypeService.GetMany(x => x.IsActive == true)
                .OrderBy(x => x.OfficeTypeName)
                .Select(x => new SelectListItem
                {
                    Text = x.OfficeTypeName,
                    Value = x.OfficeTypeId.ToString()
                }).ToList();


            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Edit(NoticeViewModel model)
        {
            try
            {
                var entity = noticeService.GetById(model.NoticeId);

                entity.Title = model.Title;
                entity.NoticeText = model.NoticeText;

                entity.PublishDate = Convert.ToDateTime(model.PublishDateMsg);
                entity.LiveFrom = Convert.ToDateTime(model.LiveFromMsg);
                entity.LiveTo = Convert.ToDateTime(model.LiveToMsg);
                model.RoleId = model.RoleId;
                model.OfficeTypeId = model.OfficeTypeId;

                entity.UpdateUser = SessionHelper.LoginUserEmployeeId;
                entity.UpdateDate = DateTime.Now;

                noticeService.Update(entity);

                return Json(new
                {
                    Result = "OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = "ERROR",
                    Message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Notice/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        private void MapDropdownListValues()
        {
            // Role List
            var roleList = roleService
                .GetMany(x => x.IsActive == true)
                .ToList();

            if (roleList.Any())
            {
                roleList = roleList
                    .Where(f => f.Name != UserRoleConstants.Super_Admin)
                    .OrderBy(f => f.Name)
                    .ToList();
            }

            var roleViewList = new List<SelectListItem>();

            roleViewList.Add(new SelectListItem()
            {
                Text = "Select Role",
                Value = ""
            });

            roleViewList.AddRange(
                roleList.Select(m => new SelectListItem()
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }).ToList()
            );

            ViewBag.RoleList = roleViewList;


            // Office Type List
            var officeTypeList = officeTypeService
                .GetMany(x => x.IsActive == true)
                .OrderBy(x => x.OfficeTypeName)
                .ToList();

            var officeTypeViewList = new List<SelectListItem>();

            officeTypeViewList.Add(new SelectListItem()
            {
                Text = "Select Office Type",
                Value = ""
            });

            officeTypeViewList.AddRange(
                officeTypeList.Select(m => new SelectListItem()
                {
                    Text = m.OfficeTypeName,
                    Value = m.OfficeTypeId.ToString()
                }).ToList()
            );

            ViewBag.OfficeTypeList = officeTypeViewList;
        }

        [SessionExpireFilter]
        [DisableCache]
        public JsonResult RoleList(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var selectedRles = roleService.GetMany(w => w.Id == id).Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = selectedRles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var allRoles = roleService.GetAll().Select(s => new { DisplayText = s.Name, Value = s.Id }).ToList();
                return new JsonResult() { Data = new { Result = "OK", Options = allRoles }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
    #endregion
}
