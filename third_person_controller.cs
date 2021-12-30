using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class third_person_controller : MonoBehaviour
{
    public bool doll = false;
    public bool dead = false;
    public bool running = false;
    public int score = 0;
    public int point = 0;
    public float timer = 0;
    public float sp_timer = 0;
    public float dizz_timer = 0;
    public float maxSp = 30;
    public float sp = 15;
    public float bufftime = 0f;
    public float jumpVelocity = 20f;
    public float mutiple = 0;
    public float smoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float gravity = 0.1f;
    public float currentGravity;
    public float maxGravity = 5.0f;
    public float groundDis = 0.1f;
    public GameObject effect;
    public CharacterController controller;
    public Animator animator;
    public AudioSource audioSource;
    public GameObject gen;
    private bool gameStart = false;
    private bool mid = false;
    private bool left = false;
    private bool right = false;
    private bool turned = false;
    private bool jumped = false;
    private bool W, S, A, D;
    private float angle, targetAngle;
    private List<GameObject> buttons;
    private Vector3 movement;
    private Vector3 gravityDic;
    private Vector3 gravityMovement;

    public void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Left") left = true;
        if (other.tag == "Right") right = true;
        if (other.tag == "Middle") mid = true;
        if (other.tag == "Obstacle"&&bufftime<=0) doll = true;
        if (other.tag == "Enemy" && bufftime <= 0) dead = true;
    }

    public void OnTriggerExit(Collider other)
    {
        turned = false;
        if(other.tag == "Left") left = false;
        if(other.tag == "Right") right = false;
        if (other.tag == "Middle") mid = false;
    }

    public void keyInput()
    {
        A = (Input.GetKey("a") ? true : false);
        D = (Input.GetKey("d") ? true : false);
        running = (Input.GetKey("w") ? true : running);
        running = (Input.GetKey("s") ? false : running);
        jumped = (Input.GetKeyDown("space") && IsGrounded() ? true : false);

        animator.SetBool("isground", IsGrounded());
        animator.SetBool("running", running);
        animator.SetBool("jump", jumped);
    }

    public void Clock()
    {
        if(gameStart && !dead) timer += Time.deltaTime;
        if (bufftime > 0)
        {
            bufftime -= Time.deltaTime;
            effect.SetActive(true);
        }
        else effect.SetActive(false);
        if (running && sp_timer<=60) sp_timer += Time.deltaTime;
        else if(!running)sp_timer = 0;
        mutiple = sp_timer / 60;

        sp = mutiple * (maxSp - sp) + 15;
        animator.speed = mutiple * 0.5f +1;
    }

    protected bool IsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, groundDis)) return true;
        return false;
    }

    public void Falling()
    {
        if (IsGrounded())
            currentGravity = 0;
        else if (currentGravity < maxGravity)
            currentGravity += gravity * Time.deltaTime;
        if (!dead&&!doll&&jumped)
            currentGravity -= jumpVelocity;

        gravityMovement = gravityDic * currentGravity;
    }

    void setState()
    {
        if (transform.position.y < -2) dead = true;

        if (doll||dead)
        {
            running = false;
            foreach (var it in this.GetComponentsInChildren<Rigidbody>())
            {
                it.isKinematic = false;
                it.drag = 4.2f;
            }

            animator.enabled = false;
            dizz_timer += Time.deltaTime;

            if (dead)
                foreach (var it in buttons)
                    it.SetActive(true);
            else if (dizz_timer > 2)
            {
                dizz_timer = 0;
                doll = false;
                Vector3 dollPos = GameObject.Find("Hips").transform.localPosition;
                transform.position += new Vector3(dollPos.x, 0, dollPos.z);
            }
        }
        else
        {
            foreach (var it in this.GetComponentsInChildren<Rigidbody>())
                it.isKinematic = true;
            animator.enabled = true;
        }
        if(running)
        {
            GameObject wyvern = GameObject.FindGameObjectWithTag("Enemy");
            wyvern.GetComponent<wyvern>().gameStart();
            gameStart = true;
            audioSource.enabled = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buttons     = new List<GameObject>();
        controller  = this.GetComponent<CharacterController>();
        animator    = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        targetAngle = this.transform.rotation.y;
        gravityDic  = Vector3.down;

        foreach (var it in GameObject.FindGameObjectsWithTag("ESC"))
        {
            buttons.Add(it);
            it.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        setState();
        Clock();
        keyInput();
        Falling();

        angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
        if (running&&!doll&&!dead)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            movement = Quaternion.Euler(0f, angle, 0f) * Vector3.forward.normalized * Time.deltaTime * sp;


            if (mid && A && !turned) { targetAngle -= 90; gen.GetComponent<road_gen>().call(0); turned = true; }
            else if (mid && D && !turned) { targetAngle += 90; gen.GetComponent<road_gen>().call(1); turned = true; }
            else if (A && left && !turned) { targetAngle -= 90; turned = true;  }
            else if (D && right && !turned) { targetAngle += 90; turned = true; }
            else if (D && !right) { movement += Quaternion.Euler(0f, angle, 0f) * Vector3.right.normalized * sp / 3 * Time.deltaTime; }
            else if (A && !left) { movement += Quaternion.Euler(0f, angle, 0f) * Vector3.left.normalized * sp / 3 * Time.deltaTime; }
        }
        else
        {
            movement = Vector3.zero;
        }

        controller.Move(gravityMovement + movement);
    }
}
