using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gHRM.Web.ViewModels
{
    public class NotificationModuleViewModel
    {
        public int NotificationModuleId { get; set; }
        public string ModuleName { get; set; }
        public string LinkText { get; set; }
        public string LinkValue { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int NotificationCount { get; set; }
    }
}