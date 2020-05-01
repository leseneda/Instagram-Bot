using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class LocationCore : ILocationCore
    {
        #region Field

        static ILocationProcessor _apiLocationProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        #region Constructor

        private LocationCore(ILocationProcessor apiLocationProcessor, ConfigurationEntity configuration)
        {
            _apiLocationProcessor = apiLocationProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static ILocationCore Build(ILocationProcessor apiLocationProcessor, ConfigurationEntity configuration)
        {
            return new LocationCore(apiLocationProcessor, configuration);
        }

        #endregion

        #region Locationing

        public async Task<ResponseEntity<IResult<InstaLocationShortList>>> SearchLocationAsync(double latitude, double longitude, string search)
        {
            var result = await _apiLocationProcessor.SearchLocationAsync(latitude, longitude, search)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaLocationShortList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaPlaceList>>> SearchPlacesAsync(double latitude, double longitude)
        {
            var result = await _apiLocationProcessor.SearchPlacesAsync(latitude, longitude, _paginationParameters)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaPlaceList>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaUserSearchLocation>>> SearchUserByLocationAsync(double latitude, double longitude, string userName, int counter)
        {
            var result = await _apiLocationProcessor.SearchUserByLocationAsync(latitude, longitude, userName, counter)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaUserSearchLocation>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetRecentLocationListAsync(long locationId)
        {
            var result = await _apiLocationProcessor.GetRecentLocationFeedsAsync(locationId, _paginationParameters)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetTopLocationListAsync(long locationId)
        {
            var result = await _apiLocationProcessor.GetTopLocationFeedsAsync(locationId, _paginationParameters)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        #endregion
    }
}
