using System;
using System.Collections.Generic;
using DeckOfCards.Models;
using System.Linq;

namespace DeckOfCards.Utility
{
    public static class Helper
    {

        public static string SerializeFinishedExercises(Dictionary<CardSymbol, int> exercises)
        {
            string serializedValue = string.Empty;

            foreach (var exercise in exercises)
            {
                serializedValue += $"{exercise.Key}.{exercise.Value},";
            }

            serializedValue.TrimEnd(',');

            return serializedValue;
        }


        public static Dictionary<CardSymbol, int> DeserializeIntoFinishedExercises(string data)
        {
            
            var dict = new Dictionary<CardSymbol, int>();

            if (string.IsNullOrEmpty(data)) return dict;

            var array = data.Split(',');

            foreach (var exerciseString in array)
            {
                if (string.IsNullOrEmpty(exerciseString)) continue;

                var values = exerciseString.Split('.');
                var cardSymbol = (CardSymbol)Enum.Parse(typeof(CardSymbol), values[0]);

                dict.Add(cardSymbol, int.Parse(values[1]));
            }

            return dict;
        }

        public static string SerializeExerciseList(List<ExerciseItem> exercises)
        {
            string serializedValue = string.Empty;

            foreach (var exercise in exercises)
            {
                serializedValue += $"{exercise.CardSymbol}.{exercise.Name},";
            }

            serializedValue.TrimEnd(',');

            return serializedValue;
        }


        public static List<ExerciseItem> DeserializeIntoExerciseList(string data)
        {
            var list = new List<ExerciseItem>();

            var array = data.Split(',');

            foreach (var exerciseString in array)
            {
                if (string.IsNullOrEmpty(exerciseString)) continue;

                var values = exerciseString.Split('.');
                var cardSymbol = (CardSymbol)Enum.Parse(typeof(CardSymbol), values[0]);

                list.Add(new ExerciseItem(cardSymbol, values[1]));
            }

            return list;
        }

        public static string SerializeCardList(List<CardItem> cards)
        {
            string serializedValue = string.Empty;

            foreach (var card in cards)
            {
                serializedValue += $"{card.Symbol}.{card.Value},";
            }

            serializedValue.TrimEnd(',');

            return serializedValue;
        }


        public static List<CardItem> DeserializeIntoCardList(string data, List<ExerciseItem> exercises)
        {
            var list = new List<CardItem>();

            var array = data.Split(',');

            foreach (var cardString in array)
            {
                if (string.IsNullOrEmpty(cardString)) continue;

                var values = cardString.Split('.');
                var cardSymbol = (CardSymbol)Enum.Parse(typeof(CardSymbol), values[0]);
                var cardValue = (CardValue)Enum.Parse(typeof(CardValue), values[1]);

                var exerciseName = exercises.Where(x => x.CardSymbol == cardSymbol).FirstOrDefault().Name;

                list.Add(new CardItem(cardSymbol, cardValue, exerciseName));
            }

            return list;
        }



        public static string GetNumberStringForValue(CardValue symbol)
        {
            var number = GetNumberForValue(symbol);
            return number == 0 ? string.Empty : number.ToString();
        }

        public static int GetNumberForValue(CardValue symbol)
        {
            switch (symbol)
            {
                case CardValue.Ace: return 1;
                case CardValue.Two: return 2;
                case CardValue.Three: return 3;
                case CardValue.Four: return 4;
                case CardValue.Five: return 5;
                case CardValue.Six: return 6;
                case CardValue.Seven: return 7;
                case CardValue.Eight: return 8;
                case CardValue.Nine: return 9;
                case CardValue.Ten: return 10;
                case CardValue.Jack: return 11;
                case CardValue.Queen: return 12;
                case CardValue.King: return 13;
                default: return 0;
            }
        }
    }
}
