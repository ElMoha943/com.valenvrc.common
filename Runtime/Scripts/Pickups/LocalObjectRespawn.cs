using UnityEngine;
using UdonSharp;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class LocalObjectRespawn : UdonSharpBehaviour
    {
        [SerializeField] GameObject objectToRespawn;
        [SerializeField,Tooltip("Where to respawn the object, if null default position will be used")] Transform respawnPoint;

        Vector3 respawnPosition;
        Quaternion respawnRotation;

        void Start(){
            if (objectToRespawn == null){
                Debug.LogError("Object to respawn is not assigned.", this);
                gameObject.SetActive(false);
                return;
            }
            if (respawnPoint == null){
                respawnPosition = objectToRespawn.transform.position;
                respawnRotation = objectToRespawn.transform.rotation;
            }
            else{
                respawnPosition = respawnPoint.position;
                respawnRotation = respawnPoint.rotation;
            }
        }

        public override void Interact(){
            _Respawn();
        }

        public void _Respawn(){
            objectToRespawn.transform.position = respawnPosition;
            objectToRespawn.transform.rotation = respawnRotation;
        }
    }
}