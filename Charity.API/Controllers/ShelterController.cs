/*
 *  File:   ShelterController.cs
 *  Author: Oleksandr Prokofiev (xproko40)
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Charity.Common.Models;
using Charity.DAL.Entities;
using Charity.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Charity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelterController : ControllerBase
    {

        private const string ApiOperationBaseName = "Shelter";
        private readonly IMapper _mapper;
        private readonly IRepository<ShelterEntity> _repository;

        public ShelterController(IMapper mapper, IRepository<ShelterEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ShelterListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<ShelterListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<ShelterListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ShelterDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<ShelterDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] ShelterDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<ShelterEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Shelter/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(ShelterDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<ShelterEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Shelter/{result.Id}", result.Id);
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
        public ActionResult<IEnumerable<ShelterListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<ShelterListModel>();

            foreach (var entity in entityList)
            {
                entity.Description ??= "";

                if (entity.Title.Contains(search) || entity.Description.Contains(search))
                {
                    resultList.Add(_mapper.Map<ShelterListModel>(entity));
                }
            }

            return resultList;
        }
    }
}
