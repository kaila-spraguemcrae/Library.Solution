// using System;
// using System.Security.Principal;

// class GenericPrincipalMembers
// {
//     [STAThread]
//     static void Main(string[] args)
//     {
//         // Retrieve a GenericPrincipal that is based on the current user's
//         // WindowsIdentity.
//         GenericPrincipal genericPrincipal = GetGenericPrincipal();

//         // Retrieve the generic identity of the GenericPrincipal object.
//         GenericIdentity principalIdentity = 
//             (GenericIdentity)genericPrincipal.Identity;

//         // Display the identity name and authentication type.
//         if (principalIdentity.IsAuthenticated)
//         {
//             Console.WriteLine(principalIdentity.Name);
//             Console.WriteLine("Type:"+principalIdentity.AuthenticationType);
//         }

//         // Verify that the generic principal has been assigned the
//         // NetworkUser role.
//         if (genericPrincipal.IsInRole("Librarian"))
//         {
//             Console.WriteLine("User belongs to the Librarian role.");
//         }

//         Console.WriteLine("The sample completed successfully; " +
//             "press Enter to continue.");
//         Console.ReadLine();
//     }

//     // Create a generic principal based on values from the current
//     // WindowsIdentity.
//     private static GenericPrincipal GetGenericPrincipal()
//     {
//         // Use values from the current WindowsIdentity to construct
//         // a set of GenericPrincipal roles.
//         WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
//         string[] roles = new string[10];
//         if (windowsIdentity.IsAuthenticated)
//         {
//             // Add custom NetworkUser role.
//             roles[0] = "Librarian";
//         }

//         if (windowsIdentity.IsGuest)
//         {
//             // Add custom GuestUser role.
//             roles[1] = "GuestUser";
//         }

//         if (windowsIdentity.IsSystem)
//         {
//             // Add custom SystemUser role.
//             roles[2] = "Patron";
//         }

//         // Construct a GenericIdentity object based on the current Windows
//         // identity name and authentication type.
//         string authenticationType = windowsIdentity.AuthenticationType;
//         string userName = windowsIdentity.Name;
//         GenericIdentity genericIdentity =
//             new GenericIdentity(userName, authenticationType);

//         // Construct a GenericPrincipal object based on the generic identity
//         // and custom roles for the user.
//         GenericPrincipal genericPrincipal =
//             new GenericPrincipal(genericIdentity, roles);

//         return genericPrincipal;
//     }
// }
