using UdonSharp;
using UnityEngine;
using UnityEngine.UI;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class PanelActivator : UdonSharpBehaviour
    {
        [SerializeField] Selectable[] collection;

        void OnEnable(){
            foreach (Selectable selectable in collection){
                if (selectable != null)
                    selectable.interactable = true;
            }
        }

        void OnDisable(){
            foreach (Selectable selectable in collection){
                if (selectable != null)
                    selectable.interactable = false;
            }
        }
    }
}