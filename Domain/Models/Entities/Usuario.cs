using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Novo_Usuario
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int PerfilID { get; set; }
    }

    public class Usuario
    {
        public Usuario() {}
        public Usuario(Usuario usuario, string senhaCriptografada, string token)
        {
            Nome = usuario.Nome;
            Senha = senhaCriptografada;
            Email = usuario.Email;
            Token = token;
            DataCriacao = AddDataCriacao();
            DataAtualizacao = AddDataAtualizacao();
            DataUltimoLogin = AddDataUltimoLogin();
            EmpresaId = usuario.EmpresaId;
            Perfil = usuario.Perfil;
            ativo = usuario.ativo;
        }

        public DateTime AddDataUltimoLogin() => DataUltimoLogin = DateTime.Now.ToLocalTime();
        public DateTime AddDataAtualizacao() => DataAtualizacao = DateTime.Now.ToLocalTime();
        public DateTime AddDataCriacao() => DataCriacao = DateTime.Now.ToLocalTime();

        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Senha { get; set; }
        //[JsonIgnore]
        //[NotMapped]
        public string NovaSenha { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime DataUltimoLogin { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public Perfil Perfil { get; set; }        
        public Empresa Empresa { get; set; }
        public int EmpresaId { get; set; }
        public int PerfilID { get; set; }
        public Boolean ativo { get; set; }
     
    } 
}