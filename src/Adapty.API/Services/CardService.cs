using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Adapty.API.Data;
using Adapty.API.Models;
using Adapty.API.DTOs;

namespace Adapty.API.Services
{
    public class CardService
    {
            
        private readonly AppDbContext _context;

        public CardService(AppDbContext context)
        {
            _context = context;
        }

        public List<Card> GetCardsByDeck(int deckId)
        {
            return _context.Cards
                .Where(c => c.DeckId == deckId)
                .ToList();
        }

        public Card AddCardToDeck(int deckId, Card card)
        {
            card.DeckId = deckId;
            _context.Cards.Add(card);
            _context.SaveChanges();
            return card;
        }
        public Card? GetCardById(int cardId)
        {
            return _context.Cards.Find(cardId);
        }

        public void DeleteCard(Card card)
        {
            _context.Cards.Remove(card);
            _context.SaveChanges();
        }

        public void UpdateCard(Card card)
        {
            _context.Cards.Update(card);
            _context.SaveChanges();
        }
    }
}