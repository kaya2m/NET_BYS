namespace NET.API.DTOs
{
    /// <summary>
    /// Doğrulama hatalarında API yanıtı için kullanılan sınıf.
    /// </summary>
    public class ValidationErrorResponse
    {
        public IDictionary<string, string[]> Errors { get; set; }
        public string Message { get; set; }

        public ValidationErrorResponse(IDictionary<string, string[]> errors, string message = "Doğrulama hatası")
        {
            Errors = errors;
            Message = message;
        }
    }
}
