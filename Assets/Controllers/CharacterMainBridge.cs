using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Enums;
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
    public CharacterLayerEnum CharacterLayerEnum = CharacterLayerEnum.None;
    public int AnimationRotationError = 0;
    public bool AmIDeath { get; private set; } = false;

    [Header("QWER Abilities")]
    public HeroAbilityScriptable AbilityQ;
    public HeroAbilityScriptable AbilityW;
    public HeroAbilityScriptable AbilityE;
    public HeroAbilityScriptable AbilityR;

    [Header("Self imposed abilities apply point")]
    public Transform ApplyPoint;

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

            if (isHumanControl)
            {
                CatchUserInput();
            }
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
//            AnimatorClipInfo[] anstate = _animator.GetCurrentAnimatorClipInfo((int)CharacterLayerEnum);
//
//            if (anstate.Length > 0 && anstate[0].clip.name != "Standing Melee Attack Downward")
//                {
//                                Debug.Log("Clip lock passed " + gameObject.name);
                    CharacterMainBridge[] enemies = FindObjectsOfType<CharacterMainBridge>()
                        .Where(element => element.isHumanControl != isHumanControl && !element.AmIDeath).ToArray();
//                    Debug.Log("Enemies found " + gameObject.name + " " + enemies.Length);

                foreach (CharacterMainBridge enemyAi in enemies)
                {
//                        if (Vector3.Distance(enemyAi.transform.position ,transform.position) < AttackRange)
                    if ((enemyAi.transform.position - transform.position).sqrMagnitude <= AttackRange * AttackRange)
                    {
//                            Debug.Log(Vector3.Distance(enemyAi.transform.position, transform.position) + " " + AttackRange + " " + (Vector3.Distance(enemyAi.transform.position, transform.position) < (float)AttackRange));
                        if (!AmIDeath)
                            {

                                _animator.SetTrigger("Slash");
//                                Debug.Log("Super fuck " + enemyAi.gameObject.name);
//                                Debug.Log(enemyAi.HealthKickerContraption);

                                enemyAi.HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);


                                Quaternion hitRot = Quaternion.LookRotation(enemyAi.transform.position - transform.position);
                                transform.rotation = Quaternion.Euler(0, hitRot.eulerAngles.y + AnimationRotationError, 0);

                                break;
                            }
                        }
                    }
                }
//            }
//        }
    }

    public void OnEnable()
    {
        _aiCharacterControl = GetComponent<AICharacterControl>();
        
        HealthKickerContraption = new HealthKickerContraption();
        HealthKickerContraption.SetHealth(HealthPoint);

//        if (!isHumanControl)
//        {
//            GetComponent<NavMeshAgent>().stoppingDistance = AttackRange;
//        }

        _animator = GetComponent<Animator>();

        if (!isHumanControl)
        {
            _ai = new BaseArtificialIntelligence();
        }

        _animator.SetLayerWeight(0, 0);

        if (CharacterLayerEnum.Swordsman == CharacterLayerEnum)
        {
            _animator.SetLayerWeight(1, 1);
        }
        else if(CharacterLayerEnum.Archer == CharacterLayerEnum)
        {
            _animator.SetLayerWeight(2, 1);
        } 
        else if (CharacterLayerEnum.Zombie == CharacterLayerEnum)
        {
            _animator.SetLayerWeight(3, 1);
        } 
        else 
        {
            _animator.SetLayerWeight(1, 1);
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
//                Debug.Log("Set trigger Death");
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
                    GetComponent<NavMeshAgent>().stoppingDistance = AttackRange;
                }
                else // If the raycast doesn't hit the player then continue with ELSE
                {
                    _playerSpotted = false; // Player has not been spotted
                    GetComponent<NavMeshAgent>().stoppingDistance = 0.2f;
                }
            }
        }
    }
    public void RunTransform(Transform applyTransform, HeroAbilityScriptable has)
    {
//        Debug.Log("key pressed 1 " + (InstObject == null));

        if (InstObject == null)
        {
//            Debug.Log("key pressed 3 " + (InstObject == null));
            has.TemporaryEffect = Instantiate(has.InstObject, applyTransform);

            InstObject = has;
            Invoke(nameof(ReleaseLock), has.StayingDuration);
        }
    }

    public void ReleaseLock()
    {
        Destroy(InstObject.TemporaryEffect);

        InstObject = null;
    }

    public HeroAbilityScriptable InstObject;

    public void CatchUserInput()
    {
        Dictionary<KeyCode, HeroAbilityScriptable> keyCodeReflect = new Dictionary<KeyCode, HeroAbilityScriptable>()
        {
            { KeyCode.Q, AbilityQ },
            { KeyCode.W, AbilityW },
            { KeyCode.E, AbilityE },
            { KeyCode.R, AbilityR },
        };

        HeroAbilityScriptable amc = null;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            amc = keyCodeReflect[KeyCode.Q];
        } 
        else if (Input.GetKeyDown(KeyCode.W))
        {
            amc = keyCodeReflect[KeyCode.W];
        } 
        else if (Input.GetKeyDown(KeyCode.E))
        {
            amc = keyCodeReflect[KeyCode.E];
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            amc = keyCodeReflect[KeyCode.R];
        }

        if (amc != null)
        {
            RunTransform(ApplyPoint, amc);
        }
    }
}
