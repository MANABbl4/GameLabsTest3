using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BattleShip : MonoBehaviour
    {
        private class Weapon
        {
            public float Damage;
            public float RateOfFire;
            public float TimeLeft;
            public LaserBeam LaserBeam;
        }

        public Action<BattleShip, float> OnFire;
        public Action<BattleShip> OnDestoyed;

        public float CurHealth { get; private set; }
        public float CurShield { get; private set; }


        [SerializeField]
        private Image _shipImage;

        [SerializeField]
        private Image _shieldImage;

        [SerializeField]
        private ProgressBar _hp;

        [SerializeField]
        private ProgressBar _shield;

        [SerializeField]
        private LaserBeam _laserBeamPrefab;

        private float _shieldRechargeRate;
        private float _maxShield = 0f;
        private Weapon[] _weapons = null;
        private bool _firing = true;

        public void SetBattleData(BattleData.ShipBattleData battleData, Settings.Settings settings)
        {
            CurHealth = battleData.Health;
            CurShield = battleData.Shield;
            _maxShield = battleData.Shield;
            _shieldRechargeRate = battleData.ShieldRechargeRate;

            _shipImage.sprite = settings.ShipSettings[battleData.ShipId].Sprite;
            _shieldImage.sprite = settings.ShipSettings[battleData.ShipId].Shield?.Sprite;

            _hp.SetMaxValue(battleData.Health);
            _hp.SetCurValue(battleData.Health);

            _shield.SetMaxValue(battleData.Shield);
            _shield.SetCurValue(battleData.Shield);

            _weapons = new Weapon[battleData.WeaponIds.Length];

            for (int i = 0; i < battleData.WeaponIds.Length; ++i)
            {
                var weaponSettings = settings.WeaponSettings[battleData.WeaponIds[i]];
                _weapons[i] = new Weapon()
                {
                    Damage = weaponSettings.Damage,
                    RateOfFire = weaponSettings.RateOfFire / battleData.RateOfFireMultiplier,
                    TimeLeft = weaponSettings.RateOfFire / battleData.RateOfFireMultiplier,
                    LaserBeam = Instantiate(_laserBeamPrefab, _shipImage.transform)
                };

                _weapons[i].LaserBeam.SetSprite(weaponSettings.LaserSprite);
                var rectTransform = _shipImage.GetComponent<RectTransform>();
                var rotatedPos = Quaternion.AngleAxis(rectTransform.localEulerAngles.z, Vector3.forward) * (rectTransform.localPosition + (Vector3)settings.ShipSettings[battleData.ShipId].WeaponSlots[i].Position);
                _weapons[i].LaserBeam.transform.position += rotatedPos;
            }
        }

        public void StopFiring()
        {
            _firing = false;
        }

        public void StartFiring()
        {
            _firing = true;
        }

        public void TakeDamage(float damage)
        {
            CurShield -= damage;

            if (CurShield < 0)
            {
                CurHealth += CurShield;
                CurShield = 0;

                if (CurHealth < 0)
                {
                    CurHealth = 0;
                    OnDestoyed?.Invoke(this);
                }
            }

            _hp.SetCurValue(CurHealth);
            _shield.SetCurValue(CurShield);
        }

        private void Update()
        {
            if (CurShield < _maxShield)
            {
                CurShield += _shieldRechargeRate * Time.deltaTime;

                if (CurShield > _maxShield)
                {
                    CurShield = _maxShield;
                }

                _shield.SetCurValue(CurShield);
            }

            if (_firing)
            {
                foreach (var weapon in _weapons)
                {
                    weapon.TimeLeft -= Time.deltaTime;

                    if (weapon.TimeLeft < 0)
                    {
                        weapon.TimeLeft += weapon.RateOfFire;
                        weapon.LaserBeam.Show();

                        OnFire?.Invoke(this, weapon.Damage);
                    }
                }
            }
        }
    }
}
