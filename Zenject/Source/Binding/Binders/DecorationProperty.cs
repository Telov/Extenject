using System;
using System.Collections.Generic;

namespace Zenject
{
    public class DecorationProperty<T> where T : class
    {
        public DecorationProperty()
        {
        }

        private readonly List<Func<T, T>> _decorators = new();
        private T _value;
        private T _finalValue;

        public void Decorate(Func<T, T> decorator)
        {
            _decorators.Add(decorator);
            decorated = false;
        }

        public void Set(T value)
        {
            _value = value;
            decorated = false;
        }
        
        public T FinalValue
        {
            get
            {
                if (_value == null) throw new Exception("No value to DecorationProperty");

                if (!decorated)
                {
                    _finalValue = _value;
                    foreach (var funcDecorator in _decorators)
                    {
                        _finalValue = funcDecorator(_finalValue);
                    }

                    decorated = true;
                }

                return _finalValue;
            }
        }

        private bool _decorated = false;
        private bool decorated
        {
            get => _decorated;
            set
            {
                if (decorated && !value)
                {
                    throw new Exception("DecorationProperty changed after someone already used the final value!");
                }

                _decorated = value;
            }
        }
    }
}