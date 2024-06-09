using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoController : MonoBehaviour
{
    [SerializeField] List<VideoPlayer> videoPlayersList;
    [SerializeField] Renderer renderer;
    [SerializeField] MeshRenderer childMesh;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.name == "Idle")
        {
            renderer.material.color = Color.white;
            childMesh.enabled = false;
            IsPlay();
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.name == "Idle")
        {
            renderer.material.color = Color.black;
            childMesh.enabled = true;
            IsStop();
        }
    }

    public void IsPlay()
    {
        foreach(VideoPlayer videoPlayer in videoPlayersList)
        {
            if(!videoPlayer.isPlaying)
            {
                videoPlayer.Play();
            }
        }
    }
    public void IsStop()
    {
        foreach(VideoPlayer videoPlayer in videoPlayersList)
        {
            videoPlayer.Stop();
        }
    }
}
