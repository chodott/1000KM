using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleButtons : MonoBehaviour
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button optionButton;
    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private GameObject optionPanel;

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        optionButton.onClick.AddListener(Option);
        exitButton.onClick.AddListener(Exit);
    }

    void Play()
    {
        //씬넘어가는코드넣기
    }

    void Option()
    {
        optionPanel.SetActive(true);
    }

    void Exit()
    {
        EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
