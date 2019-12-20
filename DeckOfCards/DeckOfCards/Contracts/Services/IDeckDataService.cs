using DeckOfCards.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Contracts.Services
{
    public interface IDeckDataService
    {
        List<CardItem> GetFullDeck();

        List<ExerciseItem> GetExercises();

        string GetExerciseForCardSymbol(CardSymbol symbol);

        int GetNumberOfCardsInDeck();
    }
}
