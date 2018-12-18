using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Bubble {
    public enum EBubbleType {
        Char1,
        Char2,
        Char3,
        Char4,
        Char5,
        End
    }

    public class BubbleManager : Pattern.MonoSingleton<BubbleManager> {
        private const int X_NUM_FOR_CREATE = 5;

        private List<BubbleController> bubbleList = new List<BubbleController>();

        private void CreateBubbles(int _bubbleNum) {
            GameObject bubblePrefab = Test.Util.PrefabFactory.Instance.FindPrefab("Bubble");
            if(bubblePrefab == null) {
                bubblePrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("InGame", "Bubble", true);
            }

            for(int i = 0; i < _bubbleNum; ++i) {
                BubbleController bubble = Instantiate(bubblePrefab, this.transform).GetComponent<BubbleController>();
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