using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
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
