using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Adapty.API.Models
{
    public class Card
    {
        public int Id { get; set; }
        public int DeckId { get; set; }
        public string Pergunta { get; set; } = string.Empty;
        public string Resposta { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        // Campos necessários para o SM-2 (Repetição Espaçada)
        public DateTime? NextReviewDate { get; set; } // Próxima data de revisão
        public double EaseFactor { get; set; } = 2.5; // Padrão do SM-2
        public int IntervalInDays { get; set; } = 0; // Intervalo atual
        public int RepetitionCount { get; set; } = 0; // Quantas vezes revisou

        // Propriedade de navegação (permite acessar os dados do Deck a partir da carta)
        [JsonIgnore] 
        public Deck Deck { get; set; }
    }
}