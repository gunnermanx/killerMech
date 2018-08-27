using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bruiser : AbstractMech {

    [SerializeField] private EnemyDetection enemyDetection;

    private void Start() {        
        Initialize();
        enemyDetection.OnEnemyDetected += HandleEnemyDetected;
    }

    private void OnDisable() {
        enemyDetection.OnEnemyDetected -= HandleEnemyDetected;
    }

    private void Update() {
        ToggleAttackDetectionBox(!controller.collisions.below);
    }

    private void ToggleAttackDetectionBox(bool toggle) {
        enemyDetection.gameObject.SetActive(toggle);
    }

    // ah w/e
    public void SetSpeedBuff() {
        hasSpeedBuff = true;
        Initialize();
    }

    private void HandleEnemyDetected() {
        animator.SetTrigger("Attack");
    }


}
