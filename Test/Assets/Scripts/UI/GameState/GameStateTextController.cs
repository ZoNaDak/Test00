using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.UI {
    public class GameStateTextController : MonoBehaviour {
        private Text ready;
        private Text start;
        private Text timeUp;

        void Awake() {
            this.ready = this.transform.Find("Ready").GetComponent<Text>();
            this.ready.gameObject.SetActive(false);
            this.start = this.transform.Find("Start").GetComponent<Text>();
            this.start.gameObject.SetActive(false);
            this.timeUp = this.transform.Find("TimeUp").GetComponent<Text>();
            this.timeUp.gameObject.SetActive(false);
        }

        public void OnReadyText() {
            this.ready.gameObject.SetActive(true);
        }

        public void OnStartText() {
            this.ready.gameObject.SetActive(false);
            this.start.gameObject.SetActive(true);
        }

        public void OffStartText() {
            this.start.gameObject.SetActive(false);
        }

        public void OnTimeUpText() {
            this.timeUp.gameObject.SetActive(true);
        }

        public void OffTimeUpText() {
            this.timeUp.gameObject.SetActive(false);
        }
    }
}