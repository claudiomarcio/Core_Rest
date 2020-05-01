using Domain.DTO;
using Domain.Models.Entities;
using Domain.EntityConfiguration;
using Domain.Interfaces.Repositories;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace webapi.Controllers
{
    [Route("api/")]
    public  class GrupoEmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IGrupoEmpresaRepository _grupoEmpresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public GrupoEmpresasController(IUsuarioRepository usuarioRepository, IEmpresaRepository empresaRepository, IGrupoEmpresaRepository grupoEmpresaRepository)
        {
            _empresaRepository = empresaRepository;
            _grupoEmpresaRepository = grupoEmpresaRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet("v1/[controller]/Lista_Grupo_Empresa/{ID}")]
        public object Listar_Empresas(int ID)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
          
            #endregion segurança

            if(ID == 0)
               return _empresaRepository.GetEmpresasDoGrupo();
            else
               return _empresaRepository.GetEmpresasDoGrupo(ID);
        }

        [HttpPost("v1/[controller]/Criar_Grupo")]
        public object Criar_Grupo(
        [FromBody] GrupoEmpresa grupoEmpresaBody,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));
        
            #endregion segurança

            GrupoEmpresa grupo = new GrupoEmpresa();
            try
            {
                var grupoExist = _grupoEmpresaRepository.CheckGrupoEmpresa(grupoEmpresaBody.EmpresaId,grupoEmpresaBody.SistemaId);             
                
                if (grupoExist == null)
                {
                    _grupoEmpresaRepository.Add(grupoEmpresaBody);
                    return Created("", new { statusCode = 201, message = "Dados gravados com sucesso." });
                }
                else
                    return BadRequest(new { statusCode = 400, message = "Empresa já associada a um grupo." });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro." + e.Message });
            }

        }
            
        [HttpPut("v1/[controller]/Alterar_Grupo")]
        public object Alterar_Grupo(
        [FromBody] GrupoEmpresa grupoEmpresaBody,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));

            #endregion segurança

            GrupoEmpresa grupo = new GrupoEmpresa();
            try
            {
                var grupoExist = _grupoEmpresaRepository.CheckGrupoEmpresa(grupoEmpresaBody.EmpresaId, grupoEmpresaBody.SistemaId);

                if (grupoExist != null)
                {
                    grupoExist.MatrizIdExterno = grupoEmpresaBody.MatrizIdExterno;
                    grupoExist.SistemaId = grupoEmpresaBody.SistemaId;

                    _grupoEmpresaRepository.Update(grupoExist);
                    return Created("", new { statusCode = 201, message = "Dados gravados com sucesso." });
                }
                else
                    return BadRequest(new { statusCode = 400, message = "Empresa não está em um grupo." });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro." + e.Message });
            }

        }

        [HttpDelete("v1/[controller]/Remover_Grupo/{ID}")]
        public object Remover_Grupo(
        int ID,
        [FromServices]SigningConfigurations signingConfigurations,
        [FromServices]TokenConfiguration tokenConfigurations)
        {
            #region segurança
            var token = ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext)).StatusCode;
            if (token == 401 || token == 440)
                return ((ObjectResult)new TokenController(_usuarioRepository).ValidarToken(HttpContext));

            #endregion segurança

            GrupoEmpresa grupo = new GrupoEmpresa();
            try
            {
                var grupoExist = _grupoEmpresaRepository.GetById(ID);

                if (grupoExist != null)
                {                 
                    _grupoEmpresaRepository.Remove(grupoExist);
                    return Created("", new { statusCode = 201, message = "Dados gravados com sucesso." });
                }
                else
                    return BadRequest(new { statusCode = 400, message = "Empresa não está em um grupo." });
            }
            catch (System.Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = 500, message = "Ocorreu um erro." + e.Message });
            }

        }

    }
}


