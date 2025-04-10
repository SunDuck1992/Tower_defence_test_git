using System;

public class ReactValue<T>
{
    private T _value;

    public T Value 
    {
        get => _value;
        set
        {
            _value = value;
            ValueChanged?.Invoke();
        }
    }

    public event Action ValueChanged;
}
