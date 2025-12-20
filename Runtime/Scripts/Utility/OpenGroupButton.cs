using UdonSharp;
using VRC.Economy;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), HelpURL("https://discord.gg/nv5ax3wDqc"), Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg")]
    public class OpenGroupButton : UdonSharpBehaviour{

        [SerializeField] string GroupIdentifier = "grp_32fd99ba-2ec3-4408-9dce-74cf4ea0b713";

        public override void Interact(){
            _OpenGroupPage();
        }

        public void _OpenGroupPage(){
            Store.OpenGroupPage(GroupIdentifier);
        }
    }
}