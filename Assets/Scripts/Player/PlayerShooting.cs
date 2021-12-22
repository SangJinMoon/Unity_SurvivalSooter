using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20; //데미지
    public float timeBetweenBullets = 0.15f; //발사속도 (줄어들수록 올라감)
    public float range = 100f; //사거리


    float timer;
    Ray shootRay = new Ray(); //무엇을 맞추었는지
    RaycastHit shootHit; //맞춘것 반환
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable"); //shootable layer 에 있는 모든것의 갯수 
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) // Fire1 : 마우스 왼쪽 or Left ctrl 키 (default)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position); //시작위치, 끝위치 (아래에서 다시설정)

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask)) //맞추면발생 shootHit은 맞춘대상
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> (); //맞춘대상의 EnemyHealt 탐색 (사물일경우 null 리턴)
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point); // shootHit.point 맞춘위치
            }
            gunLine.SetPosition (1, shootHit.point); //광선이 맞힌 지점까지 나가고 멈춤
        }
        else // 아무것도 맞히지 않은경우
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
