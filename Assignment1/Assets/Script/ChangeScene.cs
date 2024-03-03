using UnityEngine;
using UnityEngine.SceneManagement;
 
public class ChangeScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("UITest");
    }

    public void MenuPage()
    {
        SceneManager.LoadScene("StartGame");
    }
}