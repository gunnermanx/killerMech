using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour {
    private const float COLLIDER_EPSILON = 0.015f;
    private const int NUM_H_RAYS = 4;
    private const int NUM_V_RAYS = 4;

    private LayerMask groundCollisionMask;

    float hRayInterval;
    float vRayInterval;

    private BoxCollider2D collider;

    public CollisionInfo collisions;


    struct RaycastOrigins {
        public Vector2 topRight;
        public Vector2 bottomRight;
        public Vector2 bottomLeft;
        public Vector2 topLeft;
    }

    public struct CollisionInfo {
        public bool above, below, left, right;

        public void Reset() {
            above = below = left = right = false;
        }

    }


    public void Move(Vector2 velocity) {
        RaycastOrigins ro = CalcRaycastsOrigins();
        collisions.Reset();

        if (Mathf.Abs(velocity.x) > 0.0f) CastHRays(ro, ref velocity);
        if (Mathf.Abs(velocity.y) > 0.0f) CastVRays(ro, ref velocity);

        transform.Translate(velocity);
    }


    private void Awake() {
        collider = this.GetComponent<BoxCollider2D>();
        CalcRayIntervals();
        groundCollisionMask = LayerMask.GetMask("Ground");
    }

    private void CalcRayIntervals() {
        Bounds bounds = collider.bounds;
        bounds.Expand(COLLIDER_EPSILON * -2);

        hRayInterval = bounds.size.y / (NUM_H_RAYS - 1);
        vRayInterval = bounds.size.x / (NUM_V_RAYS - 1);
    }

    private RaycastOrigins CalcRaycastsOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(COLLIDER_EPSILON * -2);

        RaycastOrigins ro;
        ro.topRight = new Vector2(bounds.max.x, bounds.max.y);
        ro.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        ro.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        ro.topLeft = new Vector2(bounds.min.x, bounds.max.y);

        return ro;
    }


    private void CastHRays(RaycastOrigins ro, ref Vector2 velocity) {
        float rayDir = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + COLLIDER_EPSILON;

        for (int i = 0; i < NUM_H_RAYS; i++) {
            Vector2 rayOrigin = (rayDir > 0) ? ro.bottomRight : ro.bottomLeft;
            rayOrigin += Vector2.up * (i * hRayInterval);

            Debug.DrawRay(rayOrigin, Vector2.right * rayDir * rayLength, Color.red);
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.right * rayDir, rayLength, groundCollisionMask);
            if (hit2D) {
                velocity.x = (hit2D.distance - COLLIDER_EPSILON) * rayDir;
                rayLength = hit2D.distance;

                collisions.left = (rayDir < 0) ? true : false;
                collisions.right = !collisions.left;
            }
        }
    }


    private void CastVRays(RaycastOrigins ro, ref Vector2 velocity) {
        float rayDir = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + COLLIDER_EPSILON;

        for (int i = 0; i < NUM_V_RAYS; i++) {
            Vector2 rayOrigin = (rayDir > 0) ? ro.topLeft : ro.bottomLeft;
            rayOrigin += Vector2.right * (i * vRayInterval + velocity.x);

            Debug.DrawRay(rayOrigin, Vector2.up * rayDir * rayLength, Color.red);
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, Vector2.up * rayDir, rayLength, groundCollisionMask);
            if (hit2D) {
                velocity.y = (hit2D.distance - COLLIDER_EPSILON) * rayDir;
                rayLength = hit2D.distance;

                collisions.below = (rayDir < 0) ? true : false;
                collisions.above = !collisions.below;
            }
        }
    }


}
