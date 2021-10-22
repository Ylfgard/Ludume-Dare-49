using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : BaseCharacterLogic
{
    public Transform target;
    public Transform mainCharacter;
    public List<Transform> PatrolPoints;
    private bool targetIsRight = true;
    private bool isPatrolling = true;
    private bool isTargetDetected = false;
    private Coroutine patrolCoroutine;
    private Coroutine rotateTargetCoroutine;
    public float rotateSpeed;
    public float VisibilityDistance;
    public float cosViewingAngle = 0.5f;

    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject smokeEffect;

    FMOD.Studio.EventInstance instance;
    FMOD.Studio.EventDescription is3d;

    private void Start()
    {
        transform.position = PatrolPoints[0].position;
        rotateTargetCoroutine = StartCoroutine(RotateTargetCoroutine());
        patrolCoroutine = StartCoroutine(PatrolCoroutine());
        spriteRenderer = visual.GetComponentInChildren<SpriteRenderer>();
        visual.transform.SetParent(null);
        visual.transform.rotation = new Quaternion(0, 0, 0, 0);
        hud.GetComponent<EnemyHUD>().characterData = characterData;

        // Shoot sound
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/mashine_gun");
        //instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, GetComponent<Rigidbody>());
        instance.getDescription(out is3d);
        is3d.is3D(out bool d);
        Debug.Log(d);
    }
    private IEnumerator RotateTargetCoroutine()
    {
        float t = 0;
        float angle;
        if (targetIsRight == true) angle = 180;
        else angle = -180;
        while (t<1)
        {                        
            target.RotateAround(transform.position, transform.forward, angle * Time.deltaTime* 0.5f);
            t += Time.deltaTime* 0.5f;
            yield return null;
        }
        targetIsRight = !targetIsRight;
        StopCoroutine(rotateTargetCoroutine);
        StartCoroutine(RotateTargetCoroutine());
    }
    private IEnumerator PatrolCoroutine()
    {        
        for (int i = 0; i < PatrolPoints.Count; i++)
        {
            while (transform.position != PatrolPoints[i].position)
            {
                transform.position = Vector3.MoveTowards(transform.position, PatrolPoints[i].position, Time.deltaTime);

                Vector3 targetDirection = PatrolPoints[i].position - transform.position;
                Vector3 rotatedDirection = Quaternion.Euler(0, 0, 360) * targetDirection;
                float singleStep = rotateSpeed * Time.deltaTime;                
                Vector3 newDirection = Vector3.RotateTowards(transform.up, rotatedDirection, singleStep, 0.0f);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, newDirection);

                yield return null;
            }
        }
        StopCoroutine(patrolCoroutine);
        patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }
    private void FixedUpdate()
    {
        if(isPatrolling)
        {
            LookAround();
            Patrol();
        }
        if(isTargetDetected)
        {
            Attack();
        }
    }

    private void Patrol()
    {
        float diastance = Vector3.Distance(mainCharacter.position, transform.position);
        Vector3 direction;
        if(diastance <= VisibilityDistance)
        {
            direction = mainCharacter.position - transform.position;
            if(Vector3.Dot(transform.up, direction) >= cosViewingAngle)
            {
                isTargetDetected = true;
                isPatrolling = false;
                StopCoroutine(patrolCoroutine);
                StopCoroutine(rotateTargetCoroutine);
            }
        }        
    }
    private void LookAround()
    {
        if(isPatrolling) Aim(target.position);
    }
    private void Attack()
    {
        Aim(mainCharacter.position);
        if (!isReloadingWeapon)
        {
            Shoot();
            reloadingWeaponCoroutine = StartCoroutine(ReloadingWeaponsCoroutine());

            // Shoot sound
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            if(state == FMOD.Studio.PLAYBACK_STATE.STOPPED)
            {
                instance.start();
            }

            // Shoot particles
            Transform shot = characterData.Weapon.AimGunEndPointTransform;
            GameObject go = Instantiate(smokeEffect, shot.position, shot.rotation, shot);
            go.transform.localScale *= 0.5f;
        }
    }
    protected override void Die()
    {
        this.gameObject.SetActive(false);
        visual.gameObject.SetActive(false);
        hud.gameObject.SetActive(false);

        // Shoot sound
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        if(state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.release();
        }
    }
    private void Update()
    {
        visual.transform.position = transform.position;
    }
}
   
