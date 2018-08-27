using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public delegate void ActionPressedDelegate(Player.Team t, Player.Id i);
    public delegate void MechDestroyedDelegate(Player p);

    public bool autoAction;

    public enum Id {
        Ace,
        Drone1,
        Drone2,
        Drone3,
        Drone4,
    }

    public enum Team {
        Blue,
        Red,
        Neutral
    }

    public struct InputArgs {
        public Vector2 movement;
        public bool actionPressed;
        public bool actionReleased;
    }

    public Team team;
    public Id id;
    public AbstractMech mech;

    private string horizontalAxis;
    private string verticalAxis;
    private string action;
    
    public ActionPressedDelegate OnActionPressed;
    private void TriggerOnActionPressed() { if (OnActionPressed != null) OnActionPressed(team, id); }

    public MechDestroyedDelegate OnMechDestroyed;
    private void TriggerOnMechDestroyed() { if (OnMechDestroyed != null) OnMechDestroyed(this); }

    public void Initialize(Id id, Team team) {
        this.id = id;
        this.team = team;

        string controlPrefix = team.ToString() + "_" + id.ToString() + "_";
        horizontalAxis = controlPrefix + "horizontal";
        verticalAxis = controlPrefix + "vertical";
        action = controlPrefix + "action";
    }

    public void AssignMech(AbstractMech mech) {
        this.mech = mech;
        mech.team = this.team;
        mech.OnDestroyed = HandleOnMechDestroyed;
    }

    public bool HasMech() {
        return mech != null;
    }

    private void HandleOnMechDestroyed() {
        mech.OnDestroyed -= HandleOnMechDestroyed;
        TriggerOnMechDestroyed();
    }


    int modCount = 0;
    void Update() {
        InputArgs args;
        args.movement = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        args.actionPressed = Input.GetButtonDown(action);
        args.actionReleased = Input.GetButtonUp(action);

        if (autoAction) {
            args.actionPressed = modCount % 29 == 0;
            args.actionReleased = modCount % 31 == 0;
            if (modCount == 31) modCount = 0;
        }
        modCount++;

        if (args.actionPressed) {
            TriggerOnActionPressed();
        }        
        
        if (mech != null) {
            mech.Move(args);
        }       
    }
}