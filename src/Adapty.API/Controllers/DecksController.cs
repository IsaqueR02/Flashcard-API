using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Adapty.API.DTOs;
using Adapty.API.Models;
using Adapty.API.Services;

namespace Adapty.API.Controllers
{
    [ApiController]
    [Route("api/decks")]
    public class DecksController : ControllerBase
    {
        private readonly DeckService _deckService;
        private readonly CardService _cardService;

        public DecksController(DeckService deckService, CardService cardService)
        {
            _deckService = deckService;
            _cardService = cardService;
        }

        // 1. LISTAR TODOS OS DECKS
        [HttpGet]
        public IActionResult GetAllDecks()
        {
            var decks = _deckService.GetAllDecks();
            return Ok(decks);
        }

        // 2. CRIAR UM NOVO DECK
        [HttpPost]
        public IActionResult CreateDeck([FromBody] CreateDeckDto request)
        {
            var deck = new Deck
            {
                Nome = request.Title,
                Descricao = request.Description
            };
            _deckService.CreateDeck(deck);
            return Ok(new { message = "Deck criado!", deckId = deck.Id });
        }

        // 3. BUSCAR CARTAS DE UM DECK ESPECÍFICO (GET)
        [HttpGet("{deckId}/cards")]
        public IActionResult GetCardsByDeck(int deckId)
        {
            var cards = _cardService.GetCardsByDeck(deckId);
            return Ok(cards);
        }

        // 4. ADICIONAR CARTA EM UM DECK (POST)
        [HttpPost("{deckId}/cards")]
        public IActionResult AddCardToDeck(int deckId, [FromBody] CreateCardDto request)
        {
            var deck = _deckService.GetDeckById(deckId);
            if (deck == null) return NotFound("Deck não encontrado.");

            var card = new Card
            {
                Pergunta = request.FrontText,
                Resposta = request.BackText,
                RepetitionCount = 0,
                IntervalInDays = 0,
                EaseFactor = 2.5
            };

            _cardService.AddCardToDeck(deckId, card);
            return Ok(new { message = "Cartão criado!", cardId = card.Id });
        }
    }
}