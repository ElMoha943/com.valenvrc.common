
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace valenvrc.Common
{
    [RequireComponent(typeof(Collider))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/utility#triggerinvoke")]
    public class TriggerInvoke : UdonSharpBehaviour
    {
        [SerializeField] string[] methodNames = new string[1];
        [SerializeField] UdonBehaviour[] targets = new UdonBehaviour[1];

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (!player.isLocal) return;
            for (int i = 0; i < targets.Length; i++)
                if (targets[i] != null && !string.IsNullOrEmpty(methodNames[i]))
                    targets[i].SendCustomEvent(methodNames[i]);
        }
    }
}