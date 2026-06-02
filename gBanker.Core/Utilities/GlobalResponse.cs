using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gHRM.Core.Utilities
{
    public class GlobalResponse<T> : BaseResponse
    {
        public T Result { get; set; }

    }

    public class BaseResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool ContinueProcess { get; set; }
        public string ReturnCode { get; set; }
        public string ReturnMessage { get; set; }
    }

}
