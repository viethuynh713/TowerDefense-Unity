using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MythicEmpire.InGame
{
    public class RocketBullet : ZoneBullet
    {
        private List<Vector3> waypoints;
        private int currentWaypointIndex = 0;
        private float curveSpeed = 2f; // Tốc độ di chuyển trên đường cong
        private float t = 0f; // Biến thời gian dùng cho hàm Bezier
        private void Start()
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            var numOfPoint = Random.Range(4, 8);
            waypoints = new List<Vector3>()
            {
                transform.position + Vector3.forward,
            };
            for (int i = 1; i < numOfPoint; i++)
            {
                waypoints.Add(waypoints[0]+direction*Random.Range(0,0.2f));
            }
        }

        public override void Move()
        {
            if (target == null)
            {
                Explore();
                return;
            }

            
            if (currentWaypointIndex < waypoints.Count - 4)
            {
                // Tính toán vị trí trên đường cong Bézier
                Vector3 p0 = waypoints[currentWaypointIndex];
                Vector3 p1 = waypoints[currentWaypointIndex + 1];
                Vector3 p2 = waypoints[currentWaypointIndex + 2];
                Vector3 p3 = waypoints[currentWaypointIndex + 3];

                Vector3 positionOnCurve = CalculateBezierPoint(t, p0, p1, p2, p3);

                transform.LookAt(positionOnCurve);

                transform.position = Vector3.MoveTowards(transform.position,
                    positionOnCurve,
                    bulletSpeed * curveSpeed * Time.deltaTime);

                t += Time.deltaTime / Vector3.Distance(p0, p1) * curveSpeed;

                // Kiểm tra xem đã đi qua waypoint trung gian hiện tại chưa
                if (t >= 1f)
                {
                    currentWaypointIndex++;
                    t = 0f;
                }
            }
            else
            {
                transform.LookAt(target.transform.position);
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    target.transform.position, 
                    bulletSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position,target.transform.position) < InGameService.infinitesimal)
                {
                    Explore();
                }
            }
        }
        private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0; // (1-t)^3 * P0
            p += 3f * uu * t * p1; // 3 * (1-t)^2 * t * P1
            p += 3f * u * tt * p2; // 3 * (1-t) * t^2 * P2
            p += ttt * p3; // t^3 * P3

            return p;
        }
    }
    
}