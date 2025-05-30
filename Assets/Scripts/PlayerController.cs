﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName = "GameOver";

    [SerializeField]
    private StageData stageData;

    [SerializeField]
    private KeyCode keyCodeAttack = KeyCode.Space;

    [SerializeField]
    private KeyCode keyCodeBoom = KeyCode.Z;

    private bool isDie = false;
    private Movement2D movement2D;
    private Weapon weapon;
    private Animator animator;

    private int score;
    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        weapon = GetComponent<Weapon>();
        animator = GetComponent<Animator>(); // Animator 없으면 null
    }

    private void Update()
    {
        if (isDie) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movement2D.MoveTo(new Vector3(x, y, 0));

        if (Input.GetKeyDown(keyCodeAttack))
        {
            weapon.StartFiring();
        }
        else if (Input.GetKeyUp(keyCodeAttack))
        {
            weapon.StopFiring();
        }

        if (Input.GetKeyDown(keyCodeBoom))
        {
            weapon.StartBoom();
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, stageData.LimitMin.x, stageData.LimitMax.x),
            Mathf.Clamp(transform.position.y, stageData.LimitMin.y, stageData.LimitMax.y));
    }

    public void OnDie()
    {
        weapon.StopFiring();
        movement2D.MoveTo(Vector3.zero);
        Destroy(GetComponent<CircleCollider2D>());
        isDie = true;

        // Animator가 있으면 애니메이션 재생, 없으면 즉시 씬 이동
        if (animator != null)
        {
            animator.SetTrigger("onDie"); // 애니메이션에서 OnDieEvent() 호출
        }
        else
        {
            SceneManager.LoadScene(nextSceneName); // 바로 씬 이동
        }
    }

    public void OnDieEvent()
    {
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(nextSceneName);
    }
}
