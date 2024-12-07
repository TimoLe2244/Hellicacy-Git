using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 1f;
    private Rigidbody2D rb;
    public GameObject projectilePrefab;

    public float distanceToShoot = 8f;
    public float distanceToStop = 4f;
    public float fireRate;
    private float timeToFire;
    public Transform firingPoint;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        timeToFire = fireRate;
    }

    private void Update()
    {
        if (!target) {
            GetTarget();
        } else {
            RotateTowardsTarget();
        }
        if(Vector2.Distance(target.position, transform.position) <= distanceToShoot){
            Shoot();
        }
    }

    private void Shoot()
    {
        if (timeToFire <= 0f) {
            Instantiate(projectilePrefab, firingPoint.position, firingPoint.rotation);
            timeToFire = fireRate;
        }else{
            timeToFire -= Time.deltaTime;
        }
    }

    private void FixedUpdate(){
        if (target != null){
            if (Vector2.Distance(target.position, transform.position) >= distanceToStop){
            rb.velocity = transform.up * speed;
            } else {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void RotateTowardsTarget(){
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }
    
    private void GetTarget(){
        if (GameObject.FindGameObjectWithTag("Player")){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
