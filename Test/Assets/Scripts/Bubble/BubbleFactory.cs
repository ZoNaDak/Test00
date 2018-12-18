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

    public class BubbleFactory : Pattern.Singleton<BubbleFactory> {
        private List<BubbleController> bubbleList = new List<BubbleController>();

        private void CreateBubbles(int _bubbleNum) {

        }

        public void Initialize(int _bubbleNum) {
            CreateBubbles(_bubbleNum);
        }
    }
}