using YdkTester;

namespace ReptilianneTest;

public class ReptilianneDrawCondition : ICondition
{
    public string Name => "Reptilianne Draw";

    private static CardAction _nonNunuReptileMonster = new CardAction(c => c.Type == CardType.Reptile && c != Cards.NunutheOgdoadicRemnant);
    private static CardAction _monsterCard = new CardAction(c => c.Category.HasFlag(CardCategory.Monster));
    private static CardAction _spellCard = new CardAction(c => c.Category.HasFlag(CardCategory.Spell));

    public bool Check(CardSet deck, CardSet hand)
    {
        if (hand.UseCard(Cards.SnakeRain))
            return true;

        if (hand.HasCard(Cards.NunutheOgdoadicRemnant) || (hand.HasCard(Cards.SmallWorld) && hand.HasCard(_monsterCard)))
        {
            if (hand.HasCard(Cards.ReptilianneRamifications) ||
                hand.HasCard(Cards.FoolishBurial) ||
                hand.HasCard(_nonNunuReptileMonster))
            {
                return true;
            }
        }

        if (hand.HasCard(Cards.SummonerMonk) &&
            !hand.HasCard(Cards.KeursetheOgdoadicLight) &&
            deck.HasCard(Cards.NightSwordSerpent) &&
            hand.HasCard(_spellCard))
            return true;

        return CheckCombos(deck, hand);
    }

    private bool CheckCombos(CardSet deck, CardSet hand)
    {
        var usedRamifications = false;
        if (hand.UseCard(Cards.ReptilianneRamifications))
        {
            hand.AddCard(Cards.ReptilianneCoatl);
            usedRamifications = true;
        }

        if (hand.UseCard(Cards.NunutheOgdoadicRemnant))
        {
            if (hand.HasCard(Cards.ReptilianneRamifications) ||
                hand.HasCard(Cards.FoolishBurial) ||
                hand.HasCard(_nonNunuReptileMonster))
            {
                return true;
            }
            else
            {
                hand.AddCard(Cards.NunutheOgdoadicRemnant);
            }
        }

        // 3 card combo

        // Lvl 4 reptile + Random reptile + Ramification + Foolish = combo :O
        // TODO: Check nunu stuff

        var keurseInGY = false;
        var nauyaInGY = false;
        var ssMonster = hand.UseCard(Cards.InstantFusion) || hand.UseCard(Cards.NunutheOgdoadicRemnant);
        var coatlInHand = hand.HasCard(Cards.ReptilianneCoatl);

        if (hand.UseCard(Cards.NauyatheOgdoadicRemnant)) // Use Nauya
        {
            nauyaInGY = true;

            if (deck.HasCard(Cards.KeursetheOgdoadicLight))
            {
                keurseInGY = true;
            }
        }

        if (usedRamifications)
        {
            if (hand.UseCard(Cards.KeursetheOgdoadicLight))
            {
                keurseInGY = true;
            }
        }

        if (hand.UseCard(Cards.FoolishBurial) || hand.UseCard(Cards.OgdoadicWaterLily)) // Foolish burial is a wildcard, can be substituted for any missing card of this combo
        {
            if (!ssMonster)
            {
                ssMonster = true;
            }
            else if (!nauyaInGY)
            {
                nauyaInGY = true;
            }
            else if (!keurseInGY)
            {
                keurseInGY = true;
            }
        }

        return keurseInGY && nauyaInGY && coatlInHand && ssMonster;
    }
}
