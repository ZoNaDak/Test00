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
        private ECandyType selectedCandyType;
        private bool isClicked;
        private bool isClickable;

        void Awake() {
            //Create Line
            GameObject linePrefab = PrefabFactory.Instance.FindPrefab("Line");
            if(linePrefab == null) {
                linePrefab = PrefabFactory.Instance.CreatePrefab("InGame", "Line", true);
            }
            this.lineForCandy = Instantiate(linePrefab, this.transform.parent).GetComponent<LineController>();
            this.lineForCandy.Clear();
        }

        void Update() {
            if(isClickable) {
                CheckToClickCandy();
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

            Vector2 candyScale = candyPrefab.transform.lossyScale;
            float startPos_X = GetCandyStartPosX(_candyNum, candyScale.x);
            
            for(int i = 0; i < _candyNum; ++i) {
                CandyController candy = Instantiate(candyPrefab, this.transform).GetComponent<CandyController>();
                candy.transform.localPosition = new Vector2(
                    startPos_X + (i % X_NUM_FOR_CREATE) * (candyScale.x + SPACE_FOR_CREATE)
                    , START_POS_Y_FOR_CREATE + (i / X_NUM_FOR_CREATE) * (candyScale.y + SPACE_FOR_CREATE));
                candy.SetType((ECandyType)Random.Range(0, (int)ECandyType.End - 1));
                candy.gameObject.SetActive(false);
                this.candyList.Add(candy);
            }
        }

        private void CheckToClickCandy() {
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
            if(this.selectedCandy.Count >= 3) {
                float startPos_X = GetCandyStartPosX(this.selectedCandy.Count, this.selectedCandy.First().transform.lossyScale.x);
                for(int i = 0; i < this.selectedCandy.Count; ++i) {
                    this.selectedCandy[i].transform.localPosition = new Vector2(
                    startPos_X + (i % X_NUM_FOR_CREATE) * (selectedCandy[i].transform.lossyScale.x + SPACE_FOR_CREATE)
                    , START_POS_Y_FOR_CREATE + (i / X_NUM_FOR_CREATE) * (selectedCandy[i].transform.lossyScale.y + SPACE_FOR_CREATE));
                    this.selectedCandy[i].SetType((ECandyType)Random.Range(0, (int)ECandyType.End - 1));
                }
                int score = 100 * this.selectedCandy.Count * this.selectedCandy.Count;
                UI.UICanvasController.Instance.Score.AddScore(score);
            }

            this.lineForCandy.Clear();
            for(int i = 0; i < this.selectedCandy.Count; ++i) {
                this.selectedCandy[i].DeselectMe();
            }
            this.selectedCandy.Clear();
        }

        private float GetCandyStartPosX(int _candyNum, float candyScale_X) {
            if(_candyNum < X_NUM_FOR_CREATE) {
                if(_candyNum % 2 == 0) {
                    return -_candyNum / 2 * (candyScale_X + SPACE_FOR_CREATE) + (candyScale_X + SPACE_FOR_CREATE) * 0.5f;
                } else {
                    return -_candyNum / 2 * (candyScale_X + SPACE_FOR_CREATE);
                }
            } else {
                if(X_NUM_FOR_CREATE % 2 == 0) {
                    return -X_NUM_FOR_CREATE / 2 * (candyScale_X + SPACE_FOR_CREATE) + (candyScale_X + SPACE_FOR_CREATE) * 0.5f;
                } else {
                    return -X_NUM_FOR_CREATE / 2 * (candyScale_X + SPACE_FOR_CREATE); 
                }
            }

        }

        public void Initialize(int _candyNum) {
            CreateCandies(_candyNum);
            //Initialzie LineForCandy
            this.lineForCandy.Initialize(_candyNum);
        }

        public void StartGame() {
            for(int i = 0; i < this.candyList.Count; ++i) {
                this.candyList[i].gameObject.SetActive(true);
            }
            this.isClickable = true;
        }

        public void TimeUpGame() {
            this.isClickable = false;
        }
    }
}