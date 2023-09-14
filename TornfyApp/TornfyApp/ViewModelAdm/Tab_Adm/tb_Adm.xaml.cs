using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TornfyApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TornfyApp.ViewModelAdm
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class tb_Adm : TabbedPage
	{
		public tb_Adm()
		{
			InitializeComponent();

			this.Children.Add(new Circuitos_Adm() { Title = "Simples", IconImageSource = "user_simples.png" });
			this.Children.Add(new Categorias_Adm() { Title = "Duplas", IconImageSource = "user_dupla.png" });
			this.Children.Add(new Clubes_Adm() { Title = "Duplas", IconImageSource = "user_dupla.png" });
			this.Children.Add(new Dados_Adm() { Title = "Duplas", IconImageSource = "user_dupla.png" });

		}
	}
}