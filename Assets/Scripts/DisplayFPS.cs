using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayFPS : MonoBehaviour
{
    public string formatedString = "{value} FPS";
    public TextMeshProUGUI txtFps;
    public TextMeshProUGUI txtPos;
    public GameObject player;

    public float updateRateSeconds = 4.0f;

    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;

    void Update()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0 / updateRateSeconds)
        {
            fps = frameCount / dt;
            frameCount = 0;
            dt -= 1.0F / updateRateSeconds;
        }
        txtFps.text = formatedString.Replace("{value}", System.Math.Round(fps, 1).ToString("0.0"));
        string x_ = System.Math.Round(player.transform.position.x, 1).ToString("0.0");
        string y_ = System.Math.Round(player.transform.position.y, 1).ToString("0.0");
        string z_ = System.Math.Round(player.transform.position.z, 1).ToString("0.0");
        txtPos.text = $"X:{x_},Y:{y_},Z:{z_}";
    }
}
