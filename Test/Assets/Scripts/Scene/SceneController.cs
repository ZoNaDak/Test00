using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public enum SceneType {
        Title,
        InGame,
        End
    }

    public abstract class SceneController : MonoBehaviour {
        public SceneType sceneType { get; protected set; } = SceneType.End;
        public bool CanChangeNextStep { get; protected set;} = false;
    }
}