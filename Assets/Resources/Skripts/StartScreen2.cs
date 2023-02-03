using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartGame", 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartGame()
    {
        SceneManager.LoadScene("LoopLevel");
    }
}
