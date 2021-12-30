using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wyvern : MonoBehaviour
{
    public bool start = false;
    public float timer = 0;
    public float maxSp = 70;
    public float sp = 30;
    public float mutiple = 0;
    public float smoothTime = 0.8f;
    public float turnSmoothVelocity;
    public float angle, targetAngle = 0;
    public bool fire = false;
    public CharacterController controller;
    public Animator animator;
    private bool flying = false;
    private Vector3 movement;

    public void gameStart()
    {
        start = true;
    }

    public void stopGame()
    {
        start = false;
    }

    public void Clock()
    {
        timer = Time.time;
        if (timer <= 60)
        {
            timer += Time.deltaTime;
            mutiple = timer / 60;
            sp = mutiple * (maxSp - sp) + 5;
        }
    }

    void detect()
    {
        GameObject Aim = GameObject.FindGameObjectWithTag("Player");
        float dis_x = Aim.transform.position.x - transform.position.x, dis_z = Aim.transform.position.z - transform.position.z;
        targetAngle = Mathf.Atan2(dis_x, dis_z) * Mathf.Rad2Deg;

        if( !Aim.GetComponent<third_person_controller>().dead && Mathf.Sqrt(dis_x * dis_x + dis_z * dis_z) < 12)
        {
            if(Aim.GetComponent<third_person_controller>().doll || !Aim.GetComponent<third_person_controller>().running)
            fire = true;
        }
    }

    void set_ani()
    {
        animator.SetBool("isFlying",start);
        animator.SetBool("fire", fire);
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        set_ani();
        if (start)
        {
            Clock();
            detect();
            if (fire)
            {
                smoothTime = 0.01f;
                movement = Vector3.zero;

                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("fire")) fire = false;
            }
            else
            {
                smoothTime = 0.8f;
                movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * Time.deltaTime * sp;
            }
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        }
        transform.eulerAngles = new Vector3(0, angle, 0);
        controller.Move(movement);
    }
}
