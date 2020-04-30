using Domain.DTO;
using Domain.Models.Entities;
using Infra.Interfaces.Repositories.RepositoryBase;
using System.Collections.Generic;

namespace Infra.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        UsuarioDTO GetUserByEmail(string email);
        Usuario BuscarUsuarioPeloToken(string token);
        Usuario GetUserById(int id);
        List<Usuario> GetAllUsers();
        bool ValidarEmail(string email);
        List<Perfil> Listar_Perfis();
        Perfil Listar_Perfil(int vID);
        List<Usuario> Pesquisar_Usuarios(int vEmpresaId);
        List<Usuario> Pesquisar_Usuarios(int vEmpresaId, string vnome, int vperfil);
        List<Usuario> Pesquisar_Usuarios(int vEmpresaId, string vnome);
        List<Menus_Acesso> Listar_Menus_Acesso();
        List<Menus_Acesso> Listar_Menus_Acesso(int IDPerfil);
        string Resetar_Senha(int id);
        int Criar_Usuario(Usuario vUsuario);
        bool Remover_Usuario(int id);
    }
}