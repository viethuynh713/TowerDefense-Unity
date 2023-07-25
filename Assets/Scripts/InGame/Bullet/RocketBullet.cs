using System;
using System.Collections.Generic;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class RocketBullet : ZoneBullet
    {
        private List<Vector3> waypoints;
        private int currentWaypointIndex = 0;
        private void Start()
        {
            waypoints = new List<Vector3>()
            {
                transform.position + Vector3.up,
                transform.position + Vector3.left,
                transform.position + Vector3.right*2,
                transform.position + Vector3.down,
                transform.position + Vector3.left + Vector3.one,
            };
        }

        public override void Move()
        {
            if (target == null)
            {
                Explore();
                return;
            }

            
            if (currentWaypointIndex < waypoints.Count)
            {
                transform.LookAt(waypoints[currentWaypointIndex]);
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    waypoints[currentWaypointIndex], 
                    bulletSpeed * Time.deltaTime);

                // Kiểm tra xem đã đến waypoint trung gian hiện tại chưa
                if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex]) < 0.1f)
                {
                    currentWaypointIndex++;
                }
            }
            else
            {
                transform.LookAt(target.transform.position);
                
                transform.position = Vector3.MoveTowards(transform.position, 
                    target.transform.position, 
                    bulletSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position,
                        new Vector3(transform.position.x, transform.position.y,target.transform.position.z)) < InGameService.infinitesimal)
                {
                    Explore();
                }
            }
        }
    }
    
}