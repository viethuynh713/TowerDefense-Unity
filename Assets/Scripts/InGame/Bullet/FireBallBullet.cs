using System;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class FireBallBullet : ZoneBullet
    {
        public float height = 5f;
        
        private Vector3 initialPosition; 
        private Vector3 targetPosition; 
        private float flightDuration; 
        private float startTime; 

        private void Start()
        {
            
            initialPosition = transform.position; 
            targetPosition = target.transform.position; 

            float distanceY = targetPosition.y - initialPosition.y;
            flightDuration = Mathf.Max(0.1f, Mathf.Abs(distanceY / height));

            startTime = Time.time;
        }

        public override void Move()
        {

            if (target == null)
            {
                Explore();
            }

            else
            {
                transform.LookAt(target.transform.position);
                
                float timeSinceStart = Time.time - startTime;

                float normalizedTime = Mathf.Clamp01(timeSinceStart / flightDuration);

                float heightOffset = height * (normalizedTime - normalizedTime * normalizedTime);
                Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, normalizedTime) + Vector3.up * heightOffset;

                transform.position = newPosition;

                if (Vector3.Distance(transform.position,
                        new Vector3(transform.position.x, transform.position.y,target.transform.position.z)) < InGameService.infinitesimal 
                    ||transform.position.y == target.transform.position.y )
                {
                    Explore();
                }
            }


        }
    }
}