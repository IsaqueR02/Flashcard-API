using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Necessário para ler o ID do usuário do Token
using Adapty.API.DTOs;
using Adapty.API.Data;
using Adapty.API.Models;

namespace Adapty.API.Controllers
{
    [ApiController]
    [Route("api/decks")]
    [Authorize] // Isso exige o Token
    public class DecksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DecksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/decks
        [HttpGet]
        public IActionResult GetAllDecks()
        {
            // Opcional: Filtrar pelo usuário logado (se você tiver UserId no Deck)
            // Por enquanto, pega todos os decks do banco
            var decks = _context.Decks.ToList();
            
            // Transforma Model em DTO para retornar
            var result = decks.Select(d => new DeckDto(d.Id, d.Nome, d.Descricao, new string[] { })).ToList();
            
            return Ok(result);
        }

        // POST: api/decks
        [HttpPost]
        public IActionResult CreateDeck([FromBody] CreateDeckDto request)
        {
            var deck = new Deck
            {
                Nome = request.Title,
                Descricao = request.Description
                // Se tiver UserId, adicione aqui: UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            _context.Decks.Add(deck);
            _context.SaveChanges();

            return Ok(new { message = "Deck criado com sucesso!", deckId = deck.Id });
        }

        // GET: api/decks/{deckId}/cards
        [HttpGet("{deckId}/cards")]
        public IActionResult GetCardsByDeck(int deckId)
        {
            var cards = _context.Cards
                .Where(c => c.DeckId == deckId)
                .Select(c => new CardDto(c.Id, c.Pergunta, c.Resposta))
                .ToList();

            return Ok(cards);
        }

        // POST: api/decks/{deckId}/cards
        [HttpPost("{deckId}/cards")]
        public IActionResult AddCardToDeck(int deckId, [FromBody] CreateCardDto request)
        {
            // Valida se o deck existe
            var deck = _context.Decks.Find(deckId);
            if (deck == null) return NotFound("Deck não encontrado.");

            var card = new Card
            {
                DeckId = deckId,
                Pergunta = request.FrontText,
                Resposta = request.BackText,
                DataCriacao = DateTime.Now,
                // Inicia os valores do SM-2
                RepetitionCount = 0,
                IntervalInDays = 0,
                EaseFactor = 2.5
            };

            _context.Cards.Add(card);
            _context.SaveChanges();

            return Ok(new { message = "Cartão adicionado com sucesso!", cardId = card.Id });
        }
    }
}