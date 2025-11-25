using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adapty.API.DTOs
{
    // Auth
    public record RegisterRequestDto(string Name, string Email, string Password, string Role);
    public record UserProfileDto(string Name, string Email);
    public record LoginRequestDto(string Email, string Password);

    // Decks
    public record CreateDeckDto(string Title, string Description, string[] Tags);
    
    // Cards
    public record CreateCardDto(string FrontText, string BackText);

    // Study
    public record StartSessionDto(int DeckId, int MaxCards);
    public record ReviewCardDto(int Quality, int TimeTakenSeconds); 
    // Quality: 1=Novamente, 2=Difícil, 3=Bom, 4=Fácil

    public record DeckDto(int Id, string Title, string Description, string[] Tags);
    public record CardDto(int Id, string FrontText, string BackText);

    public record StudySessionDto(int SessionId, int DeckId, DateTime StartedAt);
    public record StudyCardDto(int CardId, string FrontText, string BackText, int ReviewCount);

    public record PagedResultDto<T>(IEnumerable<T> Items, int TotalCount, int Page, int PageSize);

    public record ErrorResponseDto(string Message);

    public record AuthResponseDto(string Token, UserProfileDto User);

    public record CardSimpleDto(int Id, string Front, string Back);

    public record DeckWithCardsDto(
        int Id, 
        string Name, 
        string Description, 
        List<CardSimpleDto> Cards
    );
}