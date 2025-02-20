using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerTimescaleSetter : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer[] videoPlayers;

    [SerializeField]
    private float[] timescaleMods;

    private void Update()
    {
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            videoPlayers[i].playbackSpeed = Time.timeScale * timescaleMods[i];
        }
    }
}
