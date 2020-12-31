using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artaplan.Errors
{
    public enum Error { UserNotFound, JobNotFound, SlotNotFound, CustomerNotFound, StageNotFound, FailedAuthentication, NonMatchingId,
        InternalServerError
    }
}
