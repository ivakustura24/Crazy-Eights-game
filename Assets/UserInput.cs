using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class UserInput : MonoBehaviour
    {
        [SerializeField]
        private Button playButton;
        private Game _game;
        private int _deckPull;
        private SelectSuit _selectSuit;
        private bool isWaitingForSuitSelection = false;
        private List<GameObject> selectedCards = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            _game = FindObjectOfType<Game>();
            _selectSuit = FindObjectOfType<SelectSuit>();
            _deckPull = 0;
            playButton.onClick.AddListener(() => PlayButton());
        }

        // Update is called once per frame
        void Update()
        {
            if (isWaitingForSuitSelection)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
                RaycastHit2D hit = Physics2D.Raycast(target, Vector2.zero);
                if (hit && _game.PlayerIndex == 1)
                {
                    if (hit.collider.CompareTag("Card"))
                    {
                        Card(hit);
                    }
                    if (hit.collider.CompareTag("Deck"))
                    {
                        Deck();
                    }
                }
            }
        }
        string GetTopCard()
        {
            GameObject card = _game.TopCard;
            return card.name;
        }
        public void Card(RaycastHit2D hit)
        {
            GameObject card = hit.transform.gameObject;
            string cardName = card.name;
            string topCard = GetTopCard();
            if(_game.SelectedSuit != null)
            {
                string suit = _game.SelectedSuit.name.Substring(0, 1);
                if(card.name.Substring(0,1) == suit)
                {
                    _game.SetCardOnTop(card);
                    _game.SelectedSuit = null;
                    _selectSuit.RemoveSuits();
                    _game.NextTurn();
                }
                else if(_deckPull == 3)
                {
                    _deckPull = 0;
                    _game.NextTurn();
                }
            }
            if (cardName.Substring(1) == "8")
            {
                _selectSuit.SetSuits();
                StartCoroutine(WaitForSelectedSuit(card));
            }
            else if (selectedCards.Count != 0)
            {
                string value = selectedCards[0].name.Substring(1);
                if (card.name.Substring(1) == value)
                {
                    SelectCards(card);
                }
            }
            else if (cardName.Substring(1) == topCard.Substring(1) && cardName.Substring(1) != "8")
            {
                List<GameObject> matchingCards = _game.Player2.Hand.FindAll(x => x.name.Substring(1) == topCard.Substring(1) && x.name.Substring(1) != "8");
                if (matchingCards.Count == 1)
                {
                    _game.SetCardOnTop(card);
                    _game.NextTurn();
                }
                else if (matchingCards.Count > 1)
                {
                    SelectCards(card);
                }
            }
            else if (cardName.Substring(0, 1) == topCard.Substring(0, 1))
            {
                List<GameObject> matchingCards =_game.Player2.Hand.FindAll(x => x.name.Substring(1) == cardName.Substring(1) && x.name.Substring(1) != "8" && x.name != cardName);
                if (matchingCards.Count ==0)
                {
                    _game.SetCardOnTop(card);
                    _game.NextTurn();
                }
                else if (matchingCards.Count > 0)
                {
                    SelectCards(card);
                }
            }
            else if (_deckPull == 3)
            {
                _deckPull = 0;
                _game.NextTurn();
            }
        }
        public void Deck()
        {
            if(_deckPull < 3 )
            {
                _game.Player2.AddCard();
                _deckPull++;
            }
            else
            {
                _deckPull = 0;
                _game.NextTurn();
            }
        }

        public void SelectCards(GameObject card)
        {
            card.GetComponent<SpriteRenderer>().color = Color.yellow;
            selectedCards.Add(card);
        }
        public void PlayButton()
        {
            if (selectedCards != null)
            {
                foreach (GameObject card in selectedCards)
                {
                    _game.SetCardOnTop(card);
                }
                _game.NextTurn();
            }
            selectedCards = new List<GameObject>();
        }
        private IEnumerator WaitForSelectedSuit(GameObject card)
        {
            isWaitingForSuitSelection = false;
            while (_game.SelectedSuit == null)
            {
                yield return null;
            }
            _game.SetCardOnTop(card);
            _game.NextTurn();
            isWaitingForSuitSelection = false;
        }
    }
}
