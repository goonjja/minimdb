using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Services;
using MiniMdb.Backend.ViewModels;
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

        // GET: api/MediaTitles/5
        [HttpGet("{id}", Name = "Get")]
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
