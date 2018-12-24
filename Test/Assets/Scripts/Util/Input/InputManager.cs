using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Util.MyInput {
    public static class InputManager {
        public static bool TouchStart() {
            #if UNITY_EDITOR
                return Input.GetMouseButtonDown(0);
            #else
                if(Input.touchCount == 0) {
                    return false;
                }

                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Began;
            #endif
        }

        public static bool Touching() {
            #if UNITY_EDITOR
                return Input.GetMouseButton(0);
            #else
                if(Input.touchCount == 0) {
                    return false;
                }

                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            #endif
        }

        public static bool TouchEnd() {
            #if UNITY_EDITOR
                return Input.GetMouseButtonUp(0);
            #else
                if(Input.touchCount == 0) {
                    return false;
                }

                Touch touch = Input.GetTouch(0);
                return touch.phase == TouchPhase.Ended;
            #endif
        }

        public static Ray GetTouchPointRay() {
            #if UNITY_EDITOR
                return Camera.main.ScreenPointToRay(Input.mousePosition);
            #else
                if(Input.touchCount == 0) {
                    return default(Ray);
                }

                Touch touch = Input.GetTouch(0);
                return Camera.main.ScreenPointToRay(touch.position);
            #endif
        }
    }
}