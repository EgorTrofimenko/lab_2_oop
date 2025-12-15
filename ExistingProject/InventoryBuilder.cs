using System;
using System.Collections.Generic;

namespace InventorySystem
{
    // BUILDER КЛАСС для создания инвентаря
    public class InventoryBuilder
    {
        // Приватные поля для параметров инвентаря
        private string _ownerName = "Неизвестный";
        private double _maxWeight = 50.0;
        private IOrganizationStrategy _organizationStrategy = new NoSortingStrategy();
        private List<Item> _initialItems = new List<Item>();
        private bool _useWarehouseMode = false;

        /// Устанавливает имя владельца инвентаря
        public InventoryBuilder WithOwnerName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым");

            _ownerName = name;
            return this; // Возвращаем этот же экземпляр класса для цепочки вызовов
        }

        /// Устанавливает максимальный вес предметов
        public InventoryBuilder WithMaxWeight(double maxWeight)
        {
            if (maxWeight <= 0)
                throw new ArgumentException("Максимальный вес должен быть положительным");

            _maxWeight = maxWeight;
            return this;
        }

        /// Устанавливает стратегию организации предметов
        public InventoryBuilder WithOrganizationStrategy(IOrganizationStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));

            _organizationStrategy = strategy;
            return this;
        }

        /// Добавляет начальный предмет в инвентарь
        public InventoryBuilder AddInitialItem(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _initialItems.Add(item);
            return this;
        }

        /// Добавляет несколько начальных предметов
        public InventoryBuilder AddInitialItems(params Item[] items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                if (item != null)
                    _initialItems.Add(item);
            }
            return this;
        }

        /// Включает режим хранилища (безлимитное место)
        public InventoryBuilder UseWarehouseMode(bool enable = true)
        {
            _useWarehouseMode = enable;
            return this;
        }

        /// ФИНАЛЬНЫЙ МЕТОД - создаёт Inventory с накопленными параметрами
        public Inventory Build()
        {
            // Проверяем: сумма весов не должна превышать максимума
            double totalWeight = 0;
            foreach (var item in _initialItems)
            {
                totalWeight += item.Weight;
            }

            if (totalWeight > _maxWeight)
            {
                Console.WriteLine($"Предупреждение: Начальные предметы весят {totalWeight}, " +
                                $"а максимум {_maxWeight}");
            }

            // Создаём и возвращаем новый инвентарь
            var inventory = new Inventory(
                _ownerName,
                _maxWeight,
                _organizationStrategy,
                _useWarehouseMode
            );

            // Добавляем начальные предметы
            foreach (var item in _initialItems)
            {
                inventory.AddItem(item);
            }

            return inventory;
        }
    }
}