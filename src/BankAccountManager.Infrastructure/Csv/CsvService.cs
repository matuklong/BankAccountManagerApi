using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.Model;
using BankAccountManager.Domain.Transaction.Service;
using BankAccountManager.Domain.Transaction.ViewModel;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Infrastructure.Csv;
public class CsvService : IFileProcessorService
{

    public async Task<List<FileProcessorResponseDto>> ProcessCsvAsync(Stream stream, AccountModel account, CultureInfo cultureInfo, string delimiter, bool hasHeaderRecord)
    {
        // CultureInfo.GetCultureInfo("pt-BR")
        var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(cultureInfo)
        {
            Delimiter = delimiter, //";",  // Use semicolon as the delimiter
            HasHeaderRecord = hasHeaderRecord, // false,  // No header in the CSV
        };

        var responseList = new List<FileProcessorResponseDto>();
        using (var reader = new StreamReader(stream))
        using (var csv = new CsvReader(reader, csvConfiguration)) // Using CsvHelper library
        {
            // Configure date and decimal formats for the specific columns
            //csv.Context.TypeConverterOptionsCache.GetOptions<DateTime?>().Formats = new[] { "dd/MM/yyyy" };  // Date format
            //csv.Context.TypeConverterOptionsCache.GetOptions<decimal?>().NumberStyles = System.Globalization.NumberStyles.AllowDecimalPoint;


            while (await csv.ReadAsync())
            {
                try
                {
                    // Mapping CSV fields to a class
                    var record = csv.GetRecord<CsvParsedData>();

                    if (record == null)
                    {
                        responseList.Add(new FileProcessorResponseDto (csv.Parser.Row, "Could not read line.", csv.Parser.RawRecord));
                        continue;
                    }

                    if (!record.Amount.HasValue || !record.TransactionDate.HasValue)
                    {
                        responseList.Add(new FileProcessorResponseDto(csv.Parser.Row, $"Missing or could not parse required fields {nameof(record.Amount)} or {nameof(record.TransactionDate)}", csv.Parser.RawRecord));
                        continue;
                    }

                    var transaction = new TransactionModel(account, record.Amount.Value, record.Description ?? "", record.TransactionDate.Value, DateTime.Now, false, false);

                    account.AddTransaction(transaction);

                    responseList.Add(new FileProcessorResponseDto(csv.Parser.Row, record, csv.Parser.RawRecord, transaction));

                }
                catch (Exception ex)
                {
                    // Log or handle error, and include the current row information
                    var errorMessage = $"Error at row {csv.Parser.Row}: {ex.Message}";
                    // Optionally, you could log the specific line that caused the issue
                    var problematicLine = string.Join(",", csv.Parser.RawRecord); // Raw line as a string

                    responseList.Add(new FileProcessorResponseDto(csv.Parser.Row, errorMessage, csv.Parser.RawRecord));

                }
            }
        }

        return responseList;
    }
}
