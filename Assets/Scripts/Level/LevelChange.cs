using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    [SerializeField] int LevelNumber;
    bool levelLoad;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            if(!levelLoad)
            {
                levelLoad = true;
                SceneManager.LoadScene(LevelNumber);
            }
        }
    }
}
