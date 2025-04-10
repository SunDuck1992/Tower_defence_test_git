using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthCanvas : MonoBehaviour
{
    [SerializeField] private GameUnit _gameUnit;
    [SerializeField] private Slider _slider;
    [SerializeField] private bool _isHideble;

    private void Start()
    {
        Debug.LogWarning($"MaxHealth: {_gameUnit.MaxHealth}, Health: {_gameUnit.Health}");

        //_slider.maxValue = _gameUnit.MaxHealth;
        //_slider.value = _gameUnit.Health;
        _slider.value = _slider.maxValue;
        _slider.gameObject.SetActive(!_isHideble);

        _gameUnit.HealthChanged += ChangeValue;

        //Debug.LogWarning(_slider.maxValue + " - maxValueSlider, " + _slider.value + " - sliderValue");
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        _gameUnit.HealthChanged -= ChangeValue;

    }

    private void ChangeValue(bool isReseted)
    {
        if (_isHideble)
        {
            if (isReseted)
            {
                _slider.gameObject.SetActive(false);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(Hide());
            }
        }

        //_slider.value =_gameUnit.Health;
        _slider.value = _gameUnit.Health / _gameUnit.MaxHealth;

        //Debug.LogWarning(_slider.maxValue + " - maxValueSlider, " + _slider.value + " - slederValue");
    }

    private IEnumerator Hide()
    {
        _slider.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        _slider.gameObject.SetActive(false);
    }
}
