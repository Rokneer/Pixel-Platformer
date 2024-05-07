using UnityEngine;

[CreateAssetMenu(menuName = "Pixel-Platformer/Options")]
public class Options : ScriptableObject
{
    public bool isFullscreen = true;
    public float masterVolumeLevel = 0.7f;
    public float sfxVolumeLevel = 0.7f;
    public float musicVolumeLevel = 0.7f;
}
