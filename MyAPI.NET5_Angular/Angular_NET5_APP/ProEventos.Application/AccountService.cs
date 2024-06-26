using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager , IMapper mapper, IUserPersist userPersist)
        {
          _userPersist = userPersist;
          _userManager = userManager;
          _signInManager = signInManager;
          _mapper = mapper;            
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password)
        {
            try
            {
                var user = await _userManager.Users
                           .SingleOrDefaultAsync(user => user.UserName == userUpdateDTO.Username);
                            //verificar se o usuario existe.

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (System.Exception ex)
            {                
                throw new Exception($"Erro ao tentar verificar password. Erro:{ex.Message} ");
            }
        }

        public async Task<UserDTO> CreateAccountAsync(UserDTO userDTO)
        {
             try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserDTO>(user);

                    return userToReturn;
                }

                throw new Exception(result.Errors.FirstOrDefault()?.Description);
                
                
            }
            catch (System.Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string username)
        {
             try
            {
                var user = await _userPersist.GetUserByUserNameAsync(username);

                if (user == null) return null;

                var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);

                return userUpdateDTO;
            }
            catch (System.Exception ex)
            {                
                throw new Exception($"Erro ao tentar selecionar usuário. Erro:{ex.Message} ");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
             try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDTO.Username);

                if (user == null) return null; 
                
                _mapper.Map(userUpdateDTO, user);       

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);  

                _userPersist.Update<User>(user);

                if(await _userPersist.SaveChangesAsync())
                {
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(userUpdateDTO.Username);

                    return _mapper.Map<UserUpdateDTO>(userRetorno);
                }

                return null;
            }
            catch (System.Exception ex)
            {                
                throw new Exception($"Erro ao tentar atualizar o usuario. Erro:{ex.Message} ");
            }
        }

        public async Task<bool> UserExists(string username)
        {
             try
            {
                return await _userManager.Users
                .AnyAsync(user => user.UserName == username.ToLower());
            }
            catch (System.Exception ex)
            {                
                throw new Exception($"Erro aoverificar se o usuario existe. Erro:{ex.Message} ");
            }
        }
    }
}