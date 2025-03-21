﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Walk>> GetAllAsync(string? FilterOn = null, string? FilterQuery = null,
             string? SortBy = null, bool IsAscending = true, int PageNumber = 1, int PageSize = 1000)
        {
            var walk= dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if(string.IsNullOrWhiteSpace(FilterOn)==false && string.IsNullOrWhiteSpace(FilterQuery)==false)
            {
                if (FilterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk = walk.Where(x => x.Name.Contains(FilterQuery));
                }
            }
            if (string.IsNullOrWhiteSpace(SortBy) == false)
            {
                if (SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk = IsAscending ? walk.OrderBy(x => x.Name) : walk.OrderByDescending(x => x.Name);
                }
            }else if (string.IsNullOrWhiteSpace(SortBy) == false)
            {
                if (SortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walk = IsAscending ? walk.OrderBy(x => x.LengthInKm) : walk.OrderByDescending(x => x.LengthInKm);
                }
            }

            var skipResult = (PageNumber - 1) * PageSize;
            return await walk.Skip(skipResult).Take(PageSize).ToListAsync();

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
