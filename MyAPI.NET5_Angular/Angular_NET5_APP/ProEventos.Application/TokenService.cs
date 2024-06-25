using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;

namespace ProEventos.Application
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config, UserManager<User> usermanager, IMapper mapper)
        {
            _config = config;
            _userManager = usermanager;
            _mapper = mapper;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        }

        public async Task<string> CreateToken(UserUpdateDTO userUpdateDto)
        {
            // Mapeia os dados do usuário para o objeto User
            var user = _mapper.Map<User>(userUpdateDto);

            // Cria uma lista de reivindicações (claims) para o token
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            // Adiciona as roles do usuário à lista de claims
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Cria as credenciais de assinatura para o token
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Descreve o token como um SecurityTokenDescriptor
            // Aqui estamos configurando o token com as informações necessárias para sua criação:
            // Subject: define as reivindicações (claims) que o token conterá
            // Expires: define quando o token irá expirar (neste caso, em 1 dia a partir do momento atual)
            // SigningCredentials: define as credenciais de assinatura para o token
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Cria o token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);

            // Retorna o token em formato de string
            return tokenHandler.WriteToken(token);
        }
    }
}