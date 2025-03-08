using UdonSharp;
using VRC.Economy;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync), HelpURL("https://discord.gg/nv5ax3wDqc"), Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg")]
    public class OpenGroupButton : UdonSharpBehaviour{

        [SerializeField] string GroupIdentifier;

        public void _OpenGroupPage(){
            Store.OpenGroupPage(GroupIdentifier);
        }
    }
}