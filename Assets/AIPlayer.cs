using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Assets
{
    public class AIPlayer : MonoBehaviour
    {
        private Game _game;
        private GameObject _topCard;
        private List<GameObject> _hand;
        private int _deckPull;
        private SelectSuit _selectSuit;
        // Start is called before the first frame update
        void Start()
        {
            _game = FindObjectOfType<Game>();
            _selectSuit = FindObjectOfType<SelectSuit>();
            _deckPull = 0;
        }
        // Update is called once per frame
        void Update()
        {
            _hand = _game.Player1.Hand;
            if (_game.SelectedSuit != null && _game.PlayerIndex == 0)
            {
                string suit = _game.SelectedSuit.name.Substring(0, 1);
                List<GameObject> matchingSelectedSuitCards = _hand.FindAll(card => card.name.Substring(0, 1) == suit);
                if (matchingSelectedSuitCards.Count > 0)
                {
                    GameObject card = matchingSelectedSuitCards[0];
                    _game.SetCardOnTop(card);
                    _game.NextTurn();
                    _selectSuit.RemoveSuits();
                    _game.SelectedSuit = null;
                }
                else if (_deckPull < 2)
                {
                    _game.Player1.AddCard();
                    _deckPull++;
                }
                else
                {
                    _deckPull = 0;
                    _game.NextTurn();
                }
            }
            else if (_game.PlayerIndex == 0)
            {
                _topCard = _game.TopCard;
                if (_hand != null)
                {
                    List<GameObject> matchingSuitCards = _hand.FindAll(card => card.name.Substring(0, 1) == _topCard.name.Substring(0, 1) && card.name.Substring(1) != "8");
                    if (matchingSuitCards.Count > 1)
                    {
                        matchingSuitCards = matchingSuitCards.OrderBy(card => card.name.Substring(1)).ToList();
                    }
                    List<GameObject> matchingValueCards = _hand.FindAll(card => card.name.Substring(1) == _topCard.name.Substring(1) && card.name.Substring(1) != "8");

                    GameObject cardEight = _hand.Find(card => card.name.Substring(1) == "8");
                    if (matchingValueCards.Count > 0)
                    {
                        foreach (var card in matchingValueCards)
                        {
                            _game.SetCardOnTop(card);
                        }
                        _game.NextTurn();
                    }
                    else if (matchingSuitCards.Count > 0)
                    {
                        GameObject card = matchingSuitCards[0];
                        _game.SetCardOnTop(card);
                        _game.NextTurn();
                    }
                    else if (cardEight != null)
                    {
                        _game.SetCardOnTop(cardEight);
                        SelectSuit();
                        _game.NextTurn();
                    }
                    else if (_deckPull < 3)
                    {
                        _game.Player1.AddCard();
                        _deckPull++;
                    }
                    else
                    {
                        _deckPull = 0;
                        _game.NextTurn();
                    }
                }
            }
        }
        void SelectSuit()
        {
            _selectSuit.SetSuits();
            _hand = _game.Player1.Hand;
            List<string> suits = new List<string>();
            foreach (var card in _hand)
            {
                suits.Add(card.name.Substring(0, 1));
            }
            int heartOccurrences = suits.Count(suit => suit == "H");
            int spadeOccurrences = suits.Count(suit => suit == "S");
            int diamondOccurrences = suits.Count(suit => suit == "D");
            int clubOccurrences = suits.Count(suit => suit == "C");
            List<int> suitsOccurrences = new List<int>(){heartOccurrences, spadeOccurrences, diamondOccurrences, clubOccurrences};
            int maxValue = suitsOccurrences.Max();
            int maxPosition = suitsOccurrences.FindIndex( x => x == maxValue);
            switch (maxPosition)
            {
                case 0:
                    SetButton(_selectSuit.Heart);
                    break;
                case 1:
                    SetButton(_selectSuit.Spade);
                    break;
                case 2:
                    SetButton(_selectSuit.Diamond);
                    break;
                case 3:
                    SetButton(_selectSuit.Club);
                    break;
            }
        }

        private void SetButton(Button selected)
        {
            _selectSuit.SelectedSuit(selected);
            _game.SelectedSuit = selected;
        }
    }
}