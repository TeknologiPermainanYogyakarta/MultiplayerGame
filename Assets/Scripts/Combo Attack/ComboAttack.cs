using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboAttack : MonoBehaviour
{
    [SerializeField]
    private float leeway = 1f;
    public ComboData comboDatas;

    [Header("Components")]
    [SerializeField] private int nextAttackIndex;
    private Attack curAttack;

    [SerializeField] private Animator anim;

    [SerializeField] private bool ignoreInput;
    [SerializeField] private bool isAnimating;

    [Header("Debug")]
    [SerializeField] private UnityEngine.UI.Image waitComboBar;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!ignoreInput)
            {
                animateAttack();
            }
        }
    }

    private void animateAttack()
    {
        isAnimating = true;
        ignoreInput = true;

        if (nextAttackIndex >= comboDatas.Attack.Count)
        {
            nextAttackIndex = 0;
        }

        Attack att = comboDatas.Attack[nextAttackIndex];
        anim.Play(att.Name, -1, 0);

        forceStopWaitCombo();
    }

    // referenced in animation event
    public void DamageTarget()
    {
        ignoreInput = false;
        nextAttackIndex++;
    }

    public void DoneAttackAnim()
    {
        isAnimating = false;

        forceStopWaitCombo();
        startWaitNewCombo();

        if (nextAttackIndex >= comboDatas.Attack.Count) // if combo finished in attack anim
        {
            resetCombo();
            forceStopWaitCombo();
        }
    }

    private void resetCombo()
    {
        nextAttackIndex = 0;
    }

    #region wait combo leeway

    private void startWaitNewCombo()
    {
        LeanTween.value(gameObject, updateWaitCombo, 0, 1, leeway).setOnComplete(completeWaitCombo);
        waitComboBar.gameObject.SetActive(true);
    }

    private void updateWaitCombo(float hi)
    {
    }

    private void completeWaitCombo()
    {
        waitComboBar.gameObject.SetActive(false);

        resetCombo();
    }

    private void forceStopWaitCombo()
    {
        if (LeanTween.isTweening(this.gameObject))
        {
            LeanTween.cancel(this.gameObject);
        }
    }

    #endregion wait combo leeway
}

[System.Serializable]
public class Attack
{
    public string Name;
    public AttackType Inputs;
}

[System.Serializable]
public class ComboData
{
    public List<Attack> Attack;
}

public enum AttackType { heavy, light };