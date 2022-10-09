// See https://aka.ms/new-console-template for more information
using MTCG_TheOrigin;

Console.WriteLine("Hello, World!");


User neuerUser = new User();
neuerUser.Name = "fritzi";
neuerUser.SetPassword("ichBindasNeuePasswort");

neuerUser.DisplayInfoTest("ichBindasNeuePasswort");
//Console.WriteLine($"{neuerUser}" ); 