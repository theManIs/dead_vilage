using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMainBridge : MonoBehaviour
{
    public BillboardHealth BillBoardHealth;
    public HealthKickerContraption HealthKickerContraption;
    public int AttackSpeed = 1;
    public int AttackRange = 3;
    public bool isHumanControl = true;
    public int HealthPoint = 10;
    public bool AmIDeath { get; private set; } = false;

    private Animator _animator;
    private BaseArtificialIntelligence _ai;
    private bool _bilboardInit = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!AmIDeath)
        {
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
                HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);
            AnimatorClipInfo[] anstate = _animator.GetCurrentAnimatorClipInfo(0);

                if (anstate[0].clip.name != "Standing Melee Attack Downward")
                {
                    CharacterMainBridge[] enemies = FindObjectsOfType<CharacterMainBridge>()
                        .Where(element => element.isHumanControl != isHumanControl && !element.AmIDeath).ToArray();

                    foreach (CharacterMainBridge enemyAi in enemies)
                    {
                        if ((enemyAi.transform.position - transform.position).sqrMagnitude < AttackRange)
                        {
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
        HealthKickerContraption = new HealthKickerContraption();
        HealthKickerContraption.SetHealth(HealthPoint);

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
}
