using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    //Based on the buttons page in the Unity guide
    ///https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Button-onClick.html
    [Header("Buttons")]
    public Button leftbutton, rightbutton;

    [Header("Scene Names")]
    public string leftsceneName = "Level_1"; // your main level scene
    public string rightsceneName = "Menu";     // your main menu scene

    void Start()
    {
        leftbutton.onClick.AddListener(() => ButtonClicked(1));
        rightbutton.onClick.AddListener(() => ButtonClicked(2));
        //leftbutton.onClick.AddListener(TaskOnClick);
        //rightbutton.onClick.AddListener(TaskOnClick);
    }
   
    void ButtonClicked(int buttonNo)
    {
        if (buttonNo == 1)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(leftsceneName);
        }
        else if (buttonNo == 2)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(rightsceneName);
        }
    }
}
