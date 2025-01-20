using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLogic.Components
{
    public class Navigator : MonoBehaviour
    {
        public void GoToMenu() => SceneManager.LoadScene("Menu");
        public void GoToGame() => SceneManager.LoadScene("Game");
        public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        public void Quit() => Application.Quit();

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0) 
                GoToMenu();
        }
    }
}
