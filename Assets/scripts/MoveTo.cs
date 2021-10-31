using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public float speed=7f;
    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null || Vector3.Distance(transform.position, target) < (transform.right * Time.deltaTime * speed).magnitude)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        Vector3 direction = target - transform.position;
        //transform.LookAt(positions.GetPosition(currentIndex),new Vector3(0,0,1f));
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        transform.position += transform.right * Time.deltaTime * speed;
    }
}
