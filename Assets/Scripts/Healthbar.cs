using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{   
    [SerializeField] public PlayerHealth playerHealth;
    [SerializeField] public Image totalHealthbar;
    [SerializeField] public Image currentHealthbar;
    [SerializeField] private Healthbar healthbar;
    [SerializeField] private float fillDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        totalHealthbar.fillAmount = playerHealth.currentHealth / 10;
        StartCoroutine(fillOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthbar.fillAmount = playerHealth.currentHealth / 10;
    }

    public IEnumerator fillOverTime() {

        yield return new WaitForSeconds(5f);

        float time = 0f;
        float startFillAmount = currentHealthbar.fillAmount;
        float targetFillAmount = 0.6f;

        while (time < fillDuration) {
            time += Time.deltaTime;
            float fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, time/fillDuration);
            currentHealthbar.fillAmount = fillAmount;
            yield return null;
        }

        currentHealthbar.fillAmount = targetFillAmount;
        totalHealthbar.fillAmount = targetFillAmount;

        playerHealth.setHealth(6f);

    }
    
}
