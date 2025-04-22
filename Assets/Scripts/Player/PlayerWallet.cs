using System;
using YG;

public class PlayerWallet
{
    private int _gold;
    private int _gem;
    private int _startGold = 300;
    private int _startGem = 5;

    public PlayerWallet()
    {
        YandexGame.GetDataEvent += OnSetValue;

        OnSetValue();
    }

    ~PlayerWallet()
    {
        YandexGame.GetDataEvent -= OnSetValue;
    }

    public event Action<int> GoldChanged;
    public event Action<int> GemChanged;

    public int Gold => _gold;
    public int Gem => _gem;

    public void AddGold(int gold)
    {
        if (gold >= 0)
        {
            _gold += gold;
            GoldChanged?.Invoke(_gold);
        }
    }

    public void AddGem(int gem)
    {
        _gem += gem;
        GemChanged?.Invoke(_gem);
    }

    public bool TrySpendGold(int gold)
    {
        if (gold >= 0 & _gold - gold >= 0)
        {
            _gold -= gold;
            GoldChanged?.Invoke(_gold);

            return true;
        }
        return false;
    }

    public bool TrySpendGem(int gem)
    {
        if (gem > 0 & _gem - gem >= 0)
        {
            _gem -= gem;
            GemChanged?.Invoke(_gem);

            return true;
        }
        return false;
    }

    public void OnSaveWallet()
    {
        YandexGame.savesData.gold = _gold;
        YandexGame.savesData.gem = _gem;
    }

    private void OnSetValue()
    {
        if (YandexGame.savesData.gold == -1)
        {
            _gold = _startGold;
        }
        else
        {
            _gold = YandexGame.savesData.gold;
        }

        if (YandexGame.savesData.gem == -1)
        {
            _gem = _startGem;
        }
        else
        {
            _gem = YandexGame.savesData.gem;
        }
    }
}
