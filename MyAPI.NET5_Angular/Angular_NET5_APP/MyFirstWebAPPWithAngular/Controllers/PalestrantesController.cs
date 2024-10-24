using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProEventos.Application.Contratos;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using ProEventos.Application.DTO;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using MyFirstWebAPPWithAngular.Extensions;
using Microsoft.AspNetCore.Authorization;
using ProEventos.Persistence.Models;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;

namespace MyFirstWebAPPWithAngular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        public IPalestranteService _palestrantesService { get; }
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public PalestrantesController(IPalestranteService eventosService, IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
            _palestrantesService = _palestrantesService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var palestrantes = await _palestrantesService.GetAllPalestrantesAsync( pageParams, true);
                if (palestrantes == null) return NoContent();

                Response.AddPagination(palestrantes.CurrentPage, palestrantes.PageSize, palestrantes.TotalCount, palestrantes.TotalPages);

                return Ok(palestrantes);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetPalestrantes(int id)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try
            {
                var palestrante = await _palestrantesService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrante == null) return NoContent();

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

       
        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDTO model)
        {
            try
            {
                var palestrante = await _palestrantesService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                //palestrante ??=, é o mesmo que dizer, if palestrante == null
                palestrante ??= await _palestrantesService.AddPalestrantes(User.GetUserId(), model);
                
                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar adicionar palestrante. Erro: {ex.Message}");
            }
        }

        [HttpPut()]
        public async Task<IActionResult> Put(PalestranteUpdateDTO model)
        {
            try
            {
                var evento = await _palestrantesService.UpdatePalestrante(User.GetUserId(), model);
                if (evento == null) return NotFound("Erro ao tentar alterar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }       
    }
}
