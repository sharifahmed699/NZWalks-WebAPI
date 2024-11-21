using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {

        Task<List<Region>> getAllAsync();
        Task<Region?> getByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id,Region region);
        Task<Region?> DeteleAsync(Guid id);
    }
}
