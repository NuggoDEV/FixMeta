using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace FixMeta;

class Program
{
    private static string _keyPath = @"SOFTWARE\WOW6432Node\Oculus VR, LLC\Oculus\Config";
    private static string _valueName = "PreventDashLaunch";
    
    static void Main()
    {
        Console.WriteLine("Press \"1\" to prevent Dash from launching, or press \"2\" to restore old functionality");
        ConsoleKeyInfo key = Console.ReadKey();
        Console.WriteLine();

        // Don't run if we aren't using Windows
        if (!OperatingSystem.IsWindows())
        {
            Console.Error.WriteLine("This app can only be run on Windows!");
            Environment.Exit(0);
            return;
        }
        
        WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        // Verify that the app is indeed being run as an admin
        if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            Console.Error.WriteLine("This app needs to be run as an Administrator! " +
                                    "If you have an issue with this, please ask supports on how to set the registry key manually!");
            Environment.Exit(0);
        }


        using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(_keyPath, writable: true);

        if (registryKey is null)
        {
            Console.Error.WriteLine("Registry key not found! Do you have the Oculus App installed?");
            Console.Error.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }
        
        
        RegistrySecurity registrySecurity = registryKey.GetAccessControl();

        SecurityIdentifier identifier = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
        NTAccount account = identifier.Translate(typeof(NTAccount)) as NTAccount;
            
        RegistryAccessRule rule = new RegistryAccessRule(
            account.ToString(),
            RegistryRights.FullControl,
            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None,
            AccessControlType.Allow);
            
        registrySecurity.AddAccessRule(rule);
            
        registryKey.SetAccessControl(registrySecurity);
            
        registryKey.SetValue(_valueName, key.KeyChar == '1' ? 1 : 0, RegistryValueKind.DWord);
        Console.WriteLine($"Set {_valueName} to {key.KeyChar}");
            
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}