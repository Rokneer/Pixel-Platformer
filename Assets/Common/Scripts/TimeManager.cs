using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    public static TimeManager Instance => _instance;

    public bool isTimerActive = false;
    private float totalTime = 0;
    public TextMeshProUGUI totalTimeText;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            if (totalTime >= 0)
            {
                totalTime += Time.deltaTime;
            }
            else
            {
                totalTime = 0;
            }
        }
    }

    public void DisplayTime()
    {
        if (totalTime < 0)
        {
            totalTime = 0;
        }
        float minutes = Mathf.FloorToInt(totalTime / 60);
        float seconds = Mathf.FloorToInt(totalTime % 60);
        totalTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
