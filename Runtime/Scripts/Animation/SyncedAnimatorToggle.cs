
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace valenvrc.Common
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual), Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.jpg"), HelpURL("https://discord.gg/nv5ax3wDqc")]
    public class SyncedAnimatorToggle : UdonSharpBehaviour
    {
        [SerializeField] Animator anim;
        [SerializeField] string parameterName;
        int hash;
        [UdonSynced] bool syncedState;

        void Start()
        {
            if (anim == null)
            {
                Debug.LogError("AnimatorToggle: Animator component not found on " + gameObject.name);
                return;
            }
            if (string.IsNullOrEmpty(parameterName))
            {
                Debug.LogError("AnimatorToggle: Parameter name is not set on " + gameObject.name);
                return;
            }
            hash = Animator.StringToHash(parameterName);
            OnDeserialization();
        }

        public void _Toggle()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            syncedState = !syncedState;
            OnDeserialization();
        }

        public override void OnDeserialization()
        {
            if(hash == 0) return; // Prevent issues if deserialization happens before Start
            anim.SetBool(hash, syncedState);
        }

        public override void Interact()
        {
            _Toggle();
        }
    }
}