using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManagerlevels : MonoBehaviour
{
    //Based on the buttons page in the Unity guide
    ///https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.Button-onClick.html
    [Header("Buttons")]
    public Button leftbutton, middlebutton, rightbutton;

    [Header("Scene Names")]
    public string leftsceneName = "left";
    public string middlesceneName = "middle";
    public string rightsceneName = "right";

    void Start()
    {
        leftbutton.onClick.AddListener(() => ButtonClicked1(1));
        middlebutton.onClick.AddListener(() => ButtonClicked1(2));
        rightbutton.onClick.AddListener(() => ButtonClicked1(3));
        //leftbutton.onClick.AddListener(TaskOnClick);
        //rightbutton.onClick.AddListener(TaskOnClick);
    }
   
    void ButtonClicked1(int buttonNo)
    {
        if (buttonNo == 1)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(leftsceneName);
        }
        else if (buttonNo == 2)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(middlesceneName);
        }
        else if (buttonNo == 3)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(rightsceneName);
        }
    }
}
