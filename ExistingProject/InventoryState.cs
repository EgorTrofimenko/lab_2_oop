using System;
using System.Collections.Generic;

namespace InventorySystem
{
    // ИНТЕРФЕЙС СОСТОЯНИЯ - контракт для всех состояний
    public interface IInventoryState
    {
        // Пытается добавить предмет в инвентарь
        bool TryAddItem(Item item, Inventory inventory);

        // Пытается удалить предмет
        bool TryRemoveItem(Item item, Inventory inventory);

        // Описание текущего состояния
        string GetStateDescription();
    }

    // СОСТОЯНИЕ 1 - Нормальное (Normal State)
    // Инвентарь работает как обычно: есть ограничение по слотам
    public class NormalInventoryState : IInventoryState
    {
        private const int MAX_SLOTS = 20; // Максимум 20 слотов

        public bool TryAddItem(Item item, Inventory inventory)
        {
            // Проверяем, есть ли свободное место
            if (inventory.GetItemCount() >= MAX_SLOTS)
            {
                Console.WriteLine("Инвентарь полон! Удалите что-нибудь.");
                // Переходим в состояние "переполненный"
                inventory.SetState(new OverflowInventoryState());
                return false;
            }

            Console.WriteLine($"Предмет {item.Name} добавлен в инвентарь.");
            return true;
        }

        public bool TryRemoveItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"Предмет {item.Name} удалён из инвентаря.");
            // Если инвентарь был переполненный, переходим обратно в нормальное состояние
            if (inventory.GetItemCount() < MAX_SLOTS)
            {
                inventory.SetState(new NormalInventoryState());
            }
            return true;
        }

        public string GetStateDescription()
        {
            return $"НОРМАЛЬНОЕ СОСТОЯНИЕ (максимум {MAX_SLOTS} слотов)";
        }
    }

    // СОСТОЯНИЕ 2 - Переполненный (Overflow State)
    // Инвентарь полон. Можно только удалять, не добавлять
    public class OverflowInventoryState : IInventoryState
    {
        public bool TryAddItem(Item item, Inventory inventory)
        {
            Console.WriteLine("ИНВЕНТАРЬ ПЕРЕПОЛНЕН! Невозможно добавить предмет.");
            Console.WriteLine("   Сначала удалите что-нибудь.");
            return false;
        }

        public bool TryRemoveItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"Предмет {item.Name} удалён.");
            // После удаления переходим в нормальное состояние
            inventory.SetState(new NormalInventoryState());
            return true;
        }

        public string GetStateDescription()
        {
            return "ПЕРЕПОЛНЕННОЕ СОСТОЯНИЕ (только удаление)";
        }
    }

    // СОСТОЯНИЕ 3 - Специальный режим (Warehouse/Хранилище)
    // Безлимитное хранилище, можно добавлять столько, сколько угодно
    public class WarehouseInventoryState : IInventoryState
    {
        public bool TryAddItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"[ХРАНИЛИЩЕ] Предмет {item.Name} добавлен (безлимит).");
            return true;
        }

        public bool TryRemoveItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"[ХРАНИЛИЩЕ] Предмет {item.Name} удалён.");
            return true;
        }

        public string GetStateDescription()
        {
            return "РЕЖИМ ХРАНИЛИЩА (безлимитное место)";
        }
    }

    // СОСТОЯНИЕ 4 - Блокированный (Locked State)
    // Инвентарь закрыт, ничего не могу ни добавлять, ни удалять
    public class LockedInventoryState : IInventoryState
    {
        private string lockReason;

        public LockedInventoryState(string reason = "Инвентарь заблокирован")
        {
            lockReason = reason;
        }

        public bool TryAddItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"Инвентарь заблокирован: {lockReason}");
            return false;
        }

        public bool TryRemoveItem(Item item, Inventory inventory)
        {
            Console.WriteLine($"Инвентарь заблокирован: {lockReason}");
            return false;
        }

        public string GetStateDescription()
        {
            return $"ЗАБЛОКИРОВАНО ({lockReason})";
        }
    }
}