using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public void TrackEvent(string eventName, string eventData)
    {
        // Here, you would typically send this data to your analytics server
        Debug.Log($"Event tracked: {eventName} - Data: {eventData}");
    }
}
