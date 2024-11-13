using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject switchRoomOverlay;

    public GameObject loader;

    public enum GameState
    {
        HowToPlay,
        Playing,
        GameOver,
    }

    public GameObject clockTimer;

    public GameState currentGameState = GameState.HowToPlay;

    private bool isCameraAnimating = false;

    private float elapsedClock = 0;
    private int elapsedMinuteClock = 57;

    private int elapsedSecondClock = 0;

    public GameObject staticOverlay;

    public GameObject popUp;

    public GameObject target;

    public GameObject square;

    private Tween pendulumTwin;

    private bool squareOnPendulum;

    private bool staticOverlayIsAnimating;

    private bool overwhelming1 = false;
    private bool overwhelming2 = false;
    private bool overwhelming3 = false;

    public Image simpleOverlay;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        loader.SetActive(false);
        SoundManager.instance.PlaySound(SoundManager.Sound.HowToPlay);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            MoveCameraFade(false);
        }

        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            MoveCameraFade(true);
        }

        if (currentGameState == GameState.Playing)
        {
            IncreaseClockTimer();
            IncreaseOverlayStrength();
        }

        RemovalLoaderFollowMousePosition();

        if (elapsedMinuteClock == 0 && currentGameState == GameState.Playing)
        {
            TriggerGameOver();
        }

        if (Vector3.Distance(square.transform.position, target.transform.position) < 0.2f)
        {
            squareOnPendulum = true;
        }
        else
        {
            squareOnPendulum = false;
        }
    }

    void TriggerGameOver()
    {
        currentGameState = GameState.GameOver;
        ScoreManager.instance.SaveHighScore();
        // Debug.Log("Game Over!");
        StartCoroutine(LoadNextLevel());
    }

    void RemovalLoaderFollowMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        loader.transform.position = new Vector3(mousePosition.x, mousePosition.y - 1, loader.transform.position.z);
    }

    void IncreaseClockTimer()
    {
        if (elapsedMinuteClock != 0)
        {
            elapsedClock += Time.deltaTime;

            if (elapsedClock > 1f && elapsedMinuteClock != 60)
            {
                TextMeshProUGUI clockTimerText = clockTimer.GetComponent<TextMeshProUGUI>();

                elapsedSecondClock++;

                if (elapsedSecondClock == 60)
                {
                    elapsedClock = 0;
                    elapsedMinuteClock++;
                    elapsedSecondClock = 0;
                }

                if (elapsedSecondClock < 10)
                {

                    clockTimerText.text = $"23:{elapsedMinuteClock}:0{elapsedSecondClock}";
                }
                else
                {
                    clockTimerText.text = $"23:{elapsedMinuteClock}:{elapsedSecondClock}";
                }

                elapsedClock = 0;
            }

            if (elapsedMinuteClock == 60)
            {
                TextMeshProUGUI clockTimerText = clockTimer.GetComponent<TextMeshProUGUI>();
                elapsedMinuteClock = 0;
                clockTimerText.text = $"00:00:00";
            }
        }
    }

    void IncreaseOverlayStrength()
    {
        Image staticOverlaySprite = staticOverlay.GetComponent<Image>();

        if (elapsedMinuteClock == 58 && !staticOverlayIsAnimating && !overwhelming1)
        {
            overwhelming1 = true;
            DOTween.To(() => staticOverlaySprite.color.a,
                   x =>
                   {
                       Color color = staticOverlaySprite.color;
                       color.a = x; // Update the alpha value
                       staticOverlaySprite.color = color; // Set the updated color
                   },
                   0.25f,
                   5f)
                   .OnPlay(() => staticOverlayIsAnimating = true)
                   .OnComplete(() => staticOverlayIsAnimating = false);
        }

        if (elapsedMinuteClock == 59 && !staticOverlayIsAnimating)
        {
            if (elapsedSecondClock < 30 && !overwhelming2)
            {
                overwhelming2 = true;
                DOTween.To(() => staticOverlaySprite.color.a,
                       x =>
                       {
                           Color color = staticOverlaySprite.color;
                           color.a = x; // Update the alpha value
                           staticOverlaySprite.color = color; // Set the updated color
                       },
                       0.5f,
                       5f)
                       .OnPlay(() => staticOverlayIsAnimating = true)
                       .OnComplete(() => staticOverlayIsAnimating = false);
            }
            else if (elapsedSecondClock > 30 && !overwhelming3)
            {
                overwhelming3 = true;
                DOTween.To(() => staticOverlaySprite.color.a,
                   x =>
                   {
                       Color color = staticOverlaySprite.color;
                       color.a = x; // Update the alpha value
                       staticOverlaySprite.color = color; // Set the updated color
                   },
                   0.95f,
                   10f)
                   .OnPlay(() => staticOverlayIsAnimating = true)
                   .OnComplete(() => staticOverlayIsAnimating = false);

                AnomalyManager.instance.DistortAllObjects();
            }
        }
    }

    // void MoveCamera(bool toRight)
    // {
    //     Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    //     Image overlayImage = switchRoomOverlay.GetComponent<Image>();

    //     if (mainCamera && !isCameraAnimating)
    //     {
    //         if (toRight && mainCamera.transform.position.x != 64)
    //         {
    //             mainCamera.transform.DOMoveX(mainCamera.transform.position.x + 64, 1f).OnPlay(() =>
    //             {
    //                 isCameraAnimating = true;
    //                 // overlayImage.DOFade(1, 0.4f).OnComplete(() =>
    //                 // {
    //                 //     overlayImage.DOFade(0, 0.4f);
    //                 // });
    //             }).OnComplete(() =>
    //             {
    //                 isCameraAnimating = false;
    //             });
    //         }
    //         else if (!toRight && mainCamera.transform.position.x != -64)
    //         {
    //             mainCamera.transform.DOMoveX(mainCamera.transform.position.x - 64, 1f).OnPlay(() =>
    //             {
    //                 isCameraAnimating = true;
    //                 // overlayImage.DOFade(1, 0.4f).OnComplete(() =>
    //                 // {
    //                 //     overlayImage.DOFade(0, 0.4f);
    //                 // });
    //             }).OnComplete(() =>
    //             {
    //                 isCameraAnimating = false;
    //             });
    //         }
    //     }
    // }

    void MoveCameraFade(bool toRight)
    {
        Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Image overlayImage = switchRoomOverlay.GetComponent<Image>();

        if (mainCamera && !isCameraAnimating)
        {
            if (toRight && mainCamera.transform.position.x != 64)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.SwitchRoom);
                isCameraAnimating = true;
                overlayImage.DOFade(1, 0.5f).OnComplete(() =>
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 64, mainCamera.transform.position.y, mainCamera.transform.position.z);
                    overlayImage.DOFade(0, 0.5f);
                    isCameraAnimating = false;
                });
            }
            else if (!toRight && mainCamera.transform.position.x != -64)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.SwitchRoom);
                isCameraAnimating = true;
                overlayImage.DOFade(1, 0.5f).OnComplete(() =>
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - 64, mainCamera.transform.position.y, mainCamera.transform.position.z);
                    overlayImage.DOFade(0, 0.5f);
                    isCameraAnimating = false;
                });
            }
        }
    }

    public void UpdateRemovalLoader(float scaleX)
    {
        if (scaleX > 0)
        {
            loader.SetActive(true);
        }
        else
        {
            loader.SetActive(false);
        }

        GameObject progress = loader.transform.GetChild(0).gameObject;

        progress.transform.localScale = new Vector3(scaleX, progress.transform.localScale.y, progress.transform.localScale.z);
    }

    public void ClosePopUp()
    {
        CanvasGroup popUpCanvasGroup = popUp.GetComponent<CanvasGroup>();

        simpleOverlay.DOFade(0, 1f);

        popUpCanvasGroup.DOFade(0, 1f).OnComplete(() =>
        {
            currentGameState = GameState.Playing;
            SoundManager.instance.PlaySound(SoundManager.Sound.MainBGM);
            popUpCanvasGroup.gameObject.SetActive(false);
        });

        SoundManager.instance.PlaySound(SoundManager.Sound.UIClick);
    }

    public void StartAnomalyFixing()
    {
        float randomX = Random.Range(-1, 1.75f);
        target.transform.DOLocalMoveX(randomX, 0);
        loader.SetActive(true);
        pendulumTwin = square.transform.DOLocalMoveX(1.75f, 1.25f)
        .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    public bool EndAnomalyFixing()
    {
        loader.SetActive(false);
        pendulumTwin.Kill();
        square.transform.DOLocalMoveX(-1.75f, 0);

        return squareOnPendulum;
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3f);
        LevelLoader.Instance.LoadNextLevel();
    }
}
