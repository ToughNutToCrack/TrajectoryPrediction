using UnityEngine;

public class Bouncer : MonoBehaviour{
    public float power;

    void OnCollisionEnter(Collision collision){
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        Vector3 dir = collision.contacts[0].normal;
        if(rb != null){
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }
}
