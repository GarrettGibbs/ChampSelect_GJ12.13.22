using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform LeftEdge;
    [SerializeField] Transform RightEdge;
    [SerializeField] LevelManager levelManager;
    [SerializeField] float speed;

    private void Update() {
        if (levelManager.inSettings || levelManager.respawning) return;

        float xAxisValue = Input.GetAxis("Horizontal");
        if ((transform.position.x >= RightEdge.localPosition.x && xAxisValue > 0) || (transform.position.x <= LeftEdge.localPosition.x && xAxisValue < 0)) return;
        if (xAxisValue != 0) {
            transform.position = new Vector3(transform.position.x + (xAxisValue*speed*Time.deltaTime), transform.position.y, transform.position.z);
        }
    }
}
