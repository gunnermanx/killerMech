using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MovementController))]
public abstract class AbstractMech : MonoBehaviour {

    public delegate void DestroyedDelegate();


    public Player.Team team;
    public Player owner { get; set; }

    public Collider2D hurtBox;

    [SerializeField] protected List<Collider2D> colliders;
    [SerializeField] protected GameObject explosionPrefab;

    protected MovementController controller;
    protected Animator animator;
    protected SpriteRenderer sprite;
    protected bool ignoresInput;
    protected bool lockYMovement;
    protected bool hasSpeedBuff;

    private Vector2 velocity;//v0, v1;


    [SerializeField] private float groundSpeed;
    [SerializeField] private float airSpeed;

    [SerializeField] private float maxJumpHeight;
    [SerializeField] private float minJumpHeight;
    [SerializeField] private float jumpTime;
    [SerializeField] private float timeToTerminalVelocity;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float terminalVelocity;
    private float gravity;


    private bool flipX = false;

    protected List<ICharacterModifier> mods = new List<ICharacterModifier>();

    public DestroyedDelegate OnDestroyed;
    private void TriggerOnDestroyed() { if (OnDestroyed != null) OnDestroyed(); }

    public virtual void Move(Player.InputArgs input) {
        if (ignoresInput) {
            // test
            input.movement.x = 0.0f;
            input.actionPressed = false;
            input.actionReleased = false;

        }

        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0.0f;
        }

        if (controller.collisions.below) {
            velocity.x = input.movement.x * groundSpeed;       // just setting v1.x to new speed, no acceleration
        } else {
            velocity.x += input.movement.x * airSpeed * 0.1f;
            velocity.x = Mathf.Clamp(velocity.x, -airSpeed, airSpeed);
        }

        // jumping
        if (input.actionPressed) {
            if (controller.collisions.below) {
                velocity.y = maxJumpVelocity;   // v0.y set to jump speed    
            }
        }
        if (input.actionReleased) { //&& !config.canBoost) {
            if (velocity.y > minJumpVelocity) {
                velocity.y = minJumpVelocity;
            }
        }

        // falling
        velocity.y = velocity.y + gravity * Time.deltaTime;
        velocity.y = Mathf.Clamp(velocity.y, terminalVelocity, float.MaxValue);

        foreach (ICharacterModifier mod in mods) {
            velocity = mod.MoveModifier(input, controller.collisions, velocity);
        }

        if (lockYMovement) {
            velocity.y = 0.0f;
        }

        Vector2 d = velocity * Time.deltaTime;

        controller.Move(d);

        UpdateColliders(input);
        UpdateSprite(input);

        //v0 = v1;
    }

    private void UpdateSprite(Player.InputArgs input) {
        float absMovementX = Mathf.Abs(input.movement.x);
        if (controller.collisions.below == false) {
            animator.SetBool("Boosting", true);
        } else {
            animator.SetBool("Boosting", false);
            animator.SetBool("Walking", absMovementX > 0.0f);
        }

        if (absMovementX > 0.0f) {
            flipX = input.movement.x > 0.0f;
        }
        sprite.flipX = flipX;     // default sprites face left, flip when moving right
    }

    private void UpdateColliders(Player.InputArgs input) {
        float absMovementX = Mathf.Abs(input.movement.x);
        if (absMovementX > 0.0f) {
            float scale = Mathf.Sign(input.movement.x) * -1.0f;  // flip collider when moving right

            foreach (Collider2D c in colliders) {
                Vector3 curScale = c.transform.localScale;
                c.transform.localScale = new Vector3(scale, curScale.y, curScale.z);
            }
        }
    }

    public void DestroyMech() {
        //die?
        TriggerOnDestroyed();
        Destroy(gameObject);
        GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    public void PushMech(Vector3 dir) {
        velocity.x = dir.x * airSpeed;
    }

    protected void Awake() {
        controller = GetComponent<MovementController>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        GetComponents<ICharacterModifier>(mods);        
        mods.Sort( (a, b) => a.GetPriority().CompareTo(b.GetPriority()) );
    }

    protected void Initialize() {
        float jumpTimeModifier      = hasSpeedBuff ? -0.05f : 0.0f;
        float boostTimeModifier     = hasSpeedBuff ? 0.0f : 0.0f;
        float groundSpeedModifier   = hasSpeedBuff ? 0.4f : 0.0f;

        groundSpeed += groundSpeedModifier;

        gravity = -(2f * maxJumpHeight) / Mathf.Pow(jumpTime, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * jumpTime;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        terminalVelocity = 0.5f * gravity * Mathf.Pow(timeToTerminalVelocity, 2f);
    }
}