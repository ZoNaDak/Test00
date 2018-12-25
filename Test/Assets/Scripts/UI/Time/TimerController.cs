using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.UI {
    public class TimerController : Pattern.MonoSingleton<TimerController> {
        private Image innerImage;
        private Text remainTimeText;
        
        public float RemainTime { get; private set; }
        public float MaxRemainTime { get; private set; }

        void Awake() {
            this.innerImage = this.transform.Find("InnerImage").GetComponent<Image>();
            this.remainTimeText = this.transform.Find("TimeText").GetComponent<Text>();
        }

        public void Initialize(float _startTime) {
            this.RemainTime = _startTime;
            this.MaxRemainTime = _startTime;
            this.remainTimeText.text = this.RemainTime.ToString();
            this.innerImage.fillAmount = 1.0f;
        }

        public void StartCountDown() {
            this.StartCoroutine(CountDown());
        }

        //Coroutine
        private IEnumerator CountDown() {
            while(this.RemainTime > 0.0f) {
                this.RemainTime -= Time.fixedDeltaTime;
                this.remainTimeText.text = ((int)this.RemainTime).ToString();
                this.innerImage.fillAmount = this.RemainTime / this.MaxRemainTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}