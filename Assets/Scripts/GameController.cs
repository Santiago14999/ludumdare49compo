using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private ObjectPlacer _placer;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PlayerWalk _playerWalk;
    [SerializeField] private Vector2 _objectsToPlaceRandom;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private AudioSource _fallSource;
    [SerializeField] private Text _itemsText;

    private int _objectsToPlace;

    private void Awake()
    {
        _placer.ObjectPlaced += OnObjectPlaced;
        _playerWalk.PlayerFelt += OnPlayerFelt;
        _playerWalk.PlayerFinished += OnPlayerFinished;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Play()
    {
        _startButton.SetActive(false);
        Physics.gravity = Vector3.up * -20f;
        ResetWorld();
    }

    private void OnPlayerFinished()
    {
        _startButton.GetComponentInChildren<Text>().text = "Press Any Key to Continue";
        Physics.gravity = Vector3.up * 20f;
        _startButton.SetActive(true);
    }

    private void OnPlayerFelt()
    {
        _fallSource.Play();
        Invoke(nameof(ResetWorld), 2f);
    }
    
    private void ResetWorld()
    {
        _playerWalk.ResetPlayer();
        _placer.LoadPlacedObjects();
        StartCoroutine(PlacingCoroutine());
    }

    private void OnObjectPlaced()
    {
        _objectsToPlace--;
        if (_objectsToPlace > 0)
        {
            _itemsText.text = "" + _objectsToPlace;
            _placer.SelectRandomObject();
        }
        else
        {
            _itemsText.gameObject.SetActive(false);
            StartCoroutine(WalkingCoroutine());
        }
    }

    private IEnumerator PlacingCoroutine()
    {
        _cameraController.ActivatePlacingCamera();
        yield return new WaitForSeconds(1f);
        _objectsToPlace = Random.Range((int)_objectsToPlaceRandom.x, (int)_objectsToPlaceRandom.y);
        _itemsText.gameObject.SetActive(true);
        _itemsText.text = "" + _objectsToPlace;
        _placer.SelectRandomObject();
    }

    private IEnumerator WalkingCoroutine()
    {
        _cameraController.ActivateWalkingCameraOrigin();
        yield return new WaitForSeconds(1f);
        _playerWalk.StartWalking();
        yield return new WaitForSeconds(1.5f);
        _placer.ActivateObjects();
    }
}
