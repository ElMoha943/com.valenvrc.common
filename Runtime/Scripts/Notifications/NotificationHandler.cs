using UdonSharp;
using UnityEngine;

namespace valenvrc.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png")]
    public class NotificationHandler: UdonSharpBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject defaultPrefab;
        [SerializeField] Transform[] targetScreenAreas;
        [SerializeField] AudioClip defaultSound;

        public void _SendNotification(string message, Sprite newIcon=null, AudioClip notificationSound=null, float displayDuration = 5.0f, GameObject notificationPrefab = null, int targetScreenAreaIndex = 0){
            if (string.IsNullOrEmpty(message)) return;

            if(notificationPrefab == null) notificationPrefab = defaultPrefab;
            if (notificationPrefab == null || targetScreenAreas == null || targetScreenAreaIndex < 0 || targetScreenAreaIndex >= targetScreenAreas.Length || targetScreenAreas[targetScreenAreaIndex] == null) return;

            var notification = Instantiate(notificationPrefab, targetScreenAreas[targetScreenAreaIndex]).GetComponent<Notification>();
            if (notification == null) return;

            if(!notificationSound) notificationSound = defaultSound;
            if (notificationSound != null) AudioSource.PlayClipAtPoint(notificationSound, transform.position);

            notification._Open(message,displayDuration,newIcon);
        }
    }
}