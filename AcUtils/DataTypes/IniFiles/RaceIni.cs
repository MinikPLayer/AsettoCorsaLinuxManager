using AcUtils.DataManagers;

namespace AcUtils.DataTypes.IniFiles;

public class RaceIni
{
    #region RACE

    [IniElement("RACE", "TRACK")]
    public string RaceTrackName { get; set; } = "magione";

    [IniElement("RACE", "CONFIG_TRACK")]
    public string RaceTrackConfig { get; set; } = "";
    
    [IniElement("RACE", "MODEL")]
    public string RaceCarModel { get; set; } = "lotus_elise_sc";
    
    [IniElement("RACE", "MODEL_CONFIG")]
    public string RaceCarModelConfig { get; set; } = "";
    
    [IniElement("RACE", "CARS")]
    public int RaceCarsNumber { get; set; } = 1;
    
    [IniElement("RACE", "AI_LEVEL")]
    public int RaceAiLevel { get; set; } = 98;

    [IniElement("RACE", "FIXED_SETUP")]
    public bool RaceFixedSetup { get; set; } = false;
    
    [IniElement("RACE", "PENALTIES")]
    public bool RacePenalties { get; set; } = false;

    #endregion
    
    #region GHOST_CAR

    [IniElement("GHOST_CAR", "RECORDING")]
    public bool GhostCarRecording { get; set; } = false;

    [IniElement("GHOST_CAR", "PLAYING")]
    public bool GhostCarPlaying { get; set; } = false;
    
    [IniElement("GHOST_CAR", "SECONDS_ADVANTAGE")]
    public int GhostCarSecondsAdvantage { get; set; } = 0;
    
    [IniElement("GHOST_CAR", "LOAD")]
    public bool GhostCarLoad { get; set; } = true;
    
    [IniElement("GHOST_CAR", "FILE")]
    public string GhostCarFile { get; set; } = "";
    
    #endregion
    
    #region REPLAY

    [IniElement("REPLAY", "FILENAME")]
    public string ReplayFilename { get; set; } = "";
    
    [IniElement("REPLAY", "ACTIVE")]
    public bool ReplayActive { get; set; } = false;

    #endregion
    
    #region LIGHTING

    [IniElement("LIGHTING", "SUN_ANGLE")]
    public int LightingSunAngle { get; set; } = -48;
    
    [IniElement("LIGHTING", "TIME_MULT")]
    public float LightingTimeMult { get; set; } = 1;
    
    [IniElement("LIGHTING", "CLOUD_SPEED")]
    public float LightingCloudSpeed { get; set; } = 0.2f;

    #endregion

    #region GROOVE

    [IniElement("GROOVE", "VIRTUAL_LAPS")]
    public int GrooveVirtualLaps { get; set; } = 10;

    [IniElement("GROOVE", "MAX_LAPS")]
    public int GrooveMaxLaps { get; set; } = 1;
    
    [IniElement("GROOVE", "STARTING_LAPS")]
    public int GrooveStartingLaps { get; set; } = 1;
    
    #endregion

    #region DYNAMIC_TRACK

    [IniElement("DYNAMIC_TRACK", "SESSION_START")]
    public float DynamicTrackSessionStart { get; set; } = 100;
    
    [IniElement("DYNAMIC_TRACK", "SESSION_TRANSFER")]
    public float DynamicTrackSessionTransfer { get; set; } = 100;
    
    [IniElement("DYNAMIC_TRACK", "RANDOMNESS")]
    public float DynamicTrackRandomness { get; set; } = 0;
    
    [IniElement("DYNAMIC_TRACK", "LAP_GAIN")]
    public float DynamicTrackLapGain { get; set; } = 1;
    
    [IniElement("DYNAMIC_TRACK", "PRESET")]
    public int DynamicTrackPreset { get; set; } = 5;

    #endregion

    #region REMOTE

    [IniElement("REMOTE", "ACTIVE")]
    public bool RemoteActive { get; set; } = false;
    
