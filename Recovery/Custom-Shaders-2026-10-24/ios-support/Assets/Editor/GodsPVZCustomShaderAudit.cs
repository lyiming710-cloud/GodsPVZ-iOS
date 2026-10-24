#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public static class GodsPVZCustomShaderAudit
{
    private const string AwaitActive = "GodsPVZ.CustomShaderAwait.Active";
    private const string AwaitStarted = "GodsPVZ.CustomShaderAwait.Started";
    private static readonly List<string> StartupErrors = new List<string>();
    private static readonly List<string> StartupAudioLimits = new List<string>();
    public static void BeginAfterStartup()
    {
        if (SessionState.GetBool(AwaitActive,false)) throw new InvalidOperationException("Custom shader startup probe already active");
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity",OpenSceneMode.Single);
        SessionState.SetBool(AwaitActive,true);
        SessionState.SetString(AwaitStarted,DateTime.UtcNow.Ticks.ToString());
        AttachAwait();
        EditorApplication.isPlaying=true;
    }
    [InitializeOnLoadMethod]
    private static void ResumeAwait()
    {
        if (SessionState.GetBool(AwaitActive,false)) AttachAwait();
    }
    private static void AttachAwait()
    {
        EditorApplication.update-=AwaitTick;EditorApplication.update+=AwaitTick;
        Application.logMessageReceived-=StartupLog;Application.logMessageReceived+=StartupLog;
    }
    private static void StartupLog(string message,string stack,LogType type)
    {
        if (!(type==LogType.Exception || type==LogType.Error || type==LogType.Assert)) return;
        if (message.StartsWith("FMOD failed to initialize the output device",StringComparison.Ordinal))
        {
            if (StartupAudioLimits.Count<10) StartupAudioLimits.Add(message);
        }
        else if (StartupErrors.Count<50) StartupErrors.Add(message+"\n"+stack);
    }
    private static void AwaitTick()
    {
        double elapsed=(DateTime.UtcNow.Ticks-Int64.Parse(SessionState.GetString(AwaitStarted,"0")))/(double)TimeSpan.TicksPerSecond;
        if (elapsed<20) return;
        if ((!EditorApplication.isPlaying || Time.frameCount<3) && elapsed<90) return;
        SessionState.SetBool(AwaitActive,false);
        EditorApplication.update-=AwaitTick;
        // Close the normal startup observation before intentional shader compiler
        // controls. Those controls have their own explicit outcome below.
        Application.logMessageReceived-=StartupLog;
        int exit=1;
        try { Run();exit=0; }
        catch (Exception error) { Debug.LogException(error); }
        EditorApplication.Exit(exit);
    }
    [Serializable] public class CompileRow
    {
        public string shader, platform, stage, keywords, sha256;
        public int pass, bytes;
        public bool success;
        public List<string> messages = new List<string>();
    }
    [Serializable] public class PixelRow
    {
        public string name;
        public Color expected, actual;
        public float max_error;
        public bool pass;
    }
    [Serializable] public class Report
    {
        public string scope = "REAL_UNITY_CUSTOM_SHADER_CROSS_COMPILATION_AND_CONTROLLED_OPENGL_RENDER";
        public bool pass, metal_device_execution = false, game_pixel_equivalence = false, negative_compile_control;
        public string graphics_api, renderer, color_space, pipeline_asset_type, pipeline_asset_guid, pipeline_instance_type, render_pipeline_tag;
        public int normal_startup_frames;
        public List<CompileRow> compilations = new List<CompileRow>();
        public List<PixelRow> pixels = new List<PixelRow>();
        public List<string> errors = new List<string>();
    }
    private const string GamePath = "Assets/Shader/Unlit_GameUnitShader.shader";
    private const string BrightPath = "Assets/Shader/Shader Graphs_Brightness.shader";

    private static string BinaryPath(string name)
    {
        string directory = Environment.GetEnvironmentVariable("GODSPVZ_SHADER_BINARY_EVIDENCE");
        if (String.IsNullOrEmpty(directory)) throw new InvalidOperationException("Separate binary evidence directory required");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, name);
    }

    public static void Run()
    {
        string folder = Environment.GetEnvironmentVariable("GODSPVZ_STARTUP_EVIDENCE");
        if (String.IsNullOrEmpty(folder)) throw new InvalidOperationException("Explicit evidence directory required");
        Directory.CreateDirectory(folder);
        var report = new Report { graphics_api = SystemInfo.graphicsDeviceType.ToString(), renderer = SystemInfo.graphicsDeviceName,
            color_space=QualitySettings.activeColorSpace.ToString(),normal_startup_frames=Time.frameCount,
            pipeline_asset_type=GraphicsSettings.currentRenderPipeline==null ? "null" : GraphicsSettings.currentRenderPipeline.GetType().FullName,
            pipeline_asset_guid=GraphicsSettings.currentRenderPipeline==null ? "" : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(GraphicsSettings.currentRenderPipeline)),
            pipeline_instance_type=RenderPipelineManager.currentPipeline==null ? "null" : RenderPipelineManager.currentPipeline.GetType().FullName,
            render_pipeline_tag=Shader.globalRenderPipeline };
        try
        {
            report.errors.AddRange(StartupErrors);
            if (!EditorApplication.isPlaying || Time.frameCount<3 || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="MainMenu")
                throw new InvalidOperationException("Require normally running MainMenu before shader rendering");
            if (report.pipeline_asset_guid!="22c2bf3291e4d8f4db5d66a87a6f8123" || RenderPipelineManager.currentPipeline==null || report.render_pipeline_tag!="UniversalPipeline")
                throw new InvalidOperationException("Original configured URP must start normally; probe may not substitute a pipeline or force a shader tag");
            Shader game = AssetDatabase.LoadAssetAtPath<Shader>(GamePath);
            Shader bright = AssetDatabase.LoadAssetAtPath<Shader>(BrightPath);
            if (game == null || game.name != "Unlit/GameUnitShader" || bright == null || bright.name != "Shader Graphs/Brightness")
                throw new InvalidOperationException("Exact two custom shaders must import");
            if (AssetDatabase.AssetPathToGUID(GamePath) != "dce507989fabaa040b1384154c02f6ed" ||
                AssetDatabase.AssetPathToGUID(BrightPath) != "c77d3165efc3124408daf752fe07f184")
                throw new InvalidOperationException("Original custom shader GUID changed");
            if (game.GetPropertyCount() != 9 || bright.GetPropertyCount() != 6)
                throw new InvalidOperationException("Original shader property inventory changed");
            foreach (var shader in new[] { game, bright })
            {
                foreach (var message in ShaderUtil.GetShaderMessages(shader))
                    if (message.severity == ShaderCompilerMessageSeverity.Error) report.errors.Add(shader.name + ": " + message.message);
            }
            Compile(report, folder, game, 0, new string[0]);
            for (int mask = 0; mask < 16; mask++)
                Compile(report, folder, bright, 0, Keywords(mask));
            Compile(report, folder, bright, 1, new string[0]);
            Compile(report, folder, bright, 2, new string[0]);
            report.negative_compile_control = RejectBrokenShader(folder);
            if (!report.negative_compile_control) report.errors.Add("Broken fragment shader was not rejected by real Unity compiler");
            RenderPixels(report, folder, game, bright);
            if (report.compilations.Count != 76) report.errors.Add("Expected 76 real stage/variant/platform compilations");
            if (report.pixels.Count != 20) report.errors.Add("Expected 20 actual render readbacks");
            report.pass = report.errors.Count == 0 && report.compilations.All(c => c.success) && report.pixels.All(p => p.pass);
        }
        catch (Exception error) { report.errors.Add(error.ToString()); report.pass = false; }
        File.WriteAllText(Path.Combine(folder, "custom-shader-audit.json"), JsonUtility.ToJson(report, true));
        Debug.Log("GODSPVZ_CUSTOM_SHADER_AUDIT=" + (report.pass ? "PASS" : "FAIL") + " compilations=" + report.compilations.Count + " pixels=" + report.pixels.Count);
        if (!report.pass) throw new InvalidOperationException("Custom shader gate failed: " + String.Join("; ", report.errors.ToArray()));
    }

    private static string[] Keywords(int mask)
    {
        return Enumerable.Range(0, 4).Where(i => (mask & (1 << i)) != 0).Select(i => "USE_SHAPE_LIGHT_TYPE_" + i).ToArray();
    }

    private static void Compile(Report report, string folder, Shader shader, int passIndex, string[] keywords)
    {
        var pass = ShaderUtil.GetShaderData(shader).GetSerializedSubshader(0).GetPass(passIndex);
        foreach (var platform in new[] { ShaderCompilerPlatform.OpenGLCore, ShaderCompilerPlatform.Metal })
        foreach (var stage in new[] { ShaderType.Vertex, ShaderType.Fragment })
        {
            var row = new CompileRow { shader=shader.name, pass=passIndex, platform=platform.ToString(), stage=stage.ToString(), keywords=String.Join(" ", keywords) };
            try
            {
                BuildTarget target = platform == ShaderCompilerPlatform.Metal ? BuildTarget.iOS : BuildTarget.StandaloneLinux64;
                var compiled = pass.CompileVariant(stage, keywords, platform, target, true);
                row.success = compiled.Success && compiled.ShaderData != null && compiled.ShaderData.Length > 0;
                if (compiled.Messages != null)
                    foreach (var message in compiled.Messages)
                    {
                        row.messages.Add(message.severity + ": " + message.message + " (" + message.file + ":" + message.line + ")");
                        if (message.severity == ShaderCompilerMessageSeverity.Error) row.success = false;
                    }
                if (compiled.ShaderData != null)
                {
                    row.bytes = compiled.ShaderData.Length;
                    using (var hash = SHA256.Create()) row.sha256 = BitConverter.ToString(hash.ComputeHash(compiled.ShaderData)).Replace("-", "").ToLowerInvariant();
                    string output = "shader-" + (shader.name == "Unlit/GameUnitShader" ? "game" : "brightness") + "-p" + passIndex + "-" + platform + "-" + stage + "-" + String.Join("_", keywords.Select(k => k.Substring(k.Length-1)).ToArray()) + ".bin";
                    File.WriteAllBytes(BinaryPath(output), compiled.ShaderData);
                }
            }
            catch (Exception error) { row.messages.Add(error.ToString()); row.success = false; }
            report.compilations.Add(row);
        }
    }

    private static bool RejectBrokenShader(string folder)
    {
        const string path = "Assets/Recovery/CustomShaderNegativeControl.shader";
        if (File.Exists(path)) throw new InvalidOperationException("Negative control path already exists");
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, "Shader \"Hidden/GodsPVZ/CompilerMustReject\" { SubShader { Pass { CGPROGRAM\n#pragma vertex vert\n#pragma fragment frag\nfloat4 vert(float4 p:POSITION):SV_POSITION{return p;}\nfloat4 frag():SV_Target{return GODSPVZ_UNDEFINED_ERROR;}\nENDCG\n} } }");
        try
        {
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (shader == null) throw new InvalidOperationException("Control must import as a shader before compiler rejection");
            var result = ShaderUtil.GetShaderData(shader).GetSerializedSubshader(0).GetPass(0).CompileVariant(ShaderType.Fragment, new string[0], ShaderCompilerPlatform.OpenGLCore, BuildTarget.StandaloneLinux64, true);
            var messages = result.Messages ?? new ShaderMessage[0];
            File.WriteAllText(Path.Combine(folder, "custom-shader-negative-control.txt"), "success=" + result.Success + "\n" + String.Join("\n", messages.Select(m => m.severity + ": " + m.message).ToArray()));
            return !result.Success && messages.Any(m => m.severity == ShaderCompilerMessageSeverity.Error && m.message.Contains("GODSPVZ_UNDEFINED_ERROR"));
        }
        finally { AssetDatabase.DeleteAsset(path); }
    }

    private static Texture2D Flat(Color color)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGBAFloat, false, true);
        texture.SetPixel(0,0,color); texture.Apply(false,false);
        texture.filterMode = FilterMode.Point; texture.wrapMode = TextureWrapMode.Clamp;
        return texture;
    }

    private static void RenderPixels(Report report, string folder, Shader game, Shader bright)
    {
        var mesh = new Mesh { name="Controlled custom shader quad" };
        mesh.vertices = new[] { new Vector3(-.8f,-.8f,0),new Vector3(-.8f,.8f,0),new Vector3(.8f,.8f,0),new Vector3(.8f,-.8f,0) };
        mesh.uv = new[] { new Vector2(0,1),new Vector2(0,0),new Vector2(1,0),new Vector2(1,1) };
        mesh.tangents = Enumerable.Repeat(new Vector4(1,0,0,1),4).ToArray();
        Color vertexColor = new Color(.8f,.7f,.6f,.75f);
        mesh.colors = Enumerable.Repeat(vertexColor,4).ToArray();
        mesh.triangles = new[] { 0,1,2,0,2,3 };
        var main = Flat(new Color(.3f,.4f,.6f,.5f));
        var materialGame = new Material(game); var materialBright = new Material(bright);
        var lights = Enumerable.Range(0,4).Select(i => Flat(new Color(.2f+i*.1f,.3f+i*.08f,.4f+i*.05f,1))).ToArray();
        Color clear = new Color(.1f,.2f,.3f,.4f);
        try
        {
            Shader.SetGlobalVector("_RendererColor", Vector4.one);
            Shader.SetGlobalVector("_GlobalMipBias", new Vector4(0,1,0,0));
            Color propertyColor=new Color(.7f,.8f,.9f,.6f);
            materialGame.SetTexture("_MainTex",main); materialGame.SetColor("_Color",propertyColor);
            materialGame.SetFloat("_Alpha",.8f); materialGame.SetFloat("_Exposure",1.5f);
            // Color properties are uploaded in linear space in this original
            // Linear project. This belongs to the Unity adapter, not HLSL math.
            Color uploadedColor=QualitySettings.activeColorSpace==ColorSpace.Linear ? propertyColor.linear : propertyColor;
            Color fragment = new Color(.3f*.8f*uploadedColor.r*1.5f,.4f*.7f*uploadedColor.g*1.5f,.6f*.6f*uploadedColor.b*1.5f,.5f*.75f*uploadedColor.a*.8f);
            ReadPixel(report,folder,"game-visible",mesh,materialGame,0,clear,Blend(fragment,clear,false));
            materialGame.SetFloat("_IsVisible",-1);
            ReadPixel(report,folder,"game-hidden",mesh,materialGame,0,clear,clear);
            materialBright.SetTexture("_MainTex",main); materialBright.SetFloat("_Brightness",2); materialBright.SetFloat("_Transparent",.8f);
            Shader.SetGlobalFloat("_HDREmulationScale",1.25f); Shader.SetGlobalFloat("_UseSceneLighting",1);
            Color raw = new Color(.3f*2*.8f,.4f*2*.7f,.6f*2*.6f,.5f*.8f*.75f);
            for (int mask=0;mask<16;mask++)
            {
                var modulate = Vector3.zero; var additive = Vector3.zero;
                for (int i=0;i<4;i++)
                {
                    string keyword="USE_SHAPE_LIGHT_TYPE_"+i;
                    if ((mask&(1<<i)) != 0) materialBright.EnableKeyword(keyword); else materialBright.DisableKeyword(keyword);
                    Shader.SetGlobalTexture("_ShapeLightTexture"+i,lights[i]);
                    Shader.SetGlobalVector("_ShapeLightBlendFactors"+i,new Vector4(.75f,.25f,0,0));
                    Shader.SetGlobalVector("_ShapeLightMaskFilter"+i,new Vector4(.1f,.2f,0,.4f));
                    Shader.SetGlobalVector("_ShapeLightInvertedFilter"+i,new Vector4(0,1,.5f,.25f));
                    if ((mask&(1<<i)) != 0)
                    {
                        var light = new Vector3(.2f+i*.1f,.3f+i*.08f,.4f+i*.05f)*.4f;
                        modulate += light*.75f; additive += light*.25f;
                    }
                }
                Color lit = raw;
                if (mask != 0)
                    lit = new Color(1.25f*(raw.r*modulate.x+additive.x),1.25f*(raw.g*modulate.y+additive.y),1.25f*(raw.b*modulate.z+additive.z),raw.a);
                ReadPixel(report,folder,"brightness-lit-"+mask,mesh,materialBright,0,clear,Blend(lit,clear,true));
            }
            // Identity view matrix encodes forward normal as (0.5,0.5,1).
            ReadPixel(report,folder,"brightness-normal",mesh,materialBright,1,clear,Blend(new Color(.5f,.5f,1,.5f*.8f),clear,true));
            ReadPixel(report,folder,"brightness-forward",mesh,materialBright,2,clear,Blend(raw,clear,true));
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(mesh); UnityEngine.Object.DestroyImmediate(main);
            UnityEngine.Object.DestroyImmediate(materialGame); UnityEngine.Object.DestroyImmediate(materialBright);
            foreach (var texture in lights) UnityEngine.Object.DestroyImmediate(texture);
        }
    }

    private static Color Blend(Color s, Color d, bool separateAlpha)
    {
        return new Color(s.r*s.a+d.r*(1-s.a),s.g*s.a+d.g*(1-s.a),s.b*s.a+d.b*(1-s.a),(separateAlpha ? s.a : s.a*s.a)+d.a*(1-s.a));
    }

    private static void ReadPixel(Report report, string folder, string name, Mesh mesh, Material material, int pass, Color clear, Color expected)
    {
        var target = new RenderTexture(32,32,0,RenderTextureFormat.ARGBFloat,RenderTextureReadWrite.Linear);
        var readback = new Texture2D(32,32,TextureFormat.RGBAFloat,false,true);
        var prior = RenderTexture.active;
        var commands = new CommandBuffer { name="GodsPVZ controlled custom shader probe" };
        try
        {
            target.Create();
            commands.SetRenderTarget(target); commands.ClearRenderTarget(false,true,clear);
            commands.SetViewProjectionMatrices(Matrix4x4.identity,GL.GetGPUProjectionMatrix(Matrix4x4.Ortho(-1,1,-1,1,-1,1),true));
            commands.DrawMesh(mesh,Matrix4x4.identity,material,0,pass);
            Graphics.ExecuteCommandBuffer(commands);
            RenderTexture.active=target;
            readback.ReadPixels(new Rect(0,0,32,32),0,0); readback.Apply(false,false);
            Color actual=readback.GetPixel(16,16);
            float error=Mathf.Max(Mathf.Abs(actual.r-expected.r),Mathf.Abs(actual.g-expected.g),Mathf.Abs(actual.b-expected.b),Mathf.Abs(actual.a-expected.a));
            report.pixels.Add(new PixelRow { name=name,expected=expected,actual=actual,max_error=error,pass=!Single.IsNaN(error)&&error<.003f });
            var preview = new Texture2D(32,32,TextureFormat.RGBA32,false,true);
            try
            {
                preview.SetPixels(readback.GetPixels()); preview.Apply(false,false);
                File.WriteAllBytes(BinaryPath("render-"+name+".png"),preview.EncodeToPNG());
            }
            finally { UnityEngine.Object.DestroyImmediate(preview); }
        }
        finally
        {
            RenderTexture.active=prior; commands.Release(); target.Release();
            UnityEngine.Object.DestroyImmediate(target); UnityEngine.Object.DestroyImmediate(readback);
        }
    }
}
#endif
