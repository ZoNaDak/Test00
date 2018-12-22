using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Candy;

namespace Test.MyInput {
    public class InputForCandy : Pattern.MonoSingleton<InputForCandy> {
        const float MAX_RANGE_FOR_SELECT = 150.0f;

        private CandyManager candyManager;

        private bool isClicked;
        private GameObject clickedCandy;
        private ECandyType selectedCandyType;

        void Awake() {
            this.candyManager = this.GetComponent<CandyManager>();
        }

        void OnDrawGizmos() {
            if(this.isClicked) {
                Gizmos.color = Color.yellow;
			    Gizmos.DrawWireSphere(this.candyManager.LastSelectedCandy.transform.position, MAX_RANGE_FOR_SELECT);
            }
        }

        void FixedUpdate() {
            if(Input.GetMouseButtonDown(0) && !this.isClicked) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 15.0f, 1 << LayerMask.NameToLayer("Candy"));
                if(rayHit) {
                    CandyController candy = rayHit.transform.gameObject.GetComponent<CandyController>();
                    this.selectedCandyType = candy.Type;
                    this.candyManager.AddSelectedCandy(candy);
                }
                this.isClicked = true;
            } else if(Input.GetMouseButton(0) && this.isClicked) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 15.0f, 1 << LayerMask.NameToLayer("Candy"));
                if(rayHit) {
                    if(this.candyManager.LastSelectedCandy.gameObject != rayHit.transform.gameObject) {
                        if(this.candyManager.SecondLastSelectedCandy != null 
                        && this.candyManager.SecondLastSelectedCandy.gameObject == rayHit.transform.gameObject) {
                            this.candyManager.RemoveLastSelectedCandy();
                        } else {
                            CandyController candy = rayHit.transform.gameObject.GetComponent<CandyController>();
                            if(!candy.Selected && candy.Type == this.selectedCandyType 
                            && MAX_RANGE_FOR_SELECT > Vector2.Distance(this.candyManager.LastSelectedCandy.transform.position, candy.transform.position)) {
                                this.candyManager.AddSelectedCandy(candy);
                            }
                        }
                    }
                }
            } else if(Input.GetMouseButtonUp(0)) {
                this.candyManager.PangCandies();
                this.isClicked = false;
            }
        }
    }
}