//< !--Author xpimen00-- >
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using AutoMapper;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Charity.Common.Models;

namespace Charity.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private const string ApiOperationBaseName = "Volunteer";
        private readonly IMapper _mapper;
        private readonly IRepository<VolunteerEntity> _repository;

        public VolunteerController(IMapper mapper, IRepository<VolunteerEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VolunteerListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<VolunteerListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<VolunteerListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VolunteerDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<VolunteerDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] VolunteerDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<VolunteerEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Volunteer/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(VolunteerDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<VolunteerEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Volunteer/{result.Id}", result.Id);
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }

        [HttpGet("search/")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Search))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<VolunteerListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<VolunteerListModel>();

            foreach (var entity in entityList)
            {
                if (entity.FirstName.Contains(search) || entity.LastName.Contains(search) || entity.Email.Contains(search))
                {
                    resultList.Add(_mapper.Map<VolunteerListModel>(entity));
                }
            }

            return resultList;
        }
    }
    
}
