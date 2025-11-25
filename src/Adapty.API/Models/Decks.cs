using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Adapty.API.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        // Cria a relação no banco: Um deck contem várias cartas
        [JsonIgnore] // Evita "loop infinito" ao converter para JSON
        public List<Card> Cards { get; set; } = new List<Card>();
    }
}