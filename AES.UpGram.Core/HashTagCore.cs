using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class HashTagCore : IHashTagCore
    {
        #region Field

        static IHashtagProcessor _hashtagProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        #region Constructor

        private HashTagCore(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            _hashtagProcessor = hashtagProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static IHashTagCore Build(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            return new HashTagCore(hashtagProcessor, configuration);
        }

        #endregion

        #region HashTagning 

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetTopHashtagListAsync(string tagName)
        {
            var result = await _hashtagProcessor.GetTopHashtagMediaListAsync(tagName, _paginationParameters)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseType = result.Info.ResponseType,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetRecentHashtagListAsync(string tagName)
        {
            var result = await _hashtagProcessor.GetRecentHashtagMediaListAsync(tagName, _paginationParameters)
                .ConfigureAwait(false);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseType = result.Info.ResponseType,
                ResponseData = result
            };
        }

        #endregion
    }
}
