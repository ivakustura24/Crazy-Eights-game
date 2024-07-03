using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class SelectSuit : MonoBehaviour
    {
        [SerializeField]
        private Button heart;
        [SerializeField]
        private Button spade;
        [SerializeField]
        private Button diamond;
        [SerializeField]
        private  Button club;
        private readonly List<Button> _suits = new List<Button>();
        private Game _game;

        public Button Heart => heart;
        public Button Spade => spade;
        public Button Diamond => diamond;
        public Button Club => club;
        // Start is called before the first frame update
        void Start()
        {
            _game = FindObjectOfType<Game>();
            heart.onClick.AddListener(() => SelectedSuit(heart));
            spade.onClick.AddListener(() => SelectedSuit(spade));
            diamond.onClick.AddListener(() => SelectedSuit(diamond));
            club.onClick.AddListener(() => SelectedSuit(club));
            _suits.Add(heart);
            _suits.Add(spade);
            _suits.Add(diamond);
            _suits.Add(club);
            RemoveSuits();
        }
        public void SelectedSuit(Button selected)
        {
            _game.SelectedSuit = selected;
            foreach (Button suit in _suits)
            {
                if (suit != selected)
                {
                    suit.transform.position = new Vector3(-500, 0, 1);
                }
            }
        }
        public void RemoveSuits()
        {
            foreach (Button suit in _suits)
            {
                suit.transform.position = new Vector3(-100, 0, 1);
            }
        }
        public void SetSuits()
        {
            int xOffset = 350;
            foreach (Button suit in _suits)
            {
                suit.transform.position = new Vector3(xOffset, 550, 1);
                xOffset += 100;
            }
        }
    }
}
