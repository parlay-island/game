//place this script in the Editor folder within Assets.
using UnityEditor;


//to be used on the command line:
//$ Unity -quit -batchmode -executeMethod WebGLBuilder.build

class WebGLBuilder {
    static void build() {
        string[] scenes = {"Assets/Scenes/StartScreen.unity", "Assets/Scenes/LoginScreen.unity", "Assets/Scenes/SignUpScreen.unity", "Assets/Scenes/ModeSelection.unity", "Assets/Scenes/Development.unity", "Assets/Scenes/RulesScreen.unity", "Assets/Scenes/RulesScreen2.unity"};
        BuildPipeline.BuildPlayer(scenes, "WebGL-Dist", BuildTarget.WebGL, BuildOptions.None);
    }
}
