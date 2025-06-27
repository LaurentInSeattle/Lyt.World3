namespace Lyt.World3.AppModel;

using Lyt.Simulation.World3;

public sealed partial class World3Model : ModelBase
{
    #region Serialized -  No model changed event

    [JsonRequired]
    public string Language { get => this.Get<string>()!; set => this.Set(value); } 

    /// <summary> This should stay true, ==> But... Just FOR NOW !  </summary>
    [JsonRequired]
    public bool IsFirstRun { get; set; } = false;

    #endregion Serialized -  No model changed event


    #region Not serialized - No model changed event

    [JsonIgnore]
    public WorldModel WorldModel { get; private set; } 

    [JsonIgnore]
    public bool ModelLoadedNotified { get; set; } = false;

    #endregion Not serialized - No model changed event


    #region NOT serialized - WITH model changed event

    //[JsonIgnore]
    //// Asynchronous: Must raise Model Updated events 
    //public bool IsInternetConnected { get => this.Get<bool>(); set => this.Set(value); }

    #endregion NOT serialized - WITH model changed event    

}
