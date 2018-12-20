using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Bubble;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        public const int BUBBLE_NUM = 42;    
        public const float GRAVITY_SCALE = 40.0f;

        private CandyManager candyManager;

        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -9.81f) * GRAVITY_SCALE;
            this.candyManager = CandyManager.Instance;
            Util.SpriteFactory.Instance.AddAtlas("InGame");
        }

        void Start() {
            this.candyManager.Initialize(BUBBLE_NUM);
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}