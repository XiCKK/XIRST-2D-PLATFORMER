using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Sound Effect")]
    [SerializeField]
    private AudioClip pauseEffect;
    [SerializeField]
    private AudioClip confirmEffect;


    [Header("Pause")]
    [SerializeField]
    private GameObject pauseScreen;
    #region Menu
    private void Awake()
    {
        pauseScreen.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //if pause screen already active = unpause 
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
                SoundManager.instance.PlaySound(pauseEffect);
            }
            
        }
    }

    public void MainMenu()
    {
        SoundManager.instance.PlaySound(confirmEffect);
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        SoundManager.instance.PlaySound(confirmEffect);
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exit play mode only work when in unity editor
        #endif
    }

    public void PauseGame(bool status)
    {
        //if status == true pause , if status == false unpause
        pauseScreen.SetActive(status);
        if (status)
        {
            Time.timeScale = 0;
        }
        else
        {
            SoundManager.instance.PlaySound(confirmEffect);
            Time.timeScale = 1;
        }
            
    }

    public void SoundVolume()
    {
        SoundManager.instance.PlaySound(confirmEffect);
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.PlaySound(confirmEffect);
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }
    #endregion
}
