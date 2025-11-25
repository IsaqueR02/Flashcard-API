using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adapty.API.DTOs; // Ou Adapty.API.Models.DTOs dependendo de como organizou
using Adapty.API.Data;
using Adapty.API.Models;
using Adapty.API.Services; // Importante para achar o Service

namespace Adapty.API.Controllers
{
    [ApiController]
    [Route("api/study")]
    public class StudyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SpacedRepetitionService _studyService; // Injetamos o Service

        // Construtor recebe o Banco E o Service
        public StudyController(AppDbContext context, SpacedRepetitionService studyService)
        {
            _context = context;
            _studyService = studyService;
        }

        [HttpPost("session")]
        public IActionResult StartSession([FromBody] StartSessionDto request)
        {
            var cards = _context.Cards // Lembre de usar o plural se estiver assim no AppDbContext
                .Where(c => c.DeckId == request.DeckId)
                .Where(c => c.NextReviewDate == null || c.NextReviewDate <= DateTime.Now)
                .OrderBy(c => c.NextReviewDate)
                .Take(request.MaxCards)
                .Select(c => new StudyCardDto(c.Id, c.Pergunta, c.Resposta, c.RepetitionCount))
                .ToList();

            if (!cards.Any())
                return Ok(new { message = "Nenhum cartão para revisar agora!" });

            return Ok(new { sessionId = Guid.NewGuid(), cards });
        }

        [HttpPut("card/{cardId}/review")]
        public IActionResult ReviewCard(int cardId, [FromBody] ReviewCardDto request)
        {
            var card = _context.Cards.Find(cardId);
            if (card == null) return NotFound("Cartão não encontrado");

            // --- AQUI A MÁGICA ACONTECE ---
            // O Controller manda o Service fazer a conta difícil
            _studyService.ProcessReview(card, request.Quality);

            _context.SaveChanges();
            
            return Ok(new { 
                message = "Revisão salva", 
                nextReview = card.NextReviewDate, 
                interval = card.IntervalInDays 
            });
        }
    }
}