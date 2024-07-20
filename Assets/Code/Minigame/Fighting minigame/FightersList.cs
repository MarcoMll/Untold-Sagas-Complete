using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingMinigameLogic
{
    public class FightersList : MonoBehaviour
    {
        [SerializeField] private Transform fightersGrid;
        [SerializeField] private FighterIconVisualizer fighterIconPrefab;

        private FighterIconVisualizer _currentSlot;
        private List<FighterIconVisualizer> _fighterIconVisualizers = new List<FighterIconVisualizer>();

        private void InstantiateFighterSlot(Fighter fighter)
        {
            FighterIconVisualizer slot = Instantiate(fighterIconPrefab, fightersGrid);
            slot.Initialize(fighter);
            
            _fighterIconVisualizers.Add(slot);
        }

        private void SelectFighterSlot(FighterIconVisualizer targetSlot)
        {
            foreach (var fighterIconSlot in _fighterIconVisualizers)
            {
                fighterIconSlot.Deselect();
            }
            
            targetSlot.Select();
            _currentSlot = targetSlot;
        }

        public void SelectSlot(int index)
        {
            SelectFighterSlot(_fighterIconVisualizers[index]);
        }
        
        public void VisualizeFighters(List<Fighter> fighters)
        {
            foreach (var fighter in fighters)
            {
                InstantiateFighterSlot(fighter);
            }
            
            SelectFighterSlot(_fighterIconVisualizers[0]);
        }
    }
}