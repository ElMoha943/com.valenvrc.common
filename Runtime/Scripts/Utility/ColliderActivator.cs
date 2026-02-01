using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/utility#collideractivator")]
    public class ColliderActivator : UdonSharpBehaviour
    {
        [SerializeField] Collider[] colliders;

        void OnEnable(){
            foreach(Collider collider in colliders){
                if(collider != null)
                    collider.enabled = true;
            }
        }

        void OnDisable() {
            foreach(Collider collider in colliders){
                if(collider != null)
                    collider.enabled = false;
            }
        }
    }
}
