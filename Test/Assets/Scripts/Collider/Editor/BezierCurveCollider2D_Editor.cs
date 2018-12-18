using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Test.Collider2D {
    [CustomEditor (typeof(BezierCurveCollider2D))]
    public class BezierCurveCollider_Editor : Editor {
        BezierCurveCollider2D bezierCurveCollider;

        private void OnEnable() {
            this.bezierCurveCollider = (BezierCurveCollider2D)target;

            if(!this.bezierCurveCollider.initialized) {
                this.bezierCurveCollider.Initialize();
            }
        }

        public override void OnInspectorGUI() {
            GUI.changed = false;
            DrawDefaultInspector();

            if(!this.bezierCurveCollider.edgeCollider.offset.Equals(Vector2.zero)) {
                this.bezierCurveCollider.edgeCollider.offset = Vector2.zero;
            }

            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Point")) {
                this.bezierCurveCollider.AddControlPoint();
            }
            if(this.bezierCurveCollider.controlPoints.Count > 2) {
                if(GUILayout.Button("Remove Point")) {
                    this.bezierCurveCollider.RemoveControlPoint();
                }
            }
            EditorGUILayout.EndHorizontal();

            if(GUILayout.Button("Reset")) {
                this.bezierCurveCollider.initialized = false;
                this.bezierCurveCollider.Initialize();
            }
            if(GUI.changed) {
                this.bezierCurveCollider.DrawCurve();
            }
        }

        private void OnSceneGUI() {
            GUI.changed = false;
            Handles.color = Color.yellow;

            for(int i = 0; i < this.bezierCurveCollider.controlPoints.Count; ++i) {
                Vector2 startPos = this.bezierCurveCollider.controlPoints[i];
                Vector2 newPos = Handles.FreeMoveHandle(this.bezierCurveCollider.controlPoints[i]
                    , Quaternion.identity, 20.0f, Vector3.zero, Handles.ConeHandleCap);
                this.bezierCurveCollider.controlPoints[i] = newPos;

                if(!startPos.Equals(newPos)) {
                    Vector2 offset = newPos - startPos;

                    if(this.bezierCurveCollider.controlPoints.Count == 2) {
                        this.bezierCurveCollider.handlerPoints[i] += offset;
                    } else if(this.bezierCurveCollider.controlPoints.Count > 2) {
                        if(i == 0) {
                            this.bezierCurveCollider.handlerPoints[0] += offset;
                        } else if(i == bezierCurveCollider.controlPoints.Count - 1) {
                            this.bezierCurveCollider.handlerPoints[this.bezierCurveCollider.handlerPoints.Count - 1] += offset;
                        } else {
                            int idx = (i * 2) - 1;
                            bezierCurveCollider.handlerPoints[idx] += offset;
                            bezierCurveCollider.handlerPoints[++idx] += offset;
                        }
                    }
                }
            }

            if(!this.bezierCurveCollider.continous) {
                for(int i = 0; i < this.bezierCurveCollider.handlerPoints.Count; ++i) {
                    this.bezierCurveCollider.handlerPoints[i] = Handles.FreeMoveHandle(
                        this.bezierCurveCollider.handlerPoints[i], Quaternion.identity, 30.0f
                        , Vector3.zero, Handles.ConeHandleCap);
                }
            } else {
                for(int i = 0; i < this.bezierCurveCollider.handlerPoints.Count; ++i) {
                    if(this.bezierCurveCollider.controlPoints.Count == 2) {
                        this.bezierCurveCollider.handlerPoints[i] = Handles.FreeMoveHandle(
                            this.bezierCurveCollider.handlerPoints[i], Quaternion.identity, 30.0f
                            , Vector3.zero, Handles.ConeHandleCap);
                    } else if(this.bezierCurveCollider.controlPoints.Count > 2) {
                        if(i == 0 || i == this.bezierCurveCollider.handlerPoints.Count - 1) {
                            this.bezierCurveCollider.handlerPoints[i] = Handles.FreeMoveHandle(
                            this.bezierCurveCollider.handlerPoints[i], Quaternion.identity, 30.0f
                            , Vector3.zero, Handles.ConeHandleCap); 
                        } else {
                            Vector2 startPos = this.bezierCurveCollider.handlerPoints[i];
                            Vector2 newPos = Handles.FreeMoveHandle(this.bezierCurveCollider.handlerPoints[i]
                                , Quaternion.identity, 30.0f, Vector3.zero, Handles.ConeHandleCap);
                            this.bezierCurveCollider.handlerPoints[i] = newPos;

                            if(!startPos.Equals(newPos)) {
                                bool movedTop = (i % 2 == 1) ? true : false;

                                if(movedTop) {
                                    int controlPtIdx = (i + 1) / 2;

                                    Vector2 dir = bezierCurveCollider.handlerPoints[i] - bezierCurveCollider.controlPoints[controlPtIdx];
                                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                                    angle = (angle + 360) % 360;

                                    float magH2 = Vector2.Distance(
                                        bezierCurveCollider.controlPoints[controlPtIdx]
                                        , bezierCurveCollider.handlerPoints[i + 1]);
                                    angle = (270 - angle);

                                    float x = bezierCurveCollider.controlPoints[controlPtIdx].x + magH2 * Mathf.Sin(angle * Mathf.Deg2Rad);
                                    float y = bezierCurveCollider.controlPoints[controlPtIdx].y + magH2 * Mathf.Cos(angle * Mathf.Deg2Rad);

                                    bezierCurveCollider.handlerPoints[i + 1] = new Vector2(x, y);
                                } else {
                                    int controlPtIdx = i / 2;
                                    Vector2 dir = bezierCurveCollider.controlPoints[controlPtIdx] - bezierCurveCollider.handlerPoints[i];
                                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                                    angle = (angle + 360) % 360;

                                    float magH2 = Vector2.Distance(
                                        bezierCurveCollider.controlPoints[controlPtIdx]
                                        , bezierCurveCollider.handlerPoints[i - 1]);
                                    angle = 360 - angle + 90;

                                    float x = bezierCurveCollider.controlPoints[controlPtIdx].x + magH2 * Mathf.Sin(angle * Mathf.Deg2Rad);
                                    float y = bezierCurveCollider.controlPoints[controlPtIdx].y + magH2 * Mathf.Cos(angle * Mathf.Deg2Rad);

                                    bezierCurveCollider.handlerPoints[i - 1] = new Vector2(x, y);
                                }
                            } 
                        }
                    }
                }
            }

            if(bezierCurveCollider.handlerPoints.Count == 2) {
                Handles.DrawLine(bezierCurveCollider.handlerPoints[0], bezierCurveCollider.controlPoints[0]);
                Handles.DrawLine(bezierCurveCollider.handlerPoints[1], bezierCurveCollider.controlPoints[1]);
            } else {
                int controlPtIdx = 0;
                for(int i = 0; i < bezierCurveCollider.handlerPoints.Count; i = i + 2) {
                    Handles.DrawLine(bezierCurveCollider.handlerPoints[i], bezierCurveCollider.controlPoints[controlPtIdx]);
                    Handles.DrawLine(bezierCurveCollider.handlerPoints[i + 1], bezierCurveCollider.controlPoints[controlPtIdx + 1]);
                }
            }

            if(GUI.changed) {
                bezierCurveCollider.DrawCurve();
            }
        }
    }
}