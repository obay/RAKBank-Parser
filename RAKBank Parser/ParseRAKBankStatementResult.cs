using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKBank_Parser
{
    class ParseRAKBankStatementResult
    {
        public string strAccountName { get; set; }
        public string strAccountNumber { get; set; }
        public string strBranchName { get; set; }
        public DateTime d8From { get; set; }
        public DateTime d8To { get; set; }
        public IEnumerable<RAKBankCSVRecord> CSVRecords { get; set; }
        public List<BankRecord> bankRecords { get; set; }
    }
}
