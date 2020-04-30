using Domain.Models.Entities;
using Infra.Interfaces.Repositories;
using Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace webapi.Controllers
{
    [Route("api/")]
    public class EmpresaController : Controller
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IGrupoEmpresaRepository _grupoEmpresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public EmpresaController(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, IGrupoEmpresaRepository grupoEmpresaRepository)
        {
            _empresaRepository = empresaRepository;
            _grupoEmpresaRepository = grupoEmpresaRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost("v1/[controller]/Criar_Empresa")]
        public object Criar_Empresa(
        [FromBody] Empresa empresaBody,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var user = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            Empresa empresa = new Empresa();
            try
            {
                var empresaExist = _empresaRepository.CheckEmpresa(empresaBody.Nome);
                if (empresaExist == null)
                {
                    empresa = empresaBody;
                    empresa.Ativo = true;
                    empresa.SistemaId = 3; //Sistema Ebag
                    _empresaRepository.Add(empresa);
                    return Created("", new { statusCode = 201, message = "Dados gravados com sucesso."});
                }
                else
                    return BadRequest(new { statusCode = 400, message = "Email já cadastrado." });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro." + e.Message });
            }

        }

        [HttpPut("v1/[controller]/Alterar_Empresa")]
        public object Alterar_Empresa(
        [FromBody] Empresa empresaBody,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var user = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança
            Empresa empresa = new Empresa();
            try
            {
                //verifica se o usuario que irá ser alterdo existe
                empresa = _empresaRepository.CheckEmpresa(empresaBody.Nome);
                if (empresa != null && empresa.Nome != null)
                    return Ok(new { statusCode = 400, message = "Empresa já existe" });

                empresa = _empresaRepository.GetById(empresaBody.EmpresaId);
                empresa.Nome = empresaBody.Nome;

                _empresaRepository.Update(empresa);
                return Created("", new { statusCode = 201, message = "Empresa Alterado com sucesso." });

            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro. " + e.Message });
            }
        }

        [HttpDelete("v1/[controller]/Remover_Empresa/{ID}")]
        public object Remover_Empresa(int ID)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));       
            #endregion segurança

            return _empresaRepository.RemoverEmpresa(ID);
        }

        [HttpGet("v1/[controller]/Listar_Empresas")]
        public object Listar_Empresas()
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _empresaRepository.GetAll();
        }

        [HttpGet("v1/[controller]/Listar_Empresas/{ID}")]
        public object Listar_Empresas(int ID)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
            var usuario = (new TokenController(_usuarioRepository).BuscarDadosUsuario(HttpContext));
            #endregion segurança

            return _empresaRepository.GetById(ID);
        }
    }
}


