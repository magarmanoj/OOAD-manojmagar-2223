﻿using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageMotor.xaml
    /// </summary>
    public partial class PageMotor : Page
    {
        private Voertuig selectedVoertuig;
        private int userId;
        public PageMotor(Voertuig voertuig, int userID)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            userId = userID;
            Fotolist();
            DisableButton();
        }

        private void Fotolist()
        {
            List<Foto> fotoList = Foto.GetFotoListByVoertuigId(selectedVoertuig.Id);

            // Update the Source property of the existing Image elements
            for (int i = 0; i < fotoList.Count; i++)
            {
                if (stpFoto.Children[i] is Image photoImage)
                {
                    using (MemoryStream stream = new MemoryStream(fotoList[i].Data))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        photoImage.Source = bitmap;
                    }
                }
            }
            Gebruiker eigenaars = Gebruiker.GetGebruikerById(selectedVoertuig.EigenaarId);
            name.Text = selectedVoertuig.Naam;
            beschrijving.Text = $"Beschrijving: {selectedVoertuig.Beschrijving}";
            merk.Text = $"Merk: {(!string.IsNullOrEmpty(selectedVoertuig.Merk) ? selectedVoertuig.Merk : "n.v.t")}";
            bouwjaar.Text = $"Bouwjaar: {(selectedVoertuig.Bouwjaar.HasValue ? selectedVoertuig.Bouwjaar.Value.ToString() : "N/A")}";
            model.Text = $"Model: {(!string.IsNullOrEmpty(selectedVoertuig.Model) ? selectedVoertuig.Model : "n.v.t")}";
            eigenaar.Text = $"Eignaar: {(eigenaars != null ? $"{eigenaars.Voornaam} {eigenaars.Achternaam}" : "Onbekend")}";
            transmissie.Text = $"Transmissie: {(selectedVoertuig.Transmissie.HasValue ? selectedVoertuig.Transmissie.ToString() : "N/A")}";
            brandstof.Text = $"Brandstof: {(selectedVoertuig.Brandstof.HasValue ? selectedVoertuig.Brandstof.ToString() : "N/A")}";
        }

        private void BtnVerzenden_Click(object sender, RoutedEventArgs e)
        {
            if (vanDateP.SelectedDate.HasValue && totDateP.SelectedDate.HasValue)
            {
                if (vanDateP.SelectedDate.Value.Date < totDateP.SelectedDate.Value.Date && vanDateP.SelectedDate.Value.Date >= DateTime.Now)
                {
                    Ontlening nieuweOntlening = new Ontlening
                    {
                        Id = selectedVoertuig.Id,
                        Vanaf = vanDateP.SelectedDate.Value.Date,
                        Tot = totDateP.SelectedDate.Value.Date,
                        Bericht = tbBericht.Text,
                        Status = Enums.OntleningStatus.InAanvraag,
                        Aanvrager = Gebruiker.GetGebruikerById(userId)
                    };

                    Ontlening.AddOntlening(nieuweOntlening);

                    vanDateP.SelectedDate = null;
                    totDateP.SelectedDate = null;
                    tbBericht.Text = string.Empty;
                }
                else
                {
                    if (vanDateP.SelectedDate.Value.Date < DateTime.Now)
                    {
                        MessageBox.Show("Gelieve een toekomstige datum te kiezen.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("De einddatum moet na de begindatum zijn.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vul de velden 'Van' en 'Tot' in.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisableButton()
        {
            if (IsCurrentUserCarOwner())
            {
                // User is the car owner, disable controls
                verzenden.IsEnabled = false;
                tbBericht.IsEnabled = false;
                vanDateP.IsEnabled = false;
                totDateP.IsEnabled = false;
                return;
            }
        }

        private bool IsCurrentUserCarOwner()
        {
            Gebruiker gebruiker = Gebruiker.GetGebruikerById(userId);
            return gebruiker != null && gebruiker.Id == selectedVoertuig.EigenaarId;
        }
    }
}
