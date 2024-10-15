using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.Damage();
            collision.gameObject.GetComponent<Enemy>().Kill();
        }
    }
}
