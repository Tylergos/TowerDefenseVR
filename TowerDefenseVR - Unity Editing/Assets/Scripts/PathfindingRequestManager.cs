using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingRequestManager : MonoBehaviour
{
    public static void RequestPath(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callback)
    {

    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _call)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _call;
        }
    }
}
