using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TitleCutscene : MonoBehaviour
{
    public PlayableDirector timeline;

    void Start()
    {
        if (timeline != null)
        {
            timeline.stopped += OnTimelineStopped;
        }
    }

    void OnDestroy()
    {
        if (timeline != null)
        {
            timeline.stopped -= OnTimelineStopped;
        }
    }

    private void OnTimelineStopped(PlayableDirector pd)
    {
        SceneManager.LoadScene(1);
    }
}
