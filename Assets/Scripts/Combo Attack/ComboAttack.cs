﻿using System;
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
        comboMoves.Reset();
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
        curAttack = comboMoves.GetAttackData(input);

        if (curAttack == null)
        {
            // no possible moves
            ResetCombo();

            curAttack = comboMoves.GetAttackData(input);
        }

        animateAttack(curAttack);
    }

    private void animateAttack(Attack att)
    {
        if (isAnimating)
        {
            isCombo = true;
        }

        isAnimating = true;
        ignoreInput = true;

        anim.Play(att.clip.name, -1, 0);
    }

    public void DamageTarget() // Referenced in animation event
    {
        ignoreInput = false;
        nextAttackIndex++;
        comboMoves.UpdatePossibleAttack(nextAttackIndex, curAttack.InputType);

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
        comboMoves.Reset();
    }
}

[System.Serializable]
public class Attack
{
    // the clip.name will be used
    public AnimationClip clip;
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
    [SerializeField]
    private List<ComboMove> ComboMoves = new List<ComboMove>();
    public List<ComboMove> PossibleMoves = new List<ComboMove>();
    public List<Attack> PossibleAttack = new List<Attack>();

    public Attack GetAttackData(AttackType input)
    {
        foreach (var att in PossibleAttack)
        {
            if (att.InputType == input) // always use the first match combo
            {
                return att;
            }
        }

        return null;
    }

    // need last inputs in array
    public void UpdatePossibleAttack(int index, AttackType lastType)
    {
        PossibleAttack.Clear();

        for (int i = PossibleMoves.Count - 1; i >= 0; i--)
        {
            if (PossibleMoves[i].Attack.Count <= index || PossibleMoves[i].Attack[index - 1].InputType != lastType)
            {
                PossibleMoves.RemoveAt(i);
            }
            else
            {
                PossibleAttack.Add(PossibleMoves[i].Attack[index]);
            }
        }
    }

    public void Reset()
    {
        PossibleMoves.Clear();
        PossibleMoves.AddRange(ComboMoves);

        PossibleAttack.Clear();

        foreach (var move in PossibleMoves)
        {
            PossibleAttack.Add(move.Attack[0]);
        }
    }
}

public enum AttackType { heavy, light };