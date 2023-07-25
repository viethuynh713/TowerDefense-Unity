using System;
using UnityEngine;

namespace MythicEmpire.InGame
{
    public class FireBallBullet : ZoneBullet
    {
        public float height = 5f;
        
        private float angle = 0f; // Góc di chuyển của viên đạn theo hình vòng
        private Vector3 initialPosition; // Vị trí ban đầu của viên đạn
        private Vector3 targetPosition; // Vị trí của đối thủ
        private Vector3 direction; // Hướng di chuyển của viên đạn
        private float flightDuration; // Thời gian bay từ vị trí ban đầu đến độ cao y=3
        private float startTime; // Thời điểm bắt đầu bay từ vị trí ban đầu

        private void Start()
        {
            
            initialPosition = transform.position; // Lưu vị trí ban đầu của viên đạn
            targetPosition = target.transform.position; // Lưu vị trí của đối thủ

            // Tính toán hướng di chuyển từ vị trí ban đầu của viên đạn đến đối thủ
            direction = (targetPosition - initialPosition).normalized;

            // Tính toán thời gian bay từ vị trí ban đầu đến độ cao y=3
            float distanceY = targetPosition.y - initialPosition.y;
            flightDuration = Mathf.Max(0.1f, Mathf.Abs(distanceY / height));

            // Lưu thời điểm bắt đầu bay
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

                // Tính toán tỷ lệ thời gian đã bay so với thời gian bay tổng cộng
                float normalizedTime = Mathf.Clamp01(timeSinceStart / flightDuration);

                // Tính toán vị trí mới của viên đạn dựa trên đường parabol
                float heightOffset = height * (normalizedTime - normalizedTime * normalizedTime);
                Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, normalizedTime) + Vector3.up * heightOffset;

                // Di chuyển viên đạn tới vị trí mới
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