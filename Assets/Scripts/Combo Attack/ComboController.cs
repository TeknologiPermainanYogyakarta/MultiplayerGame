using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace net
{
    public class ComboController : MonoBehaviour
    {
        [Header("Inputs")]
        public KeyCode heavyKey;
        public KeyCode lightKey;
        public KeyCode kickKey;

        [Header("Attacks")]
        public Attack heavyAttack;
        public Attack lightAttack;
        public Attack kickAttack;

        public List<Combo> combos;

        public float comboLeeway = 0.2f;

        [Header("Components")]
        public Animator ani;
        private Attack curAttack = null;
        private ComboInput lastInput;
        private List<int> currentCombos = new List<int>();

        private float timer = 0;
        private float leeway = 0;

        private void Start()
        {
            PrimeCombos();
        }

        private void PrimeCombos()
        {
            for (int i = 0; i < combos.Count; i++)
            {
                Combo c = combos[i];
                c.onInputted.AddListener(() =>
                {
                // attack function
                Attack(c.comboAttack);
                });
            }
        }

        private void Update()
        {
            if (curAttack != null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    curAttack = null;
                }
            }
            if (currentCombos.Count > 0)
            {
                leeway += Time.deltaTime;
                if (leeway >= comboLeeway)
                {
                    if (lastInput != null)
                    {
                        Attack(getAttackFromType(lastInput.type));
                        lastInput = null;
                    }
                    ResetCombos();
                }
            }

            ComboInput input = null;
            if (Input.GetKeyDown(heavyKey))
                input = new ComboInput(AttackType.heavy);
            if (Input.GetKeyDown(lightKey))
                input = new ComboInput(AttackType.light);
            if (Input.GetKeyDown(kickKey))
                input = new ComboInput(AttackType.kick);

            if (input == null) return;
            lastInput = input;

            List<int> removed = new List<int>();
            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                if (c.ContinueCombo(input))
                {
                    leeway = 0;
                }
                else
                {
                    removed.Add(i);
                }
            }

            foreach (int i in removed)
            {
                currentCombos.RemoveAt(i);
            }

            if (currentCombos.Count >= 0)
            {
                Attack(getAttackFromType(input.type));
            }
        }

        private void ResetCombos()
        {
            for (int i = 0; i < currentCombos.Count; i++)

            {
                Combo c = combos[currentCombos[i]];

                c.ResetCombo();
            }

            currentCombos.Clear();
        }

        private void Attack(Attack att)
        {
            curAttack = att;
            timer = att.Length;
            ani.Play(att.Name, -1, 0);
        }

        private Attack getAttackFromType(AttackType t)
        {
            if (t == AttackType.heavy)
            {
                return heavyAttack;
            }
            else if (t == AttackType.light)
            {
                return lightAttack;
            }
            else if (t == AttackType.kick)
            {
                return kickAttack;
            }

            return null;
        }
    }

    [System.Serializable]
    public class Attack
    {
        public string Name;
        public float Length;
    }

    [System.Serializable]
    public class ComboInput
    {
        public AttackType type;

        public ComboInput(AttackType i)
        {
            type = i;
        }

        public bool IsSameAs(ComboInput test)
        {
            return (type == test.type); // if not moving?
        }
    }

    [System.Serializable]
    public class Combo
    {
        public Attack comboAttack;
        public List<ComboInput> Inputs;
        public UnityEvent onInputted;
        private int curInput = 0;

        public bool ContinueCombo(ComboInput i)
        {
            if (Inputs[curInput].IsSameAs(i))
            {
                curInput++;
                if (curInput >= Inputs.Count) // finished inputs
                {
                    onInputted.Invoke();
                    curInput = 0;
                }
                return true;
            }
            else
            {
                curInput = 0;
                return false;
            }
        }

        public ComboInput CurrentComboInput()
        {
            if (curInput >= Inputs.Count)
            {
                return null;
            }
            return Inputs[curInput];
        }

        public void ResetCombo()
        {
            curInput = 0;
        }
    }

    public enum AttackType { heavy, light, kick };
}