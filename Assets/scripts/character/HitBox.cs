using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    Vector3 boxSize;

    public LayerMask layer;

    private AbstractMech.Team target;

	private void Start()
	{
        boxSize = new Vector3(1.25f, 0.75f, 0.0f);
	}

    public void Initialize(AbstractMech.Team target)
	{
        this.target = target;
	}

	// Update is called once per frame
	void FixedUpdate()
    {        
        Collider2D c = Physics2D.OverlapBox(gameObject.transform.position, boxSize, 0f, layer);
        if (c != null)
        {
            AbstractMech mech = c.gameObject.transform.parent.GetComponent<AbstractMech>();
            if (mech != null && mech.team == target) {
                Debug.Log("hit an enemy");
                mech.TakeDamage();
            }

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawWireCube(new Vector3(0.0f, -0.5f, 0.0f), new Vector3(boxSize.x, boxSize.y, 1.0f)); // Because size is halfExtents
    }
}
