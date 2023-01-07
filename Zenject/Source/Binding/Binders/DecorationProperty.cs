using System;
using System.Collections.Generic;

namespace Zenject
{
    public class DecorationProperty<T> where T : class
    {
        private readonly List<Func<T, T>> _decorators = new();
        private T _value;
        private T _finalValue;

        public void Decorate(Func<T, T> decorator)
        {
            _decorators.Add(decorator);
            Decorated = false;
        }

        public void Set(T value)
        {
            _value = value;
            Decorated = false;
        }
        
        public T FinalValue
        {
            get
            {
                if (_value == null) throw new Exception("No value to DecorationProperty");

                if (!Decorated)
                {
                    _finalValue = _value;
                    foreach (var funcDecorator in _decorators)
                    {
                        _finalValue = funcDecorator(_finalValue);
                    }

                    Decorated = true;
                }

                return _finalValue;
            }
        }

        private bool _decorated;
        private bool Decorated
        {
            get => _decorated;
            set
            {
                if (Decorated && !value)
                {
                    throw new Exception("DecorationProperty changed after someone already used the final value!");
                }

                _decorated = value;
            }
        }
    }
}