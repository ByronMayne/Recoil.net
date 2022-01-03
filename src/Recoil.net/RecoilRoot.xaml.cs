using RecoilNet.State;
using System.Windows;
using System.Windows.Controls;

namespace RecoilNet
{
    /// <summary>
    /// Interaction logic for RecoilRoot.xaml
    /// </summary>
    public partial class RecoilRoot : ContentControl
    {
        /// <summary>
        /// Gets or sets the recoil store to use
        /// </summary>
        public RecoilStore Store
        {
            get { return (RecoilStore)GetValue(StoreProperty); }
            set { SetValue(StoreProperty, value); }
        }

        /// <summary>
        /// Defaults to true. This prop only matters when this <RecoilRoot> is nested within another <RecoilRoot>.
        /// If override is true, this root will create a new Recoil scope. If override is false, this <RecoilRoot> 
        /// will perform no function other than rendering its children, thus children of this root will access the 
        /// Recoil values of the nearest ancestor RecoilRoot.
        /// </summary>
        public bool Override
        {
            get => (bool)GetValue(OverrideProperty);
            set => SetValue(OverrideProperty, value);
        }


        public static readonly DependencyProperty OverrideProperty =
			DependencyProperty.Register(nameof(Override), typeof(bool), 
                typeof(RecoilRoot), new PropertyMetadata(true));

		public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register(
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
