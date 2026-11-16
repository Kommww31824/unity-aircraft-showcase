using UnityEngine;
using UnityEngine.SceneManagement;

// Presentation only. Removing this folder restores the original presentation.
public sealed class FlightPolish : MonoBehaviour
{
    fly_jiaoben flight;
    ParticleSystem exhaust;
    Material glow;
    Vector3 lastPosition;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Register()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.path != "Assets/Scenes/New Scene 1.unity") return;
        foreach (var root in scene.GetRootGameObjects())
        foreach (var controller in root.GetComponentsInChildren<fly_jiaoben>(true))
        {
            if (!controller.isActiveAndEnabled) continue;
            if (controller.GetComponentInChildren<FlightPolish>(true)) continue;
            var host = new GameObject("Flight presentation");
            host.transform.SetParent(controller.transform, false);
            host.AddComponent<FlightPolish>().Setup(controller);
        }
    }

    void Setup(fly_jiaoben controller)
    {
        flight = controller;
        lastPosition = flight.transform.position;
        var shader = Resources.Load<Shader>("FlightGlow");
        if (!shader || !shader.isSupported) return;
        glow = new Material(shader);
        // The flight script uses local +X as its forward axis.
        var meshes = controller.GetComponentsInChildren<MeshFilter>();
        float rear = 0f, front = 0f;
        foreach (var m in meshes)
        {
            if (!m.sharedMesh) continue;
            var b = m.sharedMesh.bounds;
            for (int mask = 0; mask < 8; ++mask)
            {
                var corner = b.center + Vector3.Scale(b.extents, new Vector3(
                    (mask & 1) == 0 ? -1 : 1, (mask & 2) == 0 ? -1 : 1, (mask & 4) == 0 ? -1 : 1));
                float x = transform.InverseTransformPoint(m.transform.TransformPoint(corner)).x;
                rear = Mathf.Min(rear, x);
                front = Mathf.Max(front, x);
            }
        }
        float scale = Mathf.Clamp(Vector3.Distance(transform.TransformPoint(rear, 0, 0),
            transform.TransformPoint(front, 0, 0)), 0.8f, 24f);
        transform.localPosition = new Vector3(rear * 0.88f, 0, 0);
        transform.localRotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        exhaust = gameObject.AddComponent<ParticleSystem>();
        exhaust.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = exhaust.main;
        main.loop = true;
        main.playOnAwake = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.scalingMode = ParticleSystemScalingMode.Shape;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.23f, 0.54f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(scale * 0.13f, scale * 0.28f);
        main.startSize = new ParticleSystem.MinMaxCurve(scale * 0.015f, scale * 0.042f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(0.27f,0.72f,1f,0.72f), new Color(0.75f,0.94f,1f,0.85f));
        main.maxParticles = 96;
        var shape = exhaust.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 13;
        shape.radius = 0.02f;
        var color = exhaust.colorOverLifetime;
        color.enabled = true;
        var fade = new Gradient();
        fade.SetKeys(new[]{new GradientColorKey(Color.white,0),new GradientColorKey(new Color(0.2f,0.62f,1f),1)},
            new[]{new GradientAlphaKey(0.25f,0),new GradientAlphaKey(1,0.12f),new GradientAlphaKey(0,1)});
        color.color = fade;
        var renderer = GetComponent<ParticleSystemRenderer>();
        renderer.sharedMaterial = glow;
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        var emission = exhaust.emission;
        emission.rateOverTime = 0;
        exhaust.Play();
    }

    void LateUpdate()
    {
        if (!exhaust || !flight) return;
        var p = flight.transform.position;
        float movement = Time.deltaTime > 0 ? Vector3.Distance(p,lastPosition) / Time.deltaTime : 0;
        lastPosition = p;
        var emission = exhaust.emission;
        emission.rateOverTime = movement < 0.05f ? 0 : Mathf.Lerp(12, 62, Mathf.Clamp01(flight.speed / 24f));
    }

    void OnDestroy()
    {
        if (glow) Destroy(glow);
    }
}

