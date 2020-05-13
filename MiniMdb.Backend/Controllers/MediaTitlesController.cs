using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MiniMdb.Auth;
using MiniMdb.Backend.Helpers;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Services;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMdb.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MediaTitlesController : ControllerBase
    {
        private readonly IMediaTitlesService _service;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MediaTitlesController> _logger;

        public MediaTitlesController
        (
            IMediaTitlesService service, 
            IMapper mapper,
            IMemoryCache cache,
            ILogger<MediaTitlesController> logger
        )
        {
            _service = service;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Returns paginated listing of MediaTitle entities.
        /// </summary>
        /// <param name="typeFilter">filter by type</param>
        /// <param name="nameFilter">filter by name (starting with)</param>
        /// <param name="page">Page number (starting from 1)</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Page of MediaTitle listing</returns>
        [HttpGet]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> GetListing
        (
            [FromQuery] MediaTitleType? typeFilter = null,
            [FromQuery] string nameFilter = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5
        )
        {
            // restrict page size to be between 1 and 20
            pageSize = Math.Min(Math.Max(pageSize, 1), 20);
            page = Math.Max(1, page);

            var dataPage = await _service.List(new MediaTitleSearchCriteria(nameFilter, typeFilter), page, pageSize);
            return new ApiMessage<MediaTitleVm>
            {
                Data = _mapper.Map<IEnumerable<MediaTitleVm>>(dataPage.Items).ToArray(),
                Pagination = new ApiPagination(dataPage.Page, dataPage.PageSize, dataPage.Count)
            };
        }


        /// <summary>
        /// Get MediaTitle by id
        /// </summary>
        /// <param name="id">Title id</param>
        /// <returns>Entity or error</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Get(int id)
        {
            var cacheKey = CacheKeys.MediaTitle(id);
            if (_cache.TryGetValue(cacheKey, out var cachedEntity))
            {
                _logger.LogDebug("Retrieved from cache!");
                return ApiMessage.From(_mapper.Map<MediaTitleVm>(cachedEntity as MediaTitle));
            }

            var entity = await _service.Get(id);

            if (entity == null)
                return BadRequest(ApiMessage.MakeError(3, "Media title not found"));

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(5))
                .SetSize(1);
            _cache.Set(cacheKey, entity, cacheEntryOptions);

            return ApiMessage.From(_mapper.Map<MediaTitleVm>(entity));
        }

        /// <summary>
        /// Save new MediaTitle
        /// </summary>
        /// <param name="title">New media title payload</param>
        /// <returns>Saved data</returns>
        [HttpPost]
        [Authorize(Policy = MiniMdbRoles.AdminPolicy)]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Post([FromBody] MediaTitleVm title)
        {
            if (title.Type == MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Add(movie);
                title.Id = movie.Id;
            }
            else if (title.Type == MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Add(series);
                title.Id = series.Id;
            }
            return ApiMessage.From(title);
        }

        /// <summary>
        /// Update existing MediaTitle by id
        /// </summary>
        /// <param name="id">Title id</param>
        /// <param name="title">Media title payload</param>
        /// <returns>Saved data</returns>
        [HttpPut("{id}")]
        [Authorize(Policy = MiniMdbRoles.AdminPolicy)]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Put(int id, [FromBody] MediaTitleVm title)
        {
            // find first to determine type
            var entity = await _service.Get(id);
            if (entity == null)
                return BadRequest(ApiMessage.MakeError(3, "Media title not found"));

            title.Id = id;
            if (entity.Type == MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Update(movie);
            }
            else if (entity.Type == MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Update(series);
            }
            // invalidate cache
            _cache.Remove(CacheKeys.MediaTitle(id));

            return ApiMessage.From(title);
        }

        /// <summary>
        /// Deletes MediaTitle by its id
        /// </summary>
        /// <param name="id">Media title id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = MiniMdbRoles.AdminPolicy)]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Delete(int id)
        {
            var entity = await _service.Delete(id);

            if(entity == null)
                return BadRequest(ApiMessage.MakeError(3, "Media title not found"));

            // invalidate cache
            _cache.Remove(CacheKeys.MediaTitle(id));

            return ApiMessage.From(_mapper.Map<MediaTitleVm>(entity));
        }
    }
}
