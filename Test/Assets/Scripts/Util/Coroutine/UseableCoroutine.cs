using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Util.Coroutine {
    public static class UseableCoroutine {
        public static IEnumerator WaitThenCallback(float _time, Action _callback, UnityEngine.Coroutine _self = null) {
            yield return new WaitForSecondsRealtime(_time);
            _callback();
            _self = null;
        }
    }
}