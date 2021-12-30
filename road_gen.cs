using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class road_gen : MonoBehaviour
{
    public GameObject[] path = new GameObject[7];
    public List<int> Edge = new List<int>();

    private bool left=false, right=false ,mid = false;
    private float p_dis;
    private int now;
    private float timer = 1;

    public void call(int i)
    {
        if (i == 0) left = true;
        else right = true;
        Debug.Log(left + " " +right);
    }

    void detect()
    {
        GameObject Aim = GameObject.FindGameObjectWithTag("Player");
        float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
        p_dis = Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z);
    }

    // Start is called before the first frame update
    void Start()
    {
        now = Random.Range(0,5);
    }

    // Update is called once per frame
    void Update()
    {
        detect();
        if (p_dis > 200) return;

        float degree, dic_x, dic_z;
        float[] d;

        if(!mid) Instantiate(path[now], transform.position, transform.rotation);
        if (now == 7 && left) d = new float[] { -40, 0, -90 };
        else if(now == 7 && right) d = new float[] { 40, 0, 90 };
        else if (now == 7){mid = true;return;}
        else if (now == 5) d = new float[] { -10, 0, -90 };
        else if (now == 6) d = new float[] { 10, 0, 90 };
        else d = new float[] { 0, 30, 0 };

        degree = transform.eulerAngles.y * Mathf.PI / 180;

        dic_x = Mathf.Cos(degree) * d[0] + Mathf.Sin(degree) * d[1];
        dic_z = Mathf.Cos(degree) * d[1] - Mathf.Sin(degree) * d[0];

        transform.position += new Vector3(dic_x, 0, dic_z);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + d[2], 0);
        timer = 0;

        Edge.Clear();
        if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward.normalized, Mathf.Infinity))
            for (int i = 0; i < 5; i++)
                if (now != i) Edge.Add(i);
        if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, transform.eulerAngles.y - 90, 0f) * Vector3.forward.normalized, Mathf.Infinity) && now != 5)
            Edge.Add(5);
        if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, transform.eulerAngles.y + 90, 0f) * Vector3.forward.normalized, Mathf.Infinity) && now != 6)
            Edge.Add(6);
        if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, transform.eulerAngles.y + 90, 0f) * Vector3.forward.normalized, Mathf.Infinity) && !Physics.Raycast(transform.position, Quaternion.Euler(0f, transform.eulerAngles.y - 90, 0f) * Vector3.forward.normalized, Mathf.Infinity) && now != 7)
            Edge.Add(7);
        now = Edge[Random.Range(0, Edge.Count)];
        left = false;
        right = false;
        mid = false;
    }
}
