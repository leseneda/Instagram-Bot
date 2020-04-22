using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Enum;
using System;
using System.Linq;

namespace MeConecta.Gram.Service
{
    public static class ResponseManageService
    {
        public static bool NonTroubleResponse(ResponseType responseType) 
        {
            return Enum.GetValues(typeof(ResponseTypeNonTroubleEnum))
                .Cast<ResponseTypeNonTroubleEnum>()
                .Any(item => (int)item == (int)responseType);
        }



    }
}
