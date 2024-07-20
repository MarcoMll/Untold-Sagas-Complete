using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicInteractionSetup : SecondaryEvent, IAnimatable
{
    [SerializeField] private Image characteristicImage;
    [SerializeField] private TextVisualizer characteristicNameField;

    private string _characteristicName = "";
    private Material _imageMaterial = null;

    private IEnumerator ShowIcon()
    {
        float initialDissolveLevel = 1f;
        float dissolveTime = 0f;
        float dissolveSpeed = .5f;

        while (dissolveTime < 1f)
        {
            dissolveTime += Time.deltaTime * dissolveSpeed;
            float currentDissolveLevel = Mathf.Lerp(initialDissolveLevel, 0, dissolveTime);
            _imageMaterial.SetFloat("_Level", currentDissolveLevel);

            yield return null;
        }

        _imageMaterial.SetFloat("_Level", 0);
        characteristicNameField.WriteText(_characteristicName);
        yield break;
    }

    public void ExecuteAnimation(float delay = 0)
    {
        StartCoroutine(ShowIcon());
    }

    public bool AnimationFinished()
    {
        throw new System.NotImplementedException();
    }

    public void AnimateAfter(GameObject target)
    {

    }

    private void SetupImage()
    {
        _imageMaterial = characteristicImage.material;
    }
    
    public void SetCharacteristic(Characteristic characteristic)
    {
        characteristicImage.sprite = characteristic.Icon;
        _characteristicName = characteristic.Name;

        SetupImage();
    }
}