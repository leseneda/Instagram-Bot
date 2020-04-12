using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface ILocation : IBuild
    {
        Task<ResponseEntity<IResult<InstaLocationShortList>>> SearchLocationAsync(double latitude, double longitude, string search);
        Task<ResponseEntity<IResult<InstaPlaceList>>> SearchPlacesAsync(double latitude, double longitude, string search);
    }
}
