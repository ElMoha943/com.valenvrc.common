using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace valenvrc.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/animation#distanceanimatortrigger")]
    public class DistanceAnimatorTrigger : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("At this distance or more the parameter value is 0.0")] float MinDistance = 5f;
        [SerializeField, Tooltip("At this distance or less the parameter value is 1.0")] float MaxDistance = 1f;
        [SerializeField] Animator TargetAnimator;
        [SerializeField] string ParameterName = "DistanceParam";
        [SerializeField] Transform TargetTransform;

        int _paramHash;
        VRCPlayerApi localPlayer;

        void Start()
        {
            if(TargetAnimator == null || TargetTransform == null || string.IsNullOrEmpty(ParameterName))
            {
                Debug.LogError("[DistanceAnimatorTrigger] TargetAnimator and TargetTransform must be assigned.");
                this.enabled = false;
                return;
            }
            localPlayer = Networking.LocalPlayer;
            _paramHash = Animator.StringToHash(ParameterName);
        }

        void LateUpdate()
        {
            float distance = Vector3.Distance(TargetTransform.position, localPlayer.GetPosition());
            float paramValue = Mathf.InverseLerp(MinDistance, MaxDistance, distance);
            TargetAnimator.SetFloat(_paramHash, paramValue);
        }

        void OnDrawGizmos()
        {
            if (TargetTransform == null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(TargetTransform.position, MaxDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(TargetTransform.position, MinDistance);
        }
    }
}
