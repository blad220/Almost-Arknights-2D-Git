using MainController.Enemy;
using System.Collections;
using UnityEngine;

namespace MainController.Operator.Bullet
{
    public class BulletController : MonoBehaviour
    {
        public Transform target;
        public Vector3 curVelocity;
        public bool isLookAtTarget;
        public bool isSpin;
        public float speedOfSpeen = 20f;
        public float MaxSpeed = 30f;
        public float SmoothTime = 0.01f;
        public float DistanceDestroyBullet = 0.25f;

        private float angle;
        private bool isMove;

        void Start()
        {
            if (isLookAtTarget)
            {
                transform.LookAt(target);
            }
        }
        void Update()
        {
            if (target != null && !isMove)
            {
                StartCoroutine(BulletMove());
            }
        }

        public int Attack;
        private IEnumerator BulletMove()
        {
            isMove = true;
            while (isMove)
            {
                if (target != null)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, target.position, ref curVelocity, SmoothTime, MaxSpeed * Time.timeScale);
                    if (isSpin)
                    {
                        Vector3 rotationValue = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle += speedOfSpeen);
                        transform.rotation = Quaternion.Euler(rotationValue);
                    }
                    if (Vector3.Distance(transform.position, target.position) < DistanceDestroyBullet)
                    {
                        EnemyController tempEnemyController = target.GetComponent<EnemyController>();
                        if (!tempEnemyController.Damage(Attack))
                        {
                        }
                        KillBullet();
                    }
                }
                else
                {
                    KillBullet();
                }
                yield return new WaitForSeconds(SmoothTime);
            }
            yield return null;
        }
        public void SetTarget(EnemyController wichEnemyAttack)
        {
            transform.localPosition = Vector3.zero;
            if (isLookAtTarget)
            {
                transform.LookAt(wichEnemyAttack.transform);
            }
            target = wichEnemyAttack.transform;
        }
        public void KillBullet()
        {
            isMove = false;
            StopAllCoroutines();
            target = null;
            transform.position = new Vector3(9999f, 9999f, 9999f);
        }
    }
}