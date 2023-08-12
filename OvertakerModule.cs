using AssettoServer.Server.Plugin;
using Autofac;

namespace OvertakerPlugin;

public class OvertakerModule : AssettoServerModule<OvertakerConfiguration>
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Overtaker>().AsSelf().As<IAssettoServerAutostart>().SingleInstance();
    }
}
