namespace testFileUpload.Core.Models
{
    public enum ImportResultStatus
    {
        InvalidType,
        InvalidValidation,
        SystemError,
        Ok,
        InvalidSize,
        NoData
    }
}