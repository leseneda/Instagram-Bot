using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using System.Threading.Tasks;

namespace MeConecta.Gram.Domain.Interface
{
    public interface ICoreLocation : IBuild
    {
        Task<ResponseEntity<IResult<InstaLocationShortList>>> SearchLocationAsync(double latitude, double longitude, string search);
        Task<ResponseEntity<IResult<InstaPlaceList>>> SearchPlacesAsync(double latitude, double longitude);
        Task<ResponseEntity<IResult<InstaUserSearchLocation>>> SearchUserByLocationAsync(double latitude, double longitude, string userName, int counter);
    }
}
