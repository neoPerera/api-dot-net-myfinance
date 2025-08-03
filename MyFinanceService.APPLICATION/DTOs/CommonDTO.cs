
namespace MyFinanceService.APPLICATION.DTOs
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
