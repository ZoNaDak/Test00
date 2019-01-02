using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Candy;

namespace Test.UI.EquipedSkill {
    public class EquipedSkillController : MonoBehaviour {
        const float MAX_GUAGE_TO_CANDY_NUM = 30.0f;
        const float EXPAND_SCALE_SPEED = 0.03f;
        const float EXPAND_SCALE_MAX = 1.3f;

        private Image candy;
        private Image candyBack;
        private Image gauge;
        private ECandyType equipedCandyType = ECandyType.Candy_00;
        private bool isExapndCandy;
        private bool isCheckedFullGuage;

        private Coroutine fullGaugeCoroutine;

        void Awake() {
            this.candy = this.transform.Find("Candy").GetComponent<Image>();
            this.candyBack = this.transform.Find("CandyBack").GetComponent<Image>();
            this.gauge = this.transform.Find("Gauge").GetComponent<Image>();
        }

        void Start() {
            this.gauge.fillAmount = 0.0f;
            SetEquipedCandy(this.equipedCandyType);
        }

        private void ClearGuage() {
            this.gauge.fillAmount = 0.0f;
            this.isExapndCandy = false;
            this.isCheckedFullGuage = false;
            this.candy.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
            this.StopCoroutine(this.fullGaugeCoroutine);
        }

        public void AddGuage(int _candyNum) {
            this.gauge.fillAmount += (float)_candyNum / MAX_GUAGE_TO_CANDY_NUM;
            if(this.gauge.fillAmount >= 1.0f) {
                if(!this.isCheckedFullGuage) {
                    this.fullGaugeCoroutine = this.StartCoroutine(FullGauge());
                }
            }
        }

        public void SetEquipedCandy(ECandyType _type) {
            this.equipedCandyType = _type;
            this.candy.sprite = Test.Util.SpriteFactory.Instance.GetSprite("Default", _type.ToString());
        }

        public void OnEquipedClickCandy() {
            if(this.isCheckedFullGuage) {
                CandyManager.Instance.UseEquipedCandySkill(this.equipedCandyType);
                ClearGuage();
            }
        }

        //Coroutine
        private IEnumerator FullGauge() {
            this.isExapndCandy = true;
            this.isCheckedFullGuage = true;
            while(true) {
                if(this.isExapndCandy) {
                    this.candy.transform.localScale += new Vector3(EXPAND_SCALE_SPEED, EXPAND_SCALE_SPEED, 0.0f);
                    if(this.candy.transform.localScale.x > EXPAND_SCALE_MAX) {
                        this.candy.transform.localScale = new Vector3(EXPAND_SCALE_MAX, EXPAND_SCALE_MAX, 0.0f);
                        this.isExapndCandy = false;
                    }
                } else {
                    this.candy.transform.localScale -= new Vector3(EXPAND_SCALE_SPEED, EXPAND_SCALE_SPEED, 0.0f);
                    if(this.candy.transform.localScale.x < 1.0f) {
                        this.candy.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                        this.isExapndCandy = true;
                    }
                }
                yield return null;
            }
        }
    }
}