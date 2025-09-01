using System;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;

    public GameObject shopCutscene;
    public GameObject bossAppear;
    public GameObject gameOverUI;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnPhaseChanged(GamePhase phase, PhaseData data)
    {
        switch(phase)
        {
            case GamePhase.GameOver:
                EnableGameOver();
                break;
        }
    }

    public void EnableShop()
    {
        shopCutscene.SetActive(true);
    }

    public void EnableBoss()
    {
        bossAppear.SetActive(true);
    }

    public void EnableGameOver()
    {
        gameOverUI.SetActive(true);
    }

}
