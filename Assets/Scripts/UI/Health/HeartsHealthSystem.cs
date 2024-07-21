using System.Collections.Generic;

namespace UI.Health
{
    public class HeartsHealthSystem
    {
        public List<Heart> HeartList { get; }

        public HeartsHealthSystem(int heartAmount, int fragments)
        {
            HeartList = new List<Heart>();
            for (var i = 0; i < heartAmount; i++)
            {
                var heart = new Heart(fragments);
                HeartList.Add(heart);
            }
        }

        public void Damage(int damageAmount)
        {
            for (var i = HeartList.Count - 1; i >= 0; i--)
            {
                var heart = HeartList[i];
                if (damageAmount > heart.Fragments)
                {
                    damageAmount -= heart.Fragments;
                    heart.Damage(heart.Fragments);
                }
                else
                {
                    heart.Damage(damageAmount);
                    return;
                }
            }
        }

        public class Heart
        {
            public int Fragments { get; private set; }

            public Heart(int fragments) => Fragments = fragments;

            public void Damage(int damageAmount) =>
                Fragments = damageAmount > Fragments ? 0 : Fragments -= damageAmount;
        }
    }
}