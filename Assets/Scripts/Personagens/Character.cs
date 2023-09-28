using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;

    public bool IsMoving { get; private set; }

    public float OffsetY { get; private set; } = 0.3f;

    //CharacterAnimator animator;
    private void Awake()
    {
        //animator = GetComponent<CharacterAnimator>();
        //SetPositionAndSnapToTile(transform.position);
    }

    public IEnumerator Move(Vector3 moveVec, Action OnMoveOver = null)
    {
        var targetPos = transform.position + moveVec;
        IsMoving = true;

        // Calcula a direção do movimento
        Vector3 moveDirection = moveVec.normalized;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            // Move o NPC
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Atualiza a rotação para olhar na direção do movimento (somente no plano horizontal)
            LookTowards(transform.position + moveDirection);

            yield return null;
        }

        transform.position = targetPos;
        IsMoving = false;

        OnMoveOver?.Invoke();
    }


    public void HandleUpdate()
    {
       // animator.IsMoving = IsMoving;
    }

    public void LookTowards(Vector3 targetPos)
    {
        // Crie uma rotação olhando para a posição de destino
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);

        // Aplique a rotação ao NPC, apenas girando em torno do eixo Y (vertical)
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
    }


    /*public CharacterAnimator Animator
    {
        get => animator;
    }
    */
}
