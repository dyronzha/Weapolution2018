using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    const float minPathUpdateTime = 0.2f;
    const float pathUpdateMoveThreshold = 0.5f;

    public Transform target;
    public float speed = 5;
    public float turnSpeed = 1;
    public float turnDis = 2;
    public float stoppingDis = 2.0f;
    Vector3 goWay;
    Path path;
    

    private void Start()
    {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }
    public void OnPathFound(Vector3 []wayPoints, bool pathSuccessful ) {
        //transform.Translate(Vector3.forward,Space.Self);
        if (pathSuccessful) {
            Debug.Log(gameObject.name + "find path");
            path = new Path(wayPoints, transform.position, turnDis, stoppingDis);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (((target.position - targetPosOld).V3NormalizedtoV2()).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath() {
        //Vector3 currentWayPoint = path[0];
        bool followingPath = true;
        int pathIndex = 0;
        float speedPercent = 1;
        while (followingPath) {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
            while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.finishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            if (followingPath)
            {

                if (pathIndex >= path.slowDownIndex && stoppingDis > 0)
                {
                    speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDis);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                if (pathIndex == 0) goWay = (path.lookPoints[0] - transform.position).V3NormalizedtoV2();
                else {
                    Vector3 nextWay = (path.lookPoints[pathIndex] - transform.position).V3NormalizedtoV2();
                    goWay = Vector3.Lerp(goWay, nextWay, Time.deltaTime * turnSpeed);
                } 
                //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                transform.position += goWay* speed * Time.deltaTime;
            }
            yield return null;
            //if (transform.position == currentWayPoint) {
            //    targetIndex++;
            //    if (targetIndex >= path.Length) {
            //        yield break;
            //    }
            //    currentWayPoint = path[targetIndex];
            //}
            //transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);

        }
    }
    private void OnDrawGizmos()
    {
        if (path != null) { 
            path.DrawWithGizmos();
            //for (int i = targetIndex; i< path.Length; i++) {
            //    Gizmos.color = Color.black;
            //    Gizmos.DrawCube(path[i], Vector3.one);
            //    if (i == targetIndex)
            //    {
            //        Gizmos.DrawLine(transform.position, path[i]);
            //    }
            //    else {
            //        Gizmos.DrawLine(path[i], path[i-1]);
            //    }
            //}
        }
    }
}
