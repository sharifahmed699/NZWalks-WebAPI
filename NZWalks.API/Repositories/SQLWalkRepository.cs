using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk=await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null)
        {
            var walk= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if(string.IsNullOrWhiteSpace(FilterOn)==false && string.IsNullOrWhiteSpace(FilterQuery)==false)
            {
                if (FilterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk = walk.Where(x => x.Name.Contains(FilterQuery));
                }
            }
            return await walk.ToListAsync();
           // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync(); 
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingModel=await dbContext.Walks.FirstOrDefaultAsync(x=>x.Id==id);
            if (existingModel ==null)
            {
                return null;
            }

            existingModel.Name=walk.Name;
            existingModel.Description=walk.Description;
            existingModel.LengthInKm = walk.LengthInKm;
            existingModel.WalkImageUrl=walk.WalkImageUrl;
            existingModel.RegionId=walk.RegionId;
            existingModel.DifficultyId=walk.DifficultyId;

            await dbContext.SaveChangesAsync();
            return existingModel;
        }
    }
}
