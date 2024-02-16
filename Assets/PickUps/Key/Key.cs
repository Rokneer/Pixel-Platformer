using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject removableWalls;

    public void AddKey()
    {
        removableWalls.SetActive(false);
    }
}
