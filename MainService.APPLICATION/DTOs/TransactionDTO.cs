using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainService.APPLICATION.DTOs
{
    public class GetTransactionSequenceResponse
    {
        public List<object> Accounts { get; set; }
        public string Sequence_id { get; set; }
        public List<object> Trans_types { get; set; }
    }
    public class GetTransactionSequenceTypes
    {
        public string Value { get; set; }
        public string Label { get; set; }

    }
    public class AddTransactionRequest
    {
        public string StrId { get; set; }
        public string StrName { get; set; }
        public string StrAccount { get; set; }
        public string StrAccount2 { get; set; }
        public string StrTransCat { get; set; }
        public string StrTransType { get; set; }
        public float FloatAmount { get; set; }
        public bool IsDoubleEntry { get; set; }
    }

}
