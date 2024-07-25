using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.EnemyHealth
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Enemy.Logic.Enemy enemy;
        [SerializeField] private RectTransform healthBar;
        [FormerlySerializedAs("text")] [SerializeField] private TextMeshProUGUI textMesh;

        private void OnEnable()
        {
            enemy.Damaged += OnHealthChanged;
            enemy.Healed += OnHealthChanged;
            enemy.Died += OnDied;
        }

        private void OnDisable()
        {
            enemy.Damaged -= OnHealthChanged;
            enemy.Healed -= OnHealthChanged;
            enemy.Died -= OnDied;
        }

        private void OnHealthChanged() => ChangeHealthBar(enemy.HealthPercent);
        
        private void OnDied() => ChangeHealthBar(0f);

        private void ChangeHealthBar(float scale)
        {
            var newScale = healthBar.localScale;
            newScale.x = scale;
            healthBar.localScale = newScale;

            var percent = Mathf.RoundToInt(scale * 100f);
            textMesh.text = $"{percent}%";
        }
    }
}