using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Test.Util;
using Test.Util.ExtensionMethod;
using Test.Util.MyInput;

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
        private const float MAX_RANGE_FOR_SELECT = 150.0f;

        private List<CandyController> candyList = new List<CandyController>();
        private List<CandyController> selectedCandy = new List<CandyController>();
        private LineController lineForCandy;
        private GameObject clickedCandy;
        private bool isClicked;
        private ECandyType selectedCandyType;

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

        void Update() {
            if(InputManager.TouchStart() && !this.isClicked) {
                Ray ray = InputManager.GetTouchPointRay();
                RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 15.0f, 1 << LayerMask.NameToLayer("Candy"));
                if(rayHit) {
                    CandyController candy = rayHit.transform.gameObject.GetComponent<CandyController>();
                    this.selectedCandyType = candy.Type;
                    AddSelectedCandy(candy);
                }
                this.isClicked = true;
            } else if(InputManager.Touching() && this.isClicked) {
                Ray ray = InputManager.GetTouchPointRay();
                RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, 15.0f, 1 << LayerMask.NameToLayer("Candy"));
                if(rayHit) {
                    if(this.selectedCandy.Last().gameObject != rayHit.transform.gameObject) {
                        if(this.selectedCandy.Count > 1
                        && this.selectedCandy.SecondLast().gameObject == rayHit.transform.gameObject) {
                            RemoveLastSelectedCandy();
                        } else {
                            CandyController candy = rayHit.transform.gameObject.GetComponent<CandyController>();
                            if(!candy.Selected && candy.Type == this.selectedCandyType 
                            && MAX_RANGE_FOR_SELECT > Vector2.Distance(this.selectedCandy.Last().transform.position, candy.transform.position)) {
                                AddSelectedCandy(candy);
                            }
                        }
                    }
                }
            } else if(InputManager.TouchEnd()) {
                PangCandies();
                this.isClicked = false;
            }
        }

        void OnDrawGizmos() {
            if(this.isClicked) {
                Gizmos.color = Color.yellow;
			    Gizmos.DrawWireSphere(this.selectedCandy.Last().transform.position, MAX_RANGE_FOR_SELECT);
            }
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

        private void AddSelectedCandy(CandyController _candy) {
            this.lineForCandy.AddPoint(_candy.gameObject);
            _candy.SelectMe();
            this.selectedCandy.Add(_candy.GetComponent<CandyController>());
        }

        private void RemoveLastSelectedCandy() {
            this.lineForCandy.RemoveLastPoint();
            this.selectedCandy[this.selectedCandy.Count - 1].DeselectMe();
            this.selectedCandy.RemoveAt(this.selectedCandy.Count - 1);
        }

        private void PangCandies() {
            this.lineForCandy.Clear();
            for(int i = 0; i < this.selectedCandy.Count; ++i) {
                this.selectedCandy[i].DeselectMe();
            }
            this.selectedCandy.Clear();

            if(this.selectedCandy.Count < 3) {
                return;
            }
        }

        public void Initialize(int _candyNum) {
            CreateCandies(_candyNum);
            //Initialzie LineForCandy
            this.lineForCandy.Initialize(_candyNum);
            this.lineForCandy.Clear();
        }
    }
}