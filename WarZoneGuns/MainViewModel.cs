using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WarZoneGuns.Core;

namespace WarZoneGuns
{
    public class MainViewModel : BaseNotify
    {
        public CustomCommand AddGun { get; set; }
        public CustomCommand EditGun { get; set; }
        public CustomCommand DeleteGun { get; set; }

        private Gun selectedgun;
        public Gun SelectedGun
        {
            get => selectedgun;
            set
            {
                selectedgun = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Gun> Guns { get; set; } = new ObservableCollection<Gun>();

        public MainViewModel()
        {
            Guns = new ObservableCollection<Gun>();
            var guns = GunsClient.GetAllGuns();
            foreach (var gun in guns)
            {
                Guns.Add(gun);
            }
            AddGun = new CustomCommand(() =>
            {
                var gun = GunsClient.Post(new Gun());
                Guns.Add(gun);
                OnPropertyChanged("Guns");
            });
            EditGun = new CustomCommand(() =>
            {
                if (SelectedGun == null)
                {
                    var gun = GunsClient.Post(new Gun());
                    Guns.Add(gun);
                    OnPropertyChanged("Guns");
                }
                else
                {
                    if (!GunsClient.Put(SelectedGun))
                        MessageBox.Show("Не удалось обновить объект на сервере");
                }
            });

            DeleteGun = new CustomCommand(() =>
            {
                if (SelectedGun == null)
                    return;
                if (!GunsClient.Delete(SelectedGun))
                    MessageBox.Show("Не удалось удалить объект на сервере");
                else
                    Guns.Remove(SelectedGun);
            });
        }
    }
}
