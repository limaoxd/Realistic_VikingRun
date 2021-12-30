using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public bool collectable = true;
    public bool buff = false;
    public float angular_speed = 180;
    public float timer = 0;
    private bool canKill = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && collectable)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<third_person_controller>().score += (int)(1000f * player.GetComponent<third_person_controller>().mutiple);
            if (buff) player.GetComponent<third_person_controller>().bufftime = 10 ;
            Object.Destroy(this.gameObject);
        }
        else if (other.tag=="Obstacle" && collectable)
        {
            this.transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
        }
        else if(other.tag == "Player" && !collectable)
        {
            foreach (var it in this.GetComponentsInChildren<Rigidbody>())
                it.isKinematic = false;  
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canKill = true;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (collectable)
        {
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + angular_speed * Time.deltaTime, 0);
        }
        else
        {
            if (canKill) timer += Time.deltaTime;
            if (timer > 3) Object.Destroy(this.gameObject);
        }
    }
}
