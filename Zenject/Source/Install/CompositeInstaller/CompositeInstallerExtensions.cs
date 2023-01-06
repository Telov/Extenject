using System.Collections.Generic;
using System.Linq;

namespace Zenject.CompositeInstaller
{
    public static class CompositeInstallerExtensions
    {
        public static bool ValidateLeafInstaller<T>(
            this T leafInstaller,
            IReadOnlyList<ICompositeInstaller<T>> parentInstallers)
            where T : IInstaller
        {
            var compositeInstaller = leafInstaller as ICompositeInstaller<T>;
            if (compositeInstaller == null)
            {
                return true;
            }

            if (parentInstallers.Contains(compositeInstaller))
            {
                // Found a circular reference
                return false;
            }

            var childParentInstallers = new List<ICompositeInstaller<T>>(parentInstallers)
            {
                compositeInstaller
            };

            bool result = compositeInstaller
                .LeafInstallers
                .All(installer => installer.ValidateLeafInstaller(childParentInstallers));
            return result;
        }
    }
}