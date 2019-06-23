using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAA.Views{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MateriaView : ContentPage{
		public MateriaView (){
			InitializeComponent ();
            this.BindingContext = new ViewModels.MateriaViewModel();
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
		}
	}
}