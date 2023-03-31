using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public NavMeshAgent agent;
    public Transform target;
    public Transform myTransform;

    void Start()
    {
        agent.SetDestination(target.position);
    }

    void Update()
    {
        NavMeshPath path = agent.path;
        Vector3[] pathCorners = path.corners;

        // Get the height of each corner and set the positions of the line renderer to match the corners of the path
        List<Vector3> pathPositions = new List<Vector3>();
        for (int i = 0; i < pathCorners.Length; i++)
        {
            Vector3 corner = pathCorners[i];
            RaycastHit hit;
            if (Physics.Raycast(corner + Vector3.up * 100, Vector3.down, out hit, 200f))
            {
                float offset = 0.7f;
                if (i == 0)
                {
                    offset = 0f;
                }
                else if (i == pathCorners.Length - 1)
                {
                    offset = 0.2f;
                }
                Vector3 pathPosition = new Vector3(corner.x, hit.point.y + offset, corner.z);
                pathPositions.Add(pathPosition);
            }
        }

        lineRenderer.positionCount = pathPositions.Count;
        lineRenderer.SetPositions(pathPositions.ToArray());

        //NavMeshHit hit;
        //agent.FindClosestEdge(out hit);
        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.position);

        if (Input.GetMouseButtonDown(0))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }
    }
}
