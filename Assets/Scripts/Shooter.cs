using UnityEngine;

public class Shooter : MonoBehaviour{
    public GameObject firePoint;
    public GameObject ballPrefab;
    public float power;
    public float rotationSpeed;

    Vector3 currentPosition;
    Quaternion currentRotation;


    void Start(){
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        predict();
    }

    public Vector3 calculateForce(){
        return transform.forward * power;
    }

    void shoot(){
        GameObject ball = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
        LevelManager.instance.shoot();
    }

    void Update(){
        float vertical = Input.GetAxis ("Vertical");
        float Horizontal = Input.GetAxis ("Horizontal");

        transform.Rotate(
            -vertical * rotationSpeed, 
            Horizontal * rotationSpeed, 
            0.0f
        );

        if(currentRotation != transform.rotation){
           predict();
        }   

        if(currentPosition != transform.transform.position){
           predict();
        }       
    
        currentRotation = transform.rotation;

        if(Input.GetKeyUp(KeyCode.Space)){
            if(LevelManager.instance.currentShoots.val > 0){
                shoot();
            }
        }
    }

    void predict(){
        PredictionManager.instance.predict(ballPrefab, firePoint.transform.position, calculateForce());
    }
}
