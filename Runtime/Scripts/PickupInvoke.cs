using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class PickupInvoke : UdonSharpBehaviour
    {
        [SerializeField] string methodName;
        [SerializeField] UdonBehaviour target;
        
        public override void OnPickupUseDown(){
            target.SendCustomEvent(methodName);
        }
    }
}