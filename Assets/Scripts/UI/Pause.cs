using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))        //Pauses the game when escape is pressed
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Resume()                            //called from UI button to resume the game
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Quit()                              //called from UI button to quit the game
    { 
        Time.timeScale = 1;
        SceneManager.LoadScene("Lab");
    }
}
