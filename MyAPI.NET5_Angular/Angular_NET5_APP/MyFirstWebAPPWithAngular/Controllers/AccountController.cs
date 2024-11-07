using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebAPPWithAngular.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.MyFirstWebAPPWithAngular.Helpers;

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
        private readonly IUtil _util;
        private readonly string _destino = "Perfil"; 

        public AccountController(IAccountService accountService, ITokenService tokenService, IMapper mapper, IUtil util)
        {
            _util = util;
            _accountService = accountService;
            _tokenService = tokenService;
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

                var user = await _accountService.CreateAccountAsync(userDTO);
                if(user != null)
                {
                    return Ok(new
                    {
                        userName = user.Username,
                        primeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    });
                }

                return BadRequest($"usuario não criado. Tente novamente mais tarde");
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
                if (userUpadateDTO.Username != User.GetUserName()) return Unauthorized();

                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if(user == null) return Unauthorized("Usuário ou senha Invalido.");

                var userReturn =await _accountService.UpdateAccount(userUpadateDTO);

               if(userReturn == null) return NoContent();
               
               return Ok(
                    new
                        {
                            userName = userReturn.Username,
                            primeiroNome = userReturn.PrimeiroNome,
                            token = _tokenService.CreateToken(userReturn).Result
                        });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar atualizar usuario. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemURL, _destino);
                    user.ImagemURL = await _util.SaveImage(file, _destino);

                }

                var userRetorno = await _accountService.UpdateAccount(user);

                return Ok(userRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar realizar o upload da foto do usuario. Erro: {ex.Message}");
            }
        }
    }
}