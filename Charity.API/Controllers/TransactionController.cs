//<!-- Author xpimen00-->
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
    public class TransactionController : ControllerBase
    {
        private const string ApiOperationBaseName = "Transaction";
        private readonly IMapper _mapper;
        private readonly IRepository<TransactionEntity> _repository;
        private readonly IRepository<DonationEntity> _donationRepository;

        public TransactionController(IMapper mapper, IRepository<TransactionEntity> repository, IRepository<DonationEntity> donationRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _donationRepository = donationRepository;
        }

        [HttpGet]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<TransactionListModel>> Get()
        {
            var entityList = _repository.GetAll();
            var result = new List<TransactionListModel>();

            foreach (var entity in entityList)
            {
                result.Add(_mapper.Map<TransactionListModel>(entity));
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [OpenApiOperation(ApiOperationBaseName + nameof(Get))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TransactionDetailModel> Get(Guid id)
        {
            var result = _repository.Get(id);

            if (result is null) return NotFound();

            return Ok(_mapper.Map<TransactionDetailModel>(result));
        }

        [HttpPost]
        [OpenApiOperation(ApiOperationBaseName + nameof(Create))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Create([FromBody] TransactionDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Insert(_mapper.Map<TransactionEntity>(model));
            
            var donation = _donationRepository.Get(model.DonationId);
            if (donation.State == null)
                donation.State = 0;
            donation.State += model.Sum;
            _donationRepository.Update(donation);

            if (result is null) return BadRequest();

            return Created($"api/Transaction/{result.Id}", result.Id);
        }

        [HttpPut]
        [OpenApiOperation(ApiOperationBaseName + nameof(Update))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Guid> Update(TransactionDetailModel model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = _repository.Update(_mapper.Map<TransactionEntity>(model));

            if (result is null) return NotFound();

            return Created($"api/Transaction/{result.Id}", result.Id);
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
