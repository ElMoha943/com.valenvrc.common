using UdonSharp;
using UnityEngine.UI;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class ButtonCooldown : UdonSharpBehaviour
    {
        void setCooldown(){
            Selectable i = GetComponent<Selectable>();
            i.interactable = false;
            SendCustomEventDelayedSeconds("resetCooldown", 2);
        }

        void resetCooldown(){
            Selectable i = GetComponent<Selectable>();
            i.interactable = true;
        }
    }
}
