using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ProEventos.MyFirstWebAPPWithAngular.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public async Task<string> SaveImage(IFormFile imageFile, string destino)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                              .Take(10)
                                              .ToArray()
                                         ).Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{destino}", imageName);
             // Combina o diretório raiz do projeto com o diretório de imagens
            // e o nome da imagem para obter o caminho completo do arquivo
            // Isso é necessário pois o método IFormFile.CopyToAsync() precisa
            // de um objeto Stream para salvar o arquivo, e não pode ser salvo
            // diretamente no disco sem o caminho completo
            // Exemplo do caminho: C:\Users\Rogerio\Desktop\DotnetWithAngular\API_RestFull_.NET5_Angular\MyAPI.NET5_Angular\Angular_NET5_APP\MyFirstWebAPPWithAngular\Resources\images\evento-teste46771142.jpg

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        public void DeleteImage(string imageName, string destino)
        {
            if (!string.IsNullOrEmpty(imageName)) 
            {
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }
        }
    }
}