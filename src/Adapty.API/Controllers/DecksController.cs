using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adapty.API.DTOs;
using Adapty.API.Data;
using Adapty.API.Models;
using Adapty.API.Services;

namespace Adapty.API.Controllers
{
    [ApiController]
    [Route("api/decks")]
    [Authorize] // Isso exige o Token
    public class DecksController : ControllerBase
    {
        private readonly DeckService _deckService;
        private readonly CardService _cardService;

        public DecksController(DeckService deckService, CardService cardService)
        {
            _deckService = deckService;
            _cardService = cardService;
        }

        // GET: api/decks
        [HttpGet]
        public IActionResult GetAllDecks()
        {
            // Opcional: Filtrar pelo usuário logado (se você tiver UserId no Deck)
            // Por enquanto, pega todos os decks do banco
            var decks = _deckService.GetAllDecks();
            
            return Ok(decks);
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

            _deckService.CreateDeck(deck);

            return Ok(new { message = "Deck criado com sucesso!", deckId = deck.Id });
        }

        // POST: api/decks/{deckId}/cards
        [HttpGet("{deckId}/cards")]
        public IActionResult GetCardsByDeck(int deckId, [FromBody] CreateCardDto request)
        {
            var cards = _deckService.GetDeckById(deckId);
            if (cards == null) return NotFound("Deck não encontrado.");
            var card = new Card
            {
                DeckId = deckId,
                Pergunta = request.FrontText,
                Resposta = request.BackText
            };

            _cardService.AddCardToDeck(deckId, card);

            return Ok(new { message = "Cartão adicionado com sucesso!", cardId = card.Id });
        }
    }
}