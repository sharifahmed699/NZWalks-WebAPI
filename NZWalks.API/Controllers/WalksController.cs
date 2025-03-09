using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        public async Task<IActionResult> create([FromBody] AddWalkRequestDto dto )
        {
            var model = mapper.Map<Walk>(dto);
            await walkRepository.CreateAsync(model);

            return Ok(mapper.Map<WalkDto>(model));
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var model=await  walkRepository.GetAllAsync();
            return Ok(mapper.Map<List<WalkDto>>(model));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var model= await walkRepository.GetByIdAsync(id);
            if(model == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(model));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto dto)
        {

            var walkModel = mapper.Map<Walk>(dto);
            walkModel=await walkRepository.UpdateAsync(id, walkModel);

            if(walkModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDto>(walkModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var model= await walkRepository.DeleteAsync(id);

            if(model == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(model));

        }
    }
}
