﻿using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageOntlening.xaml
    /// </summary>
    public partial class PageOntlening : Page
    {
        private int userId;

        public PageOntlening(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadOntleningen();
            LoadAanvraag();
        }

        private void LoadOntleningenList(List<Ontlening> ontleningen, ListBox listBox)
        {
            listBox.Items.Clear();
            for (int i = 0; i < ontleningen.Count; i++)
            {
                for (int j = i + 1; j < ontleningen.Count; j++)
                {
                    if (ontleningen[j].Vanaf > ontleningen[i].Vanaf)
                    {
                        SwapOntleningen(ontleningen, i, j);
                    }
                }
            }
        }

        private void LoadOntleningen()
        {
            List<Ontlening> mijnOntleningen = Ontlening.GetOntleningen(userId);
            LoadOntleningenList(mijnOntleningen, lbOntleend);

            foreach (Ontlening ontlening in mijnOntleningen)
            {
                ListBoxItem item = new ListBoxItem();
                string displayText = $"{ontlening.VoertuigNaam} - {ontlening.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontlening.Tot.ToString("yyyy-MM-dd 00:00")}";
                TextBlock txtb = new TextBlock();
                txtb.Text = displayText.ToString();
                switch (ontlening.Status)
                {
                    case Enums.OntleningStatus.InAanvraag:
                        txtb.Foreground = Brushes.Orange;
                        break;
                    case Enums.OntleningStatus.Goedgekeurd:
                        txtb.Foreground = Brushes.Green;
                        break;
                    case Enums.OntleningStatus.Verworpen:
                        txtb.Foreground = Brushes.Red;
                        break;
                }
                item.Content = txtb;
                item.Tag = ontlening;
                lbOntleend.Items.Add(item);
            }
        }

        private void LoadAanvraag()
        {
            List<Ontlening> aanvraagOntleningen = Ontlening.GetAanvraagOntleningen(userId);
            LoadOntleningenList(aanvraagOntleningen, lbAanvraag);

            foreach (Ontlening ontlening in aanvraagOntleningen)
            {
                ListBoxItem item = new ListBoxItem();
                Gebruiker aanvrager = Gebruiker.GetGebruikerById(userId);
                string gebruikerNaam = $"{aanvrager.Voornaam.Substring(0, 1)}.{aanvrager.Achternaam}";
                string displayText = $"{ontlening.VoertuigNaam} - {ontlening.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontlening.Tot.ToString("yyyy-MM-dd 00:00")} door {gebruikerNaam.ToString()}";
                TextBlock txtb = new TextBlock();
                txtb.Text = displayText.ToString();
                txtb.Foreground = Brushes.Orange;
                item.Content = txtb;
                item.Tag = ontlening;
                lbAanvraag.Items.Add(item);
            }
        }

        private void SwapOntleningen(List<Ontlening> ontleningen, int index1, int index2)
        {
            Ontlening temp = ontleningen[index1];
            ontleningen[index1] = ontleningen[index2];
            ontleningen[index2] = temp;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (lbOntleend.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontl)
            {
                if (ontl.Tot > DateTime.Now)
                {
                    Ontlening.RemoveOntlening(ontl.Id);
                    LoadOntleningen();
                    LoadAanvraag();
                }
            }
        }

        private void LbAanvraag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAanvraag.SelectedItem is ListBoxItem item && item.Tag is Ontlening ontlening)
            {
                Gebruiker aanvraag = Gebruiker.GetGebruikerById(userId);
                string gebruikerNaam = $"{aanvraag.Voornaam} {aanvraag.Achternaam}";
                voertuig.Content = $"Voertuig: {ontlening.VoertuigNaam}";
                periode.Content = $"Periode: {ontlening.Vanaf.ToString("yyyy-MM-dd 00:00")} - {ontlening.Tot.ToString("yyyy-MM-dd 00:00")}";
                aanvrager.Content = $"Aanvrager: {gebruikerNaam}";
                bericht.Text = $"Bericht: {ontlening.Bericht}";
            }
            else
            {
                voertuig.Content = "Voertuig:";
                periode.Content = "Periode:";
                aanvrager.Content = "Aanvrager:";
                bericht.Text = "Bericht:";
            }
        }
    }
}
