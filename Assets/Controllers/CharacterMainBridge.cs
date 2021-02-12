using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterMainBridge : MonoBehaviour
{
    public BillboardHealth BillBoardHealth;
    public HealthKickerContraption HealthKickerContraption;
    public int AttackSpeed = 1;
    public int AttackRange = 3;
    public bool isHumanControl = true;
    public int HealthPoint = 10;
    public CharacterMainBridge HumanPlayer;
    public bool AmIDeath { get; private set; } = false;

    private AICharacterControl _aiCharacterControl;
    private Animator _animator;
    private BaseArtificialIntelligence _ai;
    private bool _bilboardInit = false;
    private RaycastHit _ubiquitousRaycastHit;
    private bool _playerSpotted = false;
    private LayerMask _lastPosLayer = (1 << 10),
        enemyLayer = (1 << 9),
        playerLayer = (1 << 8);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!AmIDeath)
        {
            EnemyVision();
            EnemyDies();
        }

        if (!_bilboardInit)
        {
            InitBillboard();

            _bilboardInit = true;
        }

//        if (isHumanControl)
//        {
            if (Time.frameCount % (200 / AttackSpeed) == 0)
            {
//                HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);
            AnimatorClipInfo[] anstate = _animator.GetCurrentAnimatorClipInfo(0);

                if (anstate[0].clip.name != "Standing Melee Attack Downward")
                {
                    CharacterMainBridge[] enemies = FindObjectsOfType<CharacterMainBridge>()
                        .Where(element => element.isHumanControl != isHumanControl && !element.AmIDeath).ToArray();

                    foreach (CharacterMainBridge enemyAi in enemies)
                    {
//                        if (Vector3.Distance(enemyAi.transform.position ,transform.position) < AttackRange)
                    if ((enemyAi.transform.position - transform.position).sqrMagnitude < AttackRange * AttackRange)
                        {
                            Debug.Log(Vector3.Distance(enemyAi.transform.position, transform.position) + " " + AttackRange + " " + (Vector3.Distance(enemyAi.transform.position, transform.position) < (float)AttackRange));
                        if (!AmIDeath)
                            {
                                Quaternion hitRot = Quaternion.FromToRotation(transform.forward, (enemyAi.transform.position - transform.position).normalized);
                                transform.rotation = Quaternion.Euler(0, hitRot.eulerAngles.y, 0);

                                _animator.SetTrigger("Slash");
//                                Debug.Log("Super fuck " + enemyAi.gameObject.name);
//                                Debug.Log(enemyAi.HealthKickerContraption);

                                enemyAi.HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);
//                                Quaternion hitRot = Quaternion.FromToRotation((enemyAi.transform.position - transform.position), enemyAi.transform.position);
//                                Quaternion hitRot = Quaternion.FromToRotation(transform.forward, enemyAi.transform.position.);
//                                transform.rotation = Quaternion.Euler(0, hitRot.eulerAngles.y - 90, 0);
//                                transform.rotation = hitRot;
//                                transform.LookAt(enemyAi.transform.position);

                                break;
                            }
                        }
                    }
                }
            }
//        }
    }

    public void OnEnable()
    {
        _aiCharacterControl = GetComponent<AICharacterControl>();
        
        HealthKickerContraption = new HealthKickerContraption();
        HealthKickerContraption.SetHealth(HealthPoint);

//        GetComponent<NavMeshAgent>().stoppingDistance = Mathf.Sqrt(AttackRange);
        GetComponent<NavMeshAgent>().stoppingDistance = AttackRange;

        _animator = GetComponent<Animator>();

        if (!isHumanControl)
        {
            _ai = new BaseArtificialIntelligence();
        }
    }


    private void InitBillboard()
    {
        if (BillBoardHealth && HealthKickerContraption != null)
        {
            BillBoardHealth.SetMaxHealth(HealthKickerContraption.Health);
            BillBoardHealth.SetHealth(HealthKickerContraption.Health);

            HealthKickerContraption.IAmHit((int amount) =>
            {
                BillBoardHealth.TakeDamage(HealthKickerContraption.NormalDamage);
            });
        }
    }

    void EnemyDies()
    {
        if (HealthKickerContraption.haveIKilled())
        {

            if (_animator != null)
            {
                _animator.SetTrigger("Death");
                Debug.Log("Set trigger Death");
                AmIDeath = true;
            }

            Destroy(this.gameObject, 4f);
        }
    }

    public void EnemyVision()
    {
        if (!isHumanControl && HumanPlayer)
        {
            if (Physics.Linecast(transform.position, HumanPlayer.transform.position, out RaycastHit hitPlayer, ~(_lastPosLayer))) // Linecast towards the player ignoring the last position layer
            {
//                Debug.Log("Raycast spotted");
                if (hitPlayer.collider.tag == "Player") // if the raycast hits the player then continue
                {
//                    Debug.Log("Player spotted");
                    _playerSpotted = true; // Player has been spotted
                    Debug.DrawLine(transform.position, hitPlayer.point, Color.red); //Draw a red line from the enemy to the player
                    _aiCharacterControl.SetTarget(HumanPlayer.transform);
                }
                else // If the raycast doesn't hit the player then continue with ELSE
                {
                    _playerSpotted = false; // Player has not been spotted
                }
            }
        }
    }
}
