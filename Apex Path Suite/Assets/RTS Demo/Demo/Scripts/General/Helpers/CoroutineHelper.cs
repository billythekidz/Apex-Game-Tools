namespace Apex.Demo.RTS.AI
{
    /// <summary>
    /// Simple class for facilitating running co-routines from units and structures which may be destroyed while running the coroutine, thus resulting in an exception.
    /// </summary>
    /// <seealso cref="Apex.Demo.RTS.AI.SingletonMonoBehaviour{Apex.Demo.RTS.AI.CoroutineHelper}" />
    public sealed class CoroutineHelper : SingletonMonoBehaviour<CoroutineHelper>
    {
    }
}