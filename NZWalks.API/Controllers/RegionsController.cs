﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.getAllAsync();

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
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //  var region= dbContext.Regions.Find(id);

            var region =await regionRepository.getByIdAsync(id);

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
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            regionDomainModel=  await regionRepository.CreateAsync(regionDomainModel);

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
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDoaminModel = new Region { 
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
            };
          regionDoaminModel=  await regionRepository.UpdateAsync(id,
                regionDoaminModel);

            if (regionDoaminModel == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel= await regionRepository.DeteleAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = new Region
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

    }
}
