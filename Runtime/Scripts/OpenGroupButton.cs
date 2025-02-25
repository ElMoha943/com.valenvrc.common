using UdonSharp;
using VRC.Economy;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), HelpURL("https://discord.gg/nv5ax3wDqc"), Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg")]
    public class OpenGroupButton : UdonSharpBehaviour{
        public string GroupIdentifier;
        public void OpenGroupPages(){
            Store.OpenGroupPage(GroupIdentifier);
        }
    }
}