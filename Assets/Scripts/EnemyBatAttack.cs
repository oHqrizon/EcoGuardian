using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBatAttack : MonoBehaviour
{
   private float shootInterval = 1f;
   private Transform player;
   private List<GameObject> circleCount = new List<GameObject>();
   private bool isShooting = false;
   private Coroutine shootCirclesCoroutine;
    private GameObject fireballInstant;
    [SerializeField] private GameObject fireball;
    public AudioSource fireballSound;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(checkRange());

    }
    IEnumerator checkRange() {

        while (true) {

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= 8f && !isShooting) {
                isShooting = true;
                shootCirclesCoroutine = StartCoroutine(shootCircles());

            } else if (distanceToPlayer >= 10 && isShooting) {
                isShooting = false;
                StopCoroutine(shootCirclesCoroutine);
            }
            
            yield return null;
        } 
    }

    IEnumerator shootCircles() {

        while (isShooting) {

            yield return new WaitForSeconds(shootInterval);

                if (circleCount.Count < 3) {
                    GameObject bat = GameObject.FindGameObjectWithTag("Bat");
                    GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

                    fireballInstant = Instantiate(fireball);

                    // GameObject circle = fireballInstant;

                    fireballInstant.tag = "Fireball";

                    fireballInstant.transform.position = bat.transform.position;
 
                    CircleCollision collisionHandler = fireballInstant.AddComponent<CircleCollision>();

                    StartCoroutine(fireProjectile(fireballInstant.transform, playerObj.transform.position, 6.5f));

                    circleCount.Add(fireballInstant);

                } else {
                    Destroy(circleCount[0]);
                    circleCount.RemoveAt(0);
                    Destroy(fireballInstant);
                }
        }
    }

    IEnumerator fireProjectile(Transform projectile, Vector2 targetPosition, float speed) {
        fireballSound.Play();
        targetPosition = new Vector2(targetPosition.x, targetPosition.y-2f);
        while (projectile != null) {
            float step = speed*Time.deltaTime;
            projectile.position = Vector2.MoveTowards(projectile.position, targetPosition, step);
            
            if (Vector2.Distance(projectile.position, targetPosition) < 0.1f) {
                
                Destroy(projectile.gameObject);
                yield break;
            }
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
