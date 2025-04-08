using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APPLICATION.DTOs
{
    public class DashboardResponse
    {
       
        public List<object> Chart1 { get; set; }
        public List<object> Chart2 { get; set; }
        public List<object> Chart3 { get; set; }
        public List<object> Chart4 { get; set; }


    }
}
