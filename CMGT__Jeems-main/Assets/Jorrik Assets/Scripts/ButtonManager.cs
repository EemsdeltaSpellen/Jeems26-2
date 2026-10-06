using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Update is called once per frame
    public void LoadDailyQuiz()
    {
        SceneManager.LoadScene("ThrillerDance");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu clicked!");
        SceneManager.LoadScene("StartMenu");
    }
    public void LoadTeamsTraining()
    {
        SceneManager.LoadScene("Teams");
    }
    public void Organisatiekennisload()
    {
        SceneManager.LoadScene("Organisatiekennis");
    }

    public void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}