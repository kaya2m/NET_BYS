using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Application.Common.Exceptions
{
    /// <summary>
    /// Geçersiz istek durumları için exception.
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
}
