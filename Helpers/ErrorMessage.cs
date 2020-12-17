using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Helpers
{
    public class ErrorMessage
    {
        public string Message;
        public int  StatusCode;
        public ErrorMessage(string message, int statusCode)
        {
            this.StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);

        }
        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
            case 404:
                return "Resource not found";
            case 500:
                return "An unhandled error occurred";
            default:
                return null;
        }
    }
    }
}
