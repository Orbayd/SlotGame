using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpykeGames.Showcase.Core;
using SpykeGames.Showcase.Core.Dealer;
using SpykeGames.Showcase.Core.Enums;
using UnityEngine;
using UnityEngine.TestTools;

public class SlotDealerTest
{
    private CardDeal[] _candidates;
    private Dictionary<SlotCombinationType, int[]> _results = new Dictionary<SlotCombinationType, int[]>();

    [SetUp]
    public void Setup()
    {
        var dealer = new CardDealer();
        dealer.CreateDeck();
        dealer.Shuffle();
        _candidates = DeckConfig.GetAll();
        _results = dealer.Hit()
                        .Select((x, i) => new { index = i, value = x })
                        .GroupBy(x => x.value, x => x.index)
                        .ToDictionary(x => x.Key, x => x.ToArray());
    }

    [Test]
    public void EmptySlotTest()
    {
        Assert.False(_results.ContainsKey(SlotCombinationType.None));
    }
    [Test]
    [TestCase(SlotCombinationType.Jackpot_Jackpot_Jackpot)]
    [TestCase(SlotCombinationType.Wild_Wild_Wild)]
    [TestCase(SlotCombinationType.Seven_Seven_Seven)]
    [TestCase(SlotCombinationType.Bonus_Bonus_Bonus)]
    [TestCase(SlotCombinationType.Ace_Ace_Ace)]
    [TestCase(SlotCombinationType.Bonus_A_Jackpot)]
    [TestCase(SlotCombinationType.Wild_Bonus_Ace)]
    [TestCase(SlotCombinationType.Jackpot_Jackpot_Ace)]
    [TestCase(SlotCombinationType.Wild_Wild_Seven)]
    [TestCase(SlotCombinationType.Ace_Wild_Bonus)]
    public void SlotCombinationTest(SlotCombinationType combination)
    {
        var jackpotConfig = _candidates.First(x => x.Id == combination);

        Assert.True(_results.ContainsKey(combination));

        var jackpotResults = _results[combination];
        foreach (var range in jackpotConfig.Ranges)
        {
            var rangeCount = jackpotResults.Count(index => range.Min <= index && index <= range.Max);
            Assert.AreEqual(rangeCount, 1);
        }

    }

    [Test]
    public void ShuffleTest()
    {
        var dealer = new CardDealer();
        dealer.CreateDeck();
        dealer.Shuffle();

        var r1 = dealer.Hit().ToArray();

        dealer.Shuffle();
        var r2 = dealer.Hit().ToArray();

        Assert.IsFalse(r2.SequenceEqual(r1));


    }

}
