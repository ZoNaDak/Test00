using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Bubble {
    public enum ECandyType {
        Candy_1,
        Candy_2,
        Candy_3,
        Candy_4,
        Candy_5,
        End
    }

    public class CandyManager : Pattern.MonoSingleton<CandyManager> {
        private const int X_NUM_FOR_CREATE = 5;

        private List<CandyController> bubbleList = new List<CandyController>();

        private void CreateBubbles(int _bubbleNum) {
            GameObject bubblePrefab = Test.Util.PrefabFactory.Instance.FindPrefab("Candy");
            if(bubblePrefab == null) {
                bubblePrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("InGame", "Candy", true);
            }

            for(int i = 0; i < _bubbleNum; ++i) {
                CandyController bubble = Instantiate(bubblePrefab, this.transform).GetComponent<CandyController>();
                bubble.transform.localPosition = new Vector2(
                    -240.0f + (i % X_NUM_FOR_CREATE) * 120.0f
                    , 700.0f + (i / X_NUM_FOR_CREATE) * 120.0f);
                this.bubbleList.Add(bubble);
            }
        }

        public void Initialize(int _bubbleNum) {
            CreateBubbles(_bubbleNum);
        }
    }
}