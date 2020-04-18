﻿using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using MeConecta.Gram.Domain.Entity;
using MeConecta.Gram.Domain.Interface;
using System.Threading.Tasks;

namespace MeConecta.Gram.Core
{
    public class CoreHashTag : ICoreHashTag
    {
        #region Field

        static IHashtagProcessor _hashtagProcessor;
        static PaginationParameters _paginationParameters;

        #endregion

        #region Constructor

        private CoreHashTag(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            _hashtagProcessor = hashtagProcessor;
            _paginationParameters = PaginationParameters.MaxPagesToLoad(configuration.MaxPagesToLoad);
        }

        public static ICoreHashTag Build(IHashtagProcessor hashtagProcessor, ConfigurationEntity configuration)
        {
            return new CoreHashTag(hashtagProcessor, configuration);
        }

        #endregion

        #region HashTagning 

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

        #endregion
    }
}
