using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CustomInspector;

public class RelationshipInteractionSetup : SecondaryEvent, IAnimatable
{
    [SerializeField] private TMP_Text characterNameField;
    [SerializeField] private Image characterIcon;
    [SerializeField] private RelationshipSlider relationshipSlider;

    private Character _character;
    
    public void ExecuteAnimation(float delay = 0f)
    {
        relationshipSlider.TriggerSlider(animationDelayInSeconds: delay);
    }

    public bool AnimationFinished()
    {
        throw new System.NotImplementedException();
    }
    
    public void SetTargetRelationshipValue(float targetRelationship)
    {
        relationshipSlider.SetSliderValues(_character.Relationship, (int)targetRelationship);     
    }

    public void SetCharacter(Character character)
    {
        if (character == null)
        {
            Debug.LogError("Character is null!");
            return;
        }

        _character = character;
        characterIcon.sprite = _character.Idle;
        characterNameField.text = _character.Name;
    }
}