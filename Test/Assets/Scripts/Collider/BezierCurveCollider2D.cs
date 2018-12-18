#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Collider2D {
    [AddComponentMenu("Physics 2D/Bezier Curve Collider 2D")]

    [RequireComponent(typeof(EdgeCollider2D))]
    public class BezierCurveCollider2D : MonoBehaviour {
        [Range(3,36)]
        public int smoothness = 15;

        [HideInInspector]
        public bool initialized;
        [HideInInspector]
        public EdgeCollider2D edgeCollider;

        public bool continous = true;
        public List<Vector2> controlPoints, handlerPoints;

        private List<Vector2> points;
        private Vector2 origin, center;

        private void DrawSegment(Vector2 _controlPt0, Vector2 _controlPt1, Vector2 _handlerPt0, Vector2 _handlerPt1) {
            this.points.Add(_controlPt0);
            for(int i = 1; i < smoothness; ++i) {
                this.points.Add(CalculateBezierPoint((1.0f / smoothness) * i, _controlPt0, _controlPt1, _handlerPt0, _handlerPt1));
            }
            this.points.Add(_controlPt1);
        }

        private Vector3 CalculateBezierPoint(float _t, Vector2 _controlPt0, Vector2 _controlPt1, Vector2 _handlerPt0, Vector2 _handlerPt1) {
            return (Mathf.Pow((1.0f - _t), 3) * _controlPt0) + (3 * Mathf.Pow((1 - _t), 2) * _t * _handlerPt0)
             + (3 * (1.0f - _t) * Mathf.Pow(_t, 2) * _handlerPt1) + (Mathf.Pow(_t, 3) * _controlPt1);
        }

        public void Initialize() {
            if(this.initialized) {
                return;
            }

            this.initialized = true;
            this.continous = true;
            this.smoothness = 15;

            this.controlPoints = new List<Vector2>();
            this.handlerPoints = new List<Vector2>();

            Vector2 pos = transform.localPosition;
            this.controlPoints.Add(pos);

            pos.x += 4;
            this.controlPoints.Add(pos);

            pos.x -= 4;
            pos.y += 4;
            this.handlerPoints.Add(pos);

            pos.x += 4;
            this.handlerPoints.Add(pos);
            
            DrawCurve();
        }

        public void DrawCurve() {
            this.points = new List<Vector2>();
    
            if(this.edgeCollider == null) {
                this.edgeCollider = this.gameObject.AddComponent<EdgeCollider2D>();
            }

            if(this.controlPoints.Count == 2) {
                DrawSegment(this.controlPoints[0], this.controlPoints[1], this.handlerPoints[0], this.handlerPoints[1]);
            } else if (this.controlPoints.Count > 2) {
                int handlerPtsIdx = 0;
                for(int i = 0; i < this.controlPoints.Count - 1; ++i) {
                    DrawSegment(this.controlPoints[i], this.controlPoints[i+1], this.handlerPoints[handlerPtsIdx], this.handlerPoints[handlerPtsIdx+1]);
                    handlerPtsIdx += 2;
                }
            }

            this.edgeCollider.points = this.points.ToArray();
        }

        public void AddControlPoint() {
            Vector2 pos = this.controlPoints[this.controlPoints.Count - 1];
            float handlerPosY = this.handlerPoints[this.handlerPoints.Count - 1].y;

            float mul = (handlerPosY > pos.y) ? -1 : 1;

            this.handlerPoints.Add(new Vector2(pos.x, pos.y + (4 * mul)));

            pos.x += 4;
            this.controlPoints.Add(pos);

            pos.y += 4 * mul;
            this.handlerPoints.Add(pos);

            DrawCurve();
        }

        public void RemoveControlPoint() {
            if(this.controlPoints.Count > 2) {
                this.controlPoints.RemoveAt(this.controlPoints.Count - 1);
                this.handlerPoints.RemoveRange(this.handlerPoints.Count - 2, 2);
            }
        }
    }
}
#endif