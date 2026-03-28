printf("WARNING! The project file for the C# project is already included in the repository and this is not up to date!\n Continuing may result in unexpected errors!")

io.read()

project "Frontend"
   kind "WindowedApp"
   language "C#"
   files { "*.cs", "*.xaml", "*.xaml.cs" }
   links { "System", "WindowsBase", "PresentationCore", "PresentationFramework", "System.Xaml", "System.Data", "Backend" }

   targetdir ("../Binaries/" .. OutputDir .. "/Stealify")
   objdir ("../Binaries/Intermediates/" .. OutputDir .. "/Stealify")

   filter "configurations:Debug"
       defines { "DEBUG" }
       symbols "On"

   filter "configurations:Release"
       defines { "RELEASE" }
       optimize "On"
       symbols "On"

   filter "configurations:Dist"
       defines { "DIST" }
       optimize "On"
       symbols "Off"