namespace NET.API.DTOs
{
    /// <summary>
    /// Hata durumlarında API yanıtı için kullanılan sınıf.
    /// </summary>
    public class ErrorResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public ErrorResponse(string message, List<string> errors)
        {
            Message = message;
            Errors = errors;
        }
    }
}
