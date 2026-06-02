using AutoMapper;
using gHRM.Data.CodeFirstMigration;
using gHRM.Service;
using gHRM.Service.ReportServies;
using gHRM.Service.StoreProcedure;
using gHRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gHRM.Web.Controllers.Basic
{
    public class UnionController : BaseController
    {
        #region variables          
        private readonly ILgThanaService thanaService;
        private readonly IStateOrProvinceService stateOrProvinceService;
        private readonly IDistrictService districtService;
        private readonly ICountryService countryService;
        private readonly IUnionService unionService;
        private readonly IEmployeeSPService employeeSPService;
        public UnionController(ILgThanaService thanaService, IDistrictService districtService, IStateOrProvinceService stateOrProvinceService, ICountryService countryService, IUnionService unionService, IEmployeeSPService employeeSPService)
        {
            this.countryService = countryService;
            this.thanaService = thanaService;
            this.districtService = districtService;
            this.stateOrProvinceService = stateOrProvinceService;
            this.unionService = unionService;
            this.employeeSPService = employeeSPService;
        }
        #endregion

        #region Event
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["DistList"] = items;
            ViewData["ThanaList"] = items;
            return View();
        }
        public ActionResult Create()
        {
            var model = new UnionViewModel();
            MapDropDownList(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(UnionViewModel model)
        {
            var entity = Mapper.Map<UnionViewModel, LgUnion>(model);
            try
            {
                //is exist union
                var isExistUnion = CheckExistUnion(model);

                if (isExistUnion)
                    return GetErrorMessageResult($"Union '{model.union_name_eng}' is already exist. Please try another.");

                var maxUnionCode = unionService.GetMany(f => f.union_id >= 0).OrderByDescending(p => p.union_code).FirstOrDefault();

                int unionCode = 1;
                if (maxUnionCode != null)
                {
                    unionCode = maxUnionCode.union_id + 1;
                }
                entity.union_code = unionCode.ToString();
                entity.union_name_bng = entity.union_name_bng;

                unionService.Create(entity);

                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { data = entity }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var union = unionService.GetById(id);
            var entity = Mapper.Map<LgUnion, UnionViewModel>(union);
            MapDropDownList(entity);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Edit(UnionViewModel model)
        {
            try
            {
                //is exist union
                var isExistUnion = CheckExistUnion(model);

                if (isExistUnion)
                    return GetErrorMessageResult($"Union '{model.union_name_eng}' is already exist. Please try another.");


                var entity = Mapper.Map<UnionViewModel, LgUnion>(model);
                var getUnionDetails = unionService.GetById(Convert.ToInt32(entity.union_id));
                getUnionDetails.union_name_eng = entity.union_name_eng;
                unionService.Update(getUnionDetails);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        #endregion

        #region methods
        private void MapDropDownList(UnionViewModel model)
        {
            // Country Dropdown
            var countryList = countryService.GetAll();
            var viewCountryList = countryList.Select(m => new SelectListItem() { Text = m.CountryName, Value = m.CountryId.ToString() });
            model.CountryList = viewCountryList;

            var stateList = new List<SelectListItem>();
            stateList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.StateOrProvinceList = stateList;

            //District Dropdown
            var districtList = new List<SelectListItem>();
            districtList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.DistrictList = districtList;

            var thanaList = new List<SelectListItem>();
            thanaList.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
            model.ThanaList = thanaList;
        }


        public JsonResult GetStateList(string country_id)
        {
            var stateList = stateOrProvinceService.GetMany(c => c.CountryId == Convert.ToInt32(country_id)).OrderBy(o => o.Name).ToList();
            var viewState = stateList.Select(x => new SelectListItem
            {
                Value = x.StateOrProvinceId.ToString(),
                Text = x.Name.ToString()
            });
            var state_items = new List<SelectListItem>();
            state_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            state_items.AddRange(viewState);
            return Json(state_items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictList(int state_id)
        {
            var districtList = districtService.GetMany(f => f.division_Id == state_id).OrderBy(o => o.district_name_eng).ToList();
            var viewDistrict = districtList.Select(x => new SelectListItem
            {
                Value = x.district_id.ToString(),
                Text = x.district_name_eng.ToString()
            });
            var district_items = new List<SelectListItem>();
            district_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            district_items.AddRange(viewDistrict);
            return Json(district_items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetThanaList(int district_id)
        {
            IEnumerable<SelectListItem> viewThana = new List<SelectListItem>();
            var thanaList = thanaService.GetMany(c => c.district_id == district_id).ToList();
            if (thanaList.Any())
            {
                viewThana = thanaList.Select(x => new SelectListItem
                {
                    Value = x.thana_id.ToString(),
                    Text = x.thana_name_eng.ToString()
                });
            }
            var thana_items = new List<SelectListItem>();
            thana_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            thana_items.AddRange(viewThana);

            return Json(thana_items, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUnion(int jtStartIndex, int jtPageSize, string jtSorting, string thanaId, string filterColumnName, string filterValue)
        {
            try
            {
                var param = new { ThanaId = thanaId };
                var unionList = employeeSPService.GetDataWithParameter(param, "dbo.LgUnion_GetUnionsByFilter");
                var viewUnionList = unionList.Tables[0].AsEnumerable().Select(p => new UnionViewModel
                {
                    district_name_eng = p.Field<string>("district_name_eng"),
                    thana_name_eng = p.Field<string>("thana_name_eng"),
                    union_id = p.Field<int>("union_id"),
                    union_name_eng = p.Field<string>("union_name_eng")
                }).ToList();
                var currentPageRecords = viewUnionList.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = viewUnionList.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Private Methods

        private bool CheckExistUnion(UnionViewModel model)
        {
            var isExistUnion = false;

            if (model.union_id > 0)
            {
                isExistUnion = unionService.GetMany(f =>
                                                    f.union_id != model.union_id
                                                && f.thana_id == model.thana_id
                                                && f.union_name_eng == model.union_name_eng)
                                            .Any();
                return isExistUnion;
            }

            isExistUnion = unionService.GetMany(f =>
                                                f.thana_id == model.thana_id
                                            && f.union_name_eng == model.union_name_eng)
                                        .Any();
            return isExistUnion;
        }


        #endregion
    }
}