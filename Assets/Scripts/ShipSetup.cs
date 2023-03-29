using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ShipSetup : MonoBehaviour
    {
        public Action<bool> OnShipReady;
        public bool ShipReady { get; private set; }

        [SerializeField]
        private Image _image;

        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _description;

        [SerializeField]
        private Text _hp;

        [SerializeField]
        private Text _shield;

        [SerializeField]
        private Slot _slotPrefab;

        [SerializeField]
        private ModuleScrollView _modulesScrollViewPrefab;

        [SerializeField]
        private Canvas _canvas;

        private ShipSettings _shipSettings = null;
        private WeaponSettings[] _weaponSettings = null;
        private ModuleSettings[] _moduleSettings = null;

        private ModuleScrollView _weaponScrollView = null;
        private ModuleScrollView _moduleScrollView = null;

        private List<Slot> _slots = new List<Slot>();
        private int _lastClickedSlot = -1;

        private int[] _selectedWeapons = null;
        private int[] _selectedModules = null;

        public float CurHealth { get; private set; } = 0;
        public float CurShield { get; private set; } = 0;
        public float CurShieldRechargeRate { get; private set; } = 1;
        public float CurRateOfFire { get; private set; } = 1;

        public void SetShipSettings(ShipSettings shipSettings, WeaponSettings[] weaponSettings, ModuleSettings[] moduleSettings)
        {
            _shipSettings = shipSettings;
            _weaponSettings = weaponSettings;
            _moduleSettings = moduleSettings;

            _image.sprite = _shipSettings.Sprite;
            _name.text = _shipSettings.Name;
            _description.text = _shipSettings.Description;
            _hp.text = _shipSettings.Health.ToString();
            _shield.text = _shipSettings.Shield?.Shield.ToString();

            CurHealth = _shipSettings.Health;
            CurShield = (float)_shipSettings.Shield?.Shield;
            CurShieldRechargeRate = (float)_shipSettings.Shield?.ShieldRechargeRate;

            foreach (var slot in _slots)
            {
                slot.OnClicked -= OnSlotClicked;
                Destroy(slot.gameObject);
            }

            _slots.Clear();

            _selectedWeapons = new int[shipSettings.WeaponSlots.Length];
            _selectedModules = new int[shipSettings.EngineeringModuleSlots.Length];

            for (int i = 0; i < shipSettings.WeaponSlots.Length; ++i)
            {
                var weapon = shipSettings.WeaponSlots[i];
                var slot = CreateSlot(_slotPrefab, weapon.Size, weapon.Position, _slots.Count);
                _slots.Add(slot);

                _selectedWeapons[i] = -1;
            }

            for (int i = 0; i < shipSettings.EngineeringModuleSlots.Length; ++i)
            {
                var module = shipSettings.EngineeringModuleSlots[i];
                var slot = CreateSlot(_slotPrefab, module.Size, module.Position, _slots.Count);
                _slots.Add(slot);

                _selectedModules[i] = -1;
            }


            if (_weaponScrollView == null)
            {
                _weaponScrollView = Instantiate(_modulesScrollViewPrefab, _canvas.transform);
                _weaponScrollView.transform.position = new Vector3(Screen.currentResolution.width / 2, _weaponScrollView.transform.position.y, _weaponScrollView.transform.position.z);
                _weaponScrollView.OnItemSelected += OnWeaponSelected;
            }

            if (_moduleScrollView == null)
            {
                _moduleScrollView = Instantiate(_modulesScrollViewPrefab, _canvas.transform);
                _moduleScrollView.transform.position = new Vector3(Screen.currentResolution.width / 2, _moduleScrollView.transform.position.y, _moduleScrollView.transform.position.z);
                _moduleScrollView.OnItemSelected += OnModuleSelected;
            }

            _weaponScrollView.SetItems(weaponSettings, addRemoveItem: true);
            _moduleScrollView.SetItems(moduleSettings, addRemoveItem: true);

            _weaponScrollView.Hide();
            _moduleScrollView.Hide();
        }

        public int[] GetSelectedWeapons()
        {
            return _selectedWeapons;
        }

        public ShipSettings GetShipSettings()
        {
            return _shipSettings;
        }

        private void OnWeaponSelected(int id)
        {
            _weaponScrollView.Hide();

            int weaponSlotId = _lastClickedSlot;

            if (id < 0)
            {
                // remove slot item
                _selectedWeapons[weaponSlotId] = -1;
                _slots[_lastClickedSlot].RemoveSprite();
            }
            else
            {
                // set and recalculate params
                _selectedWeapons[weaponSlotId] = id;
                _slots[_lastClickedSlot].SetSprite(_weaponSettings[id].Sprite);
            }

            CheckShipReady();
        }

        private void OnModuleSelected(int id)
        {
            _moduleScrollView.Hide();

            int moduleSlotId = _lastClickedSlot - _shipSettings.WeaponSlots.Length;

            if (id < 0)
            {
                // remove slot item
                _selectedModules[moduleSlotId] = -1;
                _slots[_lastClickedSlot].RemoveSprite();
            }
            else
            {
                // set and recalculate params
                _selectedModules[moduleSlotId] = id;
                _slots[_lastClickedSlot].SetSprite(_moduleSettings[id].Sprite);
            }

            CurHealth = _shipSettings.Health + GetModulesSumValue(ParameterType.Health);
            CurShield = (float)_shipSettings.Shield?.Shield + GetModulesSumValue(ParameterType.Shield);
            CurShieldRechargeRate = (float)_shipSettings.Shield?.ShieldRechargeRate * GetModulesMultipliedValue(ParameterType.ShieldRegenMultiplier);
            CurRateOfFire = GetModulesMultipliedValue(ParameterType.RateOfFireMultiplier);

            _hp.text = CurHealth.ToString();
            _shield.text = CurShield.ToString();


            CheckShipReady();
        }

        private Slot CreateSlot(Slot slotPrefab, Vector2 size, Vector2 position, int id)
        {
            var slot = Instantiate(slotPrefab, transform);
            var rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            slot.transform.position = slot.transform.position + (Vector3)position;
            slot.SetId(id);
            slot.OnClicked += OnSlotClicked;

            return slot;
        }

        private void OnSlotClicked(int id)
        {
            _lastClickedSlot = id;

            if (id < _shipSettings.WeaponSlots.Length)
            {
                // weapon slot
                _weaponScrollView.Show();
            }
            else
            {
                // module slot
                _moduleScrollView.Show();
            }
        }

        private float GetModulesSumValue(ParameterType type)
        {
            var curValue = 0f;

            foreach (var moduleId in _selectedModules)
            {
                if (moduleId >= 0)
                {
                    var module = _moduleSettings[moduleId];
                    curValue += GetModuleSumValue(module, type);
                }
            }

            return curValue;
        }

        private float GetModulesMultipliedValue(ParameterType type)
        {
            var curValue = 1f;

            foreach (var moduleId in _selectedModules)
            {
                if (moduleId >= 0)
                {
                    var module = _moduleSettings[moduleId];
                    curValue *= GetModuleMultipliedValue(module, type);
                }
            }

            return curValue;
        }

        private float GetModuleSumValue(ModuleSettings module, ParameterType type)
        {
            float resultValue = 0f;

            for (int i = 0; i < module.Parameters.Length; i++)
            {
                if (module.Parameters[i].Type == type)
                {
                    resultValue += module.Parameters[i].Value;
                }
            }

            return resultValue;
        }

        private float GetModuleMultipliedValue(ModuleSettings module, ParameterType type)
        {
            float resultValue = 1f;

            for (int i = 0; i < module.Parameters.Length; i++)
            {
                if (module.Parameters[i].Type == type)
                {
                    resultValue *= module.Parameters[i].Value;
                }
            }

            return resultValue;
        }

        private void CheckShipReady()
        {
            bool shipReady = true;

            foreach (var moduleId in _selectedModules)
            {
                if (moduleId < 0)
                {
                    shipReady = false;
                    break;
                }
            }

            foreach (var weaponId in _selectedWeapons)
            {
                if (weaponId < 0)
                {
                    shipReady = false;
                    break;
                }
            }

            if (shipReady != ShipReady)
            {
                ShipReady = shipReady;
                OnShipReady?.Invoke(ShipReady);
            }
        }
    }
}
