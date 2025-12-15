using System;
using System.Collections.Generic;

namespace InventorySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ ИНВЕНТАРЕМ RPG - ДЕМОНСТРАЦИЯ ПАТТЕРНОВ ПРОЕКТИРОВАНИЯ");

            // 1. ABSTRACT FACTORY - Создание предметов
            Console.WriteLine("\n\nЭТАП 1: ABSTRACT FACTORY - Создание предметов\n");

            // Получаем фабрику обычных предметов
            IItemFactory commonFactory = new CommonItemFactory();
            var commonSword = commonFactory.CreateWeapon();
            var commonArmor = commonFactory.CreateArmor();
            var commonPotion = commonFactory.CreateConsumable();

            Console.WriteLine("Созданы обычные предметы:");
            Console.WriteLine($"   - {commonSword.GetDescription()}");
            Console.WriteLine($"   - {commonArmor.GetDescription()}");
            Console.WriteLine($"   - {commonPotion.GetDescription()}");

            // Получаем фабрику редких предметов
            IItemFactory rareFactory = new RareItemFactory();
            var rareSword = rareFactory.CreateWeapon();
            var rareArmor = rareFactory.CreateArmor();

            Console.WriteLine("\nСозданы редкие предметы:");
            Console.WriteLine($"   - {rareSword.GetDescription()}");
            Console.WriteLine($"   - {rareArmor.GetDescription()}");

            // 2. BUILDER - Построение инвентаря с гибкими параметрами
            Console.WriteLine("\n\nЭТАП 2: BUILDER - Построение инвентаря\n");

            var playerInventory = new InventoryBuilder()
                .WithOwnerName("Герой-Испытатель")
                .WithMaxWeight(80.0)
                .WithOrganizationStrategy(new NoSortingStrategy())
                .AddInitialItem(commonSword)
                .AddInitialItem(commonArmor)
                .AddInitialItems(commonPotion)
                .Build();

            Console.WriteLine("Инвентарь создан через Builder!");

            // 3. STATE - Демонстрация разных состояний инвентаря
            Console.WriteLine("\n\nЭТАП 3: STATE - Состояния инвентаря\n");

            // Начальное состояние - Нормальное
            Console.WriteLine("Состояние 1: НОРМАЛЬНОЕ (Есть свободное место)");
            playerInventory.DisplayItems();

            // Добавляем много предметов, чтобы переполнить
            Console.WriteLine("Добавляем 25 предметов...\n");
            IItemFactory legendaryFactory = new LegendaryItemFactory();
            
            for (int i = 0; i < 25; i++)
            {
                var item = commonFactory.CreateConsumable();
                if (!playerInventory.AddItem(item))
                {
                    break; // Когда инвентарь переполнится, break
                }
            }

            // Состояние изменилось на Переполненный
            Console.WriteLine("\nСостояние 2: ПЕРЕПОЛНЕННЫЙ (Нельзя добавлять, только удалять)");
            playerInventory.DisplayItems();

            // Попытаемся добавить ещё предмет
            Console.WriteLine("\nПопытка добавить ещё один предмет:");
            playerInventory.AddItem(rareSword);

            // Удалим несколько предметов
            Console.WriteLine("\nУдаляем несколько предметов...\n");
            for (int i = 0; i < 10; i++)
            {
                playerInventory.RemoveItem(commonPotion);
            }

            // Состояние вернулось в Нормальное
            Console.WriteLine("\nСостояние 3: Вернулось в НОРМАЛЬНОЕ после удаления");
            playerInventory.DisplayItems();

            // Активируем режим хранилища (без ограничений)
            Console.WriteLine("\nАктивируем режим ХРАНИЛИЩА (безлимитное место):");
            playerInventory.ActivateWarehouseMode();

            Console.WriteLine("Добавляем 50 легендарных предметов в хранилище...\n");
            for (int i = 0; i < 50; i++)
            {
                var item = legendaryFactory.CreateConsumable();
                playerInventory.AddItem(item);
            }

            Console.WriteLine("50 предметов добавлено! Хранилище справилось.");
            Console.WriteLine($"Всего предметов: {playerInventory.GetItemCount()}");

            // 4. STRATEGY - Демонстрация разных стратегий организации
            Console.WriteLine("\n\nЭТАП 4: STRATEGY - Стратегии организации предметов\n");

            // Вернёмся к нормальному инвентарю
            var heroInventory = new InventoryBuilder()
                .WithOwnerName("Рыцарь")
                .WithMaxWeight(100.0)
                .AddInitialItems(
                    commonSword,
                    rareSword,
                    commonArmor,
                    rareArmor,
                    commonPotion,
                    legendaryFactory.CreateWeapon(),
                    new Consumable("Зелье маны", 0.3, ItemRarity.Common, 50, "Манна", 5),
                    new Consumable("Эликсир силы", 0.5, ItemRarity.Rare, 200, "Урон +50%", 2)
                )
                .Build();

            Console.WriteLine("Стратегия 1: БЕЗ СОРТИРОВКИ (как добавлены)");
            heroInventory.DisplayItems();

            Console.WriteLine("\nПереходим к следующей стратегии...\n");
            heroInventory.SetOrganizationStrategy(new SortByRarityStrategy());
            Console.WriteLine("Стратегия 2: СОРТИРОВКА ПО РЕДКОСТИ");
            heroInventory.DisplayItems();

            heroInventory.SetOrganizationStrategy(new SortByWeightStrategy());
            Console.WriteLine("Стратегия 3: СОРТИРОВКА ПО ВЕСУ");
            heroInventory.DisplayItems();

            heroInventory.SetOrganizationStrategy(new SortByValueStrategy());
            Console.WriteLine("Стратегия 4: СОРТИРОВКА ПО СТОИМОСТИ");
            heroInventory.DisplayItems();

            heroInventory.SetOrganizationStrategy(new GroupByTypeStrategy());
            Console.WriteLine("Стратегия 5: ГРУППИРОВКА ПО ТИПАМ");
            heroInventory.DisplayItems();

            // 5. КОМБИНИРОВАНИЕ ВСЕХ ПАТТЕРНОВ - Реальный сценарий
            Console.WriteLine("\n\nБОНУС: КОМБИНИРОВАНИЕ ВСЕХ ПАТТЕРНОВ\n");
            Console.WriteLine("Создаём инвентарь торговца с полной архитектурой:\n");

            var merchantInventory = new InventoryBuilder()
                .WithOwnerName("Торговец Гордан")
                .WithMaxWeight(150.0)
                .WithOrganizationStrategy(new SortByValueStrategy())
                .AddInitialItems(
                    new CommonItemFactory().CreateWeapon(),
                    new CommonItemFactory().CreateArmor(),
                    new RareItemFactory().CreateWeapon(),
                    new RareItemFactory().CreateArmor(),
                    new LegendaryItemFactory().CreateWeapon(),
                    new QuestItem("Магический кристалл", 0.5, ItemRarity.Rare, 1000, "Древний артефакт")
                )
                .Build();

            merchantInventory.DisplayItems();
            merchantInventory.ShowStatistics();

            // ИТОГИ
            Console.WriteLine("\n" + new string('═', 80));
            Console.WriteLine("ИТОГИ: Использованные паттерны");
            Console.WriteLine(new string('═', 80));

            Console.WriteLine(new string('═', 80));
            Console.WriteLine("  Нажмите Enter для завершения...");
            Console.ReadLine();
        }
    }
}
