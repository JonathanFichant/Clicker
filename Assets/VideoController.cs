using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string videoFileName = "Windows_XP_Loading_Screen.mp4";

    void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            string videoPath = Path.Combine(Application.streamingAssetsPath, "Videos", videoFileName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}