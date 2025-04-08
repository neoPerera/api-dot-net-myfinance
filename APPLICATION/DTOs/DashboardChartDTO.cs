using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATION.DTOs
{
    public class DashboardChart1DTO
    {
        public string? Type;
        public float? Value;
    }
    public class DashboardChart2DTO
    {
        public string? Source1;
        public string? Target;
        public float? Value;
    }public class DashboardChart3DTO
    {
        public string? Type;
        public float? Value;
    }
    //public class DashboardChart4DTO
    //{
    //    public string? Key;
    //    public string? Name;
    //    public string? Account;
    //    public float? IntAmount;
    //    public string Int_amount_char;
    //}
}

public class DashboardChart4Record
{
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? Account { get; set; }
    public float? IntAmount { get; set; }
    public string Int_amount_char { get; set; }
}

public class DashboardChart4AccountBalance
{
    public string Account_name { get; set; }
    public float Account_balance { get; set; }
}