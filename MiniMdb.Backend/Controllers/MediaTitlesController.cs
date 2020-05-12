using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Services;
using MiniMdb.Backend.Shared;
using MiniMdb.Backend.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiniMdb.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<DataPage<MediaTitleVm>>> GetListing
        (
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5
        )
        {
            // restrict page size to be between 1 and 10
            pageSize = Math.Min(Math.Max(pageSize, 1), 10);
            page = Math.Max(1, page);

            var dataPage = await _service.List(page, pageSize);
            return new DataPage<MediaTitleVm>(
                _mapper.Map<IEnumerable<MediaTitleVm>>(dataPage.Items),
                dataPage.Count, dataPage.Page, dataPage.PageSize
            );
        }


        // GET: api/MediaTitles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MediaTitleVm>> Get(int id)
        {
            return _mapper.Map<MediaTitleVm>(await _service.Get(id));
        }

        // POST: api/MediaTitles
        [HttpPost]
        public async Task Post([FromBody] MediaTitleVm title)
        {
            if (title.Type == Shared.MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Add(movie);
            }
            else if (title.Type == Shared.MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Add(series);
            }
        }

        // PUT: api/MediaTitles/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] MediaTitleVm title)
        {
            title.Id = id;
            if (title.Type == Shared.MediaTitleType.Movie)
            {
                var movie = _mapper.Map<Movie>(title);
                await _service.Update(movie);
            }
            else if (title.Type == Shared.MediaTitleType.Series)
            {
                var series = _mapper.Map<Series>(title);
                await _service.Update(series);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _service.Delete(id);
        }
    }
}
