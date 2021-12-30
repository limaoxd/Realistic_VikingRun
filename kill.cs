using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kill : MonoBehaviour
{
    public float timer = 0;
    public bool f = false , canKill = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            f = true;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" &&f) canKill = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canKill) timer += Time.deltaTime;
        if(timer>1) Object.Destroy(this.gameObject);
    }
}
