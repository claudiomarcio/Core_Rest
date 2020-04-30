using System;
namespace Domain.Models.Entities
{
    public class Sistema
    {
        public Sistema(){}

        public Sistema(Sistema sistema)
        {
            Nome = sistema.Nome;
        }

        public int SistemaId { get; set; }
        public string Nome { get; set; }

    }
}