namespace MainService.APPLICATION.DTOs
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

    public class UpdateRefRequest
    {
        public string Str_id { get; set; }
        public UpdateRefRequestUpdateFields Updates { get; set; }
    }

    public class UpdateRefRequestUpdateFields
    {
        public string Str_name { get; set; }
    }
}
