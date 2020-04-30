using Domain.Models.Entities;
using Newtonsoft.Json;
using System;

namespace Domain.DTO
{
    public class UsuarioDTO 
    {
        public UsuarioDTO() { }

        public UsuarioDTO(Usuario usuarioBase, int sistemaId, int empresaIdExterno)
        {
            Nome = usuarioBase.Nome;
            Email = usuarioBase.Email;
            UsuarioId = usuarioBase.UsuarioId;
            EmpresaId = usuarioBase.EmpresaId;
            SistemaId = sistemaId;
            EmpresaIdExterno = empresaIdExterno;
            Token = usuarioBase.Token;
            Perfil = usuarioBase.Perfil;
            Ativo = usuarioBase.ativo;
        }

        public string Nome { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public int UsuarioId { get; set; }
        [JsonIgnore]
        public int EmpresaId { get; set; }
        [JsonIgnore]
        public int SistemaId { get; set; }
        [JsonIgnore]
        public int EmpresaIdExterno { get; set; }
        
        public Perfil Perfil { get; set; }
        [JsonIgnore]
        public bool Ativo { get; set; }

        public string Token { get; set; }
        [JsonIgnore]
        public Usuario Usuario { get; set; }
    }
}
