using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Adapty.API.Data;
using Adapty.API.Models;
using Adapty.API.DTOs;

namespace Adapty.API.Services
{
    public class DeckService
    {
        private readonly AppDbContext _context;

        public DeckService(AppDbContext context)
        {
            _context = context;
        }

        public List<DeckWithCardsDto> GetAllDecks()
        {
            // 1. Busca no banco com Join (Include)
            var decks = _context.Decks
                .Include(d => d.Cards)
                .ToList();

            // 2. Faz o Mapeamento (Model -> DTO)
            var result = decks.Select(d => new DeckWithCardsDto(
                d.Id,
                d.Nome,
                d.Descricao,
                d.Cards.Select(c => new CardSimpleDto(
                    c.Id,
                    c.Pergunta,
                    c.Resposta
                )).ToList()
            )).ToList();

            return result;
        }

        public Deck CreateDeck(Deck deck)
        {
            _context.Decks.Add(deck);
            _context.SaveChanges();
            return deck;
        }

        public Deck? GetDeckById(int deckId)
        {
            return _context.Decks.Find(deckId);
        }
        
        public void DeleteDeck(Deck deck)
        {
            _context.Decks.Remove(deck);
            _context.SaveChanges();
        }

        public void UpdateDeck(Deck deck)
        {
            _context.Decks.Update(deck);
            _context.SaveChanges();
        }
    }
}