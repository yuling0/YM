using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class AStarUnit : MonoBehaviour
    {
        public float speed = 20f;
        public float turnDst = 5f;
        public float trunSpeed = 3f;
        Vector3[] wayPoints;
        Coroutine currentCoroutine;
        PathRequestManager requestManager;
        //int targetIndex;

        AStarPath path;
        Rigidbody rig;
        private void Awake()
        {
            rig = GetComponent<Rigidbody>();
            requestManager = PathRequestManager.Instance;
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000))
                {
                    requestManager.PathRequest(transform.position, hit.point, OnPathFinding);
                }
            }

        }

        void OnPathFinding(Vector3[] wayPoints, bool success)
        {
            if (success)
            {
                this.wayPoints = wayPoints;
                path = new AStarPath(wayPoints, transform.position, turnDst);

                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }


                currentCoroutine = StartCoroutine(Movement());
            }
        }
        private IEnumerator Movement1()
        {
            int currentIndex = 0;
            transform.LookAt(path.lookPoints[0]);
            while (currentIndex < wayPoints.Length)
            {
                Vector3 lookpos = wayPoints[currentIndex];
                lookpos.y = transform.position.y;

                if (Vector3.Distance(lookpos, transform.position) < 0.1f)
                {
                    currentIndex++;
                }
                if (currentIndex >= wayPoints.Length)
                {
                    break;
                }
                lookpos = wayPoints[currentIndex];
                lookpos.y = transform.position.y;

                Quaternion lookRotation = Quaternion.LookRotation(lookpos - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, trunSpeed * Time.deltaTime);
                //transform.Translate(Vector3.forward * speed * Time.deltaTime);
                rig.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
                yield return null;
            }

        }
        private IEnumerator Movement()
        {
            int currentIndex = 0;
            bool isFollowPath = true;
            transform.LookAt(path.lookPoints[0]);
            while (isFollowPath)
            {
                Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
                while (path.turnBoundaries[currentIndex].HasCrossedLine(pos2D))
                {
                    if (currentIndex == path.finishLineIndex)
                    {
                        isFollowPath = false;
                        break;
                    }
                    else
                    {
                        currentIndex++;
                    }

                }
                if (isFollowPath)
                {
                    Vector3 lookpos = path.lookPoints[currentIndex];
                    lookpos.y = transform.position.y;
                    Quaternion lookRotation = Quaternion.LookRotation(lookpos - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, trunSpeed * Time.deltaTime);
                    //transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    rig.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
                }

                yield return null;
            }

        }

        private void OnDrawGizmos()
        {
            if (path != null)
            {
                path.DrawWithGizmos();
            }
        }
    }

}
