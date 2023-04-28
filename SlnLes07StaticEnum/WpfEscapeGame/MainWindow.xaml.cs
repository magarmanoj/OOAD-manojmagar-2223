﻿using System.Windows;
using System.Windows.Controls;

namespace WpfEscapeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Room currentRoom;
        public MainWindow()
        {
            InitializeComponent();

            // define room
            Room room1 = new Room(
                "bedroom",
                "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, and a bed to the right.");

            // define items
            Item key1 = new Item(
                "small silver key",
                "A small silver key, makes me think of one I had at high school.");

            Item key2 = new Item(
                "large key",
                "A large key. Could this be my way out? ");
            Item locker = new Item("locker", "A locker. I wonder what's inside. ", false);

            locker.HiddenItem = key2;
            locker.IsLocked = true;
            locker.Key = key1;

            Item bed = new Item("bed", "Just a stoel. I am not tired right now. ", false);

            bed.HiddenItem = key1;

            room1.Items.Add(new Item(
                "floor mat",
                "A bit ragged floor mat, but still one of the most popular designs."));

            Item stoel = new Item("stoel", "Just a stoel. I am not tired right now. ", false);
            Item poster = new Item("poster", "Just a poster. ", true);

            room1.Items.Add(bed);
            room1.Items.Add(locker);
            room1.Items.Add(stoel);
            room1.Items.Add(poster);

            // start game
            currentRoom = room1;
            lblMessage.Content = "I am awake, but cannot remember who I am!? Must have been a hell of a party last night... ";
            txtRoomDesc.Text = currentRoom.Description;
            UpdateUI();
        }

        private void UpdateUI()
        {
            lstRoomItems.Items.Clear();
            foreach (Item itm in currentRoom.Items)
            {
                lstRoomItems.Items.Add(itm);
            }
        }
        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
            btnDrop.IsEnabled = lstMyItems.SelectedValue != null; // room item selected
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            // 1. find item to check
            Item roomItem = (Item)lstRoomItems.SelectedItem;

            // 2. is it locked?
            if (roomItem.IsLocked)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }

            // 3. does it contain a hidden item?
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
                lstMyItems.Items.Add(foundItem);
                roomItem.HiddenItem = null;
                return;
            }

            // 4. just another item; show description
            lblMessage.Content = roomItem.Description;
        }

        private void BtnUseOn_Click(object sender, RoutedEventArgs e)
        {
            // 1. find both items
            Item myItem = (Item)lstMyItems.SelectedItem;
            Item roomItem = (Item)lstRoomItems.SelectedItem;

            // 2. item doesn't fit
            if (roomItem.Key != myItem)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }

            // 3. item fits; other item unlocked
            roomItem.IsLocked = false;
            roomItem.Key = null;
            lstMyItems.Items.Remove(myItem);
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
        }

        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;

            if (!selItem.IsPortable)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Error);
                return;
            }

            // 2. add item to your items list
            lblMessage.Content = $"I just picked up the {selItem.Name}. ";
            lstMyItems.Items.Add(selItem);
            lstRoomItems.Items.Remove(selItem);
            currentRoom.Items.Remove(selItem);
        }

        private void BtnDrop_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstMyItems.SelectedItem;

            // 2. add item to your items list
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.Info);
            lstMyItems.Items.Remove(selItem);
            lstRoomItems.Items.Add(selItem);
            currentRoom.Items.Add(selItem);
        }
    }
}