using UdonSharp;
using UnityEngine.UI;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class ButtonCooldown : UdonSharpBehaviour
    {
        [SerializeField] float cooldown = 2;

        public void _setCooldown(){
            Selectable i = GetComponent<Selectable>();
            i.interactable = false;
            SendCustomEventDelayedSeconds("_resetCooldown", cooldown);
        }

        public void _resetCooldown(){
            Selectable i = GetComponent<Selectable>();
            i.interactable = true;
        }
    }
}
