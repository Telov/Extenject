# Extenject
For documentation on Extenject itself, go to Extenject's GitHub page owned by Mathijs Bakker. Here I tell only about what I introduced myself for Extenject 9.2.1. Further I may refer to it as Zenject.

If you are Mathijs Bakker and want to pay some kind of attention (possibly want me to close this repo), please write something in zenject-extenject channel of InfallibleCode server in Discord, I often read it.

This extension for Zenject allows you to decorate your installers when creating all the objects yourself without relying on Zenject assembling objects instead of you. The decoration I introduced is well suited to be used together with Unity's prefab system and is created with prefab system in mind.

Changes to Zenject:
1. Added CompositeInstaller in MonoBehaviour and ScriptableObject variants, also they bind their inner installers to the used container.
2. In the default Zenject contexts "foreach" through all the installers, first injecting them and then calling InstallBindings() on them. In my variant of Zenject there are a few "foreach"es, first of which binds all the installers to the used container (just like the composite installer in my variant does), in second one they get .Inject-ed and then have their .DecorateProperties() called, in the third one they have their .InstallBindings() cllaed.
3. Added virtual void DecorateBinding() {} to all the installer types, removed throwing exception in the default implementation of .InstallBindings() of MonoInstallers.
4. Added DecorationProperty<T> which is the class you will decorate to extend behaviour of your installers.

NOTE: I don't use SOInstallers and PrefabInstallers and didn't add any support for them, if you want me to support them, create an issue or something.

Here's a code example of how to use DecorationProperties - https://gist.github.com/Telov/41be85314c1cc754654c464826495d41

Here's a visual example of how to use this code - https://github.com/Telov/Extenject/blob/main/My%20extenject%20picture%20guide.png

On how to configure your installers in prefab system-friendly way: instead of creating installers and referencing them directly from GameObjectContext create a CompositeMonoInstaller, put in it your original, INSTALLING installer, and only then reference the COMPOSITE installer from your GameObjectContext. DO NOT reference the INSTALLING or DECORATING installers from GameObjectContext or other Contexts. This warning is mostly important for working with DecorationProperty, if you think your installer will never use DecorationProperty you may choose to ignore the warning, but do it at your own risk. Every time you create a new prefab variant and want to add to it some DECORATING (extending) installers, you simply add it as the LAST installer in your composite installer for that type of behaviour (I use separate installers for Health, Movement and other modules). Also I prefer to have a separate GameObject for keeping installers, this GameObject has children GameObject which hold the CompositeMonoInstaller, and all the installers of the corresponding module.

WARNING! There is one important problem when using this entire approach with DecorationProperties. You get it when someone already used the FinalValue of your DecorationProperty and now someone else tries to change the DecorationProperty. Circular dependencies made with decorators are a special case of this problem. 
Example: your INSTALLING installer first instantiates InfoProvider and only after it instantiates InfoUser, then if you decorate the InfoProvider so that the decorator uses the InfoUser from the INSTALLING installer, you get an Exception with this text: "DecorationProperty changed after someone already used the final value!", because using .Set(T value) of the DecorationProperty in your INSTALLING installer will change the DecorationProperty and therefore invalidate all the values the DecorationProperty handed over before.
