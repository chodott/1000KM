using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
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

    public PlayableDirector timeline;
    public GameObject obj;

    private void Start()
    {
        playButton.onClick.AddListener(Play);
        optionButton.onClick.AddListener(Option);
        exitButton.onClick.AddListener(Exit);
    }

    void Play()
    {
        obj.SetActive(false);
        if (timeline != null)
        {
            timeline.time = 0;
            timeline.Play();
        }
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
