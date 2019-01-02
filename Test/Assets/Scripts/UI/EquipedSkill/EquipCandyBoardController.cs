using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Candy;

namespace Test.UI.EquipedSkill {
    public class EquipCandyBoardController : MonoBehaviour {
        private EquipCandyButton equipCandy;

        public List<EquipCandyButtonController> candyButtons = new List<EquipCandyButtonController>();

        void Awake() {
            this.equipCandy = this.transform.parent.Find("EquipCandyButton").GetComponent<EquipCandyButton>();
        }

        void Start() {
            for(int i = 0; i < this.candyButtons.Count; ++i) {
                this.candyButtons[i].SetCandyType((ECandyType)i);
            }
        }

        public void SetEquipedCandy(ECandyType _type) {
            this.equipCandy.SetEquipCandy(_type);
        }
    }
}