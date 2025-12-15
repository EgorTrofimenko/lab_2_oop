using System;
using System.Collections.Generic;
using Xunit;
using InventorySystem;namespace ExistingProject.Tests;

public class ItemFactoryTests
{
    [Fact]
    public void CommonFactory_CreatesCommonWeapon()
    {
        // Arrange
        var factory = new CommonItemFactory();
        
        // Act
        var weapon = factory.CreateWeapon();
        
        // Assert
        Assert.NotNull(weapon);
        Assert.Equal(ItemRarity.Common, weapon.Rarity);
        Assert.True(weapon.Damage > 0);
    }

    [Fact]
    public void RareFactory_CreatesRareArmor()
    {
        // Arrange
        var factory = new RareItemFactory();
        
        // Act
        var armor = factory.CreateArmor();
        
        // Assert
        Assert.NotNull(armor);
        Assert.Equal(ItemRarity.Rare, armor.Rarity);
        Assert.True(armor.Defense > 0);
    }

    [Fact]
    public void LegendaryFactory_CreatesLegendaryConsumable()
    {
        // Arrange
        var factory = new LegendaryItemFactory();
        
        // Act
        var consumable = factory.CreateConsumable();
        
        // Assert
        Assert.NotNull(consumable);
        Assert.Equal(ItemRarity.Legendary, consumable.Rarity);
    }

    [Theory]
    [InlineData(ItemRarity.Common)]
    [InlineData(ItemRarity.Rare)]
    [InlineData(ItemRarity.Legendary)]
    public void FactoryProvider_ReturnsCorrectFactory(ItemRarity rarity)
    {
        // Act
        var factory = ItemFactoryProvider.GetFactory(rarity);
        
        // Assert
        Assert.NotNull(factory);
    }
}
