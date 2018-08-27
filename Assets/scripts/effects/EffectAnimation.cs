using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private float duration;

    private void Start() {
        StartCoroutine(PlayEffectAnimation());
    }

    private IEnumerator PlayEffectAnimation() {
        animator.Play("effect");
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
