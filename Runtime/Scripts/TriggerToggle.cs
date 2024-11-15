using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class TriggerToggle : UdonSharpBehaviour
    {
        [SerializeField] GameObject[] toggleableObjects;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                foreach (GameObject obj in toggleableObjects)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (player.isLocal)
            {
                foreach (GameObject obj in toggleableObjects)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
    }
}