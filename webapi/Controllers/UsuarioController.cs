using Domain.Authorize;
using Domain.DTO;
using Domain.Models.Entities;
using Domain.SecurityHash;
using Domain.TokenGenerator;
using Infra.Interfaces.Repositories;
using Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace webapi.Controllers
{
    [Route("api/")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository) => _usuarioRepository = usuarioRepository;

        [HttpPost("v1/[controller]/Criar_Usuario")]
        public object Criar_Usuario(
         [FromBody] Novo_Usuario Novo_usuario,
         [FromServices]SigningConfigurations signingConfigurations,
         [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var user = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            Usuario usuario = new Usuario();
            try
            {
                var EmailExiste = _usuarioRepository.ValidarEmail(Novo_usuario.Email);
                if (!EmailExiste)
                {
                    usuario.Nome = Novo_usuario.Nome;
                    usuario.Senha = "123";
                    usuario.NovaSenha = "";
                    usuario.Email = Novo_usuario.Email;
                    usuario.PerfilID = Novo_usuario.PerfilID;
                    usuario.EmpresaId = user.EmpresaId;
                    usuario.Empresa = user.Empresa;
                    usuario.ativo = true;
                    usuario.Perfil = _usuarioRepository.Listar_Perfil(usuario.PerfilID);

                    var obj = new Usuario(usuario, new Hash(new SHA512Managed()).CriptografarSenha(usuario.Senha), TokenGenerator.GetToken(usuario, tokenConfigurations, signingConfigurations));
                    _usuarioRepository.Add(obj);
                    return Created("", new { statusCode = 201, message = "Dados gravados com sucesso.", senha = usuario.Senha });
                }
                else
                    return BadRequest(new { statusCode = 400, message = "Email já cadastrado." });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro." + e.Message });
            }

        }

        [HttpPut("v1/[controller]/Alterar_Usuario")]
        public object Alterar_Usuario(
        [FromBody] Novo_Usuario Novo_usuario,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var user = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança
            Usuario usuario = new Usuario();
            try
            {
                //verifica se o usuario que irá ser alterdo existe
                usuario = _usuarioRepository.GetById(Novo_usuario.ID);
                if (usuario == null)
                    return Ok(new { statusCode = 400, message = "Usuário não encontrado" });

                //verifica se está tentando colocar um email existente
                if (Novo_usuario.Email != usuario.Email)
                {
                    if (_usuarioRepository.ValidarEmail(Novo_usuario.Email))
                        return BadRequest(new { statusCode = 400, message = "O novo Email já está cadastrado." });
                }

                usuario.Nome = Novo_usuario.Nome;
                usuario.Email = Novo_usuario.Email;
                usuario.PerfilID = Novo_usuario.PerfilID;
                usuario.Perfil = _usuarioRepository.Listar_Perfil(usuario.PerfilID);
                usuario.Token = "";

                _usuarioRepository.Update(usuario);
                return Created("", new { statusCode = 201, message = "Usuário Alterado com sucesso." });
               
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro. " + e.Message });
            }
        }

        [HttpDelete("v1/[controller]/Remover_Usuario/{ID}")]
        public object Remover_Usuario(int ID)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _usuarioRepository.Remover_Usuario(ID);
        }

        [HttpGet("v1/[controller]/Resetar_Senha/{ID}")]
        public object Resetar_Senha(int ID)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            string ret = _usuarioRepository.Resetar_Senha(ID);
            if (string.IsNullOrWhiteSpace(ret))
                return false;
            else
                return ret;

        }

        [HttpGet("v1/[controller]/Listar_Perfis")]
        public object Listar_Perfis()
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _usuarioRepository.Listar_Perfis();
        }

        [HttpGet("v1/[controller]/Listar_Menus")]
        public object Listar_Menus()
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _usuarioRepository.Listar_Menus_Acesso();
        }

        [HttpGet("v1/[controller]/Listar_Menus/{IDPerfil}")]
        public object Listar_Menus(int IDPerfil)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _usuarioRepository.Listar_Menus_Acesso(IDPerfil);
        }

        [HttpGet("v1/[controller]/Pesquisar_Usuarios/{nome}/{perfil}")]
        public object Pesquisar_Usuarios(string nome, int perfil)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            if (perfil == 0)
                return _usuarioRepository.Pesquisar_Usuarios(usuario.EmpresaId, nome);
            else
                return _usuarioRepository.Pesquisar_Usuarios(usuario.EmpresaId, nome, perfil);
        }

        [HttpGet("v1/[controller]/Pesquisar_Usuarios")]
        public object Pesquisar_Usuarios()
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _usuarioRepository.Pesquisar_Usuarios(usuario.EmpresaId);
        }

    }



}