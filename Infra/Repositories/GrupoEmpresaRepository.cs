using Domain.DTO;
using Domain.Interfaces.Repositories;
using Domain.Models.Entities;
using Domain.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Domain.Repositories
{
    public class GrupoEmpresaRepository : RepositoryBase<GrupoEmpresa>, IGrupoEmpresaRepository
    {
        private readonly ApplicationDbContext _contex;

        public GrupoEmpresaRepository(ApplicationDbContext contex) : base(contex)
          => _contex = contex;

        public GrupoEmpresa CheckGrupoEmpresa(int idEmpresa, int idsistema)
               => (_contex.GrupoEmpresa.Where(x => x.EmpresaId == idEmpresa && x.SistemaId == idsistema && x.MatrizIdExterno > 0).Select(x => x)).FirstOrDefault();

        public List<EmpresaIdNomeDTO> BuscarGrupoEmpresasIdNome(int empresaId, int sistemaId)
        {
            List<EmpresaIdNomeDTO> lista = new List<EmpresaIdNomeDTO>();

            string connectionString = "Server=quadribrasil.database.windows.net,1433;Database=GameTripDesenv;User ID=qsadmin;Password=cadeadoQS1";

            string queryString = String.Format("select B.Nome, B.filialidexterno, B.EmpresaId " +
                                                "from(select emp.Nome, g.filialidexterno, g.matrizidexterno, emp.EmpresaId " +
                                                "from Empresa emp " +
                                                "Join GrupoEmpresa as g  on g.filialidexterno = emp.idexterno " +
                                                "where g.matrizidexterno = (select GrupoEmpresa.matrizidexterno " +
                                                "from GrupoEmpresa " +
                                                "where GrupoEmpresa.filialidexterno in ({0}) and emp.SistemaId = {1})) as B", empresaId, sistemaId);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("EmpresaId", empresaId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var obj = new EmpresaIdNomeDTO
                        {
                            Nome = (string)reader["Nome"],
                            EmpresaIdExterno = (int)reader["filialidexterno"],
                            EmpresaIdInterno = (int)reader["EmpresaId"]
                        };
                        lista.Add(obj);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return lista;
        }


       
    }
}