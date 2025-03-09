using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.getAllAsync();

          var regionDto = mapper.Map<List<RegionDto>>(regions);

            return Ok(regionDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //  var region= dbContext.Regions.Find(id);

            var region =await regionRepository.getByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(region));

        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
             var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

             regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

             var regionDto = mapper.Map<RegionDto>(regionDomainModel);
             return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);  

        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
          
                var regionDoaminModel = mapper.Map<Region>(updateRegionRequestDto);
                regionDoaminModel = await regionRepository.UpdateAsync(id,
                      regionDoaminModel);

                if (regionDoaminModel == null)
                {
                    return NotFound();
                }

                var regionDto = mapper.Map<RegionDto>(regionDoaminModel);

                return Ok(regionDto);
            
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel= await regionRepository.DeteleAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
