using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace valenvrc.Common{

    public enum PickupInvokeType{
        Invoke,
        Toggle,
        Sound,
        Particle,
        Animation
    }

    [UdonBehaviourSyncMode(BehaviourSyncMode.None),Icon("Packages/com.valenvrc.common/Editor/Resources/ValenFace.png"), HelpURL("https://docs.valenvrc.com/valencommons/pickups#pickupinvoke")]
    public class PickupInvoke : UdonSharpBehaviour
    {
        public PickupInvokeType type;

        //Invoke
        [SerializeField] string methodName;
        [SerializeField] UdonBehaviour target;

        //Toggle
        [SerializeField] GameObject[] toggleObjects;

        //Sound
        [SerializeField] AudioSource audioSource;

        //Particle
        [SerializeField] ParticleSystem particle;

        //Animation
        [SerializeField] Animator animator;
        [SerializeField] string triggerName;

        public override void OnPickupUseDown(){
            switch(type){
                case PickupInvokeType.Invoke:
                    HandleInvoke();
                    break;
                case PickupInvokeType.Toggle:
                    HandleToggle();
                    break;
                case PickupInvokeType.Sound:
                    HandleSound();
                    break;
                case PickupInvokeType.Particle:
                    HandleParticle();
                    break;
                case PickupInvokeType.Animation:
                    HandleAnimation();
                    break;
            }
        }

        void HandleInvoke(){
            target.SendCustomEvent(methodName);
        }

        void HandleToggle(){
            foreach(GameObject obj in toggleObjects){
                obj.SetActive(!obj.activeSelf);
            }
        }

        void HandleSound(){
            audioSource.Play();
        }

        void HandleParticle(){
            particle.Play();
        }

        void HandleAnimation(){
            animator.SetTrigger(triggerName);
        }
    }
}