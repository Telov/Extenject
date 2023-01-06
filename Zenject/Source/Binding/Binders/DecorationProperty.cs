using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenject
{
    public class DecorationProperty<T> where T : class
    {
        public DecorationProperty()
        {
        }

        private readonly List<Func<T, T>> _decorators = new();

        private T _value;
        private bool _decorated = false;
        private T _finalValue;

        public void Decorate(Func<T, T> decorator)
        {
            _decorators.Add(decorator);
            _decorated = false;
        }

        public void Set(T value)
        {
            _value = value;
            _decorated = false;
        }
        
        public T FinalValue
        {
            get
            {
                if (_value == null) throw new Exception("No value to DecorationProperty");

                if (!_decorated)
                {
                    _finalValue = _value;
                    foreach (var funcDecorator in Enumerable.Reverse(_decorators))
                    {
                        _finalValue = funcDecorator(_finalValue);
                    }

                    _decorated = true;
                }

                return _finalValue;
            }
        }
    }
}