using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class HashTag : IInstaHashTag
    {
        static IHashtagProcessor _hashtagProcessor;
        static PaginationParameters _paginationParameters;

        private HashTag(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            _hashtagProcessor = hashtagProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static IInstaHashTag Build(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            return new HashTag(hashtagProcessor, configuration);
        }

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetTopHashtagListAsync(string tagName)
        {
            var result = await _hashtagProcessor.GetTopHashtagMediaListAsync(tagName, _paginationParameters);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }

        public async Task<ResponseEntity<IResult<InstaSectionMedia>>> GetRecentHashtagListAsync(string tagName)
        {
            var result = await _hashtagProcessor.GetRecentHashtagMediaListAsync(tagName, _paginationParameters);

            return new ResponseEntity<IResult<InstaSectionMedia>>()
            {
                Succeeded = result.Succeeded,
                Message = result.Info.Message,
                ResponseData = result
            };
        }
    }
}
