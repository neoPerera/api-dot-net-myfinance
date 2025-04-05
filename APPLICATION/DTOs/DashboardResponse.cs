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
       
        public List<DashboardChart1DTO> Chart1 { get; set; }
        public List<DashboardChart2DTO> Chart2 { get; set; }
        public List<DashboardChart3DTO> Chart3 { get; set; }
        public List<DashboardChart4DTO> Chart4 { get; set; }

    }
}
