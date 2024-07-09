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

namespace MyFirstWebAPPWithAngular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        public IEventoService _eventosService { get; }
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public EventosController(IEventoService eventosService , IWebHostEnvironment hostEnvironment, IAccountService accountService)
        {
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
            _eventosService = eventosService;
        }

       [HttpGet]
public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
{
    try
    {
        var eventos = await _eventosService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
        if (eventos == null) return NoContent();

        Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.TotalPages);

        return Ok(eventos);
    }
    catch (Exception ex)
    {
        return this.StatusCode(StatusCodes.Status500InternalServerError,
            $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
    }
}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)//posso colocar qualquer nome no metodo, o que manda nesse caso é o atributo do controller [HttpGet]
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image/{eventoId}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), eventoId, true);
                if (evento == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    DeleteImage(evento.ImagemURL);
                    evento.ImagemURL = await SaveImage(file);

                }

                var eventoRetorno = await _eventosService.UpdateEvento(User.GetUserId(), eventoId, evento);

                return Ok(eventoRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDTO model)
        {
            try
            {
                var evento = await _eventosService.AddEventos(User.GetUserId(), model);
                if (evento == null) return NoContent();

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDTO model)
        {
            try
            {
                var evento = await _eventosService.UpdateEvento(User.GetUserId(), id, model);
                if (evento == null) return NotFound("Erro ao tentar alterar o evento");

                return Ok(evento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventosService.GetEventoByIdAsync(User.GetUserId(), id, true);
                if (evento == null) return NoContent();

                if( await _eventosService.DeleteEvento(User.GetUserId(), id)){
                    DeleteImage(evento.ImagemURL);
                    return Ok(new { message ="Deletado"});
                }
                else
                {
                    throw new Exception("Ocorreu um problema não especifico ao tentar deletar o evento");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                 $"Erro ao tentar deletar o evento. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension
                (imageFile.FileName)
                .Take(10)
                .ToArray())
                .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);
            // Combina o diretório raiz do projeto com o diretório de imagens
            // e o nome da imagem para obter o caminho completo do arquivo
            // Isso é necessário pois o método IFormFile.CopyToAsync() precisa
            // de um objeto Stream para salvar o arquivo, e não pode ser salvo
            // diretamente no disco sem o caminho completo
            // Exemplo do caminho: C:\Users\Rogerio\Desktop\DotnetWithAngular\API_RestFull_.NET5_Angular\MyAPI.NET5_Angular\Angular_NET5_APP\MyFirstWebAPPWithAngular\Resources\images\evento-teste46771142.jpg

            using(var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        [NonAction]
        public void DeleteImage(string imagemURL)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imagemURL);

            if (System.IO.File.Exists(imagePath)) System.IO.File.Delete(imagePath);
        }
    }
}
