using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    
    public struct InputArgs {
        public Vector2 movement;
        public bool jumpPressed;
        public bool jumpReleased;
    }

    private AbstractMech character;
    private string horizontalAxis;
    private string verticalAxis;
    private string action;

    public void Initialize(AbstractMech mech, string controlPrefix)
    {
        character = mech;

        horizontalAxis = controlPrefix + "horizontal";
        verticalAxis = controlPrefix + "vertical";
        action = controlPrefix + "action";
    }
	
	void Update () {
        InputArgs args;
        args.movement       = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
        args.jumpPressed    = Input.GetButtonDown(action);
        args.jumpReleased   = Input.GetButtonUp(action);

        character.Move(args);
    }
}
