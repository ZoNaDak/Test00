using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Util;

namespace Test.Candy {
    public enum ECandyType {
        Candy_00,
        Candy_01,
        Candy_02,
        Candy_03,
        Candy_04,
        End
    }

    public class CandyManager : Pattern.MonoSingleton<CandyManager> {
        private const int X_NUM_FOR_CREATE = 7;
        private const float SPACE_FOR_CREATE = 15.0f;
        private const float START_POS_Y_FOR_CREATE = 700.0f;

        private List<CandyController> candyList = new List<CandyController>();
        private LineController lineForCandy;

        void Awake() {
            //Create Line
            GameObject linePrefab = PrefabFactory.Instance.FindPrefab("Line");
            if(linePrefab == null) {
                linePrefab = PrefabFactory.Instance.CreatePrefab("InGame", "Line", true);
            }
            this.lineForCandy = Instantiate(linePrefab, this.transform.parent).GetComponent<LineController>();
        }

        void Start() {
            
        }

        private void CreateCandies(int _candyNum) {
            GameObject candyPrefab = Test.Util.PrefabFactory.Instance.FindPrefab("Candy");
            if(candyPrefab == null) {
                candyPrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("InGame", "Candy", true);
            }

            float startPos_X;
            Vector2 candyScale = candyPrefab.transform.lossyScale;
            if(X_NUM_FOR_CREATE % 2 == 0) {
                startPos_X = -X_NUM_FOR_CREATE / 2 * (candyScale.x + SPACE_FOR_CREATE) + (candyScale.x + SPACE_FOR_CREATE) * 0.5f;
            } else {
                startPos_X = -X_NUM_FOR_CREATE / 2 * (candyScale.x + SPACE_FOR_CREATE); 
            }
            
            for(int i = 0; i < _candyNum; ++i) {
                CandyController bubble = Instantiate(candyPrefab, this.transform).GetComponent<CandyController>();
                bubble.transform.localPosition = new Vector2(
                    startPos_X + (i % X_NUM_FOR_CREATE) * (candyScale.x + SPACE_FOR_CREATE)
                    , START_POS_Y_FOR_CREATE + (i / X_NUM_FOR_CREATE) * (candyScale.y + SPACE_FOR_CREATE));
                bubble.SetType((ECandyType)Random.Range(0, (int)ECandyType.End - 1));
                this.candyList.Add(bubble);
            }
        }

        public void Initialize(int _candyNum) {
            CreateCandies(_candyNum);
            //Initialzie LineForCandy
            this.lineForCandy.Initialize(_candyNum);
            this.lineForCandy.Clear();
        }

        public void UpdateTheClickOfCandy(Vector2 _candyPos) {

        }
    }
}