using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] private float speed = 8f;

    void Update() {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));

        if (transform.position.y >= 8) {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    
}