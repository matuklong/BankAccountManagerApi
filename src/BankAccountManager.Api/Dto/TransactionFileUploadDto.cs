namespace BankAccountManager.Api.Dto;

public class TransactionFileUploadDto
{
    public IFormFile? FileUpload { get; set; }
    public int AccountId { get; set; }
}
