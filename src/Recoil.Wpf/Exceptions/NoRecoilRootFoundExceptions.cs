using RecoilNet;
using System.Windows;

namespace Recoil.Wpf.Exceptions
{
    internal class NoRecoilRootFoundExceptions : RecoilException
    {
        public override string Message { get; }

        public NoRecoilRootFoundExceptions(DependencyObject startingPoint)
        {
            bool isLoaded = startingPoint is not FrameworkElement element || element.IsLoaded;

            Message = @$"Unable to find any {nameof(RecoilRoot)} in the parent hierarchy for {startingPoint.GetType().Name}.";

            if (!isLoaded)
            {
                Message += "\n You were requesting the root from an element that has not been loaded yet, this will always fail.";
            }
        }
    }
}
