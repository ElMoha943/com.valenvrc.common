using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class PickupInvoke : UdonSharpBehaviour
    {
        [SerializeField] string methodName;
        [SerializeField] UdonBehaviour target;
        
        public override void OnPickupUseDown(){
            target.SendCustomEvent(methodName);
        }
    }
}