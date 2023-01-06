namespace Zenject
{
    // We extract the interface so that monobehaviours can be installers
    public interface IInstaller
    {
        void DecorateProperties();
        void InstallBindings();

        bool IsEnabled
        {
            get;
        }
    }

}
