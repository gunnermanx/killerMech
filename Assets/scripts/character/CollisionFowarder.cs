using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFowarder : MonoBehaviour {

    public delegate void OnCollisionEnter2DForwarder(Collision2D c);
    public delegate void OnCollisionExit2DForwarder(Collision2D c);

    public OnCollisionEnter2DForwarder onCollisionEnter2D;
    private void TriggerOnCollisionEnter2DForwarder(Collision2D c) { if (onCollisionEnter2D != null) onCollisionEnter2D(c); }

    public OnCollisionExit2DForwarder onCollisionExit2D;
    private void TriggerOnCollisionExit2DForwarder(Collision2D c) { if (onCollisionExit2D != null) onCollisionExit2D(c); }


    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision Enter!");
        TriggerOnCollisionEnter2DForwarder(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        TriggerOnCollisionExit2DForwarder(collision);
    }
}
