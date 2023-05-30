﻿using MyClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for EditGetrokken.xaml
    /// </summary>
    public partial class EditGetrokken : Page
    {
        private Voertuig selectedVoertuig;
        private int userId;
        public EditGetrokken(Voertuig voertuig, int userID)
        {
            InitializeComponent();
            selectedVoertuig = voertuig;
            userId = userID;
        }
    }
}
