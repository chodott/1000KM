using UnityEngine;
using UnityEngine.Playables;

public class Shop1Cutscene : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject[] objectToDisable;
    public GameObject[] objectToActivate;
    public bool evaluateFirstFrame = true;

    void Reset()
    {
        director = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        if (director == null) director = GetComponent<PlayableDirector>();
        if (director == null || director.playableAsset == null) return;

        director.time = 0;
        if (evaluateFirstFrame) director.Evaluate();
        director.Play();

        director.stopped += OnTimelineStopped;

        if(objectToDisable.Length!=0)
        {
            for (int i = 0; i < objectToDisable.Length; i++)
            {
                objectToDisable[i].SetActive(false);
            }
        }

        director.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
        Time.timeScale = 0f;
    }

    void OnDisable()
    {
        if (director != null)
            director.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector d)
    {
        // 1. 다른 오브젝트 켜기
        if (objectToActivate != null)
        {
            for(int i=0; i<objectToActivate.Length; i++)
            {
                objectToActivate[i].SetActive(true);
            }
        }
        // 2. 자기 자신 끄기
        gameObject.SetActive(false);
    }
}
