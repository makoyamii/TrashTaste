using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    [SerializeField] private float damage;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private void Awake() {
        initScale = enemy.localScale;
    }


    private void Update() {
        if(movingLeft) {

            if(enemy.position.x >= leftEdge.position.x) {
                MoveInDirection(-1);
            } else {
                DirectionChange();
            }
            
        } else {

            if(enemy.position.x <= rightEdge.position.x) {
                MoveInDirection(1); 
            } else {
                DirectionChange();
            }
            
        }
        
    }

    private void DirectionChange() {
        movingLeft = !movingLeft;
    }
    private void MoveInDirection(int _direction) {
        //make the enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        //move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
        enemy.position.y, enemy.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
