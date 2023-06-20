using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuUI : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneController.Instance.TransitionToDestination(new Vector3(0, 0, 0), "MainScene");
    }
    public void OnTutorButton()
    {
        SceneController.Instance.TransitionToDestination(new Vector3(0, 0, 0), "Instruction");
        //    SceneManager.LoadScene("TutorScene");
    }
    public void BackToMain()
    {
        if (GManager.Instance!=null&&GManager.Instance.gamestate == GameStates.PAUSED)
        {
            GManager.Instance.intoNormal();
        }
        SceneController.Instance.TransitionToDestination(new Vector3(0, 0, 0), "StartMenu");
    }
    public void OnQuitButton()
    {
#if UNITY_EDITOR//在编辑器模式退出
        UnityEditor.EditorApplication.isPlaying = false;
#else//发布后退出
        Application.Quit();
#endif
    }
}
