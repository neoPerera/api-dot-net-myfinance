using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.DTOs
{
    public class GetRefListResponse
    {
        public string Key { get; set; }
        public string Str_name { get; set; }
        public string Dtm_date { get; set; }
    }
    public class AddRefRequest
    {
        public string StrId { get; set; }
        public string StrName { get; set; }
    }
    public class RefResponse
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
    }
    public class RefError
    {
        public object Detail { get; set; }
    }
}
