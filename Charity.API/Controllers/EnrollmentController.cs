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
    public class EnrollmentController : ControllerBase
    {
        private const string ApiOperationBaseName = "Enrollment";
        private readonly IMapper _mapper;
        private readonly IRepository<EnrollmentEntity> _repository;

        public EnrollmentController(IMapper mapper, IRepository<EnrollmentEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<EnrollmentListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<EnrollmentListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<EnrollmentListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<EnrollmentDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<EnrollmentDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] EnrollmentDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<EnrollmentEntity>(model));

            if (result is null) return BadRequest();

            return Created($"api/Enrollment/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(EnrollmentDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<EnrollmentEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Enrollment/{result.Id}", result.Id);
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
