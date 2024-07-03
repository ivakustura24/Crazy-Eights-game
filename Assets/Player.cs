using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets
{
    public class Player : MonoBehaviour
    {
        private Game _game;
        private const float StartXPosition = -4f;
        private const float XOffset = 0.8f;
        private const float StartYPosition = -3f;
        public List<GameObject> Hand { get; set; } = new List<GameObject>();
        // Start is called before the first frame update
        void Start()
        {
            _game = FindObjectOfType<Game>();
        }

        public void RemoveCard(GameObject card)
        {
            Hand.Remove(card);
            SortCards();
        }
        public void SortCards()
        {
            float xOffset = StartXPosition;
            int sortingInLayer = 0;
            foreach (var card in Hand)
            {
                Vector3 position = card.transform.position;
                position.x = xOffset;
                card.transform.position = position;
                xOffset += XOffset;
                card.GetComponent<SpriteRenderer>().sortingOrder = sortingInLayer;
                sortingInLayer++;
            }
        }
        public void AddCard()
        {
            GameObject newCard = InstantiateCard();
            newCard = PositionCard(newCard);
            Hand.Add(newCard);
            SortCards();
            _game.RemoveLastCard();
        }

        private GameObject InstantiateCard()
        {
            float yOffset = StartYPosition;
            if (_game.PlayerIndex == 0)
            {
                yOffset = -yOffset;
            }
            GameObject newCard = Instantiate(_game.CardPrefab, new Vector3(transform.position.x, yOffset, transform.position.z), Quaternion.identity);
            return newCard;
        }
        private GameObject PositionCard(GameObject newCard)
        {
            bool faceUp = true;
            if (_game == null)
            {
                return null;
            }
            if (_game.PlayerIndex == 0)
            {
                faceUp = false;
            }
            newCard.name = _game.Deck.LastOrDefault();
            newCard.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            if (newCard.GetComponent<Selectable>())
            {
                newCard.GetComponent<Selectable>().FaceUp = faceUp;
            }
            return newCard;
        }
    }
}
