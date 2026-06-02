using gHRM.Core.Utilities;
using gHRM.Data.DBDetailModels.Company;
using gHRM.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace gHRM.Web.Infrastucture.Utility
{
    public class WebHelper
    {
        public static DataTable GetCompanyInfo()
        {
            var dtCompanyInfo = new DataTable();
            try
            {
                var companyLogoPhysicalPath = @"/Assets/img/company-default-logo.jpg";

                var companySessionInfo = SessionHelper.CompanyInfo;

                if (companySessionInfo != null && !string.IsNullOrWhiteSpace(companySessionInfo.ImagePath))
                    companyLogoPhysicalPath = companySessionInfo.ImagePath;

                var companyInfo = new List<CompanyLogoViewModel>()
                {
                    new CompanyLogoViewModel
                    {
                        CompanyName=companySessionInfo.CompanyName!=null?companySessionInfo.CompanyName:"",
                        CompanyAddress=companySessionInfo.CompanyAddress!=null?companySessionInfo.CompanyAddress:"",
                        CompanyEmail=companySessionInfo.CompanyEmail!=null?companySessionInfo.CompanyEmail:"",
                        CompanyPhone=companySessionInfo.CompanyPhone!=null?companySessionInfo.CompanyPhone:"",
                        CompanyLogo = companyLogoPhysicalPath.ImagePathToByte(),
                        CompanyLogoURI = $@"{BaseURL()}/{companyLogoPhysicalPath}"  
                    }
                };

                dtCompanyInfo = companyInfo.ToDataTable();
            }
            catch
            {
                return null;
            }

            return dtCompanyInfo;
        }

        public static CompanyLogoViewModel GetCompanyDetails()
        {
            var companyInfo = new CompanyLogoViewModel();
            try
            {
                var companyLogoPhysicalPath = @"/Assets/img/company-default-logo.jpg";

                var companySessionInfo = SessionHelper.CompanyInfo;

                if (companySessionInfo != null && !string.IsNullOrWhiteSpace(companySessionInfo.ImagePath))
                    companyLogoPhysicalPath = companySessionInfo.ImagePath;

                companyInfo=new CompanyLogoViewModel
                {
                    CompanyName = companySessionInfo.CompanyName != null ? companySessionInfo.CompanyName : "",
                    CompanyAddress = companySessionInfo.CompanyAddress != null ? companySessionInfo.CompanyAddress : "",
                    CompanyEmail = companySessionInfo.CompanyEmail != null ? companySessionInfo.CompanyEmail : "",
                    CompanyPhone = companySessionInfo.CompanyPhone != null ? companySessionInfo.CompanyPhone : "",
                    CompanyLogo = companyLogoPhysicalPath.ImagePathToByte(),
                    CompanyLogoURI = $@"{BaseURL()}{companyLogoPhysicalPath}"
                };
            }
            catch
            {
                return new CompanyLogoViewModel();
            }

            return companyInfo;
        }

        public static string BaseURL()
        {
            string baseUrl = $@"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}" ;
    
            return baseUrl;
        }
    }
}