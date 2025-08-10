using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(1);
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
