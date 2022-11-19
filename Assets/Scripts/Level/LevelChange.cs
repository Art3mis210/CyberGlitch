using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] bool ReloadLevel;
    bool levelLoad;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            if(!levelLoad)
            {
                levelLoad = true;
                if (ReloadLevel)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);     //reloads current scene
                }
                else
                {
                    if(SceneManager.GetActiveScene().name=="Level 5")           //loads menu scene when player completes level 5
                    {
                        SceneManager.LoadScene("Lab");
                    }
                    else
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //loads next level
                }
            }
        }
    }
}
