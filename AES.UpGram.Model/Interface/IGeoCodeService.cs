using MeConecta.Gram.Domain.Entity;
using System.Collections.Generic;

namespace MeConecta.Gram.Domain.Interface
{
    public interface IGeoCodeService
    {
        IEnumerable<GeocodingEntity> SearchGeoCode(string query);
    }
}
