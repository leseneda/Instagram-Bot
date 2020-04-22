using InstagramApiSharp.Classes;
using MeConecta.Gram.Domain.Enum;
using System;
using System.Linq;

namespace MeConecta.Gram.Service
{
    public static class ResponseManageService
    {
        static readonly sbyte maxCount = 5;
        static sbyte counter = 0;

        public static bool NonTroubleResponse(ResponseType responseType) 
        {
            if (counter == maxCount)
            {
                return false;
            }

            var isNonTrouble = Enum.GetValues(typeof(ResponseTypeNonTroubleEnum))
                .Cast<ResponseTypeNonTroubleEnum>()
                .Any(item => (short)item == (short)responseType);

            if (isNonTrouble)
            {
                counter++;
            }

            return (counter == maxCount) ? false : isNonTrouble;
        }
    }
}
