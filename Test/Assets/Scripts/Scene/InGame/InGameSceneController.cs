using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Candy;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        public const int CANDY_NUM = 42;    
        public const float GRAVITY_SCALE = 50.0f;

        private CandyManager candyManager;

        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -9.81f) * GRAVITY_SCALE;
            this.candyManager = CandyManager.Instance;
            Util.SpriteFactory.Instance.AddAtlas("InGame");
            
        }

        void Start() {
            this.candyManager.Initialize(CANDY_NUM);
        }

        void Update() {
        
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}