using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKBank_Parser
{
    class BankRecord
    {
        public int BankStatementSequence { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ChequeNo { get; set; }
        public string BankDescription { get; set; }
        public double AmountInOriginalCurrency { get; set; }
        public string Currency { get; set; }
        public double ExchangeRate { get; set; }
        public double AmountInBaseCurrency { get; set; }
    }
}