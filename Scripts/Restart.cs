using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour
{

    public void RestartGame()
    {
        Debug.Log("restart");
   
           SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

}