using UnityEngine;
using UnityEngine.SceneManagement;
 
public class ChangeScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void MenuPage()
    {
        SceneManager.LoadScene("StarMenu");
    }
}