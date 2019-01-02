using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Candy;

namespace Test.UI.EquipedSkill {
    public class EquipCandyButton : MonoBehaviour {

        private Image equipedCandy;
        private EquipCandyBoardController board;

        private bool onBoard;

        void Awake() {
            this.equipedCandy = this.transform.Find("EquipedCandy").GetComponent<Image>();
            this.board = this.transform.parent.Find("EquipCandyBoard").GetComponent<EquipCandyBoardController>();
            SetEquipCandy(GameController.Instance.EquipedCandy);
        }

        public void SetEquipCandy(ECandyType _type) {
            GameController.Instance.EquipedCandy = _type;
            this.equipedCandy.sprite = Util.SpriteFactory.Instance.GetSprite("Default", _type.ToString());
        }

        public void OnClick() {
            this.board.gameObject.SetActive(!this.board.gameObject.activeSelf);
            this.onBoard = this.board.gameObject.activeSelf;
        }
    }
}