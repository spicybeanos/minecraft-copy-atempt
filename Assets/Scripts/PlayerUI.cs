using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject WaterScreen;
    public GameObject DebugMenu;
    public void Quit()
    {
        Application.Quit();
    }
    public void SendToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
