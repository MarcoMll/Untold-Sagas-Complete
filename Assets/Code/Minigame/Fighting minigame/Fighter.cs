using System;
using System.Collections;
using System.Collections.Generic;
using FightingMinigameLogic;
using UnityEngine;
using Random = System.Random;

namespace FightingMinigameLogic
{
    [Serializable]
    public class Fighter
    {
        [SerializeField] private Character characterReference;
        [SerializeField] private int initialHealthPoints, initialSkillPoints;
        [SerializeField] private List<Item> inventory = new List<Item>();

        private CharacterStats _characterStats;

        private int _healthPoints;
        private int _skillPoints;
        private List<Item> _selectedItems;
        
        public Character Character => characterReference;
        public int HealthPoints => _healthPoints;
        public int SkillPoints => _skillPoints;
        public List<Item> Inventory => inventory;
        public List<Item> SelectedItems => _selectedItems;
        
        public CharacterStats CharacterStats => _characterStats;

        public Action<int> onHealthAmountChanged;
        public Action<int> onSkillsAmountChanged;
        public Action<Item> onInventoryChanged;

        public Fighter(Character character, int health, int skills)
        {
            if (character != null) characterReference = character;
            initialHealthPoints = health;
            initialSkillPoints = skills;
        }

        public void ChangeHealthPoints(int amount)
        {
            _healthPoints += amount;
            if (_healthPoints < 0) _healthPoints = 0;

            onHealthAmountChanged?.Invoke(HealthPoints);
        }

        public void ChangeSkillPoints(int amount)
        {
            _skillPoints += amount;
            if (_skillPoints < 0) _skillPoints = 0;

            onSkillsAmountChanged?.Invoke(SkillPoints);
        }

        public void AddItemToInventory(Item item)
        {
            inventory.Add(item);
            onInventoryChanged?.Invoke(item);
        }

        public void RemoveItemFromInventory(Item item)
        {
            if (inventory.Contains(item) == false)
            {
                Debug.LogError($"{item.name} not found in {this}'s inventory! \nCharacter reference: {characterReference}\nFighter inventory list: {Inventory}");
                return;
            }

            inventory.Remove(item);
            onInventoryChanged?.Invoke(item);
        }

        public void SelectRandomItemsFromInventory(int itemsAmount)
        {
            var availableItems = new List<Item>(inventory);
            var selectedItems = new List<Item>();
            
            if (itemsAmount > availableItems.Count)
            {
                Debug.LogError("The number of items required exceeds the number of available items in fighter's inventory!");
                return;
            }
            else if (itemsAmount <= 0)
            {
                return;
            }

            Random rand = new Random();
            for (var i = 0; i < itemsAmount; i++)
            {
                var randomIndex = rand.Next(0, availableItems.Count);
                selectedItems.Add(availableItems[randomIndex]);
                availableItems.RemoveAt(randomIndex);
            }
            
            SelectItems(selectedItems);
        }

        public void SelectItems(List<Item> newItemsList)
        {
            if (newItemsList == null)
            {
                Debug.LogError("Attempted to select a null items list.");
                return;
            }
            
            if (_selectedItems.Count > 0)
            {
                _selectedItems.Clear();
            }

            _selectedItems = newItemsList;
        }
        
        public void Initialize()
        {
            _healthPoints = initialHealthPoints;
            _skillPoints = initialSkillPoints;

            _selectedItems = new List<Item>();
            _characterStats = new CharacterStats();
        }
    }
}