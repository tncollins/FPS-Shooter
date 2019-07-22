using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAndArrow : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float speed = 30f;
    public float deactivateTimer = 3f;
    public float damage = 15f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    public void Launch(Camera mainCamera)
    {
        rigidBody.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + rigidBody.velocity);
    }

    void DeactivateGameObject()
    {
        if(gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Enemy")
        {
            other.GetComponent<Health>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
