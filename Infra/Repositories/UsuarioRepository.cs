using Domain.DTO;
using Domain.Interfaces.Repositories;
using Domain.Models.Entities;
using Domain.SecurityHash;
using Domain.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Domain.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _contex;

        public UsuarioRepository(ApplicationDbContext contex) : base(contex)
          => _contex = contex;

        public UsuarioDTO GetUserByEmail(string email)
        => (from u in _contex.Usuario
            join e in _contex.Empresa.Include(s => s.Sistema) on u.EmpresaId equals e.EmpresaId
            join p in _contex.Perfil.Include(p => p.ID) on u.Perfil.ID equals p.ID
            select new
            {
                u,
                e,
                p
            })
            .Where(x => x.u.Email == email)
            .Select(x => new UsuarioDTO
            {
                Nome = x.u.Nome,
                Email = x.u.Email,
                UsuarioId = x.u.UsuarioId,
                EmpresaId = x.u.EmpresaId,
                SistemaId = x.e.Sistema.SistemaId,
                EmpresaIdExterno = x.e.IdExterno,
                Usuario = x.u,
                Perfil = x.u.Perfil,
                Ativo = x.u.ativo,
            }).FirstOrDefault();

        public Usuario BuscarUsuarioPeloToken(string token)
         => (from u in _contex.Usuario
             .Include(u => u.Empresa).Include(u => u.Empresa.Sistema)
             select new { u }).Where(x => x.u.Token == token && x.u.ativo == true).Select(x => x.u).FirstOrDefault();

        public Usuario GetUserById(int id)
         => (from u in _contex.Usuario
             select new { u }).Where(x => x.u.UsuarioId == id && x.u.ativo == true).Select(x => x.u).FirstOrDefault();

        public List<Usuario> GetAllUsers()
          => (from u in _contex.Usuario
             .Include(u => u.Empresa).Include(u => u.Empresa.Sistema)
             .Include(p => p.Perfil).Include(p => p.Perfil)
              select new { u }).Where(x => x.u.ativo == true).Select(x => x.u).ToList();

        public bool ValidarEmail(string email)
                => (from u in _contex.Usuario
                    select new { u }).Where(x => x.u.Email == email).Select(x => x.u != null ? true : false).FirstOrDefault();

        public List<Perfil> Listar_Perfis()
        => (from u in _contex.Perfil
            select new { u }).Select(x => x.u).ToList();

        public Perfil Listar_Perfil(int vID)
        => (from u in _contex.Perfil
            select new { u }).Select(x => x.u).Where(x => x.ID == vID).FirstOrDefault();

        public List<Usuario> Pesquisar_Usuarios(int vEmpresaId)
          => (from u in _contex.Usuario
             .Include(e => e.Empresa)//.Include(e => e.Empresa.Sistema)
             .Include(p => p.Perfil)//.Include(p => p.Perfil)
              select new { u }).Where(x => x.u.ativo == true).Select(x => x.u).Where(x => x.EmpresaId == vEmpresaId)
            .ToList();

        public List<Usuario> Pesquisar_Usuarios(int vEmpresaId, string vnome, int vperfil)
          => (from u in _contex.Usuario
             .Include(e => e.Empresa)//.Include(e => e.Empresa.Sistema)
             .Include(p => p.Perfil)//.Include(p => p.Perfil)
              select new { u }).Where(x => x.u.ativo == true).Select(x => x.u).Where(x => x.EmpresaId == vEmpresaId)
            .Where(x => x.Nome.Contains(vnome) && x.Perfil.ID == vperfil && x.ativo == true && x.EmpresaId == vEmpresaId).Select(x => x).ToList()
            .ToList();

        public List<Usuario> Pesquisar_Usuarios(int vEmpresaId, string vnome)
          => (from u in _contex.Usuario
             .Include(e => e.Empresa)//.Include(e => e.Empresa.Sistema)
             .Include(p => p.Perfil)//.Include(p => p.Perfil)
              select new { u }).Where(x => x.u.ativo == true).Select(x => x.u).Where(x => x.EmpresaId == vEmpresaId)
            .Where(x => x.Nome.Contains(vnome) && x.ativo == true && x.EmpresaId == vEmpresaId).Select(x => x).ToList()
            .ToList();

        public List<Menus_Acesso> Listar_Menus_Acesso()
            => (from u in _contex.Menus_Acesso
                select new { u }).Select(x => x.u).ToList();

        public List<Menus_Acesso> Listar_Menus_Acesso(int IDPerfil)
        {
            List<int> a2 = new List<int>();
            var Array_Menus =
                (from u in _contex.Perfil
                 select new { u })
                .Where(x => x.u.ID == IDPerfil)
                .Select(x => x.u).FirstOrDefault()
                .IDs_Menus_Acesso;

            foreach (string item in Array_Menus.Split(","))
                a2.Add(Convert.ToInt32(item));

            //var a3 = a2.Intersect(_contex.Menus_Acesso.Select(x => x.ID)).ToList();
            List<Menus_Acesso> ret = _contex.Menus_Acesso.Where(item => a2.Contains(item.ID)).ToList();

            return ret;
        }

        public string Resetar_Senha(int id)
        {
            Usuario usu = GetUserById(id);
            if (usu == null)
                return "";

            string senha_padrao = "123";
            usu.Senha = new Hash(new SHA512Managed()).CriptografarSenha(senha_padrao);
            usu.Token = "";
            try
            {
                this.Update(usu);
                return senha_padrao;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public int Criar_Usuario(Usuario vUsuario)
        {
            //validar o email , caso ok prossegue
            //ValidarEmail

            //criar uma senha padrao para o usuario em shad5

            //enviar senha por email
            return 0;
        }

        public bool Remover_Usuario(int id)
        {
            Usuario usu = GetById(id);
            if (usu == null)
                return false;

            usu.ativo = !usu.ativo;
            usu.Token = "";
            try
            {
                this.Update(usu);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}