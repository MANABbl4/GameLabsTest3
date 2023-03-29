using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField]
        private Settings.Settings _settings;

        [SerializeField]
        private BattleShip _ship1;

        [SerializeField]
        private BattleShip _ship2;

        [SerializeField]
        private Text _battleResult;

        [SerializeField]
        private Button _goMain;

        private const string _winMessage = "{0} WIN!";
        private const string _ship1Name = "Left ship";
        private const string _ship2Name = "Right ship";

        private void Start()
        {
            _ship1.OnFire += OnFire1;
            _ship1.OnDestoyed += OnDestroyed;
            _ship2.OnFire += OnFire2;
            _ship1.OnDestoyed += OnDestroyed;

            _ship1.SetBattleData(BattleData.Ship1, _settings);
            _ship2.SetBattleData(BattleData.Ship2, _settings);

            _battleResult.gameObject.SetActive(false);
            _goMain.gameObject.SetActive(false);
            _goMain.OnClicked += OnGoMainScene;

            _ship1.StartFiring();
            _ship2.StartFiring();
        }

        private void Update()
        {

        }

        private void OnFire1(BattleShip ship, float damage)
        {
            _ship2.TakeDamage(damage);
        }

        private void OnFire2(BattleShip ship, float damage)
        {
            _ship1.TakeDamage(damage);
        }

        private void OnDestroyed(BattleShip ship)
        {
            _ship1.StopFiring();
            _ship2.StopFiring();

            _ship1.OnFire -= OnFire1;
            _ship2.OnFire -= OnFire2;
            _ship1.OnDestoyed -= OnDestroyed;
            _ship2.OnDestoyed -= OnDestroyed;

            ship.gameObject.SetActive(false);

            string shipName = _ship1Name;

            if (ship == _ship1)
            {
                shipName = _ship2Name;
            }

            _battleResult.text = string.Format(_winMessage, shipName);
            _battleResult.gameObject.SetActive(true);
            _goMain.gameObject.SetActive(true);
        }

        private void OnGoMainScene()
        {
            _goMain.OnClicked -= OnGoMainScene;

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}