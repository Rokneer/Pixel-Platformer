using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;
    public List<Collider2D> detectedColliders = new();

    #region Lifecycle
    void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
        if (detectedColliders.Count == 0)
        {
            noCollidersRemain.Invoke();
        }
    }
    #endregion
}
