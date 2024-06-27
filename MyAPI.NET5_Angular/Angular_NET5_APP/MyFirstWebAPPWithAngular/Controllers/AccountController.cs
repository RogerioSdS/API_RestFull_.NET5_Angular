using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPPWithAngular.Extensions;
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

        [HttpGet("GetUser")]
        /// Permite que essa rota seja acessada por qualquer usuário, sem a necessidade de estar autenticado.
        /// </summary>
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar o usuario. Erro: {ex.Message}");
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
                if(user.Exception is null)
                {
                    return Ok(new
                    {
                        UserName = userDTO.Username,
                        PrimeiroNome = userDTO.PrimeiroNome
                    });
                }

                return BadRequest($"{user.Exception.Message}");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao registrar o usuario. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        /// <summary>
        [AllowAnonymous]
        /// Permite que essa rota seja acessada por qualquer usuário, sem a necessidade de estar autenticado.
        /// </summary>
        public async Task<IActionResult> Login(UserLoginDTO UserLogin)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(UserLogin.Username);

                if(user == null) return Unauthorized("usuário ou senha incorretos.");

                var result = await _accountService.CheckUserPasswordAsync(user, UserLogin.Password);

                if(!result.Succeeded) return Unauthorized("Erro ao tentar logar.");

                return Ok(
                    new
                        {
                            userName = user.Username,
                            PrimeiroNome = user.PrimeiroNome,
                            token = _tokenService.CreateToken(user).Result
                        }
                );
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar realizar o login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpadateDTO)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if(user == null) return Unauthorized("Usuário ou senha Invalido.");

                var userReturn =await _accountService.UpdateAccount(userUpadateDTO);

               if(userReturn == null) return NoContent();
               
               return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar atualizar usuario. Erro: {ex.Message}");
            }
        }
    }

}