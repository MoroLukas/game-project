using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("TestAnh");
    }
    public void QuiteGame()
    {
        Application.Quit();
    }

}
