using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ParticleSystem))]
public class LaserCollision : MonoBehaviour
{

    [SerializeField] public GameObject playerObject;
    [SerializeField] private AudioClip clip;
    private Transform playerTransform;
    private ParticleSystem ps;

    private List<ParticleCollisionEvent> collisionEvents;
    //public ParticleSystem.TriggerModule trigger;
    //public ParticleSystem.CollisionModule collision;

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        playerTransform = playerObject.transform;
    }

    private void Start()
    {
        //Has been initiated, so let's set the collisions
        var coll = ps.collision;

        if (!coll.enabled) {
            coll.enabled = true;
        }

        coll.AddPlane(playerTransform);
        collisionEvents = new List<ParticleCollisionEvent>();
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetInstanceID() == playerObject.GetInstanceID())
        {
            int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

            if (numCollisionEvents > 0)
            {
                StartCoroutine(_restart(other));
            }
        }       
    }

    private IEnumerator _restart(GameObject other)
    {
        var crosshair = GameObject.FindGameObjectWithTag("crosshair");

        //SoundManager.PlaySound(clip);
        Destroy(crosshair);

        yield return new WaitForSeconds(1);
        Destroy(other);
        SceneManager.LoadScene("BeamerTankWambo");
    }
}
