using System.Collections.Generic;

namespace Zenject.CompositeInstaller
{
    public interface ICompositeInstaller<out T> : IInstaller where T : IInstaller
    {
        IReadOnlyList<T> LeafInstallers { get; }
    }
}