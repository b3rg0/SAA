﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAA.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MateriaView : ContentPage {
        ViewModels.MateriaViewModel mvm = new SAA.ViewModels.MateriaViewModel();

        public MateriaView() {
            InitializeComponent();            
            ListView lv = new ListView();
            lv.ItemsSource = mvm.materias;
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, "Nombre");
            lv.ItemTemplate.SetBinding(TextCell.DetailProperty, "ID");

            Button b1 = new Button() {
                Text = "Opciones de Materia",
                Command = mvm.ModificarMateriaCommand
            };

            this.Content = new StackLayout() {
                Children = {lv,b1}
            };

            lv.ItemTapped += async (object sender, ItemTappedEventArgs e) => {

                var materia = (Models.MateriaModel)lv.SelectedItem;
                await Navigation.PushModalAsync(new Views.SeccionView(materia.ID));                
            };
        }
        protected override void OnAppearing() {
            mvm.refresh();
        }
    }
}