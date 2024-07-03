using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField]
        private Text gameOverMessage;

        public void SetUp(string message)
        {
            gameObject.SetActive(true);
            gameOverMessage.text = message;
        }

    }
}
