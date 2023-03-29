using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameSetupManager : MonoBehaviour
    {
        [SerializeField]
        private Settings.Settings _settings;
        
        [SerializeField]
        private ModuleScrollView _shipScrollViewPrefab;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Slot _shipSlot1;

        [SerializeField]
        private Slot _shipSlot2;

        [SerializeField]
        private ShipSetup _shipPrefab;

        [SerializeField]
        private Button _playButton;

        private ModuleScrollView _shipScrollView = null;
        private ShipSetup[] _ships;
        private Slot _currentSlot;

        private void Start()
        {
            _shipScrollView = Instantiate(_shipScrollViewPrefab, _canvas.transform);
            _shipScrollView.transform.position = new Vector3(Screen.currentResolution.width / 2, _shipScrollView.transform.position.y, _shipScrollView.transform.position.z);
            _shipScrollView.SetItems(_settings.ShipSettings, addRemoveItem: false);
            _shipScrollView.OnItemSelected += OnShipSelected;
            _shipScrollView.Hide();

            _ships = new ShipSetup[_settings.ShipSettings.Length];

            _playButton.gameObject.SetActive(false);
            _playButton.OnClicked += OnPlay;

            _shipSlot1.OnClicked += OnShipSlot1Clicked;
            _shipSlot2.OnClicked += OnShipSlot2Clicked;
        }

        private void Update()
        {
            
        }

        private void OnShipSelected(int id)
        {
            var ship = GameObject.Instantiate(_shipPrefab.gameObject, _currentSlot.transform);
            int slotId = 0;

            if (_currentSlot == _shipSlot1)
            {
                _currentSlot.OnClicked -= OnShipSlot1Clicked;
                slotId = 0;
            }
            else
            {
                _currentSlot.OnClicked -= OnShipSlot2Clicked;
                slotId = 1;
            }

            _ships[slotId] = ship.GetComponent<ShipSetup>();
            _ships[slotId].SetShipSettings(_settings.ShipSettings[id], _settings.WeaponSettings, _settings.ModuleSettings);
            _ships[slotId].OnShipReady += OnShipReady;

            _currentSlot.RemoveSprite();
            _currentSlot = null;

            _shipScrollView.Hide();
        }

        private void OnShipSlot1Clicked(int id)
        {
            _currentSlot = _shipSlot1;
            _shipScrollView.Show();
        }

        private void OnShipSlot2Clicked(int id)
        {
            _currentSlot = _shipSlot2;
            _shipScrollView.Show();
        }

        private void OnPlay()
        {
            _playButton.OnClicked -= OnPlay;

            PrepareBattleData();
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        private void OnShipReady(bool ready)
        {
            bool shipsReady = true;

            foreach (var ship in _ships)
            {
                if (ship == null || !ship.ShipReady)
                {
                    shipsReady = false;
                    break;
                }
            }

            _playButton.gameObject.SetActive(shipsReady);
        }

        private void PrepareBattleData()
        {
            BattleData.Ship1 = GetShipBattleData(_ships[0]);
            BattleData.Ship2 = GetShipBattleData(_ships[1]);
        }

        private BattleData.ShipBattleData GetShipBattleData(ShipSetup ship)
        {
            var shipSettings = ship.GetShipSettings();
            int shipSettingId = -1;
            var weapons = ship.GetSelectedWeapons();

            for (int i = 0; i < _settings.ShipSettings.Length; ++i)
            {
                if (_settings.ShipSettings[i] == shipSettings)
                {
                    shipSettingId = i;
                    break;
                }
            }

            var shipData = new BattleData.ShipBattleData()
            {
                ShipId = shipSettingId,
                Shield = ship.CurShield,
                Health = ship.CurHealth,
                ShieldRechargeRate = ship.CurShieldRechargeRate,
                RateOfFireMultiplier = ship.CurRateOfFire,
                WeaponIds = weapons
            };

            return shipData;
        }
    }
}
