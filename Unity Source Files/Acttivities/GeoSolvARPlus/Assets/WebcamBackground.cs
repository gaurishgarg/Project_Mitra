
using UnityEngine;
using System.Collections;

public class WebcamBackground : MonoBehaviour
{
    [Header("Webcam Settings")]
    public int requestedWidth = 1280;
    public int requestedHeight = 720;
    public int requestedFPS = 30;

    [Header("Background Settings")]
    public Camera targetCamera;
    public Renderer backgroundRenderer;

    private WebCamTexture webcamTexture;

    IEnumerator Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (backgroundRenderer == null)
        {
            Debug.LogError("Assign the background quad Renderer.");
            yield break;
        }

        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("No webcam detected.");
            yield break;
        }

        WebCamDevice device = WebCamTexture.devices[0];

        webcamTexture = new WebCamTexture(
            device.name,
            requestedWidth,
            requestedHeight,
            requestedFPS
        );

        backgroundRenderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();

        // Wait for webcam initialization
        yield return new WaitUntil(() =>
            webcamTexture.width > 100
        );

        Debug.Log("Webcam started: " + device.name);
    }

    void OnDestroy()
    {
        if (webcamTexture != null && webcamTexture.isPlaying)
            webcamTexture.Stop();
    }
}
