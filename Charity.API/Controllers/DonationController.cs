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
    public class DonationController : ControllerBase
    {
        private const string ApiOperationBaseName = "Donation";
        private readonly IMapper _mapper;
        private readonly IRepository<DonationEntity> _repository;

        public DonationController(IMapper mapper, IRepository<DonationEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DonationListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<DonationListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<DonationListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DonationDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<DonationDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] DonationDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<DonationEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Donation/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(DonationDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<DonationEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Donation/{result.Id}", result.Id);
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
        public ActionResult<IEnumerable<DonationListModel>> Search([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search)) return BadRequest();

            var entityList = _repository.GetAll();
            var resultList = new List<DonationListModel>();

            foreach (var entity in entityList)
            {
                entity.Description ??= "";

                if (entity.Title.Contains(search) || entity.Description.Contains(search))
                {
                    resultList.Add(_mapper.Map<DonationListModel>(entity));
                }
            }

            return resultList;
        }
    }
}
