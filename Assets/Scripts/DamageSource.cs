using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public int damage;
    public AudioClip sound;
    public float lifetime;
    public float knockback;
    public bool destroyOnHit;
    private void Start()
    {
        if (lifetime > 0)
        {
            StartCoroutine(DestroyAfterTime(lifetime));
        }
    }
    // destroy after time
    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    // damage player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // deal damage to player
            other.gameObject.GetComponent<PlayerStats>().ChangeCurrentHealth(-damage);
            // play sound
            if (sound != null)
            {
                AudioSource.PlayClipAtPoint(sound, transform.position);
            }
            // knockback player using a character controller
            other.gameObject.GetComponent<CharacterController>().Move(transform.forward * knockback);
            // destroy object
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}
