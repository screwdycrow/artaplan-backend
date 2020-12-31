using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Errors
{
    public class ErrorMessage
    {
        private static Dictionary<Error, string> errorMessages;

        public static string ShowErrorMessage(Error error)
        {
            assignDefaultValues();
            return errorMessages[error];
        }

        private static void assignDefaultValues()
        {
           errorMessages = new Dictionary<Error, string>()
        {
            { Error.UserNotFound, "Could not find user"},
            { Error.StageNotFound, "Stage doesn't exist" },
            { Error.SlotNotFound, "This slot does not exist" },
            { Error.JobNotFound, "This job does not exist" },
            { Error.CustomerNotFound, "Customer not found" },
            { Error.NonMatchingId, "The Id of the request doesn't match the object Id" },
            { Error.FailedAuthentication, "Could not authenticate user" },
            { Error.InternalServerError, "Internal Server Error"}
        };
        }
    }
}
