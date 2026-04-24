using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void goToMenu()
    {
        GameManager.Instance.ReturnToMenu();
    }

    // Update is called once per frame
    public void startGame()
    {
        SceneManager.LoadScene("TestLukas");
    }

    public void endGame()
    {
        Application.Quit();
    }

    public void goToSettings()
    {

    }
}
