using UnityEngine;
using UnityEngine.Playables;

public class StartCutscene : MonoBehaviour
{
    public PlayableDirector timeline;

    public GameObject mainCam;
    public GameObject startCam;
    public GameObject panel1;
    public GameObject panel2;

    void Start()
    {
        if (timeline != null)
        {
            timeline.stopped += OnTimelineStopped;
        }
        mainCam.SetActive(false);
        startCam.SetActive(true);
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
        mainCam.SetActive(true);
        startCam.SetActive(false);
        panel1.SetActive(false);
        panel2.SetActive(true);
    }
}
