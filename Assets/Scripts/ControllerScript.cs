using System.Collections;
using UnityEngine;

public class GameControllers : MonoBehaviour
{
    //https://docs.unity3d.com/6000.0/Documentation/Manual/ios-detect-game-controllers.html
    //From the unity manual
    private bool connected = false;

    IEnumerator CheckForControllers() {
        while (true) {
            var controllersnamelist = Input.GetJoystickNames();

            if (!connected && controllersnamelist.Length > 0) {
                connected = true;
                Debug.Log("Connected");
            
            } else if (connected && controllersnamelist.Length == 0) {         
                connected = false;
                Debug.Log("Disconnected");
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Awake() {
        StartCoroutine(CheckForControllers());
    }
}
