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

namespace Charity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterAdminController : ControllerBase
    {
        private const string ApiOperationBaseName = "ShelterAdmin";
        private readonly IMapper _mapper;
        private readonly IRepository<ShelterAdminEntity> _repository;

        public ShelterAdminController(IMapper mapper, IRepository<ShelterAdminEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ShelterAdminListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<ShelterAdminListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<ShelterAdminListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ShelterAdminDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<ShelterAdminDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] ShelterAdminDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<ShelterAdminEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/ShelterAdmin/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(ShelterAdminDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<ShelterAdminEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/ShelterAdmin/{result.Id}", result.Id);
        }

        [HttpDelete("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Delete))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid id)
        {
            _repository.Delete(id);

            return Ok();
        }
    }
}
