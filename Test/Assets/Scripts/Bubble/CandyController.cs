using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Bubble {
    public class CandyController : MonoBehaviour {
        private Rigidbody2D myRigidBody;

        public ECandyType type { get; private set; }

        void Start() {
            this.myRigidBody = this.GetComponent<Rigidbody2D>();
            this.myRigidBody.velocity = Vector2.down * 400.0f;
        }

        void Update() {
            
        }
    }
}