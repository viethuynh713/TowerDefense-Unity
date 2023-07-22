using System;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class FireBallBullet : ZoneBullet
    {
        public float height = 2f;
        private Vector3 startPos;
        private float duration;
        private void Start()
        {
            startPos = transform.position;

        }

        public override void Move()
        {
            base.Move();
            // if (target == null)
            // {
            //     Explore();
            // }
            //
            // else
            // {
            //     transform.LookAt(target.transform.position);
            //     // Calculate the distance between position A and position B
            //     float distance = Vector3.Distance(startPos, target.transform.position);
            //
            //     // Calculate the duration based on the distance and the speed
            //     float duration = distance / bulletSpeed;
            //
            //     // Increment the time elapsed since the movement started
            //     float timeElapsed = Mathf.Clamp(Time.deltaTime, 0f, duration);
            //
            //     // Calculate the normalized time (0 to 1) based on the elapsed time and duration
            //     float normalizedTime = timeElapsed / duration;
            //
            //     // Calculate the vertical position using a parabolic equation (y = a*x^2 + b*x + c)
            //     float verticalOffset = height * 4f * (normalizedTime - 0.5f) * (normalizedTime - 0.5f);
            //
            //     // Calculate the horizontal position
            //     Vector3 horizontalOffset = Vector3.Lerp(startPos, target.transform.position, normalizedTime);
            //
            //     // Calculate the final position with both vertical and horizontal offsets
            //     Vector3 finalPosition = horizontalOffset + Vector3.up * verticalOffset;
            //
            //     // Update the object's position
            //     transform.position = finalPosition;
            //
            //     // Check if the movement is complete
            //     if (normalizedTime >= 1f)
            //     {
            //         Explore();
            //     }
            }
        

    }
}