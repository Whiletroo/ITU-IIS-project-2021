//<!-- Author xkrukh00-->
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NSwag.Annotations;
using AutoMapper;
using Charity.Common.Models;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Charity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteeringController : ControllerBase
    {
        private const string ApiOperationBaseName = "Volunteering";
        private readonly IMapper _mapper;
        private readonly IRepository<VolunteeringEntity> _repository;

        public VolunteeringController(IMapper mapper, IRepository<VolunteeringEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VolunteeringListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<VolunteeringListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<VolunteeringListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VolunteeringDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<VolunteeringDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] VolunteeringDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<VolunteeringEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Volunteering/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(VolunteeringDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<VolunteeringEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Volunteering/{result.Id}", result.Id);
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
        public ActionResult<IEnumerable<VolunteeringListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<VolunteeringListModel>();

            foreach (var entity in entityList)
            {
                entity.Description ??= "";

                if (entity.Title.Contains(search) || entity.Description.Contains(search))
                {
                    resultList.Add(_mapper.Map<VolunteeringListModel>(entity));
                }
            }

            return resultList;
        }
    }
}
