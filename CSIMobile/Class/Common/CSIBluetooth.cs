using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ZXing;
using ZXing.Mobile;
using System.Threading.Tasks;
using Android.Support.V4.Print;
using Android.Print;
using Android.Webkit;
using System.IO;
using Android.Graphics;
using Android.Bluetooth;
using Java.IO;
using Java.Util;
using System.Collections.ObjectModel;

namespace CSIMobile.Class.Common
{
    public interface IBluetooth
    {
        ObservableCollection<string> PairedDevices();

        void Imprimir(string pStrNomBluetooth, int intSleepTime, string pStrTextoImprimir);
    }

    public class CSIBluetooth : IBluetooth
    {
        private BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
        private BluetoothSocket socket = null;
        private BufferedWriter outReader = null;
        private BluetoothDevice device = null;

        public void Imprimir(string pStrNomBluetooth, int intSleepTime, string pStrTextoImprimir)
        {
            try
            {
                string bt_printer = (from d in adapter.BondedDevices
                                     where d.Name == pStrNomBluetooth
                                     select d).FirstOrDefault().ToString();

                device = adapter.GetRemoteDevice(bt_printer);
                UUID applicationUUID = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");

                socket = device.CreateRfcommSocketToServiceRecord(applicationUUID);
                socket.Connect();

                outReader = new BufferedWriter(new OutputStreamWriter(socket.InputStream));
                outReader.Write(pStrTextoImprimir);
                
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
            }
        }

        public ObservableCollection<string> PairedDevices()
        {
            ObservableCollection<string> devices = new ObservableCollection<string>();

            foreach (var bd in adapter.BondedDevices)
                devices.Add(bd.Name);

            return devices;
        }
    }
}