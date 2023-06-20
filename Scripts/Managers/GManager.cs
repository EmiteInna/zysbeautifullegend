using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameStates { NORMAL,PAUSED,DIALOG };
public class GManager : MonoBehaviour
{
    public static GManager Instance;
    public CharacterStats playerStats;
    public GameStates gamestate;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    private void Start()
    {
        KeyManager.Instance.OnPausePushed += PauseGame;
    }
    private void OnDisable()
    {
        KeyManager.Instance.OnPausePushed -= PauseGame;
    }
   
    public List<IEndGameObserver> endGameObservers=new List<IEndGameObserver>();
    public void RegisterPlayer(CharacterStats player)
    {
        playerStats = player;
        /*
         *for test
         */
        playerStats.currentHealth = 200;
        playerStats.maxHealth = 200;
        playerStats.defense = 0;
        playerStats.critChance = 0.2f;
        playerStats.critDamage = 2;
        playerStats.attackCoolDown = 1;
        playerStats.attackRange = 5;
        playerStats.attackDamage = 5;
    }
    public void addObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }
    public void removeObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }
    public void NotifyObservers()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
    public void intoPause()
    {
        PauseManager.Instance.show();
        gamestate = GameStates.PAUSED;
        Time.timeScale = 0.0f;
    }
    public void intoNormal()
    {
        PauseManager.Instance.unshow();
        gamestate = GameStates.NORMAL;
        Time.timeScale = 1.0f;
    }
    public void intoDialog()
    {
        gamestate = GameStates.DIALOG;
        Time.timeScale = 0.0f;
    }
    public void PauseGame()
    {
        if (gamestate == GameStates.DIALOG) return;
        if (gamestate != GameStates.PAUSED) intoPause();
        else intoNormal();
        
    }
   public void ShowDialog()
    {

        if (gamestate != GameStates.DIALOG) intoDialog();
        else  intoNormal();
    }
    
}
