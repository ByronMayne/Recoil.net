using RecoilNet.State;
using System.Windows;
using System.Windows.Controls;

namespace RecoilNet
{
    /// <summary>
    /// Interaction logic for RecoilRoot.xaml
    /// </summary>
    public partial class RecoilRoot : UserControl
    {
        /// <summary>
        /// Gets or sets the recoil store to use
        /// </summary>
        public RecoilStore Store
        {
            get { return (RecoilStore)GetValue(StoreProperty); }
            set { SetValue(StoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.RegisterAttached(
                nameof(Store),
                typeof(RecoilStore),
                typeof(RecoilRoot),
                new FrameworkPropertyMetadata(new RecoilStore()));

        public RecoilRoot()
        {
            InitializeComponent();
        }
    }
}
