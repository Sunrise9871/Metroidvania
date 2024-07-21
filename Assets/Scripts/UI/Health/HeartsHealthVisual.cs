using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Health
{
    public class HeartsHealthVisual : MonoBehaviour
    {
        [SerializeField] private Player.Logic.Player player;
        [SerializeField] private Image heartImage;
        [SerializeField] private Sprite[] heartSprites;

        private List<HeartImageLogic> _heartImageLogicList;
        private HeartsHealthSystem _heartsHealthSystem;

        private void Awake() => _heartImageLogicList = new List<HeartImageLogic>();

        private void OnEnable() => player.Damaged += OnPlayerDamaged;

        private void OnDisable() => player.Damaged -= OnPlayerDamaged;

        private void Start()
        {
            var heartAmount = player.Health / heartSprites.Length;
            if (player.Health % heartSprites.Length > 0)
                heartAmount++;

            _heartsHealthSystem = new HeartsHealthSystem(heartAmount, heartSprites.Length - 1);
            SetHeartsHealthSystem();
        }

        private void SetHeartsHealthSystem()
        {
            foreach (var heart in _heartsHealthSystem.HeartList)
                CreateHeartImage().SetHeartFragments(heart.Fragments);
        }

        private HeartImageLogic CreateHeartImage()
        {
            var spawnedHeart = Instantiate(heartImage, transform, false);
            
            var heartImageLogic = new HeartImageLogic(heartSprites, spawnedHeart);
            _heartImageLogicList.Add(heartImageLogic);

            return heartImageLogic;
        }
        
        private void OnPlayerDamaged(int damageAmount)
        {
            _heartsHealthSystem.Damage(damageAmount);
            
            var heartList = _heartsHealthSystem.HeartList;
            for (var i = 0; i < _heartImageLogicList.Count; i++)
            {
                var heartImageLogic = _heartImageLogicList[i];
                var heart = heartList[i];
                heartImageLogic.SetHeartFragments(heart.Fragments);
            }
        }

        private class HeartImageLogic
        {
            private readonly Sprite[] _heartSprites;
            private readonly Image _heartImage;

            public HeartImageLogic(Sprite[] heartSprites, Image heartImage)
            {
                _heartImage = heartImage;
                _heartSprites = heartSprites;
            }

            public void SetHeartFragments(int fragments) =>
                _heartImage.sprite = _heartSprites[fragments];
        }
    }
}