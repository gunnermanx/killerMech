using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
public abstract class AbstractMech : MonoBehaviour {

    public delegate void DestroyedDelegate();

    public struct MechConfig {
        public float maxJumpHeight;
        public float minJumpHeight;
        public float jumpTime;
        public bool canBoost;
        public bool canPickup;
        public bool canDive;
        public float boostTime;
        public float groundSpeed;
    }

    public enum Team {
        BLUE,
        RED
    }

    public Team team { get; set; }

    public Collider2D hurtBox;

    protected MovementController controller;
    protected Animator animator;
    protected SpriteRenderer sprite;

    private Vector2 v0, v1;
    private float gravity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float boostVelocity;
    private bool canBoost;
    private float groundSpeed;

    private bool flipX = false;

    public DestroyedDelegate OnDestroyed;
    private void TriggerOnDestroyed() { if (OnDestroyed != null) OnDestroyed(); }

    public void Move(Player.InputArgs input) {
        if (controller.collisions.above || controller.collisions.below) {
            v0.y = 0.0f;
        }
        if (input.actionPressed) {
            if (controller.collisions.below) {
                v0.y = maxJumpVelocity;   // v0.y set to jump speed    
            } else if (canBoost) {
                v0.y = boostVelocity;
            }
        }
        if (input.actionReleased) {
            if (v0.y > minJumpVelocity) {
                v0.y = minJumpVelocity;
            }
        }

        v1.x = input.movement.x * groundSpeed;       // just setting v1.x to new speed, no acceleration
        v1.y = v0.y + gravity * Time.deltaTime;     // v1 = v0 + at

        Vector2 d = new Vector2(v1.x * Time.deltaTime, (v0.y + v1.y) * 0.5f * Time.deltaTime);     // d = 1/2 * (v0+v1) * t
        v0 = v1;

        controller.Move(d);

        UpdateSprite(input);
    }

    private void UpdateSprite(Player.InputArgs input) {
        float absMovementX = Mathf.Abs(input.movement.x);
        animator.SetBool("Walking", absMovementX > 0.0f);
        if (absMovementX > 0.0f) {
            flipX = input.movement.x > 0.0f;
        }
        sprite.flipX = flipX;     // default sprites face left, flip when moving right
    }

    public void DestroyMech() {
        //die?
        TriggerOnDestroyed();
        Destroy(gameObject);
    }

    protected void Awake() {
        Debug.Log("Awake on mech");

        controller = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected void Initialize(MechConfig mc) {
        gravity = -(2f * mc.maxJumpHeight) / Mathf.Pow(mc.jumpTime, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * mc.jumpTime;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * mc.minJumpHeight);
        boostVelocity = Mathf.Abs(gravity) * mc.boostTime;
        canBoost = mc.canBoost;
        groundSpeed = mc.groundSpeed;
    }
}