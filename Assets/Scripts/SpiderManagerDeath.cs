using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpiderManagerDeath : MonoBehaviour
{
    public Animator animator;
    private SpiderHP spiderHP;
    [SerializeField] SpiderHP death;

    // Start is called before the first frame update
    void Start()
    {
        spiderHP = GameObject.FindGameObjectWithTag("Spider").GetComponent<SpiderHP>();
    }

    // Update is called once per frame
    void Update()
    {
        if (death.dead)
        {
            animator.Play("FadeOut");
            Invoke("SwitchScene", 4f);
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}