using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAKBank_Parser
{
    class Program
    {
        static ParseRAKBankStatementResult ParseRAKBankStatement(string strFilePath)
        {
            ParseRAKBankStatementResult result = new ParseRAKBankStatementResult();

            var strFileContent = System.IO.File.ReadAllText(strFilePath);
            var strFileContentLines = strFileContent.Split(new char[] { '\n' });

            result.strAccountName = strFileContentLines[3].Split(new char[] { ',' })[2];
            result.strAccountNumber = strFileContentLines[4].Split(new char[] { '.' })[1].Split(new char[] { ']' })[0];
            result.strBranchName = strFileContentLines[6].Split(new char[] { '[' })[1].Split(new char[] { ']' })[0];
            result.d8From = Convert.ToDateTime(DateTime.ParseExact(strFileContentLines[7].Split(new char[] { ',' })[2], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            result.d8To = Convert.ToDateTime(DateTime.ParseExact(strFileContentLines[8].Split(new char[] { ',' })[2], "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            string strCSVContent = string.Empty;
            for (int i = 10; i < strFileContentLines.Length - 2; i++)
            {
                if (i == 10)
                {
                    var Headers = strFileContentLines[i].Split(new char[] { ',' });
                    strCSVContent += $"EmptyHeader1,{Headers[1]},EmptyHeader2,{Headers[3]},{Headers[4]},{Headers[5]},{Headers[6]},{Headers[7]}{Environment.NewLine}";
                }
                else
                {
                    strCSVContent += strFileContentLines[i] + Environment.NewLine;
                }
            }

            var textReader = new System.IO.StringReader(strCSVContent);
            var csvr = new CsvHelper.CsvReader(textReader);
            csvr.Configuration.PrepareHeaderForMatch = header => header?.Trim();
            csvr.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);
            result.CSVRecords = csvr.GetRecords<RAKBankCSVRecord>();

            return result;
        }
        static List<BankRecord> GetBankRecords(IEnumerable<RAKBankCSVRecord> CSVRecords)
        {
            List<BankRecord> bankRecords = new List<BankRecord>();

            foreach (var CSVRecord in CSVRecords)
            {
                var bankRecord = new BankRecord();
                bankRecord.BankStatementSequence = bankRecords.Count + 1;
                bankRecord.TransactionDate = Convert.ToDateTime(DateTime.ParseExact(CSVRecord.Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture));
                bankRecord.ChequeNo = CSVRecord.ChequeNo;
                bankRecord.BankDescription = CSVRecord.Description;
                bankRecord.AmountInBaseCurrency = string.IsNullOrEmpty(CSVRecord.Withdrawal) ? Convert.ToDouble(CSVRecord.Deposit) : Convert.ToDouble(CSVRecord.Withdrawal) * -1;
                bankRecord.Currency =
                    bankRecord.BankDescription.Contains("USD ") ? "USD" :
                    bankRecord.BankDescription.Contains("CAD ") ? "CAD" :
                    bankRecord.BankDescription.Contains("SAR ") ? "SAR" :
                    bankRecord.BankDescription.Contains("EUR ") ? "EUR" :
                    "AED";
                try
                {
                    bankRecord.AmountInOriginalCurrency = Convert.ToDouble(bankRecord.BankDescription.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)[2].Split(new[] { ' ' })[1]);
                }
                catch (Exception)
                {
                    bankRecord.AmountInOriginalCurrency = bankRecord.AmountInBaseCurrency;
                }
                bankRecord.ExchangeRate = Math.Abs(bankRecord.AmountInBaseCurrency / bankRecord.AmountInOriginalCurrency);
                bankRecords.Add(bankRecord);
            }

            return bankRecords;
        }
        static void WriteCSVFile(ParseRAKBankStatementResult varParseRAKBankStatementResult, string strFolder)
        {
            var strFileName = $"{varParseRAKBankStatementResult.strAccountName}  - {varParseRAKBankStatementResult.strAccountNumber} ({varParseRAKBankStatementResult.d8From.ToString("ddMMyyyy")} - {varParseRAKBankStatementResult.d8To.ToString("ddMMyyyy")}).csv";
            strFileName = System.IO.Path.Combine(strFolder, strFileName);
            System.IO.TextWriter textWriter = System.IO.File.CreateText(strFileName);
            var csvWriter = new CsvHelper.CsvWriter(textWriter);
            varParseRAKBankStatementResult.bankRecords.Reverse();
            csvWriter.WriteRecords(varParseRAKBankStatementResult.bankRecords);
            textWriter.Flush();
            textWriter.Close();
        }

        static void Main(string[] args)
        {
            //var strFilePath = @"D:\Downloads\Account_Transactions_CSV26-08-2018.csv";
            var strFilePath = args[0];
            var strFolderPath = System.IO.Path.GetDirectoryName(strFilePath);
            ParseRAKBankStatementResult varParseRAKBankStatementResult = null;
            try
            {
                varParseRAKBankStatementResult = ParseRAKBankStatement(strFilePath);
                varParseRAKBankStatementResult.bankRecords = GetBankRecords(varParseRAKBankStatementResult.CSVRecords);
                WriteCSVFile(varParseRAKBankStatementResult, strFolderPath);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}