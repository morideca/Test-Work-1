using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> coinPoints;
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject youWin;
    [SerializeField]
    private GameObject youDie;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private List<Image> lifeImages;

    private AudioSource coinSound;

    private int coinsForWin;
    private int currentCoins;
    private int lifes;

    private void OnEnable()
    {
        Coin.TookCoin += CalculateCoins;
        Player.Damaged += GetDamage;
        DeadZone.Killed += Died;
        Chest.Winned += Win;
    }

    private void OnDisable()
    {
        Coin.TookCoin -= CalculateCoins;
        Player.Damaged -= GetDamage;
        DeadZone.Killed -= Died;
        Chest.Winned -= Win;
    }

    void Start()
    {
        coinSound = GetComponent<AudioSource>();
        scoreText.text = "0";
        coinsForWin = coinPoints.Count;
        lifes = lifeImages.Count;
        foreach (var point in coinPoints)
        {
            Instantiate(coin, point.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    private void CalculateCoins()
    {
        currentCoins++;
        coinSound.Play();
        scoreText.text = currentCoins.ToString();
        if (currentCoins == coinsForWin)
        {
            Win();
        }
    }

    private void Win()
    {
        Time.timeScale = 0f;
        youWin.active = true;
        restartButton.active = true;
    }

    private void GetDamage()
    {
        if (lifeImages.Count > 0)
        {
            lifes -= 1;
            StartCoroutine(MinusLife());
            if (lifes == 0) Died();
        }
    }

    private void Died()
    {
        Time.timeScale = 0f;
        youDie.active = true;
        restartButton.active = true;
        Debug.Log("Die");
    }

    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    private IEnumerator MinusLife()
    {
        var _image = lifeImages[lifeImages.Count - 1];
        float elapsed = 0f;
        Color startColor = _image.color;
        Color endColor = new Color(0f, 0f, 0f, 0f);
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            _image.color = Color.Lerp(startColor, endColor, elapsed / 0.3f);
            yield return null;
        }
        Destroy(lifeImages[lifeImages.Count - 1].gameObject);
        lifeImages.RemoveAt(lifeImages.Count - 1);
    }
}
