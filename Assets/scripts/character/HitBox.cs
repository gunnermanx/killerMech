using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour {

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask bounceMask;
    [SerializeField] private List<BoxCollider2D> hitboxColliders;
    [SerializeField] private AbstractMech owner;

    // Update is called once per frame
    void FixedUpdate() {

        foreach(BoxCollider2D hitboxCollider in hitboxColliders) {
            bool tmp = false;
            Collider2D bc = Physics2D.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.size, 0f, bounceMask);
            if (bc != null) {

                tmp = true;
                // TODO: bounce the mech away
                Debug.Log("bouncy");
                AbstractMech mech = bc.gameObject.transform.parent.GetComponent<AbstractMech>();

                Vector3 pushDir = Vector3.Normalize(bc.transform.position - hitboxCollider.transform.position);
                Debug.DrawRay(hitboxCollider.transform.position, pushDir, Color.blue);

                mech.PushMech(pushDir);
            }
            if (tmp) return;

            Collider2D c = Physics2D.OverlapBox(hitboxCollider.bounds.center, hitboxCollider.bounds.size, 0f, targetMask);
            if (c != null) {
                Debug.Log(c.gameObject.name + " " + targetMask.value.ToString());
                AbstractMech mech = c.gameObject.transform.parent.GetComponent<AbstractMech>();

                Debug.Log("hit an enemy");
                mech.DestroyMech();
            }
        }
       
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        //Gizmos.matrix = Matrix4x4.TRS(
        //    Vector3.zero, 
        //    hitboxCollider.transform.rotation, 
        //    hitboxCollider.transform.localScale
        //);
        //Gizmos.DrawWireCube(hitboxCollider.bounds.center, new Vector3(hitboxCollider.bounds.size.x, hitboxCollider.bounds.size.y, 0.0f));
    }
}
