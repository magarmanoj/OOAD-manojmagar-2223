using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            room1.ImagePath = "ss-bedroom.png";

            // define items
            Key key1 = new Key(
                "small silver key",
                "A small silver key, makes me think of one I had at high school.");

            Key key2 = new Key(
                "large key",
                "A large key. Could this be my way out? ");
            LockableItem locker = new LockableItem("locker", "A locker. I wonder what's inside. ", false);

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

            Room livingRoom = new Room("Living room", "Such a lovely living room.");
            livingRoom.ImagePath = "ss-living.png";
            Item sofa = new Item("Sofa", "nice cozy sofa", false);
            livingRoom.Items.Add(sofa);
            livingRoom.Items.Add(new Item("TV", "WOAAHHH WHAT A BIG TV!!"));
            Key key3 = new Key("Computer room Key", "A key under the sofa");
            sofa.HiddenItem = key3;

            Room computerRoom = new Room("Computer room", "Are you a hardcore gamer?");
            computerRoom.ImagePath = "ss-computer.png";
            LockableItem box = new LockableItem("Box", "What a strange box!", false);
            Item mouse = new Item("OLD Mouse", "Ohh OG mouse");
            computerRoom.Items.Add(new Item("PC", "IS THAT 4090?"));
            computerRoom.Items.Add(new Item("Gaming chair", "Nice comfy chair"));
            computerRoom.Items.Add(box);
            box.Key = key3;
            box.IsLocked = true;
            box.HiddenItem = mouse;

            Door door1 = new Door(
                "living room door", "This is big living room");
            door1.IsLocked = true;
            door1.Key = key2;
            door1.ToRoom = livingRoom;

            Door door2 = new Door(
                "computer room door", " Ohh! you have 2 computers, nice");
            door2.ToRoom = computerRoom;

            Door door3 = new Door(
                "mysterious door", "Can't see nothing, it's so dark");
            door3.IsLocked = true;
            door3.ToRoom = null; // null ruimte

            Door door4 = new Door(
                "bedroom door", "I seem to be in a medium sized bedroom. There is a locker to the left, a nice rug on the floor, and a bed to the right.");
            door4.ToRoom = room1;

            // deuren toevoegen aan kamers
            room1.Doors.Add(door1);
            livingRoom.Doors.Add(door2);
            computerRoom.Doors.Add(door1);
            livingRoom.Doors.Add(door3);
            livingRoom.Doors.Add(door4);       

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

            lstRoomDoors.Items.Clear();
            foreach (Door door in currentRoom.Doors)
            {
                lstRoomDoors.Items.Add(door);
            }

            BitmapImage roomImage = new BitmapImage(new Uri($"img/{currentRoom.ImagePath}", UriKind.RelativeOrAbsolute));
            imgFoto.Source = roomImage;
        }

        private void LstItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnCheck.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnPickUp.IsEnabled = lstRoomItems.SelectedValue != null; // room item selected
            btnUseOn.IsEnabled = lstRoomItems.SelectedValue != null && lstMyItems.SelectedValue != null; // room item and picked up item selected
            btnDrop.IsEnabled = lstMyItems.SelectedValue != null; // room item selected

            btnEnter.IsEnabled = lstRoomDoors.SelectedValue != null;
            btnOpenWith.IsEnabled = lstRoomDoors.SelectedValue != null && lstMyItems.SelectedValue != null;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            Item roomItem = (Item)lstRoomItems.SelectedItem;

            if (roomItem is LockableItem lockableItem)
            {
                if (lockableItem.IsLocked)
                {
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DOES_NOT_WORK);
                    return;
                }

            }
            Item foundItem = roomItem.HiddenItem;
            if (foundItem != null)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.IS_LOCKED);
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

            if(myItem is Key keyItem)
            {
                if(roomItem is LockableItem lockableItem)
                {
                    // 2. item doesn't fit
                    if (lockableItem.Key != keyItem)
                    {
                        lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.IS_WRONG_KEY);
                        return;
                    }

                    // 3. item fits; other item unlocked
                    lockableItem.IsLocked = false;
                    lockableItem.Key = null;
                    lstMyItems.Items.Remove(myItem);
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.IS_LOCKED);
                    return;

                }
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DOES_NOT_WORK);

            }

        }

        private void BtnPickUp_Click(object sender, RoutedEventArgs e)
        {
            // 1. find selected item
            Item selItem = (Item)lstRoomItems.SelectedItem;

            if (!selItem.IsPortable)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DOES_NOT_WORK);
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
            lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.IS_LOCKED);
            lstMyItems.Items.Remove(selItem);
            lstRoomItems.Items.Add(selItem);
            currentRoom.Items.Add(selItem);
        }

        private void BtnOpenWith_Click(object sender, RoutedEventArgs e)
        {
            if (lstRoomDoors.SelectedItem is Door door && lstMyItems.SelectedItem is Item myItem)
            {
                if (door.IsLocked && door.Key == myItem)
                {
                    door.IsLocked = false;
                    door.Key = null;
                    lstMyItems.Items.Remove(myItem);
                    lblMessage.Content = $"You unlocked the {door.Name} with the {myItem.Name}.";
                }
                else
                {
                    lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DOES_NOT_WORK);
                }
            }
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            Door selectedDoor = (Door)lstRoomDoors.SelectedItem;
            if (selectedDoor.IsLocked)
            {
                lblMessage.Content = RandomMessageGenerator.GetRandomMessage(MessageType.DOES_NOT_WORK);
                return;
            }
            currentRoom = selectedDoor.ToRoom;
            UpdateUI();
        }
    }
}
