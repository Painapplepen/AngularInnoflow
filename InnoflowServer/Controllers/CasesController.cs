using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InnoflowServer.Domain.Core.Entities;
using InnoflowServer.Domain.Core.Models;
using InnoflowServer.Services.Interfaces.Cases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoflowServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly ICaseService _service;
        private readonly IMapper _mapper;
        public CasesController(ICaseService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
       
        [Route("/GetAssociatedCases")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCasesAsync(string email)
        {
            return Ok(await _service.GetAllCasesAsync(email));
        }

        [Route("/CreateCase")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCasesAsync([FromBody] CreateCaseModel caseTemplate)
        {
            await _service.CreateAsync(_mapper.Map<Case>(caseTemplate));
            return Ok();
        }

        [Route("/AcceptCase")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AcceptCaseAsync(int id, string userId)
        {
            if (await _service.UpdateAsync(id, userId))
                return Ok();
            return BadRequest();
        }
    }
}
