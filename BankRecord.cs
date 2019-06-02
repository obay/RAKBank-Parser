using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKBank_Parser
{
    class BankRecord
    {
        // Bank Statement Sequence
        public int BankStatementSequence { get; set; }
        // Date
        public DateTime TransactionDate { get; set; }
        // Payee
        public string Payee { get; set; }
        // Payee Type
        public string PayeeType { get; set; }
        // TRN
        public string TRN { get; set; }
        // Check Number
        public string ChequeNo { get; set; }
        // Category
        public string Category { get; set; }
        // Bank Description
        public string BankDescription { get; set; }
        // Amount in Original Currency
        public double AmountInOriginalCurrency { get; set; }
        // Currency
        public string Currency { get; set; }
        // Exchange Rate
        public double ExchangeRate { get; set; }
        // Amount in Base Currency
        public double AmountInBaseCurrency { get; set; }
        // Recurring
        // Taxable
        // Tax
        // in QBO
        // Balance
        // Remarks
    }
}