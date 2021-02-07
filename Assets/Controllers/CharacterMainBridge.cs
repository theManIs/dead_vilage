using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMainBridge : MonoBehaviour
{
    public HealthKickerContraption HealthKickerContraption;
    public int AttackSpeed = 1;
    public bool Ally;
    public int AttackRange = 3;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        HealthKickerContraption = new HealthKickerContraption();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Ally)
        {
            if (Time.frameCount % (100 / AttackSpeed) == 0)
            {
                AnimatorClipInfo[] anstate = _animator.GetCurrentAnimatorClipInfo(0);

                if (anstate[0].clip.name != "mixamo.com")
                {
                    EnemyAI[] enemies = FindObjectsOfType<EnemyAI>();

                    foreach (EnemyAI enemyAi in enemies)
                    {
                        if ((enemyAi.transform.position - transform.position).sqrMagnitude < AttackRange)
                        {
                            if (!enemyAi.AmIDeath)
                            {
                                _animator.SetTrigger("Slash");
                                Debug.Log("Super fuck " + enemyAi.gameObject.name);

                                enemyAi.gameObject.GetComponent<CharacterMainBridge>()
                                    .HealthKickerContraption.hitMe(HealthKickerContraption.NormalDamage);

                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
