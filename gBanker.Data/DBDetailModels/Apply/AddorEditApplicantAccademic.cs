using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Data.DBDetailModels.Apply
{
    public class AddorEditApplicantAccademic
    {

        public Int64 ID { get; set; }


        public Int64 ApplicantId { get; set; }


        public string LevelofEducation { get; set; }


        public string ExamTitle { get; set; }


        public string Group { get; set; }


        public string InstituteName { get; set; }


        public string ResultType { get; set; }


        public decimal? CGPA { get; set; }


        public decimal? Scale { get; set; }

        public DateTime? YearsofPassing { get; set; }


        public string Duration_Years { get; set; }
    }
}
