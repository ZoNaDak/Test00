using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Util.Coroutine;

namespace Test.UI.Combo {
    public class ComboFontController : MonoBehaviour {
        const float SPACE_X = -20.0f;
        const float SPACE_Y = 40.0f;
        const float MAX_POS_X = 200.0f;
        const float ON_COMBO_TIME = 1.5f;
        const float START_OFF_ALPHA_TIME = 0.75f;
        const float COMBO_MOVE_SPEED = 1.0f;
        const float ALPAH_DOWN_SPEED = 0.2f;

        private Text linkText;
        private Text comboText;
        private bool isOnCombo;
        private bool isOffComboAlpha;

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
            this.isOnCombo = true;
            this.comboText.color = new Color(this.comboText.color.r, this.comboText.color.g, this.comboText.color.b, 1.0f);
            this.comboText.gameObject.SetActive(true);
            this.comboText.text = string.Format("{0} Combo", _combo.ToString());
            if(_pos.x < -MAX_POS_X) {
                _pos.x = -MAX_POS_X;
            } else if(_pos.x > MAX_POS_X) {
                _pos.x = MAX_POS_X;
            }
            this.transform.position = _pos;
            StartCoroutine(MoveCombo());
            StartCoroutine(UseableCoroutine.WaitThenCallback(START_OFF_ALPHA_TIME, () => this.isOffComboAlpha = true));
            StartCoroutine(UseableCoroutine.WaitThenCallback(ON_COMBO_TIME
                , ()=> {
                this.comboText.gameObject.SetActive(false);
                this.isOnCombo = false;
                this.isOffComboAlpha = false;
                }
                ));
        }

        public IEnumerator MoveCombo() {
            while(this.isOnCombo) {
                this.transform.Translate(0.0f, COMBO_MOVE_SPEED, 0.0f);
                if(this.isOffComboAlpha) {
                    this.comboText.color = new Color(
                        this.comboText.color.r
                        , this.comboText.color.g
                        , this.comboText.color.b
                        , this.comboText.color.a - ALPAH_DOWN_SPEED);
                }
                yield return null;
            }
        }
    }
}