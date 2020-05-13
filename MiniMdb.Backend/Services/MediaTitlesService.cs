using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMdb.Backend.Services
{
    public struct MediaTitleSearchCriteria
    {
        public string Name { get; }
        public MediaTitleType? Type { get; }

        public MediaTitleSearchCriteria(string name, MediaTitleType? type = null)
        {
            Name = name;
            Type = type;
        }
    }

    /// <summary>
    /// Service to access and modify MediaTitles stored in database
    /// </summary>
    public interface IMediaTitlesService
    {
        Task<long> Add(Movie movie);
        Task<long> Add(Series series);

        Task<MediaTitle> Get(long id);

        Task<bool> Update(Movie movie);
        Task<bool> Update(Series series);

        Task<MediaTitle> Delete(long id);

        Task<DataPage<MediaTitle>> List(MediaTitleSearchCriteria searchCriteria, int page, int pageSize);
    }

    public class MediaTitlesService : IMediaTitlesService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITimeService _time;

        public MediaTitlesService(AppDbContext dbContext, ITimeService time)
        {
            _dbContext = dbContext;
            _time = time;
        }

        public async Task<long> Add(Movie movie)
        {
            movie.AddedAt = _time.Now();
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            return movie.Id;
        }

        public async Task<long> Add(Series series)
        {
            series.AddedAt = _time.Now();
            _dbContext.Series.Add(series);
            await _dbContext.SaveChangesAsync();
            return series.Id;
        }

        public async Task<MediaTitle> Get(long id)
        {
            return await _dbContext.Titles.FindAsync(id); ;
        }

        public async Task<bool> Update(Movie movie)
        {
            try
            {
                movie.UpdatedAt = _time.Now();
                _dbContext.Movies.Update(movie);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Titles.Any(e => e.Id == movie.Id))
                    return false;
                else
                    throw; // todo make corresponding error code and avoid exceptions
            }
            return true;
        }

        public async Task<bool> Update(Series series)
        {
            try
            {
                series.UpdatedAt = _time.Now();
                _dbContext.Series.Update(series);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Titles.Any(e => e.Id == series.Id))
                    return false;
                else
                    throw; // todo make corresponding error code and avoid exceptions
            }
            return true;
        }

        public async Task<MediaTitle> Delete(long id)
        {
            var entity = await _dbContext.Titles.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.Titles.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<DataPage<MediaTitle>> List(MediaTitleSearchCriteria searchCriteria, int page, int pageSize)
        {
            var titles = _dbContext.Titles.AsQueryable();
            if(searchCriteria.Type.HasValue)
            {
                titles = titles.Where(t => t.Type == searchCriteria.Type.Value);
            }
            
            if (!string.IsNullOrEmpty(searchCriteria.Name))
            {
                titles = titles.Where(t => EF.Functions.ILike(t.Name, $"{searchCriteria.Name}%"));
            }
            titles = titles.OrderBy(t => t.Id);
            return await DataPage<MediaTitle>.CreateAsync(titles, page, pageSize);
        }
    }
}
