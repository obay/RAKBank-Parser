using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKBank_Parser
{
    class RAKBankCSVRecord
    {
        public string EmptyHeader1 { get; set; }
        public string Date { get; set; }
        public string EmptyHeader2 { get; set; }
        public string Description { get; set; }
        public string ChequeNo { get; set; }
        public string Withdrawal { get; set; }
        public string Deposit { get; set; }
        public string Balance { get; set; }
    }
}
