using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform pivot;
    public float sensitivity = 10f;

    public bool autoRotate = false;

    bool move = false;
    float offset = 0;

    void Update(){
        move = Input.GetMouseButton(1);
        offset = Input.GetAxis("Mouse X");

        if(autoRotate){
            move = true;
            offset = 1;
        }
    }

    void LateUpdate (){
        if(move){
            transform.RotateAround(pivot.position, Vector3.up, offset * sensitivity * Time.deltaTime);
        }
    }
}
