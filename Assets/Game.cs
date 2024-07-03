using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] _cardFaces;
        [SerializeField]
        private GameObject cardPrefab;
        [SerializeField]
        private Button playAgain;
        private int _playerIndex;
        [SerializeField]
        private GameOverScreen _gameOverScreen;
        [SerializeField]
        private GameObject _background;
        public Button SelectedSuit { get; set; }
        [SerializeField]
        private Player _player1;
        [SerializeField]
        private Player _player2;
        public Player Player1 => _player1;
        public Player Player2 => _player2;
        public Sprite[] CardFaces => _cardFaces;
        public GameObject CardPrefab => cardPrefab;
        public List<string> Deck { get; private set; }
        public GameObject TopCard { get; private set; }
        public int PlayerIndex => _playerIndex;
        private static readonly string[] Suits = new string[] { "C", "D", "H", "S" };
        private static readonly string[] Values = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        // Start is called before the first frame update
        void Start()
        {
            playAgain.onClick.AddListener(call: () => Restart());
            PlayCards();
            _playerIndex = 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (Player2.Hand.Count == 0)
            {
                print("You win! Congratulations!");
                _background.SetActive(false);
                _gameOverScreen.SetUp("You win! Congratulations!");
            }
            else if (Player1.Hand.Count == 0)
            {
                print("Matt wins! Better luck next time!");
                _background.SetActive(false);
                _gameOverScreen.SetUp("Matt wins! Better luck next time!");
            }
        }
        public void PlayCards()
        {
            Deck = GenerateDeck();
            Shuffle(Deck);

            const float xOffset = -4f;
            float yOffset = -3.0f;

            Player2.Hand = DealSetOfEightCards(xOffset, yOffset, true);

            yOffset = 3.0f;
            Player1.Hand = DealSetOfEightCards(xOffset, yOffset, false);
            TopCard = SetFirstCard();
            RemoveLastCard();
        }

        public static List<string> GenerateDeck()
        {
            List<string> newDeck = new List<string>();
            foreach (string s in Suits)
            {
                foreach (string v in Values)
                {
                    newDeck.Add(s + v);
                }
            }
            return newDeck;
        }

        void Shuffle<T>(List<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                int k = random.Next(n);
                n--;
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }
        public List<GameObject> DealSetOfEightCards(float xOffset, float yOffset, bool cardFace)
        {
            List<GameObject> hand = new List<GameObject>();
            const float zOffset = 0.0f;
            for (int i = 0; i < 8; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z + zOffset), Quaternion.identity);
                newCard.name = Deck.LastOrDefault();
                newCard.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                newCard.GetComponent<Selectable>().FaceUp = cardFace;
                xOffset += 0.8f;
                newCard.GetComponent<SpriteRenderer>().sortingOrder = i;
                hand.Add(newCard);
                RemoveLastCard();
            }
            return hand;
        }
        public GameObject SetFirstCard()
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(transform.position.x + 1.2f, transform.position.y + 0f, transform.position.z), Quaternion.identity);
            newCard.name = Deck.LastOrDefault();
            newCard.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            newCard.GetComponent<Selectable>().FaceUp = true;

            return newCard;
        }
        public void SetCardOnTop(GameObject card)
        {
            GameObject currentTopCard = TopCard;
            if (currentTopCard != null)
            {
                currentTopCard.transform.position = new Vector3(-20f, -20.0f, transform.position.z);
                List<string> newDeck = Deck.Prepend(currentTopCard.name).ToList();
                Deck = newDeck;
            }

            TopCard = card;
            TopCard.transform.position = new Vector3(1.2f, 0f, transform.position.z);
            TopCard.GetComponent<Selectable>().FaceUp = true;
            if (_playerIndex == 1)
            {
                Player2.RemoveCard(card);
            }
            else
            {
                Player1.RemoveCard(card);
            }
        }
        public void RemoveLastCard()
        {
            Deck.RemoveAt(Deck.Count - 1);
        }
        public void NextTurn()
        {
            _playerIndex = _playerIndex == 1 ? 0 : 1;
        }
        private void Restart()
        {
            SceneManager.LoadScene("SampleScene");
            print("load scene");
        }
    }
}
