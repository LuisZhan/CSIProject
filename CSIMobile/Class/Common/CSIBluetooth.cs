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
using Android.Bluetooth;
using System.IO;
using Java.Util;
using Java.Lang;

namespace CSIMobile.Class.Common
{
    class CSIBlueTooth : CSIBaseObject
    {
        // android built in classes for bluetooth operations
        BluetoothAdapter BTAdapter;
        BluetoothSocket BTSocket;
        BluetoothDevice BTDevice;

        // needed for communication to bluetooth device / network
        StreamWriter BTOutputStream;
        StreamReader BTInputStream;
        Thread workerThread;

        public bool BlueToothDeviceFound = false;
        public bool BlueToothOpen = false;
        public bool DataSent = false;
        public bool Working = false;

        // this will find a bluetooth printer device
        public void FindBluetooth(string DeviceName)
        {

            try
            {
                BTAdapter = BluetoothAdapter.DefaultAdapter;

                if (BTAdapter == null)
                {
                    BlueToothDeviceFound = false;
                }

                if (!BTAdapter.IsEnabled)
                {
                    Intent enableBluetooth = new Intent(BluetoothAdapter.ActionRequestEnable);
                    Application.Context.StartActivity(enableBluetooth);
                    //BTAdapter.Enable();
                }

                ICollection<BluetoothDevice> pairedDevices = BTAdapter.BondedDevices;

                if (pairedDevices.Count > 0)
                {
                    foreach (BluetoothDevice device in pairedDevices)
                    {
                        // RPP300 is the name of the bluetooth printer device
                        // we got this name from the list of paired devices
                        if (device.Name == DeviceName)//"RPP300"
                        {
                            BTDevice = device;
                            break;
                        }
                    }
                }

                BlueToothDeviceFound = true;
                BlueToothOpen = false;
            }
            catch (System.Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }

        // tries to open a connection to the bluetooth printer device
        public void OpenBT()
        {
            try
            {
                if (BlueToothOpen)
                {
                    return;
                }
               // Standard SerialPortService ID
                UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                BTSocket = BTDevice.CreateRfcommSocketToServiceRecord(uuid);
                BTSocket.Connect();
                BTOutputStream = new StreamWriter(BTSocket.OutputStream);
                BTInputStream = new StreamReader(BTSocket.InputStream);
                
                //                BluetoothServerSocket serverSock = mBluetoothAdapter.ListenUsingRfcommWithServiceRecord("Bluetooth", Java.Util.UUID.FromString(uuid));
                //                mmSocket = serverSock.Accept();
                //                mmSocket.InputStream.ReadTimeout = 1000;
                //                serverSock.Close();//服务器获得连接后腰及时关闭ServerSocket
                //                启动新的线程，开始数据传输

                BlueToothOpen = true;
            }
            catch (System.Exception Ex)
            {
                BlueToothOpen = false;
                BTSocket.Dispose();
                WriteErrorLog(Ex);
            }
        }

        // close the connection to bluetooth printer.
        public void CloseBT()
        {
            try
            {
                Working = false;
                BTOutputStream.Close();
                BTInputStream.Close();
                BTSocket.Close();
                BlueToothOpen = false;
            }
            catch (System.Exception Ex)
            {
                WriteErrorLog(Ex);
                BlueToothOpen = false;
            }
        }

        // this will send text data to be printed by the bluetooth printer
        public void SendData(string msg)
        {
            try
            {
                DataSent = false;
                // the text typed by the user
                msg += "\n";

                //mmOutputStream.Write(Encoding.Default.GetBytes(msg),0,msg.Length);
                BTOutputStream.Write(msg);

                // tell the user data were sent
                DataSent = true;
            }
            catch (System.Exception Ex)
            {
                WriteErrorLog(Ex);
            }
        }
    }
}