using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ParticleSystem))]
public class LaserCollision : MonoBehaviour
{

    [SerializeField] public GameObject _playerObject;
    [SerializeField]  AudioClip clip;
    Transform _playerTransform;
    ParticleSystem ps;
    public List<ParticleCollisionEvent> collisionEvents;
    //public ParticleSystem.TriggerModule trigger;
    //public ParticleSystem.CollisionModule collision;

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        _playerTransform = _playerObject.transform;
    }

    void Start()
    {
        //Has been initiated, so let's set the collisions
        var coll = ps.collision;

        if (!coll.enabled) {
            coll.enabled = true;
        }

        coll.AddPlane(_playerTransform);
        collisionEvents = new List<ParticleCollisionEvent>();
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.GetInstanceID() == _playerObject.GetInstanceID())
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

        SoundManager.PlaySound(clip, 1, 1);
        Destroy(crosshair);

        yield return new WaitForSeconds(1);
        Destroy(other);
        SceneManager.LoadScene("BeamerTankWambo");
    }
}
