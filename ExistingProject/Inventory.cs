using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem
{
    public class Inventory
    {
        // свойства (Properties)
        public string OwnerName { get; private set; }
        public double MaxWeight { get; private set; }

        // Приватное хранилище предметов
        private List<Item> _items;

        // Текущая используемая стратегия организации
        private IOrganizationStrategy _currentStrategy;

        // Текущее состояние инвентаря (State паттерн)
        private IInventoryState _currentState;

        // КОНСТРУКТОР
        // Этот конструктор вызывается из Builder, но можно использовать и напрямую
        public Inventory(string ownerName, double maxWeight, IOrganizationStrategy strategy, bool useWarehouseMode = false)
        {
            OwnerName = ownerName;
            MaxWeight = maxWeight;
            _items = new List<Item>();
            _currentStrategy = strategy ?? new NoSortingStrategy();

            // Устанавливаем начальное состояние
            if (useWarehouseMode)
            {
                _currentState = new WarehouseInventoryState();
            }
            else
            {
                _currentState = new NormalInventoryState();
            }
        }

        // МЕТОДЫ - Управление предметами

        /// Добавляет предмет в инвентарь
        /// Использует текущее состояние (State паттерн) для проверки возможности
        public bool AddItem(Item item)
        {
            if (item == null)
            {
                Console.WriteLine("Ошибка: нельзя добавить null предмет");
                return false;
            }

            // Делегируем проверку текущему состоянию
            if (_currentState.TryAddItem(item, this))
            {
                _items.Add(item);
                return true;
            }

            return false;
        }

        /// Удаляет предмет из инвентаря
        public bool RemoveItem(Item item)
        {
            if (item == null || !_items.Contains(item))
            {
                Console.WriteLine("Предмет не найден в инвентаре");
                return false;
            }

            if (_currentState.TryRemoveItem(item, this))
            {
                _items.Remove(item);
                return true;
            }

            return false;
        }
        /// Возвращает количество предметов в инвентаре
        public int GetItemCount()
        {
            return _items.Count;
        }

        /// Возвращает общий вес всех предметов
        public double GetTotalWeight()
        {
            return _items.Sum(item => item.Weight);
        }

        // МЕТОДЫ - Организация и просмотр

        /// Изменяет стратегию организации предметов
        public void SetOrganizationStrategy(IOrganizationStrategy newStrategy)
        {
            if (newStrategy == null)
            {
                Console.WriteLine("Стратегия не может быть null");
                return;
            }

            _currentStrategy = newStrategy;
            Console.WriteLine($"Стратегия изменена на: {_currentStrategy.GetStrategyName()}");
        }

        /// Отображает предметы согласно текущей стратегии
        public void DisplayItems()
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine($"ИНВЕНТАРЬ [{OwnerName}] | Состояние: {_currentState.GetStateDescription()}");
            Console.WriteLine($"   Предметов: {GetItemCount()} | Вес: {GetTotalWeight():F1} / {MaxWeight} кг");
            Console.WriteLine(new string('=', 80));

            if (_items.Count == 0)
            {
                Console.WriteLine("Инвентарь пуст");
            }
            else
            {
                // Применяем текущую стратегию к предметам
                var organizedItems = _currentStrategy.Organize(_items);

                Console.WriteLine($"Организованы по: {_currentStrategy.GetStrategyName()}\n");

                for (int i = 0; i < organizedItems.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {organizedItems[i].GetDescription()}");
                }
            }
            Console.WriteLine(new string('=', 80) + "\n");
        }
        // МЕТОДЫ - Управление состоянием (State паттерн)

        /// Устанавливает новое состояние инвентаря
        /// (используется из State классов)
        public void SetState(IInventoryState newState)
        {
            _currentState = newState;
        }

        /// Блокирует инвентарь с указанной причиной
        public void LockInventory(string reason = "Инвентарь заблокирован")
        {
            _currentState = new LockedInventoryState(reason);
            Console.WriteLine($"{reason}");
        }

        /// Разблокирует инвентарь
        public void UnlockInventory()
        {
            _currentState = new NormalInventoryState();
            Console.WriteLine("Инвентарь разблокирован");
        }

        /// Включает режим хранилища
        public void ActivateWarehouseMode()
        {
            _currentState = new WarehouseInventoryState();
            Console.WriteLine("Режим хранилища активирован");
        }

        /// Показывает статистику
        public void ShowStatistics()
        {
            Console.WriteLine("СТАТИСТИКА ИНВЕНТАРЯ");

            var totalValue = _items.Sum(item => item.Value);
            var avgWeight = _items.Count > 0 ? GetTotalWeight() / _items.Count : 0;
            var weaponCount = _items.OfType<Weapon>().Count();
            var armorCount = _items.OfType<Armor>().Count();
            var consumableCount = _items.OfType<Consumable>().Count();
            var questCount = _items.OfType<QuestItem>().Count();

            Console.WriteLine($"Общая стоимость: {totalValue}");
            Console.WriteLine($"Средний вес: {avgWeight:F2} кг");
            Console.WriteLine($"Оружие: {weaponCount}");
            Console.WriteLine($"Броня: {armorCount}");
            Console.WriteLine($"Зелья: {consumableCount}");
            Console.WriteLine($"Квесты: {questCount}");

            // Группировка по редкости
            Console.WriteLine($"\n  По редкости:");
            foreach (ItemRarity rarity in Enum.GetValues(typeof(ItemRarity)))
            {
                int count = _items.Count(item => item.Rarity == rarity);
                if (count > 0)
                    Console.WriteLine($"    - {rarity}: {count}");
            }

            Console.WriteLine(new string('─', 60) + "\n");
        }
    }
}