using System;
using System.Net;
using GNB.Infrastructure.Capabilities;

namespace GNB.Api.Helpers
{
    public static class ExceptionToHttpStatusCodeMap
    {
        public static HttpStatusCode Map(Type exType)
        {
            if (exType != typeof(GNBException))
                throw new InvalidOperationException("Map only defined for exception types");

            switch (exType)
            {
                default:
                    return HttpStatusCode.BadRequest;
            }
        }
    }
}
