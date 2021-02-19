using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Contraptions;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
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
    public int EnergyLevel = 100;
    public bool PlayerAlwaysSpotted;
    public bool AmIDeath { get; private set; } = false;

    [Header("Hero Abilities")]
    public HeroAbilityScriptable[] ActiveAbilities;    
    public HeroAbilityScriptable[] PassiveAbilities;    

    [Header("Self imposed abilities apply point")]
    public Transform ApplyPoint;

    public HeroAbilityScriptable InstObject;

    private AICharacterControl _aiCharacterControl;
    private Animator _animator;
    private BaseArtificialIntelligence _ai;
    private bool _bilboardInit = false;
    private RaycastHit _ubiquitousRaycastHit;
    private bool _playerSpotted = false;
    private LayerMask _lastPosLayer = (1 << 10), enemyLayer = (1 << 9), playerLayer = (1 << 8);
    private HeroAbilityManagerContraption hamc;
    private float _attackIndicator = 100;

//
//    // Start is called before the first frame update
//    void Start()
//    {
//
//    }

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

            AutoAttack();
        }

        if (!_bilboardInit)
        {
            InitBillboard();

            _bilboardInit = true;
        }

        if (AmIDeath && _aiCharacterControl.target != null)
        {
            Debug.Log(_aiCharacterControl.target);
        }
    }

    public void AutoAttack()
    {
        _attackIndicator += Time.deltaTime;
        float attackSpeedFloat = (float)60.0 / AttackSpeed;

        if (!isHumanControl && _attackIndicator > attackSpeedFloat && _aiCharacterControl.target == null)
        {
            Debug.Log("Slash " + _attackIndicator + " " + attackSpeedFloat);
        }

        if (!isHumanControl && (HumanPlayer.transform.position - transform.position).sqrMagnitude <= AttackRange * AttackRange)
        {
            _aiCharacterControl.SetTarget(null);
            Debug.Log("Human in range " + _attackIndicator + " " + attackSpeedFloat);
        }

        if (_attackIndicator > attackSpeedFloat && _aiCharacterControl.target == null)
        {
//            Debug.Log("Zombie Attack " + _attackIndicator);
            CharacterMainBridge[] enemies = FindObjectsOfType<CharacterMainBridge>()
                .Where(element => element.isHumanControl != isHumanControl && !element.AmIDeath).ToArray();

            foreach (CharacterMainBridge enemyAi in enemies)
            {
                if ((enemyAi.transform.position - transform.position).sqrMagnitude <= AttackRange * AttackRange)
                {
//                    Debug.Log("Enemy found - Zombie Attack " + _attackIndicator);
                    if (_attackIndicator > attackSpeedFloat)
                    {
                        if (!AmIDeath)
                        {
                            _attackIndicator = 0;

                            _animator.SetTrigger("Slash");

                            enemyAi.HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);


                            Quaternion hitRot = Quaternion.LookRotation(enemyAi.transform.position - transform.position);
                            transform.rotation = Quaternion.Euler(0, hitRot.eulerAngles.y + AnimationRotationError, 0);

                            break;
                        }
                    }
                }
            }
        }
    }

    public void Start()
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

        hamc = new HeroAbilityManagerContraption(PassiveAbilities, ApplyPoint, ActiveAbilities);
        hamc.RunPassive();
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

            if (_aiCharacterControl)
            {
                _aiCharacterControl.SetTarget(null);
            }

            Destroy(this.gameObject, 4f);
        }
    }

    public void EnemyVision()
    {
        if (PlayerAlwaysSpotted && HumanPlayer)
        {
            _playerSpotted = true;

            if ((HumanPlayer.transform.position - transform.position).sqrMagnitude <= (AttackRange * AttackRange * .9f))
            {
                
                _aiCharacterControl.SetTarget(null);
            }
            else
            {
                _aiCharacterControl.SetTarget(HumanPlayer.transform);
            }
        }

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
//    public void RunTransform(Transform applyTransform, HeroAbilityScriptable has)
//    {
////        Debug.Log("key pressed 1 " + (InstObject == null));
//
//        if (InstObject == null)
//        {
////            Debug.Log("key pressed 3 " + (InstObject == null));
//            has.TemporaryEffect = Instantiate(has.InstObject, applyTransform);
//
//            InstObject = has;
//            Invoke(nameof(ReleaseLock), has.StayingDuration);
//        }
//    }
//
//    public void ReleaseLock()
//    {
//        Destroy(InstObject.TemporaryEffect);
//
//        InstObject = null;
//    }

    public void CatchUserInput()
    {
//        Dictionary<KeyCode, HeroAbilityScriptable> keyCodeReflect = new Dictionary<KeyCode, HeroAbilityScriptable>()
//        {
//            { KeyCode.Q, AbilityQ },
//            { KeyCode.W, AbilityW },
//            { KeyCode.E, AbilityE },
//            { KeyCode.R, AbilityR },
//        };
//
//        HeroAbilityScriptable amc = null;
//        
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            amc = keyCodeReflect[KeyCode.Q];
//        } 
//        else if (Input.GetKeyDown(KeyCode.W))
//        {
//            amc = keyCodeReflect[KeyCode.W];
//        } 
//        else if (Input.GetKeyDown(KeyCode.E))
//        {
//            amc = keyCodeReflect[KeyCode.E];
//        }
//        else if (Input.GetKeyDown(KeyCode.R))
//        {
//            amc = keyCodeReflect[KeyCode.R];
//        }
//
//        if (amc != null)
//        {
//            RunTransform(ApplyPoint, amc);
//        }

        KeyCode keyPressed;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyPressed = KeyCode.Q;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            keyPressed = KeyCode.W;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            keyPressed = KeyCode.E;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            keyPressed = KeyCode.R;
        }
        else
        {
            keyPressed = KeyCode.None;
        }

        if (hamc != null)
        {
            IEnumerator abilityCoroutine = hamc.SetEnergyValue(EnergyLevel).RunActive(keyPressed);

            abilityCoroutine.MoveNext();

            if (abilityCoroutine.Current is HeroAbilityScriptable has)
            {
                EnergyLevel -= has.EnergyCost;
            }

            StartCoroutine(abilityCoroutine);
        }
    }
}
