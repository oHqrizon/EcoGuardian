using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndScene : MonoBehaviour
{
    [SerializeField]
    VideoPlayer myVideoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer.loopPointReached += AfterScene;
    }

    void AfterScene(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}
