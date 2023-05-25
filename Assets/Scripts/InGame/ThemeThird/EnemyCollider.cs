using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] private GradStudent gradStudent;
    [SerializeField] private GameObject enemyObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NonCollider"))
        {
            if (gradStudent.IsAttackTime)
            {
                Debug.Log("Enemy Stuck Some Object");
                Vector3 stuckDir = other.transform.position - this.gameObject.transform.position;
                stuckDir.y = 0.0f;
                enemyObject.transform.position -= (stuckDir / 2);
            }

        }
    }

    /*
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("2");
        if (collision.collider.CompareTag("NonCollider"))
        {
            Debug.Log("Enemy Stuck Some Object");
            Vector3 stuckDir = collision.collider.transform.position - this.gameObject.transform.position;
            enemyObject.transform.position = (-stuckDir + enemyObject.transform.forward);
            Debug.Log(-stuckDir);
        }
    }
    */
}
