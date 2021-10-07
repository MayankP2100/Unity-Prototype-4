using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody playerRb;
    private GameObject focalPoint;
    public float speed;
    public bool hasPowerup = false;
    public float powerupForce = 10;
    public float powerupCooldown = 7;
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerupIndicator.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCooldownRoutine());
        }
    }

    IEnumerator PowerupCooldownRoutine()
    {
        yield return new WaitForSeconds(powerupCooldown);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position + new Vector3(0, -0.5f, 0);

            enemyRb.AddForce(awayFromPlayer * powerupForce, ForceMode.Impulse);

            Debug.Log("Collided  with " + collision.gameObject.name + " with powerup state set to " + hasPowerup);
        }
    }
}
