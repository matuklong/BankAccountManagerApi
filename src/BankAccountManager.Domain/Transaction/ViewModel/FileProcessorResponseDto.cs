using BankAccountManager.Domain.Transaction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAccountManager.Domain.Transaction.ViewModel;
public class FileProcessorResponseDto
{
    public decimal AccountBalance { get; set; } = 0;
    public List<FileProcessorLineResponseDto> Items { get; set; } = new List<FileProcessorLineResponseDto>();
}

public class FileProcessorLineResponseDto
{
    public int LineNumber { get; init; }
    public string? ErrorMessage { get; init; }
    public string RawLine { get; init; }
    public bool Error => !string.IsNullOrEmpty(ErrorMessage);
    public bool Success => CsvParsedData != null;
    public CsvParsedData? CsvParsedData { get; init; }
    public TransactionModel? Transaction { get; init; }

    public FileProcessorLineResponseDto(int lineNumber, string errorMessage, string rawLine)
    {
        LineNumber = lineNumber;
        ErrorMessage = errorMessage;
        RawLine = rawLine;
    }

    public FileProcessorLineResponseDto(int lineNumber, CsvParsedData csvParsedData, string rawLine, TransactionModel? transaction)
    {
        LineNumber = lineNumber;
        CsvParsedData = csvParsedData;
        RawLine = rawLine;
        Transaction = transaction;
    }
}
