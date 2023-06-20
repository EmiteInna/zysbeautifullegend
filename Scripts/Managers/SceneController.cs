using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    GameObject player;
    public GameObject playerPre;
    public CanvasGroup Canvas;
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void TransitionToDestination(Vector3 position,string destName)
    {
        
        StartCoroutine(Transition(destName, position));
        
    }
    IEnumerator Transition(string sceneName,Vector3 position)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return Fade(1);
            yield return new WaitForEndOfFrame();
            yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single);
            Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
            
            //TODO:根据场景类型来判断需不需要生成玩家。
           // yield return Instantiate(playerPre, position, Quaternion.identity);
            yield return Fade(0);
        }
        else
        {
            player = GManager.Instance.playerStats.gameObject;
            player.transform.SetPositionAndRotation(position, Quaternion.identity);
        }
        yield return null;
    }
    public IEnumerator Fade(float targetAlpha)
    {
        Canvas.blocksRaycasts = true;
        float time = 0.3f;
        float speed=Mathf.Abs(Canvas.alpha-targetAlpha)/time;
        while (!Mathf.Approximately(Canvas.alpha, targetAlpha))
        {
            Canvas.alpha = Mathf.MoveTowards(Canvas.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        Canvas.blocksRaycasts = false;
    }
}
