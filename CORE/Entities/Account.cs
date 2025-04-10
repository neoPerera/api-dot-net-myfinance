namespace CORE.Entities
{
    public class Account
    {
        public string Id { get; set; }        // Corresponds to 'str_id' column
        public string Name { get; set; }      // Corresponds to 'str_name' column
        public char Active { get; set; }    // Corresponds to 'str_active' column
        public string User { get; set; }      // Corresponds to 'str_user' column
        public DateTime Date { get; set; }    // Corresponds to 'dtm_date' column
        public char IsMain { get; set; }      // Corresponds to 'str_ismain' column (default 'N')
    }
}