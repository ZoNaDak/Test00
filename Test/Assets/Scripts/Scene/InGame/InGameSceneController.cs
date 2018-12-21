using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Bubble;
using Test.Util;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        public const int CANDY_NUM = 42;    
        public const float GRAVITY_SCALE = 50.0f;

        [SerializeField] private GameObject levelCanvas;
        [SerializeField] private GameObject candyCanvas;
        [SerializeField] private GameObject uiCanvas;

        private CandyManager candyManager;
        private LineController lineForCandy;

        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -9.81f) * GRAVITY_SCALE;
            this.candyManager = CandyManager.Instance;
            Util.SpriteFactory.Instance.AddAtlas("InGame");
            //Create Line
            GameObject linePrefab = PrefabFactory.Instance.FindPrefab("Line");
            if(linePrefab == null) {
                linePrefab = PrefabFactory.Instance.CreatePrefab("InGame", "Line", true);
            }
            this.lineForCandy = Instantiate(linePrefab, this.candyCanvas.transform).GetComponent<LineController>();
        }

        void Start() {
            this.candyManager.Initialize(CANDY_NUM);
            this.lineForCandy.Initialize(CANDY_NUM);
            this.lineForCandy.Clear();
        }

        void Update() {
        
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}