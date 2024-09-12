using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool rotateStarted = false;
    public bool rotateOnce = false;

    public GameObject rotationSelect;

    public Transform firePoint;
    public GameObject projectile;


    [Header("Tower Statistics")]
    public int bulletSpeed;
    public int range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowerOnPlace();

        //FireProjectile();
    }

    void OnTick()
    {
        FireProjectile();
        Debug.Log("Tower Shoot Movement");
    }

    private void RotateTowerOnPlace()
    {
        //rotate
        if (rotateStarted)
        {
            //rotation
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

            float rotation = Mathf.Round(angle / 90) * 90f;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));

            if (Input.GetMouseButtonUp(0))
            {
                rotateStarted = false;
                rotateOnce = true;
                rotationSelect.SetActive(false);

                //StartCoroutine("TestFire");
            }
        }
    }

    public void FireProjectile()
    {
        if (!rotateOnce) return;


        GameObject bullet = Instantiate(projectile, gameObject.transform.position, Quaternion.identity, GameManager.Instance.globelParent);
        bullet.GetComponent<Projectile>().bulletRange = range;
    }

    //IEnumerator TestFire()
    //{
    //    while(true)
    //    {
    //        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);

    //        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
    //        yield return new WaitForSeconds(1);
    //    }
        
    //}

}
