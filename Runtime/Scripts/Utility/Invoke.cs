using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class Invoke : UdonSharpBehaviour
    {
        [SerializeField] string[] methodNames = new string[1];
        [SerializeField] UdonBehaviour[] targets = new UdonBehaviour[1];

        public override void Interact(){
            for (int i = 0; i < targets.Length; i++){
                if (targets[i] != null && !string.IsNullOrEmpty(methodNames[i])){
                    targets[i].SendCustomEvent(methodNames[i]);
                }
            }
        }
    }
}