    [IniElement("REMOTE", "SERVER_IP")]
    public string RemoteServerIp { get; set; } = "";
    
    [IniElement("REMOTE", "SERVER_PORT")]
    public string RemoteServerPort { get; set; } = "";
    
    [IniElement("REMOTE", "NAME")]
    public string RemoteName { get; set; } = "";

    [IniElement("REMOTE", "TEAM")]
    public string RemoteTeam { get; set; } = "";
    
    [IniElement("REMOTE", "GUID")]
    public string RemoteGuid { get; set; } = "";
    
    [IniElement("REMOTE", "REQUESTED_CAR")]
    public string RemoteRequestedCar { get; set; } = "";
    
    [IniElement("REMOTE", "PASSWORD")]
    public string RemotePassword { get; set; } = "";
    
    #endregion
    
    #region LAP_INVALIDATOR

    [IniElement("LAP_INVALIDATOR", "ALLOWED_TYRES_OUT")]
    public int LapInvalidatorAllowedTyresOut { get; set; } = -1;

    #endregion
    
    #region TEMPERATURE

    [IniElement("TEMPERATURE", "AMBIENT")]
    public float TemperatureAmbient { get; set; } = 26;
    
    [IniElement("TEMPERATURE", "ROAD")]
    public float TemperatureRoad { get; set; } = 34;

    #endregion

    #region WEATHER

    [IniElement("WEATHER", "NAME")]
    public string WeatherName { get; set; } = "4_mid_clear";

    #endregion

    #region BENCHMARK

    [IniElement("BENCHMARK", "ACTIVE")]
    public bool BenchmarkActive { get; set; } = false;
    
    #endregion

    #region WIND

    [IniElement("WIND", "SPEED_KMH_MIN")]
    public float WindSpeedKmhMin { get; set; } = 0;

    [IniElement("WIND", "SPEED_KMH_MAX")]
    public float WindSpeedKmhMax { get; set; } = 0;

    [IniElement("WIND", "DIRECTION_DEG")]
    public float WindDirectionDeg { get; set; } = -1;
    
    #endregion

    #region HEADER

    [IniElement("HEADER", "VERSION")]
    public int HeaderVersion { get; set; } = 1;

    #endregion

    #region SESSION

    public class SessionData
    {
        [IniList("NAME")]
        public string Name { get; set; } = "Time Attack";
        
        [IniList("TYPE")]
        public int Type { get; set; } = 5;
        
        [IniList("DURATION_MINUTES")]
        public float DurationMinutes { get; set; } = 0;
        
        [IniList("SPAWN_SET")]
        public string SpawnSet { get; set; } = "START";
    }

    [IniList("SESSION")]
    public List<SessionData> Sessions { get; set; } = new() { new SessionData() };

    #endregion
    
    #region CAR

    public class CarData
    {
        [IniList("MODEL")]
        public string Model { get; set; } = "-";
        
        [IniList("MODEL_CONFIG")]
        public string ModelConfig { get; set; } = "";
        
        [IniList("SKIN")]
        public string Skin { get; set; } = "0_racing_green";
        
        [IniList("DRIVER_NAME")]
        public string DriverName { get; set; } = "";
        
        [IniList("NATIONALITY")]
        public string Nationality { get; set; } = "";
        
        [IniList("NATION_CODE")]
        public string NationCode { get; set; } = "";
        
        [IniList("AI_LEVEL")]
        public int AiLevel { get; set; } = 96;
    }

    [IniList("CAR")]
    public List<CarData> Cars { get; set; } = new() { new CarData() };

    #endregion

    public IniFile ToIni() => new (this);

    public static RaceIni GetDefault(string driverName = "", string driverNationality = "")
    {
        var rIni = new RaceIni();
        rIni.Cars[0].DriverName = driverName;
        rIni.Cars[0].Nationality = driverNationality;
        
        return rIni;
    }
}