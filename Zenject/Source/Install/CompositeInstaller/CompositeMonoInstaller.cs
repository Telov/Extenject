using System.Collections.Generic;
using UnityEngine;

namespace Zenject.CompositeInstaller
{
    public class CompositeMonoInstaller : MonoInstaller<CompositeMonoInstaller>, ICompositeInstaller<MonoInstallerBase>
    {
        [SerializeField]
        List<MonoInstallerBase> _leafInstallers = new List<MonoInstallerBase>();
        public IReadOnlyList<MonoInstallerBase> LeafInstallers => _leafInstallers;

        public override void DecorateProperties()
        {
            foreach (var installer in _leafInstallers)
            {
                Container.Bind(installer.GetType()).FromInstance(installer);
            }
            
            foreach (var installer in _leafInstallers)
            {
                Container.Inject(installer);

#if ZEN_INTERNAL_PROFILING
                using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                {
                    installer.DecorateProperties();
                }
            }
        }
        
        public override void InstallBindings()
        {
            foreach (var installer in _leafInstallers)
            {
#if ZEN_INTERNAL_PROFILING
                using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
                {
                    installer.InstallBindings();
                }
            }
        }
    }
}