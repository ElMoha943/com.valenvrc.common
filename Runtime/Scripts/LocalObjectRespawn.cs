using UnityEngine;
using UdonSharp;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class LocalObjectRespawn : UdonSharpBehaviour
    {
        [SerializeField] GameObject objectToRespawn;
        [SerializeField] GameObject respawnPoint;

        public override void Interact(){
            objectToRespawn.transform.position = respawnPoint.transform.position;
            objectToRespawn.transform.rotation = respawnPoint.transform.rotation;
        }
    }
}