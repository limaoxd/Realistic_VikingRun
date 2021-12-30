using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Iscoin = true;
    public bool Buff = false;

    void Start()
    {
        if (Iscoin)
        {
            if (Random.Range(0, 6) == 5) Object.Destroy(this.gameObject);
            else transform.localPosition += new Vector3(-2 + 2 * Random.Range(0, 3), 0, 0);
        }
        else
        {
            if (Random.Range(0, 3) != 1 && !Buff) Object.Destroy(this.gameObject);
            else if (Random.Range(0, 9) != 1 && Buff) Object.Destroy(this.gameObject);
            if(Buff) transform.localPosition += new Vector3(-2 + 2 * Random.Range(0, 3), 0, Random.Range(0, 21));
            else transform.localPosition += new Vector3(0, 0, Random.Range(0, 21));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
