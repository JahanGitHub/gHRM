using System.ComponentModel.DataAnnotations;

namespace gHRM.Core.Filters.eRecruit
{
    public class DefaultSearchFilter
    {
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }

        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalSkip { get; set; }
        public int TotalCount { get; set; }
        public bool IsCalculateTotal { get; set; }

    }
}
