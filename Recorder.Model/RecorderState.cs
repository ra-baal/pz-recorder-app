namespace Recorder.Model
{
    public enum RecorderState
    {
        NoSensor,
        Ready, // Ten stan może trzeba wyrzucić? Bo obecnie kinecty od razu są w trybie podglądu.
        Preview,
        Recording
    }
}
