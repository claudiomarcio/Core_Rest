using Domain.DTO;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Entities
{
    public class Menus_Acesso
    {
        public Menus_Acesso() { }

        public int ID { get; set; }
        public string Descricao_Menu { get; set; }
    }
}
