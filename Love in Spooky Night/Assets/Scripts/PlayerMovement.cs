using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    public float groundDistance = .4f;
    public LayerMask groundMask;

    public Transform model;

    public GameObject healParticle, attackParticle;
    public Transform shootFrom;

    public int healValue = 5, damageValue = 5;

    GameObject particle;

    [HideInInspector]
    public float movementX;
    [HideInInspector]
    public float movementZ;
    [HideInInspector]
    public bool canMove = true;
    
    bool isGround = true;
    bool isShooting = false;

    float movementY = 0f;

    Vector3 movement;

    CharacterController cr;
    RaycastHit hit;

    Camera cam;

    private void Start()
    {
        cr = GetComponent<CharacterController>();

        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        //stopping player and animation when she cant move
        if (!canMove)
        {
            GetComponentInChildren<AnimationController>().hasSpeed = false;
            return;
        }

        //setting up movement
        isGround = Physics.CheckSphere(transform.position, groundDistance, groundMask);

        movementX = Input.GetAxis("Horizontal");
        movementZ = Input.GetAxis("Vertical");

        if (isGround && movementY < 0)
        {
            movementY = -2f;
        }

        movementY += gravity * Time.deltaTime;

        movement = new Vector3(movementX * Time.deltaTime * speed, 0, movementZ * Time.deltaTime * speed);
        Vector3 movOfGravity = new Vector3(0, movementY * Time.deltaTime, 0);

        cr.Move(movement);
        cr.Move(movOfGravity);

        //updating value in animator
        GetComponentInChildren<AnimationController>().hasSpeed = movement != Vector3.zero;
    }

    private void Update()
    {
        if (!canMove && !isShooting)
            return;

        //shooting mechanism
        Vector3 placeToShoot = new Vector3();
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool isHeal = false;
        if (((Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1)) && isShooting) || !canMove)
        {
            isShooting = false;
            Destroy(particle);
        }
        else if (!isShooting && Physics.Raycast(ray, out hit)) 
        {
            isShooting = true;
            if (Input.GetKeyDown(KeyCode.Mouse0) && (hit.transform.CompareTag("friend") || hit.transform.CompareTag("pasif"))) //healing
            {
                placeToShoot = hit.point;
                isHeal = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && hit.transform.CompareTag("enemy")) //attacking
            {
                placeToShoot = hit.point;
                isHeal = false;
            }
            else
                isShooting = false;
        }

        if (placeToShoot != Vector3.zero)
        {
            if (isHeal)
            {
                particle = Instantiate(healParticle, shootFrom.position, Quaternion.identity, shootFrom);
            }
            else
            {
                particle = Instantiate(attackParticle, shootFrom.position, Quaternion.identity, shootFrom);
            }
            particle.transform.localScale = new Vector3(0.1f, 0.01f, Vector3.Distance(hit.transform.position, transform.position) / 5);
        }

        //rotating player towards enemy if shoot if shoot
        if (isShooting)
        {
            if(hit.transform == null)
            {
                isShooting = false;
                Destroy(particle);
                return;
            }
            if (hit.transform.CompareTag("friend") || hit.transform.CompareTag("pasif"))
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().Heal(healValue * Time.deltaTime);
            }
            else if (hit.transform.CompareTag("enemy"))
            {
                hit.transform.gameObject.GetComponent<HealthSystem>().TakeDamage(damageValue * Time.deltaTime);
            }
            model.LookAt(hit.transform);
            particle.transform.LookAt(hit.transform);
            particle.transform.localScale = new Vector3(0.1f, 0.01f, Vector3.Distance(hit.transform.position, transform.position) / 5);
        }
        else if (movement != Vector3.zero) //rotating character if not shoot
        {
            Quaternion toRotate = Quaternion.LookRotation(new Vector3(movementX, 0, movementZ), Vector3.up);
            model.rotation = Quaternion.RotateTowards(model.rotation, toRotate, 720 * Time.deltaTime);
        }

    }
}
