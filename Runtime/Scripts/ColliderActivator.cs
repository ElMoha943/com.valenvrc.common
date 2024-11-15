﻿using UdonSharp;
using UnityEngine;

namespace valenvrc.Common{
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync),Icon("Packages/com.valenvrc.common/Runtime/PromotionalImages/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
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