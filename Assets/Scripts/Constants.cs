using UnityEngine;

public static class Constants
{
    public const string MapSettingsKey = "MapSettings";

    public const int MapSize = 200;

    public const int MaxSeedValue = 99999;
    public const int DefaultSeed = 1;
    public const float ScaleDefaultValue = 0.02f;
    public const float WaterLevelDefaultValue = 0.3f;
    public const int TreesPercentageDefaultValue = 10;

    public const int TerrainPositionY = 0;
    public const float TerrainWaterPositionY = -0.5f;
    public const float BorderSideBottomPositionY = -1f;

    public const float TreesShoreOffset = 0.04f;

    public const float ShallowLowerHeight = 0.45f;
    public const float ShallowHigherHeight = 0.5f;
    public const float ShallowUV = 0.49f;

    public const float CameraRotateFieldOfView = 15f;
    public const float CameraFlyFieldOfView = 30f;
    public const float CameraRotationSpeed = 10f;
    public const float CameraMinPositionY = 0f;
    public const float CameraMaxDistanceFromCenter = 300f;
    public const float CameraFlySpeed = 30f;
    public const float CameraFlySpeedWithShift = 100f;

    public static Vector3 CameraInitialPositionRotateMode => new Vector3(-300, 300, -300);
    public static Vector3 CameraInitialPositionFlyMode => new Vector3(-100, 100, -100);
}