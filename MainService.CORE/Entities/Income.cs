using System;

namespace MainService.CORE.Entities
{
    public class Income
    {
        public string Id { get; set; }          // Corresponds to str_id
        public string Name { get; set; }        // Corresponds to str_name
        public char Active { get; set; }        // Corresponds to str_active
        public DateTime Date { get; set; }      // Corresponds to dtm_date
    }
}
