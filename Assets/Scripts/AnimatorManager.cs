using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    int x;
    int y;
    float dead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        x = Animator.StringToHash("X");
        y = Animator.StringToHash("Y");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        float snappedHorizontal = 0;
        float snappedVertical = 0;

        snappedHorizontal = SetSnap(snappedHorizontal, horizontalMovement);
        snappedVertical = SetSnap(snappedVertical, verticalMovement);
       
        animator.SetFloat(x, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(y, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void HandleDeathAnimation()
    {
        animator.SetBool("IsDead", true);
    }

    private float SetSnap(float snap, float movement)
    {
        if (movement > 0 && movement < 0.55f)
        {
            return snap = 0.5f;
        }

        else if (movement > 0.55f)
        {
            return snap = 1;
        }

        else if (movement < 0 && movement > -0.55f)
        {
            return snap = -0.5f;
        }


        else if (movement < -0.55f)
        {
            return snap = -1;
        }

        else
        {
            return snap = 0;
        }

    }
}
