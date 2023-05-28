using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] private Transform enemyObj;
    [SerializeField] private GradStudent gradStudent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gradStudent.MovementStop();
            Vector3 attackTargetLookDir = other.gameObject.transform.position - enemyObj.position;
            attackTargetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(attackTargetLookDir.normalized);
            enemyObj.rotation = look;
            gradStudent.IsAttackTime = true;
            gradStudent.ChangeState(EnemyAttackState.GetInstance);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 attackTargetLookDir = other.gameObject.transform.position - enemyObj.position;
            attackTargetLookDir.y = 0;
            Quaternion look = Quaternion.LookRotation(attackTargetLookDir.normalized);
            enemyObj.rotation = look;
            gradStudent.IsAttackTime = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gradStudent.IsAttackTime = false;
        }
    }
}
