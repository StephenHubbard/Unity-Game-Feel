using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class MechanicsManager : Singleton<MechanicsManager>
{
    public bool PlayerControllerToggle => _playerControllerToggle.isOn;
    public bool BetterGunToggle => _betterGunToggle.isOn;
    public bool VFXToggle => _vfxToggle.isOn;
    public bool ScreenShakeToggle => _screenShakeToggle.isOn;
    public bool HitFeedbackToggle => _hitFeedbackToggle.isOn;
    public bool PostProcessingToggle => _postProcessingToggle.isOn;
    public bool PlayerAnimationsToggle => _playerAnimationsToggle.isOn;
    public bool ObjectPoolingToggle => _objectPoolingToggle.isOn;
    public bool SFXToggle => _sfxToggle.isOn;
    public bool DeathFadeToggle => _deathFadeToggle.isOn;

    [SerializeField] private Toggle _playerControllerToggle;
    [SerializeField] private Toggle _betterGunToggle;
    [SerializeField] private Toggle _vfxToggle;
    [SerializeField] private Toggle _screenShakeToggle;
    [SerializeField] private Toggle _hitFeedbackToggle;
    [SerializeField] private Toggle _postProcessingToggle;
    [SerializeField] private Toggle _playerAnimationsToggle;
    [SerializeField] private Toggle _objectPoolingToggle;
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Toggle _deathFadeToggle;

    private BasicPlayerController _basicPlayerController;
    private PlayerController _playerController;
    private Volume _globalVolume;

    private BasicGun _basicGun;
    private Gun _gun;

    protected override void Awake() {
        base.Awake();
        
        _globalVolume = FindObjectOfType<Volume>();
    }

    private void Start() {
        SetToggles();

        if (_postProcessingToggle.isOn)
        {
            PostProccessing();
        }
    }

    public void SetToggles() {
        _basicPlayerController = FindObjectOfType<BasicPlayerController>();
        _playerController = FindObjectOfType<PlayerController>();
        _basicGun = FindObjectOfType<BasicGun>();
        _gun = FindObjectOfType<Gun>();

        if (_playerControllerToggle.isOn) {
            ImprovedPlayerController();
        }

        if (_betterGunToggle.isOn) {
            BetterGun();
        }
    }

    public void ImprovedPlayerController() {
        _basicPlayerController.enabled = !_basicPlayerController.enabled;
        _playerController.enabled = !_playerController.enabled;
    }

    public void BetterGun() {
        _basicGun.enabled = !_basicGun.enabled;
        _gun.enabled = !_gun.enabled;
    }

    public void PostProccessing() {
        _globalVolume.enabled = !_globalVolume.enabled;
    }


}
