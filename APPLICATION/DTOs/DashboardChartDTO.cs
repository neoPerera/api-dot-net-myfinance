﻿namespace APPLICATION.DTOs
{
    public class DashboardResponse
    {

        public List<object> Chart1 { get; set; }
        public List<object> Chart2 { get; set; }
        public List<object> Chart3 { get; set; }
        public List<object> Chart4 { get; set; }


    }

    public class DashboardChart1DTO
    {
        public string? Type { get; set; }
        public float? Value { get; set; }
    }
    public class DashboardChart2DTO
    {
        public string? Source1 { get; set; }
        public string? Target { get; set; }
        public float? Value { get; set; }
    }
    public class DashboardChart3DTO
    {
        public string? Type { get; set; }
        public float? Value { get; set; }
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