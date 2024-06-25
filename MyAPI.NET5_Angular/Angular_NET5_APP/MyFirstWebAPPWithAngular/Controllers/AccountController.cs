using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, ITokenService tokenService, IMapper mapper)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpGet("GetUser/{userName}")]
        /// <summary>
        [AllowAnonymous]
        /// Permite que essa rota seja acessada por qualquer usuário, sem a necessidade de estar autenticado.
        /// </summary>
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

         [HttpPost("Register")]
        /// <summary>
        [AllowAnonymous]
        /// Permite que essa rota seja acessada por qualquer usuário, sem a necessidade de estar autenticado.
        /// </summary>
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                if(await _accountService.UserExists(userDTO.Username))
                {
                    return BadRequest("Ja existe um usuario com este nome.");
                }

                var user = _accountService.CreateAccountAsync(userDTO);
                if(user != null)
                {
                    return Ok(user);
                }

                return BadRequest("Erro ao tentar registrar o usuario.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }
    }

}