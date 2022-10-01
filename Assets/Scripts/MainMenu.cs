using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public int scenenumber = 0;

    public void Open(GameObject open)
    {
        open.SetActive(true);
    }

    public void Close(GameObject close)
    {
        close.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        playableDirector.Play();
    }

    private void OnEnable()
    {
        playableDirector.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (playableDirector == aDirector)
        {
            switch(scenenumber)
            {

                case 1:
                    Level1();
                    break;

                case 2:
                    Level2();
                    break;

                case 3:
                    Level3();
                    break;

                case 4:
                    Level4();
                    break;

                case 5:
                    Level5();
                    break;

                default:
                    Tutorial();
                    break;
            }
        }
    }

    private void OnDisable()
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }

    public void ChangeScene(int number)
    {
        scenenumber = number;
    }


    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void Level4()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void Level5()
    {
        SceneManager.LoadScene("Level 5");
    }
}
