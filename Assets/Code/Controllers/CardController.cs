using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [SerializeField] private DecisionCard decisionCardPrefab;
    [SerializeField] private Transform cardGrid;

    private List<DecisionCard> _currentCards = new List<DecisionCard>();

    private EventsManager _eventsManager;

    private void Awake()
    {
        _eventsManager = GetComponent<EventsManager>();
    }

    private bool ConditionsMet(Decision decision)
    {
        if (decision.CharacteristicRequirement())
        {
            bool conditionMet = PlayerStats.Instance.HasCharacteristic(decision.RequiredCharacteristic);
            if (conditionMet == false) return false;
        }

        if (decision.ItemRequirement())
        {
            bool conditionMet = PlayerStats.Instance.ItemHandler.HasItem(decision.RequiredItem) || PlayerStats.Instance.ItemHandler.QuickSlots.Contains(decision.RequiredItem);
            if (conditionMet == false) return false;
        }

        if (decision.Played)
        {
            return false;
        }

        return true;
    }

    public void ClearCards()
    {
        foreach (DecisionCard card in _currentCards)
        {
            Destroy(card.gameObject);
        }

        if (_currentCards.Count > 0) _currentCards.Clear();
    }

    public void SpawnCards(int currentEventID)
    {
        Chapter currentChapter = _eventsManager.GetCurrentChapter();
        GameEvent currentEvent = currentChapter.GetEvent(currentEventID);

        if (currentEvent.Decisions.Length > 0)
        {
            foreach (Decision decision in currentEvent.Decisions)
            {
                if (ConditionsMet(decision) == false) continue;

                DecisionCard decisionCard = Instantiate(decisionCardPrefab.gameObject, cardGrid.position,
                    decisionCardPrefab.transform.rotation).GetComponent<DecisionCard>();

                decisionCard.transform.parent = cardGrid;
                decisionCard.InitializeCard(decision, _eventsManager);

                _currentCards.Add(decisionCard);
            }
        }
    }

    public void HideAllCardsOnTheScene(DecisionCard exception)
    {
        foreach (DecisionCard card in _currentCards)
        {
            if (card == exception) continue;
            card.CardTransformHandler.HideCard();
        }
    }
}
