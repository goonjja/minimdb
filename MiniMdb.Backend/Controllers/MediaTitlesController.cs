using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public MediaTitlesController(IMediaTitlesService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns paginated listing of MediaTitle entities.
        /// </summary>
        /// <param name="page">page number (starting from 1)</param>
        /// <param name="pageSize">page size</param>
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
            // restrict page size to be between 1 and 10
            pageSize = Math.Min(Math.Max(pageSize, 1), 10);
            page = Math.Max(1, page);

            var dataPage = await _service.List(new MediaTitleSearchCriteria(nameFilter, typeFilter), page, pageSize);
            return new ApiMessage<MediaTitleVm>
            {
                Data = _mapper.Map<IEnumerable<MediaTitleVm>>(dataPage.Items).ToArray(),
                Pagination = new ApiPagination(dataPage.Page, dataPage.PageSize, dataPage.Count)
            };
        }


        // GET: api/MediaTitles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Get(int id)
        {
            return ApiMessage.From(_mapper.Map<MediaTitleVm>(await _service.Get(id)));
        }

        // POST: api/MediaTitles
        [HttpPost]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Post([FromBody] MediaTitleVm title)
        {
            if (title.Type == MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Add(movie);
            }
            else if (title.Type == MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Add(series);
            }
            return ApiMessage.From(title);
        }

        // PUT: api/MediaTitles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiMessage<MediaTitleVm>>> Put(int id, [FromBody] MediaTitleVm title)
        {
            title.Id = id;
            if (title.Type == MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Update(movie);
            }
            else if (title.Type == MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Update(series);
            }
            return ApiMessage.From(title);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.Delete(id);
        }
    }
}
