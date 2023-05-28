using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PreventStuckIn : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("1");
            Vector3 stuckDir = this.gameObject.transform.position - collision.collider.gameObject.transform.position;
            stuckDir.y = 0.0f;
            collision.collider.gameObject.transform.position += stuckDir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("2");
            Vector3 stuckDir = this.gameObject.transform.position - other.gameObject.transform.position;
            stuckDir.y = 0.0f;
            other.gameObject.transform.position += stuckDir;
        }
    }

}
