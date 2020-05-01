using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AdoConfiguration
{
    public class AdoConexao : IDisposable
    {
        private static SqlConnection conn = null;
        private SqlCommand comando;
        public string StringConexao { get; set; }

        public AdoConexao(string stringConexao)
        {
            this.StringConexao = stringConexao;
        }

        public async Task<SqlConnection> AbrirConexao()
        {
            conn = new SqlConnection(StringConexao);

            try
            {
               await conn.OpenAsync();
            }
            catch (SqlException e)
            {
                throw e;
            }

            return conn;
        }

        public void FecharConexao()
        {
            if (conn != null)
                conn.Close();
        }

        private void AdicionarParametro(Dictionary<string, object> parametros)
        {
            foreach (var parametro in parametros)
                comando.Parameters.AddWithValue(parametro.Key, parametro.Value);
        }

        public async void ExecutarComando(string comando, Dictionary<string, object> parametros)
        {
            try
            {
                SqlConnection conexao = await AbrirConexao();

                if (!string.IsNullOrEmpty(comando))
                {
                    this.comando = new SqlCommand(comando, conexao);
                    AdicionarParametro(parametros);
                    this.comando.ExecuteNonQuery();
                }

                FecharConexao();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<SqlDataReader> ExecutarConsulta(string comando, Dictionary<string, object> parametros)
        {
            try
            {
                SqlConnection conexao = await AbrirConexao();
                SqlDataReader reader = null;

                if (!string.IsNullOrEmpty(comando))
                {
                    this.comando = new SqlCommand(comando, conexao);
                    AdicionarParametro(parametros);
                    reader = this.comando.ExecuteReader();
                }

                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<object>  ExecuteScalar(string comando, Dictionary<string, object> parametros)
        {
            try
            {
                object retorno = null;
                SqlConnection conexao = await AbrirConexao();

                if (!string.IsNullOrEmpty(comando))
                {
                    this.comando = new SqlCommand(comando, conexao);
                    AdicionarParametro(parametros);
                    retorno = await this.comando.ExecuteScalarAsync();
                }

                FecharConexao();
                return retorno;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
