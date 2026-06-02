using System.Collections.Generic;
using System.Linq;

namespace gHRM.Core.Utilities.Constants
{
    public static class ConstantHelpers
    {
        public static string FindItemInList(this IEnumerable<ConstantDropdownItem> items, string searchTerm)
        {
            var item = items.FirstOrDefault(i => i.Value == searchTerm);
            return item == null ? "N/A" : item.Text;
        }
    }
}
