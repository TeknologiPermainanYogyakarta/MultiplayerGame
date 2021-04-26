using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboAttack : MonoBehaviour
{
    [SerializeField]
    private float leeway = 1f;
    public ComboData comboMoves;

    [Header("Components")]
    [SerializeField] private int nextAttackIndex;
    private Attack curAttack;

    [SerializeField] private Animator anim;

    [SerializeField] private bool ignoreInput;
    [SerializeField] private bool isAnimating;
    [SerializeField] public bool isCombo;

    [Header("Debug")]
    [SerializeField] private UnityEngine.UI.Image waitComboBar;

    private void Start()
    {
        comboMoves.UpdatePossibleAttack(nextAttackIndex);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !ignoreInput)
        {
            processInput(AttackType.heavy);
        }
        else if (Input.GetButtonDown("Fire2") && !ignoreInput)
        {
            processInput(AttackType.light);
        }
    }

    private void processInput(AttackType input)
    {
        Attack att = comboMoves.GetAttackData(input);

        if (att == null)
        {
            ResetCombo();

            att = comboMoves.GetAttackData(input);
        }

        animateAttack(att);
    }

    private void animateAttack(Attack att)
    {
        if (isAnimating)
        {
            isCombo = true;
        }

        isAnimating = true;
        ignoreInput = true;

        anim.Play(att.Name, -1, 0);
    }

    public void DamageTarget() // Referenced in animation event
    {
        ignoreInput = false;
        nextAttackIndex++;
        comboMoves.UpdatePossibleAttack(nextAttackIndex);

        // Damage enemy in front
    }

    public void Idling()
    {
        isCombo = false;
        ResetCombo();
    }

    public void DoneAttackAnim()
    {
        isAnimating = false;
    }

    public void ResetCombo()
    {
        nextAttackIndex = 0;
        comboMoves.UpdatePossibleAttack(nextAttackIndex);
    }
}

[System.Serializable]
public class Attack
{
    public string Name;
    public AttackType InputType;
}

[System.Serializable]
public class ComboMove
{
    public string AttackName;
    public List<Attack> Attack = new List<Attack>();
}

[System.Serializable]
public class ComboData
{
    public List<ComboMove> ComboMoves = new List<ComboMove>();
    public List<Attack> PossibleAttack = new List<Attack>();

    public Attack GetAttackData(AttackType input)
    {
        for (int i = 0; i < PossibleAttack.Count; i++)
        {
            if (PossibleAttack[i].InputType == input)
            {
                return PossibleAttack[i];
            }
        }

        return null;
    }

    public void UpdatePossibleAttack(int index)
    {
        PossibleAttack.Clear();
        foreach (var move in ComboMoves)
        {
            if (move.Attack.Count > index)
                PossibleAttack.Add(move.Attack[index]);
        }
    }
}

public enum AttackType { heavy, light };