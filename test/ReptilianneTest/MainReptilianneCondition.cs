using YdkTester;

namespace ReptilianneTest;

public class MainReptilianneCondition : ICondition
{
    public string Name => "Reptilianne Draw";

    public bool Check(Deck deck, CardSet hand)
    {
        if (hand.UseCard(Cards.SnakeRain))
            return true;

        if (hand.HasCard(Cards.SummonerMonk) &&
            hand.HasCard(c => c.Type.HasFlag(CardType.Spell)) &&
            !hand.HasCard(Cards.KeursetheOgdoadicLight))
            return true;

        return CheckCombos(deck, hand);
    }

    private bool CheckCombos(Deck deck, CardSet hand)
    {
        var usedRamifications = false;
        if (hand.UseCard(Cards.ReptilianneRamifications))
        {
            hand.AddCard(Cards.ReptilianneCoatl);
            usedRamifications = true;
        }

        if (hand.UseCard(Cards.NunutheOgdoadicRemnant))
        {
            if (hand.HasCard(c => c.Race == CardRace.Reptile) || hand.UseCard(Cards.FoolishBurial))
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

            if (deck.MainDeck.HasCard(Cards.KeursetheOgdoadicLight))
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
