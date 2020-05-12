using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Models;
using MiniMdb.Backend.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace MiniMdb.Backend.Services
{
    public interface IMediaTitlesService
    {
        Task<long> Add(Movie movie);
        Task<long> Add(Series series);

        Task<MediaTitle> Get(long id);

        Task<bool> Update(Movie movie);
        Task<bool> Update(Series series);

        Task<bool> Delete(long id);

        Task<DataPage<MediaTitle>> List(int page, int pageSize);
    }

    public class MediaTitlesService : IMediaTitlesService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<MediaTitlesService> _logger;

        public MediaTitlesService(AppDbContext dbContext, ILogger<MediaTitlesService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<long> Add(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
            _logger.LogTrace("Saved movie: {@m}", movie);
            return movie.Id;
        }

        public async Task<long> Add(Series series)
        {
            _dbContext.Series.Add(series);
            await _dbContext.SaveChangesAsync();
            _logger.LogTrace("Saved series: {@m}", series);
            return series.Id;
        }

        public async Task<MediaTitle> Get(long id)
        {
            return await _dbContext.Titles.FindAsync(id); ;
        }

        // todo read and update
        public async Task<bool> Update(Movie movie)
        {
            _dbContext.Movies.Update(movie);
            _dbContext.Entry(movie).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Titles.Any(e => e.Id == movie.Id))
                    return false;
                else
                    throw;
            }
            return true;
        }

        // todo read and update
        public async Task<bool> Update(Series series)
        {
            _dbContext.Series.Update(series);
            _dbContext.Entry(series).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Titles.Any(e => e.Id == series.Id))
                    return false;
                else
                    throw;
            }
            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var entity = await _dbContext.Titles.FindAsync(id);
            if (entity == null)
                return false;

            _dbContext.Titles.Remove(entity);
            await _dbContext.SaveChangesAsync();
            _logger.LogTrace("Deleted title: {@m}", entity);
            return true;
        }

        public async Task<DataPage<MediaTitle>> List(int page, int pageSize)
        {
            // TODO validation of parameters
            return await DataPage<MediaTitle>.CreateAsync(_dbContext.Titles, page, pageSize);
        }
    }
}
