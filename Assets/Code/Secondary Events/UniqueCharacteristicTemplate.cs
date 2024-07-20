using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UniqueCharacteristicTemplate : SecondaryEvent, IAnimatable
{
    [SerializeField] private Image characteristicIcon;
    [SerializeField] private TMP_Text characteristicNameField;
    [SerializeField] private TMP_Text requirementsDescriptionField;
    [SerializeField] private ProgressBar skipSlider;

    public void ExecuteAnimation(float delay = 0)
    {
        skipSlider.SetCurrentValue(0);
        skipSlider.SetTargetValue(1f, 3f);
    }

    public bool AnimationFinished()
    {
        throw new NotImplementedException();
    }

    public void Setup(UniqueCharacteristicData characteristicData)
    {
        Characteristic characteristic = characteristicData.RewardCharacteristic;
        
        characteristicIcon.sprite = characteristic.Icon;
        characteristicNameField.text = characteristic.Name;
        requirementsDescriptionField.text = characteristicData.RequirementsDescription;
    }
}