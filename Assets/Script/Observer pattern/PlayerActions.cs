using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class PlayerActions : MonoBehaviour, IObserver
{
    [SerializeField] int _destroyCount = 0;
    int _destroyThreshold = 2;
    bool comboReset; 
    Coroutine _currentDestroyResetRoutine = null;
    Coroutine _currentPowerUpResetRoutine = null;
    Coroutine _currentShotgunPowerUpResetRoutine = null;
    public AudioSource _audioPlayer1, _audioPlayer2;
    public AudioClip _shootAudioClip;
    public Animator comboAnim;
    public AudioClip _comboAudioClip;
    public AudioClip _collectAudioClip;
    public CameraScript CS;
    public Image powerUpIcon, shotgunPowerUpIcon;
    public ParticleSystem splashPart;
    float powerUpTime, shotgunTime = 5f; 
    public GameObject puCount, shotgunCount; 
    TextMeshProUGUI puCountdown, shotgunCountdown;
    public bool puCollected, shotgunCollected; 

    public void OnNotify(GameObject GameObj, Action actionType)
    {
        switch (actionType)
        {
            case (Action.OnEnemyDestroy):
                splashPart.transform.position = GameObj.transform.position;
                if (_currentDestroyResetRoutine != null)
                {
                    StopCoroutine(_currentDestroyResetRoutine);
                }
                _destroyCount += 1;
                splashPart.GetComponent<ParticleController>().play = true;

                if (_destroyCount == _destroyThreshold && !comboReset)
                {
                    comboAnim.SetTrigger("Play");
                    comboReset = true; 
                }
                StartCoroutine(ComboResetRoutine());
                _currentDestroyResetRoutine = StartCoroutine(DestroyResetRoutine());
                break; //(exits the switch)
                //default(exits the whole void function)

            case (Action.OnPlayerShoot):
                CS.ShakeCam(0.05f, 0.01f);
                _audioPlayer2.Play();
                break;

            case (Action.OnPowerUpCollect):
                if (_currentPowerUpResetRoutine != null)
                {
                    StopCoroutine(_currentPowerUpResetRoutine);
                }
                _audioPlayer1.Play();
                powerUpIcon.color += new Color(0f, 0f, 0f, 0.5f);
                puCollected = true;
                powerUpTime = 5f;
                _currentPowerUpResetRoutine = StartCoroutine(PowerUpResetRoutine());
                break;

            case (Action.OnShotgunPowerUpCollect):
                if (_currentShotgunPowerUpResetRoutine != null)
                {
                    StopCoroutine(_currentShotgunPowerUpResetRoutine);
                }
                _audioPlayer1.Play();
                shotgunPowerUpIcon.color += new Color(0f, 0f, 0f, 0.5f);
                shotgunCollected = true;
                shotgunTime = 5f;
                _currentShotgunPowerUpResetRoutine = StartCoroutine(ShotgunPowerUpResetRoutine());
                break;

            default:
                break;
        }
        Debug.Log(actionType.ToString());
    }

    void Update()
    {
        if (puCollected)
        {
            
            puCount.SetActive(true);
            puCountdown = puCount.GetComponent<TextMeshProUGUI>();
            puCountdown.text = ((int)powerUpTime).ToString();
            powerUpTime -= Time.deltaTime;
            if (powerUpTime <= 0f)
            {
                puCount.SetActive(false);
                puCollected = false;
            }
        }

        if (shotgunCollected)
        {
            
            shotgunCount.SetActive(true);
            shotgunCountdown = shotgunCount.GetComponent<TextMeshProUGUI>();
            shotgunCountdown.text = ((int)shotgunTime).ToString();
            shotgunTime -= Time.deltaTime;
            if (shotgunTime <= 0f)
            {
                shotgunCount.SetActive(false);
                shotgunCollected = false;
                
            }
        }
    }

    IEnumerator DestroyResetRoutine()
    {
        yield return new WaitForSeconds(4f);
        _destroyCount = 0;
    }

    IEnumerator PowerUpResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        powerUpIcon.color -= new Color(0f, 0f, 0f, 0.5f);
        
    }

    IEnumerator ShotgunPowerUpResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        shotgunPowerUpIcon.color -= new Color(0f, 0f, 0f, 0.5f);
        
    }

    IEnumerator ComboResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        comboReset = false; 
    }
}