using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();

            var regionDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code
                });
            }

            return Ok(regionDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            //  var region= dbContext.Regions.Find(id);

            var region = dbContext.Regions.FirstOrDefault(region => region.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto

            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code
            };

            return Ok(regionDto);

        }

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            var regionDto = new Region
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDoaminModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDoaminModel == null)
            {
                return NotFound();
            }

            regionDoaminModel.Name = updateRegionRequestDto.Name;
            regionDoaminModel.Code = updateRegionRequestDto.Code;
            regionDoaminModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges();

            var regionDto = new Region
            {
                Id = regionDoaminModel.Id,
                Code = regionDoaminModel.Code,
                Name = regionDoaminModel.Name,
                RegionImageUrl = regionDoaminModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel= dbContext.Regions.FirstOrDefault(x=>x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

            return Ok();
        }

    }
}
