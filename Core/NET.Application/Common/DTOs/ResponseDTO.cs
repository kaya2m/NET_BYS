using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Application.Common.DTOs
{
    /// <summary>
    /// API yanıtları için genel DTO sınıfı.
    /// </summary>
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ResponseDTO<T> SuccessResult(T data, string message = null)
        {
            return new ResponseDTO<T>
            {
                Success = true,
                Message = message ?? "İşlem başarıyla tamamlandı",
                Data = data
            };
        }

        public static ResponseDTO<T> ErrorResult(string message)
        {
            return new ResponseDTO<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}
