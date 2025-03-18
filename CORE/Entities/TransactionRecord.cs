using System;

namespace CORE.Entities
{
    public class TransactionRecord
    {
        public string Id { get; set; }           // Corresponds to str_id
        public string TrnType { get; set; }      // Corresponds to str_trn_type
        public string TrnCat { get; set; }       // Corresponds to str_trn_cat
        public decimal? Amount { get; set; }     // Corresponds to int_amount (numeric type in PostgreSQL)
        public DateTime? Date { get; set; }      // Corresponds to dtm_date (timestamp without time zone)
        public string Reason { get; set; }       // Corresponds to str_reason
        public string User { get; set; }         // Corresponds to str_user
        public string Account { get; set; }      // Corresponds to str_account
    }
}
