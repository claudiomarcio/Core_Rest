
using Domain.Authorize;
using Domain.Models.Entities;
using Infra.Interfaces.Repositories;
using Infra.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;

namespace webapi.Controllers
{
    public  class TokenController : Controller
    {

        private readonly IUsuarioRepository _usuarioRepositoryInterface;

        public TokenController(IUsuarioRepository usuarioRepository) => _usuarioRepositoryInterface = usuarioRepository;

        Usuario usuario;
        public IActionResult ValidarToken(dynamic token)
        {
            var tokenCapturado = TokenCapture.CapturarToken(token);
            usuario = _usuarioRepositoryInterface.BuscarUsuarioPeloToken(tokenCapturado);
            if (usuario != null)
            {
                if (tokenCapturado != usuario.Token || string.IsNullOrEmpty(tokenCapturado))
                    return NoAuthorize.DenyAccess();

                if (tokenCapturado == usuario.Token)
                {
                    //17/03/2020 - Controla o Token do usuário - TIMEOUT (era 30min, Dias pediu para por 60min)
                    var sessaosUsuario = (DateTime.Now.ToLocalTime() - usuario.DataAtualizacao.ToLocalTime()).Minutes;
                    if (sessaosUsuario > 60)
                        return NoAuthorize.LogOut();
                }
            }
            else
            {
                return NoAuthorize.DenyAccess();
            }
            usuario.DataAtualizacao = DateTime.Now;
            _usuarioRepositoryInterface.Update(usuario);
            return Ok(usuario);

        }

        public Usuario BuscarDadosUsuario(dynamic token)
        {
            var tokenCapturado = TokenCapture.CapturarToken(token);
            Usuario usuario = _usuarioRepositoryInterface.BuscarUsuarioPeloToken(tokenCapturado);
            return usuario;

        }
    }
}
