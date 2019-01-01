using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.UI.Combo {
    public class ComboFontController : MonoBehaviour {
        const float SPACE_X = -20.0f;
        const float SPACE_Y = 40.0f;

        private Text linkText;
        private Text comboText;

        void Awake() {
            this.linkText = this.transform.Find("LinkText").GetComponent<Text>();
            this.comboText = this.transform.Find("ComboText").GetComponent<Text>();
        }

        public void OnLink(int _linkNum, Vector2 _pos) {
            this.linkText.gameObject.SetActive(true);
            this.linkText.text = _linkNum.ToString();
            this.transform.position = _pos + new Vector2(SPACE_X, SPACE_Y);
        }

        public void OffLink() {
            this.linkText.gameObject.SetActive(false);
        }

        public void OnCombo(int _combo, Vector2 _pos) {
            
        }
    }
}