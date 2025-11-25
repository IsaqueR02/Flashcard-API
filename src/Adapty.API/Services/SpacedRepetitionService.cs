using System;
using Adapty.API.Models;

namespace Adapty.API.Services
{
    public class SpacedRepetitionService
    {
        // Recebe o cartão e a nota (quality), calcula e DEVOLVE o cartão atualizado
        public Card ProcessReview(Card card, int quality)
        {
            // Quality: 0 a 5 (3 é o corte de aprovação)
            
            if (quality >= 3) // Acertou
            {
                if (card.RepetitionCount == 0) 
                {
                    card.IntervalInDays = 1;
                }
                else if (card.RepetitionCount == 1) 
                {
                    card.IntervalInDays = 6;
                }
                else 
                {
                    // Fórmula clássica do SM-2
                    card.IntervalInDays = (int)Math.Round(card.IntervalInDays * card.EaseFactor);
                }

                card.RepetitionCount++;
                
                // Atualiza o Ease Factor (Fator de facilidade)
                // EF' = EF + (0.1 - (5-q) * (0.08 + (5-q)*0.02))
                card.EaseFactor = card.EaseFactor + (0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02));
                
                // O SM-2 define que o Ease Factor não deve cair abaixo de 1.3
                if (card.EaseFactor < 1.3) card.EaseFactor = 1.3;
            }
            else // Errou
            {
                card.RepetitionCount = 0;
                card.IntervalInDays = 1; // Reseta para rever amanhã
            }

            // Define a próxima data baseada no intervalo calculado
            card.NextReviewDate = DateTime.Now.AddDays(card.IntervalInDays);

            return card;
        }
    }
}