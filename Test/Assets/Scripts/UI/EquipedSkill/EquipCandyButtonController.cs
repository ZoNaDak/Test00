using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Candy;

namespace Test.UI.EquipedSkill {
    public class EquipCandyButtonController : MonoBehaviour {
        private ECandyType type;

        public void SetCandyType(ECandyType _type) {
            this.type = _type;
        }

        public void OnClick() {
            this.transform.parent.GetComponent<EquipCandyBoardController>().SetEquipedCandy(this.type);
        }
    }
}