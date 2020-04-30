using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Perfil
    {
        public Perfil() { }

        public int ID { get; set; }
        public string Descricao { get; set; }
        public string IDs_Menus_Acesso { get; set; }
    }
}
