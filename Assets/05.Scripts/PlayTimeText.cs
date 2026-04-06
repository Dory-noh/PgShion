using UnityEngine;
using UnityEngine.UI;

public class PlayTimeText : MonoBehaviour
{
    public Text timeText;

    float playTime;

    bool isActive;

    void Update()
    {
        if (!isActive) return;
        playTime += Time.deltaTime;

        int min = Mathf.FloorToInt(playTime / 60);
        int sec = Mathf.FloorToInt(playTime % 60);

        timeText.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    public void gameStart()
    {
        isActive = true;
    }
}