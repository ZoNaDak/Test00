using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Candy {
    public class CandyController : MonoBehaviour {
        private const float START_SPEED = 600.0f;
    
        private Rigidbody2D myRigidBody;
        private SpriteRenderer mySpriteRenderer;

        public ECandyType Type { get; private set; }
        public bool Selected { get; private set; }

        void Awake() {
            this.myRigidBody = this.GetComponent<Rigidbody2D>();
            this.mySpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        void Start() {
            this.myRigidBody.velocity = Vector2.down * START_SPEED;
        }

        void Update() {
            
        }

        public void SetType(ECandyType _type) {
            this.Type = _type;
            this.mySpriteRenderer.sprite = Util.SpriteFactory.Instance.GetSprite("Default", this.Type.ToString());
        }

        public void SelectMe() {
            this.Selected = true;
        }

        public void DeselectMe() {
            this.Selected = false;
        }
    }
}