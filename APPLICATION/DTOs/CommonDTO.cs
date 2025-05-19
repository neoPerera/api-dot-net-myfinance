using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.DTOs
{
    public class CommonResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
    }
    public class CommonError
    {
        public object Detail { get; set; }
    }
}
