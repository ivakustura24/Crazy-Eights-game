using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class UpdateSprite : MonoBehaviour
    {
        private Sprite _cardFace;
        [SerializeField]
        private Sprite _cardBack;
        private SpriteRenderer _spriteRenderer;
        private Selectable _selectable;

        public Sprite CardBack => _cardBack;
        // Start is called before the first frame update
        void Start()
        {
            Game game = FindObjectOfType<Game>();
            List<string> deck = Game.GenerateDeck();
            int i = 0;
            foreach (string card in deck)
            {
                if (this.name == card)
                {
                    _cardFace = game.CardFaces[i];
                }
                i++;
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _selectable = GetComponent<Selectable>();
        }
        // Update is called once per frame
        void Update()
        { 
            _spriteRenderer.sprite = _selectable.FaceUp ? _cardFace : _cardBack;
        }
    }
}