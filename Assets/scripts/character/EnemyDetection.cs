using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour {

    [SerializeField] private AbstractMech owner;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private BoxCollider2D detectCollider;

    public delegate void EnemyDetected();
    public EnemyDetected OnEnemyDetected;
    private void TriggerOnEnemyDetected() { if (OnEnemyDetected != null) OnEnemyDetected(); }
        
    // Update is called once per frame
    void FixedUpdate() {
        Collider2D c = Physics2D.OverlapBox(detectCollider.bounds.center, detectCollider.bounds.size, 0f, targetMask);
        if (c != null) {
            TriggerOnEnemyDetected();
        }
    }

    private void OnDrawGizmos() {
        //Gizmos.color = Color.red;        
        //Gizmos.DrawWireCube(detectCollider.bounds.center, new Vector3(detectCollider.bounds.size.x, detectCollider.bounds.size.y, 0.0f));
    }
}
