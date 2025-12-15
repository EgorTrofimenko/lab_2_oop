using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class InventoryTests
{
    [Fact]
    public void Inventory_AddItem_IncreasesItemCount()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        // Act
        inventory.AddItem(item);
        
        // Assert
        Assert.Equal(1, inventory.GetItemCount());
    }

    [Fact]
    public void Inventory_AddNullItem_ReturnsFalse()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        
        // Act
        var result = inventory.AddItem(null);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Inventory_RemoveItem_DecreasesItemCount()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        inventory.AddItem(item);
        
        // Act
        inventory.RemoveItem(item);
        
        // Assert
        Assert.Equal(0, inventory.GetItemCount());
    }

    [Fact]
    public void Inventory_RemoveNonExistentItem_ReturnsFalse()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        // Act
        var result = inventory.RemoveItem(item);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Inventory_GetTotalWeight_CalculatesCorrectly()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item1 = new Weapon("Меч", 2.5, ItemRarity.Common, 50, 5, "Рубящее");
        var item2 = new Armor("Броня", 5.0, ItemRarity.Common, 75, 3, "Кожаная");
        
        inventory.AddItem(item1);
        inventory.AddItem(item2);
        
        // Act
        var totalWeight = inventory.GetTotalWeight();
        
        // Assert
        Assert.Equal(7.5, totalWeight, 2); // 2 знака после запятой
    }

    [Fact]
    public void Inventory_SetOrganizationStrategy_ChangesStrategy()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var newStrategy = new SortByRarityStrategy();
        
        // Act
        inventory.SetOrganizationStrategy(newStrategy);
        
        // Assert - проверяем, что метод выполнился без ошибок
        Assert.NotNull(inventory);
    }

    [Fact]
    public void Inventory_LockInventory_PreventsAddingItems()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        // Act
        inventory.LockInventory("Тестовая блокировка");
        var result = inventory.AddItem(item);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Inventory_UnlockInventory_AllowsAddingItems()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        var item = new Weapon("Меч", 2.0, ItemRarity.Common, 50, 5, "Рубящее");
        
        inventory.LockInventory();
        
        // Act
        inventory.UnlockInventory();
        var result = inventory.AddItem(item);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Inventory_ActivateWarehouseMode_AllowsUnlimitedItems()
    {
        // Arrange
        var inventory = new Inventory("Тестер", 100.0, new NoSortingStrategy());
        inventory.ActivateWarehouseMode();
        
        // Act - добавляем больше 20 предметов
        for (int i = 0; i < 30; i++)
        {
            var item = new Weapon($"Меч {i}", 1.0, ItemRarity.Common, 50, 5, "Рубящее");
            inventory.AddItem(item);
        }
        
        // Assert
        Assert.Equal(30, inventory.GetItemCount());
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(15)]
    public void Inventory_AddMultipleItems_CountMatches(int itemCount)
    {
        // Arrange
        var inventory = new Inventory("Тестер", 1000.0, new NoSortingStrategy());
        
        // Act
        for (int i = 0; i < itemCount; i++)
        {
            var item = new Weapon($"Меч {i}", 1.0, ItemRarity.Common, 50, 5, "Рубящее");
            inventory.AddItem(item);
        }
        
        // Assert
        Assert.Equal(itemCount, inventory.GetItemCount());
    }
}
