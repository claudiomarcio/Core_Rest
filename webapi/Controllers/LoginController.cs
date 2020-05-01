
using Domain.Authorize;
using Domain.DTO;
using Domain.Models.Entities;
using Domain.SecurityHash;
using Domain.TokenGenerator;
using Domain.Interfaces.Repositories;
using Domain.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace webapi.Controllers

{
    [Route("api/v1/[controller]")]
    public class LoginController : Controller
    {
  
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public object Post([FromBody]LoginDTO usuario,            
            [FromServices]SigningConfigurations signingConfigurations,
            [FromServices]TokenConfiguration tokenConfigurations)
        {
            Usuario usuarioBase;
            UsuarioDTO usuarioBaseDTO;
            List<Menus_Acesso> Lista_Menus = new List<Menus_Acesso>();
 
            if (!string.IsNullOrEmpty(usuario.Email) && !string.IsNullOrEmpty(usuario.Senha))
            {
                var senhaCriptografada = string.Empty;
                senhaCriptografada = new Hash(new SHA512Managed()).CriptografarSenha(usuario.Senha);
                usuario.Senha = senhaCriptografada;

                usuarioBaseDTO = _usuarioRepository.GetUserByEmail(usuario.Email);
                if (usuarioBaseDTO == null)
                    return NoAuthorize.DenyAccessLogin();

                usuarioBase = usuarioBaseDTO.Usuario;
                Lista_Menus = _usuarioRepository.Listar_Menus_Acesso(usuarioBaseDTO.Perfil.ID);

                //verifica se o usuario está ativo
                if (!usuarioBase.ativo)
                    return NoAuthorize.DenyAccessLogin();


                AtualizarDadosUsuario(ref usuarioBaseDTO, usuarioBase, signingConfigurations, tokenConfigurations);

                if (usuarioBase == null)
                    return NoAuthorize.DenyAccessLogin();
                if (usuario.Email == usuarioBase.Email && senhaCriptografada != usuarioBase.Senha)
                    return NoAuthorize.DenyAccessLogin();
            }
            else
                return NoAuthorize.DenyAccessLogin();

            var Ret = new Object[] {
                new {
                        DadosUsuario = usuarioBase
                        , Menus =Lista_Menus 
                    }
                };

            return Ok(Ret[0]);
        }
        private void AtualizarDadosUsuario(ref UsuarioDTO usuarioBaseDTO, Usuario usuarioBase, SigningConfigurations signingConfigurations, TokenConfiguration tokenConfigurations)
        {           
            usuarioBase.Token = TokenGenerator.GetToken(usuarioBase, tokenConfigurations, signingConfigurations);
            usuarioBaseDTO.Token = usuarioBase.Token;
            usuarioBase.DataUltimoLogin = DateTime.Now.ToLocalTime();
            usuarioBase.DataAtualizacao = DateTime.Now.ToLocalTime();
            _usuarioRepository.Update(usuarioBase); 
        }
    }
}