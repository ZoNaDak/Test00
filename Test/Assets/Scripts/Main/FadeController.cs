using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.Fade {
    public enum EFadeState {
        FadeIn,
        FadeOut,
        End
    }
    public class FadeController : MonoBehaviour {
        private const float FADE_SPEED = 60.0f;

        private Material material;
        private float circleRad;
        private float maxCircleRad;

        public EFadeState State { get; private set; }

        void Awake() {
            this.material = this.GetComponent<SpriteRenderer>().material;
            this.maxCircleRad = this.transform.lossyScale.y;
            this.circleRad = this.maxCircleRad;
        }

        void Start() {
            
        }
        
        void Update() {
        }

        public void StartFadeIn() {
            this.StartCoroutine(FadeIn());
        }

        public void StartFadeOut() {
            this.StartCoroutine(FadeOut());
        }

        //Coroutine
        private IEnumerator FadeIn() {
            this.State = EFadeState.FadeIn;
            while(circleRad < this.maxCircleRad) {
                this.circleRad += FADE_SPEED;
                this.material.SetFloat("_Rad", this.circleRad / this.maxCircleRad);
                yield return new WaitForFixedUpdate();
            }
            this.circleRad = this.maxCircleRad;
            this.State = EFadeState.End;
        }

        private IEnumerator FadeOut() {
            this.State = EFadeState.FadeOut;
            while(circleRad > 0.0f) {
                this.circleRad -= FADE_SPEED;
                this.material.SetFloat("_Rad", this.circleRad / this.maxCircleRad);
                yield return new WaitForFixedUpdate();
            }
            this.circleRad = 0.0f;
            this.State = EFadeState.End;
        }
    }
}