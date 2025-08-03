namespace MainService.CORE.Entities
{
    public class Expense
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public char Active { get; set; }
        public DateTime Date { get; set; }
    }
}